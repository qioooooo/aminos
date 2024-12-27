using System;

namespace System.Net
{
	// Token: 0x02000412 RID: 1042
	internal class StreamChunkBytes : IReadChunkBytes
	{
		// Token: 0x060020C9 RID: 8393 RVA: 0x0008114C File Offset: 0x0008014C
		public StreamChunkBytes(ConnectStream connectStream)
		{
			this.ChunkStream = connectStream;
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x060020CA RID: 8394 RVA: 0x0008115B File Offset: 0x0008015B
		// (set) Token: 0x060020CB RID: 8395 RVA: 0x0008117E File Offset: 0x0008017E
		public int NextByte
		{
			get
			{
				if (this.HavePush)
				{
					this.HavePush = false;
					return (int)this.PushByte;
				}
				return this.ChunkStream.ReadSingleByte();
			}
			set
			{
				this.PushByte = (byte)value;
				this.HavePush = true;
			}
		}

		// Token: 0x040020CE RID: 8398
		public ConnectStream ChunkStream;

		// Token: 0x040020CF RID: 8399
		public int BytesRead;

		// Token: 0x040020D0 RID: 8400
		public int TotalBytesRead;

		// Token: 0x040020D1 RID: 8401
		private byte PushByte;

		// Token: 0x040020D2 RID: 8402
		private bool HavePush;
	}
}
