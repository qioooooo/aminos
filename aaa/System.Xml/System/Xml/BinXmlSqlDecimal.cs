using System;
using System.Diagnostics;
using System.IO;

namespace System.Xml
{
	// Token: 0x020000F4 RID: 244
	internal struct BinXmlSqlDecimal
	{
		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000ECF RID: 3791 RVA: 0x00040D78 File Offset: 0x0003FD78
		public bool IsPositive
		{
			get
			{
				return this.m_bSign == 0;
			}
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x00040D84 File Offset: 0x0003FD84
		public BinXmlSqlDecimal(byte[] data, int offset, bool trim)
		{
			byte b = data[offset];
			byte b2 = b;
			if (b2 <= 11)
			{
				if (b2 == 7)
				{
					this.m_bLen = 1;
					goto IL_0052;
				}
				if (b2 == 11)
				{
					this.m_bLen = 2;
					goto IL_0052;
				}
			}
			else
			{
				if (b2 == 15)
				{
					this.m_bLen = 3;
					goto IL_0052;
				}
				if (b2 == 19)
				{
					this.m_bLen = 4;
					goto IL_0052;
				}
			}
			throw new XmlException("XmlBinary_InvalidSqlDecimal", null);
			IL_0052:
			this.m_bPrec = data[offset + 1];
			this.m_bScale = data[offset + 2];
			this.m_bSign = ((data[offset + 3] == 0) ? 1 : 0);
			this.m_data1 = BinXmlSqlDecimal.UIntFromByteArray(data, offset + 4);
			this.m_data2 = ((this.m_bLen > 1) ? BinXmlSqlDecimal.UIntFromByteArray(data, offset + 8) : 0U);
			this.m_data3 = ((this.m_bLen > 2) ? BinXmlSqlDecimal.UIntFromByteArray(data, offset + 12) : 0U);
			this.m_data4 = ((this.m_bLen > 3) ? BinXmlSqlDecimal.UIntFromByteArray(data, offset + 16) : 0U);
			if (this.m_bLen == 4 && this.m_data4 == 0U)
			{
				this.m_bLen = 3;
			}
			if (this.m_bLen == 3 && this.m_data3 == 0U)
			{
				this.m_bLen = 2;
			}
			if (this.m_bLen == 2 && this.m_data2 == 0U)
			{
				this.m_bLen = 1;
			}
			if (trim)
			{
				this.TrimTrailingZeros();
			}
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x00040EC0 File Offset: 0x0003FEC0
		public void Write(Stream strm)
		{
			strm.WriteByte(this.m_bLen * 4 + 3);
			strm.WriteByte(this.m_bPrec);
			strm.WriteByte(this.m_bScale);
			strm.WriteByte((this.m_bSign == 0) ? 1 : 0);
			this.WriteUI4(this.m_data1, strm);
			if (this.m_bLen > 1)
			{
				this.WriteUI4(this.m_data2, strm);
				if (this.m_bLen > 2)
				{
					this.WriteUI4(this.m_data3, strm);
					if (this.m_bLen > 3)
					{
						this.WriteUI4(this.m_data4, strm);
					}
				}
			}
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00040F58 File Offset: 0x0003FF58
		private void WriteUI4(uint val, Stream strm)
		{
			strm.WriteByte((byte)(val & 255U));
			strm.WriteByte((byte)((val >> 8) & 255U));
			strm.WriteByte((byte)((val >> 16) & 255U));
			strm.WriteByte((byte)((val >> 24) & 255U));
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x00040FA8 File Offset: 0x0003FFA8
		private static uint UIntFromByteArray(byte[] data, int offset)
		{
			int num = (int)data[offset];
			num |= (int)data[offset + 1] << 8;
			num |= (int)data[offset + 2] << 16;
			return (uint)(num | ((int)data[offset + 3] << 24));
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x00040FDA File Offset: 0x0003FFDA
		private bool FZero()
		{
			return this.m_data1 == 0U && this.m_bLen <= 1;
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x00040FF2 File Offset: 0x0003FFF2
		private void StoreFromWorkingArray(uint[] rguiData)
		{
			this.m_data1 = rguiData[0];
			this.m_data2 = rguiData[1];
			this.m_data3 = rguiData[2];
			this.m_data4 = rguiData[3];
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x00041018 File Offset: 0x00040018
		private bool FGt10_38(uint[] rglData)
		{
			return (ulong)rglData[3] >= 1262177448UL && ((ulong)rglData[3] > 1262177448UL || (ulong)rglData[2] > 1518781562UL || ((ulong)rglData[2] == 1518781562UL && (ulong)rglData[1] >= 160047680UL));
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0004106C File Offset: 0x0004006C
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
			BinXmlSqlDecimal.MpNormalize(rgulU, ref ciulU);
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x000410B1 File Offset: 0x000400B1
		private static void MpNormalize(uint[] rgulU, ref int ciulU)
		{
			while (ciulU > 1 && rgulU[ciulU - 1] == 0U)
			{
				ciulU--;
			}
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x000410C8 File Offset: 0x000400C8
		internal void AdjustScale(int digits, bool fRound)
		{
			bool flag = false;
			int i = digits;
			if (i + (int)this.m_bScale < 0)
			{
				throw new XmlException("SqlTypes_ArithTruncation", null);
			}
			if (i + (int)this.m_bScale > (int)BinXmlSqlDecimal.NUMERIC_MAX_PRECISION)
			{
				throw new XmlException("SqlTypes_ArithOverflow", null);
			}
			byte b = (byte)(i + (int)this.m_bScale);
			byte b2 = (byte)Math.Min((int)BinXmlSqlDecimal.NUMERIC_MAX_PRECISION, Math.Max(1, i + (int)this.m_bPrec));
			if (i > 0)
			{
				this.m_bScale = b;
				this.m_bPrec = b2;
				while (i > 0)
				{
					uint num;
					if (i >= 9)
					{
						num = BinXmlSqlDecimal.x_rgulShiftBase[8];
						i -= 9;
					}
					else
					{
						num = BinXmlSqlDecimal.x_rgulShiftBase[i - 1];
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
						num = BinXmlSqlDecimal.x_rgulShiftBase[8];
						i += 9;
					}
					else
					{
						num = BinXmlSqlDecimal.x_rgulShiftBase[-i - 1];
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
				this.m_bSign = 0;
			}
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x000411EC File Offset: 0x000401EC
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
			if (num2 == BinXmlSqlDecimal.x_cNumeMax)
			{
				throw new XmlException("SqlTypes_ArithOverflow", null);
			}
			array[num2] = (uint)num;
			this.m_bLen += 1;
			if (this.FGt10_38(array))
			{
				throw new XmlException("SqlTypes_ArithOverflow", null);
			}
			this.StoreFromWorkingArray(array);
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x000412A4 File Offset: 0x000402A4
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
					num3 = BinXmlSqlDecimal.x_ulInt32Base;
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
				if (bLen == BinXmlSqlDecimal.x_cNumeMax)
				{
					throw new XmlException("SqlTypes_ArithOverflow", null);
				}
				array[bLen] = (uint)num;
				this.m_bLen += 1;
			}
			if (this.FGt10_38(array))
			{
				throw new XmlException("SqlTypes_ArithOverflow", null);
			}
			this.StoreFromWorkingArray(array);
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x00041378 File Offset: 0x00040378
		internal uint DivByULong(uint iDivisor)
		{
			ulong num = (ulong)iDivisor;
			ulong num2 = 0UL;
			bool flag = true;
			if (num == 0UL)
			{
				throw new XmlException("SqlTypes_DivideByZero", null);
			}
			uint[] array = new uint[] { this.m_data1, this.m_data2, this.m_data3, this.m_data4 };
			for (int i = (int)this.m_bLen; i > 0; i--)
			{
				num2 = (num2 << 32) + (ulong)array[i - 1];
				uint num3 = (uint)(num2 / num);
				array[i - 1] = num3;
				num2 %= num;
				flag = flag && num3 == 0U;
				if (flag)
				{
					this.m_bLen -= 1;
				}
			}
			this.StoreFromWorkingArray(array);
			if (flag)
			{
				this.m_bLen = 1;
			}
			return (uint)num2;
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x00041439 File Offset: 0x00040439
		private static byte CLenFromPrec(byte bPrec)
		{
			return BinXmlSqlDecimal.rgCLenFromPrec[(int)(bPrec - 1)];
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x00041444 File Offset: 0x00040444
		private static char ChFromDigit(uint uiDigit)
		{
			return (char)(uiDigit + 48U);
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x0004144C File Offset: 0x0004044C
		public decimal ToDecimal()
		{
			if (this.m_data4 != 0U || this.m_bScale > 28)
			{
				throw new XmlException("SqlTypes_ArithOverflow", null);
			}
			return new decimal((int)this.m_data1, (int)this.m_data2, (int)this.m_data3, !this.IsPositive, this.m_bScale);
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x000414A0 File Offset: 0x000404A0
		private void TrimTrailingZeros()
		{
			uint[] array = new uint[] { this.m_data1, this.m_data2, this.m_data3, this.m_data4 };
			int bLen = (int)this.m_bLen;
			if (bLen == 1 && array[0] == 0U)
			{
				this.m_bScale = 0;
				return;
			}
			while (this.m_bScale > 0 && (bLen > 1 || array[0] != 0U))
			{
				uint num;
				BinXmlSqlDecimal.MpDiv1(array, ref bLen, 10U, out num);
				if (num != 0U)
				{
					break;
				}
				this.m_data1 = array[0];
				this.m_data2 = array[1];
				this.m_data3 = array[2];
				this.m_data4 = array[3];
				this.m_bScale -= 1;
			}
			if (this.m_bLen == 4 && this.m_data4 == 0U)
			{
				this.m_bLen = 3;
			}
			if (this.m_bLen == 3 && this.m_data3 == 0U)
			{
				this.m_bLen = 2;
			}
			if (this.m_bLen == 2 && this.m_data2 == 0U)
			{
				this.m_bLen = 1;
			}
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x00041590 File Offset: 0x00040590
		public override string ToString()
		{
			uint[] array = new uint[] { this.m_data1, this.m_data2, this.m_data3, this.m_data4 };
			int bLen = (int)this.m_bLen;
			char[] array2 = new char[(int)(BinXmlSqlDecimal.NUMERIC_MAX_PRECISION + 1)];
			int i = 0;
			while (bLen > 1 || array[0] != 0U)
			{
				uint num;
				BinXmlSqlDecimal.MpDiv1(array, ref bLen, 10U, out num);
				array2[i++] = BinXmlSqlDecimal.ChFromDigit(num);
			}
			while (i <= (int)this.m_bScale)
			{
				array2[i++] = BinXmlSqlDecimal.ChFromDigit(0U);
			}
			bool isPositive = this.IsPositive;
			int num2 = (isPositive ? i : (i + 1));
			if (this.m_bScale > 0)
			{
				num2++;
			}
			char[] array3 = new char[num2];
			int num3 = 0;
			if (!isPositive)
			{
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

		// Token: 0x06000EE2 RID: 3810 RVA: 0x00041698 File Offset: 0x00040698
		[Conditional("DEBUG")]
		private void AssertValid()
		{
			uint[] array = new uint[] { this.m_data1, this.m_data2, this.m_data3, this.m_data4 };
			uint num = array[(int)(this.m_bLen - 1)];
			for (int i = (int)this.m_bLen; i < BinXmlSqlDecimal.x_cNumeMax; i++)
			{
			}
		}

		// Token: 0x040009F2 RID: 2546
		internal byte m_bLen;

		// Token: 0x040009F3 RID: 2547
		internal byte m_bPrec;

		// Token: 0x040009F4 RID: 2548
		internal byte m_bScale;

		// Token: 0x040009F5 RID: 2549
		internal byte m_bSign;

		// Token: 0x040009F6 RID: 2550
		internal uint m_data1;

		// Token: 0x040009F7 RID: 2551
		internal uint m_data2;

		// Token: 0x040009F8 RID: 2552
		internal uint m_data3;

		// Token: 0x040009F9 RID: 2553
		internal uint m_data4;

		// Token: 0x040009FA RID: 2554
		private static readonly byte NUMERIC_MAX_PRECISION = 38;

		// Token: 0x040009FB RID: 2555
		private static readonly byte MaxPrecision = BinXmlSqlDecimal.NUMERIC_MAX_PRECISION;

		// Token: 0x040009FC RID: 2556
		private static readonly byte MaxScale = BinXmlSqlDecimal.NUMERIC_MAX_PRECISION;

		// Token: 0x040009FD RID: 2557
		private static readonly int x_cNumeMax = 4;

		// Token: 0x040009FE RID: 2558
		private static readonly long x_lInt32Base = 4294967296L;

		// Token: 0x040009FF RID: 2559
		private static readonly ulong x_ulInt32Base = 4294967296UL;

		// Token: 0x04000A00 RID: 2560
		private static readonly ulong x_ulInt32BaseForMod = BinXmlSqlDecimal.x_ulInt32Base - 1UL;

		// Token: 0x04000A01 RID: 2561
		internal static readonly ulong x_llMax = 9223372036854775807UL;

		// Token: 0x04000A02 RID: 2562
		private static readonly double DUINT_BASE = (double)BinXmlSqlDecimal.x_lInt32Base;

		// Token: 0x04000A03 RID: 2563
		private static readonly double DUINT_BASE2 = BinXmlSqlDecimal.DUINT_BASE * BinXmlSqlDecimal.DUINT_BASE;

		// Token: 0x04000A04 RID: 2564
		private static readonly double DUINT_BASE3 = BinXmlSqlDecimal.DUINT_BASE2 * BinXmlSqlDecimal.DUINT_BASE;

		// Token: 0x04000A05 RID: 2565
		private static readonly uint[] x_rgulShiftBase = new uint[] { 10U, 100U, 1000U, 10000U, 100000U, 1000000U, 10000000U, 100000000U, 1000000000U };

		// Token: 0x04000A06 RID: 2566
		private static readonly byte[] rgCLenFromPrec = new byte[]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 2,
			2, 2, 2, 2, 2, 2, 2, 2, 2, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 4, 4,
			4, 4, 4, 4, 4, 4, 4, 4
		};
	}
}
