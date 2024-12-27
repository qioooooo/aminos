using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.IO;
using System.Security.Permissions;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x02000122 RID: 290
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.High)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.High)]
	public abstract class BuildProvider
	{
		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000D0F RID: 3343 RVA: 0x000362A1 File Offset: 0x000352A1
		public virtual CompilerType CodeCompilerType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x000362A4 File Offset: 0x000352A4
		public virtual void GenerateCode(AssemblyBuilder assemblyBuilder)
		{
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x000362A6 File Offset: 0x000352A6
		public virtual Type GetGeneratedType(CompilerResults results)
		{
			return null;
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x000362A9 File Offset: 0x000352A9
		public virtual string GetCustomString(CompilerResults results)
		{
			return null;
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x000362AC File Offset: 0x000352AC
		public virtual BuildProviderResultFlags GetResultFlags(CompilerResults results)
		{
			return BuildProviderResultFlags.Default;
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x000362AF File Offset: 0x000352AF
		internal virtual ICollection GetBuildResultVirtualPathDependencies()
		{
			return null;
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x000362B2 File Offset: 0x000352B2
		public virtual ICollection VirtualPathDependencies
		{
			get
			{
				return new SingleObjectCollection(this.VirtualPath);
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x000362BF File Offset: 0x000352BF
		protected internal string VirtualPath
		{
			get
			{
				return global::System.Web.VirtualPath.GetVirtualPathString(this._virtualPath);
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000D17 RID: 3351 RVA: 0x000362CC File Offset: 0x000352CC
		internal VirtualPath VirtualPathObject
		{
			get
			{
				return this._virtualPath;
			}
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x000362D4 File Offset: 0x000352D4
		protected Stream OpenStream()
		{
			return this.OpenStream(this.VirtualPath);
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x000362E2 File Offset: 0x000352E2
		protected Stream OpenStream(string virtualPath)
		{
			return VirtualPathProvider.OpenFile(virtualPath);
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x000362EA File Offset: 0x000352EA
		internal Stream OpenStream(VirtualPath virtualPath)
		{
			return virtualPath.OpenFile();
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x000362F2 File Offset: 0x000352F2
		protected TextReader OpenReader()
		{
			return this.OpenReader(this.VirtualPathObject);
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x00036300 File Offset: 0x00035300
		protected TextReader OpenReader(string virtualPath)
		{
			return this.OpenReader(global::System.Web.VirtualPath.Create(virtualPath));
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00036310 File Offset: 0x00035310
		internal TextReader OpenReader(VirtualPath virtualPath)
		{
			Stream stream = this.OpenStream(virtualPath);
			return Util.ReaderFromStream(stream, virtualPath);
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000D1E RID: 3358 RVA: 0x0003632C File Offset: 0x0003532C
		protected ICollection ReferencedAssemblies
		{
			get
			{
				return this._referencedAssemblies;
			}
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x00036334 File Offset: 0x00035334
		protected CompilerType GetDefaultCompilerTypeForLanguage(string language)
		{
			return CompilationUtil.GetCompilerInfoFromLanguage(this.VirtualPathObject, language);
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00036342 File Offset: 0x00035342
		protected CompilerType GetDefaultCompilerType()
		{
			return CompilationUtil.GetDefaultLanguageCompilerInfo(null, this.VirtualPathObject);
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000D21 RID: 3361 RVA: 0x00036350 File Offset: 0x00035350
		internal BuildProviderSet BuildProviderDependencies
		{
			get
			{
				return this._buildProviderDependencies;
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000D22 RID: 3362 RVA: 0x00036358 File Offset: 0x00035358
		internal bool IsDependedOn
		{
			get
			{
				return this.flags[1];
			}
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00036366 File Offset: 0x00035366
		internal void SetNoBuildResult()
		{
			this.flags[2] = true;
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00036375 File Offset: 0x00035375
		internal void SetContributedCode()
		{
			this.flags[32] = true;
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00036385 File Offset: 0x00035385
		internal void SetVirtualPath(VirtualPath virtualPath)
		{
			this._virtualPath = virtualPath;
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0003638E File Offset: 0x0003538E
		internal void SetReferencedAssemblies(ICollection referencedAssemblies)
		{
			this._referencedAssemblies = referencedAssemblies;
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x00036397 File Offset: 0x00035397
		internal void AddBuildProviderDependency(BuildProvider dependentBuildProvider)
		{
			if (this._buildProviderDependencies == null)
			{
				this._buildProviderDependencies = new BuildProviderSet();
			}
			this._buildProviderDependencies.Add(dependentBuildProvider);
			dependentBuildProvider.flags[1] = true;
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x000363C5 File Offset: 0x000353C5
		internal string GetCultureName()
		{
			return Util.GetCultureName(this.VirtualPath);
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x000363D4 File Offset: 0x000353D4
		internal BuildResult GetBuildResult(CompilerResults results)
		{
			BuildResult buildResult = this.CreateBuildResult(results);
			if (buildResult == null)
			{
				return null;
			}
			buildResult.VirtualPath = this.VirtualPathObject;
			this.SetBuildResultDependencies(buildResult);
			return buildResult;
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x00036404 File Offset: 0x00035404
		internal virtual BuildResult CreateBuildResult(CompilerResults results)
		{
			if (this.flags[2])
			{
				return null;
			}
			Type generatedType = this.GetGeneratedType(results);
			BuildResult buildResult;
			if (generatedType != null)
			{
				BuildResultCompiledType buildResultCompiledType = this.CreateBuildResult(generatedType);
				if (results == null || generatedType.Assembly != results.CompiledAssembly)
				{
					buildResultCompiledType.UsesExistingAssembly = true;
				}
				buildResult = buildResultCompiledType;
			}
			else
			{
				string customString = this.GetCustomString(results);
				if (customString != null)
				{
					buildResult = new BuildResultCustomString(this.flags[32] ? results.CompiledAssembly : null, customString);
				}
				else
				{
					if (results == null)
					{
						return null;
					}
					buildResult = new BuildResultCompiledAssembly(results.CompiledAssembly);
				}
			}
			int num = (int)this.GetResultFlags(results);
			if (num != 0)
			{
				num &= 65535;
				buildResult.Flags |= num;
			}
			return buildResult;
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x000364B4 File Offset: 0x000354B4
		internal virtual BuildResultCompiledType CreateBuildResult(Type t)
		{
			return new BuildResultCompiledType(t);
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x000364BC File Offset: 0x000354BC
		internal void SetBuildResultDependencies(BuildResult result)
		{
			result.AddVirtualPathDependencies(this.VirtualPathDependencies);
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x000364CC File Offset: 0x000354CC
		internal static CompilerType GetCompilerTypeFromBuildProvider(BuildProvider buildProvider)
		{
			HttpContext httpContext = null;
			if (EtwTrace.IsTraceEnabled(5, 1) && (httpContext = HttpContext.Current) != null)
			{
				EtwTrace.Trace(EtwTraceType.ETW_TYPE_PARSE_ENTER, httpContext.WorkerRequest);
			}
			CompilerType compilerType;
			try
			{
				CompilerType codeCompilerType = buildProvider.CodeCompilerType;
				if (codeCompilerType != null)
				{
					CompilationUtil.CheckCompilerOptionsAllowed(codeCompilerType.CompilerParameters.CompilerOptions, false, null, 0);
				}
				compilerType = codeCompilerType;
			}
			finally
			{
				if (EtwTrace.IsTraceEnabled(5, 1) && httpContext != null)
				{
					EtwTrace.Trace(EtwTraceType.ETW_TYPE_PARSE_LEAVE, httpContext.WorkerRequest);
				}
			}
			return compilerType;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x00036548 File Offset: 0x00035548
		internal static string GetDisplayName(BuildProvider buildProvider)
		{
			if (buildProvider.VirtualPath != null)
			{
				return buildProvider.VirtualPath;
			}
			return buildProvider.GetType().Name;
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x00036564 File Offset: 0x00035564
		internal virtual ICollection GetGeneratedTypeNames()
		{
			return null;
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000D30 RID: 3376 RVA: 0x00036567 File Offset: 0x00035567
		// (set) Token: 0x06000D31 RID: 3377 RVA: 0x00036575 File Offset: 0x00035575
		internal virtual bool IgnoreParseErrors
		{
			get
			{
				return this.flags[4];
			}
			set
			{
				this.flags[4] = value;
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000D32 RID: 3378 RVA: 0x00036584 File Offset: 0x00035584
		// (set) Token: 0x06000D33 RID: 3379 RVA: 0x00036592 File Offset: 0x00035592
		internal bool IgnoreControlProperties
		{
			get
			{
				return this.flags[8];
			}
			set
			{
				this.flags[8] = value;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000D34 RID: 3380 RVA: 0x000365A1 File Offset: 0x000355A1
		// (set) Token: 0x06000D35 RID: 3381 RVA: 0x000365B3 File Offset: 0x000355B3
		internal bool ThrowOnFirstParseError
		{
			get
			{
				return !this.flags[16];
			}
			set
			{
				this.flags[16] = !value;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000D36 RID: 3382 RVA: 0x000365C6 File Offset: 0x000355C6
		internal virtual IAssemblyDependencyParser AssemblyDependencyParser
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x000365CC File Offset: 0x000355CC
		protected internal virtual CodeCompileUnit GetCodeCompileUnit(out IDictionary linePragmasTable)
		{
			string text = Util.StringFromVirtualPath(this.VirtualPathObject);
			CodeSnippetCompileUnit codeSnippetCompileUnit = new CodeSnippetCompileUnit(text);
			LinePragmaCodeInfo linePragmaCodeInfo = new LinePragmaCodeInfo(1, 1, 1, -1, false);
			linePragmasTable = new Hashtable();
			linePragmasTable[1] = linePragmaCodeInfo;
			return codeSnippetCompileUnit;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0003660D File Offset: 0x0003560D
		internal virtual ICollection GetCompileWithDependencies()
		{
			return null;
		}

		// Token: 0x040014DF RID: 5343
		internal const int isDependedOn = 1;

		// Token: 0x040014E0 RID: 5344
		internal const int noBuildResult = 2;

		// Token: 0x040014E1 RID: 5345
		internal const int ignoreParseErrors = 4;

		// Token: 0x040014E2 RID: 5346
		internal const int ignoreControlProperties = 8;

		// Token: 0x040014E3 RID: 5347
		internal const int dontThrowOnFirstParseError = 16;

		// Token: 0x040014E4 RID: 5348
		internal const int contributedCode = 32;

		// Token: 0x040014E5 RID: 5349
		internal SimpleBitVector32 flags;

		// Token: 0x040014E6 RID: 5350
		private VirtualPath _virtualPath;

		// Token: 0x040014E7 RID: 5351
		private ICollection _referencedAssemblies;

		// Token: 0x040014E8 RID: 5352
		private BuildProviderSet _buildProviderDependencies;
	}
}
