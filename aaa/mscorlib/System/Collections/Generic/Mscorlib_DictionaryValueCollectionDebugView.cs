using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x02000288 RID: 648
	internal sealed class Mscorlib_DictionaryValueCollectionDebugView<TKey, TValue>
	{
		// Token: 0x060019DE RID: 6622 RVA: 0x00044030 File Offset: 0x00043030
		public Mscorlib_DictionaryValueCollectionDebugView(ICollection<TValue> collection)
		{
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			this.collection = collection;
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x060019DF RID: 6623 RVA: 0x00044048 File Offset: 0x00043048
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

		// Token: 0x040009DE RID: 2526
		private ICollection<TValue> collection;
	}
}
