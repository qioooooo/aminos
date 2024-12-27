using System;
using System.Security.Permissions;

namespace System.Web.SessionState
{
	// Token: 0x02000363 RID: 867
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class SessionStateStoreData
	{
		// Token: 0x06002A00 RID: 10752 RVA: 0x000BB54F File Offset: 0x000BA54F
		public SessionStateStoreData(ISessionStateItemCollection sessionItems, HttpStaticObjectsCollection staticObjects, int timeout)
		{
			this._sessionItems = sessionItems;
			this._staticObjects = staticObjects;
			this._timeout = timeout;
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06002A01 RID: 10753 RVA: 0x000BB56C File Offset: 0x000BA56C
		public virtual ISessionStateItemCollection Items
		{
			get
			{
				return this._sessionItems;
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06002A02 RID: 10754 RVA: 0x000BB574 File Offset: 0x000BA574
		public virtual HttpStaticObjectsCollection StaticObjects
		{
			get
			{
				return this._staticObjects;
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06002A03 RID: 10755 RVA: 0x000BB57C File Offset: 0x000BA57C
		// (set) Token: 0x06002A04 RID: 10756 RVA: 0x000BB584 File Offset: 0x000BA584
		public virtual int Timeout
		{
			get
			{
				return this._timeout;
			}
			set
			{
				this._timeout = value;
			}
		}

		// Token: 0x04001F37 RID: 7991
		private ISessionStateItemCollection _sessionItems;

		// Token: 0x04001F38 RID: 7992
		private HttpStaticObjectsCollection _staticObjects;

		// Token: 0x04001F39 RID: 7993
		private int _timeout;
	}
}
