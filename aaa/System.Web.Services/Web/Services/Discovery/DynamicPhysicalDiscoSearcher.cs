using System;
using System.IO;

namespace System.Web.Services.Discovery
{
	// Token: 0x020000AB RID: 171
	internal class DynamicPhysicalDiscoSearcher : DynamicDiscoSearcher
	{
		// Token: 0x0600048D RID: 1165 RVA: 0x00016C5D File Offset: 0x00015C5D
		internal DynamicPhysicalDiscoSearcher(string searchDir, string[] excludedUrls, string startUrl)
			: base(excludedUrls)
		{
			this.startDir = searchDir;
			this.origUrl = startUrl;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00016C74 File Offset: 0x00015C74
		internal override void Search(string fileToSkipAtBegin)
		{
			this.SearchInit(fileToSkipAtBegin);
			base.ScanDirectory(this.startDir);
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00016C8C File Offset: 0x00015C8C
		protected override void SearchSubDirectories(string localDir)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(localDir);
			if (!directoryInfo.Exists)
			{
				return;
			}
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			foreach (DirectoryInfo directoryInfo2 in directories)
			{
				if (!(directoryInfo2.Name == ".") && !(directoryInfo2.Name == ".."))
				{
					base.ScanDirectory(localDir + '\\' + directoryInfo2.Name);
				}
			}
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00016D08 File Offset: 0x00015D08
		protected override DirectoryInfo GetPhysicalDir(string dir)
		{
			if (!Directory.Exists(dir))
			{
				return null;
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(dir);
			if (!directoryInfo.Exists)
			{
				return null;
			}
			if ((directoryInfo.Attributes & (FileAttributes.Hidden | FileAttributes.System | FileAttributes.Temporary)) != (FileAttributes)0)
			{
				return null;
			}
			return directoryInfo;
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00016D44 File Offset: 0x00015D44
		protected override string MakeResultPath(string dirName, string fileName)
		{
			return string.Concat(new object[]
			{
				this.origUrl,
				dirName.Substring(this.startDir.Length, dirName.Length - this.startDir.Length).Replace('\\', '/'),
				'/',
				fileName
			});
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00016DA6 File Offset: 0x00015DA6
		protected override string MakeAbsExcludedPath(string pathRelativ)
		{
			return this.startDir + '\\' + pathRelativ.Replace('/', '\\');
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x00016DC4 File Offset: 0x00015DC4
		protected override bool IsVirtualSearch
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040003C7 RID: 967
		private string startDir;
	}
}
