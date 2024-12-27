using System;

namespace System.Web
{
	// Token: 0x02000013 RID: 19
	internal class IntPtrArrayAllocator : BufferAllocator
	{
		// Token: 0x0600002F RID: 47 RVA: 0x0000271F File Offset: 0x0000171F
		internal IntPtrArrayAllocator(int arraySize, int maxFree)
			: base(maxFree)
		{
			this._arraySize = arraySize;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000272F File Offset: 0x0000172F
		protected override object AllocBuffer()
		{
			return new IntPtr[this._arraySize];
		}

		// Token: 0x04000D01 RID: 3329
		private int _arraySize;
	}
}
