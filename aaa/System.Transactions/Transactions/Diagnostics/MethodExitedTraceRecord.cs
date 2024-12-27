using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000C7 RID: 199
	internal class MethodExitedTraceRecord : TraceRecord
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x0003E634 File Offset: 0x0003DA34
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/MethodExitedTraceRecord";
			}
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0003E648 File Offset: 0x0003DA48
		internal static void Trace(string traceSource, string methodName)
		{
			lock (MethodExitedTraceRecord.record)
			{
				MethodExitedTraceRecord.record.traceSource = traceSource;
				MethodExitedTraceRecord.record.methodName = methodName;
				DiagnosticTrace.TraceEvent(TraceEventType.Verbose, "http://msdn.microsoft.com/2004/06/System/Transactions/MethodExited", SR.GetString("TraceMethodExited"), MethodExitedTraceRecord.record);
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0003E6B8 File Offset: 0x0003DAB8
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			xml.WriteElementString("MethodName", this.methodName);
		}

		// Token: 0x040002F4 RID: 756
		private static MethodExitedTraceRecord record = new MethodExitedTraceRecord();

		// Token: 0x040002F5 RID: 757
		private string methodName;

		// Token: 0x040002F6 RID: 758
		private string traceSource;
	}
}
