using System;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Data.OracleClient
{
	// Token: 0x0200006F RID: 111
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct OracleNumber : IComparable, INullable
	{
		// Token: 0x0600056F RID: 1391 RVA: 0x00069DC4 File Offset: 0x000691C4
		private OracleNumber(bool isNull)
		{
			this._value = (isNull ? null : new byte[22]);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00069DE4 File Offset: 0x000691E4
		private OracleNumber(byte[] bits)
		{
			this._value = bits;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00069DF8 File Offset: 0x000691F8
		public OracleNumber(decimal decValue)
		{
			this = new OracleNumber(false);
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber.FromDecimal(errorHandle, decValue, this._value);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00069E2C File Offset: 0x0006922C
		public OracleNumber(double dblValue)
		{
			this = new OracleNumber(false);
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber.FromDouble(errorHandle, dblValue, this._value);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00069E60 File Offset: 0x00069260
		public OracleNumber(int intValue)
		{
			this = new OracleNumber(false);
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber.FromInt32(errorHandle, intValue, this._value);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00069E94 File Offset: 0x00069294
		public OracleNumber(long longValue)
		{
			this = new OracleNumber(false);
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber.FromInt64(errorHandle, longValue, this._value);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00069EC8 File Offset: 0x000692C8
		public OracleNumber(OracleNumber from)
		{
			byte[] value = from._value;
			if (value != null)
			{
				this._value = (byte[])value.Clone();
				return;
			}
			this._value = null;
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00069EFC File Offset: 0x000692FC
		internal OracleNumber(string s)
		{
			this = new OracleNumber(false);
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			this.FromString(errorHandle, s, this._value);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00069F30 File Offset: 0x00069330
		internal OracleNumber(NativeBuffer buffer, int valueOffset)
		{
			this = new OracleNumber(false);
			buffer.ReadBytes(valueOffset, this._value, 0, 22);
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x00069F58 File Offset: 0x00069358
		public bool IsNull
		{
			get
			{
				return null == this._value;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x00069F70 File Offset: 0x00069370
		public decimal Value
		{
			get
			{
				return (decimal)this;
			}
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00069F88 File Offset: 0x00069388
		public int CompareTo(object obj)
		{
			if (obj.GetType() != typeof(OracleNumber))
			{
				throw ADP.WrongType(obj.GetType(), typeof(OracleNumber));
			}
			OracleNumber oracleNumber = (OracleNumber)obj;
			if (this.IsNull)
			{
				if (!oracleNumber.IsNull)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (oracleNumber.IsNull)
				{
					return 1;
				}
				OracleConnection.ExecutePermission.Demand();
				OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
				return OracleNumber.InternalCmp(errorHandle, this._value, oracleNumber._value);
			}
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0006A00C File Offset: 0x0006940C
		public override bool Equals(object value)
		{
			return value is OracleNumber && (this == (OracleNumber)value).Value;
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0006A03C File Offset: 0x0006943C
		public override int GetHashCode()
		{
			if (!this.IsNull)
			{
				return this._value.GetHashCode();
			}
			return 0;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0006A060 File Offset: 0x00069460
		internal static decimal MarshalToDecimal(NativeBuffer buffer, int valueOffset, OracleConnection connection)
		{
			byte[] array = buffer.ReadBytes(valueOffset, 22);
			OciErrorHandle errorHandle = connection.ErrorHandle;
			return OracleNumber.ToDecimal(errorHandle, array);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0006A088 File Offset: 0x00069488
		internal static int MarshalToInt32(NativeBuffer buffer, int valueOffset, OracleConnection connection)
		{
			byte[] array = buffer.ReadBytes(valueOffset, 22);
			OciErrorHandle errorHandle = connection.ErrorHandle;
			return OracleNumber.ToInt32(errorHandle, array);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0006A0B0 File Offset: 0x000694B0
		internal static long MarshalToInt64(NativeBuffer buffer, int valueOffset, OracleConnection connection)
		{
			byte[] array = buffer.ReadBytes(valueOffset, 22);
			OciErrorHandle errorHandle = connection.ErrorHandle;
			return OracleNumber.ToInt64(errorHandle, array);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0006A0D8 File Offset: 0x000694D8
		internal static int MarshalToNative(object value, NativeBuffer buffer, int offset, OracleConnection connection)
		{
			byte[] array;
			if (value is OracleNumber)
			{
				array = ((OracleNumber)value)._value;
			}
			else
			{
				OciErrorHandle errorHandle = connection.ErrorHandle;
				array = new byte[22];
				if (value is decimal)
				{
					OracleNumber.FromDecimal(errorHandle, (decimal)value, array);
				}
				else if (value is int)
				{
					OracleNumber.FromInt32(errorHandle, (int)value, array);
				}
				else if (value is long)
				{
					OracleNumber.FromInt64(errorHandle, (long)value, array);
				}
				else
				{
					OracleNumber.FromDouble(errorHandle, (double)value, array);
				}
			}
			buffer.WriteBytes(offset, array, 0, 22);
			return 22;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0006A16C File Offset: 0x0006956C
		public static OracleNumber Parse(string s)
		{
			if (s == null)
			{
				throw ADP.ArgumentNull("s");
			}
			return new OracleNumber(s);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0006A190 File Offset: 0x00069590
		private static void InternalAdd(OciErrorHandle errorHandle, byte[] x, byte[] y, byte[] result)
		{
			int num = UnsafeNativeMethods.OCINumberAdd(errorHandle, x, y, result);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0006A1B4 File Offset: 0x000695B4
		private static int InternalCmp(OciErrorHandle errorHandle, byte[] value1, byte[] value2)
		{
			int num2;
			int num = UnsafeNativeMethods.OCINumberCmp(errorHandle, value1, value2, out num2);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return num2;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0006A1D8 File Offset: 0x000695D8
		private static void InternalDiv(OciErrorHandle errorHandle, byte[] x, byte[] y, byte[] result)
		{
			int num = UnsafeNativeMethods.OCINumberDiv(errorHandle, x, y, result);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0006A1FC File Offset: 0x000695FC
		private static bool InternalIsInt(OciErrorHandle errorHandle, byte[] n)
		{
			int num2;
			int num = UnsafeNativeMethods.OCINumberIsInt(errorHandle, n, out num2);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return 0 != num2;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0006A224 File Offset: 0x00069624
		private static void InternalMod(OciErrorHandle errorHandle, byte[] x, byte[] y, byte[] result)
		{
			int num = UnsafeNativeMethods.OCINumberMod(errorHandle, x, y, result);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0006A248 File Offset: 0x00069648
		private static void InternalMul(OciErrorHandle errorHandle, byte[] x, byte[] y, byte[] result)
		{
			int num = UnsafeNativeMethods.OCINumberMul(errorHandle, x, y, result);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0006A26C File Offset: 0x0006966C
		private static void InternalNeg(OciErrorHandle errorHandle, byte[] x, byte[] result)
		{
			int num = UnsafeNativeMethods.OCINumberNeg(errorHandle, x, result);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0006A28C File Offset: 0x0006968C
		private static int InternalSign(OciErrorHandle errorHandle, byte[] n)
		{
			int num2;
			int num = UnsafeNativeMethods.OCINumberSign(errorHandle, n, out num2);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return num2;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0006A2B0 File Offset: 0x000696B0
		private static void InternalShift(OciErrorHandle errorHandle, byte[] n, int digits, byte[] result)
		{
			int num = UnsafeNativeMethods.OCINumberShift(errorHandle, n, digits, result);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0006A2D4 File Offset: 0x000696D4
		private static void InternalSub(OciErrorHandle errorHandle, byte[] x, byte[] y, byte[] result)
		{
			int num = UnsafeNativeMethods.OCINumberSub(errorHandle, x, y, result);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0006A2F8 File Offset: 0x000696F8
		private static void InternalTrunc(OciErrorHandle errorHandle, byte[] n, int position, byte[] result)
		{
			int num = UnsafeNativeMethods.OCINumberTrunc(errorHandle, n, position, result);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0006A31C File Offset: 0x0006971C
		private static void FromDecimal(OciErrorHandle errorHandle, decimal decimalValue, byte[] result)
		{
			int[] bits = decimal.GetBits(decimalValue);
			ulong num = ((ulong)bits[1] << 32) | (ulong)bits[0];
			uint num2 = (uint)bits[2];
			int num3 = bits[3] >> 31;
			int num4 = (bits[3] >> 16) & 127;
			OracleNumber.FromUInt64(errorHandle, num, result);
			if (num2 != 0U)
			{
				byte[] array = new byte[22];
				OracleNumber.FromUInt32(errorHandle, num2, array);
				OracleNumber.InternalMul(errorHandle, array, OracleNumber.OciNumberValue_TwoPow64, array);
				OracleNumber.InternalAdd(errorHandle, result, array, result);
			}
			if (num3 != 0)
			{
				OracleNumber.InternalNeg(errorHandle, result, result);
			}
			if (num4 != 0)
			{
				OracleNumber.InternalShift(errorHandle, result, -num4, result);
			}
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0006A3A0 File Offset: 0x000697A0
		private static void FromDouble(OciErrorHandle errorHandle, double dblValue, byte[] result)
		{
			if (dblValue < OracleNumber.doubleMinValue || dblValue > OracleNumber.doubleMaxValue)
			{
				throw ADP.OperationResultedInOverflow();
			}
			int num = UnsafeNativeMethods.OCINumberFromReal(errorHandle, ref dblValue, 8U, result);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0006A3D8 File Offset: 0x000697D8
		private static void FromInt32(OciErrorHandle errorHandle, int intValue, byte[] result)
		{
			int num = UnsafeNativeMethods.OCINumberFromInt(errorHandle, ref intValue, 4U, OCI.SIGN.OCI_NUMBER_SIGNED, result);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0006A3FC File Offset: 0x000697FC
		private static void FromUInt32(OciErrorHandle errorHandle, uint uintValue, byte[] result)
		{
			int num = UnsafeNativeMethods.OCINumberFromInt(errorHandle, ref uintValue, 4U, OCI.SIGN.OCI_NUMBER_UNSIGNED, result);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0006A420 File Offset: 0x00069820
		private static void FromInt64(OciErrorHandle errorHandle, long longValue, byte[] result)
		{
			int num = UnsafeNativeMethods.OCINumberFromInt(errorHandle, ref longValue, 8U, OCI.SIGN.OCI_NUMBER_SIGNED, result);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0006A444 File Offset: 0x00069844
		private static void FromUInt64(OciErrorHandle errorHandle, ulong ulongValue, byte[] result)
		{
			int num = UnsafeNativeMethods.OCINumberFromInt(errorHandle, ref ulongValue, 8U, OCI.SIGN.OCI_NUMBER_UNSIGNED, result);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0006A468 File Offset: 0x00069868
		private void FromStringOfDigits(OciErrorHandle errorHandle, string s, byte[] result)
		{
			if (s.Length <= 63)
			{
				int num = UnsafeNativeMethods.OCINumberFromText(errorHandle, s, (uint)s.Length, "999999999999999999999999999999999999999999999999999999999999999", 63U, IntPtr.Zero, 0U, result);
				if (num != 0)
				{
					OracleException.Check(errorHandle, num);
					return;
				}
			}
			else
			{
				byte[] array = new byte[22];
				string text = s.Substring(0, 63);
				string text2 = s.Substring(63);
				this.FromStringOfDigits(errorHandle, text, array);
				this.FromStringOfDigits(errorHandle, text2, result);
				OracleNumber.InternalShift(errorHandle, array, text2.Length, array);
				OracleNumber.InternalAdd(errorHandle, result, array, result);
			}
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0006A4EC File Offset: 0x000698EC
		private void FromString(OciErrorHandle errorHandle, string s, byte[] result)
		{
			byte[] array = new byte[22];
			int num = 0;
			s = s.Trim();
			int num2 = s.IndexOfAny("eE".ToCharArray());
			if (num2 > 0)
			{
				num = int.Parse(s.Substring(num2 + 1), CultureInfo.InvariantCulture);
				s = s.Substring(0, num2);
			}
			bool flag = false;
			if ('-' == s[0])
			{
				flag = true;
				s = s.Substring(1);
			}
			else if ('+' == s[0])
			{
				s = s.Substring(1);
			}
			int num3 = s.IndexOf('.');
			if (0 <= num3)
			{
				string text = s.Substring(num3 + 1);
				this.FromStringOfDigits(errorHandle, text, result);
				OracleNumber.InternalShift(errorHandle, result, -text.Length, result);
				if (num3 != 0)
				{
					this.FromStringOfDigits(errorHandle, s.Substring(0, num3), array);
					OracleNumber.InternalAdd(errorHandle, result, array, result);
				}
			}
			else
			{
				this.FromStringOfDigits(errorHandle, s, result);
			}
			if (num != 0)
			{
				OracleNumber.InternalShift(errorHandle, result, num, result);
			}
			if (flag)
			{
				OracleNumber.InternalNeg(errorHandle, result, result);
			}
			GC.KeepAlive(s);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0006A5E8 File Offset: 0x000699E8
		private static decimal ToDecimal(OciErrorHandle errorHandle, byte[] value)
		{
			byte[] array = (byte[])value.Clone();
			byte[] array2 = new byte[22];
			byte b = 0;
			int num = OracleNumber.InternalSign(errorHandle, array);
			if (num < 0)
			{
				OracleNumber.InternalNeg(errorHandle, array, array);
			}
			if (!OracleNumber.InternalIsInt(errorHandle, array))
			{
				int num2 = (int)(2 * (array[0] - ((array[1] & 127) - 64) - 1));
				OracleNumber.InternalShift(errorHandle, array, num2, array);
				b += (byte)num2;
				while (!OracleNumber.InternalIsInt(errorHandle, array))
				{
					OracleNumber.InternalShift(errorHandle, array, 1, array);
					b += 1;
				}
			}
			OracleNumber.InternalMod(errorHandle, array, OracleNumber.OciNumberValue_TwoPow64, array2);
			ulong num3 = OracleNumber.ToUInt64(errorHandle, array2);
			OracleNumber.InternalDiv(errorHandle, array, OracleNumber.OciNumberValue_TwoPow64, array2);
			OracleNumber.InternalTrunc(errorHandle, array2, 0, array2);
			uint num4 = OracleNumber.ToUInt32(errorHandle, array2);
			decimal num5 = new decimal((int)(num3 & (ulong)(-1)), (int)(num3 >> 32), (int)num4, num < 0, b);
			return num5;
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0006A6B8 File Offset: 0x00069AB8
		private static int ToInt32(OciErrorHandle errorHandle, byte[] value)
		{
			int num2;
			int num = UnsafeNativeMethods.OCINumberToInt(errorHandle, value, 4U, OCI.SIGN.OCI_NUMBER_SIGNED, out num2);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return num2;
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0006A6DC File Offset: 0x00069ADC
		private static uint ToUInt32(OciErrorHandle errorHandle, byte[] value)
		{
			uint num2;
			int num = UnsafeNativeMethods.OCINumberToInt(errorHandle, value, 4U, OCI.SIGN.OCI_NUMBER_UNSIGNED, out num2);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return num2;
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0006A700 File Offset: 0x00069B00
		private static long ToInt64(OciErrorHandle errorHandle, byte[] value)
		{
			long num2;
			int num = UnsafeNativeMethods.OCINumberToInt(errorHandle, value, 8U, OCI.SIGN.OCI_NUMBER_SIGNED, out num2);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return num2;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0006A724 File Offset: 0x00069B24
		private static ulong ToUInt64(OciErrorHandle errorHandle, byte[] value)
		{
			ulong num2;
			int num = UnsafeNativeMethods.OCINumberToInt(errorHandle, value, 8U, OCI.SIGN.OCI_NUMBER_UNSIGNED, out num2);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return num2;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0006A748 File Offset: 0x00069B48
		private static string ToString(OciErrorHandle errorHandle, byte[] value)
		{
			byte[] array = new byte[64];
			uint num = (uint)array.Length;
			int num2 = UnsafeNativeMethods.OCINumberToText(errorHandle, value, "TM9", 3, IntPtr.Zero, 0U, ref num, array);
			if (num2 != 0)
			{
				OracleException.Check(errorHandle, num2);
			}
			int num3 = Array.IndexOf<byte>(array, 58);
			num3 = ((num3 > 0) ? num3 : Array.LastIndexOf(array, 0));
			return Encoding.Default.GetString(array, 0, (num3 > 0) ? num3 : checked((int)num));
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0006A7B8 File Offset: 0x00069BB8
		public override string ToString()
		{
			if (this.IsNull)
			{
				return ADP.NullString;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			return OracleNumber.ToString(errorHandle, this._value);
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0006A7F4 File Offset: 0x00069BF4
		public static OracleBoolean operator ==(OracleNumber x, OracleNumber y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) == 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0006A830 File Offset: 0x00069C30
		public static OracleBoolean operator >(OracleNumber x, OracleNumber y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) > 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0006A86C File Offset: 0x00069C6C
		public static OracleBoolean operator >=(OracleNumber x, OracleNumber y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) >= 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0006A8AC File Offset: 0x00069CAC
		public static OracleBoolean operator <(OracleNumber x, OracleNumber y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) < 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0006A8E8 File Offset: 0x00069CE8
		public static OracleBoolean operator <=(OracleNumber x, OracleNumber y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) <= 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0006A928 File Offset: 0x00069D28
		public static OracleBoolean operator !=(OracleNumber x, OracleNumber y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) != 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0006A968 File Offset: 0x00069D68
		public static OracleNumber operator -(OracleNumber x)
		{
			if (x.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			OracleNumber.InternalNeg(errorHandle, x._value, oracleNumber._value);
			return oracleNumber;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0006A9B4 File Offset: 0x00069DB4
		public static OracleNumber operator +(OracleNumber x, OracleNumber y)
		{
			if (x.IsNull || y.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			OracleNumber.InternalAdd(errorHandle, x._value, y._value, oracleNumber._value);
			return oracleNumber;
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0006AA10 File Offset: 0x00069E10
		public static OracleNumber operator -(OracleNumber x, OracleNumber y)
		{
			if (x.IsNull || y.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			OracleNumber.InternalSub(errorHandle, x._value, y._value, oracleNumber._value);
			return oracleNumber;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0006AA6C File Offset: 0x00069E6C
		public static OracleNumber operator *(OracleNumber x, OracleNumber y)
		{
			if (x.IsNull || y.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			OracleNumber.InternalMul(errorHandle, x._value, y._value, oracleNumber._value);
			return oracleNumber;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0006AAC8 File Offset: 0x00069EC8
		public static OracleNumber operator /(OracleNumber x, OracleNumber y)
		{
			if (x.IsNull || y.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			OracleNumber.InternalDiv(errorHandle, x._value, y._value, oracleNumber._value);
			return oracleNumber;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0006AB24 File Offset: 0x00069F24
		public static OracleNumber operator %(OracleNumber x, OracleNumber y)
		{
			if (x.IsNull || y.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			OracleNumber.InternalMod(errorHandle, x._value, y._value, oracleNumber._value);
			return oracleNumber;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0006AB80 File Offset: 0x00069F80
		public static explicit operator decimal(OracleNumber x)
		{
			if (x.IsNull)
			{
				throw ADP.DataIsNull();
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			return OracleNumber.ToDecimal(errorHandle, x._value);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0006ABBC File Offset: 0x00069FBC
		public static explicit operator double(OracleNumber x)
		{
			if (x.IsNull)
			{
				throw ADP.DataIsNull();
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			double num2;
			int num = UnsafeNativeMethods.OCINumberToReal(errorHandle, x._value, 8U, out num2);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return num2;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0006AC04 File Offset: 0x0006A004
		public static explicit operator int(OracleNumber x)
		{
			if (x.IsNull)
			{
				throw ADP.DataIsNull();
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			return OracleNumber.ToInt32(errorHandle, x._value);
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0006AC40 File Offset: 0x0006A040
		public static explicit operator long(OracleNumber x)
		{
			if (x.IsNull)
			{
				throw ADP.DataIsNull();
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			return OracleNumber.ToInt64(errorHandle, x._value);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0006AC7C File Offset: 0x0006A07C
		public static explicit operator OracleNumber(decimal x)
		{
			return new OracleNumber(x);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0006AC90 File Offset: 0x0006A090
		public static explicit operator OracleNumber(double x)
		{
			return new OracleNumber(x);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0006ACA4 File Offset: 0x0006A0A4
		public static explicit operator OracleNumber(int x)
		{
			return new OracleNumber(x);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0006ACB8 File Offset: 0x0006A0B8
		public static explicit operator OracleNumber(long x)
		{
			return new OracleNumber(x);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0006ACCC File Offset: 0x0006A0CC
		public static explicit operator OracleNumber(string x)
		{
			return new OracleNumber(x);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0006ACE0 File Offset: 0x0006A0E0
		public static OracleNumber Abs(OracleNumber n)
		{
			if (n.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberAbs(errorHandle, n._value, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0006AD34 File Offset: 0x0006A134
		public static OracleNumber Acos(OracleNumber n)
		{
			if (n.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberArcCos(errorHandle, n._value, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0006AD88 File Offset: 0x0006A188
		public static OracleNumber Add(OracleNumber x, OracleNumber y)
		{
			return x + y;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0006AD9C File Offset: 0x0006A19C
		public static OracleNumber Asin(OracleNumber n)
		{
			if (n.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberArcSin(errorHandle, n._value, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0006ADF0 File Offset: 0x0006A1F0
		public static OracleNumber Atan(OracleNumber n)
		{
			if (n.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberArcTan(errorHandle, n._value, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0006AE44 File Offset: 0x0006A244
		public static OracleNumber Atan2(OracleNumber y, OracleNumber x)
		{
			if (x.IsNull || y.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberArcTan2(errorHandle, y._value, x._value, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0006AEA8 File Offset: 0x0006A2A8
		public static OracleNumber Ceiling(OracleNumber n)
		{
			if (n.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberCeil(errorHandle, n._value, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0006AEFC File Offset: 0x0006A2FC
		public static OracleNumber Cos(OracleNumber n)
		{
			if (n.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberCos(errorHandle, n._value, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0006AF50 File Offset: 0x0006A350
		public static OracleNumber Cosh(OracleNumber n)
		{
			if (n.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberHypCos(errorHandle, n._value, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0006AFA4 File Offset: 0x0006A3A4
		public static OracleNumber Divide(OracleNumber x, OracleNumber y)
		{
			return x / y;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0006AFB8 File Offset: 0x0006A3B8
		public static OracleBoolean Equals(OracleNumber x, OracleNumber y)
		{
			return x == y;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0006AFCC File Offset: 0x0006A3CC
		public static OracleNumber Exp(OracleNumber p)
		{
			if (p.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberExp(errorHandle, p._value, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0006B020 File Offset: 0x0006A420
		public static OracleNumber Floor(OracleNumber n)
		{
			if (n.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberFloor(errorHandle, n._value, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0006B074 File Offset: 0x0006A474
		public static OracleBoolean GreaterThan(OracleNumber x, OracleNumber y)
		{
			return x > y;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0006B088 File Offset: 0x0006A488
		public static OracleBoolean GreaterThanOrEqual(OracleNumber x, OracleNumber y)
		{
			return x >= y;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0006B09C File Offset: 0x0006A49C
		public static OracleBoolean LessThan(OracleNumber x, OracleNumber y)
		{
			return x < y;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0006B0B0 File Offset: 0x0006A4B0
		public static OracleBoolean LessThanOrEqual(OracleNumber x, OracleNumber y)
		{
			return x <= y;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0006B0C4 File Offset: 0x0006A4C4
		public static OracleNumber Log(OracleNumber n)
		{
			if (n.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberLn(errorHandle, n._value, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0006B118 File Offset: 0x0006A518
		public static OracleNumber Log(OracleNumber n, int newBase)
		{
			return OracleNumber.Log(n, new OracleNumber(newBase));
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0006B134 File Offset: 0x0006A534
		public static OracleNumber Log(OracleNumber n, OracleNumber newBase)
		{
			if (n.IsNull || newBase.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberLog(errorHandle, newBase._value, n._value, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0006B198 File Offset: 0x0006A598
		public static OracleNumber Log10(OracleNumber n)
		{
			return OracleNumber.Log(n, new OracleNumber(10));
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0006B1B4 File Offset: 0x0006A5B4
		public static OracleNumber Max(OracleNumber x, OracleNumber y)
		{
			if (x.IsNull || y.IsNull)
			{
				return OracleNumber.Null;
			}
			if (!OracleBoolean.op_True(x > y))
			{
				return y;
			}
			return x;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0006B1EC File Offset: 0x0006A5EC
		public static OracleNumber Min(OracleNumber x, OracleNumber y)
		{
			if (x.IsNull || y.IsNull)
			{
				return OracleNumber.Null;
			}
			if (!OracleBoolean.op_True(x < y))
			{
				return y;
			}
			return x;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0006B224 File Offset: 0x0006A624
		public static OracleNumber Modulo(OracleNumber x, OracleNumber y)
		{
			return x % y;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0006B238 File Offset: 0x0006A638
		public static OracleNumber Multiply(OracleNumber x, OracleNumber y)
		{
			return x * y;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0006B24C File Offset: 0x0006A64C
		public static OracleNumber Negate(OracleNumber x)
		{
			return -x;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0006B260 File Offset: 0x0006A660
		public static OracleBoolean NotEquals(OracleNumber x, OracleNumber y)
		{
			return x != y;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0006B274 File Offset: 0x0006A674
		public static OracleNumber Pow(OracleNumber x, int y)
		{
			if (x.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberIntPower(errorHandle, x._value, y, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0006B2CC File Offset: 0x0006A6CC
		public static OracleNumber Pow(OracleNumber x, OracleNumber y)
		{
			if (x.IsNull || y.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberPower(errorHandle, x._value, y._value, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0006B330 File Offset: 0x0006A730
		public static OracleNumber Round(OracleNumber n, int position)
		{
			if (n.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberRound(errorHandle, n._value, position, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0006B388 File Offset: 0x0006A788
		public static OracleNumber Shift(OracleNumber n, int digits)
		{
			if (n.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			OracleNumber.InternalShift(errorHandle, n._value, digits, oracleNumber._value);
			return oracleNumber;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0006B3D4 File Offset: 0x0006A7D4
		public static OracleNumber Sign(OracleNumber n)
		{
			if (n.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			int num = OracleNumber.InternalSign(errorHandle, n._value);
			return (num > 0) ? OracleNumber.One : OracleNumber.MinusOne;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0006B420 File Offset: 0x0006A820
		public static OracleNumber Sin(OracleNumber n)
		{
			if (n.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberSin(errorHandle, n._value, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0006B474 File Offset: 0x0006A874
		public static OracleNumber Sinh(OracleNumber n)
		{
			if (n.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberHypSin(errorHandle, n._value, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0006B4C8 File Offset: 0x0006A8C8
		public static OracleNumber Sqrt(OracleNumber n)
		{
			if (n.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberSqrt(errorHandle, n._value, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0006B51C File Offset: 0x0006A91C
		public static OracleNumber Subtract(OracleNumber x, OracleNumber y)
		{
			return x - y;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0006B530 File Offset: 0x0006A930
		public static OracleNumber Tan(OracleNumber n)
		{
			if (n.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberTan(errorHandle, n._value, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0006B584 File Offset: 0x0006A984
		public static OracleNumber Tanh(OracleNumber n)
		{
			if (n.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			int num = UnsafeNativeMethods.OCINumberHypTan(errorHandle, n._value, oracleNumber._value);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return oracleNumber;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0006B5D8 File Offset: 0x0006A9D8
		public static OracleNumber Truncate(OracleNumber n, int position)
		{
			if (n.IsNull)
			{
				return OracleNumber.Null;
			}
			OracleConnection.ExecutePermission.Demand();
			OciErrorHandle errorHandle = TempEnvironment.GetErrorHandle();
			OracleNumber oracleNumber = new OracleNumber(false);
			OracleNumber.InternalTrunc(errorHandle, n._value, position, oracleNumber._value);
			return oracleNumber;
		}

		// Token: 0x0400046A RID: 1130
		private const string WholeDigitPattern = "999999999999999999999999999999999999999999999999999999999999999";

		// Token: 0x0400046B RID: 1131
		private const int WholeDigitPattern_Length = 63;

		// Token: 0x0400046C RID: 1132
		private static double doubleMinValue = -9.99999999999999E+125;

		// Token: 0x0400046D RID: 1133
		private static double doubleMaxValue = 9.99999999999999E+125;

		// Token: 0x0400046E RID: 1134
		private static readonly byte[] OciNumberValue_DecimalMaxValue = new byte[]
		{
			16, 207, 8, 93, 29, 17, 26, 15, 27, 44,
			38, 59, 93, 49, 99, 31, 40
		};

		// Token: 0x0400046F RID: 1135
		private static readonly byte[] OciNumberValue_DecimalMinValue = new byte[]
		{
			17, 48, 94, 9, 73, 85, 76, 87, 75, 58,
			64, 43, 9, 53, 3, 71, 62, 102
		};

		// Token: 0x04000470 RID: 1136
		private static readonly byte[] OciNumberValue_E = new byte[]
		{
			21, 193, 3, 72, 83, 82, 83, 85, 60, 5,
			53, 36, 37, 3, 88, 48, 14, 53, 67, 25,
			98, 77
		};

		// Token: 0x04000471 RID: 1137
		private static readonly byte[] OciNumberValue_MaxValue = new byte[]
		{
			20, byte.MaxValue, 100, 100, 100, 100, 100, 100, 100, 100,
			100, 100, 100, 100, 100, 100, 100, 100, 100, 100,
			100
		};

		// Token: 0x04000472 RID: 1138
		private static readonly byte[] OciNumberValue_MinValue = new byte[]
		{
			21, 0, 2, 2, 2, 2, 2, 2, 2, 2,
			2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
			2, 102
		};

		// Token: 0x04000473 RID: 1139
		private static readonly byte[] OciNumberValue_MinusOne = new byte[] { 3, 62, 100, 102 };

		// Token: 0x04000474 RID: 1140
		private static readonly byte[] OciNumberValue_One = new byte[] { 2, 193, 2 };

		// Token: 0x04000475 RID: 1141
		private static readonly byte[] OciNumberValue_Pi = new byte[]
		{
			21, 193, 4, 15, 16, 93, 66, 36, 90, 80,
			33, 39, 47, 27, 44, 39, 33, 80, 51, 29,
			85, 21
		};

		// Token: 0x04000476 RID: 1142
		private static readonly byte[] OciNumberValue_TwoPow64 = new byte[]
		{
			11, 202, 19, 45, 68, 45, 8, 38, 10, 56,
			17, 17
		};

		// Token: 0x04000477 RID: 1143
		private static readonly byte[] OciNumberValue_Zero = new byte[] { 1, 128 };

		// Token: 0x04000478 RID: 1144
		private byte[] _value;

		// Token: 0x04000479 RID: 1145
		public static readonly OracleNumber E = new OracleNumber(OracleNumber.OciNumberValue_E);

		// Token: 0x0400047A RID: 1146
		public static readonly int MaxPrecision = 38;

		// Token: 0x0400047B RID: 1147
		public static readonly int MaxScale = 127;

		// Token: 0x0400047C RID: 1148
		public static readonly int MinScale = -84;

		// Token: 0x0400047D RID: 1149
		public static readonly OracleNumber MaxValue = new OracleNumber(OracleNumber.OciNumberValue_MaxValue);

		// Token: 0x0400047E RID: 1150
		public static readonly OracleNumber MinValue = new OracleNumber(OracleNumber.OciNumberValue_MinValue);

		// Token: 0x0400047F RID: 1151
		public static readonly OracleNumber MinusOne = new OracleNumber(OracleNumber.OciNumberValue_MinusOne);

		// Token: 0x04000480 RID: 1152
		public static readonly OracleNumber Null = new OracleNumber(true);

		// Token: 0x04000481 RID: 1153
		public static readonly OracleNumber One = new OracleNumber(OracleNumber.OciNumberValue_One);

		// Token: 0x04000482 RID: 1154
		public static readonly OracleNumber PI = new OracleNumber(OracleNumber.OciNumberValue_Pi);

		// Token: 0x04000483 RID: 1155
		public static readonly OracleNumber Zero = new OracleNumber(OracleNumber.OciNumberValue_Zero);
	}
}
