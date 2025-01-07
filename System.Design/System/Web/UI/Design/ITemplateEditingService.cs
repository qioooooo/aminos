using System;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	[Obsolete("Use of this type is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
	public interface ITemplateEditingService
	{
		bool SupportsNestedTemplateEditing { get; }

		ITemplateEditingFrame CreateFrame(TemplatedControlDesigner designer, string frameName, string[] templateNames);

		ITemplateEditingFrame CreateFrame(TemplatedControlDesigner designer, string frameName, string[] templateNames, Style controlStyle, Style[] templateStyles);

		string GetContainingTemplateName(Control control);
	}
}
