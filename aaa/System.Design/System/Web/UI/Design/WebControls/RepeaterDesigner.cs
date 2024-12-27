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
	// Token: 0x020004A7 RID: 1191
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class RepeaterDesigner : ControlDesigner, IDataSourceProvider
	{
		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06002B15 RID: 11029 RVA: 0x000EE990 File Offset: 0x000ED990
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

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06002B16 RID: 11030 RVA: 0x000EE9C3 File Offset: 0x000ED9C3
		// (set) Token: 0x06002B17 RID: 11031 RVA: 0x000EE9D5 File Offset: 0x000ED9D5
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

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06002B18 RID: 11032 RVA: 0x000EE9F0 File Offset: 0x000ED9F0
		// (set) Token: 0x06002B19 RID: 11033 RVA: 0x000EEA20 File Offset: 0x000EDA20
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

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06002B1A RID: 11034 RVA: 0x000EEA9A File Offset: 0x000EDA9A
		public IDataSourceDesigner DataSourceDesigner
		{
			get
			{
				return this._dataSourceDesigner;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06002B1B RID: 11035 RVA: 0x000EEAA2 File Offset: 0x000EDAA2
		// (set) Token: 0x06002B1C RID: 11036 RVA: 0x000EEAB4 File Offset: 0x000EDAB4
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

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06002B1D RID: 11037 RVA: 0x000EEB20 File Offset: 0x000EDB20
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

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06002B1E RID: 11038 RVA: 0x000EEB80 File Offset: 0x000EDB80
		protected bool TemplatesExist
		{
			get
			{
				Repeater repeater = (Repeater)base.ViewControl;
				return repeater.ItemTemplate != null || repeater.HeaderTemplate != null || repeater.FooterTemplate != null || repeater.AlternatingItemTemplate != null;
			}
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x000EEBC0 File Offset: 0x000EDBC0
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

		// Token: 0x06002B20 RID: 11040 RVA: 0x000EEC25 File Offset: 0x000EDC25
		private void CreateDataSource()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.CreateDataSourceCallback), null, SR.GetString("BaseDataBoundControl_CreateDataSourceTransaction"));
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x000EEC4C File Offset: 0x000EDC4C
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

		// Token: 0x06002B22 RID: 11042 RVA: 0x000EEC98 File Offset: 0x000EDC98
		private void DataSourceChanged(object sender, EventArgs e)
		{
			this.designTimeDataTable = null;
			this.UpdateDesignTimeHtml();
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x000EECA8 File Offset: 0x000EDCA8
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

		// Token: 0x06002B24 RID: 11044 RVA: 0x000EED67 File Offset: 0x000EDD67
		protected virtual void ExecuteChooseDataSourcePostSteps()
		{
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x000EED6C File Offset: 0x000EDD6C
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

		// Token: 0x06002B26 RID: 11046 RVA: 0x000EEDE0 File Offset: 0x000EDDE0
		protected IEnumerable GetDesignTimeDataSource(int minimumRows)
		{
			IEnumerable resolvedSelectedDataSource = this.GetResolvedSelectedDataSource();
			return this.GetDesignTimeDataSource(resolvedSelectedDataSource, minimumRows);
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x000EEDFC File Offset: 0x000EDDFC
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

		// Token: 0x06002B28 RID: 11048 RVA: 0x000EEE50 File Offset: 0x000EDE50
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

		// Token: 0x06002B29 RID: 11049 RVA: 0x000EEF98 File Offset: 0x000EDF98
		protected override string GetEmptyDesignTimeHtml()
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("Repeater_NoTemplatesInst"));
		}

		// Token: 0x06002B2A RID: 11050 RVA: 0x000EEFAA File Offset: 0x000EDFAA
		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("Control_ErrorRendering"));
		}

		// Token: 0x06002B2B RID: 11051 RVA: 0x000EEFBC File Offset: 0x000EDFBC
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

		// Token: 0x06002B2C RID: 11052 RVA: 0x000EEFF8 File Offset: 0x000EDFF8
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

		// Token: 0x06002B2D RID: 11053 RVA: 0x000EF030 File Offset: 0x000EE030
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

		// Token: 0x06002B2E RID: 11054 RVA: 0x000EF0FC File Offset: 0x000EE0FC
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

		// Token: 0x06002B2F RID: 11055 RVA: 0x000EF180 File Offset: 0x000EE180
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

		// Token: 0x06002B30 RID: 11056 RVA: 0x000EF1C0 File Offset: 0x000EE1C0
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

		// Token: 0x06002B31 RID: 11057 RVA: 0x000EF20C File Offset: 0x000EE20C
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

		// Token: 0x06002B32 RID: 11058 RVA: 0x000EF26C File Offset: 0x000EE26C
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

		// Token: 0x06002B33 RID: 11059 RVA: 0x000EF2D3 File Offset: 0x000EE2D3
		public virtual void OnDataSourceChanged()
		{
			this.ConnectToDataSource();
			this.designTimeDataTable = null;
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x000EF2E3 File Offset: 0x000EE2E3
		private void OnDesignerLoadComplete(object sender, EventArgs e)
		{
			this.ConnectToDataSource();
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x000EF2EC File Offset: 0x000EE2EC
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

		// Token: 0x04001D6E RID: 7534
		internal static TraceSwitch RepeaterDesignerSwitch = new TraceSwitch("RepeaterDesigner", "Enable Repeater designer general purpose traces.");

		// Token: 0x04001D6F RID: 7535
		private DataTable dummyDataTable;

		// Token: 0x04001D70 RID: 7536
		private DataTable designTimeDataTable;

		// Token: 0x04001D71 RID: 7537
		private IDataSourceDesigner _dataSourceDesigner;
	}
}
