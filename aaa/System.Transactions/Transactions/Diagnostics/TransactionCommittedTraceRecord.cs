using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000AF RID: 175
	internal class TransactionCommittedTraceRecord : TraceRecord
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x0003D084 File Offset: 0x0003C484
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/TransactionCommittedTraceRecord";
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0003D098 File Offset: 0x0003C498
		internal static void Trace(string traceSource, TransactionTraceIdentifier txTraceId)
		{
			lock (TransactionCommittedTraceRecord.record)
			{
				TransactionCommittedTraceRecord.record.traceSource = traceSource;
				TransactionCommittedTraceRecord.record.txTraceId = txTraceId;
				DiagnosticTrace.TraceEvent(TraceEventType.Verbose, "http://msdn.microsoft.com/2004/06/System/Transactions/TransactionCommitted", SR.GetString("TraceTransactionCommitted"), TransactionCommittedTraceRecord.record);
			}
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0003D108 File Offset: 0x0003C508
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			TraceHelper.WriteTxId(xml, this.txTraceId);
		}

		// Token: 0x040002AA RID: 682
		private static TransactionCommittedTraceRecord record = new TransactionCommittedTraceRecord();

		// Token: 0x040002AB RID: 683
		private TransactionTraceIdentifier txTraceId;

		// Token: 0x040002AC RID: 684
		private string traceSource;
	}
}
