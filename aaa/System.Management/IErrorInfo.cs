using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x0200008C RID: 140
	[Guid("1CF2B120-547D-101B-8E65-08002B2BD119")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IErrorInfo
	{
		// Token: 0x06000437 RID: 1079
		Guid GetGUID();

		// Token: 0x06000438 RID: 1080
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetSource();

		// Token: 0x06000439 RID: 1081
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDescription();

		// Token: 0x0600043A RID: 1082
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetHelpFile();

		// Token: 0x0600043B RID: 1083
		uint GetHelpContext();
	}
}
