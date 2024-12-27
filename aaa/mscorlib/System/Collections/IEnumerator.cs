using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x02000011 RID: 17
	[ComVisible(true)]
	[Guid("496B0ABF-CDEE-11d3-88E8-00902754C43A")]
	public interface IEnumerator
	{
		// Token: 0x060000A9 RID: 169
		bool MoveNext();

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000AA RID: 170
		object Current { get; }

		// Token: 0x060000AB RID: 171
		void Reset();
	}
}
