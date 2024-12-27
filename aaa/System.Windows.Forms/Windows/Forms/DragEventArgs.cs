using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020003C7 RID: 967
	[ComVisible(true)]
	public class DragEventArgs : EventArgs
	{
		// Token: 0x06003A97 RID: 14999 RVA: 0x000D521C File Offset: 0x000D421C
		public DragEventArgs(IDataObject data, int keyState, int x, int y, DragDropEffects allowedEffect, DragDropEffects effect)
		{
			this.data = data;
			this.keyState = keyState;
			this.x = x;
			this.y = y;
			this.allowedEffect = allowedEffect;
			this.effect = effect;
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x06003A98 RID: 15000 RVA: 0x000D5251 File Offset: 0x000D4251
		public IDataObject Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06003A99 RID: 15001 RVA: 0x000D5259 File Offset: 0x000D4259
		public int KeyState
		{
			get
			{
				return this.keyState;
			}
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06003A9A RID: 15002 RVA: 0x000D5261 File Offset: 0x000D4261
		public int X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06003A9B RID: 15003 RVA: 0x000D5269 File Offset: 0x000D4269
		public int Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06003A9C RID: 15004 RVA: 0x000D5271 File Offset: 0x000D4271
		public DragDropEffects AllowedEffect
		{
			get
			{
				return this.allowedEffect;
			}
		}

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06003A9D RID: 15005 RVA: 0x000D5279 File Offset: 0x000D4279
		// (set) Token: 0x06003A9E RID: 15006 RVA: 0x000D5281 File Offset: 0x000D4281
		public DragDropEffects Effect
		{
			get
			{
				return this.effect;
			}
			set
			{
				this.effect = value;
			}
		}

		// Token: 0x04001D35 RID: 7477
		private readonly IDataObject data;

		// Token: 0x04001D36 RID: 7478
		private readonly int keyState;

		// Token: 0x04001D37 RID: 7479
		private readonly int x;

		// Token: 0x04001D38 RID: 7480
		private readonly int y;

		// Token: 0x04001D39 RID: 7481
		private readonly DragDropEffects allowedEffect;

		// Token: 0x04001D3A RID: 7482
		private DragDropEffects effect;
	}
}
