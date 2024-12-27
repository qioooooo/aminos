using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x0200044E RID: 1102
	public class InvalidateEventArgs : EventArgs
	{
		// Token: 0x0600419A RID: 16794 RVA: 0x000EB17A File Offset: 0x000EA17A
		public InvalidateEventArgs(Rectangle invalidRect)
		{
			this.invalidRect = invalidRect;
		}

		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x0600419B RID: 16795 RVA: 0x000EB189 File Offset: 0x000EA189
		public Rectangle InvalidRect
		{
			get
			{
				return this.invalidRect;
			}
		}

		// Token: 0x04001F9D RID: 8093
		private readonly Rectangle invalidRect;
	}
}
