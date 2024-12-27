using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000112 RID: 274
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("b30352cf-23da-4577-9b3f-b4e6573be53b")]
	[ComImport]
	internal interface IEnumReferenceIdentity
	{
		// Token: 0x06000677 RID: 1655
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] IReferenceIdentity[] ReferenceIdentity);

		// Token: 0x06000678 RID: 1656
		void Skip(uint celt);

		// Token: 0x06000679 RID: 1657
		void Reset();

		// Token: 0x0600067A RID: 1658
		IEnumReferenceIdentity Clone();
	}
}
