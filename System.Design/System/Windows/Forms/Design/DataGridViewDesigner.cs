using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	internal class DataGridViewDesigner : ControlDesigner
	{
		public DataGridViewDesigner()
		{
			base.AutoResizeHandles = true;
		}

		public override ICollection AssociatedComponents
		{
			get
			{
				DataGridView dataGridView = base.Component as DataGridView;
				if (dataGridView != null)
				{
					return dataGridView.Columns;
				}
				return base.AssociatedComponents;
			}
		}

		public DataGridViewAutoSizeColumnsMode AutoSizeColumnsMode
		{
			get
			{
				DataGridView dataGridView = base.Component as DataGridView;
				return dataGridView.AutoSizeColumnsMode;
			}
			set
			{
				DataGridView dataGridView = base.Component as DataGridView;
				IComponentChangeService componentChangeService = base.Component.Site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(DataGridViewColumn))["Width"];
				for (int i = 0; i < dataGridView.Columns.Count; i++)
				{
					componentChangeService.OnComponentChanging(dataGridView.Columns[i], propertyDescriptor);
				}
				dataGridView.AutoSizeColumnsMode = value;
				for (int j = 0; j < dataGridView.Columns.Count; j++)
				{
					componentChangeService.OnComponentChanged(dataGridView.Columns[j], propertyDescriptor, null, null);
				}
			}
		}

		public object DataSource
		{
			get
			{
				return ((DataGridView)base.Component).DataSource;
			}
			set
			{
				DataGridView dataGridView = base.Component as DataGridView;
				if (dataGridView.AutoGenerateColumns && dataGridView.DataSource == null && value != null)
				{
					dataGridView.AutoGenerateColumns = false;
				}
				((DataGridView)base.Component).DataSource = value;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				DataGridView dataGridView = base.Component as DataGridView;
				dataGridView.DataSourceChanged -= this.dataGridViewChanged;
				dataGridView.DataMemberChanged -= this.dataGridViewChanged;
				dataGridView.BindingContextChanged -= this.dataGridViewChanged;
				dataGridView.ColumnRemoved -= this.dataGridView_ColumnRemoved;
				if (this.cm != null)
				{
					this.cm.MetaDataChanged -= this.dataGridViewMetaDataChanged;
				}
				this.cm = null;
				if (base.Component.Site != null)
				{
					IComponentChangeService componentChangeService = base.Component.Site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
					if (componentChangeService != null)
					{
						componentChangeService.ComponentRemoving -= this.DataGridViewDesigner_ComponentRemoving;
					}
				}
			}
			base.Dispose(disposing);
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			if (component.Site != null)
			{
				IComponentChangeService componentChangeService = component.Site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (componentChangeService != null)
				{
					componentChangeService.ComponentRemoving += this.DataGridViewDesigner_ComponentRemoving;
				}
			}
			DataGridView dataGridView = (DataGridView)component;
			dataGridView.AutoGenerateColumns = dataGridView.DataSource == null;
			dataGridView.DataSourceChanged += this.dataGridViewChanged;
			dataGridView.DataMemberChanged += this.dataGridViewChanged;
			dataGridView.BindingContextChanged += this.dataGridViewChanged;
			this.dataGridViewChanged(base.Component, EventArgs.Empty);
			dataGridView.ColumnRemoved += this.dataGridView_ColumnRemoved;
		}

		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			((DataGridView)base.Component).ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		}

		protected override InheritanceAttribute InheritanceAttribute
		{
			get
			{
				if (base.InheritanceAttribute == InheritanceAttribute.Inherited || base.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
				{
					return InheritanceAttribute.InheritedReadOnly;
				}
				return base.InheritanceAttribute;
			}
		}

		public override DesignerVerbCollection Verbs
		{
			get
			{
				if (this.designerVerbs == null)
				{
					this.designerVerbs = new DesignerVerbCollection();
					this.designerVerbs.Add(new DesignerVerb(SR.GetString("DataGridViewEditColumnsVerb"), new EventHandler(this.OnEditColumns)));
					this.designerVerbs.Add(new DesignerVerb(SR.GetString("DataGridViewAddColumnVerb"), new EventHandler(this.OnAddColumn)));
				}
				return this.designerVerbs;
			}
		}

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (this.actionLists == null)
				{
					this.BuildActionLists();
				}
				return this.actionLists;
			}
		}

		private void BuildActionLists()
		{
			this.actionLists = new DesignerActionListCollection();
			this.actionLists.Add(new DataGridViewDesigner.DataGridViewChooseDataSourceActionList(this));
			this.actionLists.Add(new DataGridViewDesigner.DataGridViewColumnEditingActionList(this));
			this.actionLists.Add(new DataGridViewDesigner.DataGridViewPropertiesActionList(this));
			this.actionLists[0].AutoShow = true;
		}

		private void dataGridViewChanged(object sender, EventArgs e)
		{
			DataGridView dataGridView = (DataGridView)base.Component;
			CurrencyManager currencyManager = null;
			if (dataGridView.DataSource != null && dataGridView.BindingContext != null)
			{
				currencyManager = (CurrencyManager)dataGridView.BindingContext[dataGridView.DataSource, dataGridView.DataMember];
			}
			if (currencyManager != this.cm)
			{
				if (this.cm != null)
				{
					this.cm.MetaDataChanged -= this.dataGridViewMetaDataChanged;
				}
				this.cm = currencyManager;
				if (this.cm != null)
				{
					this.cm.MetaDataChanged += this.dataGridViewMetaDataChanged;
				}
			}
			if (dataGridView.BindingContext == null)
			{
				DataGridViewDesigner.MakeSureColumnsAreSited(dataGridView);
				return;
			}
			if (dataGridView.AutoGenerateColumns && dataGridView.DataSource != null)
			{
				dataGridView.AutoGenerateColumns = false;
				DataGridViewDesigner.MakeSureColumnsAreSited(dataGridView);
				return;
			}
			if (dataGridView.DataSource == null)
			{
				if (dataGridView.AutoGenerateColumns)
				{
					DataGridViewDesigner.MakeSureColumnsAreSited(dataGridView);
					return;
				}
				dataGridView.AutoGenerateColumns = true;
			}
			else
			{
				dataGridView.AutoGenerateColumns = false;
			}
			this.RefreshColumnCollection();
		}

		private void DataGridViewDesigner_ComponentRemoving(object sender, ComponentEventArgs e)
		{
			DataGridView dataGridView = base.Component as DataGridView;
			if (e.Component != null && e.Component == dataGridView.DataSource)
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				string dataMember = dataGridView.DataMember;
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(dataGridView);
				PropertyDescriptor propertyDescriptor = ((properties != null) ? properties["DataMember"] : null);
				if (componentChangeService != null && propertyDescriptor != null)
				{
					componentChangeService.OnComponentChanging(dataGridView, propertyDescriptor);
				}
				dataGridView.DataSource = null;
				if (componentChangeService != null && propertyDescriptor != null)
				{
					componentChangeService.OnComponentChanged(dataGridView, propertyDescriptor, dataMember, "");
				}
			}
		}

		private void dataGridView_ColumnRemoved(object sender, DataGridViewColumnEventArgs e)
		{
			if (e.Column != null && !e.Column.IsDataBound)
			{
				e.Column.DisplayIndex = -1;
			}
		}

		private static void MakeSureColumnsAreSited(DataGridView dataGridView)
		{
			IContainer container = ((dataGridView.Site != null) ? dataGridView.Site.Container : null);
			for (int i = 0; i < dataGridView.Columns.Count; i++)
			{
				DataGridViewColumn dataGridViewColumn = dataGridView.Columns[i];
				IContainer container2 = ((dataGridViewColumn.Site != null) ? dataGridViewColumn.Site.Container : null);
				if (container != container2)
				{
					if (container2 != null)
					{
						container2.Remove(dataGridViewColumn);
					}
					if (container != null)
					{
						container.Add(dataGridViewColumn);
					}
				}
			}
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] array = new string[] { "AutoSizeColumnsMode", "DataSource" };
			Attribute[] array2 = new Attribute[0];
			for (int i = 0; i < array.Length; i++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
				if (propertyDescriptor != null)
				{
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(DataGridViewDesigner), propertyDescriptor, array2);
				}
			}
		}

		private bool ProcessSimilarSchema(DataGridView dataGridView)
		{
			PropertyDescriptorCollection propertyDescriptorCollection = null;
			if (this.cm != null)
			{
				try
				{
					propertyDescriptorCollection = this.cm.GetItemProperties();
				}
				catch (ArgumentException ex)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewDataSourceNoLongerValid"), ex);
				}
			}
			IContainer container = ((dataGridView.Site != null) ? dataGridView.Site.Container : null);
			bool flag = false;
			for (int i = 0; i < dataGridView.Columns.Count; i++)
			{
				DataGridViewColumn dataGridViewColumn = dataGridView.Columns[i];
				if (!string.IsNullOrEmpty(dataGridViewColumn.DataPropertyName))
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(dataGridViewColumn)["UserAddedColumn"];
					if (propertyDescriptor == null || !(bool)propertyDescriptor.GetValue(dataGridViewColumn))
					{
						PropertyDescriptor propertyDescriptor2 = ((propertyDescriptorCollection != null) ? propertyDescriptorCollection[dataGridViewColumn.DataPropertyName] : null);
						bool flag2 = false;
						if (propertyDescriptor2 == null)
						{
							flag2 = true;
						}
						else if (DataGridViewDesigner.typeofIList.IsAssignableFrom(propertyDescriptor2.PropertyType))
						{
							TypeConverter converter = TypeDescriptor.GetConverter(typeof(Image));
							if (!converter.CanConvertFrom(propertyDescriptor2.PropertyType))
							{
								flag2 = true;
							}
						}
						flag = !flag2;
						if (flag)
						{
							break;
						}
					}
				}
			}
			if (flag)
			{
				IComponentChangeService componentChangeService = base.Component.Site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				PropertyDescriptor propertyDescriptor3 = TypeDescriptor.GetProperties(base.Component)["Columns"];
				try
				{
					componentChangeService.OnComponentChanging(base.Component, propertyDescriptor3);
				}
				catch (InvalidOperationException)
				{
					return flag;
				}
				int j = 0;
				while (j < dataGridView.Columns.Count)
				{
					DataGridViewColumn dataGridViewColumn2 = dataGridView.Columns[j];
					if (string.IsNullOrEmpty(dataGridViewColumn2.DataPropertyName))
					{
						j++;
					}
					else
					{
						PropertyDescriptor propertyDescriptor4 = TypeDescriptor.GetProperties(dataGridViewColumn2)["UserAddedColumn"];
						if (propertyDescriptor4 != null && (bool)propertyDescriptor4.GetValue(dataGridViewColumn2))
						{
							j++;
						}
						else
						{
							PropertyDescriptor propertyDescriptor5 = ((propertyDescriptorCollection != null) ? propertyDescriptorCollection[dataGridViewColumn2.DataPropertyName] : null);
							bool flag3 = false;
							if (propertyDescriptor5 == null)
							{
								flag3 = true;
							}
							else if (DataGridViewDesigner.typeofIList.IsAssignableFrom(propertyDescriptor5.PropertyType))
							{
								TypeConverter converter2 = TypeDescriptor.GetConverter(typeof(Image));
								if (!converter2.CanConvertFrom(propertyDescriptor5.PropertyType))
								{
									flag3 = true;
								}
							}
							if (flag3)
							{
								dataGridView.Columns.Remove(dataGridViewColumn2);
								if (container != null)
								{
									container.Remove(dataGridViewColumn2);
								}
							}
							else
							{
								j++;
							}
						}
					}
				}
				componentChangeService.OnComponentChanged(base.Component, propertyDescriptor3, null, null);
				return flag;
			}
			return flag;
		}

		private void RefreshColumnCollection()
		{
			DataGridView dataGridView = (DataGridView)base.Component;
			ISupportInitializeNotification supportInitializeNotification = dataGridView.DataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null && !supportInitializeNotification.IsInitialized)
			{
				return;
			}
			IDesignerHost designerHost = base.Component.Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (this.ProcessSimilarSchema(dataGridView))
			{
				return;
			}
			PropertyDescriptorCollection propertyDescriptorCollection = null;
			if (this.cm != null)
			{
				try
				{
					propertyDescriptorCollection = this.cm.GetItemProperties();
				}
				catch (ArgumentException ex)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewDataSourceNoLongerValid"), ex);
				}
			}
			IContainer container = ((dataGridView.Site != null) ? dataGridView.Site.Container : null);
			IComponentChangeService componentChangeService = base.Component.Site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Columns"];
			componentChangeService.OnComponentChanging(base.Component, propertyDescriptor);
			DataGridViewColumn[] array = new DataGridViewColumn[dataGridView.Columns.Count];
			int num = 0;
			for (int i = 0; i < dataGridView.Columns.Count; i++)
			{
				DataGridViewColumn dataGridViewColumn = dataGridView.Columns[i];
				if (!string.IsNullOrEmpty(dataGridViewColumn.DataPropertyName))
				{
					PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(dataGridViewColumn)["UserAddedColumn"];
					if (propertyDescriptor2 == null || !(bool)propertyDescriptor2.GetValue(dataGridViewColumn))
					{
						array[num] = dataGridViewColumn;
						num++;
					}
				}
			}
			for (int j = 0; j < num; j++)
			{
				dataGridView.Columns.Remove(array[j]);
			}
			componentChangeService.OnComponentChanged(base.Component, propertyDescriptor, null, null);
			if (container != null)
			{
				for (int k = 0; k < num; k++)
				{
					container.Remove(array[k]);
				}
			}
			DataGridViewColumn[] array2 = null;
			int num2 = 0;
			if (dataGridView.DataSource != null)
			{
				array2 = new DataGridViewColumn[propertyDescriptorCollection.Count];
				num2 = 0;
				int l = 0;
				while (l < propertyDescriptorCollection.Count)
				{
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(Image));
					Type propertyType = propertyDescriptorCollection[l].PropertyType;
					Type type;
					if (typeof(IList).IsAssignableFrom(propertyType))
					{
						if (converter.CanConvertFrom(propertyType))
						{
							type = DataGridViewDesigner.typeofDataGridViewImageColumn;
							goto IL_0278;
						}
					}
					else
					{
						if (propertyType == typeof(bool) || propertyType == typeof(CheckState))
						{
							type = DataGridViewDesigner.typeofDataGridViewCheckBoxColumn;
							goto IL_0278;
						}
						if (typeof(Image).IsAssignableFrom(propertyType) || converter.CanConvertFrom(propertyType))
						{
							type = DataGridViewDesigner.typeofDataGridViewImageColumn;
							goto IL_0278;
						}
						type = DataGridViewDesigner.typeofDataGridViewTextBoxColumn;
						goto IL_0278;
					}
					IL_0357:
					l++;
					continue;
					IL_0278:
					string text = ToolStripDesigner.NameFromText(propertyDescriptorCollection[l].Name, type, base.Component.Site);
					DataGridViewColumn dataGridViewColumn2 = TypeDescriptor.CreateInstance(designerHost, type, null, null) as DataGridViewColumn;
					dataGridViewColumn2.DataPropertyName = propertyDescriptorCollection[l].Name;
					dataGridViewColumn2.HeaderText = ((!string.IsNullOrEmpty(propertyDescriptorCollection[l].DisplayName)) ? propertyDescriptorCollection[l].DisplayName : propertyDescriptorCollection[l].Name);
					dataGridViewColumn2.Name = propertyDescriptorCollection[l].Name;
					dataGridViewColumn2.ValueType = propertyDescriptorCollection[l].PropertyType;
					dataGridViewColumn2.ReadOnly = propertyDescriptorCollection[l].IsReadOnly;
					designerHost.Container.Add(dataGridViewColumn2, text);
					array2[num2] = dataGridViewColumn2;
					num2++;
					goto IL_0357;
				}
			}
			componentChangeService.OnComponentChanging(base.Component, propertyDescriptor);
			for (int m = 0; m < num2; m++)
			{
				array2[m].DisplayIndex = -1;
				dataGridView.Columns.Add(array2[m]);
			}
			componentChangeService.OnComponentChanged(base.Component, propertyDescriptor, null, null);
		}

		private bool ShouldSerializeAutoSizeColumnsMode()
		{
			DataGridView dataGridView = base.Component as DataGridView;
			return dataGridView != null && dataGridView.AutoSizeColumnsMode != DataGridViewAutoSizeColumnsMode.None;
		}

		private bool ShouldSerializeDataSource()
		{
			return ((DataGridView)base.Component).DataSource != null;
		}

		internal static void ShowErrorDialog(IUIService uiService, Exception ex, Control dataGridView)
		{
			if (uiService != null)
			{
				uiService.ShowError(ex);
				return;
			}
			string text = ex.Message;
			if (text == null || text.Length == 0)
			{
				text = ex.ToString();
			}
			RTLAwareMessageBox.Show(dataGridView, text, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
		}

		internal static void ShowErrorDialog(IUIService uiService, string errorString, Control dataGridView)
		{
			if (uiService != null)
			{
				uiService.ShowError(errorString);
				return;
			}
			RTLAwareMessageBox.Show(dataGridView, errorString, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
		}

		private void dataGridViewMetaDataChanged(object sender, EventArgs e)
		{
			this.RefreshColumnCollection();
		}

		public void OnEditColumns(object sender, EventArgs e)
		{
			IDesignerHost designerHost = base.Component.Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
			DataGridViewColumnCollectionDialog dataGridViewColumnCollectionDialog = new DataGridViewColumnCollectionDialog();
			dataGridViewColumnCollectionDialog.SetLiveDataGridView((DataGridView)base.Component);
			DesignerTransaction designerTransaction = designerHost.CreateTransaction(SR.GetString("DataGridViewEditColumnsTransactionString"));
			DialogResult dialogResult = DialogResult.Cancel;
			try
			{
				dialogResult = this.ShowDialog(dataGridViewColumnCollectionDialog);
			}
			finally
			{
				if (dialogResult == DialogResult.OK)
				{
					designerTransaction.Commit();
				}
				else
				{
					designerTransaction.Cancel();
				}
			}
		}

		public void OnAddColumn(object sender, EventArgs e)
		{
			IDesignerHost designerHost = base.Component.Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
			DesignerTransaction designerTransaction = designerHost.CreateTransaction(SR.GetString("DataGridViewAddColumnTransactionString"));
			DialogResult dialogResult = DialogResult.Cancel;
			DataGridViewAddColumnDialog dataGridViewAddColumnDialog = new DataGridViewAddColumnDialog(((DataGridView)base.Component).Columns, (DataGridView)base.Component);
			dataGridViewAddColumnDialog.Start(((DataGridView)base.Component).Columns.Count, true);
			try
			{
				dialogResult = this.ShowDialog(dataGridViewAddColumnDialog);
			}
			finally
			{
				if (dialogResult == DialogResult.OK)
				{
					designerTransaction.Commit();
				}
				else
				{
					designerTransaction.Cancel();
				}
			}
		}

		private DialogResult ShowDialog(Form dialog)
		{
			IUIService iuiservice = base.Component.Site.GetService(typeof(IUIService)) as IUIService;
			if (iuiservice != null)
			{
				return iuiservice.ShowDialog(dialog);
			}
			return dialog.ShowDialog(base.Component as IWin32Window);
		}

		protected DesignerVerbCollection designerVerbs;

		private DesignerActionListCollection actionLists;

		private CurrencyManager cm;

		private static Type typeofIList = typeof(IList);

		private static Type typeofDataGridViewImageColumn = typeof(DataGridViewImageColumn);

		private static Type typeofDataGridViewTextBoxColumn = typeof(DataGridViewTextBoxColumn);

		private static Type typeofDataGridViewCheckBoxColumn = typeof(DataGridViewCheckBoxColumn);

		[ComplexBindingProperties("DataSource", "DataMember")]
		private class DataGridViewChooseDataSourceActionList : DesignerActionList
		{
			public DataGridViewChooseDataSourceActionList(DataGridViewDesigner owner)
				: base(owner.Component)
			{
				this.owner = owner;
			}

			public override DesignerActionItemCollection GetSortedActionItems()
			{
				return new DesignerActionItemCollection
				{
					new DesignerActionPropertyItem("DataSource", SR.GetString("DataGridViewChooseDataSource"))
					{
						RelatedComponent = this.owner.Component
					}
				};
			}

			[AttributeProvider(typeof(IListSource))]
			public object DataSource
			{
				get
				{
					return this.owner.DataSource;
				}
				set
				{
					DataGridView dataGridView = (DataGridView)this.owner.Component;
					IDesignerHost designerHost = this.owner.Component.Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(dataGridView)["DataSource"];
					IComponentChangeService componentChangeService = this.owner.Component.Site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
					DesignerTransaction designerTransaction = designerHost.CreateTransaction(SR.GetString("DataGridViewChooseDataSourceTransactionString", new object[] { dataGridView.Name }));
					try
					{
						componentChangeService.OnComponentChanging(this.owner.Component, propertyDescriptor);
						this.owner.DataSource = value;
						componentChangeService.OnComponentChanged(this.owner.Component, propertyDescriptor, null, null);
						designerTransaction.Commit();
						designerTransaction = null;
					}
					finally
					{
						if (designerTransaction != null)
						{
							designerTransaction.Cancel();
						}
					}
				}
			}

			private DataGridViewDesigner owner;
		}

		private class DataGridViewColumnEditingActionList : DesignerActionList
		{
			public DataGridViewColumnEditingActionList(DataGridViewDesigner owner)
				: base(owner.Component)
			{
				this.owner = owner;
			}

			public override DesignerActionItemCollection GetSortedActionItems()
			{
				return new DesignerActionItemCollection
				{
					new DesignerActionMethodItem(this, "EditColumns", SR.GetString("DataGridViewEditColumnsVerb"), true),
					new DesignerActionMethodItem(this, "AddColumn", SR.GetString("DataGridViewAddColumnVerb"), true)
				};
			}

			public void EditColumns()
			{
				this.owner.OnEditColumns(this, EventArgs.Empty);
			}

			public void AddColumn()
			{
				this.owner.OnAddColumn(this, EventArgs.Empty);
			}

			private DataGridViewDesigner owner;
		}

		private class DataGridViewPropertiesActionList : DesignerActionList
		{
			public DataGridViewPropertiesActionList(DataGridViewDesigner owner)
				: base(owner.Component)
			{
				this.owner = owner;
			}

			public override DesignerActionItemCollection GetSortedActionItems()
			{
				return new DesignerActionItemCollection
				{
					new DesignerActionPropertyItem("AllowUserToAddRows", SR.GetString("DataGridViewEnableAdding")),
					new DesignerActionPropertyItem("ReadOnly", SR.GetString("DataGridViewEnableEditing")),
					new DesignerActionPropertyItem("AllowUserToDeleteRows", SR.GetString("DataGridViewEnableDeleting")),
					new DesignerActionPropertyItem("AllowUserToOrderColumns", SR.GetString("DataGridViewEnableColumnReordering"))
				};
			}

			public bool AllowUserToAddRows
			{
				get
				{
					return ((DataGridView)this.owner.Component).AllowUserToAddRows;
				}
				set
				{
					if (value != this.AllowUserToAddRows)
					{
						IDesignerHost designerHost = this.owner.Component.Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
						DesignerTransaction designerTransaction;
						if (value)
						{
							designerTransaction = designerHost.CreateTransaction(SR.GetString("DataGridViewEnableAddingTransactionString"));
						}
						else
						{
							designerTransaction = designerHost.CreateTransaction(SR.GetString("DataGridViewDisableAddingTransactionString"));
						}
						try
						{
							IComponentChangeService componentChangeService = this.owner.Component.Site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
							PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.owner.Component)["AllowUserToAddRows"];
							componentChangeService.OnComponentChanging(this.owner.Component, propertyDescriptor);
							((DataGridView)this.owner.Component).AllowUserToAddRows = value;
							componentChangeService.OnComponentChanged(this.owner.Component, propertyDescriptor, null, null);
							designerTransaction.Commit();
							designerTransaction = null;
						}
						finally
						{
							if (designerTransaction != null)
							{
								designerTransaction.Cancel();
							}
						}
					}
				}
			}

			public bool AllowUserToDeleteRows
			{
				get
				{
					return ((DataGridView)this.owner.Component).AllowUserToDeleteRows;
				}
				set
				{
					if (value != this.AllowUserToDeleteRows)
					{
						IDesignerHost designerHost = this.owner.Component.Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
						DesignerTransaction designerTransaction;
						if (value)
						{
							designerTransaction = designerHost.CreateTransaction(SR.GetString("DataGridViewEnableDeletingTransactionString"));
						}
						else
						{
							designerTransaction = designerHost.CreateTransaction(SR.GetString("DataGridViewDisableDeletingTransactionString"));
						}
						try
						{
							IComponentChangeService componentChangeService = this.owner.Component.Site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
							PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.owner.Component)["AllowUserToDeleteRows"];
							componentChangeService.OnComponentChanging(this.owner.Component, propertyDescriptor);
							((DataGridView)this.owner.Component).AllowUserToDeleteRows = value;
							componentChangeService.OnComponentChanged(this.owner.Component, propertyDescriptor, null, null);
							designerTransaction.Commit();
							designerTransaction = null;
						}
						finally
						{
							if (designerTransaction != null)
							{
								designerTransaction.Cancel();
							}
						}
					}
				}
			}

			public bool AllowUserToOrderColumns
			{
				get
				{
					return ((DataGridView)this.owner.Component).AllowUserToOrderColumns;
				}
				set
				{
					if (value != this.AllowUserToOrderColumns)
					{
						IDesignerHost designerHost = this.owner.Component.Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
						DesignerTransaction designerTransaction;
						if (value)
						{
							designerTransaction = designerHost.CreateTransaction(SR.GetString("DataGridViewEnableColumnReorderingTransactionString"));
						}
						else
						{
							designerTransaction = designerHost.CreateTransaction(SR.GetString("DataGridViewDisableColumnReorderingTransactionString"));
						}
						try
						{
							IComponentChangeService componentChangeService = this.owner.Component.Site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
							PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.owner.Component)["AllowUserToReorderColumns"];
							componentChangeService.OnComponentChanging(this.owner.Component, propertyDescriptor);
							((DataGridView)this.owner.Component).AllowUserToOrderColumns = value;
							componentChangeService.OnComponentChanged(this.owner.Component, propertyDescriptor, null, null);
							designerTransaction.Commit();
							designerTransaction = null;
						}
						finally
						{
							if (designerTransaction != null)
							{
								designerTransaction.Cancel();
							}
						}
					}
				}
			}

			public bool ReadOnly
			{
				get
				{
					return !((DataGridView)this.owner.Component).ReadOnly;
				}
				set
				{
					if (value != this.ReadOnly)
					{
						IDesignerHost designerHost = this.owner.Component.Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
						DesignerTransaction designerTransaction;
						if (value)
						{
							designerTransaction = designerHost.CreateTransaction(SR.GetString("DataGridViewEnableEditingTransactionString"));
						}
						else
						{
							designerTransaction = designerHost.CreateTransaction(SR.GetString("DataGridViewDisableEditingTransactionString"));
						}
						try
						{
							IComponentChangeService componentChangeService = this.owner.Component.Site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
							PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.owner.Component)["ReadOnly"];
							componentChangeService.OnComponentChanging(this.owner.Component, propertyDescriptor);
							((DataGridView)this.owner.Component).ReadOnly = !value;
							componentChangeService.OnComponentChanged(this.owner.Component, propertyDescriptor, null, null);
							designerTransaction.Commit();
							designerTransaction = null;
						}
						finally
						{
							if (designerTransaction != null)
							{
								designerTransaction.Cancel();
							}
						}
					}
				}
			}

			private DataGridViewDesigner owner;
		}
	}
}
