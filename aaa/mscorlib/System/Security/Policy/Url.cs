using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x020004A4 RID: 1188
	[ComVisible(true)]
	[Serializable]
	public sealed class Url : IIdentityPermissionFactory, IBuiltInEvidence
	{
		// Token: 0x06002FF3 RID: 12275 RVA: 0x000A580E File Offset: 0x000A480E
		internal Url()
		{
			this.m_url = null;
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x000A581D File Offset: 0x000A481D
		internal Url(SerializationInfo info, StreamingContext context)
		{
			this.m_url = new URLString((string)info.GetValue("Url", typeof(string)));
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x000A584A File Offset: 0x000A484A
		internal Url(string name, bool parsed)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.m_url = new URLString(name, parsed);
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x000A586D File Offset: 0x000A486D
		public Url(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.m_url = new URLString(name);
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06002FF7 RID: 12279 RVA: 0x000A588F File Offset: 0x000A488F
		public string Value
		{
			get
			{
				if (this.m_url == null)
				{
					return null;
				}
				return this.m_url.ToString();
			}
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x000A58A6 File Offset: 0x000A48A6
		internal URLString GetURLString()
		{
			return this.m_url;
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x000A58AE File Offset: 0x000A48AE
		public IPermission CreateIdentityPermission(Evidence evidence)
		{
			return new UrlIdentityPermission(this.m_url);
		}

		// Token: 0x06002FFA RID: 12282 RVA: 0x000A58BC File Offset: 0x000A48BC
		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			if (!(o is Url))
			{
				return false;
			}
			Url url = (Url)o;
			if (this.m_url == null)
			{
				return url.m_url == null;
			}
			return url.m_url != null && this.m_url.Equals(url.m_url);
		}

		// Token: 0x06002FFB RID: 12283 RVA: 0x000A590C File Offset: 0x000A490C
		public override int GetHashCode()
		{
			if (this.m_url == null)
			{
				return 0;
			}
			return this.m_url.GetHashCode();
		}

		// Token: 0x06002FFC RID: 12284 RVA: 0x000A5924 File Offset: 0x000A4924
		public object Copy()
		{
			return new Url
			{
				m_url = this.m_url
			};
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x000A5944 File Offset: 0x000A4944
		internal SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("System.Security.Policy.Url");
			securityElement.AddAttribute("version", "1");
			if (this.m_url != null)
			{
				securityElement.AddChild(new SecurityElement("Url", this.m_url.ToString()));
			}
			return securityElement;
		}

		// Token: 0x06002FFE RID: 12286 RVA: 0x000A5990 File Offset: 0x000A4990
		public override string ToString()
		{
			return this.ToXml().ToString();
		}

		// Token: 0x06002FFF RID: 12287 RVA: 0x000A59A0 File Offset: 0x000A49A0
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			buffer[position++] = '\u0004';
			string value = this.Value;
			int length = value.Length;
			if (verbose)
			{
				BuiltInEvidenceHelper.CopyIntToCharArray(length, buffer, position);
				position += 2;
			}
			value.CopyTo(0, buffer, position, length);
			return length + position;
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x000A59E1 File Offset: 0x000A49E1
		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			if (verbose)
			{
				return this.Value.Length + 3;
			}
			return this.Value.Length + 1;
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x000A5A04 File Offset: 0x000A4A04
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			int intFromCharArray = BuiltInEvidenceHelper.GetIntFromCharArray(buffer, position);
			position += 2;
			this.m_url = new URLString(new string(buffer, position, intFromCharArray));
			return position + intFromCharArray;
		}

		// Token: 0x06003002 RID: 12290 RVA: 0x000A5A34 File Offset: 0x000A4A34
		internal object Normalize()
		{
			return this.m_url.NormalizeUrl();
		}

		// Token: 0x04001816 RID: 6166
		private URLString m_url;
	}
}
