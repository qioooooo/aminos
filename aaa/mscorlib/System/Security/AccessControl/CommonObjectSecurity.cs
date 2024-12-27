using System;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008FE RID: 2302
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class CommonObjectSecurity : ObjectSecurity
	{
		// Token: 0x060053D6 RID: 21462 RVA: 0x00130C36 File Offset: 0x0012FC36
		protected CommonObjectSecurity(bool isContainer)
			: base(isContainer, false)
		{
		}

		// Token: 0x060053D7 RID: 21463 RVA: 0x00130C40 File Offset: 0x0012FC40
		internal CommonObjectSecurity(CommonSecurityDescriptor securityDescriptor)
			: base(securityDescriptor)
		{
		}

		// Token: 0x060053D8 RID: 21464 RVA: 0x00130C4C File Offset: 0x0012FC4C
		private AuthorizationRuleCollection GetRules(bool access, bool includeExplicit, bool includeInherited, Type targetType)
		{
			base.ReadLock();
			AuthorizationRuleCollection authorizationRuleCollection2;
			try
			{
				AuthorizationRuleCollection authorizationRuleCollection = new AuthorizationRuleCollection();
				if (!SecurityIdentifier.IsValidTargetTypeStatic(targetType))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_MustBeIdentityReferenceType"), "targetType");
				}
				CommonAcl commonAcl = null;
				if (access)
				{
					if ((this._securityDescriptor.ControlFlags & ControlFlags.DiscretionaryAclPresent) != ControlFlags.None)
					{
						commonAcl = this._securityDescriptor.DiscretionaryAcl;
					}
				}
				else if ((this._securityDescriptor.ControlFlags & ControlFlags.SystemAclPresent) != ControlFlags.None)
				{
					commonAcl = this._securityDescriptor.SystemAcl;
				}
				if (commonAcl == null)
				{
					authorizationRuleCollection2 = authorizationRuleCollection;
				}
				else
				{
					IdentityReferenceCollection identityReferenceCollection = null;
					if (targetType != typeof(SecurityIdentifier))
					{
						IdentityReferenceCollection identityReferenceCollection2 = new IdentityReferenceCollection(commonAcl.Count);
						for (int i = 0; i < commonAcl.Count; i++)
						{
							CommonAce commonAce = commonAcl[i] as CommonAce;
							if (this.AceNeedsTranslation(commonAce, access, includeExplicit, includeInherited))
							{
								identityReferenceCollection2.Add(commonAce.SecurityIdentifier);
							}
						}
						identityReferenceCollection = identityReferenceCollection2.Translate(targetType);
					}
					int num = 0;
					for (int j = 0; j < commonAcl.Count; j++)
					{
						CommonAce commonAce2 = commonAcl[j] as CommonAce;
						if (this.AceNeedsTranslation(commonAce2, access, includeExplicit, includeInherited))
						{
							IdentityReference identityReference = ((targetType == typeof(SecurityIdentifier)) ? commonAce2.SecurityIdentifier : identityReferenceCollection[num++]);
							if (access)
							{
								AccessControlType accessControlType;
								if (commonAce2.AceQualifier == AceQualifier.AccessAllowed)
								{
									accessControlType = AccessControlType.Allow;
								}
								else
								{
									accessControlType = AccessControlType.Deny;
								}
								authorizationRuleCollection.AddRule(this.AccessRuleFactory(identityReference, commonAce2.AccessMask, commonAce2.IsInherited, commonAce2.InheritanceFlags, commonAce2.PropagationFlags, accessControlType));
							}
							else
							{
								authorizationRuleCollection.AddRule(this.AuditRuleFactory(identityReference, commonAce2.AccessMask, commonAce2.IsInherited, commonAce2.InheritanceFlags, commonAce2.PropagationFlags, commonAce2.AuditFlags));
							}
						}
					}
					authorizationRuleCollection2 = authorizationRuleCollection;
				}
			}
			finally
			{
				base.ReadUnlock();
			}
			return authorizationRuleCollection2;
		}

		// Token: 0x060053D9 RID: 21465 RVA: 0x00130E30 File Offset: 0x0012FE30
		private bool AceNeedsTranslation(CommonAce ace, bool isAccessAce, bool includeExplicit, bool includeInherited)
		{
			if (ace == null)
			{
				return false;
			}
			if (isAccessAce)
			{
				if (ace.AceQualifier != AceQualifier.AccessAllowed && ace.AceQualifier != AceQualifier.AccessDenied)
				{
					return false;
				}
			}
			else if (ace.AceQualifier != AceQualifier.SystemAudit)
			{
				return false;
			}
			return (includeExplicit && (byte)(ace.AceFlags & AceFlags.Inherited) == 0) || (includeInherited && (byte)(ace.AceFlags & AceFlags.Inherited) != 0);
		}

		// Token: 0x060053DA RID: 21466 RVA: 0x00130E8C File Offset: 0x0012FE8C
		protected override bool ModifyAccess(AccessControlModification modification, AccessRule rule, out bool modified)
		{
			base.WriteLock();
			bool flag2;
			try
			{
				bool flag = true;
				if (rule == null)
				{
					throw new ArgumentNullException("rule");
				}
				if (this._securityDescriptor.DiscretionaryAcl == null)
				{
					if (modification == AccessControlModification.Remove || modification == AccessControlModification.RemoveAll || modification == AccessControlModification.RemoveSpecific)
					{
						modified = false;
						return flag;
					}
					this._securityDescriptor.DiscretionaryAcl = new DiscretionaryAcl(base.IsContainer, base.IsDS, GenericAcl.AclRevision, 1);
					this._securityDescriptor.AddControlFlags(ControlFlags.DiscretionaryAclPresent);
				}
				SecurityIdentifier securityIdentifier = rule.IdentityReference.Translate(typeof(SecurityIdentifier)) as SecurityIdentifier;
				if (rule.AccessControlType == AccessControlType.Allow)
				{
					switch (modification)
					{
					case AccessControlModification.Add:
						this._securityDescriptor.DiscretionaryAcl.AddAccess(AccessControlType.Allow, securityIdentifier, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
						break;
					case AccessControlModification.Set:
						this._securityDescriptor.DiscretionaryAcl.SetAccess(AccessControlType.Allow, securityIdentifier, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
						break;
					case AccessControlModification.Reset:
						this._securityDescriptor.DiscretionaryAcl.RemoveAccess(AccessControlType.Deny, securityIdentifier, -1, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None);
						this._securityDescriptor.DiscretionaryAcl.SetAccess(AccessControlType.Allow, securityIdentifier, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
						break;
					case AccessControlModification.Remove:
						flag = this._securityDescriptor.DiscretionaryAcl.RemoveAccess(AccessControlType.Allow, securityIdentifier, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
						break;
					case AccessControlModification.RemoveAll:
						flag = this._securityDescriptor.DiscretionaryAcl.RemoveAccess(AccessControlType.Allow, securityIdentifier, -1, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None);
						if (!flag)
						{
							throw new SystemException();
						}
						break;
					case AccessControlModification.RemoveSpecific:
						this._securityDescriptor.DiscretionaryAcl.RemoveAccessSpecific(AccessControlType.Allow, securityIdentifier, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
						break;
					default:
						throw new ArgumentOutOfRangeException("modification", Environment.GetResourceString("ArgumentOutOfRange_Enum"));
					}
				}
				else
				{
					if (rule.AccessControlType != AccessControlType.Deny)
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[] { (int)rule.AccessControlType }), "rule.AccessControlType");
					}
					switch (modification)
					{
					case AccessControlModification.Add:
						this._securityDescriptor.DiscretionaryAcl.AddAccess(AccessControlType.Deny, securityIdentifier, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
						break;
					case AccessControlModification.Set:
						this._securityDescriptor.DiscretionaryAcl.SetAccess(AccessControlType.Deny, securityIdentifier, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
						break;
					case AccessControlModification.Reset:
						this._securityDescriptor.DiscretionaryAcl.RemoveAccess(AccessControlType.Allow, securityIdentifier, -1, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None);
						this._securityDescriptor.DiscretionaryAcl.SetAccess(AccessControlType.Deny, securityIdentifier, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
						break;
					case AccessControlModification.Remove:
						flag = this._securityDescriptor.DiscretionaryAcl.RemoveAccess(AccessControlType.Deny, securityIdentifier, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
						break;
					case AccessControlModification.RemoveAll:
						flag = this._securityDescriptor.DiscretionaryAcl.RemoveAccess(AccessControlType.Deny, securityIdentifier, -1, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None);
						if (!flag)
						{
							throw new SystemException();
						}
						break;
					case AccessControlModification.RemoveSpecific:
						this._securityDescriptor.DiscretionaryAcl.RemoveAccessSpecific(AccessControlType.Deny, securityIdentifier, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
						break;
					default:
						throw new ArgumentOutOfRangeException("modification", Environment.GetResourceString("ArgumentOutOfRange_Enum"));
					}
				}
				modified = flag;
				base.AccessRulesModified |= modified;
				flag2 = flag;
			}
			finally
			{
				base.WriteUnlock();
			}
			return flag2;
		}

		// Token: 0x060053DB RID: 21467 RVA: 0x00131218 File Offset: 0x00130218
		protected override bool ModifyAudit(AccessControlModification modification, AuditRule rule, out bool modified)
		{
			base.WriteLock();
			bool flag2;
			try
			{
				bool flag = true;
				if (rule == null)
				{
					throw new ArgumentNullException("rule");
				}
				if (this._securityDescriptor.SystemAcl == null)
				{
					if (modification == AccessControlModification.Remove || modification == AccessControlModification.RemoveAll || modification == AccessControlModification.RemoveSpecific)
					{
						modified = false;
						return flag;
					}
					this._securityDescriptor.SystemAcl = new SystemAcl(base.IsContainer, base.IsDS, GenericAcl.AclRevision, 1);
					this._securityDescriptor.AddControlFlags(ControlFlags.SystemAclPresent);
				}
				SecurityIdentifier securityIdentifier = rule.IdentityReference.Translate(typeof(SecurityIdentifier)) as SecurityIdentifier;
				switch (modification)
				{
				case AccessControlModification.Add:
					this._securityDescriptor.SystemAcl.AddAudit(rule.AuditFlags, securityIdentifier, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
					break;
				case AccessControlModification.Set:
					this._securityDescriptor.SystemAcl.SetAudit(rule.AuditFlags, securityIdentifier, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
					break;
				case AccessControlModification.Reset:
					this._securityDescriptor.SystemAcl.SetAudit(rule.AuditFlags, securityIdentifier, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
					break;
				case AccessControlModification.Remove:
					flag = this._securityDescriptor.SystemAcl.RemoveAudit(rule.AuditFlags, securityIdentifier, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
					break;
				case AccessControlModification.RemoveAll:
					flag = this._securityDescriptor.SystemAcl.RemoveAudit(AuditFlags.Success | AuditFlags.Failure, securityIdentifier, -1, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None);
					if (!flag)
					{
						throw new InvalidProgramException();
					}
					break;
				case AccessControlModification.RemoveSpecific:
					this._securityDescriptor.SystemAcl.RemoveAuditSpecific(rule.AuditFlags, securityIdentifier, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
					break;
				default:
					throw new ArgumentOutOfRangeException("modification", Environment.GetResourceString("ArgumentOutOfRange_Enum"));
				}
				modified = flag;
				base.AuditRulesModified |= modified;
				flag2 = flag;
			}
			finally
			{
				base.WriteUnlock();
			}
			return flag2;
		}

		// Token: 0x060053DC RID: 21468 RVA: 0x0013141C File Offset: 0x0013041C
		protected void AddAccessRule(AccessRule rule)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			base.WriteLock();
			try
			{
				bool flag;
				this.ModifyAccess(AccessControlModification.Add, rule, out flag);
			}
			finally
			{
				base.WriteUnlock();
			}
		}

		// Token: 0x060053DD RID: 21469 RVA: 0x00131464 File Offset: 0x00130464
		protected void SetAccessRule(AccessRule rule)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			base.WriteLock();
			try
			{
				bool flag;
				this.ModifyAccess(AccessControlModification.Set, rule, out flag);
			}
			finally
			{
				base.WriteUnlock();
			}
		}

		// Token: 0x060053DE RID: 21470 RVA: 0x001314AC File Offset: 0x001304AC
		protected void ResetAccessRule(AccessRule rule)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			base.WriteLock();
			try
			{
				bool flag;
				this.ModifyAccess(AccessControlModification.Reset, rule, out flag);
			}
			finally
			{
				base.WriteUnlock();
			}
		}

		// Token: 0x060053DF RID: 21471 RVA: 0x001314F4 File Offset: 0x001304F4
		protected bool RemoveAccessRule(AccessRule rule)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			base.WriteLock();
			bool flag;
			try
			{
				if (this._securityDescriptor == null)
				{
					flag = true;
				}
				else
				{
					bool flag2;
					flag = this.ModifyAccess(AccessControlModification.Remove, rule, out flag2);
				}
			}
			finally
			{
				base.WriteUnlock();
			}
			return flag;
		}

		// Token: 0x060053E0 RID: 21472 RVA: 0x00131548 File Offset: 0x00130548
		protected void RemoveAccessRuleAll(AccessRule rule)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			base.WriteLock();
			try
			{
				if (this._securityDescriptor != null)
				{
					bool flag;
					this.ModifyAccess(AccessControlModification.RemoveAll, rule, out flag);
				}
			}
			finally
			{
				base.WriteUnlock();
			}
		}

		// Token: 0x060053E1 RID: 21473 RVA: 0x00131598 File Offset: 0x00130598
		protected void RemoveAccessRuleSpecific(AccessRule rule)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			base.WriteLock();
			try
			{
				if (this._securityDescriptor != null)
				{
					bool flag;
					this.ModifyAccess(AccessControlModification.RemoveSpecific, rule, out flag);
				}
			}
			finally
			{
				base.WriteUnlock();
			}
		}

		// Token: 0x060053E2 RID: 21474 RVA: 0x001315E8 File Offset: 0x001305E8
		protected void AddAuditRule(AuditRule rule)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			base.WriteLock();
			try
			{
				bool flag;
				this.ModifyAudit(AccessControlModification.Add, rule, out flag);
			}
			finally
			{
				base.WriteUnlock();
			}
		}

		// Token: 0x060053E3 RID: 21475 RVA: 0x00131630 File Offset: 0x00130630
		protected void SetAuditRule(AuditRule rule)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			base.WriteLock();
			try
			{
				bool flag;
				this.ModifyAudit(AccessControlModification.Set, rule, out flag);
			}
			finally
			{
				base.WriteUnlock();
			}
		}

		// Token: 0x060053E4 RID: 21476 RVA: 0x00131678 File Offset: 0x00130678
		protected bool RemoveAuditRule(AuditRule rule)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			base.WriteLock();
			bool flag;
			try
			{
				bool flag2;
				flag = this.ModifyAudit(AccessControlModification.Remove, rule, out flag2);
			}
			finally
			{
				base.WriteUnlock();
			}
			return flag;
		}

		// Token: 0x060053E5 RID: 21477 RVA: 0x001316C0 File Offset: 0x001306C0
		protected void RemoveAuditRuleAll(AuditRule rule)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			base.WriteLock();
			try
			{
				bool flag;
				this.ModifyAudit(AccessControlModification.RemoveAll, rule, out flag);
			}
			finally
			{
				base.WriteUnlock();
			}
		}

		// Token: 0x060053E6 RID: 21478 RVA: 0x00131708 File Offset: 0x00130708
		protected void RemoveAuditRuleSpecific(AuditRule rule)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			base.WriteLock();
			try
			{
				bool flag;
				this.ModifyAudit(AccessControlModification.RemoveSpecific, rule, out flag);
			}
			finally
			{
				base.WriteUnlock();
			}
		}

		// Token: 0x060053E7 RID: 21479 RVA: 0x00131750 File Offset: 0x00130750
		public AuthorizationRuleCollection GetAccessRules(bool includeExplicit, bool includeInherited, Type targetType)
		{
			return this.GetRules(true, includeExplicit, includeInherited, targetType);
		}

		// Token: 0x060053E8 RID: 21480 RVA: 0x0013175C File Offset: 0x0013075C
		public AuthorizationRuleCollection GetAuditRules(bool includeExplicit, bool includeInherited, Type targetType)
		{
			return this.GetRules(false, includeExplicit, includeInherited, targetType);
		}
	}
}
