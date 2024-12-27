using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000E2 RID: 226
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("587bf538-4d90-4a3c-9ef1-58a200a8a9e7")]
	[ComImport]
	internal interface IDefinitionIdentity
	{
		// Token: 0x06000372 RID: 882
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetAttribute([MarshalAs(UnmanagedType.LPWStr)] [In] string Namespace, [MarshalAs(UnmanagedType.LPWStr)] [In] string Name);

		// Token: 0x06000373 RID: 883
		void SetAttribute([MarshalAs(UnmanagedType.LPWStr)] [In] string Namespace, [MarshalAs(UnmanagedType.LPWStr)] [In] string Name, [MarshalAs(UnmanagedType.LPWStr)] [In] string Value);

		// Token: 0x06000374 RID: 884
		IEnumIDENTITY_ATTRIBUTE EnumAttributes();

		// Token: 0x06000375 RID: 885
		IDefinitionIdentity Clone([In] IntPtr cDeltas, [MarshalAs(UnmanagedType.LPArray)] [In] IDENTITY_ATTRIBUTE[] Deltas);
	}
}
