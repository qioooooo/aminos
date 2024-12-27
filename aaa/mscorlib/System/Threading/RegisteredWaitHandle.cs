using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	// Token: 0x0200015C RID: 348
	[ComVisible(true)]
	public sealed class RegisteredWaitHandle : MarshalByRefObject
	{
		// Token: 0x06001366 RID: 4966 RVA: 0x000354FC File Offset: 0x000344FC
		internal RegisteredWaitHandle()
		{
			this.internalRegisteredWait = new RegisteredWaitHandleSafe();
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x0003550F File Offset: 0x0003450F
		internal void SetHandle(IntPtr handle)
		{
			this.internalRegisteredWait.SetHandle(handle);
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x0003551D File Offset: 0x0003451D
		internal void SetWaitObject(WaitHandle waitObject)
		{
			this.internalRegisteredWait.SetWaitObject(waitObject);
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x0003552B File Offset: 0x0003452B
		[ComVisible(true)]
		public bool Unregister(WaitHandle waitObject)
		{
			return this.internalRegisteredWait.Unregister(waitObject);
		}

		// Token: 0x0400066C RID: 1644
		private RegisteredWaitHandleSafe internalRegisteredWait;
	}
}
