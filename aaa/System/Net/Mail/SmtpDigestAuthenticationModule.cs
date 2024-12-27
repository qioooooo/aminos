using System;
using System.Collections;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x020006CB RID: 1739
	internal class SmtpDigestAuthenticationModule : ISmtpAuthenticationModule
	{
		// Token: 0x060035B6 RID: 13750 RVA: 0x000E54B3 File Offset: 0x000E44B3
		internal SmtpDigestAuthenticationModule()
		{
		}

		// Token: 0x060035B7 RID: 13751 RVA: 0x000E54C8 File Offset: 0x000E44C8
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
		public Authorization Authenticate(string challenge, NetworkCredential credential, object sessionCookie, string spn, ChannelBinding channelBindingToken)
		{
			Authorization authorization;
			lock (this.sessions)
			{
				NTAuthentication ntauthentication = this.sessions[sessionCookie] as NTAuthentication;
				if (ntauthentication == null)
				{
					if (credential == null)
					{
						return null;
					}
					ntauthentication = (this.sessions[sessionCookie] = new NTAuthentication(false, "WDigest", credential, spn, ContextFlags.Connection, channelBindingToken));
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
			return authorization;
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x060035B8 RID: 13752 RVA: 0x000E556C File Offset: 0x000E456C
		public string AuthenticationType
		{
			get
			{
				return "WDigest";
			}
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x000E5573 File Offset: 0x000E4573
		public void CloseContext(object sessionCookie)
		{
		}

		// Token: 0x04003107 RID: 12551
		private Hashtable sessions = new Hashtable();
	}
}
