using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000298 RID: 664
	internal class StyleCollectionEditor : CollectionEditor
	{
		// Token: 0x0600188F RID: 6287 RVA: 0x00081B54 File Offset: 0x00080B54
		public StyleCollectionEditor(Type type)
			: base(type)
		{
			this.isRowCollection = type.IsAssignableFrom(typeof(TableLayoutRowStyleCollection));
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x00081B73 File Offset: 0x00080B73
		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			return new StyleCollectionEditor.StyleEditorForm(this, this.isRowCollection);
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001891 RID: 6289 RVA: 0x00081B81 File Offset: 0x00080B81
		protected override string HelpTopic
		{
			get
			{
				return this.helptopic;
			}
		}

		// Token: 0x04001440 RID: 5184
		private bool isRowCollection;

		// Token: 0x04001441 RID: 5185
		protected string helptopic;

		// Token: 0x02000299 RID: 665
		protected class NavigationalTableLayoutPanel : TableLayoutPanel
		{
			// Token: 0x1700043C RID: 1084
			// (get) Token: 0x06001892 RID: 6290 RVA: 0x00081B8C File Offset: 0x00080B8C
			private List<RadioButton> RadioButtons
			{
				get
				{
					List<RadioButton> list = new List<RadioButton>();
					foreach (object obj in base.Controls)
					{
						Control control = (Control)obj;
						RadioButton radioButton = control as RadioButton;
						if (radioButton != null)
						{
							list.Add(radioButton);
						}
					}
					return list;
				}
			}

			// Token: 0x06001893 RID: 6291 RVA: 0x00081BFC File Offset: 0x00080BFC
			protected override bool ProcessDialogKey(Keys keyData)
			{
				bool flag = keyData == Keys.Down;
				bool flag2 = keyData == Keys.Up;
				if (flag || flag2)
				{
					List<RadioButton> radioButtons = this.RadioButtons;
					for (int i = 0; i < radioButtons.Count; i++)
					{
						RadioButton radioButton = radioButtons[i];
						if (radioButton.Focused)
						{
							int num;
							if (flag)
							{
								num = ((i == this.RadioButtons.Count - 1) ? 0 : (i + 1));
							}
							else
							{
								num = ((i == 0) ? (this.RadioButtons.Count - 1) : (i - 1));
							}
							radioButtons[num].Focus();
							return true;
						}
					}
				}
				return base.ProcessDialogKey(keyData);
			}
		}

		// Token: 0x0200029A RID: 666
		protected class StyleEditorForm : CollectionEditor.CollectionForm
		{
			// Token: 0x06001895 RID: 6293 RVA: 0x00081C98 File Offset: 0x00080C98
			internal StyleEditorForm(CollectionEditor editor, bool isRowCollection)
				: base(editor)
			{
				this.editor = (StyleCollectionEditor)editor;
				this.isRowCollection = isRowCollection;
				this.InitializeComponent();
				this.HookEvents();
				base.ActiveControl = this.columnsAndRowsListView;
				this.tlp = base.Context.Instance as TableLayoutPanel;
				this.tlp.SuspendLayout();
				this.deleteList = new ArrayList();
				IDesignerHost designerHost = this.tlp.Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (designerHost != null)
				{
					this.tlpDesigner = designerHost.GetDesigner(this.tlp) as TableLayoutPanelDesigner;
					this.compSvc = designerHost.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				}
				this.rowStyleProp = TypeDescriptor.GetProperties(this.tlp)["RowStyles"];
				this.colStyleProp = TypeDescriptor.GetProperties(this.tlp)["ColumnStyles"];
				this.tlpDesigner.SuspendEnsureAvailableStyles();
			}

			// Token: 0x06001896 RID: 6294 RVA: 0x00081D9C File Offset: 0x00080D9C
			private void HookEvents()
			{
				base.HelpButtonClicked += this.OnHelpButtonClicked;
				this.columnsAndRowsListView.SelectedIndexChanged += this.OnListViewSelectedIndexChanged;
				this.columnsOrRowsComboBox.SelectionChangeCommitted += this.OnComboBoxSelectionChangeCommitted;
				this.okButton.Click += this.OnOkButtonClick;
				this.cancelButton.Click += this.OnCancelButtonClick;
				this.addButton.Click += this.OnAddButtonClick;
				this.removeButton.Click += this.OnRemoveButtonClick;
				this.insertButton.Click += this.OnInsertButtonClick;
				this.absoluteRadioButton.Enter += this.OnAbsoluteEnter;
				this.absoluteNumericUpDown.ValueChanged += this.OnValueChanged;
				this.percentRadioButton.Enter += this.OnPercentEnter;
				this.percentNumericUpDown.ValueChanged += this.OnValueChanged;
				this.autoSizedRadioButton.Enter += this.OnAutoSizeEnter;
				base.Shown += this.OnShown;
				this.helperLinkLabel1.LinkClicked += this.OnLink1Click;
				this.helperLinkLabel2.LinkClicked += this.OnLink2Click;
			}

			// Token: 0x06001897 RID: 6295 RVA: 0x00081F10 File Offset: 0x00080F10
			private void InitializeComponent()
			{
				ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StyleCollectionEditor));
				this.addRemoveInsertTableLayoutPanel = new TableLayoutPanel();
				this.addButton = new Button();
				this.removeButton = new Button();
				this.insertButton = new Button();
				this.okCancelTableLayoutPanel = new TableLayoutPanel();
				this.okButton = new Button();
				this.cancelButton = new Button();
				this.overarchingTableLayoutPanel = new TableLayoutPanel();
				this.showTableLayoutPanel = new TableLayoutPanel();
				this.memberTypeLabel = new Label();
				this.columnsOrRowsComboBox = new ComboBox();
				this.columnsAndRowsListView = new ListView();
				this.membersColumnHeader = new ColumnHeader(componentResourceManager.GetString("columnsAndRowsListView.Columns"));
				this.sizeTypeColumnHeader = new ColumnHeader(componentResourceManager.GetString("columnsAndRowsListView.Columns1"));
				this.valueColumnHeader = new ColumnHeader(componentResourceManager.GetString("columnsAndRowsListView.Columns2"));
				this.helperTextTableLayoutPanel = new TableLayoutPanel();
				this.infoPictureBox2 = new PictureBox();
				this.infoPictureBox1 = new PictureBox();
				this.helperLinkLabel1 = new LinkLabel();
				this.helperLinkLabel2 = new LinkLabel();
				this.sizeTypeGroupBox = new GroupBox();
				this.sizeTypeTableLayoutPanel = new StyleCollectionEditor.NavigationalTableLayoutPanel();
				this.absoluteNumericUpDown = new NumericUpDown();
				this.absoluteRadioButton = new RadioButton();
				this.pixelsLabel = new Label();
				this.percentLabel = new Label();
				this.percentRadioButton = new RadioButton();
				this.autoSizedRadioButton = new RadioButton();
				this.percentNumericUpDown = new NumericUpDown();
				this.addRemoveInsertTableLayoutPanel.SuspendLayout();
				this.okCancelTableLayoutPanel.SuspendLayout();
				this.overarchingTableLayoutPanel.SuspendLayout();
				this.showTableLayoutPanel.SuspendLayout();
				this.helperTextTableLayoutPanel.SuspendLayout();
				((ISupportInitialize)this.infoPictureBox2).BeginInit();
				((ISupportInitialize)this.infoPictureBox1).BeginInit();
				this.sizeTypeGroupBox.SuspendLayout();
				this.sizeTypeTableLayoutPanel.SuspendLayout();
				((ISupportInitialize)this.absoluteNumericUpDown).BeginInit();
				((ISupportInitialize)this.percentNumericUpDown).BeginInit();
				base.SuspendLayout();
				componentResourceManager.ApplyResources(this.addRemoveInsertTableLayoutPanel, "addRemoveInsertTableLayoutPanel");
				this.addRemoveInsertTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
				this.addRemoveInsertTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
				this.addRemoveInsertTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
				this.addRemoveInsertTableLayoutPanel.Controls.Add(this.addButton, 0, 0);
				this.addRemoveInsertTableLayoutPanel.Controls.Add(this.removeButton, 1, 0);
				this.addRemoveInsertTableLayoutPanel.Controls.Add(this.insertButton, 2, 0);
				this.addRemoveInsertTableLayoutPanel.Margin = new Padding(0, 3, 3, 3);
				this.addRemoveInsertTableLayoutPanel.Name = "addRemoveInsertTableLayoutPanel";
				this.addRemoveInsertTableLayoutPanel.RowStyles.Add(new RowStyle());
				componentResourceManager.ApplyResources(this.addButton, "addButton");
				this.addButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				this.addButton.Margin = new Padding(0, 0, 4, 0);
				this.addButton.MinimumSize = new Size(75, 23);
				this.addButton.Name = "addButton";
				this.addButton.Padding = new Padding(10, 0, 10, 0);
				componentResourceManager.ApplyResources(this.removeButton, "removeButton");
				this.removeButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				this.removeButton.Margin = new Padding(2, 0, 2, 0);
				this.removeButton.MinimumSize = new Size(75, 23);
				this.removeButton.Name = "removeButton";
				this.removeButton.Padding = new Padding(10, 0, 10, 0);
				componentResourceManager.ApplyResources(this.insertButton, "insertButton");
				this.insertButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				this.insertButton.Margin = new Padding(4, 0, 0, 0);
				this.insertButton.MinimumSize = new Size(75, 23);
				this.insertButton.Name = "insertButton";
				this.insertButton.Padding = new Padding(10, 0, 10, 0);
				componentResourceManager.ApplyResources(this.okCancelTableLayoutPanel, "okCancelTableLayoutPanel");
				this.overarchingTableLayoutPanel.SetColumnSpan(this.okCancelTableLayoutPanel, 2);
				this.okCancelTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
				this.okCancelTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
				this.okCancelTableLayoutPanel.Controls.Add(this.okButton, 0, 0);
				this.okCancelTableLayoutPanel.Controls.Add(this.cancelButton, 1, 0);
				this.okCancelTableLayoutPanel.Margin = new Padding(0, 6, 0, 0);
				this.okCancelTableLayoutPanel.Name = "okCancelTableLayoutPanel";
				this.okCancelTableLayoutPanel.RowStyles.Add(new RowStyle());
				componentResourceManager.ApplyResources(this.okButton, "okButton");
				this.okButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				this.okButton.Margin = new Padding(0, 0, 3, 0);
				this.okButton.MinimumSize = new Size(75, 23);
				this.okButton.Name = "okButton";
				this.okButton.Padding = new Padding(10, 0, 10, 0);
				componentResourceManager.ApplyResources(this.cancelButton, "cancelButton");
				this.cancelButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				this.cancelButton.DialogResult = DialogResult.Cancel;
				this.cancelButton.Margin = new Padding(3, 0, 0, 0);
				this.cancelButton.MinimumSize = new Size(75, 23);
				this.cancelButton.Name = "cancelButton";
				this.cancelButton.Padding = new Padding(10, 0, 10, 0);
				componentResourceManager.ApplyResources(this.overarchingTableLayoutPanel, "overarchingTableLayoutPanel");
				this.overarchingTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
				this.overarchingTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
				this.overarchingTableLayoutPanel.Controls.Add(this.sizeTypeGroupBox, 1, 0);
				this.overarchingTableLayoutPanel.Controls.Add(this.okCancelTableLayoutPanel, 0, 4);
				this.overarchingTableLayoutPanel.Controls.Add(this.showTableLayoutPanel, 0, 0);
				this.overarchingTableLayoutPanel.Controls.Add(this.addRemoveInsertTableLayoutPanel, 0, 3);
				this.overarchingTableLayoutPanel.Controls.Add(this.columnsAndRowsListView, 0, 1);
				this.overarchingTableLayoutPanel.Controls.Add(this.helperTextTableLayoutPanel, 1, 2);
				this.overarchingTableLayoutPanel.Name = "overarchingTableLayoutPanel";
				this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle());
				this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle());
				this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
				this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle());
				this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle());
				componentResourceManager.ApplyResources(this.showTableLayoutPanel, "showTableLayoutPanel");
				this.showTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
				this.showTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
				this.showTableLayoutPanel.Controls.Add(this.memberTypeLabel, 0, 0);
				this.showTableLayoutPanel.Controls.Add(this.columnsOrRowsComboBox, 1, 0);
				this.showTableLayoutPanel.Margin = new Padding(0, 0, 3, 3);
				this.showTableLayoutPanel.Name = "showTableLayoutPanel";
				this.showTableLayoutPanel.RowStyles.Add(new RowStyle());
				componentResourceManager.ApplyResources(this.memberTypeLabel, "memberTypeLabel");
				this.memberTypeLabel.Margin = new Padding(0, 0, 3, 0);
				this.memberTypeLabel.Name = "memberTypeLabel";
				componentResourceManager.ApplyResources(this.columnsOrRowsComboBox, "columnsOrRowsComboBox");
				this.columnsOrRowsComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				this.columnsOrRowsComboBox.FormattingEnabled = true;
				this.columnsOrRowsComboBox.Items.AddRange(new object[]
				{
					componentResourceManager.GetString("columnsOrRowsComboBox.Items"),
					componentResourceManager.GetString("columnsOrRowsComboBox.Items1")
				});
				this.columnsOrRowsComboBox.Margin = new Padding(3, 0, 0, 0);
				this.columnsOrRowsComboBox.Name = "columnsOrRowsComboBox";
				componentResourceManager.ApplyResources(this.columnsAndRowsListView, "columnsAndRowsListView");
				this.columnsAndRowsListView.Columns.AddRange(new ColumnHeader[] { this.membersColumnHeader, this.sizeTypeColumnHeader, this.valueColumnHeader });
				this.columnsAndRowsListView.FullRowSelect = true;
				this.columnsAndRowsListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
				this.columnsAndRowsListView.HideSelection = false;
				this.columnsAndRowsListView.Margin = new Padding(0, 3, 3, 3);
				this.columnsAndRowsListView.Name = "columnsAndRowsListView";
				this.overarchingTableLayoutPanel.SetRowSpan(this.columnsAndRowsListView, 2);
				this.columnsAndRowsListView.View = View.Details;
				componentResourceManager.ApplyResources(this.membersColumnHeader, "membersColumnHeader");
				componentResourceManager.ApplyResources(this.sizeTypeColumnHeader, "sizeTypeColumnHeader");
				componentResourceManager.ApplyResources(this.valueColumnHeader, "valueColumnHeader");
				componentResourceManager.ApplyResources(this.helperTextTableLayoutPanel, "helperTextTableLayoutPanel");
				this.helperTextTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
				this.helperTextTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
				this.helperTextTableLayoutPanel.Controls.Add(this.infoPictureBox2, 0, 1);
				this.helperTextTableLayoutPanel.Controls.Add(this.infoPictureBox1, 0, 0);
				this.helperTextTableLayoutPanel.Controls.Add(this.helperLinkLabel1, 1, 0);
				this.helperTextTableLayoutPanel.Controls.Add(this.helperLinkLabel2, 1, 1);
				this.helperTextTableLayoutPanel.Margin = new Padding(6, 6, 0, 3);
				this.helperTextTableLayoutPanel.Name = "helperTextTableLayoutPanel";
				this.helperTextTableLayoutPanel.RowStyles.Add(new RowStyle());
				this.helperTextTableLayoutPanel.RowStyles.Add(new RowStyle());
				componentResourceManager.ApplyResources(this.infoPictureBox2, "infoPictureBox2");
				this.infoPictureBox2.Name = "infoPictureBox2";
				this.infoPictureBox2.TabStop = false;
				componentResourceManager.ApplyResources(this.infoPictureBox1, "infoPictureBox1");
				this.infoPictureBox1.Name = "infoPictureBox1";
				this.infoPictureBox1.TabStop = false;
				componentResourceManager.ApplyResources(this.helperLinkLabel1, "helperLinkLabel1");
				this.helperLinkLabel1.Margin = new Padding(3, 0, 0, 3);
				this.helperLinkLabel1.Name = "helperLinkLabel1";
				this.helperLinkLabel1.TabStop = true;
				this.helperLinkLabel1.UseCompatibleTextRendering = true;
				componentResourceManager.ApplyResources(this.helperLinkLabel2, "helperLinkLabel2");
				this.helperLinkLabel2.Margin = new Padding(3, 3, 0, 0);
				this.helperLinkLabel2.Name = "helperLinkLabel2";
				this.helperLinkLabel2.TabStop = true;
				this.helperLinkLabel2.UseCompatibleTextRendering = true;
				componentResourceManager.ApplyResources(this.sizeTypeGroupBox, "sizeTypeGroupBox");
				this.sizeTypeGroupBox.Controls.Add(this.sizeTypeTableLayoutPanel);
				this.sizeTypeGroupBox.Margin = new Padding(6, 0, 0, 3);
				this.sizeTypeGroupBox.Name = "sizeTypeGroupBox";
				this.sizeTypeGroupBox.Padding = new Padding(0);
				this.overarchingTableLayoutPanel.SetRowSpan(this.sizeTypeGroupBox, 2);
				this.sizeTypeGroupBox.TabStop = false;
				componentResourceManager.ApplyResources(this.sizeTypeTableLayoutPanel, "sizeTypeTableLayoutPanel");
				this.sizeTypeTableLayoutPanel.Controls.Add(this.absoluteNumericUpDown, 1, 0);
				this.sizeTypeTableLayoutPanel.Controls.Add(this.absoluteRadioButton, 0, 0);
				this.sizeTypeTableLayoutPanel.Controls.Add(this.pixelsLabel, 2, 0);
				this.sizeTypeTableLayoutPanel.Controls.Add(this.percentLabel, 2, 1);
				this.sizeTypeTableLayoutPanel.Controls.Add(this.percentRadioButton, 0, 1);
				this.sizeTypeTableLayoutPanel.Controls.Add(this.autoSizedRadioButton, 0, 2);
				this.sizeTypeTableLayoutPanel.Controls.Add(this.percentNumericUpDown, 1, 1);
				this.sizeTypeTableLayoutPanel.Margin = new Padding(9);
				this.sizeTypeTableLayoutPanel.Name = "sizeTypeTableLayoutPanel";
				componentResourceManager.ApplyResources(this.absoluteNumericUpDown, "absoluteNumericUpDown");
				NumericUpDown numericUpDown = this.absoluteNumericUpDown;
				int[] array = new int[4];
				array[0] = 99999;
				numericUpDown.Maximum = new decimal(array);
				this.absoluteNumericUpDown.Name = "absoluteNumericUpDown";
				componentResourceManager.ApplyResources(this.absoluteRadioButton, "absoluteRadioButton");
				this.absoluteRadioButton.Margin = new Padding(0, 3, 3, 0);
				this.absoluteRadioButton.Name = "absoluteRadioButton";
				componentResourceManager.ApplyResources(this.pixelsLabel, "pixelsLabel");
				this.pixelsLabel.Name = "pixelsLabel";
				componentResourceManager.ApplyResources(this.percentLabel, "percentLabel");
				this.percentLabel.Name = "percentLabel";
				componentResourceManager.ApplyResources(this.percentRadioButton, "percentRadioButton");
				this.percentRadioButton.Margin = new Padding(0, 3, 3, 0);
				this.percentRadioButton.Name = "percentRadioButton";
				componentResourceManager.ApplyResources(this.autoSizedRadioButton, "autoSizedRadioButton");
				this.autoSizedRadioButton.Margin = new Padding(0, 3, 3, 0);
				this.autoSizedRadioButton.Name = "autoSizedRadioButton";
				componentResourceManager.ApplyResources(this.percentNumericUpDown, "percentNumericUpDown");
				this.percentNumericUpDown.DecimalPlaces = 2;
				NumericUpDown numericUpDown2 = this.percentNumericUpDown;
				int[] array2 = new int[4];
				array2[0] = 9999;
				numericUpDown2.Maximum = new decimal(array2);
				this.percentNumericUpDown.Name = "percentNumericUpDown";
				base.AcceptButton = this.okButton;
				componentResourceManager.ApplyResources(this, "$this");
				base.AutoScaleMode = AutoScaleMode.Font;
				base.CancelButton = this.cancelButton;
				base.Controls.Add(this.overarchingTableLayoutPanel);
				base.HelpButton = true;
				base.MaximizeBox = false;
				base.MinimizeBox = false;
				base.Name = "Form1";
				base.ShowIcon = false;
				base.ShowInTaskbar = false;
				this.addRemoveInsertTableLayoutPanel.ResumeLayout(false);
				this.addRemoveInsertTableLayoutPanel.PerformLayout();
				this.okCancelTableLayoutPanel.ResumeLayout(false);
				this.okCancelTableLayoutPanel.PerformLayout();
				this.overarchingTableLayoutPanel.ResumeLayout(false);
				this.overarchingTableLayoutPanel.PerformLayout();
				this.showTableLayoutPanel.ResumeLayout(false);
				this.showTableLayoutPanel.PerformLayout();
				this.helperTextTableLayoutPanel.ResumeLayout(false);
				this.helperTextTableLayoutPanel.PerformLayout();
				((ISupportInitialize)this.infoPictureBox2).EndInit();
				((ISupportInitialize)this.infoPictureBox1).EndInit();
				this.sizeTypeGroupBox.ResumeLayout(false);
				this.sizeTypeTableLayoutPanel.ResumeLayout(false);
				this.sizeTypeTableLayoutPanel.PerformLayout();
				((ISupportInitialize)this.absoluteNumericUpDown).EndInit();
				((ISupportInitialize)this.percentNumericUpDown).EndInit();
				base.ResumeLayout(false);
			}

			// Token: 0x06001898 RID: 6296 RVA: 0x00082E54 File Offset: 0x00081E54
			private void OnShown(object sender, EventArgs e)
			{
				this.isDialogDirty = false;
			}

			// Token: 0x06001899 RID: 6297 RVA: 0x00082E60 File Offset: 0x00081E60
			private void OnLink1Click(object sender, LinkLabelLinkClickedEventArgs e)
			{
				CancelEventArgs cancelEventArgs = new CancelEventArgs();
				this.editor.helptopic = "net.ComponentModel.StyleCollectionEditor.TLP.SpanRowsColumns";
				this.OnHelpButtonClicked(sender, cancelEventArgs);
			}

			// Token: 0x0600189A RID: 6298 RVA: 0x00082E8C File Offset: 0x00081E8C
			private void OnLink2Click(object sender, LinkLabelLinkClickedEventArgs e)
			{
				CancelEventArgs cancelEventArgs = new CancelEventArgs();
				this.editor.helptopic = "net.ComponentModel.StyleCollectionEditor.TLP.AnchorDock";
				this.OnHelpButtonClicked(sender, cancelEventArgs);
			}

			// Token: 0x0600189B RID: 6299 RVA: 0x00082EB7 File Offset: 0x00081EB7
			private void OnHelpButtonClicked(object sender, CancelEventArgs e)
			{
				e.Cancel = true;
				this.editor.helptopic = "net.ComponentModel.StyleCollectionEditor";
				this.editor.ShowHelp();
			}

			// Token: 0x0600189C RID: 6300 RVA: 0x00082EDB File Offset: 0x00081EDB
			protected override void OnEditValueChanged()
			{
			}

			// Token: 0x0600189D RID: 6301 RVA: 0x00082EE0 File Offset: 0x00081EE0
			protected internal override DialogResult ShowEditorDialog(IWindowsFormsEditorService edSvc)
			{
				if (this.compSvc != null)
				{
					if (this.rowStyleProp != null)
					{
						this.compSvc.OnComponentChanging(this.tlp, this.rowStyleProp);
					}
					if (this.colStyleProp != null)
					{
						this.compSvc.OnComponentChanging(this.tlp, this.colStyleProp);
					}
				}
				int[] columnWidths = this.tlp.GetColumnWidths();
				int[] rowHeights = this.tlp.GetRowHeights();
				if (this.tlp.ColumnStyles.Count > columnWidths.Length)
				{
					int num = this.tlp.ColumnStyles.Count - columnWidths.Length;
					for (int i = 0; i < num; i++)
					{
						this.tlp.ColumnStyles.RemoveAt(this.tlp.ColumnStyles.Count - 1);
					}
				}
				if (this.tlp.RowStyles.Count > rowHeights.Length)
				{
					int num2 = this.tlp.RowStyles.Count - rowHeights.Length;
					for (int j = 0; j < num2; j++)
					{
						this.tlp.RowStyles.RemoveAt(this.tlp.RowStyles.Count - 1);
					}
				}
				this.columnsOrRowsComboBox.SelectedIndex = (this.isRowCollection ? 1 : 0);
				this.InitListView();
				return base.ShowEditorDialog(edSvc);
			}

			// Token: 0x0600189E RID: 6302 RVA: 0x00083028 File Offset: 0x00082028
			private string FormatValueString(SizeType type, float value)
			{
				if (type == SizeType.Absolute)
				{
					return value.ToString(CultureInfo.CurrentCulture);
				}
				if (type == SizeType.Percent)
				{
					return (value / 100f).ToString("P", CultureInfo.CurrentCulture);
				}
				return string.Empty;
			}

			// Token: 0x0600189F RID: 6303 RVA: 0x0008306C File Offset: 0x0008206C
			private void InitListView()
			{
				this.columnsAndRowsListView.Items.Clear();
				string text = (this.isRowCollection ? "Row" : "Column");
				int num = (this.isRowCollection ? this.tlp.RowStyles.Count : this.tlp.ColumnStyles.Count);
				for (int i = 0; i < num; i++)
				{
					string text2;
					string text3;
					if (this.isRowCollection)
					{
						RowStyle rowStyle = this.tlp.RowStyles[i];
						text2 = rowStyle.SizeType.ToString();
						text3 = this.FormatValueString(rowStyle.SizeType, rowStyle.Height);
					}
					else
					{
						ColumnStyle columnStyle = this.tlp.ColumnStyles[i];
						text2 = columnStyle.SizeType.ToString();
						text3 = this.FormatValueString(columnStyle.SizeType, columnStyle.Width);
					}
					this.columnsAndRowsListView.Items.Add(new ListViewItem(new string[]
					{
						text + (i + 1).ToString(CultureInfo.InvariantCulture),
						text2,
						text3
					}));
				}
				if (num > 0)
				{
					this.ClearAndSetSelectionAndFocus(0);
				}
				this.removeButton.Enabled = this.columnsAndRowsListView.Items.Count > 1;
			}

			// Token: 0x060018A0 RID: 6304 RVA: 0x000831CC File Offset: 0x000821CC
			private void UpdateListViewItem(int index, string member, string type, string value)
			{
				this.columnsAndRowsListView.Items[index].SubItems[StyleCollectionEditor.StyleEditorForm.MEMBER_INDEX].Text = member;
				this.columnsAndRowsListView.Items[index].SubItems[StyleCollectionEditor.StyleEditorForm.TYPE_INDEX].Text = type;
				this.columnsAndRowsListView.Items[index].SubItems[StyleCollectionEditor.StyleEditorForm.VALUE_INDEX].Text = value;
			}

			// Token: 0x060018A1 RID: 6305 RVA: 0x0008324C File Offset: 0x0008224C
			private void UpdateListViewMember()
			{
				for (int i = 0; i < this.columnsAndRowsListView.Items.Count; i++)
				{
					this.columnsAndRowsListView.Items[i].SubItems[StyleCollectionEditor.StyleEditorForm.MEMBER_INDEX].Text = (this.isRowCollection ? "Row" : "Column") + (i + 1).ToString(CultureInfo.InvariantCulture);
				}
			}

			// Token: 0x060018A2 RID: 6306 RVA: 0x000832C2 File Offset: 0x000822C2
			private void OnComboBoxSelectionChangeCommitted(object sender, EventArgs e)
			{
				this.isRowCollection = this.columnsOrRowsComboBox.SelectedIndex != 0;
				this.InitListView();
			}

			// Token: 0x060018A3 RID: 6307 RVA: 0x000832E4 File Offset: 0x000822E4
			private void OnListViewSelectedIndexChanged(object sender, EventArgs e)
			{
				ListView.SelectedListViewItemCollection selectedItems = this.columnsAndRowsListView.SelectedItems;
				if (selectedItems.Count == 0)
				{
					if (!this.haveInvoked)
					{
						base.BeginInvoke(new EventHandler(this.OnListSelectionComplete));
						this.haveInvoked = true;
					}
					return;
				}
				this.sizeTypeGroupBox.Enabled = true;
				this.insertButton.Enabled = true;
				if (selectedItems.Count == this.columnsAndRowsListView.Items.Count)
				{
					this.removeButton.Enabled = false;
				}
				else
				{
					this.removeButton.Enabled = this.columnsAndRowsListView.Items.Count > 1;
				}
				if (selectedItems.Count == 1)
				{
					int num = this.columnsAndRowsListView.Items.IndexOf(selectedItems[0]);
					if (this.isRowCollection)
					{
						this.UpdateGroupBox(this.tlp.RowStyles[num].SizeType, this.tlp.RowStyles[num].Height);
						return;
					}
					this.UpdateGroupBox(this.tlp.ColumnStyles[num].SizeType, this.tlp.ColumnStyles[num].Width);
					return;
				}
				else
				{
					bool flag = true;
					int num2 = this.columnsAndRowsListView.Items.IndexOf(selectedItems[0]);
					SizeType sizeType = (this.isRowCollection ? this.tlp.RowStyles[num2].SizeType : this.tlp.ColumnStyles[num2].SizeType);
					float num3 = (this.isRowCollection ? this.tlp.RowStyles[num2].Height : this.tlp.ColumnStyles[num2].Width);
					for (int i = 1; i < selectedItems.Count; i++)
					{
						num2 = this.columnsAndRowsListView.Items.IndexOf(selectedItems[i]);
						if (sizeType != (this.isRowCollection ? this.tlp.RowStyles[num2].SizeType : this.tlp.ColumnStyles[num2].SizeType))
						{
							flag = false;
							break;
						}
						if (num3 != (this.isRowCollection ? this.tlp.RowStyles[num2].Height : this.tlp.ColumnStyles[num2].Width))
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						this.ResetAllRadioButtons();
						return;
					}
					this.UpdateGroupBox(sizeType, num3);
					return;
				}
			}

			// Token: 0x060018A4 RID: 6308 RVA: 0x00083574 File Offset: 0x00082574
			private void OnListSelectionComplete(object sender, EventArgs e)
			{
				this.haveInvoked = false;
				if (this.columnsAndRowsListView.SelectedItems.Count == 0)
				{
					this.ResetAllRadioButtons();
					this.sizeTypeGroupBox.Enabled = false;
					this.insertButton.Enabled = false;
					this.removeButton.Enabled = false;
				}
			}

			// Token: 0x060018A5 RID: 6309 RVA: 0x000835C4 File Offset: 0x000825C4
			private void ResetAllRadioButtons()
			{
				this.absoluteRadioButton.Checked = false;
				this.ResetAbsolute();
				this.percentRadioButton.Checked = false;
				this.ResetPercent();
				this.autoSizedRadioButton.Checked = false;
			}

			// Token: 0x060018A6 RID: 6310 RVA: 0x000835F8 File Offset: 0x000825F8
			private void ResetAbsolute()
			{
				this.absoluteNumericUpDown.ValueChanged -= this.OnValueChanged;
				this.absoluteNumericUpDown.Enabled = false;
				this.absoluteNumericUpDown.Value = DesignerUtils.MINIMUMSTYLESIZE;
				this.absoluteNumericUpDown.ValueChanged += this.OnValueChanged;
			}

			// Token: 0x060018A7 RID: 6311 RVA: 0x00083654 File Offset: 0x00082654
			private void ResetPercent()
			{
				this.percentNumericUpDown.ValueChanged -= this.OnValueChanged;
				this.percentNumericUpDown.Enabled = false;
				this.percentNumericUpDown.Value = DesignerUtils.MINIMUMSTYLEPERCENT;
				this.percentNumericUpDown.ValueChanged += this.OnValueChanged;
			}

			// Token: 0x060018A8 RID: 6312 RVA: 0x000836B0 File Offset: 0x000826B0
			private void UpdateGroupBox(SizeType type, float value)
			{
				switch (type)
				{
				case SizeType.AutoSize:
					this.autoSizedRadioButton.Checked = true;
					this.ResetAbsolute();
					this.ResetPercent();
					return;
				case SizeType.Absolute:
					this.absoluteRadioButton.Checked = true;
					this.absoluteNumericUpDown.Enabled = true;
					try
					{
						this.absoluteNumericUpDown.Value = (decimal)value;
					}
					catch (ArgumentOutOfRangeException)
					{
						this.absoluteNumericUpDown.Value = DesignerUtils.MINIMUMSTYLESIZE;
					}
					this.ResetPercent();
					return;
				case SizeType.Percent:
					this.percentRadioButton.Checked = true;
					this.percentNumericUpDown.Enabled = true;
					try
					{
						this.percentNumericUpDown.Value = (decimal)value;
					}
					catch (ArgumentOutOfRangeException)
					{
						this.percentNumericUpDown.Value = DesignerUtils.MINIMUMSTYLEPERCENT;
					}
					this.ResetAbsolute();
					return;
				default:
					return;
				}
			}

			// Token: 0x060018A9 RID: 6313 RVA: 0x0008379C File Offset: 0x0008279C
			private void ClearAndSetSelectionAndFocus(int index)
			{
				this.columnsAndRowsListView.BeginUpdate();
				this.columnsAndRowsListView.Focus();
				if (this.columnsAndRowsListView.FocusedItem != null)
				{
					this.columnsAndRowsListView.FocusedItem.Focused = false;
				}
				this.columnsAndRowsListView.SelectedItems.Clear();
				this.columnsAndRowsListView.Items[index].Selected = true;
				this.columnsAndRowsListView.Items[index].Focused = true;
				this.columnsAndRowsListView.Items[index].EnsureVisible();
				this.columnsAndRowsListView.EndUpdate();
			}

			// Token: 0x060018AA RID: 6314 RVA: 0x00083840 File Offset: 0x00082840
			private void AddItem(int index)
			{
				this.tlpDesigner.InsertRowCol(this.isRowCollection, index);
				string text;
				if (this.isRowCollection)
				{
					text = "Row" + this.tlp.RowStyles.Count.ToString(CultureInfo.InvariantCulture);
				}
				else
				{
					text = "Column" + this.tlp.RowStyles.Count.ToString(CultureInfo.InvariantCulture);
				}
				if (text != null)
				{
					this.columnsAndRowsListView.Items.Insert(index, new ListViewItem(new string[]
					{
						text,
						SizeType.Absolute.ToString(),
						DesignerUtils.MINIMUMSTYLESIZE.ToString(CultureInfo.InvariantCulture)
					}));
					this.UpdateListViewMember();
					this.ClearAndSetSelectionAndFocus(index);
				}
			}

			// Token: 0x060018AB RID: 6315 RVA: 0x0008390E File Offset: 0x0008290E
			private void OnAddButtonClick(object sender, EventArgs e)
			{
				this.isDialogDirty = true;
				this.AddItem(this.columnsAndRowsListView.Items.Count);
			}

			// Token: 0x060018AC RID: 6316 RVA: 0x00083930 File Offset: 0x00082930
			private void OnInsertButtonClick(object sender, EventArgs e)
			{
				this.isDialogDirty = true;
				this.AddItem(this.columnsAndRowsListView.SelectedIndices[0]);
				this.tlpDesigner.FixUpControlsOnInsert(this.isRowCollection, this.columnsAndRowsListView.SelectedIndices[0]);
			}

			// Token: 0x060018AD RID: 6317 RVA: 0x00083980 File Offset: 0x00082980
			private void OnRemoveButtonClick(object sender, EventArgs e)
			{
				if (this.columnsAndRowsListView.Items.Count == 1 || this.columnsAndRowsListView.Items.Count == this.columnsAndRowsListView.SelectedIndices.Count)
				{
					return;
				}
				this.isDialogDirty = true;
				int num = this.columnsAndRowsListView.SelectedIndices[0];
				for (int i = this.columnsAndRowsListView.SelectedIndices.Count - 1; i >= 0; i--)
				{
					int num2 = this.columnsAndRowsListView.SelectedIndices[i];
					this.tlpDesigner.FixUpControlsOnDelete(this.isRowCollection, num2, this.deleteList);
					this.tlpDesigner.DeleteRowCol(this.isRowCollection, num2);
					if (this.isRowCollection)
					{
						this.columnsAndRowsListView.Items.RemoveAt(num2);
					}
					else
					{
						this.columnsAndRowsListView.Items.RemoveAt(num2);
					}
				}
				if (num >= this.columnsAndRowsListView.Items.Count)
				{
					num--;
				}
				this.UpdateListViewMember();
				this.ClearAndSetSelectionAndFocus(num);
			}

			// Token: 0x060018AE RID: 6318 RVA: 0x00083A88 File Offset: 0x00082A88
			private void UpdateTypeAndValue(SizeType type, float value)
			{
				for (int i = 0; i < this.columnsAndRowsListView.SelectedIndices.Count; i++)
				{
					int num = this.columnsAndRowsListView.SelectedIndices[i];
					if (this.isRowCollection)
					{
						this.tlp.RowStyles[num].SizeType = type;
						this.tlp.RowStyles[num].Height = value;
					}
					else
					{
						this.tlp.ColumnStyles[num].SizeType = type;
						this.tlp.ColumnStyles[num].Width = value;
					}
					this.UpdateListViewItem(num, this.columnsAndRowsListView.Items[num].SubItems[StyleCollectionEditor.StyleEditorForm.MEMBER_INDEX].Text, type.ToString(), this.FormatValueString(type, value));
				}
			}

			// Token: 0x060018AF RID: 6319 RVA: 0x00083B6D File Offset: 0x00082B6D
			private void OnAbsoluteEnter(object sender, EventArgs e)
			{
				this.isDialogDirty = true;
				this.UpdateTypeAndValue(SizeType.Absolute, (float)this.absoluteNumericUpDown.Value);
				this.absoluteNumericUpDown.Enabled = true;
				this.ResetPercent();
			}

			// Token: 0x060018B0 RID: 6320 RVA: 0x00083BA0 File Offset: 0x00082BA0
			private void OnPercentEnter(object sender, EventArgs e)
			{
				this.isDialogDirty = true;
				this.UpdateTypeAndValue(SizeType.Percent, (float)this.percentNumericUpDown.Value);
				this.percentNumericUpDown.Enabled = true;
				this.ResetAbsolute();
			}

			// Token: 0x060018B1 RID: 6321 RVA: 0x00083BD3 File Offset: 0x00082BD3
			private void OnAutoSizeEnter(object sender, EventArgs e)
			{
				this.isDialogDirty = true;
				this.UpdateTypeAndValue(SizeType.AutoSize, 0f);
				this.ResetAbsolute();
				this.ResetPercent();
			}

			// Token: 0x060018B2 RID: 6322 RVA: 0x00083BF4 File Offset: 0x00082BF4
			private void OnValueChanged(object sender, EventArgs e)
			{
				if (this.absoluteNumericUpDown == sender && this.absoluteRadioButton.Checked)
				{
					this.isDialogDirty = true;
					this.UpdateTypeAndValue(SizeType.Absolute, (float)this.absoluteNumericUpDown.Value);
					return;
				}
				if (this.percentNumericUpDown == sender && this.percentRadioButton.Checked)
				{
					this.isDialogDirty = true;
					this.UpdateTypeAndValue(SizeType.Percent, (float)this.percentNumericUpDown.Value);
				}
			}

			// Token: 0x060018B3 RID: 6323 RVA: 0x00083C6C File Offset: 0x00082C6C
			private void NormalizePercentStyle(bool normalizeRow)
			{
				int num = (normalizeRow ? this.tlp.RowStyles.Count : this.tlp.ColumnStyles.Count);
				float num2 = 0f;
				for (int i = 0; i < num; i++)
				{
					if (normalizeRow)
					{
						if (this.tlp.RowStyles[i].SizeType == SizeType.Percent)
						{
							num2 += this.tlp.RowStyles[i].Height;
						}
					}
					else if (this.tlp.ColumnStyles[i].SizeType == SizeType.Percent)
					{
						num2 += this.tlp.ColumnStyles[i].Width;
					}
				}
				if (num2 == 100f || num2 == 0f)
				{
					return;
				}
				for (int j = 0; j < num; j++)
				{
					if (normalizeRow)
					{
						if (this.tlp.RowStyles[j].SizeType == SizeType.Percent)
						{
							this.tlp.RowStyles[j].Height = this.tlp.RowStyles[j].Height * 100f / num2;
						}
					}
					else if (this.tlp.ColumnStyles[j].SizeType == SizeType.Percent)
					{
						this.tlp.ColumnStyles[j].Width = this.tlp.ColumnStyles[j].Width * 100f / num2;
					}
				}
			}

			// Token: 0x060018B4 RID: 6324 RVA: 0x00083DDF File Offset: 0x00082DDF
			private void NormalizePercentStyles()
			{
				this.NormalizePercentStyle(true);
				this.NormalizePercentStyle(false);
			}

			// Token: 0x060018B5 RID: 6325 RVA: 0x00083DF0 File Offset: 0x00082DF0
			private void OnOkButtonClick(object sender, EventArgs e)
			{
				if (this.isDialogDirty)
				{
					if (this.absoluteRadioButton.Checked)
					{
						this.UpdateTypeAndValue(SizeType.Absolute, (float)this.absoluteNumericUpDown.Value);
					}
					else if (this.percentRadioButton.Checked)
					{
						this.UpdateTypeAndValue(SizeType.Percent, (float)this.percentNumericUpDown.Value);
					}
					else if (this.autoSizedRadioButton.Checked)
					{
						this.UpdateTypeAndValue(SizeType.AutoSize, 0f);
					}
					this.NormalizePercentStyles();
					if (this.deleteList.Count > 0)
					{
						PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.tlp)["Controls"];
						if (this.compSvc != null && propertyDescriptor != null)
						{
							this.compSvc.OnComponentChanging(this.tlp, propertyDescriptor);
						}
						IDesignerHost designerHost = this.tlp.Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
						if (designerHost != null)
						{
							foreach (object obj in this.deleteList)
							{
								ArrayList arrayList = new ArrayList();
								DesignerUtils.GetAssociatedComponents((IComponent)obj, designerHost, arrayList);
								foreach (object obj2 in arrayList)
								{
									IComponent component = (IComponent)obj2;
									this.compSvc.OnComponentChanging(component, null);
								}
								designerHost.DestroyComponent(obj as Component);
							}
						}
						if (this.compSvc != null && propertyDescriptor != null)
						{
							this.compSvc.OnComponentChanged(this.tlp, propertyDescriptor, null, null);
						}
					}
					if (this.compSvc != null)
					{
						if (this.rowStyleProp != null)
						{
							this.compSvc.OnComponentChanged(this.tlp, this.rowStyleProp, null, null);
						}
						if (this.colStyleProp != null)
						{
							this.compSvc.OnComponentChanged(this.tlp, this.colStyleProp, null, null);
						}
					}
					base.DialogResult = DialogResult.OK;
				}
				else
				{
					base.DialogResult = DialogResult.Cancel;
				}
				this.tlpDesigner.ResumeEnsureAvailableStyles(true);
				this.tlp.ResumeLayout();
			}

			// Token: 0x060018B6 RID: 6326 RVA: 0x00084028 File Offset: 0x00083028
			private void OnCancelButtonClick(object sender, EventArgs e)
			{
				this.tlpDesigner.ResumeEnsureAvailableStyles(false);
				this.tlp.ResumeLayout();
				base.DialogResult = DialogResult.Cancel;
			}

			// Token: 0x04001442 RID: 5186
			private StyleCollectionEditor editor;

			// Token: 0x04001443 RID: 5187
			private bool isRowCollection;

			// Token: 0x04001444 RID: 5188
			private TableLayoutPanel tlp;

			// Token: 0x04001445 RID: 5189
			private TableLayoutPanelDesigner tlpDesigner;

			// Token: 0x04001446 RID: 5190
			private IComponentChangeService compSvc;

			// Token: 0x04001447 RID: 5191
			private ArrayList deleteList;

			// Token: 0x04001448 RID: 5192
			private bool isDialogDirty;

			// Token: 0x04001449 RID: 5193
			private bool haveInvoked;

			// Token: 0x0400144A RID: 5194
			private static int MEMBER_INDEX = 0;

			// Token: 0x0400144B RID: 5195
			private static int TYPE_INDEX = 1;

			// Token: 0x0400144C RID: 5196
			private static int VALUE_INDEX = 2;

			// Token: 0x0400144D RID: 5197
			private PropertyDescriptor rowStyleProp;

			// Token: 0x0400144E RID: 5198
			private PropertyDescriptor colStyleProp;

			// Token: 0x0400144F RID: 5199
			private TableLayoutPanel overarchingTableLayoutPanel;

			// Token: 0x04001450 RID: 5200
			private TableLayoutPanel addRemoveInsertTableLayoutPanel;

			// Token: 0x04001451 RID: 5201
			private Button addButton;

			// Token: 0x04001452 RID: 5202
			private Button removeButton;

			// Token: 0x04001453 RID: 5203
			private Button insertButton;

			// Token: 0x04001454 RID: 5204
			private TableLayoutPanel okCancelTableLayoutPanel;

			// Token: 0x04001455 RID: 5205
			private Button okButton;

			// Token: 0x04001456 RID: 5206
			private Button cancelButton;

			// Token: 0x04001457 RID: 5207
			private Label memberTypeLabel;

			// Token: 0x04001458 RID: 5208
			private ComboBox columnsOrRowsComboBox;

			// Token: 0x04001459 RID: 5209
			private GroupBox sizeTypeGroupBox;

			// Token: 0x0400145A RID: 5210
			private RadioButton absoluteRadioButton;

			// Token: 0x0400145B RID: 5211
			private RadioButton percentRadioButton;

			// Token: 0x0400145C RID: 5212
			private RadioButton autoSizedRadioButton;

			// Token: 0x0400145D RID: 5213
			private StyleCollectionEditor.NavigationalTableLayoutPanel sizeTypeTableLayoutPanel;

			// Token: 0x0400145E RID: 5214
			private Label pixelsLabel;

			// Token: 0x0400145F RID: 5215
			private NumericUpDown absoluteNumericUpDown;

			// Token: 0x04001460 RID: 5216
			private Label percentLabel;

			// Token: 0x04001461 RID: 5217
			private NumericUpDown percentNumericUpDown;

			// Token: 0x04001462 RID: 5218
			private ListView columnsAndRowsListView;

			// Token: 0x04001463 RID: 5219
			private ColumnHeader membersColumnHeader;

			// Token: 0x04001464 RID: 5220
			private ColumnHeader sizeTypeColumnHeader;

			// Token: 0x04001465 RID: 5221
			private TableLayoutPanel helperTextTableLayoutPanel;

			// Token: 0x04001466 RID: 5222
			private PictureBox infoPictureBox1;

			// Token: 0x04001467 RID: 5223
			private PictureBox infoPictureBox2;

			// Token: 0x04001468 RID: 5224
			private LinkLabel helperLinkLabel1;

			// Token: 0x04001469 RID: 5225
			private LinkLabel helperLinkLabel2;

			// Token: 0x0400146A RID: 5226
			private TableLayoutPanel showTableLayoutPanel;

			// Token: 0x0400146B RID: 5227
			private ColumnHeader valueColumnHeader;
		}
	}
}
