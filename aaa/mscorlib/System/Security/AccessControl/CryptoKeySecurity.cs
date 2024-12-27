using System;
using System.Runtime.CompilerServices;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000901 RID: 2305
	public sealed class CryptoKeySecurity : NativeObjectSecurity
	{
		// Token: 0x060053FB RID: 21499 RVA: 0x00131CFC File Offset: 0x00130CFC
		public CryptoKeySecurity()
			: base(false, ResourceType.FileObject)
		{
		}

		// Token: 0x060053FC RID: 21500 RVA: 0x00131D06 File Offset: 0x00130D06
		public CryptoKeySecurity(CommonSecurityDescriptor securityDescriptor)
			: base(ResourceType.FileObject, securityDescriptor)
		{
		}

		// Token: 0x060053FD RID: 21501 RVA: 0x00131D10 File Offset: 0x00130D10
		public sealed override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new CryptoKeyAccessRule(identityReference, CryptoKeyAccessRule.RightsFromAccessMask(accessMask), type);
		}

		// Token: 0x060053FE RID: 21502 RVA: 0x00131D20 File Offset: 0x00130D20
		public sealed override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new CryptoKeyAuditRule(identityReference, CryptoKeyAuditRule.RightsFromAccessMask(accessMask), flags);
		}

		// Token: 0x060053FF RID: 21503 RVA: 0x00131D30 File Offset: 0x00130D30
		public void AddAccessRule(CryptoKeyAccessRule rule)
		{
			base.AddAccessRule(rule);
		}

		// Token: 0x06005400 RID: 21504 RVA: 0x00131D39 File Offset: 0x00130D39
		public void SetAccessRule(CryptoKeyAccessRule rule)
		{
			base.SetAccessRule(rule);
		}

		// Token: 0x06005401 RID: 21505 RVA: 0x00131D42 File Offset: 0x00130D42
		public void ResetAccessRule(CryptoKeyAccessRule rule)
		{
			base.ResetAccessRule(rule);
		}

		// Token: 0x06005402 RID: 21506 RVA: 0x00131D4B File Offset: 0x00130D4B
		public bool RemoveAccessRule(CryptoKeyAccessRule rule)
		{
			return base.RemoveAccessRule(rule);
		}

		// Token: 0x06005403 RID: 21507 RVA: 0x00131D54 File Offset: 0x00130D54
		public void RemoveAccessRuleAll(CryptoKeyAccessRule rule)
		{
			base.RemoveAccessRuleAll(rule);
		}

		// Token: 0x06005404 RID: 21508 RVA: 0x00131D5D File Offset: 0x00130D5D
		public void RemoveAccessRuleSpecific(CryptoKeyAccessRule rule)
		{
			base.RemoveAccessRuleSpecific(rule);
		}

		// Token: 0x06005405 RID: 21509 RVA: 0x00131D66 File Offset: 0x00130D66
		public void AddAuditRule(CryptoKeyAuditRule rule)
		{
			base.AddAuditRule(rule);
		}

		// Token: 0x06005406 RID: 21510 RVA: 0x00131D6F File Offset: 0x00130D6F
		public void SetAuditRule(CryptoKeyAuditRule rule)
		{
			base.SetAuditRule(rule);
		}

		// Token: 0x06005407 RID: 21511 RVA: 0x00131D78 File Offset: 0x00130D78
		public bool RemoveAuditRule(CryptoKeyAuditRule rule)
		{
			return base.RemoveAuditRule(rule);
		}

		// Token: 0x06005408 RID: 21512 RVA: 0x00131D81 File Offset: 0x00130D81
		public void RemoveAuditRuleAll(CryptoKeyAuditRule rule)
		{
			base.RemoveAuditRuleAll(rule);
		}

		// Token: 0x06005409 RID: 21513 RVA: 0x00131D8A File Offset: 0x00130D8A
		public void RemoveAuditRuleSpecific(CryptoKeyAuditRule rule)
		{
			base.RemoveAuditRuleSpecific(rule);
		}

		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x0600540A RID: 21514 RVA: 0x00131D93 File Offset: 0x00130D93
		public override Type AccessRightType
		{
			get
			{
				return typeof(CryptoKeyRights);
			}
		}

		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x0600540B RID: 21515 RVA: 0x00131D9F File Offset: 0x00130D9F
		public override Type AccessRuleType
		{
			get
			{
				return typeof(CryptoKeyAccessRule);
			}
		}

		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x0600540C RID: 21516 RVA: 0x00131DAB File Offset: 0x00130DAB
		public override Type AuditRuleType
		{
			get
			{
				return typeof(CryptoKeyAuditRule);
			}
		}

		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x0600540D RID: 21517 RVA: 0x00131DB8 File Offset: 0x00130DB8
		internal AccessControlSections ChangedAccessControlSections
		{
			get
			{
				AccessControlSections accessControlSections = AccessControlSections.None;
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
					}
					finally
					{
						base.ReadLock();
						flag = true;
					}
					if (base.AccessRulesModified)
					{
						accessControlSections |= AccessControlSections.Access;
					}
					if (base.AuditRulesModified)
					{
						accessControlSections |= AccessControlSections.Audit;
					}
					if (base.GroupModified)
					{
						accessControlSections |= AccessControlSections.Group;
					}
					if (base.OwnerModified)
					{
						accessControlSections |= AccessControlSections.Owner;
					}
				}
				finally
				{
					if (flag)
					{
						base.ReadUnlock();
					}
				}
				return accessControlSections;
			}
		}

		// Token: 0x04002B49 RID: 11081
		private const ResourceType s_ResourceType = ResourceType.FileObject;
	}
}
