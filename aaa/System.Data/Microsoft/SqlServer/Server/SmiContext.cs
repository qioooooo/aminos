using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Security.Principal;
using System.Transactions;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000033 RID: 51
	internal abstract class SmiContext
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060001CD RID: 461
		// (remove) Token: 0x060001CE RID: 462
		internal abstract event EventHandler OutOfScope;

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060001CF RID: 463
		internal abstract SmiConnection ContextConnection { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060001D0 RID: 464
		internal abstract long ContextTransactionId { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060001D1 RID: 465
		internal abstract Transaction ContextTransaction { get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060001D2 RID: 466
		internal abstract bool HasContextPipe { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060001D3 RID: 467
		internal abstract WindowsIdentity WindowsIdentity { get; }

		// Token: 0x060001D4 RID: 468
		internal abstract SmiRecordBuffer CreateRecordBuffer(SmiExtendedMetaData[] columnMetaData, SmiEventSink eventSink);

		// Token: 0x060001D5 RID: 469
		internal abstract SmiRequestExecutor CreateRequestExecutor(string commandText, CommandType commandType, SmiParameterMetaData[] parameterMetaData, SmiEventSink eventSink);

		// Token: 0x060001D6 RID: 470
		internal abstract object GetContextValue(int key);

		// Token: 0x060001D7 RID: 471
		internal abstract void GetTriggerInfo(SmiEventSink eventSink, out bool[] columnsUpdated, out TriggerAction action, out SqlXml eventInstanceData);

		// Token: 0x060001D8 RID: 472
		internal abstract void SendMessageToPipe(string message, SmiEventSink eventSink);

		// Token: 0x060001D9 RID: 473
		internal abstract void SendResultsStartToPipe(SmiRecordBuffer recordBuffer, SmiEventSink eventSink);

		// Token: 0x060001DA RID: 474
		internal abstract void SendResultsRowToPipe(SmiRecordBuffer recordBuffer, SmiEventSink eventSink);

		// Token: 0x060001DB RID: 475
		internal abstract void SendResultsEndToPipe(SmiRecordBuffer recordBuffer, SmiEventSink eventSink);

		// Token: 0x060001DC RID: 476
		internal abstract void SetContextValue(int key, object value);

		// Token: 0x060001DD RID: 477 RVA: 0x001CAD1C File Offset: 0x001CA11C
		internal virtual SmiStream GetScratchStream(SmiEventSink sink)
		{
			ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
			return null;
		}
	}
}
