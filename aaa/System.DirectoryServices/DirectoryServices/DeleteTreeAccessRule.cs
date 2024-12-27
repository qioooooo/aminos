using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace System.DirectoryServices
{
	// Token: 0x02000018 RID: 24
	public sealed class DeleteTreeAccessRule : ActiveDirectoryAccessRule
	{
		// Token: 0x0600005B RID: 91 RVA: 0x00002E00 File Offset: 0x00001E00
		public DeleteTreeAccessRule(IdentityReference identity, AccessControlType type)
			: base(identity, 64, type, Guid.Empty, false, InheritanceFlags.None, PropagationFlags.None, Guid.Empty)
		{
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002E24 File Offset: 0x00001E24
		public DeleteTreeAccessRule(IdentityReference identity, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType)
			: base(identity, 64, type, Guid.Empty, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), Guid.Empty)
		{
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002E54 File Offset: 0x00001E54
		public DeleteTreeAccessRule(IdentityReference identity, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
			: base(identity, 64, type, Guid.Empty, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), inheritedObjectType)
		{
		}
	}
}
