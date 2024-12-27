using System;

namespace System.IO
{
	// Token: 0x02000009 RID: 9
	internal class ByteBufferAllocator : IByteBufferPool
	{
		// Token: 0x0600001E RID: 30 RVA: 0x0000256A File Offset: 0x0000156A
		public ByteBufferAllocator(int bufferSize)
		{
			this._bufferSize = bufferSize;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002579 File Offset: 0x00001579
		public byte[] GetBuffer()
		{
			return new byte[this._bufferSize];
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002586 File Offset: 0x00001586
		public void ReturnBuffer(byte[] buffer)
		{
		}

		// Token: 0x04000040 RID: 64
		private int _bufferSize;
	}
}
