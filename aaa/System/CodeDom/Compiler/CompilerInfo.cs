using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Security.Permissions;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001F1 RID: 497
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class CompilerInfo
	{
		// Token: 0x060010C4 RID: 4292 RVA: 0x000372BF File Offset: 0x000362BF
		private CompilerInfo()
		{
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x000372C7 File Offset: 0x000362C7
		public string[] GetLanguages()
		{
			return this.CloneCompilerLanguages();
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x000372CF File Offset: 0x000362CF
		public string[] GetExtensions()
		{
			return this.CloneCompilerExtensions();
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060010C7 RID: 4295 RVA: 0x000372D8 File Offset: 0x000362D8
		public Type CodeDomProviderType
		{
			get
			{
				if (this.type == null)
				{
					lock (this)
					{
						if (this.type == null)
						{
							this.type = Type.GetType(this._codeDomProviderTypeName);
							if (this.type == null)
							{
								if (this.configFileName == null)
								{
									throw new ConfigurationErrorsException(SR.GetString("Unable_To_Locate_Type", new object[]
									{
										this._codeDomProviderTypeName,
										string.Empty,
										0
									}));
								}
								throw new ConfigurationErrorsException(SR.GetString("Unable_To_Locate_Type", new object[] { this._codeDomProviderTypeName }), this.configFileName, this.configFileLineNumber);
							}
						}
					}
				}
				return this.type;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060010C8 RID: 4296 RVA: 0x000373A4 File Offset: 0x000363A4
		public bool IsCodeDomProviderTypeValid
		{
			get
			{
				Type type = Type.GetType(this._codeDomProviderTypeName);
				return type != null;
			}
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x000373C4 File Offset: 0x000363C4
		public CodeDomProvider CreateProvider()
		{
			if (this._providerOptions.Count > 0)
			{
				ConstructorInfo constructor = this.CodeDomProviderType.GetConstructor(new Type[] { typeof(IDictionary<string, string>) });
				if (constructor != null)
				{
					return (CodeDomProvider)constructor.Invoke(new object[] { this._providerOptions });
				}
			}
			return (CodeDomProvider)Activator.CreateInstance(this.CodeDomProviderType);
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x00037430 File Offset: 0x00036430
		public CompilerParameters CreateDefaultCompilerParameters()
		{
			return this.CloneCompilerParameters();
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x00037438 File Offset: 0x00036438
		internal CompilerInfo(CompilerParameters compilerParams, string codeDomProviderTypeName, string[] compilerLanguages, string[] compilerExtensions)
		{
			this._compilerLanguages = compilerLanguages;
			this._compilerExtensions = compilerExtensions;
			this._codeDomProviderTypeName = codeDomProviderTypeName;
			if (compilerParams == null)
			{
				compilerParams = new CompilerParameters();
			}
			this._compilerParams = compilerParams;
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x00037467 File Offset: 0x00036467
		internal CompilerInfo(CompilerParameters compilerParams, string codeDomProviderTypeName)
		{
			this._codeDomProviderTypeName = codeDomProviderTypeName;
			if (compilerParams == null)
			{
				compilerParams = new CompilerParameters();
			}
			this._compilerParams = compilerParams;
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x00037487 File Offset: 0x00036487
		public override int GetHashCode()
		{
			return this._codeDomProviderTypeName.GetHashCode();
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x00037494 File Offset: 0x00036494
		public override bool Equals(object o)
		{
			CompilerInfo compilerInfo = o as CompilerInfo;
			return o != null && (this.CodeDomProviderType == compilerInfo.CodeDomProviderType && this.CompilerParams.WarningLevel == compilerInfo.CompilerParams.WarningLevel && this.CompilerParams.IncludeDebugInformation == compilerInfo.CompilerParams.IncludeDebugInformation) && this.CompilerParams.CompilerOptions == compilerInfo.CompilerParams.CompilerOptions;
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x00037508 File Offset: 0x00036508
		private CompilerParameters CloneCompilerParameters()
		{
			return new CompilerParameters
			{
				IncludeDebugInformation = this._compilerParams.IncludeDebugInformation,
				TreatWarningsAsErrors = this._compilerParams.TreatWarningsAsErrors,
				WarningLevel = this._compilerParams.WarningLevel,
				CompilerOptions = this._compilerParams.CompilerOptions
			};
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x00037560 File Offset: 0x00036560
		private string[] CloneCompilerLanguages()
		{
			string[] array = new string[this._compilerLanguages.Length];
			Array.Copy(this._compilerLanguages, array, this._compilerLanguages.Length);
			return array;
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x00037590 File Offset: 0x00036590
		private string[] CloneCompilerExtensions()
		{
			string[] array = new string[this._compilerExtensions.Length];
			Array.Copy(this._compilerExtensions, array, this._compilerExtensions.Length);
			return array;
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x060010D2 RID: 4306 RVA: 0x000375C0 File Offset: 0x000365C0
		internal CompilerParameters CompilerParams
		{
			get
			{
				return this._compilerParams;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x060010D3 RID: 4307 RVA: 0x000375C8 File Offset: 0x000365C8
		internal IDictionary<string, string> ProviderOptions
		{
			get
			{
				return this._providerOptions;
			}
		}

		// Token: 0x04000F63 RID: 3939
		internal string _codeDomProviderTypeName;

		// Token: 0x04000F64 RID: 3940
		internal CompilerParameters _compilerParams;

		// Token: 0x04000F65 RID: 3941
		internal string[] _compilerLanguages;

		// Token: 0x04000F66 RID: 3942
		internal string[] _compilerExtensions;

		// Token: 0x04000F67 RID: 3943
		internal string configFileName;

		// Token: 0x04000F68 RID: 3944
		internal IDictionary<string, string> _providerOptions;

		// Token: 0x04000F69 RID: 3945
		internal int configFileLineNumber;

		// Token: 0x04000F6A RID: 3946
		internal bool _mapped;

		// Token: 0x04000F6B RID: 3947
		private Type type;
	}
}
