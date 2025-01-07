using System;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class NotifyIconActionList : DesignerActionList
	{
		public NotifyIconActionList(NotifyIconDesigner designer)
			: base(designer.Component)
		{
			this._designer = designer;
		}

		public void ChooseIcon()
		{
			EditorServiceContext.EditValue(this._designer, base.Component, "Icon");
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			return new DesignerActionItemCollection
			{
				new DesignerActionMethodItem(this, "ChooseIcon", SR.GetString("ChooseIconDisplayName"), true)
			};
		}

		private NotifyIconDesigner _designer;
	}
}
