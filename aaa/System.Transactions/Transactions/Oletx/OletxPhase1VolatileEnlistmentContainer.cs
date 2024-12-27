using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Transactions.Diagnostics;

namespace System.Transactions.Oletx
{
	// Token: 0x02000097 RID: 151
	internal class OletxPhase1VolatileEnlistmentContainer : OletxVolatileEnlistmentContainer
	{
		// Token: 0x06000410 RID: 1040 RVA: 0x00039BC4 File Offset: 0x00038FC4
		internal OletxPhase1VolatileEnlistmentContainer(RealOletxTransaction realOletxTransaction)
		{
			this.voterBallotShim = null;
			this.realOletxTransaction = realOletxTransaction;
			this.phase = -1;
			this.outstandingNotifications = 0;
			this.incompleteDependentClones = 0;
			this.alreadyVoted = false;
			this.collectedVoteYes = true;
			this.enlistmentList = new ArrayList();
			realOletxTransaction.IncrementUndecidedEnlistments();
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00039C24 File Offset: 0x00039024
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

		// Token: 0x06000412 RID: 1042 RVA: 0x00039C90 File Offset: 0x00039090
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

		// Token: 0x06000413 RID: 1043 RVA: 0x00039CF4 File Offset: 0x000390F4
		internal override void DependentCloneCompleted()
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				string text = string.Concat(new string[]
				{
					"OletxPhase1VolatileEnlistmentContainer.DependentCloneCompleted, outstandingNotifications = ",
					this.outstandingNotifications.ToString(CultureInfo.CurrentCulture),
					", incompleteDependentClones = ",
					this.incompleteDependentClones.ToString(CultureInfo.CurrentCulture),
					", phase = ",
					this.phase.ToString(CultureInfo.CurrentCulture)
				});
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), text);
			}
			this.incompleteDependentClones--;
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				string text2 = "OletxPhase1VolatileEnlistmentContainer.DependentCloneCompleted";
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), text2);
			}
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00039DA4 File Offset: 0x000391A4
		internal override void RollbackFromTransaction()
		{
			bool flag = false;
			IVoterBallotShim voterBallotShim = null;
			lock (this)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					string text = "OletxPhase1VolatileEnlistmentContainer.RollbackFromTransaction, outstandingNotifications = " + this.outstandingNotifications.ToString(CultureInfo.CurrentCulture) + ", incompleteDependentClones = " + this.incompleteDependentClones.ToString(CultureInfo.CurrentCulture);
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), text);
				}
				if (1 == this.phase && 0 < this.outstandingNotifications)
				{
					this.alreadyVoted = true;
					flag = true;
					voterBallotShim = this.voterBallotShim;
				}
			}
			if (flag)
			{
				try
				{
					if (voterBallotShim != null)
					{
						voterBallotShim.Vote(false);
					}
					this.Aborted();
				}
				catch (COMException ex)
				{
					if (NativeMethods.XACT_E_CONNECTION_DOWN != ex.ErrorCode && NativeMethods.XACT_E_TMNOTAVAILABLE != ex.ErrorCode)
					{
						throw;
					}
					lock (this)
					{
						if (1 == this.phase)
						{
							this.InDoubt();
						}
					}
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex);
					}
				}
				finally
				{
					HandleTable.FreeHandle(this.voterHandle);
				}
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				string text2 = "OletxPhase1VolatileEnlistmentContainer.RollbackFromTransaction";
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), text2);
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00039F38 File Offset: 0x00039338
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x00039F7C File Offset: 0x0003937C
		internal IVoterBallotShim VoterBallotShim
		{
			get
			{
				IVoterBallotShim voterBallotShim = null;
				lock (this)
				{
					voterBallotShim = this.voterBallotShim;
				}
				return voterBallotShim;
			}
			set
			{
				lock (this)
				{
					this.voterBallotShim = value;
				}
			}
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00039FC0 File Offset: 0x000393C0
		internal override void DecrementOutstandingNotifications(bool voteYes)
		{
			bool flag = false;
			IVoterBallotShim voterBallotShim = null;
			lock (this)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					string text = "OletxPhase1VolatileEnlistmentContainer.DecrementOutstandingNotifications, outstandingNotifications = " + this.outstandingNotifications.ToString(CultureInfo.CurrentCulture) + ", incompleteDependentClones = " + this.incompleteDependentClones.ToString(CultureInfo.CurrentCulture);
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), text);
				}
				this.outstandingNotifications--;
				this.collectedVoteYes = this.collectedVoteYes && voteYes;
				if (this.outstandingNotifications == 0)
				{
					if (1 == this.phase && !this.alreadyVoted)
					{
						flag = true;
						this.alreadyVoted = true;
						voterBallotShim = this.VoterBallotShim;
					}
					this.realOletxTransaction.DecrementUndecidedEnlistments();
				}
			}
			try
			{
				if (flag)
				{
					if (this.collectedVoteYes && !this.realOletxTransaction.Doomed)
					{
						if (voterBallotShim != null)
						{
							voterBallotShim.Vote(true);
						}
					}
					else
					{
						try
						{
							if (voterBallotShim != null)
							{
								voterBallotShim.Vote(false);
							}
							this.Aborted();
						}
						finally
						{
							HandleTable.FreeHandle(this.voterHandle);
						}
					}
				}
			}
			catch (COMException ex)
			{
				if (NativeMethods.XACT_E_CONNECTION_DOWN != ex.ErrorCode && NativeMethods.XACT_E_TMNOTAVAILABLE != ex.ErrorCode)
				{
					throw;
				}
				lock (this)
				{
					if (1 == this.phase)
					{
						this.InDoubt();
					}
				}
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex);
				}
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				string text2 = "OletxPhase1VolatileEnlistmentContainer.DecrementOutstandingNotifications";
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), text2);
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0003A1A4 File Offset: 0x000395A4
		internal override void OutcomeFromTransaction(TransactionStatus outcome)
		{
			bool flag = false;
			bool flag2 = false;
			lock (this)
			{
				if (1 == this.phase && 0 < this.outstandingNotifications)
				{
					if (TransactionStatus.Aborted == outcome)
					{
						flag = true;
					}
					else if (TransactionStatus.InDoubt == outcome)
					{
						flag2 = true;
					}
				}
			}
			if (flag)
			{
				this.Aborted();
			}
			if (flag2)
			{
				this.InDoubt();
			}
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0003A214 File Offset: 0x00039614
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

		// Token: 0x0600041A RID: 1050 RVA: 0x0003A2B8 File Offset: 0x000396B8
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

		// Token: 0x0600041B RID: 1051 RVA: 0x0003A35C File Offset: 0x0003975C
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

		// Token: 0x0600041C RID: 1052 RVA: 0x0003A400 File Offset: 0x00039800
		internal void VoteRequest()
		{
			int num = 0;
			bool flag = false;
			lock (this)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					string text = "OletxPhase1VolatileEnlistmentContainer.VoteRequest";
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), text);
				}
				this.phase = 1;
				if (0 < this.incompleteDependentClones)
				{
					flag = true;
					this.outstandingNotifications = 1;
				}
				else
				{
					this.outstandingNotifications = this.enlistmentList.Count;
					num = this.enlistmentList.Count;
					if (num == 0)
					{
						this.outstandingNotifications = 1;
					}
				}
				this.realOletxTransaction.TooLateForEnlistments = true;
			}
			if (flag)
			{
				this.DecrementOutstandingNotifications(false);
			}
			else if (num == 0)
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
				string text2 = "OletxPhase1VolatileEnlistmentContainer.VoteRequest";
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), text2);
			}
		}

		// Token: 0x04000235 RID: 565
		private IVoterBallotShim voterBallotShim;

		// Token: 0x04000236 RID: 566
		internal IntPtr voterHandle = IntPtr.Zero;
	}
}
