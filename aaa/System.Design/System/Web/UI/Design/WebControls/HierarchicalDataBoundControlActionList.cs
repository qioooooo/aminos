using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000454 RID: 1108
	internal class HierarchicalDataBoundControlActionList : DesignerActionList
	{
		// Token: 0x06002885 RID: 10373 RVA: 0x000DEE13 File Offset: 0x000DDE13
		public HierarchicalDataBoundControlActionList(HierarchicalDataBoundControlDesigner controlDesigner, IHierarchicalDataSourceDesigner dataSourceDesigner)
			: base(controlDesigner.Component)
		{
			this._controlDesigner = controlDesigner;
			this._dataSourceDesigner = dataSourceDesigner;
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06002886 RID: 10374 RVA: 0x000DEE2F File Offset: 0x000DDE2F
		// (set) Token: 0x06002887 RID: 10375 RVA: 0x000DEE32 File Offset: 0x000DDE32
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

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06002888 RID: 10376 RVA: 0x000DEE34 File Offset: 0x000DDE34
		// (set) Token: 0x06002889 RID: 10377 RVA: 0x000DEE61 File Offset: 0x000DDE61
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

		// Token: 0x0600288A RID: 10378 RVA: 0x000DEE70 File Offset: 0x000DDE70
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

		// Token: 0x04001C20 RID: 7200
		private IHierarchicalDataSourceDesigner _dataSourceDesigner;

		// Token: 0x04001C21 RID: 7201
		private HierarchicalDataBoundControlDesigner _controlDesigner;
	}
}
