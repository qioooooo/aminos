using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020003A8 RID: 936
	[Serializable]
	public sealed class DnsPermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x06001D43 RID: 7491 RVA: 0x0006FEB9 File Offset: 0x0006EEB9
		public DnsPermission(PermissionState state)
		{
			this.m_noRestriction = state == PermissionState.Unrestricted;
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x0006FECB File Offset: 0x0006EECB
		internal DnsPermission(bool free)
		{
			this.m_noRestriction = free;
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x0006FEDA File Offset: 0x0006EEDA
		public bool IsUnrestricted()
		{
			return this.m_noRestriction;
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x0006FEE2 File Offset: 0x0006EEE2
		public override IPermission Copy()
		{
			return new DnsPermission(this.m_noRestriction);
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x0006FEF0 File Offset: 0x0006EEF0
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			DnsPermission dnsPermission = target as DnsPermission;
			if (dnsPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			return new DnsPermission(this.m_noRestriction || dnsPermission.m_noRestriction);
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x0006FF3C File Offset: 0x0006EF3C
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			DnsPermission dnsPermission = target as DnsPermission;
			if (dnsPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			if (this.m_noRestriction && dnsPermission.m_noRestriction)
			{
				return new DnsPermission(true);
			}
			return null;
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x0006FF88 File Offset: 0x0006EF88
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return !this.m_noRestriction;
			}
			DnsPermission dnsPermission = target as DnsPermission;
			if (dnsPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			return !this.m_noRestriction || dnsPermission.m_noRestriction;
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x0006FFD4 File Offset: 0x0006EFD4
		public override void FromXml(SecurityElement securityElement)
		{
			if (securityElement == null)
			{
				throw new ArgumentNullException("securityElement");
			}
			if (!securityElement.Tag.Equals("IPermission"))
			{
				throw new ArgumentException(SR.GetString("net_no_classname"), "securityElement");
			}
			string text = securityElement.Attribute("class");
			if (text == null)
			{
				throw new ArgumentException(SR.GetString("net_no_classname"), "securityElement");
			}
			if (text.IndexOf(base.GetType().FullName) < 0)
			{
				throw new ArgumentException(SR.GetString("net_no_typename"), "securityElement");
			}
			string text2 = securityElement.Attribute("Unrestricted");
			this.m_noRestriction = text2 != null && 0 == string.Compare(text2, "true", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x0007008C File Offset: 0x0006F08C
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", base.GetType().FullName + ", " + base.GetType().Module.Assembly.FullName.Replace('"', '\''));
			securityElement.AddAttribute("version", "1");
			if (this.m_noRestriction)
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			return securityElement;
		}

		// Token: 0x04001D75 RID: 7541
		private bool m_noRestriction;
	}
}
