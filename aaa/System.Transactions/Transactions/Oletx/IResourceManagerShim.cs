using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Transactions.Oletx
{
	// Token: 0x02000084 RID: 132
	[SuppressUnmanagedCodeSecurity]
	[Guid("27C73B91-99F5-46d5-A247-732A1A16529E")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IResourceManagerShim
	{
		// Token: 0x0600035E RID: 862
		void Enlist([MarshalAs(UnmanagedType.Interface)] ITransactionShim transactionShim, IntPtr managedIdentifier, [MarshalAs(UnmanagedType.Interface)] out IEnlistmentShim enlistmentShim);

		// Token: 0x0600035F RID: 863
		void Reenlist([MarshalAs(UnmanagedType.U4)] uint prepareInfoSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] prepareInfo, out OletxTransactionOutcome outcome);

		// Token: 0x06000360 RID: 864
		void ReenlistComplete();
	}
}
