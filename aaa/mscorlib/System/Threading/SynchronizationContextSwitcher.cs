using System;
using System.Runtime.ConstrainedExecution;

namespace System.Threading
{
	// Token: 0x0200012D RID: 301
	internal struct SynchronizationContextSwitcher : IDisposable
	{
		// Token: 0x06001175 RID: 4469 RVA: 0x00031C04 File Offset: 0x00030C04
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is SynchronizationContextSwitcher))
			{
				return false;
			}
			SynchronizationContextSwitcher synchronizationContextSwitcher = (SynchronizationContextSwitcher)obj;
			return this.savedSC == synchronizationContextSwitcher.savedSC && this.currSC == synchronizationContextSwitcher.currSC && this._ec == synchronizationContextSwitcher._ec;
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x00031C54 File Offset: 0x00030C54
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00031C67 File Offset: 0x00030C67
		public static bool operator ==(SynchronizationContextSwitcher c1, SynchronizationContextSwitcher c2)
		{
			return c1.Equals(c2);
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x00031C7C File Offset: 0x00030C7C
		public static bool operator !=(SynchronizationContextSwitcher c1, SynchronizationContextSwitcher c2)
		{
			return !c1.Equals(c2);
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x00031C94 File Offset: 0x00030C94
		void IDisposable.Dispose()
		{
			this.Undo();
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x00031C9C File Offset: 0x00030C9C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal bool UndoNoThrow()
		{
			if (this._ec == null)
			{
				return true;
			}
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

		// Token: 0x0600117B RID: 4475 RVA: 0x00031CD4 File Offset: 0x00030CD4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void Undo()
		{
			if (this._ec == null)
			{
				return;
			}
			ExecutionContext executionContextNoCreate = Thread.CurrentThread.GetExecutionContextNoCreate();
			if (this._ec != executionContextNoCreate)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_SwitcherCtxMismatch"));
			}
			if (this.currSC != this._ec.SynchronizationContext)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_SwitcherCtxMismatch"));
			}
			executionContextNoCreate.SynchronizationContext = this.savedSC;
			this._ec = null;
		}

		// Token: 0x040005CD RID: 1485
		internal SynchronizationContext savedSC;

		// Token: 0x040005CE RID: 1486
		internal SynchronizationContext currSC;

		// Token: 0x040005CF RID: 1487
		internal ExecutionContext _ec;
	}
}
