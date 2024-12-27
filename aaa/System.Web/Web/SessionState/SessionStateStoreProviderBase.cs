using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Security.Permissions;

namespace System.Web.SessionState
{
	// Token: 0x0200035C RID: 860
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class SessionStateStoreProviderBase : ProviderBase
	{
		// Token: 0x060029DD RID: 10717
		public abstract void Dispose();

		// Token: 0x060029DE RID: 10718
		public abstract bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback);

		// Token: 0x060029DF RID: 10719
		public abstract void InitializeRequest(HttpContext context);

		// Token: 0x060029E0 RID: 10720
		public abstract SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions);

		// Token: 0x060029E1 RID: 10721
		public abstract SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions);

		// Token: 0x060029E2 RID: 10722
		public abstract void ReleaseItemExclusive(HttpContext context, string id, object lockId);

		// Token: 0x060029E3 RID: 10723
		public abstract void SetAndReleaseItemExclusive(HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem);

		// Token: 0x060029E4 RID: 10724
		public abstract void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item);

		// Token: 0x060029E5 RID: 10725
		public abstract void ResetItemTimeout(HttpContext context, string id);

		// Token: 0x060029E6 RID: 10726
		public abstract SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout);

		// Token: 0x060029E7 RID: 10727
		public abstract void CreateUninitializedItem(HttpContext context, string id, int timeout);

		// Token: 0x060029E8 RID: 10728
		public abstract void EndRequest(HttpContext context);

		// Token: 0x060029E9 RID: 10729 RVA: 0x000BAF4C File Offset: 0x000B9F4C
		internal virtual void Initialize(string name, NameValueCollection config, IPartitionResolver partitionResolver)
		{
		}
	}
}
