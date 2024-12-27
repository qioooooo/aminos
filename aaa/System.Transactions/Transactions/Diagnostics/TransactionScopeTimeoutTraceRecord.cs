using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000B7 RID: 183
	internal class TransactionScopeTimeoutTraceRecord : TraceRecord
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x0003D780 File Offset: 0x0003CB80
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/TransactionScopeTimeoutTraceRecord";
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0003D794 File Offset: 0x0003CB94
		internal static void Trace(string traceSource, TransactionTraceIdentifier txTraceId)
		{
			lock (TransactionScopeTimeoutTraceRecord.record)
			{
				TransactionScopeTimeoutTraceRecord.record.traceSource = traceSource;
				TransactionScopeTimeoutTraceRecord.record.txTraceId = txTraceId;
				DiagnosticTrace.TraceEvent(TraceEventType.Warning, "http://msdn.microsoft.com/2004/06/System/Transactions/TransactionScopeTimeout", SR.GetString("TraceTransactionScopeTimeout"), TransactionScopeTimeoutTraceRecord.record);
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0003D804 File Offset: 0x0003CC04
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			TraceHelper.WriteTxId(xml, this.txTraceId);
		}

		// Token: 0x040002C4 RID: 708
		private static TransactionScopeTimeoutTraceRecord record = new TransactionScopeTimeoutTraceRecord();

		// Token: 0x040002C5 RID: 709
		private TransactionTraceIdentifier txTraceId;

		// Token: 0x040002C6 RID: 710
		private string traceSource;
	}
}
