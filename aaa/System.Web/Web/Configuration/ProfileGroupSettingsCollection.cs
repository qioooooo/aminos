using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;
using System.Xml;

namespace System.Web.Configuration
{
	// Token: 0x02000228 RID: 552
	[ConfigurationCollection(typeof(ProfileGroupSettings), AddItemName = "group")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProfileGroupSettingsCollection : ConfigurationElementCollection
	{
		// Token: 0x06001D9F RID: 7583 RVA: 0x000863AD File Offset: 0x000853AD
		public ProfileGroupSettingsCollection()
		{
			base.AddElementName = "group";
			base.ClearElementName = string.Empty;
			base.EmitClear = false;
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001DA0 RID: 7584 RVA: 0x000863D2 File Offset: 0x000853D2
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ProfileGroupSettingsCollection._properties;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001DA1 RID: 7585 RVA: 0x000863D9 File Offset: 0x000853D9
		public string[] AllKeys
		{
			get
			{
				return StringUtil.ObjectArrayToStringArray(base.BaseGetAllKeys());
			}
		}

		// Token: 0x17000600 RID: 1536
		public ProfileGroupSettings this[string name]
		{
			get
			{
				return (ProfileGroupSettings)base.BaseGet(name);
			}
		}

		// Token: 0x17000601 RID: 1537
		public ProfileGroupSettings this[int index]
		{
			get
			{
				return (ProfileGroupSettings)base.BaseGet(index);
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				this.BaseAdd(index, value);
			}
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x0008641C File Offset: 0x0008541C
		internal void AddOrReplace(ProfileGroupSettings groupSettings)
		{
			base.BaseAdd(groupSettings, false);
		}

		// Token: 0x06001DA6 RID: 7590 RVA: 0x00086426 File Offset: 0x00085426
		protected override ConfigurationElement CreateNewElement()
		{
			return new ProfileGroupSettings();
		}

		// Token: 0x06001DA7 RID: 7591 RVA: 0x0008642D File Offset: 0x0008542D
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ProfileGroupSettings)element).Name;
		}

		// Token: 0x06001DA8 RID: 7592 RVA: 0x0008643A File Offset: 0x0008543A
		internal bool InternalIsModified()
		{
			return this.IsModified();
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x00086442 File Offset: 0x00085442
		internal void InternalResetModified()
		{
			this.ResetModified();
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x0008644A File Offset: 0x0008544A
		internal void InternalReset(ConfigurationElement parentElement)
		{
			this.Reset(parentElement);
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x00086454 File Offset: 0x00085454
		internal void InternalUnMerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		{
			this.Unmerge(sourceElement, parentElement, saveMode);
			base.BaseClear();
			ProfileGroupSettingsCollection profileGroupSettingsCollection = sourceElement as ProfileGroupSettingsCollection;
			ProfileGroupSettingsCollection profileGroupSettingsCollection2 = parentElement as ProfileGroupSettingsCollection;
			foreach (object obj in profileGroupSettingsCollection)
			{
				ProfileGroupSettings profileGroupSettings = (ProfileGroupSettings)obj;
				ProfileGroupSettings profileGroupSettings2 = profileGroupSettingsCollection2.Get(profileGroupSettings.Name);
				ProfileGroupSettings profileGroupSettings3 = new ProfileGroupSettings();
				profileGroupSettings3.InternalUnmerge(profileGroupSettings, profileGroupSettings2, saveMode);
				this.BaseAdd(profileGroupSettings3);
			}
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x000864EC File Offset: 0x000854EC
		internal bool InternalSerialize(XmlWriter writer, bool serializeCollectionKey)
		{
			if (base.EmitClear)
			{
				throw new ConfigurationErrorsException(SR.GetString("Clear_not_valid"));
			}
			return this.SerializeElement(writer, serializeCollectionKey);
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x0008650E File Offset: 0x0008550E
		public void Add(ProfileGroupSettings group)
		{
			this.BaseAdd(group);
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x00086517 File Offset: 0x00085517
		public ProfileGroupSettings Get(int index)
		{
			return (ProfileGroupSettings)base.BaseGet(index);
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x00086525 File Offset: 0x00085525
		public ProfileGroupSettings Get(string name)
		{
			return (ProfileGroupSettings)base.BaseGet(name);
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x00086533 File Offset: 0x00085533
		public string GetKey(int index)
		{
			return (string)base.BaseGetKey(index);
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x00086541 File Offset: 0x00085541
		public void Set(ProfileGroupSettings group)
		{
			base.BaseAdd(group, false);
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x0008654B File Offset: 0x0008554B
		public int IndexOf(ProfileGroupSettings group)
		{
			return base.BaseIndexOf(group);
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x00086554 File Offset: 0x00085554
		public void Remove(string name)
		{
			ConfigurationElement configurationElement = base.BaseGet(name);
			if (configurationElement == null)
			{
				return;
			}
			ElementInformation elementInformation = configurationElement.ElementInformation;
			if (elementInformation.IsPresent)
			{
				base.BaseRemove(name);
				return;
			}
			throw new ConfigurationErrorsException(SR.GetString("Config_base_cannot_remove_inherited_items"));
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x00086594 File Offset: 0x00085594
		public void RemoveAt(int index)
		{
			ConfigurationElement configurationElement = base.BaseGet(index);
			if (configurationElement == null)
			{
				return;
			}
			ElementInformation elementInformation = configurationElement.ElementInformation;
			if (elementInformation.IsPresent)
			{
				base.BaseRemoveAt(index);
				return;
			}
			throw new ConfigurationErrorsException(SR.GetString("Config_base_cannot_remove_inherited_items"));
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x000865D4 File Offset: 0x000855D4
		public void Clear()
		{
			int num = base.Count - 1;
			this.bModified = true;
			for (int i = num; i >= 0; i--)
			{
				ConfigurationElement configurationElement = base.BaseGet(i);
				if (configurationElement != null)
				{
					ElementInformation elementInformation = configurationElement.ElementInformation;
					if (elementInformation.IsPresent)
					{
						base.BaseRemoveAt(i);
					}
				}
			}
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x0008661E File Offset: 0x0008561E
		protected override void ResetModified()
		{
			this.bModified = false;
			base.ResetModified();
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x0008662D File Offset: 0x0008562D
		protected override bool IsModified()
		{
			return this.bModified || base.IsModified();
		}

		// Token: 0x04001984 RID: 6532
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001985 RID: 6533
		private bool bModified;
	}
}
