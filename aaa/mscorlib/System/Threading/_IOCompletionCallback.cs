using System;

namespace System.Threading
{
	// Token: 0x0200014B RID: 331
	internal class _IOCompletionCallback
	{
		// Token: 0x0600125F RID: 4703 RVA: 0x000339F1 File Offset: 0x000329F1
		internal _IOCompletionCallback(IOCompletionCallback ioCompletionCallback, ref StackCrawlMark stackMark)
		{
			this._ioCompletionCallback = ioCompletionCallback;
			this._executionContext = ExecutionContext.Capture(ref stackMark);
			ExecutionContext.ClearSyncContext(this._executionContext);
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x00033A18 File Offset: 0x00032A18
		internal static void IOCompletionCallback_Context(object state)
		{
			_IOCompletionCallback iocompletionCallback = (_IOCompletionCallback)state;
			iocompletionCallback._ioCompletionCallback(iocompletionCallback._errorCode, iocompletionCallback._numBytes, iocompletionCallback._pOVERLAP);
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x00033A4C File Offset: 0x00032A4C
		internal unsafe static void PerformIOCompletionCallback(uint errorCode, uint numBytes, NativeOverlapped* pOVERLAP)
		{
			do
			{
				Overlapped overlapped = OverlappedData.GetOverlappedFromNative(pOVERLAP).m_overlapped;
				_IOCompletionCallback iocbHelper = overlapped.iocbHelper;
				if (iocbHelper == null || iocbHelper._executionContext == null || iocbHelper._executionContext.IsDefaultFTContext())
				{
					IOCompletionCallback userCallback = overlapped.UserCallback;
					userCallback(errorCode, numBytes, pOVERLAP);
				}
				else
				{
					iocbHelper._errorCode = errorCode;
					iocbHelper._numBytes = numBytes;
					iocbHelper._pOVERLAP = pOVERLAP;
					ExecutionContext.Run(iocbHelper._executionContext.CreateCopy(), _IOCompletionCallback._ccb, iocbHelper);
				}
				OverlappedData.CheckVMForIOPacket(out pOVERLAP, out errorCode, out numBytes);
			}
			while (pOVERLAP != null);
		}

		// Token: 0x0400061C RID: 1564
		private IOCompletionCallback _ioCompletionCallback;

		// Token: 0x0400061D RID: 1565
		private ExecutionContext _executionContext;

		// Token: 0x0400061E RID: 1566
		private uint _errorCode;

		// Token: 0x0400061F RID: 1567
		private uint _numBytes;

		// Token: 0x04000620 RID: 1568
		private unsafe NativeOverlapped* _pOVERLAP;

		// Token: 0x04000621 RID: 1569
		internal static ContextCallback _ccb = new ContextCallback(_IOCompletionCallback.IOCompletionCallback_Context);
	}
}
