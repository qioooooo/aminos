using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020002E6 RID: 742
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DesignTimeVisible(false)]
	[DefaultProperty("GridEditName")]
	[ToolboxItem(false)]
	public class DataGridTextBox : TextBox
	{
		// Token: 0x06002C4B RID: 11339 RVA: 0x00077988 File Offset: 0x00076988
		public DataGridTextBox()
		{
			base.TabStop = false;
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x0007799E File Offset: 0x0007699E
		public void SetDataGrid(DataGrid parentGrid)
		{
			this.dataGrid = parentGrid;
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x000779A8 File Offset: 0x000769A8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 770 || m.Msg == 768 || m.Msg == 771)
			{
				this.IsInEditOrNavigateMode = false;
				this.dataGrid.ColumnStartedEditing(base.Bounds);
			}
			base.WndProc(ref m);
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x000779FB File Offset: 0x000769FB
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			this.dataGrid.TextBoxOnMouseWheel(e);
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x00077A0C File Offset: 0x00076A0C
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			if (e.KeyChar == ' ' && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
			{
				return;
			}
			if (base.ReadOnly)
			{
				return;
			}
			if ((Control.ModifierKeys & Keys.Control) == Keys.Control && (Control.ModifierKeys & Keys.Alt) == Keys.None)
			{
				return;
			}
			this.IsInEditOrNavigateMode = false;
			this.dataGrid.ColumnStartedEditing(base.Bounds);
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x00077A80 File Offset: 0x00076A80
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected internal override bool ProcessKeyMessage(ref Message m)
		{
			Keys keys = (Keys)(long)m.WParam;
			Keys modifierKeys = Control.ModifierKeys;
			if ((keys | modifierKeys) == Keys.Return || (keys | modifierKeys) == Keys.Escape || (keys | modifierKeys) == (Keys.LButton | Keys.MButton | Keys.Back | Keys.Control))
			{
				return m.Msg == 258 || this.ProcessKeyPreview(ref m);
			}
			if (m.Msg == 258)
			{
				return keys == Keys.LineFeed || this.ProcessKeyEventArgs(ref m);
			}
			if (m.Msg == 257)
			{
				return true;
			}
			Keys keys2 = keys & Keys.KeyCode;
			Keys keys3 = keys2;
			if (keys3 <= Keys.A)
			{
				if (keys3 != Keys.Tab)
				{
					switch (keys3)
					{
					case Keys.Space:
						if (this.IsInEditOrNavigateMode && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
						{
							return m.Msg == 258 || this.ProcessKeyPreview(ref m);
						}
						return this.ProcessKeyEventArgs(ref m);
					case Keys.Prior:
					case Keys.Next:
						break;
					case Keys.End:
					case Keys.Home:
						if (this.SelectionLength == this.Text.Length)
						{
							return this.ProcessKeyPreview(ref m);
						}
						return this.ProcessKeyEventArgs(ref m);
					case Keys.Left:
						if (base.SelectionStart + this.SelectionLength == 0 || (this.IsInEditOrNavigateMode && this.SelectionLength == this.Text.Length))
						{
							return this.ProcessKeyPreview(ref m);
						}
						return this.ProcessKeyEventArgs(ref m);
					case Keys.Up:
						if (this.Text.IndexOf("\r\n") < 0 || base.SelectionStart + this.SelectionLength < this.Text.IndexOf("\r\n"))
						{
							return this.ProcessKeyPreview(ref m);
						}
						return this.ProcessKeyEventArgs(ref m);
					case Keys.Right:
						if (base.SelectionStart + this.SelectionLength == this.Text.Length)
						{
							return this.ProcessKeyPreview(ref m);
						}
						return this.ProcessKeyEventArgs(ref m);
					case Keys.Down:
					{
						int num = base.SelectionStart + this.SelectionLength;
						if (this.Text.IndexOf("\r\n", num) == -1)
						{
							return this.ProcessKeyPreview(ref m);
						}
						return this.ProcessKeyEventArgs(ref m);
					}
					case Keys.Select:
					case Keys.Print:
					case Keys.Execute:
					case Keys.Snapshot:
					case Keys.Insert:
						goto IL_0314;
					case Keys.Delete:
						if (!this.IsInEditOrNavigateMode)
						{
							return this.ProcessKeyEventArgs(ref m);
						}
						if (this.ProcessKeyPreview(ref m))
						{
							return true;
						}
						this.IsInEditOrNavigateMode = false;
						this.dataGrid.ColumnStartedEditing(base.Bounds);
						return this.ProcessKeyEventArgs(ref m);
					default:
						if (keys3 != Keys.A)
						{
							goto IL_0314;
						}
						if (this.IsInEditOrNavigateMode && (Control.ModifierKeys & Keys.Control) == Keys.Control)
						{
							return m.Msg == 258 || this.ProcessKeyPreview(ref m);
						}
						return this.ProcessKeyEventArgs(ref m);
					}
				}
				else
				{
					if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
					{
						return this.ProcessKeyPreview(ref m);
					}
					return this.ProcessKeyEventArgs(ref m);
				}
			}
			else
			{
				switch (keys3)
				{
				case Keys.Add:
				case Keys.Subtract:
					break;
				case Keys.Separator:
					goto IL_0314;
				default:
					if (keys3 == Keys.F2)
					{
						this.IsInEditOrNavigateMode = false;
						base.SelectionStart = this.Text.Length;
						return true;
					}
					switch (keys3)
					{
					case Keys.Oemplus:
					case Keys.OemMinus:
						break;
					case Keys.Oemcomma:
						goto IL_0314;
					default:
						goto IL_0314;
					}
					break;
				}
			}
			if (this.IsInEditOrNavigateMode)
			{
				return this.ProcessKeyPreview(ref m);
			}
			return this.ProcessKeyEventArgs(ref m);
			IL_0314:
			return this.ProcessKeyEventArgs(ref m);
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06002C51 RID: 11345 RVA: 0x00077DA8 File Offset: 0x00076DA8
		// (set) Token: 0x06002C52 RID: 11346 RVA: 0x00077DB0 File Offset: 0x00076DB0
		public bool IsInEditOrNavigateMode
		{
			get
			{
				return this.isInEditOrNavigateMode;
			}
			set
			{
				this.isInEditOrNavigateMode = value;
				if (value)
				{
					base.SelectAll();
				}
			}
		}

		// Token: 0x04001832 RID: 6194
		private bool isInEditOrNavigateMode = true;

		// Token: 0x04001833 RID: 6195
		private DataGrid dataGrid;
	}
}
