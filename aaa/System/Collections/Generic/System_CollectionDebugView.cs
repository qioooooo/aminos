using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x02000229 RID: 553
	internal sealed class System_CollectionDebugView<T>
	{
		// Token: 0x06001289 RID: 4745 RVA: 0x0003E55A File Offset: 0x0003D55A
		public System_CollectionDebugView(ICollection<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.collection = collection;
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x0600128A RID: 4746 RVA: 0x0003E578 File Offset: 0x0003D578
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

		// Token: 0x040010C5 RID: 4293
		private ICollection<T> collection;
	}
}
