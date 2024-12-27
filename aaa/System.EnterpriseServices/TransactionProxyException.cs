using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Transactions;

namespace System.EnterpriseServices
{
	// Token: 0x02000045 RID: 69
	[Serializable]
	internal sealed class TransactionProxyException : COMException
	{
		// Token: 0x0600014B RID: 331 RVA: 0x00005931 File Offset: 0x00004931
		private TransactionProxyException(int hr, TransactionException exception)
			: base(null, exception)
		{
			base.HResult = hr;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00005942 File Offset: 0x00004942
		private TransactionProxyException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000594C File Offset: 0x0000494C
		public static void ThrowTransactionProxyException(int hr, TransactionException exception)
		{
			throw new TransactionProxyException(hr, exception);
		}
	}
}
