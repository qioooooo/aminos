using System;
using System.Internal;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Drawing
{
	// Token: 0x02000069 RID: 105
	[SuppressUnmanagedCodeSecurity]
	internal class UnsafeNativeMethods
	{
		// Token: 0x060006C8 RID: 1736
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "RtlMoveMemory", ExactSpelling = true, SetLastError = true)]
		public static extern void CopyMemory(HandleRef destData, HandleRef srcData, int size);

		// Token: 0x060006C9 RID: 1737
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetDC", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntGetDC(HandleRef hWnd);

		// Token: 0x060006CA RID: 1738 RVA: 0x0001AF54 File Offset: 0x00019F54
		public static IntPtr GetDC(HandleRef hWnd)
		{
			return global::System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntGetDC(hWnd), SafeNativeMethods.CommonHandles.HDC);
		}

		// Token: 0x060006CB RID: 1739
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteDC", ExactSpelling = true, SetLastError = true)]
		private static extern bool IntDeleteDC(HandleRef hDC);

		// Token: 0x060006CC RID: 1740 RVA: 0x0001AF66 File Offset: 0x00019F66
		public static bool DeleteDC(HandleRef hDC)
		{
			global::System.Internal.HandleCollector.Remove((IntPtr)hDC, SafeNativeMethods.CommonHandles.GDI);
			return UnsafeNativeMethods.IntDeleteDC(hDC);
		}

		// Token: 0x060006CD RID: 1741
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "ReleaseDC", ExactSpelling = true, SetLastError = true)]
		private static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

		// Token: 0x060006CE RID: 1742 RVA: 0x0001AF7F File Offset: 0x00019F7F
		public static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
		{
			global::System.Internal.HandleCollector.Remove((IntPtr)hDC, SafeNativeMethods.CommonHandles.HDC);
			return UnsafeNativeMethods.IntReleaseDC(hWnd, hDC);
		}

		// Token: 0x060006CF RID: 1743
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateCompatibleDC", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateCompatibleDC(HandleRef hDC);

		// Token: 0x060006D0 RID: 1744 RVA: 0x0001AF99 File Offset: 0x00019F99
		public static IntPtr CreateCompatibleDC(HandleRef hDC)
		{
			return global::System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntCreateCompatibleDC(hDC), SafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x060006D1 RID: 1745
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetStockObject(int nIndex);

		// Token: 0x060006D2 RID: 1746
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetSystemDefaultLCID();

		// Token: 0x060006D3 RID: 1747
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetSystemMetrics(int nIndex);

		// Token: 0x060006D4 RID: 1748
		[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SystemParametersInfo(int uiAction, int uiParam, [In] [Out] NativeMethods.NONCLIENTMETRICS pvParam, int fWinIni);

		// Token: 0x060006D5 RID: 1749
		[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SystemParametersInfo(int uiAction, int uiParam, [In] [Out] SafeNativeMethods.LOGFONT pvParam, int fWinIni);

		// Token: 0x060006D6 RID: 1750
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetDeviceCaps(HandleRef hDC, int nIndex);

		// Token: 0x060006D7 RID: 1751
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetObjectType(HandleRef hObject);

		// Token: 0x060006D8 RID: 1752 RVA: 0x0001AFAB File Offset: 0x00019FAB
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
		public static object PtrToStructure(IntPtr lparam, Type cls)
		{
			return Marshal.PtrToStructure(lparam, cls);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0001AFB4 File Offset: 0x00019FB4
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
		public static void PtrToStructure(IntPtr lparam, object data)
		{
			Marshal.PtrToStructure(lparam, data);
		}

		// Token: 0x0200006A RID: 106
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0000000C-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IStream
		{
			// Token: 0x060006DB RID: 1755
			int Read([In] IntPtr buf, [In] int len);

			// Token: 0x060006DC RID: 1756
			int Write([In] IntPtr buf, [In] int len);

			// Token: 0x060006DD RID: 1757
			[return: MarshalAs(UnmanagedType.I8)]
			long Seek([MarshalAs(UnmanagedType.I8)] [In] long dlibMove, [In] int dwOrigin);

			// Token: 0x060006DE RID: 1758
			void SetSize([MarshalAs(UnmanagedType.I8)] [In] long libNewSize);

			// Token: 0x060006DF RID: 1759
			[return: MarshalAs(UnmanagedType.I8)]
			long CopyTo([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm, [MarshalAs(UnmanagedType.I8)] [In] long cb, [MarshalAs(UnmanagedType.LPArray)] [Out] long[] pcbRead);

			// Token: 0x060006E0 RID: 1760
			void Commit([In] int grfCommitFlags);

			// Token: 0x060006E1 RID: 1761
			void Revert();

			// Token: 0x060006E2 RID: 1762
			void LockRegion([MarshalAs(UnmanagedType.I8)] [In] long libOffset, [MarshalAs(UnmanagedType.I8)] [In] long cb, [In] int dwLockType);

			// Token: 0x060006E3 RID: 1763
			void UnlockRegion([MarshalAs(UnmanagedType.I8)] [In] long libOffset, [MarshalAs(UnmanagedType.I8)] [In] long cb, [In] int dwLockType);

			// Token: 0x060006E4 RID: 1764
			void Stat([In] IntPtr pStatstg, [In] int grfStatFlag);

			// Token: 0x060006E5 RID: 1765
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStream Clone();
		}

		// Token: 0x0200006B RID: 107
		internal class ComStreamFromDataStream : UnsafeNativeMethods.IStream
		{
			// Token: 0x060006E6 RID: 1766 RVA: 0x0001AFC5 File Offset: 0x00019FC5
			internal ComStreamFromDataStream(Stream dataStream)
			{
				if (dataStream == null)
				{
					throw new ArgumentNullException("dataStream");
				}
				this.dataStream = dataStream;
			}

			// Token: 0x060006E7 RID: 1767 RVA: 0x0001AFEC File Offset: 0x00019FEC
			private void ActualizeVirtualPosition()
			{
				if (this.virtualPosition == -1L)
				{
					return;
				}
				if (this.virtualPosition > this.dataStream.Length)
				{
					this.dataStream.SetLength(this.virtualPosition);
				}
				this.dataStream.Position = this.virtualPosition;
				this.virtualPosition = -1L;
			}

			// Token: 0x060006E8 RID: 1768 RVA: 0x0001B041 File Offset: 0x0001A041
			public virtual UnsafeNativeMethods.IStream Clone()
			{
				UnsafeNativeMethods.ComStreamFromDataStream.NotImplemented();
				return null;
			}

			// Token: 0x060006E9 RID: 1769 RVA: 0x0001B049 File Offset: 0x0001A049
			public virtual void Commit(int grfCommitFlags)
			{
				this.dataStream.Flush();
				this.ActualizeVirtualPosition();
			}

			// Token: 0x060006EA RID: 1770 RVA: 0x0001B05C File Offset: 0x0001A05C
			public virtual long CopyTo(UnsafeNativeMethods.IStream pstm, long cb, long[] pcbRead)
			{
				int num = 4096;
				IntPtr intPtr = Marshal.AllocHGlobal(num);
				if (intPtr == IntPtr.Zero)
				{
					throw new OutOfMemoryException();
				}
				long num2 = 0L;
				try
				{
					while (num2 < cb)
					{
						int num3 = num;
						if (num2 + (long)num3 > cb)
						{
							num3 = (int)(cb - num2);
						}
						int num4 = this.Read(intPtr, num3);
						if (num4 == 0)
						{
							break;
						}
						if (pstm.Write(intPtr, num4) != num4)
						{
							throw UnsafeNativeMethods.ComStreamFromDataStream.EFail("Wrote an incorrect number of bytes");
						}
						num2 += (long)num4;
					}
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
				if (pcbRead != null && pcbRead.Length > 0)
				{
					pcbRead[0] = num2;
				}
				return num2;
			}

			// Token: 0x060006EB RID: 1771 RVA: 0x0001B0F4 File Offset: 0x0001A0F4
			public virtual Stream GetDataStream()
			{
				return this.dataStream;
			}

			// Token: 0x060006EC RID: 1772 RVA: 0x0001B0FC File Offset: 0x0001A0FC
			public virtual void LockRegion(long libOffset, long cb, int dwLockType)
			{
			}

			// Token: 0x060006ED RID: 1773 RVA: 0x0001B0FE File Offset: 0x0001A0FE
			protected static ExternalException EFail(string msg)
			{
				throw new ExternalException(msg, -2147467259);
			}

			// Token: 0x060006EE RID: 1774 RVA: 0x0001B10B File Offset: 0x0001A10B
			protected static void NotImplemented()
			{
				throw new ExternalException(SR.GetString("NotImplemented"), -2147467263);
			}

			// Token: 0x060006EF RID: 1775 RVA: 0x0001B124 File Offset: 0x0001A124
			public virtual int Read(IntPtr buf, int length)
			{
				byte[] array = new byte[length];
				int num = this.Read(array, length);
				Marshal.Copy(array, 0, buf, length);
				return num;
			}

			// Token: 0x060006F0 RID: 1776 RVA: 0x0001B14B File Offset: 0x0001A14B
			public virtual int Read(byte[] buffer, int length)
			{
				this.ActualizeVirtualPosition();
				return this.dataStream.Read(buffer, 0, length);
			}

			// Token: 0x060006F1 RID: 1777 RVA: 0x0001B161 File Offset: 0x0001A161
			public virtual void Revert()
			{
				UnsafeNativeMethods.ComStreamFromDataStream.NotImplemented();
			}

			// Token: 0x060006F2 RID: 1778 RVA: 0x0001B168 File Offset: 0x0001A168
			public virtual long Seek(long offset, int origin)
			{
				long position = this.virtualPosition;
				if (this.virtualPosition == -1L)
				{
					position = this.dataStream.Position;
				}
				long length = this.dataStream.Length;
				switch (origin)
				{
				case 0:
					if (offset <= length)
					{
						this.dataStream.Position = offset;
						this.virtualPosition = -1L;
					}
					else
					{
						this.virtualPosition = offset;
					}
					break;
				case 1:
					if (offset + position <= length)
					{
						this.dataStream.Position = position + offset;
						this.virtualPosition = -1L;
					}
					else
					{
						this.virtualPosition = offset + position;
					}
					break;
				case 2:
					if (offset <= 0L)
					{
						this.dataStream.Position = length + offset;
						this.virtualPosition = -1L;
					}
					else
					{
						this.virtualPosition = length + offset;
					}
					break;
				}
				if (this.virtualPosition != -1L)
				{
					return this.virtualPosition;
				}
				return this.dataStream.Position;
			}

			// Token: 0x060006F3 RID: 1779 RVA: 0x0001B242 File Offset: 0x0001A242
			public virtual void SetSize(long value)
			{
				this.dataStream.SetLength(value);
			}

			// Token: 0x060006F4 RID: 1780 RVA: 0x0001B250 File Offset: 0x0001A250
			public virtual void Stat(IntPtr pstatstg, int grfStatFlag)
			{
				UnsafeNativeMethods.ComStreamFromDataStream.NotImplemented();
			}

			// Token: 0x060006F5 RID: 1781 RVA: 0x0001B257 File Offset: 0x0001A257
			public virtual void UnlockRegion(long libOffset, long cb, int dwLockType)
			{
			}

			// Token: 0x060006F6 RID: 1782 RVA: 0x0001B25C File Offset: 0x0001A25C
			public virtual int Write(IntPtr buf, int length)
			{
				byte[] array = new byte[length];
				Marshal.Copy(buf, array, 0, length);
				return this.Write(array, length);
			}

			// Token: 0x060006F7 RID: 1783 RVA: 0x0001B281 File Offset: 0x0001A281
			public virtual int Write(byte[] buffer, int length)
			{
				this.ActualizeVirtualPosition();
				this.dataStream.Write(buffer, 0, length);
				return length;
			}

			// Token: 0x04000492 RID: 1170
			protected Stream dataStream;

			// Token: 0x04000493 RID: 1171
			private long virtualPosition = -1L;
		}
	}
}
