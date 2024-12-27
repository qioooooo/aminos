using System;

namespace System.IO
{
	// Token: 0x02000726 RID: 1830
	public class ErrorEventArgs : EventArgs
	{
		// Token: 0x060037D7 RID: 14295 RVA: 0x000EC17A File Offset: 0x000EB17A
		public ErrorEventArgs(Exception exception)
		{
			this.exception = exception;
		}

		// Token: 0x060037D8 RID: 14296 RVA: 0x000EC189 File Offset: 0x000EB189
		public virtual Exception GetException()
		{
			return this.exception;
		}

		// Token: 0x040031ED RID: 12781
		private Exception exception;
	}
}
