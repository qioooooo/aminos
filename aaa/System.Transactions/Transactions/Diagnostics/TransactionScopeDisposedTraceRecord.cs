using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000B3 RID: 179
	internal class TransactionScopeDisposedTraceRecord : TraceRecord
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x0003D408 File Offset: 0x0003C808
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/TransactionScopeDisposedTraceRecord";
			}
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0003D41C File Offset: 0x0003C81C
		internal static void Trace(string traceSource, TransactionTraceIdentifier txTraceId)
		{
			lock (TransactionScopeDisposedTraceRecord.record)
			{
				TransactionScopeDisposedTraceRecord.record.traceSource = traceSource;
				TransactionScopeDisposedTraceRecord.record.txTraceId = txTraceId;
				DiagnosticTrace.TraceEvent(TraceEventType.Information, "http://msdn.microsoft.com/2004/06/System/Transactions/TransactionScopeDisposed", SR.GetString("TraceTransactionScopeDisposed"), TransactionScopeDisposedTraceRecord.record);
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0003D48C File Offset: 0x0003C88C
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			TraceHelper.WriteTxId(xml, this.txTraceId);
		}

		// Token: 0x040002B7 RID: 695
		private static TransactionScopeDisposedTraceRecord record = new TransactionScopeDisposedTraceRecord();

		// Token: 0x040002B8 RID: 696
		private TransactionTraceIdentifier txTraceId;

		// Token: 0x040002B9 RID: 697
		private string traceSource;
	}
}
