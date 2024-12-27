using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x020004A0 RID: 1184
	[ComVisible(true)]
	[Serializable]
	public sealed class SiteMembershipCondition : IConstantMembershipCondition, IReportMatchMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x06002FB1 RID: 12209 RVA: 0x000A45B4 File Offset: 0x000A35B4
		internal SiteMembershipCondition()
		{
			this.m_site = null;
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x000A45C3 File Offset: 0x000A35C3
		public SiteMembershipCondition(string site)
		{
			if (site == null)
			{
				throw new ArgumentNullException("site");
			}
			this.m_site = new SiteString(site);
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06002FB4 RID: 12212 RVA: 0x000A4601 File Offset: 0x000A3601
		// (set) Token: 0x06002FB3 RID: 12211 RVA: 0x000A45E5 File Offset: 0x000A35E5
		public string Site
		{
			get
			{
				if (this.m_site == null && this.m_element != null)
				{
					this.ParseSite();
				}
				if (this.m_site != null)
				{
					return this.m_site.ToString();
				}
				return "";
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_site = new SiteString(value);
			}
		}

		// Token: 0x06002FB5 RID: 12213 RVA: 0x000A4634 File Offset: 0x000A3634
		public bool Check(Evidence evidence)
		{
			object obj = null;
			return ((IReportMatchMembershipCondition)this).Check(evidence, out obj);
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x000A464C File Offset: 0x000A364C
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
				object obj = hostEnumerator.Current;
				Site site = obj as Site;
				if (site != null)
				{
					if (this.m_site == null && this.m_element != null)
					{
						this.ParseSite();
					}
					if (site.GetSiteString().IsSubsetOf(this.m_site))
					{
						usedEvidence = site;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002FB7 RID: 12215 RVA: 0x000A46B0 File Offset: 0x000A36B0
		public IMembershipCondition Copy()
		{
			if (this.m_site == null && this.m_element != null)
			{
				this.ParseSite();
			}
			return new SiteMembershipCondition(this.m_site.ToString());
		}

		// Token: 0x06002FB8 RID: 12216 RVA: 0x000A46D8 File Offset: 0x000A36D8
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x06002FB9 RID: 12217 RVA: 0x000A46E1 File Offset: 0x000A36E1
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x000A46EC File Offset: 0x000A36EC
		public SecurityElement ToXml(PolicyLevel level)
		{
			if (this.m_site == null && this.m_element != null)
			{
				this.ParseSite();
			}
			SecurityElement securityElement = new SecurityElement("IMembershipCondition");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), "System.Security.Policy.SiteMembershipCondition");
			securityElement.AddAttribute("version", "1");
			if (this.m_site != null)
			{
				securityElement.AddAttribute("Site", this.m_site.ToString());
			}
			return securityElement;
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x000A475C File Offset: 0x000A375C
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
				this.m_site = null;
				this.m_element = e;
			}
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x000A47C8 File Offset: 0x000A37C8
		private void ParseSite()
		{
			lock (this)
			{
				if (this.m_element != null)
				{
					string text = this.m_element.Attribute("Site");
					if (text == null)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_SiteCannotBeNull"));
					}
					this.m_site = new SiteString(text);
					this.m_element = null;
				}
			}
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x000A4838 File Offset: 0x000A3838
		public override bool Equals(object o)
		{
			SiteMembershipCondition siteMembershipCondition = o as SiteMembershipCondition;
			if (siteMembershipCondition != null)
			{
				if (this.m_site == null && this.m_element != null)
				{
					this.ParseSite();
				}
				if (siteMembershipCondition.m_site == null && siteMembershipCondition.m_element != null)
				{
					siteMembershipCondition.ParseSite();
				}
				if (object.Equals(this.m_site, siteMembershipCondition.m_site))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x000A4891 File Offset: 0x000A3891
		public override int GetHashCode()
		{
			if (this.m_site == null && this.m_element != null)
			{
				this.ParseSite();
			}
			if (this.m_site != null)
			{
				return this.m_site.GetHashCode();
			}
			return typeof(SiteMembershipCondition).GetHashCode();
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x000A48CC File Offset: 0x000A38CC
		public override string ToString()
		{
			if (this.m_site == null && this.m_element != null)
			{
				this.ParseSite();
			}
			if (this.m_site != null)
			{
				return string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Site_ToStringArg"), new object[] { this.m_site });
			}
			return Environment.GetResourceString("Site_ToString");
		}

		// Token: 0x04001808 RID: 6152
		private SiteString m_site;

		// Token: 0x04001809 RID: 6153
		private SecurityElement m_element;
	}
}
