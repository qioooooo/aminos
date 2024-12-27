using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000005 RID: 5
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7D40FCC8-F81E-462e-BBA1-8A99EBDC826C")]
	[ComImport]
	internal interface IContextTransactionInfo
	{
		// Token: 0x06000005 RID: 5
		[return: MarshalAs(UnmanagedType.Interface)]
		object FetchTransaction();

		// Token: 0x06000006 RID: 6
		void RegisterTransactionProxy([MarshalAs(UnmanagedType.Interface)] [In] ITransactionProxy proxy, out Guid guid);

		// Token: 0x06000007 RID: 7
		void GetTxIsolationLevelAndTimeout(out DtcIsolationLevel isoLevel, out int timeout);
	}
}
