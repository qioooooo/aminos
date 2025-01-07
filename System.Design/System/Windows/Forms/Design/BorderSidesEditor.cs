using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Security.Permissions;

namespace System.Windows.Forms.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class BorderSidesEditor : UITypeEditor
	{
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

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		private BorderSidesEditor.BorderSidesEditorUI borderSidesEditorUI;

		private class BorderSidesEditorUI : UserControl
		{
			public BorderSidesEditorUI(BorderSidesEditor editor)
			{
				this.editor = editor;
				this.End();
				this.InitializeComponent();
				base.Size = base.PreferredSize;
			}

			public IWindowsFormsEditorService EditorService
			{
				get
				{
					return this.edSvc;
				}
			}

			public object Value
			{
				get
				{
					return this.currentValue;
				}
			}

			public void End()
			{
				this.edSvc = null;
				this.originalValue = null;
				this.currentValue = null;
				this.updateCurrentValue = false;
			}

			protected override void OnGotFocus(EventArgs e)
			{
				base.OnGotFocus(e);
				this.noneCheckBox.Focus();
			}

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

			private void noneCheckBoxClicked(object sender, EventArgs e)
			{
				if (this.noneChecked)
				{
					this.noneCheckBox.Checked = true;
				}
			}

			private void allCheckBoxClicked(object sender, EventArgs e)
			{
				if (this.allChecked)
				{
					this.allCheckBox.Checked = true;
				}
			}

			private void ResetCheckBoxState()
			{
				this.allCheckBox.Checked = false;
				this.noneCheckBox.Checked = false;
				this.topCheckBox.Checked = false;
				this.bottomCheckBox.Checked = false;
				this.leftCheckBox.Checked = false;
				this.rightCheckBox.Checked = false;
			}

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

			public void Start(IWindowsFormsEditorService edSvc, object value)
			{
				this.edSvc = edSvc;
				this.currentValue = value;
				this.originalValue = value;
				ToolStripStatusLabelBorderSides toolStripStatusLabelBorderSides = (ToolStripStatusLabelBorderSides)value;
				this.SetCheckBoxCheckState(toolStripStatusLabelBorderSides);
				this.updateCurrentValue = true;
			}

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

			private BorderSidesEditor editor;

			private IWindowsFormsEditorService edSvc;

			private object originalValue;

			private object currentValue;

			private bool updateCurrentValue;

			private TableLayoutPanel tableLayoutPanel1;

			private CheckBox allCheckBox;

			private CheckBox noneCheckBox;

			private CheckBox topCheckBox;

			private CheckBox bottomCheckBox;

			private CheckBox leftCheckBox;

			private CheckBox rightCheckBox;

			private Label splitterLabel;

			private bool allChecked;

			private bool noneChecked;
		}
	}
}
