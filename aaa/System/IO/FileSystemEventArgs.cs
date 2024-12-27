using System;

namespace System.IO
{
	// Token: 0x02000728 RID: 1832
	public class FileSystemEventArgs : EventArgs
	{
		// Token: 0x060037DD RID: 14301 RVA: 0x000EC191 File Offset: 0x000EB191
		public FileSystemEventArgs(WatcherChangeTypes changeType, string directory, string name)
		{
			this.changeType = changeType;
			this.name = name;
			if (!directory.EndsWith("\\", StringComparison.Ordinal))
			{
				directory += "\\";
			}
			this.fullPath = directory + name;
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x060037DE RID: 14302 RVA: 0x000EC1CF File Offset: 0x000EB1CF
		public WatcherChangeTypes ChangeType
		{
			get
			{
				return this.changeType;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x060037DF RID: 14303 RVA: 0x000EC1D7 File Offset: 0x000EB1D7
		public string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x060037E0 RID: 14304 RVA: 0x000EC1DF File Offset: 0x000EB1DF
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x040031EE RID: 12782
		private WatcherChangeTypes changeType;

		// Token: 0x040031EF RID: 12783
		private string name;

		// Token: 0x040031F0 RID: 12784
		private string fullPath;
	}
}
