using System;
using System.IO;

namespace System.Xml.Serialization
{
	// Token: 0x020002BB RID: 699
	internal class IndentedWriter
	{
		// Token: 0x06002163 RID: 8547 RVA: 0x0009ECE4 File Offset: 0x0009DCE4
		internal IndentedWriter(TextWriter writer, bool compact)
		{
			this.writer = writer;
			this.compact = compact;
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06002164 RID: 8548 RVA: 0x0009ECFA File Offset: 0x0009DCFA
		// (set) Token: 0x06002165 RID: 8549 RVA: 0x0009ED02 File Offset: 0x0009DD02
		internal int Indent
		{
			get
			{
				return this.indentLevel;
			}
			set
			{
				this.indentLevel = value;
			}
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x0009ED0B File Offset: 0x0009DD0B
		internal void Write(string s)
		{
			if (this.needIndent)
			{
				this.WriteIndent();
			}
			this.writer.Write(s);
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x0009ED27 File Offset: 0x0009DD27
		internal void Write(char c)
		{
			if (this.needIndent)
			{
				this.WriteIndent();
			}
			this.writer.Write(c);
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x0009ED43 File Offset: 0x0009DD43
		internal void WriteLine(string s)
		{
			if (this.needIndent)
			{
				this.WriteIndent();
			}
			this.writer.WriteLine(s);
			this.needIndent = true;
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x0009ED66 File Offset: 0x0009DD66
		internal void WriteLine()
		{
			this.writer.WriteLine();
			this.needIndent = true;
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x0009ED7C File Offset: 0x0009DD7C
		internal void WriteIndent()
		{
			this.needIndent = false;
			if (!this.compact)
			{
				for (int i = 0; i < this.indentLevel; i++)
				{
					this.writer.Write("    ");
				}
			}
		}

		// Token: 0x04001458 RID: 5208
		private TextWriter writer;

		// Token: 0x04001459 RID: 5209
		private bool needIndent;

		// Token: 0x0400145A RID: 5210
		private int indentLevel;

		// Token: 0x0400145B RID: 5211
		private bool compact;
	}
}
