using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000646 RID: 1606
	[ConfigurationCollection(typeof(BypassElement))]
	public sealed class BypassElementCollection : ConfigurationElementCollection
	{
		// Token: 0x17000B5D RID: 2909
		public BypassElement this[int index]
		{
			get
			{
				return (BypassElement)base.BaseGet(index);
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

		// Token: 0x17000B5E RID: 2910
		public BypassElement this[string name]
		{
			get
			{
				return (BypassElement)base.BaseGet(name);
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

		// Token: 0x060031B8 RID: 12728 RVA: 0x000D4A78 File Offset: 0x000D3A78
		public void Add(BypassElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x060031B9 RID: 12729 RVA: 0x000D4A81 File Offset: 0x000D3A81
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x060031BA RID: 12730 RVA: 0x000D4A89 File Offset: 0x000D3A89
		protected override ConfigurationElement CreateNewElement()
		{
			return new BypassElement();
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x000D4A90 File Offset: 0x000D3A90
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return ((BypassElement)element).Key;
		}

		// Token: 0x060031BC RID: 12732 RVA: 0x000D4AAB File Offset: 0x000D3AAB
		public int IndexOf(BypassElement element)
		{
			return base.BaseIndexOf(element);
		}

		// Token: 0x060031BD RID: 12733 RVA: 0x000D4AB4 File Offset: 0x000D3AB4
		public void Remove(BypassElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(element.Key);
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x000D4AD0 File Offset: 0x000D3AD0
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x000D4AD9 File Offset: 0x000D3AD9
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x060031C0 RID: 12736 RVA: 0x000D4AE2 File Offset: 0x000D3AE2
		protected override bool ThrowOnDuplicate
		{
			get
			{
				return false;
			}
		}
	}
}
