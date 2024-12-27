using System;
using System.IO;
using System.Threading;

namespace System.Deployment.Application
{
	// Token: 0x020000E3 RID: 227
	internal static class PathTwiddler
	{
		// Token: 0x060005D6 RID: 1494 RVA: 0x0001E2F2 File Offset: 0x0001D2F2
		public static string FilterString(string input, char chReplace, bool fMultiReplace)
		{
			return PathTwiddler.FilterString(input, PathTwiddler.InvalidFileDirNameChars, chReplace, fMultiReplace);
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x0001E301 File Offset: 0x0001D301
		private static char[] InvalidFileDirNameChars
		{
			get
			{
				if (PathTwiddler._invalidFileDirNameChars == null)
				{
					Interlocked.CompareExchange(ref PathTwiddler._invalidFileDirNameChars, Path.GetInvalidFileNameChars(), null);
				}
				return (char[])PathTwiddler._invalidFileDirNameChars;
			}
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0001E328 File Offset: 0x0001D328
		private static string FilterString(string input, char[] toFilter, char chReplacement, bool fMultiReplace)
		{
			int num = 0;
			bool flag = false;
			bool flag2 = false;
			if (input == null)
			{
				return null;
			}
			char[] array = input.ToCharArray();
			char[] array2 = new char[array.Length];
			Array.Sort<char>(toFilter);
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = Array.BinarySearch<char>(toFilter, array[i]);
				if (num2 < 0)
				{
					array2[num++] = array[i];
					flag2 = true;
					if (flag)
					{
						flag = false;
					}
				}
				else if (fMultiReplace || !flag)
				{
					array2[num++] = chReplacement;
					flag = true;
				}
			}
			if (!flag2 || num <= 0)
			{
				return null;
			}
			return new string(array2, 0, num);
		}

		// Token: 0x040004AF RID: 1199
		private static object _invalidFileDirNameChars;
	}
}
