using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Drawing.Internal
{
	// Token: 0x020000B2 RID: 178
	internal class GPStream : UnsafeNativeMethods.IStream
	{
		// Token: 0x06000ADE RID: 2782 RVA: 0x000202EC File Offset: 0x0001F2EC
		internal GPStream(Stream stream)
		{
			if (!stream.CanSeek)
			{
				byte[] array = new byte[256];
				int num = 0;
				int num2;
				do
				{
					if (array.Length < num + 256)
					{
						byte[] array2 = new byte[array.Length * 2];
						Array.Copy(array, array2, array.Length);
						array = array2;
					}
					num2 = stream.Read(array, num, 256);
					num += num2;
				}
				while (num2 != 0);
				this.dataStream = new MemoryStream(array);
				return;
			}
			this.dataStream = stream;
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x00020368 File Offset: 0x0001F368
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

		// Token: 0x06000AE0 RID: 2784 RVA: 0x000203BD File Offset: 0x0001F3BD
		public virtual UnsafeNativeMethods.IStream Clone()
		{
			GPStream.NotImplemented();
			return null;
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x000203C5 File Offset: 0x0001F3C5
		public virtual void Commit(int grfCommitFlags)
		{
			this.dataStream.Flush();
			this.ActualizeVirtualPosition();
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x000203D8 File Offset: 0x0001F3D8
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
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
						throw GPStream.EFail("Wrote an incorrect number of bytes");
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

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00020470 File Offset: 0x0001F470
		public virtual Stream GetDataStream()
		{
			return this.dataStream;
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00020478 File Offset: 0x0001F478
		public virtual void LockRegion(long libOffset, long cb, int dwLockType)
		{
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0002047A File Offset: 0x0001F47A
		protected static ExternalException EFail(string msg)
		{
			throw new ExternalException(msg, -2147467259);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00020487 File Offset: 0x0001F487
		protected static void NotImplemented()
		{
			throw new ExternalException(SR.GetString("NotImplemented"), -2147467263);
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x000204A0 File Offset: 0x0001F4A0
		public virtual int Read(IntPtr buf, int length)
		{
			byte[] array = new byte[length];
			int num = this.Read(array, length);
			Marshal.Copy(array, 0, buf, length);
			return num;
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x000204C7 File Offset: 0x0001F4C7
		public virtual int Read(byte[] buffer, int length)
		{
			this.ActualizeVirtualPosition();
			return this.dataStream.Read(buffer, 0, length);
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x000204DD File Offset: 0x0001F4DD
		public virtual void Revert()
		{
			GPStream.NotImplemented();
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x000204E4 File Offset: 0x0001F4E4
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

		// Token: 0x06000AEB RID: 2795 RVA: 0x000205BE File Offset: 0x0001F5BE
		public virtual void SetSize(long value)
		{
			this.dataStream.SetLength(value);
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x000205CC File Offset: 0x0001F5CC
		public void Stat(IntPtr pstatstg, int grfStatFlag)
		{
			Marshal.StructureToPtr(new GPStream.STATSTG
			{
				cbSize = this.dataStream.Length
			}, pstatstg, true);
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x000205F8 File Offset: 0x0001F5F8
		public virtual void UnlockRegion(long libOffset, long cb, int dwLockType)
		{
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x000205FC File Offset: 0x0001F5FC
		public virtual int Write(IntPtr buf, int length)
		{
			byte[] array = new byte[length];
			Marshal.Copy(buf, array, 0, length);
			return this.Write(array, length);
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x00020621 File Offset: 0x0001F621
		public virtual int Write(byte[] buffer, int length)
		{
			this.ActualizeVirtualPosition();
			this.dataStream.Write(buffer, 0, length);
			return length;
		}

		// Token: 0x040009C9 RID: 2505
		protected Stream dataStream;

		// Token: 0x040009CA RID: 2506
		private long virtualPosition = -1L;

		// Token: 0x020000B3 RID: 179
		[StructLayout(LayoutKind.Sequential)]
		public class STATSTG
		{
			// Token: 0x040009CB RID: 2507
			public IntPtr pwcsName = IntPtr.Zero;

			// Token: 0x040009CC RID: 2508
			public int type;

			// Token: 0x040009CD RID: 2509
			[MarshalAs(UnmanagedType.I8)]
			public long cbSize;

			// Token: 0x040009CE RID: 2510
			[MarshalAs(UnmanagedType.I8)]
			public long mtime;

			// Token: 0x040009CF RID: 2511
			[MarshalAs(UnmanagedType.I8)]
			public long ctime;

			// Token: 0x040009D0 RID: 2512
			[MarshalAs(UnmanagedType.I8)]
			public long atime;

			// Token: 0x040009D1 RID: 2513
			[MarshalAs(UnmanagedType.I4)]
			public int grfMode;

			// Token: 0x040009D2 RID: 2514
			[MarshalAs(UnmanagedType.I4)]
			public int grfLocksSupported;

			// Token: 0x040009D3 RID: 2515
			public int clsid_data1;

			// Token: 0x040009D4 RID: 2516
			[MarshalAs(UnmanagedType.I2)]
			public short clsid_data2;

			// Token: 0x040009D5 RID: 2517
			[MarshalAs(UnmanagedType.I2)]
			public short clsid_data3;

			// Token: 0x040009D6 RID: 2518
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b0;

			// Token: 0x040009D7 RID: 2519
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b1;

			// Token: 0x040009D8 RID: 2520
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b2;

			// Token: 0x040009D9 RID: 2521
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b3;

			// Token: 0x040009DA RID: 2522
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b4;

			// Token: 0x040009DB RID: 2523
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b5;

			// Token: 0x040009DC RID: 2524
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b6;

			// Token: 0x040009DD RID: 2525
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b7;

			// Token: 0x040009DE RID: 2526
			[MarshalAs(UnmanagedType.I4)]
			public int grfStateBits;

			// Token: 0x040009DF RID: 2527
			[MarshalAs(UnmanagedType.I4)]
			public int reserved;
		}
	}
}
