using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Security.AccessControl
{
	// Token: 0x0200090C RID: 2316
	public sealed class EventWaitHandleSecurity : NativeObjectSecurity
	{
		// Token: 0x06005415 RID: 21525 RVA: 0x00131E99 File Offset: 0x00130E99
		public EventWaitHandleSecurity()
			: base(true, ResourceType.KernelObject)
		{
		}

		// Token: 0x06005416 RID: 21526 RVA: 0x00131EA3 File Offset: 0x00130EA3
		internal EventWaitHandleSecurity(string name, AccessControlSections includeSections)
			: base(true, ResourceType.KernelObject, name, includeSections, new NativeObjectSecurity.ExceptionFromErrorCode(EventWaitHandleSecurity._HandleErrorCode), null)
		{
		}

		// Token: 0x06005417 RID: 21527 RVA: 0x00131EBC File Offset: 0x00130EBC
		internal EventWaitHandleSecurity(SafeWaitHandle handle, AccessControlSections includeSections)
			: base(true, ResourceType.KernelObject, handle, includeSections, new NativeObjectSecurity.ExceptionFromErrorCode(EventWaitHandleSecurity._HandleErrorCode), null)
		{
		}

		// Token: 0x06005418 RID: 21528 RVA: 0x00131ED8 File Offset: 0x00130ED8
		private static Exception _HandleErrorCode(int errorCode, string name, SafeHandle handle, object context)
		{
			Exception ex = null;
			if (errorCode == 2 || errorCode == 6 || errorCode == 123)
			{
				if (name != null && name.Length != 0)
				{
					ex = new WaitHandleCannotBeOpenedException(Environment.GetResourceString("Threading.WaitHandleCannotBeOpenedException_InvalidHandle", new object[] { name }));
				}
				else
				{
					ex = new WaitHandleCannotBeOpenedException();
				}
			}
			return ex;
		}

		// Token: 0x06005419 RID: 21529 RVA: 0x00131F26 File Offset: 0x00130F26
		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new EventWaitHandleAccessRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, type);
		}

		// Token: 0x0600541A RID: 21530 RVA: 0x00131F36 File Offset: 0x00130F36
		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new EventWaitHandleAuditRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, flags);
		}

		// Token: 0x0600541B RID: 21531 RVA: 0x00131F48 File Offset: 0x00130F48
		internal AccessControlSections GetAccessControlSectionsFromChanges()
		{
			AccessControlSections accessControlSections = AccessControlSections.None;
			if (base.AccessRulesModified)
			{
				accessControlSections = AccessControlSections.Access;
			}
			if (base.AuditRulesModified)
			{
				accessControlSections |= AccessControlSections.Audit;
			}
			if (base.OwnerModified)
			{
				accessControlSections |= AccessControlSections.Owner;
			}
			if (base.GroupModified)
			{
				accessControlSections |= AccessControlSections.Group;
			}
			return accessControlSections;
		}

		// Token: 0x0600541C RID: 21532 RVA: 0x00131F88 File Offset: 0x00130F88
		internal void Persist(SafeWaitHandle handle)
		{
			base.WriteLock();
			try
			{
				AccessControlSections accessControlSectionsFromChanges = this.GetAccessControlSectionsFromChanges();
				if (accessControlSectionsFromChanges != AccessControlSections.None)
				{
					base.Persist(handle, accessControlSectionsFromChanges);
					base.OwnerModified = (base.GroupModified = (base.AuditRulesModified = (base.AccessRulesModified = false)));
				}
			}
			finally
			{
				base.WriteUnlock();
			}
		}

		// Token: 0x0600541D RID: 21533 RVA: 0x00131FEC File Offset: 0x00130FEC
		public void AddAccessRule(EventWaitHandleAccessRule rule)
		{
			base.AddAccessRule(rule);
		}

		// Token: 0x0600541E RID: 21534 RVA: 0x00131FF5 File Offset: 0x00130FF5
		public void SetAccessRule(EventWaitHandleAccessRule rule)
		{
			base.SetAccessRule(rule);
		}

		// Token: 0x0600541F RID: 21535 RVA: 0x00131FFE File Offset: 0x00130FFE
		public void ResetAccessRule(EventWaitHandleAccessRule rule)
		{
			base.ResetAccessRule(rule);
		}

		// Token: 0x06005420 RID: 21536 RVA: 0x00132007 File Offset: 0x00131007
		public bool RemoveAccessRule(EventWaitHandleAccessRule rule)
		{
			return base.RemoveAccessRule(rule);
		}

		// Token: 0x06005421 RID: 21537 RVA: 0x00132010 File Offset: 0x00131010
		public void RemoveAccessRuleAll(EventWaitHandleAccessRule rule)
		{
			base.RemoveAccessRuleAll(rule);
		}

		// Token: 0x06005422 RID: 21538 RVA: 0x00132019 File Offset: 0x00131019
		public void RemoveAccessRuleSpecific(EventWaitHandleAccessRule rule)
		{
			base.RemoveAccessRuleSpecific(rule);
		}

		// Token: 0x06005423 RID: 21539 RVA: 0x00132022 File Offset: 0x00131022
		public void AddAuditRule(EventWaitHandleAuditRule rule)
		{
			base.AddAuditRule(rule);
		}

		// Token: 0x06005424 RID: 21540 RVA: 0x0013202B File Offset: 0x0013102B
		public void SetAuditRule(EventWaitHandleAuditRule rule)
		{
			base.SetAuditRule(rule);
		}

		// Token: 0x06005425 RID: 21541 RVA: 0x00132034 File Offset: 0x00131034
		public bool RemoveAuditRule(EventWaitHandleAuditRule rule)
		{
			return base.RemoveAuditRule(rule);
		}

		// Token: 0x06005426 RID: 21542 RVA: 0x0013203D File Offset: 0x0013103D
		public void RemoveAuditRuleAll(EventWaitHandleAuditRule rule)
		{
			base.RemoveAuditRuleAll(rule);
		}

		// Token: 0x06005427 RID: 21543 RVA: 0x00132046 File Offset: 0x00131046
		public void RemoveAuditRuleSpecific(EventWaitHandleAuditRule rule)
		{
			base.RemoveAuditRuleSpecific(rule);
		}

		// Token: 0x17000E95 RID: 3733
		// (get) Token: 0x06005428 RID: 21544 RVA: 0x0013204F File Offset: 0x0013104F
		public override Type AccessRightType
		{
			get
			{
				return typeof(EventWaitHandleRights);
			}
		}

		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x06005429 RID: 21545 RVA: 0x0013205B File Offset: 0x0013105B
		public override Type AccessRuleType
		{
			get
			{
				return typeof(EventWaitHandleAccessRule);
			}
		}

		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x0600542A RID: 21546 RVA: 0x00132067 File Offset: 0x00131067
		public override Type AuditRuleType
		{
			get
			{
				return typeof(EventWaitHandleAuditRule);
			}
		}
	}
}
