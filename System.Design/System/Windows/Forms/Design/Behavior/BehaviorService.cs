using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Microsoft.Win32;

namespace System.Windows.Forms.Design.Behavior
{
	public sealed class BehaviorService : IDisposable
	{
		internal BehaviorService(IServiceProvider serviceProvider, Control windowFrame)
		{
			this.serviceProvider = serviceProvider;
			this.adornerWindow = new BehaviorService.AdornerWindow(this, windowFrame);
			IOverlayService overlayService = (IOverlayService)serviceProvider.GetService(typeof(IOverlayService));
			if (overlayService != null)
			{
				this.adornerWindowIndex = overlayService.PushOverlay(this.adornerWindow);
			}
			this.dragEnterReplies = new Hashtable();
			this.adorners = new BehaviorServiceAdornerCollection(this);
			this.behaviorStack = new ArrayList();
			this.hitTestedGlyph = null;
			this.validDragArgs = null;
			this.actionPointer = null;
			this.trackMouseEvent = null;
			this.trackingMouseEvent = false;
			IMenuCommandService menuCommandService = serviceProvider.GetService(typeof(IMenuCommandService)) as IMenuCommandService;
			IDesignerHost designerHost = serviceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (menuCommandService != null && designerHost != null)
			{
				this.menuCommandHandler = new BehaviorService.MenuCommandHandler(this, menuCommandService);
				designerHost.RemoveService(typeof(IMenuCommandService));
				designerHost.AddService(typeof(IMenuCommandService), this.menuCommandHandler);
			}
			this.useSnapLines = false;
			this.queriedSnapLines = false;
			BehaviorService.WM_GETALLSNAPLINES = SafeNativeMethods.RegisterWindowMessage("WM_GETALLSNAPLINES");
			BehaviorService.WM_GETRECENTSNAPLINES = SafeNativeMethods.RegisterWindowMessage("WM_GETRECENTSNAPLINES");
			SystemEvents.DisplaySettingsChanged += this.OnSystemSettingChanged;
			SystemEvents.InstalledFontsChanged += this.OnSystemSettingChanged;
			SystemEvents.UserPreferenceChanged += this.OnUserPreferenceChanged;
		}

		public BehaviorServiceAdornerCollection Adorners
		{
			get
			{
				return this.adorners;
			}
		}

		internal int AdornerWindowIndex
		{
			get
			{
				return this.adornerWindowIndex;
			}
		}

		internal Control AdornerWindowControl
		{
			get
			{
				return this.adornerWindow;
			}
		}

		public Graphics AdornerWindowGraphics
		{
			get
			{
				Graphics graphics = this.adornerWindow.CreateGraphics();
				graphics.Clip = new Region(this.adornerWindow.DesignerFrameDisplayRectangle);
				return graphics;
			}
		}

		public Behavior CurrentBehavior
		{
			get
			{
				if (this.behaviorStack != null && this.behaviorStack.Count > 0)
				{
					return this.behaviorStack[0] as Behavior;
				}
				return null;
			}
		}

		internal bool CancelDrag
		{
			get
			{
				return this.cancelDrag;
			}
			set
			{
				this.cancelDrag = value;
			}
		}

		internal DesignerActionUI DesignerActionUI
		{
			get
			{
				return this.actionPointer;
			}
			set
			{
				this.actionPointer = value;
			}
		}

		internal bool Dragging
		{
			get
			{
				return this.dragging;
			}
		}

		internal bool HasCapture
		{
			get
			{
				return this.captureBehavior != null;
			}
		}

		internal bool UseSnapLines
		{
			get
			{
				if (!this.queriedSnapLines)
				{
					this.queriedSnapLines = true;
					this.useSnapLines = DesignerUtils.UseSnapLines(this.serviceProvider);
				}
				return this.useSnapLines;
			}
		}

		public Point AdornerWindowPointToScreen(Point p)
		{
			NativeMethods.POINT point = new NativeMethods.POINT(p.X, p.Y);
			NativeMethods.MapWindowPoints(this.adornerWindow.Handle, IntPtr.Zero, point, 1);
			return new Point(point.x, point.y);
		}

		public Point AdornerWindowToScreen()
		{
			Point point = new Point(0, 0);
			return this.AdornerWindowPointToScreen(point);
		}

		public Point ControlToAdornerWindow(Control c)
		{
			if (c.Parent == null)
			{
				return Point.Empty;
			}
			NativeMethods.POINT point = new NativeMethods.POINT();
			point.x = c.Left;
			point.y = c.Top;
			NativeMethods.MapWindowPoints(c.Parent.Handle, this.adornerWindow.Handle, point, 1);
			if (c.Parent.IsMirrored)
			{
				point.x -= c.Width;
			}
			return new Point(point.x, point.y);
		}

		public Point MapAdornerWindowPoint(IntPtr handle, Point pt)
		{
			NativeMethods.POINT point = new NativeMethods.POINT();
			point.x = pt.X;
			point.y = pt.Y;
			NativeMethods.MapWindowPoints(handle, this.adornerWindow.Handle, point, 1);
			return new Point(point.x, point.y);
		}

		public Rectangle ControlRectInAdornerWindow(Control c)
		{
			if (c.Parent == null)
			{
				return Rectangle.Empty;
			}
			Point point = this.ControlToAdornerWindow(c);
			return new Rectangle(point, c.Size);
		}

		internal bool IsDisposed
		{
			get
			{
				return this.adornerWindow == null || this.adornerWindow.IsDisposed;
			}
		}

		private Control DropSource
		{
			get
			{
				if (this.dropSource == null)
				{
					this.dropSource = new Control();
				}
				return this.dropSource;
			}
		}

