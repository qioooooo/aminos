using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000525 RID: 1317
	[Guid("496B0ABF-CDEE-11d3-88E8-00902754C43A")]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IEnumerator instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	internal interface UCOMIEnumerator
	{
		// Token: 0x060032F6 RID: 13046
		bool MoveNext();

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x060032F7 RID: 13047
		object Current { get; }

		// Token: 0x060032F8 RID: 13048
		void Reset();
	}
}
