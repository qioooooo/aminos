using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000455 RID: 1109
	public class HierarchicalDataBoundControlDesigner : BaseDataBoundControlDesigner
	{
		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x0600288B RID: 10379 RVA: 0x000DEF00 File Offset: 0x000DDF00
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				if (this.UseDataSourcePickerActionList)
				{
					designerActionListCollection.Add(new HierarchicalDataBoundControlActionList(this, this.DataSourceDesigner));
				}
				designerActionListCollection.AddRange(base.ActionLists);
				return designerActionListCollection;
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x0600288C RID: 10380 RVA: 0x000DEF3B File Offset: 0x000DDF3B
		public IHierarchicalDataSourceDesigner DataSourceDesigner
		{
			get
			{
				return this._dataSourceDesigner;
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x0600288D RID: 10381 RVA: 0x000DEF44 File Offset: 0x000DDF44
		public DesignerHierarchicalDataSourceView DesignerView
		{
			get
			{
				DesignerHierarchicalDataSourceView designerHierarchicalDataSourceView = null;
				if (this.DataSourceDesigner != null)
				{
					designerHierarchicalDataSourceView = this.DataSourceDesigner.GetView(string.Empty);
				}
				return designerHierarchicalDataSourceView;
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x0600288E RID: 10382 RVA: 0x000DEF6D File Offset: 0x000DDF6D
		protected virtual bool UseDataSourcePickerActionList
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600288F RID: 10383 RVA: 0x000DEF70 File Offset: 0x000DDF70
		protected override bool ConnectToDataSource()
		{
			IHierarchicalDataSourceDesigner dataSourceDesigner = this.GetDataSourceDesigner();
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

		// Token: 0x06002890 RID: 10384 RVA: 0x000DF003 File Offset: 0x000DE003
		protected override void CreateDataSource()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.CreateDataSourceCallback), null, SR.GetString("BaseDataBoundControl_CreateDataSourceTransaction"));
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x000DF028 File Offset: 0x000DE028
		private bool CreateDataSourceCallback(object context)
		{
			string text;
			DialogResult dialogResult = BaseDataBoundControlDesigner.ShowCreateDataSourceDialog(this, typeof(IHierarchicalDataSource), true, out text);
			if (text.Length > 0)
			{
				base.DataSourceID = text;
			}
			return dialogResult == DialogResult.OK;
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x000DF060 File Offset: 0x000DE060
		protected override void DataBind(BaseDataBoundControl dataBoundControl)
		{
			IHierarchicalEnumerable designTimeDataSource = this.GetDesignTimeDataSource();
			string dataSourceID = dataBoundControl.DataSourceID;
			object dataSource = dataBoundControl.DataSource;
			HierarchicalDataBoundControl hierarchicalDataBoundControl = (HierarchicalDataBoundControl)dataBoundControl;
			hierarchicalDataBoundControl.DataSource = designTimeDataSource;
			hierarchicalDataBoundControl.DataSourceID = string.Empty;
			try
			{
				if (designTimeDataSource != null)
				{
					hierarchicalDataBoundControl.DataBind();
				}
			}
			finally
			{
				hierarchicalDataBoundControl.DataSource = dataSource;
				hierarchicalDataBoundControl.DataSourceID = dataSourceID;
			}
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x000DF0C8 File Offset: 0x000DE0C8
		protected override void DisconnectFromDataSource()
		{
			if (this._dataSourceDesigner != null)
			{
				this._dataSourceDesigner.DataSourceChanged -= this.OnDataSourceChanged;
				this._dataSourceDesigner.SchemaRefreshed -= this.OnSchemaRefreshed;
				this._dataSourceDesigner = null;
			}
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x000DF108 File Offset: 0x000DE108
		private IHierarchicalDataSourceDesigner GetDataSourceDesigner()
		{
			IHierarchicalDataSourceDesigner hierarchicalDataSourceDesigner = null;
			string dataSourceID = base.DataSourceID;
			if (!string.IsNullOrEmpty(dataSourceID))
			{
				Control control = ControlHelper.FindControl(base.Component.Site, (Control)base.Component, dataSourceID);
				if (control != null && control.Site != null)
				{
					IDesignerHost designerHost = (IDesignerHost)control.Site.GetService(typeof(IDesignerHost));
					if (designerHost != null)
					{
						hierarchicalDataSourceDesigner = designerHost.GetDesigner(control) as IHierarchicalDataSourceDesigner;
					}
				}
			}
			return hierarchicalDataSourceDesigner;
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x000DF17C File Offset: 0x000DE17C
		protected virtual IHierarchicalEnumerable GetDesignTimeDataSource()
		{
			IHierarchicalEnumerable hierarchicalEnumerable = null;
			DesignerHierarchicalDataSourceView designerView = this.DesignerView;
			bool flag;
			if (designerView != null)
			{
				try
				{
					hierarchicalEnumerable = designerView.GetDesignTimeData(out flag);
					goto IL_00AC;
				}
				catch (Exception ex)
				{
					if (base.Component.Site != null)
					{
						IComponentDesignerDebugService componentDesignerDebugService = (IComponentDesignerDebugService)base.Component.Site.GetService(typeof(IComponentDesignerDebugService));
						if (componentDesignerDebugService != null)
						{
							componentDesignerDebugService.Fail(SR.GetString("DataSource_DebugService_FailedCall", new object[] { "DesignerHierarchicalDataSourceView.GetDesignTimeData", ex.Message }));
						}
					}
					goto IL_00AC;
				}
			}
			DataBinding dataBinding = base.DataBindings["DataSource"];
			if (dataBinding != null)
			{
				hierarchicalEnumerable = DesignTimeData.GetSelectedDataSource(base.Component, dataBinding.Expression, null) as IHierarchicalEnumerable;
			}
			IL_00AC:
			if (hierarchicalEnumerable != null)
			{
				ICollection collection = hierarchicalEnumerable as ICollection;
				if (collection == null || collection.Count > 0)
				{
					return hierarchicalEnumerable;
				}
			}
			flag = true;
			return this.GetSampleDataSource();
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x000DF268 File Offset: 0x000DE268
		protected virtual IHierarchicalEnumerable GetSampleDataSource()
		{
			return new HierarchicalDataBoundControlDesigner.HierarchicalSampleData(0, string.Empty);
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x000DF275 File Offset: 0x000DE275
		private void OnDataSourceChanged(object sender, EventArgs e)
		{
			this.OnDataSourceChanged(true);
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x000DF27E File Offset: 0x000DE27E
		private void OnSchemaRefreshed(object sender, EventArgs e)
		{
			this.OnSchemaRefreshed();
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x000DF288 File Offset: 0x000DE288
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["DataSource"];
			propertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[]
			{
				new TypeConverterAttribute(typeof(HierarchicalDataSourceConverter))
			});
			properties["DataSource"] = propertyDescriptor;
			propertyDescriptor = (PropertyDescriptor)properties["DataSourceID"];
			propertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[]
			{
				new TypeConverterAttribute(typeof(HierarchicalDataSourceIDConverter))
			});
			properties["DataSourceID"] = propertyDescriptor;
		}

		// Token: 0x04001C22 RID: 7202
		private IHierarchicalDataSourceDesigner _dataSourceDesigner;

		// Token: 0x02000456 RID: 1110
		private class HierarchicalSampleData : IHierarchicalEnumerable, IEnumerable
		{
			// Token: 0x0600289B RID: 10395 RVA: 0x000DF32C File Offset: 0x000DE32C
			public HierarchicalSampleData(int depth, string path)
			{
				this._list = new ArrayList();
				if (depth == 0)
				{
					this._list.Add(new HierarchicalDataBoundControlDesigner.HierarchicalSampleDataNode(SR.GetString("HierarchicalDataBoundControlDesigner_SampleRoot"), depth, path));
					return;
				}
				if (depth == 2)
				{
					this._list.Add(new HierarchicalDataBoundControlDesigner.HierarchicalSampleDataNode(SR.GetString("HierarchicalDataBoundControlDesigner_SampleLeaf", new object[] { 1 }), depth, path));
					this._list.Add(new HierarchicalDataBoundControlDesigner.HierarchicalSampleDataNode(SR.GetString("HierarchicalDataBoundControlDesigner_SampleLeaf", new object[] { 2 }), depth, path));
					return;
				}
				this._list.Add(new HierarchicalDataBoundControlDesigner.HierarchicalSampleDataNode(SR.GetString("HierarchicalDataBoundControlDesigner_SampleParent", new object[] { 1 }), depth, path));
				this._list.Add(new HierarchicalDataBoundControlDesigner.HierarchicalSampleDataNode(SR.GetString("HierarchicalDataBoundControlDesigner_SampleParent", new object[] { 2 }), depth, path));
			}

			// Token: 0x0600289C RID: 10396 RVA: 0x000DF428 File Offset: 0x000DE428
			public IEnumerator GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x0600289D RID: 10397 RVA: 0x000DF435 File Offset: 0x000DE435
			public IHierarchyData GetHierarchyData(object enumeratedItem)
			{
				return (IHierarchyData)enumeratedItem;
			}

			// Token: 0x04001C23 RID: 7203
			private ArrayList _list;
		}

		// Token: 0x02000457 RID: 1111
		private class HierarchicalSampleDataNode : IHierarchyData
		{
			// Token: 0x0600289E RID: 10398 RVA: 0x000DF43D File Offset: 0x000DE43D
			public HierarchicalSampleDataNode(string text, int depth, string path)
			{
				this._text = text;
				this._depth = depth;
				this._path = path + '\\' + text;
			}

			// Token: 0x17000781 RID: 1921
			// (get) Token: 0x0600289F RID: 10399 RVA: 0x000DF467 File Offset: 0x000DE467
			public bool HasChildren
			{
				get
				{
					return this._depth < 2;
				}
			}

			// Token: 0x17000782 RID: 1922
			// (get) Token: 0x060028A0 RID: 10400 RVA: 0x000DF475 File Offset: 0x000DE475
			public string Path
			{
				get
				{
					return this._path;
				}
			}

			// Token: 0x17000783 RID: 1923
			// (get) Token: 0x060028A1 RID: 10401 RVA: 0x000DF47D File Offset: 0x000DE47D
			public object Item
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000784 RID: 1924
			// (get) Token: 0x060028A2 RID: 10402 RVA: 0x000DF480 File Offset: 0x000DE480
			public string Type
			{
				get
				{
					return "SampleData";
				}
			}

			// Token: 0x060028A3 RID: 10403 RVA: 0x000DF487 File Offset: 0x000DE487
			public override string ToString()
			{
				return this._text;
			}

			// Token: 0x060028A4 RID: 10404 RVA: 0x000DF48F File Offset: 0x000DE48F
			public IHierarchicalEnumerable GetChildren()
			{
				return new HierarchicalDataBoundControlDesigner.HierarchicalSampleData(this._depth + 1, this._path);
			}

			// Token: 0x060028A5 RID: 10405 RVA: 0x000DF4A4 File Offset: 0x000DE4A4
			public IHierarchyData GetParent()
			{
				return null;
			}

			// Token: 0x04001C24 RID: 7204
			private string _text;

			// Token: 0x04001C25 RID: 7205
			private int _depth;

			// Token: 0x04001C26 RID: 7206
			private string _path;
		}
	}
}
