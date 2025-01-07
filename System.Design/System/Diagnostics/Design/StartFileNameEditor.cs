using System;
using System.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Diagnostics.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal class StartFileNameEditor : FileNameEditor
	{
		protected override void InitializeDialog(OpenFileDialog openFile)
		{
			openFile.Filter = SR.GetString("StartFileNameEditorAllFiles");
			openFile.Title = SR.GetString("StartFileNameEditorTitle");
		}
	}
}
