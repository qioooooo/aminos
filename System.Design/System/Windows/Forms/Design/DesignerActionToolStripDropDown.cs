using System;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal class DesignerActionToolStripDropDown : ToolStripDropDown
	{
		public DesignerActionToolStripDropDown(DesignerActionUI designerActionUI, IWin32Window mainParentWindow)
		{
			this._mainParentWindow = mainParentWindow;
			this._designerActionUI = designerActionUI;
		}

		public DesignerActionPanel CurrentPanel
		{
			get
			{
				if (this._panel != null)
				{
					return this._panel.Control as DesignerActionPanel;
				}
				return null;
			}
		}

		protected override bool TopMost
		{
			get
			{
				return false;
			}
		}

		public void UpdateContainerSize()
		{
			if (this.CurrentPanel != null)
			{
				Size preferredSize = this.CurrentPanel.GetPreferredSize(new Size(150, int.MaxValue));
				if (this.CurrentPanel.Size == preferredSize)
				{
					this.CurrentPanel.PerformLayout();
				}
				else
				{
					this.CurrentPanel.Size = preferredSize;
				}
				base.ClientSize = preferredSize;
			}
		}

		public void CheckFocusIsRight()
		{
			IntPtr intPtr = UnsafeNativeMethods.GetFocus();
			if (intPtr == base.Handle)
			{
				this._panel.Focus();
			}
			intPtr = UnsafeNativeMethods.GetFocus();
			if (this.CurrentPanel != null && this.CurrentPanel.Handle == intPtr)
			{
				this.CurrentPanel.SelectNextControl(null, true, true, true, true);
			}
			intPtr = UnsafeNativeMethods.GetFocus();
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout(levent);
			this.UpdateContainerSize();
		}

		protected override void OnClosing(ToolStripDropDownClosingEventArgs e)
		{
			if (e.CloseReason == ToolStripDropDownCloseReason.AppFocusChange && this._cancelClose)
			{
				this._cancelClose = false;
				e.Cancel = true;
			}
			else if (e.CloseReason == ToolStripDropDownCloseReason.AppFocusChange || e.CloseReason == ToolStripDropDownCloseReason.AppClicked)
			{
				IntPtr activeWindow = UnsafeNativeMethods.GetActiveWindow();
				if (base.Handle == activeWindow && e.CloseReason == ToolStripDropDownCloseReason.AppClicked)
				{
					e.Cancel = false;
				}
				else if (DesignerActionToolStripDropDown.WindowOwnsWindow(base.Handle, activeWindow))
				{
					e.Cancel = true;
				}
				else if (this._mainParentWindow != null && !DesignerActionToolStripDropDown.WindowOwnsWindow(this._mainParentWindow.Handle, activeWindow))
				{
					if (this.IsWindowEnabled(this._mainParentWindow.Handle))
					{
						e.Cancel = false;
					}
					else
					{
						e.Cancel = true;
					}
					base.OnClosing(e);
					return;
				}
				IntPtr windowLong = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, activeWindow), -8);
				if (!this.IsWindowEnabled(windowLong))
				{
					e.Cancel = true;
				}
			}
			base.OnClosing(e);
		}

		public void SetDesignerActionPanel(DesignerActionPanel panel, Glyph relatedGlyph)
		{
			if (this._panel != null && panel == (DesignerActionPanel)this._panel.Control)
			{
				return;
			}
			this.relatedGlyph = relatedGlyph;
			panel.SizeChanged += this.PanelResized;
			if (this._panel != null)
			{
				this.Items.Remove(this._panel);
				this._panel.Dispose();
				this._panel = null;
			}
			this._panel = new ToolStripControlHost(panel);
			this._panel.Margin = Padding.Empty;
			this._panel.Size = panel.Size;
			base.SuspendLayout();
			base.Size = panel.Size;
			this.Items.Add(this._panel);
			base.ResumeLayout();
			if (base.Visible)
			{
				this.CheckFocusIsRight();
			}
		}

		private void PanelResized(object sender, EventArgs e)
		{
			Control control = sender as Control;
			if (base.Size.Width != control.Size.Width || base.Size.Height != control.Size.Height)
			{
				base.SuspendLayout();
				base.Size = control.Size;
				if (this._panel != null)
				{
					this._panel.Size = control.Size;
				}
				this._designerActionUI.UpdateDAPLocation(null, this.relatedGlyph as DesignerActionGlyph);
				base.ResumeLayout();
			}
		}

		protected override void SetVisibleCore(bool visible)
		{
			base.SetVisibleCore(visible);
			if (visible)
			{
				this.CheckFocusIsRight();
			}
		}

		private static bool WindowOwnsWindow(IntPtr hWndOwner, IntPtr hWndDescendant)
		{
			if (hWndDescendant == hWndOwner)
			{
				return true;
			}
			while (hWndDescendant != IntPtr.Zero)
			{
				hWndDescendant = UnsafeNativeMethods.GetWindowLong(new HandleRef(null, hWndDescendant), -8);
				if (hWndDescendant == IntPtr.Zero)
				{
					return false;
				}
				if (hWndDescendant == hWndOwner)
				{
					return true;
				}
			}
			return false;
		}

		internal static string GetControlInformation(IntPtr hwnd)
		{
			if (hwnd == IntPtr.Zero)
			{
				return "Handle is IntPtr.Zero";
			}
			return string.Empty;
		}

		private bool IsWindowEnabled(IntPtr handle)
		{
			int num = (int)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, handle), -16);
			return (num & 134217728) == 0;
		}

		private void WmActivate(ref Message m)
		{
			if ((int)m.WParam == 0)
			{
				IntPtr lparam = m.LParam;
				if (DesignerActionToolStripDropDown.WindowOwnsWindow(base.Handle, lparam))
				{
					this._cancelClose = true;
				}
				else
				{
					this._cancelClose = false;
				}
			}
			else
			{
				this._cancelClose = false;
			}
			base.WndProc(ref m);
		}

		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 6)
			{
				this.WmActivate(ref m);
				return;
			}
			base.WndProc(ref m);
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Return)
			{
				IntPtr focus = UnsafeNativeMethods.GetFocus();
				Control control = Control.FromChildHandle(focus);
				IButtonControl buttonControl = control as IButtonControl;
				if (buttonControl != null && buttonControl is Control)
				{
					buttonControl.PerformClick();
					return true;
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		private IWin32Window _mainParentWindow;

		private ToolStripControlHost _panel;

		private DesignerActionUI _designerActionUI;

		private bool _cancelClose;

		private Glyph relatedGlyph;
	}
}
