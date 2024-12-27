using System;
using System.Configuration;
using System.Security.Permissions;
using System.Xml;

namespace System.Web.Configuration
{
	// Token: 0x02000227 RID: 551
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProfileGroupSettings : ConfigurationElement
	{
		// Token: 0x06001D93 RID: 7571 RVA: 0x00086278 File Offset: 0x00085278
		static ProfileGroupSettings()
		{
			ProfileGroupSettings._properties.Add(ProfileGroupSettings._propName);
			ProfileGroupSettings._properties.Add(ProfileGroupSettings._propProperties);
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x000862E5 File Offset: 0x000852E5
		internal void InternalDeserialize(XmlReader reader, bool serializeCollectionKey)
		{
			this.DeserializeElement(reader, serializeCollectionKey);
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x000862EF File Offset: 0x000852EF
		internal ProfileGroupSettings()
		{
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x000862F7 File Offset: 0x000852F7
		public ProfileGroupSettings(string name)
		{
			base[ProfileGroupSettings._propName] = name;
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x0008630C File Offset: 0x0008530C
		public override bool Equals(object obj)
		{
			ProfileGroupSettings profileGroupSettings = obj as ProfileGroupSettings;
			return profileGroupSettings != null && this.Name == profileGroupSettings.Name && object.Equals(this.PropertySettings, profileGroupSettings.PropertySettings);
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x00086349 File Offset: 0x00085349
		public override int GetHashCode()
		{
			return this.Name.GetHashCode() ^ this.PropertySettings.GetHashCode();
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001D99 RID: 7577 RVA: 0x00086362 File Offset: 0x00085362
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ProfileGroupSettings._properties;
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001D9A RID: 7578 RVA: 0x00086369 File Offset: 0x00085369
		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public string Name
		{
			get
			{
				return (string)base[ProfileGroupSettings._propName];
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001D9B RID: 7579 RVA: 0x0008637B File Offset: 0x0008537B
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public ProfilePropertySettingsCollection PropertySettings
		{
			get
			{
				return (ProfilePropertySettingsCollection)base[ProfileGroupSettings._propProperties];
			}
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x0008638D File Offset: 0x0008538D
		internal void InternalReset(ProfileGroupSettings parentSettings)
		{
			base.Reset(parentSettings);
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x00086396 File Offset: 0x00085396
		internal void InternalUnmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		{
			base.Unmerge(sourceElement, parentElement, saveMode);
		}

		// Token: 0x04001981 RID: 6529
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001982 RID: 6530
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), null, null, ProfilePropertyNameValidator.SingletonInstance, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x04001983 RID: 6531
		private static readonly ConfigurationProperty _propProperties = new ConfigurationProperty(null, typeof(ProfilePropertySettingsCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}
