using System;

namespace System.Windows.Forms
{
	// Token: 0x020001E5 RID: 485
	internal interface ISupportOleDropSource
	{
		// Token: 0x0600133C RID: 4924
		void OnQueryContinueDrag(QueryContinueDragEventArgs qcdevent);

		// Token: 0x0600133D RID: 4925
		void OnGiveFeedback(GiveFeedbackEventArgs gfbevent);
	}
}