		internal string[] RecentSnapLines
		{
			set
			{
				this.testHook_RecentSnapLines = value;
			}
		}

		public event BehaviorDragDropEventHandler BeginDrag
		{
			add
			{
				this.beginDragHandler = (BehaviorDragDropEventHandler)Delegate.Combine(this.beginDragHandler, value);
			}
			remove
			{
				this.beginDragHandler = (BehaviorDragDropEventHandler)Delegate.Remove(this.beginDragHandler, value);
			}
		}

		public event BehaviorDragDropEventHandler EndDrag
		{
			add
			{
				this.endDragHandler = (BehaviorDragDropEventHandler)Delegate.Combine(this.endDragHandler, value);
			}
			remove
			{
				this.endDragHandler = (BehaviorDragDropEventHandler)Delegate.Remove(this.endDragHandler, value);
			}
		}

		public event EventHandler Synchronize
		{
			add
			{
				this.synchronizeEventHandler = (EventHandler)Delegate.Combine(this.synchronizeEventHandler, value);
			}
			remove
			{
				this.synchronizeEventHandler = (EventHandler)Delegate.Remove(this.synchronizeEventHandler, value);
			}
		}

		public void Dispose()
		{
			IOverlayService overlayService = (IOverlayService)this.serviceProvider.GetService(typeof(IOverlayService));
			if (overlayService != null)
			{
				overlayService.RemoveOverlay(this.adornerWindow);
			}
			if (this.dropSource != null)
			{
				this.dropSource.Dispose();
			}
			IMenuCommandService menuCommandService = this.serviceProvider.GetService(typeof(IMenuCommandService)) as IMenuCommandService;
			IDesignerHost designerHost = this.serviceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
			BehaviorService.MenuCommandHandler menuCommandHandler = null;
			if (menuCommandService != null)
			{
				menuCommandHandler = menuCommandService as BehaviorService.MenuCommandHandler;
			}
			if (menuCommandHandler != null && designerHost != null)
			{
				IMenuCommandService menuService = menuCommandHandler.MenuService;
				designerHost.RemoveService(typeof(IMenuCommandService));
				designerHost.AddService(typeof(IMenuCommandService), menuService);
			}
			this.adornerWindow.Dispose();
			SystemEvents.DisplaySettingsChanged -= this.OnSystemSettingChanged;
			SystemEvents.InstalledFontsChanged -= this.OnSystemSettingChanged;
			SystemEvents.UserPreferenceChanged -= this.OnUserPreferenceChanged;
		}

		internal DragDropEffects DoDragDrop(DropSourceBehavior dropSourceBehavior)
		{
			this.DropSource.QueryContinueDrag += dropSourceBehavior.QueryContinueDrag;
			this.DropSource.GiveFeedback += dropSourceBehavior.GiveFeedback;
			DragDropEffects dragDropEffects = DragDropEffects.None;
			ICollection dragComponents = ((DropSourceBehavior.BehaviorDataObject)dropSourceBehavior.DataObject).DragComponents;
			BehaviorDragDropEventArgs behaviorDragDropEventArgs = new BehaviorDragDropEventArgs(dragComponents);
			try
			{
				try
				{
					this.OnBeginDrag(behaviorDragDropEventArgs);
					this.dragging = true;
					this.cancelDrag = false;
					this.dragEnterReplies.Clear();
					dragDropEffects = this.DropSource.DoDragDrop(dropSourceBehavior.DataObject, dropSourceBehavior.AllowedEffects);
				}
				finally
				{
					this.DropSource.QueryContinueDrag -= dropSourceBehavior.QueryContinueDrag;
					this.DropSource.GiveFeedback -= dropSourceBehavior.GiveFeedback;
					this.EndDragNotification();
					this.validDragArgs = null;
					this.dragging = false;
					this.cancelDrag = false;
					this.OnEndDrag(behaviorDragDropEventArgs);
				}
			}
			catch (CheckoutException ex)
			{
				if (ex != CheckoutException.Canceled)
				{
					throw;
				}
				dragDropEffects = DragDropEffects.None;
			}
			finally
			{
				if (dropSourceBehavior != null)
				{
					dropSourceBehavior.CleanupDrag();
				}
			}
			return dragDropEffects;
		}

		private void TestHook_GetAllSnapLines(ref Message m)
		{
			string text = "";
			IDesignerHost designerHost = this.serviceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (designerHost == null)
			{
				return;
			}
			foreach (object obj in designerHost.Container.Components)
			{
				Component component = (Component)obj;
				if (component is Control)
				{
					ControlDesigner controlDesigner = designerHost.GetDesigner(component) as ControlDesigner;
					if (controlDesigner != null)
					{
						foreach (object obj2 in controlDesigner.SnapLines)
						{
							SnapLine snapLine = (SnapLine)obj2;
							string text2 = text;
							text = string.Concat(new string[]
							{
								text2,
								snapLine.ToString(),
								"\tAssociated Control = ",
								controlDesigner.Control.Name,
								":::"
							});
						}
					}
				}
			}
			this.TestHook_SetText(ref m, text);
		}

		internal void EndDragNotification()
		{
			this.adornerWindow.EndDragNotification();
		}

		private MenuCommand FindCommand(CommandID commandID, IMenuCommandService menuService)
		{
			Behavior appropriateBehavior = this.GetAppropriateBehavior(this.hitTestedGlyph);
			if (appropriateBehavior != null)
			{
				if (appropriateBehavior.DisableAllCommands)
				{
					MenuCommand menuCommand = menuService.FindCommand(commandID);
					if (menuCommand != null)
					{
						menuCommand.Enabled = false;
					}
					return menuCommand;
				}
				MenuCommand menuCommand2 = appropriateBehavior.FindCommand(commandID);
				if (menuCommand2 != null)
				{
					return menuCommand2;
				}
			}
			return menuService.FindCommand(commandID);
		}

