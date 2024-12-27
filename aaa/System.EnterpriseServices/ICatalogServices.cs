using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200007F RID: 127
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("04C6BE1E-1DB1-4058-AB7A-700CCCFBF254")]
	[ComImport]
	internal interface ICatalogServices
	{
		// Token: 0x060002D2 RID: 722
		[AutoComplete(true)]
		void Autodone();

		// Token: 0x060002D3 RID: 723
		[AutoComplete(false)]
		void NotAutodone();
	}
}
