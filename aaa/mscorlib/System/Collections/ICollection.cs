using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x02000009 RID: 9
	[ComVisible(true)]
	public interface ICollection : IEnumerable
	{
		// Token: 0x06000012 RID: 18
		void CopyTo(Array array, int index);

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000013 RID: 19
		int Count { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000014 RID: 20
		object SyncRoot { get; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000015 RID: 21
		bool IsSynchronized { get; }
	}
}
