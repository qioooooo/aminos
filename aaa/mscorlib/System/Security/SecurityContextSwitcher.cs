using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Principal;
using System.Threading;

namespace System.Security
{
	// Token: 0x02000677 RID: 1655
	internal struct SecurityContextSwitcher : IDisposable
	{
		// Token: 0x06003C33 RID: 15411 RVA: 0x000CEC50 File Offset: 0x000CDC50
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is SecurityContextSwitcher))
			{
				return false;
			}
			SecurityContextSwitcher securityContextSwitcher = (SecurityContextSwitcher)obj;
			return this.prevSC == securityContextSwitcher.prevSC && this.currSC == securityContextSwitcher.currSC && this.currEC == securityContextSwitcher.currEC && this.cssw == securityContextSwitcher.cssw && this.wic == securityContextSwitcher.wic;
		}

		// Token: 0x06003C34 RID: 15412 RVA: 0x000CECC3 File Offset: 0x000CDCC3
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x000CECD6 File Offset: 0x000CDCD6
		public static bool operator ==(SecurityContextSwitcher c1, SecurityContextSwitcher c2)
		{
			return c1.Equals(c2);
		}

		// Token: 0x06003C36 RID: 15414 RVA: 0x000CECEB File Offset: 0x000CDCEB
		public static bool operator !=(SecurityContextSwitcher c1, SecurityContextSwitcher c2)
		{
			return !c1.Equals(c2);
		}

		// Token: 0x06003C37 RID: 15415 RVA: 0x000CED03 File Offset: 0x000CDD03
		void IDisposable.Dispose()
		{
			this.Undo();
		}

		// Token: 0x06003C38 RID: 15416 RVA: 0x000CED0C File Offset: 0x000CDD0C
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

		// Token: 0x06003C39 RID: 15417 RVA: 0x000CED3C File Offset: 0x000CDD3C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void Undo()
		{
			if (this.currEC == null)
			{
				return;
			}
			if (this.currEC != Thread.CurrentThread.GetExecutionContextNoCreate())
			{
				Environment.FailFast(Environment.GetResourceString("InvalidOperation_SwitcherCtxMismatch"));
			}
			if (this.currSC != this.currEC.SecurityContext)
			{
				Environment.FailFast(Environment.GetResourceString("InvalidOperation_SwitcherCtxMismatch"));
			}
			this.currEC.SecurityContext = this.prevSC;
			this.currEC = null;
			bool flag = true;
			try
			{
				if (this.wic != null)
				{
					flag &= this.wic.UndoNoThrow();
				}
			}
			catch
			{
				flag &= this.cssw.UndoNoThrow();
				Environment.FailFast(Environment.GetResourceString("ExecutionContext_UndoFailed"));
			}
			flag &= this.cssw.UndoNoThrow();
			if (!flag)
			{
				Environment.FailFast(Environment.GetResourceString("ExecutionContext_UndoFailed"));
			}
		}

		// Token: 0x04001EC6 RID: 7878
		internal SecurityContext prevSC;

		// Token: 0x04001EC7 RID: 7879
		internal SecurityContext currSC;

		// Token: 0x04001EC8 RID: 7880
		internal ExecutionContext currEC;

		// Token: 0x04001EC9 RID: 7881
		internal CompressedStackSwitcher cssw;

		// Token: 0x04001ECA RID: 7882
		internal WindowsImpersonationContext wic;
	}
}
