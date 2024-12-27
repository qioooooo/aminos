using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000E9 RID: 233
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("d91e12d8-98ed-47fa-9936-39421283d59b")]
	[ComImport]
	internal interface IDefinitionAppId
	{
		// Token: 0x06000398 RID: 920
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string get_SubscriptionId();

		// Token: 0x06000399 RID: 921
		void put_SubscriptionId([MarshalAs(UnmanagedType.LPWStr)] [In] string Subscription);

		// Token: 0x0600039A RID: 922
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string get_Codebase();

		// Token: 0x0600039B RID: 923
		void put_Codebase([MarshalAs(UnmanagedType.LPWStr)] [In] string CodeBase);

		// Token: 0x0600039C RID: 924
		IEnumDefinitionIdentity EnumAppPath();

		// Token: 0x0600039D RID: 925
		void SetAppPath([In] uint cIDefinitionIdentity, [MarshalAs(UnmanagedType.LPArray)] [In] IDefinitionIdentity[] DefinitionIdentity);
	}
}
