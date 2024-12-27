using System;

namespace System.IO.Compression
{
	// Token: 0x02000212 RID: 530
	internal class InputBuffer
	{
		// Token: 0x17000399 RID: 921
		// (get) Token: 0x060011F1 RID: 4593 RVA: 0x0003C6E8 File Offset: 0x0003B6E8
		public int AvailableBits
		{
			get
			{
				return this.bitsInBuffer;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x060011F2 RID: 4594 RVA: 0x0003C6F0 File Offset: 0x0003B6F0
		public int AvailableBytes
		{
			get
			{
				return this.end - this.start + this.bitsInBuffer / 8;
			}
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x0003C708 File Offset: 0x0003B708
		public bool EnsureBitsAvailable(int count)
		{
			if (this.bitsInBuffer < count)
			{
				if (this.NeedsInput())
				{
					return false;
				}
				this.bitBuffer |= (uint)((uint)this.buffer[this.start++] << (this.bitsInBuffer & 31));
				this.bitsInBuffer += 8;
				if (this.bitsInBuffer < count)
				{
					if (this.NeedsInput())
					{
						return false;
					}
					this.bitBuffer |= (uint)((uint)this.buffer[this.start++] << (this.bitsInBuffer & 31));
					this.bitsInBuffer += 8;
				}
			}
			return true;
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x0003C7BC File Offset: 0x0003B7BC
		public uint TryLoad16Bits()
		{
			if (this.bitsInBuffer < 8)
			{
				if (this.start < this.end)
				{
					this.bitBuffer |= (uint)((uint)this.buffer[this.start++] << (this.bitsInBuffer & 31));
					this.bitsInBuffer += 8;
				}
				if (this.start < this.end)
				{
					this.bitBuffer |= (uint)((uint)this.buffer[this.start++] << (this.bitsInBuffer & 31));
					this.bitsInBuffer += 8;
				}
			}
			else if (this.bitsInBuffer < 16 && this.start < this.end)
			{
				this.bitBuffer |= (uint)((uint)this.buffer[this.start++] << (this.bitsInBuffer & 31));
				this.bitsInBuffer += 8;
			}
			return this.bitBuffer;
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0003C8CB File Offset: 0x0003B8CB
		private uint GetBitMask(int count)
		{
			return (1U << count) - 1U;
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x0003C8D8 File Offset: 0x0003B8D8
		public int GetBits(int count)
		{
			if (!this.EnsureBitsAvailable(count))
			{
				return -1;
			}
			int num = (int)(this.bitBuffer & this.GetBitMask(count));
			this.bitBuffer >>= count;
			this.bitsInBuffer -= count;
			return num;
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0003C920 File Offset: 0x0003B920
		public int CopyTo(byte[] output, int offset, int length)
		{
			int num = 0;
			while (this.bitsInBuffer > 0 && length > 0)
			{
				output[offset++] = (byte)this.bitBuffer;
				this.bitBuffer >>= 8;
				this.bitsInBuffer -= 8;
				length--;
				num++;
			}
			if (length == 0)
			{
				return num;
			}
			int num2 = this.end - this.start;
			if (length > num2)
			{
				length = num2;
			}
			Array.Copy(this.buffer, this.start, output, offset, length);
			this.start += length;
			return num + length;
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x0003C9B1 File Offset: 0x0003B9B1
		public bool NeedsInput()
		{
			return this.start == this.end;
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0003C9C1 File Offset: 0x0003B9C1
		public void SetInput(byte[] buffer, int offset, int length)
		{
			this.buffer = buffer;
			this.start = offset;
			this.end = offset + length;
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x0003C9DA File Offset: 0x0003B9DA
		public void SkipBits(int n)
		{
			this.bitBuffer >>= n;
			this.bitsInBuffer -= n;
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x0003C9FB File Offset: 0x0003B9FB
		public void SkipToByteBoundary()
		{
			this.bitBuffer >>= this.bitsInBuffer % 8;
			this.bitsInBuffer -= this.bitsInBuffer % 8;
		}

		// Token: 0x04001070 RID: 4208
		private byte[] buffer;

		// Token: 0x04001071 RID: 4209
		private int start;

		// Token: 0x04001072 RID: 4210
		private int end;

		// Token: 0x04001073 RID: 4211
		private uint bitBuffer;

		// Token: 0x04001074 RID: 4212
		private int bitsInBuffer;
	}
}
