using System;
using System.Collections;
using System.Security.Permissions;
using System.Text;

namespace System.Web.Caching
{
	// Token: 0x02000106 RID: 262
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class AggregateCacheDependency : CacheDependency, ICacheDependencyChanged
	{
		// Token: 0x06000C3E RID: 3134 RVA: 0x00030CB8 File Offset: 0x0002FCB8
		public AggregateCacheDependency()
		{
			base.FinishInit();
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x00030CC8 File Offset: 0x0002FCC8
		public void Add(params CacheDependency[] dependencies)
		{
			DateTime dateTime = DateTime.MinValue;
			if (dependencies == null)
			{
				throw new ArgumentNullException("dependencies");
			}
			dependencies = (CacheDependency[])dependencies.Clone();
			foreach (CacheDependency cacheDependency in dependencies)
			{
				if (cacheDependency == null)
				{
					throw new ArgumentNullException("dependencies");
				}
				if (!cacheDependency.Use())
				{
					throw new InvalidOperationException(SR.GetString("Cache_dependency_used_more_that_once"));
				}
			}
			bool flag = false;
			lock (this)
			{
				if (!this._disposed)
				{
					if (this._dependencies == null)
					{
						this._dependencies = new ArrayList();
					}
					this._dependencies.AddRange(dependencies);
					foreach (CacheDependency cacheDependency2 in dependencies)
					{
						cacheDependency2.SetCacheDependencyChanged(this);
						if (cacheDependency2.UtcLastModified > dateTime)
						{
							dateTime = cacheDependency2.UtcLastModified;
						}
						if (cacheDependency2.HasChanged)
						{
							flag = true;
							break;
						}
					}
				}
			}
			base.SetUtcLastModified(dateTime);
			if (flag)
			{
				base.NotifyDependencyChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00030DE0 File Offset: 0x0002FDE0
		protected override void DependencyDispose()
		{
			CacheDependency[] array = null;
			lock (this)
			{
				this._disposed = true;
				if (this._dependencies != null)
				{
					array = (CacheDependency[])this._dependencies.ToArray(typeof(CacheDependency));
					this._dependencies = null;
				}
			}
			if (array != null)
			{
				foreach (CacheDependency cacheDependency in array)
				{
					cacheDependency.DisposeInternal();
				}
			}
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x00030E64 File Offset: 0x0002FE64
		void ICacheDependencyChanged.DependencyChanged(object sender, EventArgs e)
		{
			base.NotifyDependencyChanged(sender, e);
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x00030E70 File Offset: 0x0002FE70
		public override string GetUniqueID()
		{
			StringBuilder stringBuilder = null;
			CacheDependency[] array = null;
			if (this._dependencies == null)
			{
				return null;
			}
			lock (this)
			{
				if (this._dependencies != null)
				{
					array = (CacheDependency[])this._dependencies.ToArray(typeof(CacheDependency));
				}
			}
			if (array != null)
			{
				foreach (CacheDependency cacheDependency in array)
				{
					string uniqueID = cacheDependency.GetUniqueID();
					if (uniqueID == null)
					{
						return null;
					}
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder();
					}
					stringBuilder.Append(uniqueID);
					stringBuilder.Append(";");
				}
			}
			if (stringBuilder == null)
			{
				return null;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x00030F2C File Offset: 0x0002FF2C
		internal CacheDependency[] GetDependencyArray()
		{
			CacheDependency[] array = null;
			lock (this)
			{
				if (this._dependencies != null)
				{
					array = (CacheDependency[])this._dependencies.ToArray(typeof(CacheDependency));
				}
			}
			return array;
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00030F80 File Offset: 0x0002FF80
		internal override bool IsFileDependency()
		{
			CacheDependency[] dependencyArray = this.GetDependencyArray();
			if (dependencyArray == null)
			{
				return false;
			}
			CacheDependency[] array = dependencyArray;
			int i = 0;
			while (i < array.Length)
			{
				CacheDependency cacheDependency = array[i];
				bool flag;
				if (!object.ReferenceEquals(cacheDependency.GetType(), typeof(CacheDependency)) && !object.ReferenceEquals(cacheDependency.GetType(), typeof(AggregateCacheDependency)))
				{
					flag = false;
				}
				else
				{
					if (cacheDependency.IsFileDependency())
					{
						i++;
						continue;
					}
					flag = false;
				}
				return flag;
			}
			return true;
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00030FF8 File Offset: 0x0002FFF8
		internal override string[] GetFileDependencies()
		{
			ArrayList arrayList = null;
			CacheDependency[] dependencyArray = this.GetDependencyArray();
			if (dependencyArray == null)
			{
				return null;
			}
			foreach (CacheDependency cacheDependency in dependencyArray)
			{
				if (object.ReferenceEquals(cacheDependency.GetType(), typeof(CacheDependency)) || object.ReferenceEquals(cacheDependency.GetType(), typeof(AggregateCacheDependency)))
				{
					string[] fileDependencies = cacheDependency.GetFileDependencies();
					if (fileDependencies != null)
					{
						if (arrayList == null)
						{
							arrayList = new ArrayList();
						}
						arrayList.AddRange(fileDependencies);
					}
				}
			}
			if (arrayList != null)
			{
				return (string[])arrayList.ToArray(typeof(string));
			}
			return null;
		}

		// Token: 0x0400140B RID: 5131
		private ArrayList _dependencies;

		// Token: 0x0400140C RID: 5132
		private bool _disposed;
	}
}
