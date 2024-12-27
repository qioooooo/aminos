using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace System.DirectoryServices
{
	// Token: 0x02000012 RID: 18
	public sealed class ListChildrenAccessRule : ActiveDirectoryAccessRule
	{
		// Token: 0x0600003D RID: 61 RVA: 0x000028E4 File Offset: 0x000018E4
		public ListChildrenAccessRule(IdentityReference identity, AccessControlType type)
			: base(identity, 4, type, Guid.Empty, false, InheritanceFlags.None, PropagationFlags.None, Guid.Empty)
		{
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002908 File Offset: 0x00001908
		public ListChildrenAccessRule(IdentityReference identity, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType)
			: base(identity, 4, type, Guid.Empty, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), Guid.Empty)
		{
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002938 File Offset: 0x00001938
		public ListChildrenAccessRule(IdentityReference identity, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
			: base(identity, 4, type, Guid.Empty, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), inheritedObjectType)
		{
		}
	}
}
