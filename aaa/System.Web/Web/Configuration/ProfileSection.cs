using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x0200022C RID: 556
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProfileSection : ConfigurationSection
	{
		// Token: 0x06001DEA RID: 7658 RVA: 0x00086B28 File Offset: 0x00085B28
		static ProfileSection()
		{
			ProfileSection._properties.Add(ProfileSection._propEnabled);
			ProfileSection._properties.Add(ProfileSection._propDefaultProvider);
			ProfileSection._properties.Add(ProfileSection._propProviders);
			ProfileSection._properties.Add(ProfileSection._propProfile);
			ProfileSection._properties.Add(ProfileSection._propInherits);
			ProfileSection._properties.Add(ProfileSection._propAutomaticSaveEnabled);
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001DEB RID: 7659 RVA: 0x00086C53 File Offset: 0x00085C53
		internal long RecompilationHash
		{
			get
			{
				if (!this._recompilationHashCached)
				{
					this._recompilationHash = this.CalculateHash();
					this._recompilationHashCached = true;
				}
				return this._recompilationHash;
			}
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x00086C78 File Offset: 0x00085C78
		private long CalculateHash()
		{
			HashCodeCombiner hashCodeCombiner = new HashCodeCombiner();
			this.CalculateProfilePropertySettingsHash(this.PropertySettings, hashCodeCombiner);
			if (this.PropertySettings != null)
			{
				foreach (object obj in this.PropertySettings.GroupSettings)
				{
					ProfileGroupSettings profileGroupSettings = (ProfileGroupSettings)obj;
					hashCodeCombiner.AddObject(profileGroupSettings.Name);
					this.CalculateProfilePropertySettingsHash(profileGroupSettings.PropertySettings, hashCodeCombiner);
				}
			}
			return hashCodeCombiner.CombinedHash;
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x00086D0C File Offset: 0x00085D0C
		private void CalculateProfilePropertySettingsHash(ProfilePropertySettingsCollection settings, HashCodeCombiner hashCombiner)
		{
			foreach (object obj in settings)
			{
				ProfilePropertySettings profilePropertySettings = (ProfilePropertySettings)obj;
				hashCombiner.AddObject(profilePropertySettings.Name);
				hashCombiner.AddObject(profilePropertySettings.Type);
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001DEF RID: 7663 RVA: 0x00086D7C File Offset: 0x00085D7C
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ProfileSection._properties;
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001DF0 RID: 7664 RVA: 0x00086D83 File Offset: 0x00085D83
		// (set) Token: 0x06001DF1 RID: 7665 RVA: 0x00086D95 File Offset: 0x00085D95
		[ConfigurationProperty("automaticSaveEnabled", DefaultValue = true)]
		public bool AutomaticSaveEnabled
		{
			get
			{
				return (bool)base[ProfileSection._propAutomaticSaveEnabled];
			}
			set
			{
				base[ProfileSection._propAutomaticSaveEnabled] = value;
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001DF2 RID: 7666 RVA: 0x00086DA8 File Offset: 0x00085DA8
		// (set) Token: 0x06001DF3 RID: 7667 RVA: 0x00086DBA File Offset: 0x00085DBA
		[ConfigurationProperty("enabled", DefaultValue = true)]
		public bool Enabled
		{
			get
			{
				return (bool)base[ProfileSection._propEnabled];
			}
			set
			{
				base[ProfileSection._propEnabled] = value;
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06001DF4 RID: 7668 RVA: 0x00086DCD File Offset: 0x00085DCD
		// (set) Token: 0x06001DF5 RID: 7669 RVA: 0x00086DDF File Offset: 0x00085DDF
		[ConfigurationProperty("defaultProvider", DefaultValue = "AspNetSqlProfileProvider")]
		[StringValidator(MinLength = 1)]
		public string DefaultProvider
		{
			get
			{
				return (string)base[ProfileSection._propDefaultProvider];
			}
			set
			{
				base[ProfileSection._propDefaultProvider] = value;
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06001DF6 RID: 7670 RVA: 0x00086DED File Offset: 0x00085DED
		// (set) Token: 0x06001DF7 RID: 7671 RVA: 0x00086DFF File Offset: 0x00085DFF
		[ConfigurationProperty("inherits", DefaultValue = "")]
		public string Inherits
		{
			get
			{
				return (string)base[ProfileSection._propInherits];
			}
			set
			{
				base[ProfileSection._propInherits] = value;
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06001DF8 RID: 7672 RVA: 0x00086E0D File Offset: 0x00085E0D
		[ConfigurationProperty("providers")]
		public ProviderSettingsCollection Providers
		{
			get
			{
				return (ProviderSettingsCollection)base[ProfileSection._propProviders];
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06001DF9 RID: 7673 RVA: 0x00086E1F File Offset: 0x00085E1F
		[ConfigurationProperty("properties")]
		public RootProfilePropertySettingsCollection PropertySettings
		{
			get
			{
				return (RootProfilePropertySettingsCollection)base[ProfileSection._propProfile];
			}
		}

		// Token: 0x04001993 RID: 6547
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001994 RID: 6548
		private static readonly ConfigurationProperty _propEnabled = new ConfigurationProperty("enabled", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001995 RID: 6549
		private static readonly ConfigurationProperty _propDefaultProvider = new ConfigurationProperty("defaultProvider", typeof(string), "AspNetSqlProfileProvider", null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.None);

		// Token: 0x04001996 RID: 6550
		private static readonly ConfigurationProperty _propProviders = new ConfigurationProperty("providers", typeof(ProviderSettingsCollection), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001997 RID: 6551
		private static readonly ConfigurationProperty _propProfile = new ConfigurationProperty("properties", typeof(RootProfilePropertySettingsCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

		// Token: 0x04001998 RID: 6552
		private static readonly ConfigurationProperty _propInherits = new ConfigurationProperty("inherits", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x04001999 RID: 6553
		private static readonly ConfigurationProperty _propAutomaticSaveEnabled = new ConfigurationProperty("automaticSaveEnabled", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x0400199A RID: 6554
		private long _recompilationHash;

		// Token: 0x0400199B RID: 6555
		private bool _recompilationHashCached;
	}
}
