using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000528 RID: 1320
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IEnumConnections instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Guid("B196B287-BAB4-101A-B69C-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface UCOMIEnumConnections
	{
		// Token: 0x060032FD RID: 13053
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] CONNECTDATA[] rgelt, out int pceltFetched);

		// Token: 0x060032FE RID: 13054
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x060032FF RID: 13055
		[PreserveSig]
		void Reset();

		// Token: 0x06003300 RID: 13056
		void Clone(out UCOMIEnumConnections ppenum);
	}
}
