using System;
using System.Runtime.InteropServices;

namespace System.Transactions
{
	// Token: 0x02000065 RID: 101
	[Guid("0fb15084-af41-11ce-bd2b-204c4f4f5020")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IDtcTransaction
	{
		// Token: 0x060002C9 RID: 713
		void Commit(int retaining, [MarshalAs(UnmanagedType.I4)] int commitType, int reserved);

		// Token: 0x060002CA RID: 714
		void Abort(IntPtr reason, int retaining, int async);

		// Token: 0x060002CB RID: 715
		void GetTransactionInfo(IntPtr transactionInformation);
	}
}