		private void TestHook_GetRecentSnapLines(ref Message m)
		{
			string text = "";
			if (this.testHook_RecentSnapLines != null)
			{
				foreach (string text2 in this.testHook_RecentSnapLines)
				{
					text = text + text2 + "\n";
				}
			}
			this.TestHook_SetText(ref m, text);
		}

		private void TestHook_SetText(ref Message m, string text)
		{
			if (m.LParam == IntPtr.Zero)
			{
				m.Result = (IntPtr)((text.Length + 1) * Marshal.SystemDefaultCharSize);
				return;
			}
			if ((int)(long)m.WParam < text.Length + 1)
			{
				m.Result = (IntPtr)(-1);
				return;
			}
			char[] array = new char[1];
			char[] array2 = array;
			byte[] array3;
			byte[] array4;
			if (Marshal.SystemDefaultCharSize == 1)
			{
				array3 = Encoding.Default.GetBytes(text);
				array4 = Encoding.Default.GetBytes(array2);
			}
			else
			{
				array3 = Encoding.Unicode.GetBytes(text);
				array4 = Encoding.Unicode.GetBytes(array2);
			}
			Marshal.Copy(array3, 0, m.LParam, array3.Length);
			Marshal.Copy(array4, 0, (IntPtr)((long)m.LParam + (long)array3.Length), array4.Length);
			m.Result = (IntPtr)((array3.Length + array4.Length) / Marshal.SystemDefaultCharSize);
		}

		private Behavior GetAppropriateBehavior(Glyph g)
		{
			if (this.behaviorStack != null && this.behaviorStack.Count > 0)
			{
				return this.behaviorStack[0] as Behavior;
			}
			if (g != null && g.Behavior != null)
			{
				return g.Behavior;
			}
			return null;
		}

		public Behavior GetNextBehavior(Behavior behavior)
		{
			if (this.behaviorStack != null && this.behaviorStack.Count > 0)
			{
				int num = this.behaviorStack.IndexOf(behavior);
				if (num != -1 && num < this.behaviorStack.Count - 1)
				{
					return this.behaviorStack[num + 1] as Behavior;
				}
			}
			return null;
		}

		internal Glyph[] GetIntersectingGlyphs(Glyph primaryGlyph)
		{
			if (primaryGlyph == null)
			{
				return new Glyph[0];
			}
			Rectangle bounds = primaryGlyph.Bounds;
			ArrayList arrayList = new ArrayList();
			for (int i = this.adorners.Count - 1; i >= 0; i--)
			{
				if (this.adorners[i].Enabled)
				{
					for (int j = 0; j < this.adorners[i].Glyphs.Count; j++)
					{
						Glyph glyph = this.adorners[i].Glyphs[j];
						if (bounds.IntersectsWith(glyph.Bounds))
						{
							arrayList.Add(glyph);
						}
					}
				}
			}
			if (arrayList.Count == 0)
			{
				return new Glyph[0];
			}
			return (Glyph[])arrayList.ToArray(typeof(Glyph));
		}

		private void HookMouseEvent()
		{
			if (!this.trackingMouseEvent)
			{
				this.trackingMouseEvent = true;
				if (this.trackMouseEvent == null)
				{
					this.trackMouseEvent = new NativeMethods.TRACKMOUSEEVENT();
					this.trackMouseEvent.dwFlags = NativeMethods.TME_HOVER;
					this.trackMouseEvent.hwndTrack = this.adornerWindow.Handle;
				}
				SafeNativeMethods.TrackMouseEvent(this.trackMouseEvent);
			}
		}

		public void Invalidate()
		{
			this.adornerWindow.InvalidateAdornerWindow();
		}

		public void Invalidate(Rectangle rect)
		{
			this.adornerWindow.InvalidateAdornerWindow(rect);
		}

		public void Invalidate(Region r)
		{
			this.adornerWindow.InvalidateAdornerWindow(r);
		}

		private void InvokeMouseEnterLeave(Glyph leaveGlyph, Glyph enterGlyph)
		{
			if (leaveGlyph != null)
			{
				if (enterGlyph != null && leaveGlyph.Equals(enterGlyph))
				{
					return;
				}
				if (this.validDragArgs != null)
				{
					this.OnDragLeave(leaveGlyph, EventArgs.Empty);
				}
				else
				{
					this.OnMouseLeave(leaveGlyph);
				}
			}
			if (enterGlyph != null)
			{
				if (this.validDragArgs != null)
				{
					this.OnDragEnter(enterGlyph, this.validDragArgs);
					return;
				}
				this.OnMouseEnter(enterGlyph);
			}
		}

		public void SyncSelection()
		{
			if (this.synchronizeEventHandler != null)
			{
				this.synchronizeEventHandler(this, EventArgs.Empty);
			}
		}

		private void OnSystemSettingChanged(object sender, EventArgs e)
		{
			this.SyncSelection();
			DesignerUtils.SyncBrushes();
		}

		private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			this.SyncSelection();
			DesignerUtils.SyncBrushes();
		}

		public Behavior PopBehavior(Behavior behavior)
		{
			if (this.behaviorStack.Count == 0)
			{
				throw new InvalidOperationException();
			}
			int num = this.behaviorStack.IndexOf(behavior);
			if (num == -1)
			{
				return null;
			}
			this.behaviorStack.RemoveAt(num);
			if (behavior == this.captureBehavior)
			{
				this.adornerWindow.Capture = false;
				if (this.captureBehavior != null)
				{
					this.OnLoseCapture();
				}
			}
			return behavior;
		}

