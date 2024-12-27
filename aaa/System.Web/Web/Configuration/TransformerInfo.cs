using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x02000258 RID: 600
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class TransformerInfo : ConfigurationElement
	{
		// Token: 0x06001FB6 RID: 8118 RVA: 0x0008BBDC File Offset: 0x0008ABDC
		static TransformerInfo()
		{
			TransformerInfo._properties.Add(TransformerInfo._propName);
			TransformerInfo._properties.Add(TransformerInfo._propType);
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x0008BC53 File Offset: 0x0008AC53
		internal TransformerInfo()
		{
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x0008BC5B File Offset: 0x0008AC5B
		public TransformerInfo(string name, string type)
			: this()
		{
			this.Name = name;
			this.Type = type;
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06001FB9 RID: 8121 RVA: 0x0008BC71 File Offset: 0x0008AC71
		// (set) Token: 0x06001FBA RID: 8122 RVA: 0x0008BC83 File Offset: 0x0008AC83
		[ConfigurationProperty("name", IsRequired = true, DefaultValue = "", IsKey = true)]
		[StringValidator(MinLength = 1)]
		public string Name
		{
			get
			{
				return (string)base[TransformerInfo._propName];
			}
			set
			{
				base[TransformerInfo._propName] = value;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06001FBB RID: 8123 RVA: 0x0008BC91 File Offset: 0x0008AC91
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return TransformerInfo._properties;
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06001FBC RID: 8124 RVA: 0x0008BC98 File Offset: 0x0008AC98
		// (set) Token: 0x06001FBD RID: 8125 RVA: 0x0008BCAA File Offset: 0x0008ACAA
		[ConfigurationProperty("type", IsRequired = true, DefaultValue = "")]
		[StringValidator(MinLength = 1)]
		public string Type
		{
			get
			{
				return (string)base[TransformerInfo._propType];
			}
			set
			{
				base[TransformerInfo._propType] = value;
			}
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x0008BCB8 File Offset: 0x0008ACB8
		public override bool Equals(object o)
		{
			if (o == this)
			{
				return true;
			}
			TransformerInfo transformerInfo = o as TransformerInfo;
			return StringUtil.Equals(this.Name, transformerInfo.Name) && StringUtil.Equals(this.Type, transformerInfo.Type);
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x0008BCF8 File Offset: 0x0008ACF8
		public override int GetHashCode()
		{
			return this.Name.GetHashCode() ^ this.Type.GetHashCode();
		}

		// Token: 0x04001A6A RID: 6762
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001A6B RID: 6763
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x04001A6C RID: 6764
		private static readonly ConfigurationProperty _propType = new ConfigurationProperty("type", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired);
	}
}
