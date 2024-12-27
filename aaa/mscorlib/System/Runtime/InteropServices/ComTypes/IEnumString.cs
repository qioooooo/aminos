using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200055D RID: 1373
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000101-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IEnumString
	{
		// Token: 0x0600338D RID: 13197
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] [Out] string[] rgelt, IntPtr pceltFetched);

		// Token: 0x0600338E RID: 13198
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x0600338F RID: 13199
		void Reset();

		// Token: 0x06003390 RID: 13200
		void Clone(out IEnumString ppenum);
	}
}