		internal void ProcessPaintMessage(Rectangle paintRect)
		{
			this.adornerWindow.Invalidate(paintRect);
		}

		private bool PropagateHitTest(Point pt)
		{
			for (int i = this.adorners.Count - 1; i >= 0; i--)
			{
				if (this.adorners[i].Enabled)
				{
					for (int j = 0; j < this.adorners[i].Glyphs.Count; j++)
					{
						Cursor hitTest = this.adorners[i].Glyphs[j].GetHitTest(pt);
						if (hitTest != null)
						{
							Glyph glyph = this.adorners[i].Glyphs[j];
							this.InvokeMouseEnterLeave(this.hitTestedGlyph, glyph);
							if (this.validDragArgs == null)
							{
								this.SetAppropriateCursor(hitTest);
							}
							this.hitTestedGlyph = glyph;
							return this.hitTestedGlyph.Behavior is ControlDesigner.TransparentBehavior;
						}
					}
				}
			}
			this.InvokeMouseEnterLeave(this.hitTestedGlyph, null);
			if (this.validDragArgs == null)
			{
				Cursor cursor = Cursors.Default;
				if (this.behaviorStack != null && this.behaviorStack.Count > 0)
				{
					Behavior behavior = this.behaviorStack[0] as Behavior;
					if (behavior != null)
					{
						cursor = behavior.Cursor;
					}
				}
				this.SetAppropriateCursor(cursor);
			}
			this.hitTestedGlyph = null;
			return true;
		}

		private void PropagatePaint(PaintEventArgs pe)
		{
			for (int i = 0; i < this.adorners.Count; i++)
			{
				if (this.adorners[i].Enabled)
				{
					for (int j = this.adorners[i].Glyphs.Count - 1; j >= 0; j--)
					{
						this.adorners[i].Glyphs[j].Paint(pe);
					}
				}
			}
		}

		public void PushBehavior(Behavior behavior)
		{
			if (behavior == null)
			{
				throw new ArgumentNullException("behavior");
			}
			this.behaviorStack.Insert(0, behavior);
			if (this.captureBehavior != null && this.captureBehavior != behavior)
			{
				this.OnLoseCapture();
			}
		}

		public void PushCaptureBehavior(Behavior behavior)
		{
			this.PushBehavior(behavior);
			this.captureBehavior = behavior;
			this.adornerWindow.Capture = true;
			IUIService iuiservice = (IUIService)this.serviceProvider.GetService(typeof(IUIService));
			if (iuiservice != null)
			{
				IWin32Window dialogOwnerWindow = iuiservice.GetDialogOwnerWindow();
				if (dialogOwnerWindow != null && dialogOwnerWindow.Handle != IntPtr.Zero && dialogOwnerWindow.Handle != UnsafeNativeMethods.GetActiveWindow())
				{
					UnsafeNativeMethods.SetActiveWindow(new HandleRef(this, dialogOwnerWindow.Handle));
				}
			}
		}

		public Point ScreenToAdornerWindow(Point p)
		{
			NativeMethods.POINT point = new NativeMethods.POINT();
			point.x = p.X;
			point.y = p.Y;
			NativeMethods.MapWindowPoints(IntPtr.Zero, this.adornerWindow.Handle, point, 1);
			return new Point(point.x, point.y);
		}

		private void SetAppropriateCursor(Cursor cursor)
		{
			if (cursor == Cursors.Default)
			{
				if (this.toolboxSvc == null)
				{
					this.toolboxSvc = (IToolboxService)this.serviceProvider.GetService(typeof(IToolboxService));
				}
				if (this.toolboxSvc != null && this.toolboxSvc.SetCursor())
				{
					cursor = new Cursor(NativeMethods.GetCursor());
				}
			}
			this.adornerWindow.Cursor = cursor;
		}

		private void ShowError(Exception ex)
		{
			IUIService iuiservice = this.serviceProvider.GetService(typeof(IUIService)) as IUIService;
			if (iuiservice != null)
			{
				iuiservice.ShowError(ex);
			}
		}

		internal void StartDragNotification()
		{
			this.adornerWindow.StartDragNotification();
		}

		private void UnHookMouseEvent()
		{
			this.trackingMouseEvent = false;
		}

		private void OnBeginDrag(BehaviorDragDropEventArgs e)
		{
			if (this.beginDragHandler != null)
			{
				this.beginDragHandler(this, e);
			}
		}

		private void OnEndDrag(BehaviorDragDropEventArgs e)
		{
			if (this.endDragHandler != null)
			{
				this.endDragHandler(this, e);
			}
		}

		internal void OnLoseCapture()
		{
			if (this.captureBehavior != null)
			{
				Behavior behavior = this.captureBehavior;
				this.captureBehavior = null;
				try
				{
					behavior.OnLoseCapture(this.hitTestedGlyph, EventArgs.Empty);
				}
				catch
				{
				}
			}
		}

		private bool OnMouseDoubleClick(MouseButtons button, Point mouseLoc)
		{
			Behavior appropriateBehavior = this.GetAppropriateBehavior(this.hitTestedGlyph);
			return appropriateBehavior != null && appropriateBehavior.OnMouseDoubleClick(this.hitTestedGlyph, button, mouseLoc);
		}

		private bool OnMouseDown(MouseButtons button, Point mouseLoc)
		{
			Behavior appropriateBehavior = this.GetAppropriateBehavior(this.hitTestedGlyph);
			return appropriateBehavior != null && appropriateBehavior.OnMouseDown(this.hitTestedGlyph, button, mouseLoc);
		}

