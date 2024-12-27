using System;

namespace System.Configuration
{
	// Token: 0x0200007D RID: 125
	[ConfigurationCollection(typeof(NameValueConfigurationElement))]
	public sealed class NameValueConfigurationCollection : ConfigurationElementCollection
	{
		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x0001895E File Offset: 0x0001795E
		protected internal override ConfigurationPropertyCollection Properties
		{
			get
			{
				return NameValueConfigurationCollection._properties;
			}
		}

		// Token: 0x1700014A RID: 330
		public NameValueConfigurationElement this[string name]
		{
			get
			{
				return (NameValueConfigurationElement)base.BaseGet(name);
			}
			set
			{
				int num = -1;
				NameValueConfigurationElement nameValueConfigurationElement = (NameValueConfigurationElement)base.BaseGet(name);
				if (nameValueConfigurationElement != null)
				{
					num = base.BaseIndexOf(nameValueConfigurationElement);
					base.BaseRemoveAt(num);
				}
				this.BaseAdd(num, value);
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x000189B2 File Offset: 0x000179B2
		public string[] AllKeys
		{
			get
			{
				return StringUtil.ObjectArrayToStringArray(base.BaseGetAllKeys());
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x000189BF File Offset: 0x000179BF
		public void Add(NameValueConfigurationElement nameValue)
		{
			this.BaseAdd(nameValue);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x000189C8 File Offset: 0x000179C8
		public void Remove(NameValueConfigurationElement nameValue)
		{
			if (base.BaseIndexOf(nameValue) >= 0)
			{
				base.BaseRemove(nameValue.Name);
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x000189E0 File Offset: 0x000179E0
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x000189E9 File Offset: 0x000179E9
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x000189F1 File Offset: 0x000179F1
		protected override ConfigurationElement CreateNewElement()
		{
			return new NameValueConfigurationElement();
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x000189F8 File Offset: 0x000179F8
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((NameValueConfigurationElement)element).Name;
		}

		// Token: 0x0400034C RID: 844
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
