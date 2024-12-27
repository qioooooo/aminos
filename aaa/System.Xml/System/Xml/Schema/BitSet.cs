using System;

namespace System.Xml.Schema
{
	// Token: 0x02000185 RID: 389
	internal sealed class BitSet
	{
		// Token: 0x060014A4 RID: 5284 RVA: 0x000582FE File Offset: 0x000572FE
		private BitSet()
		{
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x00058306 File Offset: 0x00057306
		public BitSet(int count)
		{
			this.count = count;
			this.bits = new uint[this.Subscript(count + 31)];
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x060014A6 RID: 5286 RVA: 0x0005832A File Offset: 0x0005732A
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000500 RID: 1280
		public bool this[int index]
		{
			get
			{
				return this.Get(index);
			}
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x0005833C File Offset: 0x0005733C
		public void Clear()
		{
			int num = this.bits.Length;
			int num2 = num;
			while (num2-- > 0)
			{
				this.bits[num2] = 0U;
			}
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x00058368 File Offset: 0x00057368
		public void Clear(int index)
		{
			int num = this.Subscript(index);
			this.EnsureLength(num + 1);
			this.bits[num] &= ~(1U << index);
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x000583A8 File Offset: 0x000573A8
		public void Set(int index)
		{
			int num = this.Subscript(index);
			this.EnsureLength(num + 1);
			this.bits[num] |= 1U << index;
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x000583E8 File Offset: 0x000573E8
		public bool Get(int index)
		{
			bool flag = false;
			if (index < this.count)
			{
				int num = this.Subscript(index);
				flag = ((ulong)this.bits[num] & (ulong)(1L << (index & 31 & 31))) != 0UL;
			}
			return flag;
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x00058428 File Offset: 0x00057428
		public int NextSet(int startFrom)
		{
			int num = startFrom + 1;
			if (num == this.count)
			{
				return -1;
			}
			int num2 = this.Subscript(num);
			num &= 31;
			uint num3;
			for (num3 = this.bits[num2] >> num; num3 == 0U; num3 = this.bits[num2])
			{
				if (++num2 == this.bits.Length)
				{
					return -1;
				}
				num = 0;
			}
			while ((num3 & 1U) == 0U)
			{
				num3 >>= 1;
				num++;
			}
			return (num2 << 5) + num;
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x00058494 File Offset: 0x00057494
		public void And(BitSet other)
		{
			if (this == other)
			{
				return;
			}
			int num = this.bits.Length;
			int num2 = other.bits.Length;
			int i = ((num > num2) ? num2 : num);
			int num3 = i;
			while (num3-- > 0)
			{
				this.bits[num3] &= other.bits[num3];
			}
			while (i < num)
			{
				this.bits[i] = 0U;
				i++;
			}
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x00058500 File Offset: 0x00057500
		public void Or(BitSet other)
		{
			if (this == other)
			{
				return;
			}
			int num = other.bits.Length;
			this.EnsureLength(num);
			int num2 = num;
			while (num2-- > 0)
			{
				this.bits[num2] |= other.bits[num2];
			}
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x00058550 File Offset: 0x00057550
		public override int GetHashCode()
		{
			int num = 1234;
			int num2 = this.bits.Length;
			while (--num2 >= 0)
			{
				num ^= (int)(this.bits[num2] * (uint)(num2 + 1));
			}
			return num ^ num;
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x00058588 File Offset: 0x00057588
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			BitSet bitSet = (BitSet)obj;
			int num = this.bits.Length;
			int num2 = bitSet.bits.Length;
			int num3 = ((num > num2) ? num2 : num);
			int num4 = num3;
			while (num4-- > 0)
			{
				if (this.bits[num4] != bitSet.bits[num4])
				{
					return false;
				}
			}
			if (num > num3)
			{
				int num5 = num;
				while (num5-- > num3)
				{
					if (this.bits[num5] != 0U)
					{
						return false;
					}
				}
			}
			else
			{
				int num6 = num2;
				while (num6-- > num3)
				{
					if (bitSet.bits[num6] != 0U)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x0005862C File Offset: 0x0005762C
		public BitSet Clone()
		{
			return new BitSet
			{
				count = this.count,
				bits = (uint[])this.bits.Clone()
			};
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x060014B2 RID: 5298 RVA: 0x00058664 File Offset: 0x00057664
		public bool IsEmpty
		{
			get
			{
				uint num = 0U;
				for (int i = 0; i < this.bits.Length; i++)
				{
					num |= this.bits[i];
				}
				return num == 0U;
			}
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x00058698 File Offset: 0x00057698
		public bool Intersects(BitSet other)
		{
			int num = Math.Min(this.bits.Length, other.bits.Length);
			while (--num >= 0)
			{
				if ((this.bits[num] & other.bits[num]) != 0U)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x000586DB File Offset: 0x000576DB
		private int Subscript(int bitIndex)
		{
			return bitIndex >> 5;
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x000586E0 File Offset: 0x000576E0
		private void EnsureLength(int nRequiredLength)
		{
			if (nRequiredLength > this.bits.Length)
			{
				int num = 2 * this.bits.Length;
				if (num < nRequiredLength)
				{
					num = nRequiredLength;
				}
				uint[] array = new uint[num];
				Array.Copy(this.bits, array, this.bits.Length);
				this.bits = array;
			}
		}

		// Token: 0x04000C7F RID: 3199
		private const int bitSlotShift = 5;

		// Token: 0x04000C80 RID: 3200
		private const int bitSlotMask = 31;

		// Token: 0x04000C81 RID: 3201
		private int count;

		// Token: 0x04000C82 RID: 3202
		private uint[] bits;
	}
}
