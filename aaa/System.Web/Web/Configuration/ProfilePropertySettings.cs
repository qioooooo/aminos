using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x0200022A RID: 554
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProfilePropertySettings : ConfigurationElement
	{
		// Token: 0x06001DBC RID: 7612 RVA: 0x000866C8 File Offset: 0x000856C8
		static ProfilePropertySettings()
		{
			ProfilePropertySettings._properties.Add(ProfilePropertySettings._propName);
			ProfilePropertySettings._properties.Add(ProfilePropertySettings._propReadOnly);
			ProfilePropertySettings._properties.Add(ProfilePropertySettings._propSerializeAs);
			ProfilePropertySettings._properties.Add(ProfilePropertySettings._propProviderName);
			ProfilePropertySettings._properties.Add(ProfilePropertySettings._propDefaultValue);
			ProfilePropertySettings._properties.Add(ProfilePropertySettings._propType);
			ProfilePropertySettings._properties.Add(ProfilePropertySettings._propAllowAnonymous);
			ProfilePropertySettings._properties.Add(ProfilePropertySettings._propCustomProviderData);
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x00086854 File Offset: 0x00085854
		internal ProfilePropertySettings()
		{
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x0008685C File Offset: 0x0008585C
		public ProfilePropertySettings(string name)
		{
			this.Name = name;
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x0008686C File Offset: 0x0008586C
		public ProfilePropertySettings(string name, bool readOnly, SerializationMode serializeAs, string providerName, string defaultValue, string profileType, bool allowAnonymous, string customProviderData)
		{
			this.Name = name;
			this.ReadOnly = readOnly;
			this.SerializeAs = serializeAs;
			this.Provider = providerName;
			this.DefaultValue = defaultValue;
			this.Type = profileType;
			this.AllowAnonymous = allowAnonymous;
			this.CustomProviderData = customProviderData;
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001DC0 RID: 7616 RVA: 0x000868BC File Offset: 0x000858BC
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ProfilePropertySettings._properties;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001DC1 RID: 7617 RVA: 0x000868C3 File Offset: 0x000858C3
		// (set) Token: 0x06001DC2 RID: 7618 RVA: 0x000868D5 File Offset: 0x000858D5
		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public string Name
		{
			get
			{
				return (string)base[ProfilePropertySettings._propName];
			}
			set
			{
				base[ProfilePropertySettings._propName] = value;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001DC3 RID: 7619 RVA: 0x000868E3 File Offset: 0x000858E3
		// (set) Token: 0x06001DC4 RID: 7620 RVA: 0x000868F5 File Offset: 0x000858F5
		[ConfigurationProperty("readOnly", DefaultValue = false)]
		public bool ReadOnly
		{
			get
			{
				return (bool)base[ProfilePropertySettings._propReadOnly];
			}
			set
			{
				base[ProfilePropertySettings._propReadOnly] = value;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001DC5 RID: 7621 RVA: 0x00086908 File Offset: 0x00085908
		// (set) Token: 0x06001DC6 RID: 7622 RVA: 0x0008691A File Offset: 0x0008591A
		[ConfigurationProperty("serializeAs", DefaultValue = SerializationMode.ProviderSpecific)]
		public SerializationMode SerializeAs
		{
			get
			{
				return (SerializationMode)base[ProfilePropertySettings._propSerializeAs];
			}
			set
			{
				base[ProfilePropertySettings._propSerializeAs] = value;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001DC7 RID: 7623 RVA: 0x0008692D File Offset: 0x0008592D
		// (set) Token: 0x06001DC8 RID: 7624 RVA: 0x0008693F File Offset: 0x0008593F
		[ConfigurationProperty("provider", DefaultValue = "")]
		public string Provider
		{
			get
			{
				return (string)base[ProfilePropertySettings._propProviderName];
			}
			set
			{
				base[ProfilePropertySettings._propProviderName] = value;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001DC9 RID: 7625 RVA: 0x0008694D File Offset: 0x0008594D
		// (set) Token: 0x06001DCA RID: 7626 RVA: 0x00086955 File Offset: 0x00085955
		internal SettingsProvider ProviderInternal
		{
			get
			{
				return this._providerInternal;
			}
			set
			{
				this._providerInternal = value;
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001DCB RID: 7627 RVA: 0x0008695E File Offset: 0x0008595E
		// (set) Token: 0x06001DCC RID: 7628 RVA: 0x00086970 File Offset: 0x00085970
		[ConfigurationProperty("defaultValue", DefaultValue = "")]
		public string DefaultValue
		{
			get
			{
				return (string)base[ProfilePropertySettings._propDefaultValue];
			}
			set
			{
				base[ProfilePropertySettings._propDefaultValue] = value;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001DCD RID: 7629 RVA: 0x0008697E File Offset: 0x0008597E
		// (set) Token: 0x06001DCE RID: 7630 RVA: 0x00086990 File Offset: 0x00085990
		[ConfigurationProperty("type", DefaultValue = "string")]
		public string Type
		{
			get
			{
				return (string)base[ProfilePropertySettings._propType];
			}
			set
			{
				base[ProfilePropertySettings._propType] = value;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001DCF RID: 7631 RVA: 0x0008699E File Offset: 0x0008599E
		// (set) Token: 0x06001DD0 RID: 7632 RVA: 0x000869A6 File Offset: 0x000859A6
		internal Type TypeInternal
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001DD1 RID: 7633 RVA: 0x000869AF File Offset: 0x000859AF
		// (set) Token: 0x06001DD2 RID: 7634 RVA: 0x000869C1 File Offset: 0x000859C1
		[ConfigurationProperty("allowAnonymous", DefaultValue = false)]
		public bool AllowAnonymous
		{
			get
			{
				return (bool)base[ProfilePropertySettings._propAllowAnonymous];
			}
			set
			{
				base[ProfilePropertySettings._propAllowAnonymous] = value;
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001DD3 RID: 7635 RVA: 0x000869D4 File Offset: 0x000859D4
		// (set) Token: 0x06001DD4 RID: 7636 RVA: 0x000869E6 File Offset: 0x000859E6
		[ConfigurationProperty("customProviderData", DefaultValue = "")]
		public string CustomProviderData
		{
			get
			{
				return (string)base[ProfilePropertySettings._propCustomProviderData];
			}
			set
			{
				base[ProfilePropertySettings._propCustomProviderData] = value;
			}
		}

		// Token: 0x04001987 RID: 6535
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001988 RID: 6536
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), null, null, ProfilePropertyNameValidator.SingletonInstance, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x04001989 RID: 6537
		private static readonly ConfigurationProperty _propReadOnly = new ConfigurationProperty("readOnly", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x0400198A RID: 6538
		private static readonly ConfigurationProperty _propSerializeAs = new ConfigurationProperty("serializeAs", typeof(SerializationMode), SerializationMode.ProviderSpecific, ConfigurationPropertyOptions.None);

		// Token: 0x0400198B RID: 6539
		private static readonly ConfigurationProperty _propProviderName = new ConfigurationProperty("provider", typeof(string), "", ConfigurationPropertyOptions.None);

		// Token: 0x0400198C RID: 6540
		private static readonly ConfigurationProperty _propDefaultValue = new ConfigurationProperty("defaultValue", typeof(string), "", ConfigurationPropertyOptions.None);

		// Token: 0x0400198D RID: 6541
		private static readonly ConfigurationProperty _propType = new ConfigurationProperty("type", typeof(string), "string", ConfigurationPropertyOptions.None);

		// Token: 0x0400198E RID: 6542
		private static readonly ConfigurationProperty _propAllowAnonymous = new ConfigurationProperty("allowAnonymous", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x0400198F RID: 6543
		private static readonly ConfigurationProperty _propCustomProviderData = new ConfigurationProperty("customProviderData", typeof(string), "", ConfigurationPropertyOptions.None);

		// Token: 0x04001990 RID: 6544
		private Type _type;

		// Token: 0x04001991 RID: 6545
		private SettingsProvider _providerInternal;
	}
}
