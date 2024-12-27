using System;

namespace System.Web.UI.Design
{
	// Token: 0x02000382 RID: 898
	[Obsolete("The recommended alternative is System.Web.UI.Design.IControlDesignerTag and System.Web.UI.Design.IControlDesignerView. http://go.microsoft.com/fwlink/?linkid=14202")]
	public interface IControlDesignerBehavior
	{
		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x0600214F RID: 8527
		object DesignTimeElementView { get; }

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06002150 RID: 8528
		// (set) Token: 0x06002151 RID: 8529
		string DesignTimeHtml { get; set; }

		// Token: 0x06002152 RID: 8530
		void OnTemplateModeChanged();
	}
}
