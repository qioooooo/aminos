using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x02000191 RID: 401
	[BuildProviderAppliesTo(BuildProviderAppliesTo.Web)]
	internal abstract class SimpleHandlerBuildProvider : InternalBuildProvider
	{
		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x0600110F RID: 4367 RVA: 0x0004C9E1 File Offset: 0x0004B9E1
		internal override IAssemblyDependencyParser AssemblyDependencyParser
		{
			get
			{
				return this._parser;
			}
		}

		// Token: 0x06001110 RID: 4368
		protected abstract SimpleWebHandlerParser CreateParser();

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06001111 RID: 4369 RVA: 0x0004C9EC File Offset: 0x0004B9EC
		public override CompilerType CodeCompilerType
		{
			get
			{
				this._parser = this.CreateParser();
				this._parser.SetBuildProvider(this);
				this._parser.IgnoreParseErrors = this.IgnoreParseErrors;
				this._parser.Parse(base.ReferencedAssemblies);
				return this._parser.CompilerType;
			}
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x0004CA40 File Offset: 0x0004BA40
		protected internal override CodeCompileUnit GetCodeCompileUnit(out IDictionary linePragmasTable)
		{
			CodeCompileUnit codeModel = this._parser.GetCodeModel();
			linePragmasTable = this._parser.GetLinePragmasTable();
			return codeModel;
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x0004CA68 File Offset: 0x0004BA68
		public override void GenerateCode(AssemblyBuilder assemblyBuilder)
		{
			CodeCompileUnit codeModel = this._parser.GetCodeModel();
			if (codeModel == null)
			{
				return;
			}
			assemblyBuilder.AddCodeCompileUnit(this, codeModel);
			if (this._parser.AssemblyDependencies != null)
			{
				foreach (object obj in this._parser.AssemblyDependencies)
				{
					Assembly assembly = (Assembly)obj;
					assemblyBuilder.AddAssemblyReference(assembly, codeModel);
				}
			}
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x0004CAEC File Offset: 0x0004BAEC
		public override Type GetGeneratedType(CompilerResults results)
		{
			Type type;
			if (this._parser.HasInlineCode)
			{
				type = this._parser.GetTypeToCache(results.CompiledAssembly);
			}
			else
			{
				type = this._parser.GetTypeToCache(null);
			}
			return type;
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06001115 RID: 4373 RVA: 0x0004CB28 File Offset: 0x0004BB28
		public override ICollection VirtualPathDependencies
		{
			get
			{
				return this._parser.SourceDependencies;
			}
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x0004CB35 File Offset: 0x0004BB35
		internal CompilerType GetDefaultCompilerTypeForLanguageInternal(string language)
		{
			return base.GetDefaultCompilerTypeForLanguage(language);
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x0004CB3E File Offset: 0x0004BB3E
		internal CompilerType GetDefaultCompilerTypeInternal()
		{
			return base.GetDefaultCompilerType();
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x0004CB46 File Offset: 0x0004BB46
		internal TextReader OpenReaderInternal()
		{
			return base.OpenReader();
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x0004CB4E File Offset: 0x0004BB4E
		internal override ICollection GetGeneratedTypeNames()
		{
			return new SingleObjectCollection(this._parser.TypeName);
		}

		// Token: 0x0400168E RID: 5774
		private SimpleWebHandlerParser _parser;
	}
}
