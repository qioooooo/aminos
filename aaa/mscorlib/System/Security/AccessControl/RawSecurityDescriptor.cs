using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Microsoft.Win32;

namespace System.Security.AccessControl
{
	// Token: 0x02000926 RID: 2342
	public sealed class RawSecurityDescriptor : GenericSecurityDescriptor
	{
		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x060054DD RID: 21725 RVA: 0x001347CB File Offset: 0x001337CB
		internal override GenericAcl GenericSacl
		{
			get
			{
				return this._sacl;
			}
		}

		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x060054DE RID: 21726 RVA: 0x001347D3 File Offset: 0x001337D3
		internal override GenericAcl GenericDacl
		{
			get
			{
				return this._dacl;
			}
		}

		// Token: 0x060054DF RID: 21727 RVA: 0x001347DB File Offset: 0x001337DB
		private void CreateFromParts(ControlFlags flags, SecurityIdentifier owner, SecurityIdentifier group, RawAcl systemAcl, RawAcl discretionaryAcl)
		{
			this.SetFlags(flags);
			this.Owner = owner;
			this.Group = group;
			this.SystemAcl = systemAcl;
			this.DiscretionaryAcl = discretionaryAcl;
			this.ResourceManagerControl = 0;
		}

		// Token: 0x060054E0 RID: 21728 RVA: 0x00134809 File Offset: 0x00133809
		public RawSecurityDescriptor(ControlFlags flags, SecurityIdentifier owner, SecurityIdentifier group, RawAcl systemAcl, RawAcl discretionaryAcl)
		{
			this.CreateFromParts(flags, owner, group, systemAcl, discretionaryAcl);
		}

		// Token: 0x060054E1 RID: 21729 RVA: 0x0013481E File Offset: 0x0013381E
		public RawSecurityDescriptor(string sddlForm)
			: this(RawSecurityDescriptor.BinaryFormFromSddlForm(sddlForm), 0)
		{
		}

