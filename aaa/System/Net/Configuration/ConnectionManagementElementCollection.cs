using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200064A RID: 1610
	[ConfigurationCollection(typeof(ConnectionManagementElement))]
	public sealed class ConnectionManagementElementCollection : ConfigurationElementCollection
	{
		// Token: 0x17000B6C RID: 2924
		public ConnectionManagementElement this[int index]
		{
			get
			{
				return (ConnectionManagementElement)base.BaseGet(index);
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

		// Token: 0x17000B6D RID: 2925
		public ConnectionManagementElement this[string name]
		{
			get
			{
				return (ConnectionManagementElement)base.BaseGet(name);
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

		// Token: 0x060031DA RID: 12762 RVA: 0x000D4D4B File Offset: 0x000D3D4B
		public void Add(ConnectionManagementElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x060031DB RID: 12763 RVA: 0x000D4D54 File Offset: 0x000D3D54
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x000D4D5C File Offset: 0x000D3D5C
		protected override ConfigurationElement CreateNewElement()
		{
			return new ConnectionManagementElement();
		}

		// Token: 0x060031DD RID: 12765 RVA: 0x000D4D63 File Offset: 0x000D3D63
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return ((ConnectionManagementElement)element).Key;
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x000D4D7E File Offset: 0x000D3D7E
		public int IndexOf(ConnectionManagementElement element)
		{
			return base.BaseIndexOf(element);
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x000D4D87 File Offset: 0x000D3D87
		public void Remove(ConnectionManagementElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(element.Key);
		}

		// Token: 0x060031E0 RID: 12768 RVA: 0x000D4DA3 File Offset: 0x000D3DA3
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x060031E1 RID: 12769 RVA: 0x000D4DAC File Offset: 0x000D3DAC
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}
	}
}
