using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x02000289 RID: 649
	internal sealed class Mscorlib_DictionaryDebugView<K, V>
	{
		// Token: 0x060019E0 RID: 6624 RVA: 0x00044074 File Offset: 0x00043074
		public Mscorlib_DictionaryDebugView(IDictionary<K, V> dictionary)
		{
			if (dictionary == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
			}
			this.dict = dictionary;
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x060019E1 RID: 6625 RVA: 0x0004408C File Offset: 0x0004308C
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

		// Token: 0x040009DF RID: 2527
		private IDictionary<K, V> dict;
	}
}
