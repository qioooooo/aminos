using System;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x020000E9 RID: 233
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public static class VirtualPathUtility
	{
		// Token: 0x06000AF8 RID: 2808 RVA: 0x0002C050 File Offset: 0x0002B050
		public static bool IsAbsolute(string virtualPath)
		{
			VirtualPath virtualPath2 = VirtualPath.Create(virtualPath);
			return !virtualPath2.IsRelative && virtualPath2.VirtualPathStringIfAvailable != null;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0002C07C File Offset: 0x0002B07C
		public static bool IsAppRelative(string virtualPath)
		{
			VirtualPath virtualPath2 = VirtualPath.Create(virtualPath);
			return virtualPath2.VirtualPathStringIfAvailable == null;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0002C09C File Offset: 0x0002B09C
		public static string ToAppRelative(string virtualPath)
		{
			VirtualPath virtualPath2 = VirtualPath.CreateNonRelative(virtualPath);
			return virtualPath2.AppRelativeVirtualPathString;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0002C0B8 File Offset: 0x0002B0B8
		public static string ToAppRelative(string virtualPath, string applicationPath)
		{
			VirtualPath virtualPath2 = VirtualPath.CreateNonRelative(virtualPath);
			if (virtualPath2.AppRelativeVirtualPathStringIfAvailable != null)
			{
				return virtualPath2.AppRelativeVirtualPathStringIfAvailable;
			}
			VirtualPath virtualPath3 = VirtualPath.CreateAbsoluteTrailingSlash(applicationPath);
			return UrlPath.MakeVirtualPathAppRelative(virtualPath2.VirtualPathString, virtualPath3.VirtualPathString, true);
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0002C0F4 File Offset: 0x0002B0F4
		public static string ToAbsolute(string virtualPath)
		{
			VirtualPath virtualPath2 = VirtualPath.CreateNonRelative(virtualPath);
			return virtualPath2.VirtualPathString;
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0002C110 File Offset: 0x0002B110
		public static string ToAbsolute(string virtualPath, string applicationPath)
		{
			VirtualPath virtualPath2 = VirtualPath.CreateNonRelative(virtualPath);
			if (virtualPath2.VirtualPathStringIfAvailable != null)
			{
				return virtualPath2.VirtualPathStringIfAvailable;
			}
			VirtualPath virtualPath3 = VirtualPath.CreateAbsoluteTrailingSlash(applicationPath);
			return UrlPath.MakeVirtualPathAppAbsolute(virtualPath2.AppRelativeVirtualPathString, virtualPath3.VirtualPathString);
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0002C14C File Offset: 0x0002B14C
		public static string GetFileName(string virtualPath)
		{
			VirtualPath virtualPath2 = VirtualPath.CreateNonRelative(virtualPath);
			return virtualPath2.FileName;
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0002C168 File Offset: 0x0002B168
		public static string GetDirectory(string virtualPath)
		{
			VirtualPath virtualPath2 = VirtualPath.CreateNonRelative(virtualPath);
			virtualPath2 = virtualPath2.Parent;
			if (virtualPath2 == null)
			{
				return null;
			}
			return virtualPath2.VirtualPathStringWhicheverAvailable;
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x0002C194 File Offset: 0x0002B194
		public static string GetExtension(string virtualPath)
		{
			VirtualPath virtualPath2 = VirtualPath.Create(virtualPath);
			return virtualPath2.Extension;
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0002C1AE File Offset: 0x0002B1AE
		public static string AppendTrailingSlash(string virtualPath)
		{
			return UrlPath.AppendSlashToPathIfNeeded(virtualPath);
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x0002C1B6 File Offset: 0x0002B1B6
		public static string RemoveTrailingSlash(string virtualPath)
		{
			return UrlPath.RemoveSlashFromPathIfNeeded(virtualPath);
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0002C1C0 File Offset: 0x0002B1C0
		public static string Combine(string basePath, string relativePath)
		{
			VirtualPath virtualPath = VirtualPath.Combine(VirtualPath.CreateNonRelative(basePath), VirtualPath.Create(relativePath));
			return virtualPath.VirtualPathStringWhicheverAvailable;
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0002C1E5 File Offset: 0x0002B1E5
		public static string MakeRelative(string fromPath, string toPath)
		{
			return UrlPath.MakeRelative(fromPath, toPath);
		}
	}
}
