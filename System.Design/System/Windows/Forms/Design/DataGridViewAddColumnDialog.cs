using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Design;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	internal partial class DataGridViewAddColumnDialog : Form
	{
		public DataGridViewAddColumnDialog(DataGridViewColumnCollection dataGridViewColumns, DataGridView liveDataGridView)
		{
			this.dataGridViewColumns = dataGridViewColumns;
			this.liveDataGridView = liveDataGridView;
			Font font = Control.DefaultFont;
			IUIService iuiservice = (IUIService)this.liveDataGridView.Site.GetService(DataGridViewAddColumnDialog.iUIServiceType);
			if (iuiservice != null)
			{
				font = (Font)iuiservice.Styles["DialogFont"];
			}
			this.Font = font;
			this.InitializeComponent();
			this.EnableDataBoundSection();
		}

		private void AddColumn()
		{
			Type columnType = ((DataGridViewAddColumnDialog.ComboBoxItem)this.columnTypesCombo.SelectedItem).ColumnType;
			DataGridViewColumn dataGridViewColumn = Activator.CreateInstance(columnType) as DataGridViewColumn;
			bool flag = this.dataGridViewColumns.Count > this.insertAtPosition && this.dataGridViewColumns[this.insertAtPosition].Frozen;
			dataGridViewColumn.Frozen = flag;
			if (!this.persistChangesToDesigner)
			{
				dataGridViewColumn.HeaderText = this.headerTextBox.Text;
				dataGridViewColumn.Name = this.nameTextBox.Text;
				dataGridViewColumn.DisplayIndex = -1;
				this.dataGridViewColumns.Insert(this.insertAtPosition, dataGridViewColumn);
				this.insertAtPosition++;
			}
			dataGridViewColumn.HeaderText = this.headerTextBox.Text;
			dataGridViewColumn.Name = this.nameTextBox.Text;
			dataGridViewColumn.Visible = this.visibleCheckBox.Checked;
			dataGridViewColumn.Frozen = this.frozenCheckBox.Checked || flag;
			dataGridViewColumn.ReadOnly = this.readOnlyCheckBox.Checked;
			if (this.dataBoundColumnRadioButton.Checked && this.dataColumns.SelectedIndex > -1)
			{
				dataGridViewColumn.DataPropertyName = ((DataGridViewAddColumnDialog.ListBoxItem)this.dataColumns.SelectedItem).PropertyName;
			}
			if (this.persistChangesToDesigner)
			{
				try
				{
					dataGridViewColumn.DisplayIndex = -1;
					this.dataGridViewColumns.Insert(this.insertAtPosition, dataGridViewColumn);
					this.insertAtPosition++;
					this.liveDataGridView.Site.Container.Add(dataGridViewColumn, dataGridViewColumn.Name);
				}
				catch (InvalidOperationException ex)
				{
					IUIService iuiservice = (IUIService)this.liveDataGridView.Site.GetService(typeof(IUIService));
					DataGridViewDesigner.ShowErrorDialog(iuiservice, ex, this.liveDataGridView);
					return;
				}
			}
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(dataGridViewColumn);
			PropertyDescriptor propertyDescriptor = properties["UserAddedColumn"];
			if (propertyDescriptor != null)
			{
				propertyDescriptor.SetValue(dataGridViewColumn, true);
			}
			this.nameTextBox.Text = (this.headerTextBox.Text = this.AssignName());
			this.nameTextBox.Focus();
		}

		private string AssignName()
		{
			int num = 1;
			string text = "Column" + num.ToString(CultureInfo.InvariantCulture);
			IContainer container = null;
			IDesignerHost designerHost = this.liveDataGridView.Site.GetService(DataGridViewAddColumnDialog.iDesignerHostType) as IDesignerHost;
			if (designerHost != null)
			{
				container = designerHost.Container;
			}
			while (!DataGridViewAddColumnDialog.ValidName(text, this.dataGridViewColumns, container, null, this.liveDataGridView.Columns, !this.persistChangesToDesigner))
			{
				num++;
				text = "Column" + num.ToString(CultureInfo.InvariantCulture);
			}
			return text;
		}

		private void EnableDataBoundSection()
		{
			bool flag = this.dataColumns.Items.Count > 0;
			if (flag)
			{
				this.dataBoundColumnRadioButton.Enabled = true;
				this.dataBoundColumnRadioButton.Checked = true;
				this.dataBoundColumnRadioButton.Focus();
				this.headerTextBox.Text = (this.nameTextBox.Text = this.AssignName());
				return;
			}
			this.dataBoundColumnRadioButton.Enabled = false;
			this.unboundColumnRadioButton.Checked = true;
			this.nameTextBox.Focus();
			this.headerTextBox.Text = (this.nameTextBox.Text = this.AssignName());
		}

		public static ComponentDesigner GetComponentDesignerForType(ITypeResolutionService tr, Type type)
		{
			ComponentDesigner componentDesigner = null;
			DesignerAttribute designerAttribute = null;
			AttributeCollection attributes = TypeDescriptor.GetAttributes(type);
			for (int i = 0; i < attributes.Count; i++)
			{
				DesignerAttribute designerAttribute2 = attributes[i] as DesignerAttribute;
				if (designerAttribute2 != null)
				{
					Type type2 = Type.GetType(designerAttribute2.DesignerBaseTypeName);
					if (type2 != null && type2 == DataGridViewAddColumnDialog.iDesignerType)
					{
						designerAttribute = designerAttribute2;
						break;
					}
				}
			}
			if (designerAttribute != null)
			{
				Type type3;
				if (tr != null)
				{
					type3 = tr.GetType(designerAttribute.DesignerTypeName);
				}
				else
				{
					type3 = Type.GetType(designerAttribute.DesignerTypeName);
				}
				if (type3 != null && typeof(ComponentDesigner).IsAssignableFrom(type3))
				{
					componentDesigner = (ComponentDesigner)Activator.CreateInstance(type3);
				}
			}
			return componentDesigner;
		}

		private void dataBoundColumnRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			this.columnInDataSourceLabel.Enabled = this.dataBoundColumnRadioButton.Checked;
			this.dataColumns.Enabled = this.dataBoundColumnRadioButton.Checked;
			this.dataColumns_SelectedIndexChanged(null, EventArgs.Empty);
		}

		private void dataColumns_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.dataColumns.SelectedItem == null)
			{
				return;
			}
			this.headerTextBox.Text = (this.nameTextBox.Text = ((DataGridViewAddColumnDialog.ListBoxItem)this.dataColumns.SelectedItem).PropertyName);
			this.SetDefaultDataGridViewColumnType(((DataGridViewAddColumnDialog.ListBoxItem)this.dataColumns.SelectedItem).PropertyType);
		}

		private void unboundColumnRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (this.unboundColumnRadioButton.Checked)
			{
				this.nameTextBox.Text = (this.headerTextBox.Text = this.AssignName());
				this.nameTextBox.Focus();
			}
		}

		private void DataGridViewAddColumnDialog_Closed(object sender, EventArgs e)
		{
			if (this.persistChangesToDesigner)
			{
				try
				{
					IComponentChangeService componentChangeService = (IComponentChangeService)this.liveDataGridView.Site.GetService(DataGridViewAddColumnDialog.iComponentChangeServiceType);
					if (componentChangeService == null)
					{
						return;
					}
					DataGridViewColumn[] array = new DataGridViewColumn[this.liveDataGridView.Columns.Count - this.initialDataGridViewColumnsCount];
					for (int i = this.initialDataGridViewColumnsCount; i < this.liveDataGridView.Columns.Count; i++)
					{
						array[i - this.initialDataGridViewColumnsCount] = this.liveDataGridView.Columns[i];
					}
					int j = this.initialDataGridViewColumnsCount;
					while (j < this.liveDataGridView.Columns.Count)
					{
						this.liveDataGridView.Columns.RemoveAt(this.initialDataGridViewColumnsCount);
					}
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.liveDataGridView)["Columns"];
					componentChangeService.OnComponentChanging(this.liveDataGridView, propertyDescriptor);
					for (int k = 0; k < array.Length; k++)
					{
						array[k].DisplayIndex = -1;
					}
					this.liveDataGridView.Columns.AddRange(array);
					componentChangeService.OnComponentChanged(this.liveDataGridView, propertyDescriptor, null, null);
				}
				catch (InvalidOperationException)
				{
				}
			}
			base.DialogResult = DialogResult.OK;
		}

		private void DataGridViewAddColumnDialog_HelpButtonClicked(object sender, CancelEventArgs e)
		{
			this.DataGridViewAddColumnDialog_HelpRequestHandled();
			e.Cancel = true;
		}

		private void DataGridViewAddColumnDialog_HelpRequested(object sender, HelpEventArgs e)
		{
			this.DataGridViewAddColumnDialog_HelpRequestHandled();
			e.Handled = true;
		}

		private void DataGridViewAddColumnDialog_HelpRequestHandled()
		{
			IHelpService helpService = this.liveDataGridView.Site.GetService(DataGridViewAddColumnDialog.iHelpServiceType) as IHelpService;
			if (helpService != null)
			{
				helpService.ShowHelpFromKeyword("vs.DataGridViewAddColumnDialog");
			}
		}

		private void DataGridViewAddColumnDialog_Load(object sender, EventArgs e)
		{
			if (this.dataBoundColumnRadioButton.Checked)
			{
				this.headerTextBox.Text = (this.nameTextBox.Text = this.AssignName());
			}
			else
			{
				string text = this.AssignName();
				this.headerTextBox.Text = (this.nameTextBox.Text = text);
			}
			this.PopulateColumnTypesCombo();
			this.PopulateDataColumns();
			this.EnableDataBoundSection();
			this.cancelButton.Text = SR.GetString("DataGridView_Cancel");
		}

		private void DataGridViewAddColumnDialog_VisibleChanged(object sender, EventArgs e)
		{
			if (base.Visible && base.IsHandleCreated)
			{
				if (this.dataBoundColumnRadioButton.Checked)
				{
					this.dataColumns.Select();
					return;
				}
				this.nameTextBox.Select();
			}
		}

		private void nameTextBox_Validating(object sender, CancelEventArgs e)
		{
			IContainer container = null;
			IDesignerHost designerHost = this.liveDataGridView.Site.GetService(DataGridViewAddColumnDialog.iDesignerHostType) as IDesignerHost;
			if (designerHost != null)
			{
				container = designerHost.Container;
			}
			INameCreationService nameCreationService = this.liveDataGridView.Site.GetService(DataGridViewAddColumnDialog.iNameCreationServiceType) as INameCreationService;
			string empty = string.Empty;
			if (!DataGridViewAddColumnDialog.ValidName(this.nameTextBox.Text, this.dataGridViewColumns, container, nameCreationService, this.liveDataGridView.Columns, !this.persistChangesToDesigner, out empty))
			{
				IUIService iuiservice = (IUIService)this.liveDataGridView.Site.GetService(DataGridViewAddColumnDialog.iUIServiceType);
				DataGridViewDesigner.ShowErrorDialog(iuiservice, empty, this.liveDataGridView);
				e.Cancel = true;
			}
		}

		private void PopulateColumnTypesCombo()
		{
			this.columnTypesCombo.Items.Clear();
			IDesignerHost designerHost = (IDesignerHost)this.liveDataGridView.Site.GetService(DataGridViewAddColumnDialog.iDesignerHostType);
			if (designerHost == null)
			{
				return;
			}
			ITypeDiscoveryService typeDiscoveryService = (ITypeDiscoveryService)designerHost.GetService(DataGridViewAddColumnDialog.iTypeDiscoveryServiceType);
			if (typeDiscoveryService == null)
			{
				return;
			}
			ICollection collection = DesignerUtils.FilterGenericTypes(typeDiscoveryService.GetTypes(DataGridViewAddColumnDialog.dataGridViewColumnType, false));
			foreach (object obj in collection)
			{
				Type type = (Type)obj;
				if (type != DataGridViewAddColumnDialog.dataGridViewColumnType && !type.IsAbstract && (type.IsPublic || type.IsNestedPublic))
				{
					DataGridViewColumnDesignTimeVisibleAttribute dataGridViewColumnDesignTimeVisibleAttribute = TypeDescriptor.GetAttributes(type)[DataGridViewAddColumnDialog.dataGridViewColumnDesignTimeVisibleAttributeType] as DataGridViewColumnDesignTimeVisibleAttribute;
					if (dataGridViewColumnDesignTimeVisibleAttribute == null || dataGridViewColumnDesignTimeVisibleAttribute.Visible)
					{
						this.columnTypesCombo.Items.Add(new DataGridViewAddColumnDialog.ComboBoxItem(type));
					}
				}
			}
			this.columnTypesCombo.SelectedIndex = this.TypeToSelectedIndex(typeof(DataGridViewTextBoxColumn));
		}

		private void PopulateDataColumns()
		{
			int selectedIndex = this.dataColumns.SelectedIndex;
			this.dataColumns.SelectedIndex = -1;
			this.dataColumns.Items.Clear();
			if (this.liveDataGridView.DataSource != null)
			{
				CurrencyManager currencyManager = null;
				try
				{
					currencyManager = this.BindingContext[this.liveDataGridView.DataSource, this.liveDataGridView.DataMember] as CurrencyManager;
				}
				catch (ArgumentException)
				{
					currencyManager = null;
				}
				PropertyDescriptorCollection propertyDescriptorCollection = ((currencyManager != null) ? currencyManager.GetItemProperties() : null);
				if (propertyDescriptorCollection != null)
				{
					int i = 0;
					while (i < propertyDescriptorCollection.Count)
					{
						if (!typeof(IList).IsAssignableFrom(propertyDescriptorCollection[i].PropertyType))
						{
							goto IL_00C2;
						}
						TypeConverter converter = TypeDescriptor.GetConverter(typeof(Image));
						if (converter.CanConvertFrom(propertyDescriptorCollection[i].PropertyType))
						{
							goto IL_00C2;
						}
						IL_00F0:
						i++;
						continue;
						IL_00C2:
						this.dataColumns.Items.Add(new DataGridViewAddColumnDialog.ListBoxItem(propertyDescriptorCollection[i].PropertyType, propertyDescriptorCollection[i].Name));
						goto IL_00F0;
					}
				}
			}
			if (selectedIndex != -1 && selectedIndex < this.dataColumns.Items.Count)
			{
				this.dataColumns.SelectedIndex = selectedIndex;
				return;
			}
			this.dataColumns.SelectedIndex = ((this.dataColumns.Items.Count > 0) ? 0 : (-1));
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			this.cancelButton.Text = SR.GetString("DataGridView_Close");
			this.AddColumn();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & Keys.Modifiers) == Keys.None)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys == Keys.Return)
				{
					IContainer container = null;
					IDesignerHost designerHost = this.liveDataGridView.Site.GetService(DataGridViewAddColumnDialog.iDesignerHostType) as IDesignerHost;
					if (designerHost != null)
					{
						container = designerHost.Container;
					}
					INameCreationService nameCreationService = this.liveDataGridView.Site.GetService(DataGridViewAddColumnDialog.iNameCreationServiceType) as INameCreationService;
					string empty = string.Empty;
					if (DataGridViewAddColumnDialog.ValidName(this.nameTextBox.Text, this.dataGridViewColumns, container, nameCreationService, this.liveDataGridView.Columns, !this.persistChangesToDesigner, out empty))
					{
						this.AddColumn();
						base.Close();
					}
					else
					{
						IUIService iuiservice = (IUIService)this.liveDataGridView.Site.GetService(DataGridViewAddColumnDialog.iUIServiceType);
						DataGridViewDesigner.ShowErrorDialog(iuiservice, empty, this.liveDataGridView);
					}
					return true;
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		internal void Start(int insertAtPosition, bool persistChangesToDesigner)
		{
			this.insertAtPosition = insertAtPosition;
			this.persistChangesToDesigner = persistChangesToDesigner;
			if (this.persistChangesToDesigner)
			{
				this.initialDataGridViewColumnsCount = this.liveDataGridView.Columns.Count;
				return;
			}
			this.initialDataGridViewColumnsCount = -1;
		}

		private void SetDefaultDataGridViewColumnType(Type type)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(Image));
			if (type == typeof(bool) || type == typeof(CheckState))
			{
				this.columnTypesCombo.SelectedIndex = this.TypeToSelectedIndex(typeof(DataGridViewCheckBoxColumn));
				return;
			}
			if (typeof(Image).IsAssignableFrom(type) || converter.CanConvertFrom(type))
			{
				this.columnTypesCombo.SelectedIndex = this.TypeToSelectedIndex(typeof(DataGridViewImageColumn));
				return;
			}
			this.columnTypesCombo.SelectedIndex = this.TypeToSelectedIndex(typeof(DataGridViewTextBoxColumn));
		}

		private int TypeToSelectedIndex(Type type)
		{
			for (int i = 0; i < this.columnTypesCombo.Items.Count; i++)
			{
				if (type == ((DataGridViewAddColumnDialog.ComboBoxItem)this.columnTypesCombo.Items[i]).ColumnType)
				{
					return i;
				}
			}
			return -1;
		}

		public static bool ValidName(string name, DataGridViewColumnCollection columns, IContainer container, INameCreationService nameCreationService, DataGridViewColumnCollection liveColumns, bool allowDuplicateNameInLiveColumnCollection)
		{
			return !columns.Contains(name) && (container == null || container.Components[name] == null || (allowDuplicateNameInLiveColumnCollection && liveColumns != null && liveColumns.Contains(name))) && (nameCreationService == null || nameCreationService.IsValidName(name) || (allowDuplicateNameInLiveColumnCollection && liveColumns != null && liveColumns.Contains(name)));
		}

		public static bool ValidName(string name, DataGridViewColumnCollection columns, IContainer container, INameCreationService nameCreationService, DataGridViewColumnCollection liveColumns, bool allowDuplicateNameInLiveColumnCollection, out string errorString)
		{
			if (columns.Contains(name))
			{
				errorString = SR.GetString("DataGridViewDuplicateColumnName", new object[] { name });
				return false;
			}
			if (container != null && container.Components[name] != null && (!allowDuplicateNameInLiveColumnCollection || liveColumns == null || !liveColumns.Contains(name)))
			{
				errorString = SR.GetString("DesignerHostDuplicateName", new object[] { name });
				return false;
			}
			if (nameCreationService != null && !nameCreationService.IsValidName(name) && (!allowDuplicateNameInLiveColumnCollection || liveColumns == null || !liveColumns.Contains(name)))
			{
				errorString = SR.GetString("CodeDomDesignerLoaderInvalidIdentifier", new object[] { name });
				return false;
			}
			errorString = string.Empty;
			return true;
		}

		private DataGridViewColumnCollection dataGridViewColumns;

		private DataGridView liveDataGridView;

		private int insertAtPosition = -1;

		private int initialDataGridViewColumnsCount = -1;

		private bool persistChangesToDesigner;

		private static Type dataGridViewColumnType = typeof(DataGridViewColumn);

		private static Type iDesignerType = typeof(IDesigner);

		private static Type iTypeResolutionServiceType = typeof(ITypeResolutionService);

		private static Type iTypeDiscoveryServiceType = typeof(ITypeDiscoveryService);

		private static Type iComponentChangeServiceType = typeof(IComponentChangeService);

		private static Type iHelpServiceType = typeof(IHelpService);

		private static Type iUIServiceType = typeof(IUIService);

		private static Type iDesignerHostType = typeof(IDesignerHost);

		private static Type iNameCreationServiceType = typeof(INameCreationService);

		private static Type dataGridViewColumnDesignTimeVisibleAttributeType = typeof(DataGridViewColumnDesignTimeVisibleAttribute);

		private static Type[] columnTypes = new Type[]
		{
			typeof(DataGridViewButtonColumn),
			typeof(DataGridViewCheckBoxColumn),
			typeof(DataGridViewComboBoxColumn),
			typeof(DataGridViewImageColumn),
			typeof(DataGridViewLinkColumn),
			typeof(DataGridViewTextBoxColumn)
		};

		private class ListBoxItem
		{
			public ListBoxItem(Type propertyType, string propertyName)
			{
				this.propertyType = propertyType;
				this.propertyName = propertyName;
			}

			public Type PropertyType
			{
				get
				{
					return this.propertyType;
				}
			}

			public string PropertyName
			{
				get
				{
					return this.propertyName;
				}
			}

			public override string ToString()
			{
				return this.propertyName;
			}

			private Type propertyType;

			private string propertyName;
		}

		private class ComboBoxItem
		{
			public ComboBoxItem(Type columnType)
			{
				this.columnType = columnType;
			}

			public override string ToString()
			{
				return this.columnType.Name;
			}

			public Type ColumnType
			{
				get
				{
					return this.columnType;
				}
			}

			private Type columnType;
		}
	}
}
