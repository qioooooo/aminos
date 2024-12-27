using System;
using System.Collections;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007D9 RID: 2009
	internal sealed class NameCache
	{
		// Token: 0x0600476A RID: 18282 RVA: 0x000F5BA8 File Offset: 0x000F4BA8
		internal object GetCachedValue(string name)
		{
			this.name = name;
			return NameCache.ht[name];
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x000F5BBC File Offset: 0x000F4BBC
		internal void SetCachedValue(object value)
		{
			NameCache.ht[this.name] = value;
		}

		// Token: 0x0400243C RID: 9276
		private static Hashtable ht = new Hashtable();

		// Token: 0x0400243D RID: 9277
		private string name;
	}
}
