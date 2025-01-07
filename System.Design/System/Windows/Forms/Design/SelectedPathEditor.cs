using System;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class SelectedPathEditor : FolderNameEditor
	{
		protected override void InitializeDialog(FolderNameEditor.FolderBrowser folderBrowser)
		{
			folderBrowser.Description = SR.GetString("SelectedPathEditorLabel");
		}
	}
}
