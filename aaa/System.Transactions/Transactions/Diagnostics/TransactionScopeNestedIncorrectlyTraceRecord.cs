using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000B5 RID: 181
	internal class TransactionScopeNestedIncorrectlyTraceRecord : TraceRecord
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x0003D5B8 File Offset: 0x0003C9B8
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/TransactionScopeNestedIncorrectlyTraceRecord";
			}
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0003D5CC File Offset: 0x0003C9CC
		internal static void Trace(string traceSource, TransactionTraceIdentifier txTraceId)
		{
			lock (TransactionScopeNestedIncorrectlyTraceRecord.record)
			{
				TransactionScopeNestedIncorrectlyTraceRecord.record.traceSource = traceSource;
				TransactionScopeNestedIncorrectlyTraceRecord.record.txTraceId = txTraceId;
				DiagnosticTrace.TraceEvent(TraceEventType.Warning, "http://msdn.microsoft.com/2004/06/System/Transactions/TransactionScopeNestedIncorrectly", SR.GetString("TraceTransactionScopeNestedIncorrectly"), TransactionScopeNestedIncorrectlyTraceRecord.record);
			}
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0003D63C File Offset: 0x0003CA3C
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			TraceHelper.WriteTxId(xml, this.txTraceId);
		}

		// Token: 0x040002BD RID: 701
		private static TransactionScopeNestedIncorrectlyTraceRecord record = new TransactionScopeNestedIncorrectlyTraceRecord();

		// Token: 0x040002BE RID: 702
		private TransactionTraceIdentifier txTraceId;

		// Token: 0x040002BF RID: 703
		private string traceSource;
	}
}
