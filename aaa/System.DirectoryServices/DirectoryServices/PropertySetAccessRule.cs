using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace System.DirectoryServices
{
	// Token: 0x02000016 RID: 22
	public sealed class PropertySetAccessRule : ActiveDirectoryAccessRule
	{
		// Token: 0x06000052 RID: 82 RVA: 0x00002C68 File Offset: 0x00001C68
		public PropertySetAccessRule(IdentityReference identity, AccessControlType type, PropertyAccess access, Guid propertySetType)
			: base(identity, PropertyAccessTranslator.AccessMaskFromPropertyAccess(access), type, propertySetType, false, InheritanceFlags.None, PropagationFlags.None, Guid.Empty)
		{
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002C90 File Offset: 0x00001C90
		public PropertySetAccessRule(IdentityReference identity, AccessControlType type, PropertyAccess access, Guid propertySetType, ActiveDirectorySecurityInheritance inheritanceType)
			: base(identity, PropertyAccessTranslator.AccessMaskFromPropertyAccess(access), type, propertySetType, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), Guid.Empty)
		{
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002CC4 File Offset: 0x00001CC4
		public PropertySetAccessRule(IdentityReference identity, AccessControlType type, PropertyAccess access, Guid propertySetType, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
			: base(identity, PropertyAccessTranslator.AccessMaskFromPropertyAccess(access), type, propertySetType, false, ActiveDirectoryInheritanceTranslator.GetInheritanceFlags(inheritanceType), ActiveDirectoryInheritanceTranslator.GetPropagationFlags(inheritanceType), inheritedObjectType)
		{
		}
	}
}
