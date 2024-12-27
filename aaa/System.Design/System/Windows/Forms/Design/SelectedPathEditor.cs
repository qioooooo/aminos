using System;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000284 RID: 644
	internal class SelectedPathEditor : FolderNameEditor
	{
		// Token: 0x060017DB RID: 6107 RVA: 0x0007BFCB File Offset: 0x0007AFCB
		protected override void InitializeDialog(FolderNameEditor.FolderBrowser folderBrowser)
		{
			folderBrowser.Description = SR.GetString("SelectedPathEditorLabel");
		}
	}
}
