using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using Microsoft.Win32.SafeHandles;

namespace System.Security.AccessControl
{
	// Token: 0x0200091F RID: 2335
	public sealed class RegistrySecurity : NativeObjectSecurity
	{
		// Token: 0x060054AC RID: 21676 RVA: 0x001340F2 File Offset: 0x001330F2
		public RegistrySecurity()
			: base(true, ResourceType.RegistryKey)
		{
		}

		// Token: 0x060054AD RID: 21677 RVA: 0x001340FC File Offset: 0x001330FC
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		internal RegistrySecurity(SafeRegistryHandle hKey, string name, AccessControlSections includeSections)
			: base(true, ResourceType.RegistryKey, hKey, includeSections, new NativeObjectSecurity.ExceptionFromErrorCode(RegistrySecurity._HandleErrorCode), null)
		{
			new RegistryPermission(RegistryPermissionAccess.NoAccess, AccessControlActions.View, name).Demand();
		}

		// Token: 0x060054AE RID: 21678 RVA: 0x00134124 File Offset: 0x00133124
		private static Exception _HandleErrorCode(int errorCode, string name, SafeHandle handle, object context)
		{
			Exception ex = null;
			if (errorCode != 2)
			{
				if (errorCode != 6)
				{
					if (errorCode == 123)
					{
						ex = new ArgumentException(Environment.GetResourceString("Arg_RegInvalidKeyName", new object[] { "name" }));
					}
				}
				else
				{
					ex = new ArgumentException(Environment.GetResourceString("AccessControl_InvalidHandle"));
				}
			}
			else
			{
				ex = new IOException(Environment.GetResourceString("Arg_RegKeyNotFound", new object[] { errorCode }));
			}
			return ex;
		}

		// Token: 0x060054AF RID: 21679 RVA: 0x0013419A File Offset: 0x0013319A
		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new RegistryAccessRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, type);
		}

		// Token: 0x060054B0 RID: 21680 RVA: 0x001341AA File Offset: 0x001331AA
		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new RegistryAuditRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, flags);
		}

		// Token: 0x060054B1 RID: 21681 RVA: 0x001341BC File Offset: 0x001331BC
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

		// Token: 0x060054B2 RID: 21682 RVA: 0x001341FC File Offset: 0x001331FC
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		internal void Persist(SafeRegistryHandle hKey, string keyName)
		{
			new RegistryPermission(RegistryPermissionAccess.NoAccess, AccessControlActions.Change, keyName).Demand();
			base.WriteLock();
			try
			{
				AccessControlSections accessControlSectionsFromChanges = this.GetAccessControlSectionsFromChanges();
				if (accessControlSectionsFromChanges != AccessControlSections.None)
				{
					base.Persist(hKey, accessControlSectionsFromChanges);
					base.OwnerModified = (base.GroupModified = (base.AuditRulesModified = (base.AccessRulesModified = false)));
				}
			}
			finally
			{
				base.WriteUnlock();
			}
		}

		// Token: 0x060054B3 RID: 21683 RVA: 0x0013426C File Offset: 0x0013326C
		public void AddAccessRule(RegistryAccessRule rule)
		{
			base.AddAccessRule(rule);
		}

		// Token: 0x060054B4 RID: 21684 RVA: 0x00134275 File Offset: 0x00133275
		public void SetAccessRule(RegistryAccessRule rule)
		{
			base.SetAccessRule(rule);
		}

		// Token: 0x060054B5 RID: 21685 RVA: 0x0013427E File Offset: 0x0013327E
		public void ResetAccessRule(RegistryAccessRule rule)
		{
			base.ResetAccessRule(rule);
		}

		// Token: 0x060054B6 RID: 21686 RVA: 0x00134287 File Offset: 0x00133287
		public bool RemoveAccessRule(RegistryAccessRule rule)
		{
			return base.RemoveAccessRule(rule);
		}

		// Token: 0x060054B7 RID: 21687 RVA: 0x00134290 File Offset: 0x00133290
		public void RemoveAccessRuleAll(RegistryAccessRule rule)
		{
			base.RemoveAccessRuleAll(rule);
		}

		// Token: 0x060054B8 RID: 21688 RVA: 0x00134299 File Offset: 0x00133299
		public void RemoveAccessRuleSpecific(RegistryAccessRule rule)
		{
			base.RemoveAccessRuleSpecific(rule);
		}

		// Token: 0x060054B9 RID: 21689 RVA: 0x001342A2 File Offset: 0x001332A2
		public void AddAuditRule(RegistryAuditRule rule)
		{
			base.AddAuditRule(rule);
		}

		// Token: 0x060054BA RID: 21690 RVA: 0x001342AB File Offset: 0x001332AB
		public void SetAuditRule(RegistryAuditRule rule)
		{
			base.SetAuditRule(rule);
		}

		// Token: 0x060054BB RID: 21691 RVA: 0x001342B4 File Offset: 0x001332B4
		public bool RemoveAuditRule(RegistryAuditRule rule)
		{
			return base.RemoveAuditRule(rule);
		}

		// Token: 0x060054BC RID: 21692 RVA: 0x001342BD File Offset: 0x001332BD
		public void RemoveAuditRuleAll(RegistryAuditRule rule)
		{
			base.RemoveAuditRuleAll(rule);
		}

		// Token: 0x060054BD RID: 21693 RVA: 0x001342C6 File Offset: 0x001332C6
		public void RemoveAuditRuleSpecific(RegistryAuditRule rule)
		{
			base.RemoveAuditRuleSpecific(rule);
		}

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x060054BE RID: 21694 RVA: 0x001342CF File Offset: 0x001332CF
		public override Type AccessRightType
		{
			get
			{
				return typeof(RegistryRights);
			}
		}

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x060054BF RID: 21695 RVA: 0x001342DB File Offset: 0x001332DB
		public override Type AccessRuleType
		{
			get
			{
				return typeof(RegistryAccessRule);
			}
		}

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x060054C0 RID: 21696 RVA: 0x001342E7 File Offset: 0x001332E7
		public override Type AuditRuleType
		{
			get
			{
				return typeof(RegistryAuditRule);
			}
		}
	}
}
