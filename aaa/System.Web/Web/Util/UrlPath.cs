using System;
using System.Collections;
using System.IO;
using System.Text;

namespace System.Web.Util
{
	// Token: 0x02000798 RID: 1944
	internal static class UrlPath
	{
		// Token: 0x06005D38 RID: 23864 RVA: 0x0017558C File Offset: 0x0017458C
		internal static bool IsRooted(string basepath)
		{
			return string.IsNullOrEmpty(basepath) || basepath[0] == '/' || basepath[0] == '\\';
		}

		// Token: 0x06005D39 RID: 23865 RVA: 0x001755AE File Offset: 0x001745AE
		internal static bool IsRelativeUrl(string virtualPath)
		{
			return virtualPath.IndexOf(":", StringComparison.Ordinal) == -1 && !UrlPath.IsRooted(virtualPath);
		}

		// Token: 0x06005D3A RID: 23866 RVA: 0x001755CC File Offset: 0x001745CC
		internal static bool IsAppRelativePath(string path)
		{
			if (path == null)
			{
				return false;
			}
			int length = path.Length;
			return length != 0 && path[0] == '~' && (length == 1 || path[1] == '\\' || path[1] == '/');
		}

		// Token: 0x06005D3B RID: 23867 RVA: 0x00175615 File Offset: 0x00174615
		internal static bool IsValidVirtualPathWithoutProtocol(string path)
		{
			return path != null && path.IndexOf(":", StringComparison.Ordinal) == -1;
		}

		// Token: 0x06005D3C RID: 23868 RVA: 0x00175630 File Offset: 0x00174630
		internal static string GetDirectory(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentException(SR.GetString("Empty_path_has_no_directory"));
			}
			if (path[0] != '/' && path[0] != '~')
			{
				throw new ArgumentException(SR.GetString("Path_must_be_rooted", new object[] { path }));
			}
			if (path.Length == 1)
			{
				return path;
			}
			int num = path.LastIndexOf('/');
			if (num < 0)
			{
				throw new ArgumentException(SR.GetString("Path_must_be_rooted", new object[] { path }));
			}
			return path.Substring(0, num + 1);
		}

		// Token: 0x06005D3D RID: 23869 RVA: 0x001756C5 File Offset: 0x001746C5
		private static bool IsDirectorySeparatorChar(char ch)
		{
			return ch == '\\' || ch == '/';
		}

		// Token: 0x06005D3E RID: 23870 RVA: 0x001756D3 File Offset: 0x001746D3
		internal static bool IsAbsolutePhysicalPath(string path)
		{
			return path != null && path.Length >= 3 && ((path[1] == ':' && UrlPath.IsDirectorySeparatorChar(path[2])) || UrlPath.IsUncSharePath(path));
		}

		// Token: 0x06005D3F RID: 23871 RVA: 0x00175704 File Offset: 0x00174704
		internal static bool IsUncSharePath(string path)
		{
			return path.Length > 2 && UrlPath.IsDirectorySeparatorChar(path[0]) && UrlPath.IsDirectorySeparatorChar(path[1]);
		}

		// Token: 0x06005D40 RID: 23872 RVA: 0x00175730 File Offset: 0x00174730
		internal static void CheckValidVirtualPath(string path)
		{
			if (UrlPath.IsAbsolutePhysicalPath(path))
			{
				throw new HttpException(SR.GetString("Physical_path_not_allowed", new object[] { path }));
			}
			if (path.IndexOf(':') >= 0)
			{
				throw new HttpException(SR.GetString("Invalid_vpath", new object[] { path }));
			}
		}

		// Token: 0x06005D41 RID: 23873 RVA: 0x00175788 File Offset: 0x00174788
		private static string Combine(string appPath, string basepath, string relative)
		{
			if (string.IsNullOrEmpty(relative))
			{
				throw new ArgumentNullException("relative");
			}
			if (string.IsNullOrEmpty(basepath))
			{
				throw new ArgumentNullException("basepath");
			}
			if (basepath[0] == '~' && basepath.Length == 1)
			{
				basepath = "~/";
			}
			else
			{
				int num = basepath.LastIndexOf('/');
				if (num < basepath.Length - 1)
				{
					basepath = basepath.Substring(0, num + 1);
				}
			}
			UrlPath.CheckValidVirtualPath(relative);
			string text;
			if (UrlPath.IsRooted(relative))
			{
				text = relative;
			}
			else
			{
				if (relative.Length == 1 && relative[0] == '~')
				{
					return appPath;
				}
				if (UrlPath.IsAppRelativePath(relative))
				{
					if (appPath.Length > 1)
					{
						text = appPath + "/" + relative.Substring(2);
					}
					else
					{
						text = "/" + relative.Substring(2);
					}
				}
				else
				{
					text = UrlPath.SimpleCombine(basepath, relative);
				}
			}
			return UrlPath.Reduce(text);
		}

