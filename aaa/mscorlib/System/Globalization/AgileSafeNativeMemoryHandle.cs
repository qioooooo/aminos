using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Globalization
{
	// Token: 0x020003D3 RID: 979
	internal sealed class AgileSafeNativeMemoryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060028AE RID: 10414 RVA: 0x0007EB64 File Offset: 0x0007DB64
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal AgileSafeNativeMemoryHandle()
			: base(true)
		{
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x0007EB6D File Offset: 0x0007DB6D
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal AgileSafeNativeMemoryHandle(IntPtr handle, bool ownsHandle)
			: base(ownsHandle)
		{
			base.SetHandle(handle);
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x0007EB7D File Offset: 0x0007DB7D
		internal AgileSafeNativeMemoryHandle(string fileName)
			: this(fileName, null)
		{
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x0007EB88 File Offset: 0x0007DB88
		internal unsafe AgileSafeNativeMemoryHandle(string fileName, string fileMappingName)
			: base(true)
		{
			this.mode = true;
			SafeFileHandle safeFileHandle = Win32Native.UnsafeCreateFile(fileName, int.MinValue, FileShare.Read, null, FileMode.Open, 0, IntPtr.Zero);
			int num = Marshal.GetLastWin32Error();
			if (safeFileHandle.IsInvalid)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_UnexpectedWin32Error"), new object[] { num }));
			}
			int num3;
			int num2 = Win32Native.GetFileSize(safeFileHandle, out num3);
			if (num2 == -1)
			{
				safeFileHandle.Close();
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_UnexpectedWin32Error"), new object[] { num }));
			}
			this.fileSize = ((long)num3 << 32) | (long)((ulong)num2);
			if (this.fileSize == 0L)
			{
				safeFileHandle.Close();
				return;
			}
			SafeFileMappingHandle safeFileMappingHandle = Win32Native.CreateFileMapping(safeFileHandle, IntPtr.Zero, 2U, 0U, 0U, fileMappingName);
			num = Marshal.GetLastWin32Error();
			safeFileHandle.Close();
			if (safeFileMappingHandle.IsInvalid)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_UnexpectedWin32Error"), new object[] { num }));
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				this.handle = Win32Native.MapViewOfFile(safeFileMappingHandle, 4U, 0U, 0U, UIntPtr.Zero);
			}
			num = Marshal.GetLastWin32Error();
			if (this.handle == IntPtr.Zero)
			{
				safeFileMappingHandle.Close();
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_UnexpectedWin32Error"), new object[] { num }));
			}
			this.bytes = (byte*)(void*)base.DangerousGetHandle();
			safeFileMappingHandle.Close();
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x060028B2 RID: 10418 RVA: 0x0007ED3C File Offset: 0x0007DD3C
		internal long FileSize
		{
			get
			{
				return this.fileSize;
			}
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x0007ED44 File Offset: 0x0007DD44
		internal unsafe byte* GetBytePtr()
		{
			return this.bytes;
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x0007ED4C File Offset: 0x0007DD4C
		protected override bool ReleaseHandle()
		{
			if (!this.IsInvalid)
			{
				if (!this.mode)
				{
					Marshal.FreeHGlobal(this.handle);
					this.handle = IntPtr.Zero;
					return true;
				}
				if (Win32Native.UnmapViewOfFile(this.handle))
				{
					this.handle = IntPtr.Zero;
					return true;
				}
			}
			return false;
		}

		// Token: 0x040013A2 RID: 5026
		private const int PAGE_READONLY = 2;

		// Token: 0x040013A3 RID: 5027
		private const int SECTION_MAP_READ = 4;

		// Token: 0x040013A4 RID: 5028
		private unsafe byte* bytes;

		// Token: 0x040013A5 RID: 5029
		private long fileSize;

		// Token: 0x040013A6 RID: 5030
		private bool mode;
	}
}
