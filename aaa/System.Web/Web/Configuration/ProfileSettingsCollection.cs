using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x0200022E RID: 558
	[ConfigurationCollection(typeof(ProfileSettings))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProfileSettingsCollection : ConfigurationElementCollection
	{
		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06001E0B RID: 7691 RVA: 0x00087077 File Offset: 0x00086077
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ProfileSettingsCollection._properties;
			}
		}

		// Token: 0x17000622 RID: 1570
		public ProfileSettings this[int index]
		{
			get
			{
				return (ProfileSettings)base.BaseGet(index);
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

		// Token: 0x17000623 RID: 1571
		public ProfileSettings this[string key]
		{
			get
			{
				return (ProfileSettings)base.BaseGet(key);
			}
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x000870BC File Offset: 0x000860BC
		protected override ConfigurationElement CreateNewElement()
		{
			return new ProfileSettings();
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x000870C3 File Offset: 0x000860C3
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ProfileSettings)element).Name;
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x000870D0 File Offset: 0x000860D0
		public void Add(ProfileSettings profilesSettings)
		{
			this.BaseAdd(profilesSettings);
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x000870D9 File Offset: 0x000860D9
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x000870E1 File Offset: 0x000860E1
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x000870EA File Offset: 0x000860EA
		public void Insert(int index, ProfileSettings authorizationSettings)
		{
			this.BaseAdd(index, authorizationSettings);
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x000870F4 File Offset: 0x000860F4
		public int IndexOf(string name)
		{
			ConfigurationElement configurationElement = base.BaseGet(name);
			if (configurationElement == null)
			{
				return -1;
			}
			return base.BaseIndexOf(configurationElement);
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x00087115 File Offset: 0x00086115
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x0008711E File Offset: 0x0008611E
		public bool Contains(string name)
		{
			return this.IndexOf(name) != -1;
		}

		// Token: 0x040019A2 RID: 6562
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
