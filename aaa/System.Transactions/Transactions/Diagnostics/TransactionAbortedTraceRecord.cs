using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000B0 RID: 176
	internal class TransactionAbortedTraceRecord : TraceRecord
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x0003D15C File Offset: 0x0003C55C
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/TransactionAbortedTraceRecord";
			}
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0003D170 File Offset: 0x0003C570
		internal static void Trace(string traceSource, TransactionTraceIdentifier txTraceId)
		{
			lock (TransactionAbortedTraceRecord.record)
			{
				TransactionAbortedTraceRecord.record.traceSource = traceSource;
				TransactionAbortedTraceRecord.record.txTraceId = txTraceId;
				DiagnosticTrace.TraceEvent(TraceEventType.Warning, "http://msdn.microsoft.com/2004/06/System/Transactions/TransactionAborted", SR.GetString("TraceTransactionAborted"), TransactionAbortedTraceRecord.record);
			}
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0003D1E0 File Offset: 0x0003C5E0
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			TraceHelper.WriteTxId(xml, this.txTraceId);
		}

		// Token: 0x040002AD RID: 685
		private static TransactionAbortedTraceRecord record = new TransactionAbortedTraceRecord();

		// Token: 0x040002AE RID: 686
		private TransactionTraceIdentifier txTraceId;

		// Token: 0x040002AF RID: 687
		private string traceSource;
	}
}
