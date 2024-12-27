using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000271 RID: 625
	[Guid("00000103-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IEnumFORMATETC
	{
		// Token: 0x06001596 RID: 5526
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] FORMATETC[] rgelt, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pceltFetched);

		// Token: 0x06001597 RID: 5527
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x06001598 RID: 5528
		[PreserveSig]
		int Reset();

		// Token: 0x06001599 RID: 5529
		void Clone(out IEnumFORMATETC newEnum);
	}
}
