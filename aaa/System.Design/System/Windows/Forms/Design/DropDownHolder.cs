using System;
using System.Design;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200022D RID: 557
	internal partial class DropDownHolder : Form
	{
		// Token: 0x06001540 RID: 5440 RVA: 0x0006EDF4 File Offset: 0x0006DDF4
		public DropDownHolder(Control parent)
		{
			this.parent = parent;
			base.ShowInTaskbar = false;
			base.ControlBox = false;
			base.MinimizeBox = false;
			base.MaximizeBox = false;
			this.Text = "";
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.StartPosition = FormStartPosition.Manual;
			this.Font = parent.Font;
			base.Visible = false;
			this.BackColor = SystemColors.Window;
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06001541 RID: 5441 RVA: 0x0006EE64 File Offset: 0x0006DE64
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 128;
				createParams.Style |= -2139095040;
				if (this.parent != null)
				{
					createParams.Parent = this.parent.Handle;
				}
				return createParams;
			}
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x0006EEB6 File Offset: 0x0006DEB6
		public void DoModalLoop()
		{
			while (base.Visible)
			{
				Application.DoEvents();
				UnsafeNativeMethods.MsgWaitForMultipleObjectsEx(0, IntPtr.Zero, 250, 255, 4);
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06001543 RID: 5443 RVA: 0x0006EEDE File Offset: 0x0006DEDE
		public virtual Control Component
		{
			get
			{
				return this.currentControl;
			}
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x0006EEE6 File Offset: 0x0006DEE6
		public virtual bool GetUsed()
		{
			return this.currentControl != null;
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x0006EEF4 File Offset: 0x0006DEF4
		protected override void OnMouseDown(MouseEventArgs me)
		{
			if (me.Button == MouseButtons.Left)
			{
				base.Visible = false;
			}
			base.OnMouseDown(me);
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x0006EF14 File Offset: 0x0006DF14
		private bool OwnsWindow(IntPtr hWnd)
		{
			while (hWnd != IntPtr.Zero)
			{
				hWnd = UnsafeNativeMethods.GetWindowLong(new HandleRef(null, hWnd), -8);
				if (hWnd == IntPtr.Zero)
				{
					return false;
				}
				if (hWnd == base.Handle)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x0006EF60 File Offset: 0x0006DF60
		public virtual void FocusComponent()
		{
			if (this.currentControl != null && base.Visible)
			{
				this.currentControl.Focus();
			}
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x0006EF80 File Offset: 0x0006DF80
		private void OnCurrentControlResize(object o, EventArgs e)
		{
			if (this.currentControl != null)
			{
				int width = base.Width;
				this.UpdateSize();
				this.currentControl.Location = new Point(1, 1);
				base.Left -= base.Width - width;
			}
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x0006EFCC File Offset: 0x0006DFCC
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & (Keys.Shift | Keys.Control | Keys.Alt)) == Keys.None)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys == Keys.Return)
				{
					return true;
				}
				if (keys == Keys.Escape)
				{
					base.Visible = false;
					return true;
				}
				if (keys == Keys.F4)
				{
					return true;
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x0006F010 File Offset: 0x0006E010
		public virtual void SetComponent(Control ctl)
		{
			if (this.currentControl != null)
			{
				base.Controls.Remove(this.currentControl);
				this.currentControl = null;
			}
			if (ctl != null)
			{
				base.Controls.Add(ctl);
				ctl.Location = new Point(1, 1);
				ctl.Visible = true;
				this.currentControl = ctl;
				this.UpdateSize();
				this.currentControl.Resize += this.OnCurrentControlResize;
			}
			base.Enabled = this.currentControl != null;
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x0006F096 File Offset: 0x0006E096
		private void UpdateSize()
		{
			base.Size = new Size(2 + this.currentControl.Width + 2, 2 + this.currentControl.Height + 2);
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x0006F0C4 File Offset: 0x0006E0C4
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 6 && base.Visible && NativeMethods.Util.LOWORD((int)m.WParam) == 0 && !this.OwnsWindow(m.LParam))
			{
				base.Visible = false;
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x0400127B RID: 4731
		private const int BORDER = 1;

		// Token: 0x0400127C RID: 4732
		private Control parent;

		// Token: 0x0400127D RID: 4733
		private Control currentControl;
	}
}
