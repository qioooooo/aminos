using System;
using System.Globalization;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000A6 RID: 166
	internal static class TraceHelper
	{
		// Token: 0x0600049D RID: 1181 RVA: 0x0003C7E0 File Offset: 0x0003BBE0
		internal static void WriteTxId(XmlWriter writer, TransactionTraceIdentifier txTraceId)
		{
			writer.WriteStartElement("TransactionTraceIdentifier");
			if (txTraceId.TransactionIdentifier != null)
			{
				writer.WriteElementString("TransactionIdentifier", txTraceId.TransactionIdentifier);
			}
			else
			{
				writer.WriteElementString("TransactionIdentifier", "");
			}
			int cloneIdentifier = txTraceId.CloneIdentifier;
			if (cloneIdentifier != 0)
			{
				writer.WriteElementString("CloneIdentifier", cloneIdentifier.ToString(CultureInfo.CurrentCulture));
			}
			writer.WriteEndElement();
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0003C850 File Offset: 0x0003BC50
		internal static void WriteEnId(XmlWriter writer, EnlistmentTraceIdentifier enId)
		{
			writer.WriteStartElement("EnlistmentTraceIdentifier");
			writer.WriteElementString("ResourceManagerId", enId.ResourceManagerIdentifier.ToString());
			TraceHelper.WriteTxId(writer, enId.TransactionTraceId);
			writer.WriteElementString("EnlistmentIdentifier", enId.EnlistmentIdentifier.ToString(CultureInfo.CurrentCulture));
			writer.WriteEndElement();
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0003C8BC File Offset: 0x0003BCBC
		internal static void WriteTraceSource(XmlWriter writer, string traceSource)
		{
			writer.WriteElementString("TraceSource", traceSource);
		}
	}
}
