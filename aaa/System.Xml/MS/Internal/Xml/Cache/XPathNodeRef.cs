using System;

namespace MS.Internal.Xml.Cache
{
	// Token: 0x0200010A RID: 266
	internal struct XPathNodeRef
	{
		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06001058 RID: 4184 RVA: 0x0004AAC8 File Offset: 0x00049AC8
		public static XPathNodeRef Null
		{
			get
			{
				return default(XPathNodeRef);
			}
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x0004AADE File Offset: 0x00049ADE
		public XPathNodeRef(XPathNode[] page, int idx)
		{
			this.page = page;
			this.idx = idx;
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x0600105A RID: 4186 RVA: 0x0004AAEE File Offset: 0x00049AEE
		public bool IsNull
		{
			get
			{
				return this.page == null;
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x0600105B RID: 4187 RVA: 0x0004AAF9 File Offset: 0x00049AF9
		public XPathNode[] Page
		{
			get
			{
				return this.page;
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x0600105C RID: 4188 RVA: 0x0004AB01 File Offset: 0x00049B01
		public int Index
		{
			get
			{
				return this.idx;
			}
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x0004AB09 File Offset: 0x00049B09
		public override int GetHashCode()
		{
			return XPathNodeHelper.GetLocation(this.page, this.idx);
		}

		// Token: 0x04000AB0 RID: 2736
		private XPathNode[] page;

		// Token: 0x04000AB1 RID: 2737
		private int idx;
	}
}
