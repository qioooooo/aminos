using System;

namespace System.Configuration
{
	// Token: 0x02000722 RID: 1826
	public sealed class SettingElementCollection : ConfigurationElementCollection
	{
		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x060037B4 RID: 14260 RVA: 0x000EBEB0 File Offset: 0x000EAEB0
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x060037B5 RID: 14261 RVA: 0x000EBEB3 File Offset: 0x000EAEB3
		protected override string ElementName
		{
			get
			{
				return "setting";
			}
		}

		// Token: 0x060037B6 RID: 14262 RVA: 0x000EBEBA File Offset: 0x000EAEBA
		protected override ConfigurationElement CreateNewElement()
		{
			return new SettingElement();
		}

		// Token: 0x060037B7 RID: 14263 RVA: 0x000EBEC1 File Offset: 0x000EAEC1
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SettingElement)element).Key;
		}

		// Token: 0x060037B8 RID: 14264 RVA: 0x000EBECE File Offset: 0x000EAECE
		public SettingElement Get(string elementKey)
		{
			return (SettingElement)base.BaseGet(elementKey);
		}

		// Token: 0x060037B9 RID: 14265 RVA: 0x000EBEDC File Offset: 0x000EAEDC
		public void Add(SettingElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x060037BA RID: 14266 RVA: 0x000EBEE5 File Offset: 0x000EAEE5
		public void Remove(SettingElement element)
		{
			base.BaseRemove(this.GetElementKey(element));
		}

		// Token: 0x060037BB RID: 14267 RVA: 0x000EBEF4 File Offset: 0x000EAEF4
		public void Clear()
		{
			base.BaseClear();
		}
	}
}
