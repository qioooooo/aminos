using System;

namespace System.Collections.Generic
{
	// Token: 0x02000028 RID: 40
	public interface IEqualityComparer<T>
	{
		// Token: 0x060001FF RID: 511
		bool Equals(T x, T y);

		// Token: 0x06000200 RID: 512
		int GetHashCode(T obj);
	}
}
