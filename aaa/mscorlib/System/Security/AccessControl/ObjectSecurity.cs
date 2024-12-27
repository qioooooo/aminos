using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;

namespace System.Security.AccessControl
{
	// Token: 0x020008FD RID: 2301
	public abstract class ObjectSecurity
	{
		// Token: 0x060053A4 RID: 21412 RVA: 0x00130242 File Offset: 0x0012F242
		private ObjectSecurity()
		{
		}

		// Token: 0x060053A5 RID: 21413 RVA: 0x00130258 File Offset: 0x0012F258
		protected ObjectSecurity(bool isContainer, bool isDS)
			: this()
		{
			DiscretionaryAcl discretionaryAcl = new DiscretionaryAcl(isContainer, isDS, 5);
			this._securityDescriptor = new CommonSecurityDescriptor(isContainer, isDS, ControlFlags.None, null, null, null, discretionaryAcl);
		}

		// Token: 0x060053A6 RID: 21414 RVA: 0x00130286 File Offset: 0x0012F286
		internal ObjectSecurity(CommonSecurityDescriptor securityDescriptor)
			: this()
		{
			if (securityDescriptor == null)
			{
				throw new ArgumentNullException("securityDescriptor");
			}
			this._securityDescriptor = securityDescriptor;
		}

		// Token: 0x060053A7 RID: 21415 RVA: 0x001302A4 File Offset: 0x0012F2A4
		private void UpdateWithNewSecurityDescriptor(RawSecurityDescriptor newOne, AccessControlSections includeSections)
		{
			if ((includeSections & AccessControlSections.Owner) != AccessControlSections.None)
			{
				this._ownerModified = true;
				this._securityDescriptor.Owner = newOne.Owner;
			}
			if ((includeSections & AccessControlSections.Group) != AccessControlSections.None)
			{
				this._groupModified = true;
				this._securityDescriptor.Group = newOne.Group;
			}
			if ((includeSections & AccessControlSections.Audit) != AccessControlSections.None)
			{
				this._saclModified = true;
				if (newOne.SystemAcl != null)
				{
					this._securityDescriptor.SystemAcl = new SystemAcl(this.IsContainer, this.IsDS, newOne.SystemAcl, true);
				}
				else
				{
					this._securityDescriptor.SystemAcl = null;
				}
				this._securityDescriptor.UpdateControlFlags(ObjectSecurity.SACL_CONTROL_FLAGS, newOne.ControlFlags & ObjectSecurity.SACL_CONTROL_FLAGS);
			}
			if ((includeSections & AccessControlSections.Access) != AccessControlSections.None)
			{
				this._daclModified = true;
				if (newOne.DiscretionaryAcl != null)
				{
					this._securityDescriptor.DiscretionaryAcl = new DiscretionaryAcl(this.IsContainer, this.IsDS, newOne.DiscretionaryAcl, true);
				}
				else
				{
					this._securityDescriptor.DiscretionaryAcl = null;
				}
				ControlFlags controlFlags = this._securityDescriptor.ControlFlags & ControlFlags.DiscretionaryAclPresent;
				this._securityDescriptor.UpdateControlFlags(ObjectSecurity.DACL_CONTROL_FLAGS, (newOne.ControlFlags | controlFlags) & ObjectSecurity.DACL_CONTROL_FLAGS);
			}
		}

		// Token: 0x060053A8 RID: 21416 RVA: 0x001303BD File Offset: 0x0012F3BD
		protected void ReadLock()
		{
			this._lock.AcquireReaderLock(-1);
		}

		// Token: 0x060053A9 RID: 21417 RVA: 0x001303CB File Offset: 0x0012F3CB
		protected void ReadUnlock()
		{
			this._lock.ReleaseReaderLock();
		}

		// Token: 0x060053AA RID: 21418 RVA: 0x001303D8 File Offset: 0x0012F3D8
		protected void WriteLock()
		{
			this._lock.AcquireWriterLock(-1);
		}

