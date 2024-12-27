using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x0200022E RID: 558
	internal sealed class System_DictionaryValueCollectionDebugView<TKey, TValue>
	{
		// Token: 0x06001293 RID: 4755 RVA: 0x0003E688 File Offset: 0x0003D688
		public System_DictionaryValueCollectionDebugView(ICollection<TValue> collection)
		{
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			this.collection = collection;
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001294 RID: 4756 RVA: 0x0003E6A0 File Offset: 0x0003D6A0
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public TValue[] Items
		{
			get
			{
				TValue[] array = new TValue[this.collection.Count];
				this.collection.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x040010CA RID: 4298
		private ICollection<TValue> collection;
	}
}
