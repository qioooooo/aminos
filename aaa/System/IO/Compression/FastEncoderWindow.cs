using System;
using System.Diagnostics;

namespace System.IO.Compression
{
	// Token: 0x0200020A RID: 522
	internal class FastEncoderWindow
	{
		// Token: 0x060011B7 RID: 4535 RVA: 0x0003AB34 File Offset: 0x00039B34
		public FastEncoderWindow()
		{
			this.window = new byte[16646];
			this.prev = new ushort[8450];
			this.lookup = new ushort[2048];
			this.bufPos = 8192;
			this.bufEnd = this.bufPos;
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x060011B8 RID: 4536 RVA: 0x0003AB8E File Offset: 0x00039B8E
		public int BytesAvailable
		{
			get
			{
				return this.bufEnd - this.bufPos;
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x060011B9 RID: 4537 RVA: 0x0003AB9D File Offset: 0x00039B9D
		public int FreeWindowSpace
		{
			get
			{
				return 16384 - this.bufEnd;
			}
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x0003ABAB File Offset: 0x00039BAB
		public void CopyBytes(byte[] inputBuffer, int startIndex, int count)
		{
			Array.Copy(inputBuffer, startIndex, this.window, this.bufEnd, count);
			this.bufEnd += count;
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0003ABD0 File Offset: 0x00039BD0
		public void MoveWindows()
		{
			Array.Copy(this.window, this.bufPos - 8192, this.window, 0, 8192);
			for (int i = 0; i < 2048; i++)
			{
				int num = (int)(this.lookup[i] - 8192);
				if (num <= 0)
				{
					this.lookup[i] = 0;
				}
				else
				{
					this.lookup[i] = (ushort)num;
				}
			}
			for (int i = 0; i < 8192; i++)
			{
				long num2 = (long)((ulong)this.prev[i] - 8192UL);
				if (num2 <= 0L)
				{
					this.prev[i] = 0;
				}
				else
				{
					this.prev[i] = (ushort)num2;
				}
			}
			this.bufPos = 8192;
			this.bufEnd = this.bufPos;
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x0003AC8A File Offset: 0x00039C8A
		private uint HashValue(uint hash, byte b)
		{
			return (hash << 4) ^ (uint)b;
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0003AC94 File Offset: 0x00039C94
		private uint InsertString(ref uint hash)
		{
			hash = this.HashValue(hash, this.window[this.bufPos + 2]);
			uint num = (uint)this.lookup[(int)((UIntPtr)(hash & 2047U))];
			this.lookup[(int)((UIntPtr)(hash & 2047U))] = (ushort)this.bufPos;
			this.prev[this.bufPos & 8191] = (ushort)num;
			return num;
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0003ACF8 File Offset: 0x00039CF8
		private void InsertStrings(ref uint hash, int matchLen)
		{
			if (this.bufEnd - this.bufPos <= matchLen)
			{
				this.bufPos += matchLen - 1;
				return;
			}
			while (--matchLen > 0)
			{
				this.InsertString(ref hash);
				this.bufPos++;
			}
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0003AD48 File Offset: 0x00039D48
		internal bool GetNextSymbolOrMatch(Match match)
		{
			uint num = this.HashValue(0U, this.window[this.bufPos]);
			num = this.HashValue(num, this.window[this.bufPos + 1]);
			int num2 = 0;
			int num3;
			if (this.bufEnd - this.bufPos <= 3)
			{
				num3 = 0;
			}
			else
			{
				int num4 = (int)this.InsertString(ref num);
				if (num4 != 0)
				{
					num3 = this.FindMatch(num4, out num2, 32, 32);
					if (this.bufPos + num3 > this.bufEnd)
					{
						num3 = this.bufEnd - this.bufPos;
					}
				}
				else
				{
					num3 = 0;
				}
			}
			if (num3 < 3)
			{
				match.State = MatchState.HasSymbol;
				match.Symbol = this.window[this.bufPos];
				this.bufPos++;
			}
			else
			{
				this.bufPos++;
				if (num3 <= 6)
				{
					int num5 = 0;
					int num6 = (int)this.InsertString(ref num);
					int num7;
					if (num6 != 0)
					{
						num7 = this.FindMatch(num6, out num5, (num3 < 4) ? 32 : 8, 32);
						if (this.bufPos + num7 > this.bufEnd)
						{
							num7 = this.bufEnd - this.bufPos;
						}
					}
					else
					{
						num7 = 0;
					}
					if (num7 > num3)
					{
						match.State = MatchState.HasSymbolAndMatch;
						match.Symbol = this.window[this.bufPos - 1];
						match.Position = num5;
						match.Length = num7;
						this.bufPos++;
						num3 = num7;
						this.InsertStrings(ref num, num3);
					}
					else
					{
						match.State = MatchState.HasMatch;
						match.Position = num2;
						match.Length = num3;
						num3--;
						this.bufPos++;
						this.InsertStrings(ref num, num3);
					}
				}
				else
				{
					match.State = MatchState.HasMatch;
					match.Position = num2;
					match.Length = num3;
					this.InsertStrings(ref num, num3);
				}
			}
			if (this.bufPos == 16384)
			{
				this.MoveWindows();
			}
			return true;
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0003AF18 File Offset: 0x00039F18
		private int FindMatch(int search, out int matchPos, int searchDepth, int niceLength)
		{
			int num = 0;
			int num2 = 0;
			int num3 = this.bufPos - 8192;
			byte b = this.window[this.bufPos];
			while (search > num3)
			{
				if (this.window[search + num] == b)
				{
					int num4 = 0;
					while (num4 < 258 && this.window[this.bufPos + num4] == this.window[search + num4])
					{
						num4++;
					}
					if (num4 > num)
					{
						num = num4;
						num2 = search;
						if (num4 > 32)
						{
							break;
						}
						b = this.window[this.bufPos + num4];
					}
				}
				if (--searchDepth == 0)
				{
					break;
				}
				search = (int)this.prev[search & 8191];
			}
			matchPos = this.bufPos - num2 - 1;
			if (num == 3 && matchPos >= 16384)
			{
				return 0;
			}
			return num;
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0003AFE0 File Offset: 0x00039FE0
		[Conditional("DEBUG")]
		private void VerifyHashes()
		{
			for (int i = 0; i < 2048; i++)
			{
				ushort num = this.lookup[i];
				while (num != 0 && this.bufPos - (int)num < 8192)
				{
					ushort num2 = this.prev[(int)(num & 8191)];
					if (this.bufPos - (int)num2 >= 8192)
					{
						break;
					}
					num = num2;
				}
			}
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0003B03A File Offset: 0x0003A03A
		private uint RecalculateHash(int position)
		{
			return (uint)((((int)this.window[position] << 8) ^ ((int)this.window[position + 1] << 4) ^ (int)this.window[position + 2]) & 2047);
		}

		// Token: 0x04000FF9 RID: 4089
		private const int FastEncoderHashShift = 4;

		// Token: 0x04000FFA RID: 4090
		private const int FastEncoderHashtableSize = 2048;

		// Token: 0x04000FFB RID: 4091
		private const int FastEncoderHashMask = 2047;

		// Token: 0x04000FFC RID: 4092
		private const int FastEncoderWindowSize = 8192;

		// Token: 0x04000FFD RID: 4093
		private const int FastEncoderWindowMask = 8191;

		// Token: 0x04000FFE RID: 4094
		private const int FastEncoderMatch3DistThreshold = 16384;

		// Token: 0x04000FFF RID: 4095
		internal const int MaxMatch = 258;

		// Token: 0x04001000 RID: 4096
		internal const int MinMatch = 3;

		// Token: 0x04001001 RID: 4097
		private const int SearchDepth = 32;

		// Token: 0x04001002 RID: 4098
		private const int GoodLength = 4;

		// Token: 0x04001003 RID: 4099
		private const int NiceLength = 32;

		// Token: 0x04001004 RID: 4100
		private const int LazyMatchThreshold = 6;

		// Token: 0x04001005 RID: 4101
		private byte[] window;

		// Token: 0x04001006 RID: 4102
		private int bufPos;

		// Token: 0x04001007 RID: 4103
		private int bufEnd;

		// Token: 0x04001008 RID: 4104
		private ushort[] prev;

		// Token: 0x04001009 RID: 4105
		private ushort[] lookup;
	}
}
