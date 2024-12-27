using System;
using System.Data;
using System.Data.Common;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000032 RID: 50
	internal abstract class SmiConnection : IDisposable
	{
		// Token: 0x060001C1 RID: 449
		internal abstract string GetCurrentDatabase(SmiEventSink eventSink);

		// Token: 0x060001C2 RID: 450
		internal abstract void SetCurrentDatabase(string databaseName, SmiEventSink eventSink);

		// Token: 0x060001C3 RID: 451 RVA: 0x001CACD8 File Offset: 0x001CA0D8
		public virtual void Dispose()
		{
			ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x001CACF0 File Offset: 0x001CA0F0
		public virtual void Close(SmiEventSink eventSink)
		{
			ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060001C5 RID: 453
		internal abstract void BeginTransaction(string name, IsolationLevel level, SmiEventSink eventSink);

		// Token: 0x060001C6 RID: 454
		internal abstract void CommitTransaction(long transactionId, SmiEventSink eventSink);

		// Token: 0x060001C7 RID: 455
		internal abstract void CreateTransactionSavePoint(long transactionId, string name, SmiEventSink eventSink);

		// Token: 0x060001C8 RID: 456
		internal abstract byte[] GetDTCAddress(SmiEventSink eventSink);

		// Token: 0x060001C9 RID: 457
		internal abstract void EnlistTransaction(byte[] token, SmiEventSink eventSink);

		// Token: 0x060001CA RID: 458
		internal abstract byte[] PromoteTransaction(long transactionId, SmiEventSink eventSink);

		// Token: 0x060001CB RID: 459
		internal abstract void RollbackTransaction(long transactionId, string savePointName, SmiEventSink eventSink);
	}
}
