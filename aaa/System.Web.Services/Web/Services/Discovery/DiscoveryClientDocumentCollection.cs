using System;
using System.Collections;

namespace System.Web.Services.Discovery
{
	// Token: 0x02000099 RID: 153
	public sealed class DiscoveryClientDocumentCollection : DictionaryBase
	{
		// Token: 0x17000116 RID: 278
		public object this[string url]
		{
			get
			{
				return base.Dictionary[url];
			}
			set
			{
				base.Dictionary[url] = value;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x00013C47 File Offset: 0x00012C47
		public ICollection Keys
		{
			get
			{
				return base.Dictionary.Keys;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x00013C54 File Offset: 0x00012C54
		public ICollection Values
		{
			get
			{
				return base.Dictionary.Values;
			}
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00013C61 File Offset: 0x00012C61
		public void Add(string url, object value)
		{
			base.Dictionary.Add(url, value);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00013C70 File Offset: 0x00012C70
		public bool Contains(string url)
		{
			return base.Dictionary.Contains(url);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00013C7E File Offset: 0x00012C7E
		public void Remove(string url)
		{
			base.Dictionary.Remove(url);
		}
	}
}
