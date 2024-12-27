using System;
using System.Security;

namespace System.Threading
{
	// Token: 0x0200013B RID: 315
	public struct AsyncFlowControl : IDisposable
	{
		// Token: 0x060011DE RID: 4574 RVA: 0x000326F4 File Offset: 0x000316F4
		internal void Setup(SecurityContextDisableFlow flags)
		{
			this.useEC = false;
			this._sc = Thread.CurrentThread.ExecutionContext.SecurityContext;
			this._sc._disableFlow = flags;
			this._thread = Thread.CurrentThread;
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00032729 File Offset: 0x00031729
		internal void Setup()
		{
			this.useEC = true;
			this._ec = Thread.CurrentThread.ExecutionContext;
			this._ec.isFlowSuppressed = true;
			this._thread = Thread.CurrentThread;
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x00032759 File Offset: 0x00031759
		void IDisposable.Dispose()
		{
			this.Undo();
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x00032764 File Offset: 0x00031764
		public void Undo()
		{
			if (this._thread == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotUseAFCMultiple"));
			}
			if (this._thread != Thread.CurrentThread)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotUseAFCOtherThread"));
			}
			if (this.useEC)
			{
				if (Thread.CurrentThread.ExecutionContext != this._ec)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_AsyncFlowCtrlCtxMismatch"));
				}
				ExecutionContext.RestoreFlow();
			}
			else
			{
				if (Thread.CurrentThread.ExecutionContext.SecurityContext != this._sc)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_AsyncFlowCtrlCtxMismatch"));
				}
				SecurityContext.RestoreFlow();
			}
			this._thread = null;
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x0003280A File Offset: 0x0003180A
		public override int GetHashCode()
		{
			if (this._thread != null)
			{
				return this._thread.GetHashCode();
			}
			return this.ToString().GetHashCode();
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x00032831 File Offset: 0x00031831
		public override bool Equals(object obj)
		{
			return obj is AsyncFlowControl && this.Equals((AsyncFlowControl)obj);
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x00032849 File Offset: 0x00031849
		public bool Equals(AsyncFlowControl obj)
		{
			return obj.useEC == this.useEC && obj._ec == this._ec && obj._sc == this._sc && obj._thread == this._thread;
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x00032889 File Offset: 0x00031889
		public static bool operator ==(AsyncFlowControl a, AsyncFlowControl b)
		{
			return a.Equals(b);
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x00032893 File Offset: 0x00031893
		public static bool operator !=(AsyncFlowControl a, AsyncFlowControl b)
		{
			return !(a == b);
		}

		// Token: 0x040005F4 RID: 1524
		private bool useEC;

		// Token: 0x040005F5 RID: 1525
		private ExecutionContext _ec;

		// Token: 0x040005F6 RID: 1526
		private SecurityContext _sc;

		// Token: 0x040005F7 RID: 1527
		private Thread _thread;
	}
}
