using System;
using System.Internal;
using System.Runtime.InteropServices;

namespace System.Drawing.Internal
{
	// Token: 0x02000020 RID: 32
	internal sealed class WindowsRegion : MarshalByRefObject, ICloneable, IDisposable
	{
		// Token: 0x0600009E RID: 158 RVA: 0x00003F08 File Offset: 0x00002F08
		private WindowsRegion()
		{
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003F10 File Offset: 0x00002F10
		public WindowsRegion(Rectangle rect)
		{
			this.CreateRegion(rect);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003F1F File Offset: 0x00002F1F
		public WindowsRegion(int x, int y, int width, int height)
		{
			this.CreateRegion(new Rectangle(x, y, width, height));
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003F38 File Offset: 0x00002F38
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

		// Token: 0x060000A2 RID: 162 RVA: 0x00003F76 File Offset: 0x00002F76
		public static WindowsRegion FromRegion(Region region, Graphics g)
		{
			if (region.IsInfinite(g))
			{
				return new WindowsRegion();
			}
			return WindowsRegion.FromHregion(region.GetHrgn(g), true);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003F94 File Offset: 0x00002F94
		public object Clone()
		{
			if (!this.IsInfinite)
			{
				return new WindowsRegion(this.ToRectangle());
			}
			return new WindowsRegion();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003FAF File Offset: 0x00002FAF
		public IntNativeMethods.RegionFlags CombineRegion(WindowsRegion region1, WindowsRegion region2, RegionCombineMode mode)
		{
			return IntUnsafeNativeMethods.CombineRgn(new HandleRef(this, this.HRegion), new HandleRef(region1, region1.HRegion), new HandleRef(region2, region2.HRegion), mode);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003FDB File Offset: 0x00002FDB
		private void CreateRegion(Rectangle rect)
		{
			this.nativeHandle = IntSafeNativeMethods.CreateRectRgn(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
			this.ownHandle = true;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000401B File Offset: 0x0000301B
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004024 File Offset: 0x00003024
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

		// Token: 0x060000A8 RID: 168 RVA: 0x00004074 File Offset: 0x00003074
		~WindowsRegion()
		{
			this.Dispose(false);
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x000040A4 File Offset: 0x000030A4
		public IntPtr HRegion
		{
			get
			{
				return this.nativeHandle;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000040AC File Offset: 0x000030AC
		public bool IsInfinite
		{
			get
			{
				return this.nativeHandle == IntPtr.Zero;
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000040C0 File Offset: 0x000030C0
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

		// Token: 0x0400010F RID: 271
		private IntPtr nativeHandle;

		// Token: 0x04000110 RID: 272
		private bool ownHandle;
	}
}
