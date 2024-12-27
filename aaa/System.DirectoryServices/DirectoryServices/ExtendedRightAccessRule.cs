using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace System.DirectoryServices
{
	// Token: 0x02000017 RID: 23
	public sealed class ExtendedRightAccessRule : ActiveDirectoryAccessRule
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00002CF4 File Offset: 0x00001CF4
		public ExtendedRightAccessRule(IdentityReference identity, AccessControlType type)
			: base(identity, 256, type, Guid.Empty, false, InheritanceFlags.None, PropagationFlags.None, Guid.Empty)
		{
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002D1C File Offset: 0x00001D1C
		public ExtendedRightAccessRule(IdentityReference identity, AccessControlType type, Guid extendedRightType)
			: base(identity, 256, type, extendedRightType, false, InheritanceFlags.None, PropagationFlags.None, Guid.Empty)
		{
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002D40 File Offset: 0x00001D40
		public ExtendedRightAccessRule(IdentityReference identity, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType)
			: base(identity, 256, type, Guid.Empty, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), Guid.Empty)
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002D74 File Offset: 0x00001D74
		public ExtendedRightAccessRule(IdentityReference identity, AccessControlType type, Guid extendedRightType, ActiveDirectorySecurityInheritance inheritanceType)
			: base(identity, 256, type, extendedRightType, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), Guid.Empty)
		{
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002DA4 File Offset: 0x00001DA4
		public ExtendedRightAccessRule(IdentityReference identity, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
			: base(identity, 256, type, Guid.Empty, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), inheritedObjectType)
		{
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002DD4 File Offset: 0x00001DD4
		public ExtendedRightAccessRule(IdentityReference identity, AccessControlType type, Guid extendedRightType, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
			: base(identity, 256, type, extendedRightType, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), inheritedObjectType)
		{
		}
	}
}
