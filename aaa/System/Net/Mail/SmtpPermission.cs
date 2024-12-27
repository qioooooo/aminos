using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x020006D4 RID: 1748
	[Serializable]
	public sealed class SmtpPermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x060035EC RID: 13804 RVA: 0x000E6054 File Offset: 0x000E5054
		public SmtpPermission(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				this.access = SmtpAccess.ConnectToUnrestrictedPort;
				this.unrestricted = true;
				return;
			}
			this.access = SmtpAccess.None;
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x000E6076 File Offset: 0x000E5076
		public SmtpPermission(bool unrestricted)
		{
			if (unrestricted)
			{
				this.access = SmtpAccess.ConnectToUnrestrictedPort;
				this.unrestricted = true;
				return;
			}
			this.access = SmtpAccess.None;
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x000E6097 File Offset: 0x000E5097
		public SmtpPermission(SmtpAccess access)
		{
			this.access = access;
		}

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x060035EF RID: 13807 RVA: 0x000E60A6 File Offset: 0x000E50A6
		public SmtpAccess Access
		{
			get
			{
				return this.access;
			}
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x000E60AE File Offset: 0x000E50AE
		public void AddPermission(SmtpAccess access)
		{
			if (access > this.access)
			{
				this.access = access;
			}
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x000E60C0 File Offset: 0x000E50C0
		public bool IsUnrestricted()
		{
			return this.unrestricted;
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x000E60C8 File Offset: 0x000E50C8
		public override IPermission Copy()
		{
			if (this.unrestricted)
			{
				return new SmtpPermission(true);
			}
			return new SmtpPermission(this.access);
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x000E60E4 File Offset: 0x000E50E4
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			SmtpPermission smtpPermission = target as SmtpPermission;
			if (smtpPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			if (this.unrestricted || smtpPermission.IsUnrestricted())
			{
				return new SmtpPermission(true);
			}
			return new SmtpPermission((this.access > smtpPermission.access) ? this.access : smtpPermission.access);
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x000E6154 File Offset: 0x000E5154
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			SmtpPermission smtpPermission = target as SmtpPermission;
			if (smtpPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			if (this.IsUnrestricted() && smtpPermission.IsUnrestricted())
			{
				return new SmtpPermission(true);
			}
			return new SmtpPermission((this.access < smtpPermission.access) ? this.access : smtpPermission.access);
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x000E61C0 File Offset: 0x000E51C0
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this.access == SmtpAccess.None;
			}
			SmtpPermission smtpPermission = target as SmtpPermission;
			if (smtpPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			return (!this.unrestricted || smtpPermission.IsUnrestricted()) && smtpPermission.access >= this.access;
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x000E621C File Offset: 0x000E521C
		public override void FromXml(SecurityElement securityElement)
		{
			if (securityElement == null)
			{
				throw new ArgumentNullException("securityElement");
			}
			if (!securityElement.Tag.Equals("IPermission"))
			{
				throw new ArgumentException(SR.GetString("net_not_ipermission"), "securityElement");
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
			if (text2 != null && string.Compare(text2, "true", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.access = SmtpAccess.ConnectToUnrestrictedPort;
				this.unrestricted = true;
				return;
			}
			text2 = securityElement.Attribute("Access");
			if (text2 == null)
			{
				return;
			}
			if (string.Compare(text2, "Connect", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.access = SmtpAccess.Connect;
				return;
			}
			if (string.Compare(text2, "ConnectToUnrestrictedPort", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.access = SmtpAccess.ConnectToUnrestrictedPort;
				return;
			}
			if (string.Compare(text2, "None", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.access = SmtpAccess.None;
				return;
			}
			throw new ArgumentException(SR.GetString("net_perm_invalid_val_in_element"), "Access");
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x000E6340 File Offset: 0x000E5340
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", base.GetType().FullName + ", " + base.GetType().Module.Assembly.FullName.Replace('"', '\''));
			securityElement.AddAttribute("version", "1");
			if (this.unrestricted)
			{
				securityElement.AddAttribute("Unrestricted", "true");
				return securityElement;
			}
			if (this.access == SmtpAccess.Connect)
			{
				securityElement.AddAttribute("Access", "Connect");
			}
			else if (this.access == SmtpAccess.ConnectToUnrestrictedPort)
			{
				securityElement.AddAttribute("Access", "ConnectToUnrestrictedPort");
			}
			return securityElement;
		}

		// Token: 0x04003115 RID: 12565
		private SmtpAccess access;

		// Token: 0x04003116 RID: 12566
		private bool unrestricted;
	}
}
