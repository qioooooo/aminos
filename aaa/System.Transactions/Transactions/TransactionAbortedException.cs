using System;
using System.Runtime.Serialization;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000061 RID: 97
	[Serializable]
	public class TransactionAbortedException : TransactionException
	{
		// Token: 0x060002B2 RID: 690 RVA: 0x0003043C File Offset: 0x0002F83C
		internal new static TransactionAbortedException Create(string traceSource, string message, Exception innerException)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
			{
				global::System.Transactions.Diagnostics.TransactionExceptionTraceRecord.Trace(traceSource, message);
			}
			return new TransactionAbortedException(message, innerException);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00030460 File Offset: 0x0002F860
		internal static TransactionAbortedException Create(string traceSource, Exception innerException)
		{
			return TransactionAbortedException.Create(traceSource, SR.GetString("TransactionAborted"), innerException);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00030480 File Offset: 0x0002F880
		public TransactionAbortedException()
			: base(SR.GetString("TransactionAborted"))
		{
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x000304A0 File Offset: 0x0002F8A0
		public TransactionAbortedException(string message)
			: base(message)
		{
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x000304B4 File Offset: 0x0002F8B4
		public TransactionAbortedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x000304CC File Offset: 0x0002F8CC
		internal TransactionAbortedException(Exception innerException)
			: base(SR.GetString("TransactionAborted"), innerException)
		{
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x000304EC File Offset: 0x0002F8EC
		protected TransactionAbortedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
