using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000BA RID: 186
	internal class DependentCloneCompleteTraceRecord : TraceRecord
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x0003DA2C File Offset: 0x0003CE2C
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/DependentCloneCompleteTraceRecord";
			}
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0003DA40 File Offset: 0x0003CE40
		internal static void Trace(string traceSource, TransactionTraceIdentifier txTraceId)
		{
			lock (DependentCloneCompleteTraceRecord.record)
			{
				DependentCloneCompleteTraceRecord.record.traceSource = traceSource;
				DependentCloneCompleteTraceRecord.record.txTraceId = txTraceId;
				DiagnosticTrace.TraceEvent(TraceEventType.Information, "http://msdn.microsoft.com/2004/06/System/Transactions/DependentCloneComplete", SR.GetString("TraceDependentCloneComplete"), DependentCloneCompleteTraceRecord.record);
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0003DAB0 File Offset: 0x0003CEB0
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			TraceHelper.WriteTxId(xml, this.txTraceId);
		}

		// Token: 0x040002CE RID: 718
		private static DependentCloneCompleteTraceRecord record = new DependentCloneCompleteTraceRecord();

		// Token: 0x040002CF RID: 719
		private TransactionTraceIdentifier txTraceId;

		// Token: 0x040002D0 RID: 720
		private string traceSource;
	}
}
