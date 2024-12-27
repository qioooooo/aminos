using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace System.DirectoryServices
{
	// Token: 0x02000019 RID: 25
	public class ActiveDirectoryAuditRule : ObjectAuditRule
	{
		// Token: 0x0600005E RID: 94 RVA: 0x00002E80 File Offset: 0x00001E80
		public ActiveDirectoryAuditRule(IdentityReference identity, ActiveDirectoryRights adRights, AuditFlags auditFlags)
			: this(identity, ActiveDirectoryRightsTranslator.AccessMaskFromRights(adRights), auditFlags, Guid.Empty, false, InheritanceFlags.None, PropagationFlags.None, Guid.Empty)
		{
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002EA8 File Offset: 0x00001EA8
		public ActiveDirectoryAuditRule(IdentityReference identity, ActiveDirectoryRights adRights, AuditFlags auditFlags, Guid objectType)
			: this(identity, ActiveDirectoryRightsTranslator.AccessMaskFromRights(adRights), auditFlags, objectType, false, InheritanceFlags.None, PropagationFlags.None, Guid.Empty)
		{
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002ED0 File Offset: 0x00001ED0
		public ActiveDirectoryAuditRule(IdentityReference identity, ActiveDirectoryRights adRights, AuditFlags auditFlags, ActiveDirectorySecurityInheritance inheritanceType)
			: this(identity, ActiveDirectoryRightsTranslator.AccessMaskFromRights(adRights), auditFlags, Guid.Empty, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), Guid.Empty)
		{
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002F04 File Offset: 0x00001F04
		public ActiveDirectoryAuditRule(IdentityReference identity, ActiveDirectoryRights adRights, AuditFlags auditFlags, Guid objectType, ActiveDirectorySecurityInheritance inheritanceType)
			: this(identity, ActiveDirectoryRightsTranslator.AccessMaskFromRights(adRights), auditFlags, objectType, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), Guid.Empty)
		{
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002F38 File Offset: 0x00001F38
		public ActiveDirectoryAuditRule(IdentityReference identity, ActiveDirectoryRights adRights, AuditFlags auditFlags, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
			: this(identity, ActiveDirectoryRightsTranslator.AccessMaskFromRights(adRights), auditFlags, Guid.Empty, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), inheritedObjectType)
		{
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002F6C File Offset: 0x00001F6C
		public ActiveDirectoryAuditRule(IdentityReference identity, ActiveDirectoryRights adRights, AuditFlags auditFlags, Guid objectType, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
			: this(identity, ActiveDirectoryRightsTranslator.AccessMaskFromRights(adRights), auditFlags, objectType, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), inheritedObjectType)
		{
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002F9C File Offset: 0x00001F9C
		internal ActiveDirectoryAuditRule(IdentityReference identity, int accessMask, AuditFlags auditFlags, Guid objectGuid, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, Guid inheritedObjectType)
			: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, objectGuid, inheritedObjectType, auditFlags)
		{
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00002FBC File Offset: 0x00001FBC
		public ActiveDirectoryRights ActiveDirectoryRights
		{
			get
			{
				return ActiveDirectoryRightsTranslator.RightsFromAccessMask(base.AccessMask);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002FC9 File Offset: 0x00001FC9
		public ActiveDirectorySecurityInheritance InheritanceType
		{
			get
			{
				return ActiveDirectoryInheritanceTranslator.GetEffectiveInheritanceFlags(base.InheritanceFlags, base.PropagationFlags);
			}
		}
	}
}
