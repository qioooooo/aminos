using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000C5 RID: 197
	[ComVisible(true)]
	[Serializable]
	public struct Int64 : IComparable, IFormattable, IConvertible, IComparable<long>, IEquatable<long>
	{
		// Token: 0x06000B48 RID: 2888 RVA: 0x00022AD0 File Offset: 0x00021AD0
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is long))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeInt64"));
			}
			long num = (long)value;
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

		// Token: 0x06000B49 RID: 2889 RVA: 0x00022B10 File Offset: 0x00021B10
		public int CompareTo(long value)
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

		// Token: 0x06000B4A RID: 2890 RVA: 0x00022B21 File Offset: 0x00021B21
		public override bool Equals(object obj)
		{
			return obj is long && this == (long)obj;
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x00022B37 File Offset: 0x00021B37
		public bool Equals(long obj)
		{
			return this == obj;
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00022B3E File Offset: 0x00021B3E
		public override int GetHashCode()
		{
			return (int)this ^ (int)(this >> 32);
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x00022B4A File Offset: 0x00021B4A
		public override string ToString()
		{
			return Number.FormatInt64(this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x00022B59 File Offset: 0x00021B59
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatInt64(this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x00022B69 File Offset: 0x00021B69
		public string ToString(string format)
		{
			return Number.FormatInt64(this, format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x00022B78 File Offset: 0x00021B78
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatInt64(this, format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x00022B88 File Offset: 0x00021B88
		public static long Parse(string s)
		{
			return Number.ParseInt64(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x00022B96 File Offset: 0x00021B96
		public static long Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.ParseInt64(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x00022BAA File Offset: 0x00021BAA
		public static long Parse(string s, IFormatProvider provider)
		{
			return Number.ParseInt64(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x00022BB9 File Offset: 0x00021BB9
		public static long Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.ParseInt64(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x00022BCE File Offset: 0x00021BCE
		public static bool TryParse(string s, out long result)
		{
			return Number.TryParseInt64(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x00022BDD File Offset: 0x00021BDD
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out long result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.TryParseInt64(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00022BF3 File Offset: 0x00021BF3
		public TypeCode GetTypeCode()
		{
			return TypeCode.Int64;
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x00022BF7 File Offset: 0x00021BF7
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00022C00 File Offset: 0x00021C00
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00022C09 File Offset: 0x00021C09
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00022C12 File Offset: 0x00021C12
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00022C1B File Offset: 0x00021C1B
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x00022C24 File Offset: 0x00021C24
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x00022C2D File Offset: 0x00021C2D
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x00022C36 File Offset: 0x00021C36
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x00022C3F File Offset: 0x00021C3F
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x00022C43 File Offset: 0x00021C43
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x00022C4C File Offset: 0x00021C4C
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00022C55 File Offset: 0x00021C55
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x00022C5E File Offset: 0x00021C5E
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x00022C68 File Offset: 0x00021C68
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "Int64", "DateTime" }));
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x00022CA6 File Offset: 0x00021CA6
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x040003FB RID: 1019
		public const long MaxValue = 9223372036854775807L;

		// Token: 0x040003FC RID: 1020
		public const long MinValue = -9223372036854775808L;

		// Token: 0x040003FD RID: 1021
		internal long m_value;
	}
}
