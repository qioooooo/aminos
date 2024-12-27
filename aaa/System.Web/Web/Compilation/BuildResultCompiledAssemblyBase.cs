using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x02000144 RID: 324
	internal abstract class BuildResultCompiledAssemblyBase : BuildResult
	{
		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000F2D RID: 3885 RVA: 0x0004498F File Offset: 0x0004398F
		// (set) Token: 0x06000F2E RID: 3886 RVA: 0x000449A1 File Offset: 0x000439A1
		internal bool UsesExistingAssembly
		{
			get
			{
				return this._flags[131072];
			}
			set
			{
				this._flags[131072] = value;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000F2F RID: 3887 RVA: 0x000449B4 File Offset: 0x000439B4
		internal override bool IsUnloadable
		{
			get
			{
				return this.ResultAssembly == null;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000F30 RID: 3888
		// (set) Token: 0x06000F31 RID: 3889
		internal abstract Assembly ResultAssembly { get; set; }

		// Token: 0x06000F32 RID: 3890 RVA: 0x000449C0 File Offset: 0x000439C0
		internal static Assembly GetPreservedAssembly(PreservationFileReader pfr)
		{
			string attribute = pfr.GetAttribute("assembly");
			if (attribute == null)
			{
				return null;
			}
			Assembly assembly2;
			try
			{
				Assembly assembly = Assembly.Load(attribute);
				if (BuildResultCompiledAssemblyBase.AssemblyIsInvalid(assembly))
				{
					throw new InvalidOperationException();
				}
				BuildResultCompiledAssemblyBase.CheckAssemblyIsValid(assembly, new Hashtable());
				assembly2 = assembly;
			}
			catch
			{
				pfr.DiskCache.RemoveAssemblyAndRelatedFiles(attribute);
				throw;
			}
			return assembly2;
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00044A24 File Offset: 0x00043A24
		private static void CheckAssemblyIsValid(Assembly a, Hashtable checkedAssemblies)
		{
			checkedAssemblies.Add(a, null);
			foreach (AssemblyName assemblyName in a.GetReferencedAssemblies())
			{
				Assembly assembly = Assembly.Load(assemblyName);
				if (!assembly.GlobalAssemblyCache && BuildResultCompiledAssemblyBase.AssemblyIsInCodegenDir(assembly) && !checkedAssemblies.Contains(assembly))
				{
					if (BuildResultCompiledAssemblyBase.AssemblyIsInvalid(assembly))
					{
						throw new InvalidOperationException();
					}
					BuildResultCompiledAssemblyBase.CheckAssemblyIsValid(assembly, checkedAssemblies);
				}
			}
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00044A88 File Offset: 0x00043A88
		private static bool AssemblyIsInCodegenDir(Assembly a)
		{
			string assemblyCodeBase = Util.GetAssemblyCodeBase(a);
			FileInfo fileInfo = new FileInfo(assemblyCodeBase);
			string text = FileUtil.RemoveTrailingDirectoryBackSlash(fileInfo.Directory.FullName);
			if (BuildResultCompiledAssemblyBase.s_codegenDir == null)
			{
				BuildResultCompiledAssemblyBase.s_codegenDir = FileUtil.RemoveTrailingDirectoryBackSlash(HttpRuntime.CodegenDir);
			}
			return string.Equals(text, BuildResultCompiledAssemblyBase.s_codegenDir, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00044ADC File Offset: 0x00043ADC
		private static bool AssemblyIsInvalid(Assembly a)
		{
			string assemblyCodeBase = Util.GetAssemblyCodeBase(a);
			return !FileUtil.FileExists(assemblyCodeBase) || DiskBuildResultCache.HasDotDeleteFile(assemblyCodeBase);
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00044B00 File Offset: 0x00043B00
		internal override void SetPreservedAttributes(PreservationFileWriter pfw)
		{
			base.SetPreservedAttributes(pfw);
			if (this.ResultAssembly != null)
			{
				string text;
				if (this.ResultAssembly.GlobalAssemblyCache)
				{
					text = this.ResultAssembly.FullName;
				}
				else
				{
					text = this.ResultAssembly.GetName().Name;
				}
				pfw.SetAttribute("assembly", text);
			}
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00044B54 File Offset: 0x00043B54
		internal override void RemoveOutOfDateResources(PreservationFileReader pfr)
		{
			base.ReadPreservedFlags(pfr);
			if (this.UsesExistingAssembly)
			{
				return;
			}
			string attribute = pfr.GetAttribute("assembly");
			if (attribute != null)
			{
				pfr.DiskCache.RemoveAssemblyAndRelatedFiles(attribute);
			}
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x00044B8C File Offset: 0x00043B8C
		protected override void ComputeHashCode(HashCodeCombiner hashCodeCombiner)
		{
			base.ComputeHashCode(hashCodeCombiner);
			CompilationSection compilation = RuntimeConfig.GetConfig(base.VirtualPath).Compilation;
			hashCodeCombiner.AddObject(compilation.RecompilationHash);
		}

		// Token: 0x040015DE RID: 5598
		private static string s_codegenDir;
	}
}
