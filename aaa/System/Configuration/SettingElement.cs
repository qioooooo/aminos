using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000723 RID: 1827
	public sealed class SettingElement : ConfigurationElement
	{
		// Token: 0x060037BD RID: 14269 RVA: 0x000EBF04 File Offset: 0x000EAF04
		static SettingElement()
		{
			SettingElement._properties.Add(SettingElement._propName);
			SettingElement._properties.Add(SettingElement._propSerializeAs);
			SettingElement._properties.Add(SettingElement._propValue);
		}

		// Token: 0x060037BE RID: 14270 RVA: 0x000EBFAC File Offset: 0x000EAFAC
		public SettingElement()
		{
		}

		// Token: 0x060037BF RID: 14271 RVA: 0x000EBFB4 File Offset: 0x000EAFB4
		public SettingElement(string name, SettingsSerializeAs serializeAs)
			: this()
		{
			this.Name = name;
			this.SerializeAs = serializeAs;
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x060037C0 RID: 14272 RVA: 0x000EBFCA File Offset: 0x000EAFCA
		internal string Key
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x060037C1 RID: 14273 RVA: 0x000EBFD4 File Offset: 0x000EAFD4
		public override bool Equals(object settings)
		{
			SettingElement settingElement = settings as SettingElement;
			return settingElement != null && base.Equals(settings) && object.Equals(settingElement.Value, this.Value);
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x000EC007 File Offset: 0x000EB007
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.Value.GetHashCode();
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x060037C3 RID: 14275 RVA: 0x000EC01B File Offset: 0x000EB01B
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return SettingElement._properties;
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x060037C4 RID: 14276 RVA: 0x000EC022 File Offset: 0x000EB022
		// (set) Token: 0x060037C5 RID: 14277 RVA: 0x000EC034 File Offset: 0x000EB034
		[ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "")]
		public string Name
		{
			get
			{
				return (string)base[SettingElement._propName];
			}
			set
			{
				base[SettingElement._propName] = value;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x060037C6 RID: 14278 RVA: 0x000EC042 File Offset: 0x000EB042
		// (set) Token: 0x060037C7 RID: 14279 RVA: 0x000EC054 File Offset: 0x000EB054
		[ConfigurationProperty("serializeAs", IsRequired = true, DefaultValue = SettingsSerializeAs.String)]
		public SettingsSerializeAs SerializeAs
		{
			get
			{
				return (SettingsSerializeAs)base[SettingElement._propSerializeAs];
			}
			set
			{
				base[SettingElement._propSerializeAs] = value;
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x060037C8 RID: 14280 RVA: 0x000EC067 File Offset: 0x000EB067
		// (set) Token: 0x060037C9 RID: 14281 RVA: 0x000EC079 File Offset: 0x000EB079
		[ConfigurationProperty("value", IsRequired = true, DefaultValue = null)]
		public SettingValueElement Value
		{
			get
			{
				return (SettingValueElement)base[SettingElement._propValue];
			}
			set
			{
				base[SettingElement._propValue] = value;
			}
		}

		// Token: 0x040031DB RID: 12763
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040031DC RID: 12764
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), "", ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x040031DD RID: 12765
		private static readonly ConfigurationProperty _propSerializeAs = new ConfigurationProperty("serializeAs", typeof(SettingsSerializeAs), SettingsSerializeAs.String, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x040031DE RID: 12766
		private static readonly ConfigurationProperty _propValue = new ConfigurationProperty("value", typeof(SettingValueElement), null, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x040031DF RID: 12767
		private static XmlDocument doc = new XmlDocument();
	}
}
