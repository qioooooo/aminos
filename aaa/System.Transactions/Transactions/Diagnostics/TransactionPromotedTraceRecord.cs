using System;
using System.Diagnostics;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000A8 RID: 168
	internal class TransactionPromotedTraceRecord : TraceRecord
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x0003C9B0 File Offset: 0x0003BDB0
		internal override string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/TransactionPromotedTraceRecord";
			}
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0003C9C4 File Offset: 0x0003BDC4
		internal static void Trace(string traceSource, TransactionTraceIdentifier localTxTraceId, TransactionTraceIdentifier distTxTraceId)
		{
			lock (TransactionPromotedTraceRecord.record)
			{
				TransactionPromotedTraceRecord.record.traceSource = traceSource;
				TransactionPromotedTraceRecord.record.localTxTraceId = localTxTraceId;
				TransactionPromotedTraceRecord.record.distTxTraceId = distTxTraceId;
				DiagnosticTrace.TraceEvent(TraceEventType.Information, "http://msdn.microsoft.com/2004/06/System/Transactions/TransactionPromoted", SR.GetString("TraceTransactionPromoted"), TransactionPromotedTraceRecord.record);
			}
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0003CA40 File Offset: 0x0003BE40
		internal override void WriteTo(XmlWriter xml)
		{
			TraceHelper.WriteTraceSource(xml, this.traceSource);
			xml.WriteStartElement("LightweightTransaction");
			TraceHelper.WriteTxId(xml, this.localTxTraceId);
			xml.WriteEndElement();
			xml.WriteStartElement("PromotedTransaction");
			TraceHelper.WriteTxId(xml, this.distTxTraceId);
			xml.WriteEndElement();
		}

		// Token: 0x0400028F RID: 655
		private static TransactionPromotedTraceRecord record = new TransactionPromotedTraceRecord();

		// Token: 0x04000290 RID: 656
		private TransactionTraceIdentifier localTxTraceId;

		// Token: 0x04000291 RID: 657
		private TransactionTraceIdentifier distTxTraceId;

		// Token: 0x04000292 RID: 658
		private string traceSource;
	}
}
