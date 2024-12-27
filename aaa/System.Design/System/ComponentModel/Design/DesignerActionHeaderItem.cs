using System;

namespace System.ComponentModel.Design
{
	// Token: 0x02000100 RID: 256
	public sealed class DesignerActionHeaderItem : DesignerActionTextItem
	{
		// Token: 0x06000A89 RID: 2697 RVA: 0x00029056 File Offset: 0x00028056
		public DesignerActionHeaderItem(string displayName)
			: base(displayName, displayName)
		{
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00029060 File Offset: 0x00028060
		public DesignerActionHeaderItem(string displayName, string category)
			: base(displayName, category)
		{
		}
	}
}
