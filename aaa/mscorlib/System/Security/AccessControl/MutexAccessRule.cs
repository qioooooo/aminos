using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000914 RID: 2324
	public sealed class MutexAccessRule : AccessRule
	{
		// Token: 0x06005456 RID: 21590 RVA: 0x001326DB File Offset: 0x001316DB
		public MutexAccessRule(IdentityReference identity, MutexRights eventRights, AccessControlType type)
			: this(identity, (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x06005457 RID: 21591 RVA: 0x001326E9 File Offset: 0x001316E9
		public MutexAccessRule(string identity, MutexRights eventRights, AccessControlType type)
			: this(new NTAccount(identity), (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x06005458 RID: 21592 RVA: 0x001326FC File Offset: 0x001316FC
		internal MutexAccessRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
			: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x06005459 RID: 21593 RVA: 0x0013270D File Offset: 0x0013170D
		public MutexRights MutexRights
		{
			get
			{
				return (MutexRights)base.AccessMask;
			}
		}
	}
}
