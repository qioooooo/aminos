using System;
using System.Configuration;

namespace System.Security.Authentication.ExtendedProtection.Configuration
{
	// Token: 0x02000350 RID: 848
	public sealed class ServiceNameElementCollection : ConfigurationElementCollection
	{
		// Token: 0x1700051C RID: 1308
		public ServiceNameElement this[int index]
		{
			get
			{
				return (ServiceNameElement)base.BaseGet(index);
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

		// Token: 0x1700051D RID: 1309
		public ServiceNameElement this[string name]
		{
			get
			{
				return (ServiceNameElement)base.BaseGet(name);
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

		// Token: 0x06001A94 RID: 6804 RVA: 0x0005CC52 File Offset: 0x0005BC52
		public void Add(ServiceNameElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x0005CC5B File Offset: 0x0005BC5B
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x0005CC63 File Offset: 0x0005BC63
		protected override ConfigurationElement CreateNewElement()
		{
			return new ServiceNameElement();
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x0005CC6A File Offset: 0x0005BC6A
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return ((ServiceNameElement)element).Key;
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x0005CC85 File Offset: 0x0005BC85
		public int IndexOf(ServiceNameElement element)
		{
			return base.BaseIndexOf(element);
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x0005CC8E File Offset: 0x0005BC8E
		public void Remove(ServiceNameElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(element.Key);
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x0005CCAA File Offset: 0x0005BCAA
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x0005CCB3 File Offset: 0x0005BCB3
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}
	}
}
