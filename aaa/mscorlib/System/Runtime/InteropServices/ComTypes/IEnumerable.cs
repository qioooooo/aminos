using System;
using System.Collections;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000557 RID: 1367
	[Guid("496B0ABE-CDEE-11d3-88E8-00902754C43A")]
	internal interface IEnumerable
	{
		// Token: 0x0600337D RID: 13181
		[DispId(-4)]
		IEnumerator GetEnumerator();
	}
}
