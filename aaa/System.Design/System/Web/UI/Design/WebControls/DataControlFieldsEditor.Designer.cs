namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200041F RID: 1055
	[global::System.Security.Permissions.SecurityPermission(global::System.Security.Permissions.SecurityAction.Demand, Flags = global::System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
	internal sealed partial class DataControlFieldsEditor : global::System.Web.UI.Design.Util.DesignerForm
	{
		// Token: 0x060026B5 RID: 9909 RVA: 0x000D1C48 File Offset: 0x000D0C48
		private void InitializeComponent()
		{
			this._availableFieldsTree = new global::System.Web.UI.Design.WebControls.DataControlFieldsEditor.TreeViewWithEnter();
			this._selFieldsList = new global::System.Web.UI.Design.WebControls.DataControlFieldsEditor.ListViewWithEnter();
			this._okButton = new global::System.Windows.Forms.Button();
			this._cancelButton = new global::System.Windows.Forms.Button();
			this._moveFieldUpButton = new global::System.Windows.Forms.Button();
			this._moveFieldDownButton = new global::System.Windows.Forms.Button();
			this._addFieldButton = new global::System.Windows.Forms.Button();
			this._deleteFieldButton = new global::System.Windows.Forms.Button();
			this._currentFieldProps = new global::System.Windows.Forms.Design.VsPropertyGrid(base.ServiceProvider);
			this._autoFieldCheck = new global::System.Windows.Forms.CheckBox();
			this._refreshSchemaLink = new global::System.Windows.Forms.LinkLabel();
			this._templatizeLink = new global::System.Windows.Forms.LinkLabel();
			this._selFieldLabel = new global::System.Windows.Forms.Label();
			this._availableFieldsLabel = new global::System.Windows.Forms.Label();
			this._selFieldsLabel = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			this._availableFieldsTree.HideSelection = false;
			this._availableFieldsTree.ImageIndex = -1;
			this._availableFieldsTree.Indent = 15;
			this._availableFieldsTree.Location = new global::System.Drawing.Point(12, 28);
			this._availableFieldsTree.Name = "_availableFieldsTree";
			this._availableFieldsTree.SelectedImageIndex = -1;
			this._availableFieldsTree.Size = new global::System.Drawing.Size(196, 116);
			this._availableFieldsTree.TabIndex = 1;
			this._availableFieldsTree.NodeMouseDoubleClick += new global::System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnAvailableFieldsDoubleClick);
			this._availableFieldsTree.AfterSelect += new global::System.Windows.Forms.TreeViewEventHandler(this.OnSelChangedAvailableFields);
			this._availableFieldsTree.GotFocus += new global::System.EventHandler(this.OnAvailableFieldsGotFocus);
			this._availableFieldsTree.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.OnAvailableFieldsKeyPress);
			this._selFieldsList.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left;
			this._selFieldsList.HeaderStyle = global::System.Windows.Forms.ColumnHeaderStyle.None;
			this._selFieldsList.HideSelection = false;
			this._selFieldsList.LabelWrap = false;
			this._selFieldsList.Location = new global::System.Drawing.Point(12, 197);
			this._selFieldsList.MultiSelect = false;
			this._selFieldsList.Name = "_selFieldsList";
			this._selFieldsList.Size = new global::System.Drawing.Size(164, 112);
			this._selFieldsList.TabIndex = 4;
			this._selFieldsList.View = global::System.Windows.Forms.View.Details;
			this._selFieldsList.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.OnSelFieldsListKeyDown);
			this._selFieldsList.SelectedIndexChanged += new global::System.EventHandler(this.OnSelIndexChangedSelFieldsList);
			this._selFieldsList.ItemActivate += new global::System.EventHandler(this.OnClickDeleteField);
			this._selFieldsList.GotFocus += new global::System.EventHandler(this.OnSelFieldsListGotFocus);
			this._okButton.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this._okButton.DialogResult = global::System.Windows.Forms.DialogResult.OK;
			this._okButton.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this._okButton.Location = new global::System.Drawing.Point(340, 350);
			this._okButton.Name = "_okButton";
			this._okButton.TabIndex = 100;
			this._okButton.Click += new global::System.EventHandler(this.OnClickOK);
			this._cancelButton.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this._cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this._cancelButton.Location = new global::System.Drawing.Point(420, 350);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.TabIndex = 101;
			this._moveFieldUpButton.Location = new global::System.Drawing.Point(186, 197);
			this._moveFieldUpButton.Name = "_moveFieldUpButton";
			this._moveFieldUpButton.Size = new global::System.Drawing.Size(26, 23);
			this._moveFieldUpButton.TabIndex = 5;
			this._moveFieldUpButton.Click += new global::System.EventHandler(this.OnClickMoveFieldUp);
			this._moveFieldDownButton.Location = new global::System.Drawing.Point(186, 221);
			this._moveFieldDownButton.Name = "_moveFieldDownButton";
			this._moveFieldDownButton.Size = new global::System.Drawing.Size(26, 23);
			this._moveFieldDownButton.TabIndex = 6;
			this._moveFieldDownButton.Click += new global::System.EventHandler(this.OnClickMoveFieldDown);
			this._addFieldButton.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this._addFieldButton.Location = new global::System.Drawing.Point(123, 150);
			this._addFieldButton.Name = "_addFieldButton";
			this._addFieldButton.Size = new global::System.Drawing.Size(85, 23);
			this._addFieldButton.TabIndex = 2;
			this._addFieldButton.Click += new global::System.EventHandler(this.OnClickAddField);
			this._deleteFieldButton.Location = new global::System.Drawing.Point(186, 245);
			this._deleteFieldButton.Name = "_deleteFieldButton";
			this._deleteFieldButton.Size = new global::System.Drawing.Size(26, 23);
			this._deleteFieldButton.TabIndex = 7;
			this._deleteFieldButton.Click += new global::System.EventHandler(this.OnClickDeleteField);
			this._currentFieldProps.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._currentFieldProps.CommandsVisibleIfAvailable = true;
			this._currentFieldProps.Enabled = false;
			this._currentFieldProps.LargeButtons = false;
			this._currentFieldProps.LineColor = global::System.Drawing.SystemColors.ScrollBar;
			this._currentFieldProps.Location = new global::System.Drawing.Point(244, 28);
			this._currentFieldProps.Name = "_currentFieldProps";
			this._currentFieldProps.Size = new global::System.Drawing.Size(248, 281);
			this._currentFieldProps.TabIndex = 9;
			this._currentFieldProps.ToolbarVisible = true;
			this._currentFieldProps.ViewBackColor = global::System.Drawing.SystemColors.Window;
			this._currentFieldProps.ViewForeColor = global::System.Drawing.SystemColors.WindowText;
			this._currentFieldProps.PropertyValueChanged += new global::System.Windows.Forms.PropertyValueChangedEventHandler(this.OnChangedPropertyValues);
			this._autoFieldCheck.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left;
			this._autoFieldCheck.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this._autoFieldCheck.Location = new global::System.Drawing.Point(12, 313);
			this._autoFieldCheck.Name = "_autoFieldCheck";
			this._autoFieldCheck.Size = new global::System.Drawing.Size(172, 24);
			this._autoFieldCheck.TabIndex = 10;
			this._autoFieldCheck.CheckedChanged += new global::System.EventHandler(this.OnCheckChangedAutoField);
			this._autoFieldCheck.TextAlign = global::System.Drawing.ContentAlignment.TopLeft;
			this._autoFieldCheck.CheckAlign = global::System.Drawing.ContentAlignment.TopLeft;
			this._refreshSchemaLink.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left;
			this._refreshSchemaLink.Location = new global::System.Drawing.Point(12, 347);
			this._refreshSchemaLink.Name = "_refreshSchemaLink";
			this._refreshSchemaLink.Size = new global::System.Drawing.Size(196, 16);
			this._refreshSchemaLink.TabIndex = 11;
			this._refreshSchemaLink.LinkClicked += new global::System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnClickRefreshSchema);
			this._templatizeLink.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left;
			this._templatizeLink.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this._templatizeLink.Location = new global::System.Drawing.Point(244, 313);
			this._templatizeLink.Name = "_templatizeLink";
			this._templatizeLink.Size = new global::System.Drawing.Size(248, 32);
			this._templatizeLink.TabIndex = 12;
			this._templatizeLink.Visible = false;
			this._templatizeLink.LinkClicked += new global::System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnClickTemplatize);
			this._selFieldLabel.Location = new global::System.Drawing.Point(244, 12);
			this._selFieldLabel.Name = "_selFieldLabel";
			this._selFieldLabel.Size = new global::System.Drawing.Size(248, 16);
			this._selFieldLabel.TabIndex = 8;
			this._availableFieldsLabel.Location = new global::System.Drawing.Point(12, 12);
			this._availableFieldsLabel.Name = "_availableFieldsLabel";
			this._availableFieldsLabel.Size = new global::System.Drawing.Size(196, 16);
			this._availableFieldsLabel.TabIndex = 0;
			this._selFieldsLabel.Location = new global::System.Drawing.Point(12, 181);
			this._selFieldsLabel.Name = "_selFieldsLabel";
			this._selFieldsLabel.Size = new global::System.Drawing.Size(196, 16);
			this._selFieldsLabel.TabIndex = 3;
			base.AcceptButton = this._okButton;
			base.CancelButton = this._cancelButton;
			base.ClientSize = new global::System.Drawing.Size(507, 385);
			base.Controls.Add(this._selFieldsLabel);
			base.Controls.Add(this._availableFieldsLabel);
			base.Controls.Add(this._selFieldLabel);
			base.Controls.Add(this._templatizeLink);
			base.Controls.Add(this._refreshSchemaLink);
			base.Controls.Add(this._autoFieldCheck);
			base.Controls.Add(this._currentFieldProps);
			base.Controls.Add(this._deleteFieldButton);
			base.Controls.Add(this._addFieldButton);
			base.Controls.Add(this._moveFieldDownButton);
			base.Controls.Add(this._moveFieldUpButton);
			base.Controls.Add(this._cancelButton);
			base.Controls.Add(this._okButton);
			base.Controls.Add(this._selFieldsList);
			base.Controls.Add(this._availableFieldsTree);
			this.MinimumSize = new global::System.Drawing.Size(515, 419);
			base.Name = "Form1";
			base.InitializeForm();
			base.ResumeLayout(false);
		}

		// Token: 0x04001AA4 RID: 6820
		private global::System.Web.UI.Design.WebControls.DataControlFieldsEditor.TreeViewWithEnter _availableFieldsTree;

		// Token: 0x04001AA5 RID: 6821
		private global::System.Windows.Forms.Button _addFieldButton;

		// Token: 0x04001AA6 RID: 6822
		private global::System.Web.UI.Design.WebControls.DataControlFieldsEditor.ListViewWithEnter _selFieldsList;

		// Token: 0x04001AA7 RID: 6823
		private global::System.Windows.Forms.Button _okButton;

		// Token: 0x04001AA8 RID: 6824
		private global::System.Windows.Forms.Button _cancelButton;

		// Token: 0x04001AA9 RID: 6825
		private global::System.Windows.Forms.Button _moveFieldUpButton;

		// Token: 0x04001AAA RID: 6826
		private global::System.Windows.Forms.Button _moveFieldDownButton;

		// Token: 0x04001AAB RID: 6827
		private global::System.Windows.Forms.Button _deleteFieldButton;

		// Token: 0x04001AAC RID: 6828
		private global::System.Windows.Forms.PropertyGrid _currentFieldProps;

		// Token: 0x04001AAD RID: 6829
		private global::System.Windows.Forms.LinkLabel _refreshSchemaLink;

		// Token: 0x04001AAE RID: 6830
		private global::System.Windows.Forms.LinkLabel _templatizeLink;

		// Token: 0x04001AAF RID: 6831
		private global::System.Windows.Forms.CheckBox _autoFieldCheck;

		// Token: 0x04001AB0 RID: 6832
		private global::System.Windows.Forms.Label _selFieldLabel;

		// Token: 0x04001AB1 RID: 6833
		private global::System.Windows.Forms.Label _availableFieldsLabel;

		// Token: 0x04001AB2 RID: 6834
		private global::System.Windows.Forms.Label _selFieldsLabel;
	}
}
