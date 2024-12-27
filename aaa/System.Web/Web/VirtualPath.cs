using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.Util;
using Microsoft.Win32;

namespace System.Web
{
	// Token: 0x020000E7 RID: 231
	[Serializable]
	internal sealed class VirtualPath : IComparable
	{
		// Token: 0x06000AB7 RID: 2743 RVA: 0x0002B72B File Offset: 0x0002A72B
		private VirtualPath()
		{
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x0002B733 File Offset: 0x0002A733
		private VirtualPath(string virtualPath)
		{
			if (UrlPath.IsAppRelativePath(virtualPath))
			{
				this._appRelativeVirtualPath = virtualPath;
				return;
			}
			this._virtualPath = virtualPath;
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0002B754 File Offset: 0x0002A754
		int IComparable.CompareTo(object obj)
		{
			VirtualPath virtualPath = obj as VirtualPath;
			if (virtualPath == null)
			{
				throw new ArgumentException();
			}
			if (virtualPath == this)
			{
				return 0;
			}
			return StringComparer.InvariantCultureIgnoreCase.Compare(this.VirtualPathString, virtualPath.VirtualPathString);
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000ABA RID: 2746 RVA: 0x0002B798 File Offset: 0x0002A798
		public string VirtualPathString
		{
			get
			{
				if (this._virtualPath == null)
				{
					if (HttpRuntime.AppDomainAppVirtualPathObject == null)
					{
						throw new HttpException(SR.GetString("VirtualPath_CantMakeAppAbsolute", new object[] { this._appRelativeVirtualPath }));
					}
					if (this._appRelativeVirtualPath.Length == 1)
					{
						this._virtualPath = HttpRuntime.AppDomainAppVirtualPath;
					}
					else
					{
						this._virtualPath = HttpRuntime.AppDomainAppVirtualPathString + this._appRelativeVirtualPath.Substring(2);
					}
				}
				return this._virtualPath;
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x0002B818 File Offset: 0x0002A818
		internal string VirtualPathStringNoTrailingSlash
		{
			get
			{
				return UrlPath.RemoveSlashFromPathIfNeeded(this.VirtualPathString);
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x0002B825 File Offset: 0x0002A825
		internal string VirtualPathStringIfAvailable
		{
			get
			{
				return this._virtualPath;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x0002B830 File Offset: 0x0002A830
		internal string AppRelativeVirtualPathStringOrNull
		{
			get
			{
				if (this._appRelativeVirtualPath == null)
				{
					if (this.flags[4])
					{
						return null;
					}
					if (HttpRuntime.AppDomainAppVirtualPathObject == null)
					{
						throw new HttpException(SR.GetString("VirtualPath_CantMakeAppRelative", new object[] { this._virtualPath }));
					}
					this._appRelativeVirtualPath = UrlPath.MakeVirtualPathAppRelativeOrNull(this._virtualPath);
					this.flags[4] = true;
					if (this._appRelativeVirtualPath == null)
					{
						return null;
					}
				}
				return this._appRelativeVirtualPath;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x0002B8B4 File Offset: 0x0002A8B4
		public string AppRelativeVirtualPathString
		{
			get
			{
				string appRelativeVirtualPathStringOrNull = this.AppRelativeVirtualPathStringOrNull;
				if (appRelativeVirtualPathStringOrNull == null)
				{
					return this._virtualPath;
				}
				return appRelativeVirtualPathStringOrNull;
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x0002B8D3 File Offset: 0x0002A8D3
		internal string AppRelativeVirtualPathStringIfAvailable
		{
			get
			{
				return this._appRelativeVirtualPath;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x0002B8DB File Offset: 0x0002A8DB
		internal string VirtualPathStringWhicheverAvailable
		{
			get
			{
				if (this._virtualPath == null)
				{
					return this._appRelativeVirtualPath;
				}
				return this._virtualPath;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x0002B8F2 File Offset: 0x0002A8F2
		public string Extension
		{
			get
			{
				return UrlPath.GetExtension(this.VirtualPathString);
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x0002B8FF File Offset: 0x0002A8FF
		public string FileName
		{
			get
			{
				return UrlPath.GetFileName(this.VirtualPathStringNoTrailingSlash);
			}
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x0002B90C File Offset: 0x0002A90C
		public VirtualPath CombineWithAppRoot()
		{
			return HttpRuntime.AppDomainAppVirtualPathObject.Combine(this);
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x0002B91C File Offset: 0x0002A91C
		public VirtualPath Combine(VirtualPath relativePath)
		{
			if (relativePath == null)
			{
				throw new ArgumentNullException("relativePath");
			}
			if (!relativePath.IsRelative)
			{
				return relativePath;
			}
			this.FailIfRelativePath();
			string text = this.VirtualPathStringWhicheverAvailable;
			text = UrlPath.Combine(text, relativePath.VirtualPathString);
			return new VirtualPath(text);
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0002B967 File Offset: 0x0002A967
		internal VirtualPath SimpleCombine(string relativePath)
		{
			return this.SimpleCombine(relativePath, false);
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x0002B971 File Offset: 0x0002A971
		internal VirtualPath SimpleCombineWithDir(string directoryName)
		{
			return this.SimpleCombine(directoryName, true);
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x0002B97C File Offset: 0x0002A97C
		private VirtualPath SimpleCombine(string filename, bool addTrailingSlash)
		{
			string text = this.VirtualPathStringWhicheverAvailable + filename;
			if (addTrailingSlash)
			{
				text += "/";
			}
			VirtualPath virtualPath = new VirtualPath(text);
			virtualPath.CopyFlagsFrom(this, 7);
			return virtualPath;
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x0002B9B8 File Offset: 0x0002A9B8
		public VirtualPath MakeRelative(VirtualPath toVirtualPath)
		{
			VirtualPath virtualPath = new VirtualPath();
			this.FailIfRelativePath();
			toVirtualPath.FailIfRelativePath();
			virtualPath._virtualPath = UrlPath.MakeRelative(this.VirtualPathString, toVirtualPath.VirtualPathString);
			return virtualPath;
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x0002B9EF File Offset: 0x0002A9EF
		public string MapPath()
		{
			return HostingEnvironment.MapPath(this);
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x0002B9F7 File Offset: 0x0002A9F7
		internal string MapPathInternal()
		{
			return HostingEnvironment.MapPathInternal(this);
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0002B9FF File Offset: 0x0002A9FF
		internal string MapPathInternal(bool permitNull)
		{
			return HostingEnvironment.MapPathInternal(this, permitNull);
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0002BA08 File Offset: 0x0002AA08
		internal string MapPathInternal(VirtualPath baseVirtualDir, bool allowCrossAppMapping)
		{
			return HostingEnvironment.MapPathInternal(this, baseVirtualDir, allowCrossAppMapping);
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0002BA12 File Offset: 0x0002AA12
		public string GetFileHash(IEnumerable virtualPathDependencies)
		{
			return HostingEnvironment.VirtualPathProvider.GetFileHash(this, virtualPathDependencies);
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x0002BA20 File Offset: 0x0002AA20
		public CacheDependency GetCacheDependency(IEnumerable virtualPathDependencies, DateTime utcStart)
		{
			return HostingEnvironment.VirtualPathProvider.GetCacheDependency(this, virtualPathDependencies, utcStart);
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x0002BA2F File Offset: 0x0002AA2F
		public bool FileExists()
		{
			return HostingEnvironment.VirtualPathProvider.FileExists(this);
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x0002BA3C File Offset: 0x0002AA3C
		public bool DirectoryExists()
		{
			return HostingEnvironment.VirtualPathProvider.DirectoryExists(this);
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0002BA49 File Offset: 0x0002AA49
		public VirtualFile GetFile()
		{
			return HostingEnvironment.VirtualPathProvider.GetFile(this);
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0002BA56 File Offset: 0x0002AA56
		public VirtualDirectory GetDirectory()
		{
			return HostingEnvironment.VirtualPathProvider.GetDirectory(this);
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0002BA63 File Offset: 0x0002AA63
		public string GetCacheKey()
		{
			return HostingEnvironment.VirtualPathProvider.GetCacheKey(this);
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x0002BA70 File Offset: 0x0002AA70
		public Stream OpenFile()
		{
			return VirtualPathProvider.OpenFile(this);
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x0002BA78 File Offset: 0x0002AA78
		internal bool HasTrailingSlash
		{
			get
			{
				if (this._virtualPath != null)
				{
					return UrlPath.HasTrailingSlash(this._virtualPath);
				}
				return UrlPath.HasTrailingSlash(this._appRelativeVirtualPath);
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x0002BA9C File Offset: 0x0002AA9C
		public bool IsWithinAppRoot
		{
			get
			{
				if (!this.flags[1])
				{
					if (HttpRuntime.AppDomainIdInternal == null)
					{
						return true;
					}
					if (this.flags[4])
					{
						this.flags[2] = this._appRelativeVirtualPath != null;
					}
					else
					{
						this.flags[2] = UrlPath.IsEqualOrSubpath(HttpRuntime.AppDomainAppVirtualPathString, this.VirtualPathString);
					}
					this.flags[1] = true;
				}
				return this.flags[2];
			}
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0002BB20 File Offset: 0x0002AB20
		internal void FailIfNotWithinAppRoot()
		{
			if (!this.IsWithinAppRoot)
			{
				throw new ArgumentException(SR.GetString("Cross_app_not_allowed", new object[] { this.VirtualPathString }));
			}
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0002BB58 File Offset: 0x0002AB58
		internal void FailIfRelativePath()
		{
			if (this.IsRelative)
			{
				throw new ArgumentException(SR.GetString("VirtualPath_AllowRelativePath", new object[] { this._virtualPath }));
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x0002BB8E File Offset: 0x0002AB8E
		public bool IsRelative
		{
			get
			{
				return this._virtualPath != null && this._virtualPath[0] != '/';
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x0002BBAD File Offset: 0x0002ABAD
		public bool IsRoot
		{
			get
			{
				return this._virtualPath == "/";
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000ADB RID: 2779 RVA: 0x0002BBC0 File Offset: 0x0002ABC0
		public VirtualPath Parent
		{
			get
			{
				this.FailIfRelativePath();
				if (this.IsRoot)
				{
					return null;
				}
				string text = this.VirtualPathStringWhicheverAvailable;
				text = UrlPath.RemoveSlashFromPathIfNeeded(text);
				if (text == "~")
				{
					text = this.VirtualPathStringNoTrailingSlash;
				}
				int num = text.LastIndexOf('/');
				if (num == 0)
				{
					return VirtualPath.RootVirtualPath;
				}
				text = text.Substring(0, num + 1);
				return new VirtualPath(text);
			}
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0002BC22 File Offset: 0x0002AC22
		internal static VirtualPath Combine(VirtualPath v1, VirtualPath v2)
		{
			if (v1 == null)
			{
				v1 = HttpRuntime.AppDomainAppVirtualPathObject;
			}
			if (v1 == null)
			{
				v2.FailIfRelativePath();
				return v2;
			}
			return v1.Combine(v2);
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0002BC4C File Offset: 0x0002AC4C
		public static bool operator ==(VirtualPath v1, VirtualPath v2)
		{
			return VirtualPath.Equals(v1, v2);
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0002BC55 File Offset: 0x0002AC55
		public static bool operator !=(VirtualPath v1, VirtualPath v2)
		{
			return !VirtualPath.Equals(v1, v2);
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0002BC61 File Offset: 0x0002AC61
		public static bool Equals(VirtualPath v1, VirtualPath v2)
		{
			return v1 == v2 || (v1 != null && v2 != null && VirtualPath.EqualsHelper(v1, v2));
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0002BC78 File Offset: 0x0002AC78
		public override bool Equals(object value)
		{
			if (value == null)
			{
				return false;
			}
			VirtualPath virtualPath = value as VirtualPath;
			return virtualPath != null && VirtualPath.EqualsHelper(virtualPath, this);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x0002BC9D File Offset: 0x0002AC9D
		private static bool EqualsHelper(VirtualPath v1, VirtualPath v2)
		{
			return StringComparer.InvariantCultureIgnoreCase.Compare(v1.VirtualPathString, v2.VirtualPathString) == 0;
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0002BCB8 File Offset: 0x0002ACB8
		public override int GetHashCode()
		{
			return StringComparer.InvariantCultureIgnoreCase.GetHashCode(this.VirtualPathString);
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0002BCCA File Offset: 0x0002ACCA
		public override string ToString()
		{
			if (this._virtualPath == null && HttpRuntime.AppDomainAppVirtualPathObject == null)
			{
				return this._appRelativeVirtualPath;
			}
			return this.VirtualPathString;
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0002BCEE File Offset: 0x0002ACEE
		private void CopyFlagsFrom(VirtualPath virtualPath, int mask)
		{
			this.flags.IntegerValue = this.flags.IntegerValue | (virtualPath.flags.IntegerValue & mask);
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0002BD0F File Offset: 0x0002AD0F
		internal static string GetVirtualPathString(VirtualPath virtualPath)
		{
			if (!(virtualPath == null))
			{
				return virtualPath.VirtualPathString;
			}
			return null;
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0002BD22 File Offset: 0x0002AD22
		internal static string GetVirtualPathStringNoTrailingSlash(VirtualPath virtualPath)
		{
			if (!(virtualPath == null))
			{
				return virtualPath.VirtualPathStringNoTrailingSlash;
			}
			return null;
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x0002BD35 File Offset: 0x0002AD35
		internal static string GetAppRelativeVirtualPathString(VirtualPath virtualPath)
		{
			if (!(virtualPath == null))
			{
				return virtualPath.AppRelativeVirtualPathString;
			}
			return null;
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0002BD48 File Offset: 0x0002AD48
		internal static string GetAppRelativeVirtualPathStringOrEmpty(VirtualPath virtualPath)
		{
			if (!(virtualPath == null))
			{
				return virtualPath.AppRelativeVirtualPathString;
			}
			return string.Empty;
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0002BD5F File Offset: 0x0002AD5F
		public static VirtualPath Create(string virtualPath)
		{
			return VirtualPath.Create(virtualPath, VirtualPathOptions.AllowAllPath);
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0002BD69 File Offset: 0x0002AD69
		public static VirtualPath CreateTrailingSlash(string virtualPath)
		{
			return VirtualPath.Create(virtualPath, VirtualPathOptions.EnsureTrailingSlash | VirtualPathOptions.AllowAbsolutePath | VirtualPathOptions.AllowAppRelativePath | VirtualPathOptions.AllowRelativePath);
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x0002BD73 File Offset: 0x0002AD73
		public static VirtualPath CreateAllowNull(string virtualPath)
		{
			return VirtualPath.Create(virtualPath, VirtualPathOptions.AllowNull | VirtualPathOptions.AllowAbsolutePath | VirtualPathOptions.AllowAppRelativePath | VirtualPathOptions.AllowRelativePath);
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x0002BD7D File Offset: 0x0002AD7D
		public static VirtualPath CreateAbsolute(string virtualPath)
		{
			return VirtualPath.Create(virtualPath, VirtualPathOptions.AllowAbsolutePath);
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x0002BD86 File Offset: 0x0002AD86
		public static VirtualPath CreateNonRelative(string virtualPath)
		{
			return VirtualPath.Create(virtualPath, VirtualPathOptions.AllowAbsolutePath | VirtualPathOptions.AllowAppRelativePath);
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0002BD90 File Offset: 0x0002AD90
		public static VirtualPath CreateAbsoluteTrailingSlash(string virtualPath)
		{
			return VirtualPath.Create(virtualPath, VirtualPathOptions.EnsureTrailingSlash | VirtualPathOptions.AllowAbsolutePath);
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0002BD99 File Offset: 0x0002AD99
		public static VirtualPath CreateNonRelativeTrailingSlash(string virtualPath)
		{
			return VirtualPath.Create(virtualPath, VirtualPathOptions.EnsureTrailingSlash | VirtualPathOptions.AllowAbsolutePath | VirtualPathOptions.AllowAppRelativePath);
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0002BDA3 File Offset: 0x0002ADA3
		public static VirtualPath CreateAbsoluteAllowNull(string virtualPath)
		{
			return VirtualPath.Create(virtualPath, VirtualPathOptions.AllowNull | VirtualPathOptions.AllowAbsolutePath);
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0002BDAC File Offset: 0x0002ADAC
		public static VirtualPath CreateNonRelativeAllowNull(string virtualPath)
		{
			return VirtualPath.Create(virtualPath, VirtualPathOptions.AllowNull | VirtualPathOptions.AllowAbsolutePath | VirtualPathOptions.AllowAppRelativePath);
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0002BDB6 File Offset: 0x0002ADB6
		public static VirtualPath CreateNonRelativeTrailingSlashAllowNull(string virtualPath)
		{
			return VirtualPath.Create(virtualPath, VirtualPathOptions.AllowNull | VirtualPathOptions.EnsureTrailingSlash | VirtualPathOptions.AllowAbsolutePath | VirtualPathOptions.AllowAppRelativePath);
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0002BDC0 File Offset: 0x0002ADC0
		public static VirtualPath Create(string virtualPath, VirtualPathOptions options)
		{
			if (virtualPath != null)
			{
				virtualPath = virtualPath.Trim();
			}
			if (string.IsNullOrEmpty(virtualPath))
			{
				if ((options & VirtualPathOptions.AllowNull) != (VirtualPathOptions)0)
				{
					return null;
				}
				throw new ArgumentNullException("virtualPath");
			}
			else
			{
				if (VirtualPath.ContainsIllegalVirtualPathChars(virtualPath))
				{
					throw new HttpException(SR.GetString("Invalid_vpath", new object[] { virtualPath }));
				}
				string text = UrlPath.FixVirtualPathSlashes(virtualPath);
				if ((options & VirtualPathOptions.FailIfMalformed) != (VirtualPathOptions)0 && !object.ReferenceEquals(virtualPath, text))
				{
					throw new HttpException(SR.GetString("Invalid_vpath", new object[] { virtualPath }));
				}
				virtualPath = text;
				if ((options & VirtualPathOptions.EnsureTrailingSlash) != (VirtualPathOptions)0)
				{
					virtualPath = UrlPath.AppendSlashToPathIfNeeded(virtualPath);
				}
				VirtualPath virtualPath2 = new VirtualPath();
				if (UrlPath.IsAppRelativePath(virtualPath))
				{
					virtualPath = UrlPath.ReduceVirtualPath(virtualPath);
					if (virtualPath[0] == '~')
					{
						if ((options & VirtualPathOptions.AllowAppRelativePath) == (VirtualPathOptions)0)
						{
							throw new ArgumentException(SR.GetString("VirtualPath_AllowAppRelativePath", new object[] { virtualPath }));
						}
						virtualPath2._appRelativeVirtualPath = virtualPath;
					}
					else
					{
						if ((options & VirtualPathOptions.AllowAbsolutePath) == (VirtualPathOptions)0)
						{
							throw new ArgumentException(SR.GetString("VirtualPath_AllowAbsolutePath", new object[] { virtualPath }));
						}
						virtualPath2._virtualPath = virtualPath;
					}
				}
				else if (virtualPath[0] != '/')
				{
					if ((options & VirtualPathOptions.AllowRelativePath) == (VirtualPathOptions)0)
					{
						throw new ArgumentException(SR.GetString("VirtualPath_AllowRelativePath", new object[] { virtualPath }));
					}
					virtualPath2._virtualPath = virtualPath;
				}
				else
				{
					if ((options & VirtualPathOptions.AllowAbsolutePath) == (VirtualPathOptions)0)
					{
						throw new ArgumentException(SR.GetString("VirtualPath_AllowAbsolutePath", new object[] { virtualPath }));
					}
					virtualPath2._virtualPath = UrlPath.ReduceVirtualPath(virtualPath);
				}
				return virtualPath2;
			}
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0002BF40 File Offset: 0x0002AF40
		[Conditional("DBG")]
		private void ValidateState()
		{
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0002BF42 File Offset: 0x0002AF42
		private static bool ContainsIllegalVirtualPathChars(string virtualPath)
		{
			if (!VirtualPath.s_VerCompatRegLookedUp)
			{
				VirtualPath.LookUpRegForVerCompat();
			}
			return virtualPath.IndexOfAny(VirtualPath.s_illegalVirtualPathChars) >= 0;
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0002BF64 File Offset: 0x0002AF64
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		private static void LookUpRegForVerCompat()
		{
			lock (VirtualPath.s_VerCompatLock)
			{
				if (!VirtualPath.s_VerCompatRegLookedUp)
				{
					try
					{
						object value = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\ASP.NET", "VerificationCompatibility", 0);
						if (value != null && (value is int || value is uint) && (int)value == 1)
						{
							VirtualPath.s_illegalVirtualPathChars = VirtualPath.s_illegalVirtualPathChars_VerCompat;
						}
						VirtualPath.s_VerCompatRegLookedUp = true;
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0002C000 File Offset: 0x0002B000
		// Note: this type is marked as 'beforefieldinit'.
		static VirtualPath()
		{
			char[] array = new char[1];
			VirtualPath.s_illegalVirtualPathChars_VerCompat = array;
			VirtualPath.s_VerCompatRegLookedUp = false;
			VirtualPath.s_VerCompatLock = new object();
			VirtualPath.RootVirtualPath = VirtualPath.Create("/");
		}

		// Token: 0x0400132B RID: 4907
		private const int isWithinAppRootComputed = 1;

		// Token: 0x0400132C RID: 4908
		private const int isWithinAppRoot = 2;

		// Token: 0x0400132D RID: 4909
		private const int appRelativeAttempted = 4;

		// Token: 0x0400132E RID: 4910
		private static char[] s_illegalVirtualPathChars = new char[] { ':', '?', '*', '\0' };

		// Token: 0x0400132F RID: 4911
		private static char[] s_illegalVirtualPathChars_VerCompat;

		// Token: 0x04001330 RID: 4912
		private static bool s_VerCompatRegLookedUp;

		// Token: 0x04001331 RID: 4913
		private static object s_VerCompatLock;

		// Token: 0x04001332 RID: 4914
		private string _appRelativeVirtualPath;

		// Token: 0x04001333 RID: 4915
		private string _virtualPath;

		// Token: 0x04001334 RID: 4916
		private SimpleBitVector32 flags;

		// Token: 0x04001335 RID: 4917
		internal static VirtualPath RootVirtualPath;
	}
}
