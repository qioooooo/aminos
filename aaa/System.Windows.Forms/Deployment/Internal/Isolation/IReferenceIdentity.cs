using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000E1 RID: 225
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6eaf5ace-7917-4f3c-b129-e046a9704766")]
	[ComImport]
	internal interface IReferenceIdentity
	{
		// Token: 0x0600036E RID: 878
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetAttribute([MarshalAs(UnmanagedType.LPWStr)] [In] string Namespace, [MarshalAs(UnmanagedType.LPWStr)] [In] string Name);

		// Token: 0x0600036F RID: 879
		void SetAttribute([MarshalAs(UnmanagedType.LPWStr)] [In] string Namespace, [MarshalAs(UnmanagedType.LPWStr)] [In] string Name, [MarshalAs(UnmanagedType.LPWStr)] [In] string Value);

		// Token: 0x06000370 RID: 880
		IEnumIDENTITY_ATTRIBUTE EnumAttributes();

		// Token: 0x06000371 RID: 881
		IReferenceIdentity Clone([In] IntPtr cDeltas, [MarshalAs(UnmanagedType.LPArray)] [In] IDENTITY_ATTRIBUTE[] Deltas);
	}
}
