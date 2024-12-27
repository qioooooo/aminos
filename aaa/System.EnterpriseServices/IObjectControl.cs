using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200000F RID: 15
	[Guid("51372AEC-CAE7-11CF-BE81-00AA00A2FA25")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IObjectControl
	{
		// Token: 0x06000038 RID: 56
		void Activate();

		// Token: 0x06000039 RID: 57
		void Deactivate();

		// Token: 0x0600003A RID: 58
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool CanBePooled();
	}
}
