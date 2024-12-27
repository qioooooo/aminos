using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Win32
{
	// Token: 0x0200037A RID: 890
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(false)]
	[Guid("79eac9ee-baf9-11ce-8c82-00aa004ba90b")]
	[ComImport]
	internal interface IInternetSecurityManager
	{
		// Token: 0x06001BDA RID: 7130
		unsafe void SetSecuritySite(void* pSite);

		// Token: 0x06001BDB RID: 7131
		unsafe void GetSecuritySite(void** ppSite);

		// Token: 0x06001BDC RID: 7132
		[SuppressUnmanagedCodeSecurity]
		void MapUrlToZone([MarshalAs(UnmanagedType.BStr)] [In] string pwszUrl, out int pdwZone, [In] int dwFlags);

		// Token: 0x06001BDD RID: 7133
		unsafe void GetSecurityId(string pwszUrl, byte* pbSecurityId, int* pcbSecurityId, int dwReserved);

		// Token: 0x06001BDE RID: 7134
		unsafe void ProcessUrlAction(string pwszUrl, int dwAction, byte* pPolicy, int cbPolicy, byte* pContext, int cbContext, int dwFlags, int dwReserved);

		// Token: 0x06001BDF RID: 7135
		unsafe void QueryCustomPolicy(string pwszUrl, void* guidKey, byte** ppPolicy, int* pcbPolicy, byte* pContext, int cbContext, int dwReserved);

		// Token: 0x06001BE0 RID: 7136
		void SetZoneMapping(int dwZone, string lpszPattern, int dwFlags);

		// Token: 0x06001BE1 RID: 7137
		unsafe void GetZoneMappings(int dwZone, void** ppenumString, int dwFlags);
	}
}
