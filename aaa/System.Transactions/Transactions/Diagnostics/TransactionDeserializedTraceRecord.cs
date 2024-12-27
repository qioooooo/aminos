using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000C0 RID: 192
	internal class TransactionDeserializedTraceRecord : TraceRecord
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x0003DF94 File Offset: 0x0003D394
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/TransactionDeserializedTraceRecord";
			}
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0003DFA8 File Offset: 0x0003D3A8
		internal static void Trace(string traceSource, TransactionTraceIdentifier txTraceId)
		{
			lock (TransactionDeserializedTraceRecord.record)
			{
				TransactionDeserializedTraceRecord.record.traceSource = traceSource;
				TransactionDeserializedTraceRecord.record.txTraceId = txTraceId;
				DiagnosticTrace.TraceEvent(TraceEventType.Verbose, "http://msdn.microsoft.com/2004/06/System/Transactions/TransactionDeserialized", SR.GetString("TraceTransactionDeserialized"), TransactionDeserializedTraceRecord.record);
			}
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0003E018 File Offset: 0x0003D418
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			TraceHelper.WriteTxId(xml, this.txTraceId);
		}

		// Token: 0x040002E1 RID: 737
		private static TransactionDeserializedTraceRecord record = new TransactionDeserializedTraceRecord();

		// Token: 0x040002E2 RID: 738
		private TransactionTraceIdentifier txTraceId;

		// Token: 0x040002E3 RID: 739
		private string traceSource;
	}
}
