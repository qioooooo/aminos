using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000925 RID: 2341
	public abstract class GenericSecurityDescriptor
	{
		// Token: 0x060054CD RID: 21709 RVA: 0x00134470 File Offset: 0x00133470
		private static void MarshalInt(byte[] binaryForm, int offset, int number)
		{
			binaryForm[offset] = (byte)number;
			binaryForm[offset + 1] = (byte)(number >> 8);
			binaryForm[offset + 2] = (byte)(number >> 16);
			binaryForm[offset + 3] = (byte)(number >> 24);
		}

		// Token: 0x060054CE RID: 21710 RVA: 0x00134494 File Offset: 0x00133494
		internal static int UnmarshalInt(byte[] binaryForm, int offset)
		{
			return (int)binaryForm[offset] + ((int)binaryForm[offset + 1] << 8) + ((int)binaryForm[offset + 2] << 16) + ((int)binaryForm[offset + 3] << 24);
		}

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x060054D0 RID: 21712
		internal abstract GenericAcl GenericSacl { get; }

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x060054D1 RID: 21713
		internal abstract GenericAcl GenericDacl { get; }

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x060054D2 RID: 21714 RVA: 0x001344BB File Offset: 0x001334BB
		private bool IsCraftedAefaDacl
		{
			get
			{
				return this.GenericDacl is DiscretionaryAcl && (this.GenericDacl as DiscretionaryAcl).EveryOneFullAccessForNullDacl;
			}
		}

		// Token: 0x060054D3 RID: 21715 RVA: 0x001344DC File Offset: 0x001334DC
		public static bool IsSddlConversionSupported()
		{
			return Win32.IsSddlConversionSupported();
		}

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x060054D4 RID: 21716 RVA: 0x001344E3 File Offset: 0x001334E3
		public static byte Revision
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x060054D5 RID: 21717
		public abstract ControlFlags ControlFlags { get; }

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x060054D6 RID: 21718
		// (set) Token: 0x060054D7 RID: 21719
		public abstract SecurityIdentifier Owner { get; set; }

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x060054D8 RID: 21720
		// (set) Token: 0x060054D9 RID: 21721
		public abstract SecurityIdentifier Group { get; set; }

		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x060054DA RID: 21722 RVA: 0x001344E8 File Offset: 0x001334E8
		public int BinaryLength
		{
			get
			{
				int num = 20;
				if (this.Owner != null)
				{
					num += this.Owner.BinaryLength;
				}
				if (this.Group != null)
				{
					num += this.Group.BinaryLength;
				}
				if ((this.ControlFlags & ControlFlags.SystemAclPresent) != ControlFlags.None && this.GenericSacl != null)
				{
					num += this.GenericSacl.BinaryLength;
				}
				if ((this.ControlFlags & ControlFlags.DiscretionaryAclPresent) != ControlFlags.None && this.GenericDacl != null && !this.IsCraftedAefaDacl)
				{
					num += this.GenericDacl.BinaryLength;
				}
				return num;
			}
		}

		// Token: 0x060054DB RID: 21723 RVA: 0x0013457C File Offset: 0x0013357C
		public string GetSddlForm(AccessControlSections includeSections)
		{
			byte[] array = new byte[this.BinaryLength];
			this.GetBinaryForm(array, 0);
			SecurityInfos securityInfos = (SecurityInfos)0;
			if ((includeSections & AccessControlSections.Owner) != AccessControlSections.None)
			{
				securityInfos |= SecurityInfos.Owner;
			}
			if ((includeSections & AccessControlSections.Group) != AccessControlSections.None)
			{
				securityInfos |= SecurityInfos.Group;
			}
			if ((includeSections & AccessControlSections.Audit) != AccessControlSections.None)
			{
				securityInfos |= SecurityInfos.SystemAcl;
			}
			if ((includeSections & AccessControlSections.Access) != AccessControlSections.None)
			{
				securityInfos |= SecurityInfos.DiscretionaryAcl;
			}
			string text;
			int num = Win32.ConvertSdToSddl(array, 1, securityInfos, out text);
			if (num == 87 || num == 1305)
			{
				throw new InvalidOperationException();
			}
			if (num != 0)
			{
				throw new InvalidOperationException();
			}
			return text;
		}

		// Token: 0x060054DC RID: 21724 RVA: 0x001345EC File Offset: 0x001335EC
		public void GetBinaryForm(byte[] binaryForm, int offset)
		{
			int num = offset;
			if (binaryForm == null)
			{
				throw new ArgumentNullException("binaryForm");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (binaryForm.Length - offset < this.BinaryLength)
			{
				throw new ArgumentOutOfRangeException("binaryForm", Environment.GetResourceString("ArgumentOutOfRange_ArrayTooSmall"));
			}
			int binaryLength = this.BinaryLength;
			byte b = ((this is RawSecurityDescriptor && (this.ControlFlags & ControlFlags.RMControlValid) != ControlFlags.None) ? (this as RawSecurityDescriptor).ResourceManagerControl : 0);
			int num2 = (int)this.ControlFlags;
			if (this.IsCraftedAefaDacl)
			{
				num2 &= -5;
			}
			binaryForm[offset] = GenericSecurityDescriptor.Revision;
			binaryForm[offset + 1] = b;
			binaryForm[offset + 2] = (byte)num2;
			binaryForm[offset + 3] = (byte)(num2 >> 8);
			int num3 = offset + 4;
			int num4 = offset + 8;
			int num5 = offset + 12;
			int num6 = offset + 16;
			offset += 20;
			if (this.Owner != null)
			{
				GenericSecurityDescriptor.MarshalInt(binaryForm, num3, offset - num);
				this.Owner.GetBinaryForm(binaryForm, offset);
				offset += this.Owner.BinaryLength;
			}
			else
			{
				GenericSecurityDescriptor.MarshalInt(binaryForm, num3, 0);
			}
			if (this.Group != null)
			{
				GenericSecurityDescriptor.MarshalInt(binaryForm, num4, offset - num);
				this.Group.GetBinaryForm(binaryForm, offset);
				offset += this.Group.BinaryLength;
			}
			else
			{
				GenericSecurityDescriptor.MarshalInt(binaryForm, num4, 0);
			}
			if ((this.ControlFlags & ControlFlags.SystemAclPresent) != ControlFlags.None && this.GenericSacl != null)
			{
				GenericSecurityDescriptor.MarshalInt(binaryForm, num5, offset - num);
				this.GenericSacl.GetBinaryForm(binaryForm, offset);
				offset += this.GenericSacl.BinaryLength;
			}
			else
			{
				GenericSecurityDescriptor.MarshalInt(binaryForm, num5, 0);
			}
			if ((this.ControlFlags & ControlFlags.DiscretionaryAclPresent) != ControlFlags.None && this.GenericDacl != null && !this.IsCraftedAefaDacl)
			{
				GenericSecurityDescriptor.MarshalInt(binaryForm, num6, offset - num);
				this.GenericDacl.GetBinaryForm(binaryForm, offset);
				offset += this.GenericDacl.BinaryLength;
				return;
			}
			GenericSecurityDescriptor.MarshalInt(binaryForm, num6, 0);
		}

		// Token: 0x04002BFF RID: 11263
		internal const int HeaderLength = 20;

		// Token: 0x04002C00 RID: 11264
		internal const int OwnerFoundAt = 4;

		// Token: 0x04002C01 RID: 11265
		internal const int GroupFoundAt = 8;

		// Token: 0x04002C02 RID: 11266
		internal const int SaclFoundAt = 12;

		// Token: 0x04002C03 RID: 11267
		internal const int DaclFoundAt = 16;
	}
}
