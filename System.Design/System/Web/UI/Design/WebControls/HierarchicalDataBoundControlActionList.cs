using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Web.UI.Design.WebControls
{
	internal class HierarchicalDataBoundControlActionList : DesignerActionList
	{
		public HierarchicalDataBoundControlActionList(HierarchicalDataBoundControlDesigner controlDesigner, IHierarchicalDataSourceDesigner dataSourceDesigner)
			: base(controlDesigner.Component)
		{
			this._controlDesigner = controlDesigner;
			this._dataSourceDesigner = dataSourceDesigner;
		}

		public override bool AutoShow
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		[TypeConverter(typeof(HierarchicalDataSourceIDConverter))]
		public string DataSourceID
		{
			get
			{
				string dataSourceID = this._controlDesigner.DataSourceID;
				if (string.IsNullOrEmpty(dataSourceID))
				{
					return SR.GetString("DataSourceIDChromeConverter_NoDataSource");
				}
				return dataSourceID;
			}
			set
			{
				this._controlDesigner.DataSourceID = value;
			}
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._controlDesigner.Component)["DataSourceID"];
			if (propertyDescriptor != null && propertyDescriptor.IsBrowsable)
			{
				designerActionItemCollection.Add(new DesignerActionPropertyItem("DataSourceID", SR.GetString("BaseDataBoundControl_ConfigureDataVerb"), SR.GetString("BaseDataBoundControl_DataActionGroup"), SR.GetString("BaseDataBoundControl_ConfigureDataVerbDesc")));
			}
			ControlDesigner controlDesigner = this._dataSourceDesigner as ControlDesigner;
			if (controlDesigner != null)
			{
				((DesignerActionPropertyItem)designerActionItemCollection[0]).RelatedComponent = controlDesigner.Component;
			}
			return designerActionItemCollection;
		}

		private IHierarchicalDataSourceDesigner _dataSourceDesigner;

		private HierarchicalDataBoundControlDesigner _controlDesigner;
	}
}
