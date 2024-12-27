using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Reflection;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x02000124 RID: 292
	internal abstract class BaseTemplateBuildProvider : InternalBuildProvider
	{
		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000D3B RID: 3387 RVA: 0x00036620 File Offset: 0x00035620
		internal TemplateParser Parser
		{
			get
			{
				return this._parser;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000D3C RID: 3388 RVA: 0x00036628 File Offset: 0x00035628
		internal override IAssemblyDependencyParser AssemblyDependencyParser
		{
			get
			{
				return this._parser;
			}
		}

		// Token: 0x06000D3D RID: 3389
		protected abstract TemplateParser CreateParser();

		// Token: 0x06000D3E RID: 3390
		internal abstract BaseCodeDomTreeGenerator CreateCodeDomTreeGenerator(TemplateParser parser);

		// Token: 0x06000D3F RID: 3391 RVA: 0x00036630 File Offset: 0x00035630
		protected internal override CodeCompileUnit GetCodeCompileUnit(out IDictionary linePragmasTable)
		{
			Type codeDomProviderType = this._parser.CompilerType.CodeDomProviderType;
			CodeDomProvider codeDomProvider = CompilationUtil.CreateCodeDomProviderNonPublic(codeDomProviderType);
			BaseCodeDomTreeGenerator baseCodeDomTreeGenerator = this.CreateCodeDomTreeGenerator(this._parser);
			baseCodeDomTreeGenerator.SetDesignerMode();
			CodeCompileUnit codeDomTree = baseCodeDomTreeGenerator.GetCodeDomTree(codeDomProvider, new StringResourceBuilder(), base.VirtualPathObject);
			linePragmasTable = baseCodeDomTreeGenerator.LinePragmasTable;
			return codeDomTree;
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000D40 RID: 3392 RVA: 0x00036684 File Offset: 0x00035684
		public override CompilerType CodeCompilerType
		{
			get
			{
				this._parser = this.CreateParser();
				if (this.IgnoreParseErrors)
				{
					this._parser.IgnoreParseErrors = true;
				}
				if (base.IgnoreControlProperties)
				{
					this._parser.IgnoreControlProperties = true;
				}
				if (!base.ThrowOnFirstParseError)
				{
					this._parser.ThrowOnFirstParseError = false;
				}
				this._parser.Parse(base.ReferencedAssemblies, base.VirtualPathObject);
				if (!this.Parser.RequiresCompilation)
				{
					return null;
				}
				return this._parser.CompilerType;
			}
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0003670A File Offset: 0x0003570A
		internal override ICollection GetCompileWithDependencies()
		{
			if (this._parser.CodeFileVirtualPath == null)
			{
				return null;
			}
			return new SingleObjectCollection(this._parser.CodeFileVirtualPath);
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x00036734 File Offset: 0x00035734
		public override void GenerateCode(AssemblyBuilder assemblyBuilder)
		{
			if (!this.Parser.RequiresCompilation)
			{
				return;
			}
			BaseCodeDomTreeGenerator baseCodeDomTreeGenerator = this.CreateCodeDomTreeGenerator(this._parser);
			CodeCompileUnit codeDomTree = baseCodeDomTreeGenerator.GetCodeDomTree(assemblyBuilder.CodeDomProvider, assemblyBuilder.StringResourceBuilder, base.VirtualPathObject);
			if (codeDomTree != null)
			{
				if (this._parser.AssemblyDependencies != null)
				{
					foreach (object obj in ((IEnumerable)this._parser.AssemblyDependencies))
					{
						Assembly assembly = (Assembly)obj;
						assemblyBuilder.AddAssemblyReference(assembly, codeDomTree);
					}
				}
				assemblyBuilder.AddCodeCompileUnit(this, codeDomTree);
			}
			this._instantiatableFullTypeName = baseCodeDomTreeGenerator.GetInstantiatableFullTypeName();
			if (this._instantiatableFullTypeName != null)
			{
				assemblyBuilder.GenerateTypeFactory(this._instantiatableFullTypeName);
			}
			this._intermediateFullTypeName = baseCodeDomTreeGenerator.GetIntermediateFullTypeName();
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x00036810 File Offset: 0x00035810
		public override Type GetGeneratedType(CompilerResults results)
		{
			if (!this.Parser.RequiresCompilation)
			{
				return null;
			}
			string text;
			if (this._instantiatableFullTypeName == null)
			{
				if (!(this.Parser.CodeFileVirtualPath != null))
				{
					return this.Parser.BaseType;
				}
				text = this._intermediateFullTypeName;
			}
			else
			{
				text = this._instantiatableFullTypeName;
			}
			return results.CompiledAssembly.GetType(text);
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00036871 File Offset: 0x00035871
		internal override BuildResultCompiledType CreateBuildResult(Type t)
		{
			return new BuildResultCompiledTemplateType(t);
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x00036879 File Offset: 0x00035879
		public override ICollection VirtualPathDependencies
		{
			get
			{
				return this._parser.SourceDependencies;
			}
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x00036888 File Offset: 0x00035888
		internal override ICollection GetGeneratedTypeNames()
		{
			if (this._parser.GeneratedClassName == null && this._parser.BaseTypeName == null)
			{
				return null;
			}
			ArrayList arrayList = new ArrayList();
			if (this._parser.GeneratedClassName != null)
			{
				arrayList.Add(this._parser.GeneratedClassName);
			}
			if (this._parser.BaseTypeName != null)
			{
				arrayList.Add(Util.MakeFullTypeName(this._parser.BaseTypeNamespace, this._parser.BaseTypeName));
			}
			return arrayList;
		}

		// Token: 0x040014E9 RID: 5353
		private TemplateParser _parser;

		// Token: 0x040014EA RID: 5354
		private string _instantiatableFullTypeName;

		// Token: 0x040014EB RID: 5355
		private string _intermediateFullTypeName;
	}
}
