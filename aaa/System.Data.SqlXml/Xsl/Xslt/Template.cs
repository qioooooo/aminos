using System;
using System.Globalization;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x02000110 RID: 272
	internal class Template : ProtoTemplate
	{
		// Token: 0x06000BE6 RID: 3046 RVA: 0x0003D3AD File Offset: 0x0003C3AD
		public Template(QilName name, string match, QilName mode, double priority, XslVersion xslVer)
			: base(XslNodeType.Template, name, xslVer)
		{
			this.Match = match;
			this.Mode = mode;
			this.Priority = priority;
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x0003D3D0 File Offset: 0x0003C3D0
		public override string GetDebugName()
		{
			BufferBuilder bufferBuilder = new BufferBuilder();
			bufferBuilder.Append("<xsl:template");
			if (this.Match != null)
			{
				bufferBuilder.Append(" match=\"");
				bufferBuilder.Append(this.Match);
				bufferBuilder.Append('"');
			}
			if (this.Name != null)
			{
				bufferBuilder.Append(" name=\"");
				bufferBuilder.Append(this.Name.QualifiedName);
				bufferBuilder.Append('"');
			}
			if (!double.IsNaN(this.Priority))
			{
				bufferBuilder.Append(" priority=\"");
				bufferBuilder.Append(this.Priority.ToString(CultureInfo.InvariantCulture));
				bufferBuilder.Append('"');
			}
			if (this.Mode.LocalName.Length != 0)
			{
				bufferBuilder.Append(" mode=\"");
				bufferBuilder.Append(this.Mode.QualifiedName);
				bufferBuilder.Append('"');
			}
			bufferBuilder.Append('>');
			return bufferBuilder.ToString();
		}

		// Token: 0x0400085B RID: 2139
		public readonly string Match;

		// Token: 0x0400085C RID: 2140
		public readonly QilName Mode;

		// Token: 0x0400085D RID: 2141
		public readonly double Priority;

		// Token: 0x0400085E RID: 2142
		public int ImportPrecedence;

		// Token: 0x0400085F RID: 2143
		public int OrderNumber;
	}
}
