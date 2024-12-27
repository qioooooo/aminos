using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x0200022B RID: 555
	internal sealed class System_StackDebugView<T>
	{
		// Token: 0x0600128D RID: 4749 RVA: 0x0003E5CE File Offset: 0x0003D5CE
		public System_StackDebugView(Stack<T> stack)
		{
			if (stack == null)
			{
				throw new ArgumentNullException("stack");
			}
			this.stack = stack;
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x0600128E RID: 4750 RVA: 0x0003E5EB File Offset: 0x0003D5EB
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				return this.stack.ToArray();
			}
		}

		// Token: 0x040010C7 RID: 4295
		private Stack<T> stack;
	}
}
