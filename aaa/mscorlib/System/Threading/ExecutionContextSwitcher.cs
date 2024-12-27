using System;
using System.Runtime.ConstrainedExecution;
using System.Security;

namespace System.Threading
{
	// Token: 0x0200013A RID: 314
	internal struct ExecutionContextSwitcher : IDisposable
	{
		// Token: 0x060011D7 RID: 4567 RVA: 0x00032554 File Offset: 0x00031554
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is ExecutionContextSwitcher))
			{
				return false;
			}
			ExecutionContextSwitcher executionContextSwitcher = (ExecutionContextSwitcher)obj;
			return this.prevEC == executionContextSwitcher.prevEC && this.currEC == executionContextSwitcher.currEC && this.scsw == executionContextSwitcher.scsw && this.sysw == executionContextSwitcher.sysw && this.hecsw == executionContextSwitcher.hecsw && this.thread == executionContextSwitcher.thread;
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x000325DB File Offset: 0x000315DB
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x000325EE File Offset: 0x000315EE
		public static bool operator ==(ExecutionContextSwitcher c1, ExecutionContextSwitcher c2)
		{
			return c1.Equals(c2);
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x00032603 File Offset: 0x00031603
		public static bool operator !=(ExecutionContextSwitcher c1, ExecutionContextSwitcher c2)
		{
			return !c1.Equals(c2);
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x0003261B File Offset: 0x0003161B
		void IDisposable.Dispose()
		{
			this.Undo();
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x00032624 File Offset: 0x00031624
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal bool UndoNoThrow()
		{
			try
			{
				this.Undo();
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x00032654 File Offset: 0x00031654
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void Undo()
		{
			if (this.thread == null)
			{
				return;
			}
			if (this.thread != Thread.CurrentThread)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotUseSwitcherOtherThread"));
			}
			if (this.currEC != Thread.CurrentThread.GetExecutionContextNoCreate())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_SwitcherCtxMismatch"));
			}
			this.scsw.Undo();
			try
			{
				HostExecutionContextSwitcher.Undo(this.hecsw);
			}
			finally
			{
				this.sysw.Undo();
			}
			Thread.CurrentThread.SetExecutionContext(this.prevEC);
			this.thread = null;
		}

		// Token: 0x040005EE RID: 1518
		internal ExecutionContext prevEC;

		// Token: 0x040005EF RID: 1519
		internal ExecutionContext currEC;

		// Token: 0x040005F0 RID: 1520
		internal SecurityContextSwitcher scsw;

		// Token: 0x040005F1 RID: 1521
		internal SynchronizationContextSwitcher sysw;

		// Token: 0x040005F2 RID: 1522
		internal object hecsw;

		// Token: 0x040005F3 RID: 1523
		internal Thread thread;
	}
}
