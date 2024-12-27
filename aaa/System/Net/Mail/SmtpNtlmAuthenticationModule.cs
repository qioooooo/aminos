using System;
using System.Collections;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x020006D1 RID: 1745
	internal class SmtpNtlmAuthenticationModule : ISmtpAuthenticationModule
	{
		// Token: 0x060035E4 RID: 13796 RVA: 0x000E5E8C File Offset: 0x000E4E8C
		internal SmtpNtlmAuthenticationModule()
		{
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x000E5EA0 File Offset: 0x000E4EA0
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
					NTAuthentication ntauthentication = this.sessions[sessionCookie] as NTAuthentication;
					if (ntauthentication == null)
					{
						if (credential == null)
						{
							return null;
						}
						ntauthentication = (this.sessions[sessionCookie] = new NTAuthentication(false, "Ntlm", credential, spn, ContextFlags.Connection, channelBindingToken));
					}
					string outgoingBlob = ntauthentication.GetOutgoingBlob(challenge);
					if (!ntauthentication.IsCompleted)
					{
						authorization = new Authorization(outgoingBlob, false);
					}
					else
					{
						this.sessions.Remove(sessionCookie);
						authorization = new Authorization(outgoingBlob, true);
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

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x060035E6 RID: 13798 RVA: 0x000E5F80 File Offset: 0x000E4F80
		public string AuthenticationType
		{
			get
			{
				return "ntlm";
			}
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x000E5F87 File Offset: 0x000E4F87
		public void CloseContext(object sessionCookie)
		{
		}

		// Token: 0x0400310E RID: 12558
		private Hashtable sessions = new Hashtable();
	}
}