		private bool OnMouseEnter(Glyph g)
		{
			Behavior appropriateBehavior = this.GetAppropriateBehavior(g);
			return appropriateBehavior != null && appropriateBehavior.OnMouseEnter(g);
		}

		private bool OnMouseHover(Point mouseLoc)
		{
			Behavior appropriateBehavior = this.GetAppropriateBehavior(this.hitTestedGlyph);
			return appropriateBehavior != null && appropriateBehavior.OnMouseHover(this.hitTestedGlyph, mouseLoc);
		}

		private bool OnMouseLeave(Glyph g)
		{
			this.UnHookMouseEvent();
			Behavior appropriateBehavior = this.GetAppropriateBehavior(g);
			return appropriateBehavior != null && appropriateBehavior.OnMouseLeave(g);
		}

		private bool OnMouseMove(MouseButtons button, Point mouseLoc)
		{
			this.HookMouseEvent();
			Behavior appropriateBehavior = this.GetAppropriateBehavior(this.hitTestedGlyph);
			return appropriateBehavior != null && appropriateBehavior.OnMouseMove(this.hitTestedGlyph, button, mouseLoc);
		}

		private bool OnMouseUp(MouseButtons button)
		{
			this.dragEnterReplies.Clear();
			this.validDragArgs = null;
			Behavior appropriateBehavior = this.GetAppropriateBehavior(this.hitTestedGlyph);
			return appropriateBehavior != null && appropriateBehavior.OnMouseUp(this.hitTestedGlyph, button);
		}

		private void OnDragDrop(DragEventArgs e)
		{
			this.validDragArgs = null;
			Behavior appropriateBehavior = this.GetAppropriateBehavior(this.hitTestedGlyph);
			if (appropriateBehavior == null)
			{
				return;
			}
			appropriateBehavior.OnDragDrop(this.hitTestedGlyph, e);
		}

		private void OnDragEnter(Glyph g, DragEventArgs e)
		{
			if (g == null)
			{
				g = this.hitTestedGlyph;
			}
			Behavior appropriateBehavior = this.GetAppropriateBehavior(g);
			if (appropriateBehavior == null)
			{
				return;
			}
			appropriateBehavior.OnDragEnter(g, e);
			if (g != null && g is ControlBodyGlyph && e.Effect == DragDropEffects.None)
			{
				this.dragEnterReplies[g] = this;
			}
		}

		private void OnDragLeave(Glyph g, EventArgs e)
		{
			this.dragEnterReplies.Clear();
			if (g == null)
			{
				g = this.hitTestedGlyph;
			}
			Behavior appropriateBehavior = this.GetAppropriateBehavior(g);
			if (appropriateBehavior == null)
			{
				return;
			}
			appropriateBehavior.OnDragLeave(g, e);
		}

