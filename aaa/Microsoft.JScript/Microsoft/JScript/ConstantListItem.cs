using System;

namespace Microsoft.JScript
{
	// Token: 0x0200004D RID: 77
	internal class ConstantListItem
	{
		// Token: 0x060003AA RID: 938 RVA: 0x00016F84 File Offset: 0x00015F84
		internal ConstantListItem(object term, ConstantListItem prev)
		{
			this.prev = prev;
			this.term = term;
		}

		// Token: 0x040001DB RID: 475
		internal ConstantListItem prev;

		// Token: 0x040001DC RID: 476
		internal object term;
	}
}
