using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Drawing.Design
{
	// Token: 0x02000014 RID: 20
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class CursorEditor : UITypeEditor
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00004DAC File Offset: 0x00003DAC
		public override bool IsDropDownResizable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004DB0 File Offset: 0x00003DB0
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					if (this.cursorUI == null)
					{
						this.cursorUI = new CursorEditor.CursorUI(this);
					}
					this.cursorUI.Start(windowsFormsEditorService, value);
					windowsFormsEditorService.DropDownControl(this.cursorUI);
					value = this.cursorUI.Value;
					this.cursorUI.End();
				}
			}
			return value;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004E1F File Offset: 0x00003E1F
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		// Token: 0x04000077 RID: 119
		private CursorEditor.CursorUI cursorUI;

		// Token: 0x02000015 RID: 21
		private class CursorUI : ListBox
		{
			// Token: 0x06000088 RID: 136 RVA: 0x00004E2C File Offset: 0x00003E2C
			public CursorUI(UITypeEditor editor)
			{
				this.editor = editor;
				base.Height = 310;
				this.ItemHeight = Math.Max(4 + Cursors.Default.Size.Height, this.Font.Height);
				this.DrawMode = DrawMode.OwnerDrawFixed;
				base.BorderStyle = BorderStyle.None;
				this.cursorConverter = TypeDescriptor.GetConverter(typeof(Cursor));
				if (this.cursorConverter.GetStandardValuesSupported())
				{
					foreach (object obj in this.cursorConverter.GetStandardValues())
					{
						base.Items.Add(obj);
					}
				}
			}

			// Token: 0x1700001B RID: 27
			// (get) Token: 0x06000089 RID: 137 RVA: 0x00004F00 File Offset: 0x00003F00
			public object Value
			{
				get
				{
					return this.value;
				}
			}

			// Token: 0x0600008A RID: 138 RVA: 0x00004F08 File Offset: 0x00003F08
			public void End()
			{
				this.edSvc = null;
				this.value = null;
			}

			// Token: 0x0600008B RID: 139 RVA: 0x00004F18 File Offset: 0x00003F18
			protected override void OnClick(EventArgs e)
			{
				base.OnClick(e);
				this.value = base.SelectedItem;
				this.edSvc.CloseDropDown();
			}

			// Token: 0x0600008C RID: 140 RVA: 0x00004F38 File Offset: 0x00003F38
			protected override void OnDrawItem(DrawItemEventArgs die)
			{
				base.OnDrawItem(die);
				if (die.Index != -1)
				{
					Cursor cursor = (Cursor)base.Items[die.Index];
					string text = this.cursorConverter.ConvertToString(cursor);
					Font font = die.Font;
					Brush brush = new SolidBrush(die.ForeColor);
					die.DrawBackground();
					die.Graphics.FillRectangle(SystemBrushes.Control, new Rectangle(die.Bounds.X + 2, die.Bounds.Y + 2, 32, die.Bounds.Height - 4));
					die.Graphics.DrawRectangle(SystemPens.WindowText, new Rectangle(die.Bounds.X + 2, die.Bounds.Y + 2, 31, die.Bounds.Height - 4 - 1));
					cursor.DrawStretched(die.Graphics, new Rectangle(die.Bounds.X + 2, die.Bounds.Y + 2, 32, die.Bounds.Height - 4));
					die.Graphics.DrawString(text, font, brush, (float)(die.Bounds.X + 36), (float)(die.Bounds.Y + (die.Bounds.Height - font.Height) / 2));
					brush.Dispose();
				}
			}

			// Token: 0x0600008D RID: 141 RVA: 0x000050C1 File Offset: 0x000040C1
			protected override bool ProcessDialogKey(Keys keyData)
			{
				if ((keyData & Keys.KeyCode) == Keys.Return && (keyData & (Keys.Control | Keys.Alt)) == Keys.None)
				{
					this.OnClick(EventArgs.Empty);
					return true;
				}
				return base.ProcessDialogKey(keyData);
			}

			// Token: 0x0600008E RID: 142 RVA: 0x000050EC File Offset: 0x000040EC
			public void Start(IWindowsFormsEditorService edSvc, object value)
			{
				this.edSvc = edSvc;
				this.value = value;
				if (value != null)
				{
					for (int i = 0; i < base.Items.Count; i++)
					{
						if (base.Items[i] == value)
						{
							this.SelectedIndex = i;
							return;
						}
					}
				}
			}

			// Token: 0x04000078 RID: 120
			private object value;

			// Token: 0x04000079 RID: 121
			private IWindowsFormsEditorService edSvc;

			// Token: 0x0400007A RID: 122
			private TypeConverter cursorConverter;

			// Token: 0x0400007B RID: 123
			private UITypeEditor editor;
		}
	}
}
