using System;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000113 RID: 275
	[ComVisible(true)]
	[Serializable]
	public struct Single : IComparable, IFormattable, IConvertible, IComparable<float>, IEquatable<float>
	{
		// Token: 0x06001001 RID: 4097 RVA: 0x0002DB0B File Offset: 0x0002CB0B
		public unsafe static bool IsInfinity(float f)
		{
			return (*(int*)(&f) & int.MaxValue) == 2139095040;
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0002DB1E File Offset: 0x0002CB1E
		public unsafe static bool IsPositiveInfinity(float f)
		{
			return *(int*)(&f) == 2139095040;
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0002DB2B File Offset: 0x0002CB2B
		public unsafe static bool IsNegativeInfinity(float f)
		{
			return *(int*)(&f) == -8388608;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0002DB38 File Offset: 0x0002CB38
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static bool IsNaN(float f)
		{
			return f != f;
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0002DB44 File Offset: 0x0002CB44
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is float))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeSingle"));
			}
			float num = (float)value;
			if (this < num)
			{
				return -1;
			}
			if (this > num)
			{
				return 1;
			}
			if (this == num)
			{
				return 0;
			}
			if (!float.IsNaN(this))
			{
				return 1;
			}
			if (!float.IsNaN(num))
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0002DBA0 File Offset: 0x0002CBA0
		public int CompareTo(float value)
		{
			if (this < value)
			{
				return -1;
			}
			if (this > value)
			{
				return 1;
			}
			if (this == value)
			{
				return 0;
			}
			if (!float.IsNaN(this))
			{
				return 1;
			}
			if (!float.IsNaN(value))
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0002DBD0 File Offset: 0x0002CBD0
		public override bool Equals(object obj)
		{
			if (!(obj is float))
			{
				return false;
			}
			float num = (float)obj;
			return num == this || (float.IsNaN(num) && float.IsNaN(this));
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0002DC06 File Offset: 0x0002CC06
		public bool Equals(float obj)
		{
			return obj == this || (float.IsNaN(obj) && float.IsNaN(this));
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0002DC20 File Offset: 0x0002CC20
		public unsafe override int GetHashCode()
		{
			float num = this;
			if (num == 0f)
			{
				return 0;
			}
			return *(int*)(&num);
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0002DC40 File Offset: 0x0002CC40
		public override string ToString()
		{
			return Number.FormatSingle(this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0002DC4F File Offset: 0x0002CC4F
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatSingle(this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0002DC5F File Offset: 0x0002CC5F
		public string ToString(string format)
		{
			return Number.FormatSingle(this, format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0002DC6E File Offset: 0x0002CC6E
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatSingle(this, format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0002DC7E File Offset: 0x0002CC7E
		public static float Parse(string s)
		{
			return float.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0002DC90 File Offset: 0x0002CC90
		public static float Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			return float.Parse(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0002DCA4 File Offset: 0x0002CCA4
		public static float Parse(string s, IFormatProvider provider)
		{
			return float.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0002DCB7 File Offset: 0x0002CCB7
		public static float Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			return float.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0002DCCC File Offset: 0x0002CCCC
		private static float Parse(string s, NumberStyles style, NumberFormatInfo info)
		{
			float num;
			try
			{
				num = Number.ParseSingle(s, style, info);
			}
			catch (FormatException)
			{
				string text = s.Trim();
				if (text.Equals(info.PositiveInfinitySymbol))
				{
					num = float.PositiveInfinity;
				}
				else if (text.Equals(info.NegativeInfinitySymbol))
				{
					num = float.NegativeInfinity;
				}
				else
				{
					if (!text.Equals(info.NaNSymbol))
					{
						throw;
					}
					num = float.NaN;
				}
			}
			return num;
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0002DD44 File Offset: 0x0002CD44
		public static bool TryParse(string s, out float result)
		{
			return float.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0002DD57 File Offset: 0x0002CD57
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out float result)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			return float.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0002DD70 File Offset: 0x0002CD70
		private static bool TryParse(string s, NumberStyles style, NumberFormatInfo info, out float result)
		{
			if (s == null)
			{
				result = 0f;
				return false;
			}
			if (!Number.TryParseSingle(s, style, info, out result))
			{
				string text = s.Trim();
				if (text.Equals(info.PositiveInfinitySymbol))
				{
					result = float.PositiveInfinity;
				}
				else if (text.Equals(info.NegativeInfinitySymbol))
				{
					result = float.NegativeInfinity;
				}
				else
				{
					if (!text.Equals(info.NaNSymbol))
					{
						return false;
					}
					result = float.NaN;
				}
			}
			return true;
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0002DDE5 File Offset: 0x0002CDE5
		public TypeCode GetTypeCode()
		{
			return TypeCode.Single;
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0002DDE9 File Offset: 0x0002CDE9
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x0002DDF4 File Offset: 0x0002CDF4
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "Single", "Char" }));
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0002DE32 File Offset: 0x0002CE32
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0002DE3B File Offset: 0x0002CE3B
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0002DE44 File Offset: 0x0002CE44
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0002DE4D File Offset: 0x0002CE4D
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0002DE56 File Offset: 0x0002CE56
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0002DE5F File Offset: 0x0002CE5F
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0002DE68 File Offset: 0x0002CE68
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x0002DE71 File Offset: 0x0002CE71
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0002DE7A File Offset: 0x0002CE7A
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0002DE7E File Offset: 0x0002CE7E
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0002DE87 File Offset: 0x0002CE87
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0002DE90 File Offset: 0x0002CE90
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "Single", "DateTime" }));
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0002DECE File Offset: 0x0002CECE
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x0400052C RID: 1324
		public const float MinValue = -3.4028235E+38f;

		// Token: 0x0400052D RID: 1325
		public const float Epsilon = 1E-45f;

		// Token: 0x0400052E RID: 1326
		public const float MaxValue = 3.4028235E+38f;

		// Token: 0x0400052F RID: 1327
		public const float PositiveInfinity = float.PositiveInfinity;

		// Token: 0x04000530 RID: 1328
		public const float NegativeInfinity = float.NegativeInfinity;

		// Token: 0x04000531 RID: 1329
		public const float NaN = float.NaN;

		// Token: 0x04000532 RID: 1330
		internal float m_value;
	}
}
