using System;
using System.Collections;

namespace System.Web.Services.Description
{
	// Token: 0x02000128 RID: 296
	public sealed class WebReferenceCollection : CollectionBase
	{
		// Token: 0x17000260 RID: 608
		public WebReference this[int index]
		{
			get
			{
				return (WebReference)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x000426BF File Offset: 0x000416BF
		public int Add(WebReference webReference)
		{
			return base.List.Add(webReference);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x000426CD File Offset: 0x000416CD
		public void Insert(int index, WebReference webReference)
		{
			base.List.Insert(index, webReference);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x000426DC File Offset: 0x000416DC
		public int IndexOf(WebReference webReference)
		{
			return base.List.IndexOf(webReference);
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x000426EA File Offset: 0x000416EA
		public bool Contains(WebReference webReference)
		{
			return base.List.Contains(webReference);
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x000426F8 File Offset: 0x000416F8
		public void Remove(WebReference webReference)
		{
			base.List.Remove(webReference);
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x00042706 File Offset: 0x00041706
		public void CopyTo(WebReference[] array, int index)
		{
			base.List.CopyTo(array, index);
		}
	}
}
