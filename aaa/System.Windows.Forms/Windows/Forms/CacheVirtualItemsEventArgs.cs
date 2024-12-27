using System;

namespace System.Windows.Forms
{
	// Token: 0x0200025E RID: 606
	public class CacheVirtualItemsEventArgs : EventArgs
	{
		// Token: 0x06001FC7 RID: 8135 RVA: 0x0004326F File Offset: 0x0004226F
		public CacheVirtualItemsEventArgs(int startIndex, int endIndex)
		{
			this.startIndex = startIndex;
			this.endIndex = endIndex;
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001FC8 RID: 8136 RVA: 0x00043285 File Offset: 0x00042285
		public int StartIndex
		{
			get
			{
				return this.startIndex;
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001FC9 RID: 8137 RVA: 0x0004328D File Offset: 0x0004228D
		public int EndIndex
		{
			get
			{
				return this.endIndex;
			}
		}

		// Token: 0x04001466 RID: 5222
		private int startIndex;

		// Token: 0x04001467 RID: 5223
		private int endIndex;
	}
}
