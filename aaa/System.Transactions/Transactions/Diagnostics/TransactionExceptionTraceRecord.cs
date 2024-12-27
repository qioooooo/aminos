using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000C1 RID: 193
	internal class TransactionExceptionTraceRecord : TraceRecord
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x0003E06C File Offset: 0x0003D46C
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/TransactionExceptionTraceRecord";
			}
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0003E080 File Offset: 0x0003D480
		internal static void Trace(string traceSource, string exceptionMessage)
		{
			lock (TransactionExceptionTraceRecord.record)
			{
				TransactionExceptionTraceRecord.record.traceSource = traceSource;
				TransactionExceptionTraceRecord.record.exceptionMessage = exceptionMessage;
				DiagnosticTrace.TraceEvent(TraceEventType.Error, "http://msdn.microsoft.com/2004/06/System/Transactions/TransactionException", SR.GetString("TraceTransactionException"), TransactionExceptionTraceRecord.record);
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0003E0F0 File Offset: 0x0003D4F0
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			xml.WriteElementString("ExceptionMessage", this.exceptionMessage);
		}

		// Token: 0x040002E4 RID: 740
		private static TransactionExceptionTraceRecord record = new TransactionExceptionTraceRecord();

		// Token: 0x040002E5 RID: 741
		private string exceptionMessage;

		// Token: 0x040002E6 RID: 742
		private string traceSource;
	}
}
