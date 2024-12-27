using System;

namespace System.Net
{
	// Token: 0x0200054D RID: 1357
	internal class WorkerAsyncResult : LazyAsyncResult
	{
		// Token: 0x0600292F RID: 10543 RVA: 0x000AC535 File Offset: 0x000AB535
		public WorkerAsyncResult(object asyncObject, object asyncState, AsyncCallback savedAsyncCallback, byte[] buffer, int offset, int end)
			: base(asyncObject, asyncState, savedAsyncCallback)
		{
			this.Buffer = buffer;
			this.Offset = offset;
			this.End = end;
		}

		// Token: 0x0400283C RID: 10300
		public byte[] Buffer;

		// Token: 0x0400283D RID: 10301
		public int Offset;

		// Token: 0x0400283E RID: 10302
		public int End;

		// Token: 0x0400283F RID: 10303
		public bool IsWrite;

		// Token: 0x04002840 RID: 10304
		public WorkerAsyncResult ParentResult;

		// Token: 0x04002841 RID: 10305
		public bool HeaderDone;

		// Token: 0x04002842 RID: 10306
		public bool HandshakeDone;
	}
}
