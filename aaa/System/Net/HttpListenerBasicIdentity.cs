using System;
using System.Security.Principal;

namespace System.Net
{
	// Token: 0x020003C6 RID: 966
	public class HttpListenerBasicIdentity : GenericIdentity
	{
		// Token: 0x06001E60 RID: 7776 RVA: 0x000745D9 File Offset: 0x000735D9
		public HttpListenerBasicIdentity(string username, string password)
			: base(username, "Basic")
		{
			this.m_Password = password;
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001E61 RID: 7777 RVA: 0x000745EE File Offset: 0x000735EE
		public virtual string Password
		{
			get
			{
				return this.m_Password;
			}
		}

		// Token: 0x04001E45 RID: 7749
		private string m_Password;
	}
}
