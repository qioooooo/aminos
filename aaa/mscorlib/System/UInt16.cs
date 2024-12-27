using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200011B RID: 283
	[CLSCompliant(false)]
	[ComVisible(true)]
	[Serializable]
	public struct UInt16 : IComparable, IFormattable, IConvertible, IComparable<ushort>, IEquatable<ushort>
	{
		// Token: 0x06001073 RID: 4211 RVA: 0x0002ECD0 File Offset: 0x0002DCD0
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (value is ushort)
			{
				return (int)(this - (ushort)value);
			}
			throw new ArgumentException(Environment.GetResourceString("Arg_MustBeUInt16"));
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x0002ECF8 File Offset: 0x0002DCF8
		public int CompareTo(ushort value)
		{
			return (int)(this - value);
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x0002ECFE File Offset: 0x0002DCFE
		public override bool Equals(object obj)
		{
			return obj is ushort && this == (ushort)obj;
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x0002ED14 File Offset: 0x0002DD14
		public bool Equals(ushort obj)
		{
			return this == obj;
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x0002ED1B File Offset: 0x0002DD1B
		public override int GetHashCode()
		{
			return (int)this;
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x0002ED1F File Offset: 0x0002DD1F
		public override string ToString()
		{
			return Number.FormatUInt32((uint)this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x0002ED2E File Offset: 0x0002DD2E
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatUInt32((uint)this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x0002ED3E File Offset: 0x0002DD3E
		public string ToString(string format)
		{
			return Number.FormatUInt32((uint)this, format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x0002ED4D File Offset: 0x0002DD4D
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatUInt32((uint)this, format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x0002ED5D File Offset: 0x0002DD5D
		[CLSCompliant(false)]
		public static ushort Parse(string s)
		{
			return ushort.Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x0002ED6B File Offset: 0x0002DD6B
		[CLSCompliant(false)]
		public static ushort Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return ushort.Parse(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x0002ED7F File Offset: 0x0002DD7F
		[CLSCompliant(false)]
		public static ushort Parse(string s, IFormatProvider provider)
		{
			return ushort.Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x0002ED8E File Offset: 0x0002DD8E
		[CLSCompliant(false)]
		public static ushort Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return ushort.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x0002EDA4 File Offset: 0x0002DDA4
		private static ushort Parse(string s, NumberStyles style, NumberFormatInfo info)
		{
			uint num = 0U;
			try
			{
				num = Number.ParseUInt32(s, style, info);
			}
			catch (OverflowException ex)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"), ex);
			}
			if (num > 65535U)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
			}
			return (ushort)num;
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0002EDFC File Offset: 0x0002DDFC
		[CLSCompliant(false)]
		public static bool TryParse(string s, out ushort result)
		{
			return ushort.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x0002EE0B File Offset: 0x0002DE0B
		[CLSCompliant(false)]
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ushort result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return ushort.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x0002EE24 File Offset: 0x0002DE24
		private static bool TryParse(string s, NumberStyles style, NumberFormatInfo info, out ushort result)
		{
			result = 0;
			uint num;
			if (!Number.TryParseUInt32(s, style, info, out num))
			{
				return false;
			}
			if (num > 65535U)
			{
				return false;
			}
			result = (ushort)num;
			return true;
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0002EE51 File Offset: 0x0002DE51
		public TypeCode GetTypeCode()
		{
			return TypeCode.UInt16;
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x0002EE54 File Offset: 0x0002DE54
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0002EE5D File Offset: 0x0002DE5D
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x0002EE66 File Offset: 0x0002DE66
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x0002EE6F File Offset: 0x0002DE6F
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x0002EE78 File Offset: 0x0002DE78
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x0002EE81 File Offset: 0x0002DE81
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x0002EE85 File Offset: 0x0002DE85
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x0002EE8E File Offset: 0x0002DE8E
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x0002EE97 File Offset: 0x0002DE97
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x0002EEA0 File Offset: 0x0002DEA0
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0002EEA9 File Offset: 0x0002DEA9
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0002EEB2 File Offset: 0x0002DEB2
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0002EEBB File Offset: 0x0002DEBB
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x0002EEC4 File Offset: 0x0002DEC4
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "UInt16", "DateTime" }));
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x0002EF02 File Offset: 0x0002DF02
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x04000569 RID: 1385
		public const ushort MaxValue = 65535;

		// Token: 0x0400056A RID: 1386
		public const ushort MinValue = 0;

		// Token: 0x0400056B RID: 1387
		private ushort m_value;
	}
}
