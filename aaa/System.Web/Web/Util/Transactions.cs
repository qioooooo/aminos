using System;
using System.EnterpriseServices;
using System.Security.Permissions;

namespace System.Web.Util
{
	// Token: 0x02000795 RID: 1941
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class Transactions
	{
		// Token: 0x06005D2F RID: 23855 RVA: 0x001753C8 File Offset: 0x001743C8
		public static void InvokeTransacted(TransactedCallback callback, TransactionOption mode)
		{
			bool flag = false;
			Transactions.InvokeTransacted(callback, mode, ref flag);
		}

		// Token: 0x06005D30 RID: 23856 RVA: 0x001753E0 File Offset: 0x001743E0
		public static void InvokeTransacted(TransactedCallback callback, TransactionOption mode, ref bool transactionAborted)
		{
			HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Medium, "Transaction_not_supported_in_low_trust");
			bool flag = false;
			if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major <= 4)
			{
				throw new PlatformNotSupportedException(SR.GetString("RequiresNT"));
			}
			if (mode == TransactionOption.Disabled)
			{
				flag = true;
			}
			if (flag)
			{
				callback();
				transactionAborted = false;
				return;
			}
			Transactions.TransactedInvocation transactedInvocation = new Transactions.TransactedInvocation(callback);
			TransactedExecCallback transactedExecCallback = new TransactedExecCallback(transactedInvocation.ExecuteTransactedCode);
			PerfCounters.IncrementCounter(AppPerfCounter.TRANSACTIONS_PENDING);
			int num;
			try
			{
				num = UnsafeNativeMethods.TransactManagedCallback(transactedExecCallback, (int)mode);
			}
			finally
			{
				PerfCounters.DecrementCounter(AppPerfCounter.TRANSACTIONS_PENDING);
			}
			if (transactedInvocation.Error != null)
			{
				throw new HttpException(null, transactedInvocation.Error);
			}
			PerfCounters.IncrementCounter(AppPerfCounter.TRANSACTIONS_TOTAL);
			if (num == 1)
			{
				PerfCounters.IncrementCounter(AppPerfCounter.TRANSACTIONS_COMMITTED);
				transactionAborted = false;
				return;
			}
			if (num == 0)
			{
				PerfCounters.IncrementCounter(AppPerfCounter.TRANSACTIONS_ABORTED);
				transactionAborted = true;
				return;
			}
			throw new HttpException(SR.GetString("Cannot_execute_transacted_code"));
		}

		// Token: 0x02000796 RID: 1942
		internal class Utils
		{
			// Token: 0x06005D32 RID: 23858 RVA: 0x001754CC File Offset: 0x001744CC
			private Utils()
			{
			}

			// Token: 0x170017E5 RID: 6117
			// (get) Token: 0x06005D33 RID: 23859 RVA: 0x001754D4 File Offset: 0x001744D4
			internal static bool IsInTransaction
			{
				get
				{
					bool flag = false;
					try
					{
						flag = ContextUtil.IsInTransaction;
					}
					catch
					{
					}
					return flag;
				}
			}

			// Token: 0x170017E6 RID: 6118
			// (get) Token: 0x06005D34 RID: 23860 RVA: 0x00175500 File Offset: 0x00174500
			internal static bool AbortPending
			{
				get
				{
					bool flag = false;
					try
					{
						if (ContextUtil.MyTransactionVote == TransactionVote.Abort)
						{
							flag = true;
						}
					}
					catch
					{
					}
					return flag;
				}
			}
		}

		// Token: 0x02000797 RID: 1943
		internal class TransactedInvocation
		{
			// Token: 0x06005D35 RID: 23861 RVA: 0x00175530 File Offset: 0x00174530
			internal TransactedInvocation(TransactedCallback callback)
			{
				this._callback = callback;
			}

			// Token: 0x06005D36 RID: 23862 RVA: 0x00175540 File Offset: 0x00174540
			internal int ExecuteTransactedCode()
			{
				TransactedExecState transactedExecState = TransactedExecState.CommitPending;
				try
				{
					this._callback();
					if (Transactions.Utils.AbortPending)
					{
						transactedExecState = TransactedExecState.AbortPending;
					}
				}
				catch (Exception ex)
				{
					this._error = ex;
					transactedExecState = TransactedExecState.Error;
				}
				return (int)transactedExecState;
			}

			// Token: 0x170017E7 RID: 6119
			// (get) Token: 0x06005D37 RID: 23863 RVA: 0x00175584 File Offset: 0x00174584
			internal Exception Error
			{
				get
				{
					return this._error;
				}
			}

			// Token: 0x040031C0 RID: 12736
			private TransactedCallback _callback;

			// Token: 0x040031C1 RID: 12737
			private Exception _error;
		}
	}
}
