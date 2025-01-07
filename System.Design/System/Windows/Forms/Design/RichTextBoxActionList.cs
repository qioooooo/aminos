using System;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class RichTextBoxActionList : DesignerActionList
	{
		public RichTextBoxActionList(RichTextBoxDesigner designer)
			: base(designer.Component)
		{
			this._designer = designer;
		}

		public void EditLines()
		{
			EditorServiceContext.EditValue(this._designer, base.Component, "Lines");
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			return new DesignerActionItemCollection
			{
				new DesignerActionMethodItem(this, "EditLines", SR.GetString("EditLinesDisplayName"), SR.GetString("LinksCategoryName"), SR.GetString("EditLinesDescription"), true)
			};
		}

		private RichTextBoxDesigner _designer;
	}
}
