using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200007F RID: 127
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("5B4FB971-B115-4DE1-AD97-E482E3BF6EE4")]
	[ComImport]
	internal interface ISpProperties
	{
		// Token: 0x06000243 RID: 579
		[PreserveSig]
		int SetPropertyNum([MarshalAs(UnmanagedType.LPWStr)] string pName, int lValue);

		// Token: 0x06000244 RID: 580
		[PreserveSig]
		int GetPropertyNum([MarshalAs(UnmanagedType.LPWStr)] string pName, out int plValue);

		// Token: 0x06000245 RID: 581
		[PreserveSig]
		int SetPropertyString([MarshalAs(UnmanagedType.LPWStr)] string pName, [MarshalAs(UnmanagedType.LPWStr)] string pValue);

		// Token: 0x06000246 RID: 582
		[PreserveSig]
		int GetPropertyString([MarshalAs(UnmanagedType.LPWStr)] string pName, [MarshalAs(UnmanagedType.LPWStr)] out string ppCoMemValue);
	}
}
