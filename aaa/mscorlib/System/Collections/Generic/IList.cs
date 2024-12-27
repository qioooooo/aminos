using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	// Token: 0x0200028B RID: 651
	[TypeDependency("System.SZArrayHelper")]
	public interface IList<T> : ICollection<T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x170003FA RID: 1018
		T this[int index] { get; set; }

		// Token: 0x060019E6 RID: 6630
		int IndexOf(T item);

		// Token: 0x060019E7 RID: 6631
		void Insert(int index, T item);

		// Token: 0x060019E8 RID: 6632
		void RemoveAt(int index);
	}
}
