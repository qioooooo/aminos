using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace System.DirectoryServices
{
	// Token: 0x0200000D RID: 13
	public class ActiveDirectorySecurity : DirectoryObjectSecurity
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000022AC File Offset: 0x000012AC
		public ActiveDirectorySecurity()
		{
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000022BC File Offset: 0x000012BC
		internal ActiveDirectorySecurity(byte[] sdBinaryForm, SecurityMasks securityMask)
			: base(new CommonSecurityDescriptor(true, true, sdBinaryForm, 0))
		{
			this.securityMaskUsedInRetrieval = securityMask;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000022DC File Offset: 0x000012DC
		public void AddAccessRule(ActiveDirectoryAccessRule rule)
		{
			if (!this.DaclRetrieved())
			{
				throw new InvalidOperationException(Res.GetString("CannotModifyDacl"));
			}
			base.AddAccessRule(rule);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022FD File Offset: 0x000012FD
		public void SetAccessRule(ActiveDirectoryAccessRule rule)
		{
			if (!this.DaclRetrieved())
			{
				throw new InvalidOperationException(Res.GetString("CannotModifyDacl"));
			}
			base.SetAccessRule(rule);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000231E File Offset: 0x0000131E
		public void ResetAccessRule(ActiveDirectoryAccessRule rule)
		{
			if (!this.DaclRetrieved())
			{
				throw new InvalidOperationException(Res.GetString("CannotModifyDacl"));
			}
			base.ResetAccessRule(rule);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002340 File Offset: 0x00001340
		public void RemoveAccess(IdentityReference identity, AccessControlType type)
		{
			if (!this.DaclRetrieved())
			{
				throw new InvalidOperationException(Res.GetString("CannotModifyDacl"));
			}
			ActiveDirectoryAccessRule activeDirectoryAccessRule = new ActiveDirectoryAccessRule(identity, ActiveDirectoryRights.GenericRead, type, ActiveDirectorySecurityInheritance.None);
			base.RemoveAccessRuleAll(activeDirectoryAccessRule);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000237A File Offset: 0x0000137A
		public bool RemoveAccessRule(ActiveDirectoryAccessRule rule)
		{
			if (!this.DaclRetrieved())
			{
				throw new InvalidOperationException(Res.GetString("CannotModifyDacl"));
			}
			return base.RemoveAccessRule(rule);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000239B File Offset: 0x0000139B
		public void RemoveAccessRuleSpecific(ActiveDirectoryAccessRule rule)
		{
			if (!this.DaclRetrieved())
			{
				throw new InvalidOperationException(Res.GetString("CannotModifyDacl"));
			}
			base.RemoveAccessRuleSpecific(rule);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000023BC File Offset: 0x000013BC
		public override bool ModifyAccessRule(AccessControlModification modification, AccessRule rule, out bool modified)
		{
			if (!this.DaclRetrieved())
			{
				throw new InvalidOperationException(Res.GetString("CannotModifyDacl"));
			}
			return base.ModifyAccessRule(modification, rule, out modified);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000023DF File Offset: 0x000013DF
		public override void PurgeAccessRules(IdentityReference identity)
		{
			if (!this.DaclRetrieved())
			{
				throw new InvalidOperationException(Res.GetString("CannotModifyDacl"));
			}
			base.PurgeAccessRules(identity);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002400 File Offset: 0x00001400
		public void AddAuditRule(ActiveDirectoryAuditRule rule)
		{
			if (!this.SaclRetrieved())
			{
				throw new InvalidOperationException(Res.GetString("CannotModifySacl"));
			}
			base.AddAuditRule(rule);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002421 File Offset: 0x00001421
		public void SetAuditRule(ActiveDirectoryAuditRule rule)
		{
			if (!this.SaclRetrieved())
			{
				throw new InvalidOperationException(Res.GetString("CannotModifySacl"));
			}
			base.SetAuditRule(rule);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002444 File Offset: 0x00001444
		public void RemoveAudit(IdentityReference identity)
		{
			if (!this.SaclRetrieved())
			{
				throw new InvalidOperationException(Res.GetString("CannotModifySacl"));
			}
			ActiveDirectoryAuditRule activeDirectoryAuditRule = new ActiveDirectoryAuditRule(identity, ActiveDirectoryRights.GenericRead, AuditFlags.Success | AuditFlags.Failure, ActiveDirectorySecurityInheritance.None);
			base.RemoveAuditRuleAll(activeDirectoryAuditRule);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000247E File Offset: 0x0000147E
		public bool RemoveAuditRule(ActiveDirectoryAuditRule rule)
		{
			if (!this.SaclRetrieved())
			{
				throw new InvalidOperationException(Res.GetString("CannotModifySacl"));
			}
			return base.RemoveAuditRule(rule);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000249F File Offset: 0x0000149F
		public void RemoveAuditRuleSpecific(ActiveDirectoryAuditRule rule)
		{
			if (!this.SaclRetrieved())
			{
				throw new InvalidOperationException(Res.GetString("CannotModifySacl"));
			}
			base.RemoveAuditRuleSpecific(rule);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000024C0 File Offset: 0x000014C0
		public override bool ModifyAuditRule(AccessControlModification modification, AuditRule rule, out bool modified)
		{
			if (!this.SaclRetrieved())
			{
				throw new InvalidOperationException(Res.GetString("CannotModifySacl"));
			}
			return base.ModifyAuditRule(modification, rule, out modified);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000024E3 File Offset: 0x000014E3
		public override void PurgeAuditRules(IdentityReference identity)
		{
			if (!this.SaclRetrieved())
			{
				throw new InvalidOperationException(Res.GetString("CannotModifySacl"));
			}
			base.PurgeAuditRules(identity);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002504 File Offset: 0x00001504
		public sealed override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new ActiveDirectoryAccessRule(identityReference, accessMask, type, Guid.Empty, isInherited, inheritanceFlags, propagationFlags, Guid.Empty);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000252C File Offset: 0x0000152C
		public sealed override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type, Guid objectGuid, Guid inheritedObjectGuid)
		{
			return new ActiveDirectoryAccessRule(identityReference, accessMask, type, objectGuid, isInherited, inheritanceFlags, propagationFlags, inheritedObjectGuid);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000254C File Offset: 0x0000154C
		public sealed override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new ActiveDirectoryAuditRule(identityReference, accessMask, flags, Guid.Empty, isInherited, inheritanceFlags, propagationFlags, Guid.Empty);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002574 File Offset: 0x00001574
		public sealed override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags, Guid objectGuid, Guid inheritedObjectGuid)
		{
			return new ActiveDirectoryAuditRule(identityReference, accessMask, flags, objectGuid, isInherited, inheritanceFlags, propagationFlags, inheritedObjectGuid);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002594 File Offset: 0x00001594
		internal bool IsModified()
		{
			base.ReadLock();
			bool flag;
			try
			{
				flag = base.OwnerModified || base.GroupModified || base.AccessRulesModified || base.AuditRulesModified;
			}
			finally
			{
				base.ReadUnlock();
			}
			return flag;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000025E4 File Offset: 0x000015E4
		private bool DaclRetrieved()
		{
			return (this.securityMaskUsedInRetrieval & SecurityMasks.Dacl) != SecurityMasks.None;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000025F4 File Offset: 0x000015F4
		private bool SaclRetrieved()
		{
			return (this.securityMaskUsedInRetrieval & SecurityMasks.Sacl) != SecurityMasks.None;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002604 File Offset: 0x00001604
		public override Type AccessRightType
		{
			get
			{
				return typeof(ActiveDirectoryRights);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002610 File Offset: 0x00001610
		public override Type AccessRuleType
		{
			get
			{
				return typeof(ActiveDirectoryAccessRule);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000029 RID: 41 RVA: 0x0000261C File Offset: 0x0000161C
		public override Type AuditRuleType
		{
			get
			{
				return typeof(ActiveDirectoryAuditRule);
			}
		}

		// Token: 0x0400014D RID: 333
		private SecurityMasks securityMaskUsedInRetrieval = SecurityMasks.Owner | SecurityMasks.Group | SecurityMasks.Dacl | SecurityMasks.Sacl;
	}
}
