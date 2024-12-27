using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000CA RID: 202
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("CD193BC0-B4BC-11d2-9833-00C04FC31D2E")]
	[ComImport]
	internal interface IAssemblyName
	{
		// Token: 0x060004A1 RID: 1185
		[PreserveSig]
		int SetProperty(uint PropertyId, IntPtr pvProperty, uint cbProperty);

		// Token: 0x060004A2 RID: 1186
		[PreserveSig]
		int GetProperty(uint PropertyId, IntPtr pvProperty, ref uint pcbProperty);

		// Token: 0x060004A3 RID: 1187
		[PreserveSig]
		int Finalize();

		// Token: 0x060004A4 RID: 1188
		[PreserveSig]
		int GetDisplayName(IntPtr szDisplayName, ref uint pccDisplayName, uint dwDisplayFlags);

		// Token: 0x060004A5 RID: 1189
		[PreserveSig]
		int BindToObject(object refIID, object pAsmBindSink, IApplicationContext pApplicationContext, [MarshalAs(UnmanagedType.LPWStr)] string szCodeBase, long llFlags, int pvReserved, uint cbReserved, out int ppv);

		// Token: 0x060004A6 RID: 1190
		[PreserveSig]
		int GetName(out uint lpcwBuffer, out int pwzName);

		// Token: 0x060004A7 RID: 1191
		[PreserveSig]
		int GetVersion(out uint pdwVersionHi, out uint pdwVersionLow);

		// Token: 0x060004A8 RID: 1192
		[PreserveSig]
		int IsEqual(IAssemblyName pName, uint dwCmpFlags);

		// Token: 0x060004A9 RID: 1193
		[PreserveSig]
		int Clone(out IAssemblyName pName);
	}
}
