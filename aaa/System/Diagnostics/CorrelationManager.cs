using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;

namespace System.Diagnostics
{
	// Token: 0x020001BD RID: 445
	public class CorrelationManager
	{
		// Token: 0x06000DBC RID: 3516 RVA: 0x0002BDD8 File Offset: 0x0002ADD8
		internal CorrelationManager()
		{
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x0002BDE0 File Offset: 0x0002ADE0
		// (set) Token: 0x06000DBE RID: 3518 RVA: 0x0002BE07 File Offset: 0x0002AE07
		public Guid ActivityId
		{
			get
			{
				object obj = CallContext.LogicalGetData("E2ETrace.ActivityID");
				if (obj != null)
				{
					return (Guid)obj;
				}
				return Guid.Empty;
			}
			set
			{
				CallContext.LogicalSetData("E2ETrace.ActivityID", value);
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000DBF RID: 3519 RVA: 0x0002BE19 File Offset: 0x0002AE19
		public Stack LogicalOperationStack
		{
			get
			{
				return this.GetLogicalOperationStack();
			}
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0002BE24 File Offset: 0x0002AE24
		public void StartLogicalOperation(object operationId)
		{
			if (operationId == null)
			{
				throw new ArgumentNullException("operationId");
			}
			Stack logicalOperationStack = this.GetLogicalOperationStack();
			logicalOperationStack.Push(operationId);
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0002BE4D File Offset: 0x0002AE4D
		public void StartLogicalOperation()
		{
			this.StartLogicalOperation(Guid.NewGuid());
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0002BE60 File Offset: 0x0002AE60
		public void StopLogicalOperation()
		{
			Stack logicalOperationStack = this.GetLogicalOperationStack();
			logicalOperationStack.Pop();
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0002BE7C File Offset: 0x0002AE7C
		private Stack GetLogicalOperationStack()
		{
			Stack stack = CallContext.LogicalGetData("System.Diagnostics.Trace.CorrelationManagerSlot") as Stack;
			if (stack == null)
			{
				stack = new Stack();
				CallContext.LogicalSetData("System.Diagnostics.Trace.CorrelationManagerSlot", stack);
			}
			return stack;
		}

		// Token: 0x04000ED4 RID: 3796
		private const string transactionSlotName = "System.Diagnostics.Trace.CorrelationManagerSlot";

		// Token: 0x04000ED5 RID: 3797
		private const string activityIdSlotName = "E2ETrace.ActivityID";
	}
}
