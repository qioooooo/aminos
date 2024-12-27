using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Resources
{
	// Token: 0x0200041A RID: 1050
	[ComVisible(true)]
	public interface IResourceReader : IEnumerable, IDisposable
	{
		// Token: 0x06002B76 RID: 11126
		void Close();

		// Token: 0x06002B77 RID: 11127
		IDictionaryEnumerator GetEnumerator();
	}
}
