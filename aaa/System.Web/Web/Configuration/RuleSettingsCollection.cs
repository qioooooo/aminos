using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x02000243 RID: 579
	[ConfigurationCollection(typeof(RuleSettings))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class RuleSettingsCollection : ConfigurationElementCollection
	{
		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06001EC3 RID: 7875 RVA: 0x00089AB3 File Offset: 0x00088AB3
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return RuleSettingsCollection._properties;
			}
		}

		// Token: 0x17000652 RID: 1618
		public RuleSettings this[int index]
		{
			get
			{
				return (RuleSettings)base.BaseGet(index);
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

		// Token: 0x17000653 RID: 1619
		public RuleSettings this[string key]
		{
			get
			{
				return (RuleSettings)base.BaseGet(key);
			}
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x00089AF8 File Offset: 0x00088AF8
		protected override ConfigurationElement CreateNewElement()
		{
			return new RuleSettings();
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x00089AFF File Offset: 0x00088AFF
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((RuleSettings)element).Name;
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x00089B0C File Offset: 0x00088B0C
		public void Add(RuleSettings ruleSettings)
		{
			this.BaseAdd(ruleSettings);
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x00089B15 File Offset: 0x00088B15
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x00089B1D File Offset: 0x00088B1D
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x00089B26 File Offset: 0x00088B26
		public void Insert(int index, RuleSettings eventSettings)
		{
			this.BaseAdd(index, eventSettings);
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x00089B30 File Offset: 0x00088B30
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x00089B3C File Offset: 0x00088B3C
		public int IndexOf(string name)
		{
			ConfigurationElement configurationElement = base.BaseGet(name);
			if (configurationElement == null)
			{
				return -1;
			}
			return base.BaseIndexOf(configurationElement);
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x00089B5D File Offset: 0x00088B5D
		public bool Contains(string name)
		{
			return this.IndexOf(name) != -1;
		}

		// Token: 0x04001A15 RID: 6677
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
