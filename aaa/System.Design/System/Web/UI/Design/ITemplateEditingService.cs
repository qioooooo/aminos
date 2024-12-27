using System;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	// Token: 0x02000381 RID: 897
	[Obsolete("Use of this type is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
	public interface ITemplateEditingService
	{
		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x0600214B RID: 8523
		bool SupportsNestedTemplateEditing { get; }

		// Token: 0x0600214C RID: 8524
		ITemplateEditingFrame CreateFrame(TemplatedControlDesigner designer, string frameName, string[] templateNames);

		// Token: 0x0600214D RID: 8525
		ITemplateEditingFrame CreateFrame(TemplatedControlDesigner designer, string frameName, string[] templateNames, Style controlStyle, Style[] templateStyles);

		// Token: 0x0600214E RID: 8526
		string GetContainingTemplateName(Control control);
	}
}
