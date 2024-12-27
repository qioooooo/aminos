using System;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000276 RID: 630
	internal class MonthCalendarDesigner : ControlDesigner
	{
		// Token: 0x060017AA RID: 6058 RVA: 0x0007B1A4 File Offset: 0x0007A1A4
		public MonthCalendarDesigner()
		{
			base.AutoResizeHandles = true;
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x060017AB RID: 6059 RVA: 0x0007B1B4 File Offset: 0x0007A1B4
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
