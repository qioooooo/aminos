using System;
using System.Globalization;

namespace System.Security.AccessControl
{
	// Token: 0x020008E6 RID: 2278
	public sealed class CustomAce : GenericAce
	{
		// Token: 0x060052F9 RID: 21241 RVA: 0x0012D0D8 File Offset: 0x0012C0D8
		public CustomAce(AceType type, AceFlags flags, byte[] opaque)
			: base(type, flags)
		{
			if (type <= AceType.SystemAlarmCallbackObject)
			{
				throw new ArgumentOutOfRangeException("type", Environment.GetResourceString("ArgumentOutOfRange_InvalidUserDefinedAceType"));
			}
			this.SetOpaque(opaque);
		}

		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x060052FA RID: 21242 RVA: 0x0012D103 File Offset: 0x0012C103
		public int OpaqueLength
		{
			get
			{
				if (this._opaque == null)
				{
					return 0;
				}
				return this._opaque.Length;
			}
		}

		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x060052FB RID: 21243 RVA: 0x0012D117 File Offset: 0x0012C117
		public override int BinaryLength
		{
			get
			{
				return 4 + this.OpaqueLength;
			}
		}

		// Token: 0x060052FC RID: 21244 RVA: 0x0012D121 File Offset: 0x0012C121
		public byte[] GetOpaque()
		{
			return this._opaque;
		}

		// Token: 0x060052FD RID: 21245 RVA: 0x0012D12C File Offset: 0x0012C12C
		public void SetOpaque(byte[] opaque)
		{
			if (opaque != null)
			{
				if (opaque.Length > CustomAce.MaxOpaqueLength)
				{
					throw new ArgumentOutOfRangeException("opaque", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_ArrayLength"), new object[]
					{
						0,
						CustomAce.MaxOpaqueLength
					}));
				}
				if (opaque.Length % 4 != 0)
				{
					throw new ArgumentOutOfRangeException("opaque", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_ArrayLengthMultiple"), new object[] { 4 }));
				}
			}
			this._opaque = opaque;
		}

		// Token: 0x060052FE RID: 21246 RVA: 0x0012D1C1 File Offset: 0x0012C1C1
		public override void GetBinaryForm(byte[] binaryForm, int offset)
		{
			base.MarshalHeader(binaryForm, offset);
			offset += 4;
			if (this.OpaqueLength != 0)
			{
				if (this.OpaqueLength > CustomAce.MaxOpaqueLength)
				{
					throw new SystemException();
				}
				this.GetOpaque().CopyTo(binaryForm, offset);
			}
		}

		// Token: 0x04002AE7 RID: 10983
		private byte[] _opaque;

		// Token: 0x04002AE8 RID: 10984
		public static readonly int MaxOpaqueLength = 65531;
	}
}
