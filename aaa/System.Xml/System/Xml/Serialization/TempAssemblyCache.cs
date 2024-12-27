using System;
using System.Collections;

namespace System.Xml.Serialization
{
	// Token: 0x020002B6 RID: 694
	internal class TempAssemblyCache
	{
		// Token: 0x170007EE RID: 2030
		internal TempAssembly this[string ns, object o]
		{
			get
			{
				return (TempAssembly)this.cache[new TempAssemblyCacheKey(ns, o)];
			}
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x0009D854 File Offset: 0x0009C854
		internal void Add(string ns, object o, TempAssembly assembly)
		{
			TempAssemblyCacheKey tempAssemblyCacheKey = new TempAssemblyCacheKey(ns, o);
			lock (this)
			{
				if (this.cache[tempAssemblyCacheKey] != assembly)
				{
					Hashtable hashtable = new Hashtable();
					foreach (object obj in this.cache.Keys)
					{
						hashtable.Add(obj, this.cache[obj]);
					}
					this.cache = hashtable;
					this.cache[tempAssemblyCacheKey] = assembly;
				}
			}
		}

		// Token: 0x04001446 RID: 5190
		private Hashtable cache = new Hashtable();
	}
}
