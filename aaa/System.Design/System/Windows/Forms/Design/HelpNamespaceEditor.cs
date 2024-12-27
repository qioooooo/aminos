using System;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000249 RID: 585
	internal class HelpNamespaceEditor : FileNameEditor
	{
		// Token: 0x06001650 RID: 5712 RVA: 0x00074735 File Offset: 0x00073735
		protected override void InitializeDialog(OpenFileDialog openFileDialog)
		{
			openFileDialog.Filter = SR.GetString("HelpProviderEditorFilter");
			openFileDialog.Title = SR.GetString("HelpProviderEditorTitle");
		}
	}
}
