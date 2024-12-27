using System;

namespace System.Configuration
{
	// Token: 0x0200008E RID: 142
	[ConfigurationCollection(typeof(ProviderSettings))]
	public sealed class ProviderSettingsCollection : ConfigurationElementCollection
	{
		// Token: 0x06000531 RID: 1329 RVA: 0x00019F55 File Offset: 0x00018F55
		public ProviderSettingsCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x00019F62 File Offset: 0x00018F62
		protected internal override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ProviderSettingsCollection._properties;
			}
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00019F69 File Offset: 0x00018F69
		public void Add(ProviderSettings provider)
		{
			if (provider != null)
			{
				provider.UpdatePropertyCollection();
				this.BaseAdd(provider);
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00019F7C File Offset: 0x00018F7C
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00019F85 File Offset: 0x00018F85
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00019F8D File Offset: 0x00018F8D
		protected override ConfigurationElement CreateNewElement()
		{
			return new ProviderSettings();
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00019F94 File Offset: 0x00018F94
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ProviderSettings)element).Name;
		}

		// Token: 0x1700017D RID: 381
		public ProviderSettings this[string key]
		{
			get
			{
				return (ProviderSettings)base.BaseGet(key);
			}
		}

		// Token: 0x1700017E RID: 382
		public ProviderSettings this[int index]
		{
			get
			{
				return (ProviderSettings)base.BaseGet(index);
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

		// Token: 0x04000378 RID: 888
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
