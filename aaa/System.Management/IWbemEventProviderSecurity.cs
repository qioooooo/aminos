using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x0200009C RID: 156
	[Guid("631F7D96-D993-11D2-B339-00105A1F4AAF")]
	[TypeLibType(512)]
	[InterfaceType(1)]
	[ComImport]
	internal interface IWbemEventProviderSecurity
	{
		// Token: 0x06000475 RID: 1141
		[PreserveSig]
		int AccessCheck_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszQueryLanguage, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszQuery, [In] int lSidLength, [In] ref byte pSid);
	}
}
