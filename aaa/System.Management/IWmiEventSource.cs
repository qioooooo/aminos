using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000019 RID: 25
	[Guid("87A5AD68-A38A-43ef-ACA9-EFE910E5D24C")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IWmiEventSource
	{
		// Token: 0x060000D4 RID: 212
		[PreserveSig]
		void Indicate(IntPtr pIWbemClassObject);

		// Token: 0x060000D5 RID: 213
		[PreserveSig]
		void SetStatus(int lFlags, int hResult, [MarshalAs(UnmanagedType.BStr)] string strParam, IntPtr pObjParam);
	}
}
