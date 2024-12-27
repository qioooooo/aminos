using System;
using System.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Diagnostics.Design
{
	// Token: 0x02000317 RID: 791
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal class StartFileNameEditor : FileNameEditor
	{
		// Token: 0x06001DF6 RID: 7670 RVA: 0x000AB2C3 File Offset: 0x000AA2C3
		protected override void InitializeDialog(OpenFileDialog openFile)
		{
			openFile.Filter = SR.GetString("StartFileNameEditorAllFiles");
			openFile.Title = SR.GetString("StartFileNameEditorTitle");
		}
	}
}
