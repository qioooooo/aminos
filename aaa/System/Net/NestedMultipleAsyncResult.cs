using System;

namespace System.Net
{
	// Token: 0x020004F2 RID: 1266
	internal class NestedMultipleAsyncResult : LazyAsyncResult
	{
		// Token: 0x0600279F RID: 10143 RVA: 0x000A30F0 File Offset: 0x000A20F0
		internal NestedMultipleAsyncResult(object asyncObject, object asyncState, AsyncCallback asyncCallback, BufferOffsetSize[] buffers)
			: base(asyncObject, asyncState, asyncCallback)
		{
			this.Buffers = buffers;
			this.Size = 0;
			for (int i = 0; i < this.Buffers.Length; i++)
			{
				this.Size += this.Buffers[i].Size;
			}
		}

		// Token: 0x040026BE RID: 9918
		internal BufferOffsetSize[] Buffers;

		// Token: 0x040026BF RID: 9919
		internal int Size;
	}
}
