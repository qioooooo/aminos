using System;

namespace System.Windows.Forms
{
	// Token: 0x020002B2 RID: 690
	public class ControlEventArgs : EventArgs
	{
		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x060025DF RID: 9695 RVA: 0x00058E62 File Offset: 0x00057E62
		public Control Control
		{
			get
			{
				return this.control;
			}
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x00058E6A File Offset: 0x00057E6A
		public ControlEventArgs(Control control)
		{
			this.control = control;
		}

		// Token: 0x04001608 RID: 5640
		private Control control;
	}
}
