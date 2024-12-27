using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200007A RID: 122
	[ComVisible(true)]
	[Serializable]
	public struct Byte : IComparable, IFormattable, IConvertible, IComparable<byte>, IEquatable<byte>
	{
		// Token: 0x060006CD RID: 1741 RVA: 0x00016A35 File Offset: 0x00015A35
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is byte))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeByte"));
			}
			return (int)(this - (byte)value);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00016A5D File Offset: 0x00015A5D
		public int CompareTo(byte value)
		{
			return (int)(this - value);
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00016A63 File Offset: 0x00015A63
		public override bool Equals(object obj)
		{
			return obj is byte && this == (byte)obj;
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00016A79 File Offset: 0x00015A79
		public bool Equals(byte obj)
		{
			return this == obj;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00016A80 File Offset: 0x00015A80
		public override int GetHashCode()
		{
			return (int)this;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00016A84 File Offset: 0x00015A84
		public static byte Parse(string s)
		{
			return byte.Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00016A92 File Offset: 0x00015A92
		public static byte Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return byte.Parse(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00016AA6 File Offset: 0x00015AA6
		public static byte Parse(string s, IFormatProvider provider)
		{
			return byte.Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00016AB5 File Offset: 0x00015AB5
		public static byte Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return byte.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00016ACC File Offset: 0x00015ACC
		private static byte Parse(string s, NumberStyles style, NumberFormatInfo info)
		{
			int num = 0;
			try
			{
				num = Number.ParseInt32(s, style, info);
			}
			catch (OverflowException ex)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"), ex);
			}
			if (num < 0 || num > 255)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)num;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00016B28 File Offset: 0x00015B28
		public static bool TryParse(string s, out byte result)
		{
			return byte.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00016B37 File Offset: 0x00015B37
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out byte result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return byte.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00016B50 File Offset: 0x00015B50
		private static bool TryParse(string s, NumberStyles style, NumberFormatInfo info, out byte result)
		{
			result = 0;
			int num;
			if (!Number.TryParseInt32(s, style, info, out num))
			{
				return false;
			}
			if (num < 0 || num > 255)
			{
				return false;
			}
			result = (byte)num;
			return true;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00016B81 File Offset: 0x00015B81
		public override string ToString()
		{
			return Number.FormatInt32((int)this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00016B90 File Offset: 0x00015B90
		public string ToString(string format)
		{
			return Number.FormatInt32((int)this, format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00016B9F File Offset: 0x00015B9F
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatInt32((int)this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00016BAF File Offset: 0x00015BAF
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatInt32((int)this, format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00016BBF File Offset: 0x00015BBF
		public TypeCode GetTypeCode()
		{
			return TypeCode.Byte;
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00016BC2 File Offset: 0x00015BC2
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00016BCB File Offset: 0x00015BCB
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00016BD4 File Offset: 0x00015BD4
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00016BDD File Offset: 0x00015BDD
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00016BE1 File Offset: 0x00015BE1
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00016BEA File Offset: 0x00015BEA
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00016BF3 File Offset: 0x00015BF3
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00016BFC File Offset: 0x00015BFC
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00016C05 File Offset: 0x00015C05
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00016C0E File Offset: 0x00015C0E
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00016C17 File Offset: 0x00015C17
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00016C20 File Offset: 0x00015C20
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00016C29 File Offset: 0x00015C29
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00016C34 File Offset: 0x00015C34
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "Byte", "DateTime" }));
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00016C72 File Offset: 0x00015C72
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x0400021E RID: 542
		public const byte MaxValue = 255;

		// Token: 0x0400021F RID: 543
		public const byte MinValue = 0;

		// Token: 0x04000220 RID: 544
		private byte m_value;
	}
}
