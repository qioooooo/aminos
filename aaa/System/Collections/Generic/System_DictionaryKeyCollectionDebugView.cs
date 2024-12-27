using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x0200022D RID: 557
	internal sealed class System_DictionaryKeyCollectionDebugView<TKey, TValue>
	{
		// Token: 0x06001291 RID: 4753 RVA: 0x0003E644 File Offset: 0x0003D644
		public System_DictionaryKeyCollectionDebugView(ICollection<TKey> collection)
		{
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			this.collection = collection;
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001292 RID: 4754 RVA: 0x0003E65C File Offset: 0x0003D65C
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

		// Token: 0x040010C9 RID: 4297
		private ICollection<TKey> collection;
	}
}
