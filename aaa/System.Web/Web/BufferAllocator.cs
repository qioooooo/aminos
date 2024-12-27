using System;
using System.Collections;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x0200000F RID: 15
	internal abstract class BufferAllocator
	{
		// Token: 0x06000024 RID: 36 RVA: 0x000025AC File Offset: 0x000015AC
		static BufferAllocator()
		{
			if (BufferAllocator.s_ProcsFudgeFactor < 1)
			{
				BufferAllocator.s_ProcsFudgeFactor = 1;
			}
			if (BufferAllocator.s_ProcsFudgeFactor > 4)
			{
				BufferAllocator.s_ProcsFudgeFactor = 4;
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000025D4 File Offset: 0x000015D4
		internal BufferAllocator(int maxFree)
		{
			this._buffers = new Stack();
			this._numFree = 0;
			this._maxFree = maxFree * BufferAllocator.s_ProcsFudgeFactor;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000025FC File Offset: 0x000015FC
		internal object GetBuffer()
		{
			object obj = null;
			if (this._numFree > 0)
			{
				lock (this)
				{
					if (this._numFree > 0)
					{
						obj = this._buffers.Pop();
						this._numFree--;
					}
				}
			}
			if (obj == null)
			{
				obj = this.AllocBuffer();
			}
			return obj;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002664 File Offset: 0x00001664
		internal void ReuseBuffer(object buffer)
		{
			if (this._numFree < this._maxFree)
			{
				lock (this)
				{
					if (this._numFree < this._maxFree)
					{
						this._buffers.Push(buffer);
						this._numFree++;
					}
				}
			}
		}

		// Token: 0x06000028 RID: 40
		protected abstract object AllocBuffer();

		// Token: 0x04000CFA RID: 3322
		private int _maxFree;

		// Token: 0x04000CFB RID: 3323
		private int _numFree;

		// Token: 0x04000CFC RID: 3324
		private Stack _buffers;

		// Token: 0x04000CFD RID: 3325
		private static int s_ProcsFudgeFactor = SystemInfo.GetNumProcessCPUs();
	}
}
