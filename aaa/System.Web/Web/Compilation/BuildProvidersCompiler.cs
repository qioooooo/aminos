using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Web.Configuration;

namespace System.Web.Compilation
{
	// Token: 0x0200013F RID: 319
	internal class BuildProvidersCompiler
	{
		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x00043733 File Offset: 0x00042733
		internal ICollection ReferencedAssemblies
		{
			get
			{
				return this._referencedAssemblies;
			}
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x0004373B File Offset: 0x0004273B
		internal BuildProvidersCompiler(VirtualPath configPath, string outputAssemblyName)
			: this(configPath, false, outputAssemblyName)
		{
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x00043748 File Offset: 0x00042748
		internal BuildProvidersCompiler(VirtualPath configPath, bool supportLocalization, string outputAssemblyName)
		{
			this._configPath = configPath;
			this._supportLocalization = supportLocalization;
			this._compConfig = RuntimeConfig.GetConfig(this._configPath).Compilation;
			this._referencedAssemblies = BuildManager.GetReferencedAssemblies(this.CompConfig);
			this._outputAssemblyName = outputAssemblyName;
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00043798 File Offset: 0x00042798
		internal BuildProvidersCompiler(VirtualPath configPath, bool supportLocalization, string generatedFilesDir, int index)
		{
			this._configPath = configPath;
			this._supportLocalization = supportLocalization;
			this._compConfig = RuntimeConfig.GetConfig(this._configPath).Compilation;
			this._referencedAssemblies = BuildManager.GetReferencedAssemblies(this.CompConfig, index);
			this._generatedFilesDir = generatedFilesDir;
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x000437E9 File Offset: 0x000427E9
		internal CompilationSection CompConfig
		{
			get
			{
				return this._compConfig;
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000EF8 RID: 3832 RVA: 0x000437F1 File Offset: 0x000427F1
		internal string OutputAssemblyName
		{
			get
			{
				return this._outputAssemblyName;
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x000437F9 File Offset: 0x000427F9
		private bool CbmGenerateOnlyMode
		{
			get
			{
				return this._generatedFilesDir != null;
			}
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x00043807 File Offset: 0x00042807
		internal void SetBuildProviders(ICollection buildProviders)
		{
			this._buildProviders = buildProviders;
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x00043810 File Offset: 0x00042810
		private void ProcessBuildProviders()
		{
			CompilerType compilerType = null;
			BuildProvider buildProvider = null;
			if (this.OutputAssemblyName != null)
			{
				StandardDiskBuildResultCache.RemoveSatelliteAssemblies(this.OutputAssemblyName);
			}
			ArrayList arrayList = null;
			foreach (object obj in this._buildProviders)
			{
				BuildProvider buildProvider2 = (BuildProvider)obj;
				buildProvider2.SetReferencedAssemblies(this._referencedAssemblies);
				if (!BuildManager.ThrowOnFirstParseError)
				{
					InternalBuildProvider internalBuildProvider = buildProvider2 as InternalBuildProvider;
					if (internalBuildProvider != null)
					{
						internalBuildProvider.ThrowOnFirstParseError = false;
					}
				}
				CompilerType compilerTypeFromBuildProvider = BuildProvider.GetCompilerTypeFromBuildProvider(buildProvider2);
				string text = null;
				if (this._supportLocalization)
				{
					text = buildProvider2.GetCultureName();
				}
				if (compilerTypeFromBuildProvider != null)
				{
					if (text != null)
					{
						throw new HttpException(SR.GetString("Both_culture_and_language", new object[] { BuildProvider.GetDisplayName(buildProvider2) }));
					}
					if (compilerType != null)
					{
						if (!compilerTypeFromBuildProvider.Equals(compilerType))
						{
							throw new HttpException(SR.GetString("Inconsistent_language", new object[]
							{
								BuildProvider.GetDisplayName(buildProvider2),
								BuildProvider.GetDisplayName(buildProvider)
							}));
						}
					}
					else
					{
						buildProvider = buildProvider2;
						compilerType = compilerTypeFromBuildProvider;
						this._assemblyBuilder = compilerType.CreateAssemblyBuilder(this.CompConfig, this._referencedAssemblies, this._generatedFilesDir, this.OutputAssemblyName);
					}
				}
				else if (text != null)
				{
					if (!this.CbmGenerateOnlyMode)
					{
						if (this._satelliteAssemblyBuilders == null)
						{
							this._satelliteAssemblyBuilders = new Hashtable(StringComparer.OrdinalIgnoreCase);
						}
						AssemblyBuilder assemblyBuilder = (AssemblyBuilder)this._satelliteAssemblyBuilders[text];
						if (assemblyBuilder == null)
						{
							assemblyBuilder = CompilerType.GetDefaultAssemblyBuilder(this.CompConfig, this._referencedAssemblies, this._configPath, this.OutputAssemblyName);
							assemblyBuilder.CultureName = text;
							this._satelliteAssemblyBuilders[text] = assemblyBuilder;
						}
						assemblyBuilder.AddBuildProvider(buildProvider2);
						continue;
					}
					continue;
				}
				else if (this._assemblyBuilder == null)
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList();
					}
					arrayList.Add(buildProvider2);
					continue;
				}
				this._assemblyBuilder.AddBuildProvider(buildProvider2);
			}
			if (this._assemblyBuilder == null && arrayList != null)
			{
				this._assemblyBuilder = CompilerType.GetDefaultAssemblyBuilder(this.CompConfig, this._referencedAssemblies, this._configPath, this._generatedFilesDir, this.OutputAssemblyName);
			}
			if (this._assemblyBuilder != null && arrayList != null)
			{
				foreach (object obj2 in arrayList)
				{
					BuildProvider buildProvider3 = (BuildProvider)obj2;
					this._assemblyBuilder.AddBuildProvider(buildProvider3);
				}
			}
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x00043AB4 File Offset: 0x00042AB4
		internal CompilerResults PerformBuild()
		{
			this.ProcessBuildProviders();
			if (this._satelliteAssemblyBuilders != null)
			{
				foreach (object obj in this._satelliteAssemblyBuilders.Values)
				{
					AssemblyBuilder assemblyBuilder = (AssemblyBuilder)obj;
					assemblyBuilder.Compile();
				}
			}
			if (this._assemblyBuilder != null)
			{
				return this._assemblyBuilder.Compile();
			}
			return null;
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x00043B38 File Offset: 0x00042B38
		internal void GenerateSources(out Type codeDomProviderType, out CompilerParameters compilerParameters)
		{
			this.ProcessBuildProviders();
			if (this._assemblyBuilder == null)
			{
				this._assemblyBuilder = CompilerType.GetDefaultAssemblyBuilder(this.CompConfig, this._referencedAssemblies, this._configPath, this._generatedFilesDir, null);
			}
			codeDomProviderType = this._assemblyBuilder.CodeDomProviderType;
			compilerParameters = this._assemblyBuilder.GetCompilerParameters();
		}

		// Token: 0x040015B5 RID: 5557
		private ICollection _buildProviders;

		// Token: 0x040015B6 RID: 5558
		private VirtualPath _configPath;

		// Token: 0x040015B7 RID: 5559
		private bool _supportLocalization;

		// Token: 0x040015B8 RID: 5560
		private ICollection _referencedAssemblies;

		// Token: 0x040015B9 RID: 5561
		private AssemblyBuilder _assemblyBuilder;

		// Token: 0x040015BA RID: 5562
		private IDictionary _satelliteAssemblyBuilders;

		// Token: 0x040015BB RID: 5563
		private string _generatedFilesDir;

		// Token: 0x040015BC RID: 5564
		private CompilationSection _compConfig;

		// Token: 0x040015BD RID: 5565
		private string _outputAssemblyName;
	}
}