		// Token: 0x06005D42 RID: 23874 RVA: 0x00175868 File Offset: 0x00174868
		internal static string Combine(string basepath, string relative)
		{
			return UrlPath.Combine(HttpRuntime.AppDomainAppVirtualPathString, basepath, relative);
		}

		// Token: 0x06005D43 RID: 23875 RVA: 0x00175876 File Offset: 0x00174876
		internal static string SimpleCombine(string basepath, string relative)
		{
			if (UrlPath.HasTrailingSlash(basepath))
			{
				return basepath + relative;
			}
			return basepath + "/" + relative;
		}

		// Token: 0x06005D44 RID: 23876 RVA: 0x00175894 File Offset: 0x00174894
		internal static string Reduce(string path)
		{
			string text = null;
			if (path != null)
			{
				int num = path.IndexOf('?');
				if (num >= 0)
				{
					text = path.Substring(num);
					path = path.Substring(0, num);
				}
			}
			path = UrlPath.FixVirtualPathSlashes(path);
			path = UrlPath.ReduceVirtualPath(path);
			if (text == null)
			{
				return path;
			}
			return path + text;
		}

		// Token: 0x06005D45 RID: 23877 RVA: 0x001758E4 File Offset: 0x001748E4
		internal static string ReduceVirtualPath(string path)
		{
			int length = path.Length;
			int num = 0;
			for (;;)
			{
				num = path.IndexOf('.', num);
				if (num < 0)
				{
					break;
				}
				if ((num == 0 || path[num - 1] == '/') && (num + 1 == length || path[num + 1] == '/' || (path[num + 1] == '.' && (num + 2 == length || path[num + 2] == '/'))))
				{
					goto IL_0062;
				}
				num++;
			}
			return path;
			IL_0062:
			ArrayList arrayList = new ArrayList();
			StringBuilder stringBuilder = new StringBuilder();
			num = 0;
			for (;;)
			{
				int num2 = num;
				num = path.IndexOf('/', num2 + 1);
				if (num < 0)
				{
					num = length;
				}
				if (num - num2 <= 3 && (num < 1 || path[num - 1] == '.') && (num2 + 1 >= length || path[num2 + 1] == '.'))
				{
					if (num - num2 == 3)
					{
						if (arrayList.Count == 0)
						{
							break;
						}
						if (arrayList.Count == 1 && UrlPath.IsAppRelativePath(path))
						{
							goto Block_14;
						}
						stringBuilder.Length = (int)arrayList[arrayList.Count - 1];
						arrayList.RemoveRange(arrayList.Count - 1, 1);
					}
				}
				else
				{
					arrayList.Add(stringBuilder.Length);
					stringBuilder.Append(path, num2, num - num2);
				}
				if (num == length)
				{
					goto Block_15;
				}
			}
			throw new HttpException(SR.GetString("Cannot_exit_up_top_directory"));
			Block_14:
			return UrlPath.ReduceVirtualPath(UrlPath.MakeVirtualPathAppAbsolute(path));
			Block_15:
			string text = stringBuilder.ToString();
			if (text.Length == 0)
			{
				if (length > 0 && path[0] == '/')
				{
					text = "/";
				}
				else
				{
					text = ".";
				}
			}
			return text;
		}

		// Token: 0x06005D46 RID: 23878 RVA: 0x00175A68 File Offset: 0x00174A68
		internal static string FixVirtualPathSlashes(string virtualPath)
		{
			virtualPath = virtualPath.Replace('\\', '/');
			for (;;)
			{
				string text = virtualPath.Replace("//", "/");
				if (text == virtualPath)
				{
					break;
				}
				virtualPath = text;
			}
			return virtualPath;
		}

		// Token: 0x06005D47 RID: 23879 RVA: 0x00175A9C File Offset: 0x00174A9C
		internal static string MakeRelative(string from, string to)
		{
			from = UrlPath.MakeVirtualPathAppAbsolute(from);
			to = UrlPath.MakeVirtualPathAppAbsolute(to);
			if (!UrlPath.IsRooted(from))
			{
				throw new ArgumentException(SR.GetString("Path_must_be_rooted", new object[] { from }));
			}
			if (!UrlPath.IsRooted(to))
			{
				throw new ArgumentException(SR.GetString("Path_must_be_rooted", new object[] { to }));
			}
			string text = null;
			if (to != null)
			{
				int num = to.IndexOf('?');
				if (num >= 0)
				{
					text = to.Substring(num);
					to = to.Substring(0, num);
				}
			}
			Uri uri = new Uri("file://foo" + from);
			Uri uri2 = new Uri("file://foo" + to);
			string text2;
			if (uri.Equals(uri2))
			{
				int num2 = to.LastIndexOfAny(UrlPath.s_slashChars);
				if (num2 >= 0)
				{
					if (num2 == to.Length - 1)
					{
						text2 = "./";
					}
					else
					{
						text2 = to.Substring(num2 + 1);
					}
				}
				else
				{
					text2 = to;
				}
			}
			else
			{
				text2 = uri.MakeRelative(uri2);
			}
			return text2 + text + uri2.Fragment;
		}

