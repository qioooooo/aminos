using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000921 RID: 2337
	public abstract class ObjectAccessRule : AccessRule
	{
		// Token: 0x060054C1 RID: 21697 RVA: 0x001342F4 File Offset: 0x001332F4
		protected ObjectAccessRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, Guid objectType, Guid inheritedObjectType, AccessControlType type)
			: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, type)
		{
			if (!objectType.Equals(Guid.Empty) && (accessMask & ObjectAce.AccessMaskWithObjectType) != 0)
			{
				this._objectType = objectType;
				this._objectFlags |= ObjectAceFlags.ObjectAceTypePresent;
			}
			else
			{
				this._objectType = Guid.Empty;
			}
			if (!inheritedObjectType.Equals(Guid.Empty) && (inheritanceFlags & InheritanceFlags.ContainerInherit) != InheritanceFlags.None)
			{
				this._inheritedObjectType = inheritedObjectType;
				this._objectFlags |= ObjectAceFlags.InheritedObjectAceTypePresent;
				return;
			}
			this._inheritedObjectType = Guid.Empty;
		}

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x060054C2 RID: 21698 RVA: 0x00134380 File Offset: 0x00133380
		public Guid ObjectType
		{
			get
			{
				return this._objectType;
			}
		}

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x060054C3 RID: 21699 RVA: 0x00134388 File Offset: 0x00133388
		public Guid InheritedObjectType
		{
			get
			{
				return this._inheritedObjectType;
			}
		}

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x060054C4 RID: 21700 RVA: 0x00134390 File Offset: 0x00133390
		public ObjectAceFlags ObjectFlags
		{
			get
			{
				return this._objectFlags;
			}
		}

		// Token: 0x04002BE7 RID: 11239
		private readonly Guid _objectType;

		// Token: 0x04002BE8 RID: 11240
		private readonly Guid _inheritedObjectType;

		// Token: 0x04002BE9 RID: 11241
		private readonly ObjectAceFlags _objectFlags;
	}
}
