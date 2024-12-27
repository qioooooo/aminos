using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000E4 RID: 228
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9cdaae75-246e-4b00-a26d-b9aec137a3eb")]
	[ComImport]
	internal interface IEnumIDENTITY_ATTRIBUTE
	{
		// Token: 0x0600037D RID: 893
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] IDENTITY_ATTRIBUTE[] rgAttributes);

		// Token: 0x0600037E RID: 894
		IntPtr CurrentIntoBuffer([In] IntPtr Available, [MarshalAs(UnmanagedType.LPArray)] [Out] byte[] Data);

		// Token: 0x0600037F RID: 895
		void Skip([In] uint celt);

		// Token: 0x06000380 RID: 896
		void Reset();

		// Token: 0x06000381 RID: 897
		IEnumIDENTITY_ATTRIBUTE Clone();
	}
}
