using System;

namespace System.Net
{
	// Token: 0x020004D4 RID: 1236
	internal interface ISessionAuthenticationModule : IAuthenticationModule
	{
		// Token: 0x0600266C RID: 9836
		bool Update(string challenge, WebRequest webRequest);

		// Token: 0x0600266D RID: 9837
		void ClearSession(WebRequest webRequest);

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x0600266E RID: 9838
		bool CanUseDefaultCredentials { get; }
	}
}
