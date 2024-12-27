using System;
using System.Globalization;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x020004FA RID: 1274
	internal class NtlmClient : ISessionAuthenticationModule, IAuthenticationModule
	{
		// Token: 0x060027DD RID: 10205 RVA: 0x000A4917 File Offset: 0x000A3917
		public Authorization Authenticate(string challenge, WebRequest webRequest, ICredentials credentials)
		{
			return this.DoAuthenticate(challenge, webRequest, credentials, false);
		}

		// Token: 0x060027DE RID: 10206 RVA: 0x000A4924 File Offset: 0x000A3924
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
				int num = AuthenticationManager.FindSubstringNotInQuotes(challenge, NtlmClient.Signature);
				if (num < 0)
				{
					return null;
				}
				int num2 = num + NtlmClient.SignatureSize;
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
				NetworkCredential credential = credentials.GetCredential(httpWebRequest.ChallengedUri, NtlmClient.Signature);
				string text2 = string.Empty;
				if (credential == null || (!(credential is SystemNetworkCredential) && (text2 = credential.InternalGetUserName()).Length == 0))
				{
					return null;
				}
				if (text2.Length + credential.InternalGetPassword().Length + credential.InternalGetDomain().Length > 527)
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
				ntauthentication = new NTAuthentication("NTLM", credential, computeSpn, httpWebRequest, channelBinding);
				httpWebRequest.CurrentAuthenticationState.SetSecurityContext(ntauthentication, this);
			}
			string outgoingBlob = ntauthentication.GetOutgoingBlob(text);
			if (outgoingBlob == null)
			{
				return null;
			}
			bool unsafeOrProxyAuthenticatedConnectionSharing = httpWebRequest.UnsafeOrProxyAuthenticatedConnectionSharing;
			if (unsafeOrProxyAuthenticatedConnectionSharing)
			{
				httpWebRequest.LockConnection = true;
			}
			httpWebRequest.NtlmKeepAlive = text == null;
			return AuthenticationManager.GetGroupAuthorization(this, "NTLM " + outgoingBlob, ntauthentication.IsCompleted, ntauthentication, unsafeOrProxyAuthenticatedConnectionSharing, false);
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x060027DF RID: 10207 RVA: 0x000A4AE5 File Offset: 0x000A3AE5
		public bool CanPreAuthenticate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x000A4AE8 File Offset: 0x000A3AE8
		public Authorization PreAuthenticate(WebRequest webRequest, ICredentials credentials)
		{
			return this.DoAuthenticate(null, webRequest, credentials, true);
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x060027E1 RID: 10209 RVA: 0x000A4AF4 File Offset: 0x000A3AF4
		public string AuthenticationType
		{
			get
			{
				return "NTLM";
			}
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x000A4AFC File Offset: 0x000A3AFC
		public bool Update(string challenge, WebRequest webRequest)
		{
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			NTAuthentication securityContext = httpWebRequest.CurrentAuthenticationState.GetSecurityContext(this);
			if (securityContext == null)
			{
				return true;
			}
			if (!securityContext.IsCompleted && httpWebRequest.CurrentAuthenticationState.StatusCodeMatch == httpWebRequest.ResponseStatusCode)
			{
				return false;
			}
			this.ClearSession(httpWebRequest);
			if (!httpWebRequest.UnsafeOrProxyAuthenticatedConnectionSharing)
			{
				httpWebRequest.ServicePoint.ReleaseConnectionGroup(httpWebRequest.GetConnectionGroupLine());
			}
			httpWebRequest.ServicePoint.SetCachedChannelBinding(httpWebRequest.ChallengedUri, securityContext.ChannelBinding);
			return true;
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x000A4B78 File Offset: 0x000A3B78
		public void ClearSession(WebRequest webRequest)
		{
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			httpWebRequest.CurrentAuthenticationState.ClearSession();
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x060027E4 RID: 10212 RVA: 0x000A4B97 File Offset: 0x000A3B97
		public bool CanUseDefaultCredentials
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0400270B RID: 9995
		internal const string AuthType = "NTLM";

		// Token: 0x0400270C RID: 9996
		internal const int MaxNtlmCredentialSize = 527;

		// Token: 0x0400270D RID: 9997
		internal static string Signature = "NTLM".ToLower(CultureInfo.InvariantCulture);

		// Token: 0x0400270E RID: 9998
		internal static int SignatureSize = NtlmClient.Signature.Length;
	}
}
