using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x0200090B RID: 2315
	public sealed class EventWaitHandleAuditRule : AuditRule
	{
		// Token: 0x06005412 RID: 21522 RVA: 0x00131E72 File Offset: 0x00130E72
		public EventWaitHandleAuditRule(IdentityReference identity, EventWaitHandleRights eventRights, AuditFlags flags)
			: this(identity, (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, flags)
		{
		}

		// Token: 0x06005413 RID: 21523 RVA: 0x00131E80 File Offset: 0x00130E80
		internal EventWaitHandleAuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
			: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x17000E94 RID: 3732
		// (get) Token: 0x06005414 RID: 21524 RVA: 0x00131E91 File Offset: 0x00130E91
		public EventWaitHandleRights EventWaitHandleRights
		{
			get
			{
				return (EventWaitHandleRights)base.AccessMask;
			}
		}
	}
}
