using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x0200049F RID: 1183
	[ComVisible(true)]
	[Serializable]
	public sealed class Site : IIdentityPermissionFactory, IBuiltInEvidence
	{
		// Token: 0x06002FA0 RID: 12192 RVA: 0x000A4363 File Offset: 0x000A3363
		internal Site()
		{
			this.m_name = null;
		}

		// Token: 0x06002FA1 RID: 12193 RVA: 0x000A4372 File Offset: 0x000A3372
		public Site(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.m_name = new SiteString(name);
		}

		// Token: 0x06002FA2 RID: 12194 RVA: 0x000A4394 File Offset: 0x000A3394
		internal Site(byte[] id, string name)
		{
			this.m_name = Site.ParseSiteFromUrl(name);
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x000A43A8 File Offset: 0x000A33A8
		public static Site CreateFromUrl(string url)
		{
			return new Site
			{
				m_name = Site.ParseSiteFromUrl(url)
			};
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x000A43C8 File Offset: 0x000A33C8
		private static SiteString ParseSiteFromUrl(string name)
		{
			URLString urlstring = new URLString(name);
			if (string.Compare(urlstring.Scheme, "file", StringComparison.OrdinalIgnoreCase) == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidSite"));
			}
			return new SiteString(new URLString(name).Host);
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06002FA5 RID: 12197 RVA: 0x000A440F File Offset: 0x000A340F
		public string Name
		{
			get
			{
				if (this.m_name != null)
				{
					return this.m_name.ToString();
				}
				return null;
			}
		}

		// Token: 0x06002FA6 RID: 12198 RVA: 0x000A4426 File Offset: 0x000A3426
		internal SiteString GetSiteString()
		{
			return this.m_name;
		}

		// Token: 0x06002FA7 RID: 12199 RVA: 0x000A442E File Offset: 0x000A342E
		public IPermission CreateIdentityPermission(Evidence evidence)
		{
			return new SiteIdentityPermission(this.Name);
		}

		// Token: 0x06002FA8 RID: 12200 RVA: 0x000A443C File Offset: 0x000A343C
		public override bool Equals(object o)
		{
			if (!(o is Site))
			{
				return false;
			}
			Site site = (Site)o;
			if (this.Name == null)
			{
				return site.Name == null;
			}
			return string.Compare(this.Name, site.Name, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x000A4484 File Offset: 0x000A3484
		public override int GetHashCode()
		{
			string name = this.Name;
			if (name == null)
			{
				return 0;
			}
			return name.GetHashCode();
		}

		// Token: 0x06002FAA RID: 12202 RVA: 0x000A44A3 File Offset: 0x000A34A3
		public object Copy()
		{
			return new Site(this.Name);
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x000A44B0 File Offset: 0x000A34B0
		internal SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("System.Security.Policy.Site");
			securityElement.AddAttribute("version", "1");
			if (this.m_name != null)
			{
				securityElement.AddChild(new SecurityElement("Name", this.m_name.ToString()));
			}
			return securityElement;
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x000A44FC File Offset: 0x000A34FC
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			buffer[position++] = '\u0006';
			string name = this.Name;
			int length = name.Length;
			if (verbose)
			{
				BuiltInEvidenceHelper.CopyIntToCharArray(length, buffer, position);
				position += 2;
			}
			name.CopyTo(0, buffer, position, length);
			return length + position;
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x000A453D File Offset: 0x000A353D
		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			if (verbose)
			{
				return this.Name.Length + 3;
			}
			return this.Name.Length + 1;
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x000A4560 File Offset: 0x000A3560
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			int intFromCharArray = BuiltInEvidenceHelper.GetIntFromCharArray(buffer, position);
			position += 2;
			this.m_name = new SiteString(new string(buffer, position, intFromCharArray));
			return position + intFromCharArray;
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x000A4590 File Offset: 0x000A3590
		public override string ToString()
		{
			return this.ToXml().ToString();
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x000A459D File Offset: 0x000A359D
		internal object Normalize()
		{
			return this.m_name.ToString().ToUpper(CultureInfo.InvariantCulture);
		}

		// Token: 0x04001807 RID: 6151
		private SiteString m_name;
	}
}
