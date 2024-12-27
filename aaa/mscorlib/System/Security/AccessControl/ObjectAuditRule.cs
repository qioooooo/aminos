using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000922 RID: 2338
	public abstract class ObjectAuditRule : AuditRule
	{
		// Token: 0x060054C5 RID: 21701 RVA: 0x00134398 File Offset: 0x00133398
		protected ObjectAuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, Guid objectType, Guid inheritedObjectType, AuditFlags auditFlags)
			: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, auditFlags)
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

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x060054C6 RID: 21702 RVA: 0x00134424 File Offset: 0x00133424
		public Guid ObjectType
		{
			get
			{
				return this._objectType;
			}
		}

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x060054C7 RID: 21703 RVA: 0x0013442C File Offset: 0x0013342C
		public Guid InheritedObjectType
		{
			get
			{
				return this._inheritedObjectType;
			}
		}

		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x060054C8 RID: 21704 RVA: 0x00134434 File Offset: 0x00133434
		public ObjectAceFlags ObjectFlags
		{
			get
			{
				return this._objectFlags;
			}
		}

		// Token: 0x04002BEA RID: 11242
		private readonly Guid _objectType;

		// Token: 0x04002BEB RID: 11243
		private readonly Guid _inheritedObjectType;

		// Token: 0x04002BEC RID: 11244
		private readonly ObjectAceFlags _objectFlags;
	}
}
