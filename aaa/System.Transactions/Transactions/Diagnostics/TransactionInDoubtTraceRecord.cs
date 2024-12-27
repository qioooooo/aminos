using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000B1 RID: 177
	internal class TransactionInDoubtTraceRecord : TraceRecord
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x0003D234 File Offset: 0x0003C634
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/TransactionInDoubtTraceRecord";
			}
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0003D248 File Offset: 0x0003C648
		internal static void Trace(string traceSource, TransactionTraceIdentifier txTraceId)
		{
			lock (TransactionInDoubtTraceRecord.record)
			{
				TransactionInDoubtTraceRecord.record.traceSource = traceSource;
				TransactionInDoubtTraceRecord.record.txTraceId = txTraceId;
				DiagnosticTrace.TraceEvent(TraceEventType.Warning, "http://msdn.microsoft.com/2004/06/System/Transactions/TransactionInDoubt", SR.GetString("TraceTransactionInDoubt"), TransactionInDoubtTraceRecord.record);
			}
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0003D2B8 File Offset: 0x0003C6B8
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			TraceHelper.WriteTxId(xml, this.txTraceId);
		}

		// Token: 0x040002B0 RID: 688
		private static TransactionInDoubtTraceRecord record = new TransactionInDoubtTraceRecord();

		// Token: 0x040002B1 RID: 689
		private TransactionTraceIdentifier txTraceId;

		// Token: 0x040002B2 RID: 690
		private string traceSource;
	}
}
