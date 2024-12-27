using System;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000213 RID: 531
	internal class DesignerActionToolStripDropDown : ToolStripDropDown
	{
		// Token: 0x06001404 RID: 5124 RVA: 0x000661BC File Offset: 0x000651BC
		public DesignerActionToolStripDropDown(DesignerActionUI designerActionUI, IWin32Window mainParentWindow)
		{
			this._mainParentWindow = mainParentWindow;
			this._designerActionUI = designerActionUI;
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06001405 RID: 5125 RVA: 0x000661D2 File Offset: 0x000651D2
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

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06001406 RID: 5126 RVA: 0x000661EE File Offset: 0x000651EE
		protected override bool TopMost
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x000661F4 File Offset: 0x000651F4
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

		// Token: 0x06001408 RID: 5128 RVA: 0x00066258 File Offset: 0x00065258
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

		// Token: 0x06001409 RID: 5129 RVA: 0x000662BC File Offset: 0x000652BC
		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout(levent);
			this.UpdateContainerSize();
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x000662CC File Offset: 0x000652CC
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

		// Token: 0x0600140B RID: 5131 RVA: 0x000663BC File Offset: 0x000653BC
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

		// Token: 0x0600140C RID: 5132 RVA: 0x00066490 File Offset: 0x00065490
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

		// Token: 0x0600140D RID: 5133 RVA: 0x0006652A File Offset: 0x0006552A
		protected override void SetVisibleCore(bool visible)
		{
			base.SetVisibleCore(visible);
			if (visible)
			{
				this.CheckFocusIsRight();
			}
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0006653C File Offset: 0x0006553C
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

		// Token: 0x0600140F RID: 5135 RVA: 0x0006658C File Offset: 0x0006558C
		internal static string GetControlInformation(IntPtr hwnd)
		{
			if (hwnd == IntPtr.Zero)
			{
				return "Handle is IntPtr.Zero";
			}
			return string.Empty;
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x000665A8 File Offset: 0x000655A8
		private bool IsWindowEnabled(IntPtr handle)
		{
			int num = (int)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, handle), -16);
			return (num & 134217728) == 0;
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x000665D4 File Offset: 0x000655D4
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

		// Token: 0x06001412 RID: 5138 RVA: 0x00066624 File Offset: 0x00065624
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

		// Token: 0x06001413 RID: 5139 RVA: 0x0006664C File Offset: 0x0006564C
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

		// Token: 0x040011E8 RID: 4584
		private IWin32Window _mainParentWindow;

		// Token: 0x040011E9 RID: 4585
		private ToolStripControlHost _panel;

		// Token: 0x040011EA RID: 4586
		private DesignerActionUI _designerActionUI;

		// Token: 0x040011EB RID: 4587
		private bool _cancelClose;

		// Token: 0x040011EC RID: 4588
		private Glyph relatedGlyph;
	}
}
