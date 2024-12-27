using System;
using System.ComponentModel;

namespace System.Drawing.Printing
{
	// Token: 0x02000121 RID: 289
	public class PrintEventArgs : CancelEventArgs
	{
		// Token: 0x06000F36 RID: 3894 RVA: 0x0002D8EE File Offset: 0x0002C8EE
		public PrintEventArgs()
		{
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x0002D8F6 File Offset: 0x0002C8F6
		internal PrintEventArgs(PrintAction action)
		{
			this.printAction = action;
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000F38 RID: 3896 RVA: 0x0002D905 File Offset: 0x0002C905
		public PrintAction PrintAction
		{
			get
			{
				return this.printAction;
			}
		}

		// Token: 0x04000C66 RID: 3174
		private PrintAction printAction;
	}
}
