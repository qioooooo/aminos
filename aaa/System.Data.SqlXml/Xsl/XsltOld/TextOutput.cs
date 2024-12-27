using System;
using System.IO;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200019C RID: 412
	internal class TextOutput : SequentialOutput
	{
		// Token: 0x0600117C RID: 4476 RVA: 0x0005456B File Offset: 0x0005356B
		internal TextOutput(Processor processor, Stream stream)
			: base(processor)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.encoding = processor.Output.Encoding;
			this.writer = new StreamWriter(stream, this.encoding);
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x000545A5 File Offset: 0x000535A5
		internal TextOutput(Processor processor, TextWriter writer)
			: base(processor)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.encoding = writer.Encoding;
			this.writer = writer;
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x000545CF File Offset: 0x000535CF
		internal override void Write(char outputChar)
		{
			this.writer.Write(outputChar);
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x000545DD File Offset: 0x000535DD
		internal override void Write(string outputText)
		{
			this.writer.Write(outputText);
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x000545EB File Offset: 0x000535EB
		internal override void Close()
		{
			this.writer.Flush();
			this.writer = null;
		}

		// Token: 0x04000BCC RID: 3020
		private TextWriter writer;
	}
}
