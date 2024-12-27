using System;

namespace System.Collections.Generic
{
	// Token: 0x02000293 RID: 659
	internal interface IArraySortHelper<TKey, TValue>
	{
		// Token: 0x06001A51 RID: 6737
		void Sort(TKey[] keys, TValue[] values, int index, int length, IComparer<TKey> comparer);
	}
}
