using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Transactions.Diagnostics;

namespace System.Transactions.Oletx
{
	// Token: 0x02000096 RID: 150
	internal class OletxPhase0VolatileEnlistmentContainer : OletxVolatileEnlistmentContainer
	{
		// Token: 0x06000401 RID: 1025 RVA: 0x000391C4 File Offset: 0x000385C4
		internal OletxPhase0VolatileEnlistmentContainer(RealOletxTransaction realOletxTransaction)
		{
			this.phase0EnlistmentShim = null;
			this.realOletxTransaction = realOletxTransaction;
			this.phase = -1;
			this.aborting = false;
			this.tmWentDown = false;
			this.outstandingNotifications = 0;
			this.incompleteDependentClones = 0;
			this.alreadyVoted = false;
			this.collectedVoteYes = true;
			this.enlistmentList = new ArrayList();
			realOletxTransaction.IncrementUndecidedEnlistments();
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00039228 File Offset: 0x00038628
		internal void TMDown()
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxPhase0VolatileEnlistmentContainer.TMDown");
			}
			this.tmWentDown = true;
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxPhase0VolatileEnlistmentContainer.TMDown");
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x00039274 File Offset: 0x00038674
		internal bool NewEnlistmentsAllowed
		{
			get
			{
				return -1 == this.phase;
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0003928C File Offset: 0x0003868C
		internal void AddEnlistment(OletxVolatileEnlistment enlistment)
		{
			lock (this)
			{
				if (-1 != this.phase)
				{
					throw TransactionException.Create(SR.GetString("TraceSourceOletx"), SR.GetString("TooLate"), null);
				}
				this.enlistmentList.Add(enlistment);
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x000392F8 File Offset: 0x000386F8
		internal override void AddDependentClone()
		{
			lock (this)
			{
				if (-1 != this.phase)
				{
					throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceOletx"), null);
				}
				this.incompleteDependentClones++;
			}
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0003935C File Offset: 0x0003875C
		internal override void DependentCloneCompleted()
		{
			bool flag = false;
			lock (this)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					string text = string.Concat(new string[]
					{
						"OletxPhase0VolatileEnlistmentContainer.DependentCloneCompleted, outstandingNotifications = ",
						this.outstandingNotifications.ToString(CultureInfo.CurrentCulture),
						", incompleteDependentClones = ",
						this.incompleteDependentClones.ToString(CultureInfo.CurrentCulture),
						", phase = ",
						this.phase.ToString(CultureInfo.CurrentCulture)
					});
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), text);
				}
				this.incompleteDependentClones--;
				if (this.incompleteDependentClones == 0 && this.phase == 0)
				{
					this.outstandingNotifications++;
					flag = true;
				}
			}
			if (flag)
			{
				this.DecrementOutstandingNotifications(true);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				string text2 = "OletxPhase0VolatileEnlistmentContainer.DependentCloneCompleted";
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), text2);
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00039468 File Offset: 0x00038868
		internal override void RollbackFromTransaction()
		{
			lock (this)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					string text = "OletxPhase0VolatileEnlistmentContainer.RollbackFromTransaction, outstandingNotifications = " + this.outstandingNotifications.ToString(CultureInfo.CurrentCulture) + ", incompleteDependentClones = " + this.incompleteDependentClones.ToString(CultureInfo.CurrentCulture);
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), text);
				}
				if (this.phase == 0 && (0 < this.outstandingNotifications || 0 < this.incompleteDependentClones))
				{
					this.alreadyVoted = true;
					if (this.Phase0EnlistmentShim != null)
					{
						this.Phase0EnlistmentShim.Phase0Done(false);
					}
				}
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				string text2 = "OletxPhase0VolatileEnlistmentContainer.RollbackFromTransaction";
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), text2);
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x0003953C File Offset: 0x0003893C
		// (set) Token: 0x06000409 RID: 1033 RVA: 0x00039580 File Offset: 0x00038980
		internal IPhase0EnlistmentShim Phase0EnlistmentShim
		{
			get
			{
				IPhase0EnlistmentShim phase0EnlistmentShim = null;
				lock (this)
				{
					phase0EnlistmentShim = this.phase0EnlistmentShim;
				}
				return phase0EnlistmentShim;
			}
			set
			{
				lock (this)
				{
					if (this.aborting || this.tmWentDown)
					{
						value.Phase0Done(false);
					}
					this.phase0EnlistmentShim = value;
				}
			}
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x000395D8 File Offset: 0x000389D8
		internal override void DecrementOutstandingNotifications(bool voteYes)
		{
			bool flag = false;
			IPhase0EnlistmentShim phase0EnlistmentShim = null;
			lock (this)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					string text = "OletxPhase0VolatileEnlistmentContainer.DecrementOutstandingNotifications, outstandingNotifications = " + this.outstandingNotifications.ToString(CultureInfo.CurrentCulture) + ", incompleteDependentClones = " + this.incompleteDependentClones.ToString(CultureInfo.CurrentCulture);
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), text);
				}
				this.outstandingNotifications--;
				this.collectedVoteYes = this.collectedVoteYes && voteYes;
				if (this.outstandingNotifications == 0 && this.incompleteDependentClones == 0)
				{
					if (this.phase == 0 && !this.alreadyVoted)
					{
						flag = true;
						this.alreadyVoted = true;
						phase0EnlistmentShim = this.phase0EnlistmentShim;
					}
					this.realOletxTransaction.DecrementUndecidedEnlistments();
				}
			}
			try
			{
				if (flag && phase0EnlistmentShim != null)
				{
					phase0EnlistmentShim.Phase0Done(this.collectedVoteYes && !this.realOletxTransaction.Doomed);
				}
			}
			catch (COMException ex)
			{
				if (NativeMethods.XACT_E_CONNECTION_DOWN == ex.ErrorCode || NativeMethods.XACT_E_TMNOTAVAILABLE == ex.ErrorCode)
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex);
					}
				}
				else
				{
					if (NativeMethods.XACT_E_PROTOCOL != ex.ErrorCode)
					{
						throw;
					}
					this.phase0EnlistmentShim = null;
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex);
					}
				}
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				string text2 = "OletxPhase0VolatileEnlistmentContainer.DecrementOutstandingNotifications";
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), text2);
			}
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00039780 File Offset: 0x00038B80
		internal override void OutcomeFromTransaction(TransactionStatus outcome)
		{
			if (TransactionStatus.Committed == outcome)
			{
				this.Committed();
				return;
			}
			if (TransactionStatus.Aborted == outcome)
			{
				this.Aborted();
				return;
			}
			if (TransactionStatus.InDoubt == outcome)
			{
				this.InDoubt();
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000397B0 File Offset: 0x00038BB0
		internal override void Committed()
		{
			int num = 0;
			lock (this)
			{
				this.phase = 2;
				num = this.enlistmentList.Count;
			}
			for (int i = 0; i < num; i++)
			{
				OletxVolatileEnlistment oletxVolatileEnlistment = this.enlistmentList[i] as OletxVolatileEnlistment;
				if (oletxVolatileEnlistment == null)
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Critical)
					{
						global::System.Transactions.Diagnostics.InternalErrorTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "");
					}
					throw new InvalidOperationException(SR.GetString("InternalError"));
				}
				oletxVolatileEnlistment.Commit();
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00039854 File Offset: 0x00038C54
		internal override void Aborted()
		{
			int num = 0;
			lock (this)
			{
				this.phase = 2;
				num = this.enlistmentList.Count;
			}
			for (int i = 0; i < num; i++)
			{
				OletxVolatileEnlistment oletxVolatileEnlistment = this.enlistmentList[i] as OletxVolatileEnlistment;
				if (oletxVolatileEnlistment == null)
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Critical)
					{
						global::System.Transactions.Diagnostics.InternalErrorTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "");
					}
					throw new InvalidOperationException(SR.GetString("InternalError"));
				}
				oletxVolatileEnlistment.Rollback();
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000398F8 File Offset: 0x00038CF8
		internal override void InDoubt()
		{
			int num = 0;
			lock (this)
			{
				this.phase = 2;
				num = this.enlistmentList.Count;
			}
			for (int i = 0; i < num; i++)
			{
				OletxVolatileEnlistment oletxVolatileEnlistment = this.enlistmentList[i] as OletxVolatileEnlistment;
				if (oletxVolatileEnlistment == null)
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Critical)
					{
						global::System.Transactions.Diagnostics.InternalErrorTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "");
					}
					throw new InvalidOperationException(SR.GetString("InternalError"));
				}
				oletxVolatileEnlistment.InDoubt();
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0003999C File Offset: 0x00038D9C
		internal void Phase0Request(bool abortHint)
		{
			int num = 0;
			bool flag = false;
			lock (this)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					string text = "OletxPhase0VolatileEnlistmentContainer.Phase0Request, abortHint = " + abortHint.ToString(CultureInfo.CurrentCulture) + ", phase = " + this.phase.ToString(CultureInfo.CurrentCulture);
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), text);
				}
				this.aborting = abortHint;
				OletxCommittableTransaction committableTransaction = this.realOletxTransaction.committableTransaction;
				if (committableTransaction != null && !committableTransaction.CommitCalled)
				{
					flag = true;
					this.aborting = true;
				}
				if (2 != this.phase && -1 != this.phase)
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Critical)
					{
						global::System.Transactions.Diagnostics.InternalErrorTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxPhase0VolatileEnlistmentContainer.Phase0Request, phase != -1");
					}
					throw new InvalidOperationException(SR.GetString("InternalError"));
				}
				if (-1 == this.phase)
				{
					this.phase = 0;
				}
				if (this.aborting || this.tmWentDown || flag || 2 == this.phase)
				{
					if (this.phase0EnlistmentShim != null)
					{
						try
						{
							this.phase0EnlistmentShim.Phase0Done(false);
						}
						catch (COMException ex)
						{
							if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
							{
								global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex);
							}
						}
					}
					return;
				}
				this.outstandingNotifications = this.enlistmentList.Count;
				num = this.enlistmentList.Count;
				if (num == 0)
				{
					this.outstandingNotifications = 1;
				}
			}
			if (num == 0)
			{
				this.DecrementOutstandingNotifications(true);
			}
			else
			{
				for (int i = 0; i < num; i++)
				{
					OletxVolatileEnlistment oletxVolatileEnlistment = this.enlistmentList[i] as OletxVolatileEnlistment;
					if (oletxVolatileEnlistment == null)
					{
						if (global::System.Transactions.Diagnostics.DiagnosticTrace.Critical)
						{
							global::System.Transactions.Diagnostics.InternalErrorTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "");
						}
						throw new InvalidOperationException(SR.GetString("InternalError"));
					}
					oletxVolatileEnlistment.Prepare(this);
				}
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				string text2 = "OletxPhase0VolatileEnlistmentContainer.Phase0Request, abortHint = " + abortHint.ToString(CultureInfo.CurrentCulture);
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), text2);
			}
		}

		// Token: 0x04000232 RID: 562
		private IPhase0EnlistmentShim phase0EnlistmentShim;

		// Token: 0x04000233 RID: 563
		private bool aborting;

		// Token: 0x04000234 RID: 564
		private bool tmWentDown;
	}
}
