using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000642 RID: 1602
	[ConfigurationCollection(typeof(AuthenticationModuleElement))]
	public sealed class AuthenticationModuleElementCollection : ConfigurationElementCollection
	{
		// Token: 0x17000B54 RID: 2900
		public AuthenticationModuleElement this[int index]
		{
			get
			{
				return (AuthenticationModuleElement)base.BaseGet(index);
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

		// Token: 0x17000B55 RID: 2901
		public AuthenticationModuleElement this[string name]
		{
			get
			{
				return (AuthenticationModuleElement)base.BaseGet(name);
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

		// Token: 0x0600319C RID: 12700 RVA: 0x000D45FC File Offset: 0x000D35FC
		public void Add(AuthenticationModuleElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x000D4605 File Offset: 0x000D3605
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x000D460D File Offset: 0x000D360D
		protected override ConfigurationElement CreateNewElement()
		{
			return new AuthenticationModuleElement();
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x000D4614 File Offset: 0x000D3614
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return ((AuthenticationModuleElement)element).Key;
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x000D462F File Offset: 0x000D362F
		public int IndexOf(AuthenticationModuleElement element)
		{
			return base.BaseIndexOf(element);
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x000D4638 File Offset: 0x000D3638
		public void Remove(AuthenticationModuleElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(element.Key);
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x000D4654 File Offset: 0x000D3654
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x000D465D File Offset: 0x000D365D
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}
	}
}
