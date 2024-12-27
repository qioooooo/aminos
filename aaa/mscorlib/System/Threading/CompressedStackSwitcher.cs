using System;
using System.Runtime.ConstrainedExecution;

namespace System.Threading
{
	// Token: 0x02000131 RID: 305
	internal struct CompressedStackSwitcher : IDisposable
	{
		// Token: 0x0600118E RID: 4494 RVA: 0x00031E6C File Offset: 0x00030E6C
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is CompressedStackSwitcher))
			{
				return false;
			}
			CompressedStackSwitcher compressedStackSwitcher = (CompressedStackSwitcher)obj;
			return this.curr_CS == compressedStackSwitcher.curr_CS && this.prev_CS == compressedStackSwitcher.prev_CS && this.prev_ADStack == compressedStackSwitcher.prev_ADStack;
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x00031EBF File Offset: 0x00030EBF
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x00031ED2 File Offset: 0x00030ED2
		public static bool operator ==(CompressedStackSwitcher c1, CompressedStackSwitcher c2)
		{
			return c1.Equals(c2);
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x00031EE7 File Offset: 0x00030EE7
		public static bool operator !=(CompressedStackSwitcher c1, CompressedStackSwitcher c2)
		{
			return !c1.Equals(c2);
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x00031EFF File Offset: 0x00030EFF
		void IDisposable.Dispose()
		{
			this.Undo();
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x00031F08 File Offset: 0x00030F08
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

		// Token: 0x06001194 RID: 4500 RVA: 0x00031F38 File Offset: 0x00030F38
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void Undo()
		{
			if (this.curr_CS == null && this.prev_CS == null)
			{
				return;
			}
			if (this.prev_ADStack != (IntPtr)0)
			{
				CompressedStack.RestoreAppDomainStack(this.prev_ADStack);
			}
			CompressedStack.SetCompressedStackThread(this.prev_CS);
			this.prev_CS = null;
			this.curr_CS = null;
			this.prev_ADStack = (IntPtr)0;
		}

		// Token: 0x040005D4 RID: 1492
		internal CompressedStack curr_CS;

		// Token: 0x040005D5 RID: 1493
		internal CompressedStack prev_CS;

		// Token: 0x040005D6 RID: 1494
		internal IntPtr prev_ADStack;
	}
}
