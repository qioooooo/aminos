using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Security.Permissions;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001A6 RID: 422
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class BorderSidesEditor : UITypeEditor
	{
		// Token: 0x06001031 RID: 4145 RVA: 0x000498C8 File Offset: 0x000488C8
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					if (this.borderSidesEditorUI == null)
					{
						this.borderSidesEditorUI = new BorderSidesEditor.BorderSidesEditorUI(this);
					}
					this.borderSidesEditorUI.Start(windowsFormsEditorService, value);
					windowsFormsEditorService.DropDownControl(this.borderSidesEditorUI);
					if (this.borderSidesEditorUI.Value != null)
					{
						value = this.borderSidesEditorUI.Value;
					}
					this.borderSidesEditorUI.End();
				}
			}
			return value;
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x00049944 File Offset: 0x00048944
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		// Token: 0x04001044 RID: 4164
		private BorderSidesEditor.BorderSidesEditorUI borderSidesEditorUI;

		// Token: 0x020001A7 RID: 423
		private class BorderSidesEditorUI : UserControl
		{
			// Token: 0x06001034 RID: 4148 RVA: 0x0004994F File Offset: 0x0004894F
			public BorderSidesEditorUI(BorderSidesEditor editor)
			{
				this.editor = editor;
				this.End();
				this.InitializeComponent();
				base.Size = base.PreferredSize;
			}

			// Token: 0x170002A6 RID: 678
			// (get) Token: 0x06001035 RID: 4149 RVA: 0x00049976 File Offset: 0x00048976
			public IWindowsFormsEditorService EditorService
			{
				get
				{
					return this.edSvc;
				}
			}

			// Token: 0x170002A7 RID: 679
			// (get) Token: 0x06001036 RID: 4150 RVA: 0x0004997E File Offset: 0x0004897E
			public object Value
			{
				get
				{
					return this.currentValue;
				}
			}

			// Token: 0x06001037 RID: 4151 RVA: 0x00049986 File Offset: 0x00048986
			public void End()
			{
				this.edSvc = null;
				this.originalValue = null;
				this.currentValue = null;
				this.updateCurrentValue = false;
			}

			// Token: 0x06001038 RID: 4152 RVA: 0x000499A4 File Offset: 0x000489A4
			protected override void OnGotFocus(EventArgs e)
			{
				base.OnGotFocus(e);
				this.noneCheckBox.Focus();
			}

			// Token: 0x06001039 RID: 4153 RVA: 0x000499BC File Offset: 0x000489BC
			private void InitializeComponent()
			{
				ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(BorderSidesEditor));
				this.tableLayoutPanel1 = new TableLayoutPanel();
				this.noneCheckBox = new CheckBox();
				this.allCheckBox = new CheckBox();
				this.topCheckBox = new CheckBox();
				this.bottomCheckBox = new CheckBox();
				this.rightCheckBox = new CheckBox();
				this.leftCheckBox = new CheckBox();
				this.splitterLabel = new Label();
				this.tableLayoutPanel1.SuspendLayout();
				base.SuspendLayout();
				componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
				this.tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				this.tableLayoutPanel1.BackColor = SystemColors.Window;
				this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
				this.tableLayoutPanel1.Controls.Add(this.noneCheckBox, 0, 0);
				this.tableLayoutPanel1.Controls.Add(this.allCheckBox, 0, 2);
				this.tableLayoutPanel1.Controls.Add(this.topCheckBox, 0, 3);
				this.tableLayoutPanel1.Controls.Add(this.bottomCheckBox, 0, 4);
				this.tableLayoutPanel1.Controls.Add(this.rightCheckBox, 0, 6);
				this.tableLayoutPanel1.Controls.Add(this.leftCheckBox, 0, 5);
				this.tableLayoutPanel1.Controls.Add(this.splitterLabel, 0, 1);
				this.tableLayoutPanel1.Name = "tableLayoutPanel1";
				this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
				this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
				this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
				this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
				this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
				this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
				this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
				this.tableLayoutPanel1.Margin = new Padding(0);
				componentResourceManager.ApplyResources(this.noneCheckBox, "noneCheckBox");
				this.noneCheckBox.Name = "noneCheckBox";
				this.noneCheckBox.Margin = new Padding(3, 3, 3, 1);
				componentResourceManager.ApplyResources(this.allCheckBox, "allCheckBox");
				this.allCheckBox.Name = "allCheckBox";
				this.allCheckBox.Margin = new Padding(3, 3, 3, 1);
				componentResourceManager.ApplyResources(this.topCheckBox, "topCheckBox");
				this.topCheckBox.Margin = new Padding(20, 1, 3, 1);
				this.topCheckBox.Name = "topCheckBox";
				componentResourceManager.ApplyResources(this.bottomCheckBox, "bottomCheckBox");
				this.bottomCheckBox.Margin = new Padding(20, 1, 3, 1);
				this.bottomCheckBox.Name = "bottomCheckBox";
				componentResourceManager.ApplyResources(this.rightCheckBox, "rightCheckBox");
				this.rightCheckBox.Margin = new Padding(20, 1, 3, 1);
				this.rightCheckBox.Name = "rightCheckBox";
				componentResourceManager.ApplyResources(this.leftCheckBox, "leftCheckBox");
				this.leftCheckBox.Margin = new Padding(20, 1, 3, 1);
				this.leftCheckBox.Name = "leftCheckBox";
				componentResourceManager.ApplyResources(this.splitterLabel, "splitterLabel");
				this.splitterLabel.BackColor = SystemColors.ControlDark;
				this.splitterLabel.Name = "splitterLabel";
				componentResourceManager.ApplyResources(this, "$this");
				base.Controls.Add(this.tableLayoutPanel1);
				base.Padding = new Padding(1, 1, 1, 1);
				base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				base.AutoScaleMode = AutoScaleMode.Font;
				base.AutoScaleDimensions = new SizeF(6f, 13f);
				this.tableLayoutPanel1.ResumeLayout(false);
				this.tableLayoutPanel1.PerformLayout();
				base.ResumeLayout(false);
				base.PerformLayout();
				this.rightCheckBox.CheckedChanged += this.rightCheckBox_CheckedChanged;
				this.leftCheckBox.CheckedChanged += this.leftCheckBox_CheckedChanged;
				this.bottomCheckBox.CheckedChanged += this.bottomCheckBox_CheckedChanged;
				this.topCheckBox.CheckedChanged += this.topCheckBox_CheckedChanged;
				this.noneCheckBox.CheckedChanged += this.noneCheckBox_CheckedChanged;
				this.allCheckBox.CheckedChanged += this.allCheckBox_CheckedChanged;
				this.noneCheckBox.Click += this.noneCheckBoxClicked;
				this.allCheckBox.Click += this.allCheckBoxClicked;
			}

			// Token: 0x0600103A RID: 4154 RVA: 0x00049E88 File Offset: 0x00048E88
			private void rightCheckBox_CheckedChanged(object sender, EventArgs e)
			{
				CheckBox checkBox = sender as CheckBox;
				if (checkBox.Checked)
				{
					this.noneCheckBox.Checked = false;
				}
				else if (this.allCheckBox.Checked)
				{
					this.allCheckBox.Checked = false;
				}
				this.UpdateCurrentValue();
			}

			// Token: 0x0600103B RID: 4155 RVA: 0x00049ED4 File Offset: 0x00048ED4
			private void leftCheckBox_CheckedChanged(object sender, EventArgs e)
			{
				CheckBox checkBox = sender as CheckBox;
				if (checkBox.Checked)
				{
					this.noneCheckBox.Checked = false;
				}
				else if (this.allCheckBox.Checked)
				{
					this.allCheckBox.Checked = false;
				}
				this.UpdateCurrentValue();
			}

			// Token: 0x0600103C RID: 4156 RVA: 0x00049F20 File Offset: 0x00048F20
			private void bottomCheckBox_CheckedChanged(object sender, EventArgs e)
			{
				CheckBox checkBox = sender as CheckBox;
				if (checkBox.Checked)
				{
					this.noneCheckBox.Checked = false;
				}
				else if (this.allCheckBox.Checked)
				{
					this.allCheckBox.Checked = false;
				}
				this.UpdateCurrentValue();
			}

			// Token: 0x0600103D RID: 4157 RVA: 0x00049F6C File Offset: 0x00048F6C
			private void topCheckBox_CheckedChanged(object sender, EventArgs e)
			{
				CheckBox checkBox = sender as CheckBox;
				if (checkBox.Checked)
				{
					this.noneCheckBox.Checked = false;
				}
				else if (this.allCheckBox.Checked)
				{
					this.allCheckBox.Checked = false;
				}
				this.UpdateCurrentValue();
			}

			// Token: 0x0600103E RID: 4158 RVA: 0x00049FB8 File Offset: 0x00048FB8
			private void noneCheckBox_CheckedChanged(object sender, EventArgs e)
			{
				CheckBox checkBox = sender as CheckBox;
				if (checkBox.Checked)
				{
					this.allCheckBox.Checked = false;
					this.topCheckBox.Checked = false;
					this.bottomCheckBox.Checked = false;
					this.leftCheckBox.Checked = false;
					this.rightCheckBox.Checked = false;
				}
				this.UpdateCurrentValue();
			}

			// Token: 0x0600103F RID: 4159 RVA: 0x0004A018 File Offset: 0x00049018
			private void allCheckBox_CheckedChanged(object sender, EventArgs e)
			{
				CheckBox checkBox = sender as CheckBox;
				if (checkBox.Checked)
				{
					this.noneCheckBox.Checked = false;
					this.topCheckBox.Checked = true;
					this.bottomCheckBox.Checked = true;
					this.leftCheckBox.Checked = true;
					this.rightCheckBox.Checked = true;
				}
				this.UpdateCurrentValue();
			}

			// Token: 0x06001040 RID: 4160 RVA: 0x0004A076 File Offset: 0x00049076
			private void noneCheckBoxClicked(object sender, EventArgs e)
			{
				if (this.noneChecked)
				{
					this.noneCheckBox.Checked = true;
				}
			}

			// Token: 0x06001041 RID: 4161 RVA: 0x0004A08C File Offset: 0x0004908C
			private void allCheckBoxClicked(object sender, EventArgs e)
			{
				if (this.allChecked)
				{
					this.allCheckBox.Checked = true;
				}
			}

			// Token: 0x06001042 RID: 4162 RVA: 0x0004A0A4 File Offset: 0x000490A4
			private void ResetCheckBoxState()
			{
				this.allCheckBox.Checked = false;
				this.noneCheckBox.Checked = false;
				this.topCheckBox.Checked = false;
				this.bottomCheckBox.Checked = false;
				this.leftCheckBox.Checked = false;
				this.rightCheckBox.Checked = false;
			}

			// Token: 0x06001043 RID: 4163 RVA: 0x0004A0FC File Offset: 0x000490FC
			private void SetCheckBoxCheckState(ToolStripStatusLabelBorderSides sides)
			{
				this.ResetCheckBoxState();
				if ((sides & ToolStripStatusLabelBorderSides.All) == ToolStripStatusLabelBorderSides.All)
				{
					this.allCheckBox.Checked = true;
					this.topCheckBox.Checked = true;
					this.bottomCheckBox.Checked = true;
					this.leftCheckBox.Checked = true;
					this.rightCheckBox.Checked = true;
					this.allCheckBox.Checked = true;
					return;
				}
				this.noneCheckBox.Checked = true;
				this.topCheckBox.Checked = (sides & ToolStripStatusLabelBorderSides.Top) == ToolStripStatusLabelBorderSides.Top;
				this.bottomCheckBox.Checked = (sides & ToolStripStatusLabelBorderSides.Bottom) == ToolStripStatusLabelBorderSides.Bottom;
				this.leftCheckBox.Checked = (sides & ToolStripStatusLabelBorderSides.Left) == ToolStripStatusLabelBorderSides.Left;
				this.rightCheckBox.Checked = (sides & ToolStripStatusLabelBorderSides.Right) == ToolStripStatusLabelBorderSides.Right;
			}

			// Token: 0x06001044 RID: 4164 RVA: 0x0004A1B0 File Offset: 0x000491B0
			public void Start(IWindowsFormsEditorService edSvc, object value)
			{
				this.edSvc = edSvc;
				this.currentValue = value;
				this.originalValue = value;
				ToolStripStatusLabelBorderSides toolStripStatusLabelBorderSides = (ToolStripStatusLabelBorderSides)value;
				this.SetCheckBoxCheckState(toolStripStatusLabelBorderSides);
				this.updateCurrentValue = true;
			}

			// Token: 0x06001045 RID: 4165 RVA: 0x0004A1EC File Offset: 0x000491EC
			private void UpdateCurrentValue()
			{
				if (!this.updateCurrentValue)
				{
					return;
				}
				ToolStripStatusLabelBorderSides toolStripStatusLabelBorderSides = ToolStripStatusLabelBorderSides.None;
				if (this.allCheckBox.Checked)
				{
					toolStripStatusLabelBorderSides |= ToolStripStatusLabelBorderSides.All;
					this.currentValue = toolStripStatusLabelBorderSides;
					this.allChecked = true;
					this.noneChecked = false;
					return;
				}
				if (this.noneCheckBox.Checked)
				{
					toolStripStatusLabelBorderSides = toolStripStatusLabelBorderSides;
				}
				if (this.topCheckBox.Checked)
				{
					toolStripStatusLabelBorderSides |= ToolStripStatusLabelBorderSides.Top;
				}
				if (this.bottomCheckBox.Checked)
				{
					toolStripStatusLabelBorderSides |= ToolStripStatusLabelBorderSides.Bottom;
				}
				if (this.leftCheckBox.Checked)
				{
					toolStripStatusLabelBorderSides |= ToolStripStatusLabelBorderSides.Left;
				}
				if (this.rightCheckBox.Checked)
				{
					toolStripStatusLabelBorderSides |= ToolStripStatusLabelBorderSides.Right;
				}
				if (toolStripStatusLabelBorderSides == ToolStripStatusLabelBorderSides.None)
				{
					this.allChecked = false;
					this.noneChecked = true;
					this.noneCheckBox.Checked = true;
				}
				if (toolStripStatusLabelBorderSides == ToolStripStatusLabelBorderSides.All)
				{
					this.allChecked = true;
					this.noneChecked = false;
					this.allCheckBox.Checked = true;
				}
				this.currentValue = toolStripStatusLabelBorderSides;
			}

			// Token: 0x04001045 RID: 4165
			private BorderSidesEditor editor;

			// Token: 0x04001046 RID: 4166
			private IWindowsFormsEditorService edSvc;

			// Token: 0x04001047 RID: 4167
			private object originalValue;

			// Token: 0x04001048 RID: 4168
			private object currentValue;

			// Token: 0x04001049 RID: 4169
			private bool updateCurrentValue;

			// Token: 0x0400104A RID: 4170
			private TableLayoutPanel tableLayoutPanel1;

			// Token: 0x0400104B RID: 4171
			private CheckBox allCheckBox;

			// Token: 0x0400104C RID: 4172
			private CheckBox noneCheckBox;

			// Token: 0x0400104D RID: 4173
			private CheckBox topCheckBox;

			// Token: 0x0400104E RID: 4174
			private CheckBox bottomCheckBox;

			// Token: 0x0400104F RID: 4175
			private CheckBox leftCheckBox;

			// Token: 0x04001050 RID: 4176
			private CheckBox rightCheckBox;

			// Token: 0x04001051 RID: 4177
			private Label splitterLabel;

			// Token: 0x04001052 RID: 4178
			private bool allChecked;

			// Token: 0x04001053 RID: 4179
			private bool noneChecked;
		}
	}
}
