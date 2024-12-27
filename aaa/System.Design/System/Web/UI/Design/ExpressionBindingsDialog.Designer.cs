namespace System.Web.UI.Design
{
	// Token: 0x02000366 RID: 870
	internal sealed partial class ExpressionBindingsDialog : global::System.Web.UI.Design.Util.DesignerForm
	{
		// Token: 0x060020BB RID: 8379 RVA: 0x000B7768 File Offset: 0x000B6768
		private void InitializeComponent()
		{
			this._instructionLabel = new global::System.Windows.Forms.Label();
			this._bindablePropsLabels = new global::System.Windows.Forms.Label();
			this._bindablePropsTree = new global::System.Windows.Forms.TreeView();
			this._okButton = new global::System.Windows.Forms.Button();
			this._cancelButton = new global::System.Windows.Forms.Button();
			this._expressionBuilderComboBox = new global::System.Web.UI.Design.Util.AutoSizeComboBox();
			this._expressionBuilderPropertyGrid = new global::System.Windows.Forms.Design.VsPropertyGrid(base.ServiceProvider);
			this._expressionBuilderLabel = new global::System.Windows.Forms.Label();
			this._propertyGridLabel = new global::System.Windows.Forms.Label();
			this._propertiesPanel = new global::System.Windows.Forms.Panel();
			this._generatedHelpLabel = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			this._instructionLabel.Location = new global::System.Drawing.Point(12, 12);
			this._instructionLabel.Name = "_instructionLabel";
			this._instructionLabel.Size = new global::System.Drawing.Size(476, 36);
			this._instructionLabel.TabIndex = 0;
			this._bindablePropsLabels.Location = new global::System.Drawing.Point(12, 52);
			this._bindablePropsLabels.Name = "_bindablePropsLabels";
			this._bindablePropsLabels.Size = new global::System.Drawing.Size(196, 16);
			this._bindablePropsLabels.TabIndex = 1;
			this._bindablePropsTree.HideSelection = false;
			this._bindablePropsTree.ImageIndex = -1;
			this._bindablePropsTree.Location = new global::System.Drawing.Point(12, 70);
			this._bindablePropsTree.Name = "_bindablePropsTree";
			this._bindablePropsTree.SelectedImageIndex = -1;
			this._bindablePropsTree.Sorted = true;
			this._bindablePropsTree.ShowLines = false;
			this._bindablePropsTree.ShowPlusMinus = false;
			this._bindablePropsTree.ShowRootLines = false;
			this._bindablePropsTree.Size = new global::System.Drawing.Size(196, 182);
			this._bindablePropsTree.TabIndex = 2;
			this._bindablePropsTree.AfterSelect += new global::System.Windows.Forms.TreeViewEventHandler(this.OnBindablePropsTreeAfterSelect);
			this._okButton.Location = new global::System.Drawing.Point(312, 262);
			this._okButton.Name = "_okButton";
			this._okButton.TabIndex = 16;
			this._okButton.Size = new global::System.Drawing.Size(85, 23);
			this._okButton.Click += new global::System.EventHandler(this.OnOKButtonClick);
			this._cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new global::System.Drawing.Point(403, 262);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new global::System.Drawing.Size(85, 23);
			this._cancelButton.TabIndex = 17;
			this._expressionBuilderLabel.Location = new global::System.Drawing.Point(0, 0);
			this._expressionBuilderLabel.Name = "_expressionBuilderLabel";
			this._expressionBuilderLabel.Size = new global::System.Drawing.Size(268, 16);
			this._expressionBuilderLabel.TabIndex = 10;
			this._expressionBuilderComboBox.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._expressionBuilderComboBox.Location = new global::System.Drawing.Point(0, 18);
			this._expressionBuilderComboBox.Name = "_expressionBuilderComboBox";
			this._expressionBuilderComboBox.TabIndex = 20;
			this._expressionBuilderComboBox.Size = new global::System.Drawing.Size(268, 21);
			this._expressionBuilderComboBox.Sorted = true;
			this._expressionBuilderComboBox.SelectedIndexChanged += new global::System.EventHandler(this.OnExpressionBuilderComboBoxSelectedIndexChanged);
			this._propertyGridLabel.Location = new global::System.Drawing.Point(0, 43);
			this._propertyGridLabel.Name = "_propertyGridLabel";
			this._propertyGridLabel.Size = new global::System.Drawing.Size(268, 16);
			this._propertyGridLabel.TabIndex = 30;
			this._expressionBuilderPropertyGrid.Location = new global::System.Drawing.Point(0, 61);
			this._expressionBuilderPropertyGrid.Name = "_expressionBuilderPropertyGrid";
			this._expressionBuilderPropertyGrid.TabIndex = 40;
			this._expressionBuilderPropertyGrid.Size = new global::System.Drawing.Size(268, 139);
			this._expressionBuilderPropertyGrid.PropertySort = global::System.Windows.Forms.PropertySort.Alphabetical;
			this._expressionBuilderPropertyGrid.ToolbarVisible = false;
			this._expressionBuilderPropertyGrid.PropertyValueChanged += new global::System.Windows.Forms.PropertyValueChangedEventHandler(this.OnExpressionBuilderPropertyGridPropertyValueChanged);
			this._expressionBuilderPropertyGrid.Site = this._control.Site;
			this._propertiesPanel.Location = new global::System.Drawing.Point(220, 52);
			this._propertiesPanel.Name = "_propertiesPanel";
			this._propertiesPanel.Size = new global::System.Drawing.Size(268, 200);
			this._propertiesPanel.TabIndex = 5;
			this._propertiesPanel.Controls.AddRange(new global::System.Windows.Forms.Control[] { this._expressionBuilderLabel, this._expressionBuilderComboBox, this._propertyGridLabel, this._expressionBuilderPropertyGrid });
			this._generatedHelpLabel.Location = new global::System.Drawing.Point(220, 72);
			this._generatedHelpLabel.Name = "_generatedHelpLabel";
			this._generatedHelpLabel.Size = new global::System.Drawing.Size(268, 180);
			this._generatedHelpLabel.TabIndex = 5;
			base.CancelButton = this._cancelButton;
			base.ClientSize = new global::System.Drawing.Size(500, 297);
			base.Controls.AddRange(new global::System.Windows.Forms.Control[] { this._cancelButton, this._okButton, this._propertiesPanel, this._bindablePropsTree, this._bindablePropsLabels, this._instructionLabel, this._generatedHelpLabel });
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "ExpressionBindingsDialog";
			base.InitializeForm();
			base.ResumeLayout(false);
		}

		// Token: 0x040017F0 RID: 6128
		private global::System.Windows.Forms.Label _instructionLabel;

		// Token: 0x040017F1 RID: 6129
		private global::System.Windows.Forms.Label _bindablePropsLabels;

		// Token: 0x040017F2 RID: 6130
		private global::System.Windows.Forms.TreeView _bindablePropsTree;

		// Token: 0x040017F3 RID: 6131
		private global::System.Windows.Forms.Button _okButton;

		// Token: 0x040017F4 RID: 6132
		private global::System.Windows.Forms.Button _cancelButton;

		// Token: 0x040017F5 RID: 6133
		private global::System.Web.UI.Design.Util.AutoSizeComboBox _expressionBuilderComboBox;

		// Token: 0x040017F6 RID: 6134
		private global::System.Windows.Forms.PropertyGrid _expressionBuilderPropertyGrid;

		// Token: 0x040017F7 RID: 6135
		private global::System.Windows.Forms.Label _expressionBuilderLabel;

		// Token: 0x040017F8 RID: 6136
		private global::System.Windows.Forms.Label _propertyGridLabel;

		// Token: 0x040017F9 RID: 6137
		private global::System.Windows.Forms.Panel _propertiesPanel;

		// Token: 0x040017FA RID: 6138
		private global::System.Windows.Forms.Label _generatedHelpLabel;

		// Token: 0x040017FB RID: 6139
		private global::System.Web.UI.Control _control;
	}
}
