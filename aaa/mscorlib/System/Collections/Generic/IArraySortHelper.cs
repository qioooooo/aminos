using System;

namespace System.Collections.Generic
{
	// Token: 0x02000290 RID: 656
	internal interface IArraySortHelper<TKey>
	{
		// Token: 0x06001A41 RID: 6721
		void Sort(TKey[] keys, int index, int length, IComparer<TKey> comparer);

		// Token: 0x06001A42 RID: 6722
		int BinarySearch(TKey[] keys, int index, int length, TKey value, IComparer<TKey> comparer);
	}
}
