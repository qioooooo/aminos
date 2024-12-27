using System;
using System.Text;

namespace System.Collections.Specialized
{
	// Token: 0x0200024B RID: 587
	public struct BitVector32
	{
		// Token: 0x0600141C RID: 5148 RVA: 0x00043028 File Offset: 0x00042028
		public BitVector32(int data)
		{
			this.data = (uint)data;
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x00043031 File Offset: 0x00042031
		public BitVector32(BitVector32 value)
		{
			this.data = value.data;
		}

		// Token: 0x1700041F RID: 1055
		public bool this[int bit]
		{
			get
			{
				return ((ulong)this.data & (ulong)((long)bit)) == (ulong)bit;
			}
			set
			{
				if (value)
				{
					this.data |= (uint)bit;
					return;
				}
				this.data &= (uint)(~(uint)bit);
			}
		}

		// Token: 0x17000420 RID: 1056
		public int this[BitVector32.Section section]
		{
			get
			{
				return (int)((this.data & (uint)((uint)section.Mask << (int)section.Offset)) >> (int)section.Offset);
			}
			set
			{
				value <<= (int)section.Offset;
				int num = (65535 & (int)section.Mask) << (int)section.Offset;
				this.data = (this.data & (uint)(~(uint)num)) | (uint)(value & num);
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x000430E3 File Offset: 0x000420E3
		public int Data
		{
			get
			{
				return (int)this.data;
			}
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x000430EC File Offset: 0x000420EC
		private static short CountBitsSet(short mask)
		{
			short num = 0;
			while ((mask & 1) != 0)
			{
				num += 1;
				mask = (short)(mask >> 1);
			}
			return num;
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x0004310E File Offset: 0x0004210E
		public static int CreateMask()
		{
			return BitVector32.CreateMask(0);
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x00043116 File Offset: 0x00042116
		public static int CreateMask(int previous)
		{
			if (previous == 0)
			{
				return 1;
			}
			if (previous == -2147483648)
			{
				throw new InvalidOperationException(SR.GetString("BitVectorFull"));
			}
			return previous << 1;
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x00043138 File Offset: 0x00042138
		private static short CreateMaskFromHighValue(short highValue)
		{
			short num = 16;
			while (((int)highValue & 32768) == 0)
			{
				num -= 1;
				highValue = (short)(highValue << 1);
			}
			ushort num2 = 0;
			while (num > 0)
			{
				num -= 1;
				num2 = (ushort)(num2 << 1);
				num2 |= 1;
			}
			return (short)num2;
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x00043177 File Offset: 0x00042177
		public static BitVector32.Section CreateSection(short maxValue)
		{
			return BitVector32.CreateSectionHelper(maxValue, 0, 0);
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x00043181 File Offset: 0x00042181
		public static BitVector32.Section CreateSection(short maxValue, BitVector32.Section previous)
		{
			return BitVector32.CreateSectionHelper(maxValue, previous.Mask, previous.Offset);
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x00043198 File Offset: 0x00042198
		private static BitVector32.Section CreateSectionHelper(short maxValue, short priorMask, short priorOffset)
		{
			if (maxValue < 1)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidValue", new object[] { "maxValue", 0 }), "maxValue");
			}
			short num = priorOffset + BitVector32.CountBitsSet(priorMask);
			if (num >= 32)
			{
				throw new InvalidOperationException(SR.GetString("BitVectorFull"));
			}
			return new BitVector32.Section(BitVector32.CreateMaskFromHighValue(maxValue), num);
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x00043202 File Offset: 0x00042202
		public override bool Equals(object o)
		{
			return o is BitVector32 && this.data == ((BitVector32)o).data;
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x00043221 File Offset: 0x00042221
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x00043234 File Offset: 0x00042234
		public static string ToString(BitVector32 value)
		{
			StringBuilder stringBuilder = new StringBuilder(45);
			stringBuilder.Append("BitVector32{");
			int num = (int)value.data;
			for (int i = 0; i < 32; i++)
			{
				if (((long)num & (long)((ulong)(-2147483648))) != 0L)
				{
					stringBuilder.Append("1");
				}
				else
				{
					stringBuilder.Append("0");
				}
				num <<= 1;
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x000432A7 File Offset: 0x000422A7
		public override string ToString()
		{
			return BitVector32.ToString(this);
		}

		// Token: 0x0400116B RID: 4459
		private uint data;

		// Token: 0x0200024C RID: 588
		public struct Section
		{
			// Token: 0x0600142E RID: 5166 RVA: 0x000432B4 File Offset: 0x000422B4
			internal Section(short mask, short offset)
			{
				this.mask = mask;
				this.offset = offset;
			}

			// Token: 0x17000422 RID: 1058
			// (get) Token: 0x0600142F RID: 5167 RVA: 0x000432C4 File Offset: 0x000422C4
			public short Mask
			{
				get
				{
					return this.mask;
				}
			}

			// Token: 0x17000423 RID: 1059
			// (get) Token: 0x06001430 RID: 5168 RVA: 0x000432CC File Offset: 0x000422CC
			public short Offset
			{
				get
				{
					return this.offset;
				}
			}

			// Token: 0x06001431 RID: 5169 RVA: 0x000432D4 File Offset: 0x000422D4
			public override bool Equals(object o)
			{
				return o is BitVector32.Section && this.Equals((BitVector32.Section)o);
			}

			// Token: 0x06001432 RID: 5170 RVA: 0x000432EC File Offset: 0x000422EC
			public bool Equals(BitVector32.Section obj)
			{
				return obj.mask == this.mask && obj.offset == this.offset;
			}

			// Token: 0x06001433 RID: 5171 RVA: 0x0004330E File Offset: 0x0004230E
			public static bool operator ==(BitVector32.Section a, BitVector32.Section b)
			{
				return a.Equals(b);
			}

			// Token: 0x06001434 RID: 5172 RVA: 0x00043318 File Offset: 0x00042318
			public static bool operator !=(BitVector32.Section a, BitVector32.Section b)
			{
				return !(a == b);
			}

			// Token: 0x06001435 RID: 5173 RVA: 0x00043324 File Offset: 0x00042324
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x06001436 RID: 5174 RVA: 0x00043338 File Offset: 0x00042338
			public static string ToString(BitVector32.Section value)
			{
				return string.Concat(new string[]
				{
					"Section{0x",
					Convert.ToString(value.Mask, 16),
					", 0x",
					Convert.ToString(value.Offset, 16),
					"}"
				});
			}

			// Token: 0x06001437 RID: 5175 RVA: 0x0004338C File Offset: 0x0004238C
			public override string ToString()
			{
				return BitVector32.Section.ToString(this);
			}

			// Token: 0x0400116C RID: 4460
			private readonly short mask;

			// Token: 0x0400116D RID: 4461
			private readonly short offset;
		}
	}
}
