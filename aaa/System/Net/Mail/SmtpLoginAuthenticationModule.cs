using System;
using System.Collections;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x020006CF RID: 1743
	internal class SmtpLoginAuthenticationModule : ISmtpAuthenticationModule
	{
		// Token: 0x060035DB RID: 13787 RVA: 0x000E5B4F File Offset: 0x000E4B4F
		internal SmtpLoginAuthenticationModule()
		{
		}

		// Token: 0x060035DC RID: 13788 RVA: 0x000E5B64 File Offset: 0x000E4B64
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
		public Authorization Authenticate(string challenge, NetworkCredential credential, object sessionCookie, string spn, ChannelBinding channelBindingToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "Authenticate", null);
			}
			Authorization authorization;
			try
			{
				lock (this.sessions)
				{
					NetworkCredential networkCredential = this.sessions[sessionCookie] as NetworkCredential;
					if (networkCredential == null)
					{
						if (credential == null || credential is SystemNetworkCredential)
						{
							authorization = null;
						}
						else
						{
							this.sessions[sessionCookie] = credential;
							string text = credential.UserName;
							string domain = credential.Domain;
							if (domain != null && domain.Length > 0)
							{
								text = domain + "\\" + text;
							}
							authorization = new Authorization(Convert.ToBase64String(Encoding.ASCII.GetBytes(text)), false);
						}
					}
					else
					{
						this.sessions.Remove(sessionCookie);
						authorization = new Authorization(Convert.ToBase64String(Encoding.ASCII.GetBytes(networkCredential.Password)), true);
					}
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "Authenticate", null);
				}
			}
			return authorization;
		}

		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x060035DD RID: 13789 RVA: 0x000E5C74 File Offset: 0x000E4C74
		public string AuthenticationType
		{
			get
			{
				return "login";
			}
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x000E5C7B File Offset: 0x000E4C7B
		public void CloseContext(object sessionCookie)
		{
		}

		// Token: 0x0400310C RID: 12556
		private Hashtable sessions = new Hashtable();
	}
}