		// Token: 0x06005D48 RID: 23880 RVA: 0x00175BA8 File Offset: 0x00174BA8
		internal static string GetDirectoryOrRootName(string path)
		{
			string text = Path.GetDirectoryName(path);
			if (text == null)
			{
				text = Path.GetPathRoot(path);
			}
			return text;
		}

		// Token: 0x06005D49 RID: 23881 RVA: 0x00175BC7 File Offset: 0x00174BC7
		internal static string GetFileName(string virtualPath)
		{
			return Path.GetFileName(virtualPath);
		}

		// Token: 0x06005D4A RID: 23882 RVA: 0x00175BCF File Offset: 0x00174BCF
		internal static string GetFileNameWithoutExtension(string virtualPath)
		{
			return Path.GetFileNameWithoutExtension(virtualPath);
		}

		// Token: 0x06005D4B RID: 23883 RVA: 0x00175BD7 File Offset: 0x00174BD7
		internal static string GetExtension(string virtualPath)
		{
			return Path.GetExtension(virtualPath);
		}

		// Token: 0x06005D4C RID: 23884 RVA: 0x00175BDF File Offset: 0x00174BDF
		internal static bool HasTrailingSlash(string virtualPath)
		{
			return virtualPath[virtualPath.Length - 1] == '/';
		}

		// Token: 0x06005D4D RID: 23885 RVA: 0x00175BF4 File Offset: 0x00174BF4
		internal static string AppendSlashToPathIfNeeded(string path)
		{
			if (path == null)
			{
				return null;
			}
			int length = path.Length;
			if (length == 0)
			{
				return path;
			}
			if (path[length - 1] != '/')
			{
				path += '/';
			}
			return path;
		}

