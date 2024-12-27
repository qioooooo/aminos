using System;
using System.CodeDom;
using System.Collections;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x02000194 RID: 404
	internal sealed class SourceFileBuildProvider : InternalBuildProvider
	{
		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x0600111F RID: 4383 RVA: 0x0004CB92 File Offset: 0x0004BB92
		public override CompilerType CodeCompilerType
		{
			get
			{
				return CompilationUtil.GetCompilerInfoFromVirtualPath(base.VirtualPathObject);
			}
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x0004CBA0 File Offset: 0x0004BBA0
		private void EnsureCodeCompileUnit()
		{
			if (this._snippetCompileUnit == null)
			{
				string text = Util.StringFromVirtualPath(base.VirtualPathObject);
				this._snippetCompileUnit = new CodeSnippetCompileUnit(text);
				this._snippetCompileUnit.LinePragma = BaseCodeDomTreeGenerator.CreateCodeLinePragmaHelper(base.VirtualPath, 1);
			}
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x0004CBE4 File Offset: 0x0004BBE4
		public override void GenerateCode(AssemblyBuilder assemblyBuilder)
		{
			this.EnsureCodeCompileUnit();
			assemblyBuilder.AddCodeCompileUnit(this, this._snippetCompileUnit);
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x0004CBF9 File Offset: 0x0004BBF9
		protected internal override CodeCompileUnit GetCodeCompileUnit(out IDictionary linePragmasTable)
		{
			this.EnsureCodeCompileUnit();
			linePragmasTable = new Hashtable();
			linePragmasTable[1] = this._snippetCompileUnit.LinePragma;
			return this._snippetCompileUnit;
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06001123 RID: 4387 RVA: 0x0004CC26 File Offset: 0x0004BC26
		// (set) Token: 0x06001124 RID: 4388 RVA: 0x0004CC2E File Offset: 0x0004BC2E
		internal BuildProvider OwningBuildProvider
		{
			get
			{
				return this._owningBuildProvider;
			}
			set
			{
				this._owningBuildProvider = value;
			}
		}

		// Token: 0x0400168F RID: 5775
		private CodeSnippetCompileUnit _snippetCompileUnit;

		// Token: 0x04001690 RID: 5776
		private BuildProvider _owningBuildProvider;
	}
}
