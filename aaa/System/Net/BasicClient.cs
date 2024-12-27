using System;
using System.Globalization;
using System.Text;

namespace System.Net
{
	// Token: 0x020004B6 RID: 1206
	internal class BasicClient : IAuthenticationModule
	{
		// Token: 0x0600254E RID: 9550 RVA: 0x00094D28 File Offset: 0x00093D28
		public Authorization Authenticate(string challenge, WebRequest webRequest, ICredentials credentials)
		{
			if (credentials == null || credentials is SystemNetworkCredential)
			{
				return null;
			}
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest == null || httpWebRequest.ChallengedUri == null)
			{
				return null;
			}
			int num = AuthenticationManager.FindSubstringNotInQuotes(challenge, BasicClient.Signature);
			if (num < 0)
			{
				return null;
			}
			return this.Lookup(httpWebRequest, credentials);
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x0600254F RID: 9551 RVA: 0x00094D76 File Offset: 0x00093D76
		public bool CanPreAuthenticate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x00094D7C File Offset: 0x00093D7C
		public Authorization PreAuthenticate(WebRequest webRequest, ICredentials credentials)
		{
			if (credentials == null || credentials is SystemNetworkCredential)
			{
				return null;
			}
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest == null)
			{
				return null;
			}
			return this.Lookup(httpWebRequest, credentials);
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06002551 RID: 9553 RVA: 0x00094DAA File Offset: 0x00093DAA
		public string AuthenticationType
		{
			get
			{
				return "Basic";
			}
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x00094DB4 File Offset: 0x00093DB4
		private Authorization Lookup(HttpWebRequest httpWebRequest, ICredentials credentials)
		{
			NetworkCredential credential = credentials.GetCredential(httpWebRequest.ChallengedUri, BasicClient.Signature);
			if (credential == null)
			{
				return null;
			}
			ICredentialPolicy credentialPolicy = AuthenticationManager.CredentialPolicy;
			if (credentialPolicy != null && !credentialPolicy.ShouldSendCredential(httpWebRequest.ChallengedUri, httpWebRequest, credential, this))
			{
				return null;
			}
			string text = credential.InternalGetUserName();
			string text2 = credential.InternalGetDomain();
			if (ValidationHelper.IsBlankString(text))
			{
				return null;
			}
			string text3 = ((!ValidationHelper.IsBlankString(text2)) ? (text2 + "\\") : "") + text + ":" + credential.InternalGetPassword();
			byte[] array = BasicClient.EncodingRightGetBytes(text3);
			string text4 = "Basic " + Convert.ToBase64String(array);
			return new Authorization(text4, true);
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x00094E60 File Offset: 0x00093E60
		internal static byte[] EncodingRightGetBytes(string rawString)
		{
			byte[] bytes = Encoding.Default.GetBytes(rawString);
			string @string = Encoding.Default.GetString(bytes);
			if (string.Compare(rawString, @string, StringComparison.Ordinal) != 0)
			{
				throw ExceptionHelper.MethodNotSupportedException;
			}
			return bytes;
		}

		// Token: 0x04002515 RID: 9493
		internal const string AuthType = "Basic";

		// Token: 0x04002516 RID: 9494
		internal static string Signature = "Basic".ToLower(CultureInfo.InvariantCulture);

		// Token: 0x04002517 RID: 9495
		internal static int SignatureSize = BasicClient.Signature.Length;
	}
}
