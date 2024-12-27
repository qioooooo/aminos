using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000AE RID: 174
	internal class TransactionRollbackCalledTraceRecord : TraceRecord
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x0003CFAC File Offset: 0x0003C3AC
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/TransactionRollbackCalledTraceRecord";
			}
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0003CFC0 File Offset: 0x0003C3C0
		internal static void Trace(string traceSource, TransactionTraceIdentifier txTraceId)
		{
			lock (TransactionRollbackCalledTraceRecord.record)
			{
				TransactionRollbackCalledTraceRecord.record.traceSource = traceSource;
				TransactionRollbackCalledTraceRecord.record.txTraceId = txTraceId;
				DiagnosticTrace.TraceEvent(TraceEventType.Warning, "http://msdn.microsoft.com/2004/06/System/Transactions/TransactionRollbackCalled", SR.GetString("TraceTransactionRollbackCalled"), TransactionRollbackCalledTraceRecord.record);
			}
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0003D030 File Offset: 0x0003C430
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			TraceHelper.WriteTxId(xml, this.txTraceId);
		}

		// Token: 0x040002A7 RID: 679
		private static TransactionRollbackCalledTraceRecord record = new TransactionRollbackCalledTraceRecord();

		// Token: 0x040002A8 RID: 680
		private TransactionTraceIdentifier txTraceId;

		// Token: 0x040002A9 RID: 681
		private string traceSource;
	}
}
