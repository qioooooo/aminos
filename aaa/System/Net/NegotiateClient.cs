using System;
using System.Globalization;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x020004F1 RID: 1265
	internal class NegotiateClient : ISessionAuthenticationModule, IAuthenticationModule
	{
		// Token: 0x06002795 RID: 10133 RVA: 0x000A2D9C File Offset: 0x000A1D9C
		public NegotiateClient()
		{
			if (!ComNetOS.IsWin2K)
			{
				throw new PlatformNotSupportedException(SR.GetString("Win2000Required"));
			}
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x000A2DBB File Offset: 0x000A1DBB
		public Authorization Authenticate(string challenge, WebRequest webRequest, ICredentials credentials)
		{
			return this.DoAuthenticate(challenge, webRequest, credentials, false);
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x000A2DC8 File Offset: 0x000A1DC8
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
				int num = AuthenticationManager.FindSubstringNotInQuotes(challenge, NegotiateClient.Signature);
				if (num < 0)
				{
					return null;
				}
				int num2 = num + NegotiateClient.SignatureSize;
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
				NetworkCredential credential = credentials.GetCredential(httpWebRequest.ChallengedUri, NegotiateClient.Signature);
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
				ntauthentication = new NTAuthentication("Negotiate", credential, computeSpn, httpWebRequest, channelBinding);
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
			httpWebRequest.NtlmKeepAlive = text == null && ntauthentication.IsValidContext && !ntauthentication.IsKerberos;
			return AuthenticationManager.GetGroupAuthorization(this, "Negotiate " + outgoingBlob, ntauthentication.IsCompleted, ntauthentication, unsafeOrProxyAuthenticatedConnectionSharing, ntauthentication.IsKerberos);
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06002798 RID: 10136 RVA: 0x000A2FA1 File Offset: 0x000A1FA1
		public bool CanPreAuthenticate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x000A2FA4 File Offset: 0x000A1FA4
		public Authorization PreAuthenticate(WebRequest webRequest, ICredentials credentials)
		{
			return this.DoAuthenticate(null, webRequest, credentials, true);
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x0600279A RID: 10138 RVA: 0x000A2FB0 File Offset: 0x000A1FB0
		public string AuthenticationType
		{
			get
			{
				return "Negotiate";
			}
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x000A2FB8 File Offset: 0x000A1FB8
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
			if (!httpWebRequest.UnsafeOrProxyAuthenticatedConnectionSharing)
			{
				httpWebRequest.ServicePoint.ReleaseConnectionGroup(httpWebRequest.GetConnectionGroupLine());
			}
			int num = ((challenge == null) ? (-1) : AuthenticationManager.FindSubstringNotInQuotes(challenge, NegotiateClient.Signature));
			if (num >= 0)
			{
				int num2 = num + NegotiateClient.SignatureSize;
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

		// Token: 0x0600279C RID: 10140 RVA: 0x000A30A8 File Offset: 0x000A20A8
		public void ClearSession(WebRequest webRequest)
		{
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			httpWebRequest.CurrentAuthenticationState.ClearSession();
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x0600279D RID: 10141 RVA: 0x000A30C7 File Offset: 0x000A20C7
		public bool CanUseDefaultCredentials
		{
			get
			{
				return true;
			}
		}

		// Token: 0x040026BB RID: 9915
		internal const string AuthType = "Negotiate";

		// Token: 0x040026BC RID: 9916
		internal static string Signature = "Negotiate".ToLower(CultureInfo.InvariantCulture);

		// Token: 0x040026BD RID: 9917
		internal static int SignatureSize = NegotiateClient.Signature.Length;
	}
}
