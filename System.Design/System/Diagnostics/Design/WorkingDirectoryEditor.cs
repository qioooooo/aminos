using System;
using System.Design;
using System.Security.Permissions;
using System.Windows.Forms.Design;

namespace System.Diagnostics.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal class WorkingDirectoryEditor : FolderNameEditor
	{
		protected override void InitializeDialog(FolderNameEditor.FolderBrowser folderBrowser)
		{
			folderBrowser.Description = SR.GetString("WorkingDirectoryEditorLabel");
		}
	}
}
