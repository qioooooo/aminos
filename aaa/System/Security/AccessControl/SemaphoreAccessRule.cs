using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000224 RID: 548
	[ComVisible(false)]
	public sealed class SemaphoreAccessRule : AccessRule
	{
		// Token: 0x06001266 RID: 4710 RVA: 0x0003E308 File Offset: 0x0003D308
		public SemaphoreAccessRule(IdentityReference identity, SemaphoreRights eventRights, AccessControlType type)
			: this(identity, (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x0003E316 File Offset: 0x0003D316
		public SemaphoreAccessRule(string identity, SemaphoreRights eventRights, AccessControlType type)
			: this(new NTAccount(identity), (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x0003E329 File Offset: 0x0003D329
		internal SemaphoreAccessRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
			: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06001269 RID: 4713 RVA: 0x0003E33A File Offset: 0x0003D33A
		public SemaphoreRights SemaphoreRights
		{
			get
			{
				return (SemaphoreRights)base.AccessMask;
			}
		}
	}
}
