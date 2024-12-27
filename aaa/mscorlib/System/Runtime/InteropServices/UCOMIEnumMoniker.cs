using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000526 RID: 1318
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IEnumMoniker instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Guid("00000102-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface UCOMIEnumMoniker
	{
		// Token: 0x060032F9 RID: 13049
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] UCOMIMoniker[] rgelt, out int pceltFetched);

		// Token: 0x060032FA RID: 13050
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x060032FB RID: 13051
		[PreserveSig]
		int Reset();

		// Token: 0x060032FC RID: 13052
		void Clone(out UCOMIEnumMoniker ppenum);
	}
}
