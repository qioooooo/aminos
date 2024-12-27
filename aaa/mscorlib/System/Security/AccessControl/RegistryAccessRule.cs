using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x0200091D RID: 2333
	public sealed class RegistryAccessRule : AccessRule
	{
		// Token: 0x060054A2 RID: 21666 RVA: 0x00134055 File Offset: 0x00133055
		public RegistryAccessRule(IdentityReference identity, RegistryRights registryRights, AccessControlType type)
			: this(identity, (int)registryRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x060054A3 RID: 21667 RVA: 0x00134063 File Offset: 0x00133063
		public RegistryAccessRule(string identity, RegistryRights registryRights, AccessControlType type)
			: this(new NTAccount(identity), (int)registryRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x060054A4 RID: 21668 RVA: 0x00134076 File Offset: 0x00133076
		public RegistryAccessRule(IdentityReference identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
			: this(identity, (int)registryRights, false, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x060054A5 RID: 21669 RVA: 0x00134086 File Offset: 0x00133086
		public RegistryAccessRule(string identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
			: this(new NTAccount(identity), (int)registryRights, false, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x060054A6 RID: 21670 RVA: 0x0013409B File Offset: 0x0013309B
		internal RegistryAccessRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
			: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x060054A7 RID: 21671 RVA: 0x001340AC File Offset: 0x001330AC
		public RegistryRights RegistryRights
		{
			get
			{
				return (RegistryRights)base.AccessMask;
			}
		}
	}
}
