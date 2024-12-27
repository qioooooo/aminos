using System;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Web.Security
{
	// Token: 0x0200033C RID: 828
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public class FormsIdentity : IIdentity
	{
		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06002868 RID: 10344 RVA: 0x000B1D41 File Offset: 0x000B0D41
		public string Name
		{
			get
			{
				return this._Ticket.Name;
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06002869 RID: 10345 RVA: 0x000B1D4E File Offset: 0x000B0D4E
		public string AuthenticationType
		{
			get
			{
				return "Forms";
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x0600286A RID: 10346 RVA: 0x000B1D55 File Offset: 0x000B0D55
		public bool IsAuthenticated
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x0600286B RID: 10347 RVA: 0x000B1D58 File Offset: 0x000B0D58
		public FormsAuthenticationTicket Ticket
		{
			get
			{
				return this._Ticket;
			}
		}

		// Token: 0x0600286C RID: 10348 RVA: 0x000B1D60 File Offset: 0x000B0D60
		public FormsIdentity(FormsAuthenticationTicket ticket)
		{
			this._Ticket = ticket;
		}

		// Token: 0x04001EB0 RID: 7856
		private FormsAuthenticationTicket _Ticket;
	}
}
