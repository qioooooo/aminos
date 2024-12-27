using System;
using System.ComponentModel;
using System.Runtime.Remoting.Channels;
using System.Security.Principal;
using System.Web;
using System.Web.SessionState;

namespace System.Runtime.Remoting.Services
{
	// Token: 0x020000B7 RID: 183
	public class RemotingService : Component
	{
		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x000200E7 File Offset: 0x0001F0E7
		public HttpApplicationState Application
		{
			get
			{
				return this.Context.Application;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x000200F4 File Offset: 0x0001F0F4
		public HttpContext Context
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				if (httpContext == null)
				{
					throw new RemotingException(CoreChannel.GetResourceString("Remoting_HttpContextNotAvailable"));
				}
				return httpContext;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x0002011B File Offset: 0x0001F11B
		public HttpSessionState Session
		{
			get
			{
				return this.Context.Session;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x00020128 File Offset: 0x0001F128
		public HttpServerUtility Server
		{
			get
			{
				return this.Context.Server;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x00020135 File Offset: 0x0001F135
		public IPrincipal User
		{
			get
			{
				return this.Context.User;
			}
		}
	}
}
