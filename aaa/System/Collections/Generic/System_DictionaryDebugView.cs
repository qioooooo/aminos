using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x0200022C RID: 556
	internal sealed class System_DictionaryDebugView<K, V>
	{
		// Token: 0x0600128F RID: 4751 RVA: 0x0003E5F8 File Offset: 0x0003D5F8
		public System_DictionaryDebugView(IDictionary<K, V> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			this.dict = dictionary;
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001290 RID: 4752 RVA: 0x0003E618 File Offset: 0x0003D618
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public KeyValuePair<K, V>[] Items
		{
			get
			{
				KeyValuePair<K, V>[] array = new KeyValuePair<K, V>[this.dict.Count];
				this.dict.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x040010C8 RID: 4296
		private IDictionary<K, V> dict;
	}
}
