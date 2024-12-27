using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Transactions.Diagnostics;
using System.Transactions.Oletx;

namespace System.Transactions
{
	// Token: 0x02000066 RID: 102
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public static class TransactionInterop
	{
		// Token: 0x060002CC RID: 716 RVA: 0x000306B8 File Offset: 0x0002FAB8
		internal static global::System.Transactions.Oletx.OletxTransaction ConvertToOletxTransaction(Transaction transaction)
		{
			if (null == transaction)
			{
				throw new ArgumentNullException("transaction");
			}
			if (transaction.Disposed)
			{
				throw new ObjectDisposedException("Transaction");
			}
			if (transaction.complete)
			{
				throw TransactionException.CreateTransactionCompletedException(SR.GetString("TraceSourceLtm"));
			}
			return transaction.Promote();
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0003070C File Offset: 0x0002FB0C
		public static byte[] GetExportCookie(Transaction transaction, byte[] whereabouts)
		{
			if (!TransactionManager._platformValidated)
			{
				TransactionManager.ValidatePlatform();
			}
			byte[] array = null;
			if (null == transaction)
			{
				throw new ArgumentNullException("transaction");
			}
			if (whereabouts == null)
			{
				throw new ArgumentNullException("whereabouts");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "TransactionInterop.GetExportCookie");
			}
			byte[] array2 = new byte[whereabouts.Length];
			Array.Copy(whereabouts, array2, whereabouts.Length);
			whereabouts = array2;
			int num = 0;
			uint num2 = 0U;
			global::System.Transactions.Oletx.CoTaskMemHandle coTaskMemHandle = null;
			global::System.Transactions.Oletx.OletxTransaction oletxTransaction = TransactionInterop.ConvertToOletxTransaction(transaction);
			try
			{
				oletxTransaction.realOletxTransaction.TransactionShim.Export(Convert.ToUInt32(whereabouts.Length), whereabouts, out num, out num2, out coTaskMemHandle);
				array = new byte[num2];
				Marshal.Copy(coTaskMemHandle.DangerousGetHandle(), array, 0, Convert.ToInt32(num2));
			}
			catch (COMException ex)
			{
				global::System.Transactions.Oletx.OletxTransactionManager.ProxyException(ex);
				throw TransactionManagerCommunicationException.Create(SR.GetString("TraceSourceOletx"), ex);
			}
			finally
			{
				if (coTaskMemHandle != null)
				{
					coTaskMemHandle.Close();
				}
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "TransactionInterop.GetExportCookie");
			}
			return array;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00030840 File Offset: 0x0002FC40
		public static Transaction GetTransactionFromExportCookie(byte[] cookie)
		{
			if (!TransactionManager._platformValidated)
			{
				TransactionManager.ValidatePlatform();
			}
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			if (cookie.Length < 32)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument"), "cookie");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "TransactionInterop.GetTransactionFromExportCookie");
			}
			byte[] array = new byte[cookie.Length];
			Array.Copy(cookie, array, cookie.Length);
			cookie = array;
			global::System.Transactions.Oletx.ITransactionShim transactionShim = null;
			Guid empty = Guid.Empty;
			global::System.Transactions.Oletx.OletxTransactionIsolationLevel oletxTransactionIsolationLevel = global::System.Transactions.Oletx.OletxTransactionIsolationLevel.ISOLATIONLEVEL_SERIALIZABLE;
			global::System.Transactions.Oletx.OutcomeEnlistment outcomeEnlistment = null;
			byte[] array2 = new byte[16];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = cookie[i + 16];
			}
			Guid guid = new Guid(array2);
			Transaction transaction = TransactionManager.FindPromotedTransaction(guid);
			if (null != transaction)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "TransactionInterop.GetTransactionFromExportCookie");
				}
				return transaction;
			}
			global::System.Transactions.Oletx.OletxTransactionManager distributedTransactionManager = TransactionManager.DistributedTransactionManager;
			distributedTransactionManager.dtcTransactionManagerLock.AcquireReaderLock(-1);
			try
			{
				outcomeEnlistment = new global::System.Transactions.Oletx.OutcomeEnlistment();
				IntPtr intPtr = IntPtr.Zero;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					intPtr = global::System.Transactions.Oletx.HandleTable.AllocHandle(outcomeEnlistment);
					distributedTransactionManager.DtcTransactionManager.ProxyShimFactory.Import(Convert.ToUInt32(cookie.Length), cookie, intPtr, out empty, out oletxTransactionIsolationLevel, out transactionShim);
				}
				finally
				{
					if (transactionShim == null && intPtr != IntPtr.Zero)
					{
						global::System.Transactions.Oletx.HandleTable.FreeHandle(intPtr);
					}
				}
			}
			catch (COMException ex)
			{
				global::System.Transactions.Oletx.OletxTransactionManager.ProxyException(ex);
				throw TransactionManagerCommunicationException.Create(SR.GetString("TraceSourceOletx"), ex);
			}
			finally
			{
				distributedTransactionManager.dtcTransactionManagerLock.ReleaseReaderLock();
			}
			global::System.Transactions.Oletx.RealOletxTransaction realOletxTransaction = new global::System.Transactions.Oletx.RealOletxTransaction(distributedTransactionManager, transactionShim, outcomeEnlistment, empty, oletxTransactionIsolationLevel, false);
			global::System.Transactions.Oletx.OletxTransaction oletxTransaction = new global::System.Transactions.Oletx.OletxTransaction(realOletxTransaction);
			transaction = TransactionManager.FindOrCreatePromotedTransaction(guid, oletxTransaction);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "TransactionInterop.GetTransactionFromExportCookie");
			}
			return transaction;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00030A4C File Offset: 0x0002FE4C
		public static byte[] GetTransmitterPropagationToken(Transaction transaction)
		{
			if (!TransactionManager._platformValidated)
			{
				TransactionManager.ValidatePlatform();
			}
			if (null == transaction)
			{
				throw new ArgumentNullException("transaction");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "TransactionInterop.GetTransmitterPropagationToken");
			}
			global::System.Transactions.Oletx.OletxTransaction oletxTransaction = TransactionInterop.ConvertToOletxTransaction(transaction);
			byte[] transmitterPropagationToken = TransactionInterop.GetTransmitterPropagationToken(oletxTransaction);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "TransactionInterop.GetTransmitterPropagationToken");
			}
			return transmitterPropagationToken;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00030AC0 File Offset: 0x0002FEC0
		internal static byte[] GetTransmitterPropagationToken(global::System.Transactions.Oletx.OletxTransaction oletxTx)
		{
			byte[] array = null;
			global::System.Transactions.Oletx.CoTaskMemHandle coTaskMemHandle = null;
			uint num = 0U;
			try
			{
				oletxTx.realOletxTransaction.TransactionShim.GetPropagationToken(out num, out coTaskMemHandle);
				array = new byte[num];
				Marshal.Copy(coTaskMemHandle.DangerousGetHandle(), array, 0, Convert.ToInt32(num));
			}
			catch (COMException ex)
			{
				global::System.Transactions.Oletx.OletxTransactionManager.ProxyException(ex);
				throw;
			}
			finally
			{
				if (coTaskMemHandle != null)
				{
					coTaskMemHandle.Close();
				}
			}
			return array;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00030B50 File Offset: 0x0002FF50
		public static Transaction GetTransactionFromTransmitterPropagationToken(byte[] propagationToken)
		{
			if (!TransactionManager._platformValidated)
			{
				TransactionManager.ValidatePlatform();
			}
			if (propagationToken == null)
			{
				throw new ArgumentNullException("propagationToken");
			}
			if (propagationToken.Length < 24)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument"), "propagationToken");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "TransactionInterop.GetTransactionFromTransmitterPropagationToken");
			}
			byte[] array = new byte[16];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = propagationToken[i + 8];
			}
			Guid guid = new Guid(array);
			Transaction transaction = TransactionManager.FindPromotedTransaction(guid);
			if (null != transaction)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "TransactionInterop.GetTransactionFromTransmitterPropagationToken");
				}
				return transaction;
			}
			global::System.Transactions.Oletx.OletxTransaction oletxTransactionFromTransmitterPropigationToken = TransactionInterop.GetOletxTransactionFromTransmitterPropigationToken(propagationToken);
			Transaction transaction2 = TransactionManager.FindOrCreatePromotedTransaction(guid, oletxTransactionFromTransmitterPropigationToken);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "TransactionInterop.GetTransactionFromTransmitterPropagationToken");
			}
			return transaction2;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00030C34 File Offset: 0x00030034
		internal static global::System.Transactions.Oletx.OletxTransaction GetOletxTransactionFromTransmitterPropigationToken(byte[] propagationToken)
		{
			global::System.Transactions.Oletx.ITransactionShim transactionShim = null;
			if (propagationToken == null)
			{
				throw new ArgumentNullException("propagationToken");
			}
			if (propagationToken.Length < 24)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument"), "propagationToken");
			}
			byte[] array = new byte[propagationToken.Length];
			Array.Copy(propagationToken, array, propagationToken.Length);
			propagationToken = array;
			global::System.Transactions.Oletx.OletxTransactionManager distributedTransactionManager = TransactionManager.DistributedTransactionManager;
			distributedTransactionManager.dtcTransactionManagerLock.AcquireReaderLock(-1);
			global::System.Transactions.Oletx.OutcomeEnlistment outcomeEnlistment;
			Guid guid;
			global::System.Transactions.Oletx.OletxTransactionIsolationLevel oletxTransactionIsolationLevel;
			try
			{
				outcomeEnlistment = new global::System.Transactions.Oletx.OutcomeEnlistment();
				IntPtr intPtr = IntPtr.Zero;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					intPtr = global::System.Transactions.Oletx.HandleTable.AllocHandle(outcomeEnlistment);
					distributedTransactionManager.DtcTransactionManager.ProxyShimFactory.ReceiveTransaction(Convert.ToUInt32(propagationToken.Length), propagationToken, intPtr, out guid, out oletxTransactionIsolationLevel, out transactionShim);
				}
				finally
				{
					if (transactionShim == null && intPtr != IntPtr.Zero)
					{
						global::System.Transactions.Oletx.HandleTable.FreeHandle(intPtr);
					}
				}
			}
			catch (COMException ex)
			{
				global::System.Transactions.Oletx.OletxTransactionManager.ProxyException(ex);
				throw TransactionManagerCommunicationException.Create(SR.GetString("TraceSourceOletx"), ex);
			}
			finally
			{
				distributedTransactionManager.dtcTransactionManagerLock.ReleaseReaderLock();
			}
			global::System.Transactions.Oletx.RealOletxTransaction realOletxTransaction = new global::System.Transactions.Oletx.RealOletxTransaction(distributedTransactionManager, transactionShim, outcomeEnlistment, guid, oletxTransactionIsolationLevel, false);
			return new global::System.Transactions.Oletx.OletxTransaction(realOletxTransaction);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00030D7C File Offset: 0x0003017C
		public static IDtcTransaction GetDtcTransaction(Transaction transaction)
		{
			if (!TransactionManager._platformValidated)
			{
				TransactionManager.ValidatePlatform();
			}
			if (null == transaction)
			{
				throw new ArgumentNullException("transaction");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "TransactionInterop.GetDtcTransaction");
			}
			IDtcTransaction dtcTransaction = null;
			global::System.Transactions.Oletx.OletxTransaction oletxTransaction = TransactionInterop.ConvertToOletxTransaction(transaction);
			try
			{
				oletxTransaction.realOletxTransaction.TransactionShim.GetITransactionNative(out dtcTransaction);
			}
			catch (COMException ex)
			{
				global::System.Transactions.Oletx.OletxTransactionManager.ProxyException(ex);
				throw;
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "TransactionInterop.GetDtcTransaction");
			}
			return dtcTransaction;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00030E24 File Offset: 0x00030224
		public static Transaction GetTransactionFromDtcTransaction(IDtcTransaction transactionNative)
		{
			if (!TransactionManager._platformValidated)
			{
				TransactionManager.ValidatePlatform();
			}
			bool flag = false;
			global::System.Transactions.Oletx.ITransactionShim transactionShim = null;
			Guid empty = Guid.Empty;
			global::System.Transactions.Oletx.OletxTransactionIsolationLevel oletxTransactionIsolationLevel = global::System.Transactions.Oletx.OletxTransactionIsolationLevel.ISOLATIONLEVEL_SERIALIZABLE;
			global::System.Transactions.Oletx.OutcomeEnlistment outcomeEnlistment = null;
			if (transactionNative == null)
			{
				throw new ArgumentNullException("transactionNative");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "TransactionInterop.GetTransactionFromDtc");
			}
			global::System.Transactions.Oletx.ITransactionNativeInternal transactionNativeInternal = transactionNative as global::System.Transactions.Oletx.ITransactionNativeInternal;
			if (transactionNativeInternal == null)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument"), "transactionNative");
			}
			global::System.Transactions.Oletx.OletxXactTransInfo oletxXactTransInfo;
			try
			{
				transactionNativeInternal.GetTransactionInfo(out oletxXactTransInfo);
			}
			catch (COMException ex)
			{
				if (global::System.Transactions.Oletx.NativeMethods.XACT_E_NOTRANSACTION != ex.ErrorCode)
				{
					throw;
				}
				flag = true;
				oletxXactTransInfo.uow = Guid.Empty;
			}
			global::System.Transactions.Oletx.OletxTransactionManager distributedTransactionManager = TransactionManager.DistributedTransactionManager;
			Transaction transaction;
			if (!flag)
			{
				transaction = TransactionManager.FindPromotedTransaction(oletxXactTransInfo.uow);
				if (null != transaction)
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "TransactionInterop.GetTransactionFromDtcTransaction");
					}
					return transaction;
				}
				distributedTransactionManager.dtcTransactionManagerLock.AcquireReaderLock(-1);
				try
				{
					outcomeEnlistment = new global::System.Transactions.Oletx.OutcomeEnlistment();
					IntPtr intPtr = IntPtr.Zero;
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						intPtr = global::System.Transactions.Oletx.HandleTable.AllocHandle(outcomeEnlistment);
						distributedTransactionManager.DtcTransactionManager.ProxyShimFactory.CreateTransactionShim(transactionNative, intPtr, out empty, out oletxTransactionIsolationLevel, out transactionShim);
					}
					finally
					{
						if (transactionShim == null && intPtr != IntPtr.Zero)
						{
							global::System.Transactions.Oletx.HandleTable.FreeHandle(intPtr);
						}
					}
				}
				catch (COMException ex2)
				{
					global::System.Transactions.Oletx.OletxTransactionManager.ProxyException(ex2);
					throw;
				}
				finally
				{
					distributedTransactionManager.dtcTransactionManagerLock.ReleaseReaderLock();
				}
				global::System.Transactions.Oletx.RealOletxTransaction realOletxTransaction = new global::System.Transactions.Oletx.RealOletxTransaction(distributedTransactionManager, transactionShim, outcomeEnlistment, empty, oletxTransactionIsolationLevel, false);
				global::System.Transactions.Oletx.OletxTransaction oletxTransaction = new global::System.Transactions.Oletx.OletxTransaction(realOletxTransaction);
				transaction = TransactionManager.FindOrCreatePromotedTransaction(oletxXactTransInfo.uow, oletxTransaction);
			}
			else
			{
				global::System.Transactions.Oletx.RealOletxTransaction realOletxTransaction = new global::System.Transactions.Oletx.RealOletxTransaction(distributedTransactionManager, null, null, empty, global::System.Transactions.Oletx.OletxTransactionIsolationLevel.ISOLATIONLEVEL_SERIALIZABLE, false);
				global::System.Transactions.Oletx.OletxTransaction oletxTransaction = new global::System.Transactions.Oletx.OletxTransaction(realOletxTransaction);
				transaction = new Transaction(oletxTransaction);
				TransactionManager.FireDistributedTransactionStarted(transaction);
				oletxTransaction.savedLtmPromotedTransaction = transaction;
				InternalTransaction.DistributedTransactionOutcome(transaction.internalTransaction, TransactionStatus.InDoubt);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "TransactionInterop.GetTransactionFromDtc");
			}
			return transaction;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00031070 File Offset: 0x00030470
		public static byte[] GetWhereabouts()
		{
			if (!TransactionManager._platformValidated)
			{
				TransactionManager.ValidatePlatform();
			}
			byte[] array = null;
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "TransactionInterop.GetWhereabouts");
			}
			global::System.Transactions.Oletx.OletxTransactionManager distributedTransactionManager = TransactionManager.DistributedTransactionManager;
			if (distributedTransactionManager == null)
			{
				throw new ArgumentException(SR.GetString("ArgumentWrongType"), "transactionManager");
			}
			distributedTransactionManager.dtcTransactionManagerLock.AcquireReaderLock(-1);
			try
			{
				array = distributedTransactionManager.DtcTransactionManager.Whereabouts;
			}
			finally
			{
				distributedTransactionManager.dtcTransactionManagerLock.ReleaseReaderLock();
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "TransactionInterop.GetWhereabouts");
			}
			return array;
		}
	}
}
