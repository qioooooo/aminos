using System;
using System.Collections.Specialized;
using System.Web.Caching;

namespace System.Web.UI
{
	// Token: 0x020003F3 RID: 1011
	internal sealed class FileDataSourceCache : DataSourceCache
	{
		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06003202 RID: 12802 RVA: 0x000DC02D File Offset: 0x000DB02D
		public StringCollection FileDependencies
		{
			get
			{
				if (this._fileDependencies == null)
				{
					this._fileDependencies = new StringCollection();
				}
				return this._fileDependencies;
			}
		}

		// Token: 0x06003203 RID: 12803 RVA: 0x000DC048 File Offset: 0x000DB048
		protected override void SaveDataToCacheInternal(string key, object data, CacheDependency dependency)
		{
			int count = this.FileDependencies.Count;
			string[] array = new string[count];
			this.FileDependencies.CopyTo(array, 0);
			CacheDependency cacheDependency = new CacheDependency(0, array);
			if (dependency != null)
			{
				AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();
				aggregateCacheDependency.Add(new CacheDependency[] { cacheDependency, dependency });
				dependency = aggregateCacheDependency;
			}
			else
			{
				dependency = cacheDependency;
			}
			base.SaveDataToCacheInternal(key, data, dependency);
		}

		// Token: 0x040022ED RID: 8941
		private StringCollection _fileDependencies;
	}
}
