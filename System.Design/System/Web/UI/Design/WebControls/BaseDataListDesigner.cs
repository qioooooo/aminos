using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Design;
using System.Security.Permissions;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public abstract class BaseDataListDesigner : TemplatedControlDesigner, IDataBindingSchemaProvider, IDataSourceProvider
	{
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new BaseDataListActionList(this, this.DataSourceDesigner));
				return designerActionListCollection;
			}
		}

		public string DataKeyField
		{
			get
			{
				return this.bdl.DataKeyField;
			}
			set
			{
				this.bdl.DataKeyField = value;
			}
		}

		public string DataMember
		{
			get
			{
				return this.bdl.DataMember;
			}
			set
			{
				this.bdl.DataMember = value;
				this.OnDataSourceChanged();
			}
		}

		public string DataSource
		{
			get
			{
				DataBinding dataBinding = base.DataBindings["DataSource"];
				if (dataBinding != null)
				{
					return dataBinding.Expression;
				}
				return string.Empty;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					base.DataBindings.Remove("DataSource");
				}
				else
				{
					DataBinding dataBinding = base.DataBindings["DataSource"];
					if (dataBinding == null)
					{
						dataBinding = new DataBinding("DataSource", typeof(IEnumerable), value);
					}
					else
					{
						dataBinding.Expression = value;
					}
					base.DataBindings.Add(dataBinding);
				}
				this.OnDataSourceChanged();
				base.OnBindingsCollectionChangedInternal("DataSource");
			}
		}

		public IDataSourceDesigner DataSourceDesigner
		{
			get
			{
				return this._dataSourceDesigner;
			}
		}

		public string DataSourceID
		{
			get
			{
				return this.bdl.DataSourceID;
			}
			set
			{
				if (value == this.DataSourceID)
				{
					return;
				}
				if (value == SR.GetString("DataSourceIDChromeConverter_NewDataSource"))
				{
					this.CreateDataSource();
					return;
				}
				if (value == SR.GetString("DataSourceIDChromeConverter_NoDataSource"))
				{
					value = string.Empty;
				}
				this.bdl.DataSourceID = value;
				this.OnDataSourceChanged();
				this.OnSchemaRefreshed();
			}
		}

		public DesignerDataSourceView DesignerView
		{
			get
			{
				DesignerDataSourceView designerDataSourceView = null;
				if (this.DataSourceDesigner != null)
				{
					designerDataSourceView = this.DataSourceDesigner.GetView(this.DataMember);
					if (designerDataSourceView == null && string.IsNullOrEmpty(this.DataMember))
					{
						string[] viewNames = this.DataSourceDesigner.GetViewNames();
						if (viewNames != null && viewNames.Length > 0)
						{
							designerDataSourceView = this.DataSourceDesigner.GetView(viewNames[0]);
						}
					}
				}
				return designerDataSourceView;
			}
		}

		private bool ConnectToDataSource()
		{
			IDataSourceDesigner dataSourceDesigner = this.GetDataSourceDesigner();
			if (this._dataSourceDesigner != dataSourceDesigner)
			{
				if (this._dataSourceDesigner != null)
				{
					this._dataSourceDesigner.DataSourceChanged -= this.DataSourceChanged;
					this._dataSourceDesigner.SchemaRefreshed -= this.SchemaRefreshed;
				}
				this._dataSourceDesigner = dataSourceDesigner;
				if (this._dataSourceDesigner != null)
				{
					this._dataSourceDesigner.DataSourceChanged += this.DataSourceChanged;
					this._dataSourceDesigner.SchemaRefreshed += this.SchemaRefreshed;
				}
				return true;
			}
			return false;
		}

		private void CreateDataSource()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.CreateDataSourceCallback), null, SR.GetString("BaseDataBoundControl_CreateDataSourceTransaction"));
		}

		private bool CreateDataSourceCallback(object context)
		{
			CreateDataSourceDialog createDataSourceDialog = new CreateDataSourceDialog(this, typeof(IDataSource), true);
			DialogResult dialogResult = UIServiceHelper.ShowDialog(base.Component.Site, createDataSourceDialog);
			string dataSourceID = createDataSourceDialog.DataSourceID;
			if (dataSourceID.Length > 0)
			{
				this.DataSourceID = dataSourceID;
			}
			return dialogResult == DialogResult.OK;
		}

		private void DataSourceChanged(object sender, EventArgs e)
		{
			this.designTimeDataTable = null;
			this.UpdateDesignTimeHtml();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (base.Component != null && base.Component.Site != null)
				{
					if (base.RootDesigner != null)
					{
						base.RootDesigner.LoadComplete -= this.OnDesignerLoadComplete;
					}
					IComponentChangeService componentChangeService = (IComponentChangeService)base.Component.Site.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						componentChangeService.ComponentAdded -= this.OnComponentAdded;
						componentChangeService.ComponentRemoving -= this.OnComponentRemoving;
						componentChangeService.ComponentRemoved -= this.OnComponentRemoved;
						componentChangeService.ComponentChanged -= this.OnAnyComponentChanged;
					}
				}
				this.bdl = null;
				if (this._dataSourceDesigner != null)
				{
					this._dataSourceDesigner.DataSourceChanged -= this.DataSourceChanged;
					this._dataSourceDesigner.SchemaRefreshed -= this.SchemaRefreshed;
					this._dataSourceDesigner = null;
				}
			}
			base.Dispose(disposing);
		}

		private IDataSourceDesigner GetDataSourceDesigner()
		{
			IDataSourceDesigner dataSourceDesigner = null;
			string dataSourceID = this.DataSourceID;
			if (!string.IsNullOrEmpty(dataSourceID))
			{
				Control control = ControlHelper.FindControl(base.Component.Site, (Control)base.Component, dataSourceID);
				if (control != null && control.Site != null)
				{
					IDesignerHost designerHost = (IDesignerHost)control.Site.GetService(typeof(IDesignerHost));
					if (designerHost != null)
					{
						dataSourceDesigner = designerHost.GetDesigner(control) as IDataSourceDesigner;
					}
				}
			}
			return dataSourceDesigner;
		}

		protected IEnumerable GetDesignTimeDataSource(int minimumRows, out bool dummyDataSource)
		{
			IEnumerable resolvedSelectedDataSource = this.GetResolvedSelectedDataSource();
			return this.GetDesignTimeDataSource(resolvedSelectedDataSource, minimumRows, out dummyDataSource);
		}

		protected IEnumerable GetDesignTimeDataSource(IEnumerable selectedDataSource, int minimumRows, out bool dummyDataSource)
		{
			DataTable dataTable = this.designTimeDataTable;
			dummyDataSource = false;
			if (dataTable == null)
			{
				if (selectedDataSource != null)
				{
					this.designTimeDataTable = DesignTimeData.CreateSampleDataTable(selectedDataSource);
					dataTable = this.designTimeDataTable;
				}
				if (dataTable == null)
				{
					if (this.dummyDataTable == null)
					{
						this.dummyDataTable = DesignTimeData.CreateDummyDataTable();
					}
					dataTable = this.dummyDataTable;
					dummyDataSource = true;
				}
			}
			return DesignTimeData.GetDesignTimeDataSource(dataTable, minimumRows);
		}

		public IEnumerable GetResolvedSelectedDataSource()
		{
			IEnumerable enumerable = null;
			DataBinding dataBinding = base.DataBindings["DataSource"];
			if (dataBinding != null)
			{
				enumerable = DesignTimeData.GetSelectedDataSource(this.bdl, dataBinding.Expression, this.DataMember);
			}
			return enumerable;
		}

		public object GetSelectedDataSource()
		{
			object obj = null;
			DataBinding dataBinding = base.DataBindings["DataSource"];
			if (dataBinding != null)
			{
				obj = DesignTimeData.GetSelectedDataSource(this.bdl, dataBinding.Expression);
			}
			return obj;
		}

		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public override IEnumerable GetTemplateContainerDataSource(string templateName)
		{
			return this.GetResolvedSelectedDataSource();
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(BaseDataList));
			this.bdl = (BaseDataList)component;
			base.Initialize(component);
			base.SetViewFlags(ViewFlags.DesignTimeHtmlRequiresLoadComplete, true);
			if (base.RootDesigner != null)
			{
				if (base.RootDesigner.IsLoading)
				{
					base.RootDesigner.LoadComplete += this.OnDesignerLoadComplete;
				}
				else
				{
					this.OnDesignerLoadComplete(null, EventArgs.Empty);
				}
			}
			IComponentChangeService componentChangeService = (IComponentChangeService)component.Site.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentAdded += this.OnComponentAdded;
				componentChangeService.ComponentRemoving += this.OnComponentRemoving;
				componentChangeService.ComponentRemoved += this.OnComponentRemoved;
				componentChangeService.ComponentChanged += this.OnAnyComponentChanged;
			}
		}

		protected internal void InvokePropertyBuilder(int initialPage)
		{
			ComponentEditor componentEditor;
			if (this.bdl is global::System.Web.UI.WebControls.DataGrid)
			{
				componentEditor = new DataGridComponentEditor(initialPage);
			}
			else
			{
				componentEditor = new DataListComponentEditor(initialPage);
			}
			componentEditor.EditComponent(this.bdl);
		}

		private void OnAnyComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (e.Member != null)
			{
				object component = e.Component;
				IDataSource dataSource = component as IDataSource;
				if (dataSource != null && dataSource is Control && e.Member.Name == "ID" && base.Component != null && ((string)e.OldValue == this.DataSourceID || (string)e.NewValue == this.DataSourceID))
				{
					this.ConnectToDataSource();
					this.UpdateDesignTimeHtml();
				}
			}
		}

		[Obsolete("Use of this method is not recommended because the AutoFormat dialog is launched by the designer host. The list of available AutoFormats is exposed on the ControlDesigner in the AutoFormats property. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected void OnAutoFormat(object sender, EventArgs e)
		{
		}

		public override void OnAutoFormatApplied(DesignerAutoFormat appliedAutoFormat)
		{
			this.OnStylesChanged();
			base.OnAutoFormatApplied(appliedAutoFormat);
		}

		private void OnComponentAdded(object sender, ComponentEventArgs e)
		{
			IComponent component = e.Component;
			IDataSource dataSource = component as IDataSource;
			if (dataSource != null && component is Control && ((Control)dataSource).ID == this.DataSourceID)
			{
				this.ConnectToDataSource();
				this.UpdateDesignTimeHtml();
			}
		}

		public override void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (e.Member != null)
			{
				string name = e.Member.Name;
				if (name.Equals("DataSource") || name.Equals("DataMember"))
				{
					this.OnDataSourceChanged();
				}
				else if (name.Equals("ItemStyle") || name.Equals("AlternatingItemStyle") || name.Equals("SelectedItemStyle") || name.Equals("EditItemStyle") || name.Equals("HeaderStyle") || name.Equals("FooterStyle") || name.Equals("SeparatorStyle") || name.Equals("Font") || name.Equals("ForeColor") || name.Equals("BackColor"))
				{
					this.OnStylesChanged();
				}
			}
			base.OnComponentChanged(sender, e);
		}

		private void OnComponentRemoving(object sender, ComponentEventArgs e)
		{
			IComponent component = e.Component;
			IDataSource dataSource = component as IDataSource;
			if (dataSource != null && dataSource is Control && base.Component != null && ((Control)dataSource).ID == this.DataSourceID && this._dataSourceDesigner != null)
			{
				this._dataSourceDesigner.DataSourceChanged -= this.DataSourceChanged;
				this._dataSourceDesigner.SchemaRefreshed -= this.SchemaRefreshed;
				this._dataSourceDesigner = null;
			}
		}

		private void OnComponentRemoved(object sender, ComponentEventArgs e)
		{
			IComponent component = e.Component;
			IDataSource dataSource = component as IDataSource;
			if (dataSource != null && dataSource is Control && base.Component != null && ((Control)dataSource).ID == this.DataSourceID)
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null && !designerHost.Loading)
				{
					this.ConnectToDataSource();
					this.UpdateDesignTimeHtml();
				}
			}
		}

		protected internal virtual void OnDataSourceChanged()
		{
			this.ConnectToDataSource();
			this.designTimeDataTable = null;
		}

		private void OnDesignerLoadComplete(object sender, EventArgs e)
		{
			this.ConnectToDataSource();
		}

		protected void OnPropertyBuilder(object sender, EventArgs e)
		{
			this.InvokePropertyBuilder(0);
		}

		protected virtual void OnSchemaRefreshed()
		{
			this.UpdateDesignTimeHtml();
		}

		protected internal void OnStylesChanged()
		{
			this.OnTemplateEditingVerbsChanged();
		}

		protected abstract void OnTemplateEditingVerbsChanged();

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["DataSource"];
			AttributeCollection attributes = propertyDescriptor.Attributes;
			int num = -1;
			int count = attributes.Count;
			string dataSource = this.DataSource;
			if (dataSource.Length > 0)
			{
				this._keepDataSourceBrowsable = true;
			}
			for (int i = 0; i < attributes.Count; i++)
			{
				if (attributes[i] is BrowsableAttribute)
				{
					num = i;
					break;
				}
			}
			int num2;
			if (num == -1 && dataSource.Length == 0 && !this._keepDataSourceBrowsable)
			{
				num2 = count + 2;
			}
			else
			{
				num2 = count + 1;
			}
			Attribute[] array = new Attribute[num2];
			attributes.CopyTo(array, 0);
			array[count] = new TypeConverterAttribute(typeof(DataSourceConverter));
			if (dataSource.Length == 0 && !this._keepDataSourceBrowsable)
			{
				if (num == -1)
				{
					array[count + 1] = BrowsableAttribute.No;
				}
				else
				{
					array[num] = BrowsableAttribute.No;
				}
			}
			propertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), "DataSource", typeof(string), array);
			properties["DataSource"] = propertyDescriptor;
			propertyDescriptor = (PropertyDescriptor)properties["DataMember"];
			propertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[]
			{
				new TypeConverterAttribute(typeof(DataMemberConverter))
			});
			properties["DataMember"] = propertyDescriptor;
			propertyDescriptor = (PropertyDescriptor)properties["DataKeyField"];
			propertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[]
			{
				new TypeConverterAttribute(typeof(DataFieldConverter))
			});
			properties["DataKeyField"] = propertyDescriptor;
			propertyDescriptor = (PropertyDescriptor)properties["DataSourceID"];
			propertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[]
			{
				new TypeConverterAttribute(typeof(DataSourceIDConverter))
			});
			properties["DataSourceID"] = propertyDescriptor;
		}

		private void SchemaRefreshed(object sender, EventArgs e)
		{
			this.OnSchemaRefreshed();
		}

		bool IDataBindingSchemaProvider.CanRefreshSchema
		{
			get
			{
				IDataSourceDesigner dataSourceDesigner = this.DataSourceDesigner;
				return dataSourceDesigner != null && dataSourceDesigner.CanRefreshSchema;
			}
		}

		IDataSourceViewSchema IDataBindingSchemaProvider.Schema
		{
			get
			{
				DesignerDataSourceView designerView = this.DesignerView;
				if (designerView != null)
				{
					return designerView.Schema;
				}
				return null;
			}
		}

		void IDataBindingSchemaProvider.RefreshSchema(bool preferSilent)
		{
			IDataSourceDesigner dataSourceDesigner = this.DataSourceDesigner;
			if (dataSourceDesigner != null)
			{
				dataSourceDesigner.RefreshSchema(preferSilent);
			}
		}

		private BaseDataList bdl;

		private DataTable dummyDataTable;

		private DataTable designTimeDataTable;

		private IDataSourceDesigner _dataSourceDesigner;

		private bool _keepDataSourceBrowsable;
	}
}
