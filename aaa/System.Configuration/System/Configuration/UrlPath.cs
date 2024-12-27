using System;
using System.IO;

namespace System.Configuration
{
	// Token: 0x020000AF RID: 175
	internal static class UrlPath
	{
		// Token: 0x06000693 RID: 1683 RVA: 0x0001DAF8 File Offset: 0x0001CAF8
		internal static string GetDirectoryOrRootName(string path)
		{
			string text = Path.GetDirectoryName(path);
			if (text == null)
			{
				text = Path.GetPathRoot(path);
			}
			return text;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001DB18 File Offset: 0x0001CB18
		internal static bool IsEqualOrSubdirectory(string dir, string subdir)
		{
			if (string.IsNullOrEmpty(dir))
			{
				return true;
			}
			if (string.IsNullOrEmpty(subdir))
			{
				return false;
			}
			int num = dir.Length;
			if (dir[num - 1] == '\\')
			{
				num--;
			}
			int num2 = subdir.Length;
			if (subdir[num2 - 1] == '\\')
			{
				num2--;
			}
			return num2 >= num && string.Compare(dir, 0, subdir, 0, num, StringComparison.OrdinalIgnoreCase) == 0 && (num2 <= num || subdir[num] == '\\');
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0001DB90 File Offset: 0x0001CB90
		internal static bool IsEqualOrSubpath(string path, string subpath)
		{
			return UrlPath.IsEqualOrSubpathImpl(path, subpath, false);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001DB9A File Offset: 0x0001CB9A
		internal static bool IsSubpath(string path, string subpath)
		{
			return UrlPath.IsEqualOrSubpathImpl(path, subpath, true);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0001DBA4 File Offset: 0x0001CBA4
		private static bool IsEqualOrSubpathImpl(string path, string subpath, bool excludeEqual)
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
			return num2 >= num && (!excludeEqual || num2 != num) && string.Compare(path, 0, subpath, 0, num, StringComparison.OrdinalIgnoreCase) == 0 && (num2 <= num || subpath[num] == '/');
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0001DC25 File Offset: 0x0001CC25
		private static bool IsDirectorySeparatorChar(char ch)
		{
			return ch == '\\' || ch == '/';
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0001DC33 File Offset: 0x0001CC33
		private static bool IsAbsoluteLocalPhysicalPath(string path)
		{
			return path != null && path.Length >= 3 && path[1] == ':' && UrlPath.IsDirectorySeparatorChar(path[2]);
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0001DC5C File Offset: 0x0001CC5C
		private static bool IsAbsoluteUNCPhysicalPath(string path)
		{
			return path != null && path.Length >= 3 && UrlPath.IsDirectorySeparatorChar(path[0]) && UrlPath.IsDirectorySeparatorChar(path[1]);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0001DC88 File Offset: 0x0001CC88
		internal static string ConvertFileNameToUrl(string fileName)
		{
			string text;
			if (UrlPath.IsAbsoluteLocalPhysicalPath(fileName))
			{
				text = "file:///";
			}
			else
			{
				if (!UrlPath.IsAbsoluteUNCPhysicalPath(fileName))
				{
					throw ExceptionUtil.ParameterInvalid("filename");
				}
				text = "file:";
			}
			return text + fileName.Replace('\\', '/');
		}

		// Token: 0x040003FE RID: 1022
		private const string FILE_URL_LOCAL = "file:///";

		// Token: 0x040003FF RID: 1023
		private const string FILE_URL_UNC = "file:";
	}
}
