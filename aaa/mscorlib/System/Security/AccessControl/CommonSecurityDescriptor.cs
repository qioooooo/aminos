using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000927 RID: 2343
	public sealed class CommonSecurityDescriptor : GenericSecurityDescriptor
	{
		// Token: 0x060054F0 RID: 21744 RVA: 0x00134ADC File Offset: 0x00133ADC
		private void CreateFromParts(bool isContainer, bool isDS, ControlFlags flags, SecurityIdentifier owner, SecurityIdentifier group, SystemAcl systemAcl, DiscretionaryAcl discretionaryAcl)
		{
			if (systemAcl != null && systemAcl.IsContainer != isContainer)
			{
				throw new ArgumentException(Environment.GetResourceString(isContainer ? "AccessControl_MustSpecifyContainerAcl" : "AccessControl_MustSpecifyLeafObjectAcl"), "systemAcl");
			}
			if (discretionaryAcl != null && discretionaryAcl.IsContainer != isContainer)
			{
				throw new ArgumentException(Environment.GetResourceString(isContainer ? "AccessControl_MustSpecifyContainerAcl" : "AccessControl_MustSpecifyLeafObjectAcl"), "discretionaryAcl");
			}
			this._isContainer = isContainer;
			if (systemAcl != null && systemAcl.IsDS != isDS)
			{
				throw new ArgumentException(Environment.GetResourceString(isDS ? "AccessControl_MustSpecifyDirectoryObjectAcl" : "AccessControl_MustSpecifyNonDirectoryObjectAcl"), "systemAcl");
			}
			if (discretionaryAcl != null && discretionaryAcl.IsDS != isDS)
			{
				throw new ArgumentException(Environment.GetResourceString(isDS ? "AccessControl_MustSpecifyDirectoryObjectAcl" : "AccessControl_MustSpecifyNonDirectoryObjectAcl"), "discretionaryAcl");
			}
			this._isDS = isDS;
			this._sacl = systemAcl;
			if (discretionaryAcl == null)
			{
				discretionaryAcl = DiscretionaryAcl.CreateAllowEveryoneFullAccess(this._isDS, this._isContainer);
			}
			this._dacl = discretionaryAcl;
			ControlFlags controlFlags = flags | ControlFlags.DiscretionaryAclPresent;
			if (systemAcl == null)
			{
				controlFlags &= ~ControlFlags.SystemAclPresent;
			}
			else
			{
				controlFlags |= ControlFlags.SystemAclPresent;
			}
			this._rawSd = new RawSecurityDescriptor(controlFlags, owner, group, (systemAcl == null) ? null : systemAcl.RawAcl, discretionaryAcl.RawAcl);
		}

		// Token: 0x060054F1 RID: 21745 RVA: 0x00134C0B File Offset: 0x00133C0B
		public CommonSecurityDescriptor(bool isContainer, bool isDS, ControlFlags flags, SecurityIdentifier owner, SecurityIdentifier group, SystemAcl systemAcl, DiscretionaryAcl discretionaryAcl)
		{
			this.CreateFromParts(isContainer, isDS, flags, owner, group, systemAcl, discretionaryAcl);
		}

		// Token: 0x060054F2 RID: 21746 RVA: 0x00134C24 File Offset: 0x00133C24
		private CommonSecurityDescriptor(bool isContainer, bool isDS, ControlFlags flags, SecurityIdentifier owner, SecurityIdentifier group, RawAcl systemAcl, RawAcl discretionaryAcl)
			: this(isContainer, isDS, flags, owner, group, (systemAcl == null) ? null : new SystemAcl(isContainer, isDS, systemAcl), (discretionaryAcl == null) ? null : new DiscretionaryAcl(isContainer, isDS, discretionaryAcl))
		{
		}

		// Token: 0x060054F3 RID: 21747 RVA: 0x00134C5E File Offset: 0x00133C5E
		public CommonSecurityDescriptor(bool isContainer, bool isDS, RawSecurityDescriptor rawSecurityDescriptor)
			: this(isContainer, isDS, rawSecurityDescriptor, false)
		{
		}

		// Token: 0x060054F4 RID: 21748 RVA: 0x00134C6C File Offset: 0x00133C6C
		internal CommonSecurityDescriptor(bool isContainer, bool isDS, RawSecurityDescriptor rawSecurityDescriptor, bool trusted)
		{
			if (rawSecurityDescriptor == null)
			{
				throw new ArgumentNullException("rawSecurityDescriptor");
			}
			this.CreateFromParts(isContainer, isDS, rawSecurityDescriptor.ControlFlags, rawSecurityDescriptor.Owner, rawSecurityDescriptor.Group, (rawSecurityDescriptor.SystemAcl == null) ? null : new SystemAcl(isContainer, isDS, rawSecurityDescriptor.SystemAcl, trusted), (rawSecurityDescriptor.DiscretionaryAcl == null) ? null : new DiscretionaryAcl(isContainer, isDS, rawSecurityDescriptor.DiscretionaryAcl, trusted));
		}

		// Token: 0x060054F5 RID: 21749 RVA: 0x00134CDB File Offset: 0x00133CDB
		public CommonSecurityDescriptor(bool isContainer, bool isDS, string sddlForm)
			: this(isContainer, isDS, new RawSecurityDescriptor(sddlForm), true)
		{
		}

		// Token: 0x060054F6 RID: 21750 RVA: 0x00134CEC File Offset: 0x00133CEC
		public CommonSecurityDescriptor(bool isContainer, bool isDS, byte[] binaryForm, int offset)
			: this(isContainer, isDS, new RawSecurityDescriptor(binaryForm, offset), true)
		{
		}

		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x060054F7 RID: 21751 RVA: 0x00134CFF File Offset: 0x00133CFF
		internal sealed override GenericAcl GenericSacl
		{
			get
			{
				return this._sacl;
			}
		}

		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x060054F8 RID: 21752 RVA: 0x00134D07 File Offset: 0x00133D07
		internal sealed override GenericAcl GenericDacl
		{
			get
			{
				return this._dacl;
			}
		}

		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x060054F9 RID: 21753 RVA: 0x00134D0F File Offset: 0x00133D0F
		public bool IsContainer
		{
			get
			{
				return this._isContainer;
			}
		}

		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x060054FA RID: 21754 RVA: 0x00134D17 File Offset: 0x00133D17
		public bool IsDS
		{
			get
			{
				return this._isDS;
			}
		}

		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x060054FB RID: 21755 RVA: 0x00134D1F File Offset: 0x00133D1F
		public override ControlFlags ControlFlags
		{
			get
			{
				return this._rawSd.ControlFlags;
			}
		}

		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x060054FC RID: 21756 RVA: 0x00134D2C File Offset: 0x00133D2C
		// (set) Token: 0x060054FD RID: 21757 RVA: 0x00134D39 File Offset: 0x00133D39
		public override SecurityIdentifier Owner
		{
			get
			{
				return this._rawSd.Owner;
			}
			set
			{
				this._rawSd.Owner = value;
			}
		}

		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x060054FE RID: 21758 RVA: 0x00134D47 File Offset: 0x00133D47
		// (set) Token: 0x060054FF RID: 21759 RVA: 0x00134D54 File Offset: 0x00133D54
		public override SecurityIdentifier Group
		{
			get
			{
				return this._rawSd.Group;
			}
			set
			{
				this._rawSd.Group = value;
			}
		}

		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x06005500 RID: 21760 RVA: 0x00134D62 File Offset: 0x00133D62
		// (set) Token: 0x06005501 RID: 21761 RVA: 0x00134D6C File Offset: 0x00133D6C
		public SystemAcl SystemAcl
		{
			get
			{
				return this._sacl;
			}
			set
			{
				if (value != null)
				{
					if (value.IsContainer != this.IsContainer)
					{
						throw new ArgumentException(Environment.GetResourceString(this.IsContainer ? "AccessControl_MustSpecifyContainerAcl" : "AccessControl_MustSpecifyLeafObjectAcl"), "value");
					}
					if (value.IsDS != this.IsDS)
					{
						throw new ArgumentException(Environment.GetResourceString(this.IsDS ? "AccessControl_MustSpecifyDirectoryObjectAcl" : "AccessControl_MustSpecifyNonDirectoryObjectAcl"), "value");
					}
				}
				this._sacl = value;
				if (this._sacl != null)
				{
					this._rawSd.SystemAcl = this._sacl.RawAcl;
					this.AddControlFlags(ControlFlags.SystemAclPresent);
					return;
				}
				this._rawSd.SystemAcl = null;
				this.RemoveControlFlags(ControlFlags.SystemAclPresent);
			}
		}

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x06005502 RID: 21762 RVA: 0x00134E22 File Offset: 0x00133E22
		// (set) Token: 0x06005503 RID: 21763 RVA: 0x00134E2C File Offset: 0x00133E2C
		public DiscretionaryAcl DiscretionaryAcl
		{
			get
			{
				return this._dacl;
			}
			set
			{
				if (value != null)
				{
					if (value.IsContainer != this.IsContainer)
					{
						throw new ArgumentException(Environment.GetResourceString(this.IsContainer ? "AccessControl_MustSpecifyContainerAcl" : "AccessControl_MustSpecifyLeafObjectAcl"), "value");
					}
					if (value.IsDS != this.IsDS)
					{
						throw new ArgumentException(Environment.GetResourceString(this.IsDS ? "AccessControl_MustSpecifyDirectoryObjectAcl" : "AccessControl_MustSpecifyNonDirectoryObjectAcl"), "value");
					}
				}
				if (value == null)
				{
					this._dacl = DiscretionaryAcl.CreateAllowEveryoneFullAccess(this.IsDS, this.IsContainer);
				}
				else
				{
					this._dacl = value;
				}
				this._rawSd.DiscretionaryAcl = this._dacl.RawAcl;
				this.AddControlFlags(ControlFlags.DiscretionaryAclPresent);
			}
		}

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x06005504 RID: 21764 RVA: 0x00134EE0 File Offset: 0x00133EE0
		public bool IsSystemAclCanonical
		{
			get
			{
				return this.SystemAcl == null || this.SystemAcl.IsCanonical;
			}
		}

		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x06005505 RID: 21765 RVA: 0x00134EF7 File Offset: 0x00133EF7
		public bool IsDiscretionaryAclCanonical
		{
			get
			{
				return this.DiscretionaryAcl == null || this.DiscretionaryAcl.IsCanonical;
			}
		}

		// Token: 0x06005506 RID: 21766 RVA: 0x00134F0E File Offset: 0x00133F0E
		public void SetSystemAclProtection(bool isProtected, bool preserveInheritance)
		{
			if (!isProtected)
			{
				this.RemoveControlFlags(ControlFlags.SystemAclProtected);
				return;
			}
			if (!preserveInheritance && this.SystemAcl != null)
			{
				this.SystemAcl.RemoveInheritedAces();
			}
			this.AddControlFlags(ControlFlags.SystemAclProtected);
		}

		// Token: 0x06005507 RID: 21767 RVA: 0x00134F40 File Offset: 0x00133F40
		public void SetDiscretionaryAclProtection(bool isProtected, bool preserveInheritance)
		{
			if (!isProtected)
			{
				this.RemoveControlFlags(ControlFlags.DiscretionaryAclProtected);
			}
			else
			{
				if (!preserveInheritance && this.DiscretionaryAcl != null)
				{
					this.DiscretionaryAcl.RemoveInheritedAces();
				}
				this.AddControlFlags(ControlFlags.DiscretionaryAclProtected);
			}
			if (this.DiscretionaryAcl != null && this.DiscretionaryAcl.EveryOneFullAccessForNullDacl)
			{
				this.DiscretionaryAcl.EveryOneFullAccessForNullDacl = false;
			}
		}

		// Token: 0x06005508 RID: 21768 RVA: 0x00134F9F File Offset: 0x00133F9F
		public void PurgeAccessControl(SecurityIdentifier sid)
		{
			if (sid == null)
			{
				throw new ArgumentNullException("sid");
			}
			if (this.DiscretionaryAcl != null)
			{
				this.DiscretionaryAcl.Purge(sid);
			}
		}

		// Token: 0x06005509 RID: 21769 RVA: 0x00134FC9 File Offset: 0x00133FC9
		public void PurgeAudit(SecurityIdentifier sid)
		{
			if (sid == null)
			{
				throw new ArgumentNullException("sid");
			}
			if (this.SystemAcl != null)
			{
				this.SystemAcl.Purge(sid);
			}
		}

		// Token: 0x0600550A RID: 21770 RVA: 0x00134FF4 File Offset: 0x00133FF4
		internal void UpdateControlFlags(ControlFlags flagsToUpdate, ControlFlags newFlags)
		{
			ControlFlags controlFlags = newFlags | (this._rawSd.ControlFlags & ~flagsToUpdate);
			this._rawSd.SetFlags(controlFlags);
		}

		// Token: 0x0600550B RID: 21771 RVA: 0x0013501E File Offset: 0x0013401E
		internal void AddControlFlags(ControlFlags flags)
		{
			this._rawSd.SetFlags(this._rawSd.ControlFlags | flags);
		}

		// Token: 0x0600550C RID: 21772 RVA: 0x00135038 File Offset: 0x00134038
		internal void RemoveControlFlags(ControlFlags flags)
		{
			this._rawSd.SetFlags(this._rawSd.ControlFlags & ~flags);
		}

		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x0600550D RID: 21773 RVA: 0x00135053 File Offset: 0x00134053
		internal bool IsSystemAclPresent
		{
			get
			{
				return (this._rawSd.ControlFlags & ControlFlags.SystemAclPresent) != ControlFlags.None;
			}
		}

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x0600550E RID: 21774 RVA: 0x00135069 File Offset: 0x00134069
		internal bool IsDiscretionaryAclPresent
		{
			get
			{
				return (this._rawSd.ControlFlags & ControlFlags.DiscretionaryAclPresent) != ControlFlags.None;
			}
		}

		// Token: 0x04002C0A RID: 11274
		private bool _isContainer;

		// Token: 0x04002C0B RID: 11275
		private bool _isDS;

		// Token: 0x04002C0C RID: 11276
		private RawSecurityDescriptor _rawSd;

		// Token: 0x04002C0D RID: 11277
		private SystemAcl _sacl;

		// Token: 0x04002C0E RID: 11278
		private DiscretionaryAcl _dacl;
	}
}
