using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Internal
{
	// Token: 0x0200000A RID: 10
	internal sealed class DeviceContext : MarshalByRefObject, IDeviceContext, IDisposable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600000D RID: 13 RVA: 0x0000229D File Offset: 0x0000129D
		// (remove) Token: 0x0600000E RID: 14 RVA: 0x000022B6 File Offset: 0x000012B6
		public event EventHandler Disposing;

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000022CF File Offset: 0x000012CF
		public DeviceContextType DeviceContextType
		{
			get
			{
				return this.dcType;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000022D7 File Offset: 0x000012D7
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

		// Token: 0x06000011 RID: 17 RVA: 0x0000230C File Offset: 0x0000130C
		private void CacheInitialState()
		{
			this.hCurrentPen = (this.hInitialPen = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(this, this.hDC), 1));
			this.hCurrentBrush = (this.hInitialBrush = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(this, this.hDC), 2));
			this.hCurrentBmp = (this.hInitialBmp = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(this, this.hDC), 7));
			this.hCurrentFont = (this.hInitialFont = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(this, this.hDC), 6));
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000023A0 File Offset: 0x000013A0
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

		// Token: 0x06000013 RID: 19 RVA: 0x0000248A File Offset: 0x0000148A
		private DeviceContext(IntPtr hWnd)
		{
			this.hWnd = hWnd;
			this.dcType = DeviceContextType.Display;
			DeviceContexts.AddDeviceContext(this);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000024B4 File Offset: 0x000014B4
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

		// Token: 0x06000015 RID: 21 RVA: 0x00002508 File Offset: 0x00001508
		public static DeviceContext CreateDC(string driverName, string deviceName, string fileName, HandleRef devMode)
		{
			IntPtr intPtr = IntUnsafeNativeMethods.CreateDC(driverName, deviceName, fileName, devMode);
			return new DeviceContext(intPtr, DeviceContextType.NamedDevice);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002528 File Offset: 0x00001528
		public static DeviceContext CreateIC(string driverName, string deviceName, string fileName, HandleRef devMode)
		{
			IntPtr intPtr = IntUnsafeNativeMethods.CreateIC(driverName, deviceName, fileName, devMode);
			return new DeviceContext(intPtr, DeviceContextType.Information);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002548 File Offset: 0x00001548
		public static DeviceContext FromCompatibleDC(IntPtr hdc)
		{
			IntPtr intPtr = IntUnsafeNativeMethods.CreateCompatibleDC(new HandleRef(null, hdc));
			return new DeviceContext(intPtr, DeviceContextType.Memory);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002569 File Offset: 0x00001569
		public static DeviceContext FromHdc(IntPtr hdc)
		{
			return new DeviceContext(hdc, DeviceContextType.Unknown);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002572 File Offset: 0x00001572
		public static DeviceContext FromHwnd(IntPtr hwnd)
		{
			return new DeviceContext(hwnd);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000257C File Offset: 0x0000157C
		~DeviceContext()
		{
			this.Dispose(false);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000025AC File Offset: 0x000015AC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000025BC File Offset: 0x000015BC
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
			this.DisposeFont(disposing);
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

		// Token: 0x0600001D RID: 29 RVA: 0x00002661 File Offset: 0x00001661
		IntPtr IDeviceContext.GetHdc()
		{
			if (this.hDC == IntPtr.Zero)
			{
				this.hDC = IntUnsafeNativeMethods.GetDC(new HandleRef(this, this.hWnd));
			}
			return this.hDC;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002694 File Offset: 0x00001694
		void IDeviceContext.ReleaseHdc()
		{
			if (this.hDC != IntPtr.Zero && this.dcType == DeviceContextType.Display)
			{
				IntUnsafeNativeMethods.ReleaseDC(new HandleRef(this, this.hWnd), new HandleRef(this, this.hDC));
				this.hDC = IntPtr.Zero;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000026E5 File Offset: 0x000016E5
		public DeviceContextGraphicsMode GraphicsMode
		{
			get
			{
				return (DeviceContextGraphicsMode)IntUnsafeNativeMethods.GetGraphicsMode(new HandleRef(this, this.Hdc));
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000026F8 File Offset: 0x000016F8
		public DeviceContextGraphicsMode SetGraphicsMode(DeviceContextGraphicsMode newMode)
		{
			return (DeviceContextGraphicsMode)IntUnsafeNativeMethods.SetGraphicsMode(new HandleRef(this, this.Hdc), (int)newMode);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000270C File Offset: 0x0000170C
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
				if (graphicsState.font != null && graphicsState.font.IsAlive)
				{
					this.selectedFont = graphicsState.font.Target as WindowsFont;
				}
				else
				{
					WindowsFont windowsFont = this.selectedFont;
					this.selectedFont = null;
					if (windowsFont != null && MeasurementDCInfo.IsMeasurementDC(this))
					{
						windowsFont.Dispose();
					}
				}
			}
			MeasurementDCInfo.ResetIfIsMeasurementDC(this.hDC);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000027D0 File Offset: 0x000017D0
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
			graphicsState.font = new WeakReference(this.selectedFont);
			this.contextStack.Push(graphicsState);
			return num;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000285C File Offset: 0x0000185C
		public void SetClip(WindowsRegion region)
		{
			HandleRef handleRef = new HandleRef(this, this.Hdc);
			HandleRef handleRef2 = new HandleRef(region, region.HRegion);
			IntUnsafeNativeMethods.SelectClipRgn(handleRef, handleRef2);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002890 File Offset: 0x00001890
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

		// Token: 0x06000025 RID: 37 RVA: 0x00002908 File Offset: 0x00001908
		public void TranslateTransform(int dx, int dy)
		{
			IntNativeMethods.POINT point = new IntNativeMethods.POINT();
			IntUnsafeNativeMethods.OffsetViewportOrgEx(new HandleRef(this, this.Hdc), dx, dy, point);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002930 File Offset: 0x00001930
		public override bool Equals(object obj)
		{
			DeviceContext deviceContext = obj as DeviceContext;
			return deviceContext == this || (deviceContext != null && deviceContext.Hdc == this.Hdc);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002960 File Offset: 0x00001960
		public override int GetHashCode()
		{
			return this.Hdc.GetHashCode();
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002981 File Offset: 0x00001981
		public WindowsFont ActiveFont
		{
			get
			{
				return this.selectedFont;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002989 File Offset: 0x00001989
		public Color BackgroundColor
		{
			get
			{
				return ColorTranslator.FromWin32(IntUnsafeNativeMethods.GetBkColor(new HandleRef(this, this.Hdc)));
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000029A1 File Offset: 0x000019A1
		public Color SetBackgroundColor(Color newColor)
		{
			return ColorTranslator.FromWin32(IntUnsafeNativeMethods.SetBkColor(new HandleRef(this, this.Hdc), ColorTranslator.ToWin32(newColor)));
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000029BF File Offset: 0x000019BF
		public DeviceContextBackgroundMode BackgroundMode
		{
			get
			{
				return (DeviceContextBackgroundMode)IntUnsafeNativeMethods.GetBkMode(new HandleRef(this, this.Hdc));
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000029D2 File Offset: 0x000019D2
		public DeviceContextBackgroundMode SetBackgroundMode(DeviceContextBackgroundMode newMode)
		{
			return (DeviceContextBackgroundMode)IntUnsafeNativeMethods.SetBkMode(new HandleRef(this, this.Hdc), (int)newMode);
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000029E6 File Offset: 0x000019E6
		public DeviceContextBinaryRasterOperationFlags BinaryRasterOperation
		{
			get
			{
				return (DeviceContextBinaryRasterOperationFlags)IntUnsafeNativeMethods.GetROP2(new HandleRef(this, this.Hdc));
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000029F9 File Offset: 0x000019F9
		public DeviceContextBinaryRasterOperationFlags SetRasterOperation(DeviceContextBinaryRasterOperationFlags rasterOperation)
		{
			return (DeviceContextBinaryRasterOperationFlags)IntUnsafeNativeMethods.SetROP2(new HandleRef(this, this.Hdc), (int)rasterOperation);
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002A0D File Offset: 0x00001A0D
		public Size Dpi
		{
			get
			{
				return new Size(this.GetDeviceCapabilities(DeviceCapabilities.LogicalPixelsX), this.GetDeviceCapabilities(DeviceCapabilities.LogicalPixelsY));
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002A24 File Offset: 0x00001A24
		public int DpiX
		{
			get
			{
				return this.GetDeviceCapabilities(DeviceCapabilities.LogicalPixelsX);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002A2E File Offset: 0x00001A2E
		public int DpiY
		{
			get
			{
				return this.GetDeviceCapabilities(DeviceCapabilities.LogicalPixelsY);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002A38 File Offset: 0x00001A38
		public WindowsFont Font
		{
			get
			{
				if (MeasurementDCInfo.IsMeasurementDC(this))
				{
					WindowsFont lastUsedFont = MeasurementDCInfo.LastUsedFont;
					if (lastUsedFont != null && lastUsedFont.Hfont != IntPtr.Zero)
					{
						return lastUsedFont;
					}
				}
				return WindowsFont.FromHdc(this.Hdc);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002A75 File Offset: 0x00001A75
		public static DeviceContext ScreenDC
		{
			get
			{
				return DeviceContext.FromHwnd(IntPtr.Zero);
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002A84 File Offset: 0x00001A84
		internal void DisposeFont(bool disposing)
		{
			if (disposing)
			{
				DeviceContexts.RemoveDeviceContext(this);
			}
			if (this.selectedFont != null && this.selectedFont.Hfont != IntPtr.Zero)
			{
				IntPtr currentObject = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(this, this.hDC), 6);
				if (currentObject == this.selectedFont.Hfont)
				{
					IntUnsafeNativeMethods.SelectObject(new HandleRef(this, this.Hdc), new HandleRef(null, this.hInitialFont));
					currentObject = this.hInitialFont;
				}
				this.selectedFont.Dispose(disposing);
				this.selectedFont = null;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002B18 File Offset: 0x00001B18
		public IntPtr SelectFont(WindowsFont font)
		{
			if (font.Equals(this.Font))
			{
				return IntPtr.Zero;
			}
			IntPtr intPtr = this.SelectObject(font.Hfont, GdiObjectType.Font);
			WindowsFont windowsFont = this.selectedFont;
			this.selectedFont = font;
			this.hCurrentFont = font.Hfont;
			if (windowsFont != null && MeasurementDCInfo.IsMeasurementDC(this))
			{
				windowsFont.Dispose();
			}
			if (MeasurementDCInfo.IsMeasurementDC(this))
			{
				if (intPtr != IntPtr.Zero)
				{
					MeasurementDCInfo.LastUsedFont = font;
				}
				else
				{
					MeasurementDCInfo.Reset();
				}
			}
			return intPtr;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002B95 File Offset: 0x00001B95
		public void ResetFont()
		{
			MeasurementDCInfo.ResetIfIsMeasurementDC(this.Hdc);
			IntUnsafeNativeMethods.SelectObject(new HandleRef(this, this.Hdc), new HandleRef(null, this.hInitialFont));
			this.selectedFont = null;
			this.hCurrentFont = this.hInitialFont;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002BD3 File Offset: 0x00001BD3
		public int GetDeviceCapabilities(DeviceCapabilities capabilityIndex)
		{
			return IntUnsafeNativeMethods.GetDeviceCaps(new HandleRef(this, this.Hdc), (int)capabilityIndex);
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002BE7 File Offset: 0x00001BE7
		public DeviceContextMapMode MapMode
		{
			get
			{
				return (DeviceContextMapMode)IntUnsafeNativeMethods.GetMapMode(new HandleRef(this, this.Hdc));
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002BFC File Offset: 0x00001BFC
		public bool IsFontOnContextStack(WindowsFont wf)
		{
			if (this.contextStack == null)
			{
				return false;
			}
			foreach (object obj in this.contextStack)
			{
				DeviceContext.GraphicsState graphicsState = (DeviceContext.GraphicsState)obj;
				if (graphicsState.hFont == wf.Hfont)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002C74 File Offset: 0x00001C74
		public DeviceContextMapMode SetMapMode(DeviceContextMapMode newMode)
		{
			return (DeviceContextMapMode)IntUnsafeNativeMethods.SetMapMode(new HandleRef(this, this.Hdc), (int)newMode);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002C88 File Offset: 0x00001C88
		public IntPtr SelectObject(IntPtr hObj, GdiObjectType type)
		{
			switch (type)
			{
			case GdiObjectType.Pen:
				this.hCurrentPen = hObj;
				break;
			case GdiObjectType.Brush:
				this.hCurrentBrush = hObj;
				break;
			default:
				if (type == GdiObjectType.Bitmap)
				{
					this.hCurrentBmp = hObj;
				}
				break;
			}
			return IntUnsafeNativeMethods.SelectObject(new HandleRef(this, this.Hdc), new HandleRef(null, hObj));
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002CDE File Offset: 0x00001CDE
		public DeviceContextTextAlignment TextAlignment
		{
			get
			{
				return (DeviceContextTextAlignment)IntUnsafeNativeMethods.GetTextAlign(new HandleRef(this, this.Hdc));
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002CF1 File Offset: 0x00001CF1
		public DeviceContextTextAlignment SetTextAlignment(DeviceContextTextAlignment newAligment)
		{
			return (DeviceContextTextAlignment)IntUnsafeNativeMethods.SetTextAlign(new HandleRef(this, this.Hdc), (int)newAligment);
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002D05 File Offset: 0x00001D05
		public Color TextColor
		{
			get
			{
				return ColorTranslator.FromWin32(IntUnsafeNativeMethods.GetTextColor(new HandleRef(this, this.Hdc)));
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002D1D File Offset: 0x00001D1D
		public Color SetTextColor(Color newColor)
		{
			return ColorTranslator.FromWin32(IntUnsafeNativeMethods.SetTextColor(new HandleRef(this, this.Hdc), ColorTranslator.ToWin32(newColor)));
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002D3C File Offset: 0x00001D3C
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00002D68 File Offset: 0x00001D68
		public Size ViewportExtent
		{
			get
			{
				IntNativeMethods.SIZE size = new IntNativeMethods.SIZE();
				IntUnsafeNativeMethods.GetViewportExtEx(new HandleRef(this, this.Hdc), size);
				return size.ToSize();
			}
			set
			{
				this.SetViewportExtent(value);
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002D74 File Offset: 0x00001D74
		public Size SetViewportExtent(Size newExtent)
		{
			IntNativeMethods.SIZE size = new IntNativeMethods.SIZE();
			IntUnsafeNativeMethods.SetViewportExtEx(new HandleRef(this, this.Hdc), newExtent.Width, newExtent.Height, size);
			return size.ToSize();
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002DB0 File Offset: 0x00001DB0
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002DDC File Offset: 0x00001DDC
		public Point ViewportOrigin
		{
			get
			{
				IntNativeMethods.POINT point = new IntNativeMethods.POINT();
				IntUnsafeNativeMethods.GetViewportOrgEx(new HandleRef(this, this.Hdc), point);
				return point.ToPoint();
			}
			set
			{
				this.SetViewportOrigin(value);
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002DE8 File Offset: 0x00001DE8
		public Point SetViewportOrigin(Point newOrigin)
		{
			IntNativeMethods.POINT point = new IntNativeMethods.POINT();
			IntUnsafeNativeMethods.SetViewportOrgEx(new HandleRef(this, this.Hdc), newOrigin.X, newOrigin.Y, point);
			return point.ToPoint();
		}

		// Token: 0x04000995 RID: 2453
		private IntPtr hDC;

		// Token: 0x04000996 RID: 2454
		private DeviceContextType dcType;

		// Token: 0x04000998 RID: 2456
		private bool disposed;

		// Token: 0x04000999 RID: 2457
		private IntPtr hWnd = (IntPtr)(-1);

		// Token: 0x0400099A RID: 2458
		private IntPtr hInitialPen;

		// Token: 0x0400099B RID: 2459
		private IntPtr hInitialBrush;

		// Token: 0x0400099C RID: 2460
		private IntPtr hInitialBmp;

		// Token: 0x0400099D RID: 2461
		private IntPtr hInitialFont;

		// Token: 0x0400099E RID: 2462
		private IntPtr hCurrentPen;

		// Token: 0x0400099F RID: 2463
		private IntPtr hCurrentBrush;

		// Token: 0x040009A0 RID: 2464
		private IntPtr hCurrentBmp;

		// Token: 0x040009A1 RID: 2465
		private IntPtr hCurrentFont;

		// Token: 0x040009A2 RID: 2466
		private Stack contextStack;

		// Token: 0x040009A3 RID: 2467
		private WindowsFont selectedFont;

		// Token: 0x0200000B RID: 11
		internal class GraphicsState
		{
			// Token: 0x040009A4 RID: 2468
			internal IntPtr hBrush;

			// Token: 0x040009A5 RID: 2469
			internal IntPtr hFont;

			// Token: 0x040009A6 RID: 2470
			internal IntPtr hPen;

			// Token: 0x040009A7 RID: 2471
			internal IntPtr hBitmap;

			// Token: 0x040009A8 RID: 2472
			internal WeakReference font;
		}
	}
}
