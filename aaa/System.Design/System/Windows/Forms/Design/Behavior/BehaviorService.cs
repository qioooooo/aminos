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
	// Token: 0x020002E7 RID: 743
	public sealed class BehaviorService : IDisposable
	{
		// Token: 0x06001C7C RID: 7292 RVA: 0x0009F964 File Offset: 0x0009E964
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

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001C7D RID: 7293 RVA: 0x0009FAC6 File Offset: 0x0009EAC6
		public BehaviorServiceAdornerCollection Adorners
		{
			get
			{
				return this.adorners;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001C7E RID: 7294 RVA: 0x0009FACE File Offset: 0x0009EACE
		internal int AdornerWindowIndex
		{
			get
			{
				return this.adornerWindowIndex;
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06001C7F RID: 7295 RVA: 0x0009FAD6 File Offset: 0x0009EAD6
		internal Control AdornerWindowControl
		{
			get
			{
				return this.adornerWindow;
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001C80 RID: 7296 RVA: 0x0009FAE0 File Offset: 0x0009EAE0
		public Graphics AdornerWindowGraphics
		{
			get
			{
				Graphics graphics = this.adornerWindow.CreateGraphics();
				graphics.Clip = new Region(this.adornerWindow.DesignerFrameDisplayRectangle);
				return graphics;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001C81 RID: 7297 RVA: 0x0009FB10 File Offset: 0x0009EB10
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

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001C82 RID: 7298 RVA: 0x0009FB3B File Offset: 0x0009EB3B
		// (set) Token: 0x06001C83 RID: 7299 RVA: 0x0009FB43 File Offset: 0x0009EB43
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

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001C84 RID: 7300 RVA: 0x0009FB4C File Offset: 0x0009EB4C
		// (set) Token: 0x06001C85 RID: 7301 RVA: 0x0009FB54 File Offset: 0x0009EB54
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

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001C86 RID: 7302 RVA: 0x0009FB5D File Offset: 0x0009EB5D
		internal bool Dragging
		{
			get
			{
				return this.dragging;
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001C87 RID: 7303 RVA: 0x0009FB65 File Offset: 0x0009EB65
		internal bool HasCapture
		{
			get
			{
				return this.captureBehavior != null;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001C88 RID: 7304 RVA: 0x0009FB73 File Offset: 0x0009EB73
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

		// Token: 0x06001C89 RID: 7305 RVA: 0x0009FB9C File Offset: 0x0009EB9C
		public Point AdornerWindowPointToScreen(Point p)
		{
			NativeMethods.POINT point = new NativeMethods.POINT(p.X, p.Y);
			NativeMethods.MapWindowPoints(this.adornerWindow.Handle, IntPtr.Zero, point, 1);
			return new Point(point.x, point.y);
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x0009FBE8 File Offset: 0x0009EBE8
		public Point AdornerWindowToScreen()
		{
			Point point = new Point(0, 0);
			return this.AdornerWindowPointToScreen(point);
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x0009FC08 File Offset: 0x0009EC08
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

		// Token: 0x06001C8C RID: 7308 RVA: 0x0009FC90 File Offset: 0x0009EC90
		public Point MapAdornerWindowPoint(IntPtr handle, Point pt)
		{
			NativeMethods.POINT point = new NativeMethods.POINT();
			point.x = pt.X;
			point.y = pt.Y;
			NativeMethods.MapWindowPoints(handle, this.adornerWindow.Handle, point, 1);
			return new Point(point.x, point.y);
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x0009FCE4 File Offset: 0x0009ECE4
		public Rectangle ControlRectInAdornerWindow(Control c)
		{
			if (c.Parent == null)
			{
				return Rectangle.Empty;
			}
			Point point = this.ControlToAdornerWindow(c);
			return new Rectangle(point, c.Size);
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001C8E RID: 7310 RVA: 0x0009FD13 File Offset: 0x0009ED13
		internal bool IsDisposed
		{
			get
			{
				return this.adornerWindow == null || this.adornerWindow.IsDisposed;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06001C8F RID: 7311 RVA: 0x0009FD2A File Offset: 0x0009ED2A
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

		// Token: 0x170004FC RID: 1276
		// (set) Token: 0x06001C90 RID: 7312 RVA: 0x0009FD45 File Offset: 0x0009ED45
		internal string[] RecentSnapLines
		{
			set
			{
				this.testHook_RecentSnapLines = value;
			}
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06001C91 RID: 7313 RVA: 0x0009FD4E File Offset: 0x0009ED4E
		// (remove) Token: 0x06001C92 RID: 7314 RVA: 0x0009FD67 File Offset: 0x0009ED67
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

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06001C93 RID: 7315 RVA: 0x0009FD80 File Offset: 0x0009ED80
		// (remove) Token: 0x06001C94 RID: 7316 RVA: 0x0009FD99 File Offset: 0x0009ED99
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

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06001C95 RID: 7317 RVA: 0x0009FDB2 File Offset: 0x0009EDB2
		// (remove) Token: 0x06001C96 RID: 7318 RVA: 0x0009FDCB File Offset: 0x0009EDCB
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

		// Token: 0x06001C97 RID: 7319 RVA: 0x0009FDE4 File Offset: 0x0009EDE4
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

		// Token: 0x06001C98 RID: 7320 RVA: 0x0009FEE0 File Offset: 0x0009EEE0
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

		// Token: 0x06001C99 RID: 7321 RVA: 0x000A0008 File Offset: 0x0009F008
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

		// Token: 0x06001C9A RID: 7322 RVA: 0x000A0148 File Offset: 0x0009F148
		internal void EndDragNotification()
		{
			this.adornerWindow.EndDragNotification();
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x000A0158 File Offset: 0x0009F158
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

		// Token: 0x06001C9C RID: 7324 RVA: 0x000A01A8 File Offset: 0x0009F1A8
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

		// Token: 0x06001C9D RID: 7325 RVA: 0x000A01F4 File Offset: 0x0009F1F4
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

		// Token: 0x06001C9E RID: 7326 RVA: 0x000A02DA File Offset: 0x0009F2DA
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

		// Token: 0x06001C9F RID: 7327 RVA: 0x000A0318 File Offset: 0x0009F318
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

		// Token: 0x06001CA0 RID: 7328 RVA: 0x000A0374 File Offset: 0x0009F374
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

		// Token: 0x06001CA1 RID: 7329 RVA: 0x000A043C File Offset: 0x0009F43C
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

		// Token: 0x06001CA2 RID: 7330 RVA: 0x000A049D File Offset: 0x0009F49D
		public void Invalidate()
		{
			this.adornerWindow.InvalidateAdornerWindow();
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x000A04AA File Offset: 0x0009F4AA
		public void Invalidate(Rectangle rect)
		{
			this.adornerWindow.InvalidateAdornerWindow(rect);
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x000A04B8 File Offset: 0x0009F4B8
		public void Invalidate(Region r)
		{
			this.adornerWindow.InvalidateAdornerWindow(r);
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x000A04C8 File Offset: 0x0009F4C8
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

		// Token: 0x06001CA6 RID: 7334 RVA: 0x000A0524 File Offset: 0x0009F524
		public void SyncSelection()
		{
			if (this.synchronizeEventHandler != null)
			{
				this.synchronizeEventHandler(this, EventArgs.Empty);
			}
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x000A053F File Offset: 0x0009F53F
		private void OnSystemSettingChanged(object sender, EventArgs e)
		{
			this.SyncSelection();
			DesignerUtils.SyncBrushes();
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x000A054C File Offset: 0x0009F54C
		private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			this.SyncSelection();
			DesignerUtils.SyncBrushes();
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x000A055C File Offset: 0x0009F55C
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

		// Token: 0x06001CAA RID: 7338 RVA: 0x000A05BF File Offset: 0x0009F5BF
		internal void ProcessPaintMessage(Rectangle paintRect)
		{
			this.adornerWindow.Invalidate(paintRect);
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x000A05D0 File Offset: 0x0009F5D0
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

		// Token: 0x06001CAC RID: 7340 RVA: 0x000A0710 File Offset: 0x0009F710
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

		// Token: 0x06001CAD RID: 7341 RVA: 0x000A0786 File Offset: 0x0009F786
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

		// Token: 0x06001CAE RID: 7342 RVA: 0x000A07BC File Offset: 0x0009F7BC
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

		// Token: 0x06001CAF RID: 7343 RVA: 0x000A0844 File Offset: 0x0009F844
		public Point ScreenToAdornerWindow(Point p)
		{
			NativeMethods.POINT point = new NativeMethods.POINT();
			point.x = p.X;
			point.y = p.Y;
			NativeMethods.MapWindowPoints(IntPtr.Zero, this.adornerWindow.Handle, point, 1);
			return new Point(point.x, point.y);
		}

		// Token: 0x06001CB0 RID: 7344 RVA: 0x000A089C File Offset: 0x0009F89C
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

		// Token: 0x06001CB1 RID: 7345 RVA: 0x000A090C File Offset: 0x0009F90C
		private void ShowError(Exception ex)
		{
			IUIService iuiservice = this.serviceProvider.GetService(typeof(IUIService)) as IUIService;
			if (iuiservice != null)
			{
				iuiservice.ShowError(ex);
			}
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x000A093E File Offset: 0x0009F93E
		internal void StartDragNotification()
		{
			this.adornerWindow.StartDragNotification();
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x000A094B File Offset: 0x0009F94B
		private void UnHookMouseEvent()
		{
			this.trackingMouseEvent = false;
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x000A0954 File Offset: 0x0009F954
		private void OnBeginDrag(BehaviorDragDropEventArgs e)
		{
			if (this.beginDragHandler != null)
			{
				this.beginDragHandler(this, e);
			}
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x000A096B File Offset: 0x0009F96B
		private void OnEndDrag(BehaviorDragDropEventArgs e)
		{
			if (this.endDragHandler != null)
			{
				this.endDragHandler(this, e);
			}
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x000A0984 File Offset: 0x0009F984
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

		// Token: 0x06001CB7 RID: 7351 RVA: 0x000A09D0 File Offset: 0x0009F9D0
		private bool OnMouseDoubleClick(MouseButtons button, Point mouseLoc)
		{
			Behavior appropriateBehavior = this.GetAppropriateBehavior(this.hitTestedGlyph);
			return appropriateBehavior != null && appropriateBehavior.OnMouseDoubleClick(this.hitTestedGlyph, button, mouseLoc);
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x000A0A00 File Offset: 0x0009FA00
		private bool OnMouseDown(MouseButtons button, Point mouseLoc)
		{
			Behavior appropriateBehavior = this.GetAppropriateBehavior(this.hitTestedGlyph);
			return appropriateBehavior != null && appropriateBehavior.OnMouseDown(this.hitTestedGlyph, button, mouseLoc);
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x000A0A30 File Offset: 0x0009FA30
		private bool OnMouseEnter(Glyph g)
		{
			Behavior appropriateBehavior = this.GetAppropriateBehavior(g);
			return appropriateBehavior != null && appropriateBehavior.OnMouseEnter(g);
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x000A0A54 File Offset: 0x0009FA54
		private bool OnMouseHover(Point mouseLoc)
		{
			Behavior appropriateBehavior = this.GetAppropriateBehavior(this.hitTestedGlyph);
			return appropriateBehavior != null && appropriateBehavior.OnMouseHover(this.hitTestedGlyph, mouseLoc);
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x000A0A80 File Offset: 0x0009FA80
		private bool OnMouseLeave(Glyph g)
		{
			this.UnHookMouseEvent();
			Behavior appropriateBehavior = this.GetAppropriateBehavior(g);
			return appropriateBehavior != null && appropriateBehavior.OnMouseLeave(g);
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x000A0AA8 File Offset: 0x0009FAA8
		private bool OnMouseMove(MouseButtons button, Point mouseLoc)
		{
			this.HookMouseEvent();
			Behavior appropriateBehavior = this.GetAppropriateBehavior(this.hitTestedGlyph);
			return appropriateBehavior != null && appropriateBehavior.OnMouseMove(this.hitTestedGlyph, button, mouseLoc);
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x000A0ADC File Offset: 0x0009FADC
		private bool OnMouseUp(MouseButtons button)
		{
			this.dragEnterReplies.Clear();
			this.validDragArgs = null;
			Behavior appropriateBehavior = this.GetAppropriateBehavior(this.hitTestedGlyph);
			return appropriateBehavior != null && appropriateBehavior.OnMouseUp(this.hitTestedGlyph, button);
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x000A0B1C File Offset: 0x0009FB1C
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

		// Token: 0x06001CBF RID: 7359 RVA: 0x000A0B50 File Offset: 0x0009FB50
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

		// Token: 0x06001CC0 RID: 7360 RVA: 0x000A0B9C File Offset: 0x0009FB9C
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

		// Token: 0x06001CC1 RID: 7361 RVA: 0x000A0BD4 File Offset: 0x0009FBD4
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

		// Token: 0x06001CC2 RID: 7362 RVA: 0x000A0C38 File Offset: 0x0009FC38
		private void OnGiveFeedback(GiveFeedbackEventArgs e)
		{
			Behavior appropriateBehavior = this.GetAppropriateBehavior(this.hitTestedGlyph);
			if (appropriateBehavior == null)
			{
				return;
			}
			appropriateBehavior.OnGiveFeedback(this.hitTestedGlyph, e);
		}

		// Token: 0x06001CC3 RID: 7363 RVA: 0x000A0C64 File Offset: 0x0009FC64
		private void OnQueryContinueDrag(QueryContinueDragEventArgs e)
		{
			Behavior appropriateBehavior = this.GetAppropriateBehavior(this.hitTestedGlyph);
			if (appropriateBehavior == null)
			{
				return;
			}
			appropriateBehavior.OnQueryContinueDrag(this.hitTestedGlyph, e);
		}

		// Token: 0x040015E7 RID: 5607
		private const string ToolboxFormat = ".NET Toolbox Item";

		// Token: 0x040015E8 RID: 5608
		private IServiceProvider serviceProvider;

		// Token: 0x040015E9 RID: 5609
		private BehaviorService.AdornerWindow adornerWindow;

		// Token: 0x040015EA RID: 5610
		private BehaviorServiceAdornerCollection adorners;

		// Token: 0x040015EB RID: 5611
		private ArrayList behaviorStack;

		// Token: 0x040015EC RID: 5612
		private Behavior captureBehavior;

		// Token: 0x040015ED RID: 5613
		private Glyph hitTestedGlyph;

		// Token: 0x040015EE RID: 5614
		private IToolboxService toolboxSvc;

		// Token: 0x040015EF RID: 5615
		private Control dropSource;

		// Token: 0x040015F0 RID: 5616
		private DragEventArgs validDragArgs;

		// Token: 0x040015F1 RID: 5617
		private BehaviorDragDropEventHandler beginDragHandler;

		// Token: 0x040015F2 RID: 5618
		private BehaviorDragDropEventHandler endDragHandler;

		// Token: 0x040015F3 RID: 5619
		private EventHandler synchronizeEventHandler;

		// Token: 0x040015F4 RID: 5620
		private NativeMethods.TRACKMOUSEEVENT trackMouseEvent;

		// Token: 0x040015F5 RID: 5621
		private bool trackingMouseEvent;

		// Token: 0x040015F6 RID: 5622
		private string[] testHook_RecentSnapLines;

		// Token: 0x040015F7 RID: 5623
		private BehaviorService.MenuCommandHandler menuCommandHandler;

		// Token: 0x040015F8 RID: 5624
		private bool useSnapLines;

		// Token: 0x040015F9 RID: 5625
		private bool queriedSnapLines;

		// Token: 0x040015FA RID: 5626
		private Hashtable dragEnterReplies;

		// Token: 0x040015FB RID: 5627
		private static TraceSwitch dragDropSwitch = new TraceSwitch("BSDRAGDROP", "Behavior service drag & drop messages");

		// Token: 0x040015FC RID: 5628
		private bool dragging;

		// Token: 0x040015FD RID: 5629
		private bool cancelDrag;

		// Token: 0x040015FE RID: 5630
		private int adornerWindowIndex = -1;

		// Token: 0x040015FF RID: 5631
		private static int WM_GETALLSNAPLINES;

		// Token: 0x04001600 RID: 5632
		private static int WM_GETRECENTSNAPLINES;

		// Token: 0x04001601 RID: 5633
		private DesignerActionUI actionPointer;

		// Token: 0x020002E8 RID: 744
		private class AdornerWindow : Control
		{
			// Token: 0x06001CC5 RID: 7365 RVA: 0x000A0CA8 File Offset: 0x0009FCA8
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

			// Token: 0x170004FD RID: 1277
			// (get) Token: 0x06001CC6 RID: 7366 RVA: 0x000A0CFC File Offset: 0x0009FCFC
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

			// Token: 0x170004FE RID: 1278
			// (get) Token: 0x06001CC7 RID: 7367 RVA: 0x000A0D32 File Offset: 0x0009FD32
			// (set) Token: 0x06001CC8 RID: 7368 RVA: 0x000A0D3A File Offset: 0x0009FD3A
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

			// Token: 0x06001CC9 RID: 7369 RVA: 0x000A0D43 File Offset: 0x0009FD43
			protected override void OnHandleCreated(EventArgs e)
			{
				base.OnHandleCreated(e);
				BehaviorService.AdornerWindow.AdornerWindowList.Add(this);
				if (BehaviorService.AdornerWindow.mouseHook == null)
				{
					BehaviorService.AdornerWindow.mouseHook = new BehaviorService.AdornerWindow.MouseHook();
				}
			}

			// Token: 0x06001CCA RID: 7370 RVA: 0x000A0D68 File Offset: 0x0009FD68
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

			// Token: 0x06001CCB RID: 7371 RVA: 0x000A0DA0 File Offset: 0x0009FDA0
			protected override void Dispose(bool disposing)
			{
				if (disposing && this.designerFrame != null)
				{
					this.designerFrame.HandleDestroyed -= this.OnDesignerFrameHandleDestroyed;
					this.designerFrame = null;
				}
				base.Dispose(disposing);
			}

			// Token: 0x170004FF RID: 1279
			// (get) Token: 0x06001CCC RID: 7372 RVA: 0x000A0DD2 File Offset: 0x0009FDD2
			internal Control DesignerFrame
			{
				get
				{
					return this.designerFrame;
				}
			}

			// Token: 0x17000500 RID: 1280
			// (get) Token: 0x06001CCD RID: 7373 RVA: 0x000A0DDA File Offset: 0x0009FDDA
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

			// Token: 0x17000501 RID: 1281
			// (get) Token: 0x06001CCE RID: 7374 RVA: 0x000A0DFA File Offset: 0x0009FDFA
			private bool DesignerFrameValid
			{
				get
				{
					return this.designerFrame != null && !this.designerFrame.IsDisposed && this.designerFrame.IsHandleCreated;
				}
			}

			// Token: 0x06001CCF RID: 7375 RVA: 0x000A0E21 File Offset: 0x0009FE21
			internal void EndDragNotification()
			{
				this.ProcessingDrag = false;
			}

			// Token: 0x06001CD0 RID: 7376 RVA: 0x000A0E2A File Offset: 0x0009FE2A
			internal void InvalidateAdornerWindow()
			{
				if (this.DesignerFrameValid)
				{
					this.designerFrame.Invalidate(true);
					this.designerFrame.Update();
				}
			}

			// Token: 0x06001CD1 RID: 7377 RVA: 0x000A0E4C File Offset: 0x0009FE4C
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

			// Token: 0x06001CD2 RID: 7378 RVA: 0x000A0EA0 File Offset: 0x0009FEA0
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

			// Token: 0x06001CD3 RID: 7379 RVA: 0x000A0EF3 File Offset: 0x0009FEF3
			private void OnDesignerFrameHandleDestroyed(object s, EventArgs e)
			{
				if (BehaviorService.AdornerWindow.mouseHook != null)
				{
					BehaviorService.AdornerWindow.mouseHook.Dispose();
					BehaviorService.AdornerWindow.mouseHook = null;
				}
			}

			// Token: 0x06001CD4 RID: 7380 RVA: 0x000A0F0C File Offset: 0x0009FF0C
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

			// Token: 0x06001CD5 RID: 7381 RVA: 0x000A0F40 File Offset: 0x0009FF40
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

			// Token: 0x06001CD6 RID: 7382 RVA: 0x000A0F9C File Offset: 0x0009FF9C
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

			// Token: 0x06001CD7 RID: 7383 RVA: 0x000A1014 File Offset: 0x000A0014
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

			// Token: 0x06001CD8 RID: 7384 RVA: 0x000A1054 File Offset: 0x000A0054
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

			// Token: 0x06001CD9 RID: 7385 RVA: 0x000A10C8 File Offset: 0x000A00C8
			protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
			{
				this.behaviorService.OnGiveFeedback(e);
			}

			// Token: 0x06001CDA RID: 7386 RVA: 0x000A10D6 File Offset: 0x000A00D6
			protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
			{
				this.behaviorService.OnQueryContinueDrag(e);
			}

			// Token: 0x06001CDB RID: 7387 RVA: 0x000A10E4 File Offset: 0x000A00E4
			internal void StartDragNotification()
			{
				this.ProcessingDrag = true;
			}

			// Token: 0x06001CDC RID: 7388 RVA: 0x000A10F0 File Offset: 0x000A00F0
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

			// Token: 0x06001CDD RID: 7389 RVA: 0x000A1318 File Offset: 0x000A0318
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

			// Token: 0x04001602 RID: 5634
			private BehaviorService behaviorService;

			// Token: 0x04001603 RID: 5635
			private Control designerFrame;

			// Token: 0x04001604 RID: 5636
			private static BehaviorService.AdornerWindow.MouseHook mouseHook;

			// Token: 0x04001605 RID: 5637
			private static List<BehaviorService.AdornerWindow> AdornerWindowList = new List<BehaviorService.AdornerWindow>();

			// Token: 0x04001606 RID: 5638
			private bool processingDrag;

			// Token: 0x020002E9 RID: 745
			private class MouseHook
			{
				// Token: 0x06001CDF RID: 7391 RVA: 0x000A1428 File Offset: 0x000A0428
				public MouseHook()
				{
					this.HookMouse();
				}

				// Token: 0x06001CE0 RID: 7392 RVA: 0x000A1441 File Offset: 0x000A0441
				public void Dispose()
				{
					this.UnhookMouse();
				}

				// Token: 0x06001CE1 RID: 7393 RVA: 0x000A144C File Offset: 0x000A044C
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

				// Token: 0x06001CE2 RID: 7394 RVA: 0x000A1518 File Offset: 0x000A0518
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

				// Token: 0x06001CE3 RID: 7395 RVA: 0x000A15C0 File Offset: 0x000A05C0
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

				// Token: 0x06001CE4 RID: 7396 RVA: 0x000A1630 File Offset: 0x000A0630
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

				// Token: 0x06001CE5 RID: 7397 RVA: 0x000A17E4 File Offset: 0x000A07E4
				public static int MAKELONG(int low, int high)
				{
					return (high << 16) | (low & 65535);
				}

				// Token: 0x04001607 RID: 5639
				private BehaviorService.AdornerWindow currentAdornerWindow;

				// Token: 0x04001608 RID: 5640
				private int thisProcessID;

				// Token: 0x04001609 RID: 5641
				private GCHandle mouseHookRoot;

				// Token: 0x0400160A RID: 5642
				private IntPtr mouseHookHandle = IntPtr.Zero;

				// Token: 0x0400160B RID: 5643
				private bool processingMessage;

				// Token: 0x0400160C RID: 5644
				private bool isHooked;

				// Token: 0x0400160D RID: 5645
				private int lastLButtonDownTimeStamp;
			}
		}

		// Token: 0x020002EA RID: 746
		private class MenuCommandHandler : IMenuCommandService
		{
			// Token: 0x06001CE6 RID: 7398 RVA: 0x000A17F2 File Offset: 0x000A07F2
			public MenuCommandHandler(BehaviorService owner, IMenuCommandService menuService)
			{
				this.owner = owner;
				this.menuService = menuService;
			}

			// Token: 0x17000502 RID: 1282
			// (get) Token: 0x06001CE7 RID: 7399 RVA: 0x000A1813 File Offset: 0x000A0813
			public IMenuCommandService MenuService
			{
				get
				{
					return this.menuService;
				}
			}

			// Token: 0x06001CE8 RID: 7400 RVA: 0x000A181B File Offset: 0x000A081B
			void IMenuCommandService.AddCommand(MenuCommand command)
			{
				this.menuService.AddCommand(command);
			}

			// Token: 0x06001CE9 RID: 7401 RVA: 0x000A1829 File Offset: 0x000A0829
			void IMenuCommandService.RemoveVerb(DesignerVerb verb)
			{
				this.menuService.RemoveVerb(verb);
			}

			// Token: 0x06001CEA RID: 7402 RVA: 0x000A1837 File Offset: 0x000A0837
			void IMenuCommandService.RemoveCommand(MenuCommand command)
			{
				this.menuService.RemoveCommand(command);
			}

			// Token: 0x06001CEB RID: 7403 RVA: 0x000A1848 File Offset: 0x000A0848
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

			// Token: 0x06001CEC RID: 7404 RVA: 0x000A18A8 File Offset: 0x000A08A8
			bool IMenuCommandService.GlobalInvoke(CommandID commandID)
			{
				return this.menuService.GlobalInvoke(commandID);
			}

			// Token: 0x06001CED RID: 7405 RVA: 0x000A18B6 File Offset: 0x000A08B6
			void IMenuCommandService.ShowContextMenu(CommandID menuID, int x, int y)
			{
				this.menuService.ShowContextMenu(menuID, x, y);
			}

			// Token: 0x06001CEE RID: 7406 RVA: 0x000A18C6 File Offset: 0x000A08C6
			void IMenuCommandService.AddVerb(DesignerVerb verb)
			{
				this.menuService.AddVerb(verb);
			}

			// Token: 0x17000503 RID: 1283
			// (get) Token: 0x06001CEF RID: 7407 RVA: 0x000A18D4 File Offset: 0x000A08D4
			DesignerVerbCollection IMenuCommandService.Verbs
			{
				get
				{
					return this.menuService.Verbs;
				}
			}

			// Token: 0x0400160E RID: 5646
			private BehaviorService owner;

			// Token: 0x0400160F RID: 5647
			private IMenuCommandService menuService;

			// Token: 0x04001610 RID: 5648
			private Stack<CommandID> currentCommands = new Stack<CommandID>();
		}
	}
}
