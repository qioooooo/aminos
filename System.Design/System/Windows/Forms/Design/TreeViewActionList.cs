using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class TreeViewActionList : DesignerActionList
	{
		public TreeViewActionList(TreeViewDesigner designer)
			: base(designer.Component)
		{
			this._designer = designer;
		}

		public void InvokeNodesDialog()
		{
			EditorServiceContext.EditValue(this._designer, base.Component, "Nodes");
		}

		public ImageList ImageList
		{
			get
			{
				return ((TreeView)base.Component).ImageList;
			}
			set
			{
				TypeDescriptor.GetProperties(base.Component)["ImageList"].SetValue(base.Component, value);
			}
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			return new DesignerActionItemCollection
			{
				new DesignerActionMethodItem(this, "InvokeNodesDialog", SR.GetString("InvokeNodesDialogDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("InvokeNodesDialogDescription"), true),
				new DesignerActionPropertyItem("ImageList", SR.GetString("ImageListDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("ImageListDescription"))
			};
		}

		private TreeViewDesigner _designer;
	}
}
