using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Security.Permissions;
using System.Web.Configuration;

namespace System.Web.Compilation
{
	// Token: 0x02000168 RID: 360
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class CompilerType
	{
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x00048659 File Offset: 0x00047659
		public Type CodeDomProviderType
		{
			get
			{
				return this._codeDomProviderType;
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x00048661 File Offset: 0x00047661
		public CompilerParameters CompilerParameters
		{
			get
			{
				return this._compilParams;
			}
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x00048669 File Offset: 0x00047669
		internal CompilerType(Type codeDomProviderType, CompilerParameters compilParams)
		{
			this._codeDomProviderType = codeDomProviderType;
			if (compilParams == null)
			{
				this._compilParams = new CompilerParameters();
				return;
			}
			this._compilParams = compilParams;
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0004868E File Offset: 0x0004768E
		internal CompilerType Clone()
		{
			return new CompilerType(this._codeDomProviderType, this.CloneCompilerParameters());
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x000486A4 File Offset: 0x000476A4
		private CompilerParameters CloneCompilerParameters()
		{
			return new CompilerParameters
			{
				IncludeDebugInformation = this._compilParams.IncludeDebugInformation,
				TreatWarningsAsErrors = this._compilParams.TreatWarningsAsErrors,
				WarningLevel = this._compilParams.WarningLevel,
				CompilerOptions = this._compilParams.CompilerOptions
			};
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x000486FC File Offset: 0x000476FC
		public override int GetHashCode()
		{
			return this._codeDomProviderType.GetHashCode();
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0004870C File Offset: 0x0004770C
		public override bool Equals(object o)
		{
			CompilerType compilerType = o as CompilerType;
			return o != null && (this._codeDomProviderType == compilerType._codeDomProviderType && this._compilParams.WarningLevel == compilerType._compilParams.WarningLevel && this._compilParams.IncludeDebugInformation == compilerType._compilParams.IncludeDebugInformation) && this._compilParams.CompilerOptions == compilerType._compilParams.CompilerOptions;
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x00048780 File Offset: 0x00047780
		internal AssemblyBuilder CreateAssemblyBuilder(CompilationSection compConfig, ICollection referencedAssemblies)
		{
			return this.CreateAssemblyBuilder(compConfig, referencedAssemblies, null, null);
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0004878C File Offset: 0x0004778C
		internal AssemblyBuilder CreateAssemblyBuilder(CompilationSection compConfig, ICollection referencedAssemblies, string generatedFilesDir, string outputAssemblyName)
		{
			if (generatedFilesDir != null)
			{
				return new CbmCodeGeneratorBuildProviderHost(compConfig, referencedAssemblies, this, generatedFilesDir, outputAssemblyName);
			}
			return new AssemblyBuilder(compConfig, referencedAssemblies, this, outputAssemblyName);
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x000487A7 File Offset: 0x000477A7
		private static CompilerType GetDefaultCompilerTypeWithParams(CompilationSection compConfig, VirtualPath configPath)
		{
			return CompilationUtil.GetCSharpCompilerInfo(compConfig, configPath);
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x000487B0 File Offset: 0x000477B0
		internal static AssemblyBuilder GetDefaultAssemblyBuilder(CompilationSection compConfig, ICollection referencedAssemblies, VirtualPath configPath, string outputAssemblyName)
		{
			return CompilerType.GetDefaultAssemblyBuilder(compConfig, referencedAssemblies, configPath, null, outputAssemblyName);
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x000487BC File Offset: 0x000477BC
		internal static AssemblyBuilder GetDefaultAssemblyBuilder(CompilationSection compConfig, ICollection referencedAssemblies, VirtualPath configPath, string generatedFilesDir, string outputAssemblyName)
		{
			CompilerType defaultCompilerTypeWithParams = CompilerType.GetDefaultCompilerTypeWithParams(compConfig, configPath);
			return defaultCompilerTypeWithParams.CreateAssemblyBuilder(compConfig, referencedAssemblies, generatedFilesDir, outputAssemblyName);
		}

		// Token: 0x04001641 RID: 5697
		private Type _codeDomProviderType;

		// Token: 0x04001642 RID: 5698
		private CompilerParameters _compilParams;
	}
}
