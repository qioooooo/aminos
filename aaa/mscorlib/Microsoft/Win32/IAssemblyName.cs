using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	// Token: 0x0200042D RID: 1069
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("CD193BC0-B4BC-11d2-9833-00C04FC31D2E")]
	[ComImport]
	internal interface IAssemblyName
	{
		// Token: 0x06002C17 RID: 11287
		[PreserveSig]
		int SetProperty(uint PropertyId, IntPtr pvProperty, uint cbProperty);

		// Token: 0x06002C18 RID: 11288
		[PreserveSig]
		int GetProperty(uint PropertyId, IntPtr pvProperty, ref uint pcbProperty);

		// Token: 0x06002C19 RID: 11289
		[PreserveSig]
		int Finalize();

		// Token: 0x06002C1A RID: 11290
		[PreserveSig]
		int GetDisplayName(IntPtr szDisplayName, ref uint pccDisplayName, uint dwDisplayFlags);

		// Token: 0x06002C1B RID: 11291
		[PreserveSig]
		int BindToObject(object refIID, object pAsmBindSink, IApplicationContext pApplicationContext, [MarshalAs(UnmanagedType.LPWStr)] string szCodeBase, long llFlags, int pvReserved, uint cbReserved, out int ppv);

		// Token: 0x06002C1C RID: 11292
		[PreserveSig]
		int GetName(out uint lpcwBuffer, out int pwzName);

		// Token: 0x06002C1D RID: 11293
		[PreserveSig]
		int GetVersion(out uint pdwVersionHi, out uint pdwVersionLow);

		// Token: 0x06002C1E RID: 11294
		[PreserveSig]
		int IsEqual(IAssemblyName pName, uint dwCmpFlags);

		// Token: 0x06002C1F RID: 11295
		[PreserveSig]
		int Clone(out IAssemblyName pName);
	}
}
