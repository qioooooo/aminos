using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x02000211 RID: 529
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class MembershipSection : ConfigurationSection
	{
		// Token: 0x06001C6C RID: 7276 RVA: 0x0008279C File Offset: 0x0008179C
		static MembershipSection()
		{
			MembershipSection._properties.Add(MembershipSection._propProviders);
			MembershipSection._properties.Add(MembershipSection._propDefaultProvider);
			MembershipSection._properties.Add(MembershipSection._propUserIsOnlineTimeWindow);
			MembershipSection._properties.Add(MembershipSection._propHashAlgorithmType);
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001C6E RID: 7278 RVA: 0x000828A0 File Offset: 0x000818A0
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return MembershipSection._properties;
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001C6F RID: 7279 RVA: 0x000828A7 File Offset: 0x000818A7
		[ConfigurationProperty("providers")]
		public ProviderSettingsCollection Providers
		{
			get
			{
				return (ProviderSettingsCollection)base[MembershipSection._propProviders];
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001C70 RID: 7280 RVA: 0x000828B9 File Offset: 0x000818B9
		// (set) Token: 0x06001C71 RID: 7281 RVA: 0x000828CB File Offset: 0x000818CB
		[ConfigurationProperty("defaultProvider", DefaultValue = "AspNetSqlMembershipProvider")]
		[StringValidator(MinLength = 1)]
		public string DefaultProvider
		{
			get
			{
				return (string)base[MembershipSection._propDefaultProvider];
			}
			set
			{
				base[MembershipSection._propDefaultProvider] = value;
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001C72 RID: 7282 RVA: 0x000828D9 File Offset: 0x000818D9
		// (set) Token: 0x06001C73 RID: 7283 RVA: 0x000828EB File Offset: 0x000818EB
		[ConfigurationProperty("hashAlgorithmType", DefaultValue = "")]
		public string HashAlgorithmType
		{
			get
			{
				return (string)base[MembershipSection._propHashAlgorithmType];
			}
			set
			{
				base[MembershipSection._propHashAlgorithmType] = value;
			}
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x000828FC File Offset: 0x000818FC
		internal void ThrowHashAlgorithmException()
		{
			throw new ConfigurationErrorsException(SR.GetString("Invalid_hash_algorithm_type", new object[] { this.HashAlgorithmType }), base.ElementInformation.Properties["hashAlgorithmType"].Source, base.ElementInformation.Properties["hashAlgorithmType"].LineNumber);
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001C75 RID: 7285 RVA: 0x0008295D File Offset: 0x0008195D
		// (set) Token: 0x06001C76 RID: 7286 RVA: 0x0008296F File Offset: 0x0008196F
		[TimeSpanValidator(MinValueString = "00:01:00", MaxValueString = "10675199.02:48:05.4775807")]
		[TypeConverter(typeof(TimeSpanMinutesConverter))]
		[ConfigurationProperty("userIsOnlineTimeWindow", DefaultValue = "00:15:00")]
		public TimeSpan UserIsOnlineTimeWindow
		{
			get
			{
				return (TimeSpan)base[MembershipSection._propUserIsOnlineTimeWindow];
			}
			set
			{
				base[MembershipSection._propUserIsOnlineTimeWindow] = value;
			}
		}

		// Token: 0x040018E3 RID: 6371
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040018E4 RID: 6372
		private static readonly ConfigurationProperty _propProviders = new ConfigurationProperty("providers", typeof(ProviderSettingsCollection), null, ConfigurationPropertyOptions.None);

		// Token: 0x040018E5 RID: 6373
		private static readonly ConfigurationProperty _propDefaultProvider = new ConfigurationProperty("defaultProvider", typeof(string), "AspNetSqlMembershipProvider", null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.None);

		// Token: 0x040018E6 RID: 6374
		private static readonly ConfigurationProperty _propUserIsOnlineTimeWindow = new ConfigurationProperty("userIsOnlineTimeWindow", typeof(TimeSpan), TimeSpan.FromMinutes(15.0), StdValidatorsAndConverters.TimeSpanMinutesConverter, new TimeSpanValidator(TimeSpan.FromMinutes(1.0), TimeSpan.MaxValue), ConfigurationPropertyOptions.None);

		// Token: 0x040018E7 RID: 6375
		private static readonly ConfigurationProperty _propHashAlgorithmType = new ConfigurationProperty("hashAlgorithmType", typeof(string), string.Empty, ConfigurationPropertyOptions.None);
	}
}
