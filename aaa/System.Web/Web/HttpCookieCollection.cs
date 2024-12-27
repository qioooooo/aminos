using System;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000068 RID: 104
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpCookieCollection : NameObjectCollectionBase
	{
		// Token: 0x0600047A RID: 1146 RVA: 0x00013652 File Offset: 0x00012652
		internal HttpCookieCollection(HttpResponse response, bool readOnly)
			: base(StringComparer.OrdinalIgnoreCase)
		{
			this._response = response;
			base.IsReadOnly = readOnly;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0001366D File Offset: 0x0001266D
		public HttpCookieCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x0001367A File Offset: 0x0001267A
		// (set) Token: 0x0600047D RID: 1149 RVA: 0x00013682 File Offset: 0x00012682
		internal bool Changed
		{
			get
			{
				return this._changed;
			}
			set
			{
				this._changed = value;
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0001368C File Offset: 0x0001268C
		internal void AddCookie(HttpCookie cookie, bool append)
		{
			this.ThrowIfMaxHttpCollectionKeysExceeded();
			this._all = null;
			this._allKeys = null;
			if (append)
			{
				cookie.Added = true;
				base.BaseAdd(cookie.Name, cookie);
				return;
			}
			if (base.BaseGet(cookie.Name) != null)
			{
				cookie.Changed = true;
			}
			base.BaseSet(cookie.Name, cookie);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x000136E7 File Offset: 0x000126E7
		private void ThrowIfMaxHttpCollectionKeysExceeded()
		{
			if (this.Count >= AppSettings.MaxHttpCollectionKeys)
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x000136FC File Offset: 0x000126FC
		internal void RemoveCookie(string name)
		{
			this._all = null;
			this._allKeys = null;
			base.BaseRemove(name);
			this._changed = true;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0001371A File Offset: 0x0001271A
		internal void Reset()
		{
			this._all = null;
			this._allKeys = null;
			base.BaseClear();
			this._changed = true;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00013737 File Offset: 0x00012737
		public void Add(HttpCookie cookie)
		{
			if (this._response != null)
			{
				this._response.BeforeCookieCollectionChange();
			}
			this.AddCookie(cookie, true);
			if (this._response != null)
			{
				this._response.OnCookieAdd(cookie);
			}
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00013768 File Offset: 0x00012768
		public void CopyTo(Array dest, int index)
		{
			if (this._all == null)
			{
				int count = this.Count;
				this._all = new HttpCookie[count];
				for (int i = 0; i < count; i++)
				{
					this._all[i] = this.Get(i);
				}
			}
			this._all.CopyTo(dest, index);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x000137B8 File Offset: 0x000127B8
		public void Set(HttpCookie cookie)
		{
			if (this._response != null)
			{
				this._response.BeforeCookieCollectionChange();
			}
			this.AddCookie(cookie, false);
			if (this._response != null)
			{
				this._response.OnCookieCollectionChange();
			}
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x000137E8 File Offset: 0x000127E8
		public void Remove(string name)
		{
			if (this._response != null)
			{
				this._response.BeforeCookieCollectionChange();
			}
			this.RemoveCookie(name);
			if (this._response != null)
			{
				this._response.OnCookieCollectionChange();
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00013817 File Offset: 0x00012817
		public void Clear()
		{
			this.Reset();
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00013820 File Offset: 0x00012820
		public HttpCookie Get(string name)
		{
			HttpCookie httpCookie = (HttpCookie)base.BaseGet(name);
			if (httpCookie == null && this._response != null)
			{
				httpCookie = new HttpCookie(name);
				this.AddCookie(httpCookie, true);
				this._response.OnCookieAdd(httpCookie);
			}
			return httpCookie;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00013861 File Offset: 0x00012861
		internal HttpCookie GetNoCreate(string name)
		{
			return (HttpCookie)base.BaseGet(name);
		}

		// Token: 0x170001C7 RID: 455
		public HttpCookie this[string name]
		{
			get
			{
				return this.Get(name);
			}
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00013878 File Offset: 0x00012878
		public HttpCookie Get(int index)
		{
			return (HttpCookie)base.BaseGet(index);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00013886 File Offset: 0x00012886
		public string GetKey(int index)
		{
			return base.BaseGetKey(index);
		}

		// Token: 0x170001C8 RID: 456
		public HttpCookie this[int index]
		{
			get
			{
				return this.Get(index);
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x00013898 File Offset: 0x00012898
		public string[] AllKeys
		{
			get
			{
				if (this._allKeys == null)
				{
					this._allKeys = base.BaseGetAllKeys();
				}
				return this._allKeys;
			}
		}

		// Token: 0x04001030 RID: 4144
		private HttpResponse _response;

		// Token: 0x04001031 RID: 4145
		private HttpCookie[] _all;

		// Token: 0x04001032 RID: 4146
		private string[] _allKeys;

		// Token: 0x04001033 RID: 4147
		private bool _changed;
	}
}
