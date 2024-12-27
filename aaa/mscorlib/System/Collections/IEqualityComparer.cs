using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x02000027 RID: 39
	[ComVisible(true)]
	public interface IEqualityComparer
	{
		// Token: 0x060001FD RID: 509
		bool Equals(object x, object y);

		// Token: 0x060001FE RID: 510
		int GetHashCode(object obj);
	}
}
