using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace System.DirectoryServices
{
	// Token: 0x02000015 RID: 21
	public sealed class PropertyAccessRule : ActiveDirectoryAccessRule
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00002B4C File Offset: 0x00001B4C
		public PropertyAccessRule(IdentityReference identity, AccessControlType type, PropertyAccess access)
			: base(identity, PropertyAccessTranslator.AccessMaskFromPropertyAccess(access), type, Guid.Empty, false, InheritanceFlags.None, PropagationFlags.None, Guid.Empty)
		{
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002B74 File Offset: 0x00001B74
		public PropertyAccessRule(IdentityReference identity, AccessControlType type, PropertyAccess access, Guid propertyType)
			: base(identity, PropertyAccessTranslator.AccessMaskFromPropertyAccess(access), type, propertyType, false, InheritanceFlags.None, PropagationFlags.None, Guid.Empty)
		{
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002B9C File Offset: 0x00001B9C
		public PropertyAccessRule(IdentityReference identity, AccessControlType type, PropertyAccess access, ActiveDirectorySecurityInheritance inheritanceType)
			: base(identity, PropertyAccessTranslator.AccessMaskFromPropertyAccess(access), type, Guid.Empty, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), Guid.Empty)
		{
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002BD0 File Offset: 0x00001BD0
		public PropertyAccessRule(IdentityReference identity, AccessControlType type, PropertyAccess access, Guid propertyType, ActiveDirectorySecurityInheritance inheritanceType)
			: base(identity, PropertyAccessTranslator.AccessMaskFromPropertyAccess(access), type, propertyType, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), Guid.Empty)
		{
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002C04 File Offset: 0x00001C04
		public PropertyAccessRule(IdentityReference identity, AccessControlType type, PropertyAccess access, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
			: base(identity, PropertyAccessTranslator.AccessMaskFromPropertyAccess(access), type, Guid.Empty, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), inheritedObjectType)
		{
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002C38 File Offset: 0x00001C38
		public PropertyAccessRule(IdentityReference identity, AccessControlType type, PropertyAccess access, Guid propertyType, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
			: base(identity, PropertyAccessTranslator.AccessMaskFromPropertyAccess(access), type, propertyType, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), inheritedObjectType)
		{
		}
	}
}
