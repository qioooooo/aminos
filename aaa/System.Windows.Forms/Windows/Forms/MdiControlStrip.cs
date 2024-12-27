using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Forms
{
	// Token: 0x020004A7 RID: 1191
	internal class MdiControlStrip : MenuStrip
	{
		// Token: 0x06004762 RID: 18274 RVA: 0x00102F40 File Offset: 0x00101F40
		public MdiControlStrip(IWin32Window target)
		{
			IntPtr systemMenu = UnsafeNativeMethods.GetSystemMenu(new HandleRef(this, Control.GetSafeHandle(target)), false);
			this.target = target;
			this.minimize = new MdiControlStrip.ControlBoxMenuItem(systemMenu, 61472, target);
			this.close = new MdiControlStrip.ControlBoxMenuItem(systemMenu, 61536, target);
			this.restore = new MdiControlStrip.ControlBoxMenuItem(systemMenu, 61728, target);
			this.system = new MdiControlStrip.SystemMenuItem();
			Control control = target as Control;
			if (control != null)
			{
				control.HandleCreated += this.OnTargetWindowHandleRecreated;
				control.Disposed += this.OnTargetWindowDisposed;
			}
			this.Items.AddRange(new ToolStripItem[] { this.minimize, this.restore, this.close, this.system });
			base.SuspendLayout();
			foreach (object obj in this.Items)
			{
				ToolStripItem toolStripItem = (ToolStripItem)obj;
				toolStripItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
				toolStripItem.MergeIndex = 0;
				toolStripItem.MergeAction = MergeAction.Insert;
				toolStripItem.Overflow = ToolStripItemOverflow.Never;
				toolStripItem.Alignment = ToolStripItemAlignment.Right;
				toolStripItem.Padding = Padding.Empty;
			}
			this.system.Image = this.GetTargetWindowIcon();
			this.system.Alignment = ToolStripItemAlignment.Left;
			this.system.DropDownOpening += this.OnSystemMenuDropDownOpening;
			this.system.ImageScaling = ToolStripItemImageScaling.None;
			this.system.DoubleClickEnabled = true;
			this.system.DoubleClick += this.OnSystemMenuDoubleClick;
			this.system.Padding = Padding.Empty;
			this.system.ShortcutKeys = Keys.LButton | Keys.MButton | Keys.Back | Keys.ShiftKey | Keys.Space | Keys.F17 | Keys.Alt;
			base.ResumeLayout(false);
		}

		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x06004763 RID: 18275 RVA: 0x00103120 File Offset: 0x00102120
		public ToolStripMenuItem Close
		{
			get
			{
				return this.close;
			}
		}

		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x06004764 RID: 18276 RVA: 0x00103128 File Offset: 0x00102128
		// (set) Token: 0x06004765 RID: 18277 RVA: 0x00103130 File Offset: 0x00102130
		internal MenuStrip MergedMenu
		{
			get
			{
				return this.mergedMenu;
			}
			set
			{
				this.mergedMenu = value;
			}
		}

		// Token: 0x06004766 RID: 18278 RVA: 0x0010313C File Offset: 0x0010213C
		private Image GetTargetWindowIcon()
		{
			Image image = null;
			IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, Control.GetSafeHandle(this.target)), 127, 0, 0);
			IntSecurity.ObjectFromWin32Handle.Assert();
			try
			{
				Icon icon = ((intPtr != IntPtr.Zero) ? Icon.FromHandle(intPtr) : Form.DefaultIcon);
				Icon icon2 = new Icon(icon, SystemInformation.SmallIconSize);
				image = icon2.ToBitmap();
				icon2.Dispose();
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return image;
		}

		// Token: 0x06004767 RID: 18279 RVA: 0x001031C0 File Offset: 0x001021C0
		protected internal override void OnItemAdded(ToolStripItemEventArgs e)
		{
			base.OnItemAdded(e);
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x001031C9 File Offset: 0x001021C9
		private void OnTargetWindowDisposed(object sender, EventArgs e)
		{
			this.UnhookTarget();
			this.target = null;
		}

		// Token: 0x06004769 RID: 18281 RVA: 0x001031D8 File Offset: 0x001021D8
		private void OnTargetWindowHandleRecreated(object sender, EventArgs e)
		{
			this.system.SetNativeTargetWindow(this.target);
			this.minimize.SetNativeTargetWindow(this.target);
			this.close.SetNativeTargetWindow(this.target);
			this.restore.SetNativeTargetWindow(this.target);
			IntPtr systemMenu = UnsafeNativeMethods.GetSystemMenu(new HandleRef(this, Control.GetSafeHandle(this.target)), false);
			this.system.SetNativeTargetMenu(systemMenu);
			this.minimize.SetNativeTargetMenu(systemMenu);
			this.close.SetNativeTargetMenu(systemMenu);
			this.restore.SetNativeTargetMenu(systemMenu);
			if (this.system.HasDropDownItems)
			{
				this.system.DropDown.Items.Clear();
				this.system.DropDown.Dispose();
			}
			this.system.Image = this.GetTargetWindowIcon();
		}

		// Token: 0x0600476A RID: 18282 RVA: 0x001032B4 File Offset: 0x001022B4
		private void OnSystemMenuDropDownOpening(object sender, EventArgs e)
		{
			if (!this.system.HasDropDownItems && this.target != null)
			{
				this.system.DropDown = ToolStripDropDownMenu.FromHMenu(UnsafeNativeMethods.GetSystemMenu(new HandleRef(this, Control.GetSafeHandle(this.target)), false), this.target);
				return;
			}
			if (this.MergedMenu == null)
			{
				this.system.DropDown.Dispose();
			}
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x0010331C File Offset: 0x0010231C
		private void OnSystemMenuDoubleClick(object sender, EventArgs e)
		{
			this.Close.PerformClick();
		}

		// Token: 0x0600476C RID: 18284 RVA: 0x00103329 File Offset: 0x00102329
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.UnhookTarget();
				this.target = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600476D RID: 18285 RVA: 0x00103344 File Offset: 0x00102344
		private void UnhookTarget()
		{
			if (this.target != null)
			{
				Control control = this.target as Control;
				if (control != null)
				{
					control.HandleCreated -= this.OnTargetWindowHandleRecreated;
					control.Disposed -= this.OnTargetWindowDisposed;
				}
				this.target = null;
			}
		}

		// Token: 0x040021DA RID: 8666
		private ToolStripMenuItem system;

		// Token: 0x040021DB RID: 8667
		private ToolStripMenuItem close;

		// Token: 0x040021DC RID: 8668
		private ToolStripMenuItem minimize;

		// Token: 0x040021DD RID: 8669
		private ToolStripMenuItem restore;

		// Token: 0x040021DE RID: 8670
		private MenuStrip mergedMenu;

		// Token: 0x040021DF RID: 8671
		private IWin32Window target;

		// Token: 0x020004AC RID: 1196
		internal class ControlBoxMenuItem : ToolStripMenuItem
		{
			// Token: 0x060047F2 RID: 18418 RVA: 0x0010561A File Offset: 0x0010461A
			internal ControlBoxMenuItem(IntPtr hMenu, int nativeMenuCommandId, IWin32Window targetWindow)
				: base(hMenu, nativeMenuCommandId, targetWindow)
			{
			}

			// Token: 0x17000E58 RID: 3672
			// (get) Token: 0x060047F3 RID: 18419 RVA: 0x00105625 File Offset: 0x00104625
			internal override bool CanKeyboardSelect
			{
				get
				{
					return false;
				}
			}
		}

		// Token: 0x020004AD RID: 1197
		internal class SystemMenuItem : ToolStripMenuItem
		{
			// Token: 0x060047F5 RID: 18421 RVA: 0x00105630 File Offset: 0x00104630
			protected internal override bool ProcessCmdKey(ref Message m, Keys keyData)
			{
				if (base.Visible && base.ShortcutKeys == keyData)
				{
					base.ShowDropDown();
					base.DropDown.SelectNextToolStripItem(null, true);
					return true;
				}
				return base.ProcessCmdKey(ref m, keyData);
			}

			// Token: 0x060047F6 RID: 18422 RVA: 0x00105661 File Offset: 0x00104661
			protected override void OnOwnerChanged(EventArgs e)
			{
				if (this.HasDropDownItems && base.DropDown.Visible)
				{
					base.HideDropDown();
				}
				base.OnOwnerChanged(e);
			}
		}
	}
}