		// Token: 0x060054E2 RID: 21730 RVA: 0x00134830 File Offset: 0x00133830
		public RawSecurityDescriptor(byte[] binaryForm, int offset)
		{
			if (binaryForm == null)
			{
				throw new ArgumentNullException("binaryForm");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (binaryForm.Length - offset < 20)
			{
				throw new ArgumentOutOfRangeException("binaryForm", Environment.GetResourceString("ArgumentOutOfRange_ArrayTooSmall"));
			}
			if (binaryForm[offset] != GenericSecurityDescriptor.Revision)
			{
				throw new ArgumentOutOfRangeException("binaryForm", Environment.GetResourceString("AccessControl_InvalidSecurityDescriptorRevision"));
			}
			byte b = binaryForm[offset + 1];
			ControlFlags controlFlags = (ControlFlags)((int)binaryForm[offset + 2] + ((int)binaryForm[offset + 3] << 8));
			if ((controlFlags & ControlFlags.SelfRelative) == ControlFlags.None)
			{
				throw new ArgumentException(Environment.GetResourceString("AccessControl_InvalidSecurityDescriptorSelfRelativeForm"), "binaryForm");
			}
			int num = GenericSecurityDescriptor.UnmarshalInt(binaryForm, offset + 4);
			SecurityIdentifier securityIdentifier;
			if (num != 0)
			{
				securityIdentifier = new SecurityIdentifier(binaryForm, offset + num);
			}
			else
			{
				securityIdentifier = null;
			}
			int num2 = GenericSecurityDescriptor.UnmarshalInt(binaryForm, offset + 8);
			SecurityIdentifier securityIdentifier2;
			if (num2 != 0)
			{
				securityIdentifier2 = new SecurityIdentifier(binaryForm, offset + num2);
			}
			else
			{
				securityIdentifier2 = null;
			}
			int num3 = GenericSecurityDescriptor.UnmarshalInt(binaryForm, offset + 12);
			RawAcl rawAcl;
			if ((controlFlags & ControlFlags.SystemAclPresent) != ControlFlags.None && num3 != 0)
			{
				rawAcl = new RawAcl(binaryForm, offset + num3);
			}
			else
			{
				rawAcl = null;
			}
			int num4 = GenericSecurityDescriptor.UnmarshalInt(binaryForm, offset + 16);
			RawAcl rawAcl2;
			if ((controlFlags & ControlFlags.DiscretionaryAclPresent) != ControlFlags.None && num4 != 0)
			{
				rawAcl2 = new RawAcl(binaryForm, offset + num4);
			}
			else
			{
				rawAcl2 = null;
			}
			this.CreateFromParts(controlFlags, securityIdentifier, securityIdentifier2, rawAcl, rawAcl2);
			if ((controlFlags & ControlFlags.RMControlValid) != ControlFlags.None)
			{
				this.ResourceManagerControl = b;
			}
		}

		// Token: 0x060054E3 RID: 21731 RVA: 0x00134980 File Offset: 0x00133980
		private static byte[] BinaryFormFromSddlForm(string sddlForm)
		{
			if (!GenericSecurityDescriptor.IsSddlConversionSupported())
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
			}
			if (sddlForm == null)
			{
				throw new ArgumentNullException("sddlForm");
			}
			IntPtr zero = IntPtr.Zero;
			uint num = 0U;
			byte[] array = null;
			try
			{
				if (1 != Win32Native.ConvertStringSdToSd(sddlForm, (uint)GenericSecurityDescriptor.Revision, out zero, ref num))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (lastWin32Error == 87 || lastWin32Error == 1336 || lastWin32Error == 1338 || lastWin32Error == 1305)
					{
						throw new ArgumentException(Environment.GetResourceString("ArgumentException_InvalidSDSddlForm"), "sddlForm");
					}
					if (lastWin32Error == 8)
					{
						throw new OutOfMemoryException();
					}
					if (lastWin32Error == 1337)
					{
						throw new ArgumentException(Environment.GetResourceString("AccessControl_InvalidSidInSDDLString"), "sddlForm");
					}
					if (lastWin32Error != 0)
					{
						throw new SystemException();
					}
				}
				array = new byte[num];
				Marshal.Copy(zero, array, 0, (int)num);
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					Win32Native.LocalFree(zero);
				}
			}
			return array;
		}

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x060054E4 RID: 21732 RVA: 0x00134A70 File Offset: 0x00133A70
		public override ControlFlags ControlFlags
		{
			get
			{
				return this._flags;
			}
		}

		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x060054E5 RID: 21733 RVA: 0x00134A78 File Offset: 0x00133A78
		// (set) Token: 0x060054E6 RID: 21734 RVA: 0x00134A80 File Offset: 0x00133A80
		public override SecurityIdentifier Owner
		{
			get
			{
				return this._owner;
			}
			set
			{
				this._owner = value;
			}
		}

		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x060054E7 RID: 21735 RVA: 0x00134A89 File Offset: 0x00133A89
		// (set) Token: 0x060054E8 RID: 21736 RVA: 0x00134A91 File Offset: 0x00133A91
		public override SecurityIdentifier Group
		{
			get
			{
				return this._group;
			}
			set
			{
				this._group = value;
			}
		}

		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x060054E9 RID: 21737 RVA: 0x00134A9A File Offset: 0x00133A9A
		// (set) Token: 0x060054EA RID: 21738 RVA: 0x00134AA2 File Offset: 0x00133AA2
		public RawAcl SystemAcl
		{
			get
			{
				return this._sacl;
			}
			set
			{
				this._sacl = value;
			}
		}

		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x060054EB RID: 21739 RVA: 0x00134AAB File Offset: 0x00133AAB
		// (set) Token: 0x060054EC RID: 21740 RVA: 0x00134AB3 File Offset: 0x00133AB3
		public RawAcl DiscretionaryAcl
		{
			get
			{
				return this._dacl;
			}
			set
			{
				this._dacl = value;
			}
		}

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x060054ED RID: 21741 RVA: 0x00134ABC File Offset: 0x00133ABC
		// (set) Token: 0x060054EE RID: 21742 RVA: 0x00134AC4 File Offset: 0x00133AC4
		public byte ResourceManagerControl
		{
			get
			{
				return this._rmControl;
			}
			set
			{
				this._rmControl = value;
			}
		}

		// Token: 0x060054EF RID: 21743 RVA: 0x00134ACD File Offset: 0x00133ACD
		public void SetFlags(ControlFlags flags)
		{
			this._flags = flags | ControlFlags.SelfRelative;
		}

		// Token: 0x04002C04 RID: 11268
		private SecurityIdentifier _owner;

		// Token: 0x04002C05 RID: 11269
		private SecurityIdentifier _group;

		// Token: 0x04002C06 RID: 11270
		private ControlFlags _flags;

		// Token: 0x04002C07 RID: 11271
		private RawAcl _sacl;

		// Token: 0x04002C08 RID: 11272
		private RawAcl _dacl;

		// Token: 0x04002C09 RID: 11273
		private byte _rmControl;
	}
}
