using System;
using System.Data.Common;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x02000349 RID: 841
	[XmlSchemaProvider("GetXsdType")]
	[Serializable]
	public struct SqlDecimal : INullable, IComparable, IXmlSerializable
	{
		// Token: 0x06002C84 RID: 11396 RVA: 0x002A5EB0 File Offset: 0x002A52B0
		private byte CalculatePrecision()
		{
			int num;
			uint[] array;
			uint num2;
			if (this.m_data4 != 0U)
			{
				num = 33;
				array = SqlDecimal.DecimalHelpersHiHi;
				num2 = this.m_data4;
			}
			else if (this.m_data3 != 0U)
			{
				num = 24;
				array = SqlDecimal.DecimalHelpersHi;
				num2 = this.m_data3;
			}
			else if (this.m_data2 != 0U)
			{
				num = 15;
				array = SqlDecimal.DecimalHelpersMid;
				num2 = this.m_data2;
			}
			else
			{
				num = 5;
				array = SqlDecimal.DecimalHelpersLo;
				num2 = this.m_data1;
			}
			if (num2 < array[num])
			{
				num -= 2;
				if (num2 < array[num])
				{
					num -= 2;
					if (num2 < array[num])
					{
						num--;
					}
					else
					{
						num++;
					}
				}
				else
				{
					num++;
				}
			}
			else
			{
				num += 2;
				if (num2 < array[num])
				{
					num--;
				}
				else
				{
					num++;
				}
			}
			if (num2 >= array[num])
			{
				num++;
				if (num == 37 && num2 >= array[num])
				{
					num++;
				}
			}
			byte b = (byte)(num + 1);
			if (b > 1 && this.VerifyPrecision(b - 1))
			{
				b -= 1;
			}
			return Math.Max(b, this.m_bScale);
		}

		// Token: 0x06002C85 RID: 11397 RVA: 0x002A5F9C File Offset: 0x002A539C
		private bool VerifyPrecision(byte precision)
		{
			int num = (int)(checked(precision - 1));
			if (this.m_data4 < SqlDecimal.DecimalHelpersHiHi[num])
			{
				return true;
			}
			if (this.m_data4 == SqlDecimal.DecimalHelpersHiHi[num])
			{
				if (this.m_data3 < SqlDecimal.DecimalHelpersHi[num])
				{
					return true;
				}
				if (this.m_data3 == SqlDecimal.DecimalHelpersHi[num])
				{
					if (this.m_data2 < SqlDecimal.DecimalHelpersMid[num])
					{
						return true;
					}
					if (this.m_data2 == SqlDecimal.DecimalHelpersMid[num] && this.m_data1 < SqlDecimal.DecimalHelpersLo[num])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002C86 RID: 11398 RVA: 0x002A6020 File Offset: 0x002A5420
		private SqlDecimal(bool fNull)
		{
			this.m_bLen = (this.m_bPrec = (this.m_bScale = 0));
			this.m_bStatus = 0;
			this.m_data1 = (this.m_data2 = (this.m_data3 = (this.m_data4 = SqlDecimal.x_uiZero)));
		}

		// Token: 0x06002C87 RID: 11399 RVA: 0x002A6078 File Offset: 0x002A5478
		public SqlDecimal(decimal value)
		{
			this.m_bStatus = SqlDecimal.x_bNotNull;
			int[] bits = decimal.GetBits(value);
			uint num = (uint)bits[3];
			this.m_data1 = (uint)bits[0];
			this.m_data2 = (uint)bits[1];
			this.m_data3 = (uint)bits[2];
			this.m_data4 = SqlDecimal.x_uiZero;
			this.m_bStatus |= (((num & 2147483648U) == 2147483648U) ? SqlDecimal.x_bNegative : 0);
			if (this.m_data3 != 0U)
			{
				this.m_bLen = 3;
			}
			else if (this.m_data2 != 0U)
			{
				this.m_bLen = 2;
			}
			else
			{
				this.m_bLen = 1;
			}
			this.m_bScale = (byte)((int)(num & 16711680U) >> 16);
			this.m_bPrec = 0;
			this.m_bPrec = this.CalculatePrecision();
		}

		// Token: 0x06002C88 RID: 11400 RVA: 0x002A6134 File Offset: 0x002A5534
		public SqlDecimal(int value)
		{
			this.m_bStatus = SqlDecimal.x_bNotNull;
			uint num = (uint)value;
			if (value < 0)
			{
				this.m_bStatus |= SqlDecimal.x_bNegative;
				if (value != -2147483648)
				{
					num = (uint)(-(uint)value);
				}
			}
			this.m_data1 = num;
			this.m_data2 = (this.m_data3 = (this.m_data4 = SqlDecimal.x_uiZero));
			this.m_bLen = 1;
			this.m_bPrec = SqlDecimal.BGetPrecUI4(this.m_data1);
			this.m_bScale = 0;
		}

		// Token: 0x06002C89 RID: 11401 RVA: 0x002A61B4 File Offset: 0x002A55B4
		public SqlDecimal(long value)
		{
			this.m_bStatus = SqlDecimal.x_bNotNull;
			ulong num = (ulong)value;
			if (value < 0L)
			{
				this.m_bStatus |= SqlDecimal.x_bNegative;
				if (value != -9223372036854775808L)
				{
					num = (ulong)(-(ulong)value);
				}
			}
			this.m_data1 = (uint)num;
			this.m_data2 = (uint)(num >> 32);
			this.m_data3 = (this.m_data4 = 0U);
			this.m_bLen = ((this.m_data2 == 0U) ? 1 : 2);
			this.m_bPrec = SqlDecimal.BGetPrecUI8(num);
			this.m_bScale = 0;
		}

		// Token: 0x06002C8A RID: 11402 RVA: 0x002A6240 File Offset: 0x002A5640
		public SqlDecimal(byte bPrecision, byte bScale, bool fPositive, int[] bits)
		{
			SqlDecimal.CheckValidPrecScale(bPrecision, bScale);
			if (bits == null)
			{
				throw new ArgumentNullException("bits");
			}
			if (bits.Length != 4)
			{
				throw new ArgumentException(SQLResource.InvalidArraySizeMessage, "bits");
			}
			this.m_bPrec = bPrecision;
			this.m_bScale = bScale;
			this.m_data1 = (uint)bits[0];
			this.m_data2 = (uint)bits[1];
			this.m_data3 = (uint)bits[2];
			this.m_data4 = (uint)bits[3];
			this.m_bLen = 1;
			for (int i = 3; i >= 0; i--)
			{
				if (bits[i] != 0)
				{
					this.m_bLen = (byte)(i + 1);
					break;
				}
			}
			this.m_bStatus = SqlDecimal.x_bNotNull;
			if (!fPositive)
			{
				this.m_bStatus |= SqlDecimal.x_bNegative;
			}
			if (this.FZero())
			{
				this.SetPositive();
			}
			if (bPrecision < this.CalculatePrecision())
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x002A6318 File Offset: 0x002A5718
		public SqlDecimal(byte bPrecision, byte bScale, bool fPositive, int data1, int data2, int data3, int data4)
		{
			SqlDecimal.CheckValidPrecScale(bPrecision, bScale);
			this.m_bPrec = bPrecision;
			this.m_bScale = bScale;
			this.m_data1 = (uint)data1;
			this.m_data2 = (uint)data2;
			this.m_data3 = (uint)data3;
			this.m_data4 = (uint)data4;
			this.m_bLen = 1;
			if (data4 == 0)
			{
				if (data3 == 0)
				{
					if (data2 == 0)
					{
						this.m_bLen = 1;
					}
					else
					{
						this.m_bLen = 2;
					}
				}
				else
				{
					this.m_bLen = 3;
				}
			}
			else
			{
				this.m_bLen = 4;
			}
			this.m_bStatus = SqlDecimal.x_bNotNull;
			if (!fPositive)
			{
				this.m_bStatus |= SqlDecimal.x_bNegative;
			}
			if (this.FZero())
			{
				this.SetPositive();
			}
			if (bPrecision < this.CalculatePrecision())
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x002A63D4 File Offset: 0x002A57D4
		public SqlDecimal(double dVal)
		{
			this = new SqlDecimal(false);
			this.m_bStatus = SqlDecimal.x_bNotNull;
			if (dVal < 0.0)
			{
				dVal = -dVal;
				this.m_bStatus |= SqlDecimal.x_bNegative;
			}
			if (dVal >= SqlDecimal.DMAX_NUME)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			double num = Math.Floor(dVal);
			double num2 = dVal - num;
			this.m_bPrec = SqlDecimal.NUMERIC_MAX_PRECISION;
			this.m_bLen = 1;
			if (num > 0.0)
			{
				dVal = Math.Floor(num / SqlDecimal.DUINT_BASE);
				this.m_data1 = (uint)(num - dVal * SqlDecimal.DUINT_BASE);
				num = dVal;
				if (num > 0.0)
				{
					dVal = Math.Floor(num / SqlDecimal.DUINT_BASE);
					this.m_data2 = (uint)(num - dVal * SqlDecimal.DUINT_BASE);
					num = dVal;
					this.m_bLen += 1;
					if (num > 0.0)
					{
						dVal = Math.Floor(num / SqlDecimal.DUINT_BASE);
						this.m_data3 = (uint)(num - dVal * SqlDecimal.DUINT_BASE);
						num = dVal;
						this.m_bLen += 1;
						if (num > 0.0)
						{
							dVal = Math.Floor(num / SqlDecimal.DUINT_BASE);
							this.m_data4 = (uint)(num - dVal * SqlDecimal.DUINT_BASE);
							this.m_bLen += 1;
						}
					}
				}
			}
			uint num3 = (uint)(this.FZero() ? 0 : this.CalculatePrecision());
			if (num3 > SqlDecimal.DBL_DIG)
			{
				uint num4 = num3 - SqlDecimal.DBL_DIG;
				uint num5;
				do
				{
					num5 = this.DivByULong(10U);
					num4 -= 1U;
				}
				while (num4 > 0U);
				num4 = num3 - SqlDecimal.DBL_DIG;
				if (num5 >= 5U)
				{
					this.AddULong(1U);
					num3 = (uint)this.CalculatePrecision() + num4;
				}
				do
				{
					this.MultByULong(10U);
					num4 -= 1U;
				}
				while (num4 > 0U);
			}
			this.m_bScale = (byte)((num3 < SqlDecimal.DBL_DIG) ? (SqlDecimal.DBL_DIG - num3) : 0U);
			this.m_bPrec = (byte)(num3 + (uint)this.m_bScale);
			if (this.m_bScale > 0)
			{
				num3 = (uint)this.m_bScale;
				do
				{
					uint num6 = ((num3 >= 9U) ? 9U : num3);
					num2 *= SqlDecimal.x_rgulShiftBase[(int)(num6 - 1U)];
					num3 -= num6;
					this.MultByULong(SqlDecimal.x_rgulShiftBase[(int)(num6 - 1U)]);
					this.AddULong((uint)num2);
					num2 -= Math.Floor(num2);
				}
				while (num3 > 0U);
			}
			if (num2 >= 0.5)
			{
				this.AddULong(1U);
			}
			if (this.FZero())
			{
				this.SetPositive();
			}
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x002A6628 File Offset: 0x002A5A28
		private SqlDecimal(uint[] rglData, byte bLen, byte bPrec, byte bScale, bool fPositive)
		{
			SqlDecimal.CheckValidPrecScale(bPrec, bScale);
			this.m_bLen = bLen;
			this.m_bPrec = bPrec;
			this.m_bScale = bScale;
			this.m_data1 = rglData[0];
			this.m_data2 = rglData[1];
			this.m_data3 = rglData[2];
			this.m_data4 = rglData[3];
			this.m_bStatus = SqlDecimal.x_bNotNull;
			if (!fPositive)
			{
				this.m_bStatus |= SqlDecimal.x_bNegative;
			}
			if (this.FZero())
			{
				this.SetPositive();
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06002C8E RID: 11406 RVA: 0x002A66A8 File Offset: 0x002A5AA8
		public bool IsNull
		{
			get
			{
				return (this.m_bStatus & SqlDecimal.x_bNullMask) == SqlDecimal.x_bIsNull;
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06002C8F RID: 11407 RVA: 0x002A66C8 File Offset: 0x002A5AC8
		public decimal Value
		{
			get
			{
				return this.ToDecimal();
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06002C90 RID: 11408 RVA: 0x002A66DC File Offset: 0x002A5ADC
		public bool IsPositive
		{
			get
			{
				if (this.IsNull)
				{
					throw new SqlNullValueException();
				}
				return (this.m_bStatus & SqlDecimal.x_bSignMask) == SqlDecimal.x_bPositive;
			}
		}

		// Token: 0x06002C91 RID: 11409 RVA: 0x002A670C File Offset: 0x002A5B0C
		private void SetPositive()
		{
			this.m_bStatus &= SqlDecimal.x_bReverseSignMask;
		}

		// Token: 0x06002C92 RID: 11410 RVA: 0x002A672C File Offset: 0x002A5B2C
		private void SetSignBit(bool fPositive)
		{
			this.m_bStatus = (fPositive ? (this.m_bStatus & SqlDecimal.x_bReverseSignMask) : (this.m_bStatus | SqlDecimal.x_bNegative));
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06002C93 RID: 11411 RVA: 0x002A6760 File Offset: 0x002A5B60
		public byte Precision
		{
			get
			{
				if (this.IsNull)
				{
					throw new SqlNullValueException();
				}
				return this.m_bPrec;
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06002C94 RID: 11412 RVA: 0x002A6784 File Offset: 0x002A5B84
		public byte Scale
		{
			get
			{
				if (this.IsNull)
				{
					throw new SqlNullValueException();
				}
				return this.m_bScale;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06002C95 RID: 11413 RVA: 0x002A67A8 File Offset: 0x002A5BA8
		public int[] Data
		{
			get
			{
				if (this.IsNull)
				{
					throw new SqlNullValueException();
				}
				return new int[]
				{
					(int)this.m_data1,
					(int)this.m_data2,
					(int)this.m_data3,
					(int)this.m_data4
				};
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06002C96 RID: 11414 RVA: 0x002A67F0 File Offset: 0x002A5BF0
		public byte[] BinData
		{
			get
			{
				if (this.IsNull)
				{
					throw new SqlNullValueException();
				}
				int num = (int)this.m_data1;
				int num2 = (int)this.m_data2;
				int num3 = (int)this.m_data3;
				int num4 = (int)this.m_data4;
				byte[] array = new byte[16];
				array[0] = (byte)(num & 255);
				num >>= 8;
				array[1] = (byte)(num & 255);
				num >>= 8;
				array[2] = (byte)(num & 255);
				num >>= 8;
				array[3] = (byte)(num & 255);
				array[4] = (byte)(num2 & 255);
				num2 >>= 8;
				array[5] = (byte)(num2 & 255);
				num2 >>= 8;
				array[6] = (byte)(num2 & 255);
				num2 >>= 8;
				array[7] = (byte)(num2 & 255);
				array[8] = (byte)(num3 & 255);
				num3 >>= 8;
				array[9] = (byte)(num3 & 255);
				num3 >>= 8;
				array[10] = (byte)(num3 & 255);
				num3 >>= 8;
				array[11] = (byte)(num3 & 255);
				array[12] = (byte)(num4 & 255);
				num4 >>= 8;
				array[13] = (byte)(num4 & 255);
				num4 >>= 8;
				array[14] = (byte)(num4 & 255);
				num4 >>= 8;
				array[15] = (byte)(num4 & 255);
				return array;
			}
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x002A6924 File Offset: 0x002A5D24
		public override string ToString()
		{
			if (this.IsNull)
			{
				return SQLResource.NullString;
			}
			uint[] array = new uint[] { this.m_data1, this.m_data2, this.m_data3, this.m_data4 };
			int bLen = (int)this.m_bLen;
			char[] array2 = new char[(int)(SqlDecimal.NUMERIC_MAX_PRECISION + 1)];
			int i = 0;
			while (bLen > 1 || array[0] != 0U)
			{
				uint num;
				SqlDecimal.MpDiv1(array, ref bLen, SqlDecimal.x_ulBase10, out num);
				array2[i++] = SqlDecimal.ChFromDigit(num);
			}
			while (i <= (int)this.m_bScale)
			{
				array2[i++] = SqlDecimal.ChFromDigit(0U);
			}
			int num2 = 0;
			int num3 = 0;
			if (this.m_bScale > 0)
			{
				num2 = 1;
			}
			char[] array3;
			if (this.IsPositive)
			{
				array3 = new char[num2 + i];
			}
			else
			{
				array3 = new char[num2 + i + 1];
				array3[num3++] = '-';
			}
			while (i > 0)
			{
				if (i-- == (int)this.m_bScale)
				{
					array3[num3++] = '.';
				}
				array3[num3++] = array2[i];
			}
			return new string(array3);
		}

		// Token: 0x06002C98 RID: 11416 RVA: 0x002A6A34 File Offset: 0x002A5E34
		public static SqlDecimal Parse(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (s == SQLResource.NullString)
			{
				return SqlDecimal.Null;
			}
			SqlDecimal @null = SqlDecimal.Null;
			char[] array = s.ToCharArray();
			int num = array.Length;
			int num2 = -1;
			int num3 = 0;
			@null.m_bPrec = 1;
			@null.m_bScale = 0;
			@null.SetToZero();
			while (num != 0 && array[num - 1] == ' ')
			{
				num--;
			}
			if (num == 0)
			{
				throw new FormatException(SQLResource.FormatMessage);
			}
			while (array[num3] == ' ')
			{
				num3++;
				num--;
			}
			if (array[num3] == '-')
			{
				@null.SetSignBit(false);
				num3++;
				num--;
			}
			else
			{
				@null.SetSignBit(true);
				if (array[num3] == '+')
				{
					num3++;
					num--;
				}
			}
			while (num > 2 && array[num3] == '0')
			{
				num3++;
				num--;
			}
			if (2 == num && '0' == array[num3] && '.' == array[num3 + 1])
			{
				array[num3] = '.';
				array[num3 + 1] = '0';
			}
			if (num == 0 || num > (int)(SqlDecimal.NUMERIC_MAX_PRECISION + 1))
			{
				throw new FormatException(SQLResource.FormatMessage);
			}
			while (num > 1 && array[num3] == '0')
			{
				num3++;
				num--;
			}
			int i;
			for (i = 0; i < num; i++)
			{
				char c = array[num3];
				num3++;
				if (c >= '0' && c <= '9')
				{
					c -= '0';
					@null.MultByULong(SqlDecimal.x_ulBase10);
					@null.AddULong((uint)c);
				}
				else
				{
					if (c != '.' || num2 >= 0)
					{
						throw new FormatException(SQLResource.FormatMessage);
					}
					num2 = i;
				}
			}
			if (num2 < 0)
			{
				@null.m_bPrec = (byte)i;
				@null.m_bScale = 0;
			}
			else
			{
				@null.m_bPrec = (byte)(i - 1);
				@null.m_bScale = (byte)((int)@null.m_bPrec - num2);
			}
			if (@null.m_bPrec > SqlDecimal.NUMERIC_MAX_PRECISION)
			{
				throw new FormatException(SQLResource.FormatMessage);
			}
			if (@null.m_bPrec == 0)
			{
				throw new FormatException(SQLResource.FormatMessage);
			}
			if (@null.FZero())
			{
				@null.SetPositive();
			}
			return @null;
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x002A6C20 File Offset: 0x002A6020
		public double ToDouble()
		{
			if (this.IsNull)
			{
				throw new SqlNullValueException();
			}
			double num = this.m_data4;
			num = num * (double)SqlDecimal.x_lInt32Base + this.m_data3;
			num = num * (double)SqlDecimal.x_lInt32Base + this.m_data2;
			num = num * (double)SqlDecimal.x_lInt32Base + this.m_data1;
			num /= Math.Pow(10.0, (double)this.m_bScale);
			if (!this.IsPositive)
			{
				return -num;
			}
			return num;
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x002A6CA8 File Offset: 0x002A60A8
		private decimal ToDecimal()
		{
			if (this.IsNull)
			{
				throw new SqlNullValueException();
			}
			if (this.m_data4 != 0U || this.m_bScale > 28)
			{
				throw new OverflowException(SQLResource.ConversionOverflowMessage);
			}
			return new decimal((int)this.m_data1, (int)this.m_data2, (int)this.m_data3, !this.IsPositive, this.m_bScale);
		}

		// Token: 0x06002C9B RID: 11419 RVA: 0x002A6D08 File Offset: 0x002A6108
		public static implicit operator SqlDecimal(decimal x)
		{
			return new SqlDecimal(x);
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x002A6D1C File Offset: 0x002A611C
		public static explicit operator SqlDecimal(double x)
		{
			return new SqlDecimal(x);
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x002A6D30 File Offset: 0x002A6130
		public static implicit operator SqlDecimal(long x)
		{
			return new SqlDecimal(new decimal(x));
		}

		// Token: 0x06002C9E RID: 11422 RVA: 0x002A6D48 File Offset: 0x002A6148
		public static explicit operator decimal(SqlDecimal x)
		{
			return x.Value;
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x002A6D5C File Offset: 0x002A615C
		public static SqlDecimal operator -(SqlDecimal x)
		{
			if (x.IsNull)
			{
				return SqlDecimal.Null;
			}
			SqlDecimal sqlDecimal = x;
			if (sqlDecimal.FZero())
			{
				sqlDecimal.SetPositive();
			}
			else
			{
				sqlDecimal.SetSignBit(!sqlDecimal.IsPositive);
			}
			return sqlDecimal;
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x002A6DA0 File Offset: 0x002A61A0
		public static SqlDecimal operator +(SqlDecimal x, SqlDecimal y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlDecimal.Null;
			}
			bool flag = true;
			bool flag2 = x.IsPositive;
			bool flag3 = y.IsPositive;
			int bScale = (int)x.m_bScale;
			int bScale2 = (int)y.m_bScale;
			int num = Math.Max((int)x.m_bPrec - bScale, (int)y.m_bPrec - bScale2);
			int num2 = Math.Max(bScale, bScale2);
			int num3 = num + num2 + 1;
			num3 = Math.Min((int)SqlDecimal.MaxPrecision, num3);
			if (num3 - num < num2)
			{
				num2 = num3 - num;
			}
			if (bScale != num2)
			{
				x.AdjustScale(num2 - bScale, true);
			}
			if (bScale2 != num2)
			{
				y.AdjustScale(num2 - bScale2, true);
			}
			if (!flag2)
			{
				flag2 = !flag2;
				flag3 = !flag3;
				flag = !flag;
			}
			int num4 = (int)x.m_bLen;
			int num5 = (int)y.m_bLen;
			uint[] array = new uint[] { x.m_data1, x.m_data2, x.m_data3, x.m_data4 };
			uint[] array2 = new uint[] { y.m_data1, y.m_data2, y.m_data3, y.m_data4 };
			byte b;
			if (flag3)
			{
				ulong num6 = 0UL;
				int num7 = 0;
				while (num7 < num4 || num7 < num5)
				{
					if (num7 < num4)
					{
						num6 += (ulong)array[num7];
					}
					if (num7 < num5)
					{
						num6 += (ulong)array2[num7];
					}
					array[num7] = (uint)num6;
					num6 >>= 32;
					num7++;
				}
				if (num6 != 0UL)
				{
					if (num7 == SqlDecimal.x_cNumeMax)
					{
						throw new OverflowException(SQLResource.ArithOverflowMessage);
					}
					array[num7] = (uint)num6;
					num7++;
				}
				b = (byte)num7;
			}
			else
			{
				int num8 = 0;
				if (x.LAbsCmp(y) < 0)
				{
					flag = !flag;
					uint[] array3 = array2;
					array2 = array;
					array = array3;
					num4 = num5;
					num5 = (int)x.m_bLen;
				}
				ulong num6 = SqlDecimal.x_ulInt32Base;
				int num7 = 0;
				while (num7 < num4 || num7 < num5)
				{
					if (num7 < num4)
					{
						num6 += (ulong)array[num7];
					}
					if (num7 < num5)
					{
						num6 -= (ulong)array2[num7];
					}
					array[num7] = (uint)num6;
					if (array[num7] != 0U)
					{
						num8 = num7;
					}
					num6 >>= 32;
					num6 += SqlDecimal.x_ulInt32BaseForMod;
					num7++;
				}
				b = (byte)(num8 + 1);
			}
			SqlDecimal sqlDecimal = new SqlDecimal(array, b, (byte)num3, (byte)num2, flag);
			if (sqlDecimal.FGt10_38() || sqlDecimal.CalculatePrecision() > SqlDecimal.NUMERIC_MAX_PRECISION)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			if (sqlDecimal.FZero())
			{
				sqlDecimal.SetPositive();
			}
			return sqlDecimal;
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x002A701C File Offset: 0x002A641C
		public static SqlDecimal operator -(SqlDecimal x, SqlDecimal y)
		{
			return x + -y;
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x002A7038 File Offset: 0x002A6438
		public static SqlDecimal operator *(SqlDecimal x, SqlDecimal y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlDecimal.Null;
			}
			int bLen = (int)y.m_bLen;
			int num = (int)(x.m_bScale + y.m_bScale);
			int num2 = num;
			int num3 = (int)(x.m_bPrec - x.m_bScale + (y.m_bPrec - y.m_bScale) + 1);
			int num4 = num2 + num3;
			if (num4 > (int)SqlDecimal.NUMERIC_MAX_PRECISION)
			{
				num4 = (int)SqlDecimal.NUMERIC_MAX_PRECISION;
			}
			if (num2 > (int)SqlDecimal.NUMERIC_MAX_PRECISION)
			{
				num2 = (int)SqlDecimal.NUMERIC_MAX_PRECISION;
			}
			num2 = Math.Min(num4 - num3, num2);
			num2 = Math.Max(num2, Math.Min(num, (int)SqlDecimal.x_cNumeDivScaleMin));
			int num5 = num2 - num;
			bool flag = x.IsPositive == y.IsPositive;
			uint[] array = new uint[] { x.m_data1, x.m_data2, x.m_data3, x.m_data4 };
			uint[] array2 = new uint[] { y.m_data1, y.m_data2, y.m_data3, y.m_data4 };
			uint[] array3 = new uint[9];
			int i = 0;
			for (int j = 0; j < (int)x.m_bLen; j++)
			{
				uint num6 = array[j];
				ulong num7 = 0UL;
				i = j;
				for (int k = 0; k < bLen; k++)
				{
					ulong num8 = num7 + (ulong)array3[i];
					ulong num9 = (ulong)array2[k];
					num7 = (ulong)num6 * num9;
					num7 += num8;
					if (num7 < num8)
					{
						num8 = SqlDecimal.x_ulInt32Base;
					}
					else
					{
						num8 = 0UL;
					}
					array3[i++] = (uint)num7;
					num7 = (num7 >> 32) + num8;
				}
				if (num7 != 0UL)
				{
					array3[i++] = (uint)num7;
				}
			}
			while (array3[i] == 0U && i > 0)
			{
				i--;
			}
			int num10 = i + 1;
			if (num5 != 0)
			{
				if (num5 < 0)
				{
					uint num11;
					uint num12;
					do
					{
						if (num5 <= -9)
						{
							num11 = SqlDecimal.x_rgulShiftBase[8];
							num5 += 9;
						}
						else
						{
							num11 = SqlDecimal.x_rgulShiftBase[-num5 - 1];
							num5 = 0;
						}
						SqlDecimal.MpDiv1(array3, ref num10, num11, out num12);
					}
					while (num5 != 0);
					if (num10 > SqlDecimal.x_cNumeMax)
					{
						throw new OverflowException(SQLResource.ArithOverflowMessage);
					}
					for (i = num10; i < SqlDecimal.x_cNumeMax; i++)
					{
						array3[i] = 0U;
					}
					SqlDecimal sqlDecimal = new SqlDecimal(array3, (byte)num10, (byte)num4, (byte)num2, flag);
					if (sqlDecimal.FGt10_38())
					{
						throw new OverflowException(SQLResource.ArithOverflowMessage);
					}
					if (num12 >= num11 / 2U)
					{
						sqlDecimal.AddULong(1U);
					}
					if (sqlDecimal.FZero())
					{
						sqlDecimal.SetPositive();
					}
					return sqlDecimal;
				}
				else
				{
					if (num10 > SqlDecimal.x_cNumeMax)
					{
						throw new OverflowException(SQLResource.ArithOverflowMessage);
					}
					for (i = num10; i < SqlDecimal.x_cNumeMax; i++)
					{
						array3[i] = 0U;
					}
					SqlDecimal sqlDecimal = new SqlDecimal(array3, (byte)num10, (byte)num4, (byte)num, flag);
					if (sqlDecimal.FZero())
					{
						sqlDecimal.SetPositive();
					}
					sqlDecimal.AdjustScale(num5, true);
					return sqlDecimal;
				}
			}
			else
			{
				if (num10 > SqlDecimal.x_cNumeMax)
				{
					throw new OverflowException(SQLResource.ArithOverflowMessage);
				}
				for (i = num10; i < SqlDecimal.x_cNumeMax; i++)
				{
					array3[i] = 0U;
				}
				SqlDecimal sqlDecimal = new SqlDecimal(array3, (byte)num10, (byte)num4, (byte)num2, flag);
				if (sqlDecimal.FGt10_38())
				{
					throw new OverflowException(SQLResource.ArithOverflowMessage);
				}
				if (sqlDecimal.FZero())
				{
					sqlDecimal.SetPositive();
				}
				return sqlDecimal;
			}
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x002A737C File Offset: 0x002A677C
		public static SqlDecimal operator /(SqlDecimal x, SqlDecimal y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlDecimal.Null;
			}
			if (y.FZero())
			{
				throw new DivideByZeroException(SQLResource.DivideByZeroMessage);
			}
			bool flag = x.IsPositive == y.IsPositive;
			int num = Math.Max((int)(x.m_bScale + y.m_bPrec + 1), (int)SqlDecimal.x_cNumeDivScaleMin);
			int num2 = (int)(x.m_bPrec - x.m_bScale + y.m_bScale);
			int num3 = num + (int)x.m_bPrec + (int)y.m_bPrec + 1;
			int num4 = Math.Min(num, (int)SqlDecimal.x_cNumeDivScaleMin);
			num2 = Math.Min(num2, (int)SqlDecimal.NUMERIC_MAX_PRECISION);
			num3 = num2 + num;
			if (num3 > (int)SqlDecimal.NUMERIC_MAX_PRECISION)
			{
				num3 = (int)SqlDecimal.NUMERIC_MAX_PRECISION;
			}
			num = Math.Min(num3 - num2, num);
			num = Math.Max(num, num4);
			int num5 = num - (int)x.m_bScale + (int)y.m_bScale;
			x.AdjustScale(num5, true);
			uint[] array = new uint[] { x.m_data1, x.m_data2, x.m_data3, x.m_data4 };
			uint[] array2 = new uint[] { y.m_data1, y.m_data2, y.m_data3, y.m_data4 };
			uint[] array3 = new uint[SqlDecimal.x_cNumeMax + 1];
			uint[] array4 = new uint[SqlDecimal.x_cNumeMax];
			int num6;
			int num7;
			SqlDecimal.MpDiv(array, (int)x.m_bLen, array2, (int)y.m_bLen, array4, out num6, array3, out num7);
			SqlDecimal.ZeroToMaxLen(array4, num6);
			SqlDecimal sqlDecimal = new SqlDecimal(array4, (byte)num6, (byte)num3, (byte)num, flag);
			if (sqlDecimal.FZero())
			{
				sqlDecimal.SetPositive();
			}
			return sqlDecimal;
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x002A753C File Offset: 0x002A693C
		public static explicit operator SqlDecimal(SqlBoolean x)
		{
			if (!x.IsNull)
			{
				return new SqlDecimal((int)x.ByteValue);
			}
			return SqlDecimal.Null;
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x002A7564 File Offset: 0x002A6964
		public static implicit operator SqlDecimal(SqlByte x)
		{
			if (!x.IsNull)
			{
				return new SqlDecimal((int)x.Value);
			}
			return SqlDecimal.Null;
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x002A758C File Offset: 0x002A698C
		public static implicit operator SqlDecimal(SqlInt16 x)
		{
			if (!x.IsNull)
			{
				return new SqlDecimal((int)x.Value);
			}
			return SqlDecimal.Null;
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x002A75B4 File Offset: 0x002A69B4
		public static implicit operator SqlDecimal(SqlInt32 x)
		{
			if (!x.IsNull)
			{
				return new SqlDecimal(x.Value);
			}
			return SqlDecimal.Null;
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x002A75DC File Offset: 0x002A69DC
		public static implicit operator SqlDecimal(SqlInt64 x)
		{
			if (!x.IsNull)
			{
				return new SqlDecimal(x.Value);
			}
			return SqlDecimal.Null;
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x002A7604 File Offset: 0x002A6A04
		public static implicit operator SqlDecimal(SqlMoney x)
		{
			if (!x.IsNull)
			{
				return new SqlDecimal(x.ToDecimal());
			}
			return SqlDecimal.Null;
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x002A762C File Offset: 0x002A6A2C
		public static explicit operator SqlDecimal(SqlSingle x)
		{
			if (!x.IsNull)
			{
				return new SqlDecimal((double)x.Value);
			}
			return SqlDecimal.Null;
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x002A7658 File Offset: 0x002A6A58
		public static explicit operator SqlDecimal(SqlDouble x)
		{
			if (!x.IsNull)
			{
				return new SqlDecimal(x.Value);
			}
			return SqlDecimal.Null;
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x002A7680 File Offset: 0x002A6A80
		public static explicit operator SqlDecimal(SqlString x)
		{
			if (!x.IsNull)
			{
				return SqlDecimal.Parse(x.Value);
			}
			return SqlDecimal.Null;
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x002A76A8 File Offset: 0x002A6AA8
		[Conditional("DEBUG")]
		private void AssertValid()
		{
			if (this.IsNull)
			{
				return;
			}
			uint[] array = new uint[] { this.m_data1, this.m_data2, this.m_data3, this.m_data4 };
			uint num = array[(int)(this.m_bLen - 1)];
			for (int i = (int)this.m_bLen; i < SqlDecimal.x_cNumeMax; i++)
			{
			}
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x002A770C File Offset: 0x002A6B0C
		private static void ZeroToMaxLen(uint[] rgulData, int cUI4sCur)
		{
			switch (cUI4sCur)
			{
			case 1:
				rgulData[1] = (rgulData[2] = (rgulData[3] = 0U));
				return;
			case 2:
				rgulData[2] = (rgulData[3] = 0U);
				return;
			case 3:
				rgulData[3] = 0U;
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x002A7750 File Offset: 0x002A6B50
		private static byte CLenFromPrec(byte bPrec)
		{
			return SqlDecimal.rgCLenFromPrec[(int)(bPrec - 1)];
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x002A7768 File Offset: 0x002A6B68
		private bool FZero()
		{
			return this.m_data1 == 0U && this.m_bLen <= 1;
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x002A778C File Offset: 0x002A6B8C
		private bool FGt10_38()
		{
			return (ulong)this.m_data4 >= 1262177448UL && this.m_bLen == 4 && ((ulong)this.m_data4 > 1262177448UL || (ulong)this.m_data3 > 1518781562UL || ((ulong)this.m_data3 == 1518781562UL && (ulong)this.m_data2 >= 160047680UL));
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x002A77F8 File Offset: 0x002A6BF8
		private bool FGt10_38(uint[] rglData)
		{
			return (ulong)rglData[3] >= 1262177448UL && ((ulong)rglData[3] > 1262177448UL || (ulong)rglData[2] > 1518781562UL || ((ulong)rglData[2] == 1518781562UL && (ulong)rglData[1] >= 160047680UL));
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x002A784C File Offset: 0x002A6C4C
		private static byte BGetPrecUI4(uint value)
		{
			int num;
			if (value < SqlDecimal.x_ulT4)
			{
				if (value < SqlDecimal.x_ulT2)
				{
					num = ((value >= SqlDecimal.x_ulT1) ? 2 : 1);
				}
				else
				{
					num = ((value >= SqlDecimal.x_ulT3) ? 4 : 3);
				}
			}
			else if (value < SqlDecimal.x_ulT8)
			{
				if (value < SqlDecimal.x_ulT6)
				{
					num = ((value >= SqlDecimal.x_ulT5) ? 6 : 5);
				}
				else
				{
					num = ((value >= SqlDecimal.x_ulT7) ? 8 : 7);
				}
			}
			else
			{
				num = ((value >= SqlDecimal.x_ulT9) ? 10 : 9);
			}
			return (byte)num;
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x002A78C8 File Offset: 0x002A6CC8
		private static byte BGetPrecUI8(uint ulU0, uint ulU1)
		{
			ulong num = (ulong)ulU0 + ((ulong)ulU1 << 32);
			return SqlDecimal.BGetPrecUI8(num);
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x002A78E4 File Offset: 0x002A6CE4
		private static byte BGetPrecUI8(ulong dwlVal)
		{
			int num2;
			if (dwlVal < (ulong)SqlDecimal.x_ulT8)
			{
				uint num = (uint)dwlVal;
				if (num < SqlDecimal.x_ulT4)
				{
					if (num < SqlDecimal.x_ulT2)
					{
						num2 = ((num >= SqlDecimal.x_ulT1) ? 2 : 1);
					}
					else
					{
						num2 = ((num >= SqlDecimal.x_ulT3) ? 4 : 3);
					}
				}
				else if (num < SqlDecimal.x_ulT6)
				{
					num2 = ((num >= SqlDecimal.x_ulT5) ? 6 : 5);
				}
				else
				{
					num2 = ((num >= SqlDecimal.x_ulT7) ? 8 : 7);
				}
			}
			else if (dwlVal < SqlDecimal.x_dwlT16)
			{
				if (dwlVal < SqlDecimal.x_dwlT12)
				{
					if (dwlVal < SqlDecimal.x_dwlT10)
					{
						num2 = ((dwlVal >= (ulong)SqlDecimal.x_ulT9) ? 10 : 9);
					}
					else
					{
						num2 = ((dwlVal >= SqlDecimal.x_dwlT11) ? 12 : 11);
					}
				}
				else if (dwlVal < SqlDecimal.x_dwlT14)
				{
					num2 = ((dwlVal >= SqlDecimal.x_dwlT13) ? 14 : 13);
				}
				else
				{
					num2 = ((dwlVal >= SqlDecimal.x_dwlT15) ? 16 : 15);
				}
			}
			else if (dwlVal < SqlDecimal.x_dwlT18)
			{
				num2 = ((dwlVal >= SqlDecimal.x_dwlT17) ? 18 : 17);
			}
			else
			{
				num2 = ((dwlVal >= SqlDecimal.x_dwlT19) ? 20 : 19);
			}
			return (byte)num2;
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x002A79EC File Offset: 0x002A6DEC
		private void AddULong(uint ulAdd)
		{
			ulong num = (ulong)ulAdd;
			int bLen = (int)this.m_bLen;
			uint[] array = new uint[] { this.m_data1, this.m_data2, this.m_data3, this.m_data4 };
			int num2 = 0;
			for (;;)
			{
				num += (ulong)array[num2];
				array[num2] = (uint)num;
				num >>= 32;
				if (0UL == num)
				{
					break;
				}
				num2++;
				if (num2 >= bLen)
				{
					goto Block_2;
				}
			}
			this.StoreFromWorkingArray(array);
			return;
			Block_2:
			if (num2 == SqlDecimal.x_cNumeMax)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			array[num2] = (uint)num;
			this.m_bLen += 1;
			if (this.FGt10_38(array))
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			this.StoreFromWorkingArray(array);
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x002A7A9C File Offset: 0x002A6E9C
		private void MultByULong(uint uiMultiplier)
		{
			int bLen = (int)this.m_bLen;
			ulong num = 0UL;
			uint[] array = new uint[] { this.m_data1, this.m_data2, this.m_data3, this.m_data4 };
			for (int i = 0; i < bLen; i++)
			{
				ulong num2 = (ulong)array[i];
				ulong num3 = num2 * (ulong)uiMultiplier;
				num += num3;
				if (num < num3)
				{
					num3 = SqlDecimal.x_ulInt32Base;
				}
				else
				{
					num3 = 0UL;
				}
				array[i] = (uint)num;
				num = (num >> 32) + num3;
			}
			if (num != 0UL)
			{
				if (bLen == SqlDecimal.x_cNumeMax)
				{
					throw new OverflowException(SQLResource.ArithOverflowMessage);
				}
				array[bLen] = (uint)num;
				this.m_bLen += 1;
			}
			if (this.FGt10_38(array))
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			this.StoreFromWorkingArray(array);
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x002A7B6C File Offset: 0x002A6F6C
		private uint DivByULong(uint iDivisor)
		{
			ulong num = (ulong)iDivisor;
			ulong num2 = 0UL;
			bool flag = true;
			if (num == 0UL)
			{
				throw new DivideByZeroException(SQLResource.DivideByZeroMessage);
			}
			uint[] array = new uint[] { this.m_data1, this.m_data2, this.m_data3, this.m_data4 };
			for (int i = (int)this.m_bLen; i > 0; i--)
			{
				num2 = (num2 << 32) + (ulong)array[i - 1];
				uint num3 = (uint)(num2 / num);
				array[i - 1] = num3;
				num2 %= num;
				if (flag && num3 == 0U)
				{
					this.m_bLen -= 1;
				}
				else
				{
					flag = false;
				}
			}
			this.StoreFromWorkingArray(array);
			if (flag)
			{
				this.m_bLen = 1;
			}
			return (uint)num2;
		}

		// Token: 0x06002CB9 RID: 11449 RVA: 0x002A7C24 File Offset: 0x002A7024
		internal void AdjustScale(int digits, bool fRound)
		{
			bool flag = false;
			int i = digits;
			if (i + (int)this.m_bScale < 0)
			{
				throw new SqlTruncateException();
			}
			if (i + (int)this.m_bScale > (int)SqlDecimal.NUMERIC_MAX_PRECISION)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			byte b = (byte)(i + (int)this.m_bScale);
			byte b2 = (byte)Math.Min((int)SqlDecimal.NUMERIC_MAX_PRECISION, Math.Max(1, i + (int)this.m_bPrec));
			if (i > 0)
			{
				this.m_bScale = b;
				this.m_bPrec = b2;
				while (i > 0)
				{
					uint num;
					if (i >= 9)
					{
						num = SqlDecimal.x_rgulShiftBase[8];
						i -= 9;
					}
					else
					{
						num = SqlDecimal.x_rgulShiftBase[i - 1];
						i = 0;
					}
					this.MultByULong(num);
				}
			}
			else if (i < 0)
			{
				uint num;
				uint num2;
				do
				{
					if (i <= -9)
					{
						num = SqlDecimal.x_rgulShiftBase[8];
						i += 9;
					}
					else
					{
						num = SqlDecimal.x_rgulShiftBase[-i - 1];
						i = 0;
					}
					num2 = this.DivByULong(num);
				}
				while (i < 0);
				flag = num2 >= num / 2U;
				this.m_bScale = b;
				this.m_bPrec = b2;
			}
			if (flag && fRound)
			{
				this.AddULong(1U);
				return;
			}
			if (this.FZero())
			{
				this.SetPositive();
			}
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x002A7D30 File Offset: 0x002A7130
		public static SqlDecimal AdjustScale(SqlDecimal n, int digits, bool fRound)
		{
			if (n.IsNull)
			{
				return SqlDecimal.Null;
			}
			SqlDecimal sqlDecimal = n;
			sqlDecimal.AdjustScale(digits, fRound);
			return sqlDecimal;
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x002A7D58 File Offset: 0x002A7158
		public static SqlDecimal ConvertToPrecScale(SqlDecimal n, int precision, int scale)
		{
			SqlDecimal.CheckValidPrecScale(precision, scale);
			if (n.IsNull)
			{
				return SqlDecimal.Null;
			}
			SqlDecimal sqlDecimal = n;
			int num = scale - (int)sqlDecimal.m_bScale;
			sqlDecimal.AdjustScale(num, true);
			byte b = SqlDecimal.CLenFromPrec((byte)precision);
			if (b < sqlDecimal.m_bLen)
			{
				throw new SqlTruncateException();
			}
			if (b == sqlDecimal.m_bLen && precision < (int)sqlDecimal.CalculatePrecision())
			{
				throw new SqlTruncateException();
			}
			sqlDecimal.m_bPrec = (byte)precision;
			return sqlDecimal;
		}

		// Token: 0x06002CBC RID: 11452 RVA: 0x002A7DCC File Offset: 0x002A71CC
		private int LAbsCmp(SqlDecimal snumOp)
		{
			int bLen = (int)snumOp.m_bLen;
			int bLen2 = (int)this.m_bLen;
			if (bLen != bLen2)
			{
				if (bLen2 <= bLen)
				{
					return -1;
				}
				return 1;
			}
			else
			{
				uint[] array = new uint[] { this.m_data1, this.m_data2, this.m_data3, this.m_data4 };
				uint[] array2 = new uint[] { snumOp.m_data1, snumOp.m_data2, snumOp.m_data3, snumOp.m_data4 };
				int num = bLen - 1;
				while (array[num] == array2[num])
				{
					num--;
					if (num < 0)
					{
						return 0;
					}
				}
				if (array[num] <= array2[num])
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x002A7E7C File Offset: 0x002A727C
		private static void MpMove(uint[] rgulS, int ciulS, uint[] rgulD, out int ciulD)
		{
			ciulD = ciulS;
			for (int i = 0; i < ciulS; i++)
			{
				rgulD[i] = rgulS[i];
			}
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x002A7EA0 File Offset: 0x002A72A0
		private static void MpSet(uint[] rgulD, out int ciulD, uint iulN)
		{
			ciulD = 1;
			rgulD[0] = iulN;
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x002A7EB4 File Offset: 0x002A72B4
		private static void MpNormalize(uint[] rgulU, ref int ciulU)
		{
			while (ciulU > 1 && rgulU[ciulU - 1] == 0U)
			{
				ciulU--;
			}
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x002A7ED8 File Offset: 0x002A72D8
		private static void MpMul1(uint[] piulD, ref int ciulD, uint iulX)
		{
			uint num = 0U;
			int i;
			for (i = 0; i < ciulD; i++)
			{
				ulong num2 = (ulong)piulD[i];
				ulong num3 = (ulong)num + num2 * (ulong)iulX;
				num = SqlDecimal.HI(num3);
				piulD[i] = SqlDecimal.LO(num3);
			}
			if (num != 0U)
			{
				piulD[i] = num;
				ciulD++;
			}
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x002A7F20 File Offset: 0x002A7320
		private static void MpDiv1(uint[] rgulU, ref int ciulU, uint iulD, out uint iulR)
		{
			uint num = 0U;
			ulong num2 = (ulong)iulD;
			int i = ciulU;
			while (i > 0)
			{
				i--;
				ulong num3 = ((ulong)num << 32) + (ulong)rgulU[i];
				rgulU[i] = (uint)(num3 / num2);
				num = (uint)(num3 - (ulong)rgulU[i] * num2);
			}
			iulR = num;
			SqlDecimal.MpNormalize(rgulU, ref ciulU);
		}

		// Token: 0x06002CC2 RID: 11458 RVA: 0x002A7F68 File Offset: 0x002A7368
		internal static ulong DWL(uint lo, uint hi)
		{
			return (ulong)lo + ((ulong)hi << 32);
		}

		// Token: 0x06002CC3 RID: 11459 RVA: 0x002A7F80 File Offset: 0x002A7380
		private static uint HI(ulong x)
		{
			return (uint)(x >> 32);
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x002A7F94 File Offset: 0x002A7394
		private static uint LO(ulong x)
		{
			return (uint)x;
		}

		// Token: 0x06002CC5 RID: 11461 RVA: 0x002A7FA4 File Offset: 0x002A73A4
		private static void MpDiv(uint[] rgulU, int ciulU, uint[] rgulD, int ciulD, uint[] rgulQ, out int ciulQ, uint[] rgulR, out int ciulR)
		{
			if (ciulD == 1 && rgulD[0] == 0U)
			{
				ciulQ = (ciulR = 0);
				return;
			}
			if (ciulU == 1 && ciulD == 1)
			{
				SqlDecimal.MpSet(rgulQ, out ciulQ, rgulU[0] / rgulD[0]);
				SqlDecimal.MpSet(rgulR, out ciulR, rgulU[0] % rgulD[0]);
				return;
			}
			if (ciulD > ciulU)
			{
				SqlDecimal.MpMove(rgulU, ciulU, rgulR, out ciulR);
				SqlDecimal.MpSet(rgulQ, out ciulQ, 0U);
				return;
			}
			if (ciulU <= 2)
			{
				ulong num = SqlDecimal.DWL(rgulU[0], rgulU[1]);
				ulong num2 = (ulong)rgulD[0];
				if (ciulD > 1)
				{
					num2 += (ulong)rgulD[1] << 32;
				}
				ulong num3 = num / num2;
				rgulQ[0] = SqlDecimal.LO(num3);
				rgulQ[1] = SqlDecimal.HI(num3);
				ciulQ = ((SqlDecimal.HI(num3) != 0U) ? 2 : 1);
				num3 = num % num2;
				rgulR[0] = SqlDecimal.LO(num3);
				rgulR[1] = SqlDecimal.HI(num3);
				ciulR = ((SqlDecimal.HI(num3) != 0U) ? 2 : 1);
				return;
			}
			if (ciulD == 1)
			{
				SqlDecimal.MpMove(rgulU, ciulU, rgulQ, out ciulQ);
				uint num4;
				SqlDecimal.MpDiv1(rgulQ, ref ciulQ, rgulD[0], out num4);
				rgulR[0] = num4;
				ciulR = 1;
				return;
			}
			ciulQ = (ciulR = 0);
			if (rgulU != rgulR)
			{
				SqlDecimal.MpMove(rgulU, ciulU, rgulR, out ciulR);
			}
			ciulQ = ciulU - ciulD + 1;
			uint num5 = rgulD[ciulD - 1];
			rgulR[ciulU] = 0U;
			int num6 = ciulU;
			uint num7 = (uint)(SqlDecimal.x_ulInt32Base / ((ulong)num5 + 1UL));
			if (num7 > 1U)
			{
				SqlDecimal.MpMul1(rgulD, ref ciulD, num7);
				num5 = rgulD[ciulD - 1];
				SqlDecimal.MpMul1(rgulR, ref ciulR, num7);
			}
			uint num8 = rgulD[ciulD - 2];
			do
			{
				ulong num9 = SqlDecimal.DWL(rgulR[num6 - 1], rgulR[num6]);
				uint num10;
				if (num5 == rgulR[num6])
				{
					num10 = (uint)(SqlDecimal.x_ulInt32Base - 1UL);
				}
				else
				{
					num10 = (uint)(num9 / (ulong)num5);
				}
				ulong num11 = (ulong)num10;
				uint num12 = (uint)(num9 - num11 * (ulong)num5);
				while ((ulong)num8 * num11 > SqlDecimal.DWL(rgulR[num6 - 2], num12))
				{
					num10 -= 1U;
					if (num12 >= -num5)
					{
						break;
					}
					num12 += num5;
					num11 = (ulong)num10;
				}
				num9 = SqlDecimal.x_ulInt32Base;
				ulong num13 = 0UL;
				int i = 0;
				int num14 = num6 - ciulD;
				while (i < ciulD)
				{
					ulong num15 = (ulong)rgulD[i];
					num13 += (ulong)num10 * num15;
					num9 += (ulong)rgulR[num14] - (ulong)SqlDecimal.LO(num13);
					num13 = (ulong)SqlDecimal.HI(num13);
					rgulR[num14] = SqlDecimal.LO(num9);
					num9 = (ulong)SqlDecimal.HI(num9) + SqlDecimal.x_ulInt32Base - 1UL;
					i++;
					num14++;
				}
				num9 += (ulong)rgulR[num14] - num13;
				rgulR[num14] = SqlDecimal.LO(num9);
				rgulQ[num6 - ciulD] = num10;
				if (SqlDecimal.HI(num9) == 0U)
				{
					rgulQ[num6 - ciulD] = num10 - 1U;
					uint num16 = 0U;
					i = 0;
					num14 = num6 - ciulD;
					while (i < ciulD)
					{
						num9 = (ulong)rgulD[i] + (ulong)rgulR[num14] + (ulong)num16;
						num16 = SqlDecimal.HI(num9);
						rgulR[num14] = SqlDecimal.LO(num9);
						i++;
						num14++;
					}
					rgulR[num14] += num16;
				}
				num6--;
			}
			while (num6 >= ciulD);
			SqlDecimal.MpNormalize(rgulQ, ref ciulQ);
			ciulR = ciulD;
			SqlDecimal.MpNormalize(rgulR, ref ciulR);
			if (num7 > 1U)
			{
				uint num17;
				SqlDecimal.MpDiv1(rgulD, ref ciulD, num7, out num17);
				SqlDecimal.MpDiv1(rgulR, ref ciulR, num7, out num17);
			}
		}

		// Token: 0x06002CC6 RID: 11462 RVA: 0x002A82B0 File Offset: 0x002A76B0
		private EComparison CompareNm(SqlDecimal snumOp)
		{
			int num = (this.IsPositive ? 1 : (-1));
			int num2 = (snumOp.IsPositive ? 1 : (-1));
			if (num == num2)
			{
				SqlDecimal sqlDecimal = this;
				SqlDecimal sqlDecimal2 = snumOp;
				int num3 = (int)(this.m_bScale - snumOp.m_bScale);
				if (num3 < 0)
				{
					try
					{
						sqlDecimal.AdjustScale(-num3, true);
						goto IL_007A;
					}
					catch (OverflowException)
					{
						return (num > 0) ? EComparison.GT : EComparison.LT;
					}
				}
				if (num3 > 0)
				{
					try
					{
						sqlDecimal2.AdjustScale(num3, true);
					}
					catch (OverflowException)
					{
						return (num > 0) ? EComparison.LT : EComparison.GT;
					}
				}
				IL_007A:
				int num4 = sqlDecimal.LAbsCmp(sqlDecimal2);
				if (num4 == 0)
				{
					return EComparison.EQ;
				}
				int num5 = num * num4;
				if (num5 < 0)
				{
					return EComparison.LT;
				}
				return EComparison.GT;
			}
			if (num != 1)
			{
				return EComparison.LT;
			}
			return EComparison.GT;
		}

		// Token: 0x06002CC7 RID: 11463 RVA: 0x002A838C File Offset: 0x002A778C
		private static void CheckValidPrecScale(byte bPrec, byte bScale)
		{
			if (bPrec < 1 || bPrec > SqlDecimal.MaxPrecision || bScale < 0 || bScale > SqlDecimal.MaxScale || bScale > bPrec)
			{
				throw new SqlTypeException(SQLResource.InvalidPrecScaleMessage);
			}
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x002A83C0 File Offset: 0x002A77C0
		private static void CheckValidPrecScale(int iPrec, int iScale)
		{
			if (iPrec < 1 || iPrec > (int)SqlDecimal.MaxPrecision || iScale < 0 || iScale > (int)SqlDecimal.MaxScale || iScale > iPrec)
			{
				throw new SqlTypeException(SQLResource.InvalidPrecScaleMessage);
			}
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x002A83F4 File Offset: 0x002A77F4
		public static SqlBoolean operator ==(SqlDecimal x, SqlDecimal y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.CompareNm(y) == EComparison.EQ);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002CCA RID: 11466 RVA: 0x002A842C File Offset: 0x002A782C
		public static SqlBoolean operator !=(SqlDecimal x, SqlDecimal y)
		{
			return !(x == y);
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x002A8448 File Offset: 0x002A7848
		public static SqlBoolean operator <(SqlDecimal x, SqlDecimal y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.CompareNm(y) == EComparison.LT);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002CCC RID: 11468 RVA: 0x002A8480 File Offset: 0x002A7880
		public static SqlBoolean operator >(SqlDecimal x, SqlDecimal y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.CompareNm(y) == EComparison.GT);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x002A84B8 File Offset: 0x002A78B8
		public static SqlBoolean operator <=(SqlDecimal x, SqlDecimal y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlBoolean.Null;
			}
			EComparison ecomparison = x.CompareNm(y);
			return new SqlBoolean(ecomparison == EComparison.LT || ecomparison == EComparison.EQ);
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x002A84F8 File Offset: 0x002A78F8
		public static SqlBoolean operator >=(SqlDecimal x, SqlDecimal y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlBoolean.Null;
			}
			EComparison ecomparison = x.CompareNm(y);
			return new SqlBoolean(ecomparison == EComparison.GT || ecomparison == EComparison.EQ);
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x002A8538 File Offset: 0x002A7938
		public static SqlDecimal Add(SqlDecimal x, SqlDecimal y)
		{
			return x + y;
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x002A854C File Offset: 0x002A794C
		public static SqlDecimal Subtract(SqlDecimal x, SqlDecimal y)
		{
			return x - y;
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x002A8560 File Offset: 0x002A7960
		public static SqlDecimal Multiply(SqlDecimal x, SqlDecimal y)
		{
			return x * y;
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x002A8574 File Offset: 0x002A7974
		public static SqlDecimal Divide(SqlDecimal x, SqlDecimal y)
		{
			return x / y;
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x002A8588 File Offset: 0x002A7988
		public static SqlBoolean Equals(SqlDecimal x, SqlDecimal y)
		{
			return x == y;
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x002A859C File Offset: 0x002A799C
		public static SqlBoolean NotEquals(SqlDecimal x, SqlDecimal y)
		{
			return x != y;
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x002A85B0 File Offset: 0x002A79B0
		public static SqlBoolean LessThan(SqlDecimal x, SqlDecimal y)
		{
			return x < y;
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x002A85C4 File Offset: 0x002A79C4
		public static SqlBoolean GreaterThan(SqlDecimal x, SqlDecimal y)
		{
			return x > y;
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x002A85D8 File Offset: 0x002A79D8
		public static SqlBoolean LessThanOrEqual(SqlDecimal x, SqlDecimal y)
		{
			return x <= y;
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x002A85EC File Offset: 0x002A79EC
		public static SqlBoolean GreaterThanOrEqual(SqlDecimal x, SqlDecimal y)
		{
			return x >= y;
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x002A8600 File Offset: 0x002A7A00
		public SqlBoolean ToSqlBoolean()
		{
			return (SqlBoolean)this;
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x002A8618 File Offset: 0x002A7A18
		public SqlByte ToSqlByte()
		{
			return (SqlByte)this;
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x002A8630 File Offset: 0x002A7A30
		public SqlDouble ToSqlDouble()
		{
			return this;
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x002A8648 File Offset: 0x002A7A48
		public SqlInt16 ToSqlInt16()
		{
			return (SqlInt16)this;
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x002A8660 File Offset: 0x002A7A60
		public SqlInt32 ToSqlInt32()
		{
			return (SqlInt32)this;
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x002A8678 File Offset: 0x002A7A78
		public SqlInt64 ToSqlInt64()
		{
			return (SqlInt64)this;
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x002A8690 File Offset: 0x002A7A90
		public SqlMoney ToSqlMoney()
		{
			return (SqlMoney)this;
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x002A86A8 File Offset: 0x002A7AA8
		public SqlSingle ToSqlSingle()
		{
			return this;
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x002A86C0 File Offset: 0x002A7AC0
		public SqlString ToSqlString()
		{
			return (SqlString)this;
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x002A86D8 File Offset: 0x002A7AD8
		private static char ChFromDigit(uint uiDigit)
		{
			return (char)(uiDigit + 48U);
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x002A86EC File Offset: 0x002A7AEC
		private void StoreFromWorkingArray(uint[] rguiData)
		{
			this.m_data1 = rguiData[0];
			this.m_data2 = rguiData[1];
			this.m_data3 = rguiData[2];
			this.m_data4 = rguiData[3];
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x002A8720 File Offset: 0x002A7B20
		private void SetToZero()
		{
			this.m_bLen = 1;
			this.m_data1 = (this.m_data2 = (this.m_data3 = (this.m_data4 = 0U)));
			this.m_bStatus = SqlDecimal.x_bNotNull | SqlDecimal.x_bPositive;
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x002A8768 File Offset: 0x002A7B68
		private void MakeInteger(out bool fFraction)
		{
			int i = (int)this.m_bScale;
			fFraction = false;
			while (i > 0)
			{
				uint num;
				if (i >= 9)
				{
					num = this.DivByULong(SqlDecimal.x_rgulShiftBase[8]);
					i -= 9;
				}
				else
				{
					num = this.DivByULong(SqlDecimal.x_rgulShiftBase[i - 1]);
					i = 0;
				}
				if (num != 0U)
				{
					fFraction = true;
				}
			}
			this.m_bScale = 0;
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x002A87C0 File Offset: 0x002A7BC0
		public static SqlDecimal Abs(SqlDecimal n)
		{
			if (n.IsNull)
			{
				return SqlDecimal.Null;
			}
			n.SetPositive();
			return n;
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x002A87E4 File Offset: 0x002A7BE4
		public static SqlDecimal Ceiling(SqlDecimal n)
		{
			if (n.IsNull)
			{
				return SqlDecimal.Null;
			}
			if (n.m_bScale == 0)
			{
				return n;
			}
			bool flag;
			n.MakeInteger(out flag);
			if (flag && n.IsPositive)
			{
				n.AddULong(1U);
			}
			if (n.FZero())
			{
				n.SetPositive();
			}
			return n;
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x002A883C File Offset: 0x002A7C3C
		public static SqlDecimal Floor(SqlDecimal n)
		{
			if (n.IsNull)
			{
				return SqlDecimal.Null;
			}
			if (n.m_bScale == 0)
			{
				return n;
			}
			bool flag;
			n.MakeInteger(out flag);
			if (flag && !n.IsPositive)
			{
				n.AddULong(1U);
			}
			if (n.FZero())
			{
				n.SetPositive();
			}
			return n;
		}

		// Token: 0x06002CE9 RID: 11497 RVA: 0x002A8894 File Offset: 0x002A7C94
		public static SqlInt32 Sign(SqlDecimal n)
		{
			if (n.IsNull)
			{
				return SqlInt32.Null;
			}
			if (n == new SqlDecimal(0))
			{
				return SqlInt32.Zero;
			}
			if (n.IsNull)
			{
				return SqlInt32.Null;
			}
			if (!n.IsPositive)
			{
				return new SqlInt32(-1);
			}
			return new SqlInt32(1);
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x002A88F0 File Offset: 0x002A7CF0
		private static SqlDecimal Round(SqlDecimal n, int lPosition, bool fTruncate)
		{
			if (n.IsNull)
			{
				return SqlDecimal.Null;
			}
			if (lPosition >= 0)
			{
				lPosition = Math.Min((int)SqlDecimal.NUMERIC_MAX_PRECISION, lPosition);
				if (lPosition >= (int)n.m_bScale)
				{
					return n;
				}
			}
			else
			{
				lPosition = Math.Max((int)(-(int)SqlDecimal.NUMERIC_MAX_PRECISION), lPosition);
				if (lPosition < (int)(n.m_bScale - n.m_bPrec))
				{
					n.SetToZero();
					return n;
				}
			}
			uint num = 0U;
			int i = Math.Abs(lPosition - (int)n.m_bScale);
			uint num2 = 1U;
			while (i > 0)
			{
				if (i >= 9)
				{
					num = n.DivByULong(SqlDecimal.x_rgulShiftBase[8]);
					num2 = SqlDecimal.x_rgulShiftBase[8];
					i -= 9;
				}
				else
				{
					num = n.DivByULong(SqlDecimal.x_rgulShiftBase[i - 1]);
					num2 = SqlDecimal.x_rgulShiftBase[i - 1];
					i = 0;
				}
			}
			if (num2 > 1U)
			{
				num /= num2 / 10U;
			}
			if (n.FZero() && (fTruncate || num < 5U))
			{
				n.SetPositive();
				return n;
			}
			if (num >= 5U && !fTruncate)
			{
				n.AddULong(1U);
			}
			i = Math.Abs(lPosition - (int)n.m_bScale);
			while (i-- > 0)
			{
				n.MultByULong(SqlDecimal.x_ulBase10);
			}
			return n;
		}

		// Token: 0x06002CEB RID: 11499 RVA: 0x002A8A04 File Offset: 0x002A7E04
		public static SqlDecimal Round(SqlDecimal n, int position)
		{
			return SqlDecimal.Round(n, position, false);
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x002A8A1C File Offset: 0x002A7E1C
		public static SqlDecimal Truncate(SqlDecimal n, int position)
		{
			return SqlDecimal.Round(n, position, true);
		}

		// Token: 0x06002CED RID: 11501 RVA: 0x002A8A34 File Offset: 0x002A7E34
		public static SqlDecimal Power(SqlDecimal n, double exp)
		{
			if (n.IsNull)
			{
				return SqlDecimal.Null;
			}
			byte precision = n.Precision;
			int scale = (int)n.Scale;
			double num = n.ToDouble();
			n = new SqlDecimal(Math.Pow(num, exp));
			n.AdjustScale(scale - (int)n.Scale, true);
			n.m_bPrec = SqlDecimal.MaxPrecision;
			return n;
		}

		// Token: 0x06002CEE RID: 11502 RVA: 0x002A8A94 File Offset: 0x002A7E94
		public int CompareTo(object value)
		{
			if (value is SqlDecimal)
			{
				SqlDecimal sqlDecimal = (SqlDecimal)value;
				return this.CompareTo(sqlDecimal);
			}
			throw ADP.WrongType(value.GetType(), typeof(SqlDecimal));
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x002A8AD0 File Offset: 0x002A7ED0
		public int CompareTo(SqlDecimal value)
		{
			if (this.IsNull)
			{
				if (!value.IsNull)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (value.IsNull)
				{
					return 1;
				}
				if (this < value)
				{
					return -1;
				}
				if (this > value)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x002A8B28 File Offset: 0x002A7F28
		public override bool Equals(object value)
		{
			if (!(value is SqlDecimal))
			{
				return false;
			}
			SqlDecimal sqlDecimal = (SqlDecimal)value;
			if (sqlDecimal.IsNull || this.IsNull)
			{
				return sqlDecimal.IsNull && this.IsNull;
			}
			return (this == sqlDecimal).Value;
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x002A8B80 File Offset: 0x002A7F80
		public override int GetHashCode()
		{
			if (this.IsNull)
			{
				return 0;
			}
			SqlDecimal sqlDecimal = this;
			int num = (int)sqlDecimal.CalculatePrecision();
			sqlDecimal.AdjustScale((int)SqlDecimal.NUMERIC_MAX_PRECISION - num, true);
			int bLen = (int)sqlDecimal.m_bLen;
			int num2 = 0;
			int[] data = sqlDecimal.Data;
			for (int i = 0; i < bLen; i++)
			{
				int num3 = (num2 >> 28) & 255;
				num2 <<= 4;
				num2 = num2 ^ data[i] ^ num3;
			}
			return num2;
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x002A8BF0 File Offset: 0x002A7FF0
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x002A8C00 File Offset: 0x002A8000
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string attribute = reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			if (attribute != null && XmlConvert.ToBoolean(attribute))
			{
				this.m_bStatus = SqlDecimal.x_bReverseNullMask & this.m_bStatus;
				return;
			}
			SqlDecimal sqlDecimal = SqlDecimal.Parse(reader.ReadElementString());
			this.m_bStatus = sqlDecimal.m_bStatus;
			this.m_bLen = sqlDecimal.m_bLen;
			this.m_bPrec = sqlDecimal.m_bPrec;
			this.m_bScale = sqlDecimal.m_bScale;
			this.m_data1 = sqlDecimal.m_data1;
			this.m_data2 = sqlDecimal.m_data2;
			this.m_data3 = sqlDecimal.m_data3;
			this.m_data4 = sqlDecimal.m_data4;
		}

		// Token: 0x06002CF4 RID: 11508 RVA: 0x002A8CB4 File Offset: 0x002A80B4
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (this.IsNull)
			{
				writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
				return;
			}
			writer.WriteString(this.ToString());
		}

		// Token: 0x06002CF5 RID: 11509 RVA: 0x002A8CF8 File Offset: 0x002A80F8
		public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
		{
			return new XmlQualifiedName("decimal", "http://www.w3.org/2001/XMLSchema");
		}

		// Token: 0x04001C97 RID: 7319
		private const int HelperTableStartIndexLo = 5;

		// Token: 0x04001C98 RID: 7320
		private const int HelperTableStartIndexMid = 15;

		// Token: 0x04001C99 RID: 7321
		private const int HelperTableStartIndexHi = 24;

		// Token: 0x04001C9A RID: 7322
		private const int HelperTableStartIndexHiHi = 33;

		// Token: 0x04001C9B RID: 7323
		internal byte m_bStatus;

		// Token: 0x04001C9C RID: 7324
		internal byte m_bLen;

		// Token: 0x04001C9D RID: 7325
		internal byte m_bPrec;

		// Token: 0x04001C9E RID: 7326
		internal byte m_bScale;

		// Token: 0x04001C9F RID: 7327
		internal uint m_data1;

		// Token: 0x04001CA0 RID: 7328
		internal uint m_data2;

		// Token: 0x04001CA1 RID: 7329
		internal uint m_data3;

		// Token: 0x04001CA2 RID: 7330
		internal uint m_data4;

		// Token: 0x04001CA3 RID: 7331
		private static readonly byte NUMERIC_MAX_PRECISION = 38;

		// Token: 0x04001CA4 RID: 7332
		public static readonly byte MaxPrecision = SqlDecimal.NUMERIC_MAX_PRECISION;

		// Token: 0x04001CA5 RID: 7333
		public static readonly byte MaxScale = SqlDecimal.NUMERIC_MAX_PRECISION;

		// Token: 0x04001CA6 RID: 7334
		private static readonly byte x_bNullMask = 1;

		// Token: 0x04001CA7 RID: 7335
		private static readonly byte x_bIsNull = 0;

		// Token: 0x04001CA8 RID: 7336
		private static readonly byte x_bNotNull = 1;

		// Token: 0x04001CA9 RID: 7337
		private static readonly byte x_bReverseNullMask = ~SqlDecimal.x_bNullMask;

		// Token: 0x04001CAA RID: 7338
		private static readonly byte x_bSignMask = 2;

		// Token: 0x04001CAB RID: 7339
		private static readonly byte x_bPositive = 0;

		// Token: 0x04001CAC RID: 7340
		private static readonly byte x_bNegative = 2;

		// Token: 0x04001CAD RID: 7341
		private static readonly byte x_bReverseSignMask = ~SqlDecimal.x_bSignMask;

		// Token: 0x04001CAE RID: 7342
		private static readonly uint x_uiZero = 0U;

		// Token: 0x04001CAF RID: 7343
		private static readonly int x_cNumeMax = 4;

		// Token: 0x04001CB0 RID: 7344
		private static readonly long x_lInt32Base = 4294967296L;

		// Token: 0x04001CB1 RID: 7345
		private static readonly ulong x_ulInt32Base = 4294967296UL;

		// Token: 0x04001CB2 RID: 7346
		private static readonly ulong x_ulInt32BaseForMod = SqlDecimal.x_ulInt32Base - 1UL;

		// Token: 0x04001CB3 RID: 7347
		internal static readonly ulong x_llMax = 9223372036854775807UL;

		// Token: 0x04001CB4 RID: 7348
		private static readonly uint x_ulBase10 = 10U;

		// Token: 0x04001CB5 RID: 7349
		private static readonly double DUINT_BASE = (double)SqlDecimal.x_lInt32Base;

		// Token: 0x04001CB6 RID: 7350
		private static readonly double DUINT_BASE2 = SqlDecimal.DUINT_BASE * SqlDecimal.DUINT_BASE;

		// Token: 0x04001CB7 RID: 7351
		private static readonly double DUINT_BASE3 = SqlDecimal.DUINT_BASE2 * SqlDecimal.DUINT_BASE;

		// Token: 0x04001CB8 RID: 7352
		private static readonly double DMAX_NUME = 1E+38;

		// Token: 0x04001CB9 RID: 7353
		private static readonly uint DBL_DIG = 17U;

		// Token: 0x04001CBA RID: 7354
		private static readonly byte x_cNumeDivScaleMin = 6;

		// Token: 0x04001CBB RID: 7355
		private static readonly uint[] x_rgulShiftBase = new uint[] { 10U, 100U, 1000U, 10000U, 100000U, 1000000U, 10000000U, 100000000U, 1000000000U };

		// Token: 0x04001CBC RID: 7356
		private static readonly uint[] DecimalHelpersLo = new uint[]
		{
			10U, 100U, 1000U, 10000U, 100000U, 1000000U, 10000000U, 100000000U, 1000000000U, 1410065408U,
			1215752192U, 3567587328U, 1316134912U, 276447232U, 2764472320U, 1874919424U, 1569325056U, 2808348672U, 2313682944U, 1661992960U,
			3735027712U, 2990538752U, 4135583744U, 2701131776U, 1241513984U, 3825205248U, 3892314112U, 268435456U, 2684354560U, 1073741824U,
			2147483648U, 0U, 0U, 0U, 0U, 0U, 0U, 0U
		};

		// Token: 0x04001CBD RID: 7357
		private static readonly uint[] DecimalHelpersMid = new uint[]
		{
			0U, 0U, 0U, 0U, 0U, 0U, 0U, 0U, 0U, 2U,
			23U, 232U, 2328U, 23283U, 232830U, 2328306U, 23283064U, 232830643U, 2328306436U, 1808227885U,
			902409669U, 434162106U, 46653770U, 466537709U, 370409800U, 3704098002U, 2681241660U, 1042612833U, 1836193738U, 1182068202U,
			3230747430U, 2242703233U, 952195850U, 932023908U, 730304488U, 3008077584U, 16004768U, 160047680U
		};

		// Token: 0x04001CBE RID: 7358
		private static readonly uint[] DecimalHelpersHi = new uint[]
		{
			0U, 0U, 0U, 0U, 0U, 0U, 0U, 0U, 0U, 0U,
			0U, 0U, 0U, 0U, 0U, 0U, 0U, 0U, 0U, 5U,
			54U, 542U, 5421U, 54210U, 542101U, 5421010U, 54210108U, 542101086U, 1126043566U, 2670501072U,
			935206946U, 762134875U, 3326381459U, 3199043520U, 1925664130U, 2076772117U, 3587851993U, 1518781562U
		};

		// Token: 0x04001CBF RID: 7359
		private static readonly uint[] DecimalHelpersHiHi = new uint[]
		{
			0U, 0U, 0U, 0U, 0U, 0U, 0U, 0U, 0U, 0U,
			0U, 0U, 0U, 0U, 0U, 0U, 0U, 0U, 0U, 0U,
			0U, 0U, 0U, 0U, 0U, 0U, 0U, 0U, 1U, 12U,
			126U, 1262U, 12621U, 126217U, 1262177U, 12621774U, 126217744U, 1262177448U
		};

		// Token: 0x04001CC0 RID: 7360
		private static readonly byte[] rgCLenFromPrec = new byte[]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 2,
			2, 2, 2, 2, 2, 2, 2, 2, 2, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 4, 4,
			4, 4, 4, 4, 4, 4, 4, 4
		};

		// Token: 0x04001CC1 RID: 7361
		private static readonly uint x_ulT1 = 10U;

		// Token: 0x04001CC2 RID: 7362
		private static readonly uint x_ulT2 = 100U;

		// Token: 0x04001CC3 RID: 7363
		private static readonly uint x_ulT3 = 1000U;

		// Token: 0x04001CC4 RID: 7364
		private static readonly uint x_ulT4 = 10000U;

		// Token: 0x04001CC5 RID: 7365
		private static readonly uint x_ulT5 = 100000U;

		// Token: 0x04001CC6 RID: 7366
		private static readonly uint x_ulT6 = 1000000U;

		// Token: 0x04001CC7 RID: 7367
		private static readonly uint x_ulT7 = 10000000U;

		// Token: 0x04001CC8 RID: 7368
		private static readonly uint x_ulT8 = 100000000U;

		// Token: 0x04001CC9 RID: 7369
		private static readonly uint x_ulT9 = 1000000000U;

		// Token: 0x04001CCA RID: 7370
		private static readonly ulong x_dwlT10 = 10000000000UL;

		// Token: 0x04001CCB RID: 7371
		private static readonly ulong x_dwlT11 = 100000000000UL;

		// Token: 0x04001CCC RID: 7372
		private static readonly ulong x_dwlT12 = 1000000000000UL;

		// Token: 0x04001CCD RID: 7373
		private static readonly ulong x_dwlT13 = 10000000000000UL;

		// Token: 0x04001CCE RID: 7374
		private static readonly ulong x_dwlT14 = 100000000000000UL;

		// Token: 0x04001CCF RID: 7375
		private static readonly ulong x_dwlT15 = 1000000000000000UL;

		// Token: 0x04001CD0 RID: 7376
		private static readonly ulong x_dwlT16 = 10000000000000000UL;

		// Token: 0x04001CD1 RID: 7377
		private static readonly ulong x_dwlT17 = 100000000000000000UL;

		// Token: 0x04001CD2 RID: 7378
		private static readonly ulong x_dwlT18 = 1000000000000000000UL;

		// Token: 0x04001CD3 RID: 7379
		private static readonly ulong x_dwlT19 = 10000000000000000000UL;

		// Token: 0x04001CD4 RID: 7380
		public static readonly SqlDecimal Null = new SqlDecimal(true);

		// Token: 0x04001CD5 RID: 7381
		public static readonly SqlDecimal MinValue = SqlDecimal.Parse("-99999999999999999999999999999999999999");

		// Token: 0x04001CD6 RID: 7382
		public static readonly SqlDecimal MaxValue = SqlDecimal.Parse("99999999999999999999999999999999999999");
	}
}
