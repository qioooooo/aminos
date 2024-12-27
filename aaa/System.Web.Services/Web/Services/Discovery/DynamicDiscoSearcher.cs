using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace System.Web.Services.Discovery
{
	// Token: 0x020000A9 RID: 169
	internal abstract class DynamicDiscoSearcher
	{
		// Token: 0x0600047A RID: 1146 RVA: 0x00016936 File Offset: 0x00015936
		internal DynamicDiscoSearcher(string[] excludeUrlsList)
		{
			this.excludedUrls = excludeUrlsList;
			this.filesFound = new ArrayList();
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0001695B File Offset: 0x0001595B
		internal virtual void SearchInit(string fileToSkipAtBegin)
		{
			this.subDirLevel = 0;
			this.fileToSkipFirst = fileToSkipAtBegin;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0001696C File Offset: 0x0001596C
		protected bool IsExcluded(string url)
		{
			if (this.excludedUrlsTable == null)
			{
				this.excludedUrlsTable = new Hashtable();
				foreach (string text in this.excludedUrls)
				{
					this.excludedUrlsTable.Add(this.MakeAbsExcludedPath(text).ToLower(CultureInfo.InvariantCulture), null);
				}
			}
			return this.excludedUrlsTable.Contains(url.ToLower(CultureInfo.InvariantCulture));
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x000169D8 File Offset: 0x000159D8
		internal DiscoveryDocument DiscoveryDocument
		{
			get
			{
				return this.discoDoc;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x000169E0 File Offset: 0x000159E0
		internal DiscoverySearchPattern[] PrimarySearchPattern
		{
			get
			{
				if (this.primarySearchPatterns == null)
				{
					this.primarySearchPatterns = new DiscoverySearchPattern[]
					{
						new DiscoveryDocumentSearchPattern()
					};
				}
				return this.primarySearchPatterns;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x00016A14 File Offset: 0x00015A14
		internal DiscoverySearchPattern[] SecondarySearchPattern
		{
			get
			{
				if (this.secondarySearchPatterns == null)
				{
					this.secondarySearchPatterns = new DiscoverySearchPattern[]
					{
						new ContractSearchPattern(),
						new DiscoveryDocumentLinksPattern()
					};
				}
				return this.secondarySearchPatterns;
			}
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00016A50 File Offset: 0x00015A50
		protected void ScanDirectory(string directory)
		{
			bool traceVerbose = CompModSwitches.DynamicDiscoverySearcher.TraceVerbose;
			if (this.IsExcluded(directory))
			{
				return;
			}
			if (!this.ScanDirByPattern(directory, true, this.PrimarySearchPattern))
			{
				if (!this.IsVirtualSearch)
				{
					this.ScanDirByPattern(directory, false, this.SecondarySearchPattern);
				}
				else if (this.subDirLevel != 0)
				{
					DiscoverySearchPattern[] array = new DiscoverySearchPattern[]
					{
						new DiscoveryDocumentLinksPattern()
					};
					this.ScanDirByPattern(directory, false, array);
				}
				if (this.IsVirtualSearch && this.subDirLevel > 0)
				{
					return;
				}
				this.subDirLevel++;
				this.fileToSkipFirst = "";
				this.SearchSubDirectories(directory);
				this.subDirLevel--;
			}
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00016B00 File Offset: 0x00015B00
		protected bool ScanDirByPattern(string dir, bool IsPrimary, DiscoverySearchPattern[] patterns)
		{
			DirectoryInfo physicalDir = this.GetPhysicalDir(dir);
			if (physicalDir == null)
			{
				return false;
			}
			bool traceVerbose = CompModSwitches.DynamicDiscoverySearcher.TraceVerbose;
			bool flag = false;
			for (int i = 0; i < patterns.Length; i++)
			{
				FileInfo[] files = physicalDir.GetFiles(patterns[i].Pattern);
				foreach (FileInfo fileInfo in files)
				{
					if ((fileInfo.Attributes & FileAttributes.Directory) == (FileAttributes)0)
					{
						bool traceVerbose2 = CompModSwitches.DynamicDiscoverySearcher.TraceVerbose;
						if (string.Compare(fileInfo.Name, this.fileToSkipFirst, StringComparison.OrdinalIgnoreCase) != 0)
						{
							string text = this.MakeResultPath(dir, fileInfo.Name);
							this.filesFound.Add(text);
							this.discoDoc.References.Add(patterns[i].GetDiscoveryReference(text));
							flag = true;
						}
					}
				}
			}
			return IsPrimary && flag;
		}

		// Token: 0x06000482 RID: 1154
		internal abstract void Search(string fileToSkipAtBegin);

		// Token: 0x06000483 RID: 1155
		protected abstract DirectoryInfo GetPhysicalDir(string dir);

		// Token: 0x06000484 RID: 1156
		protected abstract void SearchSubDirectories(string directory);

		// Token: 0x06000485 RID: 1157
		protected abstract string MakeResultPath(string dirName, string fileName);

		// Token: 0x06000486 RID: 1158
		protected abstract string MakeAbsExcludedPath(string pathRelativ);

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000487 RID: 1159
		protected abstract bool IsVirtualSearch { get; }

		// Token: 0x040003BC RID: 956
		protected string origUrl;

		// Token: 0x040003BD RID: 957
		protected string[] excludedUrls;

		// Token: 0x040003BE RID: 958
		protected string fileToSkipFirst;

		// Token: 0x040003BF RID: 959
		protected ArrayList filesFound;

		// Token: 0x040003C0 RID: 960
		protected DiscoverySearchPattern[] primarySearchPatterns;

		// Token: 0x040003C1 RID: 961
		protected DiscoverySearchPattern[] secondarySearchPatterns;

		// Token: 0x040003C2 RID: 962
		protected DiscoveryDocument discoDoc = new DiscoveryDocument();

		// Token: 0x040003C3 RID: 963
		protected Hashtable excludedUrlsTable;

		// Token: 0x040003C4 RID: 964
		protected int subDirLevel;
	}
}
