using System;
using System.Collections;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	internal class PrintDialogDesigner : ComponentDesigner
	{
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
