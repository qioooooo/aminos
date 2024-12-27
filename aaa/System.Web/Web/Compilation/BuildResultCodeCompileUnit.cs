using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x02000152 RID: 338
	internal class BuildResultCodeCompileUnit : BuildResult
	{
		// Token: 0x06000F7C RID: 3964 RVA: 0x000453C8 File Offset: 0x000443C8
		internal BuildResultCodeCompileUnit()
		{
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x000453D0 File Offset: 0x000443D0
		internal BuildResultCodeCompileUnit(Type codeDomProviderType, CodeCompileUnit codeCompileUnit, CompilerParameters compilerParameters, IDictionary linePragmasTable)
		{
			this._codeDomProviderType = codeDomProviderType;
			this._codeCompileUnit = codeCompileUnit;
			this._compilerParameters = compilerParameters;
			this._linePragmasTable = linePragmasTable;
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000F7E RID: 3966 RVA: 0x000453F5 File Offset: 0x000443F5
		internal Type CodeDomProviderType
		{
			get
			{
				return this._codeDomProviderType;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000F7F RID: 3967 RVA: 0x000453FD File Offset: 0x000443FD
		internal CodeCompileUnit CodeCompileUnit
		{
			get
			{
				return this._codeCompileUnit;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06000F80 RID: 3968 RVA: 0x00045405 File Offset: 0x00044405
		internal CompilerParameters CompilerParameters
		{
			get
			{
				return this._compilerParameters;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06000F81 RID: 3969 RVA: 0x0004540D File Offset: 0x0004440D
		internal IDictionary LinePragmasTable
		{
			get
			{
				return this._linePragmasTable;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06000F82 RID: 3970 RVA: 0x00045415 File Offset: 0x00044415
		internal override bool CacheToDisk
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x00045418 File Offset: 0x00044418
		internal override BuildResultTypeCode GetCode()
		{
			return BuildResultTypeCode.BuildResultCodeCompileUnit;
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0004541B File Offset: 0x0004441B
		private string GetPreservationFileName()
		{
			return this._cacheKey + ".ccu";
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x00045430 File Offset: 0x00044430
		protected override void ComputeHashCode(HashCodeCombiner hashCodeCombiner)
		{
			base.ComputeHashCode(hashCodeCombiner);
			CompilationSection compilation = RuntimeConfig.GetConfig(base.VirtualPath).Compilation;
			hashCodeCombiner.AddObject(compilation.RecompilationHash);
			PagesSection pages = RuntimeConfig.GetConfig(base.VirtualPath).Pages;
			hashCodeCombiner.AddObject(Util.GetRecompilationHash(pages));
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x00045480 File Offset: 0x00044480
		internal override void GetPreservedAttributes(PreservationFileReader pfr)
		{
			base.GetPreservedAttributes(pfr);
			string text = pfr.GetAttribute("CCUpreservationFileName");
			text = Path.Combine(HttpRuntime.CodegenDirInternal, text);
			using (FileStream fileStream = File.Open(text, FileMode.Open))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				this._codeCompileUnit = binaryFormatter.Deserialize(fileStream) as CodeCompileUnit;
				this._codeDomProviderType = (Type)binaryFormatter.Deserialize(fileStream);
				this._compilerParameters = (CompilerParameters)binaryFormatter.Deserialize(fileStream);
				this._linePragmasTable = binaryFormatter.Deserialize(fileStream) as IDictionary;
			}
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x00045520 File Offset: 0x00044520
		internal void SetCacheKey(string cacheKey)
		{
			this._cacheKey = cacheKey;
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0004552C File Offset: 0x0004452C
		internal override void SetPreservedAttributes(PreservationFileWriter pfw)
		{
			base.SetPreservedAttributes(pfw);
			string text = this.GetPreservationFileName();
			pfw.SetAttribute("CCUpreservationFileName", text);
			text = Path.Combine(HttpRuntime.CodegenDirInternal, text);
			using (FileStream fileStream = File.Open(text, FileMode.Create))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				if (this._codeCompileUnit != null)
				{
					binaryFormatter.Serialize(fileStream, this._codeCompileUnit);
				}
				else
				{
					binaryFormatter.Serialize(fileStream, new object());
				}
				binaryFormatter.Serialize(fileStream, this._codeDomProviderType);
				binaryFormatter.Serialize(fileStream, this._compilerParameters);
				if (this._linePragmasTable != null)
				{
					binaryFormatter.Serialize(fileStream, this._linePragmasTable);
				}
				else
				{
					binaryFormatter.Serialize(fileStream, new object());
				}
			}
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x000455EC File Offset: 0x000445EC
		internal override void RemoveOutOfDateResources(PreservationFileReader pfr)
		{
			string text = pfr.GetAttribute("CCUpreservationFileName");
			text = Path.Combine(HttpRuntime.CodegenDirInternal, text);
			File.Delete(text);
		}

		// Token: 0x040015F2 RID: 5618
		private const string fileNameAttribute = "CCUpreservationFileName";

		// Token: 0x040015F3 RID: 5619
		private Type _codeDomProviderType;

		// Token: 0x040015F4 RID: 5620
		private CodeCompileUnit _codeCompileUnit;

		// Token: 0x040015F5 RID: 5621
		private CompilerParameters _compilerParameters;

		// Token: 0x040015F6 RID: 5622
		private IDictionary _linePragmasTable;

		// Token: 0x040015F7 RID: 5623
		private string _cacheKey;
	}
}
