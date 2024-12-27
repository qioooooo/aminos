using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000BC RID: 188
	internal class RecoveryCompleteTraceRecord : TraceRecord
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x0003DBDC File Offset: 0x0003CFDC
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/RecoveryCompleteTraceRecord";
			}
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0003DBF0 File Offset: 0x0003CFF0
		internal static void Trace(string traceSource, Guid rmId)
		{
			lock (RecoveryCompleteTraceRecord.record)
			{
				RecoveryCompleteTraceRecord.record.traceSource = traceSource;
				RecoveryCompleteTraceRecord.record.rmId = rmId;
				DiagnosticTrace.TraceEvent(TraceEventType.Information, "http://msdn.microsoft.com/2004/06/System/Transactions/RecoveryComplete", SR.GetString("TraceRecoveryComplete"), RecoveryCompleteTraceRecord.record);
			}
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0003DC60 File Offset: 0x0003D060
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			xml.WriteElementString("ResourceManagerId", this.rmId.ToString());
		}

		// Token: 0x040002D4 RID: 724
		private static RecoveryCompleteTraceRecord record = new RecoveryCompleteTraceRecord();

		// Token: 0x040002D5 RID: 725
		private Guid rmId;

		// Token: 0x040002D6 RID: 726
		private string traceSource;
	}
}
