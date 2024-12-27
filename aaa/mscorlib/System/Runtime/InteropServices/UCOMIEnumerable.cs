using System;
using System.Collections;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000524 RID: 1316
	[Guid("496B0ABE-CDEE-11d3-88E8-00902754C43A")]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IEnumerable instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	internal interface UCOMIEnumerable
	{
		// Token: 0x060032F5 RID: 13045
		[DispId(-4)]
		IEnumerator GetEnumerator();
	}
}
