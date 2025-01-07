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
	internal sealed class ObjectDataSourceChooseTypePanel : WizardPanel
	{
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

		private void InitializeUI()
		{
			base.Caption = SR.GetString("ObjectDataSourceChooseTypePanel_PanelCaption");
			this._helpLabel.Text = SR.GetString("ObjectDataSourceChooseTypePanel_HelpLabel");
			this._nameLabel.Text = SR.GetString("ObjectDataSourceChooseTypePanel_NameLabel");
			this._exampleLabel.Text = SR.GetString("ObjectDataSourceChooseTypePanel_ExampleLabel");
			this._filterCheckBox.Text = SR.GetString("ObjectDataSourceChooseTypePanel_FilterCheckBox");
		}

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

		private void OnFilterCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			this.UpdateTypeList();
		}

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

		public override void OnPrevious()
		{
		}

		private void OnTypeNameComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			this.UpdateEnabledState();
		}

		private void OnTypeNameTextBoxTextChanged(object sender, EventArgs e)
		{
			this.UpdateEnabledState();
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (base.Visible)
			{
				this.UpdateEnabledState();
			}
		}

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

		private const string CompareAllValuesFormatString = "original_{0}";

		private global::System.Windows.Forms.TextBox _typeNameTextBox;

		private global::System.Windows.Forms.CheckBox _filterCheckBox;

		private global::System.Windows.Forms.Label _helpLabel;

		private global::System.Windows.Forms.Label _nameLabel;

		private global::System.Windows.Forms.Label _exampleLabel;

		private AutoSizeComboBox _typeNameComboBox;

		private ObjectDataSource _objectDataSource;

		private ObjectDataSourceDesigner _objectDataSourceDesigner;

		private Type _previousSelectedType;

		private bool _discoveryServiceMode;

		private List<ObjectDataSourceChooseTypePanel.TypeItem> _typeItems;

		private sealed class TypeItem
		{
			public TypeItem(string typeName, bool filtered)
			{
				this._typeName = typeName;
				this._prettyTypeName = this._typeName;
				this._type = null;
				this._filtered = filtered;
			}

			public TypeItem(Type type, bool filtered)
			{
				StringBuilder stringBuilder = new StringBuilder(64);
				ObjectDataSourceMethodEditor.AppendTypeName(type, true, stringBuilder);
				this._prettyTypeName = stringBuilder.ToString();
				this._typeName = type.FullName;
				this._type = type;
				this._filtered = filtered;
			}

			public bool Filtered
			{
				get
				{
					return this._filtered;
				}
			}

			public string TypeName
			{
				get
				{
					return this._typeName;
				}
			}

			public Type Type
			{
				get
				{
					return this._type;
				}
			}

			public override string ToString()
			{
				return this._prettyTypeName;
			}

			private string _prettyTypeName;

			private string _typeName;

			private Type _type;

			private bool _filtered;
		}
	}
}
