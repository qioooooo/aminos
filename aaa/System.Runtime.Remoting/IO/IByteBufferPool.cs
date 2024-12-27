using System;

namespace System.IO
{
	// Token: 0x02000008 RID: 8
	internal interface IByteBufferPool
	{
		// Token: 0x0600001C RID: 28
		byte[] GetBuffer();

		// Token: 0x0600001D RID: 29
		void ReturnBuffer(byte[] buffer);
	}
}
