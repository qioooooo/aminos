using System;

namespace System.Configuration
{
	// Token: 0x02000073 RID: 115
	[ConfigurationCollection(typeof(KeyValueConfigurationElement))]
	public class KeyValueConfigurationCollection : ConfigurationElementCollection
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x0001411B File Offset: 0x0001311B
		protected internal override ConfigurationPropertyCollection Properties
		{
			get
			{
				return KeyValueConfigurationCollection._properties;
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00014122 File Offset: 0x00013122
		public KeyValueConfigurationCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
			this.internalAddToEnd = true;
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x00014136 File Offset: 0x00013136
		protected override bool ThrowOnDuplicate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000129 RID: 297
		public KeyValueConfigurationElement this[string key]
		{
			get
			{
				return (KeyValueConfigurationElement)base.BaseGet(key);
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x00014147 File Offset: 0x00013147
		public string[] AllKeys
		{
			get
			{
				return StringUtil.ObjectArrayToStringArray(base.BaseGetAllKeys());
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00014154 File Offset: 0x00013154
		public void Add(KeyValueConfigurationElement keyValue)
		{
			keyValue.Init();
			KeyValueConfigurationElement keyValueConfigurationElement = (KeyValueConfigurationElement)base.BaseGet(keyValue.Key);
			if (keyValueConfigurationElement == null)
			{
				this.BaseAdd(keyValue);
				return;
			}
			KeyValueConfigurationElement keyValueConfigurationElement2 = keyValueConfigurationElement;
			keyValueConfigurationElement2.Value = keyValueConfigurationElement2.Value + "," + keyValue.Value;
			int num = base.BaseIndexOf(keyValueConfigurationElement);
			base.BaseRemoveAt(num);
			this.BaseAdd(num, keyValueConfigurationElement);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x000141B8 File Offset: 0x000131B8
		public void Add(string key, string value)
		{
			KeyValueConfigurationElement keyValueConfigurationElement = new KeyValueConfigurationElement(key, value);
			this.Add(keyValueConfigurationElement);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x000141D4 File Offset: 0x000131D4
		public void Remove(string key)
		{
			base.BaseRemove(key);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x000141DD File Offset: 0x000131DD
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x000141E5 File Offset: 0x000131E5
		protected override ConfigurationElement CreateNewElement()
		{
			return new KeyValueConfigurationElement();
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x000141EC File Offset: 0x000131EC
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((KeyValueConfigurationElement)element).Key;
		}

		// Token: 0x04000327 RID: 807
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
