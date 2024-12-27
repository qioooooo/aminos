using System;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000010 RID: 16
	public class TransactionInformation
	{
		// Token: 0x0600003E RID: 62 RVA: 0x00028720 File Offset: 0x00027B20
		internal TransactionInformation(InternalTransaction internalTransaction)
		{
			this.internalTransaction = internalTransaction;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003F RID: 63 RVA: 0x0002873C File Offset: 0x00027B3C
		public string LocalIdentifier
		{
			get
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "TransactionInformation.get_LocalIdentifier");
				}
				string transactionIdentifier;
				try
				{
					transactionIdentifier = this.internalTransaction.TransactionTraceId.TransactionIdentifier;
				}
				finally
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "TransactionInformation.get_LocalIdentifier");
					}
				}
				return transactionIdentifier;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000040 RID: 64 RVA: 0x000287B4 File Offset: 0x00027BB4
		public Guid DistributedIdentifier
		{
			get
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "TransactionInformation.get_DistributedIdentifier");
				}
				Guid guid;
				try
				{
					lock (this.internalTransaction)
					{
						guid = this.internalTransaction.State.get_Identifier(this.internalTransaction);
					}
				}
				finally
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "TransactionInformation.get_DistributedIdentifier");
					}
				}
				return guid;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00028860 File Offset: 0x00027C60
		public DateTime CreationTime
		{
			get
			{
				return new DateTime(this.internalTransaction.CreationTime);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00028880 File Offset: 0x00027C80
		public TransactionStatus Status
		{
			get
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "TransactionInformation.get_Status");
				}
				TransactionStatus transactionStatus;
				try
				{
					transactionStatus = this.internalTransaction.State.get_Status(this.internalTransaction);
				}
				finally
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "TransactionInformation.get_Status");
					}
				}
				return transactionStatus;
			}
		}

		// Token: 0x0400009F RID: 159
		private InternalTransaction internalTransaction;
	}
}
