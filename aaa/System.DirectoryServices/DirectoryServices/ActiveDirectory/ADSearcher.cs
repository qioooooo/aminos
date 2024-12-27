using System;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000087 RID: 135
	[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
	internal class ADSearcher
	{
		// Token: 0x0600041E RID: 1054 RVA: 0x00016580 File Offset: 0x00015580
		public ADSearcher(DirectoryEntry searchRoot, string filter, string[] propertiesToLoad, SearchScope scope)
		{
			this.searcher = new DirectorySearcher(searchRoot, filter, propertiesToLoad, scope);
			this.searcher.CacheResults = false;
			this.searcher.ClientTimeout = ADSearcher.defaultTimeSpan;
			this.searcher.ServerPageTimeLimit = ADSearcher.defaultTimeSpan;
			this.searcher.PageSize = 512;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x000165E0 File Offset: 0x000155E0
		public ADSearcher(DirectoryEntry searchRoot, string filter, string[] propertiesToLoad, SearchScope scope, bool pagedSearch, bool cacheResults)
		{
			this.searcher = new DirectorySearcher(searchRoot, filter, propertiesToLoad, scope);
			this.searcher.ClientTimeout = ADSearcher.defaultTimeSpan;
			if (pagedSearch)
			{
				this.searcher.PageSize = 512;
				this.searcher.ServerPageTimeLimit = ADSearcher.defaultTimeSpan;
			}
			if (cacheResults)
			{
				this.searcher.CacheResults = true;
				return;
			}
			this.searcher.CacheResults = false;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00016654 File Offset: 0x00015654
		public SearchResult FindOne()
		{
			return this.searcher.FindOne();
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00016661 File Offset: 0x00015661
		public SearchResultCollection FindAll()
		{
			return this.searcher.FindAll();
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x0001666E File Offset: 0x0001566E
		public StringCollection PropertiesToLoad
		{
			get
			{
				return this.searcher.PropertiesToLoad;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x0001667B File Offset: 0x0001567B
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x00016688 File Offset: 0x00015688
		public string Filter
		{
			get
			{
				return this.searcher.Filter;
			}
			set
			{
				this.searcher.Filter = value;
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00016696 File Offset: 0x00015696
		public void Dispose()
		{
			this.searcher.Dispose();
		}

		// Token: 0x040003B6 RID: 950
		private DirectorySearcher searcher;

		// Token: 0x040003B7 RID: 951
		private static TimeSpan defaultTimeSpan = new TimeSpan(0, 120, 0);
	}
}
