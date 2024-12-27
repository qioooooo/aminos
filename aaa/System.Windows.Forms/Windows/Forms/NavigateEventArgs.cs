using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200059D RID: 1437
	[ComVisible(true)]
	public class NavigateEventArgs : EventArgs
	{
		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x06004A6C RID: 19052 RVA: 0x0010E4E3 File Offset: 0x0010D4E3
		public bool Forward
		{
			get
			{
				return this.isForward;
			}
		}

		// Token: 0x06004A6D RID: 19053 RVA: 0x0010E4EB File Offset: 0x0010D4EB
		public NavigateEventArgs(bool isForward)
		{
			this.isForward = isForward;
		}

		// Token: 0x040030B3 RID: 12467
		private bool isForward = true;
	}
}
