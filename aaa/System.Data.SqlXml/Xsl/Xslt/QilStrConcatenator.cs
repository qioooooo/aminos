using System;
using System.Xml.Xsl.Qil;
using System.Xml.Xsl.XPath;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x02000101 RID: 257
	internal class QilStrConcatenator
	{
		// Token: 0x06000B94 RID: 2964 RVA: 0x0003BBDD File Offset: 0x0003ABDD
		public QilStrConcatenator(XPathQilFactory f)
		{
			this.f = f;
			this.builder = new BufferBuilder();
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x0003BBF7 File Offset: 0x0003ABF7
		public void Reset()
		{
			this.inUse = true;
			this.builder.Clear();
			this.concat = null;
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0003BC14 File Offset: 0x0003AC14
		private void FlushBuilder()
		{
			if (this.concat == null)
			{
				this.concat = this.f.BaseFactory.Sequence();
			}
			if (this.builder.Length != 0)
			{
				this.concat.Add(this.f.String(this.builder.ToString()));
				this.builder.Length = 0;
			}
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x0003BC79 File Offset: 0x0003AC79
		public void Append(string value)
		{
			this.builder.Append(value);
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x0003BC87 File Offset: 0x0003AC87
		public void Append(char value)
		{
			this.builder.Append(value);
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x0003BC95 File Offset: 0x0003AC95
		public void Append(QilNode value)
		{
			if (value != null)
			{
				if (value.NodeType == QilNodeType.LiteralString)
				{
					this.builder.Append((QilLiteral)value);
					return;
				}
				this.FlushBuilder();
				this.concat.Add(value);
			}
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0003BCCD File Offset: 0x0003ACCD
		public QilNode ToQil()
		{
			this.inUse = false;
			if (this.concat == null)
			{
				return this.f.String(this.builder.ToString());
			}
			this.FlushBuilder();
			return this.f.StrConcat(this.concat);
		}

		// Token: 0x040007F8 RID: 2040
		private XPathQilFactory f;

		// Token: 0x040007F9 RID: 2041
		private BufferBuilder builder;

		// Token: 0x040007FA RID: 2042
		private QilList concat;

		// Token: 0x040007FB RID: 2043
		private bool inUse;
	}
}
