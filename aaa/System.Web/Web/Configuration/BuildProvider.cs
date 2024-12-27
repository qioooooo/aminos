using System;
using System.Configuration;
using System.Globalization;
using System.Security.Permissions;
using System.Web.Compilation;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x020001AF RID: 431
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class BuildProvider : ConfigurationElement
	{
		// Token: 0x060018FC RID: 6396 RVA: 0x00077DA8 File Offset: 0x00076DA8
		static BuildProvider()
		{
			BuildProvider._properties.Add(BuildProvider._propExtension);
			BuildProvider._properties.Add(BuildProvider._propType);
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x00077E1F File Offset: 0x00076E1F
		public BuildProvider(string extension, string type)
			: this()
		{
			this.Extension = extension;
			this.Type = type;
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x00077E35 File Offset: 0x00076E35
		internal BuildProvider()
		{
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x060018FF RID: 6399 RVA: 0x00077E3D File Offset: 0x00076E3D
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return BuildProvider._properties;
			}
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x00077E44 File Offset: 0x00076E44
		public override bool Equals(object provider)
		{
			BuildProvider buildProvider = provider as BuildProvider;
			return buildProvider != null && StringUtil.EqualsIgnoreCase(this.Extension, buildProvider.Extension) && this.Type == buildProvider.Type;
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x00077E81 File Offset: 0x00076E81
		public override int GetHashCode()
		{
			return HashCodeCombiner.CombineHashCodes(this.Extension.ToLower(CultureInfo.InvariantCulture).GetHashCode(), this.Type.GetHashCode());
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001902 RID: 6402 RVA: 0x00077EA8 File Offset: 0x00076EA8
		// (set) Token: 0x06001903 RID: 6403 RVA: 0x00077EBA File Offset: 0x00076EBA
		[ConfigurationProperty("extension", IsRequired = true, IsKey = true, DefaultValue = "")]
		[StringValidator(MinLength = 1)]
		public string Extension
		{
			get
			{
				return (string)base[BuildProvider._propExtension];
			}
			set
			{
				base[BuildProvider._propExtension] = value;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001904 RID: 6404 RVA: 0x00077EC8 File Offset: 0x00076EC8
		// (set) Token: 0x06001905 RID: 6405 RVA: 0x00077EDA File Offset: 0x00076EDA
		[StringValidator(MinLength = 1)]
		[ConfigurationProperty("type", IsRequired = true, DefaultValue = "")]
		public string Type
		{
			get
			{
				return (string)base[BuildProvider._propType];
			}
			set
			{
				base[BuildProvider._propType] = value;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001906 RID: 6406 RVA: 0x00077EE8 File Offset: 0x00076EE8
		internal Type TypeInternal
		{
			get
			{
				if (this._type == null)
				{
					lock (this)
					{
						if (this._type == null)
						{
							this._type = CompilationUtil.LoadTypeWithChecks(this.Type, typeof(BuildProvider), null, this, "type");
						}
					}
				}
				return this._type;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001907 RID: 6407 RVA: 0x00077F50 File Offset: 0x00076F50
		internal BuildProviderAppliesTo AppliesToInternal
		{
			get
			{
				if (this._appliesToInternal != (BuildProviderAppliesTo)0)
				{
					return this._appliesToInternal;
				}
				object[] customAttributes = this.TypeInternal.GetCustomAttributes(typeof(BuildProviderAppliesToAttribute), true);
				if (customAttributes != null && customAttributes.Length > 0)
				{
					this._appliesToInternal = ((BuildProviderAppliesToAttribute)customAttributes[0]).AppliesTo;
				}
				else
				{
					this._appliesToInternal = BuildProviderAppliesTo.All;
				}
				return this._appliesToInternal;
			}
		}

		// Token: 0x040016FB RID: 5883
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040016FC RID: 5884
		private static readonly ConfigurationProperty _propExtension = new ConfigurationProperty("extension", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x040016FD RID: 5885
		private static readonly ConfigurationProperty _propType = new ConfigurationProperty("type", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x040016FE RID: 5886
		private Type _type;

		// Token: 0x040016FF RID: 5887
		private BuildProviderAppliesTo _appliesToInternal;
	}
}
