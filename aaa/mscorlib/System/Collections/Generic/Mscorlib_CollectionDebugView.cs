using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x02000286 RID: 646
	internal sealed class Mscorlib_CollectionDebugView<T>
	{
		// Token: 0x060019DA RID: 6618 RVA: 0x00043FA5 File Offset: 0x00042FA5
		public Mscorlib_CollectionDebugView(ICollection<T> collection)
		{
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			this.collection = collection;
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x060019DB RID: 6619 RVA: 0x00043FC0 File Offset: 0x00042FC0
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				T[] array = new T[this.collection.Count];
				this.collection.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x040009DC RID: 2524
		private ICollection<T> collection;
	}
}
