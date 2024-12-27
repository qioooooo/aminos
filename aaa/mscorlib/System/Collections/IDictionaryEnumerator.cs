using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x0200024E RID: 590
	[ComVisible(true)]
	public interface IDictionaryEnumerator : IEnumerator
	{
		// Token: 0x1700035E RID: 862
		// (get) Token: 0x060017AF RID: 6063
		object Key { get; }

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x060017B0 RID: 6064
		object Value { get; }

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x060017B1 RID: 6065
		DictionaryEntry Entry { get; }
	}
}
