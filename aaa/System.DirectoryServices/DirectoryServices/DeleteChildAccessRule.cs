using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace System.DirectoryServices
{
	// Token: 0x02000014 RID: 20
	public sealed class DeleteChildAccessRule : ActiveDirectoryAccessRule
	{
		// Token: 0x06000046 RID: 70 RVA: 0x00002A58 File Offset: 0x00001A58
		public DeleteChildAccessRule(IdentityReference identity, AccessControlType type)
			: base(identity, 2, type, Guid.Empty, false, InheritanceFlags.None, PropagationFlags.None, Guid.Empty)
		{
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002A7C File Offset: 0x00001A7C
		public DeleteChildAccessRule(IdentityReference identity, AccessControlType type, Guid childType)
			: base(identity, 2, type, childType, false, InheritanceFlags.None, PropagationFlags.None, Guid.Empty)
		{
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002A9C File Offset: 0x00001A9C
		public DeleteChildAccessRule(IdentityReference identity, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType)
			: base(identity, 2, type, Guid.Empty, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), Guid.Empty)
		{
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002ACC File Offset: 0x00001ACC
		public DeleteChildAccessRule(IdentityReference identity, AccessControlType type, Guid childType, ActiveDirectorySecurityInheritance inheritanceType)
			: base(identity, 2, type, childType, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), Guid.Empty)
		{
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002AF8 File Offset: 0x00001AF8
		public DeleteChildAccessRule(IdentityReference identity, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
			: base(identity, 2, type, Guid.Empty, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), inheritedObjectType)
		{
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002B24 File Offset: 0x00001B24
		public DeleteChildAccessRule(IdentityReference identity, AccessControlType type, Guid childType, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
			: base(identity, 2, type, childType, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), inheritedObjectType)
		{
		}
	}
}
