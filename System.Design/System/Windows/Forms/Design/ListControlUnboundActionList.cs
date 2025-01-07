using System;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class ListControlUnboundActionList : DesignerActionList
	{
		public ListControlUnboundActionList(ComponentDesigner designer)
			: base(designer.Component)
		{
			this._designer = designer;
		}

		public void InvokeItemsDialog()
		{
			EditorServiceContext.EditValue(this._designer, base.Component, "Items");
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			return new DesignerActionItemCollection
			{
				new DesignerActionMethodItem(this, "InvokeItemsDialog", SR.GetString("ListControlUnboundActionListEditItemsDisplayName"), SR.GetString("ItemsCategoryName"), SR.GetString("ListControlUnboundActionListEditItemsDescription"), true)
			};
		}

		private ComponentDesigner _designer;
	}
}
