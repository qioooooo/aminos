using System;

namespace System.Web
{
	// Token: 0x02000011 RID: 17
	internal class CharBufferAllocator : BufferAllocator
	{
		// Token: 0x0600002B RID: 43 RVA: 0x000026E5 File Offset: 0x000016E5
		internal CharBufferAllocator(int bufferSize, int maxFree)
			: base(maxFree)
		{
			this._bufferSize = bufferSize;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000026F5 File Offset: 0x000016F5
		protected override object AllocBuffer()
		{
			return new char[this._bufferSize];
		}

		// Token: 0x04000CFF RID: 3327
		private int _bufferSize;
	}
}
