using System;

namespace System.Web
{
	// Token: 0x02000012 RID: 18
	internal class IntegerArrayAllocator : BufferAllocator
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00002702 File Offset: 0x00001702
		internal IntegerArrayAllocator(int arraySize, int maxFree)
			: base(maxFree)
		{
			this._arraySize = arraySize;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002712 File Offset: 0x00001712
		protected override object AllocBuffer()
		{
			return new int[this._arraySize];
		}

		// Token: 0x04000D00 RID: 3328
		private int _arraySize;
	}
}
