using System;

namespace System.Windows.Forms.Design
{
	internal class MonthCalendarDesigner : ControlDesigner
	{
		public MonthCalendarDesigner()
		{
			base.AutoResizeHandles = true;
		}

		public override SelectionRules SelectionRules
		{
			get
			{
				SelectionRules selectionRules = base.SelectionRules;
				if (this.Control.Parent == null || (this.Control.Parent != null && !this.Control.Parent.IsMirrored))
				{
					selectionRules &= ~(SelectionRules.TopSizeable | SelectionRules.LeftSizeable);
				}
				else
				{
					selectionRules &= ~(SelectionRules.TopSizeable | SelectionRules.RightSizeable);
				}
				return selectionRules;
			}
		}
	}
}
