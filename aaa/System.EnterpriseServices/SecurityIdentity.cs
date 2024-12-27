using System;

namespace System.EnterpriseServices
{
	// Token: 0x0200004C RID: 76
	public sealed class SecurityIdentity
	{
		// Token: 0x06000188 RID: 392 RVA: 0x0000619B File Offset: 0x0000519B
		private SecurityIdentity()
		{
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000061A3 File Offset: 0x000051A3
		internal SecurityIdentity(ISecurityIdentityColl ifc)
		{
			this._ex = ifc;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600018A RID: 394 RVA: 0x000061B2 File Offset: 0x000051B2
		public string AccountName
		{
			get
			{
				return (string)this._ex.GetItem("AccountName");
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600018B RID: 395 RVA: 0x000061C9 File Offset: 0x000051C9
		public int AuthenticationService
		{
			get
			{
				return (int)this._ex.GetItem("AuthenticationService");
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600018C RID: 396 RVA: 0x000061E0 File Offset: 0x000051E0
		public ImpersonationLevelOption ImpersonationLevel
		{
			get
			{
				return (ImpersonationLevelOption)this._ex.GetItem("ImpersonationLevel");
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600018D RID: 397 RVA: 0x000061F7 File Offset: 0x000051F7
		public AuthenticationOption AuthenticationLevel
		{
			get
			{
				return (AuthenticationOption)this._ex.GetItem("AuthenticationLevel");
			}
		}

		// Token: 0x04000098 RID: 152
		private ISecurityIdentityColl _ex;
	}
}
