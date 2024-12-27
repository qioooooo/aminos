using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200010C RID: 268
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6eaf5ace-7917-4f3c-b129-e046a9704766")]
	[ComImport]
	internal interface IReferenceIdentity
	{
		// Token: 0x06000658 RID: 1624
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetAttribute([MarshalAs(UnmanagedType.LPWStr)] [In] string Namespace, [MarshalAs(UnmanagedType.LPWStr)] [In] string Name);

		// Token: 0x06000659 RID: 1625
		void SetAttribute([MarshalAs(UnmanagedType.LPWStr)] [In] string Namespace, [MarshalAs(UnmanagedType.LPWStr)] [In] string Name, [MarshalAs(UnmanagedType.LPWStr)] [In] string Value);

		// Token: 0x0600065A RID: 1626
		IEnumIDENTITY_ATTRIBUTE EnumAttributes();

		// Token: 0x0600065B RID: 1627
		IReferenceIdentity Clone([In] IntPtr cDeltas, [MarshalAs(UnmanagedType.LPArray)] [In] IDENTITY_ATTRIBUTE[] Deltas);
	}
}
