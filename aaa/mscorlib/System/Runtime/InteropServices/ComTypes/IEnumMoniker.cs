using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000559 RID: 1369
	[Guid("00000102-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IEnumMoniker
	{
		// Token: 0x06003381 RID: 13185
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] IMoniker[] rgelt, IntPtr pceltFetched);

		// Token: 0x06003382 RID: 13186
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x06003383 RID: 13187
		void Reset();

		// Token: 0x06003384 RID: 13188
		void Clone(out IEnumMoniker ppenum);
	}
}
