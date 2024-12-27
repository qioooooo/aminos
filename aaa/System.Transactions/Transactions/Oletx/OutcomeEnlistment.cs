using System;

namespace System.Transactions.Oletx
{
	// Token: 0x02000092 RID: 146
	internal sealed class OutcomeEnlistment
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x00037F10 File Offset: 0x00037310
		internal Guid TransactionIdentifier
		{
			get
			{
				return this.txGuid;
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00037F24 File Offset: 0x00037324
		internal OutcomeEnlistment()
		{
			this.haveIssuedOutcome = false;
			this.savedStatus = TransactionStatus.InDoubt;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00037F48 File Offset: 0x00037348
		internal void SetRealTransaction(RealOletxTransaction realTx)
		{
			bool flag = false;
			TransactionStatus transactionStatus = TransactionStatus.InDoubt;
			lock (this)
			{
				flag = this.haveIssuedOutcome;
				transactionStatus = this.savedStatus;
				if (!flag)
				{
					this.weakRealTransaction = new WeakReference(realTx);
					this.txGuid = realTx.TxGuid;
				}
			}
			if (flag)
			{
				realTx.FireOutcome(transactionStatus);
				if ((TransactionStatus.Aborted == transactionStatus || TransactionStatus.InDoubt == transactionStatus) && realTx.phase1EnlistVolatilementContainer != null)
				{
					realTx.phase1EnlistVolatilementContainer.OutcomeFromTransaction(transactionStatus);
				}
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00037FD8 File Offset: 0x000373D8
		internal void UnregisterOutcomeCallback()
		{
			this.weakRealTransaction = null;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00037FEC File Offset: 0x000373EC
		private void InvokeOutcomeFunction(TransactionStatus status)
		{
			WeakReference weakReference = null;
			lock (this)
			{
				if (this.haveIssuedOutcome)
				{
					return;
				}
				this.haveIssuedOutcome = true;
				this.savedStatus = status;
				weakReference = this.weakRealTransaction;
			}
			if (weakReference != null)
			{
				RealOletxTransaction realOletxTransaction = weakReference.Target as RealOletxTransaction;
				if (realOletxTransaction != null)
				{
					realOletxTransaction.FireOutcome(status);
					if (realOletxTransaction.phase0EnlistVolatilementContainerList != null)
					{
						foreach (object obj in realOletxTransaction.phase0EnlistVolatilementContainerList)
						{
							OletxPhase0VolatileEnlistmentContainer oletxPhase0VolatileEnlistmentContainer = (OletxPhase0VolatileEnlistmentContainer)obj;
							oletxPhase0VolatileEnlistmentContainer.OutcomeFromTransaction(status);
						}
					}
					if ((TransactionStatus.Aborted == status || TransactionStatus.InDoubt == status) && realOletxTransaction.phase1EnlistVolatilementContainer != null)
					{
						realOletxTransaction.phase1EnlistVolatilementContainer.OutcomeFromTransaction(status);
					}
				}
				weakReference.Target = null;
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x000380EC File Offset: 0x000374EC
		internal bool TransactionIsInDoubt(RealOletxTransaction realTx)
		{
			return (realTx.committableTransaction == null || realTx.committableTransaction.CommitCalled) && realTx.UndecidedEnlistments == 0;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0003811C File Offset: 0x0003751C
		internal void TMDown()
		{
			bool flag = true;
			RealOletxTransaction realOletxTransaction = null;
			lock (this)
			{
				if (this.weakRealTransaction != null)
				{
					realOletxTransaction = this.weakRealTransaction.Target as RealOletxTransaction;
				}
			}
			if (realOletxTransaction != null)
			{
				lock (realOletxTransaction)
				{
					flag = this.TransactionIsInDoubt(realOletxTransaction);
				}
			}
			if (flag)
			{
				this.InDoubt();
				return;
			}
			this.Aborted();
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x000381B8 File Offset: 0x000375B8
		public void Committed()
		{
			this.InvokeOutcomeFunction(TransactionStatus.Committed);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x000381CC File Offset: 0x000375CC
		public void Aborted()
		{
			this.InvokeOutcomeFunction(TransactionStatus.Aborted);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000381E0 File Offset: 0x000375E0
		public void InDoubt()
		{
			this.InvokeOutcomeFunction(TransactionStatus.InDoubt);
		}

		// Token: 0x04000217 RID: 535
		private WeakReference weakRealTransaction;

		// Token: 0x04000218 RID: 536
		private Guid txGuid;

		// Token: 0x04000219 RID: 537
		private bool haveIssuedOutcome;

		// Token: 0x0400021A RID: 538
		private TransactionStatus savedStatus;
	}
}
