using System;
using System.Collections;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000025 RID: 37
	internal class ClientTypeCache
	{
		// Token: 0x1700002C RID: 44
		internal object this[Type key]
		{
			get
			{
				return this.cache[key];
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003178 File Offset: 0x00002178
		internal void Add(Type key, object value)
		{
			lock (this)
			{
				if (this.cache[key] != value)
				{
					Hashtable hashtable = new Hashtable();
					foreach (object obj in this.cache.Keys)
					{
						hashtable.Add(obj, this.cache[obj]);
					}
					this.cache = hashtable;
					this.cache[key] = value;
				}
			}
		}

		// Token: 0x0400023B RID: 571
		private Hashtable cache = new Hashtable();
	}
}