		// Token: 0x06005D4E RID: 23886 RVA: 0x00175C30 File Offset: 0x00174C30
		internal static string RemoveSlashFromPathIfNeeded(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}
			int length = path.Length;
			if (length <= 1 || path[length - 1] != '/')
			{
				return path;
			}
			return path.Substring(0, length - 1);
		}

		// Token: 0x06005D4F RID: 23887 RVA: 0x00175C6C File Offset: 0x00174C6C
		private static bool VirtualPathStartsWithVirtualPath(string virtualPath1, string virtualPath2)
		{
			if (virtualPath1 == null)
			{
				throw new ArgumentNullException("virtualPath1");
			}
			if (virtualPath2 == null)
			{
				throw new ArgumentNullException("virtualPath2");
			}
			if (!StringUtil.StringStartsWithIgnoreCase(virtualPath1, virtualPath2))
			{
				return false;
			}
			int length = virtualPath2.Length;
			return virtualPath1.Length == length || length == 1 || virtualPath2[length - 1] == '/' || virtualPath1[length] == '/';
		}

		// Token: 0x06005D50 RID: 23888 RVA: 0x00175CD5 File Offset: 0x00174CD5
		internal static bool VirtualPathStartsWithAppPath(string virtualPath)
		{
			return UrlPath.VirtualPathStartsWithVirtualPath(virtualPath, HttpRuntime.AppDomainAppVirtualPathString);
		}

		// Token: 0x06005D51 RID: 23889 RVA: 0x00175CE2 File Offset: 0x00174CE2
		internal static string MakeVirtualPathAppRelative(string virtualPath)
		{
			return UrlPath.MakeVirtualPathAppRelative(virtualPath, HttpRuntime.AppDomainAppVirtualPathString, false);
		}

		// Token: 0x06005D52 RID: 23890 RVA: 0x00175CF0 File Offset: 0x00174CF0
		internal static string MakeVirtualPathAppRelativeOrNull(string virtualPath)
		{
			return UrlPath.MakeVirtualPathAppRelative(virtualPath, HttpRuntime.AppDomainAppVirtualPathString, true);
		}

		// Token: 0x06005D53 RID: 23891 RVA: 0x00175D00 File Offset: 0x00174D00
		internal static string MakeVirtualPathAppRelative(string virtualPath, string applicationPath, bool nullIfNotInApp)
		{
			if (virtualPath == null)
			{
				throw new ArgumentNullException("virtualPath");
			}
			int length = applicationPath.Length;
			int length2 = virtualPath.Length;
			if (length2 == length - 1 && StringUtil.StringStartsWithIgnoreCase(applicationPath, virtualPath))
			{
				return "~/";
			}
			if (!UrlPath.VirtualPathStartsWithVirtualPath(virtualPath, applicationPath))
			{
				if (nullIfNotInApp)
				{
					return null;
				}
				return virtualPath;
			}
			else
			{
				if (length2 == length)
				{
					return "~/";
				}
				if (length == 1)
				{
					return '~' + virtualPath;
				}
				return '~' + virtualPath.Substring(length - 1);
			}
		}

		// Token: 0x06005D54 RID: 23892 RVA: 0x00175D7F File Offset: 0x00174D7F
		internal static string MakeVirtualPathAppAbsolute(string virtualPath)
		{
			return UrlPath.MakeVirtualPathAppAbsolute(virtualPath, HttpRuntime.AppDomainAppVirtualPathString);
		}

		// Token: 0x06005D55 RID: 23893 RVA: 0x00175D8C File Offset: 0x00174D8C
		internal static string MakeVirtualPathAppAbsolute(string virtualPath, string applicationPath)
		{
			if (virtualPath.Length == 1 && virtualPath[0] == '~')
			{
				return applicationPath;
			}
			if (virtualPath.Length >= 2 && virtualPath[0] == '~' && (virtualPath[1] == '/' || virtualPath[1] == '\\'))
			{
				if (applicationPath.Length > 1)
				{
					return applicationPath + virtualPath.Substring(2);
				}
				return "/" + virtualPath.Substring(2);
			}
			else
			{
				if (!UrlPath.IsRooted(virtualPath))
				{
					throw new ArgumentOutOfRangeException("virtualPath");
				}
				return virtualPath;
			}
		}

		// Token: 0x06005D56 RID: 23894 RVA: 0x00175E18 File Offset: 0x00174E18
		internal static string MakeVirtualPathAppAbsoluteReduceAndCheck(string virtualPath)
		{
			if (virtualPath == null)
			{
				throw new ArgumentNullException("virtualPath");
			}
			string text = UrlPath.Reduce(UrlPath.MakeVirtualPathAppAbsolute(virtualPath));
			if (!UrlPath.VirtualPathStartsWithAppPath(text))
			{
				throw new ArgumentException(SR.GetString("Invalid_app_VirtualPath", new object[] { virtualPath }));
			}
			return text;
		}

		// Token: 0x06005D57 RID: 23895 RVA: 0x00175E64 File Offset: 0x00174E64
		internal static bool PathEndsWithExtraSlash(string path)
		{
			if (path == null)
			{
				return false;
			}
			int length = path.Length;
			return length != 0 && path[length - 1] == '\\' && (length != 3 || path[1] != ':');
		}

		// Token: 0x06005D58 RID: 23896 RVA: 0x00175EA4 File Offset: 0x00174EA4
		internal static bool PathIsDriveRoot(string path)
		{
			if (path != null)
			{
				int length = path.Length;
				if (length == 3 && path[1] == ':' && path[2] == '\\')
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005D59 RID: 23897 RVA: 0x00175ED8 File Offset: 0x00174ED8
		internal static bool IsEqualOrSubpath(string path, string subpath)
		{
			if (string.IsNullOrEmpty(path))
			{
				return true;
			}
			if (string.IsNullOrEmpty(subpath))
			{
				return false;
			}
			int num = path.Length;
			if (path[num - 1] == '/')
			{
				num--;
			}
			int num2 = subpath.Length;
			if (subpath[num2 - 1] == '/')
			{
				num2--;
			}
			return num2 >= num && StringUtil.EqualsIgnoreCase(path, 0, subpath, 0, num) && (num2 <= num || subpath[num] == '/');
		}

		// Token: 0x06005D5A RID: 23898 RVA: 0x00175F50 File Offset: 0x00174F50
		internal static bool IsPathOnSameServer(string absUriOrLocalPath, Uri currentRequestUri)
		{
			Uri uri;
			if (!Uri.TryCreate(absUriOrLocalPath, UriKind.Absolute, out uri))
			{
				return AppSettings.AllowRelaxedRelativeUrl || ((UrlPath.IsRooted(absUriOrLocalPath) || UrlPath.IsRelativeUrl(absUriOrLocalPath)) && !absUriOrLocalPath.TrimStart(new char[] { ' ' }).StartsWith("//"));
			}
			return uri.IsLoopback || string.Equals(currentRequestUri.Host, uri.Host, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x040031C2 RID: 12738
		internal const char appRelativeCharacter = '~';

		// Token: 0x040031C3 RID: 12739
		internal const string appRelativeCharacterString = "~/";

		// Token: 0x040031C4 RID: 12740
		private const string dummyProtocolAndServer = "file://foo";

		// Token: 0x040031C5 RID: 12741
		private static char[] s_slashChars = new char[] { '\\', '/' };
	}
}
