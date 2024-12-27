using System;
using System.Runtime.Serialization;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000060 RID: 96
	[Serializable]
	public class TransactionException : SystemException
	{
		// Token: 0x060002A9 RID: 681 RVA: 0x00030314 File Offset: 0x0002F714
		internal static TransactionException Create(string traceSource, string message, Exception innerException)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
			{
				global::System.Transactions.Diagnostics.TransactionExceptionTraceRecord.Trace(traceSource, message);
			}
			return new TransactionException(message, innerException);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00030338 File Offset: 0x0002F738
		internal static TransactionException CreateTransactionStateException(string traceSource, Exception innerException)
		{
			return TransactionException.Create(traceSource, SR.GetString("TransactionStateException"), innerException);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00030358 File Offset: 0x0002F758
		internal static Exception CreateEnlistmentStateException(string traceSource, Exception innerException)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
			{
				global::System.Transactions.Diagnostics.InvalidOperationExceptionTraceRecord.Trace(traceSource, SR.GetString("EnlistmentStateException"));
			}
			return new InvalidOperationException(SR.GetString("EnlistmentStateException"), innerException);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0003038C File Offset: 0x0002F78C
		internal static Exception CreateTransactionCompletedException(string traceSource)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
			{
				global::System.Transactions.Diagnostics.InvalidOperationExceptionTraceRecord.Trace(traceSource, SR.GetString("TransactionAlreadyCompleted"));
			}
			return new InvalidOperationException(SR.GetString("TransactionAlreadyCompleted"));
		}

		// Token: 0x060002AD RID: 685 RVA: 0x000303C0 File Offset: 0x0002F7C0
		internal static Exception CreateInvalidOperationException(string traceSource, string message, Exception innerException)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
			{
				global::System.Transactions.Diagnostics.InvalidOperationExceptionTraceRecord.Trace(traceSource, message);
			}
			return new InvalidOperationException(message, innerException);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x000303E4 File Offset: 0x0002F7E4
		public TransactionException()
		{
		}

		// Token: 0x060002AF RID: 687 RVA: 0x000303F8 File Offset: 0x0002F7F8
		public TransactionException(string message)
			: base(message)
		{
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0003040C File Offset: 0x0002F80C
		public TransactionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00030424 File Offset: 0x0002F824
		protected TransactionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
