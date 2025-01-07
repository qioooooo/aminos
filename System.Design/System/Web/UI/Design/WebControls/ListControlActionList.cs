using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	internal class ListControlActionList : DesignerActionList
	{
		public ListControlActionList(ListControlDesigner listControlDesigner, IDataSourceDesigner dataSourceDesigner)
			: base(listControlDesigner.Component)
		{
			this._listControlDesigner = listControlDesigner;
			this._dataSourceDesigner = dataSourceDesigner;
		}

		public bool AutoPostBack
		{
			get
			{
				return ((ListControl)this._listControlDesigner.Component).AutoPostBack;
			}
			set
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._listControlDesigner.Component)["AutoPostBack"];
				propertyDescriptor.SetValue(this._listControlDesigner.Component, value);
			}
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

		public void EditItems()
		{
			this._listControlDesigner.EditItems();
		}

		public void ConnectToDataSource()
		{
			this._listControlDesigner.ConnectToDataSourceAction();
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this._listControlDesigner.Component);
			PropertyDescriptor propertyDescriptor = properties["DataSourceID"];
			if (propertyDescriptor != null && propertyDescriptor.IsBrowsable)
			{
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ConnectToDataSource", SR.GetString("ListControl_ConfigureDataVerb"), SR.GetString("BaseDataBoundControl_DataActionGroup"), SR.GetString("BaseDataBoundControl_ConfigureDataVerbDesc")));
			}
			ControlDesigner controlDesigner = this._dataSourceDesigner as ControlDesigner;
			if (controlDesigner != null)
			{
				((DesignerActionMethodItem)designerActionItemCollection[0]).RelatedComponent = controlDesigner.Component;
			}
			propertyDescriptor = properties["Items"];
			if (propertyDescriptor != null && propertyDescriptor.IsBrowsable)
			{
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "EditItems", SR.GetString("ListControl_EditItems"), "Actions", SR.GetString("ListControl_EditItemsDesc")));
			}
			propertyDescriptor = properties["AutoPostBack"];
			if (propertyDescriptor != null && propertyDescriptor.IsBrowsable)
			{
				designerActionItemCollection.Add(new DesignerActionPropertyItem("AutoPostBack", SR.GetString("ListControl_EnableAutoPostBack"), "Behavior", SR.GetString("ListControl_EnableAutoPostBackDesc")));
			}
			return designerActionItemCollection;
		}

		private IDataSourceDesigner _dataSourceDesigner;

		private ListControlDesigner _listControlDesigner;
	}
}
