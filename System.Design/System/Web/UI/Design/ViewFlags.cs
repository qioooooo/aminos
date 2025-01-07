using System;

namespace System.Web.UI.Design
{
	[Flags]
	public enum ViewFlags
	{
		CustomPaint = 1,
		DesignTimeHtmlRequiresLoadComplete = 2,
		TemplateEditing = 4
	}
}
