using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000EB RID: 235
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("054f0bef-9e45-4363-8f5a-2f8e142d9a3b")]
	[ComImport]
	internal interface IReferenceAppId
	{
		// Token: 0x060003A5 RID: 933
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string get_SubscriptionId();

		// Token: 0x060003A6 RID: 934
		void put_SubscriptionId([MarshalAs(UnmanagedType.LPWStr)] [In] string Subscription);

		// Token: 0x060003A7 RID: 935
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string get_Codebase();

		// Token: 0x060003A8 RID: 936
		void put_Codebase([MarshalAs(UnmanagedType.LPWStr)] [In] string CodeBase);

		// Token: 0x060003A9 RID: 937
		IEnumReferenceIdentity EnumAppPath();
	}
}
