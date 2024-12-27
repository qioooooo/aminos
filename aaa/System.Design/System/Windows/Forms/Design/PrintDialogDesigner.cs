using System;
using System.Collections;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200027C RID: 636
	internal class PrintDialogDesigner : ComponentDesigner
	{
		// Token: 0x060017C0 RID: 6080 RVA: 0x0007BA6C File Offset: 0x0007AA6C
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			PrintDialog printDialog = base.Component as PrintDialog;
			if (printDialog != null)
			{
				printDialog.UseEXDialog = true;
			}
		}
	}
}
