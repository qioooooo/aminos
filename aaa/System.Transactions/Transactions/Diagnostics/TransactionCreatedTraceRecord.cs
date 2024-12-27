using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000A7 RID: 167
	internal class TransactionCreatedTraceRecord : TraceRecord
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x0003C8D8 File Offset: 0x0003BCD8
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/TransactionCreatedTraceRecord";
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0003C8EC File Offset: 0x0003BCEC
		internal static void Trace(string traceSource, TransactionTraceIdentifier txTraceId)
		{
			lock (TransactionCreatedTraceRecord.record)
			{
				TransactionCreatedTraceRecord.record.traceSource = traceSource;
				TransactionCreatedTraceRecord.record.txTraceId = txTraceId;
				DiagnosticTrace.TraceEvent(TraceEventType.Information, "http://msdn.microsoft.com/2004/06/System/Transactions/TransactionCreated", SR.GetString("TraceTransactionCreated"), TransactionCreatedTraceRecord.record);
			}
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0003C95C File Offset: 0x0003BD5C
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			TraceHelper.WriteTxId(xml, this.txTraceId);
		}

		// Token: 0x0400028C RID: 652
		private static TransactionCreatedTraceRecord record = new TransactionCreatedTraceRecord();

		// Token: 0x0400028D RID: 653
		private TransactionTraceIdentifier txTraceId;

		// Token: 0x0400028E RID: 654
		private string traceSource;
	}
}
