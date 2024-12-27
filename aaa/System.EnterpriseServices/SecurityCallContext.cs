using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200004F RID: 79
	public sealed class SecurityCallContext
	{
		// Token: 0x06000197 RID: 407 RVA: 0x000062C7 File Offset: 0x000052C7
		private SecurityCallContext()
		{
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000062CF File Offset: 0x000052CF
		private SecurityCallContext(ISecurityCallContext ctx)
		{
			this._ex = ctx;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000199 RID: 409 RVA: 0x000062E0 File Offset: 0x000052E0
		public static SecurityCallContext CurrentCall
		{
			get
			{
				Platform.Assert(Platform.W2K, "SecurityCallContext");
				SecurityCallContext securityCallContext2;
				try
				{
					ISecurityCallContext securityCallContext;
					Util.CoGetCallContext(Util.IID_ISecurityCallContext, out securityCallContext);
					securityCallContext2 = new SecurityCallContext(securityCallContext);
				}
				catch (InvalidCastException)
				{
					throw new COMException(Resource.FormatString("Err_NoSecurityContext"), Util.E_NOINTERFACE);
				}
				return securityCallContext2;
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00006338 File Offset: 0x00005338
		public bool IsCallerInRole(string role)
		{
			return this._ex.IsCallerInRole(role);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00006348 File Offset: 0x00005348
		public bool IsUserInRole(string user, string role)
		{
			object obj = user;
			return this._ex.IsUserInRole(ref obj, role);
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00006365 File Offset: 0x00005365
		public bool IsSecurityEnabled
		{
			get
			{
				return this._ex.IsSecurityEnabled();
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00006374 File Offset: 0x00005374
		public SecurityIdentity DirectCaller
		{
			get
			{
				ISecurityIdentityColl securityIdentityColl = (ISecurityIdentityColl)this._ex.GetItem("DirectCaller");
				return new SecurityIdentity(securityIdentityColl);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600019E RID: 414 RVA: 0x000063A0 File Offset: 0x000053A0
		public SecurityIdentity OriginalCaller
		{
			get
			{
				ISecurityIdentityColl securityIdentityColl = (ISecurityIdentityColl)this._ex.GetItem("OriginalCaller");
				return new SecurityIdentity(securityIdentityColl);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600019F RID: 415 RVA: 0x000063C9 File Offset: 0x000053C9
		public int NumCallers
		{
			get
			{
				return (int)this._ex.GetItem("NumCallers");
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x000063E0 File Offset: 0x000053E0
		public int MinAuthenticationLevel
		{
			get
			{
				return (int)this._ex.GetItem("MinAuthenticationLevel");
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x000063F8 File Offset: 0x000053F8
		public SecurityCallers Callers
		{
			get
			{
				ISecurityCallersColl securityCallersColl = (ISecurityCallersColl)this._ex.GetItem("Callers");
				return new SecurityCallers(securityCallersColl);
			}
		}

		// Token: 0x0400009C RID: 156
		private ISecurityCallContext _ex;
	}
}
