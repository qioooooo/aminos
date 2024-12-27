using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200055B RID: 1371
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B196B287-BAB4-101A-B69C-00AA00341D07")]
	[ComImport]
	public interface IEnumConnections
	{
		// Token: 0x06003385 RID: 13189
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] CONNECTDATA[] rgelt, IntPtr pceltFetched);

		// Token: 0x06003386 RID: 13190
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x06003387 RID: 13191
		void Reset();

		// Token: 0x06003388 RID: 13192
		void Clone(out IEnumConnections ppenum);
	}
}
