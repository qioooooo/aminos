using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Design;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020003E7 RID: 999
	public class DataBoundControlDesigner : BaseDataBoundControlDesigner, IDataBindingSchemaProvider, IDataSourceProvider
	{
		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x060024EB RID: 9451 RVA: 0x000C67D4 File Offset: 0x000C57D4
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				if (this.UseDataSourcePickerActionList)
				{
					designerActionListCollection.Add(new DataBoundControlActionList(this, this.DataSourceDesigner));
				}
				designerActionListCollection.AddRange(base.ActionLists);
				return designerActionListCollection;
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x060024EC RID: 9452 RVA: 0x000C680F File Offset: 0x000C580F
		// (set) Token: 0x060024ED RID: 9453 RVA: 0x000C6821 File Offset: 0x000C5821
		public string DataMember
		{
			get
			{
				return ((DataBoundControl)base.Component).DataMember;
			}
			set
			{
				((DataBoundControl)base.Component).DataMember = value;
				this.OnDataSourceChanged(true);
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x060024EE RID: 9454 RVA: 0x000C683B File Offset: 0x000C583B
		public IDataSourceDesigner DataSourceDesigner
		{
			get
			{
				return this._dataSourceDesigner;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x060024EF RID: 9455 RVA: 0x000C6844 File Offset: 0x000C5844
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

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x060024F0 RID: 9456 RVA: 0x000C68A2 File Offset: 0x000C58A2
		protected virtual int SampleRowCount
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x060024F1 RID: 9457 RVA: 0x000C68A5 File Offset: 0x000C58A5
		protected virtual bool UseDataSourcePickerActionList
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x000C68A8 File Offset: 0x000C58A8
		protected override bool ConnectToDataSource()
		{
			IDataSourceDesigner dataSourceDesigner = this.GetDataSourceDesigner();
			if (this._dataSourceDesigner != dataSourceDesigner)
			{
				if (this._dataSourceDesigner != null)
				{
					this._dataSourceDesigner.DataSourceChanged -= this.OnDataSourceChanged;
					this._dataSourceDesigner.SchemaRefreshed -= this.OnSchemaRefreshed;
				}
				this._dataSourceDesigner = dataSourceDesigner;
				if (this._dataSourceDesigner != null)
				{
					this._dataSourceDesigner.DataSourceChanged += this.OnDataSourceChanged;
					this._dataSourceDesigner.SchemaRefreshed += this.OnSchemaRefreshed;
				}
				return true;
			}
			return false;
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x000C693B File Offset: 0x000C593B
		protected override void CreateDataSource()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.CreateDataSourceCallback), null, SR.GetString("BaseDataBoundControl_CreateDataSourceTransaction"));
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x000C6960 File Offset: 0x000C5960
		private bool CreateDataSourceCallback(object context)
		{
			string text;
			DialogResult dialogResult = BaseDataBoundControlDesigner.ShowCreateDataSourceDialog(this, typeof(IDataSource), true, out text);
			if (text.Length > 0)
			{
				base.DataSourceID = text;
			}
			return dialogResult == DialogResult.OK;
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x000C6998 File Offset: 0x000C5998
		protected override void DataBind(BaseDataBoundControl dataBoundControl)
		{
			IEnumerable designTimeDataSource = this.GetDesignTimeDataSource();
			string dataSourceID = dataBoundControl.DataSourceID;
			object dataSource = dataBoundControl.DataSource;
			dataBoundControl.DataSource = designTimeDataSource;
			dataBoundControl.DataSourceID = string.Empty;
			try
			{
				if (designTimeDataSource != null)
				{
					dataBoundControl.DataBind();
				}
			}
			finally
			{
				dataBoundControl.DataSource = dataSource;
				dataBoundControl.DataSourceID = dataSourceID;
			}
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x000C69F8 File Offset: 0x000C59F8
		protected override void DisconnectFromDataSource()
		{
			if (this._dataSourceDesigner != null)
			{
				this._dataSourceDesigner.DataSourceChanged -= this.OnDataSourceChanged;
				this._dataSourceDesigner.SchemaRefreshed -= this.OnSchemaRefreshed;
				this._dataSourceDesigner = null;
			}
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x000C6A38 File Offset: 0x000C5A38
		protected override void Dispose(bool disposing)
		{
			if (disposing && this._dataSourceDesigner != null)
			{
				this._dataSourceDesigner.DataSourceChanged -= this.OnDataSourceChanged;
				this._dataSourceDesigner.SchemaRefreshed -= this.OnSchemaRefreshed;
				this._dataSourceDesigner = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x000C6A8C File Offset: 0x000C5A8C
		private IDataSourceDesigner GetDataSourceDesigner()
		{
			IDataSourceDesigner dataSourceDesigner = null;
			string dataSourceID = base.DataSourceID;
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

		// Token: 0x060024F9 RID: 9465 RVA: 0x000C6B00 File Offset: 0x000C5B00
		protected virtual IEnumerable GetDesignTimeDataSource()
		{
			IEnumerable enumerable = null;
			DesignerDataSourceView designerView = this.DesignerView;
			bool flag;
			if (designerView != null)
			{
				try
				{
					enumerable = designerView.GetDesignTimeData(this.SampleRowCount, out flag);
					goto IL_00A8;
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
					goto IL_00A8;
				}
			}
			IEnumerable resolvedSelectedDataSource = ((IDataSourceProvider)this).GetResolvedSelectedDataSource();
			if (resolvedSelectedDataSource != null)
			{
				DataTable dataTable = DesignTimeData.CreateSampleDataTable(resolvedSelectedDataSource);
				enumerable = DesignTimeData.GetDesignTimeDataSource(dataTable, this.SampleRowCount);
				flag = true;
			}
			IL_00A8:
			if (enumerable != null)
			{
				ICollection collection = enumerable as ICollection;
				if (collection == null || collection.Count > 0)
				{
					return enumerable;
				}
			}
			flag = true;
			return this.GetSampleDataSource();
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x000C6BE8 File Offset: 0x000C5BE8
		protected virtual IEnumerable GetSampleDataSource()
		{
			DataTable dataTable;
			if (((DataBoundControl)base.Component).DataSourceID.Length > 0)
			{
				dataTable = DesignTimeData.CreateDummyDataBoundDataTable();
			}
			else
			{
				dataTable = DesignTimeData.CreateDummyDataTable();
			}
			return DesignTimeData.GetDesignTimeDataSource(dataTable, this.SampleRowCount);
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x000C6C2B File Offset: 0x000C5C2B
		private void OnDataSourceChanged(object sender, EventArgs e)
		{
			this.OnDataSourceChanged(true);
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x000C6C34 File Offset: 0x000C5C34
		private void OnSchemaRefreshed(object sender, EventArgs e)
		{
			this.OnSchemaRefreshed();
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x000C6C3C File Offset: 0x000C5C3C
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["DataMember"];
			propertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[]
			{
				new TypeConverterAttribute(typeof(DataMemberConverter))
			});
			properties["DataMember"] = propertyDescriptor;
			propertyDescriptor = (PropertyDescriptor)properties["DataSource"];
			propertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[]
			{
				new TypeConverterAttribute(typeof(DataSourceConverter))
			});
			properties["DataSource"] = propertyDescriptor;
			propertyDescriptor = (PropertyDescriptor)properties["DataSourceID"];
			propertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[]
			{
				new TypeConverterAttribute(typeof(DataSourceIDConverter))
			});
			properties["DataSourceID"] = propertyDescriptor;
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x000C6D1C File Offset: 0x000C5D1C
		IEnumerable IDataSourceProvider.GetResolvedSelectedDataSource()
		{
			IEnumerable enumerable = null;
			DataBinding dataBinding = base.DataBindings["DataSource"];
			if (dataBinding != null)
			{
				enumerable = DesignTimeData.GetSelectedDataSource((DataBoundControl)base.Component, dataBinding.Expression, this.DataMember);
			}
			return enumerable;
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x000C6D60 File Offset: 0x000C5D60
		object IDataSourceProvider.GetSelectedDataSource()
		{
			object obj = null;
			DataBinding dataBinding = base.DataBindings["DataSource"];
			if (dataBinding != null)
			{
				obj = DesignTimeData.GetSelectedDataSource((DataBoundControl)base.Component, dataBinding.Expression);
			}
			return obj;
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06002500 RID: 9472 RVA: 0x000C6D9C File Offset: 0x000C5D9C
		bool IDataBindingSchemaProvider.CanRefreshSchema
		{
			get
			{
				IDataSourceDesigner dataSourceDesigner = this.DataSourceDesigner;
				return dataSourceDesigner != null && dataSourceDesigner.CanRefreshSchema;
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06002501 RID: 9473 RVA: 0x000C6DBC File Offset: 0x000C5DBC
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

		// Token: 0x06002502 RID: 9474 RVA: 0x000C6DDC File Offset: 0x000C5DDC
		void IDataBindingSchemaProvider.RefreshSchema(bool preferSilent)
		{
			IDataSourceDesigner dataSourceDesigner = this.DataSourceDesigner;
			if (dataSourceDesigner != null)
			{
				dataSourceDesigner.RefreshSchema(preferSilent);
			}
		}

		// Token: 0x0400195C RID: 6492
		private IDataSourceDesigner _dataSourceDesigner;
	}
}
