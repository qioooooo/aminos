using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001F0 RID: 496
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("d91e12d8-98ed-47fa-9936-39421283d59b")]
	[ComImport]
	internal interface IDefinitionAppId
	{
		// Token: 0x06001527 RID: 5415
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string get_SubscriptionId();

		// Token: 0x06001528 RID: 5416
		void put_SubscriptionId([MarshalAs(UnmanagedType.LPWStr)] [In] string Subscription);

		// Token: 0x06001529 RID: 5417
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string get_Codebase();

		// Token: 0x0600152A RID: 5418
		void put_Codebase([MarshalAs(UnmanagedType.LPWStr)] [In] string CodeBase);

		// Token: 0x0600152B RID: 5419
		IEnumDefinitionIdentity EnumAppPath();

		// Token: 0x0600152C RID: 5420
		void SetAppPath([In] uint cIDefinitionIdentity, [MarshalAs(UnmanagedType.LPArray)] [In] IDefinitionIdentity[] DefinitionIdentity);
	}
}
