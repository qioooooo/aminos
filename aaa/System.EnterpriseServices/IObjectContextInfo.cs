using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200000D RID: 13
	[Guid("75B52DDB-E8ED-11D1-93AD-00AA00BA3258")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IObjectContextInfo
	{
		// Token: 0x0600002B RID: 43
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsInTransaction();

		// Token: 0x0600002C RID: 44
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetTransaction();

		// Token: 0x0600002D RID: 45
		Guid GetTransactionId();

		// Token: 0x0600002E RID: 46
		Guid GetActivityId();

		// Token: 0x0600002F RID: 47
		Guid GetContextId();
	}
}
