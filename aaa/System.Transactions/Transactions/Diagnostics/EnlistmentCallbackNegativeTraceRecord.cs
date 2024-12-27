using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000AC RID: 172
	internal class EnlistmentCallbackNegativeTraceRecord : TraceRecord
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0003CDD8 File Offset: 0x0003C1D8
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/EnlistmentCallbackNegativeTraceRecord";
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0003CDEC File Offset: 0x0003C1EC
		internal static void Trace(string traceSource, EnlistmentTraceIdentifier enTraceId, EnlistmentCallback callback)
		{
			lock (EnlistmentCallbackNegativeTraceRecord.record)
			{
				EnlistmentCallbackNegativeTraceRecord.record.traceSource = traceSource;
				EnlistmentCallbackNegativeTraceRecord.record.enTraceId = enTraceId;
				EnlistmentCallbackNegativeTraceRecord.record.callback = callback;
				DiagnosticTrace.TraceEvent(TraceEventType.Warning, "http://msdn.microsoft.com/2004/06/System/Transactions/EnlistmentCallbackNegative", SR.GetString("TraceEnlistmentCallbackNegative"), EnlistmentCallbackNegativeTraceRecord.record);
			}
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0003CE68 File Offset: 0x0003C268
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			TraceHelper.WriteEnId(xml, this.enTraceId);
			xml.WriteElementString("EnlistmentCallback", this.callback.ToString());
		}

		// Token: 0x040002A0 RID: 672
		private static EnlistmentCallbackNegativeTraceRecord record = new EnlistmentCallbackNegativeTraceRecord();

		// Token: 0x040002A1 RID: 673
		private EnlistmentTraceIdentifier enTraceId;

		// Token: 0x040002A2 RID: 674
		private EnlistmentCallback callback;

		// Token: 0x040002A3 RID: 675
		private string traceSource;
	}
}
