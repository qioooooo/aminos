using System;

namespace System.Configuration
{
	// Token: 0x02000052 RID: 82
	[ConfigurationCollection(typeof(ConnectionStringSettings))]
	public sealed class ConnectionStringSettingsCollection : ConfigurationElementCollection
	{
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600035B RID: 859 RVA: 0x000125F5 File Offset: 0x000115F5
		protected internal override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ConnectionStringSettingsCollection._properties;
			}
		}

		// Token: 0x0600035C RID: 860 RVA: 0x000125FC File Offset: 0x000115FC
		public ConnectionStringSettingsCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x170000E8 RID: 232
		public ConnectionStringSettings this[int index]
		{
			get
			{
				return (ConnectionStringSettings)base.BaseGet(index);
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

		// Token: 0x170000E9 RID: 233
		public ConnectionStringSettings this[string name]
		{
			get
			{
				return (ConnectionStringSettings)base.BaseGet(name);
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0001263F File Offset: 0x0001163F
		public int IndexOf(ConnectionStringSettings settings)
		{
			return base.BaseIndexOf(settings);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00012648 File Offset: 0x00011648
		protected override void BaseAdd(int index, ConfigurationElement element)
		{
			if (index == -1)
			{
				base.BaseAdd(element, false);
				return;
			}
			base.BaseAdd(index, element);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0001265F File Offset: 0x0001165F
		public void Add(ConnectionStringSettings settings)
		{
			this.BaseAdd(settings);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00012668 File Offset: 0x00011668
		public void Remove(ConnectionStringSettings settings)
		{
			if (base.BaseIndexOf(settings) >= 0)
			{
				base.BaseRemove(settings.Key);
			}
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00012680 File Offset: 0x00011680
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00012689 File Offset: 0x00011689
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00012692 File Offset: 0x00011692
		protected override ConfigurationElement CreateNewElement()
		{
			return new ConnectionStringSettings();
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00012699 File Offset: 0x00011699
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ConnectionStringSettings)element).Key;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x000126A6 File Offset: 0x000116A6
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x040002CE RID: 718
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
