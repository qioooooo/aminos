using System;
using System.Net;

namespace Microsoft.Win32
{
	// Token: 0x0200037B RID: 891
	public class IntranetZoneCredentialPolicy : ICredentialPolicy
	{
		// Token: 0x06001BE2 RID: 7138 RVA: 0x0006951B File Offset: 0x0006851B
		public IntranetZoneCredentialPolicy()
		{
			ExceptionHelper.ControlPolicyPermission.Demand();
			this._ManagerRef = (IInternetSecurityManager)new InternetSecurityManager();
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x00069540 File Offset: 0x00068540
		public virtual bool ShouldSendCredential(Uri challengeUri, WebRequest request, NetworkCredential credential, IAuthenticationModule authModule)
		{
			int num;
			this._ManagerRef.MapUrlToZone(challengeUri.AbsoluteUri, out num, 0);
			return num == 1;
		}

		// Token: 0x04001C83 RID: 7299
		private const int URLZONE_INTRANET = 1;

		// Token: 0x04001C84 RID: 7300
		private IInternetSecurityManager _ManagerRef;
	}
}
