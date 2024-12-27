using System;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002DF RID: 735
	internal class VsPropertyGrid : PropertyGrid
	{
		// Token: 0x06001C4D RID: 7245 RVA: 0x0009F570 File Offset: 0x0009E570
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
