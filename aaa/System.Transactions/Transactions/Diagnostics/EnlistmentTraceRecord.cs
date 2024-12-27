using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000A9 RID: 169
	internal class EnlistmentTraceRecord : TraceRecord
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x0003CAC0 File Offset: 0x0003BEC0
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/EnlistmentTraceRecord";
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0003CAD4 File Offset: 0x0003BED4
		internal static void Trace(string traceSource, EnlistmentTraceIdentifier enTraceId, EnlistmentType enType, EnlistmentOptions enOptions)
		{
			lock (EnlistmentTraceRecord.record)
			{
				EnlistmentTraceRecord.record.traceSource = traceSource;
				EnlistmentTraceRecord.record.enTraceId = enTraceId;
				EnlistmentTraceRecord.record.enType = enType;
				EnlistmentTraceRecord.record.enOptions = enOptions;
				DiagnosticTrace.TraceEvent(TraceEventType.Information, "http://msdn.microsoft.com/2004/06/System/Transactions/Enlistment", SR.GetString("TraceEnlistment"), EnlistmentTraceRecord.record);
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0003CB58 File Offset: 0x0003BF58
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			TraceHelper.WriteEnId(xml, this.enTraceId);
			xml.WriteElementString("EnlistmentType", this.enType.ToString());
			xml.WriteElementString("EnlistmentOptions", this.enOptions.ToString());
		}

		// Token: 0x04000293 RID: 659
		private static EnlistmentTraceRecord record = new EnlistmentTraceRecord();

		// Token: 0x04000294 RID: 660
		private EnlistmentTraceIdentifier enTraceId;

		// Token: 0x04000295 RID: 661
		private EnlistmentType enType;

		// Token: 0x04000296 RID: 662
		private EnlistmentOptions enOptions;

		// Token: 0x04000297 RID: 663
		private string traceSource;
	}
}
