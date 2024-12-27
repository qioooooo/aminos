using System;
using System.Data.Common;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000039 RID: 57
	internal abstract class SmiEventStream : IDisposable
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000214 RID: 532
		internal abstract bool HasEvents { get; }

		// Token: 0x06000215 RID: 533
		internal abstract void Close(SmiEventSink sink);

		// Token: 0x06000216 RID: 534 RVA: 0x001CB5D4 File Offset: 0x001CA9D4
		public virtual void Dispose()
		{
			ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000217 RID: 535
		internal abstract void ProcessEvent(SmiEventSink sink);
	}
}
