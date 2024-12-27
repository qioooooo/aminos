using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Transactions.Oletx
{
	// Token: 0x02000086 RID: 134
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0fb15084-af41-11ce-bd2b-204c4f4f5020")]
	[SuppressUnmanagedCodeSecurity]
	[ComImport]
	internal interface ITransactionNativeInternal
	{
		// Token: 0x06000369 RID: 873
		void Commit(int retaining, [MarshalAs(UnmanagedType.I4)] OletxXacttc commitType, int reserved);

		// Token: 0x0600036A RID: 874
		void Abort(IntPtr reason, int retaining, int async);

		// Token: 0x0600036B RID: 875
		void GetTransactionInfo(out OletxXactTransInfo xactInfo);
	}
}
