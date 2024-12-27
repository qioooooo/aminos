using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001F1 RID: 497
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("054f0bef-9e45-4363-8f5a-2f8e142d9a3b")]
	[ComImport]
	internal interface IReferenceAppId
	{
		// Token: 0x0600152D RID: 5421
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string get_SubscriptionId();

		// Token: 0x0600152E RID: 5422
		void put_SubscriptionId([MarshalAs(UnmanagedType.LPWStr)] [In] string Subscription);

		// Token: 0x0600152F RID: 5423
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string get_Codebase();

		// Token: 0x06001530 RID: 5424
		void put_Codebase([MarshalAs(UnmanagedType.LPWStr)] [In] string CodeBase);

		// Token: 0x06001531 RID: 5425
		IEnumReferenceIdentity EnumAppPath();
	}
}
