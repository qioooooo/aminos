using System;
using System.Globalization;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x02000016 RID: 22
	internal sealed class PrimitiveArray
	{
		// Token: 0x0600007F RID: 127 RVA: 0x00005B17 File Offset: 0x00004B17
		internal PrimitiveArray(InternalPrimitiveTypeE code, Array array)
		{
			this.Init(code, array);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00005B28 File Offset: 0x00004B28
		internal void Init(InternalPrimitiveTypeE code, Array array)
		{
			this.code = code;
			switch (code)
			{
			case InternalPrimitiveTypeE.Boolean:
				this.booleanA = (bool[])array;
				return;
			case InternalPrimitiveTypeE.Byte:
			case InternalPrimitiveTypeE.Currency:
			case InternalPrimitiveTypeE.Decimal:
			case InternalPrimitiveTypeE.TimeSpan:
			case InternalPrimitiveTypeE.DateTime:
				break;
			case InternalPrimitiveTypeE.Char:
				this.charA = (char[])array;
				return;
			case InternalPrimitiveTypeE.Double:
				this.doubleA = (double[])array;
				return;
			case InternalPrimitiveTypeE.Int16:
				this.int16A = (short[])array;
				return;
			case InternalPrimitiveTypeE.Int32:
				this.int32A = (int[])array;
				return;
			case InternalPrimitiveTypeE.Int64:
				this.int64A = (long[])array;
				return;
			case InternalPrimitiveTypeE.SByte:
				this.sbyteA = (sbyte[])array;
				return;
			case InternalPrimitiveTypeE.Single:
				this.singleA = (float[])array;
				return;
			case InternalPrimitiveTypeE.UInt16:
				this.uint16A = (ushort[])array;
				return;
			case InternalPrimitiveTypeE.UInt32:
				this.uint32A = (uint[])array;
				return;
			case InternalPrimitiveTypeE.UInt64:
				this.uint64A = (ulong[])array;
				break;
			default:
				return;
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00005C18 File Offset: 0x00004C18
		internal string GetValue(int index)
		{
			string text = null;
			switch (this.code)
			{
			case InternalPrimitiveTypeE.Boolean:
				text = this.booleanA[index].ToString();
				break;
			case InternalPrimitiveTypeE.Char:
				if (this.charA[index] == '\0')
				{
					text = "_0x00_";
				}
				else
				{
					text = char.ToString(this.charA[index]);
				}
				break;
			case InternalPrimitiveTypeE.Double:
				if (double.IsPositiveInfinity(this.doubleA[index]))
				{
					text = "INF";
				}
				else if (double.IsNegativeInfinity(this.doubleA[index]))
				{
					text = "-INF";
				}
				else
				{
					text = this.doubleA[index].ToString("R", CultureInfo.InvariantCulture);
				}
				break;
			case InternalPrimitiveTypeE.Int16:
				text = this.int16A[index].ToString(CultureInfo.InvariantCulture);
				break;
			case InternalPrimitiveTypeE.Int32:
				text = this.int32A[index].ToString(CultureInfo.InvariantCulture);
				break;
			case InternalPrimitiveTypeE.Int64:
				text = this.int64A[index].ToString(CultureInfo.InvariantCulture);
				break;
			case InternalPrimitiveTypeE.SByte:
				text = this.sbyteA[index].ToString(CultureInfo.InvariantCulture);
				break;
			case InternalPrimitiveTypeE.Single:
				if (float.IsPositiveInfinity(this.singleA[index]))
				{
					text = "INF";
				}
				else if (float.IsNegativeInfinity(this.singleA[index]))
				{
					text = "-INF";
				}
				else
				{
					text = this.singleA[index].ToString("R", CultureInfo.InvariantCulture);
				}
				break;
			case InternalPrimitiveTypeE.UInt16:
				text = this.uint16A[index].ToString(CultureInfo.InvariantCulture);
				break;
			case InternalPrimitiveTypeE.UInt32:
				text = this.uint32A[index].ToString(CultureInfo.InvariantCulture);
				break;
			case InternalPrimitiveTypeE.UInt64:
				text = this.uint64A[index].ToString(CultureInfo.InvariantCulture);
				break;
			}
			return text;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00005E18 File Offset: 0x00004E18
		internal void SetValue(string value, int index)
		{
			switch (this.code)
			{
			case InternalPrimitiveTypeE.Boolean:
				this.booleanA[index] = bool.Parse(value);
				return;
			case InternalPrimitiveTypeE.Byte:
			case InternalPrimitiveTypeE.Currency:
			case InternalPrimitiveTypeE.Decimal:
			case InternalPrimitiveTypeE.TimeSpan:
			case InternalPrimitiveTypeE.DateTime:
				break;
			case InternalPrimitiveTypeE.Char:
				if (value[0] == '_' && value.Equals("_0x00_"))
				{
					this.charA[index] = '\0';
					return;
				}
				this.charA[index] = char.Parse(value);
				return;
			case InternalPrimitiveTypeE.Double:
				if (value == "INF")
				{
					this.doubleA[index] = double.PositiveInfinity;
					return;
				}
				if (value == "-INF")
				{
					this.doubleA[index] = double.NegativeInfinity;
					return;
				}
				this.doubleA[index] = double.Parse(value, CultureInfo.InvariantCulture);
				return;
			case InternalPrimitiveTypeE.Int16:
				this.int16A[index] = short.Parse(value, CultureInfo.InvariantCulture);
				return;
			case InternalPrimitiveTypeE.Int32:
				this.int32A[index] = int.Parse(value, CultureInfo.InvariantCulture);
				return;
			case InternalPrimitiveTypeE.Int64:
				this.int64A[index] = long.Parse(value, CultureInfo.InvariantCulture);
				return;
			case InternalPrimitiveTypeE.SByte:
				this.sbyteA[index] = sbyte.Parse(value, CultureInfo.InvariantCulture);
				return;
			case InternalPrimitiveTypeE.Single:
				if (value == "INF")
				{
					this.singleA[index] = float.PositiveInfinity;
					return;
				}
				if (value == "-INF")
				{
					this.singleA[index] = float.NegativeInfinity;
					return;
				}
				this.singleA[index] = float.Parse(value, CultureInfo.InvariantCulture);
				return;
			case InternalPrimitiveTypeE.UInt16:
				this.uint16A[index] = ushort.Parse(value, CultureInfo.InvariantCulture);
				return;
			case InternalPrimitiveTypeE.UInt32:
				this.uint32A[index] = uint.Parse(value, CultureInfo.InvariantCulture);
				return;
			case InternalPrimitiveTypeE.UInt64:
				this.uint64A[index] = ulong.Parse(value, CultureInfo.InvariantCulture);
				break;
			default:
				return;
			}
		}

		// Token: 0x040000AD RID: 173
		private InternalPrimitiveTypeE code;

		// Token: 0x040000AE RID: 174
		private bool[] booleanA;

		// Token: 0x040000AF RID: 175
		private char[] charA;

		// Token: 0x040000B0 RID: 176
		private double[] doubleA;

		// Token: 0x040000B1 RID: 177
		private short[] int16A;

		// Token: 0x040000B2 RID: 178
		private int[] int32A;

		// Token: 0x040000B3 RID: 179
		private long[] int64A;

		// Token: 0x040000B4 RID: 180
		private sbyte[] sbyteA;

		// Token: 0x040000B5 RID: 181
		private float[] singleA;

		// Token: 0x040000B6 RID: 182
		private ushort[] uint16A;

		// Token: 0x040000B7 RID: 183
		private uint[] uint32A;

		// Token: 0x040000B8 RID: 184
		private ulong[] uint64A;
	}
}
