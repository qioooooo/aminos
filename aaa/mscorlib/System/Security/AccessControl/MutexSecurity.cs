using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Security.AccessControl
{
	// Token: 0x02000916 RID: 2326
	public sealed class MutexSecurity : NativeObjectSecurity
	{
		// Token: 0x0600545D RID: 21597 RVA: 0x0013273C File Offset: 0x0013173C
		public MutexSecurity()
			: base(true, ResourceType.KernelObject)
		{
		}

		// Token: 0x0600545E RID: 21598 RVA: 0x00132746 File Offset: 0x00131746
		public MutexSecurity(string name, AccessControlSections includeSections)
			: base(true, ResourceType.KernelObject, name, includeSections, new NativeObjectSecurity.ExceptionFromErrorCode(MutexSecurity._HandleErrorCode), null)
		{
		}

		// Token: 0x0600545F RID: 21599 RVA: 0x0013275F File Offset: 0x0013175F
		internal MutexSecurity(SafeWaitHandle handle, AccessControlSections includeSections)
			: base(true, ResourceType.KernelObject, handle, includeSections, new NativeObjectSecurity.ExceptionFromErrorCode(MutexSecurity._HandleErrorCode), null)
		{
		}

		// Token: 0x06005460 RID: 21600 RVA: 0x00132778 File Offset: 0x00131778
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

		// Token: 0x06005461 RID: 21601 RVA: 0x001327C6 File Offset: 0x001317C6
		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new MutexAccessRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, type);
		}

		// Token: 0x06005462 RID: 21602 RVA: 0x001327D6 File Offset: 0x001317D6
		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new MutexAuditRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, flags);
		}

		// Token: 0x06005463 RID: 21603 RVA: 0x001327E8 File Offset: 0x001317E8
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

		// Token: 0x06005464 RID: 21604 RVA: 0x00132828 File Offset: 0x00131828
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

		// Token: 0x06005465 RID: 21605 RVA: 0x0013288C File Offset: 0x0013188C
		public void AddAccessRule(MutexAccessRule rule)
		{
			base.AddAccessRule(rule);
		}

		// Token: 0x06005466 RID: 21606 RVA: 0x00132895 File Offset: 0x00131895
		public void SetAccessRule(MutexAccessRule rule)
		{
			base.SetAccessRule(rule);
		}

		// Token: 0x06005467 RID: 21607 RVA: 0x0013289E File Offset: 0x0013189E
		public void ResetAccessRule(MutexAccessRule rule)
		{
			base.ResetAccessRule(rule);
		}

		// Token: 0x06005468 RID: 21608 RVA: 0x001328A7 File Offset: 0x001318A7
		public bool RemoveAccessRule(MutexAccessRule rule)
		{
			return base.RemoveAccessRule(rule);
		}

		// Token: 0x06005469 RID: 21609 RVA: 0x001328B0 File Offset: 0x001318B0
		public void RemoveAccessRuleAll(MutexAccessRule rule)
		{
			base.RemoveAccessRuleAll(rule);
		}

		// Token: 0x0600546A RID: 21610 RVA: 0x001328B9 File Offset: 0x001318B9
		public void RemoveAccessRuleSpecific(MutexAccessRule rule)
		{
			base.RemoveAccessRuleSpecific(rule);
		}

		// Token: 0x0600546B RID: 21611 RVA: 0x001328C2 File Offset: 0x001318C2
		public void AddAuditRule(MutexAuditRule rule)
		{
			base.AddAuditRule(rule);
		}

		// Token: 0x0600546C RID: 21612 RVA: 0x001328CB File Offset: 0x001318CB
		public void SetAuditRule(MutexAuditRule rule)
		{
			base.SetAuditRule(rule);
		}

		// Token: 0x0600546D RID: 21613 RVA: 0x001328D4 File Offset: 0x001318D4
		public bool RemoveAuditRule(MutexAuditRule rule)
		{
			return base.RemoveAuditRule(rule);
		}

		// Token: 0x0600546E RID: 21614 RVA: 0x001328DD File Offset: 0x001318DD
		public void RemoveAuditRuleAll(MutexAuditRule rule)
		{
			base.RemoveAuditRuleAll(rule);
		}

		// Token: 0x0600546F RID: 21615 RVA: 0x001328E6 File Offset: 0x001318E6
		public void RemoveAuditRuleSpecific(MutexAuditRule rule)
		{
			base.RemoveAuditRuleSpecific(rule);
		}

		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x06005470 RID: 21616 RVA: 0x001328EF File Offset: 0x001318EF
		public override Type AccessRightType
		{
			get
			{
				return typeof(MutexRights);
			}
		}

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x06005471 RID: 21617 RVA: 0x001328FB File Offset: 0x001318FB
		public override Type AccessRuleType
		{
			get
			{
				return typeof(MutexAccessRule);
			}
		}

		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x06005472 RID: 21618 RVA: 0x00132907 File Offset: 0x00131907
		public override Type AuditRuleType
		{
			get
			{
				return typeof(MutexAuditRule);
			}
		}
	}
}
