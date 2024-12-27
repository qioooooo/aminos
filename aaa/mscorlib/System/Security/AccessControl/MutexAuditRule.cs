using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000915 RID: 2325
	public sealed class MutexAuditRule : AuditRule
	{
		// Token: 0x0600545A RID: 21594 RVA: 0x00132715 File Offset: 0x00131715
		public MutexAuditRule(IdentityReference identity, MutexRights eventRights, AuditFlags flags)
			: this(identity, (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, flags)
		{
		}

		// Token: 0x0600545B RID: 21595 RVA: 0x00132723 File Offset: 0x00131723
		internal MutexAuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
			: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x0600545C RID: 21596 RVA: 0x00132734 File Offset: 0x00131734
		public MutexRights MutexRights
		{
			get
			{
				return (MutexRights)base.AccessMask;
			}
		}
	}
}
