using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Transactions.Diagnostics;

namespace System.Transactions.Oletx
{
	// Token: 0x0200008E RID: 142
	internal class OletxEnlistment : OletxBaseEnlistment, IPromotedEnlistment
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00034E28 File Offset: 0x00034228
		internal Guid TransactionIdentifier
		{
			get
			{
				return this.transactionGuid;
			}
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00034E3C File Offset: 0x0003423C
		internal OletxEnlistment(bool canDoSinglePhase, IEnlistmentNotificationInternal enlistmentNotification, Guid transactionGuid, EnlistmentOptions enlistmentOptions, OletxResourceManager oletxResourceManager, OletxTransaction oletxTransaction)
			: base(oletxResourceManager, oletxTransaction)
		{
			Guid empty = Guid.Empty;
			this.enlistmentShim = null;
			this.phase0Shim = null;
			this.canDoSinglePhase = canDoSinglePhase;
			this.iEnlistmentNotification = enlistmentNotification;
			this.state = OletxEnlistment.OletxEnlistmentState.Active;
			this.transactionGuid = transactionGuid;
			this.proxyPrepareInfoByteArray = null;
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
			{
				global::System.Transactions.Diagnostics.EnlistmentTraceRecord.Trace(SR.GetString("TraceSourceOletx"), base.InternalTraceIdentifier, global::System.Transactions.Diagnostics.EnlistmentType.Durable, enlistmentOptions);
			}
			base.AddToEnlistmentTable();
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00034EC8 File Offset: 0x000342C8
		internal OletxEnlistment(IEnlistmentNotificationInternal enlistmentNotification, OletxTransactionStatus xactStatus, byte[] prepareInfoByteArray, OletxResourceManager oletxResourceManager)
			: base(oletxResourceManager, null)
		{
			Guid empty = Guid.Empty;
			this.enlistmentShim = null;
			this.phase0Shim = null;
			this.canDoSinglePhase = false;
			this.iEnlistmentNotification = enlistmentNotification;
			this.state = OletxEnlistment.OletxEnlistmentState.Active;
			int num = prepareInfoByteArray.Length;
			this.proxyPrepareInfoByteArray = new byte[num];
			Array.Copy(prepareInfoByteArray, this.proxyPrepareInfoByteArray, num);
			byte[] array = new byte[16];
			Array.Copy(this.proxyPrepareInfoByteArray, array, 16);
			this.transactionGuid = new Guid(array);
			this.transactionGuidString = this.transactionGuid.ToString();
			if (xactStatus != OletxTransactionStatus.OLETX_TRANSACTION_STATUS_PREPARED)
			{
				if (xactStatus == OletxTransactionStatus.OLETX_TRANSACTION_STATUS_ABORTED)
				{
					this.state = OletxEnlistment.OletxEnlistmentState.Aborting;
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.EnlistmentNotificationCallTraceRecord.Trace(SR.GetString("TraceSourceOletx"), base.InternalTraceIdentifier, global::System.Transactions.Diagnostics.NotificationCall.Rollback);
					}
					this.iEnlistmentNotification.Rollback(this);
					goto IL_01B8;
				}
				if (xactStatus == OletxTransactionStatus.OLETX_TRANSACTION_STATUS_COMMITTED)
				{
					this.state = OletxEnlistment.OletxEnlistmentState.Committing;
					lock (oletxResourceManager.reenlistList)
					{
						oletxResourceManager.reenlistPendingList.Add(this);
					}
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.EnlistmentNotificationCallTraceRecord.Trace(SR.GetString("TraceSourceOletx"), base.InternalTraceIdentifier, global::System.Transactions.Diagnostics.NotificationCall.Commit);
					}
					this.iEnlistmentNotification.Commit(this);
					goto IL_01B8;
				}
			}
			else
			{
				this.state = OletxEnlistment.OletxEnlistmentState.Prepared;
				lock (oletxResourceManager.reenlistList)
				{
					oletxResourceManager.reenlistList.Add(this);
					oletxResourceManager.StartReenlistThread();
					goto IL_01B8;
				}
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Critical)
			{
				global::System.Transactions.Diagnostics.InternalErrorTraceRecord.Trace(SR.GetString("TraceSourceOletx"), SR.GetString("OletxEnlistmentUnexpectedTransactionStatus"));
			}
			throw TransactionException.Create(SR.GetString("TraceSourceOletx"), SR.GetString("OletxEnlistmentUnexpectedTransactionStatus"), null);
			IL_01B8:
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
			{
				global::System.Transactions.Diagnostics.EnlistmentTraceRecord.Trace(SR.GetString("TraceSourceOletx"), base.InternalTraceIdentifier, global::System.Transactions.Diagnostics.EnlistmentType.Durable, EnlistmentOptions.None);
			}
			base.AddToEnlistmentTable();
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000394 RID: 916 RVA: 0x000350E8 File Offset: 0x000344E8
		internal IEnlistmentNotificationInternal EnlistmentNotification
		{
			get
			{
				return this.iEnlistmentNotification;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000395 RID: 917 RVA: 0x000350FC File Offset: 0x000344FC
		// (set) Token: 0x06000396 RID: 918 RVA: 0x00035110 File Offset: 0x00034510
		internal IEnlistmentShim EnlistmentShim
		{
			get
			{
				return this.enlistmentShim;
			}
			set
			{
				this.enlistmentShim = value;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00035124 File Offset: 0x00034524
		// (set) Token: 0x06000398 RID: 920 RVA: 0x00035138 File Offset: 0x00034538
		internal IPhase0EnlistmentShim Phase0EnlistmentShim
		{
			get
			{
				return this.phase0Shim;
			}
			set
			{
				lock (this)
				{
					if (value != null && (this.aborting || this.tmWentDown))
					{
						value.Phase0Done(false);
					}
					this.phase0Shim = value;
				}
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00035194 File Offset: 0x00034594
		// (set) Token: 0x0600039A RID: 922 RVA: 0x000351A8 File Offset: 0x000345A8
		internal OletxEnlistment.OletxEnlistmentState State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600039B RID: 923 RVA: 0x000351BC File Offset: 0x000345BC
		internal byte[] ProxyPrepareInfoByteArray
		{
			get
			{
				return this.proxyPrepareInfoByteArray;
			}
		}

		// Token: 0x0600039C RID: 924 RVA: 0x000351D0 File Offset: 0x000345D0
		internal void FinishEnlistment()
		{
			lock (this)
			{
				if (this.enlistmentShim == null)
				{
					this.oletxResourceManager.RemoveFromReenlistPending(this);
				}
				this.iEnlistmentNotification = null;
				base.RemoveFromEnlistmentTable();
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0003522C File Offset: 0x0003462C
		internal void TMDownFromInternalRM(OletxTransactionManager oletxTm)
		{
			lock (this)
			{
				if (this.oletxTransaction == null || oletxTm == this.oletxTransaction.realOletxTransaction.OletxTransactionManagerInstance)
				{
					this.tmWentDown = true;
				}
			}
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00035288 File Offset: 0x00034688
		public bool PrepareRequest(bool singlePhase, byte[] prepareInfo)
		{
			IEnlistmentShim enlistmentShim = null;
			OletxEnlistment.OletxEnlistmentState oletxEnlistmentState = OletxEnlistment.OletxEnlistmentState.Active;
			IEnlistmentNotificationInternal enlistmentNotificationInternal = null;
			lock (this)
			{
				if (this.state == OletxEnlistment.OletxEnlistmentState.Active)
				{
					oletxEnlistmentState = (this.state = OletxEnlistment.OletxEnlistmentState.Preparing);
				}
				else
				{
					oletxEnlistmentState = this.state;
				}
				enlistmentNotificationInternal = this.iEnlistmentNotification;
				enlistmentShim = this.EnlistmentShim;
				this.oletxTransaction.realOletxTransaction.TooLateForEnlistments = true;
			}
			bool flag;
			if (OletxEnlistment.OletxEnlistmentState.Preparing == oletxEnlistmentState)
			{
				OletxRecoveryInformation oletxRecoveryInformation = new OletxRecoveryInformation(prepareInfo);
				this.isSinglePhase = singlePhase;
				long num = (long)prepareInfo.Length;
				this.proxyPrepareInfoByteArray = new byte[num];
				Array.Copy(prepareInfo, this.proxyPrepareInfoByteArray, num);
				if (this.isSinglePhase && this.canDoSinglePhase)
				{
					ISinglePhaseNotificationInternal singlePhaseNotificationInternal = (ISinglePhaseNotificationInternal)enlistmentNotificationInternal;
					this.state = OletxEnlistment.OletxEnlistmentState.SinglePhaseCommitting;
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.EnlistmentNotificationCallTraceRecord.Trace(SR.GetString("TraceSourceOletx"), base.InternalTraceIdentifier, global::System.Transactions.Diagnostics.NotificationCall.SinglePhaseCommit);
					}
					singlePhaseNotificationInternal.SinglePhaseCommit(this);
					flag = true;
				}
				else
				{
					byte[] array = TransactionManager.ConvertToByteArray(oletxRecoveryInformation);
					this.state = OletxEnlistment.OletxEnlistmentState.Preparing;
					this.prepareInfoByteArray = TransactionManager.GetRecoveryInformation(this.oletxResourceManager.oletxTransactionManager.CreationNodeName, array);
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.EnlistmentNotificationCallTraceRecord.Trace(SR.GetString("TraceSourceOletx"), base.InternalTraceIdentifier, global::System.Transactions.Diagnostics.NotificationCall.Prepare);
					}
					enlistmentNotificationInternal.Prepare(this);
					flag = false;
				}
			}
			else
			{
				if (OletxEnlistment.OletxEnlistmentState.Prepared == oletxEnlistmentState)
				{
					try
					{
						enlistmentShim.PrepareRequestDone(OletxPrepareVoteType.Prepared);
						return false;
					}
					catch (COMException ex)
					{
						OletxTransactionManager.ProxyException(ex);
						throw;
					}
				}
				if (OletxEnlistment.OletxEnlistmentState.Done == oletxEnlistmentState)
				{
					try
					{
						try
						{
							enlistmentShim.PrepareRequestDone(OletxPrepareVoteType.ReadOnly);
							flag = true;
						}
						finally
						{
							this.FinishEnlistment();
						}
						return flag;
					}
					catch (COMException ex2)
					{
						OletxTransactionManager.ProxyException(ex2);
						throw;
					}
				}
				try
				{
					enlistmentShim.PrepareRequestDone(OletxPrepareVoteType.Failed);
				}
				catch (COMException ex3)
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex3);
					}
				}
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x000354B0 File Offset: 0x000348B0
		public void CommitRequest()
		{
			IEnlistmentNotificationInternal enlistmentNotificationInternal = null;
			IEnlistmentShim enlistmentShim = null;
			bool flag = false;
			lock (this)
			{
				if (OletxEnlistment.OletxEnlistmentState.Prepared == this.state)
				{
					this.state = OletxEnlistment.OletxEnlistmentState.Committing;
					enlistmentNotificationInternal = this.iEnlistmentNotification;
				}
				else
				{
					enlistmentShim = this.EnlistmentShim;
					flag = true;
				}
			}
			if (enlistmentNotificationInternal != null)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.EnlistmentNotificationCallTraceRecord.Trace(SR.GetString("TraceSourceOletx"), base.InternalTraceIdentifier, global::System.Transactions.Diagnostics.NotificationCall.Commit);
				}
				enlistmentNotificationInternal.Commit(this);
				return;
			}
			if (enlistmentShim != null)
			{
				try
				{
					enlistmentShim.CommitRequestDone();
				}
				catch (COMException ex)
				{
					if (NativeMethods.XACT_E_CONNECTION_DOWN != ex.ErrorCode && NativeMethods.XACT_E_TMNOTAVAILABLE != ex.ErrorCode)
					{
						throw;
					}
					flag = true;
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex);
					}
				}
				finally
				{
					if (flag)
					{
						this.FinishEnlistment();
					}
				}
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x000355C0 File Offset: 0x000349C0
		public void AbortRequest()
		{
			IEnlistmentNotificationInternal enlistmentNotificationInternal = null;
			IEnlistmentShim enlistmentShim = null;
			bool flag = false;
			lock (this)
			{
				if (this.state == OletxEnlistment.OletxEnlistmentState.Active || OletxEnlistment.OletxEnlistmentState.Prepared == this.state)
				{
					this.state = OletxEnlistment.OletxEnlistmentState.Aborting;
					enlistmentNotificationInternal = this.iEnlistmentNotification;
				}
				else
				{
					if (OletxEnlistment.OletxEnlistmentState.Phase0Preparing == this.state)
					{
						this.fabricateRollback = true;
					}
					else
					{
						flag = true;
					}
					enlistmentShim = this.EnlistmentShim;
				}
			}
			if (enlistmentNotificationInternal != null)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.EnlistmentNotificationCallTraceRecord.Trace(SR.GetString("TraceSourceOletx"), base.InternalTraceIdentifier, global::System.Transactions.Diagnostics.NotificationCall.Rollback);
				}
				enlistmentNotificationInternal.Rollback(this);
				return;
			}
			if (enlistmentShim != null)
			{
				try
				{
					enlistmentShim.AbortRequestDone();
				}
				catch (COMException ex)
				{
					if (NativeMethods.XACT_E_CONNECTION_DOWN != ex.ErrorCode && NativeMethods.XACT_E_TMNOTAVAILABLE != ex.ErrorCode)
					{
						throw;
					}
					flag = true;
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex);
					}
				}
				finally
				{
					if (flag)
					{
						this.FinishEnlistment();
					}
				}
			}
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x000356E8 File Offset: 0x00034AE8
		public void TMDown()
		{
			lock (this.oletxResourceManager.reenlistList)
			{
				lock (this)
				{
					this.tmWentDown = true;
					if (OletxEnlistment.OletxEnlistmentState.Prepared == this.state || OletxEnlistment.OletxEnlistmentState.Committing == this.state)
					{
						this.oletxResourceManager.reenlistList.Add(this);
					}
				}
			}
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00035780 File Offset: 0x00034B80
		public void Phase0Request(bool abortingHint)
		{
			IEnlistmentNotificationInternal enlistmentNotificationInternal = null;
			OletxEnlistment.OletxEnlistmentState oletxEnlistmentState = OletxEnlistment.OletxEnlistmentState.Active;
			bool flag = false;
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxEnlistment.Phase0Request");
			}
			OletxCommittableTransaction committableTransaction = this.oletxTransaction.realOletxTransaction.committableTransaction;
			if (committableTransaction != null && !committableTransaction.CommitCalled)
			{
				flag = true;
			}
			lock (this)
			{
				this.aborting = abortingHint;
				if (this.state == OletxEnlistment.OletxEnlistmentState.Active)
				{
					if (this.aborting || flag || this.tmWentDown)
					{
						if (this.phase0Shim == null)
						{
							goto IL_00B9;
						}
						try
						{
							this.phase0Shim.Phase0Done(false);
							goto IL_00B9;
						}
						catch (COMException ex)
						{
							if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
							{
								global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex);
							}
							goto IL_00B9;
						}
					}
					oletxEnlistmentState = (this.state = OletxEnlistment.OletxEnlistmentState.Phase0Preparing);
					enlistmentNotificationInternal = this.iEnlistmentNotification;
				}
				IL_00B9:;
			}
			if (enlistmentNotificationInternal != null)
			{
				if (OletxEnlistment.OletxEnlistmentState.Phase0Preparing != oletxEnlistmentState)
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxEnlistment.Phase0Request");
					}
					return;
				}
				byte[] array = this.transactionGuid.ToByteArray();
				byte[] array2 = this.oletxResourceManager.resourceManagerIdentifier.ToByteArray();
				byte[] array3 = new byte[array.Length + array2.Length];
				Thread.MemoryBarrier();
				this.proxyPrepareInfoByteArray = array3;
				for (int i = 0; i < array.Length; i++)
				{
					this.proxyPrepareInfoByteArray[i] = array[i];
				}
				for (int i = 0; i < array2.Length; i++)
				{
					this.proxyPrepareInfoByteArray[array.Length + i] = array2[i];
				}
				OletxRecoveryInformation oletxRecoveryInformation = new OletxRecoveryInformation(this.proxyPrepareInfoByteArray);
				byte[] array4 = TransactionManager.ConvertToByteArray(oletxRecoveryInformation);
				this.prepareInfoByteArray = TransactionManager.GetRecoveryInformation(this.oletxResourceManager.oletxTransactionManager.CreationNodeName, array4);
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.EnlistmentNotificationCallTraceRecord.Trace(SR.GetString("TraceSourceOletx"), base.InternalTraceIdentifier, global::System.Transactions.Diagnostics.NotificationCall.Prepare);
				}
				enlistmentNotificationInternal.Prepare(this);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxEnlistment.Phase0Request");
			}
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00035994 File Offset: 0x00034D94
		public void EnlistmentDone()
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxEnlistment.EnlistmentDone");
				global::System.Transactions.Diagnostics.EnlistmentCallbackPositiveTraceRecord.Trace(SR.GetString("TraceSourceOletx"), base.InternalTraceIdentifier, global::System.Transactions.Diagnostics.EnlistmentCallback.Done);
			}
			IEnlistmentShim enlistmentShim = null;
			IPhase0EnlistmentShim phase0EnlistmentShim = null;
			OletxEnlistment.OletxEnlistmentState oletxEnlistmentState = OletxEnlistment.OletxEnlistmentState.Active;
			bool flag = false;
			bool flag2;
			lock (this)
			{
				oletxEnlistmentState = this.state;
				if (this.state == OletxEnlistment.OletxEnlistmentState.Active)
				{
					phase0EnlistmentShim = this.Phase0EnlistmentShim;
					if (phase0EnlistmentShim != null)
					{
						this.oletxTransaction.realOletxTransaction.DecrementUndecidedEnlistments();
					}
					flag2 = false;
				}
				else if (OletxEnlistment.OletxEnlistmentState.Preparing == this.state)
				{
					enlistmentShim = this.EnlistmentShim;
					flag2 = true;
				}
				else if (OletxEnlistment.OletxEnlistmentState.Phase0Preparing == this.state)
				{
					phase0EnlistmentShim = this.Phase0EnlistmentShim;
					this.oletxTransaction.realOletxTransaction.DecrementUndecidedEnlistments();
					flag2 = this.fabricateRollback;
				}
				else
				{
					if (OletxEnlistment.OletxEnlistmentState.Committing != this.state && OletxEnlistment.OletxEnlistmentState.Aborting != this.state && OletxEnlistment.OletxEnlistmentState.SinglePhaseCommitting != this.state)
					{
						throw TransactionException.CreateEnlistmentStateException(SR.GetString("TraceSourceOletx"), null);
					}
					enlistmentShim = this.EnlistmentShim;
					flag2 = true;
				}
				flag = this.fabricateRollback;
				this.state = OletxEnlistment.OletxEnlistmentState.Done;
			}
			try
			{
				if (enlistmentShim != null)
				{
					if (OletxEnlistment.OletxEnlistmentState.Preparing == oletxEnlistmentState)
					{
						try
						{
							enlistmentShim.PrepareRequestDone(OletxPrepareVoteType.ReadOnly);
							goto IL_018C;
						}
						finally
						{
							HandleTable.FreeHandle(this.phase1Handle);
						}
					}
					if (OletxEnlistment.OletxEnlistmentState.Committing == oletxEnlistmentState)
					{
						enlistmentShim.CommitRequestDone();
					}
					else if (OletxEnlistment.OletxEnlistmentState.Aborting == oletxEnlistmentState)
					{
						if (!flag)
						{
							enlistmentShim.AbortRequestDone();
						}
					}
					else
					{
						if (OletxEnlistment.OletxEnlistmentState.SinglePhaseCommitting != oletxEnlistmentState)
						{
							throw TransactionException.CreateEnlistmentStateException(SR.GetString("TraceSourceOletx"), null);
						}
						enlistmentShim.PrepareRequestDone(OletxPrepareVoteType.SinglePhase);
					}
				}
				else if (phase0EnlistmentShim != null)
				{
					if (oletxEnlistmentState == OletxEnlistment.OletxEnlistmentState.Active)
					{
						phase0EnlistmentShim.Unenlist();
					}
					else
					{
						if (OletxEnlistment.OletxEnlistmentState.Phase0Preparing != oletxEnlistmentState)
						{
							throw TransactionException.CreateEnlistmentStateException(SR.GetString("TraceSourceOletx"), null);
						}
						phase0EnlistmentShim.Phase0Done(true);
					}
				}
				IL_018C:;
			}
			catch (COMException ex)
			{
				flag2 = true;
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex);
				}
			}
			finally
			{
				if (flag2)
				{
					this.FinishEnlistment();
				}
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxEnlistment.EnlistmentDone");
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x00035BD8 File Offset: 0x00034FD8
		public EnlistmentTraceIdentifier EnlistmentTraceId
		{
			get
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxEnlistment.get_TraceIdentifier");
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxEnlistment.get_TraceIdentifier");
				}
				return base.InternalTraceIdentifier;
			}
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00035C1C File Offset: 0x0003501C
		public void Prepared()
		{
			int s_OK = NativeMethods.S_OK;
			IEnlistmentShim enlistmentShim = null;
			IPhase0EnlistmentShim phase0EnlistmentShim = null;
			bool flag = false;
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxPreparingEnlistment.Prepared");
				global::System.Transactions.Diagnostics.EnlistmentCallbackPositiveTraceRecord.Trace(SR.GetString("TraceSourceOletx"), base.InternalTraceIdentifier, global::System.Transactions.Diagnostics.EnlistmentCallback.Prepared);
			}
			lock (this)
			{
				if (OletxEnlistment.OletxEnlistmentState.Preparing == this.state)
				{
					enlistmentShim = this.EnlistmentShim;
				}
				else
				{
					if (OletxEnlistment.OletxEnlistmentState.Phase0Preparing != this.state)
					{
						throw TransactionException.CreateEnlistmentStateException(SR.GetString("TraceSourceOletx"), null);
					}
					phase0EnlistmentShim = this.Phase0EnlistmentShim;
					if (this.oletxTransaction.realOletxTransaction.Doomed || this.fabricateRollback)
					{
						this.fabricateRollback = true;
						flag = this.fabricateRollback;
					}
				}
				this.state = OletxEnlistment.OletxEnlistmentState.Prepared;
			}
			try
			{
				if (enlistmentShim != null)
				{
					enlistmentShim.PrepareRequestDone(OletxPrepareVoteType.Prepared);
				}
				else if (phase0EnlistmentShim != null)
				{
					this.oletxTransaction.realOletxTransaction.DecrementUndecidedEnlistments();
					phase0EnlistmentShim.Phase0Done(!flag);
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					this.AbortRequest();
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
					this.Phase0EnlistmentShim = null;
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex);
					}
				}
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxPreparingEnlistment.Prepared");
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00035DCC File Offset: 0x000351CC
		public void ForceRollback()
		{
			this.ForceRollback(null);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00035DE0 File Offset: 0x000351E0
		public void ForceRollback(Exception e)
		{
			IEnlistmentShim enlistmentShim = null;
			IPhase0EnlistmentShim phase0EnlistmentShim = null;
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxPreparingEnlistment.ForceRollback");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
			{
				global::System.Transactions.Diagnostics.EnlistmentCallbackNegativeTraceRecord.Trace(SR.GetString("TraceSourceOletx"), base.InternalTraceIdentifier, global::System.Transactions.Diagnostics.EnlistmentCallback.ForceRollback);
			}
			lock (this)
			{
				if (OletxEnlistment.OletxEnlistmentState.Preparing == this.state)
				{
					enlistmentShim = this.EnlistmentShim;
				}
				else
				{
					if (OletxEnlistment.OletxEnlistmentState.Phase0Preparing != this.state)
					{
						throw TransactionException.CreateEnlistmentStateException(SR.GetString("TraceSourceOletx"), null);
					}
					phase0EnlistmentShim = this.Phase0EnlistmentShim;
					if (phase0EnlistmentShim != null)
					{
						this.oletxTransaction.realOletxTransaction.DecrementUndecidedEnlistments();
					}
				}
				this.state = OletxEnlistment.OletxEnlistmentState.Aborted;
			}
			Interlocked.CompareExchange<Exception>(ref this.oletxTransaction.realOletxTransaction.innerException, e, null);
			try
			{
				if (enlistmentShim != null)
				{
					try
					{
						enlistmentShim.PrepareRequestDone(OletxPrepareVoteType.Failed);
					}
					finally
					{
						HandleTable.FreeHandle(this.phase1Handle);
					}
				}
				if (phase0EnlistmentShim != null)
				{
					phase0EnlistmentShim.Phase0Done(false);
				}
			}
			catch (COMException ex)
			{
				if (NativeMethods.XACT_E_CONNECTION_DOWN != ex.ErrorCode && NativeMethods.XACT_E_TMNOTAVAILABLE != ex.ErrorCode)
				{
					throw;
				}
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex);
				}
			}
			finally
			{
				this.FinishEnlistment();
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxPreparingEnlistment.ForceRollback");
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00035F88 File Offset: 0x00035388
		public void Committed()
		{
			IEnlistmentShim enlistmentShim = null;
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxSinglePhaseEnlistment.Committed");
				global::System.Transactions.Diagnostics.EnlistmentCallbackPositiveTraceRecord.Trace(SR.GetString("TraceSourceOletx"), base.InternalTraceIdentifier, global::System.Transactions.Diagnostics.EnlistmentCallback.Committed);
			}
			lock (this)
			{
				if (!this.isSinglePhase || OletxEnlistment.OletxEnlistmentState.SinglePhaseCommitting != this.state)
				{
					throw TransactionException.CreateEnlistmentStateException(SR.GetString("TraceSourceOletx"), null);
				}
				this.state = OletxEnlistment.OletxEnlistmentState.Committed;
				enlistmentShim = this.EnlistmentShim;
			}
			try
			{
				if (enlistmentShim != null)
				{
					enlistmentShim.PrepareRequestDone(OletxPrepareVoteType.SinglePhase);
				}
			}
			catch (COMException ex)
			{
				if (NativeMethods.XACT_E_CONNECTION_DOWN != ex.ErrorCode && NativeMethods.XACT_E_TMNOTAVAILABLE != ex.ErrorCode)
				{
					throw;
				}
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex);
				}
			}
			finally
			{
				this.FinishEnlistment();
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxSinglePhaseEnlistment.Committed");
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x000360C0 File Offset: 0x000354C0
		public void Aborted()
		{
			this.Aborted(null);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x000360D4 File Offset: 0x000354D4
		public void Aborted(Exception e)
		{
			IEnlistmentShim enlistmentShim = null;
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxSinglePhaseEnlistment.Aborted");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
			{
				global::System.Transactions.Diagnostics.EnlistmentCallbackNegativeTraceRecord.Trace(SR.GetString("TraceSourceOletx"), base.InternalTraceIdentifier, global::System.Transactions.Diagnostics.EnlistmentCallback.Aborted);
			}
			lock (this)
			{
				if (!this.isSinglePhase || OletxEnlistment.OletxEnlistmentState.SinglePhaseCommitting != this.state)
				{
					throw TransactionException.CreateEnlistmentStateException(SR.GetString("TraceSourceOletx"), null);
				}
				this.state = OletxEnlistment.OletxEnlistmentState.Aborted;
				enlistmentShim = this.EnlistmentShim;
			}
			Interlocked.CompareExchange<Exception>(ref this.oletxTransaction.realOletxTransaction.innerException, e, null);
			try
			{
				if (enlistmentShim != null)
				{
					enlistmentShim.PrepareRequestDone(OletxPrepareVoteType.Failed);
				}
			}
			catch (COMException ex)
			{
				if (NativeMethods.XACT_E_CONNECTION_DOWN != ex.ErrorCode && NativeMethods.XACT_E_TMNOTAVAILABLE != ex.ErrorCode)
				{
					throw;
				}
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex);
				}
			}
			finally
			{
				this.FinishEnlistment();
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxSinglePhaseEnlistment.Aborted");
			}
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00036228 File Offset: 0x00035628
		public void InDoubt()
		{
			this.InDoubt(null);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0003623C File Offset: 0x0003563C
		public void InDoubt(Exception e)
		{
			IEnlistmentShim enlistmentShim = null;
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxSinglePhaseEnlistment.InDoubt");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
			{
				global::System.Transactions.Diagnostics.EnlistmentCallbackNegativeTraceRecord.Trace(SR.GetString("TraceSourceOletx"), base.InternalTraceIdentifier, global::System.Transactions.Diagnostics.EnlistmentCallback.InDoubt);
			}
			lock (this)
			{
				if (!this.isSinglePhase || OletxEnlistment.OletxEnlistmentState.SinglePhaseCommitting != this.state)
				{
					throw TransactionException.CreateEnlistmentStateException(SR.GetString("TraceSourceOletx"), null);
				}
				this.state = OletxEnlistment.OletxEnlistmentState.InDoubt;
				enlistmentShim = this.EnlistmentShim;
			}
			lock (this.oletxTransaction.realOletxTransaction)
			{
				if (this.oletxTransaction.realOletxTransaction.innerException == null)
				{
					this.oletxTransaction.realOletxTransaction.innerException = e;
				}
			}
			try
			{
				if (enlistmentShim != null)
				{
					enlistmentShim.PrepareRequestDone(OletxPrepareVoteType.InDoubt);
				}
			}
			catch (COMException ex)
			{
				if (NativeMethods.XACT_E_CONNECTION_DOWN != ex.ErrorCode && NativeMethods.XACT_E_TMNOTAVAILABLE != ex.ErrorCode)
				{
					throw;
				}
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex);
				}
			}
			finally
			{
				this.FinishEnlistment();
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxSinglePhaseEnlistment.InDoubt");
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x000363D0 File Offset: 0x000357D0
		public byte[] GetRecoveryInformation()
		{
			if (this.prepareInfoByteArray == null)
			{
				throw TransactionException.CreateEnlistmentStateException(SR.GetString("TraceSourceOletx"), null);
			}
			return this.prepareInfoByteArray;
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060003AE RID: 942 RVA: 0x000363FC File Offset: 0x000357FC
		// (set) Token: 0x060003AF RID: 943 RVA: 0x00036410 File Offset: 0x00035810
		public InternalEnlistment InternalEnlistment
		{
			get
			{
				return this.internalEnlistment;
			}
			set
			{
				this.internalEnlistment = value;
			}
		}

		// Token: 0x040001E0 RID: 480
		private IEnlistmentShim enlistmentShim;

		// Token: 0x040001E1 RID: 481
		private IPhase0EnlistmentShim phase0Shim;

		// Token: 0x040001E2 RID: 482
		private bool canDoSinglePhase;

		// Token: 0x040001E3 RID: 483
		private IEnlistmentNotificationInternal iEnlistmentNotification;

		// Token: 0x040001E4 RID: 484
		private byte[] proxyPrepareInfoByteArray;

		// Token: 0x040001E5 RID: 485
		private OletxEnlistment.OletxEnlistmentState state;

		// Token: 0x040001E6 RID: 486
		private bool isSinglePhase;

		// Token: 0x040001E7 RID: 487
		private Guid transactionGuid = Guid.Empty;

		// Token: 0x040001E8 RID: 488
		internal IntPtr phase1Handle = IntPtr.Zero;

		// Token: 0x040001E9 RID: 489
		private bool fabricateRollback;

		// Token: 0x040001EA RID: 490
		private bool tmWentDown;

		// Token: 0x040001EB RID: 491
		private bool aborting;

		// Token: 0x040001EC RID: 492
		private byte[] prepareInfoByteArray;

		// Token: 0x0200008F RID: 143
		internal enum OletxEnlistmentState
		{
			// Token: 0x040001EE RID: 494
			Active,
			// Token: 0x040001EF RID: 495
			Phase0Preparing,
			// Token: 0x040001F0 RID: 496
			Preparing,
			// Token: 0x040001F1 RID: 497
			SinglePhaseCommitting,
			// Token: 0x040001F2 RID: 498
			Prepared,
			// Token: 0x040001F3 RID: 499
			Committing,
			// Token: 0x040001F4 RID: 500
			Committed,
			// Token: 0x040001F5 RID: 501
			Aborting,
			// Token: 0x040001F6 RID: 502
			Aborted,
			// Token: 0x040001F7 RID: 503
			InDoubt,
			// Token: 0x040001F8 RID: 504
			Done
		}
	}
}
