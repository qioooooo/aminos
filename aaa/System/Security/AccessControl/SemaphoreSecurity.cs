using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Security.AccessControl
{
	// Token: 0x02000226 RID: 550
	[ComVisible(false)]
	public sealed class SemaphoreSecurity : NativeObjectSecurity
	{
		// Token: 0x0600126D RID: 4717 RVA: 0x0003E369 File Offset: 0x0003D369
		public SemaphoreSecurity()
			: base(true, ResourceType.KernelObject)
		{
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x0003E373 File Offset: 0x0003D373
		public SemaphoreSecurity(string name, AccessControlSections includeSections)
			: base(true, ResourceType.KernelObject, name, includeSections, new NativeObjectSecurity.ExceptionFromErrorCode(SemaphoreSecurity._HandleErrorCode), null)
		{
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x0003E38C File Offset: 0x0003D38C
		internal SemaphoreSecurity(SafeWaitHandle handle, AccessControlSections includeSections)
			: base(true, ResourceType.KernelObject, handle, includeSections, new NativeObjectSecurity.ExceptionFromErrorCode(SemaphoreSecurity._HandleErrorCode), null)
		{
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0003E3A8 File Offset: 0x0003D3A8
		private static Exception _HandleErrorCode(int errorCode, string name, SafeHandle handle, object context)
		{
			Exception ex = null;
			if (errorCode == 2 || errorCode == 6 || errorCode == 123)
			{
				if (name != null && name.Length != 0)
				{
					ex = new WaitHandleCannotBeOpenedException(SR.GetString("WaitHandleCannotBeOpenedException_InvalidHandle", new object[] { name }));
				}
				else
				{
					ex = new WaitHandleCannotBeOpenedException();
				}
			}
			return ex;
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x0003E3F6 File Offset: 0x0003D3F6
		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new SemaphoreAccessRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, type);
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x0003E406 File Offset: 0x0003D406
		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new SemaphoreAuditRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, flags);
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x0003E418 File Offset: 0x0003D418
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

		// Token: 0x06001274 RID: 4724 RVA: 0x0003E458 File Offset: 0x0003D458
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

		// Token: 0x06001275 RID: 4725 RVA: 0x0003E4BC File Offset: 0x0003D4BC
		public void AddAccessRule(SemaphoreAccessRule rule)
		{
			base.AddAccessRule(rule);
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x0003E4C5 File Offset: 0x0003D4C5
		public void SetAccessRule(SemaphoreAccessRule rule)
		{
			base.SetAccessRule(rule);
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x0003E4CE File Offset: 0x0003D4CE
		public void ResetAccessRule(SemaphoreAccessRule rule)
		{
			base.ResetAccessRule(rule);
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x0003E4D7 File Offset: 0x0003D4D7
		public bool RemoveAccessRule(SemaphoreAccessRule rule)
		{
			return base.RemoveAccessRule(rule);
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x0003E4E0 File Offset: 0x0003D4E0
		public void RemoveAccessRuleAll(SemaphoreAccessRule rule)
		{
			base.RemoveAccessRuleAll(rule);
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x0003E4E9 File Offset: 0x0003D4E9
		public void RemoveAccessRuleSpecific(SemaphoreAccessRule rule)
		{
			base.RemoveAccessRuleSpecific(rule);
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x0003E4F2 File Offset: 0x0003D4F2
		public void AddAuditRule(SemaphoreAuditRule rule)
		{
			base.AddAuditRule(rule);
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x0003E4FB File Offset: 0x0003D4FB
		public void SetAuditRule(SemaphoreAuditRule rule)
		{
			base.SetAuditRule(rule);
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0003E504 File Offset: 0x0003D504
		public bool RemoveAuditRule(SemaphoreAuditRule rule)
		{
			return base.RemoveAuditRule(rule);
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x0003E50D File Offset: 0x0003D50D
		public void RemoveAuditRuleAll(SemaphoreAuditRule rule)
		{
			base.RemoveAuditRuleAll(rule);
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x0003E516 File Offset: 0x0003D516
		public void RemoveAuditRuleSpecific(SemaphoreAuditRule rule)
		{
			base.RemoveAuditRuleSpecific(rule);
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001280 RID: 4736 RVA: 0x0003E51F File Offset: 0x0003D51F
		public override Type AccessRightType
		{
			get
			{
				return typeof(SemaphoreRights);
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06001281 RID: 4737 RVA: 0x0003E52B File Offset: 0x0003D52B
		public override Type AccessRuleType
		{
			get
			{
				return typeof(SemaphoreAccessRule);
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06001282 RID: 4738 RVA: 0x0003E537 File Offset: 0x0003D537
		public override Type AuditRuleType
		{
			get
			{
				return typeof(SemaphoreAuditRule);
			}
		}
	}
}
