using System;
using System.Runtime.Serialization;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000063 RID: 99
	[Serializable]
	public class TransactionManagerCommunicationException : TransactionException
	{
		// Token: 0x060002BF RID: 703 RVA: 0x000305AC File Offset: 0x0002F9AC
		internal new static TransactionManagerCommunicationException Create(string traceSource, string message, Exception innerException)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
			{
				global::System.Transactions.Diagnostics.TransactionExceptionTraceRecord.Trace(traceSource, message);
			}
			return new TransactionManagerCommunicationException(message, innerException);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x000305D0 File Offset: 0x0002F9D0
		internal static TransactionManagerCommunicationException Create(string traceSource, Exception innerException)
		{
			return TransactionManagerCommunicationException.Create(traceSource, SR.GetString("TransactionManagerCommunicationException"), innerException);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x000305F0 File Offset: 0x0002F9F0
		public TransactionManagerCommunicationException()
			: base(SR.GetString("TransactionManagerCommunicationException"))
		{
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00030610 File Offset: 0x0002FA10
		public TransactionManagerCommunicationException(string message)
			: base(message)
		{
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00030624 File Offset: 0x0002FA24
		public TransactionManagerCommunicationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0003063C File Offset: 0x0002FA3C
		protected TransactionManagerCommunicationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
