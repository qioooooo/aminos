using System;
using System.Deployment.Internal.Isolation.Manifest;
using System.Globalization;

namespace System.Deployment.Application
{
	// Token: 0x02000030 RID: 48
	internal class Hash
	{
		// Token: 0x060001A0 RID: 416 RVA: 0x0000B949 File Offset: 0x0000A949
		public Hash(byte[] digestValue, CMS_HASH_DIGESTMETHOD digestMethod, CMS_HASH_TRANSFORM transform)
		{
			if (digestValue == null)
			{
				throw new ArgumentException(Resources.GetString("Ex_HashNullDigestValue"));
			}
			this._digestValue = digestValue;
			this._digestMethod = digestMethod;
			this._transform = transform;
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000B979 File Offset: 0x0000A979
		public byte[] DigestValue
		{
			get
			{
				return this._digestValue;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000B981 File Offset: 0x0000A981
		public CMS_HASH_DIGESTMETHOD DigestMethod
		{
			get
			{
				return this._digestMethod;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x0000B989 File Offset: 0x0000A989
		public CMS_HASH_TRANSFORM Transform
		{
			get
			{
				return this._transform;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000B991 File Offset: 0x0000A991
		public string CompositString
		{
			get
			{
				return this.DigestMethodCodeString + this.TranformCodeString + HexString.FromBytes(this.DigestValue);
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000B9AF File Offset: 0x0000A9AF
		protected string TranformCodeString
		{
			get
			{
				return Hash.ToCodedString((uint)this.Transform);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000B9BC File Offset: 0x0000A9BC
		protected string DigestMethodCodeString
		{
			get
			{
				return Hash.ToCodedString((uint)this.DigestMethod);
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000B9CC File Offset: 0x0000A9CC
		protected static string ToCodedString(uint value)
		{
			if (value > 255U)
			{
				throw new ArgumentException(Resources.GetString("Ex_CodeLimitExceeded"));
			}
			return string.Format(CultureInfo.InvariantCulture, "{0:x2}", new object[] { value });
		}

		// Token: 0x0400010C RID: 268
		private byte[] _digestValue;

		// Token: 0x0400010D RID: 269
		private CMS_HASH_DIGESTMETHOD _digestMethod;

		// Token: 0x0400010E RID: 270
		private CMS_HASH_TRANSFORM _transform;
	}
}
