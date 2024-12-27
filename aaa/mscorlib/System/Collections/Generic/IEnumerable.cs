using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	// Token: 0x02000023 RID: 35
	[TypeDependency("System.SZArrayHelper")]
	public interface IEnumerable<T> : IEnumerable
	{
		// Token: 0x06000137 RID: 311
		IEnumerator<T> GetEnumerator();
	}
}
