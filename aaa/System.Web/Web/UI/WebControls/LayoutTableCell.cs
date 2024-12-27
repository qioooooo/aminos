using System;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005C6 RID: 1478
	internal sealed class LayoutTableCell : TableCell
	{
		// Token: 0x0600481C RID: 18460 RVA: 0x00126AD6 File Offset: 0x00125AD6
		protected internal override void AddedControl(Control control, int index)
		{
			if (control.Page == null)
			{
				control.Page = this.Page;
			}
		}

		// Token: 0x0600481D RID: 18461 RVA: 0x00126AEC File Offset: 0x00125AEC
		protected internal override void RemovedControl(Control control)
		{
		}
	}
}
