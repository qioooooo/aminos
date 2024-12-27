using System;
using System.Runtime.InteropServices;
using System.Transactions;

namespace System.EnterpriseServices
{
	// Token: 0x02000046 RID: 70
	internal sealed class TransactionProxy : MarshalByRefObject, ITransactionProxy
	{
		// Token: 0x0600014E RID: 334 RVA: 0x00005955 File Offset: 0x00004955
		private void MapTxExceptionToHR(TransactionException txException)
		{
			this.MapTxExceptionToHR(txException, false);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00005960 File Offset: 0x00004960
		private void MapTxExceptionToHR(TransactionException txException, bool isInCommit)
		{
			TransactionAbortedException ex = txException as TransactionAbortedException;
			if (ex != null)
			{
				if (isInCommit)
				{
					TransactionProxyException.ThrowTransactionProxyException(Util.CONTEXT_E_ABORTED, ex);
				}
				else
				{
					TransactionProxyException.ThrowTransactionProxyException(Util.CONTEXT_E_ABORTING, ex);
				}
			}
			TransactionManagerCommunicationException ex2 = txException as TransactionManagerCommunicationException;
			if (ex2 != null)
			{
				TransactionProxyException.ThrowTransactionProxyException(Util.CONTEXT_E_TMNOTAVAILABLE, ex2);
			}
			COMException ex3 = txException.GetBaseException() as COMException;
			if (ex3 != null)
			{
				TransactionProxyException.ThrowTransactionProxyException(ex3.ErrorCode, txException);
				return;
			}
			TransactionProxyException.ThrowTransactionProxyException(Util.E_UNEXPECTED, txException);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000059D0 File Offset: 0x000049D0
		public TransactionProxy(DtcIsolationLevel isoLevel, int timeout)
		{
			this.committableTx = new CommittableTransaction(new TransactionOptions
			{
				Timeout = TimeSpan.FromSeconds((double)timeout),
				IsolationLevel = TransactionProxy.ConvertIsolationLevelFromDtc(isoLevel)
			});
			this.systemTx = this.committableTx.Clone();
			this.owned = false;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00005A2C File Offset: 0x00004A2C
		public Guid GetIdentifier()
		{
			try
			{
				ITransaction transaction = (ITransaction)TransactionInterop.GetDtcTransaction(this.systemTx);
				return this.systemTx.TransactionInformation.DistributedIdentifier;
			}
			catch (TransactionException ex)
			{
				this.MapTxExceptionToHR(ex);
			}
			return Guid.Empty;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00005A80 File Offset: 0x00004A80
		public bool IsReusable()
		{
			return false;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00005A83 File Offset: 0x00004A83
		public void SetOwnerGuid(Guid guid)
		{
			this.ownerGuid = guid;
			this.owned = true;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00005A93 File Offset: 0x00004A93
		public TransactionProxy(Transaction systemTx)
		{
			this.systemTx = systemTx;
			this.owned = false;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00005AAC File Offset: 0x00004AAC
		internal static IsolationLevel ConvertIsolationLevelFromDtc(DtcIsolationLevel proxyIsolationLevel)
		{
			if (proxyIsolationLevel <= DtcIsolationLevel.ISOLATIONLEVEL_CURSORSTABILITY)
			{
				if (proxyIsolationLevel == DtcIsolationLevel.ISOLATIONLEVEL_READUNCOMMITTED)
				{
					return IsolationLevel.ReadUncommitted;
				}
				if (proxyIsolationLevel == DtcIsolationLevel.ISOLATIONLEVEL_CURSORSTABILITY)
				{
					return IsolationLevel.ReadCommitted;
				}
			}
			else
			{
				if (proxyIsolationLevel == DtcIsolationLevel.ISOLATIONLEVEL_REPEATABLEREAD)
				{
					return IsolationLevel.RepeatableRead;
				}
				if (proxyIsolationLevel == DtcIsolationLevel.ISOLATIONLEVEL_SERIALIZABLE)
				{
					return IsolationLevel.Serializable;
				}
			}
			return IsolationLevel.Serializable;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00005AF8 File Offset: 0x00004AF8
		public void Commit(Guid guid)
		{
			try
			{
				if (this.committableTx == null)
				{
					Marshal.ThrowExceptionForHR(Util.E_UNEXPECTED);
				}
				else if (this.owned)
				{
					if (guid == this.ownerGuid)
					{
						this.committableTx.Commit();
					}
					else
					{
						Marshal.ThrowExceptionForHR(Util.E_UNEXPECTED);
					}
				}
				else
				{
					this.committableTx.Commit();
				}
			}
			catch (TransactionException ex)
			{
				this.MapTxExceptionToHR(ex, true);
			}
			finally
			{
				this.committableTx.Dispose();
				this.committableTx = null;
				this.systemTx = null;
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00005BA0 File Offset: 0x00004BA0
		public void Abort()
		{
			try
			{
				this.systemTx.Rollback();
			}
			catch (TransactionException ex)
			{
				this.MapTxExceptionToHR(ex);
			}
			finally
			{
				if (this.committableTx != null)
				{
					this.committableTx.Dispose();
					this.committableTx = null;
					this.systemTx = null;
				}
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005C0C File Offset: 0x00004C0C
		public IDtcTransaction Promote()
		{
			try
			{
				return TransactionInterop.GetDtcTransaction(this.systemTx);
			}
			catch (TransactionException ex)
			{
				this.MapTxExceptionToHR(ex);
			}
			return null;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005C44 File Offset: 0x00004C44
		public void CreateVoter(ITransactionVoterNotifyAsync2 voterNotification, out ITransactionVoterBallotAsync2 voterBallot)
		{
			voterBallot = null;
			try
			{
				if (voterNotification == null)
				{
					throw new ArgumentNullException("voterNotification");
				}
				voterBallot = new VoterBallot(voterNotification, this.systemTx);
			}
			catch (TransactionException ex)
			{
				this.MapTxExceptionToHR(ex);
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00005C8C File Offset: 0x00004C8C
		public DtcIsolationLevel GetIsolationLevel()
		{
			try
			{
				DtcIsolationLevel dtcIsolationLevel;
				switch (this.systemTx.IsolationLevel)
				{
				case IsolationLevel.Serializable:
					dtcIsolationLevel = DtcIsolationLevel.ISOLATIONLEVEL_SERIALIZABLE;
					break;
				case IsolationLevel.RepeatableRead:
					dtcIsolationLevel = DtcIsolationLevel.ISOLATIONLEVEL_REPEATABLEREAD;
					break;
				case IsolationLevel.ReadCommitted:
					dtcIsolationLevel = DtcIsolationLevel.ISOLATIONLEVEL_CURSORSTABILITY;
					break;
				case IsolationLevel.ReadUncommitted:
					dtcIsolationLevel = DtcIsolationLevel.ISOLATIONLEVEL_READUNCOMMITTED;
					break;
				default:
					dtcIsolationLevel = DtcIsolationLevel.ISOLATIONLEVEL_SERIALIZABLE;
					break;
				}
				return dtcIsolationLevel;
			}
			catch (TransactionException ex)
			{
				this.MapTxExceptionToHR(ex);
			}
			return DtcIsolationLevel.ISOLATIONLEVEL_SERIALIZABLE;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00005D08 File Offset: 0x00004D08
		internal Transaction SystemTransaction
		{
			get
			{
				return this.systemTx;
			}
		}

		// Token: 0x0400008D RID: 141
		private CommittableTransaction committableTx;

		// Token: 0x0400008E RID: 142
		private Transaction systemTx;

		// Token: 0x0400008F RID: 143
		private Guid ownerGuid;

		// Token: 0x04000090 RID: 144
		private bool owned;
	}
}
