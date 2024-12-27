using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Text;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000483 RID: 1155
	internal sealed class ObjectDataSourceChooseTypePanel : WizardPanel
	{
		// Token: 0x060029E4 RID: 10724 RVA: 0x000E5FF8 File Offset: 0x000E4FF8
		public ObjectDataSourceChooseTypePanel(ObjectDataSourceDesigner objectDataSourceDesigner)
		{
			this._objectDataSourceDesigner = objectDataSourceDesigner;
			this._objectDataSource = (ObjectDataSource)this._objectDataSourceDesigner.Component;
			this.InitializeComponent();
			this.InitializeUI();
			ITypeDiscoveryService typeDiscoveryService = null;
			if (this._objectDataSource.Site != null)
			{
				typeDiscoveryService = (ITypeDiscoveryService)this._objectDataSource.Site.GetService(typeof(ITypeDiscoveryService));
			}
			this._discoveryServiceMode = typeDiscoveryService != null;
			if (this._discoveryServiceMode)
			{
				this._typeNameTextBox.Visible = false;
				this._exampleLabel.Visible = false;
				Cursor cursor = Cursor.Current;
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					ICollection collection = DesignerUtils.FilterGenericTypes(typeDiscoveryService.GetTypes(typeof(object), true));
					this._typeNameComboBox.BeginUpdate();
					if (collection != null)
					{
						StringCollection stringCollection = new StringCollection();
						stringCollection.Add("My.MyApplication");
						stringCollection.Add("My.MyComputer");
						stringCollection.Add("My.MyProject");
						stringCollection.Add("My.MyUser");
						this._typeItems = new List<ObjectDataSourceChooseTypePanel.TypeItem>(collection.Count);
						bool flag = false;
						foreach (object obj in collection)
						{
							Type type = (Type)obj;
							if (!type.IsEnum && !type.IsInterface)
							{
								object[] customAttributes = type.GetCustomAttributes(typeof(DataObjectAttribute), true);
								if (customAttributes.Length > 0 && ((DataObjectAttribute)customAttributes[0]).IsDataObject)
								{
									this._typeItems.Add(new ObjectDataSourceChooseTypePanel.TypeItem(type, true));
									flag = true;
								}
								else if (!stringCollection.Contains(type.FullName))
								{
									this._typeItems.Add(new ObjectDataSourceChooseTypePanel.TypeItem(type, false));
								}
							}
						}
						object showOnlyDataComponentsState = this._objectDataSourceDesigner.ShowOnlyDataComponentsState;
						if (showOnlyDataComponentsState == null)
						{
							this._filterCheckBox.Checked = flag;
						}
						else
						{
							this._filterCheckBox.Checked = (bool)showOnlyDataComponentsState;
						}
						this.UpdateTypeList();
					}
					goto IL_0229;
				}
				finally
				{
					this._typeNameComboBox.EndUpdate();
					Cursor.Current = cursor;
				}
			}
			this._typeNameComboBox.Visible = false;
			this._filterCheckBox.Visible = false;
			IL_0229:
			this.TypeName = this._objectDataSource.TypeName;
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x060029E5 RID: 10725 RVA: 0x000E6274 File Offset: 0x000E5274
		// (set) Token: 0x060029E6 RID: 10726 RVA: 0x000E6298 File Offset: 0x000E5298
		private string TypeName
		{
			get
			{
				ObjectDataSourceChooseTypePanel.TypeItem selectedTypeItem = this.SelectedTypeItem;
				if (selectedTypeItem != null)
				{
					return selectedTypeItem.TypeName;
				}
				return string.Empty;
			}
			set
			{
				if (this._discoveryServiceMode)
				{
					foreach (object obj in this._typeNameComboBox.Items)
					{
						ObjectDataSourceChooseTypePanel.TypeItem typeItem = (ObjectDataSourceChooseTypePanel.TypeItem)obj;
						if (string.Compare(typeItem.TypeName, value, StringComparison.OrdinalIgnoreCase) == 0)
						{
							this._typeNameComboBox.SelectedItem = typeItem;
							break;
						}
					}
					if (this._typeNameComboBox.SelectedItem == null && value.Length > 0)
					{
						ObjectDataSourceChooseTypePanel.TypeItem typeItem2 = new ObjectDataSourceChooseTypePanel.TypeItem(value, true);
						this._typeItems.Add(typeItem2);
						this.UpdateTypeList();
						this._typeNameComboBox.SelectedItem = typeItem2;
						return;
					}
				}
				else
				{
					this._typeNameTextBox.Text = value;
				}
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x060029E7 RID: 10727 RVA: 0x000E6360 File Offset: 0x000E5360
		private ObjectDataSourceChooseTypePanel.TypeItem SelectedTypeItem
		{
			get
			{
				if (this._discoveryServiceMode)
				{
					return this._typeNameComboBox.SelectedItem as ObjectDataSourceChooseTypePanel.TypeItem;
				}
				return new ObjectDataSourceChooseTypePanel.TypeItem(this._typeNameTextBox.Text, false);
			}
		}

		// Token: 0x060029E8 RID: 10728 RVA: 0x000E638C File Offset: 0x000E538C
		private void InitializeComponent()
		{
			this._helpLabel = new global::System.Windows.Forms.Label();
			this._nameLabel = new global::System.Windows.Forms.Label();
			this._exampleLabel = new global::System.Windows.Forms.Label();
			this._typeNameTextBox = new global::System.Windows.Forms.TextBox();
			this._typeNameComboBox = new AutoSizeComboBox();
			this._filterCheckBox = new global::System.Windows.Forms.CheckBox();
			base.SuspendLayout();
			this._helpLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._helpLabel.Location = new Point(0, 0);
			this._helpLabel.Name = "_helpLabel";
			this._helpLabel.Size = new Size(544, 60);
			this._helpLabel.TabIndex = 10;
			this._nameLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._nameLabel.Location = new Point(0, 68);
			this._nameLabel.Name = "_nameLabel";
			this._nameLabel.Size = new Size(544, 16);
			this._nameLabel.TabIndex = 20;
			this._typeNameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._typeNameTextBox.Location = new Point(0, 86);
			this._typeNameTextBox.Name = "_typeNameTextBox";
			this._typeNameTextBox.Size = new Size(300, 20);
			this._typeNameTextBox.TabIndex = 30;
			this._typeNameTextBox.TextChanged += this.OnTypeNameTextBoxTextChanged;
			this._typeNameComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._typeNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this._typeNameComboBox.Location = new Point(0, 86);
			this._typeNameComboBox.Name = "_typeNameComboBox";
			this._typeNameComboBox.Size = new Size(300, 21);
			this._typeNameComboBox.Sorted = true;
			this._typeNameComboBox.TabIndex = 30;
			this._typeNameComboBox.SelectedIndexChanged += this.OnTypeNameComboBoxSelectedIndexChanged;
			this._filterCheckBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this._filterCheckBox.Location = new Point(306, 86);
			this._filterCheckBox.Name = "_filterCheckBox";
			this._filterCheckBox.Size = new Size(200, 18);
			this._filterCheckBox.TabIndex = 50;
			this._filterCheckBox.CheckedChanged += this.OnFilterCheckBoxCheckedChanged;
			this._exampleLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._exampleLabel.ForeColor = SystemColors.GrayText;
			this._exampleLabel.Location = new Point(0, 122);
			this._exampleLabel.Name = "_exampleLabel";
			this._exampleLabel.Size = new Size(544, 16);
			this._exampleLabel.TabIndex = 60;
			base.Controls.Add(this._filterCheckBox);
			base.Controls.Add(this._typeNameComboBox);
			base.Controls.Add(this._typeNameTextBox);
			base.Controls.Add(this._exampleLabel);
			base.Controls.Add(this._nameLabel);
			base.Controls.Add(this._helpLabel);
			base.Name = "ObjectDataSourceChooseTypePanel";
			base.Size = new Size(544, 274);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x060029E9 RID: 10729 RVA: 0x000E66DC File Offset: 0x000E56DC
		private void InitializeUI()
		{
			base.Caption = SR.GetString("ObjectDataSourceChooseTypePanel_PanelCaption");
			this._helpLabel.Text = SR.GetString("ObjectDataSourceChooseTypePanel_HelpLabel");
			this._nameLabel.Text = SR.GetString("ObjectDataSourceChooseTypePanel_NameLabel");
			this._exampleLabel.Text = SR.GetString("ObjectDataSourceChooseTypePanel_ExampleLabel");
			this._filterCheckBox.Text = SR.GetString("ObjectDataSourceChooseTypePanel_FilterCheckBox");
		}

		// Token: 0x060029EA RID: 10730 RVA: 0x000E6750 File Offset: 0x000E5750
		protected internal override void OnComplete()
		{
			if (this._objectDataSource.TypeName != this.TypeName)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._objectDataSource)["TypeName"];
				propertyDescriptor.SetValue(this._objectDataSource, this.TypeName);
			}
			if (this.SelectedTypeItem != null && this.SelectedTypeItem.Filtered)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._objectDataSource)["OldValuesParameterFormatString"];
				propertyDescriptor.SetValue(this._objectDataSource, "original_{0}");
			}
			this._objectDataSourceDesigner.ShowOnlyDataComponentsState = this._filterCheckBox.Checked;
		}

		// Token: 0x060029EB RID: 10731 RVA: 0x000E67F4 File Offset: 0x000E57F4
		private void OnFilterCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			this.UpdateTypeList();
		}

		// Token: 0x060029EC RID: 10732 RVA: 0x000E67FC File Offset: 0x000E57FC
		public override bool OnNext()
		{
			ObjectDataSourceChooseTypePanel.TypeItem selectedTypeItem = this.SelectedTypeItem;
			Type type = selectedTypeItem.Type;
			if (type == null)
			{
				ITypeResolutionService typeResolutionService = (ITypeResolutionService)base.ServiceProvider.GetService(typeof(ITypeResolutionService));
				if (typeResolutionService == null)
				{
					return false;
				}
				try
				{
					type = typeResolutionService.GetType(selectedTypeItem.TypeName, true, true);
				}
				catch (Exception ex)
				{
					UIServiceHelper.ShowError(base.ServiceProvider, ex, SR.GetString("ObjectDataSourceDesigner_CannotGetType", new object[] { selectedTypeItem.TypeName }));
					return false;
				}
			}
			if (type == null)
			{
				return false;
			}
			if (type != this._previousSelectedType)
			{
				ObjectDataSourceChooseMethodsPanel objectDataSourceChooseMethodsPanel = base.NextPanel as ObjectDataSourceChooseMethodsPanel;
				objectDataSourceChooseMethodsPanel.SetType(type);
				this._previousSelectedType = type;
			}
			return true;
		}

		// Token: 0x060029ED RID: 10733 RVA: 0x000E68BC File Offset: 0x000E58BC
		public override void OnPrevious()
		{
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x000E68BE File Offset: 0x000E58BE
		private void OnTypeNameComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			this.UpdateEnabledState();
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x000E68C6 File Offset: 0x000E58C6
		private void OnTypeNameTextBoxTextChanged(object sender, EventArgs e)
		{
			this.UpdateEnabledState();
		}

		// Token: 0x060029F0 RID: 10736 RVA: 0x000E68CE File Offset: 0x000E58CE
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (base.Visible)
			{
				this.UpdateEnabledState();
			}
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x000E68E8 File Offset: 0x000E58E8
		private void UpdateEnabledState()
		{
			if (base.ParentWizard != null)
			{
				base.ParentWizard.FinishButton.Enabled = false;
				if (this._discoveryServiceMode)
				{
					base.ParentWizard.NextButton.Enabled = this._typeNameComboBox.SelectedItem != null;
					return;
				}
				base.ParentWizard.NextButton.Enabled = this._typeNameTextBox.Text.Length > 0;
			}
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x000E695C File Offset: 0x000E595C
		private void UpdateTypeList()
		{
			object selectedItem = this._typeNameComboBox.SelectedItem;
			try
			{
				this._typeNameComboBox.BeginUpdate();
				this._typeNameComboBox.Items.Clear();
				bool @checked = this._filterCheckBox.Checked;
				foreach (ObjectDataSourceChooseTypePanel.TypeItem typeItem in this._typeItems)
				{
					if (@checked)
					{
						if (typeItem.Filtered)
						{
							this._typeNameComboBox.Items.Add(typeItem);
						}
					}
					else
					{
						this._typeNameComboBox.Items.Add(typeItem);
					}
				}
			}
			finally
			{
				this._typeNameComboBox.EndUpdate();
			}
			this._typeNameComboBox.SelectedItem = selectedItem;
			this.UpdateEnabledState();
			this._typeNameComboBox.InvalidateDropDownWidth();
		}

		// Token: 0x04001CB1 RID: 7345
		private const string CompareAllValuesFormatString = "original_{0}";

		// Token: 0x04001CB2 RID: 7346
		private global::System.Windows.Forms.TextBox _typeNameTextBox;

		// Token: 0x04001CB3 RID: 7347
		private global::System.Windows.Forms.CheckBox _filterCheckBox;

		// Token: 0x04001CB4 RID: 7348
		private global::System.Windows.Forms.Label _helpLabel;

		// Token: 0x04001CB5 RID: 7349
		private global::System.Windows.Forms.Label _nameLabel;

		// Token: 0x04001CB6 RID: 7350
		private global::System.Windows.Forms.Label _exampleLabel;

		// Token: 0x04001CB7 RID: 7351
		private AutoSizeComboBox _typeNameComboBox;

		// Token: 0x04001CB8 RID: 7352
		private ObjectDataSource _objectDataSource;

		// Token: 0x04001CB9 RID: 7353
		private ObjectDataSourceDesigner _objectDataSourceDesigner;

		// Token: 0x04001CBA RID: 7354
		private Type _previousSelectedType;

		// Token: 0x04001CBB RID: 7355
		private bool _discoveryServiceMode;

		// Token: 0x04001CBC RID: 7356
		private List<ObjectDataSourceChooseTypePanel.TypeItem> _typeItems;

		// Token: 0x02000484 RID: 1156
		private sealed class TypeItem
		{
			// Token: 0x060029F3 RID: 10739 RVA: 0x000E6A44 File Offset: 0x000E5A44
			public TypeItem(string typeName, bool filtered)
			{
				this._typeName = typeName;
				this._prettyTypeName = this._typeName;
				this._type = null;
				this._filtered = filtered;
			}

			// Token: 0x060029F4 RID: 10740 RVA: 0x000E6A70 File Offset: 0x000E5A70
			public TypeItem(Type type, bool filtered)
			{
				StringBuilder stringBuilder = new StringBuilder(64);
				ObjectDataSourceMethodEditor.AppendTypeName(type, true, stringBuilder);
				this._prettyTypeName = stringBuilder.ToString();
				this._typeName = type.FullName;
				this._type = type;
				this._filtered = filtered;
			}

			// Token: 0x170007CF RID: 1999
			// (get) Token: 0x060029F5 RID: 10741 RVA: 0x000E6AB9 File Offset: 0x000E5AB9
			public bool Filtered
			{
				get
				{
					return this._filtered;
				}
			}

			// Token: 0x170007D0 RID: 2000
			// (get) Token: 0x060029F6 RID: 10742 RVA: 0x000E6AC1 File Offset: 0x000E5AC1
			public string TypeName
			{
				get
				{
					return this._typeName;
				}
			}

			// Token: 0x170007D1 RID: 2001
			// (get) Token: 0x060029F7 RID: 10743 RVA: 0x000E6AC9 File Offset: 0x000E5AC9
			public Type Type
			{
				get
				{
					return this._type;
				}
			}

			// Token: 0x060029F8 RID: 10744 RVA: 0x000E6AD1 File Offset: 0x000E5AD1
			public override string ToString()
			{
				return this._prettyTypeName;
			}

			// Token: 0x04001CBD RID: 7357
			private string _prettyTypeName;

			// Token: 0x04001CBE RID: 7358
			private string _typeName;

			// Token: 0x04001CBF RID: 7359
			private Type _type;

			// Token: 0x04001CC0 RID: 7360
			private bool _filtered;
		}
	}
}
