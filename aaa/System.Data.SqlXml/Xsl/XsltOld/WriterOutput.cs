using System;
using System.Collections;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x020001A0 RID: 416
	internal class WriterOutput : RecordOutput
	{
		// Token: 0x06001185 RID: 4485 RVA: 0x00054698 File Offset: 0x00053698
		internal WriterOutput(Processor processor, XmlWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
			this.processor = processor;
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x000546BC File Offset: 0x000536BC
		public Processor.OutputResult RecordDone(RecordBuilder record)
		{
			BuilderInfo mainNode = record.MainNode;
			switch (mainNode.NodeType)
			{
			case XmlNodeType.Element:
				this.writer.WriteStartElement(mainNode.Prefix, mainNode.LocalName, mainNode.NamespaceURI);
				this.WriteAttributes(record.AttributeList, record.AttributeCount);
				if (mainNode.IsEmptyTag)
				{
					this.writer.WriteEndElement();
				}
				break;
			case XmlNodeType.Text:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				this.writer.WriteString(mainNode.Value);
				break;
			case XmlNodeType.CDATA:
				this.writer.WriteCData(mainNode.Value);
				break;
			case XmlNodeType.EntityReference:
				this.writer.WriteEntityRef(mainNode.LocalName);
				break;
			case XmlNodeType.ProcessingInstruction:
				this.writer.WriteProcessingInstruction(mainNode.LocalName, mainNode.Value);
				break;
			case XmlNodeType.Comment:
				this.writer.WriteComment(mainNode.Value);
				break;
			case XmlNodeType.DocumentType:
				this.writer.WriteRaw(mainNode.Value);
				break;
			case XmlNodeType.EndElement:
				this.writer.WriteFullEndElement();
				break;
			}
			record.Reset();
			return Processor.OutputResult.Continue;
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x000547FA File Offset: 0x000537FA
		public void TheEnd()
		{
			this.writer.Flush();
			this.writer = null;
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x00054810 File Offset: 0x00053810
		private void WriteAttributes(ArrayList list, int count)
		{
			for (int i = 0; i < count; i++)
			{
				BuilderInfo builderInfo = (BuilderInfo)list[i];
				this.writer.WriteAttributeString(builderInfo.Prefix, builderInfo.LocalName, builderInfo.NamespaceURI, builderInfo.Value);
			}
		}

		// Token: 0x04000BD6 RID: 3030
		private XmlWriter writer;

		// Token: 0x04000BD7 RID: 3031
		private Processor processor;
	}
}
