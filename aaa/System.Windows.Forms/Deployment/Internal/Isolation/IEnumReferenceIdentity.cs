using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000E7 RID: 231
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("b30352cf-23da-4577-9b3f-b4e6573be53b")]
	[ComImport]
	internal interface IEnumReferenceIdentity
	{
		// Token: 0x0600038D RID: 909
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] IReferenceIdentity[] ReferenceIdentity);

		// Token: 0x0600038E RID: 910
		void Skip(uint celt);

		// Token: 0x0600038F RID: 911
		void Reset();

		// Token: 0x06000390 RID: 912
		IEnumReferenceIdentity Clone();
	}
}
