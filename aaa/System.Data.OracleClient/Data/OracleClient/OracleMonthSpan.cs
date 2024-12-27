using System;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Data.OracleClient
{
	// Token: 0x0200006E RID: 110
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct OracleMonthSpan : IComparable, INullable
	{
		// Token: 0x06000551 RID: 1361 RVA: 0x00069860 File Offset: 0x00068C60
		internal OracleMonthSpan(bool isNull)
		{
			this._value = int.MaxValue;
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00069878 File Offset: 0x00068C78
		public OracleMonthSpan(int months)
		{
			this._value = months;
			OracleMonthSpan.AssertValid(this._value);
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00069898 File Offset: 0x00068C98
		public OracleMonthSpan(int years, int months)
		{
			try
			{
				this._value = checked(years * 12 + months);
			}
			catch (OverflowException)
			{
				throw ADP.MonthOutOfRange();
			}
			OracleMonthSpan.AssertValid(this._value);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x000698E4 File Offset: 0x00068CE4
		public OracleMonthSpan(OracleMonthSpan from)
		{
			this._value = from._value;
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00069900 File Offset: 0x00068D00
		internal OracleMonthSpan(NativeBuffer buffer, int valueOffset)
		{
			this._value = OracleMonthSpan.MarshalToInt32(buffer, valueOffset);
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x0006991C File Offset: 0x00068D1C
		public bool IsNull
		{
			get
			{
				return int.MaxValue == this._value;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x00069938 File Offset: 0x00068D38
		public int Value
		{
			get
			{
				if (this.IsNull)
				{
					throw ADP.DataIsNull();
				}
				return this._value;
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0006995C File Offset: 0x00068D5C
		private static void AssertValid(int monthSpan)
		{
			if (monthSpan < -176556 || monthSpan > 176556)
			{
				throw ADP.MonthOutOfRange();
			}
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00069980 File Offset: 0x00068D80
		public int CompareTo(object obj)
		{
			if (obj.GetType() != typeof(OracleMonthSpan))
			{
				throw ADP.WrongType(obj.GetType(), typeof(OracleMonthSpan));
			}
			OracleMonthSpan oracleMonthSpan = (OracleMonthSpan)obj;
			if (this.IsNull)
			{
				if (!oracleMonthSpan.IsNull)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (oracleMonthSpan.IsNull)
				{
					return 1;
				}
				return this._value.CompareTo(oracleMonthSpan._value);
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x000699F0 File Offset: 0x00068DF0
		public override bool Equals(object value)
		{
			return value is OracleMonthSpan && (this == (OracleMonthSpan)value).Value;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00069A20 File Offset: 0x00068E20
		public override int GetHashCode()
		{
			if (!this.IsNull)
			{
				return this._value.GetHashCode();
			}
			return 0;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00069A44 File Offset: 0x00068E44
		internal static int MarshalToInt32(NativeBuffer buffer, int valueOffset)
		{
			byte[] array = buffer.ReadBytes(valueOffset, 5);
			int num = (int)((long)(((int)array[0] << 24) | ((int)array[1] << 16) | ((int)array[2] << 8) | (int)array[3]) - (long)((ulong)int.MinValue));
			int num2 = (int)(array[4] - 60);
			int num3 = num * 12 + num2;
			OracleMonthSpan.AssertValid(num3);
			return num3;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00069A90 File Offset: 0x00068E90
		internal static int MarshalToNative(object value, NativeBuffer buffer, int offset)
		{
			int num;
			if (value is OracleMonthSpan)
			{
				num = ((OracleMonthSpan)value)._value;
			}
			else
			{
				num = (int)value;
			}
			byte[] array = new byte[5];
			int num2 = (int)((long)(num / 12) + (long)((ulong)int.MinValue));
			int num3 = num % 12;
			array[0] = (byte)(num2 >> 24);
			array[1] = (byte)((num2 >> 16) & 255);
			array[2] = (byte)((num2 >> 8) & 255);
			array[3] = (byte)(num2 & 255);
			array[4] = (byte)(num3 + 60);
			buffer.WriteBytes(offset, array, 0, 5);
			return 5;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00069B18 File Offset: 0x00068F18
		public static OracleMonthSpan Parse(string s)
		{
			int num = int.Parse(s, CultureInfo.InvariantCulture);
			return new OracleMonthSpan(num);
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00069B38 File Offset: 0x00068F38
		public override string ToString()
		{
			if (this.IsNull)
			{
				return ADP.NullString;
			}
			return this.Value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00069B68 File Offset: 0x00068F68
		public static OracleBoolean Equals(OracleMonthSpan x, OracleMonthSpan y)
		{
			return x == y;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00069B7C File Offset: 0x00068F7C
		public static OracleBoolean GreaterThan(OracleMonthSpan x, OracleMonthSpan y)
		{
			return x > y;
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00069B90 File Offset: 0x00068F90
		public static OracleBoolean GreaterThanOrEqual(OracleMonthSpan x, OracleMonthSpan y)
		{
			return x >= y;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00069BA4 File Offset: 0x00068FA4
		public static OracleBoolean LessThan(OracleMonthSpan x, OracleMonthSpan y)
		{
			return x < y;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00069BB8 File Offset: 0x00068FB8
		public static OracleBoolean LessThanOrEqual(OracleMonthSpan x, OracleMonthSpan y)
		{
			return x <= y;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00069BCC File Offset: 0x00068FCC
		public static OracleBoolean NotEquals(OracleMonthSpan x, OracleMonthSpan y)
		{
			return x != y;
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00069BE0 File Offset: 0x00068FE0
		public static explicit operator int(OracleMonthSpan x)
		{
			if (x.IsNull)
			{
				throw ADP.DataIsNull();
			}
			return x.Value;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00069C04 File Offset: 0x00069004
		public static explicit operator OracleMonthSpan(string x)
		{
			return OracleMonthSpan.Parse(x);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00069C18 File Offset: 0x00069018
		public static OracleBoolean operator ==(OracleMonthSpan x, OracleMonthSpan y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) == 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00069C54 File Offset: 0x00069054
		public static OracleBoolean operator >(OracleMonthSpan x, OracleMonthSpan y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) > 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00069C90 File Offset: 0x00069090
		public static OracleBoolean operator >=(OracleMonthSpan x, OracleMonthSpan y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) >= 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00069CD0 File Offset: 0x000690D0
		public static OracleBoolean operator <(OracleMonthSpan x, OracleMonthSpan y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) < 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00069D0C File Offset: 0x0006910C
		public static OracleBoolean operator <=(OracleMonthSpan x, OracleMonthSpan y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) <= 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00069D4C File Offset: 0x0006914C
		public static OracleBoolean operator !=(OracleMonthSpan x, OracleMonthSpan y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) != 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x04000463 RID: 1123
		private const int MaxMonth = 176556;

		// Token: 0x04000464 RID: 1124
		private const int MinMonth = -176556;

		// Token: 0x04000465 RID: 1125
		private const int NullValue = 2147483647;

		// Token: 0x04000466 RID: 1126
		private int _value;

		// Token: 0x04000467 RID: 1127
		public static readonly OracleMonthSpan MaxValue = new OracleMonthSpan(176556);

		// Token: 0x04000468 RID: 1128
		public static readonly OracleMonthSpan MinValue = new OracleMonthSpan(-176556);

		// Token: 0x04000469 RID: 1129
		public static readonly OracleMonthSpan Null = new OracleMonthSpan(true);
	}
}
