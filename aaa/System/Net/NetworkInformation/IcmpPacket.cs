using System;
using System.Diagnostics;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000625 RID: 1573
	internal class IcmpPacket
	{
		// Token: 0x06003066 RID: 12390 RVA: 0x000D157B File Offset: 0x000D057B
		internal IcmpPacket(byte[] buffer)
		{
			this.type = 8;
			this.buffer = buffer;
			ushort num = IcmpPacket.staticSequenceNumber;
			IcmpPacket.staticSequenceNumber = num + 1;
			this.sequenceNumber = num;
			this.checkSum = (ushort)this.GetCheckSum();
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06003067 RID: 12391 RVA: 0x000D15B2 File Offset: 0x000D05B2
		internal ushort Identifier
		{
			get
			{
				if (IcmpPacket.identifier == 0)
				{
					IcmpPacket.identifier = (ushort)Process.GetCurrentProcess().Id;
				}
				return IcmpPacket.identifier;
			}
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x000D15D0 File Offset: 0x000D05D0
		private uint GetCheckSum()
		{
			uint num = (uint)((ushort)this.type + this.Identifier + this.sequenceNumber);
			for (int i = 0; i < this.buffer.Length; i++)
			{
				num += (uint)((int)this.buffer[i] + ((int)this.buffer[++i] << 8));
			}
			num = (num >> 16) + (num & 65535U);
			num += num >> 16;
			return ~num;
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x000D1638 File Offset: 0x000D0638
		internal byte[] GetBytes()
		{
			byte[] array = new byte[this.buffer.Length + 8];
			byte[] bytes = BitConverter.GetBytes(this.checkSum);
			byte[] bytes2 = BitConverter.GetBytes(this.Identifier);
			byte[] bytes3 = BitConverter.GetBytes(this.sequenceNumber);
			array[0] = this.type;
			array[1] = this.subCode;
			Array.Copy(bytes, 0, array, 2, 2);
			Array.Copy(bytes2, 0, array, 4, 2);
			Array.Copy(bytes3, 0, array, 6, 2);
			Array.Copy(this.buffer, 0, array, 8, this.buffer.Length);
			return array;
		}

		// Token: 0x04002E14 RID: 11796
		private static ushort staticSequenceNumber;

		// Token: 0x04002E15 RID: 11797
		internal byte type;

		// Token: 0x04002E16 RID: 11798
		internal byte subCode;

		// Token: 0x04002E17 RID: 11799
		internal ushort checkSum;

		// Token: 0x04002E18 RID: 11800
		internal static ushort identifier;

		// Token: 0x04002E19 RID: 11801
		internal ushort sequenceNumber;

		// Token: 0x04002E1A RID: 11802
		internal byte[] buffer;
	}
}
