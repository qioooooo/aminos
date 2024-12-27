using System;
using System.IO;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x0200001D RID: 29
	internal class AsyncCopyStreamResult : BasicAsyncResult
	{
		// Token: 0x060000E1 RID: 225 RVA: 0x000054F4 File Offset: 0x000044F4
		internal AsyncCopyStreamResult(AsyncCallback callback, object state)
			: base(callback, state)
		{
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000054FE File Offset: 0x000044FE
		internal override void CleanupOnComplete()
		{
			if (this.Buffer != null)
			{
				CoreChannel.BufferPool.ReturnBuffer(this.Buffer);
			}
			if (this.CloseSource)
			{
				this.Source.Close();
			}
			if (this.CloseTarget)
			{
				this.Target.Close();
			}
		}

		// Token: 0x040000B1 RID: 177
		internal Stream Source;

		// Token: 0x040000B2 RID: 178
		internal Stream Target;

		// Token: 0x040000B3 RID: 179
		internal byte[] Buffer;

		// Token: 0x040000B4 RID: 180
		internal bool AsyncRead;

		// Token: 0x040000B5 RID: 181
		internal bool AsyncWrite;

		// Token: 0x040000B6 RID: 182
		internal bool CloseSource;

		// Token: 0x040000B7 RID: 183
		internal bool CloseTarget;
	}
}
