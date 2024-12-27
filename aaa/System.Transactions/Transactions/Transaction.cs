using System;
using System.EnterpriseServices;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;
using System.Transactions.Diagnostics;
using System.Transactions.Oletx;

namespace System.Transactions
{
	// Token: 0x0200000C RID: 12
	[Serializable]
	public class Transaction : IDisposable, ISerializable
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00027660 File Offset: 0x00026A60
		internal static bool EnterpriseServicesOk
		{
			get
			{
				if (Transaction._enterpriseServicesOk == EnterpriseServicesState.Unknown)
				{
					if (Type.GetType("System.EnterpriseServices.ContextUtil, System.EnterpriseServices, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false) != null)
					{
						Transaction._enterpriseServicesOk = EnterpriseServicesState.Available;
					}
					else
					{
						Transaction._enterpriseServicesOk = EnterpriseServicesState.Unavailable;
					}
				}
				return Transaction._enterpriseServicesOk == EnterpriseServicesState.Available;
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00027698 File Offset: 0x00026A98
		internal static void VerifyEnterpriseServicesOk()
		{
			if (!Transaction.EnterpriseServicesOk)
			{
				throw new NotSupportedException(SR.GetString("EsNotSupported"));
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000276BC File Offset: 0x00026ABC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Transaction JitSafeGetContextTransaction(ContextData contextData)
		{
			SafeIUnknown safeIUnknown = null;
			if (contextData.WeakDefaultComContext != null)
			{
				safeIUnknown = (SafeIUnknown)contextData.WeakDefaultComContext.Target;
			}
			if (contextData.DefaultComContextState == DefaultComContextState.Unknown || (contextData.DefaultComContextState == DefaultComContextState.Available && safeIUnknown == null))
			{
				try
				{
					global::System.Transactions.NativeMethods.CoGetDefaultContext(-1, ref Transaction.IID_IObjContext, out safeIUnknown);
					contextData.WeakDefaultComContext = new WeakReference(safeIUnknown);
					contextData.DefaultComContextState = DefaultComContextState.Available;
				}
				catch (EntryPointNotFoundException ex)
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceBase"), ex);
					}
					contextData.DefaultComContextState = DefaultComContextState.Unavailable;
				}
			}
			if (contextData.DefaultComContextState == DefaultComContextState.Available)
			{
				IntPtr zero = IntPtr.Zero;
				global::System.Transactions.NativeMethods.CoGetContextToken(out zero);
				if (safeIUnknown.DangerousGetHandle() == zero)
				{
					return null;
				}
			}
			if (!ContextUtil.IsInTransaction)
			{
				return null;
			}
			return ContextUtil.SystemTransaction;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0002778C File Offset: 0x00026B8C
		internal static Transaction GetContextTransaction(ContextData contextData)
		{
			if (Transaction.EnterpriseServicesOk)
			{
				return Transaction.JitSafeGetContextTransaction(contextData);
			}
			return null;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000277A8 File Offset: 0x00026BA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static bool UseServiceDomainForCurrent()
		{
			return !ContextUtil.IsDefaultContext();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000277C0 File Offset: 0x00026BC0
		internal static EnterpriseServicesInteropOption InteropMode(TransactionScope currentScope)
		{
			if (currentScope != null)
			{
				return currentScope.InteropMode;
			}
			return EnterpriseServicesInteropOption.None;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000277D8 File Offset: 0x00026BD8
		internal static Transaction FastGetTransaction(TransactionScope currentScope, ContextData contextData, out Transaction contextTransaction)
		{
			Transaction transaction = null;
			contextTransaction = null;
			contextTransaction = contextData.CurrentTransaction;
			switch (Transaction.InteropMode(currentScope))
			{
			case EnterpriseServicesInteropOption.None:
				transaction = contextTransaction;
				if (transaction == null && currentScope == null)
				{
					if (TransactionManager.currentDelegateSet)
					{
						transaction = TransactionManager.currentDelegate();
					}
					else
					{
						transaction = Transaction.GetContextTransaction(contextData);
					}
				}
				break;
			case EnterpriseServicesInteropOption.Automatic:
				if (Transaction.UseServiceDomainForCurrent())
				{
					transaction = Transaction.GetContextTransaction(contextData);
				}
				else
				{
					transaction = contextData.CurrentTransaction;
				}
				break;
			case EnterpriseServicesInteropOption.Full:
				transaction = Transaction.GetContextTransaction(contextData);
				break;
			}
			return transaction;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0002785C File Offset: 0x00026C5C
		internal static void GetCurrentTransactionAndScope(out Transaction current, out TransactionScope currentScope, out ContextData contextData, out Transaction contextTransaction)
		{
			contextData = ContextData.CurrentData;
			currentScope = contextData.CurrentScope;
			current = Transaction.FastGetTransaction(currentScope, contextData, out contextTransaction);
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00027888 File Offset: 0x00026C88
		// (set) Token: 0x0600001C RID: 28 RVA: 0x000278FC File Offset: 0x00026CFC
		public static Transaction Current
		{
			get
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceBase"), "Transaction.get_Current");
				}
				Transaction transaction = null;
				TransactionScope transactionScope = null;
				ContextData contextData = null;
				Transaction transaction2 = null;
				Transaction.GetCurrentTransactionAndScope(out transaction, out transactionScope, out contextData, out transaction2);
				if (transactionScope != null && transactionScope.ScopeComplete)
				{
					throw new InvalidOperationException(SR.GetString("TransactionScopeComplete"));
				}
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceBase"), "Transaction.get_Current");
				}
				return transaction;
			}
			set
			{
				if (!TransactionManager._platformValidated)
				{
					TransactionManager.ValidatePlatform();
				}
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceBase"), "Transaction.set_Current");
				}
				if (Transaction.InteropMode(ContextData.CurrentData.CurrentScope) != EnterpriseServicesInteropOption.None)
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
					{
						global::System.Transactions.Diagnostics.InvalidOperationExceptionTraceRecord.Trace(SR.GetString("TraceSourceBase"), SR.GetString("CannotSetCurrent"));
					}
					throw new InvalidOperationException(SR.GetString("CannotSetCurrent"));
				}
				ContextData.CurrentData.CurrentTransaction = value;
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceBase"), "Transaction.set_Current");
				}
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00027998 File Offset: 0x00026D98
		internal bool Disposed
		{
			get
			{
				return this.disposed == 1;
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000279B0 File Offset: 0x00026DB0
		private Transaction()
		{
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000279C4 File Offset: 0x00026DC4
		internal Transaction(IsolationLevel isoLevel, InternalTransaction internalTransaction)
		{
			TransactionManager.ValidateIsolationLevel(isoLevel);
			this.isoLevel = isoLevel;
			if (IsolationLevel.Unspecified == this.isoLevel)
			{
				this.isoLevel = TransactionManager.DefaultIsolationLevel;
			}
			if (internalTransaction != null)
			{
				this.internalTransaction = internalTransaction;
				this.cloneId = Interlocked.Increment(ref this.internalTransaction.cloneCount);
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00027A18 File Offset: 0x00026E18
		internal Transaction(global::System.Transactions.Oletx.OletxTransaction oleTransaction)
		{
			this.isoLevel = oleTransaction.IsolationLevel;
			this.internalTransaction = new InternalTransaction(this, oleTransaction);
			this.cloneId = Interlocked.Increment(ref this.internalTransaction.cloneCount);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00027A5C File Offset: 0x00026E5C
		internal Transaction(IsolationLevel isoLevel, ISimpleTransactionSuperior superior)
		{
			TransactionManager.ValidateIsolationLevel(isoLevel);
			if (superior == null)
			{
				throw new ArgumentNullException("superior");
			}
			this.isoLevel = isoLevel;
			if (IsolationLevel.Unspecified == this.isoLevel)
			{
				this.isoLevel = TransactionManager.DefaultIsolationLevel;
			}
			this.internalTransaction = new InternalTransaction(this, superior);
			this.cloneId = 1;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00027AB4 File Offset: 0x00026EB4
		public override int GetHashCode()
		{
			return this.internalTransaction.TransactionHash;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00027ACC File Offset: 0x00026ECC
		public override bool Equals(object obj)
		{
			Transaction transaction = obj as Transaction;
			return !(null == transaction) && this.internalTransaction.TransactionHash == transaction.internalTransaction.TransactionHash;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00027B04 File Offset: 0x00026F04
		public static bool operator ==(Transaction x, Transaction y)
		{
			if (x != null)
			{
				return x.Equals(y);
			}
			return y == null;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00027B20 File Offset: 0x00026F20
		public static bool operator !=(Transaction x, Transaction y)
		{
			if (x != null)
			{
				return !x.Equals(y);
			}
			return y != null;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00027B44 File Offset: 0x00026F44
		public TransactionInformation TransactionInformation
		{
			get
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.get_TransactionInformation");
				}
				if (this.Disposed)
				{
					throw new ObjectDisposedException("Transaction");
				}
				TransactionInformation transactionInformation = this.internalTransaction.transactionInformation;
				if (transactionInformation == null)
				{
					transactionInformation = new TransactionInformation(this.internalTransaction);
					this.internalTransaction.transactionInformation = transactionInformation;
				}
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.get_TransactionInformation");
				}
				return transactionInformation;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00027BC4 File Offset: 0x00026FC4
		public IsolationLevel IsolationLevel
		{
			get
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.get_IsolationLevel");
				}
				if (this.Disposed)
				{
					throw new ObjectDisposedException("Transaction");
				}
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.get_IsolationLevel");
				}
				return this.isoLevel;
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00027C20 File Offset: 0x00027020
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public Enlistment EnlistDurable(Guid resourceManagerIdentifier, IEnlistmentNotification enlistmentNotification, EnlistmentOptions enlistmentOptions)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.EnlistDurable( IEnlistmentNotification )");
			}
			if (this.Disposed)
			{
				throw new ObjectDisposedException("Transaction");
			}
			if (resourceManagerIdentifier == Guid.Empty)
			{
				throw new ArgumentException(SR.GetString("BadResourceManagerId"), "resourceManagerIdentifier");
			}
			if (enlistmentNotification == null)
			{
				throw new ArgumentNullException("enlistmentNotification");
			}
			if (enlistmentOptions != EnlistmentOptions.None && enlistmentOptions != EnlistmentOptions.EnlistDuringPrepareRequired)
			{
				throw new ArgumentOutOfRangeException("enlistmentOptions");
			}
			if (this.complete)
			{
				throw TransactionException.CreateTransactionCompletedException(SR.GetString("TraceSourceLtm"));
			}
			Enlistment enlistment2;
			lock (this.internalTransaction)
			{
				Enlistment enlistment = this.internalTransaction.State.EnlistDurable(this.internalTransaction, resourceManagerIdentifier, enlistmentNotification, enlistmentOptions, this);
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.EnlistDurable( IEnlistmentNotification )");
				}
				enlistment2 = enlistment;
			}
			return enlistment2;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00027D20 File Offset: 0x00027120
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public Enlistment EnlistDurable(Guid resourceManagerIdentifier, ISinglePhaseNotification singlePhaseNotification, EnlistmentOptions enlistmentOptions)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.EnlistDurable( ISinglePhaseNotification )");
			}
			if (this.Disposed)
			{
				throw new ObjectDisposedException("Transaction");
			}
			if (resourceManagerIdentifier == Guid.Empty)
			{
				throw new ArgumentException(SR.GetString("BadResourceManagerId"), "resourceManagerIdentifier");
			}
			if (singlePhaseNotification == null)
			{
				throw new ArgumentNullException("singlePhaseNotification");
			}
			if (enlistmentOptions != EnlistmentOptions.None && enlistmentOptions != EnlistmentOptions.EnlistDuringPrepareRequired)
			{
				throw new ArgumentOutOfRangeException("enlistmentOptions");
			}
			if (this.complete)
			{
				throw TransactionException.CreateTransactionCompletedException(SR.GetString("TraceSourceLtm"));
			}
			Enlistment enlistment2;
			lock (this.internalTransaction)
			{
				Enlistment enlistment = this.internalTransaction.State.EnlistDurable(this.internalTransaction, resourceManagerIdentifier, singlePhaseNotification, enlistmentOptions, this);
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.EnlistDurable( ISinglePhaseNotification )");
				}
				enlistment2 = enlistment;
			}
			return enlistment2;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00027E20 File Offset: 0x00027220
		public void Rollback()
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.Rollback");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
			{
				global::System.Transactions.Diagnostics.TransactionRollbackCalledTraceRecord.Trace(SR.GetString("TraceSourceLtm"), this.TransactionTraceId);
			}
			if (this.Disposed)
			{
				throw new ObjectDisposedException("Transaction");
			}
			lock (this.internalTransaction)
			{
				this.internalTransaction.State.Rollback(this.internalTransaction, null);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.Rollback");
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00027EDC File Offset: 0x000272DC
		public void Rollback(Exception e)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.Rollback");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
			{
				global::System.Transactions.Diagnostics.TransactionRollbackCalledTraceRecord.Trace(SR.GetString("TraceSourceLtm"), this.TransactionTraceId);
			}
			if (this.Disposed)
			{
				throw new ObjectDisposedException("Transaction");
			}
			lock (this.internalTransaction)
			{
				this.internalTransaction.State.Rollback(this.internalTransaction, e);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.Rollback");
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00027F98 File Offset: 0x00027398
		public Enlistment EnlistVolatile(IEnlistmentNotification enlistmentNotification, EnlistmentOptions enlistmentOptions)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.EnlistVolatile( IEnlistmentNotification )");
			}
			if (this.Disposed)
			{
				throw new ObjectDisposedException("Transaction");
			}
			if (enlistmentNotification == null)
			{
				throw new ArgumentNullException("enlistmentNotification");
			}
			if (enlistmentOptions != EnlistmentOptions.None && enlistmentOptions != EnlistmentOptions.EnlistDuringPrepareRequired)
			{
				throw new ArgumentOutOfRangeException("enlistmentOptions");
			}
			if (this.complete)
			{
				throw TransactionException.CreateTransactionCompletedException(SR.GetString("TraceSourceLtm"));
			}
			Enlistment enlistment2;
			lock (this.internalTransaction)
			{
				Enlistment enlistment = this.internalTransaction.State.EnlistVolatile(this.internalTransaction, enlistmentNotification, enlistmentOptions, this);
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.EnlistVolatile( IEnlistmentNotification )");
				}
				enlistment2 = enlistment;
			}
			return enlistment2;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00028078 File Offset: 0x00027478
		public Enlistment EnlistVolatile(ISinglePhaseNotification singlePhaseNotification, EnlistmentOptions enlistmentOptions)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.EnlistVolatile( ISinglePhaseNotification )");
			}
			if (this.Disposed)
			{
				throw new ObjectDisposedException("Transaction");
			}
			if (singlePhaseNotification == null)
			{
				throw new ArgumentNullException("singlePhaseNotification");
			}
			if (enlistmentOptions != EnlistmentOptions.None && enlistmentOptions != EnlistmentOptions.EnlistDuringPrepareRequired)
			{
				throw new ArgumentOutOfRangeException("enlistmentOptions");
			}
			if (this.complete)
			{
				throw TransactionException.CreateTransactionCompletedException(SR.GetString("TraceSourceLtm"));
			}
			Enlistment enlistment2;
			lock (this.internalTransaction)
			{
				Enlistment enlistment = this.internalTransaction.State.EnlistVolatile(this.internalTransaction, singlePhaseNotification, enlistmentOptions, this);
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.EnlistVolatile( ISinglePhaseNotification )");
				}
				enlistment2 = enlistment;
			}
			return enlistment2;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00028158 File Offset: 0x00027558
		public Transaction Clone()
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.Clone");
			}
			if (this.Disposed)
			{
				throw new ObjectDisposedException("Transaction");
			}
			if (this.complete)
			{
				throw TransactionException.CreateTransactionCompletedException(SR.GetString("TraceSourceLtm"));
			}
			Transaction transaction = this.InternalClone();
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.Clone");
			}
			return transaction;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000281D0 File Offset: 0x000275D0
		internal Transaction InternalClone()
		{
			Transaction transaction = new Transaction(this.isoLevel, this.internalTransaction);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.CloneCreatedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), transaction.TransactionTraceId);
			}
			return transaction;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0002820C File Offset: 0x0002760C
		public DependentTransaction DependentClone(DependentCloneOption cloneOption)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.DependentClone");
			}
			if (cloneOption != DependentCloneOption.BlockCommitUntilComplete && cloneOption != DependentCloneOption.RollbackIfNotComplete)
			{
				throw new ArgumentOutOfRangeException("cloneOption");
			}
			if (this.Disposed)
			{
				throw new ObjectDisposedException("Transaction");
			}
			if (this.complete)
			{
				throw TransactionException.CreateTransactionCompletedException(SR.GetString("TraceSourceLtm"));
			}
			DependentTransaction dependentTransaction = new DependentTransaction(this.isoLevel, this.internalTransaction, cloneOption == DependentCloneOption.BlockCommitUntilComplete);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
			{
				global::System.Transactions.Diagnostics.DependentCloneCreatedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), dependentTransaction.TransactionTraceId, cloneOption);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.DependentClone");
			}
			return dependentTransaction;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000282C0 File Offset: 0x000276C0
		internal TransactionTraceIdentifier TransactionTraceId
		{
			get
			{
				if (this.traceIdentifier == TransactionTraceIdentifier.Empty)
				{
					lock (this.internalTransaction)
					{
						if (this.traceIdentifier == TransactionTraceIdentifier.Empty)
						{
							TransactionTraceIdentifier transactionTraceIdentifier = new TransactionTraceIdentifier(this.internalTransaction.TransactionTraceId.TransactionIdentifier, this.cloneId);
							Thread.MemoryBarrier();
							this.traceIdentifier = transactionTraceIdentifier;
						}
					}
				}
				return this.traceIdentifier;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000032 RID: 50 RVA: 0x00028358 File Offset: 0x00027758
		// (remove) Token: 0x06000033 RID: 51 RVA: 0x000283C4 File Offset: 0x000277C4
		public event TransactionCompletedEventHandler TransactionCompleted
		{
			add
			{
				if (this.Disposed)
				{
					throw new ObjectDisposedException("Transaction");
				}
				lock (this.internalTransaction)
				{
					this.internalTransaction.State.AddOutcomeRegistrant(this.internalTransaction, value);
				}
			}
			remove
			{
				lock (this.internalTransaction)
				{
					this.internalTransaction.transactionCompletedDelegate = (TransactionCompletedEventHandler)Delegate.Remove(this.internalTransaction.transactionCompletedDelegate, value);
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00028424 File Offset: 0x00027824
		public void Dispose()
		{
			this.InternalDispose();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00028438 File Offset: 0x00027838
		internal virtual void InternalDispose()
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "IDisposable.Dispose");
			}
			if (Interlocked.Exchange(ref this.disposed, 1) == 1)
			{
				return;
			}
			long num = (long)Interlocked.Decrement(ref this.internalTransaction.cloneCount);
			if (num == 0L)
			{
				this.internalTransaction.Dispose();
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "IDisposable.Dispose");
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000284B0 File Offset: 0x000278B0
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext context)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "ISerializable.GetObjectData");
			}
			if (this.Disposed)
			{
				throw new ObjectDisposedException("Transaction");
			}
			if (serializationInfo == null)
			{
				throw new ArgumentNullException("serializationInfo");
			}
			if (this.complete)
			{
				throw TransactionException.CreateTransactionCompletedException(SR.GetString("TraceSourceLtm"));
			}
			lock (this.internalTransaction)
			{
				this.internalTransaction.State.GetObjectData(this.internalTransaction, serializationInfo, context);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
			{
				global::System.Transactions.Diagnostics.TransactionSerializedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), this.TransactionTraceId);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "ISerializable.GetObjectData");
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00028594 File Offset: 0x00027994
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public bool EnlistPromotableSinglePhase(IPromotableSinglePhaseNotification promotableSinglePhaseNotification)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.EnlistPromotableSinglePhase");
			}
			if (this.Disposed)
			{
				throw new ObjectDisposedException("Transaction");
			}
			if (promotableSinglePhaseNotification == null)
			{
				throw new ArgumentNullException("promotableSinglePhaseNotification");
			}
			if (this.complete)
			{
				throw TransactionException.CreateTransactionCompletedException(SR.GetString("TraceSourceLtm"));
			}
			bool flag = false;
			lock (this.internalTransaction)
			{
				flag = this.internalTransaction.State.EnlistPromotableSinglePhase(this.internalTransaction, promotableSinglePhaseNotification, this);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Transaction.EnlistPromotableSinglePhase");
			}
			return flag;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00028660 File Offset: 0x00027A60
		internal global::System.Transactions.Oletx.OletxTransaction Promote()
		{
			global::System.Transactions.Oletx.OletxTransaction promotedTransaction;
			lock (this.internalTransaction)
			{
				this.internalTransaction.State.Promote(this.internalTransaction);
				promotedTransaction = this.internalTransaction.PromotedTransaction;
			}
			return promotedTransaction;
		}

		// Token: 0x0400008D RID: 141
		internal const int disposedTrueValue = 1;

		// Token: 0x0400008E RID: 142
		private static EnterpriseServicesState _enterpriseServicesOk = EnterpriseServicesState.Unknown;

		// Token: 0x0400008F RID: 143
		private static Guid IID_IObjContext = new Guid("000001c6-0000-0000-C000-000000000046");

		// Token: 0x04000090 RID: 144
		internal IsolationLevel isoLevel;

		// Token: 0x04000091 RID: 145
		internal bool complete;

		// Token: 0x04000092 RID: 146
		internal int cloneId;

		// Token: 0x04000093 RID: 147
		internal int disposed;

		// Token: 0x04000094 RID: 148
		internal InternalTransaction internalTransaction;

		// Token: 0x04000095 RID: 149
		internal TransactionTraceIdentifier traceIdentifier;
	}
}
