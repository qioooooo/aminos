using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x020006D0 RID: 1744
	internal class SmtpNegotiateAuthenticationModule : ISmtpAuthenticationModule
	{
		// Token: 0x060035DF RID: 13791 RVA: 0x000E5C7D File Offset: 0x000E4C7D
		internal SmtpNegotiateAuthenticationModule()
		{
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x000E5C90 File Offset: 0x000E4C90
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
						ntauthentication = (this.sessions[sessionCookie] = new NTAuthentication(false, "Negotiate", credential, spn, ContextFlags.Connection | ContextFlags.AcceptStream, channelBindingToken));
					}
					string text = null;
					if (!ntauthentication.IsCompleted)
					{
						byte[] array = null;
						if (challenge != null)
						{
							array = Convert.FromBase64String(challenge);
						}
						SecurityStatus securityStatus;
						byte[] outgoingBlob = ntauthentication.GetOutgoingBlob(array, false, out securityStatus);
						if (ntauthentication.IsCompleted && outgoingBlob == null)
						{
							text = "\r\n";
						}
						if (outgoingBlob != null)
						{
							text = Convert.ToBase64String(outgoingBlob);
						}
					}
					else
					{
						text = this.GetSecurityLayerOutgoingBlob(challenge, ntauthentication);
					}
					authorization = new Authorization(text, ntauthentication.IsCompleted);
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

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x060035E1 RID: 13793 RVA: 0x000E5DA0 File Offset: 0x000E4DA0
		public string AuthenticationType
		{
			get
			{
				return "gssapi";
			}
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x000E5DA8 File Offset: 0x000E4DA8
		public void CloseContext(object sessionCookie)
		{
			NTAuthentication ntauthentication = null;
			lock (this.sessions)
			{
				ntauthentication = this.sessions[sessionCookie] as NTAuthentication;
				if (ntauthentication != null)
				{
					this.sessions.Remove(sessionCookie);
				}
			}
			if (ntauthentication != null)
			{
				ntauthentication.CloseContext();
			}
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x000E5E08 File Offset: 0x000E4E08
		private string GetSecurityLayerOutgoingBlob(string challenge, NTAuthentication clientContext)
		{
			if (challenge == null)
			{
				return null;
			}
			byte[] array = Convert.FromBase64String(challenge);
			int num;
			try
			{
				num = clientContext.VerifySignature(array, 0, array.Length);
			}
			catch (Win32Exception)
			{
				return null;
			}
			if (num < 4 || array[0] != 1 || array[1] != 0 || array[2] != 0 || array[3] != 0)
			{
				return null;
			}
			byte[] array2 = null;
			try
			{
				num = clientContext.MakeSignature(array, 0, 4, ref array2);
			}
			catch (Win32Exception)
			{
				return null;
			}
			return Convert.ToBase64String(array2, 0, num);
		}

		// Token: 0x0400310D RID: 12557
		private Hashtable sessions = new Hashtable();
	}
}
