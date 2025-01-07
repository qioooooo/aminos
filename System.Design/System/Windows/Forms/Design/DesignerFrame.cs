using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design.Behavior;
using Microsoft.Win32;

namespace System.Windows.Forms.Design
{
	internal class DesignerFrame : Control, IOverlayService, ISplitWindowService
	{
		public DesignerFrame(ISite site)
		{
			this.Text = "DesignerFrame";
			this.designerSite = site;
			this.designerRegion = new DesignerFrame.OverlayControl(site);
			base.Controls.Add(this.designerRegion);
			this.designerRegion.AutoScroll = true;
			this.designerRegion.Dock = DockStyle.Fill;
			SystemEvents.UserPreferenceChanged += this.OnUserPreferenceChanged;
		}

		internal Point AutoScrollPosition
		{
			get
			{
				return this.designerRegion.AutoScrollPosition;
			}
		}

		private BehaviorService BehaviorService
		{
			get
			{
				if (this.behaviorService == null)
				{
					this.behaviorService = this.designerSite.GetService(typeof(BehaviorService)) as BehaviorService;
				}
				return this.behaviorService;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.designer != null)
			{
				Control control = this.designer;
				this.designer = null;
				control.Visible = false;
				control.Parent = null;
				SystemEvents.UserPreferenceChanged -= this.OnUserPreferenceChanged;
			}
			base.Dispose(disposing);
		}

		private void ForceDesignerRedraw(bool focus)
		{
			if (this.designer != null && this.designer.IsHandleCreated)
			{
				NativeMethods.SendMessage(this.designer.Handle, 134, focus ? 1 : 0, 0);
				SafeNativeMethods.RedrawWindow(this.designer.Handle, null, IntPtr.Zero, 1024);
			}
		}

		public void Initialize(Control view)
		{
			this.designer = view;
			Form form = this.designer as Form;
			if (form != null)
			{
				form.TopLevel = false;
			}
			this.designerRegion.Controls.Add(this.designer);
			this.SyncDesignerUI();
			this.designer.Visible = true;
			this.designer.Enabled = true;
			IntPtr handle = this.designer.Handle;
		}

		protected override void OnGotFocus(EventArgs e)
		{
			this.ForceDesignerRedraw(true);
			ISelectionService selectionService = (ISelectionService)this.designerSite.GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				Control control = selectionService.PrimarySelection as Control;
				if (control != null)
				{
					UnsafeNativeMethods.NotifyWinEvent(32773, new HandleRef(control, control.Handle), -4, 0);
				}
			}
		}

		protected override void OnLostFocus(EventArgs e)
		{
			this.ForceDesignerRedraw(false);
		}

		private void OnSplitterMoved(object sender, SplitterEventArgs e)
		{
			IComponentChangeService componentChangeService = this.designerSite.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if (componentChangeService != null)
			{
				try
				{
					componentChangeService.OnComponentChanging(this.designerSite.Component, null);
					componentChangeService.OnComponentChanged(this.designerSite.Component, null, null, null);
				}
				catch
				{
				}
			}
		}

