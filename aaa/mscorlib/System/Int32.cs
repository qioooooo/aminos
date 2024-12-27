using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000C4 RID: 196
	[ComVisible(true)]
	[Serializable]
	public struct Int32 : IComparable, IFormattable, IConvertible, IComparable<int>, IEquatable<int>
	{
		// Token: 0x06000B29 RID: 2857 RVA: 0x000228F0 File Offset: 0x000218F0
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is int))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeInt32"));
			}
			int num = (int)value;
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

		// Token: 0x06000B2A RID: 2858 RVA: 0x00022930 File Offset: 0x00021930
		public int CompareTo(int value)
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

		// Token: 0x06000B2B RID: 2859 RVA: 0x00022941 File Offset: 0x00021941
		public override bool Equals(object obj)
		{
			return obj is int && this == (int)obj;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00022957 File Offset: 0x00021957
		public bool Equals(int obj)
		{
			return this == obj;
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0002295E File Offset: 0x0002195E
		public override int GetHashCode()
		{
			return this;
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x00022962 File Offset: 0x00021962
		public override string ToString()
		{
			return Number.FormatInt32(this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x00022971 File Offset: 0x00021971
		public string ToString(string format)
		{
			return Number.FormatInt32(this, format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00022980 File Offset: 0x00021980
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatInt32(this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00022990 File Offset: 0x00021990
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatInt32(this, format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x000229A0 File Offset: 0x000219A0
		public static int Parse(string s)
		{
			return Number.ParseInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x000229AE File Offset: 0x000219AE
		public static int Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.ParseInt32(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x000229C2 File Offset: 0x000219C2
		public static int Parse(string s, IFormatProvider provider)
		{
			return Number.ParseInt32(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x000229D1 File Offset: 0x000219D1
		public static int Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.ParseInt32(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x000229E6 File Offset: 0x000219E6
		public static bool TryParse(string s, out int result)
		{
			return Number.TryParseInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x000229F5 File Offset: 0x000219F5
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out int result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.TryParseInt32(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x00022A0B File Offset: 0x00021A0B
		public TypeCode GetTypeCode()
		{
			return TypeCode.Int32;
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x00022A0F File Offset: 0x00021A0F
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x00022A18 File Offset: 0x00021A18
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00022A21 File Offset: 0x00021A21
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00022A2A File Offset: 0x00021A2A
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00022A33 File Offset: 0x00021A33
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x00022A3C File Offset: 0x00021A3C
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00022A45 File Offset: 0x00021A45
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00022A49 File Offset: 0x00021A49
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00022A52 File Offset: 0x00021A52
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x00022A5B File Offset: 0x00021A5B
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00022A64 File Offset: 0x00021A64
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00022A6D File Offset: 0x00021A6D
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00022A76 File Offset: 0x00021A76
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00022A80 File Offset: 0x00021A80
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "Int32", "DateTime" }));
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00022ABE File Offset: 0x00021ABE
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x040003F8 RID: 1016
		public const int MaxValue = 2147483647;

		// Token: 0x040003F9 RID: 1017
		public const int MinValue = -2147483648;

		// Token: 0x040003FA RID: 1018
		internal int m_value;
	}
}
