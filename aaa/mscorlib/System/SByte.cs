using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000110 RID: 272
	[CLSCompliant(false)]
	[ComVisible(true)]
	[Serializable]
	public struct SByte : IComparable, IFormattable, IConvertible, IComparable<sbyte>, IEquatable<sbyte>
	{
		// Token: 0x06000FD5 RID: 4053 RVA: 0x0002D677 File Offset: 0x0002C677
		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is sbyte))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeSByte"));
			}
			return (int)(this - (sbyte)obj);
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0002D69F File Offset: 0x0002C69F
		public int CompareTo(sbyte value)
		{
			return (int)(this - value);
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x0002D6A5 File Offset: 0x0002C6A5
		public override bool Equals(object obj)
		{
			return obj is sbyte && this == (sbyte)obj;
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0002D6BB File Offset: 0x0002C6BB
		public bool Equals(sbyte obj)
		{
			return this == obj;
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0002D6C2 File Offset: 0x0002C6C2
		public override int GetHashCode()
		{
			return (int)this ^ ((int)this << 8);
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0002D6CB File Offset: 0x0002C6CB
		public override string ToString()
		{
			return Number.FormatInt32((int)this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0002D6DA File Offset: 0x0002C6DA
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatInt32((int)this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0002D6EA File Offset: 0x0002C6EA
		public string ToString(string format)
		{
			return this.ToString(format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0002D6F8 File Offset: 0x0002C6F8
		public string ToString(string format, IFormatProvider provider)
		{
			return this.ToString(format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x0002D708 File Offset: 0x0002C708
		private string ToString(string format, NumberFormatInfo info)
		{
			if (this < 0 && format != null && format.Length > 0 && (format[0] == 'X' || format[0] == 'x'))
			{
				uint num = (uint)this & 255U;
				return Number.FormatUInt32(num, format, info);
			}
			return Number.FormatInt32((int)this, format, info);
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x0002D757 File Offset: 0x0002C757
		[CLSCompliant(false)]
		public static sbyte Parse(string s)
		{
			return sbyte.Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0002D765 File Offset: 0x0002C765
		[CLSCompliant(false)]
		public static sbyte Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return sbyte.Parse(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0002D779 File Offset: 0x0002C779
		[CLSCompliant(false)]
		public static sbyte Parse(string s, IFormatProvider provider)
		{
			return sbyte.Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0002D788 File Offset: 0x0002C788
		[CLSCompliant(false)]
		public static sbyte Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return sbyte.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0002D7A0 File Offset: 0x0002C7A0
		private static sbyte Parse(string s, NumberStyles style, NumberFormatInfo info)
		{
			int num = 0;
			try
			{
				num = Number.ParseInt32(s, style, info);
			}
			catch (OverflowException ex)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"), ex);
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (num < 0 || num > 255)
				{
					throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
				}
				return (sbyte)num;
			}
			else
			{
				if (num < -128 || num > 127)
				{
					throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
				}
				return (sbyte)num;
			}
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0002D820 File Offset: 0x0002C820
		[CLSCompliant(false)]
		public static bool TryParse(string s, out sbyte result)
		{
			return sbyte.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0002D82F File Offset: 0x0002C82F
		[CLSCompliant(false)]
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out sbyte result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return sbyte.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0002D848 File Offset: 0x0002C848
		private static bool TryParse(string s, NumberStyles style, NumberFormatInfo info, out sbyte result)
		{
			result = 0;
			int num;
			if (!Number.TryParseInt32(s, style, info, out num))
			{
				return false;
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (num < 0 || num > 255)
				{
					return false;
				}
				result = (sbyte)num;
				return true;
			}
			else
			{
				if (num < -128 || num > 127)
				{
					return false;
				}
				result = (sbyte)num;
				return true;
			}
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0002D894 File Offset: 0x0002C894
		public TypeCode GetTypeCode()
		{
			return TypeCode.SByte;
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0002D897 File Offset: 0x0002C897
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0002D8A0 File Offset: 0x0002C8A0
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0002D8A9 File Offset: 0x0002C8A9
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0002D8AD File Offset: 0x0002C8AD
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0002D8B6 File Offset: 0x0002C8B6
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0002D8BF File Offset: 0x0002C8BF
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0002D8C8 File Offset: 0x0002C8C8
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return (int)this;
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0002D8CC File Offset: 0x0002C8CC
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x0002D8D5 File Offset: 0x0002C8D5
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0002D8DE File Offset: 0x0002C8DE
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0002D8E7 File Offset: 0x0002C8E7
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0002D8F0 File Offset: 0x0002C8F0
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0002D8F9 File Offset: 0x0002C8F9
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0002D904 File Offset: 0x0002C904
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromTo", new object[] { "SByte", "DateTime" }));
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0002D938 File Offset: 0x0002C938
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x04000524 RID: 1316
		public const sbyte MaxValue = 127;

		// Token: 0x04000525 RID: 1317
		public const sbyte MinValue = -128;

		// Token: 0x04000526 RID: 1318
		private sbyte m_value;
	}
}
