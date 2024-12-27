using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.Threading;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x02000102 RID: 258
	internal class ScriptClass
	{
		// Token: 0x06000B9B RID: 2971 RVA: 0x0003BD0C File Offset: 0x0003AD0C
		public ScriptClass(string ns, CompilerInfo compilerInfo)
		{
			this.ns = ns;
			this.compilerInfo = compilerInfo;
			this.typeDecl = new CodeTypeDeclaration(ScriptClass.GenerateUniqueClassName());
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0003BD5E File Offset: 0x0003AD5E
		private static string GenerateUniqueClassName()
		{
			return "Script" + Interlocked.Increment(ref ScriptClass.scriptClassCounter);
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x0003BD7C File Offset: 0x0003AD7C
		public void AddScriptBlock(string source, string uriString, int lineNumber, int endLine, int endPos)
		{
			CodeSnippetTypeMember codeSnippetTypeMember = new CodeSnippetTypeMember(source);
			string fileName = SourceLineInfo.GetFileName(uriString);
			if (lineNumber > 0)
			{
				codeSnippetTypeMember.LinePragma = new CodeLinePragma(fileName, lineNumber);
				this.scriptFiles.Add(fileName);
			}
			this.typeDecl.Members.Add(codeSnippetTypeMember);
			this.endFileName = fileName;
			this.endLine = endLine;
			this.endPos = endPos;
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0003BDE0 File Offset: 0x0003ADE0
		public CompilerError CreateCompileExceptionError(Exception e)
		{
			string text = XslTransformException.CreateMessage("Xslt_ScriptCompileException", new string[] { e.Message });
			return new CompilerError(this.endFileName, this.endLine, this.endPos, string.Empty, text);
		}

		// Token: 0x040007FC RID: 2044
		public string ns;

		// Token: 0x040007FD RID: 2045
		public CompilerInfo compilerInfo;

		// Token: 0x040007FE RID: 2046
		public StringCollection refAssemblies = new StringCollection();

		// Token: 0x040007FF RID: 2047
		public StringCollection nsImports = new StringCollection();

		// Token: 0x04000800 RID: 2048
		public StringCollection scriptFiles = new StringCollection();

		// Token: 0x04000801 RID: 2049
		public CodeTypeDeclaration typeDecl;

		// Token: 0x04000802 RID: 2050
		public bool refAssembliesByHref;

		// Token: 0x04000803 RID: 2051
		public string endFileName;

		// Token: 0x04000804 RID: 2052
		public int endLine;

		// Token: 0x04000805 RID: 2053
		public int endPos;

		// Token: 0x04000806 RID: 2054
		private static long scriptClassCounter;
	}
}
