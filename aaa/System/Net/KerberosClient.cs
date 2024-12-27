using System;
using System.Globalization;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x020004E4 RID: 1252
	internal class KerberosClient : ISessionAuthenticationModule, IAuthenticationModule
	{
		// Token: 0x060026E6 RID: 9958 RVA: 0x000A09DE File Offset: 0x0009F9DE
		internal KerberosClient()
		{
			if (!ComNetOS.IsWin2K)
			{
				throw new PlatformNotSupportedException(SR.GetString("Win2000Required"));
			}
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x000A09FD File Offset: 0x0009F9FD
		public Authorization Authenticate(string challenge, WebRequest webRequest, ICredentials credentials)
		{
			return this.DoAuthenticate(challenge, webRequest, credentials, false);
		}

		// Token: 0x060026E8 RID: 9960 RVA: 0x000A0A0C File Offset: 0x0009FA0C
		private Authorization DoAuthenticate(string challenge, WebRequest webRequest, ICredentials credentials, bool preAuthenticate)
		{
			if (credentials == null)
			{
				return null;
			}
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			NTAuthentication ntauthentication = null;
			string text = null;
			if (!preAuthenticate)
			{
				int num = AuthenticationManager.FindSubstringNotInQuotes(challenge, KerberosClient.Signature);
				if (num < 0)
				{
					return null;
				}
				int num2 = num + KerberosClient.SignatureSize;
				if (challenge.Length > num2 && challenge[num2] != ',')
				{
					num2++;
				}
				else
				{
					num = -1;
				}
				if (num >= 0 && challenge.Length > num2)
				{
					num = challenge.IndexOf(',', num2);
					if (num != -1)
					{
						text = challenge.Substring(num2, num - num2);
					}
					else
					{
						text = challenge.Substring(num2);
					}
				}
				ntauthentication = httpWebRequest.CurrentAuthenticationState.GetSecurityContext(this);
			}
			if (ntauthentication == null)
			{
				NetworkCredential credential = credentials.GetCredential(httpWebRequest.ChallengedUri, KerberosClient.Signature);
				if (credential == null || (!(credential is SystemNetworkCredential) && credential.InternalGetUserName().Length == 0))
				{
					return null;
				}
				ICredentialPolicy credentialPolicy = AuthenticationManager.CredentialPolicy;
				if (credentialPolicy != null && !credentialPolicy.ShouldSendCredential(httpWebRequest.ChallengedUri, httpWebRequest, credential, this))
				{
					return null;
				}
				string computeSpn = httpWebRequest.CurrentAuthenticationState.GetComputeSpn(httpWebRequest);
				ChannelBinding channelBinding = null;
				if (httpWebRequest.CurrentAuthenticationState.TransportContext != null)
				{
					channelBinding = httpWebRequest.CurrentAuthenticationState.TransportContext.GetChannelBinding(ChannelBindingKind.Endpoint);
				}
				ntauthentication = new NTAuthentication("Kerberos", credential, computeSpn, httpWebRequest, channelBinding);
				httpWebRequest.CurrentAuthenticationState.SetSecurityContext(ntauthentication, this);
			}
			string outgoingBlob = ntauthentication.GetOutgoingBlob(text);
			if (outgoingBlob == null)
			{
				return null;
			}
			return new Authorization("Kerberos " + outgoingBlob, ntauthentication.IsCompleted, string.Empty, ntauthentication.IsMutualAuthFlag);
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x060026E9 RID: 9961 RVA: 0x000A0B82 File Offset: 0x0009FB82
		public bool CanPreAuthenticate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060026EA RID: 9962 RVA: 0x000A0B85 File Offset: 0x0009FB85
		public Authorization PreAuthenticate(WebRequest webRequest, ICredentials credentials)
		{
			return this.DoAuthenticate(null, webRequest, credentials, true);
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x060026EB RID: 9963 RVA: 0x000A0B91 File Offset: 0x0009FB91
		public string AuthenticationType
		{
			get
			{
				return "Kerberos";
			}
		}

		// Token: 0x060026EC RID: 9964 RVA: 0x000A0B98 File Offset: 0x0009FB98
		public bool Update(string challenge, WebRequest webRequest)
		{
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			NTAuthentication securityContext = httpWebRequest.CurrentAuthenticationState.GetSecurityContext(this);
			if (securityContext == null)
			{
				return true;
			}
			if (httpWebRequest.CurrentAuthenticationState.StatusCodeMatch == httpWebRequest.ResponseStatusCode)
			{
				return false;
			}
			int num = ((challenge == null) ? (-1) : AuthenticationManager.FindSubstringNotInQuotes(challenge, KerberosClient.Signature));
			if (num >= 0)
			{
				int num2 = num + KerberosClient.SignatureSize;
				string text = null;
				if (challenge.Length > num2 && challenge[num2] != ',')
				{
					num2++;
				}
				else
				{
					num = -1;
				}
				if (num >= 0 && challenge.Length > num2)
				{
					text = challenge.Substring(num2);
				}
				securityContext.GetOutgoingBlob(text);
				httpWebRequest.CurrentAuthenticationState.Authorization.MutuallyAuthenticated = securityContext.IsMutualAuthFlag;
			}
			httpWebRequest.ServicePoint.SetCachedChannelBinding(httpWebRequest.ChallengedUri, securityContext.ChannelBinding);
			this.ClearSession(httpWebRequest);
			return true;
		}

		// Token: 0x060026ED RID: 9965 RVA: 0x000A0C64 File Offset: 0x0009FC64
		public void ClearSession(WebRequest webRequest)
		{
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			httpWebRequest.CurrentAuthenticationState.ClearSession();
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x060026EE RID: 9966 RVA: 0x000A0C83 File Offset: 0x0009FC83
		public bool CanUseDefaultCredentials
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0400268F RID: 9871
		internal const string AuthType = "Kerberos";

		// Token: 0x04002690 RID: 9872
		internal static string Signature = "Kerberos".ToLower(CultureInfo.InvariantCulture);

		// Token: 0x04002691 RID: 9873
		internal static int SignatureSize = KerberosClient.Signature.Length;
	}
}
