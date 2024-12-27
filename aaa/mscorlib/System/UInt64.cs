using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200011D RID: 285
	[ComVisible(true)]
	[CLSCompliant(false)]
	[Serializable]
	public struct UInt64 : IComparable, IFormattable, IConvertible, IComparable<ulong>, IEquatable<ulong>
	{
		// Token: 0x060010B3 RID: 4275 RVA: 0x0002F0F4 File Offset: 0x0002E0F4
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is ulong))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeUInt64"));
			}
			ulong num = (ulong)value;
			if (this < num)
			{
				return -1;
			}
			if (this > num)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x0002F134 File Offset: 0x0002E134
		public int CompareTo(ulong value)
		{
			if (this < value)
			{
				return -1;
			}
			if (this > value)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x0002F145 File Offset: 0x0002E145
		public override bool Equals(object obj)
		{
			return obj is ulong && this == (ulong)obj;
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x0002F15B File Offset: 0x0002E15B
		public bool Equals(ulong obj)
		{
			return this == obj;
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x0002F162 File Offset: 0x0002E162
		public override int GetHashCode()
		{
			return (int)this ^ (int)(this >> 32);
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0002F16E File Offset: 0x0002E16E
		public override string ToString()
		{
			return Number.FormatUInt64(this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x0002F17D File Offset: 0x0002E17D
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatUInt64(this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x0002F18D File Offset: 0x0002E18D
		public string ToString(string format)
		{
			return Number.FormatUInt64(this, format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x0002F19C File Offset: 0x0002E19C
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatUInt64(this, format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x0002F1AC File Offset: 0x0002E1AC
		[CLSCompliant(false)]
		public static ulong Parse(string s)
		{
			return Number.ParseUInt64(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x0002F1BA File Offset: 0x0002E1BA
		[CLSCompliant(false)]
		public static ulong Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.ParseUInt64(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x0002F1CE File Offset: 0x0002E1CE
		[CLSCompliant(false)]
		public static ulong Parse(string s, IFormatProvider provider)
		{
			return Number.ParseUInt64(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x0002F1DD File Offset: 0x0002E1DD
		[CLSCompliant(false)]
		public static ulong Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.ParseUInt64(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x0002F1F2 File Offset: 0x0002E1F2
		[CLSCompliant(false)]
		public static bool TryParse(string s, out ulong result)
		{
			return Number.TryParseUInt64(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x0002F201 File Offset: 0x0002E201
		[CLSCompliant(false)]
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ulong result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.TryParseUInt64(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x0002F217 File Offset: 0x0002E217
		public TypeCode GetTypeCode()
		{
			return TypeCode.UInt64;
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x0002F21B File Offset: 0x0002E21B
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x0002F224 File Offset: 0x0002E224
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x0002F22D File Offset: 0x0002E22D
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x0002F236 File Offset: 0x0002E236
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x0002F23F File Offset: 0x0002E23F
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x0002F248 File Offset: 0x0002E248
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x0002F251 File Offset: 0x0002E251
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0002F25A File Offset: 0x0002E25A
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x0002F263 File Offset: 0x0002E263
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x0002F26C File Offset: 0x0002E26C
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x0002F270 File Offset: 0x0002E270
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x0002F279 File Offset: 0x0002E279
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x0002F282 File Offset: 0x0002E282
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x0002F28C File Offset: 0x0002E28C
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "UInt64", "DateTime" }));
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x0002F2CA File Offset: 0x0002E2CA
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x0400056F RID: 1391
		public const ulong MaxValue = 18446744073709551615UL;

		// Token: 0x04000570 RID: 1392
		public const ulong MinValue = 0UL;

		// Token: 0x04000571 RID: 1393
		private ulong m_value;
	}
}
