using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;
using System.Transactions.Diagnostics;

namespace System.Transactions.Oletx
{
	// Token: 0x02000089 RID: 137
	[Serializable]
	internal class OletxTransaction : ISerializable, IObjectReference
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000377 RID: 887 RVA: 0x00034348 File Offset: 0x00033748
		internal RealOletxTransaction RealTransaction
		{
			get
			{
				return this.realOletxTransaction;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000378 RID: 888 RVA: 0x0003435C File Offset: 0x0003375C
		internal Guid Identifier
		{
			get
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxTransaction.get_Identifier");
				}
				Guid identifier = this.realOletxTransaction.Identifier;
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxTransaction.get_Identifier");
				}
				return identifier;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000379 RID: 889 RVA: 0x000343AC File Offset: 0x000337AC
		internal TransactionStatus Status
		{
			get
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxTransaction.get_Status");
				}
				TransactionStatus status = this.realOletxTransaction.Status;
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxTransaction.get_Status");
				}
				return status;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600037A RID: 890 RVA: 0x000343FC File Offset: 0x000337FC
		internal Exception InnerException
		{
			get
			{
				return this.realOletxTransaction.innerException;
			}
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00034414 File Offset: 0x00033814
		internal OletxTransaction(RealOletxTransaction realOletxTransaction)
		{
			this.realOletxTransaction = realOletxTransaction;
			this.realOletxTransaction.OletxTransactionCreated();
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00034444 File Offset: 0x00033844
		protected OletxTransaction(SerializationInfo serializationInfo, StreamingContext context)
		{
			if (serializationInfo == null)
			{
				throw new ArgumentNullException("serializationInfo");
			}
			this.propagationTokenForDeserialize = (byte[])serializationInfo.GetValue("OletxTransactionPropagationToken", typeof(byte[]));
			if (this.propagationTokenForDeserialize.Length < 24)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument"), "serializationInfo");
			}
		}

		// Token: 0x0600037D RID: 893 RVA: 0x000344B4 File Offset: 0x000338B4
		public object GetRealObject(StreamingContext context)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "IObjectReference.GetRealObject");
			}
			if (this.propagationTokenForDeserialize == null)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Critical)
				{
					global::System.Transactions.Diagnostics.InternalErrorTraceRecord.Trace(SR.GetString("TraceSourceOletx"), SR.GetString("UnableToDeserializeTransaction"));
				}
				throw TransactionException.Create(SR.GetString("TraceSourceOletx"), SR.GetString("UnableToDeserializeTransactionInternalError"), null);
			}
			if (null != this.savedLtmPromotedTransaction)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "IObjectReference.GetRealObject");
				}
				return this.savedLtmPromotedTransaction;
			}
			Transaction transactionFromTransmitterPropagationToken = TransactionInterop.GetTransactionFromTransmitterPropagationToken(this.propagationTokenForDeserialize);
			this.savedLtmPromotedTransaction = transactionFromTransmitterPropagationToken;
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.TransactionDeserializedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), transactionFromTransmitterPropagationToken.internalTransaction.PromotedTransaction.TransactionTraceId);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "IObjectReference.GetRealObject");
			}
			return transactionFromTransmitterPropagationToken;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x000345A4 File Offset: 0x000339A4
		internal void Dispose()
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "IDisposable.Dispose");
			}
			if (Interlocked.CompareExchange(ref this.disposed, 1, 0) == 0)
			{
				this.realOletxTransaction.OletxTransactionDisposed();
			}
			GC.SuppressFinalize(this);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "IDisposable.Dispose");
			}
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0003460C File Offset: 0x00033A0C
		internal void Rollback()
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxTransaction.Rollback");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
			{
				global::System.Transactions.Diagnostics.TransactionRollbackCalledTraceRecord.Trace(SR.GetString("TraceSourceOletx"), this.TransactionTraceId);
			}
			this.realOletxTransaction.Rollback();
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxTransaction.Rollback");
			}
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00034678 File Offset: 0x00033A78
		internal IPromotedEnlistment EnlistVolatile(ISinglePhaseNotificationInternal singlePhaseNotification, EnlistmentOptions enlistmentOptions)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxTransaction.EnlistVolatile( ISinglePhaseNotificationInternal )");
			}
			if (this.realOletxTransaction == null || this.realOletxTransaction.TooLateForEnlistments)
			{
				throw TransactionException.Create(SR.GetString("TraceSourceOletx"), SR.GetString("TooLate"), null);
			}
			IPromotedEnlistment promotedEnlistment = this.realOletxTransaction.EnlistVolatile(singlePhaseNotification, enlistmentOptions, this);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxTransaction.EnlistVolatile( ISinglePhaseNotificationInternal )");
			}
			return promotedEnlistment;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x000346FC File Offset: 0x00033AFC
		internal IPromotedEnlistment EnlistVolatile(IEnlistmentNotificationInternal enlistmentNotification, EnlistmentOptions enlistmentOptions)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxTransaction.EnlistVolatile( IEnlistmentNotificationInternal )");
			}
			if (this.realOletxTransaction == null || this.realOletxTransaction.TooLateForEnlistments)
			{
				throw TransactionException.Create(SR.GetString("TraceSourceOletx"), SR.GetString("TooLate"), null);
			}
			IPromotedEnlistment promotedEnlistment = this.realOletxTransaction.EnlistVolatile(enlistmentNotification, enlistmentOptions, this);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxTransaction.EnlistVolatile( IEnlistmentNotificationInternal )");
			}
			return promotedEnlistment;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00034780 File Offset: 0x00033B80
		internal IPromotedEnlistment EnlistDurable(Guid resourceManagerIdentifier, ISinglePhaseNotificationInternal singlePhaseNotification, bool canDoSinglePhase, EnlistmentOptions enlistmentOptions)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxTransaction.EnlistDurable( ISinglePhaseNotificationInternal )");
			}
			if (this.realOletxTransaction == null || this.realOletxTransaction.TooLateForEnlistments)
			{
				throw TransactionException.Create(SR.GetString("TraceSourceOletx"), SR.GetString("TooLate"), null);
			}
			OletxTransactionManager oletxTransactionManagerInstance = this.realOletxTransaction.OletxTransactionManagerInstance;
			OletxResourceManager oletxResourceManager = oletxTransactionManagerInstance.FindOrRegisterResourceManager(resourceManagerIdentifier);
			OletxEnlistment oletxEnlistment = oletxResourceManager.EnlistDurable(this, canDoSinglePhase, singlePhaseNotification, enlistmentOptions);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxTransaction.EnlistDurable( ISinglePhaseNotificationInternal )");
			}
			return oletxEnlistment;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00034814 File Offset: 0x00033C14
		internal OletxDependentTransaction DependentClone(bool delayCommit)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxTransaction.DependentClone");
			}
			if (TransactionStatus.Aborted == this.Status)
			{
				throw TransactionAbortedException.Create(SR.GetString("TraceSourceOletx"), this.realOletxTransaction.innerException);
			}
			if (TransactionStatus.InDoubt == this.Status)
			{
				throw TransactionInDoubtException.Create(SR.GetString("TraceSourceOletx"), this.realOletxTransaction.innerException);
			}
			if (this.Status != TransactionStatus.Active)
			{
				throw TransactionException.Create(SR.GetString("TraceSourceOletx"), SR.GetString("TransactionAlreadyOver"), null);
			}
			OletxDependentTransaction oletxDependentTransaction = new OletxDependentTransaction(this.realOletxTransaction, delayCommit);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxTransaction.DependentClone");
			}
			return oletxDependentTransaction;
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000384 RID: 900 RVA: 0x000348D4 File Offset: 0x00033CD4
		internal TransactionTraceIdentifier TransactionTraceId
		{
			get
			{
				if (TransactionTraceIdentifier.Empty == this.traceIdentifier)
				{
					lock (this.realOletxTransaction)
					{
						if (TransactionTraceIdentifier.Empty == this.traceIdentifier)
						{
							try
							{
								TransactionTraceIdentifier transactionTraceIdentifier = new TransactionTraceIdentifier(this.realOletxTransaction.Identifier.ToString(), 0);
								Thread.MemoryBarrier();
								this.traceIdentifier = transactionTraceIdentifier;
							}
							catch (TransactionException ex)
							{
								if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
								{
									global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex);
								}
							}
						}
					}
				}
				return this.traceIdentifier;
			}
		}

		// Token: 0x06000385 RID: 901 RVA: 0x000349A0 File Offset: 0x00033DA0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo serializationInfo, StreamingContext context)
		{
			if (serializationInfo == null)
			{
				throw new ArgumentNullException("serializationInfo");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxTransaction.GetObjectData");
			}
			byte[] transmitterPropagationToken = TransactionInterop.GetTransmitterPropagationToken(this);
			serializationInfo.SetType(typeof(OletxTransaction));
			serializationInfo.AddValue("OletxTransactionPropagationToken", transmitterPropagationToken);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
			{
				global::System.Transactions.Diagnostics.TransactionSerializedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), this.TransactionTraceId);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "OletxTransaction.GetObjectData");
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000386 RID: 902 RVA: 0x00034A34 File Offset: 0x00033E34
		public virtual IsolationLevel IsolationLevel
		{
			get
			{
				return this.realOletxTransaction.TransactionIsolationLevel;
			}
		}

		// Token: 0x040001CF RID: 463
		protected const string propagationTokenString = "OletxTransactionPropagationToken";

		// Token: 0x040001D0 RID: 464
		internal RealOletxTransaction realOletxTransaction;

		// Token: 0x040001D1 RID: 465
		private byte[] propagationTokenForDeserialize;

		// Token: 0x040001D2 RID: 466
		protected int disposed;

		// Token: 0x040001D3 RID: 467
		internal Transaction savedLtmPromotedTransaction;

		// Token: 0x040001D4 RID: 468
		private TransactionTraceIdentifier traceIdentifier = TransactionTraceIdentifier.Empty;
	}
}
