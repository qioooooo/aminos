using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008E8 RID: 2280
	public sealed class CompoundAce : KnownAce
	{
		// Token: 0x06005300 RID: 21248 RVA: 0x0012D204 File Offset: 0x0012C204
		public CompoundAce(AceFlags flags, int accessMask, CompoundAceType compoundAceType, SecurityIdentifier sid)
			: base(AceType.AccessAllowedCompound, flags, accessMask, sid)
		{
			this._compoundAceType = compoundAceType;
		}

		// Token: 0x06005301 RID: 21249 RVA: 0x0012D218 File Offset: 0x0012C218
		internal static bool ParseBinaryForm(byte[] binaryForm, int offset, out int accessMask, out CompoundAceType compoundAceType, out SecurityIdentifier sid)
		{
			GenericAce.VerifyHeader(binaryForm, offset);
			if (binaryForm.Length - offset >= 12 + SecurityIdentifier.MinBinaryLength)
			{
				int num = offset + 4;
				int num2 = 0;
				accessMask = (int)binaryForm[num] + ((int)binaryForm[num + 1] << 8) + ((int)binaryForm[num + 2] << 16) + ((int)binaryForm[num + 3] << 24);
				num2 += 4;
				compoundAceType = (CompoundAceType)((int)binaryForm[num + num2] + ((int)binaryForm[num + num2 + 1] << 8));
				num2 += 4;
				sid = new SecurityIdentifier(binaryForm, num + num2);
				return true;
			}
			accessMask = 0;
			compoundAceType = (CompoundAceType)0;
			sid = null;
			return false;
		}

		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x06005302 RID: 21250 RVA: 0x0012D292 File Offset: 0x0012C292
		// (set) Token: 0x06005303 RID: 21251 RVA: 0x0012D29A File Offset: 0x0012C29A
		public CompoundAceType CompoundAceType
		{
			get
			{
				return this._compoundAceType;
			}
			set
			{
				this._compoundAceType = value;
			}
		}

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x06005304 RID: 21252 RVA: 0x0012D2A3 File Offset: 0x0012C2A3
		public override int BinaryLength
		{
			get
			{
				return 12 + base.SecurityIdentifier.BinaryLength;
			}
		}

		// Token: 0x06005305 RID: 21253 RVA: 0x0012D2B4 File Offset: 0x0012C2B4
		public override void GetBinaryForm(byte[] binaryForm, int offset)
		{
			base.MarshalHeader(binaryForm, offset);
			int num = offset + 4;
			int num2 = 0;
			binaryForm[num] = (byte)base.AccessMask;
			binaryForm[num + 1] = (byte)(base.AccessMask >> 8);
			binaryForm[num + 2] = (byte)(base.AccessMask >> 16);
			binaryForm[num + 3] = (byte)(base.AccessMask >> 24);
			num2 += 4;
			binaryForm[num + num2] = (byte)((ushort)this.CompoundAceType);
			binaryForm[num + num2 + 1] = (byte)((ushort)this.CompoundAceType >> 8);
			binaryForm[num + num2 + 2] = 0;
			binaryForm[num + num2 + 3] = 0;
			num2 += 4;
			base.SecurityIdentifier.GetBinaryForm(binaryForm, num + num2);
		}

		// Token: 0x04002AEB RID: 10987
		private const int AceTypeLength = 4;

		// Token: 0x04002AEC RID: 10988
		private CompoundAceType _compoundAceType;
	}
}
