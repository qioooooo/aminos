using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200052A RID: 1322
	[Guid("00000101-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IEnumString instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[ComImport]
	public interface UCOMIEnumString
	{
		// Token: 0x06003305 RID: 13061
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] [Out] string[] rgelt, out int pceltFetched);

		// Token: 0x06003306 RID: 13062
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x06003307 RID: 13063
		[PreserveSig]
		int Reset();

		// Token: 0x06003308 RID: 13064
		void Clone(out UCOMIEnumString ppenum);
	}
}
