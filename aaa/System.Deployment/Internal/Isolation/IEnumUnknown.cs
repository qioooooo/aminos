using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001DB RID: 475
	[Guid("00000100-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumUnknown
	{
		// Token: 0x06000869 RID: 2153
		[PreserveSig]
		int Next(uint celt, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.IUnknown)] [Out] object[] rgelt, ref uint celtFetched);

		// Token: 0x0600086A RID: 2154
		[PreserveSig]
		int Skip(uint celt);

		// Token: 0x0600086B RID: 2155
		[PreserveSig]
		int Reset();

		// Token: 0x0600086C RID: 2156
		[PreserveSig]
		int Clone(out IEnumUnknown enumUnknown);
	}
}
