using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Compilation;

namespace System.Web.Configuration
{
	// Token: 0x020001C7 RID: 455
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class Compiler : ConfigurationElement
	{
		// Token: 0x060019E7 RID: 6631 RVA: 0x0007A6C8 File Offset: 0x000796C8
		static Compiler()
		{
			Compiler._properties.Add(Compiler._propLanguage);
			Compiler._properties.Add(Compiler._propExtension);
			Compiler._properties.Add(Compiler._propType);
			Compiler._properties.Add(Compiler._propWarningLevel);
			Compiler._properties.Add(Compiler._propCompilerOptions);
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x0007A7CE File Offset: 0x000797CE
		internal Compiler()
		{
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x0007A7D8 File Offset: 0x000797D8
		public Compiler(string compilerOptions, string extension, string language, string type, int warningLevel)
			: this()
		{
			base[Compiler._propCompilerOptions] = compilerOptions;
			base[Compiler._propExtension] = extension;
			base[Compiler._propLanguage] = language;
			base[Compiler._propType] = type;
			base[Compiler._propWarningLevel] = warningLevel;
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x060019EA RID: 6634 RVA: 0x0007A82E File Offset: 0x0007982E
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return Compiler._properties;
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x060019EB RID: 6635 RVA: 0x0007A835 File Offset: 0x00079835
		[ConfigurationProperty("language", DefaultValue = "", IsRequired = true, IsKey = true)]
		public string Language
		{
			get
			{
				return (string)base[Compiler._propLanguage];
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x060019EC RID: 6636 RVA: 0x0007A847 File Offset: 0x00079847
		[ConfigurationProperty("extension", DefaultValue = "")]
		public string Extension
		{
			get
			{
				return (string)base[Compiler._propExtension];
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x060019ED RID: 6637 RVA: 0x0007A859 File Offset: 0x00079859
		[ConfigurationProperty("type", IsRequired = true, DefaultValue = "")]
		public string Type
		{
			get
			{
				return (string)base[Compiler._propType];
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x060019EE RID: 6638 RVA: 0x0007A86C File Offset: 0x0007986C
		internal CompilerType CompilerTypeInternal
		{
			get
			{
				if (this._compilerType == null)
				{
					lock (this)
					{
						if (this._compilerType == null)
						{
							Type type = CompilationUtil.LoadTypeWithChecks(this.Type, typeof(CodeDomProvider), null, this, "type");
							CompilerParameters compilerParameters = new CompilerParameters();
							compilerParameters.WarningLevel = this.WarningLevel;
							compilerParameters.TreatWarningsAsErrors = this.WarningLevel > 0;
							string compilerOptions = this.CompilerOptions;
							CompilationUtil.CheckCompilerOptionsAllowed(compilerOptions, true, base.ElementInformation.Properties["compilerOptions"].Source, base.ElementInformation.Properties["compilerOptions"].LineNumber);
							compilerParameters.CompilerOptions = compilerOptions;
							this._compilerType = new CompilerType(type, compilerParameters);
						}
					}
				}
				return this._compilerType;
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x060019EF RID: 6639 RVA: 0x0007A94C File Offset: 0x0007994C
		[ConfigurationProperty("warningLevel", DefaultValue = 0)]
		[IntegerValidator(MinValue = 0, MaxValue = 4)]
		public int WarningLevel
		{
			get
			{
				return (int)base[Compiler._propWarningLevel];
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x060019F0 RID: 6640 RVA: 0x0007A95E File Offset: 0x0007995E
		[ConfigurationProperty("compilerOptions", DefaultValue = "")]
		public string CompilerOptions
		{
			get
			{
				return (string)base[Compiler._propCompilerOptions];
			}
		}

		// Token: 0x040017A5 RID: 6053
		private const string compilerOptionsAttribName = "compilerOptions";

		// Token: 0x040017A6 RID: 6054
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040017A7 RID: 6055
		private static readonly ConfigurationProperty _propLanguage = new ConfigurationProperty("language", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x040017A8 RID: 6056
		private static readonly ConfigurationProperty _propExtension = new ConfigurationProperty("extension", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x040017A9 RID: 6057
		private static readonly ConfigurationProperty _propType = new ConfigurationProperty("type", typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x040017AA RID: 6058
		private static readonly ConfigurationProperty _propWarningLevel = new ConfigurationProperty("warningLevel", typeof(int), 0, null, new IntegerValidator(0, 4), ConfigurationPropertyOptions.None);

		// Token: 0x040017AB RID: 6059
		private static readonly ConfigurationProperty _propCompilerOptions = new ConfigurationProperty("compilerOptions", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x040017AC RID: 6060
		private CompilerType _compilerType;
	}
}
