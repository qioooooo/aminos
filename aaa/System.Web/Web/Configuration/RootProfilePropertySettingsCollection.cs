using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;
using System.Xml;

namespace System.Web.Configuration
{
	// Token: 0x0200023C RID: 572
	[ConfigurationCollection(typeof(ProfilePropertySettings))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class RootProfilePropertySettingsCollection : ProfilePropertySettingsCollection
	{
		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06001E9D RID: 7837 RVA: 0x000894F8 File Offset: 0x000884F8
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return RootProfilePropertySettingsCollection._properties;
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06001E9F RID: 7839 RVA: 0x00089512 File Offset: 0x00088512
		protected override bool AllowClear
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06001EA0 RID: 7840 RVA: 0x00089515 File Offset: 0x00088515
		protected override bool ThrowOnDuplicate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x00089518 File Offset: 0x00088518
		protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
		{
			bool flag;
			if (elementName == "group")
			{
				ProfileGroupSettings profileGroupSettings = null;
				string attribute = reader.GetAttribute("name");
				ProfileGroupSettingsCollection groupSettings = this.GroupSettings;
				if (attribute != null)
				{
					profileGroupSettings = groupSettings[attribute];
				}
				ProfileGroupSettings profileGroupSettings2 = new ProfileGroupSettings();
				profileGroupSettings2.InternalReset(profileGroupSettings);
				profileGroupSettings2.InternalDeserialize(reader, false);
				groupSettings.AddOrReplace(profileGroupSettings2);
				flag = true;
			}
			else
			{
				if (elementName == "clear")
				{
					this.GroupSettings.Clear();
				}
				flag = base.OnDeserializeUnrecognizedElement(elementName, reader);
			}
			return flag;
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x00089599 File Offset: 0x00088599
		protected override bool IsModified()
		{
			return base.IsModified() || this.GroupSettings.InternalIsModified();
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x000895B0 File Offset: 0x000885B0
		protected override void ResetModified()
		{
			base.ResetModified();
			this.GroupSettings.InternalResetModified();
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x000895C4 File Offset: 0x000885C4
		public override bool Equals(object rootProfilePropertySettingsCollection)
		{
			RootProfilePropertySettingsCollection rootProfilePropertySettingsCollection2 = rootProfilePropertySettingsCollection as RootProfilePropertySettingsCollection;
			return rootProfilePropertySettingsCollection2 != null && object.Equals(this, rootProfilePropertySettingsCollection2) && object.Equals(this.GroupSettings, rootProfilePropertySettingsCollection2.GroupSettings);
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x000895F7 File Offset: 0x000885F7
		public override int GetHashCode()
		{
			return HashCodeCombiner.CombineHashCodes(base.GetHashCode(), this.GroupSettings.GetHashCode());
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x00089610 File Offset: 0x00088610
		protected override void Reset(ConfigurationElement parentElement)
		{
			RootProfilePropertySettingsCollection rootProfilePropertySettingsCollection = parentElement as RootProfilePropertySettingsCollection;
			base.Reset(parentElement);
			this.GroupSettings.InternalReset(rootProfilePropertySettingsCollection.GroupSettings);
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x0008963C File Offset: 0x0008863C
		protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		{
			RootProfilePropertySettingsCollection rootProfilePropertySettingsCollection = parentElement as RootProfilePropertySettingsCollection;
			RootProfilePropertySettingsCollection rootProfilePropertySettingsCollection2 = sourceElement as RootProfilePropertySettingsCollection;
			base.Unmerge(sourceElement, parentElement, saveMode);
			this.GroupSettings.InternalUnMerge(rootProfilePropertySettingsCollection2.GroupSettings, (rootProfilePropertySettingsCollection != null) ? rootProfilePropertySettingsCollection.GroupSettings : null, saveMode);
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x00089680 File Offset: 0x00088680
		protected override bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
		{
			bool flag = false;
			if (base.SerializeElement(null, false) || this.GroupSettings.InternalSerialize(null, false))
			{
				flag |= base.SerializeElement(writer, false);
				flag |= this.GroupSettings.InternalSerialize(writer, false);
			}
			return flag;
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06001EA9 RID: 7849 RVA: 0x000896C4 File Offset: 0x000886C4
		[ConfigurationProperty("group")]
		public ProfileGroupSettingsCollection GroupSettings
		{
			get
			{
				return this._propGroups;
			}
		}

		// Token: 0x040019E5 RID: 6629
		private ProfileGroupSettingsCollection _propGroups = new ProfileGroupSettingsCollection();

		// Token: 0x040019E6 RID: 6630
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
