using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200055E RID: 1374
	[Guid("00020404-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IEnumVARIANT
	{
		// Token: 0x06003391 RID: 13201
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] object[] rgVar, IntPtr pceltFetched);

		// Token: 0x06003392 RID: 13202
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x06003393 RID: 13203
		[PreserveSig]
		int Reset();

		// Token: 0x06003394 RID: 13204
		IEnumVARIANT Clone();
	}
}
