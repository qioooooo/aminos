using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000AA RID: 170
	internal class EnlistmentNotificationCallTraceRecord : TraceRecord
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x0003CBE0 File Offset: 0x0003BFE0
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/EnlistmentNotificationCallTraceRecord";
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0003CBF4 File Offset: 0x0003BFF4
		internal static void Trace(string traceSource, EnlistmentTraceIdentifier enTraceId, NotificationCall notCall)
		{
			lock (EnlistmentNotificationCallTraceRecord.record)
			{
				EnlistmentNotificationCallTraceRecord.record.traceSource = traceSource;
				EnlistmentNotificationCallTraceRecord.record.enTraceId = enTraceId;
				EnlistmentNotificationCallTraceRecord.record.notCall = notCall;
				DiagnosticTrace.TraceEvent(TraceEventType.Verbose, "http://msdn.microsoft.com/2004/06/System/Transactions/EnlistmentNotificationCall", SR.GetString("TraceEnlistmentNotificationCall"), EnlistmentNotificationCallTraceRecord.record);
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0003CC70 File Offset: 0x0003C070
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			TraceHelper.WriteEnId(xml, this.enTraceId);
			xml.WriteElementString("NotificationCall", this.notCall.ToString());
		}

		// Token: 0x04000298 RID: 664
		private static EnlistmentNotificationCallTraceRecord record = new EnlistmentNotificationCallTraceRecord();

		// Token: 0x04000299 RID: 665
		private EnlistmentTraceIdentifier enTraceId;

		// Token: 0x0400029A RID: 666
		private NotificationCall notCall;

		// Token: 0x0400029B RID: 667
		private string traceSource;
	}
}
