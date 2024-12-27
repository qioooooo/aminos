using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002DD RID: 733
	internal class UpDownBaseDesigner : ControlDesigner
	{
		// Token: 0x06001C45 RID: 7237 RVA: 0x0009F3C8 File Offset: 0x0009E3C8
		public UpDownBaseDesigner()
		{
			base.AutoResizeHandles = true;
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001C46 RID: 7238 RVA: 0x0009F3D8 File Offset: 0x0009E3D8
		public override SelectionRules SelectionRules
		{
			get
			{
				SelectionRules selectionRules = base.SelectionRules;
				return selectionRules & ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001C47 RID: 7239 RVA: 0x0009F3F4 File Offset: 0x0009E3F4
		public override IList SnapLines
		{
			get
			{
				ArrayList arrayList = base.SnapLines as ArrayList;
				int num = DesignerUtils.GetTextBaseline(this.Control, ContentAlignment.TopLeft);
				BorderStyle borderStyle = BorderStyle.Fixed3D;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["BorderStyle"];
				if (propertyDescriptor != null)
				{
					borderStyle = (BorderStyle)propertyDescriptor.GetValue(base.Component);
				}
				if (borderStyle == BorderStyle.None)
				{
					num--;
				}
				else
				{
					num += 2;
				}
				arrayList.Add(new SnapLine(SnapLineType.Baseline, num, SnapLinePriority.Medium));
				return arrayList;
			}
		}
	}
}
