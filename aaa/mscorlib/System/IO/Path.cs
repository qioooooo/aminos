using System;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.IO
{
	// Token: 0x020005AA RID: 1450
	[ComVisible(true)]
	public static class Path
	{
		// Token: 0x0600363A RID: 13882 RVA: 0x000B71E0 File Offset: 0x000B61E0
		public static string ChangeExtension(string path, string extension)
		{
			if (path != null)
			{
				Path.CheckInvalidPathChars(path);
				string text = path;
				int num = path.Length;
				while (--num >= 0)
				{
					char c = path[num];
					if (c == '.')
					{
						text = path.Substring(0, num);
						break;
					}
					if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar || c == Path.VolumeSeparatorChar)
					{
						break;
					}
				}
				if (extension != null && path.Length != 0)
				{
					if (extension.Length == 0 || extension[0] != '.')
					{
						text += ".";
					}
					text += extension;
				}
				return text;
			}
			return null;
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x000B7270 File Offset: 0x000B6270
		public static string GetDirectoryName(string path)
		{
			if (path != null)
			{
				Path.CheckInvalidPathChars(path);
				path = Path.FixupPath(path);
				int rootLength = Path.GetRootLength(path);
				int num = path.Length;
				if (num > rootLength)
				{
					num = path.Length;
					if (num == rootLength)
					{
						return null;
					}
					while (num > rootLength && path[--num] != Path.DirectorySeparatorChar && path[num] != Path.AltDirectorySeparatorChar)
					{
					}
					return path.Substring(0, num);
				}
			}
			return null;
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x000B72DC File Offset: 0x000B62DC
		internal static int GetRootLength(string path)
		{
			Path.CheckInvalidPathChars(path);
			int i = 0;
			int length = path.Length;
			if (length >= 1 && Path.IsDirectorySeparator(path[0]))
			{
				i = 1;
				if (length >= 2 && Path.IsDirectorySeparator(path[1]))
				{
					i = 2;
					int num = 2;
					while (i < length)
					{
						if ((path[i] == Path.DirectorySeparatorChar || path[i] == Path.AltDirectorySeparatorChar) && --num <= 0)
						{
							break;
						}
						i++;
					}
				}
			}
			else if (length >= 2 && path[1] == Path.VolumeSeparatorChar)
			{
				i = 2;
				if (length >= 3 && Path.IsDirectorySeparator(path[2]))
				{
					i++;
				}
			}
			return i;
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x000B737D File Offset: 0x000B637D
		internal static bool IsDirectorySeparator(char c)
		{
			return c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar;
		}

		// Token: 0x0600363E RID: 13886 RVA: 0x000B7391 File Offset: 0x000B6391
		public static char[] GetInvalidPathChars()
		{
			return (char[])Path.RealInvalidPathChars.Clone();
		}

		// Token: 0x0600363F RID: 13887 RVA: 0x000B73A2 File Offset: 0x000B63A2
		public static char[] GetInvalidFileNameChars()
		{
			return (char[])Path.InvalidFileNameChars.Clone();
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x000B73B4 File Offset: 0x000B63B4
		public static string GetExtension(string path)
		{
			if (path == null)
			{
				return null;
			}
			Path.CheckInvalidPathChars(path);
			int length = path.Length;
			int num = length;
			while (--num >= 0)
			{
				char c = path[num];
				if (c == '.')
				{
					if (num != length - 1)
					{
						return path.Substring(num, length - num);
					}
					return string.Empty;
				}
				else if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar || c == Path.VolumeSeparatorChar)
				{
					break;
				}
			}
			return string.Empty;
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x000B7420 File Offset: 0x000B6420
		public static string GetFullPath(string path)
		{
			string fullPathInternal = Path.GetFullPathInternal(path);
			new FileIOPermission(FileIOPermissionAccess.PathDiscovery, new string[] { fullPathInternal }, false, false).Demand();
			return fullPathInternal;
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x000B7450 File Offset: 0x000B6450
		internal static string GetFullPathInternal(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			return Path.NormalizePath(path, true);
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x000B7474 File Offset: 0x000B6474
		internal static string NormalizePath(string path, bool fullCheck)
		{
			if (Environment.nativeIsWin9x())
			{
				return Path.NormalizePathSlow(path, fullCheck);
			}
			return Path.NormalizePathFast(path, fullCheck);
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x000B748C File Offset: 0x000B648C
		internal static string NormalizePathSlow(string path, bool fullCheck)
		{
			if (fullCheck)
			{
				path = path.TrimEnd(new char[0]);
				Path.CheckInvalidPathChars(path);
			}
			int i = 0;
			char[] array = new char[Path.MaxPath];
			int num = 0;
			uint num2 = 0U;
			uint num3 = 0U;
			bool flag = false;
			uint num4 = 0U;
			int num5 = -1;
			bool flag2 = false;
			bool flag3 = true;
			bool flag4 = false;
			if (path.Length > 0 && (path[0] == Path.DirectorySeparatorChar || path[0] == Path.AltDirectorySeparatorChar))
			{
				array[num++] = '\\';
				i++;
				num5 = 0;
			}
			while (i < path.Length)
			{
				char c = path[i];
				if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar)
				{
					if (num4 == 0U)
					{
						if (num3 > 0U)
						{
							int num6 = num5 + 1;
							if (path[num6] != '.')
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
							}
							if (num3 >= 2U)
							{
								if (flag2 && num3 > 2U)
								{
									throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
								}
								if (path[num6 + 1] == '.')
								{
									int num7 = num6 + 2;
									while ((long)num7 < (long)num6 + (long)((ulong)num3))
									{
										if (path[num7] != '.')
										{
											throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
										}
										num7++;
									}
									num3 = 2U;
								}
								else
								{
									if (num3 > 1U)
									{
										throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
									}
									num3 = 1U;
								}
							}
							if ((long)num + (long)((ulong)num3) + 1L >= (long)Path.MaxPath)
							{
								throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
							}
							if (num3 == 2U)
							{
								array[num++] = '.';
							}
							array[num++] = '.';
							flag = false;
						}
						if (num2 > 0U && flag3 && i + 1 < path.Length && (path[i + 1] == Path.DirectorySeparatorChar || path[i + 1] == Path.AltDirectorySeparatorChar))
						{
							array[num++] = Path.DirectorySeparatorChar;
						}
					}
					num3 = 0U;
					num2 = 0U;
					if (!flag)
					{
						flag = true;
						if (num + 1 >= Path.MaxPath)
						{
							throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
						}
						array[num++] = Path.DirectorySeparatorChar;
					}
					num4 = 0U;
					num5 = i;
					flag2 = false;
					flag3 = false;
					if (flag4)
					{
						array[num] = '\0';
						Path.TryExpandShortFileName(array, ref num, 260);
						flag4 = false;
					}
				}
				else if (c == '.')
				{
					num3 += 1U;
				}
				else if (c == ' ')
				{
					num2 += 1U;
				}
				else
				{
					if (c == '~')
					{
						flag4 = true;
					}
					flag = false;
					if (flag3 && c == Path.VolumeSeparatorChar)
					{
						char c2 = ((i > 0) ? path[i - 1] : ' ');
						if (num3 != 0U || num4 < 1U || c2 == ' ')
						{
							throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
						}
						flag2 = true;
						if (num4 > 1U)
						{
							uint num8 = 0U;
							while ((ulong)num8 < (ulong)((long)num) && array[(int)((UIntPtr)num8)] == ' ')
							{
								num8 += 1U;
							}
							if (num4 - num8 == 1U)
							{
								array[0] = c2;
								num = 1;
							}
						}
						num4 = 0U;
					}
					else
					{
						num4 += 1U + num3 + num2;
					}
					if (num3 > 0U || num2 > 0U)
					{
						int num9 = ((num5 >= 0) ? (i - num5 - 1) : i);
						if (num9 > 0)
						{
							path.CopyTo(num5 + 1, array, num, num9);
							num += num9;
						}
						num3 = 0U;
						num2 = 0U;
					}
					if (num + 1 >= Path.MaxPath)
					{
						throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
					}
					array[num++] = c;
					num5 = i;
				}
				i++;
			}
			if (num4 == 0U && num3 > 0U)
			{
				int num10 = num5 + 1;
				if (path[num10] != '.')
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
				}
				if (num3 >= 2U)
				{
					if (flag2 && num3 > 2U)
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
					}
					if (path[num10 + 1] == '.')
					{
						int num11 = num10 + 2;
						while ((long)num11 < (long)num10 + (long)((ulong)num3))
						{
							if (path[num11] != '.')
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
							}
							num11++;
						}
						num3 = 2U;
					}
					else
					{
						if (num3 > 1U)
						{
							throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
						}
						num3 = 1U;
					}
				}
				if ((long)num + (long)((ulong)num3) >= (long)Path.MaxPath)
				{
					throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
				}
				if (num3 == 2U)
				{
					array[num++] = '.';
				}
				array[num++] = '.';
			}
			if (num == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
			}
			array[num] = '\0';
			if (fullCheck && (Path.CharArrayStartsWithOrdinal(array, num, "http:", false) || Path.CharArrayStartsWithOrdinal(array, num, "file:", false)))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_PathUriFormatNotSupported"));
			}
			if (flag4)
			{
				Path.TryExpandShortFileName(array, ref num, Path.MaxPath);
			}
			int num12 = 1;
			char[] array3;
			int num13;
			if (fullCheck)
			{
				char[] array2 = new char[Path.MaxPath + 1];
				num12 = Win32Native.GetFullPathName(array, Path.MaxPath + 1, array2, IntPtr.Zero);
				if (num12 > Path.MaxPath)
				{
					array2 = new char[num12];
					num12 = Win32Native.GetFullPathName(array, num12, array2, IntPtr.Zero);
					if (num12 > Path.MaxPath)
					{
						throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
					}
				}
				if (num12 == 0 && array[0] != '\0')
				{
					__Error.WinIOError();
				}
				else if (num12 < Path.MaxPath)
				{
					array2[num12] = '\0';
				}
				if (Environment.nativeIsWin9x())
				{
					for (int j = 0; j < 260; j++)
					{
						if (array2[j] == '\0')
						{
							num12 = j;
							break;
						}
					}
				}
				array3 = array2;
				num13 = num12;
				flag4 = false;
				uint num14 = 0U;
				while ((ulong)num14 < (ulong)((long)num13) && !flag4)
				{
					if (array2[(int)((UIntPtr)num14)] == '~')
					{
						flag4 = true;
					}
					num14 += 1U;
				}
				if (flag4 && !Path.TryExpandShortFileName(array2, ref num13, Path.MaxPath))
				{
					int num15 = Array.LastIndexOf<char>(array2, Path.DirectorySeparatorChar, num13 - 1, num13);
					if (num15 >= 0)
					{
						char[] array4 = new char[num13 - num15 - 1];
						Array.Copy(array2, num15 + 1, array4, 0, num13 - num15 - 1);
						array2[num15] = '\0';
						bool flag5 = Path.TryExpandShortFileName(array2, ref num15, Path.MaxPath);
						array2[num15] = Path.DirectorySeparatorChar;
						Array.Copy(array4, 0, array2, num15 + 1, array4.Length);
						if (flag5)
						{
							num13 = num15 + 1 + array4.Length;
						}
					}
				}
			}
			else
			{
				array3 = array;
				num13 = num;
			}
			if (num12 != 0 && array3[0] == '\\' && array3[1] == '\\')
			{
				int k;
				for (k = 2; k < num12; k++)
				{
					if (array3[k] == '\\')
					{
						k++;
						break;
					}
				}
				if (k == num12)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegalUNC"));
				}
				if (Path.CharArrayStartsWithOrdinal(array3, num13, "\\\\?\\globalroot", true))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_PathGlobalRoot"));
				}
			}
			if (num13 >= Path.MaxPath)
			{
				throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
			}
			if (num12 == 0)
			{
				int num16 = Marshal.GetLastWin32Error();
				if (num16 == 0)
				{
					num16 = 161;
				}
				__Error.WinIOError(num16, path);
				return null;
			}
			return new string(array3, 0, num13);
		}

		// Token: 0x06003645 RID: 13893 RVA: 0x000B7B4C File Offset: 0x000B6B4C
		private static bool CharArrayStartsWithOrdinal(char[] array, int numChars, string compareTo, bool ignoreCase)
		{
			if (numChars < compareTo.Length)
			{
				return false;
			}
			if (ignoreCase)
			{
				string text = new string(array, 0, compareTo.Length);
				return compareTo.Equals(text, StringComparison.OrdinalIgnoreCase);
			}
			for (int i = 0; i < compareTo.Length; i++)
			{
				if (array[i] != compareTo[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x000B7BA0 File Offset: 0x000B6BA0
		private static bool TryExpandShortFileName(char[] buffer, ref int bufferLength, int maxBufferSize)
		{
			char[] array = new char[Path.MaxPath + 1];
			int num = Win32Native.GetLongPathName(buffer, array, Path.MaxPath);
			if (num >= Path.MaxPath)
			{
				throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
			}
			if (num == 0)
			{
				return false;
			}
			if (Environment.nativeIsWin9x())
			{
				for (int i = 0; i < 260; i++)
				{
					if (array[i] == '\0')
					{
						num = i;
						break;
					}
				}
			}
			Buffer.BlockCopy(array, 0, buffer, 0, 2 * num);
			bufferLength = num;
			buffer[bufferLength] = '\0';
			return true;
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x000B7C19 File Offset: 0x000B6C19
		private unsafe static void SafeSetStackPointerValue(char* buffer, int index, char value)
		{
			if (index >= Path.MaxPath)
			{
				throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
			}
			buffer[index] = value;
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x000B7C3C File Offset: 0x000B6C3C
		internal unsafe static string NormalizePathFast(string path, bool fullCheck)
		{
			if (fullCheck)
			{
				path = path.TrimEnd(new char[0]);
				Path.CheckInvalidPathChars(path);
			}
			int i = 0;
			char* ptr = stackalloc char[2 * Path.MaxPath];
			int num = 0;
			uint num2 = 0U;
			uint num3 = 0U;
			bool flag = false;
			uint num4 = 0U;
			int num5 = -1;
			bool flag2 = false;
			bool flag3 = true;
			bool flag4 = false;
			if (path.Length > 0 && (path[0] == Path.DirectorySeparatorChar || path[0] == Path.AltDirectorySeparatorChar))
			{
				Path.SafeSetStackPointerValue(ptr, num++, '\\');
				i++;
				num5 = 0;
			}
			while (i < path.Length)
			{
				char c = path[i];
				if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar)
				{
					if (num4 == 0U)
					{
						if (num3 > 0U)
						{
							int num6 = num5 + 1;
							if (path[num6] != '.')
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
							}
							if (num3 >= 2U)
							{
								if (flag2 && num3 > 2U)
								{
									throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
								}
								if (path[num6 + 1] == '.')
								{
									int num7 = num6 + 2;
									while ((long)num7 < (long)num6 + (long)((ulong)num3))
									{
										if (path[num7] != '.')
										{
											throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
										}
										num7++;
									}
									num3 = 2U;
								}
								else
								{
									if (num3 > 1U)
									{
										throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
									}
									num3 = 1U;
								}
							}
							if (num3 == 2U)
							{
								Path.SafeSetStackPointerValue(ptr, num++, '.');
							}
							Path.SafeSetStackPointerValue(ptr, num++, '.');
							flag = false;
						}
						if (num2 > 0U && flag3 && i + 1 < path.Length && (path[i + 1] == Path.DirectorySeparatorChar || path[i + 1] == Path.AltDirectorySeparatorChar))
						{
							Path.SafeSetStackPointerValue(ptr, num++, Path.DirectorySeparatorChar);
						}
					}
					num3 = 0U;
					num2 = 0U;
					if (!flag)
					{
						flag = true;
						Path.SafeSetStackPointerValue(ptr, num++, Path.DirectorySeparatorChar);
					}
					num4 = 0U;
					num5 = i;
					flag2 = false;
					flag3 = false;
					if (flag4)
					{
						Path.SafeSetStackPointerValue(ptr, num, '\0');
						Path.TryExpandShortFileName(ptr, ref num, 260);
						flag4 = false;
					}
				}
				else if (c == '.')
				{
					num3 += 1U;
				}
				else if (c == ' ')
				{
					num2 += 1U;
				}
				else
				{
					if (c == '~')
					{
						flag4 = true;
					}
					flag = false;
					if (flag3 && c == Path.VolumeSeparatorChar)
					{
						char c2 = ((i > 0) ? path[i - 1] : ' ');
						if (num3 != 0U || num4 < 1U || c2 == ' ')
						{
							throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
						}
						flag2 = true;
						if (num4 > 1U)
						{
							uint num8 = 0U;
							while ((ulong)num8 < (ulong)((long)num) && ptr[num8] == ' ')
							{
								num8 += 1U;
							}
							if (num4 - num8 == 1U)
							{
								*ptr = c2;
								num = 1;
							}
						}
						num4 = 0U;
					}
					else
					{
						num4 += 1U + num3 + num2;
					}
					if (num3 > 0U || num2 > 0U)
					{
						int num9 = ((num5 >= 0) ? (i - num5 - 1) : i);
						if (num9 > 0)
						{
							for (int j = 0; j < num9; j++)
							{
								Path.SafeSetStackPointerValue(ptr, num++, path[num5 + 1 + j]);
							}
						}
						num3 = 0U;
						num2 = 0U;
					}
					Path.SafeSetStackPointerValue(ptr, num++, c);
					num5 = i;
				}
				i++;
			}
			if (num4 == 0U && num3 > 0U)
			{
				int num10 = num5 + 1;
				if (path[num10] != '.')
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
				}
				if (num3 >= 2U)
				{
					if (flag2 && num3 > 2U)
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
					}
					if (path[num10 + 1] == '.')
					{
						int num11 = num10 + 2;
						while ((long)num11 < (long)num10 + (long)((ulong)num3))
						{
							if (path[num11] != '.')
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
							}
							num11++;
						}
						num3 = 2U;
					}
					else
					{
						if (num3 > 1U)
						{
							throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
						}
						num3 = 1U;
					}
				}
				if (num3 == 2U)
				{
					Path.SafeSetStackPointerValue(ptr, num++, '.');
				}
				Path.SafeSetStackPointerValue(ptr, num++, '.');
			}
			if (num == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
			}
			Path.SafeSetStackPointerValue(ptr, num, '\0');
			if (fullCheck && (Path.CharArrayStartsWithOrdinal(ptr, num, "http:", false) || Path.CharArrayStartsWithOrdinal(ptr, num, "file:", false)))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_PathUriFormatNotSupported"));
			}
			if (flag4)
			{
				Path.TryExpandShortFileName(ptr, ref num, Path.MaxPath);
			}
			int num12 = 1;
			char* ptr4;
			int num13;
			if (fullCheck)
			{
				char* ptr2 = stackalloc char[2 * (Path.MaxPath + 1)];
				num12 = Win32Native.GetFullPathName(ptr, Path.MaxPath + 1, ptr2, IntPtr.Zero);
				if (num12 > Path.MaxPath)
				{
					char* ptr3 = stackalloc char[2 * num12];
					ptr2 = ptr3;
					num12 = Win32Native.GetFullPathName(ptr, num12, ptr2, IntPtr.Zero);
				}
				if (num12 >= Path.MaxPath)
				{
					throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
				}
				if (num12 == 0 && *ptr != '\0')
				{
					__Error.WinIOError();
				}
				else if (num12 < Path.MaxPath)
				{
					ptr2[num12] = '\0';
				}
				ptr4 = ptr2;
				num13 = num12;
				flag4 = false;
				uint num14 = 0U;
				while ((ulong)num14 < (ulong)((long)num13) && !flag4)
				{
					if (ptr2[num14] == '~')
					{
						flag4 = true;
					}
					num14 += 1U;
				}
				if (flag4 && !Path.TryExpandShortFileName(ptr2, ref num13, Path.MaxPath))
				{
					int num15 = -1;
					for (int k = num13 - 1; k >= 0; k--)
					{
						if (ptr2[k] == Path.DirectorySeparatorChar)
						{
							num15 = k;
							break;
						}
					}
					if (num15 >= 0)
					{
						if (num13 >= Path.MaxPath)
						{
							throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
						}
						int num16 = num13 - num15 - 1;
						char* ptr5 = stackalloc char[2 * num16];
						Buffer.memcpy(ptr2, num15 + 1, ptr5, 0, num16);
						Path.SafeSetStackPointerValue(ptr2, num15, '\0');
						bool flag5 = Path.TryExpandShortFileName(ptr2, ref num15, Path.MaxPath);
						Path.SafeSetStackPointerValue(ptr2, num15, Path.DirectorySeparatorChar);
						if (num15 + 1 + num16 >= Path.MaxPath)
						{
							throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
						}
						Buffer.memcpy(ptr5, 0, ptr2, num15 + 1, num16);
						if (flag5)
						{
							num13 = num15 + 1 + num16;
						}
					}
				}
			}
			else
			{
				ptr4 = ptr;
				num13 = num;
			}
			if (num12 != 0 && *ptr4 == '\\' && ptr4[1] == '\\')
			{
				int l;
				for (l = 2; l < num12; l++)
				{
					if (ptr4[l] == '\\')
					{
						l++;
						break;
					}
				}
				if (l == num12)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegalUNC"));
				}
				if (Path.CharArrayStartsWithOrdinal(ptr4, num13, "\\\\?\\globalroot", true))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_PathGlobalRoot"));
				}
			}
			if (num13 >= Path.MaxPath)
			{
				throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
			}
			if (num12 == 0)
			{
				int num17 = Marshal.GetLastWin32Error();
				if (num17 == 0)
				{
					num17 = 161;
				}
				__Error.WinIOError(num17, path);
				return null;
			}
			return new string(ptr4, 0, num13);
		}

		// Token: 0x06003649 RID: 13897 RVA: 0x000B830C File Offset: 0x000B730C
		private unsafe static bool CharArrayStartsWithOrdinal(char* array, int numChars, string compareTo, bool ignoreCase)
		{
			if (numChars < compareTo.Length)
			{
				return false;
			}
			if (ignoreCase)
			{
				string text = new string(array, 0, compareTo.Length);
				return compareTo.Equals(text, StringComparison.OrdinalIgnoreCase);
			}
			for (int i = 0; i < compareTo.Length; i++)
			{
				if (array[i] != compareTo[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600364A RID: 13898 RVA: 0x000B8364 File Offset: 0x000B7364
		private unsafe static bool TryExpandShortFileName(char* buffer, ref int bufferLength, int maxBufferSize)
		{
			char* ptr = stackalloc char[2 * (Path.MaxPath + 1)];
			int longPathName = Win32Native.GetLongPathName(buffer, ptr, Path.MaxPath);
			if (longPathName >= Path.MaxPath)
			{
				throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
			}
			if (longPathName == 0)
			{
				return false;
			}
			Buffer.memcpy(ptr, 0, buffer, 0, longPathName);
			bufferLength = longPathName;
			buffer[bufferLength] = '\0';
			return true;
		}

		// Token: 0x0600364B RID: 13899 RVA: 0x000B83C0 File Offset: 0x000B73C0
		internal static string FixupPath(string path)
		{
			return Path.NormalizePath(path, false);
		}

		// Token: 0x0600364C RID: 13900 RVA: 0x000B83D8 File Offset: 0x000B73D8
		public static string GetFileName(string path)
		{
			if (path != null)
			{
				Path.CheckInvalidPathChars(path);
				int length = path.Length;
				int num = length;
				while (--num >= 0)
				{
					char c = path[num];
					if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar || c == Path.VolumeSeparatorChar)
					{
						return path.Substring(num + 1, length - num - 1);
					}
				}
			}
			return path;
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x000B8434 File Offset: 0x000B7434
		public static string GetFileNameWithoutExtension(string path)
		{
			path = Path.GetFileName(path);
			if (path == null)
			{
				return null;
			}
			int num;
			if ((num = path.LastIndexOf('.')) == -1)
			{
				return path;
			}
			return path.Substring(0, num);
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x000B8465 File Offset: 0x000B7465
		public static string GetPathRoot(string path)
		{
			if (path == null)
			{
				return null;
			}
			path = Path.FixupPath(path);
			return path.Substring(0, Path.GetRootLength(path));
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x000B8484 File Offset: 0x000B7484
		public static string GetTempPath()
		{
			new EnvironmentPermission(PermissionState.Unrestricted).Demand();
			StringBuilder stringBuilder = new StringBuilder(260);
			int num = Path.GetTempPathVersion;
			if (num == 0)
			{
				num = 1;
				string environmentVariable = Environment.GetEnvironmentVariable("COMPlus_Disable_GetTempPath2");
				if (!string.Equals(environmentVariable, "true", StringComparison.Ordinal) && Win32Native.DoesWin32MethodExist("kernel32.dll", "GetTempPath2W") && Win32Native.DoesWin32MethodExist("kernel32.dll", "GetTempPath2A"))
				{
					num = 2;
				}
				Path.GetTempPathVersion = num;
			}
			uint num2 = ((num == 2) ? Win32Native.GetTempPath2(260, stringBuilder) : Win32Native.GetTempPath(260, stringBuilder));
			string text = stringBuilder.ToString();
			if (num2 == 0U)
			{
				__Error.WinIOError();
			}
			return Path.GetFullPathInternal(text);
		}

		// Token: 0x06003650 RID: 13904 RVA: 0x000B8530 File Offset: 0x000B7530
		public static string GetRandomFileName()
		{
			byte[] array = new byte[10];
			RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider();
			rngcryptoServiceProvider.GetBytes(array);
			char[] array2 = IsolatedStorage.ToBase32StringSuitableForDirName(array).ToCharArray();
			array2[8] = '.';
			return new string(array2, 0, 12);
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x000B856C File Offset: 0x000B756C
		public static string GetTempFileName()
		{
			string tempPath = Path.GetTempPath();
			new FileIOPermission(FileIOPermissionAccess.Write, tempPath).Demand();
			StringBuilder stringBuilder = new StringBuilder(260);
			if (Win32Native.GetTempFileName(tempPath, "tmp", 0U, stringBuilder) == 0U)
			{
				__Error.WinIOError();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x000B85B4 File Offset: 0x000B75B4
		public static bool HasExtension(string path)
		{
			if (path != null)
			{
				Path.CheckInvalidPathChars(path);
				int num = path.Length;
				while (--num >= 0)
				{
					char c = path[num];
					if (c == '.')
					{
						return num != path.Length - 1;
					}
					if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar || c == Path.VolumeSeparatorChar)
					{
						break;
					}
				}
			}
			return false;
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x000B8610 File Offset: 0x000B7610
		public static bool IsPathRooted(string path)
		{
			if (path != null)
			{
				Path.CheckInvalidPathChars(path);
				int length = path.Length;
				if ((length >= 1 && (path[0] == Path.DirectorySeparatorChar || path[0] == Path.AltDirectorySeparatorChar)) || (length >= 2 && path[1] == Path.VolumeSeparatorChar))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003654 RID: 13908 RVA: 0x000B8664 File Offset: 0x000B7664
		public static string Combine(string path1, string path2)
		{
			if (path1 == null || path2 == null)
			{
				throw new ArgumentNullException((path1 == null) ? "path1" : "path2");
			}
			Path.CheckInvalidPathChars(path1);
			Path.CheckInvalidPathChars(path2);
			if (path2.Length == 0)
			{
				return path1;
			}
			if (path1.Length == 0)
			{
				return path2;
			}
			if (Path.IsPathRooted(path2))
			{
				return path2;
			}
			char c = path1[path1.Length - 1];
			if (c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar && c != Path.VolumeSeparatorChar)
			{
				return path1 + Path.DirectorySeparatorChar + path2;
			}
			return path1 + path2;
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x000B86F8 File Offset: 0x000B76F8
		internal static void CheckSearchPattern(string searchPattern)
		{
			if ((Environment.OSInfo & Environment.OSName.Win9x) != Environment.OSName.Invalid && Path.CanPathCircumventSecurityNative(searchPattern))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidSearchPattern"));
			}
			int num;
			while ((num = searchPattern.IndexOf("..", StringComparison.Ordinal)) != -1)
			{
				if (num + 2 == searchPattern.Length)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidSearchPattern"));
				}
				if (searchPattern[num + 2] == Path.DirectorySeparatorChar || searchPattern[num + 2] == Path.AltDirectorySeparatorChar)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidSearchPattern"));
				}
				searchPattern = searchPattern.Substring(num + 2);
			}
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x000B8790 File Offset: 0x000B7790
		internal static void CheckInvalidPathChars(string path)
		{
			foreach (int num in path)
			{
				if (num == 34 || num == 60 || num == 62 || num == 124 || num < 32)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidPathChars"));
				}
			}
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x000B87E0 File Offset: 0x000B77E0
		internal static string InternalCombine(string path1, string path2)
		{
			if (path1 == null || path2 == null)
			{
				throw new ArgumentNullException((path1 == null) ? "path1" : "path2");
			}
			Path.CheckInvalidPathChars(path1);
			Path.CheckInvalidPathChars(path2);
			if (path2.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_PathEmpty"), "path2");
			}
			if (Path.IsPathRooted(path2))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_Path2IsRooted"), "path2");
			}
			int length = path1.Length;
			if (length == 0)
			{
				return path2;
			}
			char c = path1[length - 1];
			if (c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar && c != Path.VolumeSeparatorChar)
			{
				return path1 + Path.DirectorySeparatorChar + path2;
			}
			return path1 + path2;
		}

		// Token: 0x06003658 RID: 13912
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CanPathCircumventSecurityNative(string partOfPath);

		// Token: 0x04001C44 RID: 7236
		internal const int MAX_PATH = 260;

		// Token: 0x04001C45 RID: 7237
		internal const int MAX_DIRECTORY_PATH = 248;

		// Token: 0x04001C46 RID: 7238
		public static readonly char DirectorySeparatorChar = '\\';

		// Token: 0x04001C47 RID: 7239
		public static readonly char AltDirectorySeparatorChar = '/';

		// Token: 0x04001C48 RID: 7240
		public static readonly char VolumeSeparatorChar = ':';

		// Token: 0x04001C49 RID: 7241
		[Obsolete("Please use GetInvalidPathChars or GetInvalidFileNameChars instead.")]
		public static readonly char[] InvalidPathChars = new char[]
		{
			'"', '<', '>', '|', '\0', '\u0001', '\u0002', '\u0003', '\u0004', '\u0005',
			'\u0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\u000e', '\u000f',
			'\u0010', '\u0011', '\u0012', '\u0013', '\u0014', '\u0015', '\u0016', '\u0017', '\u0018', '\u0019',
			'\u001a', '\u001b', '\u001c', '\u001d', '\u001e', '\u001f'
		};

		// Token: 0x04001C4A RID: 7242
		private static readonly char[] RealInvalidPathChars = new char[]
		{
			'"', '<', '>', '|', '\0', '\u0001', '\u0002', '\u0003', '\u0004', '\u0005',
			'\u0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\u000e', '\u000f',
			'\u0010', '\u0011', '\u0012', '\u0013', '\u0014', '\u0015', '\u0016', '\u0017', '\u0018', '\u0019',
			'\u001a', '\u001b', '\u001c', '\u001d', '\u001e', '\u001f'
		};

		// Token: 0x04001C4B RID: 7243
		private static readonly char[] InvalidFileNameChars = new char[]
		{
			'"', '<', '>', '|', '\0', '\u0001', '\u0002', '\u0003', '\u0004', '\u0005',
			'\u0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\u000e', '\u000f',
			'\u0010', '\u0011', '\u0012', '\u0013', '\u0014', '\u0015', '\u0016', '\u0017', '\u0018', '\u0019',
			'\u001a', '\u001b', '\u001c', '\u001d', '\u001e', '\u001f', ':', '*', '?', '\\',
			'/'
		};

		// Token: 0x04001C4C RID: 7244
		public static readonly char PathSeparator = ';';

		// Token: 0x04001C4D RID: 7245
		internal static readonly int MaxPath = 260;

		// Token: 0x04001C4E RID: 7246
		private static int GetTempPathVersion = 0;
	}
}
