using System;

namespace System.Net
{
	// Token: 0x02000532 RID: 1330
	internal class ScatterGatherBuffers
	{
		// Token: 0x060028A9 RID: 10409 RVA: 0x000A808A File Offset: 0x000A708A
		internal ScatterGatherBuffers()
		{
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x000A809D File Offset: 0x000A709D
		internal ScatterGatherBuffers(long totalSize)
		{
			if (totalSize > 0L)
			{
				this.currentChunk = this.AllocateMemoryChunk((totalSize > 2147483647L) ? int.MaxValue : ((int)totalSize));
			}
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x000A80D4 File Offset: 0x000A70D4
		internal BufferOffsetSize[] GetBuffers()
		{
			if (this.Empty)
			{
				return null;
			}
			BufferOffsetSize[] array = new BufferOffsetSize[this.chunkCount];
			int num = 0;
			for (ScatterGatherBuffers.MemoryChunk next = this.headChunk; next != null; next = next.Next)
			{
				array[num] = new BufferOffsetSize(next.Buffer, 0, next.FreeOffset, false);
				num++;
			}
			return array;
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x060028AC RID: 10412 RVA: 0x000A8127 File Offset: 0x000A7127
		private bool Empty
		{
			get
			{
				return this.headChunk == null || this.chunkCount == 0;
			}
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x060028AD RID: 10413 RVA: 0x000A813C File Offset: 0x000A713C
		internal int Length
		{
			get
			{
				return this.totalLength;
			}
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x000A8144 File Offset: 0x000A7144
		internal void Write(byte[] buffer, int offset, int count)
		{
			while (count > 0)
			{
				int num = (this.Empty ? 0 : (this.currentChunk.Buffer.Length - this.currentChunk.FreeOffset));
				if (num == 0)
				{
					ScatterGatherBuffers.MemoryChunk memoryChunk = this.AllocateMemoryChunk(count);
					if (this.currentChunk != null)
					{
						this.currentChunk.Next = memoryChunk;
					}
					this.currentChunk = memoryChunk;
				}
				int num2 = ((count < num) ? count : num);
				Buffer.BlockCopy(buffer, offset, this.currentChunk.Buffer, this.currentChunk.FreeOffset, num2);
				offset += num2;
				count -= num2;
				this.totalLength += num2;
				this.currentChunk.FreeOffset += num2;
			}
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x000A81FC File Offset: 0x000A71FC
		private ScatterGatherBuffers.MemoryChunk AllocateMemoryChunk(int newSize)
		{
			if (newSize > this.nextChunkLength)
			{
				this.nextChunkLength = newSize;
			}
			ScatterGatherBuffers.MemoryChunk memoryChunk = new ScatterGatherBuffers.MemoryChunk(this.nextChunkLength);
			if (this.Empty)
			{
				this.headChunk = memoryChunk;
			}
			this.nextChunkLength *= 2;
			this.chunkCount++;
			return memoryChunk;
		}

		// Token: 0x0400278F RID: 10127
		private ScatterGatherBuffers.MemoryChunk headChunk;

		// Token: 0x04002790 RID: 10128
		private ScatterGatherBuffers.MemoryChunk currentChunk;

		// Token: 0x04002791 RID: 10129
		private int nextChunkLength = 1024;

		// Token: 0x04002792 RID: 10130
		private int totalLength;

		// Token: 0x04002793 RID: 10131
		private int chunkCount;

		// Token: 0x02000533 RID: 1331
		private class MemoryChunk
		{
			// Token: 0x060028B0 RID: 10416 RVA: 0x000A8251 File Offset: 0x000A7251
			internal MemoryChunk(int bufferSize)
			{
				this.Buffer = new byte[bufferSize];
			}

			// Token: 0x04002794 RID: 10132
			internal byte[] Buffer;

			// Token: 0x04002795 RID: 10133
			internal int FreeOffset;

			// Token: 0x04002796 RID: 10134
			internal ScatterGatherBuffers.MemoryChunk Next;
		}
	}
}
