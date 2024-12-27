using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x0200090A RID: 2314
	public sealed class EventWaitHandleAccessRule : AccessRule
	{
		// Token: 0x0600540E RID: 21518 RVA: 0x00131E38 File Offset: 0x00130E38
		public EventWaitHandleAccessRule(IdentityReference identity, EventWaitHandleRights eventRights, AccessControlType type)
			: this(identity, (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x0600540F RID: 21519 RVA: 0x00131E46 File Offset: 0x00130E46
		public EventWaitHandleAccessRule(string identity, EventWaitHandleRights eventRights, AccessControlType type)
			: this(new NTAccount(identity), (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x06005410 RID: 21520 RVA: 0x00131E59 File Offset: 0x00130E59
		internal EventWaitHandleAccessRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
			: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x17000E93 RID: 3731
		// (get) Token: 0x06005411 RID: 21521 RVA: 0x00131E6A File Offset: 0x00130E6A
		public EventWaitHandleRights EventWaitHandleRights
		{
			get
			{
				return (EventWaitHandleRights)base.AccessMask;
			}
		}
	}
}
