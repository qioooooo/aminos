using System;
using System.Design;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Design
{
	internal partial class DropDownHolder : Form
	{
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

		public void DoModalLoop()
		{
			while (base.Visible)
			{
				Application.DoEvents();
				UnsafeNativeMethods.MsgWaitForMultipleObjectsEx(0, IntPtr.Zero, 250, 255, 4);
			}
		}

		public virtual Control Component
		{
			get
			{
				return this.currentControl;
			}
		}

		public virtual bool GetUsed()
		{
			return this.currentControl != null;
		}

		protected override void OnMouseDown(MouseEventArgs me)
		{
			if (me.Button == MouseButtons.Left)
			{
				base.Visible = false;
			}
			base.OnMouseDown(me);
		}

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

		public virtual void FocusComponent()
		{
			if (this.currentControl != null && base.Visible)
			{
				this.currentControl.Focus();
			}
		}

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

		private void UpdateSize()
		{
			base.Size = new Size(2 + this.currentControl.Width + 2, 2 + this.currentControl.Height + 2);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 6 && base.Visible && NativeMethods.Util.LOWORD((int)m.WParam) == 0 && !this.OwnsWindow(m.LParam))
			{
				base.Visible = false;
				return;
			}
			base.WndProc(ref m);
		}

		private const int BORDER = 1;

		private Control parent;

		private Control currentControl;
	}
}
