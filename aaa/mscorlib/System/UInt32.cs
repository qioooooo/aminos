using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200011C RID: 284
	[CLSCompliant(false)]
	[ComVisible(true)]
	[Serializable]
	public struct UInt32 : IComparable, IFormattable, IConvertible, IComparable<uint>, IEquatable<uint>
	{
		// Token: 0x06001094 RID: 4244 RVA: 0x0002EF14 File Offset: 0x0002DF14
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is uint))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeUInt32"));
			}
			uint num = (uint)value;
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

		// Token: 0x06001095 RID: 4245 RVA: 0x0002EF54 File Offset: 0x0002DF54
		public int CompareTo(uint value)
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

		// Token: 0x06001096 RID: 4246 RVA: 0x0002EF65 File Offset: 0x0002DF65
		public override bool Equals(object obj)
		{
			return obj is uint && this == (uint)obj;
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x0002EF7B File Offset: 0x0002DF7B
		public bool Equals(uint obj)
		{
			return this == obj;
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0002EF82 File Offset: 0x0002DF82
		public override int GetHashCode()
		{
			return (int)this;
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0002EF86 File Offset: 0x0002DF86
		public override string ToString()
		{
			return Number.FormatUInt32(this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0002EF95 File Offset: 0x0002DF95
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatUInt32(this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0002EFA5 File Offset: 0x0002DFA5
		public string ToString(string format)
		{
			return Number.FormatUInt32(this, format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0002EFB4 File Offset: 0x0002DFB4
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatUInt32(this, format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0002EFC4 File Offset: 0x0002DFC4
		[CLSCompliant(false)]
		public static uint Parse(string s)
		{
			return Number.ParseUInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x0002EFD2 File Offset: 0x0002DFD2
		[CLSCompliant(false)]
		public static uint Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.ParseUInt32(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x0002EFE6 File Offset: 0x0002DFE6
		[CLSCompliant(false)]
		public static uint Parse(string s, IFormatProvider provider)
		{
			return Number.ParseUInt32(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x0002EFF5 File Offset: 0x0002DFF5
		[CLSCompliant(false)]
		public static uint Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.ParseUInt32(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x0002F00A File Offset: 0x0002E00A
		[CLSCompliant(false)]
		public static bool TryParse(string s, out uint result)
		{
			return Number.TryParseUInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x0002F019 File Offset: 0x0002E019
		[CLSCompliant(false)]
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out uint result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.TryParseUInt32(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x0002F02F File Offset: 0x0002E02F
		public TypeCode GetTypeCode()
		{
			return TypeCode.UInt32;
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x0002F033 File Offset: 0x0002E033
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x0002F03C File Offset: 0x0002E03C
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x0002F045 File Offset: 0x0002E045
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0002F04E File Offset: 0x0002E04E
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0002F057 File Offset: 0x0002E057
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x0002F060 File Offset: 0x0002E060
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0002F069 File Offset: 0x0002E069
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x0002F072 File Offset: 0x0002E072
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x0002F076 File Offset: 0x0002E076
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x0002F07F File Offset: 0x0002E07F
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x0002F088 File Offset: 0x0002E088
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x0002F091 File Offset: 0x0002E091
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x0002F09A File Offset: 0x0002E09A
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x0002F0A4 File Offset: 0x0002E0A4
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "UInt32", "DateTime" }));
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x0002F0E2 File Offset: 0x0002E0E2
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x0400056C RID: 1388
		public const uint MaxValue = 4294967295U;

		// Token: 0x0400056D RID: 1389
		public const uint MinValue = 0U;

		// Token: 0x0400056E RID: 1390
		private uint m_value;
	}
}
