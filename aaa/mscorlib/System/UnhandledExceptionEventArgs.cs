using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000121 RID: 289
	[ComVisible(true)]
	[Serializable]
	public class UnhandledExceptionEventArgs : EventArgs
	{
		// Token: 0x060010F3 RID: 4339 RVA: 0x0002FA7A File Offset: 0x0002EA7A
		public UnhandledExceptionEventArgs(object exception, bool isTerminating)
		{
			this._Exception = exception;
			this._IsTerminating = isTerminating;
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060010F4 RID: 4340 RVA: 0x0002FA90 File Offset: 0x0002EA90
		public object ExceptionObject
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this._Exception;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x0002FA98 File Offset: 0x0002EA98
		public bool IsTerminating
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this._IsTerminating;
			}
		}

		// Token: 0x04000588 RID: 1416
		private object _Exception;

		// Token: 0x04000589 RID: 1417
		private bool _IsTerminating;
	}
}
