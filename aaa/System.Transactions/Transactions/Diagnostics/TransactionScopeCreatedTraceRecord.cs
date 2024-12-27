using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000B2 RID: 178
	internal class TransactionScopeCreatedTraceRecord : TraceRecord
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0003D30C File Offset: 0x0003C70C
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/TransactionScopeCreatedTraceRecord";
			}
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0003D320 File Offset: 0x0003C720
		internal static void Trace(string traceSource, TransactionTraceIdentifier txTraceId, TransactionScopeResult txScopeResult)
		{
			lock (TransactionScopeCreatedTraceRecord.record)
			{
				TransactionScopeCreatedTraceRecord.record.traceSource = traceSource;
				TransactionScopeCreatedTraceRecord.record.txTraceId = txTraceId;
				TransactionScopeCreatedTraceRecord.record.txScopeResult = txScopeResult;
				DiagnosticTrace.TraceEvent(TraceEventType.Information, "http://msdn.microsoft.com/2004/06/System/Transactions/TransactionScopeCreated", SR.GetString("TraceTransactionScopeCreated"), TransactionScopeCreatedTraceRecord.record);
			}
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0003D39C File Offset: 0x0003C79C
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			TraceHelper.WriteTxId(xml, this.txTraceId);
			xml.WriteElementString("TransactionScopeResult", this.txScopeResult.ToString());
		}

		// Token: 0x040002B3 RID: 691
		private static TransactionScopeCreatedTraceRecord record = new TransactionScopeCreatedTraceRecord();

		// Token: 0x040002B4 RID: 692
		private TransactionTraceIdentifier txTraceId;

		// Token: 0x040002B5 RID: 693
		private TransactionScopeResult txScopeResult;

		// Token: 0x040002B6 RID: 694
		private string traceSource;
	}
}
