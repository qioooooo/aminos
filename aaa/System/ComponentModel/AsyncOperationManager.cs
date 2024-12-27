using System;
using System.Security.Permissions;
using System.Threading;

namespace System.ComponentModel
{
	// Token: 0x0200009A RID: 154
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public static class AsyncOperationManager
	{
		// Token: 0x06000585 RID: 1413 RVA: 0x00016F3E File Offset: 0x00015F3E
		public static AsyncOperation CreateOperation(object userSuppliedState)
		{
			return AsyncOperation.CreateOperation(userSuppliedState, AsyncOperationManager.SynchronizationContext);
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x00016F4B File Offset: 0x00015F4B
		// (set) Token: 0x06000587 RID: 1415 RVA: 0x00016F63 File Offset: 0x00015F63
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static SynchronizationContext SynchronizationContext
		{
			get
			{
				if (SynchronizationContext.Current == null)
				{
					SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
				}
				return SynchronizationContext.Current;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				SynchronizationContext.SetSynchronizationContext(value);
			}
		}
	}
}
