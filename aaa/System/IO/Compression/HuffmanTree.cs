using System;

namespace System.IO.Compression
{
	// Token: 0x0200020E RID: 526
	internal class HuffmanTree
	{
		// Token: 0x17000396 RID: 918
		// (get) Token: 0x060011DD RID: 4573 RVA: 0x0003B6B7 File Offset: 0x0003A6B7
		public static HuffmanTree StaticLiteralLengthTree
		{
			get
			{
				return HuffmanTree.staticLiteralLengthTree;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x060011DE RID: 4574 RVA: 0x0003B6BE File Offset: 0x0003A6BE
		public static HuffmanTree StaticDistanceTree
		{
			get
			{
				return HuffmanTree.staticDistanceTree;
			}
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x0003B6C8 File Offset: 0x0003A6C8
		public HuffmanTree(byte[] codeLengths)
		{
			this.codeLengthArray = codeLengths;
			if (this.codeLengthArray.Length == 288)
			{
				this.tableBits = 9;
			}
			else
			{
				this.tableBits = 7;
			}
			this.tableMask = (1 << this.tableBits) - 1;
			this.CreateTable();
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x0003B71C File Offset: 0x0003A71C
		private static byte[] GetStaticLiteralTreeLength()
		{
			byte[] array = new byte[288];
			for (int i = 0; i <= 143; i++)
			{
				array[i] = 8;
			}
			for (int j = 144; j <= 255; j++)
			{
				array[j] = 9;
			}
			for (int k = 256; k <= 279; k++)
			{
				array[k] = 7;
			}
			for (int l = 280; l <= 287; l++)
			{
				array[l] = 8;
			}
			return array;
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x0003B798 File Offset: 0x0003A798
		private static byte[] GetStaticDistanceTreeLength()
		{
			byte[] array = new byte[32];
			for (int i = 0; i < 32; i++)
			{
				array[i] = 5;
			}
			return array;
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x0003B7C0 File Offset: 0x0003A7C0
		private uint[] CalculateHuffmanCode()
		{
			uint[] array = new uint[17];
			foreach (int num in this.codeLengthArray)
			{
				array[num] += 1U;
			}
			array[0] = 0U;
			uint[] array3 = new uint[17];
			uint num2 = 0U;
			for (int j = 1; j <= 16; j++)
			{
				num2 = num2 + array[j - 1] << 1;
				array3[j] = num2;
			}
			uint[] array4 = new uint[288];
			for (int k = 0; k < this.codeLengthArray.Length; k++)
			{
				int num3 = (int)this.codeLengthArray[k];
				if (num3 > 0)
				{
					array4[k] = DecodeHelper.BitReverse(array3[num3], num3);
					array3[num3] += 1U;
				}
			}
			return array4;
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x0003B894 File Offset: 0x0003A894
		private void CreateTable()
		{
			uint[] array = this.CalculateHuffmanCode();
			this.table = new short[1 << this.tableBits];
			this.left = new short[2 * this.codeLengthArray.Length];
			this.right = new short[2 * this.codeLengthArray.Length];
			short num = (short)this.codeLengthArray.Length;
			for (int i = 0; i < this.codeLengthArray.Length; i++)
			{
				int num2 = (int)this.codeLengthArray[i];
				if (num2 > 0)
				{
					int num3 = (int)array[i];
					if (num2 <= this.tableBits)
					{
						int num4 = 1 << num2;
						if (num3 >= num4)
						{
							throw new InvalidDataException(SR.GetString("InvalidHuffmanData"));
						}
						int num5 = 1 << this.tableBits - num2;
						for (int j = 0; j < num5; j++)
						{
							this.table[num3] = (short)i;
							num3 += num4;
						}
					}
					else
					{
						int num6 = num2 - this.tableBits;
						int num7 = 1 << this.tableBits;
						int num8 = num3 & ((1 << this.tableBits) - 1);
						short[] array2 = this.table;
						do
						{
							short num9 = array2[num8];
							if (num9 == 0)
							{
								array2[num8] = -num;
								num9 = -num;
								num += 1;
							}
							if ((num3 & num7) == 0)
							{
								array2 = this.left;
							}
							else
							{
								array2 = this.right;
							}
							num8 = (int)(-(int)num9);
							num7 <<= 1;
							num6--;
						}
						while (num6 != 0);
						array2[num8] = (short)i;
					}
				}
			}
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x0003BA04 File Offset: 0x0003AA04
		public int GetNextSymbol(InputBuffer input)
		{
			uint num = input.TryLoad16Bits();
			if (input.AvailableBits == 0)
			{
				return -1;
			}
			int num2;
			checked
			{
				num2 = (int)this.table[(int)((IntPtr)(unchecked((ulong)num & (ulong)((long)this.tableMask))))];
			}
			if (num2 < 0)
			{
				uint num3 = 1U << this.tableBits;
				do
				{
					num2 = -num2;
					if ((num & num3) == 0U)
					{
						num2 = (int)this.left[num2];
					}
					else
					{
						num2 = (int)this.right[num2];
					}
					num3 <<= 1;
				}
				while (num2 < 0);
			}
			if ((int)this.codeLengthArray[num2] > input.AvailableBits)
			{
				return -1;
			}
			input.SkipBits((int)this.codeLengthArray[num2]);
			return num2;
		}

		// Token: 0x0400102A RID: 4138
		internal const int MaxLiteralTreeElements = 288;

		// Token: 0x0400102B RID: 4139
		internal const int MaxDistTreeElements = 32;

		// Token: 0x0400102C RID: 4140
		internal const int EndOfBlockCode = 256;

		// Token: 0x0400102D RID: 4141
		internal const int NumberOfCodeLengthTreeElements = 19;

		// Token: 0x0400102E RID: 4142
		private int tableBits;

		// Token: 0x0400102F RID: 4143
		private short[] table;

		// Token: 0x04001030 RID: 4144
		private short[] left;

		// Token: 0x04001031 RID: 4145
		private short[] right;

		// Token: 0x04001032 RID: 4146
		private byte[] codeLengthArray;

		// Token: 0x04001033 RID: 4147
		private int tableMask;

		// Token: 0x04001034 RID: 4148
		private static HuffmanTree staticLiteralLengthTree = new HuffmanTree(HuffmanTree.GetStaticLiteralTreeLength());

		// Token: 0x04001035 RID: 4149
		private static HuffmanTree staticDistanceTree = new HuffmanTree(HuffmanTree.GetStaticDistanceTreeLength());
	}
}
