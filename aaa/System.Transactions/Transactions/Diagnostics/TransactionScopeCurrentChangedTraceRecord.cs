using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000B6 RID: 182
	internal class TransactionScopeCurrentChangedTraceRecord : TraceRecord
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x0003D690 File Offset: 0x0003CA90
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/TransactionScopeCurrentChangedTraceRecord";
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0003D6A4 File Offset: 0x0003CAA4
		internal static void Trace(string traceSource, TransactionTraceIdentifier scopeTxTraceId, TransactionTraceIdentifier currentTxTraceId)
		{
			lock (TransactionScopeCurrentChangedTraceRecord.record)
			{
				TransactionScopeCurrentChangedTraceRecord.record.traceSource = traceSource;
				TransactionScopeCurrentChangedTraceRecord.record.scopeTxTraceId = scopeTxTraceId;
				TransactionScopeCurrentChangedTraceRecord.record.currentTxTraceId = currentTxTraceId;
				DiagnosticTrace.TraceEvent(TraceEventType.Warning, "http://msdn.microsoft.com/2004/06/System/Transactions/TransactionScopeCurrentTransactionChanged", SR.GetString("TraceTransactionScopeCurrentTransactionChanged"), TransactionScopeCurrentChangedTraceRecord.record);
			}
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0003D720 File Offset: 0x0003CB20
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			TraceHelper.WriteTxId(xml, this.scopeTxTraceId);
			TraceHelper.WriteTxId(xml, this.currentTxTraceId);
		}

		// Token: 0x040002C0 RID: 704
		private static TransactionScopeCurrentChangedTraceRecord record = new TransactionScopeCurrentChangedTraceRecord();

		// Token: 0x040002C1 RID: 705
		private TransactionTraceIdentifier scopeTxTraceId;

		// Token: 0x040002C2 RID: 706
		private TransactionTraceIdentifier currentTxTraceId;

		// Token: 0x040002C3 RID: 707
		private string traceSource;
	}
}
