using System;

namespace System.Web.UI.Design
{
	// Token: 0x0200037C RID: 892
	[Obsolete("The recommended alternative is System.Web.UI.Design.IControlDesignerTag and System.Web.UI.Design.IControlDesignerView. http://go.microsoft.com/fwlink/?linkid=14202")]
	public interface IHtmlControlDesignerBehavior
	{
		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06002126 RID: 8486
		// (set) Token: 0x06002127 RID: 8487
		HtmlControlDesigner Designer { get; set; }

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06002128 RID: 8488
		object DesignTimeElement { get; }

		// Token: 0x06002129 RID: 8489
		object GetAttribute(string attribute, bool ignoreCase);

		// Token: 0x0600212A RID: 8490
		void RemoveAttribute(string attribute, bool ignoreCase);

		// Token: 0x0600212B RID: 8491
		void SetAttribute(string attribute, object value, bool ignoreCase);

		// Token: 0x0600212C RID: 8492
		object GetStyleAttribute(string attribute, bool designTimeOnly, bool ignoreCase);

		// Token: 0x0600212D RID: 8493
		void RemoveStyleAttribute(string attribute, bool designTimeOnly, bool ignoreCase);

		// Token: 0x0600212E RID: 8494
		void SetStyleAttribute(string attribute, bool designTimeOnly, object value, bool ignoreCase);
	}
}
