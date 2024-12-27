using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Proxies
{
	// Token: 0x02000752 RID: 1874
	internal class AgileAsyncWorkerItem
	{
		// Token: 0x06004311 RID: 17169 RVA: 0x000E602D File Offset: 0x000E502D
		public AgileAsyncWorkerItem(IMethodCallMessage message, AsyncResult ar, object target)
		{
			this._message = new MethodCall(message);
			this._ar = ar;
			this._target = target;
		}

		// Token: 0x06004312 RID: 17170 RVA: 0x000E604F File Offset: 0x000E504F
		public static void ThreadPoolCallBack(object o)
		{
			((AgileAsyncWorkerItem)o).DoAsyncCall();
		}

		// Token: 0x06004313 RID: 17171 RVA: 0x000E605C File Offset: 0x000E505C
		public void DoAsyncCall()
		{
			new StackBuilderSink(this._target).AsyncProcessMessage(this._message, this._ar);
		}

		// Token: 0x0400218D RID: 8589
		private IMethodCallMessage _message;

		// Token: 0x0400218E RID: 8590
		private AsyncResult _ar;

		// Token: 0x0400218F RID: 8591
		private object _target;
	}
}
