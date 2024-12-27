using System;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000072 RID: 114
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpFileCollection : NameObjectCollectionBase
	{
		// Token: 0x060004E6 RID: 1254 RVA: 0x00014404 File Offset: 0x00013404
		internal HttpFileCollection()
			: base(Misc.CaseInsensitiveInvariantKeyComparer)
		{
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00014414 File Offset: 0x00013414
		public void CopyTo(Array dest, int index)
		{
			if (this._all == null)
			{
				int count = this.Count;
				this._all = new HttpPostedFile[count];
				for (int i = 0; i < count; i++)
				{
					this._all[i] = this.Get(i);
				}
			}
			if (this._all != null)
			{
				this._all.CopyTo(dest, index);
			}
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001446C File Offset: 0x0001346C
		internal void AddFile(string key, HttpPostedFile file)
		{
			this.ThrowIfMaxHttpCollectionKeysExceeded();
			this._all = null;
			this._allKeys = null;
			base.BaseAdd(key, file);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001448A File Offset: 0x0001348A
		private void ThrowIfMaxHttpCollectionKeysExceeded()
		{
			if (this.Count >= AppSettings.MaxHttpCollectionKeys)
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001449F File Offset: 0x0001349F
		public HttpPostedFile Get(string name)
		{
			return (HttpPostedFile)base.BaseGet(name);
		}

		// Token: 0x170001DD RID: 477
		public HttpPostedFile this[string name]
		{
			get
			{
				return this.Get(name);
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x000144B6 File Offset: 0x000134B6
		public HttpPostedFile Get(int index)
		{
			return (HttpPostedFile)base.BaseGet(index);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x000144C4 File Offset: 0x000134C4
		public string GetKey(int index)
		{
			return base.BaseGetKey(index);
		}

		// Token: 0x170001DE RID: 478
		public HttpPostedFile this[int index]
		{
			get
			{
				return this.Get(index);
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x000144D6 File Offset: 0x000134D6
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

		// Token: 0x04001045 RID: 4165
		private HttpPostedFile[] _all;

		// Token: 0x04001046 RID: 4166
		private string[] _allKeys;
	}
}
