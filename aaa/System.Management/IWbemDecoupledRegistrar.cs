using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000CE RID: 206
	[InterfaceType(1)]
	[Guid("1005CBCF-E64F-4646-BCD3-3A089D8A84B4")]
	[ComImport]
	internal interface IWbemDecoupledRegistrar
	{
		// Token: 0x0600061D RID: 1565
		[PreserveSig]
		int Register_([In] int flags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext context, [MarshalAs(UnmanagedType.LPWStr)] [In] string user, [MarshalAs(UnmanagedType.LPWStr)] [In] string locale, [MarshalAs(UnmanagedType.LPWStr)] [In] string scope, [MarshalAs(UnmanagedType.LPWStr)] [In] string registration, [MarshalAs(UnmanagedType.IUnknown)] [In] object unknown);

		// Token: 0x0600061E RID: 1566
		[PreserveSig]
		int UnRegister_();
	}
}
