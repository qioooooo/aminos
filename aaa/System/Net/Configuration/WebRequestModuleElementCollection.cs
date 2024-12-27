using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200066E RID: 1646
	[ConfigurationCollection(typeof(WebRequestModuleElement))]
	public sealed class WebRequestModuleElementCollection : ConfigurationElementCollection
	{
		// Token: 0x17000BF0 RID: 3056
		public WebRequestModuleElement this[int index]
		{
			get
			{
				return (WebRequestModuleElement)base.BaseGet(index);
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

		// Token: 0x17000BF1 RID: 3057
		public WebRequestModuleElement this[string name]
		{
			get
			{
				return (WebRequestModuleElement)base.BaseGet(name);
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

		// Token: 0x060032DE RID: 13022 RVA: 0x000D7763 File Offset: 0x000D6763
		public void Add(WebRequestModuleElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x060032DF RID: 13023 RVA: 0x000D776C File Offset: 0x000D676C
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x060032E0 RID: 13024 RVA: 0x000D7774 File Offset: 0x000D6774
		protected override ConfigurationElement CreateNewElement()
		{
			return new WebRequestModuleElement();
		}

		// Token: 0x060032E1 RID: 13025 RVA: 0x000D777B File Offset: 0x000D677B
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return ((WebRequestModuleElement)element).Key;
		}

		// Token: 0x060032E2 RID: 13026 RVA: 0x000D7796 File Offset: 0x000D6796
		public int IndexOf(WebRequestModuleElement element)
		{
			return base.BaseIndexOf(element);
		}

		// Token: 0x060032E3 RID: 13027 RVA: 0x000D779F File Offset: 0x000D679F
		public void Remove(WebRequestModuleElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(element.Key);
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x000D77BB File Offset: 0x000D67BB
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x000D77C4 File Offset: 0x000D67C4
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}
	}
}
