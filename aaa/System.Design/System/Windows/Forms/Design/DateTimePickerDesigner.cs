using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001F9 RID: 505
	internal class DateTimePickerDesigner : ControlDesigner
	{
		// Token: 0x0600134D RID: 4941 RVA: 0x000628B6 File Offset: 0x000618B6
		public DateTimePickerDesigner()
		{
			base.AutoResizeHandles = true;
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x0600134E RID: 4942 RVA: 0x000628C8 File Offset: 0x000618C8
		public override SelectionRules SelectionRules
		{
			get
			{
				SelectionRules selectionRules = base.SelectionRules;
				return selectionRules & ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x0600134F RID: 4943 RVA: 0x000628E4 File Offset: 0x000618E4
		public override IList SnapLines
		{
			get
			{
				ArrayList arrayList = base.SnapLines as ArrayList;
				int num = DesignerUtils.GetTextBaseline(this.Control, ContentAlignment.MiddleLeft);
				num += 2;
				arrayList.Add(new SnapLine(SnapLineType.Baseline, num, SnapLinePriority.Medium));
				return arrayList;
			}
		}
	}
}
