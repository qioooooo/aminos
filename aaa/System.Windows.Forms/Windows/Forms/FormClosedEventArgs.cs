using System;

namespace System.Windows.Forms
{
	// Token: 0x0200040E RID: 1038
	public class FormClosedEventArgs : EventArgs
	{
		// Token: 0x06003E43 RID: 15939 RVA: 0x000E30F6 File Offset: 0x000E20F6
		public FormClosedEventArgs(CloseReason closeReason)
		{
			this.closeReason = closeReason;
		}

		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x06003E44 RID: 15940 RVA: 0x000E3105 File Offset: 0x000E2105
		public CloseReason CloseReason
		{
			get
			{
				return this.closeReason;
			}
		}

		// Token: 0x04001EB0 RID: 7856
		private CloseReason closeReason;
	}
}
