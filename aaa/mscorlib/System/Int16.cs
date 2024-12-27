using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000C3 RID: 195
	[ComVisible(true)]
	[Serializable]
	public struct Int16 : IComparable, IFormattable, IConvertible, IComparable<short>, IEquatable<short>
	{
		// Token: 0x06000B07 RID: 2823 RVA: 0x000225FB File Offset: 0x000215FB
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (value is short)
			{
				return (int)(this - (short)value);
			}
			throw new ArgumentException(Environment.GetResourceString("Arg_MustBeInt16"));
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00022623 File Offset: 0x00021623
		public int CompareTo(short value)
		{
			return (int)(this - value);
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x00022629 File Offset: 0x00021629
		public override bool Equals(object obj)
		{
			return obj is short && this == (short)obj;
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0002263F File Offset: 0x0002163F
		public bool Equals(short obj)
		{
			return this == obj;
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00022646 File Offset: 0x00021646
		public override int GetHashCode()
		{
			return (int)((ushort)this) | ((int)this << 16);
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x00022651 File Offset: 0x00021651
		public override string ToString()
		{
			return Number.FormatInt32((int)this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00022660 File Offset: 0x00021660
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatInt32((int)this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00022670 File Offset: 0x00021670
		public string ToString(string format)
		{
			return this.ToString(format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x0002267E File Offset: 0x0002167E
		public string ToString(string format, IFormatProvider provider)
		{
			return this.ToString(format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x00022690 File Offset: 0x00021690
		private string ToString(string format, NumberFormatInfo info)
		{
			if (this < 0 && format != null && format.Length > 0 && (format[0] == 'X' || format[0] == 'x'))
			{
				uint num = (uint)this & 65535U;
				return Number.FormatUInt32(num, format, info);
			}
			return Number.FormatInt32((int)this, format, info);
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x000226DF File Offset: 0x000216DF
		public static short Parse(string s)
		{
			return short.Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x000226ED File Offset: 0x000216ED
		public static short Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return short.Parse(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00022701 File Offset: 0x00021701
		public static short Parse(string s, IFormatProvider provider)
		{
			return short.Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x00022710 File Offset: 0x00021710
		public static short Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return short.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x00022728 File Offset: 0x00021728
		private static short Parse(string s, NumberStyles style, NumberFormatInfo info)
		{
			int num = 0;
			try
			{
				num = Number.ParseInt32(s, style, info);
			}
			catch (OverflowException ex)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int16"), ex);
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (num < 0 || num > 65535)
				{
					throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
				}
				return (short)num;
			}
			else
			{
				if (num < -32768 || num > 32767)
				{
					throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
				}
				return (short)num;
			}
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x000227B0 File Offset: 0x000217B0
		public static bool TryParse(string s, out short result)
		{
			return short.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x000227BF File Offset: 0x000217BF
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out short result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return short.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x000227D8 File Offset: 0x000217D8
		private static bool TryParse(string s, NumberStyles style, NumberFormatInfo info, out short result)
		{
			result = 0;
			int num;
			if (!Number.TryParseInt32(s, style, info, out num))
			{
				return false;
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (num < 0 || num > 65535)
				{
					return false;
				}
				result = (short)num;
				return true;
			}
			else
			{
				if (num < -32768 || num > 32767)
				{
					return false;
				}
				result = (short)num;
				return true;
			}
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0002282A File Offset: 0x0002182A
		public TypeCode GetTypeCode()
		{
			return TypeCode.Int16;
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0002282D File Offset: 0x0002182D
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00022836 File Offset: 0x00021836
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0002283F File Offset: 0x0002183F
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00022848 File Offset: 0x00021848
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x00022851 File Offset: 0x00021851
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x00022855 File Offset: 0x00021855
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0002285E File Offset: 0x0002185E
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00022867 File Offset: 0x00021867
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00022870 File Offset: 0x00021870
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00022879 File Offset: 0x00021879
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x00022882 File Offset: 0x00021882
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x0002288B File Offset: 0x0002188B
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00022894 File Offset: 0x00021894
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x000228A0 File Offset: 0x000218A0
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "Int16", "DateTime" }));
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x000228DE File Offset: 0x000218DE
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x040003F5 RID: 1013
		public const short MaxValue = 32767;

		// Token: 0x040003F6 RID: 1014
		public const short MinValue = -32768;

		// Token: 0x040003F7 RID: 1015
		internal short m_value;
	}
}
