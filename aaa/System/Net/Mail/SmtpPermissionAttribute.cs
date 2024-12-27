using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x020006D3 RID: 1747
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class SmtpPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060035E8 RID: 13800 RVA: 0x000E5F89 File Offset: 0x000E4F89
		public SmtpPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x060035E9 RID: 13801 RVA: 0x000E5F92 File Offset: 0x000E4F92
		// (set) Token: 0x060035EA RID: 13802 RVA: 0x000E5F9A File Offset: 0x000E4F9A
		public string Access
		{
			get
			{
				return this.access;
			}
			set
			{
				this.access = value;
			}
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x000E5FA4 File Offset: 0x000E4FA4
		public override IPermission CreatePermission()
		{
			SmtpPermission smtpPermission;
			if (base.Unrestricted)
			{
				smtpPermission = new SmtpPermission(PermissionState.Unrestricted);
			}
			else
			{
				smtpPermission = new SmtpPermission(PermissionState.None);
				if (this.access != null)
				{
					if (string.Compare(this.access, "Connect", StringComparison.OrdinalIgnoreCase) == 0)
					{
						smtpPermission.AddPermission(SmtpAccess.Connect);
					}
					else if (string.Compare(this.access, "ConnectToUnrestrictedPort", StringComparison.OrdinalIgnoreCase) == 0)
					{
						smtpPermission.AddPermission(SmtpAccess.ConnectToUnrestrictedPort);
					}
					else
					{
						if (string.Compare(this.access, "None", StringComparison.OrdinalIgnoreCase) != 0)
						{
							throw new ArgumentException(SR.GetString("net_perm_invalid_val", new object[] { "Access", this.access }));
						}
						smtpPermission.AddPermission(SmtpAccess.None);
					}
				}
			}
			return smtpPermission;
		}

		// Token: 0x04003113 RID: 12563
		private const string strAccess = "Access";

		// Token: 0x04003114 RID: 12564
		private string access;
	}
}
