using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Drawing.Internal
{
	// Token: 0x0200001A RID: 26
	internal sealed class DeviceContext : MarshalByRefObject, IDeviceContext, IDisposable
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600007F RID: 127 RVA: 0x000037EE File Offset: 0x000027EE
		// (remove) Token: 0x06000080 RID: 128 RVA: 0x00003807 File Offset: 0x00002807
		public event EventHandler Disposing;

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003820 File Offset: 0x00002820
		public DeviceContextType DeviceContextType
		{
			get
			{
				return this.dcType;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003828 File Offset: 0x00002828
		public IntPtr Hdc
		{
			get
			{
				if (this.hDC == IntPtr.Zero && this.dcType == DeviceContextType.Display)
				{
					this.hDC = ((IDeviceContext)this).GetHdc();
					this.CacheInitialState();
				}
				return this.hDC;
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003860 File Offset: 0x00002860
		private void CacheInitialState()
		{
			this.hCurrentPen = (this.hInitialPen = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(this, this.hDC), 1));
			this.hCurrentBrush = (this.hInitialBrush = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(this, this.hDC), 2));
			this.hCurrentBmp = (this.hInitialBmp = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(this, this.hDC), 7));
			this.hCurrentFont = (this.hInitialFont = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(this, this.hDC), 6));
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000038F4 File Offset: 0x000028F4
		public void DeleteObject(IntPtr handle, GdiObjectType type)
		{
			IntPtr intPtr = IntPtr.Zero;
			switch (type)
			{
			case GdiObjectType.Pen:
				if (handle == this.hCurrentPen)
				{
					IntUnsafeNativeMethods.SelectObject(new HandleRef(this, this.Hdc), new HandleRef(this, this.hInitialPen));
					this.hCurrentPen = IntPtr.Zero;
				}
				intPtr = handle;
				break;
			case GdiObjectType.Brush:
				if (handle == this.hCurrentBrush)
				{
					IntUnsafeNativeMethods.SelectObject(new HandleRef(this, this.Hdc), new HandleRef(this, this.hInitialBrush));
					this.hCurrentBrush = IntPtr.Zero;
				}
				intPtr = handle;
				break;
			default:
				if (type == GdiObjectType.Bitmap)
				{
					if (handle == this.hCurrentBmp)
					{
						IntUnsafeNativeMethods.SelectObject(new HandleRef(this, this.Hdc), new HandleRef(this, this.hInitialBmp));
						this.hCurrentBmp = IntPtr.Zero;
					}
					intPtr = handle;
				}
				break;
			}
			IntUnsafeNativeMethods.DeleteObject(new HandleRef(this, intPtr));
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000039DE File Offset: 0x000029DE
		private DeviceContext(IntPtr hWnd)
		{
			this.hWnd = hWnd;
			this.dcType = DeviceContextType.Display;
			DeviceContexts.AddDeviceContext(this);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003A08 File Offset: 0x00002A08
		private DeviceContext(IntPtr hDC, DeviceContextType dcType)
		{
			this.hDC = hDC;
			this.dcType = dcType;
			this.CacheInitialState();
			DeviceContexts.AddDeviceContext(this);
			if (dcType == DeviceContextType.Display)
			{
				this.hWnd = IntUnsafeNativeMethods.WindowFromDC(new HandleRef(this, this.hDC));
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003A5C File Offset: 0x00002A5C
		public static DeviceContext CreateDC(string driverName, string deviceName, string fileName, HandleRef devMode)
		{
			IntPtr intPtr = IntUnsafeNativeMethods.CreateDC(driverName, deviceName, fileName, devMode);
			return new DeviceContext(intPtr, DeviceContextType.NamedDevice);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003A7C File Offset: 0x00002A7C
		public static DeviceContext CreateIC(string driverName, string deviceName, string fileName, HandleRef devMode)
		{
			IntPtr intPtr = IntUnsafeNativeMethods.CreateIC(driverName, deviceName, fileName, devMode);
			return new DeviceContext(intPtr, DeviceContextType.Information);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003A9C File Offset: 0x00002A9C
		public static DeviceContext FromCompatibleDC(IntPtr hdc)
		{
			IntPtr intPtr = IntUnsafeNativeMethods.CreateCompatibleDC(new HandleRef(null, hdc));
			return new DeviceContext(intPtr, DeviceContextType.Memory);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003ABD File Offset: 0x00002ABD
		public static DeviceContext FromHdc(IntPtr hdc)
		{
			return new DeviceContext(hdc, DeviceContextType.Unknown);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003AC6 File Offset: 0x00002AC6
		public static DeviceContext FromHwnd(IntPtr hwnd)
		{
			return new DeviceContext(hwnd);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003AD0 File Offset: 0x00002AD0
		~DeviceContext()
		{
			this.Dispose(false);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003B00 File Offset: 0x00002B00
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003B10 File Offset: 0x00002B10
		internal void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			if (this.Disposing != null)
			{
				this.Disposing(this, EventArgs.Empty);
			}
			this.disposed = true;
			switch (this.dcType)
			{
			case DeviceContextType.Unknown:
			case DeviceContextType.NCWindow:
				break;
			case DeviceContextType.Display:
				((IDeviceContext)this).ReleaseHdc();
				return;
			case DeviceContextType.NamedDevice:
			case DeviceContextType.Information:
				IntUnsafeNativeMethods.DeleteHDC(new HandleRef(this, this.hDC));
				this.hDC = IntPtr.Zero;
				return;
			case DeviceContextType.Memory:
				IntUnsafeNativeMethods.DeleteDC(new HandleRef(this, this.hDC));
				this.hDC = IntPtr.Zero;
				break;
			default:
				return;
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003BAE File Offset: 0x00002BAE
		IntPtr IDeviceContext.GetHdc()
		{
			if (this.hDC == IntPtr.Zero)
			{
				this.hDC = IntUnsafeNativeMethods.GetDC(new HandleRef(this, this.hWnd));
			}
			return this.hDC;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003BE0 File Offset: 0x00002BE0
		void IDeviceContext.ReleaseHdc()
		{
			if (this.hDC != IntPtr.Zero && this.dcType == DeviceContextType.Display)
			{
				IntUnsafeNativeMethods.ReleaseDC(new HandleRef(this, this.hWnd), new HandleRef(this, this.hDC));
				this.hDC = IntPtr.Zero;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00003C31 File Offset: 0x00002C31
		public DeviceContextGraphicsMode GraphicsMode
		{
			get
			{
				return (DeviceContextGraphicsMode)IntUnsafeNativeMethods.GetGraphicsMode(new HandleRef(this, this.Hdc));
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003C44 File Offset: 0x00002C44
		public DeviceContextGraphicsMode SetGraphicsMode(DeviceContextGraphicsMode newMode)
		{
			return (DeviceContextGraphicsMode)IntUnsafeNativeMethods.SetGraphicsMode(new HandleRef(this, this.Hdc), (int)newMode);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003C58 File Offset: 0x00002C58
		public void RestoreHdc()
		{
			IntUnsafeNativeMethods.RestoreDC(new HandleRef(this, this.hDC), -1);
			if (this.contextStack != null)
			{
				DeviceContext.GraphicsState graphicsState = (DeviceContext.GraphicsState)this.contextStack.Pop();
				this.hCurrentBmp = graphicsState.hBitmap;
				this.hCurrentBrush = graphicsState.hBrush;
				this.hCurrentPen = graphicsState.hPen;
				this.hCurrentFont = graphicsState.hFont;
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003CC4 File Offset: 0x00002CC4
		public int SaveHdc()
		{
			HandleRef handleRef = new HandleRef(this, this.Hdc);
			int num = IntUnsafeNativeMethods.SaveDC(handleRef);
			if (this.contextStack == null)
			{
				this.contextStack = new Stack();
			}
			DeviceContext.GraphicsState graphicsState = new DeviceContext.GraphicsState();
			graphicsState.hBitmap = this.hCurrentBmp;
			graphicsState.hBrush = this.hCurrentBrush;
			graphicsState.hPen = this.hCurrentPen;
			graphicsState.hFont = this.hCurrentFont;
			this.contextStack.Push(graphicsState);
			return num;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003D3C File Offset: 0x00002D3C
		public void SetClip(WindowsRegion region)
		{
			HandleRef handleRef = new HandleRef(this, this.Hdc);
			HandleRef handleRef2 = new HandleRef(region, region.HRegion);
			IntUnsafeNativeMethods.SelectClipRgn(handleRef, handleRef2);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003D70 File Offset: 0x00002D70
		public void IntersectClip(WindowsRegion wr)
		{
			if (wr.HRegion == IntPtr.Zero)
			{
				return;
			}
			WindowsRegion windowsRegion = new WindowsRegion(0, 0, 0, 0);
			try
			{
				int clipRgn = IntUnsafeNativeMethods.GetClipRgn(new HandleRef(this, this.Hdc), new HandleRef(windowsRegion, windowsRegion.HRegion));
				if (clipRgn == 1)
				{
					wr.CombineRegion(windowsRegion, wr, RegionCombineMode.AND);
				}
				this.SetClip(wr);
			}
			finally
			{
				windowsRegion.Dispose();
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003DE8 File Offset: 0x00002DE8
		public void TranslateTransform(int dx, int dy)
		{
			IntNativeMethods.POINT point = new IntNativeMethods.POINT();
			IntUnsafeNativeMethods.OffsetViewportOrgEx(new HandleRef(this, this.Hdc), dx, dy, point);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003E10 File Offset: 0x00002E10
		public override bool Equals(object obj)
		{
			DeviceContext deviceContext = obj as DeviceContext;
			return deviceContext == this || (deviceContext != null && deviceContext.Hdc == this.Hdc);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003E40 File Offset: 0x00002E40
		public override int GetHashCode()
		{
			return this.Hdc.GetHashCode();
		}

		// Token: 0x040000E1 RID: 225
		private IntPtr hDC;

		// Token: 0x040000E2 RID: 226
		private DeviceContextType dcType;

		// Token: 0x040000E4 RID: 228
		private bool disposed;

		// Token: 0x040000E5 RID: 229
		private IntPtr hWnd = (IntPtr)(-1);

		// Token: 0x040000E6 RID: 230
		private IntPtr hInitialPen;

		// Token: 0x040000E7 RID: 231
		private IntPtr hInitialBrush;

		// Token: 0x040000E8 RID: 232
		private IntPtr hInitialBmp;

		// Token: 0x040000E9 RID: 233
		private IntPtr hInitialFont;

		// Token: 0x040000EA RID: 234
		private IntPtr hCurrentPen;

		// Token: 0x040000EB RID: 235
		private IntPtr hCurrentBrush;

		// Token: 0x040000EC RID: 236
		private IntPtr hCurrentBmp;

		// Token: 0x040000ED RID: 237
		private IntPtr hCurrentFont;

		// Token: 0x040000EE RID: 238
		private Stack contextStack;

		// Token: 0x0200001B RID: 27
		internal class GraphicsState
		{
			// Token: 0x040000EF RID: 239
			internal IntPtr hBrush;

			// Token: 0x040000F0 RID: 240
			internal IntPtr hFont;

			// Token: 0x040000F1 RID: 241
			internal IntPtr hPen;

			// Token: 0x040000F2 RID: 242
			internal IntPtr hBitmap;
		}
	}
}
