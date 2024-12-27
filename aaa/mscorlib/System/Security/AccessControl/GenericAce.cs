using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008E4 RID: 2276
	public abstract class GenericAce
	{
		// Token: 0x060052E0 RID: 21216 RVA: 0x0012CAFC File Offset: 0x0012BAFC
		internal void MarshalHeader(byte[] binaryForm, int offset)
		{
			int binaryLength = this.BinaryLength;
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
			if (binaryLength > 65535)
			{
				throw new SystemException();
			}
			binaryForm[offset] = (byte)this.AceType;
			binaryForm[offset + 1] = (byte)this.AceFlags;
			binaryForm[offset + 2] = (byte)binaryLength;
			binaryForm[offset + 3] = (byte)(binaryLength >> 8);
		}

		// Token: 0x060052E1 RID: 21217 RVA: 0x0012CB8B File Offset: 0x0012BB8B
		internal GenericAce(AceType type, AceFlags flags)
		{
			this._type = type;
			this._flags = flags;
		}

		// Token: 0x060052E2 RID: 21218 RVA: 0x0012CBA4 File Offset: 0x0012BBA4
		internal static AceFlags AceFlagsFromAuditFlags(AuditFlags auditFlags)
		{
			AceFlags aceFlags = AceFlags.None;
			if ((auditFlags & AuditFlags.Success) != AuditFlags.None)
			{
				aceFlags |= AceFlags.SuccessfulAccess;
			}
			if ((auditFlags & AuditFlags.Failure) != AuditFlags.None)
			{
				aceFlags |= AceFlags.FailedAccess;
			}
			if (aceFlags == AceFlags.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumAtLeastOneFlag"), "auditFlags");
			}
			return aceFlags;
		}

		// Token: 0x060052E3 RID: 21219 RVA: 0x0012CBE8 File Offset: 0x0012BBE8
		internal static AceFlags AceFlagsFromInheritanceFlags(InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			AceFlags aceFlags = AceFlags.None;
			if ((inheritanceFlags & InheritanceFlags.ContainerInherit) != InheritanceFlags.None)
			{
				aceFlags |= AceFlags.ContainerInherit;
			}
			if ((inheritanceFlags & InheritanceFlags.ObjectInherit) != InheritanceFlags.None)
			{
				aceFlags |= AceFlags.ObjectInherit;
			}
			if (aceFlags != AceFlags.None)
			{
				if ((propagationFlags & PropagationFlags.NoPropagateInherit) != PropagationFlags.None)
				{
					aceFlags |= AceFlags.NoPropagateInherit;
				}
				if ((propagationFlags & PropagationFlags.InheritOnly) != PropagationFlags.None)
				{
					aceFlags |= AceFlags.InheritOnly;
				}
			}
			return aceFlags;
		}

		// Token: 0x060052E4 RID: 21220 RVA: 0x0012CC24 File Offset: 0x0012BC24
		internal static void VerifyHeader(byte[] binaryForm, int offset)
		{
			if (binaryForm == null)
			{
				throw new ArgumentNullException("binaryForm");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (binaryForm.Length - offset < 4)
			{
				throw new ArgumentOutOfRangeException("binaryForm", Environment.GetResourceString("ArgumentOutOfRange_ArrayTooSmall"));
			}
			if (((int)binaryForm[offset + 3] << 8) + (int)binaryForm[offset + 2] > binaryForm.Length - offset)
			{
				throw new ArgumentOutOfRangeException("binaryForm", Environment.GetResourceString("ArgumentOutOfRange_ArrayTooSmall"));
			}
		}

		// Token: 0x060052E5 RID: 21221 RVA: 0x0012CCA0 File Offset: 0x0012BCA0
		public static GenericAce CreateFromBinaryForm(byte[] binaryForm, int offset)
		{
			GenericAce.VerifyHeader(binaryForm, offset);
			AceType aceType = (AceType)binaryForm[offset];
			GenericAce genericAce;
			if (aceType == AceType.AccessAllowed || aceType == AceType.AccessDenied || aceType == AceType.SystemAudit || aceType == AceType.SystemAlarm || aceType == AceType.AccessAllowedCallback || aceType == AceType.AccessDeniedCallback || aceType == AceType.SystemAuditCallback || aceType == AceType.SystemAlarmCallback)
			{
				AceQualifier aceQualifier;
				int num;
				SecurityIdentifier securityIdentifier;
				bool flag;
				byte[] array;
				if (!CommonAce.ParseBinaryForm(binaryForm, offset, out aceQualifier, out num, out securityIdentifier, out flag, out array))
				{
					goto IL_01A8;
				}
				AceFlags aceFlags = (AceFlags)binaryForm[offset + 1];
				genericAce = new CommonAce(aceFlags, aceQualifier, num, securityIdentifier, flag, array);
			}
			else if (aceType == AceType.AccessAllowedObject || aceType == AceType.AccessDeniedObject || aceType == AceType.SystemAuditObject || aceType == AceType.SystemAlarmObject || aceType == AceType.AccessAllowedCallbackObject || aceType == AceType.AccessDeniedCallbackObject || aceType == AceType.SystemAuditCallbackObject || aceType == AceType.SystemAlarmCallbackObject)
			{
				AceQualifier aceQualifier2;
				int num2;
				SecurityIdentifier securityIdentifier2;
				ObjectAceFlags objectAceFlags;
				Guid guid;
				Guid guid2;
				bool flag2;
				byte[] array2;
				if (!ObjectAce.ParseBinaryForm(binaryForm, offset, out aceQualifier2, out num2, out securityIdentifier2, out objectAceFlags, out guid, out guid2, out flag2, out array2))
				{
					goto IL_01A8;
				}
				AceFlags aceFlags2 = (AceFlags)binaryForm[offset + 1];
				genericAce = new ObjectAce(aceFlags2, aceQualifier2, num2, securityIdentifier2, objectAceFlags, guid, guid2, flag2, array2);
			}
			else if (aceType == AceType.AccessAllowedCompound)
			{
				int num3;
				CompoundAceType compoundAceType;
				SecurityIdentifier securityIdentifier3;
				if (!CompoundAce.ParseBinaryForm(binaryForm, offset, out num3, out compoundAceType, out securityIdentifier3))
				{
					goto IL_01A8;
				}
				AceFlags aceFlags3 = (AceFlags)binaryForm[offset + 1];
				genericAce = new CompoundAce(aceFlags3, num3, compoundAceType, securityIdentifier3);
			}
			else
			{
				AceFlags aceFlags4 = (AceFlags)binaryForm[offset + 1];
				byte[] array3 = null;
				int num4 = (int)binaryForm[offset + 2] + ((int)binaryForm[offset + 3] << 8);
				if (num4 % 4 != 0)
				{
					goto IL_01A8;
				}
				int num5 = num4 - 4;
				if (num5 > 0)
				{
					array3 = new byte[num5];
					for (int i = 0; i < num5; i++)
					{
						array3[i] = binaryForm[offset + num4 - num5 + i];
					}
				}
				genericAce = new CustomAce(aceType, aceFlags4, array3);
			}
			if ((genericAce is ObjectAce || (int)binaryForm[offset + 2] + ((int)binaryForm[offset + 3] << 8) == genericAce.BinaryLength) && (!(genericAce is ObjectAce) || (int)binaryForm[offset + 2] + ((int)binaryForm[offset + 3] << 8) == genericAce.BinaryLength || (int)binaryForm[offset + 2] + ((int)binaryForm[offset + 3] << 8) - 32 == genericAce.BinaryLength))
			{
				return genericAce;
			}
			IL_01A8:
			throw new ArgumentException(Environment.GetResourceString("ArgumentException_InvalidAceBinaryForm"), "binaryForm");
		}

		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x060052E6 RID: 21222 RVA: 0x0012CE69 File Offset: 0x0012BE69
		public AceType AceType
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000E4D RID: 3661
		// (get) Token: 0x060052E7 RID: 21223 RVA: 0x0012CE71 File Offset: 0x0012BE71
		// (set) Token: 0x060052E8 RID: 21224 RVA: 0x0012CE79 File Offset: 0x0012BE79
		public AceFlags AceFlags
		{
			get
			{
				return this._flags;
			}
			set
			{
				this._flags = value;
			}
		}

		// Token: 0x17000E4E RID: 3662
		// (get) Token: 0x060052E9 RID: 21225 RVA: 0x0012CE82 File Offset: 0x0012BE82
		public bool IsInherited
		{
			get
			{
				return (byte)(this.AceFlags & AceFlags.Inherited) != 0;
			}
		}

		// Token: 0x17000E4F RID: 3663
		// (get) Token: 0x060052EA RID: 21226 RVA: 0x0012CE94 File Offset: 0x0012BE94
		public InheritanceFlags InheritanceFlags
		{
			get
			{
				InheritanceFlags inheritanceFlags = InheritanceFlags.None;
				if ((byte)(this.AceFlags & AceFlags.ContainerInherit) != 0)
				{
					inheritanceFlags |= InheritanceFlags.ContainerInherit;
				}
				if ((byte)(this.AceFlags & AceFlags.ObjectInherit) != 0)
				{
					inheritanceFlags |= InheritanceFlags.ObjectInherit;
				}
				return inheritanceFlags;
			}
		}

		// Token: 0x17000E50 RID: 3664
		// (get) Token: 0x060052EB RID: 21227 RVA: 0x0012CEC4 File Offset: 0x0012BEC4
		public PropagationFlags PropagationFlags
		{
			get
			{
				PropagationFlags propagationFlags = PropagationFlags.None;
				if ((byte)(this.AceFlags & AceFlags.InheritOnly) != 0)
				{
					propagationFlags |= PropagationFlags.InheritOnly;
				}
				if ((byte)(this.AceFlags & AceFlags.NoPropagateInherit) != 0)
				{
					propagationFlags |= PropagationFlags.NoPropagateInherit;
				}
				return propagationFlags;
			}
		}

		// Token: 0x17000E51 RID: 3665
		// (get) Token: 0x060052EC RID: 21228 RVA: 0x0012CEF4 File Offset: 0x0012BEF4
		public AuditFlags AuditFlags
		{
			get
			{
				AuditFlags auditFlags = AuditFlags.None;
				if ((byte)(this.AceFlags & AceFlags.SuccessfulAccess) != 0)
				{
					auditFlags |= AuditFlags.Success;
				}
				if ((byte)(this.AceFlags & AceFlags.FailedAccess) != 0)
				{
					auditFlags |= AuditFlags.Failure;
				}
				return auditFlags;
			}
		}

		// Token: 0x17000E52 RID: 3666
		// (get) Token: 0x060052ED RID: 21229
		public abstract int BinaryLength { get; }

		// Token: 0x060052EE RID: 21230
		public abstract void GetBinaryForm(byte[] binaryForm, int offset);

		// Token: 0x060052EF RID: 21231 RVA: 0x0012CF28 File Offset: 0x0012BF28
		public GenericAce Copy()
		{
			byte[] array = new byte[this.BinaryLength];
			this.GetBinaryForm(array, 0);
			return GenericAce.CreateFromBinaryForm(array, 0);
		}

		// Token: 0x060052F0 RID: 21232 RVA: 0x0012CF50 File Offset: 0x0012BF50
		public sealed override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			GenericAce genericAce = o as GenericAce;
			if (genericAce == null)
			{
				return false;
			}
			if (this.AceType != genericAce.AceType || this.AceFlags != genericAce.AceFlags)
			{
				return false;
			}
			int binaryLength = this.BinaryLength;
			int binaryLength2 = genericAce.BinaryLength;
			if (binaryLength != binaryLength2)
			{
				return false;
			}
			byte[] array = new byte[binaryLength];
			byte[] array2 = new byte[binaryLength2];
			this.GetBinaryForm(array, 0);
			genericAce.GetBinaryForm(array2, 0);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != array2[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060052F1 RID: 21233 RVA: 0x0012CFE8 File Offset: 0x0012BFE8
		public sealed override int GetHashCode()
		{
			int binaryLength = this.BinaryLength;
			byte[] array = new byte[binaryLength];
			this.GetBinaryForm(array, 0);
			int num = 0;
			for (int i = 0; i < binaryLength; i += 4)
			{
				int num2 = (int)array[i] + ((int)array[i + 1] << 8) + ((int)array[i + 2] << 16) + ((int)array[i + 3] << 24);
				num ^= num2;
			}
			return num;
		}

		// Token: 0x060052F2 RID: 21234 RVA: 0x0012D040 File Offset: 0x0012C040
		public static bool operator ==(GenericAce left, GenericAce right)
		{
			return (left == null && right == null) || (left != null && right != null && left.Equals(right));
		}

		// Token: 0x060052F3 RID: 21235 RVA: 0x0012D068 File Offset: 0x0012C068
		public static bool operator !=(GenericAce left, GenericAce right)
		{
			return !(left == right);
		}

		// Token: 0x04002AE0 RID: 10976
		internal const int HeaderLength = 4;

		// Token: 0x04002AE1 RID: 10977
		private readonly AceType _type;

		// Token: 0x04002AE2 RID: 10978
		private AceFlags _flags;

		// Token: 0x04002AE3 RID: 10979
		internal ushort _indexInAcl;
	}
}
