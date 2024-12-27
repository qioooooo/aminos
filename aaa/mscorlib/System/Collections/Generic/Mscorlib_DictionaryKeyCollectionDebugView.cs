using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x02000287 RID: 647
	internal sealed class Mscorlib_DictionaryKeyCollectionDebugView<TKey, TValue>
	{
		// Token: 0x060019DC RID: 6620 RVA: 0x00043FEC File Offset: 0x00042FEC
		public Mscorlib_DictionaryKeyCollectionDebugView(ICollection<TKey> collection)
		{
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			this.collection = collection;
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x060019DD RID: 6621 RVA: 0x00044004 File Offset: 0x00043004
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public TKey[] Items
		{
			get
			{
				TKey[] array = new TKey[this.collection.Count];
				this.collection.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x040009DD RID: 2525
		private ICollection<TKey> collection;
	}
}
