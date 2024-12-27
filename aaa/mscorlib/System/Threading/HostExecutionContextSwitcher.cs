using System;
using System.Runtime.ConstrainedExecution;

namespace System.Threading
{
	// Token: 0x02000141 RID: 321
	internal class HostExecutionContextSwitcher
	{
		// Token: 0x06001221 RID: 4641 RVA: 0x00032F20 File Offset: 0x00031F20
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void Undo(object switcherObject)
		{
			if (switcherObject == null)
			{
				return;
			}
			HostExecutionContextManager currentHostExecutionContextManager = HostExecutionContextManager.GetCurrentHostExecutionContextManager();
			if (currentHostExecutionContextManager != null)
			{
				currentHostExecutionContextManager.Revert(switcherObject);
			}
		}

		// Token: 0x04000606 RID: 1542
		internal ExecutionContext executionContext;

		// Token: 0x04000607 RID: 1543
		internal HostExecutionContext previousHostContext;

		// Token: 0x04000608 RID: 1544
		internal HostExecutionContext currentHostContext;
	}
}
