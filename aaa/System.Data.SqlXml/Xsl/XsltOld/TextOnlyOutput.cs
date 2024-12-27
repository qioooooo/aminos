using System;
using System.IO;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200019B RID: 411
	internal class TextOnlyOutput : RecordOutput
	{
		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06001176 RID: 4470 RVA: 0x000544A4 File Offset: 0x000534A4
		internal XsltOutput Output
		{
			get
			{
				return this.processor.Output;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06001177 RID: 4471 RVA: 0x000544B1 File Offset: 0x000534B1
		public TextWriter Writer
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x000544B9 File Offset: 0x000534B9
		internal TextOnlyOutput(Processor processor, Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.processor = processor;
			this.writer = new StreamWriter(stream, this.Output.Encoding);
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x000544ED File Offset: 0x000534ED
		internal TextOnlyOutput(Processor processor, TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.processor = processor;
			this.writer = writer;
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x00054514 File Offset: 0x00053514
		public Processor.OutputResult RecordDone(RecordBuilder record)
		{
			BuilderInfo mainNode = record.MainNode;
			XmlNodeType nodeType = mainNode.NodeType;
			if (nodeType != XmlNodeType.Text)
			{
				switch (nodeType)
				{
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					break;
				default:
					goto IL_0036;
				}
			}
			this.writer.Write(mainNode.Value);
			IL_0036:
			record.Reset();
			return Processor.OutputResult.Continue;
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x0005455E File Offset: 0x0005355E
		public void TheEnd()
		{
			this.writer.Flush();
		}

		// Token: 0x04000BCA RID: 3018
		private Processor processor;

		// Token: 0x04000BCB RID: 3019
		private TextWriter writer;
	}
}
