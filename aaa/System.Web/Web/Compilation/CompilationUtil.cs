using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Security.Permissions;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.Util;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

namespace System.Web.Compilation
{
	// Token: 0x02000167 RID: 359
	internal static class CompilationUtil
	{
		// Token: 0x0600101C RID: 4124 RVA: 0x00047BFC File Offset: 0x00046BFC
		internal static bool IsDebuggingEnabled(HttpContext context)
		{
			CompilationSection compilation = RuntimeConfig.GetConfig(context).Compilation;
			return compilation.Debug;
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x00047C1C File Offset: 0x00046C1C
		internal static bool IsBatchingEnabled(string configPath)
		{
			CompilationSection compilation = RuntimeConfig.GetConfig(configPath).Compilation;
			return compilation.Batch;
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x00047C3C File Offset: 0x00046C3C
		internal static int GetRecompilationsBeforeAppRestarts()
		{
			CompilationSection compilation = RuntimeConfig.GetAppConfig().Compilation;
			return compilation.NumRecompilesBeforeAppRestart;
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x00047C5A File Offset: 0x00046C5A
		internal static CompilerType GetCodeDefaultLanguageCompilerInfo()
		{
			return new CompilerType(typeof(VBCodeProvider), null);
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x00047C6C File Offset: 0x00046C6C
		internal static CompilerType GetDefaultLanguageCompilerInfo(CompilationSection compConfig, VirtualPath configPath)
		{
			if (compConfig == null)
			{
				compConfig = RuntimeConfig.GetConfig(configPath).Compilation;
			}
			if (compConfig.DefaultLanguage == null)
			{
				return CompilationUtil.GetCodeDefaultLanguageCompilerInfo();
			}
			return compConfig.GetCompilerInfoFromLanguage(compConfig.DefaultLanguage);
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x00047C98 File Offset: 0x00046C98
		internal static CompilerType GetCompilerInfoFromVirtualPath(VirtualPath virtualPath)
		{
			string extension = virtualPath.Extension;
			if (extension.Length == 0)
			{
				throw new HttpException(SR.GetString("Empty_extension", new object[] { virtualPath }));
			}
			return CompilationUtil.GetCompilerInfoFromExtension(virtualPath, extension);
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x00047CD8 File Offset: 0x00046CD8
		private static CompilerType GetCompilerInfoFromExtension(VirtualPath configPath, string extension)
		{
			CompilationSection compilation = RuntimeConfig.GetConfig(configPath).Compilation;
			return compilation.GetCompilerInfoFromExtension(extension, true);
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x00047CFC File Offset: 0x00046CFC
		internal static CompilerType GetCompilerInfoFromLanguage(VirtualPath configPath, string language)
		{
			CompilationSection compilation = RuntimeConfig.GetConfig(configPath).Compilation;
			return compilation.GetCompilerInfoFromLanguage(language);
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x00047D1C File Offset: 0x00046D1C
		internal static CompilerType GetCSharpCompilerInfo(CompilationSection compConfig, VirtualPath configPath)
		{
			if (compConfig == null)
			{
				compConfig = RuntimeConfig.GetConfig(configPath).Compilation;
			}
			if (compConfig.DefaultLanguage == null)
			{
				return new CompilerType(typeof(CSharpCodeProvider), null);
			}
			return compConfig.GetCompilerInfoFromLanguage("c#");
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x00047D54 File Offset: 0x00046D54
		internal static CodeSubDirectoriesCollection GetCodeSubDirectories()
		{
			CompilationSection compilation = RuntimeConfig.GetAppConfig().Compilation;
			CodeSubDirectoriesCollection codeSubDirectories = compilation.CodeSubDirectories;
			if (codeSubDirectories != null)
			{
				codeSubDirectories.EnsureRuntimeValidation();
			}
			return codeSubDirectories;
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x00047D80 File Offset: 0x00046D80
		internal static long GetRecompilationHash(CompilationSection ps)
		{
			HashCodeCombiner hashCodeCombiner = new HashCodeCombiner();
			hashCodeCombiner.AddObject(ps.Debug);
			hashCodeCombiner.AddObject(ps.Strict);
			hashCodeCombiner.AddObject(ps.Explicit);
			hashCodeCombiner.AddObject(ps.Batch);
			hashCodeCombiner.AddObject(ps.OptimizeCompilations);
			hashCodeCombiner.AddObject(ps.BatchTimeout);
			hashCodeCombiner.AddObject(ps.MaxBatchGeneratedFileSize);
			hashCodeCombiner.AddObject(ps.MaxBatchSize);
			hashCodeCombiner.AddObject(ps.NumRecompilesBeforeAppRestart);
			hashCodeCombiner.AddObject(ps.DefaultLanguage);
			hashCodeCombiner.AddObject(ps.UrlLinePragmas);
			if (ps.AssemblyPostProcessorTypeInternal != null)
			{
				hashCodeCombiner.AddObject(ps.AssemblyPostProcessorTypeInternal.FullName);
			}
			foreach (object obj in ps.Compilers)
			{
				Compiler compiler = (Compiler)obj;
				hashCodeCombiner.AddObject(compiler.Language);
				hashCodeCombiner.AddObject(compiler.Extension);
				hashCodeCombiner.AddObject(compiler.Type);
				hashCodeCombiner.AddObject(compiler.WarningLevel);
				hashCodeCombiner.AddObject(compiler.CompilerOptions);
			}
			foreach (object obj2 in ps.ExpressionBuilders)
			{
				ExpressionBuilder expressionBuilder = (ExpressionBuilder)obj2;
				hashCodeCombiner.AddObject(expressionBuilder.ExpressionPrefix);
				hashCodeCombiner.AddObject(expressionBuilder.Type);
			}
			AssemblyCollection assemblies = ps.Assemblies;
			if (assemblies.Count == 0)
			{
				hashCodeCombiner.AddObject("__clearassemblies");
			}
			else
			{
				foreach (object obj3 in assemblies)
				{
					AssemblyInfo assemblyInfo = (AssemblyInfo)obj3;
					hashCodeCombiner.AddObject(assemblyInfo.Assembly);
				}
			}
			BuildProviderCollection buildProviders = ps.BuildProviders;
			if (buildProviders.Count == 0)
			{
				hashCodeCombiner.AddObject("__clearbuildproviders");
			}
			else
			{
				foreach (object obj4 in buildProviders)
				{
					BuildProvider buildProvider = (BuildProvider)obj4;
					hashCodeCombiner.AddObject(buildProvider.Type);
					hashCodeCombiner.AddObject(buildProvider.Extension);
				}
			}
			CodeSubDirectoriesCollection codeSubDirectories = ps.CodeSubDirectories;
			if (codeSubDirectories.Count == 0)
			{
				hashCodeCombiner.AddObject("__clearcodesubdirs");
			}
			else
			{
				foreach (object obj5 in codeSubDirectories)
				{
					CodeSubDirectory codeSubDirectory = (CodeSubDirectory)obj5;
					hashCodeCombiner.AddObject(codeSubDirectory.DirectoryName);
				}
			}
			CompilerInfo[] allCompilerInfo = CodeDomProvider.GetAllCompilerInfo();
			if (allCompilerInfo != null)
			{
				foreach (CompilerInfo compilerInfo in allCompilerInfo)
				{
					if (compilerInfo.IsCodeDomProviderTypeValid)
					{
						CompilerParameters compilerParameters = compilerInfo.CreateDefaultCompilerParameters();
						string compilerOptions = compilerParameters.CompilerOptions;
						if (!string.IsNullOrEmpty(compilerOptions))
						{
							Type codeDomProviderType = compilerInfo.CodeDomProviderType;
							if (codeDomProviderType != null)
							{
								hashCodeCombiner.AddObject(codeDomProviderType.FullName);
							}
							hashCodeCombiner.AddObject(compilerOptions);
						}
						if (compilerInfo.CodeDomProviderType != null)
						{
							IDictionary<string, string> providerOptions = CompilationUtil.GetProviderOptions(compilerInfo);
							if (providerOptions != null && providerOptions.Count > 0)
							{
								string fullName = compilerInfo.CodeDomProviderType.FullName;
								foreach (string text in providerOptions.Keys)
								{
									string text2 = providerOptions[text];
									hashCodeCombiner.AddObject(string.Concat(new string[] { fullName, ":", text, "=", text2 }));
								}
							}
						}
					}
				}
			}
			return hashCodeCombiner.CombinedHash;
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x000481B4 File Offset: 0x000471B4
		internal static AssemblyCollection GetAssembliesForAppLevel()
		{
			CompilationSection compilation = RuntimeConfig.GetAppConfig().Compilation;
			return compilation.Assemblies;
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x000481D4 File Offset: 0x000471D4
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.MemberAccess)]
		internal static Type GetTypeFromAssemblies(AssemblyCollection assembliesCollection, string typeName, bool ignoreCase)
		{
			if (assembliesCollection == null)
			{
				return null;
			}
			Type type = null;
			foreach (object obj in assembliesCollection)
			{
				AssemblyInfo assemblyInfo = (AssemblyInfo)obj;
				foreach (Assembly assembly in assemblyInfo.AssemblyInternal)
				{
					Type type2 = assembly.GetType(typeName, false, ignoreCase);
					if (type2 != null)
					{
						if (type != null && type2 != type)
						{
							throw new HttpException(SR.GetString("Ambiguous_type", new object[]
							{
								typeName,
								Util.GetAssemblySafePathFromType(type),
								Util.GetAssemblySafePathFromType(type2)
							}));
						}
						type = type2;
					}
				}
			}
			return type;
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0004829C File Offset: 0x0004729C
		internal static Type GetBuildProviderTypeFromExtension(VirtualPath configPath, string extension, BuildProviderAppliesTo neededFor, bool failIfUnknown)
		{
			CompilationSection compilation = RuntimeConfig.GetConfig(configPath).Compilation;
			return CompilationUtil.GetBuildProviderTypeFromExtension(compilation, extension, neededFor, failIfUnknown);
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x000482C0 File Offset: 0x000472C0
		internal static Type GetBuildProviderTypeFromExtension(CompilationSection config, string extension, BuildProviderAppliesTo neededFor, bool failIfUnknown)
		{
			BuildProvider buildProvider = config.BuildProviders[extension];
			Type type = null;
			if (buildProvider != null && buildProvider.TypeInternal != typeof(IgnoreFileBuildProvider) && buildProvider.TypeInternal != typeof(ForceCopyBuildProvider))
			{
				type = buildProvider.TypeInternal;
			}
			if (neededFor == BuildProviderAppliesTo.Web && BuildManager.PrecompilingForUpdatableDeployment && !typeof(BaseTemplateBuildProvider).IsAssignableFrom(type))
			{
				type = null;
			}
			if (type != null)
			{
				if ((neededFor & buildProvider.AppliesToInternal) != (BuildProviderAppliesTo)0)
				{
					return type;
				}
			}
			else if (neededFor != BuildProviderAppliesTo.Resources && config.GetCompilerInfoFromExtension(extension, false) != null)
			{
				return typeof(SourceFileBuildProvider);
			}
			if (failIfUnknown)
			{
				throw new HttpException(SR.GetString("Unknown_buildprovider_extension", new object[]
				{
					extension,
					neededFor.ToString()
				}));
			}
			return null;
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x00048380 File Offset: 0x00047380
		internal static void CheckCompilerOptionsAllowed(string compilerOptions, bool config, string file, int line)
		{
			if (string.IsNullOrEmpty(compilerOptions))
			{
				return;
			}
			if (HttpRuntime.HasUnmanagedPermission())
			{
				return;
			}
			string @string = SR.GetString("Insufficient_trust_for_attribute", new object[] { "compilerOptions" });
			if (config)
			{
				throw new ConfigurationErrorsException(@string, file, line);
			}
			throw new HttpException(@string);
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x000483CC File Offset: 0x000473CC
		internal static bool NeedToCopyFile(VirtualPath virtualPath, bool updatable, out bool createStub)
		{
			createStub = false;
			CompilationSection compilation = RuntimeConfig.GetConfig(virtualPath).Compilation;
			string extension = virtualPath.Extension;
			BuildProvider buildProvider = compilation.BuildProviders[extension];
			if (buildProvider == null)
			{
				return compilation.GetCompilerInfoFromExtension(extension, false) == null && !StringUtil.EqualsIgnoreCase(extension, ".asax") && (updatable || !StringUtil.EqualsIgnoreCase(extension, ".skin"));
			}
			if ((BuildProviderAppliesTo.Web & buildProvider.AppliesToInternal) == (BuildProviderAppliesTo)0)
			{
				return true;
			}
			if (buildProvider.TypeInternal == typeof(ForceCopyBuildProvider))
			{
				return true;
			}
			if (buildProvider.TypeInternal != typeof(IgnoreFileBuildProvider) && BuildManager.PrecompilingForUpdatableDeployment)
			{
				return true;
			}
			createStub = true;
			if (buildProvider.TypeInternal == typeof(UserControlBuildProvider) || buildProvider.TypeInternal == typeof(MasterPageBuildProvider) || buildProvider.TypeInternal == typeof(IgnoreFileBuildProvider))
			{
				createStub = false;
			}
			return false;
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x000484A8 File Offset: 0x000474A8
		internal static Type LoadTypeWithChecks(string typeName, Type requiredBaseType, Type requiredBaseType2, ConfigurationElement elem, string propertyName)
		{
			Type type = ConfigUtil.GetType(typeName, propertyName, elem);
			if (requiredBaseType2 == null)
			{
				ConfigUtil.CheckAssignableType(requiredBaseType, type, elem, propertyName);
			}
			else
			{
				ConfigUtil.CheckAssignableType(requiredBaseType, requiredBaseType2, type, elem, propertyName);
			}
			return type;
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x000484DC File Offset: 0x000474DC
		internal static CodeDomProvider CreateCodeDomProvider(Type codeDomProviderType)
		{
			CodeDomProvider codeDomProvider = CompilationUtil.CreateCodeDomProviderWithPropertyOptions(codeDomProviderType);
			if (codeDomProvider != null)
			{
				return codeDomProvider;
			}
			return (CodeDomProvider)Activator.CreateInstance(codeDomProviderType);
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x00048500 File Offset: 0x00047500
		internal static CodeDomProvider CreateCodeDomProviderNonPublic(Type codeDomProviderType)
		{
			CodeDomProvider codeDomProvider = CompilationUtil.CreateCodeDomProviderWithPropertyOptions(codeDomProviderType);
			if (codeDomProvider != null)
			{
				return codeDomProvider;
			}
			return (CodeDomProvider)HttpRuntime.CreateNonPublicInstance(codeDomProviderType);
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x00048524 File Offset: 0x00047524
		[ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
		private static CodeDomProvider CreateCodeDomProviderWithPropertyOptions(Type codeDomProviderType)
		{
			IDictionary<string, string> providerOptions = CompilationUtil.GetProviderOptions(codeDomProviderType);
			if (providerOptions != null && providerOptions.Count > 0)
			{
				ConstructorInfo constructor = codeDomProviderType.GetConstructor(new Type[] { typeof(IDictionary<string, string>) });
				if (constructor != null)
				{
					return (CodeDomProvider)constructor.Invoke(new object[] { providerOptions });
				}
			}
			return null;
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0004857C File Offset: 0x0004757C
		[ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
		internal static IDictionary<string, string> GetProviderOptions(Type codeDomProviderType)
		{
			CodeDomProvider codeDomProvider = (CodeDomProvider)Activator.CreateInstance(codeDomProviderType);
			if (CodeDomProvider.IsDefinedExtension(codeDomProvider.FileExtension))
			{
				CompilerInfo compilerInfo = CodeDomProvider.GetCompilerInfo(CodeDomProvider.GetLanguageFromExtension(codeDomProvider.FileExtension));
				return CompilationUtil.GetProviderOptions(compilerInfo);
			}
			return null;
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x000485BC File Offset: 0x000475BC
		[ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
		private static IDictionary<string, string> GetProviderOptions(CompilerInfo ci)
		{
			PropertyInfo property = ci.GetType().GetProperty("ProviderOptions", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property != null)
			{
				return (IDictionary<string, string>)property.GetValue(ci, null);
			}
			return null;
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x000485F0 File Offset: 0x000475F0
		internal static bool IsCompilerVersion35(Type codeDomProviderType)
		{
			IDictionary<string, string> providerOptions = CompilationUtil.GetProviderOptions(codeDomProviderType);
			string text;
			if (providerOptions == null || !providerOptions.TryGetValue("CompilerVersion", out text))
			{
				return false;
			}
			if (text == "v2.0")
			{
				return false;
			}
			if (text == "v3.5")
			{
				return true;
			}
			throw new ConfigurationException(SR.GetString("Invalid_attribute_value", new object[] { text, "system.codedom/compilers/compiler/ProviderOption/CompilerVersion" }));
		}

		// Token: 0x04001640 RID: 5696
		internal const string CodeDomProviderOptionPath = "system.codedom/compilers/compiler/ProviderOption/";
	}
}
