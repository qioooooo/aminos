using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace System.DirectoryServices
{
	// Token: 0x02000011 RID: 17
	public class ActiveDirectoryAccessRule : ObjectAccessRule
	{
		// Token: 0x06000034 RID: 52 RVA: 0x00002788 File Offset: 0x00001788
		public ActiveDirectoryAccessRule(IdentityReference identity, ActiveDirectoryRights adRights, AccessControlType type)
			: this(identity, ActiveDirectoryRightsTranslator.AccessMaskFromRights(adRights), type, Guid.Empty, false, InheritanceFlags.None, PropagationFlags.None, Guid.Empty)
		{
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000027B0 File Offset: 0x000017B0
		public ActiveDirectoryAccessRule(IdentityReference identity, ActiveDirectoryRights adRights, AccessControlType type, Guid objectType)
			: this(identity, ActiveDirectoryRightsTranslator.AccessMaskFromRights(adRights), type, objectType, false, InheritanceFlags.None, PropagationFlags.None, Guid.Empty)
		{
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000027D8 File Offset: 0x000017D8
		public ActiveDirectoryAccessRule(IdentityReference identity, ActiveDirectoryRights adRights, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType)
			: this(identity, ActiveDirectoryRightsTranslator.AccessMaskFromRights(adRights), type, Guid.Empty, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), Guid.Empty)
		{
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000280C File Offset: 0x0000180C
		public ActiveDirectoryAccessRule(IdentityReference identity, ActiveDirectoryRights adRights, AccessControlType type, Guid objectType, ActiveDirectorySecurityInheritance inheritanceType)
			: this(identity, ActiveDirectoryRightsTranslator.AccessMaskFromRights(adRights), type, objectType, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), Guid.Empty)
		{
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002840 File Offset: 0x00001840
		public ActiveDirectoryAccessRule(IdentityReference identity, ActiveDirectoryRights adRights, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
			: this(identity, ActiveDirectoryRightsTranslator.AccessMaskFromRights(adRights), type, Guid.Empty, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), inheritedObjectType)
		{
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002874 File Offset: 0x00001874
		public ActiveDirectoryAccessRule(IdentityReference identity, ActiveDirectoryRights adRights, AccessControlType type, Guid objectType, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
			: this(identity, ActiveDirectoryRightsTranslator.AccessMaskFromRights(adRights), type, objectType, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), inheritedObjectType)
		{
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000028A4 File Offset: 0x000018A4
		internal ActiveDirectoryAccessRule(IdentityReference identity, int accessMask, AccessControlType type, Guid objectType, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, Guid inheritedObjectType)
			: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, objectType, inheritedObjectType, type)
		{
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600003B RID: 59 RVA: 0x000028C4 File Offset: 0x000018C4
		public ActiveDirectoryRights ActiveDirectoryRights
		{
			get
			{
				return ActiveDirectoryRightsTranslator.RightsFromAccessMask(base.AccessMask);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003C RID: 60 RVA: 0x000028D1 File Offset: 0x000018D1
		public ActiveDirectorySecurityInheritance InheritanceType
		{
			get
			{
				return ActiveDirectoryInheritanceTranslator.GetEffectiveInheritanceFlags(base.InheritanceFlags, base.PropagationFlags);
			}
		}
	}
}
