using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000410 RID: 1040
	public class FormClosingEventArgs : CancelEventArgs
	{
		// Token: 0x06003E49 RID: 15945 RVA: 0x000E310D File Offset: 0x000E210D
		public FormClosingEventArgs(CloseReason closeReason, bool cancel)
			: base(cancel)
		{
			this.closeReason = closeReason;
		}

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x06003E4A RID: 15946 RVA: 0x000E311D File Offset: 0x000E211D
		public CloseReason CloseReason
		{
			get
			{
				return this.closeReason;
			}
		}

		// Token: 0x04001EB1 RID: 7857
		private CloseReason closeReason;
	}
}
