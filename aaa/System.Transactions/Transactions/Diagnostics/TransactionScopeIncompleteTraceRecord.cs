using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000B4 RID: 180
	internal class TransactionScopeIncompleteTraceRecord : TraceRecord
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x0003D4E0 File Offset: 0x0003C8E0
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/TransactionScopeIncompleteTraceRecord";
			}
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0003D4F4 File Offset: 0x0003C8F4
		internal static void Trace(string traceSource, TransactionTraceIdentifier txTraceId)
		{
			lock (TransactionScopeIncompleteTraceRecord.record)
			{
				TransactionScopeIncompleteTraceRecord.record.traceSource = traceSource;
				TransactionScopeIncompleteTraceRecord.record.txTraceId = txTraceId;
				DiagnosticTrace.TraceEvent(TraceEventType.Warning, "http://msdn.microsoft.com/2004/06/System/Transactions/TransactionScopeIncomplete", SR.GetString("TraceTransactionScopeIncomplete"), TransactionScopeIncompleteTraceRecord.record);
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0003D564 File Offset: 0x0003C964
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			TraceHelper.WriteTxId(xml, this.txTraceId);
		}

		// Token: 0x040002BA RID: 698
		private static TransactionScopeIncompleteTraceRecord record = new TransactionScopeIncompleteTraceRecord();

		// Token: 0x040002BB RID: 699
		private TransactionTraceIdentifier txTraceId;

		// Token: 0x040002BC RID: 700
		private string traceSource;
	}
}
