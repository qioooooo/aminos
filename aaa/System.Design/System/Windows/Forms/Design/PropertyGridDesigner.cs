using System;
using System.Collections;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200027D RID: 637
	internal class PropertyGridDesigner : ControlDesigner
	{
		// Token: 0x060017C2 RID: 6082 RVA: 0x0007BA97 File Offset: 0x0007AA97
		public PropertyGridDesigner()
		{
			base.AutoResizeHandles = true;
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x0007BAA6 File Offset: 0x0007AAA6
		protected override void PreFilterProperties(IDictionary properties)
		{
			properties.Remove("AutoScroll");
			properties.Remove("AutoScrollMargin");
			properties.Remove("DockPadding");
			properties.Remove("AutoScrollMinSize");
			base.PreFilterProperties(properties);
		}
	}
}
