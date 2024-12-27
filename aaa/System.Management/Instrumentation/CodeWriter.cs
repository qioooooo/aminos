using System;
using System.Collections;
using System.Globalization;
using System.IO;

namespace System.Management.Instrumentation
{
	// Token: 0x02000093 RID: 147
	internal class CodeWriter
	{
		// Token: 0x06000456 RID: 1110 RVA: 0x00021983 File Offset: 0x00020983
		public static explicit operator string(CodeWriter writer)
		{
			return writer.ToString();
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0002198C File Offset: 0x0002098C
		public override string ToString()
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			this.WriteCode(stringWriter);
			string text = stringWriter.ToString();
			stringWriter.Close();
			return text;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x000219BC File Offset: 0x000209BC
		private void WriteCode(TextWriter writer)
		{
			string text = new string(' ', this.depth * 4);
			foreach (object obj in this.children)
			{
				if (obj == null)
				{
					writer.WriteLine();
				}
				else if (obj is string)
				{
					writer.Write(text);
					writer.WriteLine(obj);
				}
				else
				{
					((CodeWriter)obj).WriteCode(writer);
				}
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00021A48 File Offset: 0x00020A48
		public CodeWriter AddChild(string name)
		{
			this.Line(name);
			this.Line("{");
			CodeWriter codeWriter = new CodeWriter();
			codeWriter.depth = this.depth + 1;
			this.children.Add(codeWriter);
			this.Line("}");
			return codeWriter;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00021A94 File Offset: 0x00020A94
		public CodeWriter AddChild(params string[] parts)
		{
			return this.AddChild(string.Concat(parts));
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00021AA4 File Offset: 0x00020AA4
		public CodeWriter AddChildNoIndent(string name)
		{
			this.Line(name);
			CodeWriter codeWriter = new CodeWriter();
			codeWriter.depth = this.depth + 1;
			this.children.Add(codeWriter);
			return codeWriter;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00021ADA File Offset: 0x00020ADA
		public CodeWriter AddChild(CodeWriter snippet)
		{
			snippet.depth = this.depth;
			this.children.Add(snippet);
			return snippet;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00021AF6 File Offset: 0x00020AF6
		public void Line(string line)
		{
			this.children.Add(line);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00021B05 File Offset: 0x00020B05
		public void Line(params string[] parts)
		{
			this.Line(string.Concat(parts));
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00021B13 File Offset: 0x00020B13
		public void Line()
		{
			this.children.Add(null);
		}

		// Token: 0x04000261 RID: 609
		private int depth;

		// Token: 0x04000262 RID: 610
		private ArrayList children = new ArrayList();
	}
}
