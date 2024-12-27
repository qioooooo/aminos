using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x0200028A RID: 650
	internal sealed class Mscorlib_KeyedCollectionDebugView<K, T>
	{
		// Token: 0x060019E2 RID: 6626 RVA: 0x000440B8 File Offset: 0x000430B8
		public Mscorlib_KeyedCollectionDebugView(KeyedCollection<K, T> keyedCollection)
		{
			if (keyedCollection == null)
			{
				throw new ArgumentNullException("keyedCollection");
			}
			this.kc = keyedCollection;
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x060019E3 RID: 6627 RVA: 0x000440D8 File Offset: 0x000430D8
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				T[] array = new T[this.kc.Count];
				this.kc.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x040009E0 RID: 2528
		private KeyedCollection<K, T> kc;
	}
}
