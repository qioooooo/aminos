using System;

namespace System.IO.Compression
{
	// Token: 0x02000214 RID: 532
	internal class OutputWindow
	{
		// Token: 0x06001201 RID: 4609 RVA: 0x0003CA64 File Offset: 0x0003BA64
		public void Write(byte b)
		{
			this.window[this.end++] = b;
			this.end &= 32767;
			this.bytesUsed++;
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x0003CAAC File Offset: 0x0003BAAC
		public void WriteLengthDistance(int length, int distance)
		{
			this.bytesUsed += length;
			int num = (this.end - distance) & 32767;
			int num2 = 32768 - length;
			if (num > num2 || this.end >= num2)
			{
				while (length-- > 0)
				{
					this.window[this.end++] = this.window[num++];
					this.end &= 32767;
					num &= 32767;
				}
				return;
			}
			if (length <= distance)
			{
				Array.Copy(this.window, num, this.window, this.end, length);
				this.end += length;
				return;
			}
			while (length-- > 0)
			{
				this.window[this.end++] = this.window[num++];
			}
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x0003CB94 File Offset: 0x0003BB94
		public int CopyFrom(InputBuffer input, int length)
		{
			length = Math.Min(Math.Min(length, 32768 - this.bytesUsed), input.AvailableBytes);
			int num = 32768 - this.end;
			int num2;
			if (length > num)
			{
				num2 = input.CopyTo(this.window, this.end, num);
				if (num2 == num)
				{
					num2 += input.CopyTo(this.window, 0, length - num);
				}
			}
			else
			{
				num2 = input.CopyTo(this.window, this.end, length);
			}
			this.end = (this.end + num2) & 32767;
			this.bytesUsed += num2;
			return num2;
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06001204 RID: 4612 RVA: 0x0003CC35 File Offset: 0x0003BC35
		public int FreeBytes
		{
			get
			{
				return 32768 - this.bytesUsed;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06001205 RID: 4613 RVA: 0x0003CC43 File Offset: 0x0003BC43
		public int AvailableBytes
		{
			get
			{
				return this.bytesUsed;
			}
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x0003CC4C File Offset: 0x0003BC4C
		public int CopyTo(byte[] output, int offset, int length)
		{
			int num;
			if (length > this.bytesUsed)
			{
				num = this.end;
				length = this.bytesUsed;
			}
			else
			{
				num = (this.end - this.bytesUsed + length) & 32767;
			}
			int num2 = length;
			int num3 = length - num;
			if (num3 > 0)
			{
				Array.Copy(this.window, 32768 - num3, output, offset, num3);
				offset += num3;
				length = num;
			}
			Array.Copy(this.window, num - length, output, offset, length);
			this.bytesUsed -= num2;
			return num2;
		}

		// Token: 0x04001075 RID: 4213
		private const int WindowSize = 32768;

		// Token: 0x04001076 RID: 4214
		private const int WindowMask = 32767;

		// Token: 0x04001077 RID: 4215
		private byte[] window = new byte[32768];

		// Token: 0x04001078 RID: 4216
		private int end;

		// Token: 0x04001079 RID: 4217
		private int bytesUsed;
	}
}
