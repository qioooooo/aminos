using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000C4 RID: 196
	internal class InvalidOperationExceptionTraceRecord : TraceRecord
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x0003E3A0 File Offset: 0x0003D7A0
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/InvalidOperationExceptionTraceRecord";
			}
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0003E3B4 File Offset: 0x0003D7B4
		internal static void Trace(string traceSource, string exceptionMessage)
		{
			lock (InvalidOperationExceptionTraceRecord.record)
			{
				InvalidOperationExceptionTraceRecord.record.traceSource = traceSource;
				InvalidOperationExceptionTraceRecord.record.exceptionMessage = exceptionMessage;
				DiagnosticTrace.TraceEvent(TraceEventType.Error, "http://msdn.microsoft.com/2004/06/System/Transactions/InvalidOperationException", SR.GetString("TraceInvalidOperationException"), InvalidOperationExceptionTraceRecord.record);
			}
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0003E424 File Offset: 0x0003D824
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			xml.WriteElementString("ExceptionMessage", this.exceptionMessage);
		}

		// Token: 0x040002EB RID: 747
		private static InvalidOperationExceptionTraceRecord record = new InvalidOperationExceptionTraceRecord();

		// Token: 0x040002EC RID: 748
		private string exceptionMessage;

		// Token: 0x040002ED RID: 749
		private string traceSource;
	}
}
