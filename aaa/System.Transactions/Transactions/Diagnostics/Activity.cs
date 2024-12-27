using System;

namespace System.Transactions.Diagnostics
{
	// Token: 0x0200009E RID: 158
	internal class Activity : IDisposable
	{
		// Token: 0x06000442 RID: 1090 RVA: 0x0003B0D8 File Offset: 0x0003A4D8
		private Activity(ref Guid newGuid, bool emitTransfer)
		{
			this.emitTransfer = emitTransfer;
			if (DiagnosticTrace.ShouldCorrelate && newGuid != Guid.Empty)
			{
				this.newGuid = newGuid;
				this.oldGuid = DiagnosticTrace.GetActivityId();
				if (this.oldGuid != newGuid)
				{
					this.mustDispose = true;
					if (this.emitTransfer)
					{
						DiagnosticTrace.TraceTransfer(newGuid);
					}
					DiagnosticTrace.SetActivityId(newGuid);
				}
			}
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0003B15C File Offset: 0x0003A55C
		internal static Activity CreateActivity(Guid newGuid, bool emitTransfer)
		{
			Activity activity = null;
			if (DiagnosticTrace.ShouldCorrelate && newGuid != Guid.Empty && newGuid != DiagnosticTrace.GetActivityId())
			{
				activity = new Activity(ref newGuid, emitTransfer);
			}
			return activity;
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0003B198 File Offset: 0x0003A598
		public void Dispose()
		{
			if (this.mustDispose)
			{
				this.mustDispose = false;
				if (this.emitTransfer)
				{
					DiagnosticTrace.TraceTransfer(this.oldGuid);
				}
				DiagnosticTrace.SetActivityId(this.oldGuid);
			}
		}

		// Token: 0x0400024F RID: 591
		private Guid oldGuid;

		// Token: 0x04000250 RID: 592
		private Guid newGuid;

		// Token: 0x04000251 RID: 593
		private bool emitTransfer;

		// Token: 0x04000252 RID: 594
		private bool mustDispose;
	}
}
