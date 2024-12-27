using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001ED RID: 493
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9cdaae75-246e-4b00-a26d-b9aec137a3eb")]
	[ComImport]
	internal interface IEnumIDENTITY_ATTRIBUTE
	{
		// Token: 0x0600151A RID: 5402
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] IDENTITY_ATTRIBUTE[] rgAttributes);

		// Token: 0x0600151B RID: 5403
		IntPtr CurrentIntoBuffer([In] IntPtr Available, [MarshalAs(UnmanagedType.LPArray)] [Out] byte[] Data);

		// Token: 0x0600151C RID: 5404
		void Skip([In] uint celt);

		// Token: 0x0600151D RID: 5405
		void Reset();

		// Token: 0x0600151E RID: 5406
		IEnumIDENTITY_ATTRIBUTE Clone();
	}
}
