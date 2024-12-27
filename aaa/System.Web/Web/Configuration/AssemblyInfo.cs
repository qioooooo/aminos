using System;
using System.Configuration;
using System.Reflection;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x0200019E RID: 414
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class AssemblyInfo : ConfigurationElement
	{
		// Token: 0x0600115F RID: 4447 RVA: 0x0004D9A4 File Offset: 0x0004C9A4
		internal void SetCompilationReference(CompilationSection compSection)
		{
			this._compilationSection = compSection;
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x0004D9AD File Offset: 0x0004C9AD
		static AssemblyInfo()
		{
			AssemblyInfo._properties.Add(AssemblyInfo._propAssembly);
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x0004D9E9 File Offset: 0x0004C9E9
		internal AssemblyInfo()
		{
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x0004D9F1 File Offset: 0x0004C9F1
		public AssemblyInfo(string assemblyName)
		{
			this.Assembly = assemblyName;
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001163 RID: 4451 RVA: 0x0004DA00 File Offset: 0x0004CA00
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return AssemblyInfo._properties;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001164 RID: 4452 RVA: 0x0004DA07 File Offset: 0x0004CA07
		// (set) Token: 0x06001165 RID: 4453 RVA: 0x0004DA19 File Offset: 0x0004CA19
		[StringValidator(MinLength = 1)]
		[ConfigurationProperty("assembly", IsRequired = true, IsKey = true, DefaultValue = "")]
		public string Assembly
		{
			get
			{
				return (string)base[AssemblyInfo._propAssembly];
			}
			set
			{
				base[AssemblyInfo._propAssembly] = value;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001166 RID: 4454 RVA: 0x0004DA27 File Offset: 0x0004CA27
		// (set) Token: 0x06001167 RID: 4455 RVA: 0x0004DA49 File Offset: 0x0004CA49
		internal Assembly[] AssemblyInternal
		{
			get
			{
				if (this._assembly == null)
				{
					this._assembly = this._compilationSection.LoadAssembly(this);
				}
				return this._assembly;
			}
			set
			{
				this._assembly = value;
			}
		}

		// Token: 0x040016A4 RID: 5796
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040016A5 RID: 5797
		private static readonly ConfigurationProperty _propAssembly = new ConfigurationProperty("assembly", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x040016A6 RID: 5798
		private Assembly[] _assembly;

		// Token: 0x040016A7 RID: 5799
		private CompilationSection _compilationSection;
	}
}
