using System;
using System.Configuration;

namespace System.Xml.Serialization.Configuration
{
	// Token: 0x02000352 RID: 850
	[ConfigurationCollection(typeof(SchemaImporterExtensionElement))]
	public sealed class SchemaImporterExtensionElementCollection : ConfigurationElementCollection
	{
		// Token: 0x170009C1 RID: 2497
		public SchemaImporterExtensionElement this[int index]
		{
			get
			{
				return (SchemaImporterExtensionElement)base.BaseGet(index);
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

		// Token: 0x170009C2 RID: 2498
		public SchemaImporterExtensionElement this[string name]
		{
			get
			{
				return (SchemaImporterExtensionElement)base.BaseGet(name);
			}
			set
			{
				if (base.BaseGet(name) != null)
				{
					base.BaseRemove(name);
				}
				this.BaseAdd(value);
			}
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x000D34F7 File Offset: 0x000D24F7
		public void Add(SchemaImporterExtensionElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x000D3500 File Offset: 0x000D2500
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x000D3508 File Offset: 0x000D2508
		protected override ConfigurationElement CreateNewElement()
		{
			return new SchemaImporterExtensionElement();
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x000D350F File Offset: 0x000D250F
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SchemaImporterExtensionElement)element).Key;
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x000D351C File Offset: 0x000D251C
		public int IndexOf(SchemaImporterExtensionElement element)
		{
			return base.BaseIndexOf(element);
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x000D3525 File Offset: 0x000D2525
		public void Remove(SchemaImporterExtensionElement element)
		{
			base.BaseRemove(element.Key);
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x000D3533 File Offset: 0x000D2533
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x000D353C File Offset: 0x000D253C
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}
	}
}