		private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			if (e.Category == UserPreferenceCategory.Window)
			{
				this.SyncDesignerUI();
			}
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			return false;
		}

		private void SyncDesignerUI()
		{
			Size adornmentDimensions = DesignerUtils.GetAdornmentDimensions(AdornmentType.Maximum);
			this.designerRegion.AutoScrollMargin = adornmentDimensions;
			this.designer.Location = new Point(adornmentDimensions.Width, adornmentDimensions.Height);
			if (this.BehaviorService != null)
			{
				this.BehaviorService.SyncSelection();
			}
		}

		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 123)
			{
				if (msg != 256)
				{
					if (msg == 522 && !this.designerRegion.messageMouseWheelProcessed)
					{
						this.designerRegion.messageMouseWheelProcessed = true;
						NativeMethods.SendMessage(this.designerRegion.Handle, 522, m.WParam, m.LParam);
						return;
					}
				}
				else
				{
					int num = 0;
					int num2 = 0;
					switch ((int)m.WParam & 65535)
					{
					case 33:
						num = 2;
						num2 = 277;
						break;
					case 34:
						num = 3;
						num2 = 277;
						break;
					case 35:
						num = 7;
						num2 = 277;
						break;
					case 36:
						num = 6;
						num2 = 277;
						break;
					case 37:
						num = 0;
						num2 = 276;
						break;
					case 38:
						num = 0;
						num2 = 277;
						break;
					case 39:
						num = 1;
						num2 = 276;
						break;
					case 40:
						num = 1;
						num2 = 277;
						break;
					}
					if (num2 == 277 || num2 == 276)
					{
						NativeMethods.SendMessage(this.designerRegion.Handle, num2, NativeMethods.Util.MAKELONG(num, 0), 0);
						return;
					}
				}
				base.WndProc(ref m);
				return;
			}
			NativeMethods.SendMessage(this.designer.Handle, m.Msg, m.WParam, m.LParam);
		}

		int IOverlayService.PushOverlay(Control control)
		{
			return this.designerRegion.PushOverlay(control);
		}

		void IOverlayService.RemoveOverlay(Control control)
		{
			this.designerRegion.RemoveOverlay(control);
		}

		void IOverlayService.InsertOverlay(Control control, int index)
		{
			this.designerRegion.InsertOverlay(control, index);
		}

		void IOverlayService.InvalidateOverlays(Rectangle screenRectangle)
		{
			this.designerRegion.InvalidateOverlays(screenRectangle);
		}

		void IOverlayService.InvalidateOverlays(Region screenRegion)
		{
			this.designerRegion.InvalidateOverlays(screenRegion);
		}

		void ISplitWindowService.AddSplitWindow(Control window)
		{
			if (this.splitter == null)
			{
				this.splitter = new Splitter();
				this.splitter.BackColor = SystemColors.Control;
				this.splitter.BorderStyle = BorderStyle.Fixed3D;
				this.splitter.Height = 7;
				this.splitter.Dock = DockStyle.Bottom;
				this.splitter.SplitterMoved += this.OnSplitterMoved;
			}
			base.SuspendLayout();
			window.Dock = DockStyle.Bottom;
			int num = 80;
			if (window.Height < num)
			{
				window.Height = num;
			}
			base.Controls.Add(this.splitter);
			base.Controls.Add(window);
			base.ResumeLayout();
		}

		void ISplitWindowService.RemoveSplitWindow(Control window)
		{
			base.SuspendLayout();
			base.Controls.Remove(window);
			base.Controls.Remove(this.splitter);
			base.ResumeLayout();
		}

		private ISite designerSite;

		private DesignerFrame.OverlayControl designerRegion;

		private Splitter splitter;

		private Control designer;

		private BehaviorService behaviorService;

		private class OverlayControl : ScrollableControl
		{
			public OverlayControl(IServiceProvider provider)
			{
				this.provider = provider;
				this.overlayList = new ArrayList();
				this.AutoScroll = true;
				this.Text = "OverlayControl";
			}

			protected override AccessibleObject CreateAccessibilityInstance()
			{
				return new DesignerFrame.OverlayControl.OverlayControlAccessibleObject(this);
			}

			private BehaviorService BehaviorService
			{
				get
				{
					if (this.behaviorService == null)
					{
						this.behaviorService = this.provider.GetService(typeof(BehaviorService)) as BehaviorService;
					}
					return this.behaviorService;
				}
			}

			protected override void OnCreateControl()
			{
				base.OnCreateControl();
				if (this.overlayList != null)
				{
					foreach (object obj in this.overlayList)
					{
						Control control = (Control)obj;
						this.ParentOverlay(control);
					}
				}
				if (this.BehaviorService != null)
				{
					this.BehaviorService.SyncSelection();
				}
			}

			protected override void OnLayout(LayoutEventArgs e)
			{
				base.OnLayout(e);
				Rectangle displayRectangle = this.DisplayRectangle;
				if (this.overlayList != null)
				{
					foreach (object obj in this.overlayList)
					{
						Control control = (Control)obj;
						control.Bounds = displayRectangle;
					}
				}
			}

			private void ParentOverlay(Control control)
			{
				NativeMethods.SetParent(control.Handle, base.Handle);
				SafeNativeMethods.SetWindowPos(control.Handle, (IntPtr)0, 0, 0, 0, 0, 3);
			}

			public int PushOverlay(Control control)
			{
				this.overlayList.Add(control);
				if (base.IsHandleCreated)
				{
					this.ParentOverlay(control);
					control.Bounds = this.DisplayRectangle;
				}
				return this.overlayList.IndexOf(control);
			}

			public void RemoveOverlay(Control control)
			{
				this.overlayList.Remove(control);
				control.Visible = false;
				control.Parent = null;
			}

			public void InsertOverlay(Control control, int index)
			{
				Control control2 = (Control)this.overlayList[index];
				this.RemoveOverlay(control2);
				this.PushOverlay(control);
				this.PushOverlay(control2);
				control2.Visible = true;
			}

			public void InvalidateOverlays(Rectangle screenRectangle)
			{
				for (int i = this.overlayList.Count - 1; i >= 0; i--)
				{
					Control control = this.overlayList[i] as Control;
					if (control != null)
					{
						Rectangle rectangle = new Rectangle(control.PointToClient(screenRectangle.Location), screenRectangle.Size);
						if (control.ClientRectangle.IntersectsWith(rectangle))
						{
							control.Invalidate(rectangle);
						}
					}
				}
			}

			public void InvalidateOverlays(Region screenRegion)
			{
				for (int i = this.overlayList.Count - 1; i >= 0; i--)
				{
					Control control = this.overlayList[i] as Control;
					if (control != null)
					{
						Rectangle bounds = control.Bounds;
						bounds.Location = control.PointToScreen(control.Location);
						using (Region region = screenRegion.Clone())
						{
							region.Intersect(bounds);
							region.Translate(-bounds.X, -bounds.Y);
							control.Invalidate(region);
						}
					}
				}
			}

			protected override void WndProc(ref Message m)
			{
				base.WndProc(ref m);
				if (m.Msg == 528 && NativeMethods.Util.LOWORD((int)m.WParam) == 1)
				{
					if (this.overlayList == null)
					{
						return;
					}
					bool flag = false;
					foreach (object obj in this.overlayList)
					{
						Control control = (Control)obj;
						if (control.IsHandleCreated && m.LParam == control.Handle)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						return;
					}
					using (IEnumerator enumerator2 = this.overlayList.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							Control control2 = (Control)obj2;
							SafeNativeMethods.SetWindowPos(control2.Handle, (IntPtr)0, 0, 0, 0, 0, 3);
						}
						return;
					}
				}
				if ((m.Msg == 277 || m.Msg == 276) && this.BehaviorService != null)
				{
					this.BehaviorService.SyncSelection();
					return;
				}
				if (m.Msg == 522)
				{
					this.messageMouseWheelProcessed = false;
					if (this.BehaviorService != null)
					{
						this.BehaviorService.SyncSelection();
					}
				}
			}

			private ArrayList overlayList;

			private IServiceProvider provider;

			internal bool messageMouseWheelProcessed;

			private BehaviorService behaviorService;

			public class OverlayControlAccessibleObject : Control.ControlAccessibleObject
			{
				public OverlayControlAccessibleObject(DesignerFrame.OverlayControl owner)
					: base(owner)
				{
				}

				public override AccessibleObject HitTest(int x, int y)
				{
					foreach (object obj in base.Owner.Controls)
					{
						Control control = (Control)obj;
						AccessibleObject accessibilityObject = control.AccessibilityObject;
						if (accessibilityObject.Bounds.Contains(x, y))
						{
							return accessibilityObject;
						}
					}
					return base.HitTest(x, y);
				}
			}
		}
	}
}
