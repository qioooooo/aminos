using System;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200025F RID: 607
	internal class ListControlUnboundActionList : DesignerActionList
	{
		// Token: 0x06001706 RID: 5894 RVA: 0x00076AE1 File Offset: 0x00075AE1
		public ListControlUnboundActionList(ComponentDesigner designer)
			: base(designer.Component)
		{
			this._designer = designer;
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x00076AF6 File Offset: 0x00075AF6
		public void InvokeItemsDialog()
		{
			EditorServiceContext.EditValue(this._designer, base.Component, "Items");
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x00076B10 File Offset: 0x00075B10
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			return new DesignerActionItemCollection
			{
				new DesignerActionMethodItem(this, "InvokeItemsDialog", SR.GetString("ListControlUnboundActionListEditItemsDisplayName"), SR.GetString("ItemsCategoryName"), SR.GetString("ListControlUnboundActionListEditItemsDescription"), true)
			};
		}

		// Token: 0x04001313 RID: 4883
		private ComponentDesigner _designer;
	}
}
