using System;

namespace System.IO.Compression
{
	// Token: 0x02000204 RID: 516
	internal class FastEncoder
	{
		// Token: 0x06001191 RID: 4497 RVA: 0x0003980F File Offset: 0x0003880F
		public FastEncoder(bool doGZip)
		{
			this.usingGzip = doGZip;
			this.inputWindow = new FastEncoderWindow();
			this.inputBuffer = new DeflateInput();
			this.output = new FastEncoder.Output();
			this.currentMatch = new Match();
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x0003984A File Offset: 0x0003884A
		public void SetInput(byte[] input, int startIndex, int count)
		{
			this.inputBuffer.Buffer = input;
			this.inputBuffer.Count = count;
			this.inputBuffer.StartIndex = startIndex;
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x00039870 File Offset: 0x00038870
		public bool NeedsInput()
		{
			return this.inputBuffer.Count == 0 && this.inputWindow.BytesAvailable == 0;
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x00039890 File Offset: 0x00038890
		public int GetCompressedOutput(byte[] outputBuffer)
		{
			this.output.UpdateBuffer(outputBuffer);
			if (this.usingGzip && !this.hasGzipHeader)
			{
				this.output.WriteGzipHeader(3);
				this.hasGzipHeader = true;
			}
			if (!this.hasBlockHeader)
			{
				this.hasBlockHeader = true;
				this.output.WritePreamble();
			}
			for (;;)
			{
				int num = ((this.inputBuffer.Count < this.inputWindow.FreeWindowSpace) ? this.inputBuffer.Count : this.inputWindow.FreeWindowSpace);
				if (num > 0)
				{
					this.inputWindow.CopyBytes(this.inputBuffer.Buffer, this.inputBuffer.StartIndex, num);
					if (this.usingGzip)
					{
						this.gzipCrc32 = DecodeHelper.UpdateCrc32(this.gzipCrc32, this.inputBuffer.Buffer, this.inputBuffer.StartIndex, num);
						uint num2 = this.inputStreamSize + (uint)num;
						if (num2 < this.inputStreamSize)
						{
							break;
						}
						this.inputStreamSize = num2;
					}
					this.inputBuffer.ConsumeBytes(num);
				}
				while (this.inputWindow.BytesAvailable > 0 && this.output.SafeToWriteTo())
				{
					this.inputWindow.GetNextSymbolOrMatch(this.currentMatch);
					if (this.currentMatch.State == MatchState.HasSymbol)
					{
						this.output.WriteChar(this.currentMatch.Symbol);
					}
					else if (this.currentMatch.State == MatchState.HasMatch)
					{
						this.output.WriteMatch(this.currentMatch.Length, this.currentMatch.Position);
					}
					else
					{
						this.output.WriteChar(this.currentMatch.Symbol);
						this.output.WriteMatch(this.currentMatch.Length, this.currentMatch.Position);
					}
				}
				if (!this.output.SafeToWriteTo() || this.NeedsInput())
				{
					goto IL_01E3;
				}
			}
			throw new InvalidDataException(SR.GetString("StreamSizeOverflow"));
			IL_01E3:
			this.needsEOB = true;
			return this.output.BytesWritten;
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x00039A94 File Offset: 0x00038A94
		public int Finish(byte[] outputBuffer)
		{
			this.output.UpdateBuffer(outputBuffer);
			if (this.needsEOB)
			{
				uint num = FastEncoderStatics.FastEncoderLiteralCodeInfo[256];
				int num2 = (int)(num & 31U);
				this.output.WriteBits(num2, num >> 5);
				this.output.FlushBits();
				if (this.usingGzip)
				{
					this.output.WriteGzipFooter(this.gzipCrc32, this.inputStreamSize);
				}
			}
			return this.output.BytesWritten;
		}

		// Token: 0x04000FD6 RID: 4054
		private bool hasBlockHeader;

		// Token: 0x04000FD7 RID: 4055
		private bool hasGzipHeader;

		// Token: 0x04000FD8 RID: 4056
		private bool usingGzip;

		// Token: 0x04000FD9 RID: 4057
		private uint gzipCrc32;

		// Token: 0x04000FDA RID: 4058
		private uint inputStreamSize;

		// Token: 0x04000FDB RID: 4059
		private FastEncoderWindow inputWindow;

		// Token: 0x04000FDC RID: 4060
		private DeflateInput inputBuffer;

		// Token: 0x04000FDD RID: 4061
		private FastEncoder.Output output;

		// Token: 0x04000FDE RID: 4062
		private Match currentMatch;

		// Token: 0x04000FDF RID: 4063
		private bool needsEOB;

		// Token: 0x02000205 RID: 517
		internal class Output
		{
			// Token: 0x06001196 RID: 4502 RVA: 0x00039B0A File Offset: 0x00038B0A
			static Output()
			{
				FastEncoder.Output.GenerateSlotTables();
			}

			// Token: 0x06001197 RID: 4503 RVA: 0x00039B20 File Offset: 0x00038B20
			internal static void GenerateSlotTables()
			{
				int num = 0;
				int i;
				for (i = 0; i < 16; i++)
				{
					for (int j = 0; j < 1 << (int)FastEncoderStatics.ExtraDistanceBits[i]; j++)
					{
						FastEncoder.Output.distLookup[num++] = (byte)i;
					}
				}
				num >>= 7;
				while (i < 30)
				{
					for (int k = 0; k < 1 << (int)(FastEncoderStatics.ExtraDistanceBits[i] - 7); k++)
					{
						FastEncoder.Output.distLookup[256 + num++] = (byte)i;
					}
					i++;
				}
			}

			// Token: 0x06001198 RID: 4504 RVA: 0x00039B9B File Offset: 0x00038B9B
			internal void UpdateBuffer(byte[] output)
			{
				this.outputBuf = output;
				this.outputPos = 0;
			}

			// Token: 0x06001199 RID: 4505 RVA: 0x00039BAB File Offset: 0x00038BAB
			internal bool SafeToWriteTo()
			{
				return this.outputBuf.Length - this.outputPos > 16;
			}

			// Token: 0x17000383 RID: 899
			// (get) Token: 0x0600119A RID: 4506 RVA: 0x00039BC0 File Offset: 0x00038BC0
			internal int BytesWritten
			{
				get
				{
					return this.outputPos;
				}
			}

			// Token: 0x17000384 RID: 900
			// (get) Token: 0x0600119B RID: 4507 RVA: 0x00039BC8 File Offset: 0x00038BC8
			internal int FreeBytes
			{
				get
				{
					return this.outputBuf.Length - this.outputPos;
				}
			}

			// Token: 0x0600119C RID: 4508 RVA: 0x00039BDC File Offset: 0x00038BDC
			internal void WritePreamble()
			{
				Array.Copy(FastEncoderStatics.FastEncoderTreeStructureData, 0, this.outputBuf, this.outputPos, FastEncoderStatics.FastEncoderTreeStructureData.Length);
				this.outputPos += FastEncoderStatics.FastEncoderTreeStructureData.Length;
				this.bitCount = 9;
				this.bitBuf = 34U;
			}

			// Token: 0x0600119D RID: 4509 RVA: 0x00039C2C File Offset: 0x00038C2C
			internal void WriteMatch(int matchLen, int matchPos)
			{
				uint num = FastEncoderStatics.FastEncoderLiteralCodeInfo[254 + matchLen];
				int num2 = (int)(num & 31U);
				if (num2 <= 16)
				{
					this.WriteBits(num2, num >> 5);
				}
				else
				{
					this.WriteBits(16, (num >> 5) & 65535U);
					this.WriteBits(num2 - 16, num >> 21);
				}
				num = FastEncoderStatics.FastEncoderDistanceCodeInfo[this.GetSlot(matchPos)];
				this.WriteBits((int)(num & 15U), num >> 8);
				int num3 = (int)((num >> 4) & 15U);
				if (num3 != 0)
				{
					this.WriteBits(num3, (uint)(matchPos & (int)FastEncoderStatics.BitMask[num3]));
				}
			}

			// Token: 0x0600119E RID: 4510 RVA: 0x00039CB4 File Offset: 0x00038CB4
			internal void WriteGzipFooter(uint gzipCrc32, uint inputStreamSize)
			{
				this.outputBuf[this.outputPos++] = (byte)(gzipCrc32 & 255U);
				this.outputBuf[this.outputPos++] = (byte)((gzipCrc32 >> 8) & 255U);
				this.outputBuf[this.outputPos++] = (byte)((gzipCrc32 >> 16) & 255U);
				this.outputBuf[this.outputPos++] = (byte)((gzipCrc32 >> 24) & 255U);
				this.outputBuf[this.outputPos++] = (byte)(inputStreamSize & 255U);
				this.outputBuf[this.outputPos++] = (byte)((inputStreamSize >> 8) & 255U);
				this.outputBuf[this.outputPos++] = (byte)((inputStreamSize >> 16) & 255U);
				this.outputBuf[this.outputPos++] = (byte)((inputStreamSize >> 24) & 255U);
			}

			// Token: 0x0600119F RID: 4511 RVA: 0x00039DDC File Offset: 0x00038DDC
			internal void WriteGzipHeader(int compression_level)
			{
				this.outputBuf[this.outputPos++] = 31;
				this.outputBuf[this.outputPos++] = 139;
				this.outputBuf[this.outputPos++] = 8;
				this.outputBuf[this.outputPos++] = 0;
				this.outputBuf[this.outputPos++] = 0;
				this.outputBuf[this.outputPos++] = 0;
				this.outputBuf[this.outputPos++] = 0;
				this.outputBuf[this.outputPos++] = 0;
				if (compression_level == 10)
				{
					this.outputBuf[this.outputPos++] = 2;
				}
				else
				{
					this.outputBuf[this.outputPos++] = 4;
				}
				this.outputBuf[this.outputPos++] = 0;
			}

			// Token: 0x060011A0 RID: 4512 RVA: 0x00039F18 File Offset: 0x00038F18
			internal void WriteChar(byte b)
			{
				uint num = FastEncoderStatics.FastEncoderLiteralCodeInfo[(int)b];
				this.WriteBits((int)(num & 31U), num >> 5);
			}

			// Token: 0x060011A1 RID: 4513 RVA: 0x00039F3C File Offset: 0x00038F3C
			internal void WriteBits(int n, uint bits)
			{
				this.bitBuf |= bits << this.bitCount;
				this.bitCount += n;
				if (this.bitCount >= 16)
				{
					this.outputBuf[this.outputPos++] = (byte)this.bitBuf;
					this.outputBuf[this.outputPos++] = (byte)(this.bitBuf >> 8);
					this.bitCount -= 16;
					this.bitBuf >>= 16;
				}
			}

			// Token: 0x060011A2 RID: 4514 RVA: 0x00039FD7 File Offset: 0x00038FD7
			internal int GetSlot(int pos)
			{
				return (int)FastEncoder.Output.distLookup[(pos < 256) ? pos : (256 + (pos >> 7))];
			}

			// Token: 0x060011A3 RID: 4515 RVA: 0x00039FF4 File Offset: 0x00038FF4
			internal void FlushBits()
			{
				while (this.bitCount >= 8)
				{
					this.outputBuf[this.outputPos++] = (byte)this.bitBuf;
					this.bitCount -= 8;
					this.bitBuf >>= 8;
				}
				if (this.bitCount > 0)
				{
					this.outputBuf[this.outputPos++] = (byte)this.bitBuf;
					this.bitCount = 0;
				}
			}

			// Token: 0x04000FE0 RID: 4064
			private byte[] outputBuf;

			// Token: 0x04000FE1 RID: 4065
			private int outputPos;

			// Token: 0x04000FE2 RID: 4066
			private uint bitBuf;

			// Token: 0x04000FE3 RID: 4067
			private int bitCount;

			// Token: 0x04000FE4 RID: 4068
			private static byte[] distLookup = new byte[512];
		}
	}
}
