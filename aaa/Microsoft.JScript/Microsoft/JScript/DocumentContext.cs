using System;
using System.Diagnostics.SymbolStore;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x0200006A RID: 106
	public class DocumentContext
	{
		// Token: 0x06000531 RID: 1329 RVA: 0x000251B0 File Offset: 0x000241B0
		internal DocumentContext(string name, VsaEngine engine)
		{
			this.documentName = name;
			this.documentWriter = null;
			this.startLine = 0;
			this.startCol = 0;
			this.lastLineInSource = 0;
			this.sourceItem = null;
			this.engine = engine;
			this.debugOn = engine != null && engine.GenerateDebugInfo;
			this._compilerGlobals = null;
			this.reportedVariables = null;
			this.checkForFirst = false;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0002521C File Offset: 0x0002421C
		internal DocumentContext(VsaItem sourceItem)
		{
			if (sourceItem.codebase != null)
			{
				this.documentName = sourceItem.codebase;
			}
			else
			{
				string rootMoniker = sourceItem.engine.RootMoniker;
				this.documentName = rootMoniker + (rootMoniker.EndsWith("/", StringComparison.Ordinal) ? "" : "/") + sourceItem.Name;
			}
			this.documentWriter = null;
			this.startLine = 0;
			this.startCol = 0;
			this.lastLineInSource = 0;
			this.sourceItem = sourceItem;
			this.engine = sourceItem.engine;
			this.debugOn = this.engine != null && this.engine.GenerateDebugInfo;
			this._compilerGlobals = null;
			this.checkForFirst = false;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x000252D8 File Offset: 0x000242D8
		internal DocumentContext(string documentName, int startLine, int startCol, int lastLineInSource, VsaItem sourceItem)
		{
			this.documentName = documentName;
			this.documentWriter = null;
			this.startLine = startLine;
			this.startCol = startCol;
			this.lastLineInSource = lastLineInSource;
			this.sourceItem = sourceItem;
			this.engine = sourceItem.engine;
			this.debugOn = this.engine != null && this.engine.GenerateDebugInfo;
			this._compilerGlobals = null;
			this.checkForFirst = false;
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x0002534E File Offset: 0x0002434E
		internal CompilerGlobals compilerGlobals
		{
			get
			{
				if (this._compilerGlobals == null)
				{
					this._compilerGlobals = this.engine.CompilerGlobals;
				}
				return this._compilerGlobals;
			}
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00025370 File Offset: 0x00024370
		internal void EmitLineInfo(ILGenerator ilgen, int line, int column, int endLine, int endColumn)
		{
			if (this.debugOn)
			{
				if (this.checkForFirst && line == this.firstStartLine && column == this.firstStartCol && endLine == this.firstEndLine && endColumn == this.firstEndCol)
				{
					this.checkForFirst = false;
					return;
				}
				if (this.documentWriter == null)
				{
					this.documentWriter = this.GetSymDocument(this.documentName);
				}
				ilgen.MarkSequencePoint(this.documentWriter, this.startLine + line - this.lastLineInSource, this.startCol + column + 1, this.startLine - this.lastLineInSource + endLine, this.startCol + endColumn + 1);
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00025418 File Offset: 0x00024418
		internal void EmitFirstLineInfo(ILGenerator ilgen, int line, int column, int endLine, int endColumn)
		{
			this.EmitLineInfo(ilgen, line, column, endLine, endColumn);
			this.checkForFirst = true;
			this.firstStartLine = line;
			this.firstStartCol = column;
			this.firstEndLine = endLine;
			this.firstEndCol = endColumn;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0002544C File Offset: 0x0002444C
		private ISymbolDocumentWriter GetSymDocument(string documentName)
		{
			SimpleHashtable documents = this.compilerGlobals.documents;
			object obj = documents[documentName];
			if (obj == null)
			{
				obj = this._compilerGlobals.module.DefineDocument(this.documentName, DocumentContext.language, DocumentContext.vendor, Guid.Empty);
				documents[documentName] = obj;
			}
			return (ISymbolDocumentWriter)obj;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x000254A4 File Offset: 0x000244A4
		internal void HandleError(JScriptException error)
		{
			if (this.sourceItem == null)
			{
				if (error.Severity == 0)
				{
					throw error;
				}
				return;
			}
			else
			{
				if (!this.sourceItem.engine.OnCompilerError(error))
				{
					throw new EndOfFile();
				}
				return;
			}
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x000254D2 File Offset: 0x000244D2
		internal bool HasAlreadySeenErrorFor(string varName)
		{
			if (this.reportedVariables == null)
			{
				this.reportedVariables = new SimpleHashtable(8U);
			}
			else if (this.reportedVariables[varName] != null)
			{
				return true;
			}
			this.reportedVariables[varName] = varName;
			return false;
		}

		// Token: 0x04000232 RID: 562
		internal string documentName;

		// Token: 0x04000233 RID: 563
		internal ISymbolDocumentWriter documentWriter;

		// Token: 0x04000234 RID: 564
		internal int startLine;

		// Token: 0x04000235 RID: 565
		internal int startCol;

		// Token: 0x04000236 RID: 566
		internal int lastLineInSource;

		// Token: 0x04000237 RID: 567
		internal VsaItem sourceItem;

		// Token: 0x04000238 RID: 568
		internal VsaEngine engine;

		// Token: 0x04000239 RID: 569
		internal bool debugOn;

		// Token: 0x0400023A RID: 570
		private CompilerGlobals _compilerGlobals;

		// Token: 0x0400023B RID: 571
		private SimpleHashtable reportedVariables;

		// Token: 0x0400023C RID: 572
		private bool checkForFirst;

		// Token: 0x0400023D RID: 573
		private int firstStartLine;

		// Token: 0x0400023E RID: 574
		private int firstStartCol;

		// Token: 0x0400023F RID: 575
		private int firstEndLine;

		// Token: 0x04000240 RID: 576
		private int firstEndCol;

		// Token: 0x04000241 RID: 577
		internal static readonly Guid language = new Guid("3a12d0b6-c26c-11d0-b442-00a0244a1dd2");

		// Token: 0x04000242 RID: 578
		internal static readonly Guid vendor = new Guid("994b45c4-e6e9-11d2-903f-00c04fa302a1");
	}
}
