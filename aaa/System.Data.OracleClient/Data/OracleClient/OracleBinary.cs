using System;
using System.Data.Common;
using System.Data.SqlTypes;

namespace System.Data.OracleClient
{
	// Token: 0x0200004B RID: 75
	public struct OracleBinary : IComparable, INullable
	{
		// Token: 0x06000244 RID: 580 RVA: 0x0005BF68 File Offset: 0x0005B368
		private OracleBinary(bool isNull)
		{
			this._value = (isNull ? null : new byte[0]);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0005BF88 File Offset: 0x0005B388
		public OracleBinary(byte[] b)
		{
			this._value = ((b == null) ? b : ((byte[])b.Clone()));
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0005BFAC File Offset: 0x0005B3AC
		internal OracleBinary(NativeBuffer buffer, int valueOffset, int lengthOffset, MetaType metaType)
		{
			int length = OracleBinary.GetLength(buffer, lengthOffset, metaType);
			this._value = new byte[length];
			OracleBinary.GetBytes(buffer, valueOffset, metaType, 0, this._value, 0, length);
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0005BFE4 File Offset: 0x0005B3E4
		public bool IsNull
		{
			get
			{
				return null == this._value;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0005BFFC File Offset: 0x0005B3FC
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

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0005C020 File Offset: 0x0005B420
		public byte[] Value
		{
			get
			{
				if (this.IsNull)
				{
					throw ADP.DataIsNull();
				}
				return (byte[])this._value.Clone();
			}
		}

		// Token: 0x1700005A RID: 90
		public byte this[int index]
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

		// Token: 0x0600024B RID: 587 RVA: 0x0005C070 File Offset: 0x0005B470
		public int CompareTo(object obj)
		{
			if (obj.GetType() != typeof(OracleBinary))
			{
				throw ADP.WrongType(obj.GetType(), typeof(OracleBinary));
			}
			OracleBinary oracleBinary = (OracleBinary)obj;
			if (this.IsNull)
			{
				if (!oracleBinary.IsNull)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (oracleBinary.IsNull)
				{
					return 1;
				}
				return OracleBinary.PerformCompareByte(this._value, oracleBinary._value);
			}
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0005C0E0 File Offset: 0x0005B4E0
		public override bool Equals(object value)
		{
			return value is OracleBinary && (this == (OracleBinary)value).Value;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0005C110 File Offset: 0x0005B510
		internal static int GetBytes(NativeBuffer buffer, int valueOffset, MetaType metaType, int sourceOffset, byte[] destinationBuffer, int destinationOffset, int byteCount)
		{
			if (!metaType.IsLong)
			{
				buffer.ReadBytes(valueOffset + sourceOffset, destinationBuffer, destinationOffset, byteCount);
			}
			else
			{
				NativeBuffer_LongColumnData.CopyOutOfLineBytes(buffer.ReadIntPtr(valueOffset), sourceOffset, destinationBuffer, destinationOffset, byteCount);
			}
			return byteCount;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0005C14C File Offset: 0x0005B54C
		internal static int GetLength(NativeBuffer buffer, int lengthOffset, MetaType metaType)
		{
			if (metaType.IsLong)
			{
				return buffer.ReadInt32(lengthOffset);
			}
			return (int)buffer.ReadInt16(lengthOffset);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0005C170 File Offset: 0x0005B570
		public override int GetHashCode()
		{
			if (!this.IsNull)
			{
				return this._value.GetHashCode();
			}
			return 0;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0005C194 File Offset: 0x0005B594
		private static int PerformCompareByte(byte[] x, byte[] y)
		{
			int num = x.Length;
			int num2 = y.Length;
			bool flag = num < num2;
			int num3 = (flag ? num : num2);
			int i = 0;
			while (i < num3)
			{
				if (x[i] != y[i])
				{
					if (x[i] < y[i])
					{
						return -1;
					}
					return 1;
				}
				else
				{
					i++;
				}
			}
			if (num == num2)
			{
				return 0;
			}
			byte b = 0;
			if (flag)
			{
				for (i = num3; i < num2; i++)
				{
					if (y[i] != b)
					{
						return -1;
					}
				}
			}
			else
			{
				for (i = num3; i < num; i++)
				{
					if (x[i] != b)
					{
						return 1;
					}
				}
			}
			return 0;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0005C214 File Offset: 0x0005B614
		public static OracleBinary Concat(OracleBinary x, OracleBinary y)
		{
			return x + y;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0005C228 File Offset: 0x0005B628
		public static OracleBoolean Equals(OracleBinary x, OracleBinary y)
		{
			return x == y;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0005C23C File Offset: 0x0005B63C
		public static OracleBoolean GreaterThan(OracleBinary x, OracleBinary y)
		{
			return x > y;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0005C250 File Offset: 0x0005B650
		public static OracleBoolean GreaterThanOrEqual(OracleBinary x, OracleBinary y)
		{
			return x >= y;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0005C264 File Offset: 0x0005B664
		public static OracleBoolean LessThan(OracleBinary x, OracleBinary y)
		{
			return x < y;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0005C278 File Offset: 0x0005B678
		public static OracleBoolean LessThanOrEqual(OracleBinary x, OracleBinary y)
		{
			return x <= y;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0005C28C File Offset: 0x0005B68C
		public static OracleBoolean NotEquals(OracleBinary x, OracleBinary y)
		{
			return x != y;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0005C2A0 File Offset: 0x0005B6A0
		public static implicit operator OracleBinary(byte[] b)
		{
			return new OracleBinary(b);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0005C2B4 File Offset: 0x0005B6B4
		public static explicit operator byte[](OracleBinary x)
		{
			return x.Value;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0005C2C8 File Offset: 0x0005B6C8
		public static OracleBinary operator +(OracleBinary x, OracleBinary y)
		{
			if (x.IsNull || y.IsNull)
			{
				return OracleBinary.Null;
			}
			byte[] array = new byte[x._value.Length + y._value.Length];
			x._value.CopyTo(array, 0);
			y._value.CopyTo(array, x.Value.Length);
			OracleBinary oracleBinary = new OracleBinary(array);
			return oracleBinary;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0005C334 File Offset: 0x0005B734
		public static OracleBoolean operator ==(OracleBinary x, OracleBinary y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) == 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0005C370 File Offset: 0x0005B770
		public static OracleBoolean operator >(OracleBinary x, OracleBinary y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) > 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0005C3AC File Offset: 0x0005B7AC
		public static OracleBoolean operator >=(OracleBinary x, OracleBinary y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) >= 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0005C3EC File Offset: 0x0005B7EC
		public static OracleBoolean operator <(OracleBinary x, OracleBinary y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) < 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0005C428 File Offset: 0x0005B828
		public static OracleBoolean operator <=(OracleBinary x, OracleBinary y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) <= 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0005C468 File Offset: 0x0005B868
		public static OracleBoolean operator !=(OracleBinary x, OracleBinary y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x.CompareTo(y) != 0);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x04000331 RID: 817
		private byte[] _value;

		// Token: 0x04000332 RID: 818
		public static readonly OracleBinary Null = new OracleBinary(true);
	}
}
