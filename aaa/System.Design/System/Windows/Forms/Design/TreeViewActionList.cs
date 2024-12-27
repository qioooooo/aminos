using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002DC RID: 732
	internal class TreeViewActionList : DesignerActionList
	{
		// Token: 0x06001C40 RID: 7232 RVA: 0x0009F2F0 File Offset: 0x0009E2F0
		public TreeViewActionList(TreeViewDesigner designer)
			: base(designer.Component)
		{
			this._designer = designer;
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x0009F305 File Offset: 0x0009E305
		public void InvokeNodesDialog()
		{
			EditorServiceContext.EditValue(this._designer, base.Component, "Nodes");
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06001C42 RID: 7234 RVA: 0x0009F31E File Offset: 0x0009E31E
		// (set) Token: 0x06001C43 RID: 7235 RVA: 0x0009F330 File Offset: 0x0009E330
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

		// Token: 0x06001C44 RID: 7236 RVA: 0x0009F354 File Offset: 0x0009E354
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			return new DesignerActionItemCollection
			{
				new DesignerActionMethodItem(this, "InvokeNodesDialog", SR.GetString("InvokeNodesDialogDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("InvokeNodesDialogDescription"), true),
				new DesignerActionPropertyItem("ImageList", SR.GetString("ImageListDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("ImageListDescription"))
			};
		}

		// Token: 0x040015DE RID: 5598
		private TreeViewDesigner _designer;
	}
}
