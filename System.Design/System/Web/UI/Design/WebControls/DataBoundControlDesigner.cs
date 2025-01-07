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
	public class DataBoundControlDesigner : BaseDataBoundControlDesigner, IDataBindingSchemaProvider, IDataSourceProvider
	{
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

		public IDataSourceDesigner DataSourceDesigner
		{
			get
			{
				return this._dataSourceDesigner;
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

		protected virtual int SampleRowCount
		{
			get
			{
				return 5;
			}
		}

		protected virtual bool UseDataSourcePickerActionList
		{
			get
			{
				return true;
			}
		}

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

		protected override void CreateDataSource()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.CreateDataSourceCallback), null, SR.GetString("BaseDataBoundControl_CreateDataSourceTransaction"));
		}

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

		protected override void DisconnectFromDataSource()
		{
			if (this._dataSourceDesigner != null)
			{
				this._dataSourceDesigner.DataSourceChanged -= this.OnDataSourceChanged;
				this._dataSourceDesigner.SchemaRefreshed -= this.OnSchemaRefreshed;
				this._dataSourceDesigner = null;
			}
		}

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

		private void OnDataSourceChanged(object sender, EventArgs e)
		{
			this.OnDataSourceChanged(true);
		}

		private void OnSchemaRefreshed(object sender, EventArgs e)
		{
			this.OnSchemaRefreshed();
		}

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

		private IDataSourceDesigner _dataSourceDesigner;
	}
}
