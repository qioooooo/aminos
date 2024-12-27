using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000272 RID: 626
	[Guid("00000103-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IEnumSTATDATA
	{
		// Token: 0x0600159A RID: 5530
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] STATDATA[] rgelt, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] [Out] int[] pceltFetched);

		// Token: 0x0600159B RID: 5531
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x0600159C RID: 5532
		[PreserveSig]
		int Reset();

		// Token: 0x0600159D RID: 5533
		void Clone(out IEnumSTATDATA newEnum);
	}
}
