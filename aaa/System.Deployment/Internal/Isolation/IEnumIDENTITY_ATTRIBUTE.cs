using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200010F RID: 271
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9cdaae75-246e-4b00-a26d-b9aec137a3eb")]
	[ComImport]
	internal interface IEnumIDENTITY_ATTRIBUTE
	{
		// Token: 0x06000667 RID: 1639
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] IDENTITY_ATTRIBUTE[] rgAttributes);

		// Token: 0x06000668 RID: 1640
		IntPtr CurrentIntoBuffer([In] IntPtr Available, [MarshalAs(UnmanagedType.LPArray)] [Out] byte[] Data);

		// Token: 0x06000669 RID: 1641
		void Skip([In] uint celt);

		// Token: 0x0600066A RID: 1642
		void Reset();

		// Token: 0x0600066B RID: 1643
		IEnumIDENTITY_ATTRIBUTE Clone();
	}
}
