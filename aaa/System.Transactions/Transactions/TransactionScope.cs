using System;
using System.EnterpriseServices;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Transactions.Diagnostics;
using System.Transactions.Oletx;

namespace System.Transactions
{
	// Token: 0x02000073 RID: 115
	public sealed class TransactionScope : IDisposable
	{
		// Token: 0x06000323 RID: 803 RVA: 0x000329E8 File Offset: 0x00031DE8
		public TransactionScope()
			: this(TransactionScopeOption.Required)
		{
		}

		// Token: 0x06000324 RID: 804 RVA: 0x000329FC File Offset: 0x00031DFC
		public TransactionScope(TransactionScopeOption scopeOption)
		{
			if (!TransactionManager._platformValidated)
			{
				TransactionManager.ValidatePlatform();
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.ctor( TransactionScopeOption )");
			}
			if (this.NeedToCreateTransaction(scopeOption))
			{
				this.committableTransaction = new CommittableTransaction();
				this.expectedCurrent = this.committableTransaction.Clone();
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
			{
				if (null == this.expectedCurrent)
				{
					global::System.Transactions.Diagnostics.TransactionScopeCreatedTraceRecord.Trace(SR.GetString("TraceSourceBase"), TransactionTraceIdentifier.Empty, global::System.Transactions.Diagnostics.TransactionScopeResult.NoTransaction);
				}
				else
				{
					global::System.Transactions.Diagnostics.TransactionScopeResult transactionScopeResult;
					if (null == this.committableTransaction)
					{
						transactionScopeResult = global::System.Transactions.Diagnostics.TransactionScopeResult.UsingExistingCurrent;
					}
					else
					{
						transactionScopeResult = global::System.Transactions.Diagnostics.TransactionScopeResult.CreatedTransaction;
					}
					global::System.Transactions.Diagnostics.TransactionScopeCreatedTraceRecord.Trace(SR.GetString("TraceSourceBase"), this.expectedCurrent.TransactionTraceId, transactionScopeResult);
				}
			}
			this.PushScope();
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.ctor( TransactionScopeOption )");
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00032AD8 File Offset: 0x00031ED8
		public TransactionScope(TransactionScopeOption scopeOption, TimeSpan scopeTimeout)
		{
			if (!TransactionManager._platformValidated)
			{
				TransactionManager.ValidatePlatform();
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.ctor( TransactionScopeOption, TimeSpan )");
			}
			this.ValidateScopeTimeout("scopeTimeout", scopeTimeout);
			TimeSpan timeSpan = TransactionManager.ValidateTimeout(scopeTimeout);
			if (this.NeedToCreateTransaction(scopeOption))
			{
				this.committableTransaction = new CommittableTransaction(timeSpan);
				this.expectedCurrent = this.committableTransaction.Clone();
			}
			if (null != this.expectedCurrent && null == this.committableTransaction && TimeSpan.Zero != scopeTimeout)
			{
				this.scopeTimer = new Timer(new TimerCallback(TransactionScope.TimerCallback), this, scopeTimeout, TimeSpan.Zero);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
			{
				if (null == this.expectedCurrent)
				{
					global::System.Transactions.Diagnostics.TransactionScopeCreatedTraceRecord.Trace(SR.GetString("TraceSourceBase"), TransactionTraceIdentifier.Empty, global::System.Transactions.Diagnostics.TransactionScopeResult.NoTransaction);
				}
				else
				{
					global::System.Transactions.Diagnostics.TransactionScopeResult transactionScopeResult;
					if (null == this.committableTransaction)
					{
						transactionScopeResult = global::System.Transactions.Diagnostics.TransactionScopeResult.UsingExistingCurrent;
					}
					else
					{
						transactionScopeResult = global::System.Transactions.Diagnostics.TransactionScopeResult.CreatedTransaction;
					}
					global::System.Transactions.Diagnostics.TransactionScopeCreatedTraceRecord.Trace(SR.GetString("TraceSourceBase"), this.expectedCurrent.TransactionTraceId, transactionScopeResult);
				}
			}
			this.PushScope();
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.ctor( TransactionScopeOption, TimeSpan )");
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00032C10 File Offset: 0x00032010
		public TransactionScope(TransactionScopeOption scopeOption, TransactionOptions transactionOptions)
		{
			if (!TransactionManager._platformValidated)
			{
				TransactionManager.ValidatePlatform();
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.ctor( TransactionScopeOption, TransactionOptions )");
			}
			this.ValidateScopeTimeout("transactionOptions.Timeout", transactionOptions.Timeout);
			TimeSpan timeout = transactionOptions.Timeout;
			transactionOptions.Timeout = TransactionManager.ValidateTimeout(transactionOptions.Timeout);
			TransactionManager.ValidateIsolationLevel(transactionOptions.IsolationLevel);
			if (this.NeedToCreateTransaction(scopeOption))
			{
				this.committableTransaction = new CommittableTransaction(transactionOptions);
				this.expectedCurrent = this.committableTransaction.Clone();
			}
			else if (null != this.expectedCurrent && IsolationLevel.Unspecified != transactionOptions.IsolationLevel && this.expectedCurrent.IsolationLevel != transactionOptions.IsolationLevel)
			{
				throw new ArgumentException(SR.GetString("TransactionScopeIsolationLevelDifferentFromTransaction"), "transactionOptions.IsolationLevel");
			}
			if (null != this.expectedCurrent && null == this.committableTransaction && TimeSpan.Zero != timeout)
			{
				this.scopeTimer = new Timer(new TimerCallback(TransactionScope.TimerCallback), this, timeout, TimeSpan.Zero);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
			{
				if (null == this.expectedCurrent)
				{
					global::System.Transactions.Diagnostics.TransactionScopeCreatedTraceRecord.Trace(SR.GetString("TraceSourceBase"), TransactionTraceIdentifier.Empty, global::System.Transactions.Diagnostics.TransactionScopeResult.NoTransaction);
				}
				else
				{
					global::System.Transactions.Diagnostics.TransactionScopeResult transactionScopeResult;
					if (null == this.committableTransaction)
					{
						transactionScopeResult = global::System.Transactions.Diagnostics.TransactionScopeResult.UsingExistingCurrent;
					}
					else
					{
						transactionScopeResult = global::System.Transactions.Diagnostics.TransactionScopeResult.CreatedTransaction;
					}
					global::System.Transactions.Diagnostics.TransactionScopeCreatedTraceRecord.Trace(SR.GetString("TraceSourceBase"), this.expectedCurrent.TransactionTraceId, transactionScopeResult);
				}
			}
			this.PushScope();
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.ctor( TransactionScopeOption, TransactionOptions )");
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00032DB0 File Offset: 0x000321B0
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public TransactionScope(TransactionScopeOption scopeOption, TransactionOptions transactionOptions, EnterpriseServicesInteropOption interopOption)
		{
			if (!TransactionManager._platformValidated)
			{
				TransactionManager.ValidatePlatform();
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.ctor( TransactionScopeOption, TransactionOptions, EnterpriseServicesInteropOption )");
			}
			this.ValidateScopeTimeout("transactionOptions.Timeout", transactionOptions.Timeout);
			TimeSpan timeout = transactionOptions.Timeout;
			transactionOptions.Timeout = TransactionManager.ValidateTimeout(transactionOptions.Timeout);
			TransactionManager.ValidateIsolationLevel(transactionOptions.IsolationLevel);
			this.ValidateInteropOption(interopOption);
			this.interopModeSpecified = true;
			this.interopOption = interopOption;
			if (this.NeedToCreateTransaction(scopeOption))
			{
				this.committableTransaction = new CommittableTransaction(transactionOptions);
				this.expectedCurrent = this.committableTransaction.Clone();
			}
			else if (null != this.expectedCurrent && IsolationLevel.Unspecified != transactionOptions.IsolationLevel && this.expectedCurrent.IsolationLevel != transactionOptions.IsolationLevel)
			{
				throw new ArgumentException(SR.GetString("TransactionScopeIsolationLevelDifferentFromTransaction"), "transactionOptions.IsolationLevel");
			}
			if (null != this.expectedCurrent && null == this.committableTransaction && TimeSpan.Zero != timeout)
			{
				this.scopeTimer = new Timer(new TimerCallback(TransactionScope.TimerCallback), this, timeout, TimeSpan.Zero);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
			{
				if (null == this.expectedCurrent)
				{
					global::System.Transactions.Diagnostics.TransactionScopeCreatedTraceRecord.Trace(SR.GetString("TraceSourceBase"), TransactionTraceIdentifier.Empty, global::System.Transactions.Diagnostics.TransactionScopeResult.NoTransaction);
				}
				else
				{
					global::System.Transactions.Diagnostics.TransactionScopeResult transactionScopeResult;
					if (null == this.committableTransaction)
					{
						transactionScopeResult = global::System.Transactions.Diagnostics.TransactionScopeResult.UsingExistingCurrent;
					}
					else
					{
						transactionScopeResult = global::System.Transactions.Diagnostics.TransactionScopeResult.CreatedTransaction;
					}
					global::System.Transactions.Diagnostics.TransactionScopeCreatedTraceRecord.Trace(SR.GetString("TraceSourceBase"), this.expectedCurrent.TransactionTraceId, transactionScopeResult);
				}
			}
			this.PushScope();
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.ctor( TransactionScopeOption, TransactionOptions, EnterpriseServicesInteropOption )");
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00032F64 File Offset: 0x00032364
		public TransactionScope(Transaction transactionToUse)
		{
			if (!TransactionManager._platformValidated)
			{
				TransactionManager.ValidatePlatform();
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.ctor( Transaction )");
			}
			this.Initialize(transactionToUse, TimeSpan.Zero, false);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.ctor( Transaction )");
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00032FC8 File Offset: 0x000323C8
		public TransactionScope(Transaction transactionToUse, TimeSpan scopeTimeout)
		{
			if (!TransactionManager._platformValidated)
			{
				TransactionManager.ValidatePlatform();
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.ctor( Transaction, TimeSpan )");
			}
			this.Initialize(transactionToUse, scopeTimeout, false);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.ctor( Transaction, TimeSpan )");
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00033028 File Offset: 0x00032428
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public TransactionScope(Transaction transactionToUse, TimeSpan scopeTimeout, EnterpriseServicesInteropOption interopOption)
		{
			if (!TransactionManager._platformValidated)
			{
				TransactionManager.ValidatePlatform();
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.ctor( Transaction, TimeSpan, EnterpriseServicesInteropOption )");
			}
			this.ValidateInteropOption(interopOption);
			this.interopOption = interopOption;
			this.Initialize(transactionToUse, scopeTimeout, true);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.ctor( Transaction, TimeSpan, EnterpriseServicesInteropOption )");
			}
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00033094 File Offset: 0x00032494
		private bool NeedToCreateTransaction(TransactionScopeOption scopeOption)
		{
			bool flag = false;
			this.CommonInitialize();
			switch (scopeOption)
			{
			case TransactionScopeOption.Required:
				this.expectedCurrent = this.savedCurrent;
				if (null == this.expectedCurrent)
				{
					flag = true;
				}
				break;
			case TransactionScopeOption.RequiresNew:
				flag = true;
				break;
			case TransactionScopeOption.Suppress:
				this.expectedCurrent = null;
				flag = false;
				break;
			default:
				throw new ArgumentOutOfRangeException("scopeOption");
			}
			return flag;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x000330F8 File Offset: 0x000324F8
		private void Initialize(Transaction transactionToUse, TimeSpan scopeTimeout, bool interopModeSpecified)
		{
			if (null == transactionToUse)
			{
				throw new ArgumentNullException("transactionToUse");
			}
			this.ValidateScopeTimeout("scopeTimeout", scopeTimeout);
			this.CommonInitialize();
			if (TimeSpan.Zero != scopeTimeout)
			{
				this.scopeTimer = new Timer(new TimerCallback(TransactionScope.TimerCallback), this, scopeTimeout, TimeSpan.Zero);
			}
			this.expectedCurrent = transactionToUse;
			this.interopModeSpecified = interopModeSpecified;
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
			{
				global::System.Transactions.Diagnostics.TransactionScopeCreatedTraceRecord.Trace(SR.GetString("TraceSourceBase"), this.expectedCurrent.TransactionTraceId, global::System.Transactions.Diagnostics.TransactionScopeResult.TransactionPassed);
			}
			this.PushScope();
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0003318C File Offset: 0x0003258C
		public void Dispose()
		{
			bool flag = false;
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.Dispose");
			}
			if (this.disposed)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.Dispose");
				}
				return;
			}
			if (this.scopeThread != Thread.CurrentThread)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
				{
					global::System.Transactions.Diagnostics.InvalidOperationExceptionTraceRecord.Trace(SR.GetString("TraceSourceBase"), SR.GetString("InvalidScopeThread"));
				}
				throw new InvalidOperationException(SR.GetString("InvalidScopeThread"));
			}
			Exception ex = null;
			try
			{
				this.disposed = true;
				TransactionScope transactionScope = this.threadContextData.CurrentScope;
				Transaction transaction = null;
				Transaction transaction2 = Transaction.FastGetTransaction(transactionScope, this.threadContextData, out transaction);
				if (!this.Equals(transactionScope))
				{
					if (transactionScope == null)
					{
						Transaction transaction3 = this.committableTransaction;
						if (transaction3 == null)
						{
							transaction3 = this.dependentTransaction;
						}
						transaction3.Rollback();
						flag = true;
						throw TransactionException.CreateInvalidOperationException(SR.GetString("TraceSourceBase"), SR.GetString("TransactionScopeInvalidNesting"), null);
					}
					if (transactionScope.interopOption != EnterpriseServicesInteropOption.None || ((!(null != transactionScope.expectedCurrent) || transactionScope.expectedCurrent.Equals(transaction2)) && (!(null != transaction2) || !(null == transactionScope.expectedCurrent))))
					{
						goto IL_0252;
					}
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
					{
						TransactionTraceIdentifier transactionTraceIdentifier;
						if (null == transaction2)
						{
							transactionTraceIdentifier = TransactionTraceIdentifier.Empty;
						}
						else
						{
							transactionTraceIdentifier = transaction2.TransactionTraceId;
						}
						TransactionTraceIdentifier transactionTraceIdentifier2;
						if (null == this.expectedCurrent)
						{
							transactionTraceIdentifier2 = TransactionTraceIdentifier.Empty;
						}
						else
						{
							transactionTraceIdentifier2 = this.expectedCurrent.TransactionTraceId;
						}
						global::System.Transactions.Diagnostics.TransactionScopeCurrentChangedTraceRecord.Trace(SR.GetString("TraceSourceBase"), transactionTraceIdentifier2, transactionTraceIdentifier);
					}
					ex = TransactionException.CreateInvalidOperationException(SR.GetString("TraceSourceBase"), SR.GetString("TransactionScopeIncorrectCurrent"), null);
					if (!(null != transaction2))
					{
						goto IL_0252;
					}
					try
					{
						transaction2.Rollback();
						goto IL_0252;
					}
					catch (TransactionException)
					{
						goto IL_0252;
					}
					catch (ObjectDisposedException)
					{
						goto IL_0252;
					}
					IL_01CA:
					if (ex == null)
					{
						ex = TransactionException.CreateInvalidOperationException(SR.GetString("TraceSourceBase"), SR.GetString("TransactionScopeInvalidNesting"), null);
					}
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
					{
						if (null == transactionScope.expectedCurrent)
						{
							global::System.Transactions.Diagnostics.TransactionScopeNestedIncorrectlyTraceRecord.Trace(SR.GetString("TraceSourceBase"), TransactionTraceIdentifier.Empty);
						}
						else
						{
							global::System.Transactions.Diagnostics.TransactionScopeNestedIncorrectlyTraceRecord.Trace(SR.GetString("TraceSourceBase"), transactionScope.expectedCurrent.TransactionTraceId);
						}
					}
					transactionScope.complete = false;
					try
					{
						transactionScope.InternalDispose();
					}
					catch (TransactionException)
					{
					}
					transactionScope = this.threadContextData.CurrentScope;
					this.complete = false;
					IL_0252:
					if (!this.Equals(transactionScope))
					{
						goto IL_01CA;
					}
				}
				else if (this.interopOption == EnterpriseServicesInteropOption.None && ((null != this.expectedCurrent && !this.expectedCurrent.Equals(transaction2)) || (null != transaction2 && null == this.expectedCurrent)))
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
					{
						TransactionTraceIdentifier transactionTraceIdentifier3;
						if (null == transaction2)
						{
							transactionTraceIdentifier3 = TransactionTraceIdentifier.Empty;
						}
						else
						{
							transactionTraceIdentifier3 = transaction2.TransactionTraceId;
						}
						TransactionTraceIdentifier transactionTraceIdentifier4;
						if (null == this.expectedCurrent)
						{
							transactionTraceIdentifier4 = TransactionTraceIdentifier.Empty;
						}
						else
						{
							transactionTraceIdentifier4 = this.expectedCurrent.TransactionTraceId;
						}
						global::System.Transactions.Diagnostics.TransactionScopeCurrentChangedTraceRecord.Trace(SR.GetString("TraceSourceBase"), transactionTraceIdentifier4, transactionTraceIdentifier3);
					}
					if (ex == null)
					{
						ex = TransactionException.CreateInvalidOperationException(SR.GetString("TraceSourceBase"), SR.GetString("TransactionScopeIncorrectCurrent"), null);
					}
					if (null != transaction2)
					{
						try
						{
							transaction2.Rollback();
						}
						catch (TransactionException)
						{
						}
						catch (ObjectDisposedException)
						{
						}
					}
					this.complete = false;
				}
				flag = true;
			}
			finally
			{
				if (!flag)
				{
					this.PopScope();
				}
			}
			this.InternalDispose();
			if (ex != null)
			{
				throw ex;
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.Dispose");
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x000335A0 File Offset: 0x000329A0
		private void InternalDispose()
		{
			this.disposed = true;
			try
			{
				this.PopScope();
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
				{
					if (null == this.expectedCurrent)
					{
						global::System.Transactions.Diagnostics.TransactionScopeDisposedTraceRecord.Trace(SR.GetString("TraceSourceBase"), TransactionTraceIdentifier.Empty);
					}
					else
					{
						global::System.Transactions.Diagnostics.TransactionScopeDisposedTraceRecord.Trace(SR.GetString("TraceSourceBase"), this.expectedCurrent.TransactionTraceId);
					}
				}
				if (null != this.expectedCurrent)
				{
					if (!this.complete)
					{
						if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
						{
							global::System.Transactions.Diagnostics.TransactionScopeIncompleteTraceRecord.Trace(SR.GetString("TraceSourceBase"), this.expectedCurrent.TransactionTraceId);
						}
						Transaction transaction = this.committableTransaction;
						if (transaction == null)
						{
							transaction = this.dependentTransaction;
						}
						transaction.Rollback();
					}
					else if (null != this.committableTransaction)
					{
						this.committableTransaction.Commit();
					}
					else
					{
						this.dependentTransaction.Complete();
					}
				}
			}
			finally
			{
				if (this.scopeTimer != null)
				{
					this.scopeTimer.Dispose();
				}
				if (null != this.committableTransaction)
				{
					this.committableTransaction.Dispose();
					this.expectedCurrent.Dispose();
				}
				if (null != this.dependentTransaction)
				{
					this.dependentTransaction.Dispose();
				}
			}
		}

		// Token: 0x0600032F RID: 815 RVA: 0x000336EC File Offset: 0x00032AEC
		public void Complete()
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.Complete");
			}
			if (this.disposed)
			{
				throw new ObjectDisposedException("TransactionScope");
			}
			if (this.complete)
			{
				throw TransactionException.CreateInvalidOperationException(SR.GetString("TraceSourceBase"), SR.GetString("DisposeScope"), null);
			}
			this.complete = true;
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionScope.Complete");
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0003376C File Offset: 0x00032B6C
		private static void TimerCallback(object state)
		{
			TransactionScope transactionScope = state as TransactionScope;
			if (transactionScope == null)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Critical)
				{
					global::System.Transactions.Diagnostics.InternalErrorTraceRecord.Trace(SR.GetString("TraceSourceBase"), SR.GetString("TransactionScopeTimerObjectInvalid"));
				}
				throw TransactionException.Create(SR.GetString("TraceSourceBase"), SR.GetString("InternalError") + SR.GetString("TransactionScopeTimerObjectInvalid"), null);
			}
			transactionScope.Timeout();
		}

		// Token: 0x06000331 RID: 817 RVA: 0x000337D4 File Offset: 0x00032BD4
		private void Timeout()
		{
			if (!this.complete && null != this.expectedCurrent)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
				{
					global::System.Transactions.Diagnostics.TransactionScopeTimeoutTraceRecord.Trace(SR.GetString("TraceSourceBase"), this.expectedCurrent.TransactionTraceId);
				}
				try
				{
					this.expectedCurrent.Rollback();
				}
				catch (ObjectDisposedException ex)
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceBase"), ex);
					}
				}
				catch (TransactionException ex2)
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceBase"), ex2);
					}
				}
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00033890 File Offset: 0x00032C90
		private void CommonInitialize()
		{
			this.complete = false;
			this.dependentTransaction = null;
			this.disposed = false;
			this.committableTransaction = null;
			this.expectedCurrent = null;
			this.scopeTimer = null;
			this.scopeThread = Thread.CurrentThread;
			Transaction.GetCurrentTransactionAndScope(out this.savedCurrent, out this.savedCurrentScope, out this.threadContextData, out this.contextTransaction);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x000338F0 File Offset: 0x00032CF0
		private void PushScope()
		{
			if (!this.interopModeSpecified)
			{
				this.interopOption = Transaction.InteropMode(this.savedCurrentScope);
			}
			this.SetCurrent(this.expectedCurrent);
			this.threadContextData.CurrentScope = this;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00033930 File Offset: 0x00032D30
		private void PopScope()
		{
			this.threadContextData.CurrentScope = this.savedCurrentScope;
			this.RestoreCurrent();
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00033954 File Offset: 0x00032D54
		private void SetCurrent(Transaction newCurrent)
		{
			if (this.dependentTransaction == null && this.committableTransaction == null && newCurrent != null)
			{
				this.dependentTransaction = newCurrent.DependentClone(DependentCloneOption.RollbackIfNotComplete);
			}
			switch (this.interopOption)
			{
			case EnterpriseServicesInteropOption.None:
				this.threadContextData.CurrentTransaction = newCurrent;
				return;
			case EnterpriseServicesInteropOption.Automatic:
				Transaction.VerifyEnterpriseServicesOk();
				if (Transaction.UseServiceDomainForCurrent())
				{
					this.PushServiceDomain(newCurrent);
					return;
				}
				this.threadContextData.CurrentTransaction = newCurrent;
				return;
			case EnterpriseServicesInteropOption.Full:
				Transaction.VerifyEnterpriseServicesOk();
				this.PushServiceDomain(newCurrent);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000336 RID: 822 RVA: 0x000339E8 File Offset: 0x00032DE8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void PushServiceDomain(Transaction newCurrent)
		{
			if ((newCurrent != null && newCurrent.Equals(ContextUtil.SystemTransaction)) || (newCurrent == null && ContextUtil.SystemTransaction == null))
			{
				return;
			}
			ServiceConfig serviceConfig = new ServiceConfig();
			try
			{
				if (newCurrent != null)
				{
					serviceConfig.Synchronization = SynchronizationOption.RequiresNew;
					ServiceDomain.Enter(serviceConfig);
					this.createdDoubleServiceDomain = true;
					serviceConfig.Synchronization = SynchronizationOption.Required;
					serviceConfig.BringYourOwnSystemTransaction = newCurrent;
				}
				ServiceDomain.Enter(serviceConfig);
				this.createdServiceDomain = true;
			}
			catch (COMException ex)
			{
				if (global::System.Transactions.Oletx.NativeMethods.XACT_E_NOTRANSACTION == ex.ErrorCode)
				{
					throw TransactionException.Create(SR.GetString("TraceSourceBase"), SR.GetString("TransactionAlreadyOver"), ex);
				}
				throw TransactionException.Create(SR.GetString("TraceSourceBase"), ex.Message, ex);
			}
			finally
			{
				if (!this.createdServiceDomain && this.createdDoubleServiceDomain)
				{
					ServiceDomain.Leave();
				}
			}
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00033AF4 File Offset: 0x00032EF4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void JitSafeLeaveServiceDomain()
		{
			if (this.createdDoubleServiceDomain)
			{
				ServiceDomain.Leave();
			}
			ServiceDomain.Leave();
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00033B18 File Offset: 0x00032F18
		private void RestoreCurrent()
		{
			if (this.createdServiceDomain)
			{
				this.JitSafeLeaveServiceDomain();
			}
			this.threadContextData.CurrentTransaction = this.contextTransaction;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00033B44 File Offset: 0x00032F44
		private void ValidateInteropOption(EnterpriseServicesInteropOption interopOption)
		{
			if (interopOption < EnterpriseServicesInteropOption.None || interopOption > EnterpriseServicesInteropOption.Full)
			{
				throw new ArgumentOutOfRangeException("interopOption");
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00033B64 File Offset: 0x00032F64
		private void ValidateScopeTimeout(string paramName, TimeSpan scopeTimeout)
		{
			if (scopeTimeout < TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException(paramName);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600033B RID: 827 RVA: 0x00033B88 File Offset: 0x00032F88
		internal bool ScopeComplete
		{
			get
			{
				return this.complete;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600033C RID: 828 RVA: 0x00033B9C File Offset: 0x00032F9C
		internal EnterpriseServicesInteropOption InteropMode
		{
			get
			{
				return this.interopOption;
			}
		}

		// Token: 0x0400014F RID: 335
		private bool complete;

		// Token: 0x04000150 RID: 336
		private Transaction savedCurrent;

		// Token: 0x04000151 RID: 337
		private Transaction contextTransaction;

		// Token: 0x04000152 RID: 338
		private TransactionScope savedCurrentScope;

		// Token: 0x04000153 RID: 339
		private ContextData threadContextData;

		// Token: 0x04000154 RID: 340
		private Transaction expectedCurrent;

		// Token: 0x04000155 RID: 341
		private CommittableTransaction committableTransaction;

		// Token: 0x04000156 RID: 342
		private DependentTransaction dependentTransaction;

		// Token: 0x04000157 RID: 343
		private bool disposed;

		// Token: 0x04000158 RID: 344
		private Timer scopeTimer;

		// Token: 0x04000159 RID: 345
		private Thread scopeThread;

		// Token: 0x0400015A RID: 346
		private bool createdServiceDomain;

		// Token: 0x0400015B RID: 347
		private bool createdDoubleServiceDomain;

		// Token: 0x0400015C RID: 348
		private bool interopModeSpecified;

		// Token: 0x0400015D RID: 349
		private EnterpriseServicesInteropOption interopOption;
	}
}
