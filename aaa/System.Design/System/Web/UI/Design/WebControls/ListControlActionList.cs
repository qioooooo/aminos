using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200045E RID: 1118
	internal class ListControlActionList : DesignerActionList
	{
		// Token: 0x060028B4 RID: 10420 RVA: 0x000DF6D6 File Offset: 0x000DE6D6
		public ListControlActionList(ListControlDesigner listControlDesigner, IDataSourceDesigner dataSourceDesigner)
			: base(listControlDesigner.Component)
		{
			this._listControlDesigner = listControlDesigner;
			this._dataSourceDesigner = dataSourceDesigner;
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x060028B5 RID: 10421 RVA: 0x000DF6F2 File Offset: 0x000DE6F2
		// (set) Token: 0x060028B6 RID: 10422 RVA: 0x000DF70C File Offset: 0x000DE70C
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

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x060028B7 RID: 10423 RVA: 0x000DF74B File Offset: 0x000DE74B
		// (set) Token: 0x060028B8 RID: 10424 RVA: 0x000DF74E File Offset: 0x000DE74E
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

		// Token: 0x060028B9 RID: 10425 RVA: 0x000DF750 File Offset: 0x000DE750
		public void EditItems()
		{
			this._listControlDesigner.EditItems();
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x000DF75D File Offset: 0x000DE75D
		public void ConnectToDataSource()
		{
			this._listControlDesigner.ConnectToDataSourceAction();
		}

		// Token: 0x060028BB RID: 10427 RVA: 0x000DF76C File Offset: 0x000DE76C
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

		// Token: 0x04001C27 RID: 7207
		private IDataSourceDesigner _dataSourceDesigner;

		// Token: 0x04001C28 RID: 7208
		private ListControlDesigner _listControlDesigner;
	}
}
