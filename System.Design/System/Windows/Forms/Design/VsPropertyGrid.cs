using System;

namespace System.Windows.Forms.Design
{
	internal class VsPropertyGrid : PropertyGrid
	{
		public VsPropertyGrid(IServiceProvider serviceProvider)
		{
			if (serviceProvider != null)
			{
				IUIService iuiservice = serviceProvider.GetService(typeof(IUIService)) as IUIService;
				if (iuiservice != null)
				{
					base.ToolStripRenderer = (ToolStripProfessionalRenderer)iuiservice.Styles["VsToolWindowRenderer"];
				}
			}
		}
	}
}
