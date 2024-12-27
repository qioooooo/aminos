using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000529 RID: 1321
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IEnumConnectionPoints instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Guid("B196B285-BAB4-101A-B69C-00AA00341D07")]
	[ComImport]
	public interface UCOMIEnumConnectionPoints
	{
		// Token: 0x06003301 RID: 13057
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] UCOMIConnectionPoint[] rgelt, out int pceltFetched);

		// Token: 0x06003302 RID: 13058
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x06003303 RID: 13059
		[PreserveSig]
		int Reset();

		// Token: 0x06003304 RID: 13060
		void Clone(out UCOMIEnumConnectionPoints ppenum);
	}
}
