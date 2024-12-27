using System;
using System.Collections;

namespace System.Web.Services.Discovery
{
	// Token: 0x020000A6 RID: 166
	public sealed class DiscoveryExceptionDictionary : DictionaryBase
	{
		// Token: 0x1700012D RID: 301
		public Exception this[string url]
		{
			get
			{
				return (Exception)base.Dictionary[url];
			}
			set
			{
				base.Dictionary[url] = value;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x0001657F File Offset: 0x0001557F
		public ICollection Keys
		{
			get
			{
				return base.Dictionary.Keys;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x0001658C File Offset: 0x0001558C
		public ICollection Values
		{
			get
			{
				return base.Dictionary.Values;
			}
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00016599 File Offset: 0x00015599
		public void Add(string url, Exception value)
		{
			base.Dictionary.Add(url, value);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000165A8 File Offset: 0x000155A8
		public bool Contains(string url)
		{
			return base.Dictionary.Contains(url);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000165B6 File Offset: 0x000155B6
		public void Remove(string url)
		{
			base.Dictionary.Remove(url);
		}
	}
}
