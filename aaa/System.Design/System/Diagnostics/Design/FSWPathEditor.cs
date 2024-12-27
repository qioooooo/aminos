using System;
using System.Design;
using System.Security.Permissions;
using System.Windows.Forms.Design;

namespace System.Diagnostics.Design
{
	// Token: 0x02000312 RID: 786
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal class FSWPathEditor : FolderNameEditor
	{
		// Token: 0x06001DE9 RID: 7657 RVA: 0x000AB014 File Offset: 0x000AA014
		protected override void InitializeDialog(FolderNameEditor.FolderBrowser folderBrowser)
		{
			folderBrowser.Description = SR.GetString("FSWPathEditorLabel");
		}
	}
}
