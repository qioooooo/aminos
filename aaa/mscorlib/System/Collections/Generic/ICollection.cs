using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	// Token: 0x02000274 RID: 628
	[TypeDependency("System.SZArrayHelper")]
	public interface ICollection<T> : IEnumerable<T>, IEnumerable
	{
		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06001931 RID: 6449
		int Count { get; }

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06001932 RID: 6450
		bool IsReadOnly { get; }

		// Token: 0x06001933 RID: 6451
		void Add(T item);

		// Token: 0x06001934 RID: 6452
		void Clear();

		// Token: 0x06001935 RID: 6453
		bool Contains(T item);

		// Token: 0x06001936 RID: 6454
		void CopyTo(T[] array, int arrayIndex);

		// Token: 0x06001937 RID: 6455
		bool Remove(T item);
	}
}
