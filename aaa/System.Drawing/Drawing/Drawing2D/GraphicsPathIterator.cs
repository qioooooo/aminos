using System;
using System.Drawing.Internal;
using System.Runtime.InteropServices;

namespace System.Drawing.Drawing2D
{
	// Token: 0x020000B6 RID: 182
	public sealed class GraphicsPathIterator : MarshalByRefObject, IDisposable
	{
		// Token: 0x06000B56 RID: 2902 RVA: 0x00021F2C File Offset: 0x00020F2C
		public GraphicsPathIterator(GraphicsPath path)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreatePathIter(out zero, new HandleRef(path, (path == null) ? IntPtr.Zero : path.nativePath));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.nativeIter = zero;
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00021F74 File Offset: 0x00020F74
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x00021F84 File Offset: 0x00020F84
		private void Dispose(bool disposing)
		{
			if (this.nativeIter != IntPtr.Zero)
			{
				try
				{
					SafeNativeMethods.Gdip.GdipDeletePathIter(new HandleRef(this, this.nativeIter));
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				finally
				{
					this.nativeIter = IntPtr.Zero;
				}
			}
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00021FF0 File Offset: 0x00020FF0
		~GraphicsPathIterator()
		{
			this.Dispose(false);
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00022020 File Offset: 0x00021020
		public int NextSubpath(out int startIndex, out int endIndex, out bool isClosed)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = SafeNativeMethods.Gdip.GdipPathIterNextSubpath(new HandleRef(this, this.nativeIter), out num, out num2, out num3, out isClosed);
			if (num4 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num4);
			}
			startIndex = num2;
			endIndex = num3;
			return num;
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00022060 File Offset: 0x00021060
		public int NextSubpath(GraphicsPath path, out bool isClosed)
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipPathIterNextSubpathPath(new HandleRef(this, this.nativeIter), out num, new HandleRef(path, (path == null) ? IntPtr.Zero : path.nativePath), out isClosed);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return num;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x000220A8 File Offset: 0x000210A8
		public int NextPathType(out byte pathType, out int startIndex, out int endIndex)
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipPathIterNextPathType(new HandleRef(this, this.nativeIter), out num, out pathType, out startIndex, out endIndex);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return num;
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x000220DC File Offset: 0x000210DC
		public int NextMarker(out int startIndex, out int endIndex)
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipPathIterNextMarker(new HandleRef(this, this.nativeIter), out num, out startIndex, out endIndex);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return num;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0002210C File Offset: 0x0002110C
		public int NextMarker(GraphicsPath path)
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipPathIterNextMarkerPath(new HandleRef(this, this.nativeIter), out num, new HandleRef(path, (path == null) ? IntPtr.Zero : path.nativePath));
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return num;
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x00022150 File Offset: 0x00021150
		public int Count
		{
			get
			{
				int num = 0;
				int num2 = SafeNativeMethods.Gdip.GdipPathIterGetCount(new HandleRef(this, this.nativeIter), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return num;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000B60 RID: 2912 RVA: 0x00022180 File Offset: 0x00021180
		public int SubpathCount
		{
			get
			{
				int num = 0;
				int num2 = SafeNativeMethods.Gdip.GdipPathIterGetSubpathCount(new HandleRef(this, this.nativeIter), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return num;
			}
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x000221B0 File Offset: 0x000211B0
		public bool HasCurve()
		{
			bool flag = false;
			int num = SafeNativeMethods.Gdip.GdipPathIterHasCurve(new HandleRef(this, this.nativeIter), out flag);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return flag;
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x000221E0 File Offset: 0x000211E0
		public void Rewind()
		{
			int num = SafeNativeMethods.Gdip.GdipPathIterRewind(new HandleRef(this, this.nativeIter));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0002220C File Offset: 0x0002120C
		public int Enumerate(ref PointF[] points, ref byte[] types)
		{
			if (points.Length != types.Length)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			int num = 0;
			int num2 = Marshal.SizeOf(typeof(GPPOINTF));
			int num3 = points.Length;
			byte[] array = new byte[num3];
			checked
			{
				IntPtr intPtr = Marshal.AllocHGlobal(num3 * num2);
				try
				{
					int num4 = SafeNativeMethods.Gdip.GdipPathIterEnumerate(new HandleRef(this, this.nativeIter), out num, intPtr, array, num3);
					if (num4 != 0)
					{
						throw SafeNativeMethods.Gdip.StatusException(num4);
					}
					if (num < num3)
					{
						SafeNativeMethods.ZeroMemory((IntPtr)((long)intPtr + unchecked((long)(checked(num * num2)))), (UIntPtr)((ulong)(unchecked((long)((num3 - num) * num2)))));
					}
					points = SafeNativeMethods.Gdip.ConvertGPPOINTFArrayF(intPtr, num3);
					array.CopyTo(types, 0);
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
				return num;
			}
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x000222CC File Offset: 0x000212CC
		public int CopyData(ref PointF[] points, ref byte[] types, int startIndex, int endIndex)
		{
			if (points.Length != types.Length || endIndex - startIndex + 1 > points.Length)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			int num = 0;
			int num2 = Marshal.SizeOf(typeof(GPPOINTF));
			int num3 = points.Length;
			byte[] array = new byte[num3];
			checked
			{
				IntPtr intPtr = Marshal.AllocHGlobal(num3 * num2);
				try
				{
					int num4 = SafeNativeMethods.Gdip.GdipPathIterCopyData(new HandleRef(this, this.nativeIter), out num, intPtr, array, startIndex, endIndex);
					if (num4 != 0)
					{
						throw SafeNativeMethods.Gdip.StatusException(num4);
					}
					if (num < num3)
					{
						SafeNativeMethods.ZeroMemory((IntPtr)((long)intPtr + unchecked((long)(checked(num * num2)))), (UIntPtr)((ulong)(unchecked((long)((num3 - num) * num2)))));
					}
					points = SafeNativeMethods.Gdip.ConvertGPPOINTFArrayF(intPtr, num3);
					array.CopyTo(types, 0);
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
				return num;
			}
		}

		// Token: 0x040009E2 RID: 2530
		internal IntPtr nativeIter;
	}
}