		// Token: 0x060053AB RID: 21419 RVA: 0x001303E6 File Offset: 0x0012F3E6
		protected void WriteUnlock()
		{
			this._lock.ReleaseWriterLock();
		}

		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x060053AC RID: 21420 RVA: 0x001303F3 File Offset: 0x0012F3F3
		// (set) Token: 0x060053AD RID: 21421 RVA: 0x00130425 File Offset: 0x0012F425
		protected bool OwnerModified
		{
			get
			{
				if (!this._lock.IsReaderLockHeld && !this._lock.IsWriterLockHeld)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustLockForReadOrWrite"));
				}
				return this._ownerModified;
			}
			set
			{
				if (!this._lock.IsWriterLockHeld)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustLockForWrite"));
				}
				this._ownerModified = value;
			}
		}

		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x060053AE RID: 21422 RVA: 0x0013044B File Offset: 0x0012F44B
		// (set) Token: 0x060053AF RID: 21423 RVA: 0x0013047D File Offset: 0x0012F47D
		protected bool GroupModified
		{
			get
			{
				if (!this._lock.IsReaderLockHeld && !this._lock.IsWriterLockHeld)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustLockForReadOrWrite"));
				}
				return this._groupModified;
			}
			set
			{
				if (!this._lock.IsWriterLockHeld)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustLockForWrite"));
				}
				this._groupModified = value;
			}
		}

		// Token: 0x17000E84 RID: 3716
		// (get) Token: 0x060053B0 RID: 21424 RVA: 0x001304A3 File Offset: 0x0012F4A3
		// (set) Token: 0x060053B1 RID: 21425 RVA: 0x001304D5 File Offset: 0x0012F4D5
		protected bool AuditRulesModified
		{
			get
			{
				if (!this._lock.IsReaderLockHeld && !this._lock.IsWriterLockHeld)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustLockForReadOrWrite"));
				}
				return this._saclModified;
			}
			set
			{
				if (!this._lock.IsWriterLockHeld)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustLockForWrite"));
				}
				this._saclModified = value;
			}
		}

		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x060053B2 RID: 21426 RVA: 0x001304FB File Offset: 0x0012F4FB
		// (set) Token: 0x060053B3 RID: 21427 RVA: 0x0013052D File Offset: 0x0012F52D
		protected bool AccessRulesModified
		{
			get
			{
				if (!this._lock.IsReaderLockHeld && !this._lock.IsWriterLockHeld)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustLockForReadOrWrite"));
				}
				return this._daclModified;
			}
			set
			{
				if (!this._lock.IsWriterLockHeld)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustLockForWrite"));
				}
				this._daclModified = value;
			}
		}

		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x060053B4 RID: 21428 RVA: 0x00130553 File Offset: 0x0012F553
		protected bool IsContainer
		{
			get
			{
				return this._securityDescriptor.IsContainer;
			}
		}

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x060053B5 RID: 21429 RVA: 0x00130560 File Offset: 0x0012F560
		protected bool IsDS
		{
			get
			{
				return this._securityDescriptor.IsDS;
			}
		}

		// Token: 0x060053B6 RID: 21430 RVA: 0x0013056D File Offset: 0x0012F56D
		protected virtual void Persist(string name, AccessControlSections includeSections)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060053B7 RID: 21431 RVA: 0x00130574 File Offset: 0x0012F574
		protected virtual void Persist(bool enableOwnershipPrivilege, string name, AccessControlSections includeSections)
		{
			Privilege privilege = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				if (enableOwnershipPrivilege)
				{
					privilege = new Privilege("SeTakeOwnershipPrivilege");
					try
					{
						privilege.Enable();
					}
					catch (PrivilegeNotHeldException)
					{
					}
				}
				this.Persist(name, includeSections);
			}
			catch
			{
				if (privilege != null)
				{
					privilege.Revert();
				}
				throw;
			}
			finally
			{
				if (privilege != null)
				{
					privilege.Revert();
				}
			}
		}

		// Token: 0x060053B8 RID: 21432 RVA: 0x001305EC File Offset: 0x0012F5EC
		protected virtual void Persist(SafeHandle handle, AccessControlSections includeSections)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060053B9 RID: 21433 RVA: 0x001305F4 File Offset: 0x0012F5F4
		public IdentityReference GetOwner(Type targetType)
		{
			this.ReadLock();
			IdentityReference identityReference;
			try
			{
				if (this._securityDescriptor.Owner == null)
				{
					identityReference = null;
				}
				else
				{
					identityReference = this._securityDescriptor.Owner.Translate(targetType);
				}
			}
			finally
			{
				this.ReadUnlock();
			}
			return identityReference;
		}

		// Token: 0x060053BA RID: 21434 RVA: 0x0013064C File Offset: 0x0012F64C
		public void SetOwner(IdentityReference identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			this.WriteLock();
			try
			{
				this._securityDescriptor.Owner = identity.Translate(typeof(SecurityIdentifier)) as SecurityIdentifier;
				this._ownerModified = true;
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x060053BB RID: 21435 RVA: 0x001306B4 File Offset: 0x0012F6B4
		public IdentityReference GetGroup(Type targetType)
		{
			this.ReadLock();
			IdentityReference identityReference;
			try
			{
				if (this._securityDescriptor.Group == null)
				{
					identityReference = null;
				}
				else
				{
					identityReference = this._securityDescriptor.Group.Translate(targetType);
				}
			}
			finally
			{
				this.ReadUnlock();
			}
			return identityReference;
		}

		// Token: 0x060053BC RID: 21436 RVA: 0x0013070C File Offset: 0x0012F70C
		public void SetGroup(IdentityReference identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			this.WriteLock();
			try
			{
				this._securityDescriptor.Group = identity.Translate(typeof(SecurityIdentifier)) as SecurityIdentifier;
				this._groupModified = true;
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x060053BD RID: 21437 RVA: 0x00130774 File Offset: 0x0012F774
		public virtual void PurgeAccessRules(IdentityReference identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			this.WriteLock();
			try
			{
				this._securityDescriptor.PurgeAccessControl(identity.Translate(typeof(SecurityIdentifier)) as SecurityIdentifier);
				this._daclModified = true;
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x060053BE RID: 21438 RVA: 0x001307DC File Offset: 0x0012F7DC
		public virtual void PurgeAuditRules(IdentityReference identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			this.WriteLock();
			try
			{
				this._securityDescriptor.PurgeAudit(identity.Translate(typeof(SecurityIdentifier)) as SecurityIdentifier);
				this._saclModified = true;
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x060053BF RID: 21439 RVA: 0x00130844 File Offset: 0x0012F844
		public bool AreAccessRulesProtected
		{
			get
			{
				this.ReadLock();
				bool flag;
				try
				{
					flag = (this._securityDescriptor.ControlFlags & ControlFlags.DiscretionaryAclProtected) != ControlFlags.None;
				}
				finally
				{
					this.ReadUnlock();
				}
				return flag;
			}
		}

		// Token: 0x060053C0 RID: 21440 RVA: 0x0013088C File Offset: 0x0012F88C
		public void SetAccessRuleProtection(bool isProtected, bool preserveInheritance)
		{
			this.WriteLock();
			try
			{
				this._securityDescriptor.SetDiscretionaryAclProtection(isProtected, preserveInheritance);
				this._daclModified = true;
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x060053C1 RID: 21441 RVA: 0x001308CC File Offset: 0x0012F8CC
		public bool AreAuditRulesProtected
		{
			get
			{
				this.ReadLock();
				bool flag;
				try
				{
					flag = (this._securityDescriptor.ControlFlags & ControlFlags.SystemAclProtected) != ControlFlags.None;
				}
				finally
				{
					this.ReadUnlock();
				}
				return flag;
			}
		}

		// Token: 0x060053C2 RID: 21442 RVA: 0x00130914 File Offset: 0x0012F914
		public void SetAuditRuleProtection(bool isProtected, bool preserveInheritance)
		{
			this.WriteLock();
			try
			{
				this._securityDescriptor.SetSystemAclProtection(isProtected, preserveInheritance);
				this._saclModified = true;
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x060053C3 RID: 21443 RVA: 0x00130954 File Offset: 0x0012F954
		public bool AreAccessRulesCanonical
		{
			get
			{
				this.ReadLock();
				bool isDiscretionaryAclCanonical;
				try
				{
					isDiscretionaryAclCanonical = this._securityDescriptor.IsDiscretionaryAclCanonical;
				}
				finally
				{
					this.ReadUnlock();
				}
				return isDiscretionaryAclCanonical;
			}
		}

		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x060053C4 RID: 21444 RVA: 0x00130990 File Offset: 0x0012F990
		public bool AreAuditRulesCanonical
		{
			get
			{
				this.ReadLock();
				bool isSystemAclCanonical;
				try
				{
					isSystemAclCanonical = this._securityDescriptor.IsSystemAclCanonical;
				}
				finally
				{
					this.ReadUnlock();
				}
				return isSystemAclCanonical;
			}
		}

		// Token: 0x060053C5 RID: 21445 RVA: 0x001309CC File Offset: 0x0012F9CC
		public static bool IsSddlConversionSupported()
		{
			return Win32.IsSddlConversionSupported();
		}

		// Token: 0x060053C6 RID: 21446 RVA: 0x001309D4 File Offset: 0x0012F9D4
		public string GetSecurityDescriptorSddlForm(AccessControlSections includeSections)
		{
			this.ReadLock();
			string sddlForm;
			try
			{
				sddlForm = this._securityDescriptor.GetSddlForm(includeSections);
			}
			finally
			{
				this.ReadUnlock();
			}
			return sddlForm;
		}

		// Token: 0x060053C7 RID: 21447 RVA: 0x00130A10 File Offset: 0x0012FA10
		public void SetSecurityDescriptorSddlForm(string sddlForm)
		{
			this.SetSecurityDescriptorSddlForm(sddlForm, AccessControlSections.All);
		}

		// Token: 0x060053C8 RID: 21448 RVA: 0x00130A1C File Offset: 0x0012FA1C
		public void SetSecurityDescriptorSddlForm(string sddlForm, AccessControlSections includeSections)
		{
			if (sddlForm == null)
			{
				throw new ArgumentNullException("sddlForm");
			}
			if ((includeSections & AccessControlSections.All) == AccessControlSections.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumAtLeastOneFlag"), "includeSections");
			}
			this.WriteLock();
			try
			{
				this.UpdateWithNewSecurityDescriptor(new RawSecurityDescriptor(sddlForm), includeSections);
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x060053C9 RID: 21449 RVA: 0x00130A80 File Offset: 0x0012FA80
		public byte[] GetSecurityDescriptorBinaryForm()
		{
			this.ReadLock();
			byte[] array2;
			try
			{
				byte[] array = new byte[this._securityDescriptor.BinaryLength];
				this._securityDescriptor.GetBinaryForm(array, 0);
				array2 = array;
			}
			finally
			{
				this.ReadUnlock();
			}
			return array2;
		}

		// Token: 0x060053CA RID: 21450 RVA: 0x00130AD0 File Offset: 0x0012FAD0
		public void SetSecurityDescriptorBinaryForm(byte[] binaryForm)
		{
			this.SetSecurityDescriptorBinaryForm(binaryForm, AccessControlSections.All);
		}

		// Token: 0x060053CB RID: 21451 RVA: 0x00130ADC File Offset: 0x0012FADC
		public void SetSecurityDescriptorBinaryForm(byte[] binaryForm, AccessControlSections includeSections)
		{
			if (binaryForm == null)
			{
				throw new ArgumentNullException("binaryForm");
			}
			if ((includeSections & AccessControlSections.All) == AccessControlSections.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumAtLeastOneFlag"), "includeSections");
			}
			this.WriteLock();
			try
			{
				this.UpdateWithNewSecurityDescriptor(new RawSecurityDescriptor(binaryForm, 0), includeSections);
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x060053CC RID: 21452
		public abstract Type AccessRightType { get; }

		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x060053CD RID: 21453
		public abstract Type AccessRuleType { get; }

		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x060053CE RID: 21454
		public abstract Type AuditRuleType { get; }

		// Token: 0x060053CF RID: 21455
		protected abstract bool ModifyAccess(AccessControlModification modification, AccessRule rule, out bool modified);

		// Token: 0x060053D0 RID: 21456
		protected abstract bool ModifyAudit(AccessControlModification modification, AuditRule rule, out bool modified);

		// Token: 0x060053D1 RID: 21457 RVA: 0x00130B40 File Offset: 0x0012FB40
		public virtual bool ModifyAccessRule(AccessControlModification modification, AccessRule rule, out bool modified)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			if (!this.AccessRuleType.IsAssignableFrom(rule.GetType()))
			{
				throw new ArgumentException(Environment.GetResourceString("AccessControl_InvalidAccessRuleType"), "rule");
			}
			this.WriteLock();
			bool flag;
			try
			{
				flag = this.ModifyAccess(modification, rule, out modified);
			}
			finally
			{
				this.WriteUnlock();
			}
			return flag;
		}

		// Token: 0x060053D2 RID: 21458 RVA: 0x00130BB0 File Offset: 0x0012FBB0
		public virtual bool ModifyAuditRule(AccessControlModification modification, AuditRule rule, out bool modified)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			if (!this.AuditRuleType.IsAssignableFrom(rule.GetType()))
			{
				throw new ArgumentException(Environment.GetResourceString("AccessControl_InvalidAuditRuleType"), "rule");
			}
			this.WriteLock();
			bool flag;
			try
			{
				flag = this.ModifyAudit(modification, rule, out modified);
			}
			finally
			{
				this.WriteUnlock();
			}
			return flag;
		}

		// Token: 0x060053D3 RID: 21459
		public abstract AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type);

		// Token: 0x060053D4 RID: 21460
		public abstract AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags);

		// Token: 0x04002B3A RID: 11066
		private readonly ReaderWriterLock _lock = new ReaderWriterLock();

		// Token: 0x04002B3B RID: 11067
		internal CommonSecurityDescriptor _securityDescriptor;

		// Token: 0x04002B3C RID: 11068
		private bool _ownerModified;

		// Token: 0x04002B3D RID: 11069
		private bool _groupModified;

		// Token: 0x04002B3E RID: 11070
		private bool _saclModified;

		// Token: 0x04002B3F RID: 11071
		private bool _daclModified;

		// Token: 0x04002B40 RID: 11072
		private static readonly ControlFlags SACL_CONTROL_FLAGS = ControlFlags.SystemAclPresent | ControlFlags.SystemAclAutoInherited | ControlFlags.SystemAclProtected;

		// Token: 0x04002B41 RID: 11073
		private static readonly ControlFlags DACL_CONTROL_FLAGS = ControlFlags.DiscretionaryAclPresent | ControlFlags.DiscretionaryAclAutoInherited | ControlFlags.DiscretionaryAclProtected;
	}
}
