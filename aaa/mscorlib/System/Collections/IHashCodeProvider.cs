using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x02000246 RID: 582
	[Obsolete("Please use IEqualityComparer instead.")]
	[ComVisible(true)]
	public interface IHashCodeProvider
	{
		// Token: 0x06001746 RID: 5958
		int GetHashCode(object obj);
	}
}
