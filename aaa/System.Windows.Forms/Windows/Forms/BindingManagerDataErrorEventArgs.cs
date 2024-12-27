using System;

namespace System.Windows.Forms
{
	// Token: 0x02000241 RID: 577
	public class BindingManagerDataErrorEventArgs : EventArgs
	{
		// Token: 0x06001BA6 RID: 7078 RVA: 0x00035E09 File Offset: 0x00034E09
		public BindingManagerDataErrorEventArgs(Exception exception)
		{
			this.exception = exception;
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06001BA7 RID: 7079 RVA: 0x00035E18 File Offset: 0x00034E18
		public Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		// Token: 0x04001326 RID: 4902
		private Exception exception;
	}
}
