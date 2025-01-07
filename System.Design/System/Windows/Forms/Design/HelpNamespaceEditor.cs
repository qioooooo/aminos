using System;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class HelpNamespaceEditor : FileNameEditor
	{
		protected override void InitializeDialog(OpenFileDialog openFileDialog)
		{
			openFileDialog.Filter = SR.GetString("HelpProviderEditorFilter");
			openFileDialog.Title = SR.GetString("HelpProviderEditorTitle");
		}
	}
}
