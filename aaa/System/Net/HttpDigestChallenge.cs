using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace System.Net
{
	// Token: 0x020004D6 RID: 1238
	internal class HttpDigestChallenge
	{
		// Token: 0x0600267E RID: 9854 RVA: 0x0009CC1C File Offset: 0x0009BC1C
		internal void SetFromRequest(HttpWebRequest httpWebRequest)
		{
			this.HostName = httpWebRequest.ChallengedUri.Host;
			this.Method = httpWebRequest.CurrentMethod.Name;
			this.Uri = httpWebRequest.Address.AbsolutePath;
			this.ChallengedUri = httpWebRequest.ChallengedUri;
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x0009CC68 File Offset: 0x0009BC68
		internal HttpDigestChallenge CopyAndIncrementNonce()
		{
			HttpDigestChallenge httpDigestChallenge = null;
			lock (this)
			{
				httpDigestChallenge = base.MemberwiseClone() as HttpDigestChallenge;
				this.NonceCount++;
			}
			httpDigestChallenge.MD5provider = new MD5CryptoServiceProvider();
			return httpDigestChallenge;
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x0009CCC0 File Offset: 0x0009BCC0
		public bool defineAttribute(string name, string value)
		{
			name = name.Trim().ToLower(CultureInfo.InvariantCulture);
			if (name.Equals("algorithm"))
			{
				this.Algorithm = value;
			}
			else if (name.Equals("cnonce"))
			{
				this.ClientNonce = value;
			}
			else if (name.Equals("nc"))
			{
				this.NonceCount = int.Parse(value, NumberFormatInfo.InvariantInfo);
			}
			else if (name.Equals("nonce"))
			{
				this.Nonce = value;
			}
			else if (name.Equals("opaque"))
			{
				this.Opaque = value;
			}
			else if (name.Equals("qop"))
			{
				this.QualityOfProtection = value;
				this.QopPresent = this.QualityOfProtection != null && this.QualityOfProtection.Length > 0;
			}
			else if (name.Equals("realm"))
			{
				this.Realm = value;
			}
			else if (name.Equals("domain"))
			{
				this.Domain = value;
			}
			else if (!name.Equals("response"))
			{
				if (name.Equals("stale"))
				{
					this.Stale = value.ToLower(CultureInfo.InvariantCulture).Equals("true");
				}
				else if (name.Equals("uri"))
				{
					this.Uri = value;
				}
				else if (name.Equals("charset"))
				{
					this.Charset = value;
				}
				else if (!name.Equals("cipher") && !name.Equals("username"))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x0009CE50 File Offset: 0x0009BE50
		internal string ToBlob()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(HttpDigest.pair("realm", this.Realm, true));
			if (this.Algorithm != null)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("algorithm", this.Algorithm, true));
			}
			if (this.Charset != null)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("charset", this.Charset, false));
			}
			if (this.Nonce != null)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("nonce", this.Nonce, true));
			}
			if (this.Uri != null)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("uri", this.Uri, true));
			}
			if (this.ClientNonce != null)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("cnonce", this.ClientNonce, true));
			}
			if (this.NonceCount > 0)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("nc", this.NonceCount.ToString("x8", NumberFormatInfo.InvariantInfo), true));
			}
			if (this.QualityOfProtection != null)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("qop", this.QualityOfProtection, true));
			}
			if (this.Opaque != null)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("opaque", this.Opaque, true));
			}
			if (this.Domain != null)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("domain", this.Domain, true));
			}
			if (this.Stale)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("stale", "true", true));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040025F1 RID: 9713
		internal string HostName;

		// Token: 0x040025F2 RID: 9714
		internal string Realm;

		// Token: 0x040025F3 RID: 9715
		internal Uri ChallengedUri;

		// Token: 0x040025F4 RID: 9716
		internal string Uri;

		// Token: 0x040025F5 RID: 9717
		internal string Nonce;

		// Token: 0x040025F6 RID: 9718
		internal string Opaque;

		// Token: 0x040025F7 RID: 9719
		internal bool Stale;

		// Token: 0x040025F8 RID: 9720
		internal string Algorithm;

		// Token: 0x040025F9 RID: 9721
		internal string Method;

		// Token: 0x040025FA RID: 9722
		internal string Domain;

		// Token: 0x040025FB RID: 9723
		internal string QualityOfProtection;

		// Token: 0x040025FC RID: 9724
		internal string ClientNonce;

		// Token: 0x040025FD RID: 9725
		internal int NonceCount;

		// Token: 0x040025FE RID: 9726
		internal string Charset;

		// Token: 0x040025FF RID: 9727
		internal string ServiceName;

		// Token: 0x04002600 RID: 9728
		internal string ChannelBinding;

		// Token: 0x04002601 RID: 9729
		internal bool UTF8Charset;

		// Token: 0x04002602 RID: 9730
		internal bool QopPresent;

		// Token: 0x04002603 RID: 9731
		internal MD5CryptoServiceProvider MD5provider = new MD5CryptoServiceProvider();
	}
}
