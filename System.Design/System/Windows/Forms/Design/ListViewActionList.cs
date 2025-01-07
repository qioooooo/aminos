using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class ListViewActionList : DesignerActionList
	{
		public ListViewActionList(ComponentDesigner designer)
			: base(designer.Component)
		{
			this._designer = designer;
		}

		public void InvokeItemsDialog()
		{
			EditorServiceContext.EditValue(this._designer, base.Component, "Items");
		}

		public void InvokeColumnsDialog()
		{
			EditorServiceContext.EditValue(this._designer, base.Component, "Columns");
		}

		public void InvokeGroupsDialog()
		{
			EditorServiceContext.EditValue(this._designer, base.Component, "Groups");
		}

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

		private ComponentDesigner _designer;
	}
}
