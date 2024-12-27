using System;
using System.IO;
using System.Threading;

namespace System.Deployment.Application
{
	// Token: 0x020000D0 RID: 208
	internal static class UriHelper
	{
		// Token: 0x0600056F RID: 1391 RVA: 0x0001D46E File Offset: 0x0001C46E
		public static void ValidateSupportedScheme(Uri uri)
		{
			if (!UriHelper.IsSupportedScheme(uri))
			{
				throw new InvalidDeploymentException(ExceptionTypes.UriSchemeNotSupported, Resources.GetString("Ex_NotSupportedUriScheme"));
			}
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0001D48A File Offset: 0x0001C48A
		public static void ValidateSupportedSchemeInArgument(Uri uri, string argumentName)
		{
			if (!UriHelper.IsSupportedScheme(uri))
			{
				throw new ArgumentException(Resources.GetString("Ex_NotSupportedUriScheme"), argumentName);
			}
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001D4A5 File Offset: 0x0001C4A5
		public static bool IsSupportedScheme(Uri uri)
		{
			return uri.Scheme == Uri.UriSchemeFile || uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001D4E0 File Offset: 0x0001C4E0
		public static Uri UriFromRelativeFilePath(Uri baseUri, string path)
		{
			if (!UriHelper.IsValidRelativeFilePath(path))
			{
				throw new ArgumentException(Resources.GetString("Ex_InvalidRelativePath"));
			}
			if (path.IndexOf('%') >= 0)
			{
				path = path.Replace("%", Uri.HexEscape('%'));
			}
			if (path.IndexOf('#') >= 0)
			{
				path = path.Replace("#", Uri.HexEscape('#'));
			}
			Uri uri = new Uri(baseUri, path);
			UriHelper.ValidateSupportedScheme(uri);
			return uri;
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0001D554 File Offset: 0x0001C554
		public static bool IsValidRelativeFilePath(string path)
		{
			if (path == null || path.Length == 0 || path.IndexOfAny(UriHelper.InvalidRelativePathChars) >= 0)
			{
				return false;
			}
			if (Path.IsPathRooted(path))
			{
				return false;
			}
			string text = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			string text2 = Path.Combine(Path.DirectorySeparatorChar.ToString(), text);
			string fullPath = Path.GetFullPath(text2);
			string pathRoot = Path.GetPathRoot(fullPath);
			string text3 = fullPath.Substring(pathRoot.Length);
			if (text3.Length > 0 && text3[0] == '\\')
			{
				text3 = text3.Substring(1);
			}
			return string.Compare(text3, text, StringComparison.Ordinal) == 0;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001D5F6 File Offset: 0x0001C5F6
		public static string NormalizePathDirectorySeparators(string path)
		{
			if (path == null)
			{
				return null;
			}
			return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0001D60D File Offset: 0x0001C60D
		public static bool PathContainDirectorySeparators(string path)
		{
			return path != null && path.IndexOfAny(UriHelper._directorySeparators) >= 0;
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x0001D628 File Offset: 0x0001C628
		private static char[] InvalidRelativePathChars
		{
			get
			{
				if (UriHelper._invalidRelativePathChars == null)
				{
					char[] invalidPathChars = Path.GetInvalidPathChars();
					char[] array = new char[invalidPathChars.Length + 3];
					invalidPathChars.CopyTo(array, 0);
					int num = invalidPathChars.Length;
					array[num++] = Path.VolumeSeparatorChar;
					array[num++] = '*';
					array[num++] = '?';
					Interlocked.CompareExchange(ref UriHelper._invalidRelativePathChars, array, null);
				}
				return (char[])UriHelper._invalidRelativePathChars;
			}
		}

		// Token: 0x0400048A RID: 1162
		private static object _invalidRelativePathChars;

		// Token: 0x0400048B RID: 1163
		private static char[] _directorySeparators = new char[]
		{
			Path.DirectorySeparatorChar,
			Path.AltDirectorySeparatorChar
		};
	}
}
