using System;
using System.Security.Permissions;

namespace System.Web.SessionState
{
	// Token: 0x02000367 RID: 871
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface ISessionIDManager
	{
		// Token: 0x06002A26 RID: 10790
		bool InitializeRequest(HttpContext context, bool suppressAutoDetectRedirect, out bool supportSessionIDReissue);

		// Token: 0x06002A27 RID: 10791
		string GetSessionID(HttpContext context);

		// Token: 0x06002A28 RID: 10792
		string CreateSessionID(HttpContext context);

		// Token: 0x06002A29 RID: 10793
		void SaveSessionID(HttpContext context, string id, out bool redirected, out bool cookieAdded);

		// Token: 0x06002A2A RID: 10794
		void RemoveSessionID(HttpContext context);

		// Token: 0x06002A2B RID: 10795
		bool Validate(string id);

		// Token: 0x06002A2C RID: 10796
		void Initialize();
	}
}
