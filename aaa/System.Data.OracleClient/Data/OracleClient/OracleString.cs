using System;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Data.OracleClient
{
	// Token: 0x0200007C RID: 124
	public struct OracleString : IComparable, INullable
	{
		// Token: 0x06000697 RID: 1687 RVA: 0x0006F0B0 File Offset: 0x0006E4B0
		private OracleString(bool isNull)
		{
			this._value = (isNull ? null : string.Empty);
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0006F0D0 File Offset: 0x0006E4D0
		public OracleString(string s)
		{
			this._value = s;
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0006F0E4 File Offset: 0x0006E4E4
		internal OracleString(NativeBuffer buffer, int valueOffset, int lengthOffset, MetaType metaType, OracleConnection connection, bool boundAsUCS2, bool outputParameterBinding)
		{
			this._value = OracleString.MarshalToString(buffer, valueOffset, lengthOffset, metaType, connection, boundAsUCS2, outputParameterBinding);
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x0006F108 File Offset: 0x0006E508
		public bool IsNull
		{
			get
			{
				return null == this._value;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x0006F120 File Offset: 0x0006E520
		public int Length
		{
			get
			{
				if (this.IsNull)
				{
					throw ADP.DataIsNull();
				}
				return this._value.Length;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x0006F148 File Offset: 0x0006E548
		public string Value
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

		// Token: 0x17000141 RID: 321
		public char this[int index]
		{
			get
			{
				if (this.IsNull)
				{
					throw ADP.DataIsNull();
				}
				return this._value[index];
			}
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0006F194 File Offset: 0x0006E594
		public int CompareTo(object obj)
		{
			if (obj.GetType() != typeof(OracleString))
			{
				throw ADP.WrongType(obj.GetType(), typeof(OracleString));
			}
			OracleString oracleString = (OracleString)obj;
			if (this.IsNull)
			{
				if (!oracleString.IsNull)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (oracleString.IsNull)
				{
					return 1;
				}
				return CultureInfo.CurrentCulture.CompareInfo.Compare(this._value, oracleString._value);
			}
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0006F20C File Offset: 0x0006E60C
		public override bool Equals(object value)
		{
			return value is OracleString && (this == (OracleString)value).Value;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0006F23C File Offset: 0x0006E63C
		internal static int GetChars(NativeBuffer buffer, int valueOffset, int lengthOffset, MetaType metaType, OracleConnection connection, bool boundAsUCS2, int sourceOffset, char[] destinationBuffer, int destinationOffset, int charCount)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				buffer.DangerousAddRef(ref flag);
				if (boundAsUCS2)
				{
					if (!metaType.IsLong)
					{
						Marshal.Copy(buffer.DangerousGetDataPtrWithBaseOffset(valueOffset + ADP.CharSize * sourceOffset), destinationBuffer, destinationOffset, charCount);
					}
					else
					{
						NativeBuffer_LongColumnData.CopyOutOfLineChars(buffer.ReadIntPtr(valueOffset), sourceOffset, destinationBuffer, destinationOffset, charCount);
					}
				}
				else
				{
					string text = OracleString.MarshalToString(buffer, valueOffset, lengthOffset, metaType, connection, boundAsUCS2, false);
					int length = text.Length;
					int num = ((sourceOffset + charCount > length) ? (length - sourceOffset) : charCount);
					char[] array = text.ToCharArray(sourceOffset, num);
					Buffer.BlockCopy(array, 0, destinationBuffer, destinationOffset * ADP.CharSize, num * ADP.CharSize);
					charCount = num;
				}
			}
			finally
			{
				if (flag)
				{
					buffer.DangerousRelease();
				}
			}
			return charCount;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0006F310 File Offset: 0x0006E710
		public override int GetHashCode()
		{
			if (!this.IsNull)
			{
				return this._value.GetHashCode();
			}
			return 0;
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0006F334 File Offset: 0x0006E734
		internal static int GetLength(NativeBuffer buffer, int lengthOffset, MetaType metaType)
		{
			int num;
			if (metaType.IsLong)
			{
				num = buffer.ReadInt32(lengthOffset);
			}
			else
			{
				num = (int)buffer.ReadInt16(lengthOffset);
			}
			GC.KeepAlive(buffer);
			return num;
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0006F364 File Offset: 0x0006E764
		internal static string MarshalToString(NativeBuffer buffer, int valueOffset, int lengthOffset, MetaType metaType, OracleConnection connection, bool boundAsUCS2, bool outputParameterBinding)
		{
			int num = OracleString.GetLength(buffer, lengthOffset, metaType);
			if (boundAsUCS2 && outputParameterBinding)
			{
				num /= 2;
			}
			bool flag = metaType.IsLong && !outputParameterBinding;
			IntPtr zero = IntPtr.Zero;
			string text;
			if (boundAsUCS2)
			{
				if (flag)
				{
					byte[] array = new byte[num * ADP.CharSize];
					NativeBuffer_LongColumnData.CopyOutOfLineBytes(buffer.ReadIntPtr(valueOffset), 0, array, 0, num * ADP.CharSize);
					text = Encoding.Unicode.GetString(array);
				}
				else
				{
					text = buffer.PtrToStringUni(valueOffset, num);
				}
			}
			else
			{
				byte[] array2;
				if (flag)
				{
					array2 = new byte[num];
					NativeBuffer_LongColumnData.CopyOutOfLineBytes(buffer.ReadIntPtr(valueOffset), 0, array2, 0, num);
				}
				else
				{
					array2 = buffer.ReadBytes(valueOffset, num);
				}
				text = connection.GetString(array2, metaType.UsesNationalCharacterSet);
			}
			GC.KeepAlive(buffer);
			return text;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0006F420 File Offset: 0x0006E820
		internal static int MarshalToNative(object value, int offset, int size, NativeBuffer buffer, int bufferOffset, OCI.DATATYPE ociType, bool bindAsUCS2)
		{
			Encoding encoding = (bindAsUCS2 ? Encoding.Unicode : Encoding.UTF8);
			string text;
			if (value is OracleString)
			{
				text = ((OracleString)value)._value;
			}
			else
			{
				text = (string)value;
			}
			string text2;
			if (offset == 0 && size == 0)
			{
				text2 = text;
			}
			else if (size == 0 || offset + size > text.Length)
			{
				text2 = text.Substring(offset);
			}
			else
			{
				text2 = text.Substring(offset, size);
			}
			byte[] bytes = encoding.GetBytes(text2);
			int num = bytes.Length;
			int num2 = num;
			if (num != 0)
			{
				int num3 = num;
				if (bindAsUCS2)
				{
					num3 /= 2;
				}
				if (OCI.DATATYPE.LONGVARCHAR == ociType)
				{
					buffer.WriteInt32(bufferOffset, num3);
					checked
					{
						bufferOffset += 4;
					}
					num2 += 4;
				}
				buffer.WriteBytes(bufferOffset, bytes, 0, num);
			}
			return num2;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0006F4D0 File Offset: 0x0006E8D0
		public override string ToString()
		{
			if (this.IsNull)
			{
				return ADP.NullString;
			}
			return this._value;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0006F4F4 File Offset: 0x0006E8F4
		public static OracleString Concat(OracleString x, OracleString y)
		{
			return x + y;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0006F508 File Offset: 0x0006E908
		public static OracleBoolean Equals(OracleString x, OracleString y)
		{
			return x == y;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0006F51C File Offset: 0x0006E91C
		public static OracleBoolean GreaterThan(OracleString x, OracleString y)
		{
			return x > y;
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0006F530 File Offset: 0x0006E930
		public static OracleBoolean GreaterThanOrEqual(OracleString x, OracleString y)
		{
			return x >= y;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0006F544 File Offset: 0x0006E944
		public static OracleBoolean LessThan(OracleString x, OracleString y)
		{
			return x < y;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0006F558 File Offset: 0x0006E958
		public static OracleBoolean LessThanOrEqual(OracleString x, OracleString y)
		{
			return x <= y;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0006F56C File Offset: 0x0006E96C
		public static OracleBoolean NotEquals(OracleString x, OracleString y)
		{
			return x != y;
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0006F580 File Offset: 0x0006E980
		public static implicit operator OracleString(string s)
		{
			return new OracleString(s);
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0006F594 File Offset: 0x0006E994
		public static explicit operator string(OracleString x)
		{
			return x.Value;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0006F5A8 File Offset: 0x0006E9A8
		public static OracleString operator +(OracleString x, OracleString y)
		{
			if (x.IsNull || y.IsNull)
			{
				return OracleString.Null;
			}
			OracleString oracleString = new OracleString(x._value + y._value);
			return oracleString;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0006F5E8 File Offset: 0x0006E9E8
		public static OracleBoolean operator ==(OracleString x, OracleString y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) == 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0006F624 File Offset: 0x0006EA24
		public static OracleBoolean operator >(OracleString x, OracleString y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) > 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0006F660 File Offset: 0x0006EA60
		public static OracleBoolean operator >=(OracleString x, OracleString y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) >= 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0006F6A0 File Offset: 0x0006EAA0
		public static OracleBoolean operator <(OracleString x, OracleString y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) < 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0006F6DC File Offset: 0x0006EADC
		public static OracleBoolean operator <=(OracleString x, OracleString y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) <= 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0006F71C File Offset: 0x0006EB1C
		public static OracleBoolean operator !=(OracleString x, OracleString y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) != 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x040004C3 RID: 1219
		private string _value;

		// Token: 0x040004C4 RID: 1220
		public static readonly OracleString Empty = new OracleString(false);

		// Token: 0x040004C5 RID: 1221
		public static readonly OracleString Null = new OracleString(true);
	}
}
