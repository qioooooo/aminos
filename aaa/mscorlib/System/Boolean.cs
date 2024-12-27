using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000078 RID: 120
	[ComVisible(true)]
	[Serializable]
	public struct Boolean : IComparable, IConvertible, IComparable<bool>, IEquatable<bool>
	{
		// Token: 0x060006A8 RID: 1704 RVA: 0x00016599 File Offset: 0x00015599
		public override int GetHashCode()
		{
			if (!this)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x000165A2 File Offset: 0x000155A2
		public override string ToString()
		{
			if (!this)
			{
				return "False";
			}
			return "True";
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x000165B3 File Offset: 0x000155B3
		public string ToString(IFormatProvider provider)
		{
			if (!this)
			{
				return "False";
			}
			return "True";
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x000165C4 File Offset: 0x000155C4
		public override bool Equals(object obj)
		{
			return obj is bool && this == (bool)obj;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x000165DA File Offset: 0x000155DA
		public bool Equals(bool obj)
		{
			return this == obj;
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x000165E1 File Offset: 0x000155E1
		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is bool))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeBoolean"));
			}
			if (this == (bool)obj)
			{
				return 0;
			}
			if (!this)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00016613 File Offset: 0x00015613
		public int CompareTo(bool value)
		{
			if (this == value)
			{
				return 0;
			}
			if (!this)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00016624 File Offset: 0x00015624
		public static bool Parse(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			bool flag = false;
			if (!bool.TryParse(value, out flag))
			{
				throw new FormatException(Environment.GetResourceString("Format_BadBoolean"));
			}
			return flag;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0001665C File Offset: 0x0001565C
		public static bool TryParse(string value, out bool result)
		{
			result = false;
			if (value == null)
			{
				return false;
			}
			if ("True".Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				result = true;
				return true;
			}
			if ("False".Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				result = false;
				return true;
			}
			if (bool.m_trimmableChars == null)
			{
				char[] array = new char[string.WhitespaceChars.Length + 1];
				Array.Copy(string.WhitespaceChars, array, string.WhitespaceChars.Length);
				array[array.Length - 1] = '\0';
				bool.m_trimmableChars = array;
			}
			value = value.Trim(bool.m_trimmableChars);
			if ("True".Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				result = true;
				return true;
			}
			if ("False".Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				result = false;
				return true;
			}
			return false;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00016701 File Offset: 0x00015701
		public TypeCode GetTypeCode()
		{
			return TypeCode.Boolean;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00016704 File Offset: 0x00015704
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00016708 File Offset: 0x00015708
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "Boolean", "Char" }));
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00016746 File Offset: 0x00015746
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001674F File Offset: 0x0001574F
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00016758 File Offset: 0x00015758
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00016761 File Offset: 0x00015761
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001676A File Offset: 0x0001576A
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00016773 File Offset: 0x00015773
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0001677C File Offset: 0x0001577C
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00016785 File Offset: 0x00015785
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0001678E File Offset: 0x0001578E
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00016797 File Offset: 0x00015797
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x000167A0 File Offset: 0x000157A0
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x000167AC File Offset: 0x000157AC
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "Boolean", "DateTime" }));
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x000167EA File Offset: 0x000157EA
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x04000216 RID: 534
		internal const int True = 1;

		// Token: 0x04000217 RID: 535
		internal const int False = 0;

		// Token: 0x04000218 RID: 536
		internal const string TrueLiteral = "True";

		// Token: 0x04000219 RID: 537
		internal const string FalseLiteral = "False";

		// Token: 0x0400021A RID: 538
		private bool m_value;

		// Token: 0x0400021B RID: 539
		private static char[] m_trimmableChars;

		// Token: 0x0400021C RID: 540
		public static readonly string TrueString = "True";

		// Token: 0x0400021D RID: 541
		public static readonly string FalseString = "False";
	}
}
