using System;
using System.Design;
using System.Security.Permissions;
using System.Windows.Forms.Design;

namespace System.Diagnostics.Design
{
	// Token: 0x02000318 RID: 792
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal class WorkingDirectoryEditor : FolderNameEditor
	{
		// Token: 0x06001DF8 RID: 7672 RVA: 0x000AB2ED File Offset: 0x000AA2ED
		protected override void InitializeDialog(FolderNameEditor.FolderBrowser folderBrowser)
		{
			folderBrowser.Description = SR.GetString("WorkingDirectoryEditorLabel");
		}
	}
}
