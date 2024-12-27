using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200004A RID: 74
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000100-0000-0000-C000-000000000046")]
	[ComImport]
	internal interface IEnumUnknown
	{
		// Token: 0x060001FA RID: 506
		[PreserveSig]
		int Next(uint celt, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.IUnknown)] [Out] object[] rgelt, ref uint celtFetched);

		// Token: 0x060001FB RID: 507
		[PreserveSig]
		int Skip(uint celt);

		// Token: 0x060001FC RID: 508
		[PreserveSig]
		int Reset();

		// Token: 0x060001FD RID: 509
		[PreserveSig]
		int Clone(out IEnumUnknown enumUnknown);
	}
}
