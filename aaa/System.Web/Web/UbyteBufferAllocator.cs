using System;

namespace System.Web
{
	// Token: 0x02000010 RID: 16
	internal class UbyteBufferAllocator : BufferAllocator
	{
		// Token: 0x06000029 RID: 41 RVA: 0x000026C8 File Offset: 0x000016C8
		internal UbyteBufferAllocator(int bufferSize, int maxFree)
			: base(maxFree)
		{
			this._bufferSize = bufferSize;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000026D8 File Offset: 0x000016D8
		protected override object AllocBuffer()
		{
			return new byte[this._bufferSize];
		}

		// Token: 0x04000CFE RID: 3326
		private int _bufferSize;
	}
}
