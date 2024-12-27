using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000029 RID: 41
	[Guid("1427c51a-4584-49d8-90a0-c50d8086cbe9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IManagedObjectInfo
	{
		// Token: 0x06000093 RID: 147
		void GetIUnknown(out IntPtr pUnk);

		// Token: 0x06000094 RID: 148
		void GetIObjectControl(out IObjectControl pCtrl);

		// Token: 0x06000095 RID: 149
		void SetInPool([MarshalAs(UnmanagedType.Bool)] bool fInPool, IntPtr pPooledObject);

		// Token: 0x06000096 RID: 150
		void SetWrapperStrength([MarshalAs(UnmanagedType.Bool)] bool bStrong);
	}
}
