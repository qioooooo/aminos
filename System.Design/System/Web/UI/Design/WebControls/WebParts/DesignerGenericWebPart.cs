using System;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	internal sealed class DesignerGenericWebPart : GenericWebPart
	{
		public DesignerGenericWebPart(Control control)
			: base(control)
		{
		}

		protected override void CreateChildControls()
		{
		}
	}
}
