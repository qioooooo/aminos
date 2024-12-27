using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;
using System.Web.Caching;
using System.Web.Util;

namespace System.Web.Hosting
{
	// Token: 0x02000137 RID: 311
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Medium)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.High)]
	public abstract class VirtualPathProvider : MarshalByRefObject
	{
		// Token: 0x06000EC2 RID: 3778 RVA: 0x00043245 File Offset: 0x00042245
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x00043248 File Offset: 0x00042248
		internal virtual void Initialize(VirtualPathProvider previous)
		{
			this._previous = previous;
			this.Initialize();
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x00043257 File Offset: 0x00042257
		protected virtual void Initialize()
		{
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x00043259 File Offset: 0x00042259
		protected internal VirtualPathProvider Previous
		{
			get
			{
				return this._previous;
			}
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00043261 File Offset: 0x00042261
		public virtual string GetFileHash(string virtualPath, IEnumerable virtualPathDependencies)
		{
			if (this._previous == null)
			{
				return null;
			}
			return this._previous.GetFileHash(virtualPath, virtualPathDependencies);
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0004327A File Offset: 0x0004227A
		internal string GetFileHash(VirtualPath virtualPath, IEnumerable virtualPathDependencies)
		{
			return this.GetFileHash(virtualPath.VirtualPathString, virtualPathDependencies);
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x00043289 File Offset: 0x00042289
		public virtual CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
		{
			if (this._previous == null)
			{
				return null;
			}
			return this._previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x000432A3 File Offset: 0x000422A3
		internal CacheDependency GetCacheDependency(VirtualPath virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
		{
			return this.GetCacheDependency(virtualPath.VirtualPathString, virtualPathDependencies, utcStart);
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x000432B3 File Offset: 0x000422B3
		public virtual bool FileExists(string virtualPath)
		{
			return this._previous != null && this._previous.FileExists(virtualPath);
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x000432CB File Offset: 0x000422CB
		internal bool FileExists(VirtualPath virtualPath)
		{
			return this.FileExists(virtualPath.VirtualPathString);
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x000432D9 File Offset: 0x000422D9
		public virtual bool DirectoryExists(string virtualDir)
		{
			return this._previous != null && this._previous.DirectoryExists(virtualDir);
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x000432F1 File Offset: 0x000422F1
		internal bool DirectoryExists(VirtualPath virtualDir)
		{
			return this.DirectoryExists(virtualDir.VirtualPathString);
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x000432FF File Offset: 0x000422FF
		public virtual VirtualFile GetFile(string virtualPath)
		{
			if (this._previous == null)
			{
				return null;
			}
			return this._previous.GetFile(virtualPath);
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00043317 File Offset: 0x00042317
		internal VirtualFile GetFile(VirtualPath virtualPath)
		{
			return this.GetFileWithCheck(virtualPath.VirtualPathString);
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x00043328 File Offset: 0x00042328
		internal VirtualFile GetFileWithCheck(string virtualPath)
		{
			VirtualFile file = this.GetFile(virtualPath);
			if (file == null)
			{
				return null;
			}
			if (!StringUtil.EqualsIgnoreCase(virtualPath, file.VirtualPath))
			{
				throw new HttpException(SR.GetString("Bad_VirtualPath_in_VirtualFileBase", new object[] { "VirtualFile", file.VirtualPath, virtualPath }));
			}
			return file;
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0004337E File Offset: 0x0004237E
		public virtual VirtualDirectory GetDirectory(string virtualDir)
		{
			if (this._previous == null)
			{
				return null;
			}
			return this._previous.GetDirectory(virtualDir);
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00043396 File Offset: 0x00042396
		internal VirtualDirectory GetDirectory(VirtualPath virtualDir)
		{
			return this.GetDirectoryWithCheck(virtualDir.VirtualPathString);
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x000433A4 File Offset: 0x000423A4
		internal VirtualDirectory GetDirectoryWithCheck(string virtualPath)
		{
			VirtualDirectory directory = this.GetDirectory(virtualPath);
			if (directory == null)
			{
				return null;
			}
			if (!StringUtil.EqualsIgnoreCase(virtualPath, directory.VirtualPath))
			{
				throw new HttpException(SR.GetString("Bad_VirtualPath_in_VirtualFileBase", new object[] { "VirtualDirectory", directory.VirtualPath, virtualPath }));
			}
			return directory;
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x000433FA File Offset: 0x000423FA
		public virtual string GetCacheKey(string virtualPath)
		{
			return null;
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x000433FD File Offset: 0x000423FD
		internal string GetCacheKey(VirtualPath virtualPath)
		{
			return this.GetCacheKey(virtualPath.VirtualPathString);
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0004340C File Offset: 0x0004240C
		public virtual string CombineVirtualPaths(string basePath, string relativePath)
		{
			string text = null;
			if (!string.IsNullOrEmpty(basePath))
			{
				text = UrlPath.GetDirectory(basePath);
			}
			return UrlPath.Combine(text, relativePath);
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x00043434 File Offset: 0x00042434
		internal VirtualPath CombineVirtualPaths(VirtualPath basePath, VirtualPath relativePath)
		{
			string text = this.CombineVirtualPaths(basePath.VirtualPathString, relativePath.VirtualPathString);
			return VirtualPath.Create(text);
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0004345C File Offset: 0x0004245C
		public static Stream OpenFile(string virtualPath)
		{
			VirtualPathProvider virtualPathProvider = HostingEnvironment.VirtualPathProvider;
			VirtualFile fileWithCheck = virtualPathProvider.GetFileWithCheck(virtualPath);
			return fileWithCheck.Open();
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0004347D File Offset: 0x0004247D
		internal static Stream OpenFile(VirtualPath virtualPath)
		{
			return VirtualPathProvider.OpenFile(virtualPath.VirtualPathString);
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0004348C File Offset: 0x0004248C
		internal static CacheDependency GetCacheDependency(VirtualPath virtualPath)
		{
			VirtualPathProvider virtualPathProvider = HostingEnvironment.VirtualPathProvider;
			return virtualPathProvider.GetCacheDependency(virtualPath, new SingleObjectCollection(virtualPath.VirtualPathString), DateTime.MaxValue);
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x000434B8 File Offset: 0x000424B8
		internal static VirtualPath CombineVirtualPathsInternal(VirtualPath basePath, VirtualPath relativePath)
		{
			VirtualPathProvider virtualPathProvider = HostingEnvironment.VirtualPathProvider;
			if (virtualPathProvider != null)
			{
				return virtualPathProvider.CombineVirtualPaths(basePath, relativePath);
			}
			return basePath.Parent.Combine(relativePath);
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x000434E4 File Offset: 0x000424E4
		internal static bool DirectoryExistsNoThrow(string virtualDir)
		{
			bool flag;
			try
			{
				flag = HostingEnvironment.VirtualPathProvider.DirectoryExists(virtualDir);
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x00043518 File Offset: 0x00042518
		internal static bool DirectoryExistsNoThrow(VirtualPath virtualDir)
		{
			return VirtualPathProvider.DirectoryExistsNoThrow(virtualDir.VirtualPathString);
		}

		// Token: 0x040015A8 RID: 5544
		private VirtualPathProvider _previous;
	}
}
