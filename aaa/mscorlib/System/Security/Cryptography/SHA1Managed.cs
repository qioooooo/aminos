using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000896 RID: 2198
	[ComVisible(true)]
	public class SHA1Managed : SHA1
	{
		// Token: 0x06005051 RID: 20561 RVA: 0x0011FB94 File Offset: 0x0011EB94
		public SHA1Managed()
		{
			if (Utils.FipsAlgorithmPolicy == 1)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Cryptography_NonCompliantFIPSAlgorithm"));
			}
			this._stateSHA1 = new uint[5];
			this._buffer = new byte[64];
			this._expandedBuffer = new uint[80];
			this.InitializeState();
		}

		// Token: 0x06005052 RID: 20562 RVA: 0x0011FBEB File Offset: 0x0011EBEB
		public override void Initialize()
		{
			this.InitializeState();
			Array.Clear(this._buffer, 0, this._buffer.Length);
			Array.Clear(this._expandedBuffer, 0, this._expandedBuffer.Length);
		}

		// Token: 0x06005053 RID: 20563 RVA: 0x0011FC1B File Offset: 0x0011EC1B
		protected override void HashCore(byte[] rgb, int ibStart, int cbSize)
		{
			this._HashData(rgb, ibStart, cbSize);
		}

		// Token: 0x06005054 RID: 20564 RVA: 0x0011FC26 File Offset: 0x0011EC26
		protected override byte[] HashFinal()
		{
			return this._EndHash();
		}

		// Token: 0x06005055 RID: 20565 RVA: 0x0011FC30 File Offset: 0x0011EC30
		private void InitializeState()
		{
			this._count = 0L;
			this._stateSHA1[0] = 1732584193U;
			this._stateSHA1[1] = 4023233417U;
			this._stateSHA1[2] = 2562383102U;
			this._stateSHA1[3] = 271733878U;
			this._stateSHA1[4] = 3285377520U;
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x0011FC88 File Offset: 0x0011EC88
		private unsafe void _HashData(byte[] partIn, int ibStart, int cbSize)
		{
			int i = cbSize;
			int num = ibStart;
			int num2 = (int)(this._count & 63L);
			this._count += (long)i;
			fixed (uint* stateSHA = this._stateSHA1)
			{
				fixed (byte* buffer = this._buffer)
				{
					fixed (uint* expandedBuffer = this._expandedBuffer)
					{
						if (num2 > 0 && num2 + i >= 64)
						{
							Buffer.InternalBlockCopy(partIn, num, this._buffer, num2, 64 - num2);
							num += 64 - num2;
							i -= 64 - num2;
							SHA1Managed.SHATransform(expandedBuffer, stateSHA, buffer);
							num2 = 0;
						}
						while (i >= 64)
						{
							Buffer.InternalBlockCopy(partIn, num, this._buffer, 0, 64);
							num += 64;
							i -= 64;
							SHA1Managed.SHATransform(expandedBuffer, stateSHA, buffer);
						}
						if (i > 0)
						{
							Buffer.InternalBlockCopy(partIn, num, this._buffer, num2, i);
						}
					}
				}
			}
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x0011FD9C File Offset: 0x0011ED9C
		private byte[] _EndHash()
		{
			byte[] array = new byte[20];
			int num = 64 - (int)(this._count & 63L);
			if (num <= 8)
			{
				num += 64;
			}
			byte[] array2 = new byte[num];
			array2[0] = 128;
			long num2 = this._count * 8L;
			array2[num - 8] = (byte)((num2 >> 56) & 255L);
			array2[num - 7] = (byte)((num2 >> 48) & 255L);
			array2[num - 6] = (byte)((num2 >> 40) & 255L);
			array2[num - 5] = (byte)((num2 >> 32) & 255L);
			array2[num - 4] = (byte)((num2 >> 24) & 255L);
			array2[num - 3] = (byte)((num2 >> 16) & 255L);
			array2[num - 2] = (byte)((num2 >> 8) & 255L);
			array2[num - 1] = (byte)(num2 & 255L);
			this._HashData(array2, 0, array2.Length);
			Utils.DWORDToBigEndian(array, this._stateSHA1, 5);
			this.HashValue = array;
			return array;
		}

		// Token: 0x06005058 RID: 20568 RVA: 0x0011FE88 File Offset: 0x0011EE88
		private unsafe static void SHATransform(uint* expandedBuffer, uint* state, byte* block)
		{
			uint num = *state;
			uint num2 = state[1];
			uint num3 = state[2];
			uint num4 = state[3];
			uint num5 = state[4];
			Utils.DWORDFromBigEndian(expandedBuffer, 16, block);
			SHA1Managed.SHAExpand(expandedBuffer);
			int i;
			for (i = 0; i < 20; i += 5)
			{
				num5 += ((num << 5) | (num >> 27)) + (num4 ^ (num2 & (num3 ^ num4))) + expandedBuffer[i] + 1518500249U;
				num2 = (num2 << 30) | (num2 >> 2);
				num4 += ((num5 << 5) | (num5 >> 27)) + (num3 ^ (num & (num2 ^ num3))) + expandedBuffer[i + 1] + 1518500249U;
				num = (num << 30) | (num >> 2);
				num3 += ((num4 << 5) | (num4 >> 27)) + (num2 ^ (num5 & (num ^ num2))) + expandedBuffer[i + 2] + 1518500249U;
				num5 = (num5 << 30) | (num5 >> 2);
				num2 += ((num3 << 5) | (num3 >> 27)) + (num ^ (num4 & (num5 ^ num))) + expandedBuffer[i + 3] + 1518500249U;
				num4 = (num4 << 30) | (num4 >> 2);
				num += ((num2 << 5) | (num2 >> 27)) + (num5 ^ (num3 & (num4 ^ num5))) + expandedBuffer[i + 4] + 1518500249U;
				num3 = (num3 << 30) | (num3 >> 2);
			}
			while (i < 40)
			{
				num5 += ((num << 5) | (num >> 27)) + (num2 ^ num3 ^ num4) + expandedBuffer[i] + 1859775393U;
				num2 = (num2 << 30) | (num2 >> 2);
				num4 += ((num5 << 5) | (num5 >> 27)) + (num ^ num2 ^ num3) + expandedBuffer[i + 1] + 1859775393U;
				num = (num << 30) | (num >> 2);
				num3 += ((num4 << 5) | (num4 >> 27)) + (num5 ^ num ^ num2) + expandedBuffer[i + 2] + 1859775393U;
				num5 = (num5 << 30) | (num5 >> 2);
				num2 += ((num3 << 5) | (num3 >> 27)) + (num4 ^ num5 ^ num) + expandedBuffer[i + 3] + 1859775393U;
				num4 = (num4 << 30) | (num4 >> 2);
				num += ((num2 << 5) | (num2 >> 27)) + (num3 ^ num4 ^ num5) + expandedBuffer[i + 4] + 1859775393U;
				num3 = (num3 << 30) | (num3 >> 2);
				i += 5;
			}
			while (i < 60)
			{
				num5 += ((num << 5) | (num >> 27)) + ((num2 & num3) | (num4 & (num2 | num3))) + expandedBuffer[i] + 2400959708U;
				num2 = (num2 << 30) | (num2 >> 2);
				num4 += ((num5 << 5) | (num5 >> 27)) + ((num & num2) | (num3 & (num | num2))) + expandedBuffer[i + 1] + 2400959708U;
				num = (num << 30) | (num >> 2);
				num3 += ((num4 << 5) | (num4 >> 27)) + ((num5 & num) | (num2 & (num5 | num))) + expandedBuffer[i + 2] + 2400959708U;
				num5 = (num5 << 30) | (num5 >> 2);
				num2 += ((num3 << 5) | (num3 >> 27)) + ((num4 & num5) | (num & (num4 | num5))) + expandedBuffer[i + 3] + 2400959708U;
				num4 = (num4 << 30) | (num4 >> 2);
				num += ((num2 << 5) | (num2 >> 27)) + ((num3 & num4) | (num5 & (num3 | num4))) + expandedBuffer[i + 4] + 2400959708U;
				num3 = (num3 << 30) | (num3 >> 2);
				i += 5;
			}
			while (i < 80)
			{
				num5 += ((num << 5) | (num >> 27)) + (num2 ^ num3 ^ num4) + expandedBuffer[i] + 3395469782U;
				num2 = (num2 << 30) | (num2 >> 2);
				num4 += ((num5 << 5) | (num5 >> 27)) + (num ^ num2 ^ num3) + expandedBuffer[i + 1] + 3395469782U;
				num = (num << 30) | (num >> 2);
				num3 += ((num4 << 5) | (num4 >> 27)) + (num5 ^ num ^ num2) + expandedBuffer[i + 2] + 3395469782U;
				num5 = (num5 << 30) | (num5 >> 2);
				num2 += ((num3 << 5) | (num3 >> 27)) + (num4 ^ num5 ^ num) + expandedBuffer[i + 3] + 3395469782U;
				num4 = (num4 << 30) | (num4 >> 2);
				num += ((num2 << 5) | (num2 >> 27)) + (num3 ^ num4 ^ num5) + expandedBuffer[i + 4] + 3395469782U;
				num3 = (num3 << 30) | (num3 >> 2);
				i += 5;
			}
			*state += num;
			state[1] += num2;
			state[2] += num3;
			state[3] += num4;
			state[4] += num5;
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x001202E0 File Offset: 0x0011F2E0
		private unsafe static void SHAExpand(uint* x)
		{
			for (int i = 16; i < 80; i++)
			{
				uint num = x[i - 3] ^ x[i - 8] ^ x[i - 14] ^ x[i - 16];
				x[i] = (num << 1) | (num >> 31);
			}
		}

		// Token: 0x04002926 RID: 10534
		private byte[] _buffer;

		// Token: 0x04002927 RID: 10535
		private long _count;

		// Token: 0x04002928 RID: 10536
		private uint[] _stateSHA1;

		// Token: 0x04002929 RID: 10537
		private uint[] _expandedBuffer;
	}
}
