using System;
using System.Runtime.Serialization;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000062 RID: 98
	[Serializable]
	public class TransactionInDoubtException : TransactionException
	{
		// Token: 0x060002B9 RID: 697 RVA: 0x00030504 File Offset: 0x0002F904
		internal new static TransactionInDoubtException Create(string traceSource, string message, Exception innerException)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
			{
				global::System.Transactions.Diagnostics.TransactionExceptionTraceRecord.Trace(traceSource, message);
			}
			return new TransactionInDoubtException(message, innerException);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00030528 File Offset: 0x0002F928
		internal static TransactionInDoubtException Create(string traceSource, Exception innerException)
		{
			return TransactionInDoubtException.Create(traceSource, SR.GetString("TransactionIndoubt"), innerException);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00030548 File Offset: 0x0002F948
		public TransactionInDoubtException()
			: base(SR.GetString("TransactionIndoubt"))
		{
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00030568 File Offset: 0x0002F968
		public TransactionInDoubtException(string message)
			: base(message)
		{
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0003057C File Offset: 0x0002F97C
		public TransactionInDoubtException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00030594 File Offset: 0x0002F994
		protected TransactionInDoubtException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
