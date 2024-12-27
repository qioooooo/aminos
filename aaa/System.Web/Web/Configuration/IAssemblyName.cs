using System;
using System.Runtime.InteropServices;

namespace System.Web.Configuration
{
	// Token: 0x02000203 RID: 515
	[Guid("CD193BC0-B4BC-11d2-9833-00C04FC31D2E")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IAssemblyName
	{
		// Token: 0x06001C07 RID: 7175
		[PreserveSig]
		int SetProperty(uint PropertyId, IntPtr pvProperty, uint cbProperty);

		// Token: 0x06001C08 RID: 7176
		[PreserveSig]
		int GetProperty(uint PropertyId, IntPtr pvProperty, ref uint pcbProperty);

		// Token: 0x06001C09 RID: 7177
		[PreserveSig]
		int Finalize();

		// Token: 0x06001C0A RID: 7178
		[PreserveSig]
		int GetDisplayName(IntPtr szDisplayName, ref uint pccDisplayName, uint dwDisplayFlags);

		// Token: 0x06001C0B RID: 7179
		[PreserveSig]
		int BindToObject(object refIID, object pAsmBindSink, IApplicationContext pApplicationContext, [MarshalAs(UnmanagedType.LPWStr)] string szCodeBase, long llFlags, int pvReserved, uint cbReserved, out int ppv);

		// Token: 0x06001C0C RID: 7180
		[PreserveSig]
		int GetName(out uint lpcwBuffer, out int pwzName);

		// Token: 0x06001C0D RID: 7181
		[PreserveSig]
		int GetVersion(out uint pdwVersionHi, out uint pdwVersionLow);

		// Token: 0x06001C0E RID: 7182
		[PreserveSig]
		int IsEqual(IAssemblyName pName, uint dwCmpFlags);

		// Token: 0x06001C0F RID: 7183
		[PreserveSig]
		int Clone(out IAssemblyName pName);
	}
}
