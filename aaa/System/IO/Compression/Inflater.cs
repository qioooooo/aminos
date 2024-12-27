using System;

namespace System.IO.Compression
{
	// Token: 0x02000211 RID: 529
	internal class Inflater
	{
		// Token: 0x060011E5 RID: 4581 RVA: 0x0003BA8C File Offset: 0x0003AA8C
		public Inflater(bool doGZip)
		{
			this.using_gzip = doGZip;
			this.output = new OutputWindow();
			this.input = new InputBuffer();
			this.gZipDecoder = new GZipDecoder(this.input);
			this.codeList = new byte[320];
			this.codeLengthTreeCodeLength = new byte[19];
			this.Reset();
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x0003BAFC File Offset: 0x0003AAFC
		public void Reset()
		{
			if (this.using_gzip)
			{
				this.gZipDecoder.Reset();
				this.state = InflaterState.ReadingGZIPHeader;
				this.streamSize = 0U;
				this.crc32 = 0U;
				return;
			}
			this.state = InflaterState.ReadingBFinal;
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x0003BB2E File Offset: 0x0003AB2E
		public void SetInput(byte[] inputBytes, int offset, int length)
		{
			this.input.SetInput(inputBytes, offset, length);
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x0003BB3E File Offset: 0x0003AB3E
		public bool Finished()
		{
			return this.state == InflaterState.Done || this.state == InflaterState.VerifyingGZIPFooter;
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x060011E9 RID: 4585 RVA: 0x0003BB56 File Offset: 0x0003AB56
		public int AvailableOutput
		{
			get
			{
				return this.output.AvailableBytes;
			}
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0003BB63 File Offset: 0x0003AB63
		public bool NeedsInput()
		{
			return this.input.NeedsInput();
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x0003BB70 File Offset: 0x0003AB70
		public int Inflate(byte[] bytes, int offset, int length)
		{
			int num = 0;
			for (;;)
			{
				int num2 = this.output.CopyTo(bytes, offset, length);
				if (num2 > 0)
				{
					if (this.using_gzip)
					{
						this.crc32 = DecodeHelper.UpdateCrc32(this.crc32, bytes, offset, num2);
						uint num3 = this.streamSize + (uint)num2;
						if (num3 < this.streamSize)
						{
							break;
						}
						this.streamSize = num3;
					}
					offset += num2;
					num += num2;
					length -= num2;
				}
				if (length == 0 || this.Finished() || !this.Decode())
				{
					goto IL_007B;
				}
			}
			throw new InvalidDataException(SR.GetString("StreamSizeOverflow"));
			IL_007B:
			if (this.state == InflaterState.VerifyingGZIPFooter && this.output.AvailableBytes == 0)
			{
				if (this.crc32 != this.gZipDecoder.Crc32)
				{
					throw new InvalidDataException(SR.GetString("InvalidCRC"));
				}
				if (this.streamSize != this.gZipDecoder.StreamSize)
				{
					throw new InvalidDataException(SR.GetString("InvalidStreamSize"));
				}
			}
			return num;
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x0003BC58 File Offset: 0x0003AC58
		private bool Decode()
		{
			bool flag = false;
			if (this.Finished())
			{
				return true;
			}
			if (this.using_gzip)
			{
				if (this.state == InflaterState.ReadingGZIPHeader)
				{
					if (!this.gZipDecoder.ReadGzipHeader())
					{
						return false;
					}
					this.state = InflaterState.ReadingBFinal;
				}
				else if (this.state == InflaterState.StartReadingGZIPFooter || this.state == InflaterState.ReadingGZIPFooter)
				{
					if (!this.gZipDecoder.ReadGzipFooter())
					{
						return false;
					}
					this.state = InflaterState.VerifyingGZIPFooter;
					return true;
				}
			}
			if (this.state == InflaterState.ReadingBFinal)
			{
				if (!this.input.EnsureBitsAvailable(1))
				{
					return false;
				}
				this.bfinal = this.input.GetBits(1);
				this.state = InflaterState.ReadingBType;
			}
			if (this.state == InflaterState.ReadingBType)
			{
				if (!this.input.EnsureBitsAvailable(2))
				{
					this.state = InflaterState.ReadingBType;
					return false;
				}
				this.blockType = (BlockType)this.input.GetBits(2);
				if (this.blockType == BlockType.Dynamic)
				{
					this.state = InflaterState.ReadingNumLitCodes;
				}
				else if (this.blockType == BlockType.Static)
				{
					this.literalLengthTree = HuffmanTree.StaticLiteralLengthTree;
					this.distanceTree = HuffmanTree.StaticDistanceTree;
					this.state = InflaterState.DecodeTop;
				}
				else
				{
					if (this.blockType != BlockType.Uncompressed)
					{
						throw new InvalidDataException(SR.GetString("UnknownBlockType"));
					}
					this.state = InflaterState.UncompressedAligning;
				}
			}
			bool flag2;
			if (this.blockType == BlockType.Dynamic)
			{
				if (this.state < InflaterState.DecodeTop)
				{
					flag2 = this.DecodeDynamicBlockHeader();
				}
				else
				{
					flag2 = this.DecodeBlock(out flag);
				}
			}
			else if (this.blockType == BlockType.Static)
			{
				flag2 = this.DecodeBlock(out flag);
			}
			else
			{
				if (this.blockType != BlockType.Uncompressed)
				{
					throw new InvalidDataException(SR.GetString("UnknownBlockType"));
				}
				flag2 = this.DecodeUncompressedBlock(out flag);
			}
			if (flag && this.bfinal != 0)
			{
				if (this.using_gzip)
				{
					this.state = InflaterState.StartReadingGZIPFooter;
				}
				else
				{
					this.state = InflaterState.Done;
				}
			}
			return flag2;
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x0003BE10 File Offset: 0x0003AE10
		private bool DecodeUncompressedBlock(out bool end_of_block)
		{
			end_of_block = false;
			for (;;)
			{
				switch (this.state)
				{
				case InflaterState.UncompressedAligning:
					this.input.SkipToByteBoundary();
					this.state = InflaterState.UncompressedByte1;
					goto IL_0043;
				case InflaterState.UncompressedByte1:
				case InflaterState.UncompressedByte2:
				case InflaterState.UncompressedByte3:
				case InflaterState.UncompressedByte4:
					goto IL_0043;
				case InflaterState.DecodingUncompressed:
					goto IL_00D6;
				}
				break;
				IL_0043:
				int bits = this.input.GetBits(8);
				if (bits < 0)
				{
					return false;
				}
				this.blockLengthBuffer[this.state - InflaterState.UncompressedByte1] = (byte)bits;
				if (this.state == InflaterState.UncompressedByte4)
				{
					this.blockLength = (int)this.blockLengthBuffer[0] + (int)this.blockLengthBuffer[1] * 256;
					int num = (int)this.blockLengthBuffer[2] + (int)this.blockLengthBuffer[3] * 256;
					if ((ushort)this.blockLength != (ushort)(~(ushort)num))
					{
						goto Block_4;
					}
				}
				this.state++;
			}
			throw new InvalidDataException(SR.GetString("UnknownState"));
			Block_4:
			throw new InvalidDataException(SR.GetString("InvalidBlockLength"));
			IL_00D6:
			int num2 = this.output.CopyFrom(this.input, this.blockLength);
			this.blockLength -= num2;
			if (this.blockLength == 0)
			{
				this.state = InflaterState.ReadingBFinal;
				end_of_block = true;
				return true;
			}
			return this.output.FreeBytes == 0;
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x0003BF50 File Offset: 0x0003AF50
		private bool DecodeBlock(out bool end_of_block_code_seen)
		{
			end_of_block_code_seen = false;
			int i = this.output.FreeBytes;
			while (i > 258)
			{
				switch (this.state)
				{
				case InflaterState.DecodeTop:
				{
					int num = this.literalLengthTree.GetNextSymbol(this.input);
					if (num < 0)
					{
						return false;
					}
					if (num < 256)
					{
						this.output.Write((byte)num);
						i--;
						continue;
					}
					if (num == 256)
					{
						end_of_block_code_seen = true;
						this.state = InflaterState.ReadingBFinal;
						return true;
					}
					num -= 257;
					if (num < 8)
					{
						num += 3;
						this.extraBits = 0;
					}
					else if (num == 28)
					{
						num = 258;
						this.extraBits = 0;
					}
					else
					{
						this.extraBits = (int)Inflater.extraLengthBits[num];
					}
					this.length = num;
					goto IL_00C6;
				}
				case InflaterState.HaveInitialLength:
					goto IL_00C6;
				case InflaterState.HaveFullLength:
					goto IL_010B;
				case InflaterState.HaveDistCode:
					break;
				default:
					throw new InvalidDataException(SR.GetString("UnknownState"));
				}
				IL_016D:
				int num2;
				if (this.distanceCode > 3)
				{
					this.extraBits = this.distanceCode - 2 >> 1;
					int bits = this.input.GetBits(this.extraBits);
					if (bits < 0)
					{
						return false;
					}
					num2 = Inflater.distanceBasePosition[this.distanceCode] + bits;
				}
				else
				{
					num2 = this.distanceCode + 1;
				}
				this.output.WriteLengthDistance(this.length, num2);
				i -= this.length;
				this.state = InflaterState.DecodeTop;
				continue;
				IL_010B:
				if (this.blockType == BlockType.Dynamic)
				{
					this.distanceCode = this.distanceTree.GetNextSymbol(this.input);
				}
				else
				{
					this.distanceCode = this.input.GetBits(5);
					if (this.distanceCode >= 0)
					{
						this.distanceCode = (int)Inflater.staticDistanceTreeTable[this.distanceCode];
					}
				}
				if (this.distanceCode < 0)
				{
					return false;
				}
				this.state = InflaterState.HaveDistCode;
				goto IL_016D;
				IL_00C6:
				if (this.extraBits > 0)
				{
					this.state = InflaterState.HaveInitialLength;
					int bits2 = this.input.GetBits(this.extraBits);
					if (bits2 < 0)
					{
						return false;
					}
					this.length = Inflater.lengthBase[this.length] + bits2;
				}
				this.state = InflaterState.HaveFullLength;
				goto IL_010B;
			}
			return true;
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x0003C15C File Offset: 0x0003B15C
		private bool DecodeDynamicBlockHeader()
		{
			switch (this.state)
			{
			case InflaterState.ReadingNumLitCodes:
				this.literalLengthCodeCount = this.input.GetBits(5);
				if (this.literalLengthCodeCount < 0)
				{
					return false;
				}
				this.literalLengthCodeCount += 257;
				this.state = InflaterState.ReadingNumDistCodes;
				goto IL_0064;
			case InflaterState.ReadingNumDistCodes:
				goto IL_0064;
			case InflaterState.ReadingNumCodeLengthCodes:
				goto IL_0096;
			case InflaterState.ReadingCodeLengthCodes:
				break;
			case InflaterState.ReadingTreeCodesBefore:
			case InflaterState.ReadingTreeCodesAfter:
				goto IL_0315;
			default:
				throw new InvalidDataException(SR.GetString("UnknownState"));
			}
			IL_0107:
			while (this.loopCounter < this.codeLengthCodeCount)
			{
				int bits = this.input.GetBits(3);
				if (bits < 0)
				{
					return false;
				}
				this.codeLengthTreeCodeLength[(int)Inflater.codeOrder[this.loopCounter]] = (byte)bits;
				this.loopCounter++;
			}
			for (int i = this.codeLengthCodeCount; i < Inflater.codeOrder.Length; i++)
			{
				this.codeLengthTreeCodeLength[(int)Inflater.codeOrder[i]] = 0;
			}
			this.codeLengthTree = new HuffmanTree(this.codeLengthTreeCodeLength);
			this.codeArraySize = this.literalLengthCodeCount + this.distanceCodeCount;
			this.loopCounter = 0;
			this.state = InflaterState.ReadingTreeCodesBefore;
			IL_0315:
			while (this.loopCounter < this.codeArraySize)
			{
				if (this.state == InflaterState.ReadingTreeCodesBefore && (this.lengthCode = this.codeLengthTree.GetNextSymbol(this.input)) < 0)
				{
					return false;
				}
				if (this.lengthCode <= 15)
				{
					this.codeList[this.loopCounter++] = (byte)this.lengthCode;
				}
				else
				{
					if (!this.input.EnsureBitsAvailable(7))
					{
						this.state = InflaterState.ReadingTreeCodesAfter;
						return false;
					}
					if (this.lengthCode == 16)
					{
						if (this.loopCounter == 0)
						{
							throw new InvalidDataException();
						}
						byte b = this.codeList[this.loopCounter - 1];
						int num = this.input.GetBits(2) + 3;
						if (this.loopCounter + num > this.codeArraySize)
						{
							throw new InvalidDataException();
						}
						for (int j = 0; j < num; j++)
						{
							this.codeList[this.loopCounter++] = b;
						}
					}
					else if (this.lengthCode == 17)
					{
						int num = this.input.GetBits(3) + 3;
						if (this.loopCounter + num > this.codeArraySize)
						{
							throw new InvalidDataException();
						}
						for (int k = 0; k < num; k++)
						{
							this.codeList[this.loopCounter++] = 0;
						}
					}
					else
					{
						int num = this.input.GetBits(7) + 11;
						if (this.loopCounter + num > this.codeArraySize)
						{
							throw new InvalidDataException();
						}
						for (int l = 0; l < num; l++)
						{
							this.codeList[this.loopCounter++] = 0;
						}
					}
				}
				this.state = InflaterState.ReadingTreeCodesBefore;
			}
			byte[] array = new byte[288];
			byte[] array2 = new byte[32];
			Array.Copy(this.codeList, array, this.literalLengthCodeCount);
			Array.Copy(this.codeList, this.literalLengthCodeCount, array2, 0, this.distanceCodeCount);
			if (array[256] == 0)
			{
				throw new InvalidDataException();
			}
			this.literalLengthTree = new HuffmanTree(array);
			this.distanceTree = new HuffmanTree(array2);
			this.state = InflaterState.DecodeTop;
			return true;
			IL_0064:
			this.distanceCodeCount = this.input.GetBits(5);
			if (this.distanceCodeCount < 0)
			{
				return false;
			}
			this.distanceCodeCount++;
			this.state = InflaterState.ReadingNumCodeLengthCodes;
			IL_0096:
			this.codeLengthCodeCount = this.input.GetBits(4);
			if (this.codeLengthCodeCount < 0)
			{
				return false;
			}
			this.codeLengthCodeCount += 4;
			this.loopCounter = 0;
			this.state = InflaterState.ReadingCodeLengthCodes;
			goto IL_0107;
		}

		// Token: 0x04001052 RID: 4178
		private static readonly byte[] extraLengthBits = new byte[]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
			1, 1, 2, 2, 2, 2, 3, 3, 3, 3,
			4, 4, 4, 4, 5, 5, 5, 5, 0
		};

		// Token: 0x04001053 RID: 4179
		private static readonly int[] lengthBase = new int[]
		{
			3, 4, 5, 6, 7, 8, 9, 10, 11, 13,
			15, 17, 19, 23, 27, 31, 35, 43, 51, 59,
			67, 83, 99, 115, 131, 163, 195, 227, 258
		};

		// Token: 0x04001054 RID: 4180
		private static readonly int[] distanceBasePosition = new int[]
		{
			1, 2, 3, 4, 5, 7, 9, 13, 17, 25,
			33, 49, 65, 97, 129, 193, 257, 385, 513, 769,
			1025, 1537, 2049, 3073, 4097, 6145, 8193, 12289, 16385, 24577,
			0, 0
		};

		// Token: 0x04001055 RID: 4181
		private static readonly byte[] codeOrder = new byte[]
		{
			16, 17, 18, 0, 8, 7, 9, 6, 10, 5,
			11, 4, 12, 3, 13, 2, 14, 1, 15
		};

		// Token: 0x04001056 RID: 4182
		private static readonly byte[] staticDistanceTreeTable = new byte[]
		{
			0, 16, 8, 24, 4, 20, 12, 28, 2, 18,
			10, 26, 6, 22, 14, 30, 1, 17, 9, 25,
			5, 21, 13, 29, 3, 19, 11, 27, 7, 23,
			15, 31
		};

		// Token: 0x04001057 RID: 4183
		private OutputWindow output;

		// Token: 0x04001058 RID: 4184
		private InputBuffer input;

		// Token: 0x04001059 RID: 4185
		private HuffmanTree literalLengthTree;

		// Token: 0x0400105A RID: 4186
		private HuffmanTree distanceTree;

		// Token: 0x0400105B RID: 4187
		private InflaterState state;

		// Token: 0x0400105C RID: 4188
		private bool using_gzip;

		// Token: 0x0400105D RID: 4189
		private int bfinal;

		// Token: 0x0400105E RID: 4190
		private BlockType blockType;

		// Token: 0x0400105F RID: 4191
		private uint crc32;

		// Token: 0x04001060 RID: 4192
		private uint streamSize;

		// Token: 0x04001061 RID: 4193
		private byte[] blockLengthBuffer = new byte[4];

		// Token: 0x04001062 RID: 4194
		private int blockLength;

		// Token: 0x04001063 RID: 4195
		private int length;

		// Token: 0x04001064 RID: 4196
		private int distanceCode;

		// Token: 0x04001065 RID: 4197
		private int extraBits;

		// Token: 0x04001066 RID: 4198
		private int loopCounter;

		// Token: 0x04001067 RID: 4199
		private int literalLengthCodeCount;

		// Token: 0x04001068 RID: 4200
		private int distanceCodeCount;

		// Token: 0x04001069 RID: 4201
		private int codeLengthCodeCount;

		// Token: 0x0400106A RID: 4202
		private int codeArraySize;

		// Token: 0x0400106B RID: 4203
		private int lengthCode;

		// Token: 0x0400106C RID: 4204
		private byte[] codeList;

		// Token: 0x0400106D RID: 4205
		private byte[] codeLengthTreeCodeLength;

		// Token: 0x0400106E RID: 4206
		private HuffmanTree codeLengthTree;

		// Token: 0x0400106F RID: 4207
		private GZipDecoder gZipDecoder;
	}
}
