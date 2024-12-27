using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000C5 RID: 197
	internal class InternalErrorTraceRecord : TraceRecord
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x0003E47C File Offset: 0x0003D87C
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/InternalErrorTraceRecord";
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0003E490 File Offset: 0x0003D890
		internal static void Trace(string traceSource, string exceptionMessage)
		{
			lock (InternalErrorTraceRecord.record)
			{
				InternalErrorTraceRecord.record.traceSource = traceSource;
				InternalErrorTraceRecord.record.exceptionMessage = exceptionMessage;
				DiagnosticTrace.TraceEvent(TraceEventType.Critical, "http://msdn.microsoft.com/2004/06/System/Transactions/InternalError", SR.GetString("TraceInternalError"), InternalErrorTraceRecord.record);
			}
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0003E500 File Offset: 0x0003D900
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			xml.WriteElementString("ExceptionMessage", this.exceptionMessage);
		}

		// Token: 0x040002EE RID: 750
		private static InternalErrorTraceRecord record = new InternalErrorTraceRecord();

		// Token: 0x040002EF RID: 751
		private string exceptionMessage;

		// Token: 0x040002F0 RID: 752
		private string traceSource;
	}
}
