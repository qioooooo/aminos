using System;
using System.Security.Permissions;

namespace System.IO
{
	// Token: 0x02000730 RID: 1840
	public class RenamedEventArgs : FileSystemEventArgs
	{
		// Token: 0x06003826 RID: 14374 RVA: 0x000ED48B File Offset: 0x000EC48B
		public RenamedEventArgs(WatcherChangeTypes changeType, string directory, string name, string oldName)
			: base(changeType, directory, name)
		{
			if (!directory.EndsWith("\\", StringComparison.Ordinal))
			{
				directory += "\\";
			}
			this.oldName = oldName;
			this.oldFullPath = directory + oldName;
		}

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x06003827 RID: 14375 RVA: 0x000ED4C7 File Offset: 0x000EC4C7
		public string OldFullPath
		{
			get
			{
				new FileIOPermission(FileIOPermissionAccess.Read, Path.GetPathRoot(this.oldFullPath)).Demand();
				return this.oldFullPath;
			}
		}

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x06003828 RID: 14376 RVA: 0x000ED4E5 File Offset: 0x000EC4E5
		public string OldName
		{
			get
			{
				return this.oldName;
			}
		}

		// Token: 0x0400321D RID: 12829
		private string oldName;

		// Token: 0x0400321E RID: 12830
		private string oldFullPath;
	}
}
