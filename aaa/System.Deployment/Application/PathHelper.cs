using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace System.Deployment.Application
{
	// Token: 0x020000E4 RID: 228
	internal static class PathHelper
	{
		// Token: 0x060005D9 RID: 1497 RVA: 0x0001E3B4 File Offset: 0x0001D3B4
		public static string GetShortPath(string longPath)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			int shortPathName = NativeMethods.GetShortPathName(longPath, stringBuilder, stringBuilder.Capacity);
			if (shortPathName == 0)
			{
				PathHelper.GetShortPathNameThrowExceptionForLastError(longPath);
			}
			if (shortPathName >= stringBuilder.Capacity)
			{
				stringBuilder.Capacity = shortPathName + 1;
				if (NativeMethods.GetShortPathName(longPath, stringBuilder, stringBuilder.Capacity) == 0)
				{
					PathHelper.GetShortPathNameThrowExceptionForLastError(longPath);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0001E414 File Offset: 0x0001D414
		public static string ShortShimDllPath
		{
			get
			{
				if (PathHelper._shortShimDllPath == null)
				{
					string text = Path.Combine(Environment.SystemDirectory, "dfshim.dll");
					Interlocked.CompareExchange(ref PathHelper._shortShimDllPath, PathHelper.GetShortPath(text), null);
				}
				return (string)PathHelper._shortShimDllPath;
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001E454 File Offset: 0x0001D454
		public static string GenerateRandomPath(uint segmentCount)
		{
			if (segmentCount == 0U)
			{
				return null;
			}
			uint num = 11U * segmentCount;
			uint num2 = (uint)Math.Ceiling(num * 0.625);
			byte[] array = new byte[num2];
			RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider();
			rngcryptoServiceProvider.GetBytes(array);
			string text = Base32String.FromBytes(array);
			if ((long)text.Length < (long)((ulong)num))
			{
				throw new DeploymentException(Resources.GetString("Ex_TempPathRandomStringTooShort"));
			}
			if (text.IndexOf('\\') >= 0)
			{
				throw new DeploymentException(Resources.GetString("Ex_TempPathRandomStringInvalid"));
			}
			for (int i = (int)(segmentCount - 1U); i > 0; i--)
			{
				int num3 = i * 11;
				if (num3 >= text.Length)
				{
					throw new DeploymentException(Resources.GetString("Ex_TempPathRandomStringInvalid"));
				}
				text = text.Insert(num3, "\\");
			}
			string[] array2 = text.Split(new char[] { '\\' });
			if ((long)array2.Length < (long)((ulong)segmentCount))
			{
				throw new DeploymentException(Resources.GetString("Ex_TempPathRandomStringInvalid"));
			}
			string text2 = null;
			for (uint num4 = 0U; num4 < segmentCount; num4 += 1U)
			{
				if (array2[(int)((UIntPtr)num4)].Length < 11)
				{
					throw new DeploymentException(Resources.GetString("Ex_TempPathRandomStringInvalid"));
				}
				string text3 = array2[(int)((UIntPtr)num4)].Substring(0, 11);
				text3 = text3.Insert(8, ".");
				if (text2 == null)
				{
					text2 = text3;
				}
				else
				{
					text2 = Path.Combine(text2, text3);
				}
			}
			return text2;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0001E5B2 File Offset: 0x0001D5B2
		public static string GetRootSegmentPath(string path, uint segmentCount)
		{
			if (segmentCount == 0U)
			{
				throw new ArgumentException("segmentCount");
			}
			if (segmentCount == 1U)
			{
				return path;
			}
			return PathHelper.GetRootSegmentPath(Path.GetDirectoryName(path), segmentCount - 1U);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0001E5D8 File Offset: 0x0001D5D8
		private static void GetShortPathNameThrowExceptionForLastError(string path)
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (lastWin32Error == 2)
			{
				throw new FileNotFoundException(path);
			}
			if (lastWin32Error == 87)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_ShortFileNameNotSupported"));
			}
			throw new Win32Exception(lastWin32Error);
		}

		// Token: 0x040004B0 RID: 1200
		private const int MAX_PATH = 260;

		// Token: 0x040004B1 RID: 1201
		private const int ERROR_FILE_NOT_FOUND = 2;

		// Token: 0x040004B2 RID: 1202
		private const int ERROR_INVALID_PARAMETER = 87;

		// Token: 0x040004B3 RID: 1203
		private static object _shortShimDllPath;
	}
}
