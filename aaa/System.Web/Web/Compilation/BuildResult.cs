using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Web.Caching;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x02000142 RID: 322
	internal abstract class BuildResult
	{
		// Token: 0x06000F09 RID: 3849 RVA: 0x000445A0 File Offset: 0x000435A0
		internal static BuildResult CreateBuildResultFromCode(BuildResultTypeCode code, VirtualPath virtualPath)
		{
			BuildResult buildResult;
			switch (code)
			{
			case BuildResultTypeCode.BuildResultCompiledAssembly:
				buildResult = new BuildResultCompiledAssembly();
				goto IL_0074;
			case BuildResultTypeCode.BuildResultCompiledType:
				buildResult = new BuildResultCompiledType();
				goto IL_0074;
			case BuildResultTypeCode.BuildResultCompiledTemplateType:
				buildResult = new BuildResultCompiledTemplateType();
				goto IL_0074;
			case BuildResultTypeCode.BuildResultCustomString:
				buildResult = new BuildResultCustomString();
				goto IL_0074;
			case BuildResultTypeCode.BuildResultMainCodeAssembly:
				buildResult = new BuildResultMainCodeAssembly();
				goto IL_0074;
			case BuildResultTypeCode.BuildResultCodeCompileUnit:
				buildResult = new BuildResultCodeCompileUnit();
				goto IL_0074;
			case BuildResultTypeCode.BuildResultCompiledGlobalAsaxType:
				buildResult = new BuildResultCompiledGlobalAsaxType();
				goto IL_0074;
			case BuildResultTypeCode.BuildResultResourceAssembly:
				buildResult = new BuildResultResourceAssembly();
				goto IL_0074;
			}
			return null;
			IL_0074:
			buildResult.VirtualPath = virtualPath;
			buildResult._nextUpToDateCheck = DateTime.MinValue;
			return buildResult;
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x00044634 File Offset: 0x00043634
		internal virtual BuildResultTypeCode GetCode()
		{
			return BuildResultTypeCode.Invalid;
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000F0B RID: 3851 RVA: 0x00044637 File Offset: 0x00043637
		// (set) Token: 0x06000F0C RID: 3852 RVA: 0x00044644 File Offset: 0x00043644
		internal int Flags
		{
			get
			{
				return this._flags.IntegerValue;
			}
			set
			{
				this._flags.IntegerValue = value;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000F0D RID: 3853 RVA: 0x00044652 File Offset: 0x00043652
		// (set) Token: 0x06000F0E RID: 3854 RVA: 0x0004465A File Offset: 0x0004365A
		internal VirtualPath VirtualPath
		{
			get
			{
				return this._virtualPath;
			}
			set
			{
				this._virtualPath = value;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000F0F RID: 3855 RVA: 0x00044663 File Offset: 0x00043663
		// (set) Token: 0x06000F10 RID: 3856 RVA: 0x00044675 File Offset: 0x00043675
		internal bool UsesCacheDependency
		{
			get
			{
				return this._flags[65536];
			}
			set
			{
				this._flags[65536] = value;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000F11 RID: 3857 RVA: 0x00044688 File Offset: 0x00043688
		internal bool ShutdownAppDomainOnChange
		{
			get
			{
				return this._flags[1];
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000F12 RID: 3858 RVA: 0x00044696 File Offset: 0x00043696
		internal ICollection VirtualPathDependencies
		{
			get
			{
				return this._virtualPathDependencies;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000F13 RID: 3859 RVA: 0x0004469E File Offset: 0x0004369E
		// (set) Token: 0x06000F14 RID: 3860 RVA: 0x000446AC File Offset: 0x000436AC
		internal string VirtualPathDependenciesHash
		{
			get
			{
				this.EnsureVirtualPathDependenciesHashComputed();
				return this._virtualPathDependenciesHash;
			}
			set
			{
				this._virtualPathDependenciesHash = value;
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000F15 RID: 3861 RVA: 0x000446B5 File Offset: 0x000436B5
		internal bool DependenciesHashComputed
		{
			get
			{
				return this._flags[1048576];
			}
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x000446C7 File Offset: 0x000436C7
		internal void EnsureVirtualPathDependenciesHashComputed()
		{
			if (!this.DependenciesHashComputed)
			{
				if (this._virtualPathDependencies != null)
				{
					this._virtualPathDependencies.Sort(InvariantComparer.Default);
				}
				this._virtualPathDependenciesHash = this.ComputeSourceDependenciesHashCode(null);
				this._flags[1048576] = true;
			}
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x00044707 File Offset: 0x00043707
		internal void SetVirtualPathDependencies(ArrayList sourceDependencies)
		{
			this._virtualPathDependencies = sourceDependencies;
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x00044710 File Offset: 0x00043710
		internal void AddVirtualPathDependencies(ICollection sourceDependencies)
		{
			if (sourceDependencies == null)
			{
				return;
			}
			if (this._virtualPathDependencies == null)
			{
				this._virtualPathDependencies = new ArrayList(sourceDependencies);
				return;
			}
			this._virtualPathDependencies.AddRange(sourceDependencies);
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000F19 RID: 3865 RVA: 0x00044737 File Offset: 0x00043737
		internal virtual bool IsUnloadable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000F1A RID: 3866 RVA: 0x0004473A File Offset: 0x0004373A
		internal virtual bool CacheToDisk
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000F1B RID: 3867 RVA: 0x0004473D File Offset: 0x0004373D
		// (set) Token: 0x06000F1C RID: 3868 RVA: 0x00044752 File Offset: 0x00043752
		internal bool CacheToMemory
		{
			get
			{
				return !this._flags[262144];
			}
			set
			{
				this._flags[262144] = !value;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x00044768 File Offset: 0x00043768
		internal virtual DateTime MemoryCacheExpiration
		{
			get
			{
				return Cache.NoAbsoluteExpiration;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000F1E RID: 3870 RVA: 0x0004476F File Offset: 0x0004376F
		internal virtual TimeSpan MemoryCacheSlidingExpiration
		{
			get
			{
				return Cache.NoSlidingExpiration;
			}
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x00044778 File Offset: 0x00043778
		protected void ReadPreservedFlags(PreservationFileReader pfr)
		{
			string attribute = pfr.GetAttribute("flags");
			if (attribute != null && attribute.Length != 0)
			{
				this.Flags = int.Parse(attribute, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x000447B2 File Offset: 0x000437B2
		internal virtual void GetPreservedAttributes(PreservationFileReader pfr)
		{
			this.ReadPreservedFlags(pfr);
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x000447BC File Offset: 0x000437BC
		internal virtual void SetPreservedAttributes(PreservationFileWriter pfw)
		{
			if (this.Flags != 0)
			{
				pfw.SetAttribute("flags", this.Flags.ToString("x", CultureInfo.InvariantCulture));
			}
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x000447F4 File Offset: 0x000437F4
		internal virtual void RemoveOutOfDateResources(PreservationFileReader pfw)
		{
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x000447F6 File Offset: 0x000437F6
		internal long ComputeHashCode(long hashCode)
		{
			return this.ComputeHashCode(hashCode, 0L);
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x00044804 File Offset: 0x00043804
		internal long ComputeHashCode(long hashCode1, long hashCode2)
		{
			HashCodeCombiner hashCodeCombiner = new HashCodeCombiner();
			if (hashCode1 != 0L)
			{
				hashCodeCombiner.AddObject(hashCode1);
			}
			if (hashCode2 != 0L)
			{
				hashCodeCombiner.AddObject(hashCode2);
			}
			this.ComputeHashCode(hashCodeCombiner);
			return hashCodeCombiner.CombinedHash;
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x0004483C File Offset: 0x0004383C
		protected virtual void ComputeHashCode(HashCodeCombiner hashCodeCombiner)
		{
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0004483E File Offset: 0x0004383E
		internal virtual string ComputeSourceDependenciesHashCode(VirtualPath virtualPath)
		{
			if (this.VirtualPathDependencies == null)
			{
				return string.Empty;
			}
			if (virtualPath == null)
			{
				virtualPath = this.VirtualPath;
			}
			return virtualPath.GetFileHash(this.VirtualPathDependencies);
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x0004486C File Offset: 0x0004386C
		internal bool IsUpToDate(VirtualPath virtualPath)
		{
			if (this._lock < 0)
			{
				return false;
			}
			DateTime now = DateTime.Now;
			if (now < this._nextUpToDateCheck && !BuildManagerHost.InClientBuildManager)
			{
				return true;
			}
			if (Interlocked.CompareExchange(ref this._lock, 1, 0) != 0)
			{
				return true;
			}
			string text;
			try
			{
				text = this.ComputeSourceDependenciesHashCode(virtualPath);
			}
			catch
			{
				Interlocked.Exchange(ref this._lock, 0);
				throw;
			}
			if (text == null || text != this._virtualPathDependenciesHash)
			{
				this._lock = -1;
				return false;
			}
			this._nextUpToDateCheck = now.AddSeconds(2.0);
			Interlocked.Exchange(ref this._lock, 0);
			return true;
		}

		// Token: 0x040015D1 RID: 5585
		protected const int usesCacheDependency = 65536;

		// Token: 0x040015D2 RID: 5586
		protected const int usesExistingAssembly = 131072;

		// Token: 0x040015D3 RID: 5587
		private const int noMemoryCache = 262144;

		// Token: 0x040015D4 RID: 5588
		protected const int hasAppOrSessionObjects = 524288;

		// Token: 0x040015D5 RID: 5589
		protected const int dependenciesHashComputed = 1048576;

		// Token: 0x040015D6 RID: 5590
		private const int UpdateInterval = 2;

		// Token: 0x040015D7 RID: 5591
		protected SimpleBitVector32 _flags;

		// Token: 0x040015D8 RID: 5592
		private VirtualPath _virtualPath;

		// Token: 0x040015D9 RID: 5593
		private ArrayList _virtualPathDependencies;

		// Token: 0x040015DA RID: 5594
		private string _virtualPathDependenciesHash;

		// Token: 0x040015DB RID: 5595
		private DateTime _nextUpToDateCheck = DateTime.Now.AddSeconds(2.0);

		// Token: 0x040015DC RID: 5596
		private int _lock;
	}
}
