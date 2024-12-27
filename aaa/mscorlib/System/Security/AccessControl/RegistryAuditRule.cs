using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x0200091E RID: 2334
	public sealed class RegistryAuditRule : AuditRule
	{
		// Token: 0x060054A8 RID: 21672 RVA: 0x001340B4 File Offset: 0x001330B4
		public RegistryAuditRule(IdentityReference identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
			: this(identity, (int)registryRights, false, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x060054A9 RID: 21673 RVA: 0x001340C4 File Offset: 0x001330C4
		public RegistryAuditRule(string identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
			: this(new NTAccount(identity), (int)registryRights, false, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x060054AA RID: 21674 RVA: 0x001340D9 File Offset: 0x001330D9
		internal RegistryAuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
			: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x060054AB RID: 21675 RVA: 0x001340EA File Offset: 0x001330EA
		public RegistryRights RegistryRights
		{
			get
			{
				return (RegistryRights)base.AccessMask;
			}
		}
	}
}
