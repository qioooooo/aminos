using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000030 RID: 48
	internal sealed class FileSecurity
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x00005890 File Offset: 0x00004890
		internal static byte[] GetDacl(string filename)
		{
			int num = 0;
			int fileSecurity = UnsafeNativeMethods.GetFileSecurity(filename, 7, null, 0, ref num);
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (fileSecurity != 0)
			{
				return FileSecurity.s_nullDacl;
			}
			int num2 = HttpException.HResultFromLastError(lastWin32Error);
			if (num2 != -2147024774)
			{
				return null;
			}
			byte[] array = new byte[num];
			if (UnsafeNativeMethods.GetFileSecurity(filename, 7, array, array.Length, ref num) == 0)
			{
				return null;
			}
			byte[] array2 = (byte[])FileSecurity.s_interned[array];
			if (array2 == null)
			{
				lock (FileSecurity.s_interned.SyncRoot)
				{
					array2 = (byte[])FileSecurity.s_interned[array];
					if (array2 == null)
					{
						array2 = array;
						FileSecurity.s_interned[array2] = array2;
					}
				}
			}
			return array2;
		}

		// Token: 0x04000DAD RID: 3501
		private const int DACL_INFORMATION = 7;

		// Token: 0x04000DAE RID: 3502
		private static Hashtable s_interned = new Hashtable(0, 1f, new FileSecurity.DaclComparer());

		// Token: 0x04000DAF RID: 3503
		private static byte[] s_nullDacl = new byte[0];

		// Token: 0x02000031 RID: 49
		private class DaclComparer : IEqualityComparer
		{
			// Token: 0x060000F4 RID: 244 RVA: 0x00005960 File Offset: 0x00004960
			private int Compare(byte[] a, byte[] b)
			{
				int num = a.Length - b.Length;
				int num2 = 0;
				while (num == 0 && num2 < a.Length)
				{
					num = (int)(a[num2] - b[num2]);
					num2++;
				}
				return num;
			}

			// Token: 0x060000F5 RID: 245 RVA: 0x00005990 File Offset: 0x00004990
			bool IEqualityComparer.Equals(object x, object y)
			{
				if (x == null && y == null)
				{
					return true;
				}
				if (x == null || y == null)
				{
					return false;
				}
				byte[] array = x as byte[];
				byte[] array2 = y as byte[];
				return array != null && array2 != null && this.Compare(array, array2) == 0;
			}

			// Token: 0x060000F6 RID: 246 RVA: 0x000059D0 File Offset: 0x000049D0
			int IEqualityComparer.GetHashCode(object obj)
			{
				byte[] array = (byte[])obj;
				HashCodeCombiner hashCodeCombiner = new HashCodeCombiner();
				foreach (byte b in array)
				{
					hashCodeCombiner.AddObject(b);
				}
				return hashCodeCombiner.CombinedHash32;
			}
		}
	}
}
