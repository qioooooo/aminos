using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Design;
using System.Diagnostics;
using System.Security.Permissions;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class RepeaterDesigner : ControlDesigner, IDataSourceProvider
	{
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new DataBoundControlActionList(this, this.DataSourceDesigner));
				return designerActionListCollection;
			}
		}

		public string DataMember
		{
			get
			{
				return ((Repeater)base.Component).DataMember;
			}
			set
			{
				((Repeater)base.Component).DataMember = value;
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
				return ((Repeater)base.Component).DataSourceID;
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
				((Repeater)base.Component).DataSourceID = value;
				this.OnDataSourceChanged();
				this.ExecuteChooseDataSourcePostSteps();
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

		protected bool TemplatesExist
		{
			get
			{
				Repeater repeater = (Repeater)base.ViewControl;
				return repeater.ItemTemplate != null || repeater.HeaderTemplate != null || repeater.FooterTemplate != null || repeater.AlternatingItemTemplate != null;
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
				}
				this._dataSourceDesigner = dataSourceDesigner;
				if (this._dataSourceDesigner != null)
				{
					this._dataSourceDesigner.DataSourceChanged += this.DataSourceChanged;
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
			if (disposing && base.Component != null && base.Component.Site != null)
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
			base.Dispose(disposing);
		}

		protected virtual void ExecuteChooseDataSourcePostSteps()
		{
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

		protected IEnumerable GetDesignTimeDataSource(int minimumRows)
		{
			IEnumerable resolvedSelectedDataSource = this.GetResolvedSelectedDataSource();
			return this.GetDesignTimeDataSource(resolvedSelectedDataSource, minimumRows);
		}

		protected IEnumerable GetDesignTimeDataSource(IEnumerable selectedDataSource, int minimumRows)
		{
			DataTable dataTable = this.designTimeDataTable;
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
				}
			}
			return DesignTimeData.GetDesignTimeDataSource(dataTable, minimumRows);
		}

		public override string GetDesignTimeHtml()
		{
			bool templatesExist = this.TemplatesExist;
			Repeater repeater = (Repeater)base.ViewControl;
			string text2;
			if (templatesExist)
			{
				DesignerDataSourceView designerView = this.DesignerView;
				IEnumerable enumerable = null;
				bool flag = false;
				string text = string.Empty;
				if (designerView == null)
				{
					IEnumerable resolvedSelectedDataSource = this.GetResolvedSelectedDataSource();
					enumerable = this.GetDesignTimeDataSource(resolvedSelectedDataSource, 5);
				}
				else
				{
					try
					{
						bool flag2;
						enumerable = designerView.GetDesignTimeData(5, out flag2);
					}
					catch (Exception ex)
					{
						if (base.Component.Site != null)
						{
							IComponentDesignerDebugService componentDesignerDebugService = (IComponentDesignerDebugService)base.Component.Site.GetService(typeof(IComponentDesignerDebugService));
							if (componentDesignerDebugService != null)
							{
								componentDesignerDebugService.Fail(SR.GetString("DataSource_DebugService_FailedCall", new object[] { "DesignerDataSourceView.GetDesignTimeData", ex.Message }));
							}
						}
					}
				}
				try
				{
					try
					{
						repeater.DataSource = enumerable;
						text = repeater.DataSourceID;
						repeater.DataSourceID = string.Empty;
						flag = true;
						repeater.DataBind();
						text2 = base.GetDesignTimeHtml();
					}
					catch (Exception ex2)
					{
						text2 = this.GetErrorDesignTimeHtml(ex2);
					}
					return text2;
				}
				finally
				{
					repeater.DataSource = null;
					if (flag)
					{
						repeater.DataSourceID = text;
					}
				}
			}
			text2 = this.GetEmptyDesignTimeHtml();
			return text2;
		}

		protected override string GetEmptyDesignTimeHtml()
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("Repeater_NoTemplatesInst"));
		}

		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("Control_ErrorRendering"));
		}

		public IEnumerable GetResolvedSelectedDataSource()
		{
			IEnumerable enumerable = null;
			DataBinding dataBinding = base.DataBindings["DataSource"];
			if (dataBinding != null)
			{
				enumerable = DesignTimeData.GetSelectedDataSource(base.Component, dataBinding.Expression, this.DataMember);
			}
			return enumerable;
		}

		public object GetSelectedDataSource()
		{
			object obj = null;
			DataBinding dataBinding = base.DataBindings["DataSource"];
			if (dataBinding != null)
			{
				obj = DesignTimeData.GetSelectedDataSource(base.Component, dataBinding.Expression);
			}
			return obj;
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(Repeater));
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

		private void OnAnyComponentChanged(object source, ComponentChangedEventArgs ce)
		{
			if (ce.Member != null)
			{
				object component = ce.Component;
				Control control = component as Control;
				if (control != null && ce.Member.Name == "ID" && base.Component != null && ((string)ce.OldValue == this.DataSourceID || (string)ce.NewValue == this.DataSourceID))
				{
					this.ConnectToDataSource();
					this.UpdateDesignTimeHtml();
				}
			}
		}

		private void OnComponentAdded(object sender, ComponentEventArgs e)
		{
			IComponent component = e.Component;
			Control control = component as Control;
			if (control != null && control.ID == this.DataSourceID)
			{
				this.ConnectToDataSource();
				this.UpdateDesignTimeHtml();
			}
		}

		public override void OnComponentChanged(object source, ComponentChangedEventArgs ce)
		{
			if (ce.Member != null)
			{
				string name = ce.Member.Name;
				if (name.Equals("DataSource") || name.Equals("DataMember"))
				{
					this.OnDataSourceChanged();
				}
			}
			base.OnComponentChanged(source, ce);
		}

		private void OnComponentRemoving(object sender, ComponentEventArgs e)
		{
			IComponent component = e.Component;
			Control control = component as Control;
			if (control != null && control.ID == this.DataSourceID && base.Component != null && this._dataSourceDesigner != null)
			{
				this._dataSourceDesigner.DataSourceChanged -= this.DataSourceChanged;
				this._dataSourceDesigner = null;
			}
		}

		private void OnComponentRemoved(object sender, ComponentEventArgs e)
		{
			IComponent component = e.Component;
			Control control = component as Control;
			if (control != null && base.Component != null && control.ID == this.DataSourceID)
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null && !designerHost.Loading)
				{
					this.ConnectToDataSource();
					this.UpdateDesignTimeHtml();
				}
			}
		}

		public virtual void OnDataSourceChanged()
		{
			this.ConnectToDataSource();
			this.designTimeDataTable = null;
		}

		private void OnDesignerLoadComplete(object sender, EventArgs e)
		{
			this.ConnectToDataSource();
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["DataSource"];
			AttributeCollection attributes = propertyDescriptor.Attributes;
			int num = -1;
			int count = attributes.Count;
			string dataSource = this.DataSource;
			for (int i = 0; i < attributes.Count; i++)
			{
				if (attributes[i] is BrowsableAttribute)
				{
					num = i;
				}
			}
			int num2;
			if (num == -1 && dataSource.Length == 0)
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
			if (dataSource.Length == 0)
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
			propertyDescriptor = (PropertyDescriptor)properties["DataSourceID"];
			propertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[]
			{
				new TypeConverterAttribute(typeof(DataSourceIDConverter))
			});
			properties["DataSourceID"] = propertyDescriptor;
		}

		internal static TraceSwitch RepeaterDesignerSwitch = new TraceSwitch("RepeaterDesigner", "Enable Repeater designer general purpose traces.");

		private DataTable dummyDataTable;

		private DataTable designTimeDataTable;

		private IDataSourceDesigner _dataSourceDesigner;
	}
}
