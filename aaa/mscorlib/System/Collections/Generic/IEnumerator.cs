using System;

namespace System.Collections.Generic
{
	// Token: 0x02000016 RID: 22
	public interface IEnumerator<T> : IDisposable, IEnumerator
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000C6 RID: 198
		T Current { get; }
	}
}
