using System;
using System.Drawing;
using System.Internal;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Internal
{
	// Token: 0x0200002D RID: 45
	internal sealed class WindowsRegion : MarshalByRefObject, ICloneable, IDisposable
	{
		// Token: 0x06000140 RID: 320 RVA: 0x000053A9 File Offset: 0x000043A9
		private WindowsRegion()
		{
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000053B1 File Offset: 0x000043B1
		public WindowsRegion(Rectangle rect)
		{
			this.CreateRegion(rect);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000053C0 File Offset: 0x000043C0
		public WindowsRegion(int x, int y, int width, int height)
		{
			this.CreateRegion(new Rectangle(x, y, width, height));
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000053D8 File Offset: 0x000043D8
		public static WindowsRegion FromHregion(IntPtr hRegion, bool takeOwnership)
		{
			WindowsRegion windowsRegion = new WindowsRegion();
			if (hRegion != IntPtr.Zero)
			{
				windowsRegion.nativeHandle = hRegion;
				if (takeOwnership)
				{
					windowsRegion.ownHandle = true;
					global::System.Internal.HandleCollector.Add(hRegion, IntSafeNativeMethods.CommonHandles.GDI);
				}
			}
			return windowsRegion;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00005416 File Offset: 0x00004416
		public static WindowsRegion FromRegion(Region region, Graphics g)
		{
			if (region.IsInfinite(g))
			{
				return new WindowsRegion();
			}
			return WindowsRegion.FromHregion(region.GetHrgn(g), true);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00005434 File Offset: 0x00004434
		public object Clone()
		{
			if (!this.IsInfinite)
			{
				return new WindowsRegion(this.ToRectangle());
			}
			return new WindowsRegion();
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000544F File Offset: 0x0000444F
		public IntNativeMethods.RegionFlags CombineRegion(WindowsRegion region1, WindowsRegion region2, RegionCombineMode mode)
		{
			return IntUnsafeNativeMethods.CombineRgn(new HandleRef(this, this.HRegion), new HandleRef(region1, region1.HRegion), new HandleRef(region2, region2.HRegion), mode);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000547B File Offset: 0x0000447B
		private void CreateRegion(Rectangle rect)
		{
			this.nativeHandle = IntSafeNativeMethods.CreateRectRgn(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
			this.ownHandle = true;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000054BB File Offset: 0x000044BB
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000054C4 File Offset: 0x000044C4
		public void Dispose(bool disposing)
		{
			if (this.nativeHandle != IntPtr.Zero)
			{
				if (this.ownHandle)
				{
					IntUnsafeNativeMethods.DeleteObject(new HandleRef(this, this.nativeHandle));
				}
				this.nativeHandle = IntPtr.Zero;
				if (disposing)
				{
					GC.SuppressFinalize(this);
				}
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005514 File Offset: 0x00004514
		~WindowsRegion()
		{
			this.Dispose(false);
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00005544 File Offset: 0x00004544
		public IntPtr HRegion
		{
			get
			{
				return this.nativeHandle;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600014C RID: 332 RVA: 0x0000554C File Offset: 0x0000454C
		public bool IsInfinite
		{
			get
			{
				return this.nativeHandle == IntPtr.Zero;
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00005560 File Offset: 0x00004560
		public Rectangle ToRectangle()
		{
			if (this.IsInfinite)
			{
				return new Rectangle(-2147483647, -2147483647, int.MaxValue, int.MaxValue);
			}
			IntNativeMethods.RECT rect = default(IntNativeMethods.RECT);
			IntUnsafeNativeMethods.GetRgnBox(new HandleRef(this, this.nativeHandle), ref rect);
			return new Rectangle(new Point(rect.left, rect.top), rect.Size);
		}

		// Token: 0x04000AF8 RID: 2808
		private IntPtr nativeHandle;

		// Token: 0x04000AF9 RID: 2809
		private bool ownHandle;
	}
}
