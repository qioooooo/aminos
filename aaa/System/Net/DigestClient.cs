using System;
using System.Globalization;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x020004D5 RID: 1237
	internal class DigestClient : ISessionAuthenticationModule, IAuthenticationModule
	{
		// Token: 0x06002670 RID: 9840 RVA: 0x0009C4A6 File Offset: 0x0009B4A6
		public Authorization Authenticate(string challenge, WebRequest webRequest, ICredentials credentials)
		{
			return this.DoAuthenticate(challenge, webRequest, credentials, false);
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x0009C4B4 File Offset: 0x0009B4B4
		private Authorization DoAuthenticate(string challenge, WebRequest webRequest, ICredentials credentials, bool preAuthenticate)
		{
			if (credentials == null)
			{
				return null;
			}
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			NetworkCredential credential = credentials.GetCredential(httpWebRequest.ChallengedUri, DigestClient.Signature);
			if (credential is SystemNetworkCredential)
			{
				if (DigestClient.WDigestAvailable)
				{
					return this.XPDoAuthenticate(challenge, httpWebRequest, credentials, preAuthenticate);
				}
				return null;
			}
			else
			{
				HttpDigestChallenge httpDigestChallenge;
				if (!preAuthenticate)
				{
					int num = AuthenticationManager.FindSubstringNotInQuotes(challenge, DigestClient.Signature);
					if (num < 0)
					{
						return null;
					}
					httpDigestChallenge = HttpDigest.Interpret(challenge, num, httpWebRequest);
				}
				else
				{
					httpDigestChallenge = DigestClient.challengeCache.Lookup(httpWebRequest.ChallengedUri.AbsoluteUri) as HttpDigestChallenge;
				}
				if (httpDigestChallenge == null)
				{
					return null;
				}
				if (!DigestClient.CheckQOP(httpDigestChallenge))
				{
					throw new NotSupportedException(SR.GetString("net_QOPNotSupportedException", new object[] { httpDigestChallenge.QualityOfProtection }));
				}
				if (preAuthenticate)
				{
					httpDigestChallenge = httpDigestChallenge.CopyAndIncrementNonce();
					httpDigestChallenge.SetFromRequest(httpWebRequest);
				}
				if (credential == null)
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
				Authorization authorization = HttpDigest.Authenticate(httpDigestChallenge, credential, computeSpn, channelBinding);
				if (!preAuthenticate && authorization != null)
				{
					string[] array = ((httpDigestChallenge.Domain == null) ? new string[] { httpWebRequest.ChallengedUri.GetParts(UriComponents.SchemeAndServer, UriFormat.UriEscaped) } : httpDigestChallenge.Domain.Split(DigestClient.singleSpaceArray));
					authorization.ProtectionRealm = ((httpDigestChallenge.Domain == null) ? null : array);
					for (int i = 0; i < array.Length; i++)
					{
						DigestClient.challengeCache.Add(array[i], httpDigestChallenge);
					}
				}
				return authorization;
			}
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x0009C65B File Offset: 0x0009B65B
		public Authorization PreAuthenticate(WebRequest webRequest, ICredentials credentials)
		{
			return this.DoAuthenticate(null, webRequest, credentials, true);
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06002673 RID: 9843 RVA: 0x0009C667 File Offset: 0x0009B667
		public bool CanPreAuthenticate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06002674 RID: 9844 RVA: 0x0009C66A File Offset: 0x0009B66A
		public string AuthenticationType
		{
			get
			{
				return "Digest";
			}
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x0009C674 File Offset: 0x0009B674
		internal static bool CheckQOP(HttpDigestChallenge challenge)
		{
			if (challenge.QopPresent)
			{
				for (int i = 0; i >= 0; i += "auth".Length)
				{
					i = challenge.QualityOfProtection.IndexOf("auth", i);
					if (i < 0)
					{
						return false;
					}
					if ((i == 0 || ", \"'\t\r\n".IndexOf(challenge.QualityOfProtection[i - 1]) >= 0) && (i + "auth".Length == challenge.QualityOfProtection.Length || ", \"'\t\r\n".IndexOf(challenge.QualityOfProtection[i + "auth".Length]) >= 0))
					{
						break;
					}
				}
			}
			return true;
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x0009C71C File Offset: 0x0009B71C
		public bool Update(string challenge, WebRequest webRequest)
		{
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest.CurrentAuthenticationState.GetSecurityContext(this) != null)
			{
				return this.XPUpdate(challenge, httpWebRequest);
			}
			if (httpWebRequest.ResponseStatusCode != httpWebRequest.CurrentAuthenticationState.StatusCodeMatch)
			{
				ChannelBinding channelBinding = null;
				if (httpWebRequest.CurrentAuthenticationState.TransportContext != null)
				{
					channelBinding = httpWebRequest.CurrentAuthenticationState.TransportContext.GetChannelBinding(ChannelBindingKind.Endpoint);
				}
				httpWebRequest.ServicePoint.SetCachedChannelBinding(httpWebRequest.ChallengedUri, channelBinding);
				return true;
			}
			int num = ((challenge == null) ? (-1) : AuthenticationManager.FindSubstringNotInQuotes(challenge, DigestClient.Signature));
			if (num < 0)
			{
				return true;
			}
			int num2 = num + DigestClient.SignatureSize;
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
				challenge.Substring(num2);
			}
			HttpDigestChallenge httpDigestChallenge = HttpDigest.Interpret(challenge, num, httpWebRequest);
			return httpDigestChallenge == null || !httpDigestChallenge.Stale;
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06002677 RID: 9847 RVA: 0x0009C7FB File Offset: 0x0009B7FB
		public bool CanUseDefaultCredentials
		{
			get
			{
				return DigestClient.WDigestAvailable;
			}
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06002678 RID: 9848 RVA: 0x0009C802 File Offset: 0x0009B802
		internal static bool WDigestAvailable
		{
			get
			{
				return DigestClient._WDigestAvailable;
			}
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x0009C80C File Offset: 0x0009B80C
		public void ClearSession(WebRequest webRequest)
		{
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			httpWebRequest.CurrentAuthenticationState.ClearSession();
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x0009C82C File Offset: 0x0009B82C
		private Authorization XPDoAuthenticate(string challenge, HttpWebRequest httpWebRequest, ICredentials credentials, bool preAuthenticate)
		{
			NTAuthentication ntauthentication = null;
			string text;
			if (!preAuthenticate)
			{
				int num = AuthenticationManager.FindSubstringNotInQuotes(challenge, DigestClient.Signature);
				if (num < 0)
				{
					return null;
				}
				ntauthentication = httpWebRequest.CurrentAuthenticationState.GetSecurityContext(this);
				text = DigestClient.RefineDigestChallenge(challenge, num);
			}
			else
			{
				HttpDigestChallenge httpDigestChallenge = DigestClient.challengeCache.Lookup(httpWebRequest.ChallengedUri.AbsoluteUri) as HttpDigestChallenge;
				if (httpDigestChallenge == null)
				{
					return null;
				}
				httpDigestChallenge = httpDigestChallenge.CopyAndIncrementNonce();
				httpDigestChallenge.SetFromRequest(httpWebRequest);
				text = httpDigestChallenge.ToBlob();
			}
			UriComponents uriComponents;
			if (httpWebRequest.CurrentMethod.ConnectRequest)
			{
				uriComponents = UriComponents.HostAndPort;
			}
			else if (httpWebRequest.UsesProxySemantics)
			{
				uriComponents = UriComponents.HttpRequestUrl;
			}
			else
			{
				uriComponents = UriComponents.PathAndQuery;
			}
			string parts = httpWebRequest.Address.GetParts(uriComponents, UriFormat.UriEscaped);
			if (ntauthentication == null)
			{
				NetworkCredential credential = credentials.GetCredential(httpWebRequest.ChallengedUri, DigestClient.Signature);
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
				ntauthentication = new NTAuthentication("WDigest", credential, computeSpn, httpWebRequest, channelBinding);
				httpWebRequest.CurrentAuthenticationState.SetSecurityContext(ntauthentication, this);
			}
			SecurityStatus securityStatus;
			string outgoingDigestBlob = ntauthentication.GetOutgoingDigestBlob(text, httpWebRequest.CurrentMethod.Name, parts, null, false, true, out securityStatus);
			Authorization authorization = new Authorization("Digest " + outgoingDigestBlob, ntauthentication.IsCompleted, string.Empty, ntauthentication.IsMutualAuthFlag);
			if (!preAuthenticate)
			{
				HttpDigestChallenge httpDigestChallenge2 = HttpDigest.Interpret(text, -1, httpWebRequest);
				string[] array = ((httpDigestChallenge2.Domain == null) ? new string[] { httpWebRequest.ChallengedUri.GetParts(UriComponents.SchemeAndServer, UriFormat.UriEscaped) } : httpDigestChallenge2.Domain.Split(DigestClient.singleSpaceArray));
				authorization.ProtectionRealm = ((httpDigestChallenge2.Domain == null) ? null : array);
				for (int i = 0; i < array.Length; i++)
				{
					DigestClient.challengeCache.Add(array[i], httpDigestChallenge2);
				}
			}
			return authorization;
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x0009CA44 File Offset: 0x0009BA44
		private bool XPUpdate(string challenge, HttpWebRequest httpWebRequest)
		{
			NTAuthentication securityContext = httpWebRequest.CurrentAuthenticationState.GetSecurityContext(this);
			if (securityContext == null)
			{
				return false;
			}
			int num = ((challenge == null) ? (-1) : AuthenticationManager.FindSubstringNotInQuotes(challenge, DigestClient.Signature));
			if (num < 0)
			{
				httpWebRequest.ServicePoint.SetCachedChannelBinding(httpWebRequest.ChallengedUri, securityContext.ChannelBinding);
				this.ClearSession(httpWebRequest);
				return true;
			}
			if (httpWebRequest.ResponseStatusCode != httpWebRequest.CurrentAuthenticationState.StatusCodeMatch)
			{
				httpWebRequest.ServicePoint.SetCachedChannelBinding(httpWebRequest.ChallengedUri, securityContext.ChannelBinding);
				this.ClearSession(httpWebRequest);
				return true;
			}
			string text = DigestClient.RefineDigestChallenge(challenge, num);
			SecurityStatus securityStatus;
			securityContext.GetOutgoingDigestBlob(text, httpWebRequest.CurrentMethod.Name, null, null, false, true, out securityStatus);
			httpWebRequest.CurrentAuthenticationState.Authorization.MutuallyAuthenticated = securityContext.IsMutualAuthFlag;
			return securityContext.IsCompleted;
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x0009CB0C File Offset: 0x0009BB0C
		private static string RefineDigestChallenge(string challenge, int index)
		{
			if (challenge == null || index >= challenge.Length)
			{
				throw new ArgumentOutOfRangeException("challenge", challenge);
			}
			int num = index + DigestClient.SignatureSize;
			if (challenge.Length > num && challenge[num] != ',')
			{
				num++;
			}
			else
			{
				index = -1;
			}
			if (index >= 0 && challenge.Length > num)
			{
				string text = challenge.Substring(num);
				int num2 = 0;
				int num3 = num2;
				bool flag = true;
				HttpDigestChallenge httpDigestChallenge = new HttpDigestChallenge();
				int num4;
				for (;;)
				{
					num4 = num3;
					index = AuthenticationManager.SplitNoQuotes(text, ref num4);
					if (num4 < 0)
					{
						break;
					}
					string text2 = text.Substring(num3, num4 - num3);
					string text3;
					if (index < 0)
					{
						text3 = HttpDigest.unquote(text.Substring(num4 + 1));
					}
					else
					{
						text3 = HttpDigest.unquote(text.Substring(num4 + 1, index - num4 - 1));
					}
					flag = httpDigestChallenge.defineAttribute(text2, text3);
					if (index < 0 || !flag)
					{
						break;
					}
					index = (num3 = index + 1);
				}
				if ((!flag || num4 < 0) && num3 < text.Length)
				{
					text = text.Substring(0, num3 - 1);
				}
				return text;
			}
			throw new ArgumentOutOfRangeException("challenge", challenge);
		}

		// Token: 0x040025EB RID: 9707
		internal const string AuthType = "Digest";

		// Token: 0x040025EC RID: 9708
		internal static string Signature = "Digest".ToLower(CultureInfo.InvariantCulture);

		// Token: 0x040025ED RID: 9709
		internal static int SignatureSize = DigestClient.Signature.Length;

		// Token: 0x040025EE RID: 9710
		private static PrefixLookup challengeCache = new PrefixLookup();

		// Token: 0x040025EF RID: 9711
		private static readonly char[] singleSpaceArray = new char[] { ' ' };

		// Token: 0x040025F0 RID: 9712
		private static bool _WDigestAvailable = SSPIWrapper.GetVerifyPackageInfo(GlobalSSPI.SSPIAuth, "WDigest") != null;
	}
}
