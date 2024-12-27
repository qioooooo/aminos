using System;

namespace System.Web.UI.Design
{
	// Token: 0x02000387 RID: 903
	[Obsolete("The recommended alternative is System.Web.UI.Design.WebFormsRootDesigner. The WebFormsRootDesigner contains additional functionality and allows for more extensibility. To get the WebFormsRootDesigner use the RootDesigner property from your ControlDesigner. http://go.microsoft.com/fwlink/?linkid=14202")]
	public interface IWebFormsDocumentService
	{
		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x0600215C RID: 8540
		string DocumentUrl { get; }

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x0600215D RID: 8541
		bool IsLoading { get; }

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x0600215E RID: 8542
		// (remove) Token: 0x0600215F RID: 8543
		event EventHandler LoadComplete;

		// Token: 0x06002160 RID: 8544
		object CreateDiscardableUndoUnit();

		// Token: 0x06002161 RID: 8545
		void DiscardUndoUnit(object discardableUndoUnit);

		// Token: 0x06002162 RID: 8546
		void EnableUndo(bool enable);

		// Token: 0x06002163 RID: 8547
		void UpdateSelection();
	}
}
