using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Drawing.Design
{
	// Token: 0x02000012 RID: 18
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class ContentAlignmentEditor : UITypeEditor
	{
		// Token: 0x06000074 RID: 116 RVA: 0x0000413C File Offset: 0x0000313C
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					if (this.contentUI == null)
					{
						this.contentUI = new ContentAlignmentEditor.ContentUI();
					}
					this.contentUI.Start(windowsFormsEditorService, value);
					windowsFormsEditorService.DropDownControl(this.contentUI);
					value = this.contentUI.Value;
					this.contentUI.End();
				}
			}
			return value;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000041AA File Offset: 0x000031AA
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		// Token: 0x0400006B RID: 107
		private ContentAlignmentEditor.ContentUI contentUI;

		// Token: 0x02000013 RID: 19
		private class ContentUI : Control
		{
			// Token: 0x06000077 RID: 119 RVA: 0x000041B8 File Offset: 0x000031B8
			public ContentUI()
			{
				this.InitComponent();
			}

			// Token: 0x17000016 RID: 22
			// (get) Token: 0x06000078 RID: 120 RVA: 0x00004234 File Offset: 0x00003234
			// (set) Token: 0x06000079 RID: 121 RVA: 0x000042CC File Offset: 0x000032CC
			private ContentAlignment Align
			{
				get
				{
					if (this.topLeft.Checked)
					{
						return ContentAlignment.TopLeft;
					}
					if (this.topCenter.Checked)
					{
						return ContentAlignment.TopCenter;
					}
					if (this.topRight.Checked)
					{
						return ContentAlignment.TopRight;
					}
					if (this.middleLeft.Checked)
					{
						return ContentAlignment.MiddleLeft;
					}
					if (this.middleCenter.Checked)
					{
						return ContentAlignment.MiddleCenter;
					}
					if (this.middleRight.Checked)
					{
						return ContentAlignment.MiddleRight;
					}
					if (this.bottomLeft.Checked)
					{
						return ContentAlignment.BottomLeft;
					}
					if (this.bottomCenter.Checked)
					{
						return ContentAlignment.BottomCenter;
					}
					return ContentAlignment.BottomRight;
				}
				set
				{
					if (value <= ContentAlignment.MiddleCenter)
					{
						switch (value)
						{
						case ContentAlignment.TopLeft:
							this.topLeft.Checked = true;
							return;
						case ContentAlignment.TopCenter:
							this.topCenter.Checked = true;
							return;
						case (ContentAlignment)3:
							break;
						case ContentAlignment.TopRight:
							this.topRight.Checked = true;
							return;
						default:
							if (value == ContentAlignment.MiddleLeft)
							{
								this.middleLeft.Checked = true;
								return;
							}
							if (value != ContentAlignment.MiddleCenter)
							{
								return;
							}
							this.middleCenter.Checked = true;
							return;
						}
					}
					else if (value <= ContentAlignment.BottomLeft)
					{
						if (value == ContentAlignment.MiddleRight)
						{
							this.middleRight.Checked = true;
							return;
						}
						if (value != ContentAlignment.BottomLeft)
						{
							return;
						}
						this.bottomLeft.Checked = true;
						return;
					}
					else
					{
						if (value == ContentAlignment.BottomCenter)
						{
							this.bottomCenter.Checked = true;
							return;
						}
						if (value != ContentAlignment.BottomRight)
						{
							return;
						}
						this.bottomRight.Checked = true;
					}
				}
			}

			// Token: 0x17000017 RID: 23
			// (get) Token: 0x0600007A RID: 122 RVA: 0x0000439E File Offset: 0x0000339E
			protected override bool ShowFocusCues
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000018 RID: 24
			// (get) Token: 0x0600007B RID: 123 RVA: 0x000043A1 File Offset: 0x000033A1
			public object Value
			{
				get
				{
					return this.value;
				}
			}

			// Token: 0x0600007C RID: 124 RVA: 0x000043A9 File Offset: 0x000033A9
			public void End()
			{
				this.edSvc = null;
				this.value = null;
			}

			// Token: 0x0600007D RID: 125 RVA: 0x000043BC File Offset: 0x000033BC
			private void InitComponent()
			{
				base.Size = new Size(125, 89);
				this.BackColor = SystemColors.Control;
				this.ForeColor = SystemColors.ControlText;
				base.AccessibleName = SR.GetString("ContentAlignmentEditorAccName");
				this.topLeft.Size = new Size(24, 25);
				this.topLeft.TabIndex = 8;
				this.topLeft.Text = "";
				this.topLeft.Appearance = Appearance.Button;
				this.topLeft.Click += this.OptionClick;
				this.topLeft.AccessibleName = SR.GetString("ContentAlignmentEditorTopLeftAccName");
				this.topCenter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this.topCenter.Location = new Point(32, 0);
				this.topCenter.Size = new Size(59, 25);
				this.topCenter.TabIndex = 0;
				this.topCenter.Text = "";
				this.topCenter.Appearance = Appearance.Button;
				this.topCenter.Click += this.OptionClick;
				this.topCenter.AccessibleName = SR.GetString("ContentAlignmentEditorTopCenterAccName");
				this.topRight.Anchor = AnchorStyles.Top | AnchorStyles.Right;
				this.topRight.Location = new Point(99, 0);
				this.topRight.Size = new Size(24, 25);
				this.topRight.TabIndex = 1;
				this.topRight.Text = "";
				this.topRight.Appearance = Appearance.Button;
				this.topRight.Click += this.OptionClick;
				this.topRight.AccessibleName = SR.GetString("ContentAlignmentEditorTopRightAccName");
				this.middleLeft.Location = new Point(0, 32);
				this.middleLeft.Size = new Size(24, 25);
				this.middleLeft.TabIndex = 2;
				this.middleLeft.Text = "";
				this.middleLeft.Appearance = Appearance.Button;
				this.middleLeft.Click += this.OptionClick;
				this.middleLeft.AccessibleName = SR.GetString("ContentAlignmentEditorMiddleLeftAccName");
				this.middleCenter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this.middleCenter.Location = new Point(32, 32);
				this.middleCenter.Size = new Size(59, 25);
				this.middleCenter.TabIndex = 3;
				this.middleCenter.Text = "";
				this.middleCenter.Appearance = Appearance.Button;
				this.middleCenter.Click += this.OptionClick;
				this.middleCenter.AccessibleName = SR.GetString("ContentAlignmentEditorMiddleCenterAccName");
				this.middleRight.Anchor = AnchorStyles.Top | AnchorStyles.Right;
				this.middleRight.Location = new Point(99, 32);
				this.middleRight.Size = new Size(24, 25);
				this.middleRight.TabIndex = 4;
				this.middleRight.Text = "";
				this.middleRight.Appearance = Appearance.Button;
				this.middleRight.Click += this.OptionClick;
				this.middleRight.AccessibleName = SR.GetString("ContentAlignmentEditorMiddleRightAccName");
				this.bottomLeft.Location = new Point(0, 64);
				this.bottomLeft.Size = new Size(24, 25);
				this.bottomLeft.TabIndex = 5;
				this.bottomLeft.Text = "";
				this.bottomLeft.Appearance = Appearance.Button;
				this.bottomLeft.Click += this.OptionClick;
				this.bottomLeft.AccessibleName = SR.GetString("ContentAlignmentEditorBottomLeftAccName");
				this.bottomCenter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this.bottomCenter.Location = new Point(32, 64);
				this.bottomCenter.Size = new Size(59, 25);
				this.bottomCenter.TabIndex = 6;
				this.bottomCenter.Text = "";
				this.bottomCenter.Appearance = Appearance.Button;
				this.bottomCenter.Click += this.OptionClick;
				this.bottomCenter.AccessibleName = SR.GetString("ContentAlignmentEditorBottomCenterAccName");
				this.bottomRight.Anchor = AnchorStyles.Top | AnchorStyles.Right;
				this.bottomRight.Location = new Point(99, 64);
				this.bottomRight.Size = new Size(24, 25);
				this.bottomRight.TabIndex = 7;
				this.bottomRight.Text = "";
				this.bottomRight.Appearance = Appearance.Button;
				this.bottomRight.Click += this.OptionClick;
				this.bottomRight.AccessibleName = SR.GetString("ContentAlignmentEditorBottomRightAccName");
				base.Controls.Clear();
				base.Controls.AddRange(new Control[] { this.bottomRight, this.bottomCenter, this.bottomLeft, this.middleRight, this.middleCenter, this.middleLeft, this.topRight, this.topCenter, this.topLeft });
			}

			// Token: 0x0600007E RID: 126 RVA: 0x00004900 File Offset: 0x00003900
			protected override bool IsInputKey(Keys keyData)
			{
				switch (keyData)
				{
				case Keys.Left:
				case Keys.Up:
				case Keys.Right:
				case Keys.Down:
					return false;
				default:
					return base.IsInputKey(keyData);
				}
			}

			// Token: 0x0600007F RID: 127 RVA: 0x00004933 File Offset: 0x00003933
			private void OptionClick(object sender, EventArgs e)
			{
				this.value = this.Align;
				this.edSvc.CloseDropDown();
			}

			// Token: 0x06000080 RID: 128 RVA: 0x00004954 File Offset: 0x00003954
			public void Start(IWindowsFormsEditorService edSvc, object value)
			{
				this.edSvc = edSvc;
				this.value = value;
				ContentAlignment contentAlignment;
				if (value == null)
				{
					contentAlignment = ContentAlignment.MiddleLeft;
				}
				else
				{
					contentAlignment = (ContentAlignment)value;
				}
				this.Align = contentAlignment;
			}

			// Token: 0x06000081 RID: 129 RVA: 0x00004988 File Offset: 0x00003988
			protected override bool ProcessDialogKey(Keys keyData)
			{
				RadioButton checkedControl = this.CheckedControl;
				if ((keyData & Keys.KeyCode) == Keys.Left)
				{
					if (checkedControl == this.bottomRight)
					{
						this.CheckedControl = this.bottomCenter;
					}
					else if (checkedControl == this.middleRight)
					{
						this.CheckedControl = this.middleCenter;
					}
					else if (checkedControl == this.topRight)
					{
						this.CheckedControl = this.topCenter;
					}
					else if (checkedControl == this.bottomCenter)
					{
						this.CheckedControl = this.bottomLeft;
					}
					else if (checkedControl == this.middleCenter)
					{
						this.CheckedControl = this.middleLeft;
					}
					else if (checkedControl == this.topCenter)
					{
						this.CheckedControl = this.topLeft;
					}
					return true;
				}
				if ((keyData & Keys.KeyCode) == Keys.Right)
				{
					if (checkedControl == this.bottomLeft)
					{
						this.CheckedControl = this.bottomCenter;
					}
					else if (checkedControl == this.middleLeft)
					{
						this.CheckedControl = this.middleCenter;
					}
					else if (checkedControl == this.topLeft)
					{
						this.CheckedControl = this.topCenter;
					}
					else if (checkedControl == this.bottomCenter)
					{
						this.CheckedControl = this.bottomRight;
					}
					else if (checkedControl == this.middleCenter)
					{
						this.CheckedControl = this.middleRight;
					}
					else if (checkedControl == this.topCenter)
					{
						this.CheckedControl = this.topRight;
					}
					return true;
				}
				if ((keyData & Keys.KeyCode) == Keys.Up)
				{
					if (checkedControl == this.bottomRight)
					{
						this.CheckedControl = this.middleRight;
					}
					else if (checkedControl == this.middleRight)
					{
						this.CheckedControl = this.topRight;
					}
					else if (checkedControl == this.bottomCenter)
					{
						this.CheckedControl = this.middleCenter;
					}
					else if (checkedControl == this.middleCenter)
					{
						this.CheckedControl = this.topCenter;
					}
					else if (checkedControl == this.bottomLeft)
					{
						this.CheckedControl = this.middleLeft;
					}
					else if (checkedControl == this.middleLeft)
					{
						this.CheckedControl = this.topLeft;
					}
					return true;
				}
				if ((keyData & Keys.KeyCode) == Keys.Down)
				{
					if (checkedControl == this.topRight)
					{
						this.CheckedControl = this.middleRight;
					}
					else if (checkedControl == this.middleRight)
					{
						this.CheckedControl = this.bottomRight;
					}
					else if (checkedControl == this.topCenter)
					{
						this.CheckedControl = this.middleCenter;
					}
					else if (checkedControl == this.middleCenter)
					{
						this.CheckedControl = this.bottomCenter;
					}
					else if (checkedControl == this.topLeft)
					{
						this.CheckedControl = this.middleLeft;
					}
					else if (checkedControl == this.middleLeft)
					{
						this.CheckedControl = this.bottomLeft;
					}
					return true;
				}
				if ((keyData & Keys.KeyCode) == Keys.Space)
				{
					this.OptionClick(this, EventArgs.Empty);
					return true;
				}
				if ((keyData & Keys.KeyCode) == Keys.Return && (keyData & (Keys.Control | Keys.Alt)) == Keys.None)
				{
					this.OptionClick(this, EventArgs.Empty);
					return true;
				}
				if ((keyData & Keys.KeyCode) == Keys.Escape && (keyData & (Keys.Control | Keys.Alt)) == Keys.None)
				{
					this.edSvc.CloseDropDown();
					return true;
				}
				if ((keyData & Keys.KeyCode) == Keys.Tab && (keyData & (Keys.Control | Keys.Alt)) == Keys.None)
				{
					int num = this.CheckedControl.TabIndex + (((keyData & Keys.Shift) == Keys.None) ? 1 : (-1));
					if (num < 0)
					{
						num = base.Controls.Count - 1;
					}
					else if (num >= base.Controls.Count)
					{
						num = 0;
					}
					for (int i = 0; i < base.Controls.Count; i++)
					{
						if (base.Controls[i] is RadioButton && base.Controls[i].TabIndex == num)
						{
							this.CheckedControl = (RadioButton)base.Controls[i];
							return true;
						}
					}
					return true;
				}
				return base.ProcessDialogKey(keyData);
			}

			// Token: 0x17000019 RID: 25
			// (get) Token: 0x06000082 RID: 130 RVA: 0x00004D10 File Offset: 0x00003D10
			// (set) Token: 0x06000083 RID: 131 RVA: 0x00004D76 File Offset: 0x00003D76
			private RadioButton CheckedControl
			{
				get
				{
					for (int i = 0; i < base.Controls.Count; i++)
					{
						if (base.Controls[i] is RadioButton && ((RadioButton)base.Controls[i]).Checked)
						{
							return (RadioButton)base.Controls[i];
						}
					}
					return this.middleLeft;
				}
				set
				{
					this.CheckedControl.Checked = false;
					value.Checked = true;
					if (value.IsHandleCreated)
					{
						UnsafeNativeMethods.NotifyWinEvent(32773, new HandleRef(value, value.Handle), -4, 0);
					}
				}
			}

			// Token: 0x0400006C RID: 108
			private IWindowsFormsEditorService edSvc;

			// Token: 0x0400006D RID: 109
			private object value;

			// Token: 0x0400006E RID: 110
			private RadioButton topLeft = new RadioButton();

			// Token: 0x0400006F RID: 111
			private RadioButton topCenter = new RadioButton();

			// Token: 0x04000070 RID: 112
			private RadioButton topRight = new RadioButton();

			// Token: 0x04000071 RID: 113
			private RadioButton middleLeft = new RadioButton();

			// Token: 0x04000072 RID: 114
			private RadioButton middleCenter = new RadioButton();

			// Token: 0x04000073 RID: 115
			private RadioButton middleRight = new RadioButton();

			// Token: 0x04000074 RID: 116
			private RadioButton bottomLeft = new RadioButton();

			// Token: 0x04000075 RID: 117
			private RadioButton bottomCenter = new RadioButton();

			// Token: 0x04000076 RID: 118
			private RadioButton bottomRight = new RadioButton();
		}
	}
}
