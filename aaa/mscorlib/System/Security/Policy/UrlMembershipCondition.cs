using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x020004A5 RID: 1189
	[ComVisible(true)]
	[Serializable]
	public sealed class UrlMembershipCondition : IConstantMembershipCondition, IReportMatchMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x06003003 RID: 12291 RVA: 0x000A5A41 File Offset: 0x000A4A41
		internal UrlMembershipCondition()
		{
			this.m_url = null;
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x000A5A50 File Offset: 0x000A4A50
		public UrlMembershipCondition(string url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			this.m_url = new URLString(url, false, true);
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06003006 RID: 12294 RVA: 0x000A5A90 File Offset: 0x000A4A90
		// (set) Token: 0x06003005 RID: 12293 RVA: 0x000A5A74 File Offset: 0x000A4A74
		public string Url
		{
			get
			{
				if (this.m_url == null && this.m_element != null)
				{
					this.ParseURL();
				}
				return this.m_url.ToString();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_url = new URLString(value);
			}
		}

		// Token: 0x06003007 RID: 12295 RVA: 0x000A5AB4 File Offset: 0x000A4AB4
		public bool Check(Evidence evidence)
		{
			object obj = null;
			return ((IReportMatchMembershipCondition)this).Check(evidence, out obj);
		}

		// Token: 0x06003008 RID: 12296 RVA: 0x000A5ACC File Offset: 0x000A4ACC
		bool IReportMatchMembershipCondition.Check(Evidence evidence, out object usedEvidence)
		{
			usedEvidence = null;
			if (evidence == null)
			{
				return false;
			}
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				if (hostEnumerator.Current is Url)
				{
					if (this.m_url == null && this.m_element != null)
					{
						this.ParseURL();
					}
					if (((Url)hostEnumerator.Current).GetURLString().IsSubsetOf(this.m_url))
					{
						usedEvidence = hostEnumerator.Current;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x000A5B40 File Offset: 0x000A4B40
		public IMembershipCondition Copy()
		{
			if (this.m_url == null && this.m_element != null)
			{
				this.ParseURL();
			}
			return new UrlMembershipCondition
			{
				m_url = new URLString(this.m_url.ToString())
			};
		}

		// Token: 0x0600300A RID: 12298 RVA: 0x000A5B80 File Offset: 0x000A4B80
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x0600300B RID: 12299 RVA: 0x000A5B89 File Offset: 0x000A4B89
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x0600300C RID: 12300 RVA: 0x000A5B94 File Offset: 0x000A4B94
		public SecurityElement ToXml(PolicyLevel level)
		{
			if (this.m_url == null && this.m_element != null)
			{
				this.ParseURL();
			}
			SecurityElement securityElement = new SecurityElement("IMembershipCondition");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), "System.Security.Policy.UrlMembershipCondition");
			securityElement.AddAttribute("version", "1");
			if (this.m_url != null)
			{
				securityElement.AddAttribute("Url", this.m_url.ToString());
			}
			return securityElement;
		}

		// Token: 0x0600300D RID: 12301 RVA: 0x000A5C04 File Offset: 0x000A4C04
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (!e.Tag.Equals("IMembershipCondition"))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MembershipConditionElement"));
			}
			lock (this)
			{
				this.m_element = e;
				this.m_url = null;
			}
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x000A5C70 File Offset: 0x000A4C70
		private void ParseURL()
		{
			lock (this)
			{
				if (this.m_element != null)
				{
					string text = this.m_element.Attribute("Url");
					if (text == null)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_UrlCannotBeNull"));
					}
					this.m_url = new URLString(text);
					this.m_element = null;
				}
			}
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x000A5CE0 File Offset: 0x000A4CE0
		public override bool Equals(object o)
		{
			UrlMembershipCondition urlMembershipCondition = o as UrlMembershipCondition;
			if (urlMembershipCondition != null)
			{
				if (this.m_url == null && this.m_element != null)
				{
					this.ParseURL();
				}
				if (urlMembershipCondition.m_url == null && urlMembershipCondition.m_element != null)
				{
					urlMembershipCondition.ParseURL();
				}
				if (object.Equals(this.m_url, urlMembershipCondition.m_url))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x000A5D39 File Offset: 0x000A4D39
		public override int GetHashCode()
		{
			if (this.m_url == null && this.m_element != null)
			{
				this.ParseURL();
			}
			if (this.m_url != null)
			{
				return this.m_url.GetHashCode();
			}
			return typeof(UrlMembershipCondition).GetHashCode();
		}

		// Token: 0x06003011 RID: 12305 RVA: 0x000A5D74 File Offset: 0x000A4D74
		public override string ToString()
		{
			if (this.m_url == null && this.m_element != null)
			{
				this.ParseURL();
			}
			if (this.m_url != null)
			{
				return string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Url_ToStringArg"), new object[] { this.m_url.ToString() });
			}
			return Environment.GetResourceString("Url_ToString");
		}

		// Token: 0x04001817 RID: 6167
		private URLString m_url;

		// Token: 0x04001818 RID: 6168
		private SecurityElement m_element;
	}
}
