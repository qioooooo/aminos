using System;
using System.Security.Principal;

namespace System.Security.Permissions
{
	// Token: 0x02000638 RID: 1592
	[Serializable]
	internal class IDRole
	{
		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x060039D6 RID: 14806 RVA: 0x000C2CD8 File Offset: 0x000C1CD8
		internal SecurityIdentifier Sid
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_role))
				{
					return null;
				}
				if (this.m_sid == null)
				{
					NTAccount ntaccount = new NTAccount(this.m_role);
					IdentityReferenceCollection identityReferenceCollection = NTAccount.Translate(new IdentityReferenceCollection(1) { ntaccount }, typeof(SecurityIdentifier), false);
					this.m_sid = identityReferenceCollection[0] as SecurityIdentifier;
				}
				return this.m_sid;
			}
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x000C2D48 File Offset: 0x000C1D48
		internal SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("Identity");
			if (this.m_authenticated)
			{
				securityElement.AddAttribute("Authenticated", "true");
			}
			if (this.m_id != null)
			{
				securityElement.AddAttribute("ID", SecurityElement.Escape(this.m_id));
			}
			if (this.m_role != null)
			{
				securityElement.AddAttribute("Role", SecurityElement.Escape(this.m_role));
			}
			return securityElement;
		}

		// Token: 0x060039D8 RID: 14808 RVA: 0x000C2DB8 File Offset: 0x000C1DB8
		internal void FromXml(SecurityElement e)
		{
			string text = e.Attribute("Authenticated");
			if (text != null)
			{
				this.m_authenticated = string.Compare(text, "true", StringComparison.OrdinalIgnoreCase) == 0;
			}
			else
			{
				this.m_authenticated = false;
			}
			string text2 = e.Attribute("ID");
			if (text2 != null)
			{
				this.m_id = text2;
			}
			else
			{
				this.m_id = null;
			}
			string text3 = e.Attribute("Role");
			if (text3 != null)
			{
				this.m_role = text3;
				return;
			}
			this.m_role = null;
		}

		// Token: 0x060039D9 RID: 14809 RVA: 0x000C2E2F File Offset: 0x000C1E2F
		public override int GetHashCode()
		{
			return (this.m_authenticated ? 0 : 101) + ((this.m_id == null) ? 0 : this.m_id.GetHashCode()) + ((this.m_role == null) ? 0 : this.m_role.GetHashCode());
		}

		// Token: 0x04001DDE RID: 7646
		internal bool m_authenticated;

		// Token: 0x04001DDF RID: 7647
		internal string m_id;

		// Token: 0x04001DE0 RID: 7648
		internal string m_role;

		// Token: 0x04001DE1 RID: 7649
		[NonSerialized]
		private SecurityIdentifier m_sid;
	}
}
