using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000BE RID: 190
	internal class DistributedTransactionManagerCreatedTraceRecord : TraceRecord
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x0003DDAC File Offset: 0x0003D1AC
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/TransactionManagerCreatedTraceRecord";
			}
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0003DDC0 File Offset: 0x0003D1C0
		internal static void Trace(string traceSource, Type tmType, string nodeName)
		{
			lock (DistributedTransactionManagerCreatedTraceRecord.record)
			{
				DistributedTransactionManagerCreatedTraceRecord.record.traceSource = traceSource;
				DistributedTransactionManagerCreatedTraceRecord.record.tmType = tmType;
				DistributedTransactionManagerCreatedTraceRecord.record.nodeName = nodeName;
				DiagnosticTrace.TraceEvent(TraceEventType.Verbose, "http://msdn.microsoft.com/2004/06/System/Transactions/TransactionManagerCreated", SR.GetString("TraceTransactionManagerCreated"), DistributedTransactionManagerCreatedTraceRecord.record);
			}
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0003DE3C File Offset: 0x0003D23C
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			xml.WriteElementString("TransactionManagerType", this.tmType.ToString());
			xml.WriteStartElement("TransactionManagerProperties");
			xml.WriteElementString("DistributedTransactionManagerName", this.nodeName);
			xml.WriteEndElement();
		}

		// Token: 0x040002DA RID: 730
		private static DistributedTransactionManagerCreatedTraceRecord record = new DistributedTransactionManagerCreatedTraceRecord();

		// Token: 0x040002DB RID: 731
		private Type tmType;

		// Token: 0x040002DC RID: 732
		private string nodeName;

		// Token: 0x040002DD RID: 733
		private string traceSource;
	}
}
