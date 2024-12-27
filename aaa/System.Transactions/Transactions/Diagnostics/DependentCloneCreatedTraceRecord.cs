using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000B9 RID: 185
	internal class DependentCloneCreatedTraceRecord : TraceRecord
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x0003D930 File Offset: 0x0003CD30
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/DependentCloneCreatedTraceRecord";
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0003D944 File Offset: 0x0003CD44
		internal static void Trace(string traceSource, TransactionTraceIdentifier txTraceId, DependentCloneOption option)
		{
			lock (DependentCloneCreatedTraceRecord.record)
			{
				DependentCloneCreatedTraceRecord.record.traceSource = traceSource;
				DependentCloneCreatedTraceRecord.record.txTraceId = txTraceId;
				DependentCloneCreatedTraceRecord.record.option = option;
				DiagnosticTrace.TraceEvent(TraceEventType.Information, "http://msdn.microsoft.com/2004/06/System/Transactions/DependentCloneCreated", SR.GetString("TraceDependentCloneCreated"), DependentCloneCreatedTraceRecord.record);
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0003D9C0 File Offset: 0x0003CDC0
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			TraceHelper.WriteTxId(xml, this.txTraceId);
			xml.WriteElementString("DependentCloneOption", this.option.ToString());
		}

		// Token: 0x040002CA RID: 714
		private static DependentCloneCreatedTraceRecord record = new DependentCloneCreatedTraceRecord();

		// Token: 0x040002CB RID: 715
		private TransactionTraceIdentifier txTraceId;

		// Token: 0x040002CC RID: 716
		private DependentCloneOption option;

		// Token: 0x040002CD RID: 717
		private string traceSource;
	}
}
