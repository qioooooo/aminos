using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000225 RID: 549
	[ComVisible(false)]
	public sealed class SemaphoreAuditRule : AuditRule
	{
		// Token: 0x0600126A RID: 4714 RVA: 0x0003E342 File Offset: 0x0003D342
		public SemaphoreAuditRule(IdentityReference identity, SemaphoreRights eventRights, AuditFlags flags)
			: this(identity, (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, flags)
		{
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x0003E350 File Offset: 0x0003D350
		internal SemaphoreAuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
			: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x0600126C RID: 4716 RVA: 0x0003E361 File Offset: 0x0003D361
		public SemaphoreRights SemaphoreRights
		{
			get
			{
				return (SemaphoreRights)base.AccessMask;
			}
		}
	}
}
