using System;
using System.Data.Common;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000035 RID: 53
	internal abstract class SmiEventSink
	{
		// Token: 0x060001E6 RID: 486
		internal abstract void BatchCompleted();

		// Token: 0x060001E7 RID: 487 RVA: 0x001CAF9C File Offset: 0x001CA39C
		internal virtual void ParameterAvailable(SmiParameterMetaData metaData, SmiTypedGetterSetter paramValue, int ordinal)
		{
			ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060001E8 RID: 488
		internal abstract void DefaultDatabaseChanged(string databaseName);

		// Token: 0x060001E9 RID: 489
		internal abstract void MessagePosted(int number, byte state, byte errorClass, string server, string message, string procedure, int lineNumber);

		// Token: 0x060001EA RID: 490
		internal abstract void MetaDataAvailable(SmiQueryMetaData[] metaData, bool nextEventIsRow);

		// Token: 0x060001EB RID: 491 RVA: 0x001CAFB4 File Offset: 0x001CA3B4
		internal virtual void RowAvailable(SmiTypedGetterSetter rowData)
		{
			ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060001EC RID: 492
		internal abstract void StatementCompleted(int rowsAffected);

		// Token: 0x060001ED RID: 493
		internal abstract void TransactionCommitted(long transactionId);

		// Token: 0x060001EE RID: 494
		internal abstract void TransactionDefected(long transactionId);

		// Token: 0x060001EF RID: 495
		internal abstract void TransactionEnlisted(long transactionId);

		// Token: 0x060001F0 RID: 496
		internal abstract void TransactionEnded(long transactionId);

		// Token: 0x060001F1 RID: 497
		internal abstract void TransactionRolledBack(long transactionId);

		// Token: 0x060001F2 RID: 498
		internal abstract void TransactionStarted(long transactionId);

		// Token: 0x060001F3 RID: 499 RVA: 0x001CAFCC File Offset: 0x001CA3CC
		internal virtual void ParametersAvailable(SmiParameterMetaData[] metaData, ITypedGettersV3 paramValues)
		{
			ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x001CAFE4 File Offset: 0x001CA3E4
		internal virtual void RowAvailable(ITypedGettersV3 rowData)
		{
			ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x001CAFFC File Offset: 0x001CA3FC
		internal virtual void RowAvailable(ITypedGetters rowData)
		{
			ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}
	}
}
