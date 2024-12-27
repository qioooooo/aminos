using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000BD RID: 189
	internal class ReenlistTraceRecord : TraceRecord
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x0003DCC4 File Offset: 0x0003D0C4
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/ReenlistTraceRecord";
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0003DCD8 File Offset: 0x0003D0D8
		internal static void Trace(string traceSource, Guid rmId)
		{
			lock (ReenlistTraceRecord.record)
			{
				ReenlistTraceRecord.record.traceSource = traceSource;
				ReenlistTraceRecord.record.rmId = rmId;
				DiagnosticTrace.TraceEvent(TraceEventType.Information, "http://msdn.microsoft.com/2004/06/System/Transactions/Reenlist", SR.GetString("TraceReenlist"), ReenlistTraceRecord.record);
			}
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0003DD48 File Offset: 0x0003D148
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			xml.WriteElementString("ResourceManagerId", this.rmId.ToString());
		}

		// Token: 0x040002D7 RID: 727
		private static ReenlistTraceRecord record = new ReenlistTraceRecord();

		// Token: 0x040002D8 RID: 728
		private Guid rmId;

		// Token: 0x040002D9 RID: 729
		private string traceSource;
	}
}
