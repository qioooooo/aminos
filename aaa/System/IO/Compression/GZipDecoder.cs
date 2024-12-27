using System;

namespace System.IO.Compression
{
	// Token: 0x0200020C RID: 524
	internal class GZipDecoder
	{
		// Token: 0x060011C3 RID: 4547 RVA: 0x0003B064 File Offset: 0x0003A064
		public GZipDecoder(InputBuffer input)
		{
			this.input = input;
			this.Reset();
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x0003B079 File Offset: 0x0003A079
		public void Reset()
		{
			this.gzipHeaderSubstate = GZIPHeaderState.ReadingID1;
			this.gzipFooterSubstate = GZIPHeaderState.ReadingCRC;
			this.gzipCrc32 = 0U;
			this.gzipOutputStreamSize = 0U;
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x060011C5 RID: 4549 RVA: 0x0003B098 File Offset: 0x0003A098
		public uint Crc32
		{
			get
			{
				return this.gzipCrc32;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x060011C6 RID: 4550 RVA: 0x0003B0A0 File Offset: 0x0003A0A0
		public uint StreamSize
		{
			get
			{
				return this.gzipOutputStreamSize;
			}
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x0003B0A8 File Offset: 0x0003A0A8
		public bool ReadGzipHeader()
		{
			int num;
			switch (this.gzipHeaderSubstate)
			{
			case GZIPHeaderState.ReadingID1:
				num = this.input.GetBits(8);
				if (num < 0)
				{
					return false;
				}
				if (num != 31)
				{
					throw new InvalidDataException(SR.GetString("CorruptedGZipHeader"));
				}
				this.gzipHeaderSubstate = GZIPHeaderState.ReadingID2;
				break;
			case GZIPHeaderState.ReadingID2:
				break;
			case GZIPHeaderState.ReadingCM:
				goto IL_00AF;
			case GZIPHeaderState.ReadingFLG:
				goto IL_00DD;
			case GZIPHeaderState.ReadingMMTime:
				goto IL_0105;
			case GZIPHeaderState.ReadingXFL:
				goto IL_0141;
			case GZIPHeaderState.ReadingOS:
				goto IL_015B;
			case GZIPHeaderState.ReadingXLen1:
				goto IL_0175;
			case GZIPHeaderState.ReadingXLen2:
				goto IL_01A3;
			case GZIPHeaderState.ReadingXLenData:
				goto IL_01D5;
			case GZIPHeaderState.ReadingFileName:
				goto IL_0217;
			case GZIPHeaderState.ReadingComment:
				goto IL_0249;
			case GZIPHeaderState.ReadingCRC16Part1:
				goto IL_027C;
			case GZIPHeaderState.ReadingCRC16Part2:
				goto IL_02AB;
			case GZIPHeaderState.Done:
				return true;
			default:
				throw new InvalidDataException(SR.GetString("UnknownState"));
			}
			num = this.input.GetBits(8);
			if (num < 0)
			{
				return false;
			}
			if (num != 139)
			{
				throw new InvalidDataException(SR.GetString("CorruptedGZipHeader"));
			}
			this.gzipHeaderSubstate = GZIPHeaderState.ReadingCM;
			IL_00AF:
			num = this.input.GetBits(8);
			if (num < 0)
			{
				return false;
			}
			if (num != 8)
			{
				throw new InvalidDataException(SR.GetString("UnknownCompressionMode"));
			}
			this.gzipHeaderSubstate = GZIPHeaderState.ReadingFLG;
			IL_00DD:
			num = this.input.GetBits(8);
			if (num < 0)
			{
				return false;
			}
			this.gzip_header_flag = num;
			this.gzipHeaderSubstate = GZIPHeaderState.ReadingMMTime;
			this.loopCounter = 0;
			IL_0105:
			while (this.loopCounter < 4)
			{
				num = this.input.GetBits(8);
				if (num < 0)
				{
					return false;
				}
				this.loopCounter++;
			}
			this.gzipHeaderSubstate = GZIPHeaderState.ReadingXFL;
			this.loopCounter = 0;
			IL_0141:
			num = this.input.GetBits(8);
			if (num < 0)
			{
				return false;
			}
			this.gzipHeaderSubstate = GZIPHeaderState.ReadingOS;
			IL_015B:
			num = this.input.GetBits(8);
			if (num < 0)
			{
				return false;
			}
			this.gzipHeaderSubstate = GZIPHeaderState.ReadingXLen1;
			IL_0175:
			if ((this.gzip_header_flag & 4) == 0)
			{
				goto IL_0217;
			}
			num = this.input.GetBits(8);
			if (num < 0)
			{
				return false;
			}
			this.gzip_header_xlen = num;
			this.gzipHeaderSubstate = GZIPHeaderState.ReadingXLen2;
			IL_01A3:
			num = this.input.GetBits(8);
			if (num < 0)
			{
				return false;
			}
			this.gzip_header_xlen |= num << 8;
			this.gzipHeaderSubstate = GZIPHeaderState.ReadingXLenData;
			this.loopCounter = 0;
			IL_01D5:
			while (this.loopCounter < this.gzip_header_xlen)
			{
				num = this.input.GetBits(8);
				if (num < 0)
				{
					return false;
				}
				this.loopCounter++;
			}
			this.gzipHeaderSubstate = GZIPHeaderState.ReadingFileName;
			this.loopCounter = 0;
			IL_0217:
			if ((this.gzip_header_flag & 8) == 0)
			{
				this.gzipHeaderSubstate = GZIPHeaderState.ReadingComment;
			}
			else
			{
				for (;;)
				{
					num = this.input.GetBits(8);
					if (num < 0)
					{
						break;
					}
					if (num == 0)
					{
						goto Block_20;
					}
				}
				return false;
				Block_20:
				this.gzipHeaderSubstate = GZIPHeaderState.ReadingComment;
			}
			IL_0249:
			if ((this.gzip_header_flag & 16) == 0)
			{
				this.gzipHeaderSubstate = GZIPHeaderState.ReadingCRC16Part1;
			}
			else
			{
				for (;;)
				{
					num = this.input.GetBits(8);
					if (num < 0)
					{
						break;
					}
					if (num == 0)
					{
						goto Block_23;
					}
				}
				return false;
				Block_23:
				this.gzipHeaderSubstate = GZIPHeaderState.ReadingCRC16Part1;
			}
			IL_027C:
			if ((this.gzip_header_flag & 2) == 0)
			{
				this.gzipHeaderSubstate = GZIPHeaderState.Done;
				return true;
			}
			num = this.input.GetBits(8);
			if (num < 0)
			{
				return false;
			}
			this.gzipHeaderSubstate = GZIPHeaderState.ReadingCRC16Part2;
			IL_02AB:
			num = this.input.GetBits(8);
			if (num < 0)
			{
				return false;
			}
			this.gzipHeaderSubstate = GZIPHeaderState.Done;
			return true;
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x0003B38C File Offset: 0x0003A38C
		public bool ReadGzipFooter()
		{
			this.input.SkipToByteBoundary();
			if (this.gzipFooterSubstate == GZIPHeaderState.ReadingCRC)
			{
				while (this.loopCounter < 4)
				{
					int bits = this.input.GetBits(8);
					if (bits < 0)
					{
						return false;
					}
					this.gzipCrc32 |= (uint)((uint)bits << 8 * this.loopCounter);
					this.loopCounter++;
				}
				this.gzipFooterSubstate = GZIPHeaderState.ReadingFileSize;
				this.loopCounter = 0;
			}
			if (this.gzipFooterSubstate == GZIPHeaderState.ReadingFileSize)
			{
				if (this.loopCounter == 0)
				{
					this.gzipOutputStreamSize = 0U;
				}
				while (this.loopCounter < 4)
				{
					int bits2 = this.input.GetBits(8);
					if (bits2 < 0)
					{
						return false;
					}
					this.gzipOutputStreamSize |= (uint)((uint)bits2 << 8 * this.loopCounter);
					this.loopCounter++;
				}
			}
			return true;
		}

		// Token: 0x0400101C RID: 4124
		private const int FileText = 1;

		// Token: 0x0400101D RID: 4125
		private const int CRCFlag = 2;

		// Token: 0x0400101E RID: 4126
		private const int ExtraFieldsFlag = 4;

		// Token: 0x0400101F RID: 4127
		private const int FileNameFlag = 8;

		// Token: 0x04001020 RID: 4128
		private const int CommentFlag = 16;

		// Token: 0x04001021 RID: 4129
		private InputBuffer input;

		// Token: 0x04001022 RID: 4130
		private GZIPHeaderState gzipHeaderSubstate;

		// Token: 0x04001023 RID: 4131
		private GZIPHeaderState gzipFooterSubstate;

		// Token: 0x04001024 RID: 4132
		private int gzip_header_flag;

		// Token: 0x04001025 RID: 4133
		private int gzip_header_xlen;

		// Token: 0x04001026 RID: 4134
		private uint gzipCrc32;

		// Token: 0x04001027 RID: 4135
		private uint gzipOutputStreamSize;

		// Token: 0x04001028 RID: 4136
		private int loopCounter;
	}
}
