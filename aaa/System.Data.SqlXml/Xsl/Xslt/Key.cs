using System;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x02000114 RID: 276
	internal class Key : XslNode
	{
		// Token: 0x06000BEC RID: 3052 RVA: 0x0003D50D File Offset: 0x0003C50D
		public Key(QilName name, string match, string use, XslVersion xslVer)
			: base(XslNodeType.Key, name, null, xslVer)
		{
			this.Match = match;
			this.Use = use;
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x0003D52C File Offset: 0x0003C52C
		public string GetDebugName()
		{
			BufferBuilder bufferBuilder = new BufferBuilder();
			bufferBuilder.Append("<xsl:key name=\"");
			bufferBuilder.Append(this.Name.QualifiedName);
			bufferBuilder.Append('"');
			if (this.Match != null)
			{
				bufferBuilder.Append(" match=\"");
				bufferBuilder.Append(this.Match);
				bufferBuilder.Append('"');
			}
			if (this.Use != null)
			{
				bufferBuilder.Append(" use=\"");
				bufferBuilder.Append(this.Use);
				bufferBuilder.Append('"');
			}
			bufferBuilder.Append('>');
			return bufferBuilder.ToString();
		}

		// Token: 0x04000866 RID: 2150
		public readonly string Match;

		// Token: 0x04000867 RID: 2151
		public readonly string Use;

		// Token: 0x04000868 RID: 2152
		public QilFunction Function;
	}
}
