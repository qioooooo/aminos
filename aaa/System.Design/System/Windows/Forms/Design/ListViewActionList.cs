using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000265 RID: 613
	internal class ListViewActionList : DesignerActionList
	{
		// Token: 0x06001731 RID: 5937 RVA: 0x00077BB0 File Offset: 0x00076BB0
		public ListViewActionList(ComponentDesigner designer)
			: base(designer.Component)
		{
			this._designer = designer;
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x00077BC5 File Offset: 0x00076BC5
		public void InvokeItemsDialog()
		{
			EditorServiceContext.EditValue(this._designer, base.Component, "Items");
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x00077BDE File Offset: 0x00076BDE
		public void InvokeColumnsDialog()
		{
			EditorServiceContext.EditValue(this._designer, base.Component, "Columns");
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x00077BF7 File Offset: 0x00076BF7
		public void InvokeGroupsDialog()
		{
			EditorServiceContext.EditValue(this._designer, base.Component, "Groups");
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06001735 RID: 5941 RVA: 0x00077C10 File Offset: 0x00076C10
		// (set) Token: 0x06001736 RID: 5942 RVA: 0x00077C22 File Offset: 0x00076C22
		public View View
		{
			get
			{
				return ((ListView)base.Component).View;
			}
			set
			{
				TypeDescriptor.GetProperties(base.Component)["View"].SetValue(base.Component, value);
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06001737 RID: 5943 RVA: 0x00077C4A File Offset: 0x00076C4A
		// (set) Token: 0x06001738 RID: 5944 RVA: 0x00077C5C File Offset: 0x00076C5C
		public ImageList LargeImageList
		{
			get
			{
				return ((ListView)base.Component).LargeImageList;
			}
			set
			{
				TypeDescriptor.GetProperties(base.Component)["LargeImageList"].SetValue(base.Component, value);
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06001739 RID: 5945 RVA: 0x00077C7F File Offset: 0x00076C7F
		// (set) Token: 0x0600173A RID: 5946 RVA: 0x00077C91 File Offset: 0x00076C91
		public ImageList SmallImageList
		{
			get
			{
				return ((ListView)base.Component).SmallImageList;
			}
			set
			{
				TypeDescriptor.GetProperties(base.Component)["SmallImageList"].SetValue(base.Component, value);
			}
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x00077CB4 File Offset: 0x00076CB4
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			return new DesignerActionItemCollection
			{
				new DesignerActionMethodItem(this, "InvokeItemsDialog", SR.GetString("ListViewActionListEditItemsDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("ListViewActionListEditItemsDescription"), true),
				new DesignerActionMethodItem(this, "InvokeColumnsDialog", SR.GetString("ListViewActionListEditColumnsDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("ListViewActionListEditColumnsDescription"), true),
				new DesignerActionMethodItem(this, "InvokeGroupsDialog", SR.GetString("ListViewActionListEditGroupsDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("ListViewActionListEditGroupsDescription"), true),
				new DesignerActionPropertyItem("View", SR.GetString("ListViewActionListViewDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("ListViewActionListViewDescription")),
				new DesignerActionPropertyItem("SmallImageList", SR.GetString("ListViewActionListSmallImagesDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("ListViewActionListSmallImagesDescription")),
				new DesignerActionPropertyItem("LargeImageList", SR.GetString("ListViewActionListLargeImagesDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("ListViewActionListLargeImagesDescription"))
			};
		}

		// Token: 0x04001321 RID: 4897
		private ComponentDesigner _designer;
	}
}
