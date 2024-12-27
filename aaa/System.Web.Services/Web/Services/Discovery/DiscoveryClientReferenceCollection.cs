using System;
using System.Collections;

namespace System.Web.Services.Discovery
{
	// Token: 0x0200009E RID: 158
	public sealed class DiscoveryClientReferenceCollection : DictionaryBase
	{
		// Token: 0x17000123 RID: 291
		public DiscoveryReference this[string url]
		{
			get
			{
				return (DiscoveryReference)base.Dictionary[url];
			}
			set
			{
				base.Dictionary[url] = value;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x00014D50 File Offset: 0x00013D50
		public ICollection Keys
		{
			get
			{
				return base.Dictionary.Keys;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x00014D5D File Offset: 0x00013D5D
		public ICollection Values
		{
			get
			{
				return base.Dictionary.Values;
			}
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00014D6A File Offset: 0x00013D6A
		public void Add(DiscoveryReference value)
		{
			this.Add(value.Url, value);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00014D79 File Offset: 0x00013D79
		public void Add(string url, DiscoveryReference value)
		{
			base.Dictionary.Add(url, value);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00014D88 File Offset: 0x00013D88
		public bool Contains(string url)
		{
			return base.Dictionary.Contains(url);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00014D96 File Offset: 0x00013D96
		public void Remove(string url)
		{
			base.Dictionary.Remove(url);
		}
	}
}
