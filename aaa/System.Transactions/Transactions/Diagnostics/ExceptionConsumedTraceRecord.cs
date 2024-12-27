using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000C3 RID: 195
	internal class ExceptionConsumedTraceRecord : TraceRecord
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x0003E2A8 File Offset: 0x0003D6A8
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/ExceptionConsumedTraceRecord";
			}
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0003E2BC File Offset: 0x0003D6BC
		internal static void Trace(string traceSource, Exception exception)
		{
			lock (ExceptionConsumedTraceRecord.record)
			{
				ExceptionConsumedTraceRecord.record.traceSource = traceSource;
				ExceptionConsumedTraceRecord.record.exception = exception;
				DiagnosticTrace.TraceEvent(TraceEventType.Verbose, "http://msdn.microsoft.com/2004/06/System/Transactions/ExceptionConsumed", SR.GetString("TraceExceptionConsumed"), ExceptionConsumedTraceRecord.record);
			}
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0003E32C File Offset: 0x0003D72C
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			xml.WriteElementString("ExceptionMessage", this.exception.Message);
			xml.WriteElementString("ExceptionStack", this.exception.StackTrace);
		}

		// Token: 0x040002E8 RID: 744
		private static ExceptionConsumedTraceRecord record = new ExceptionConsumedTraceRecord();

		// Token: 0x040002E9 RID: 745
		private Exception exception;

		// Token: 0x040002EA RID: 746
		private string traceSource;
	}
}
