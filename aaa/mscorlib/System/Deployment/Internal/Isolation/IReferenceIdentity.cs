using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001EB RID: 491
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6eaf5ace-7917-4f3c-b129-e046a9704766")]
	[ComImport]
	internal interface IReferenceIdentity
	{
		// Token: 0x06001512 RID: 5394
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetAttribute([MarshalAs(UnmanagedType.LPWStr)] [In] string Namespace, [MarshalAs(UnmanagedType.LPWStr)] [In] string Name);

		// Token: 0x06001513 RID: 5395
		void SetAttribute([MarshalAs(UnmanagedType.LPWStr)] [In] string Namespace, [MarshalAs(UnmanagedType.LPWStr)] [In] string Name, [MarshalAs(UnmanagedType.LPWStr)] [In] string Value);

		// Token: 0x06001514 RID: 5396
		IEnumIDENTITY_ATTRIBUTE EnumAttributes();

		// Token: 0x06001515 RID: 5397
		IReferenceIdentity Clone([In] IntPtr cDeltas, [MarshalAs(UnmanagedType.LPArray)] [In] IDENTITY_ATTRIBUTE[] Deltas);
	}
}
