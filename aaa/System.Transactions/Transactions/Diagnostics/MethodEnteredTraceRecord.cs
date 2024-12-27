using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000C6 RID: 198
	internal class MethodEnteredTraceRecord : TraceRecord
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x0003E558 File Offset: 0x0003D958
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/MethodEnteredTraceRecord";
			}
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0003E56C File Offset: 0x0003D96C
		internal static void Trace(string traceSource, string methodName)
		{
			lock (MethodEnteredTraceRecord.record)
			{
				MethodEnteredTraceRecord.record.traceSource = traceSource;
				MethodEnteredTraceRecord.record.methodName = methodName;
				DiagnosticTrace.TraceEvent(TraceEventType.Verbose, "http://msdn.microsoft.com/2004/06/System/Transactions/MethodEntered", SR.GetString("TraceMethodEntered"), MethodEnteredTraceRecord.record);
			}
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0003E5DC File Offset: 0x0003D9DC
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			xml.WriteElementString("MethodName", this.methodName);
		}

		// Token: 0x040002F1 RID: 753
		private static MethodEnteredTraceRecord record = new MethodEnteredTraceRecord();

		// Token: 0x040002F2 RID: 754
		private string methodName;

		// Token: 0x040002F3 RID: 755
		private string traceSource;
	}
}
