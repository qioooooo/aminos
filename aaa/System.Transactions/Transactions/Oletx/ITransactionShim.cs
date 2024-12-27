using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Transactions.Oletx
{
	// Token: 0x02000083 RID: 131
	[Guid("279031AF-B00E-42e6-A617-79747E22DD22")]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ITransactionShim
	{
		// Token: 0x06000356 RID: 854
		void Commit();

		// Token: 0x06000357 RID: 855
		void Abort();

		// Token: 0x06000358 RID: 856
		void GetITransactionNative([MarshalAs(UnmanagedType.Interface)] out IDtcTransaction transactionNative);

		// Token: 0x06000359 RID: 857
		void Export([MarshalAs(UnmanagedType.U4)] uint whereaboutsSize, [MarshalAs(UnmanagedType.LPArray)] byte[] whereabouts, [MarshalAs(UnmanagedType.I4)] out int cookieIndex, [MarshalAs(UnmanagedType.U4)] out uint cookieSize, out CoTaskMemHandle cookieBuffer);

		// Token: 0x0600035A RID: 858
		void CreateVoter(IntPtr managedIdentifier, [MarshalAs(UnmanagedType.Interface)] out IVoterBallotShim voterBallotShim);

		// Token: 0x0600035B RID: 859
		void GetPropagationToken([MarshalAs(UnmanagedType.U4)] out uint propagationTokeSize, out CoTaskMemHandle propgationToken);

		// Token: 0x0600035C RID: 860
		void Phase0Enlist(IntPtr managedIdentifier, [MarshalAs(UnmanagedType.Interface)] out IPhase0EnlistmentShim phase0EnlistmentShim);

		// Token: 0x0600035D RID: 861
		void GetTransactionDoNotUse(out IntPtr transaction);
	}
}
