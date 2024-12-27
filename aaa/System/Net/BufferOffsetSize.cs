using System;

namespace System.Net
{
	// Token: 0x020004B7 RID: 1207
	internal class BufferOffsetSize
	{
		// Token: 0x06002556 RID: 9558 RVA: 0x00094EC8 File Offset: 0x00093EC8
		internal BufferOffsetSize(byte[] buffer, int offset, int size, bool copyBuffer)
		{
			if (copyBuffer)
			{
				byte[] array = new byte[size];
				global::System.Buffer.BlockCopy(buffer, offset, array, 0, size);
				offset = 0;
				buffer = array;
			}
			this.Buffer = buffer;
			this.Offset = offset;
			this.Size = size;
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x00094F0B File Offset: 0x00093F0B
		internal BufferOffsetSize(byte[] buffer, bool copyBuffer)
			: this(buffer, 0, buffer.Length, copyBuffer)
		{
		}

		// Token: 0x04002518 RID: 9496
		internal byte[] Buffer;

		// Token: 0x04002519 RID: 9497
		internal int Offset;

		// Token: 0x0400251A RID: 9498
		internal int Size;
	}
}
