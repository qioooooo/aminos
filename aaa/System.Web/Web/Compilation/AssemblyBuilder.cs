using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Management;
using System.Web.UI;
using System.Web.Util;
using System.Xml;
using System.Xml.Schema;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

namespace System.Web.Compilation
{
	// Token: 0x0200012A RID: 298
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class AssemblyBuilder
	{
		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000D81 RID: 3457 RVA: 0x00037DB1 File Offset: 0x00036DB1
		internal ICollection BuildProviders
		{
			get
			{
				return this._buildProviders.Values;
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x00037DBE File Offset: 0x00036DBE
		internal Type CodeDomProviderType
		{
			get
			{
				return this._compilerType.CodeDomProviderType;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000D83 RID: 3459 RVA: 0x00037DCB File Offset: 0x00036DCB
		internal StringResourceBuilder StringResourceBuilder
		{
			get
			{
				if (this._stringResourceBuilder == null)
				{
					this._stringResourceBuilder = new StringResourceBuilder();
				}
				return this._stringResourceBuilder;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x00037DE6 File Offset: 0x00036DE6
		// (set) Token: 0x06000D85 RID: 3461 RVA: 0x00037DEE File Offset: 0x00036DEE
		internal string CultureName
		{
			get
			{
				return this._cultureName;
			}
			set
			{
				this._cultureName = value;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000D86 RID: 3462 RVA: 0x00037DF8 File Offset: 0x00036DF8
		private string OutputAssemblyName
		{
			get
			{
				if (this._outputAssemblyName == null)
				{
					string basePath = this._tempFiles.BasePath;
					string fileName = Path.GetFileName(basePath);
					this._outputAssemblyName = "App_Web_" + fileName;
				}
				return this._outputAssemblyName;
			}
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00037E38 File Offset: 0x00036E38
		internal bool ContainsTypeNames(ICollection typeNames)
		{
			if (this._registeredTypeNames != null && typeNames != null)
			{
				foreach (object obj in typeNames)
				{
					string text = (string)obj;
					if (this._registeredTypeNames.Contains(text))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00037EA8 File Offset: 0x00036EA8
		internal void AddTypeNames(ICollection typeNames)
		{
			if (typeNames == null)
			{
				return;
			}
			if (this._registeredTypeNames == null)
			{
				this._registeredTypeNames = new CaseInsensitiveStringSet();
			}
			this._registeredTypeNames.AddCollection(typeNames);
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x00037ED0 File Offset: 0x00036ED0
		internal AssemblyBuilder(CompilationSection compConfig, ICollection referencedAssemblies, CompilerType compilerType, string outputAssemblyName)
		{
			this._compConfig = compConfig;
			this._outputAssemblyName = outputAssemblyName;
			this._initialReferencedAssemblies = AssemblySet.Create(referencedAssemblies);
			this._compilerType = compilerType.Clone();
			if (BuildManager.PrecompilingWithDebugInfo)
			{
				this._compilerType.CompilerParameters.IncludeDebugInformation = true;
			}
			else if (BuildManager.PrecompilingForDeployment)
			{
				this._compilerType.CompilerParameters.IncludeDebugInformation = false;
			}
			else if (DeploymentSection.RetailInternal)
			{
				this._compilerType.CompilerParameters.IncludeDebugInformation = false;
			}
			else if (this._compConfig.AssemblyPostProcessorTypeInternal != null)
			{
				this._compilerType.CompilerParameters.IncludeDebugInformation = true;
			}
			this._tempFiles.KeepFiles = this._compilerType.CompilerParameters.IncludeDebugInformation;
			this._codeProvider = CompilationUtil.CreateCodeDomProviderNonPublic(this._compilerType.CodeDomProviderType);
			this._maxBatchSize = this._compConfig.MaxBatchSize;
			this._maxBatchGeneratedFileSize = (long)(this._compConfig.MaxBatchGeneratedFileSize * 1024);
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x00037FFB File Offset: 0x00036FFB
		public void AddAssemblyReference(Assembly a)
		{
			if (this._additionalReferencedAssemblies == null)
			{
				this._additionalReferencedAssemblies = new AssemblySet();
			}
			this._additionalReferencedAssemblies.Add(a);
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x0003801C File Offset: 0x0003701C
		internal void AddAssemblyReference(Assembly a, CodeCompileUnit ccu)
		{
			this.AddAssemblyReference(a);
			Util.AddAssemblyToStringCollection(a, ccu.ReferencedAssemblies);
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x00038034 File Offset: 0x00037034
		internal virtual TextWriter CreateCodeFile(BuildProvider buildProvider, out string filename)
		{
			string tempFilePhysicalPath = this.GetTempFilePhysicalPath(this._codeProvider.FileExtension);
			filename = tempFilePhysicalPath;
			if (buildProvider != null)
			{
				if (this._buildProviderToSourceFileMap == null)
				{
					this._buildProviderToSourceFileMap = new Hashtable();
				}
				this._buildProviderToSourceFileMap[buildProvider] = tempFilePhysicalPath;
				buildProvider.SetContributedCode();
			}
			this._sourceFiles.Add(tempFilePhysicalPath);
			Stream stream = new FileStream(tempFilePhysicalPath, FileMode.Create, FileAccess.Write, FileShare.Read);
			return new StreamWriter(stream, Encoding.UTF8);
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x000380A0 File Offset: 0x000370A0
		public TextWriter CreateCodeFile(BuildProvider buildProvider)
		{
			string text;
			return this.CreateCodeFile(buildProvider, out text);
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x000380B6 File Offset: 0x000370B6
		internal bool IsBatchFull
		{
			get
			{
				return this._sourceFiles.Count >= this._maxBatchSize || this._totalFileLength >= this._maxBatchGeneratedFileSize;
			}
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x000380E0 File Offset: 0x000370E0
		public void AddCodeCompileUnit(BuildProvider buildProvider, CodeCompileUnit compileUnit)
		{
			this.AddChecksumPragma(buildProvider, compileUnit);
			Util.AddAssembliesToStringCollection(this._initialReferencedAssemblies, compileUnit.ReferencedAssemblies);
			Util.AddAssembliesToStringCollection(this._additionalReferencedAssemblies, compileUnit.ReferencedAssemblies);
			string text;
			using (new ProcessImpersonationContext())
			{
				TextWriter textWriter = this.CreateCodeFile(buildProvider, out text);
				try
				{
					this._codeProvider.GenerateCodeFromCompileUnit(compileUnit, textWriter, null);
				}
				finally
				{
					textWriter.Flush();
					textWriter.Close();
				}
			}
			if (text != null)
			{
				FileInfo fileInfo = new FileInfo(text);
				this._totalFileLength += fileInfo.Length;
			}
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0003818C File Offset: 0x0003718C
		public void GenerateTypeFactory(string typeName)
		{
			if (this._objectFactoryGenerator == null)
			{
				this._objectFactoryGenerator = new ObjectFactoryCodeDomTreeGenerator(this.OutputAssemblyName);
			}
			this._objectFactoryGenerator.AddFactoryMethod(typeName);
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x000381B4 File Offset: 0x000371B4
		public Stream CreateEmbeddedResource(BuildProvider buildProvider, string name)
		{
			if (!Util.IsValidFileName(name))
			{
				throw new ArgumentException(null, name);
			}
			string text = Path.Combine(this._tempFiles.TempDir, name);
			this._tempFiles.AddFile(text, this._tempFiles.KeepFiles);
			if (this._embeddedResourceFiles == null)
			{
				this._embeddedResourceFiles = new StringSet();
			}
			this._embeddedResourceFiles.Add(text);
			return File.OpenWrite(text);
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x0003821F File Offset: 0x0003721F
		public CodeDomProvider CodeDomProvider
		{
			get
			{
				return this._codeProvider;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000D93 RID: 3475 RVA: 0x00038228 File Offset: 0x00037228
		private string TempFilePhysicalPathPrefix
		{
			get
			{
				if (this._tempFilePhysicalPathPrefix == null)
				{
					this._tempFilePhysicalPathPrefix = Path.Combine(this._tempFiles.TempDir, this.OutputAssemblyName) + ".";
					if (this.CultureName != null)
					{
						this._tempFilePhysicalPathPrefix = this._tempFilePhysicalPathPrefix + this.CultureName + "_";
					}
				}
				return this._tempFilePhysicalPathPrefix;
			}
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00038290 File Offset: 0x00037290
		public string GetTempFilePhysicalPath(string extension)
		{
			string text;
			if (!string.IsNullOrEmpty(extension) && extension[0] == '.')
			{
				text = this.TempFilePhysicalPathPrefix + this._fileCount++ + extension;
			}
			else
			{
				text = string.Concat(new object[]
				{
					this.TempFilePhysicalPathPrefix,
					this._fileCount++,
					".",
					extension
				});
			}
			this._tempFiles.AddFile(text, this._tempFiles.KeepFiles);
			InternalSecurityPermissions.PathDiscovery(text).Demand();
			return text;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x00038334 File Offset: 0x00037334
		private void AddCompileWithBuildProvider(VirtualPath virtualPath, BuildProvider owningBuildProvider)
		{
			BuildProvider buildProvider = BuildManager.CreateBuildProvider(virtualPath, this._compConfig, this._initialReferencedAssemblies, true);
			buildProvider.SetNoBuildResult();
			SourceFileBuildProvider sourceFileBuildProvider = buildProvider as SourceFileBuildProvider;
			if (sourceFileBuildProvider != null)
			{
				sourceFileBuildProvider.OwningBuildProvider = owningBuildProvider;
			}
			this.AddBuildProvider(buildProvider);
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00038374 File Offset: 0x00037374
		internal virtual void AddBuildProvider(BuildProvider buildProvider)
		{
			object obj = buildProvider;
			if (buildProvider.VirtualPath != null)
			{
				obj = buildProvider.VirtualPath;
				if (this._buildProviders.ContainsKey(obj))
				{
					return;
				}
			}
			this._buildProviders[obj] = buildProvider;
			try
			{
				buildProvider.GenerateCode(this);
			}
			catch (XmlException ex)
			{
				throw new HttpParseException(ex.Message, null, buildProvider.VirtualPath, null, ex.LineNumber);
			}
			catch (XmlSchemaException ex2)
			{
				throw new HttpParseException(ex2.Message, null, buildProvider.VirtualPath, null, ex2.LineNumber);
			}
			catch (Exception ex3)
			{
				throw new HttpParseException(ex3.Message, ex3, buildProvider.VirtualPath, null, 1);
			}
			InternalBuildProvider internalBuildProvider = buildProvider as InternalBuildProvider;
			if (internalBuildProvider != null)
			{
				ICollection compileWithDependencies = internalBuildProvider.GetCompileWithDependencies();
				if (compileWithDependencies != null)
				{
					foreach (object obj2 in compileWithDependencies)
					{
						VirtualPath virtualPath = (VirtualPath)obj2;
						if (!this._buildProviders.ContainsKey(virtualPath.VirtualPathString))
						{
							this.AddCompileWithBuildProvider(virtualPath, internalBuildProvider);
						}
					}
				}
			}
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x000384A8 File Offset: 0x000374A8
		private void AddAssemblyCultureAttribute()
		{
			if (this.CultureName == null)
			{
				return;
			}
			CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(new CodeTypeReference(typeof(AssemblyCultureAttribute)), new CodeAttributeArgument[]
			{
				new CodeAttributeArgument(new CodePrimitiveExpression(this.CultureName))
			});
			this.AddAssemblyAttribute(codeAttributeDeclaration);
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x000384F8 File Offset: 0x000374F8
		private void AddAspNetGeneratedCodeAttribute()
		{
			this.AddAssemblyAttribute(new CodeAttributeDeclaration(new CodeTypeReference(typeof(GeneratedCodeAttribute)))
			{
				Arguments = 
				{
					new CodeAttributeArgument(new CodePrimitiveExpression("ASP.NET")),
					new CodeAttributeArgument(new CodePrimitiveExpression(VersionInfo.SystemWebVersion))
				}
			});
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00038558 File Offset: 0x00037558
		private void AddAllowPartiallyTrustedCallersAttribute()
		{
			if (BuildManager.CompileWithAllowPartiallyTrustedCallersAttribute)
			{
				CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(new CodeTypeReference(typeof(AllowPartiallyTrustedCallersAttribute)));
				this.AddAssemblyAttribute(codeAttributeDeclaration);
			}
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00038588 File Offset: 0x00037588
		private void AddAssemblyKeyFileAttribute()
		{
			if (!string.IsNullOrEmpty(BuildManager.StrongNameKeyFile))
			{
				CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(new CodeTypeReference(typeof(AssemblyKeyFileAttribute)), new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(new CodePrimitiveExpression(BuildManager.StrongNameKeyFile))
				});
				this.AddAssemblyAttribute(codeAttributeDeclaration);
			}
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x000385D8 File Offset: 0x000375D8
		private void AddAssemblyKeyContainerAttribute()
		{
			if (!string.IsNullOrEmpty(BuildManager.StrongNameKeyContainer))
			{
				CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(new CodeTypeReference(typeof(AssemblyKeyNameAttribute)), new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(new CodePrimitiveExpression(BuildManager.StrongNameKeyContainer))
				});
				this.AddAssemblyAttribute(codeAttributeDeclaration);
			}
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00038628 File Offset: 0x00037628
		private void AddAssemblyDelaySignAttribute()
		{
			if (BuildManager.CompileWithDelaySignAttribute)
			{
				CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(new CodeTypeReference(typeof(AssemblyDelaySignAttribute)), new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(new CodePrimitiveExpression(true))
				});
				this.AddAssemblyAttribute(codeAttributeDeclaration);
			}
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00038673 File Offset: 0x00037673
		private void AddAssemblyAttribute(CodeAttributeDeclaration declaration)
		{
			if (this._miscCodeCompileUnit == null)
			{
				this._miscCodeCompileUnit = new CodeCompileUnit();
			}
			this._miscCodeCompileUnit.AssemblyCustomAttributes.Add(declaration);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0003869A File Offset: 0x0003769A
		private void GenerateMiscCodeCompileUnit()
		{
			if (this._miscCodeCompileUnit == null)
			{
				return;
			}
			this.AddCodeCompileUnit(null, this._miscCodeCompileUnit);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x000386B4 File Offset: 0x000376B4
		private void AddChecksumPragma(BuildProvider buildProvider, CodeCompileUnit compileUnit)
		{
			if (buildProvider == null || buildProvider.VirtualPath == null)
			{
				return;
			}
			if (!this._compilerType.CompilerParameters.IncludeDebugInformation)
			{
				return;
			}
			string text = HostingEnvironment.MapPathInternal(buildProvider.VirtualPath);
			if (!File.Exists(text))
			{
				return;
			}
			if (AssemblyBuilder.s_md5HashAlgorithm == null)
			{
				AssemblyBuilder.s_md5HashAlgorithm = new MD5CryptoServiceProvider();
				AssemblyBuilder.s_hashMD5Guid = new Guid(1080993376, 25807, 19586, 182, 240, 66, 212, 129, 114, 167, 153);
			}
			CodeChecksumPragma codeChecksumPragma = new CodeChecksumPragma();
			if (this._compConfig.UrlLinePragmas)
			{
				codeChecksumPragma.FileName = ErrorFormatter.MakeHttpLinePragma(buildProvider.VirtualPathObject.VirtualPathString);
			}
			else
			{
				codeChecksumPragma.FileName = text;
			}
			codeChecksumPragma.ChecksumAlgorithmId = AssemblyBuilder.s_hashMD5Guid;
			using (Stream stream = new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				codeChecksumPragma.ChecksumData = AssemblyBuilder.s_md5HashAlgorithm.ComputeHash(stream);
			}
			compileUnit.StartDirectives.Add(codeChecksumPragma);
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x000387C4 File Offset: 0x000377C4
		internal CompilerParameters GetCompilerParameters()
		{
			CompilerParameters compilerParameters = this._compilerType.CompilerParameters;
			string text = this._tempFiles.TempDir;
			if (this.CultureName != null)
			{
				text = Path.Combine(text, this.CultureName);
				Directory.CreateDirectory(text);
				compilerParameters.OutputAssembly = Path.Combine(text, this.OutputAssemblyName + ".resources.dll");
			}
			else
			{
				compilerParameters.OutputAssembly = Path.Combine(text, this.OutputAssemblyName + ".dll");
			}
			if (File.Exists(compilerParameters.OutputAssembly))
			{
				Util.RemoveOrRenameFile(compilerParameters.OutputAssembly);
			}
			compilerParameters.TempFiles = this._tempFiles;
			if (this._stringResourceBuilder != null && this._stringResourceBuilder.HasStrings)
			{
				string text2 = this._tempFiles.AddExtension("res");
				this._stringResourceBuilder.CreateResourceFile(text2);
				compilerParameters.Win32Resource = text2;
			}
			if (this._embeddedResourceFiles != null)
			{
				foreach (object obj in ((IEnumerable)this._embeddedResourceFiles))
				{
					string text3 = (string)obj;
					compilerParameters.EmbeddedResources.Add(text3);
				}
			}
			if (this._additionalReferencedAssemblies != null)
			{
				foreach (object obj2 in ((IEnumerable)this._additionalReferencedAssemblies))
				{
					Assembly assembly = (Assembly)obj2;
					this._initialReferencedAssemblies.Add(assembly);
				}
			}
			Util.AddAssembliesToStringCollection(this._initialReferencedAssemblies, compilerParameters.ReferencedAssemblies);
			AssemblyBuilder.FixUpCompilerParameters(this._compilerType.CodeDomProviderType, compilerParameters);
			return compilerParameters;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00038980 File Offset: 0x00037980
		private static void AddVBGlobalNamespaceImports(CompilerParameters compilParams)
		{
			if (AssemblyBuilder.s_vbImportsString == null)
			{
				PagesSection pages = RuntimeConfig.GetAppConfig().Pages;
				if (pages.Namespaces == null)
				{
					AssemblyBuilder.s_vbImportsString = string.Empty;
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("/imports:");
					bool flag = false;
					if (pages.Namespaces.AutoImportVBNamespace)
					{
						stringBuilder.Append("Microsoft.VisualBasic");
						flag = true;
					}
					foreach (object obj in pages.Namespaces)
					{
						NamespaceInfo namespaceInfo = (NamespaceInfo)obj;
						if (flag)
						{
							stringBuilder.Append(',');
						}
						stringBuilder.Append(namespaceInfo.Namespace);
						flag = true;
					}
					AssemblyBuilder.s_vbImportsString = stringBuilder.ToString();
				}
			}
			if (AssemblyBuilder.s_vbImportsString.Length > 0)
			{
				if (compilParams.CompilerOptions == null)
				{
					compilParams.CompilerOptions = AssemblyBuilder.s_vbImportsString;
					return;
				}
				compilParams.CompilerOptions = AssemblyBuilder.s_vbImportsString + " " + compilParams.CompilerOptions;
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00038A98 File Offset: 0x00037A98
		private static void AddVBMyFlags(CompilerParameters compilParams)
		{
			if (compilParams.CompilerOptions == null)
			{
				compilParams.CompilerOptions = "/define:_MYTYPE=\\\"Web\\\"";
				return;
			}
			compilParams.CompilerOptions = "/define:_MYTYPE=\\\"Web\\\" " + compilParams.CompilerOptions;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00038AC4 File Offset: 0x00037AC4
		internal static void FixUpCompilerParameters(Type codeDomProviderType, CompilerParameters compilParams)
		{
			if (codeDomProviderType == typeof(CSharpCodeProvider))
			{
				CodeDomUtility.PrependCompilerOption(compilParams, "/nowarn:1659;1699;1701");
			}
			else if (codeDomProviderType == typeof(VBCodeProvider))
			{
				AssemblyBuilder.AddVBGlobalNamespaceImports(compilParams);
				AssemblyBuilder.AddVBMyFlags(compilParams);
			}
			AssemblyBuilder.ProcessProviderOptions(codeDomProviderType, compilParams);
			AssemblyBuilder.FixTreatWarningsAsErrors(codeDomProviderType, compilParams);
			if (BuildManager.PrecompilingWithCodeAnalysisSymbol)
			{
				CodeDomUtility.PrependCompilerOption(compilParams, "/define:CODE_ANALYSIS");
			}
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00038B24 File Offset: 0x00037B24
		internal static void FixTreatWarningsAsErrors(Type codeDomProviderType, CompilerParameters compilParams)
		{
			if (codeDomProviderType != typeof(CSharpCodeProvider) && codeDomProviderType != typeof(VBCodeProvider))
			{
				return;
			}
			if (CultureInfo.InvariantCulture.CompareInfo.IndexOf(compilParams.CompilerOptions, "/warnaserror", CompareOptions.IgnoreCase) >= 0)
			{
				compilParams.TreatWarningsAsErrors = false;
			}
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00038B74 File Offset: 0x00037B74
		private static void ProcessProviderOptions(Type codeDomProviderType, CompilerParameters compilParams)
		{
			IDictionary<string, string> providerOptions = CompilationUtil.GetProviderOptions(codeDomProviderType);
			if (providerOptions == null)
			{
				return;
			}
			if (codeDomProviderType == typeof(VBCodeProvider) || codeDomProviderType == typeof(CSharpCodeProvider))
			{
				AssemblyBuilder.ProcessBooleanProviderOption("WarnAsError", "/warnaserror+", "/warnaserror-", providerOptions, compilParams);
			}
			if (codeDomProviderType == null || !CompilationUtil.IsCompilerVersion35(codeDomProviderType))
			{
				return;
			}
			if (codeDomProviderType == typeof(VBCodeProvider))
			{
				AssemblyBuilder.ProcessBooleanProviderOption("OptionInfer", "/optionInfer+", "/optionInfer-", providerOptions, compilParams);
			}
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00038BEC File Offset: 0x00037BEC
		private static void ProcessBooleanProviderOption(string providerOptionName, string trueCompilerOption, string falseCompilerOption, IDictionary<string, string> providerOptions, CompilerParameters compilParams)
		{
			if (providerOptions == null || compilParams == null)
			{
				return;
			}
			string text = null;
			if (!providerOptions.TryGetValue(providerOptionName, out text))
			{
				return;
			}
			if (string.IsNullOrEmpty(text))
			{
				throw new ConfigurationException(SR.GetString("Property_NullOrEmpty", new object[] { "system.codedom/compilers/compiler/ProviderOption/" + providerOptionName }));
			}
			bool flag;
			if (!bool.TryParse(text, out flag))
			{
				throw new ConfigurationException(SR.GetString("Value_must_be_boolean", new object[] { "system.codedom/compilers/compiler/ProviderOption/" + providerOptionName }));
			}
			if (flag)
			{
				CodeDomUtility.AppendCompilerOption(compilParams, trueCompilerOption);
				return;
			}
			CodeDomUtility.AppendCompilerOption(compilParams, falseCompilerOption);
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00038C84 File Offset: 0x00037C84
		internal CompilerResults Compile()
		{
			if (this._sourceFiles.Count == 0 && this._embeddedResourceFiles == null)
			{
				return null;
			}
			if (this._objectFactoryGenerator != null)
			{
				this._miscCodeCompileUnit = this._objectFactoryGenerator.CodeCompileUnit;
			}
			this.AddAssemblyCultureAttribute();
			this.AddAspNetGeneratedCodeAttribute();
			this.AddAllowPartiallyTrustedCallersAttribute();
			this.AddAssemblyDelaySignAttribute();
			this.AddAssemblyKeyFileAttribute();
			this.AddAssemblyKeyContainerAttribute();
			this.GenerateMiscCodeCompileUnit();
			CompilerParameters compilerParameters = this.GetCompilerParameters();
			string[] array = new string[this._sourceFiles.Count];
			this._sourceFiles.CopyTo(array, 0);
			PerfCounters.IncrementCounter(AppPerfCounter.COMPILATIONS);
			WebBaseEvent.RaiseSystemEvent(this, 1003);
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null && EtwTrace.IsTraceEnabled(5, 1))
			{
				EtwTrace.Trace(EtwTraceType.ETW_TYPE_COMPILE_ENTER, httpContext.WorkerRequest);
			}
			CompilerResults compilerResults = null;
			try
			{
				try
				{
					using (new ProcessImpersonationContext())
					{
						compilerResults = this._codeProvider.CompileAssemblyFromFile(compilerParameters, array);
					}
				}
				finally
				{
					if (EtwTrace.IsTraceEnabled(5, 1) && httpContext != null)
					{
						string text = null;
						if (this._buildProviders.Count < 20)
						{
							IDictionaryEnumerator enumerator = this._buildProviders.GetEnumerator();
							while (enumerator.MoveNext())
							{
								if (text != null)
								{
									text += ",";
								}
								text += enumerator.Key;
							}
						}
						else
						{
							text = string.Format(CultureInfo.InstalledUICulture, SR.Resources.GetString("Etw_Batch_Compilation", CultureInfo.InstalledUICulture), new object[] { this._buildProviders.Count });
						}
						string text2;
						if (compilerResults != null && (compilerResults.NativeCompilerReturnValue != 0 || compilerResults.Errors.HasErrors))
						{
							text2 = SR.Resources.GetString("Etw_Failure", CultureInfo.InstalledUICulture);
						}
						else
						{
							text2 = SR.Resources.GetString("Etw_Success", CultureInfo.InstalledUICulture);
						}
						EtwTrace.Trace(EtwTraceType.ETW_TYPE_COMPILE_LEAVE, httpContext.WorkerRequest, text, text2);
					}
				}
			}
			catch
			{
				throw;
			}
			Type assemblyPostProcessorTypeInternal = this._compConfig.AssemblyPostProcessorTypeInternal;
			if (assemblyPostProcessorTypeInternal != null)
			{
				using (IAssemblyPostProcessor assemblyPostProcessor = (IAssemblyPostProcessor)HttpRuntime.FastCreatePublicInstance(assemblyPostProcessorTypeInternal))
				{
					assemblyPostProcessor.PostProcessAssembly(compilerResults.PathToAssembly);
				}
			}
			WebBaseEvent.RaiseSystemEvent(this, 1004);
			if (compilerResults != null)
			{
				this.InvalidateInvalidAssembly(compilerResults, compilerParameters);
				this.FixUpLinePragmas(compilerResults);
				if (BuildManager.CBMCallback != null)
				{
					foreach (object obj in compilerResults.Errors)
					{
						CompilerError compilerError = (CompilerError)obj;
						BuildManager.CBMCallback.ReportCompilerError(compilerError);
					}
				}
				if (compilerResults.NativeCompilerReturnValue != 0 || compilerResults.Errors.HasErrors)
				{
					PerfCounters.IncrementCounter(AppPerfCounter.ERRORS_COMPILING);
					PerfCounters.IncrementCounter(AppPerfCounter.ERRORS_TOTAL);
					throw new HttpCompileException(compilerResults, this.GetErrorSourceFileContents(compilerResults));
				}
			}
			return compilerResults;
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00038FC0 File Offset: 0x00037FC0
		private void InvalidateInvalidAssembly(CompilerResults results, CompilerParameters compilParams)
		{
			if (results == null || !results.Errors.HasErrors)
			{
				return;
			}
			foreach (object obj in results.Errors)
			{
				CompilerError compilerError = (CompilerError)obj;
				if (!compilerError.IsWarning && StringUtil.EqualsIgnoreCase(compilerError.ErrorNumber, "CS0016"))
				{
					if (this.CultureName != null)
					{
						string tempDir = this._tempFiles.TempDir;
						string text = Path.Combine(tempDir, this.OutputAssemblyName + ".dll");
						DiskBuildResultCache.TryDeleteFile(new FileInfo(text));
					}
					DiskBuildResultCache.TryDeleteFile(compilParams.OutputAssembly);
				}
			}
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00039084 File Offset: 0x00038084
		private void FixUpLinePragmas(CompilerResults results)
		{
			CompilerError compilerError = null;
			for (int i = results.Errors.Count - 1; i >= 0; i--)
			{
				CompilerError compilerError2 = results.Errors[i];
				string text = ErrorFormatter.ResolveHttpFileName(compilerError2.FileName);
				if (File.Exists(text))
				{
					compilerError2.FileName = text;
					if (compilerError2.Line == 912304 || (compilerError2.Line == 912305 && compilerError2.ErrorText != null && compilerError2.ErrorText.IndexOf("FrameworkInitialize", StringComparison.OrdinalIgnoreCase) >= 0))
					{
						compilerError = compilerError2;
						results.Errors.RemoveAt(i);
					}
					else if (compilerError2.Line > 912304 && compilerError2.Line < 912354)
					{
						results.Errors.RemoveAt(i);
					}
				}
			}
			if (compilerError != null)
			{
				string text2 = Util.StringFromFile(compilerError.FileName);
				int num = CultureInfo.InvariantCulture.CompareInfo.IndexOf(text2, "partial class", CompareOptions.IgnoreCase);
				if (num >= 0)
				{
					compilerError.Line = Util.LineCount(text2, 0, num) + 1;
				}
				else
				{
					compilerError.Line = 1;
				}
				compilerError.ErrorText = SR.GetString("Bad_Base_Class_In_Code_File");
				compilerError.ErrorNumber = "ASPNET";
				results.Errors.Insert(0, compilerError);
			}
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x000391B8 File Offset: 0x000381B8
		private string GetErrorSourceFileContents(CompilerResults results)
		{
			if (!results.Errors.HasErrors)
			{
				return null;
			}
			string fileName = results.Errors[0].FileName;
			BuildProvider buildProviderFromLinePragma = this.GetBuildProviderFromLinePragma(fileName);
			if (buildProviderFromLinePragma != null)
			{
				return this.GetGeneratedSourceFromBuildProvider(buildProviderFromLinePragma);
			}
			return Util.StringFromFileIfExists(fileName);
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00039200 File Offset: 0x00038200
		internal string GetGeneratedSourceFromBuildProvider(BuildProvider buildProvider)
		{
			string text = (string)this._buildProviderToSourceFileMap[buildProvider];
			return Util.StringFromFileIfExists(text);
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00039228 File Offset: 0x00038228
		internal BuildProvider GetBuildProviderFromLinePragma(string linePragma)
		{
			BuildProvider buildProvider = this.GetBuildProviderFromLinePragmaInternal(linePragma);
			SourceFileBuildProvider sourceFileBuildProvider = buildProvider as SourceFileBuildProvider;
			if (sourceFileBuildProvider != null)
			{
				buildProvider = sourceFileBuildProvider.OwningBuildProvider;
			}
			return buildProvider;
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00039250 File Offset: 0x00038250
		private BuildProvider GetBuildProviderFromLinePragmaInternal(string linePragma)
		{
			if (this._buildProviderToSourceFileMap == null)
			{
				return null;
			}
			string virtualPathFromHttpLinePragma = ErrorFormatter.GetVirtualPathFromHttpLinePragma(linePragma);
			foreach (object obj in this.BuildProviders)
			{
				BuildProvider buildProvider = (BuildProvider)obj;
				if (buildProvider.VirtualPath != null)
				{
					if (virtualPathFromHttpLinePragma != null)
					{
						if (StringUtil.EqualsIgnoreCase(virtualPathFromHttpLinePragma, buildProvider.VirtualPath))
						{
							return buildProvider;
						}
					}
					else
					{
						string text = HostingEnvironment.MapPathInternal(buildProvider.VirtualPath);
						if (StringUtil.EqualsIgnoreCase(linePragma, text))
						{
							return buildProvider;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x04001504 RID: 5380
		private const string MySupport = "/define:_MYTYPE=\\\"Web\\\"";

		// Token: 0x04001505 RID: 5381
		private CompilationSection _compConfig;

		// Token: 0x04001506 RID: 5382
		private static HashAlgorithm s_md5HashAlgorithm;

		// Token: 0x04001507 RID: 5383
		private static Guid s_hashMD5Guid;

		// Token: 0x04001508 RID: 5384
		private Hashtable _buildProviders = new Hashtable(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04001509 RID: 5385
		private StringSet _sourceFiles = new StringSet();

		// Token: 0x0400150A RID: 5386
		private CodeCompileUnit _miscCodeCompileUnit;

		// Token: 0x0400150B RID: 5387
		private StringSet _embeddedResourceFiles;

		// Token: 0x0400150C RID: 5388
		private AssemblySet _initialReferencedAssemblies;

		// Token: 0x0400150D RID: 5389
		private AssemblySet _additionalReferencedAssemblies;

		// Token: 0x0400150E RID: 5390
		internal CodeDomProvider _codeProvider;

		// Token: 0x0400150F RID: 5391
		private Hashtable _buildProviderToSourceFileMap;

		// Token: 0x04001510 RID: 5392
		private CompilerType _compilerType;

		// Token: 0x04001511 RID: 5393
		private ObjectFactoryCodeDomTreeGenerator _objectFactoryGenerator;

		// Token: 0x04001512 RID: 5394
		private StringResourceBuilder _stringResourceBuilder;

		// Token: 0x04001513 RID: 5395
		private TempFileCollection _tempFiles = new TempFileCollection(HttpRuntime.CodegenDirInternal);

		// Token: 0x04001514 RID: 5396
		private int _fileCount;

		// Token: 0x04001515 RID: 5397
		private string _cultureName;

		// Token: 0x04001516 RID: 5398
		private string _outputAssemblyName;

		// Token: 0x04001517 RID: 5399
		private int _maxBatchSize;

		// Token: 0x04001518 RID: 5400
		private long _maxBatchGeneratedFileSize;

		// Token: 0x04001519 RID: 5401
		private long _totalFileLength;

		// Token: 0x0400151A RID: 5402
		private CaseInsensitiveStringSet _registeredTypeNames;

		// Token: 0x0400151B RID: 5403
		private string _tempFilePhysicalPathPrefix;

		// Token: 0x0400151C RID: 5404
		private static string s_vbImportsString;
	}
}
