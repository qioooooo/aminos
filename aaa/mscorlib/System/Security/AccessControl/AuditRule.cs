using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008FB RID: 2299
	public abstract class AuditRule : AuthorizationRule
	{
		// Token: 0x0600539C RID: 21404 RVA: 0x00130194 File Offset: 0x0012F194
		protected AuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags auditFlags)
			: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags)
		{
			if (auditFlags == AuditFlags.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumAtLeastOneFlag"), "auditFlags");
			}
			if ((auditFlags & ~(AuditFlags.Success | AuditFlags.Failure)) != AuditFlags.None)
			{
				throw new ArgumentOutOfRangeException("auditFlags", Environment.GetResourceString("ArgumentOutOfRange_Enum"));
			}
			this._flags = auditFlags;
		}

		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x0600539D RID: 21405 RVA: 0x001301EB File Offset: 0x0012F1EB
		public AuditFlags AuditFlags
		{
			get
			{
				return this._flags;
			}
		}

		// Token: 0x04002B39 RID: 11065
		private readonly AuditFlags _flags;
	}
}
