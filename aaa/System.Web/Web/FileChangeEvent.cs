using System;

namespace System.Web
{
	// Token: 0x0200002E RID: 46
	internal sealed class FileChangeEvent : EventArgs
	{
		// Token: 0x060000ED RID: 237 RVA: 0x00005803 File Offset: 0x00004803
		internal FileChangeEvent(FileAction action, string fileName)
		{
			this.Action = action;
			this.FileName = fileName;
		}

		// Token: 0x04000DA7 RID: 3495
		internal FileAction Action;

		// Token: 0x04000DA8 RID: 3496
		internal string FileName;
	}
}
