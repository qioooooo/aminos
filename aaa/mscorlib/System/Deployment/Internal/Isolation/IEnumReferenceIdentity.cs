using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001EF RID: 495
	[Guid("b30352cf-23da-4577-9b3f-b4e6573be53b")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumReferenceIdentity
	{
		// Token: 0x06001523 RID: 5411
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] IReferenceIdentity[] ReferenceIdentity);

		// Token: 0x06001524 RID: 5412
		void Skip(uint celt);

		// Token: 0x06001525 RID: 5413
		void Reset();

		// Token: 0x06001526 RID: 5414
		IEnumReferenceIdentity Clone();
	}
}
