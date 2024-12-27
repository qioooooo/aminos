using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000558 RID: 1368
	[Guid("496B0ABF-CDEE-11d3-88E8-00902754C43A")]
	internal interface IEnumerator
	{
		// Token: 0x0600337E RID: 13182
		bool MoveNext();

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x0600337F RID: 13183
		object Current { get; }

		// Token: 0x06003380 RID: 13184
		void Reset();
	}
}
