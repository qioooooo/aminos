using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace System.DirectoryServices
{
	// Token: 0x02000013 RID: 19
	public sealed class CreateChildAccessRule : ActiveDirectoryAccessRule
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002964 File Offset: 0x00001964
		public CreateChildAccessRule(IdentityReference identity, AccessControlType type)
			: base(identity, 1, type, Guid.Empty, false, InheritanceFlags.None, PropagationFlags.None, Guid.Empty)
		{
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002988 File Offset: 0x00001988
		public CreateChildAccessRule(IdentityReference identity, AccessControlType type, Guid childType)
			: base(identity, 1, type, childType, false, InheritanceFlags.None, PropagationFlags.None, Guid.Empty)
		{
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000029A8 File Offset: 0x000019A8
		public CreateChildAccessRule(IdentityReference identity, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType)
			: base(identity, 1, type, Guid.Empty, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), Guid.Empty)
		{
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000029D8 File Offset: 0x000019D8
		public CreateChildAccessRule(IdentityReference identity, AccessControlType type, Guid childType, ActiveDirectorySecurityInheritance inheritanceType)
			: base(identity, 1, type, childType, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), Guid.Empty)
		{
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002A04 File Offset: 0x00001A04
		public CreateChildAccessRule(IdentityReference identity, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
			: base(identity, 1, type, Guid.Empty, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), inheritedObjectType)
		{
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002A30 File Offset: 0x00001A30
		public CreateChildAccessRule(IdentityReference identity, AccessControlType type, Guid childType, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
			: base(identity, 1, type, childType, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), inheritedObjectType)
		{
		}
	}
}
