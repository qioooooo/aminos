using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x0200075D RID: 1885
	[Guid("73386977-D6FD-11D2-BED5-00C04F79E3AE")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface ICollectData
	{
		// Token: 0x060039E9 RID: 14825
		[return: MarshalAs(UnmanagedType.I4)]
		void CollectData([MarshalAs(UnmanagedType.I4)] [In] int id, [MarshalAs(UnmanagedType.SysInt)] [In] IntPtr valueName, [MarshalAs(UnmanagedType.SysInt)] [In] IntPtr data, [MarshalAs(UnmanagedType.I4)] [In] int totalBytes, [MarshalAs(UnmanagedType.SysInt)] out IntPtr res);

		// Token: 0x060039EA RID: 14826
		void CloseData();
	}
}
