using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x020001C6 RID: 454
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class CompilationSection : ConfigurationSection
	{
		// Token: 0x060019B7 RID: 6583 RVA: 0x000797FC File Offset: 0x000787FC
		static CompilationSection()
		{
			CompilationSection._properties.Add(CompilationSection._propTempDirectory);
			CompilationSection._properties.Add(CompilationSection._propDebug);
			CompilationSection._properties.Add(CompilationSection._propStrict);
			CompilationSection._properties.Add(CompilationSection._propExplicit);
			CompilationSection._properties.Add(CompilationSection._propBatch);
			CompilationSection._properties.Add(CompilationSection._propOptimizeCompilations);
			CompilationSection._properties.Add(CompilationSection._propBatchTimeout);
			CompilationSection._properties.Add(CompilationSection._propMaxBatchSize);
			CompilationSection._properties.Add(CompilationSection._propMaxBatchGeneratedFileSize);
			CompilationSection._properties.Add(CompilationSection._propNumRecompilesBeforeAppRestart);
			CompilationSection._properties.Add(CompilationSection._propDefaultLanguage);
			CompilationSection._properties.Add(CompilationSection._propCompilers);
			CompilationSection._properties.Add(CompilationSection._propAssemblies);
			CompilationSection._properties.Add(CompilationSection._propBuildProviders);
			CompilationSection._properties.Add(CompilationSection._propExpressionBuilders);
			CompilationSection._properties.Add(CompilationSection._propUrlLinePragmas);
			CompilationSection._properties.Add(CompilationSection._propCodeSubDirs);
			CompilationSection._properties.Add(CompilationSection._propAssemblyPreprocessorType);
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x060019B9 RID: 6585 RVA: 0x00079B75 File Offset: 0x00078B75
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return CompilationSection._properties;
			}
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x00079B7C File Offset: 0x00078B7C
		protected override object GetRuntimeObject()
		{
			this._isRuntimeObject = true;
			return base.GetRuntimeObject();
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x060019BB RID: 6587 RVA: 0x00079B8B File Offset: 0x00078B8B
		// (set) Token: 0x060019BC RID: 6588 RVA: 0x00079B9D File Offset: 0x00078B9D
		[ConfigurationProperty("tempDirectory", DefaultValue = "")]
		public string TempDirectory
		{
			get
			{
				return (string)base[CompilationSection._propTempDirectory];
			}
			set
			{
				base[CompilationSection._propTempDirectory] = value;
			}
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x00079BAC File Offset: 0x00078BAC
		internal void GetTempDirectoryErrorInfo(out string tempDirAttribName, out string configFileName, out int configLineNumber)
		{
			tempDirAttribName = "tempDirectory";
			configFileName = base.ElementInformation.Properties["tempDirectory"].Source;
			configLineNumber = base.ElementInformation.Properties["tempDirectory"].LineNumber;
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x060019BE RID: 6590 RVA: 0x00079BF8 File Offset: 0x00078BF8
		// (set) Token: 0x060019BF RID: 6591 RVA: 0x00079C0A File Offset: 0x00078C0A
		[ConfigurationProperty("debug", DefaultValue = false)]
		public bool Debug
		{
			get
			{
				return (bool)base[CompilationSection._propDebug];
			}
			set
			{
				base[CompilationSection._propDebug] = value;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x060019C0 RID: 6592 RVA: 0x00079C1D File Offset: 0x00078C1D
		// (set) Token: 0x060019C1 RID: 6593 RVA: 0x00079C2F File Offset: 0x00078C2F
		[ConfigurationProperty("strict", DefaultValue = false)]
		public bool Strict
		{
			get
			{
				return (bool)base[CompilationSection._propStrict];
			}
			set
			{
				base[CompilationSection._propStrict] = value;
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x060019C2 RID: 6594 RVA: 0x00079C42 File Offset: 0x00078C42
		// (set) Token: 0x060019C3 RID: 6595 RVA: 0x00079C54 File Offset: 0x00078C54
		[ConfigurationProperty("explicit", DefaultValue = true)]
		public bool Explicit
		{
			get
			{
				return (bool)base[CompilationSection._propExplicit];
			}
			set
			{
				base[CompilationSection._propExplicit] = value;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x060019C4 RID: 6596 RVA: 0x00079C67 File Offset: 0x00078C67
		// (set) Token: 0x060019C5 RID: 6597 RVA: 0x00079C79 File Offset: 0x00078C79
		[ConfigurationProperty("batch", DefaultValue = true)]
		public bool Batch
		{
			get
			{
				return (bool)base[CompilationSection._propBatch];
			}
			set
			{
				base[CompilationSection._propBatch] = value;
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x060019C6 RID: 6598 RVA: 0x00079C8C File Offset: 0x00078C8C
		// (set) Token: 0x060019C7 RID: 6599 RVA: 0x00079C9E File Offset: 0x00078C9E
		[ConfigurationProperty("optimizeCompilations", DefaultValue = false)]
		public bool OptimizeCompilations
		{
			get
			{
				return (bool)base[CompilationSection._propOptimizeCompilations];
			}
			set
			{
				base[CompilationSection._propOptimizeCompilations] = value;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x060019C8 RID: 6600 RVA: 0x00079CB1 File Offset: 0x00078CB1
		// (set) Token: 0x060019C9 RID: 6601 RVA: 0x00079CC3 File Offset: 0x00078CC3
		[ConfigurationProperty("urlLinePragmas", DefaultValue = false)]
		public bool UrlLinePragmas
		{
			get
			{
				return (bool)base[CompilationSection._propUrlLinePragmas];
			}
			set
			{
				base[CompilationSection._propUrlLinePragmas] = value;
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x060019CA RID: 6602 RVA: 0x00079CD6 File Offset: 0x00078CD6
		// (set) Token: 0x060019CB RID: 6603 RVA: 0x00079CE8 File Offset: 0x00078CE8
		[ConfigurationProperty("batchTimeout", DefaultValue = "00:15:00")]
		[TypeConverter(typeof(TimeSpanSecondsOrInfiniteConverter))]
		[TimeSpanValidator(MinValueString = "00:00:00", MaxValueString = "10675199.02:48:05.4775807")]
		public TimeSpan BatchTimeout
		{
			get
			{
				return (TimeSpan)base[CompilationSection._propBatchTimeout];
			}
			set
			{
				base[CompilationSection._propBatchTimeout] = value;
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x060019CC RID: 6604 RVA: 0x00079CFB File Offset: 0x00078CFB
		// (set) Token: 0x060019CD RID: 6605 RVA: 0x00079D0D File Offset: 0x00078D0D
		[ConfigurationProperty("maxBatchSize", DefaultValue = 1000)]
		public int MaxBatchSize
		{
			get
			{
				return (int)base[CompilationSection._propMaxBatchSize];
			}
			set
			{
				base[CompilationSection._propMaxBatchSize] = value;
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x060019CE RID: 6606 RVA: 0x00079D20 File Offset: 0x00078D20
		// (set) Token: 0x060019CF RID: 6607 RVA: 0x00079D32 File Offset: 0x00078D32
		[ConfigurationProperty("maxBatchGeneratedFileSize", DefaultValue = 1000)]
		public int MaxBatchGeneratedFileSize
		{
			get
			{
				return (int)base[CompilationSection._propMaxBatchGeneratedFileSize];
			}
			set
			{
				base[CompilationSection._propMaxBatchGeneratedFileSize] = value;
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x060019D0 RID: 6608 RVA: 0x00079D45 File Offset: 0x00078D45
		// (set) Token: 0x060019D1 RID: 6609 RVA: 0x00079D57 File Offset: 0x00078D57
		[ConfigurationProperty("numRecompilesBeforeAppRestart", DefaultValue = 15)]
		public int NumRecompilesBeforeAppRestart
		{
			get
			{
				return (int)base[CompilationSection._propNumRecompilesBeforeAppRestart];
			}
			set
			{
				base[CompilationSection._propNumRecompilesBeforeAppRestart] = value;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x060019D2 RID: 6610 RVA: 0x00079D6A File Offset: 0x00078D6A
		// (set) Token: 0x060019D3 RID: 6611 RVA: 0x00079D7C File Offset: 0x00078D7C
		[ConfigurationProperty("defaultLanguage", DefaultValue = "vb")]
		public string DefaultLanguage
		{
			get
			{
				return (string)base[CompilationSection._propDefaultLanguage];
			}
			set
			{
				base[CompilationSection._propDefaultLanguage] = value;
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x060019D4 RID: 6612 RVA: 0x00079D8A File Offset: 0x00078D8A
		[ConfigurationProperty("compilers")]
		public CompilerCollection Compilers
		{
			get
			{
				return (CompilerCollection)base[CompilationSection._propCompilers];
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x060019D5 RID: 6613 RVA: 0x00079D9C File Offset: 0x00078D9C
		[ConfigurationProperty("assemblies")]
		public AssemblyCollection Assemblies
		{
			get
			{
				if (this._isRuntimeObject)
				{
					this.EnsureReferenceSet();
				}
				return this.GetAssembliesCollection();
			}
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x00079DB2 File Offset: 0x00078DB2
		private AssemblyCollection GetAssembliesCollection()
		{
			return (AssemblyCollection)base[CompilationSection._propAssemblies];
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x060019D7 RID: 6615 RVA: 0x00079DC4 File Offset: 0x00078DC4
		[ConfigurationProperty("buildProviders")]
		public BuildProviderCollection BuildProviders
		{
			get
			{
				return (BuildProviderCollection)base[CompilationSection._propBuildProviders];
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x060019D8 RID: 6616 RVA: 0x00079DD6 File Offset: 0x00078DD6
		[ConfigurationProperty("expressionBuilders")]
		public ExpressionBuilderCollection ExpressionBuilders
		{
			get
			{
				return (ExpressionBuilderCollection)base[CompilationSection._propExpressionBuilders];
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x060019D9 RID: 6617 RVA: 0x00079DE8 File Offset: 0x00078DE8
		// (set) Token: 0x060019DA RID: 6618 RVA: 0x00079DFA File Offset: 0x00078DFA
		[ConfigurationProperty("assemblyPostProcessorType", DefaultValue = "")]
		public string AssemblyPostProcessorType
		{
			get
			{
				return (string)base[CompilationSection._propAssemblyPreprocessorType];
			}
			set
			{
				base[CompilationSection._propAssemblyPreprocessorType] = value;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x060019DB RID: 6619 RVA: 0x00079E08 File Offset: 0x00078E08
		internal Type AssemblyPostProcessorTypeInternal
		{
			get
			{
				if (this._assemblyPostProcessorType == null && !string.IsNullOrEmpty(this.AssemblyPostProcessorType))
				{
					lock (this)
					{
						if (this._assemblyPostProcessorType == null)
						{
							if (!HttpRuntime.HasUnmanagedPermission())
							{
								throw new ConfigurationErrorsException(SR.GetString("Insufficient_trust_for_attribute", new object[] { "assemblyPostProcessorType" }), base.ElementInformation.Properties["assemblyPostProcessorType"].Source, base.ElementInformation.Properties["assemblyPostProcessorType"].LineNumber);
							}
							Type type = ConfigUtil.GetType(this.AssemblyPostProcessorType, "assemblyPostProcessorType", this);
							ConfigUtil.CheckBaseType(typeof(IAssemblyPostProcessor), type, "assemblyPostProcessorType", this);
							this._assemblyPostProcessorType = type;
						}
					}
				}
				return this._assemblyPostProcessorType;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x060019DC RID: 6620 RVA: 0x00079EEC File Offset: 0x00078EEC
		[ConfigurationProperty("codeSubDirectories")]
		public CodeSubDirectoriesCollection CodeSubDirectories
		{
			get
			{
				return (CodeSubDirectoriesCollection)base[CompilationSection._propCodeSubDirs];
			}
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x00079F00 File Offset: 0x00078F00
		private void EnsureCompilerCacheInit()
		{
			if (this._compilerLanguages == null)
			{
				lock (this)
				{
					if (this._compilerLanguages == null)
					{
						Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
						this._compilerExtensions = new Hashtable(StringComparer.OrdinalIgnoreCase);
						foreach (object obj in this.Compilers)
						{
							Compiler compiler = (Compiler)obj;
							string[] array = compiler.Language.Split(new char[] { ';' });
							string[] array2 = compiler.Extension.Split(new char[] { ';' });
							foreach (string text in array)
							{
								hashtable[text] = compiler;
							}
							foreach (string text2 in array2)
							{
								this._compilerExtensions[text2] = compiler;
							}
						}
						this._compilerLanguages = hashtable;
					}
				}
			}
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x0007A040 File Offset: 0x00079040
		internal CompilerType GetCompilerInfoFromExtension(string extension, bool throwOnFail)
		{
			this.EnsureCompilerCacheInit();
			object obj = this._compilerExtensions[extension];
			Compiler compiler = obj as Compiler;
			CompilerType compilerType;
			if (compiler != null)
			{
				compilerType = compiler.CompilerTypeInternal;
				this._compilerExtensions[extension] = compilerType;
			}
			else
			{
				compilerType = obj as CompilerType;
			}
			if (compilerType == null && CodeDomProvider.IsDefinedExtension(extension))
			{
				string languageFromExtension = CodeDomProvider.GetLanguageFromExtension(extension);
				CompilerInfo compilerInfo = CodeDomProvider.GetCompilerInfo(languageFromExtension);
				compilerType = new CompilerType(compilerInfo.CodeDomProviderType, compilerInfo.CreateDefaultCompilerParameters());
				this._compilerExtensions[extension] = compilerType;
			}
			if (compilerType != null)
			{
				compilerType = compilerType.Clone();
				compilerType.CompilerParameters.IncludeDebugInformation = this.Debug;
				return compilerType;
			}
			if (!throwOnFail)
			{
				return null;
			}
			throw new HttpException(SR.GetString("Invalid_lang_extension", new object[] { extension }));
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x0007A104 File Offset: 0x00079104
		internal CompilerType GetCompilerInfoFromLanguage(string language)
		{
			this.EnsureCompilerCacheInit();
			object obj = this._compilerLanguages[language];
			Compiler compiler = obj as Compiler;
			CompilerType compilerType;
			if (compiler != null)
			{
				compilerType = compiler.CompilerTypeInternal;
				this._compilerLanguages[language] = compilerType;
			}
			else
			{
				compilerType = obj as CompilerType;
			}
			if (compilerType == null && CodeDomProvider.IsDefinedLanguage(language))
			{
				CompilerInfo compilerInfo = CodeDomProvider.GetCompilerInfo(language);
				compilerType = new CompilerType(compilerInfo.CodeDomProviderType, compilerInfo.CreateDefaultCompilerParameters());
				this._compilerLanguages[language] = compilerType;
			}
			if (compilerType == null)
			{
				throw new HttpException(SR.GetString("Invalid_lang", new object[] { language }));
			}
			CompilationUtil.CheckCompilerOptionsAllowed(compilerType.CompilerParameters.CompilerOptions, true, null, 0);
			compilerType = compilerType.Clone();
			compilerType.CompilerParameters.IncludeDebugInformation = this.Debug;
			return compilerType;
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x0007A1CC File Offset: 0x000791CC
		private void EnsureReferenceSet()
		{
			if (!this._referenceSet)
			{
				foreach (object obj in this.GetAssembliesCollection())
				{
					AssemblyInfo assemblyInfo = (AssemblyInfo)obj;
					assemblyInfo.SetCompilationReference(this);
				}
				this._referenceSet = true;
			}
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x0007A234 File Offset: 0x00079234
		internal Assembly[] LoadAssembly(AssemblyInfo ai)
		{
			Assembly[] array = null;
			if (ai.Assembly == "*")
			{
				array = this.LoadAllAssembliesFromAppDomainBinDirectory();
			}
			else
			{
				Assembly assembly = this.LoadAssemblyHelper(ai.Assembly, false);
				if (assembly != null)
				{
					array = new Assembly[] { assembly };
				}
			}
			return array;
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x0007A27C File Offset: 0x0007927C
		internal Assembly LoadAssembly(string assemblyName, bool throwOnFail)
		{
			try
			{
				return Assembly.Load(assemblyName);
			}
			catch
			{
				AssemblyName assemblyName2 = new AssemblyName(assemblyName);
				byte[] publicKeyToken = assemblyName2.GetPublicKeyToken();
				if ((publicKeyToken == null || publicKeyToken.Length == 0) && assemblyName2.Version == null)
				{
					this.EnsureReferenceSet();
					foreach (object obj in this.GetAssembliesCollection())
					{
						AssemblyInfo assemblyInfo = (AssemblyInfo)obj;
						Assembly[] assemblyInternal = assemblyInfo.AssemblyInternal;
						if (assemblyInternal != null)
						{
							for (int i = 0; i < assemblyInternal.Length; i++)
							{
								if (StringUtil.EqualsIgnoreCase(assemblyName2.Name, assemblyInternal[i].GetName().Name))
								{
									return assemblyInternal[i];
								}
							}
						}
					}
				}
				if (throwOnFail)
				{
					throw;
				}
			}
			return null;
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x0007A36C File Offset: 0x0007936C
		private Assembly LoadAssemblyHelper(string assemblyName, bool starDirective)
		{
			Assembly assembly = null;
			try
			{
				assembly = Assembly.Load(assemblyName);
			}
			catch (Exception ex)
			{
				bool flag = false;
				if (starDirective)
				{
					int hrforException = Marshal.GetHRForException(ex);
					if (hrforException == -2146234344)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					string text = ex.Message;
					if (string.IsNullOrEmpty(text))
					{
						if (ex is FileLoadException)
						{
							text = SR.GetString("Config_base_file_load_exception_no_message", new object[] { "assembly" });
						}
						else if (ex is BadImageFormatException)
						{
							text = SR.GetString("Config_base_bad_image_exception_no_message", new object[] { assemblyName });
						}
						else
						{
							text = SR.GetString("Config_base_report_exception_type", new object[] { ex.GetType().ToString() });
						}
					}
					string text2 = base.ElementInformation.Properties["assemblies"].Source;
					int num = base.ElementInformation.Properties["assemblies"].LineNumber;
					if (starDirective)
					{
						assemblyName = "*";
					}
					if (this.Assemblies[assemblyName] != null)
					{
						text2 = this.Assemblies[assemblyName].ElementInformation.Source;
						num = this.Assemblies[assemblyName].ElementInformation.LineNumber;
					}
					throw new ConfigurationErrorsException(text, ex, text2, num);
				}
			}
			return assembly;
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x0007A4D4 File Offset: 0x000794D4
		internal Assembly[] LoadAllAssembliesFromAppDomainBinDirectory()
		{
			string binDirectoryInternal = HttpRuntime.BinDirectoryInternal;
			Assembly assembly = null;
			Assembly[] array = null;
			if (FileUtil.DirectoryExists(binDirectoryInternal))
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(binDirectoryInternal);
				FileInfo[] files = directoryInfo.GetFiles("*.dll");
				if (files.Length > 0)
				{
					ArrayList arrayList = new ArrayList(files.Length);
					for (int i = 0; i < files.Length; i++)
					{
						string assemblyNameFromFileName = Util.GetAssemblyNameFromFileName(files[i].Name);
						if (!assemblyNameFromFileName.StartsWith("App_Web_", StringComparison.Ordinal))
						{
							if (!this.GetAssembliesCollection().IsRemoved(assemblyNameFromFileName))
							{
								assembly = this.LoadAssemblyHelper(assemblyNameFromFileName, true);
							}
							if (assembly != null)
							{
								arrayList.Add(assembly);
							}
						}
					}
					array = (Assembly[])arrayList.ToArray(typeof(Assembly));
				}
			}
			if (array == null)
			{
				array = new Assembly[0];
			}
			return array;
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x060019E5 RID: 6629 RVA: 0x0007A594 File Offset: 0x00079594
		internal long RecompilationHash
		{
			get
			{
				if (this._recompilationHash == -1L)
				{
					lock (this)
					{
						if (this._recompilationHash == -1L)
						{
							this._recompilationHash = CompilationUtil.GetRecompilationHash(this);
						}
					}
				}
				return this._recompilationHash;
			}
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x0007A5E8 File Offset: 0x000795E8
		protected override void PostDeserialize()
		{
			WebContext webContext = base.EvaluationContext.HostingContext as WebContext;
			if (webContext != null && webContext.ApplicationLevel == WebApplicationLevel.BelowApplication)
			{
				if (this.CodeSubDirectories.ElementInformation.IsPresent)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_element_below_app_illegal", new object[] { CompilationSection._propCodeSubDirs.Name }), this.CodeSubDirectories.ElementInformation.Source, this.CodeSubDirectories.ElementInformation.LineNumber);
				}
				if (this.BuildProviders.ElementInformation.IsPresent)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_element_below_app_illegal", new object[] { CompilationSection._propBuildProviders.Name }), this.BuildProviders.ElementInformation.Source, this.BuildProviders.ElementInformation.LineNumber);
				}
			}
		}

		// Token: 0x04001789 RID: 6025
		private const string tempDirectoryAttributeName = "tempDirectory";

		// Token: 0x0400178A RID: 6026
		private const string assemblyPostProcessorTypeAttributeName = "assemblyPostProcessorType";

		// Token: 0x0400178B RID: 6027
		private const char fieldSeparator = ';';

		// Token: 0x0400178C RID: 6028
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x0400178D RID: 6029
		private static readonly ConfigurationProperty _propTempDirectory = new ConfigurationProperty("tempDirectory", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x0400178E RID: 6030
		private static readonly ConfigurationProperty _propDebug = new ConfigurationProperty("debug", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x0400178F RID: 6031
		private static readonly ConfigurationProperty _propStrict = new ConfigurationProperty("strict", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001790 RID: 6032
		private static readonly ConfigurationProperty _propExplicit = new ConfigurationProperty("explicit", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001791 RID: 6033
		private static readonly ConfigurationProperty _propBatch = new ConfigurationProperty("batch", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001792 RID: 6034
		private static readonly ConfigurationProperty _propOptimizeCompilations = new ConfigurationProperty("optimizeCompilations", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001793 RID: 6035
		private static readonly ConfigurationProperty _propBatchTimeout = new ConfigurationProperty("batchTimeout", typeof(TimeSpan), TimeSpan.FromMinutes(15.0), StdValidatorsAndConverters.TimeSpanSecondsOrInfiniteConverter, StdValidatorsAndConverters.PositiveTimeSpanValidator, ConfigurationPropertyOptions.None);

		// Token: 0x04001794 RID: 6036
		private static readonly ConfigurationProperty _propMaxBatchSize = new ConfigurationProperty("maxBatchSize", typeof(int), 1000, ConfigurationPropertyOptions.None);

		// Token: 0x04001795 RID: 6037
		private static readonly ConfigurationProperty _propMaxBatchGeneratedFileSize = new ConfigurationProperty("maxBatchGeneratedFileSize", typeof(int), 1000, ConfigurationPropertyOptions.None);

		// Token: 0x04001796 RID: 6038
		private static readonly ConfigurationProperty _propNumRecompilesBeforeAppRestart = new ConfigurationProperty("numRecompilesBeforeAppRestart", typeof(int), 15, ConfigurationPropertyOptions.None);

		// Token: 0x04001797 RID: 6039
		private static readonly ConfigurationProperty _propDefaultLanguage = new ConfigurationProperty("defaultLanguage", typeof(string), "vb", ConfigurationPropertyOptions.None);

		// Token: 0x04001798 RID: 6040
		private static readonly ConfigurationProperty _propCompilers = new ConfigurationProperty("compilers", typeof(CompilerCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

		// Token: 0x04001799 RID: 6041
		private static readonly ConfigurationProperty _propAssemblies = new ConfigurationProperty("assemblies", typeof(AssemblyCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

		// Token: 0x0400179A RID: 6042
		private static readonly ConfigurationProperty _propBuildProviders = new ConfigurationProperty("buildProviders", typeof(BuildProviderCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

		// Token: 0x0400179B RID: 6043
		private static readonly ConfigurationProperty _propExpressionBuilders = new ConfigurationProperty("expressionBuilders", typeof(ExpressionBuilderCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

		// Token: 0x0400179C RID: 6044
		private static readonly ConfigurationProperty _propUrlLinePragmas = new ConfigurationProperty("urlLinePragmas", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x0400179D RID: 6045
		private static readonly ConfigurationProperty _propCodeSubDirs = new ConfigurationProperty("codeSubDirectories", typeof(CodeSubDirectoriesCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

		// Token: 0x0400179E RID: 6046
		private static readonly ConfigurationProperty _propAssemblyPreprocessorType = new ConfigurationProperty("assemblyPostProcessorType", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x0400179F RID: 6047
		private bool _referenceSet;

		// Token: 0x040017A0 RID: 6048
		private Hashtable _compilerLanguages;

		// Token: 0x040017A1 RID: 6049
		private Hashtable _compilerExtensions;

		// Token: 0x040017A2 RID: 6050
		private long _recompilationHash = -1L;

		// Token: 0x040017A3 RID: 6051
		private bool _isRuntimeObject;

		// Token: 0x040017A4 RID: 6052
		private Type _assemblyPostProcessorType;
	}
}
