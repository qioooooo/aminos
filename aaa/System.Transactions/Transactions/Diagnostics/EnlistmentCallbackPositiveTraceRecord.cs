using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000AB RID: 171
	internal class EnlistmentCallbackPositiveTraceRecord : TraceRecord
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x0003CCDC File Offset: 0x0003C0DC
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/EnlistmentCallbackPositiveTraceRecord";
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0003CCF0 File Offset: 0x0003C0F0
		internal static void Trace(string traceSource, EnlistmentTraceIdentifier enTraceId, EnlistmentCallback callback)
		{
			lock (EnlistmentCallbackPositiveTraceRecord.record)
			{
				EnlistmentCallbackPositiveTraceRecord.record.traceSource = traceSource;
				EnlistmentCallbackPositiveTraceRecord.record.enTraceId = enTraceId;
				EnlistmentCallbackPositiveTraceRecord.record.callback = callback;
				DiagnosticTrace.TraceEvent(TraceEventType.Verbose, "http://msdn.microsoft.com/2004/06/System/Transactions/EnlistmentCallbackPositive", SR.GetString("TraceEnlistmentCallbackPositive"), EnlistmentCallbackPositiveTraceRecord.record);
			}
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0003CD6C File Offset: 0x0003C16C
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			TraceHelper.WriteEnId(xml, this.enTraceId);
			xml.WriteElementString("EnlistmentCallback", this.callback.ToString());
		}

		// Token: 0x0400029C RID: 668
		private static EnlistmentCallbackPositiveTraceRecord record = new EnlistmentCallbackPositiveTraceRecord();

		// Token: 0x0400029D RID: 669
		private EnlistmentTraceIdentifier enTraceId;

		// Token: 0x0400029E RID: 670
		private EnlistmentCallback callback;

		// Token: 0x0400029F RID: 671
		private string traceSource;
	}
}
