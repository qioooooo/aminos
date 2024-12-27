using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000176 RID: 374
	[Guid("00000100-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumUnknown
	{
		// Token: 0x060013DF RID: 5087
		[PreserveSig]
		int Next(uint celt, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.IUnknown)] [Out] object[] rgelt, ref uint celtFetched);

		// Token: 0x060013E0 RID: 5088
		[PreserveSig]
		int Skip(uint celt);

		// Token: 0x060013E1 RID: 5089
		[PreserveSig]
		int Reset();

		// Token: 0x060013E2 RID: 5090
		[PreserveSig]
		int Clone(out IEnumUnknown enumUnknown);
	}
}
