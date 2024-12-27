using System;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000079 RID: 121
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpModuleCollection : NameObjectCollectionBase
	{
		// Token: 0x0600052B RID: 1323 RVA: 0x000152E7 File Offset: 0x000142E7
		internal HttpModuleCollection()
			: base(Misc.CaseInsensitiveInvariantKeyComparer)
		{
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x000152F4 File Offset: 0x000142F4
		public void CopyTo(Array dest, int index)
		{
			if (this._all == null)
			{
				int count = this.Count;
				this._all = new IHttpModule[count];
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

		// Token: 0x0600052D RID: 1325 RVA: 0x0001534C File Offset: 0x0001434C
		internal void AddModule(string name, IHttpModule m)
		{
			this._all = null;
			this._allKeys = null;
			base.BaseAdd(name, m);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00015364 File Offset: 0x00014364
		public IHttpModule Get(string name)
		{
			return (IHttpModule)base.BaseGet(name);
		}

		// Token: 0x170001E7 RID: 487
		public IHttpModule this[string name]
		{
			get
			{
				return this.Get(name);
			}
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0001537B File Offset: 0x0001437B
		public IHttpModule Get(int index)
		{
			return (IHttpModule)base.BaseGet(index);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00015389 File Offset: 0x00014389
		public string GetKey(int index)
		{
			return base.BaseGetKey(index);
		}

		// Token: 0x170001E8 RID: 488
		public IHttpModule this[int index]
		{
			get
			{
				return this.Get(index);
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x0001539B File Offset: 0x0001439B
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

		// Token: 0x04001059 RID: 4185
		private IHttpModule[] _all;

		// Token: 0x0400105A RID: 4186
		private string[] _allKeys;
	}
}
