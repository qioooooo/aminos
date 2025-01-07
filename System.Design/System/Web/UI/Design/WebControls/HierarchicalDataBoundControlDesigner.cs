using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	public class HierarchicalDataBoundControlDesigner : BaseDataBoundControlDesigner
	{
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

		public IHierarchicalDataSourceDesigner DataSourceDesigner
		{
			get
			{
				return this._dataSourceDesigner;
			}
		}

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

		protected virtual bool UseDataSourcePickerActionList
		{
			get
			{
				return true;
			}
		}

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

		protected override void CreateDataSource()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.CreateDataSourceCallback), null, SR.GetString("BaseDataBoundControl_CreateDataSourceTransaction"));
		}

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

		protected override void DisconnectFromDataSource()
		{
			if (this._dataSourceDesigner != null)
			{
				this._dataSourceDesigner.DataSourceChanged -= this.OnDataSourceChanged;
				this._dataSourceDesigner.SchemaRefreshed -= this.OnSchemaRefreshed;
				this._dataSourceDesigner = null;
			}
		}

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

		protected virtual IHierarchicalEnumerable GetSampleDataSource()
		{
			return new HierarchicalDataBoundControlDesigner.HierarchicalSampleData(0, string.Empty);
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

		private IHierarchicalDataSourceDesigner _dataSourceDesigner;

		private class HierarchicalSampleData : IHierarchicalEnumerable, IEnumerable
		{
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

			public IEnumerator GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			public IHierarchyData GetHierarchyData(object enumeratedItem)
			{
				return (IHierarchyData)enumeratedItem;
			}

			private ArrayList _list;
		}

		private class HierarchicalSampleDataNode : IHierarchyData
		{
			public HierarchicalSampleDataNode(string text, int depth, string path)
			{
				this._text = text;
				this._depth = depth;
				this._path = path + '\\' + text;
			}

			public bool HasChildren
			{
				get
				{
					return this._depth < 2;
				}
			}

			public string Path
			{
				get
				{
					return this._path;
				}
			}

			public object Item
			{
				get
				{
					return this;
				}
			}

			public string Type
			{
				get
				{
					return "SampleData";
				}
			}

			public override string ToString()
			{
				return this._text;
			}

			public IHierarchicalEnumerable GetChildren()
			{
				return new HierarchicalDataBoundControlDesigner.HierarchicalSampleData(this._depth + 1, this._path);
			}

			public IHierarchyData GetParent()
			{
				return null;
			}

			private string _text;

			private int _depth;

			private string _path;
		}
	}
}