		private void OnDragOver(DragEventArgs e)
		{
			this.validDragArgs = e;
			Behavior appropriateBehavior = this.GetAppropriateBehavior(this.hitTestedGlyph);
			if (appropriateBehavior == null)
			{
				e.Effect = DragDropEffects.None;
				return;
			}
			if (this.hitTestedGlyph == null || (this.hitTestedGlyph != null && !this.dragEnterReplies.ContainsKey(this.hitTestedGlyph)))
			{
				appropriateBehavior.OnDragOver(this.hitTestedGlyph, e);
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		private void OnGiveFeedback(GiveFeedbackEventArgs e)
		{
			Behavior appropriateBehavior = this.GetAppropriateBehavior(this.hitTestedGlyph);
			if (appropriateBehavior == null)
			{
				return;
			}
			appropriateBehavior.OnGiveFeedback(this.hitTestedGlyph, e);
		}

		private void OnQueryContinueDrag(QueryContinueDragEventArgs e)
		{
			Behavior appropriateBehavior = this.GetAppropriateBehavior(this.hitTestedGlyph);
			if (appropriateBehavior == null)
			{
				return;
			}
			appropriateBehavior.OnQueryContinueDrag(this.hitTestedGlyph, e);
		}

		private const string ToolboxFormat = ".NET Toolbox Item";

		private IServiceProvider serviceProvider;

		private BehaviorService.AdornerWindow adornerWindow;

		private BehaviorServiceAdornerCollection adorners;

		private ArrayList behaviorStack;

		private Behavior captureBehavior;

		private Glyph hitTestedGlyph;

		private IToolboxService toolboxSvc;

		private Control dropSource;

		private DragEventArgs validDragArgs;

		private BehaviorDragDropEventHandler beginDragHandler;

		private BehaviorDragDropEventHandler endDragHandler;

		private EventHandler synchronizeEventHandler;

		private NativeMethods.TRACKMOUSEEVENT trackMouseEvent;

		private bool trackingMouseEvent;

		private string[] testHook_RecentSnapLines;

		private BehaviorService.MenuCommandHandler menuCommandHandler;

		private bool useSnapLines;

		private bool queriedSnapLines;

		private Hashtable dragEnterReplies;

		private static TraceSwitch dragDropSwitch = new TraceSwitch("BSDRAGDROP", "Behavior service drag & drop messages");

		private bool dragging;

		private bool cancelDrag;

		private int adornerWindowIndex = -1;

		private static int WM_GETALLSNAPLINES;

		private static int WM_GETRECENTSNAPLINES;

		private DesignerActionUI actionPointer;

		private class AdornerWindow : Control
		{
			internal AdornerWindow(BehaviorService behaviorService, Control designerFrame)
			{
				this.behaviorService = behaviorService;
				this.designerFrame = designerFrame;
				this.Dock = DockStyle.Fill;
				this.AllowDrop = true;
				this.Text = "AdornerWindow";
				base.SetStyle(ControlStyles.Opaque, true);
				designerFrame.HandleDestroyed += this.OnDesignerFrameHandleDestroyed;
			}

			protected override CreateParams CreateParams
			{
				get
				{
					CreateParams createParams = base.CreateParams;
					createParams.Style &= -100663297;
					createParams.ExStyle |= 32;
					return createParams;
				}
			}

			internal bool ProcessingDrag
			{
				get
				{
					return this.processingDrag;
				}
				set
				{
					this.processingDrag = value;
				}
			}

			protected override void OnHandleCreated(EventArgs e)
			{
				base.OnHandleCreated(e);
				BehaviorService.AdornerWindow.AdornerWindowList.Add(this);
				if (BehaviorService.AdornerWindow.mouseHook == null)
				{
					BehaviorService.AdornerWindow.mouseHook = new BehaviorService.AdornerWindow.MouseHook();
				}
			}

			protected override void OnHandleDestroyed(EventArgs e)
			{
				BehaviorService.AdornerWindow.AdornerWindowList.Remove(this);
				if (BehaviorService.AdornerWindow.AdornerWindowList.Count == 0 && BehaviorService.AdornerWindow.mouseHook != null)
				{
					BehaviorService.AdornerWindow.mouseHook.Dispose();
					BehaviorService.AdornerWindow.mouseHook = null;
				}
				base.OnHandleDestroyed(e);
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing && this.designerFrame != null)
				{
					this.designerFrame.HandleDestroyed -= this.OnDesignerFrameHandleDestroyed;
					this.designerFrame = null;
				}
				base.Dispose(disposing);
			}

			internal Control DesignerFrame
			{
				get
				{
					return this.designerFrame;
				}
			}

			internal Rectangle DesignerFrameDisplayRectangle
			{
				get
				{
					if (this.DesignerFrameValid)
					{
						return ((DesignerFrame)this.designerFrame).DisplayRectangle;
					}
					return Rectangle.Empty;
				}
			}

			private bool DesignerFrameValid
			{
				get
				{
					return this.designerFrame != null && !this.designerFrame.IsDisposed && this.designerFrame.IsHandleCreated;
				}
			}

			internal void EndDragNotification()
			{
				this.ProcessingDrag = false;
			}

			internal void InvalidateAdornerWindow()
			{
				if (this.DesignerFrameValid)
				{
					this.designerFrame.Invalidate(true);
					this.designerFrame.Update();
				}
			}

			internal void InvalidateAdornerWindow(Region region)
			{
				if (this.DesignerFrameValid)
				{
					Point autoScrollPosition = ((DesignerFrame)this.designerFrame).AutoScrollPosition;
					region.Translate(autoScrollPosition.X, autoScrollPosition.Y);
					this.designerFrame.Invalidate(region, true);
					this.designerFrame.Update();
				}
			}

			internal void InvalidateAdornerWindow(Rectangle rectangle)
			{
				if (this.DesignerFrameValid)
				{
					Point autoScrollPosition = ((DesignerFrame)this.designerFrame).AutoScrollPosition;
					rectangle.Offset(autoScrollPosition.X, autoScrollPosition.Y);
					this.designerFrame.Invalidate(rectangle, true);
					this.designerFrame.Update();
				}
			}

			private void OnDesignerFrameHandleDestroyed(object s, EventArgs e)
			{
				if (BehaviorService.AdornerWindow.mouseHook != null)
				{
					BehaviorService.AdornerWindow.mouseHook.Dispose();
					BehaviorService.AdornerWindow.mouseHook = null;
				}
			}

			protected override void OnDragDrop(DragEventArgs e)
			{
				try
				{
					this.behaviorService.OnDragDrop(e);
				}
				finally
				{
					this.ProcessingDrag = false;
				}
			}

			private static bool IsLocalDrag(DragEventArgs e)
			{
				if (e.Data is DropSourceBehavior.BehaviorDataObject)
				{
					return true;
				}
				string[] formats = e.Data.GetFormats();
				for (int i = 0; i < formats.Length; i++)
				{
					if (formats[i].Length == ".NET Toolbox Item".Length && string.Equals(".NET Toolbox Item", formats[i]))
					{
						return true;
					}
				}
				return false;
			}

			protected override void OnDragEnter(DragEventArgs e)
			{
				this.ProcessingDrag = true;
				if (!BehaviorService.AdornerWindow.IsLocalDrag(e))
				{
					this.behaviorService.validDragArgs = e;
					NativeMethods.POINT point = new NativeMethods.POINT();
					NativeMethods.GetCursorPos(point);
					NativeMethods.MapWindowPoints(IntPtr.Zero, base.Handle, point, 1);
					Point point2 = new Point(point.x, point.y);
					this.behaviorService.PropagateHitTest(point2);
				}
				this.behaviorService.OnDragEnter(null, e);
			}

			protected override void OnDragLeave(EventArgs e)
			{
				this.behaviorService.validDragArgs = null;
				try
				{
					this.behaviorService.OnDragLeave(null, e);
				}
				finally
				{
					this.ProcessingDrag = false;
				}
			}

			protected override void OnDragOver(DragEventArgs e)
			{
				this.ProcessingDrag = true;
				if (!BehaviorService.AdornerWindow.IsLocalDrag(e))
				{
					this.behaviorService.validDragArgs = e;
					NativeMethods.POINT point = new NativeMethods.POINT();
					NativeMethods.GetCursorPos(point);
					NativeMethods.MapWindowPoints(IntPtr.Zero, base.Handle, point, 1);
					Point point2 = new Point(point.x, point.y);
					this.behaviorService.PropagateHitTest(point2);
				}
				this.behaviorService.OnDragOver(e);
			}

			protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
			{
				this.behaviorService.OnGiveFeedback(e);
			}

			protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
			{
				this.behaviorService.OnQueryContinueDrag(e);
			}

			internal void StartDragNotification()
			{
				this.ProcessingDrag = true;
			}

			protected override void WndProc(ref Message m)
			{
				if (m.Msg == BehaviorService.WM_GETALLSNAPLINES)
				{
					this.behaviorService.TestHook_GetAllSnapLines(ref m);
				}
				else if (m.Msg == BehaviorService.WM_GETRECENTSNAPLINES)
				{
					this.behaviorService.TestHook_GetRecentSnapLines(ref m);
				}
				int msg = m.Msg;
				if (msg != 15)
				{
					if (msg != 132)
					{
						if (msg != 533)
						{
							base.WndProc(ref m);
							return;
						}
						base.WndProc(ref m);
						this.behaviorService.OnLoseCapture();
						return;
					}
				}
				else
				{
					IntPtr intPtr = NativeMethods.CreateRectRgn(0, 0, 0, 0);
					NativeMethods.GetUpdateRgn(m.HWnd, intPtr, true);
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					NativeMethods.GetUpdateRect(m.HWnd, ref rect, true);
					Rectangle rectangle = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
					try
					{
						using (Region region = Region.FromHrgn(intPtr))
						{
							this.DefWndProc(ref m);
							using (Graphics graphics = Graphics.FromHwnd(m.HWnd))
							{
								using (PaintEventArgs paintEventArgs = new PaintEventArgs(graphics, rectangle))
								{
									graphics.Clip = region;
									this.behaviorService.PropagatePaint(paintEventArgs);
								}
							}
						}
						return;
					}
					finally
					{
						NativeMethods.DeleteObject(intPtr);
					}
				}
				Point point = new Point((int)((short)NativeMethods.Util.LOWORD((int)m.LParam)), (int)((short)NativeMethods.Util.HIWORD((int)m.LParam)));
				NativeMethods.POINT point2 = new NativeMethods.POINT();
				point2.x = 0;
				point2.y = 0;
				NativeMethods.MapWindowPoints(IntPtr.Zero, base.Handle, point2, 1);
				point.Offset(point2.x, point2.y);
				if (this.behaviorService.PropagateHitTest(point) && !this.ProcessingDrag)
				{
					m.Result = (IntPtr)(-1);
					return;
				}
				m.Result = (IntPtr)1;
			}

			private bool WndProcProxy(ref Message m, int x, int y)
			{
				Point point = new Point(x, y);
				this.behaviorService.PropagateHitTest(point);
				int msg = m.Msg;
				switch (msg)
				{
				case 512:
					if (this.behaviorService.OnMouseMove(Control.MouseButtons, point))
					{
						return false;
					}
					break;
				case 513:
					if (this.behaviorService.OnMouseDown(MouseButtons.Left, point))
					{
						return false;
					}
					break;
				case 514:
					if (this.behaviorService.OnMouseUp(MouseButtons.Left))
					{
						return false;
					}
					break;
				case 515:
					if (this.behaviorService.OnMouseDoubleClick(MouseButtons.Left, point))
					{
						return false;
					}
					break;
				case 516:
					if (this.behaviorService.OnMouseDown(MouseButtons.Right, point))
					{
						return false;
					}
					break;
				case 517:
					if (this.behaviorService.OnMouseUp(MouseButtons.Right))
					{
						return false;
					}
					break;
				case 518:
					if (this.behaviorService.OnMouseDoubleClick(MouseButtons.Right, point))
					{
						return false;
					}
					break;
				default:
					if (msg == 673)
					{
						if (this.behaviorService.OnMouseHover(point))
						{
							return false;
						}
					}
					break;
				}
				return true;
			}

			private BehaviorService behaviorService;

			private Control designerFrame;

			private static BehaviorService.AdornerWindow.MouseHook mouseHook;

			private static List<BehaviorService.AdornerWindow> AdornerWindowList = new List<BehaviorService.AdornerWindow>();

			private bool processingDrag;

			private class MouseHook
			{
				public MouseHook()
				{
					this.HookMouse();
				}

				public void Dispose()
				{
					this.UnhookMouse();
				}

				private void HookMouse()
				{
					lock (this)
					{
						if (!(this.mouseHookHandle != IntPtr.Zero) && BehaviorService.AdornerWindow.AdornerWindowList.Count != 0)
						{
							if (this.thisProcessID == 0)
							{
								BehaviorService.AdornerWindow adornerWindow = BehaviorService.AdornerWindow.AdornerWindowList[0];
								UnsafeNativeMethods.GetWindowThreadProcessId(new HandleRef(adornerWindow, adornerWindow.Handle), out this.thisProcessID);
							}
							UnsafeNativeMethods.HookProc hookProc = new UnsafeNativeMethods.HookProc(this.MouseHookProc);
							this.mouseHookRoot = GCHandle.Alloc(hookProc);
							this.mouseHookHandle = UnsafeNativeMethods.SetWindowsHookEx(7, hookProc, new HandleRef(null, IntPtr.Zero), AppDomain.GetCurrentThreadId());
							if (this.mouseHookHandle != IntPtr.Zero)
							{
								this.isHooked = true;
							}
						}
					}
				}

				private unsafe IntPtr MouseHookProc(int nCode, IntPtr wparam, IntPtr lparam)
				{
					if (this.isHooked && nCode == 0)
					{
						NativeMethods.MOUSEHOOKSTRUCT* ptr = (NativeMethods.MOUSEHOOKSTRUCT*)(void*)lparam;
						if (ptr != null)
						{
							try
							{
								if (this.ProcessMouseMessage(ptr->hWnd, (int)wparam, ptr->pt_x, ptr->pt_y))
								{
									return (IntPtr)1;
								}
							}
							catch (Exception ex)
							{
								this.currentAdornerWindow.Capture = false;
								if (ex != CheckoutException.Canceled)
								{
									this.currentAdornerWindow.behaviorService.ShowError(ex);
								}
								if (ClientUtils.IsCriticalException(ex))
								{
									throw;
								}
							}
						}
					}
					return UnsafeNativeMethods.CallNextHookEx(new HandleRef(this, this.mouseHookHandle), nCode, wparam, lparam);
				}

				private void UnhookMouse()
				{
					lock (this)
					{
						if (this.mouseHookHandle != IntPtr.Zero)
						{
							UnsafeNativeMethods.UnhookWindowsHookEx(new HandleRef(this, this.mouseHookHandle));
							this.mouseHookRoot.Free();
							this.mouseHookHandle = IntPtr.Zero;
							this.isHooked = false;
						}
					}
				}

				private bool ProcessMouseMessage(IntPtr hWnd, int msg, int x, int y)
				{
					if (this.processingMessage)
					{
						return false;
					}
					new NamedPermissionSet("FullTrust").Assert();
					foreach (BehaviorService.AdornerWindow adornerWindow in BehaviorService.AdornerWindow.AdornerWindowList)
					{
						this.currentAdornerWindow = adornerWindow;
						IntPtr handle = adornerWindow.DesignerFrame.Handle;
						if (adornerWindow.ProcessingDrag || (hWnd != handle && SafeNativeMethods.IsChild(new HandleRef(this, handle), new HandleRef(this, hWnd))))
						{
							int num;
							UnsafeNativeMethods.GetWindowThreadProcessId(new HandleRef(null, hWnd), out num);
							if (num != this.thisProcessID)
							{
								return false;
							}
							try
							{
								this.processingMessage = true;
								NativeMethods.POINT point = new NativeMethods.POINT();
								point.x = x;
								point.y = y;
								NativeMethods.MapWindowPoints(IntPtr.Zero, adornerWindow.Handle, point, 1);
								Message message = Message.Create(hWnd, msg, (IntPtr)0, (IntPtr)BehaviorService.AdornerWindow.MouseHook.MAKELONG(point.y, point.x));
								if (message.Msg == 513)
								{
									this.lastLButtonDownTimeStamp = UnsafeNativeMethods.GetMessageTime();
								}
								else if (message.Msg == 515)
								{
									int messageTime = UnsafeNativeMethods.GetMessageTime();
									if (messageTime == this.lastLButtonDownTimeStamp)
									{
										return true;
									}
								}
								if (!adornerWindow.WndProcProxy(ref message, point.x, point.y))
								{
									return true;
								}
								break;
							}
							finally
							{
								this.processingMessage = false;
								this.currentAdornerWindow = null;
							}
						}
					}
					return false;
				}

				public static int MAKELONG(int low, int high)
				{
					return (high << 16) | (low & 65535);
				}

				private BehaviorService.AdornerWindow currentAdornerWindow;

				private int thisProcessID;

				private GCHandle mouseHookRoot;

				private IntPtr mouseHookHandle = IntPtr.Zero;

				private bool processingMessage;

				private bool isHooked;

				private int lastLButtonDownTimeStamp;
			}
		}

		private class MenuCommandHandler : IMenuCommandService
		{
			public MenuCommandHandler(BehaviorService owner, IMenuCommandService menuService)
			{
				this.owner = owner;
				this.menuService = menuService;
			}

			public IMenuCommandService MenuService
			{
				get
				{
					return this.menuService;
				}
			}

			void IMenuCommandService.AddCommand(MenuCommand command)
			{
				this.menuService.AddCommand(command);
			}

			void IMenuCommandService.RemoveVerb(DesignerVerb verb)
			{
				this.menuService.RemoveVerb(verb);
			}

			void IMenuCommandService.RemoveCommand(MenuCommand command)
			{
				this.menuService.RemoveCommand(command);
			}

			MenuCommand IMenuCommandService.FindCommand(CommandID commandID)
			{
				MenuCommand menuCommand;
				try
				{
					if (this.currentCommands.Contains(commandID))
					{
						menuCommand = null;
					}
					else
					{
						this.currentCommands.Push(commandID);
						menuCommand = this.owner.FindCommand(commandID, this.menuService);
					}
				}
				finally
				{
					this.currentCommands.Pop();
				}
				return menuCommand;
			}

			bool IMenuCommandService.GlobalInvoke(CommandID commandID)
			{
				return this.menuService.GlobalInvoke(commandID);
			}

			void IMenuCommandService.ShowContextMenu(CommandID menuID, int x, int y)
			{
				this.menuService.ShowContextMenu(menuID, x, y);
			}

			void IMenuCommandService.AddVerb(DesignerVerb verb)
			{
				this.menuService.AddVerb(verb);
			}

			DesignerVerbCollection IMenuCommandService.Verbs
			{
				get
				{
					return this.menuService.Verbs;
				}
			}

			private BehaviorService owner;

			private IMenuCommandService menuService;

			private Stack<CommandID> currentCommands = new Stack<CommandID>();
		}
	}
}
