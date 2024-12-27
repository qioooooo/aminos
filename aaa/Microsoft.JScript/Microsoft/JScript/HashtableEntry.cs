using System;

namespace Microsoft.JScript
{
	// Token: 0x02000114 RID: 276
	internal sealed class HashtableEntry
	{
		// Token: 0x06000B73 RID: 2931 RVA: 0x00057594 File Offset: 0x00056594
		internal HashtableEntry(object key, object value, uint hashCode, HashtableEntry next)
		{
			this.key = key;
			this.value = value;
			this.hashCode = hashCode;
			this.next = next;
		}

		// Token: 0x040006E5 RID: 1765
		internal object key;

		// Token: 0x040006E6 RID: 1766
		internal object value;

		// Token: 0x040006E7 RID: 1767
		internal uint hashCode;

		// Token: 0x040006E8 RID: 1768
		internal HashtableEntry next;
	}
}
