using System;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000038 RID: 56
	internal class SmiEventSink_DeferedProcessing : SmiEventSink_Default
	{
		// Token: 0x06000212 RID: 530 RVA: 0x001CB5B0 File Offset: 0x001CA9B0
		internal SmiEventSink_DeferedProcessing(SmiEventSink parent)
			: base(parent)
		{
		}

		// Token: 0x06000213 RID: 531 RVA: 0x001CB5C4 File Offset: 0x001CA9C4
		protected override void DispatchMessages(bool ignoreNonFatalMessages)
		{
		}
	}
}
