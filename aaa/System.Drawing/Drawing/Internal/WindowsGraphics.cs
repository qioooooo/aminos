using System;
using System.Drawing.Drawing2D;

namespace System.Drawing.Internal
{
	// Token: 0x02000022 RID: 34
	internal sealed class WindowsGraphics : MarshalByRefObject, IDeviceContext, IDisposable
	{
		// Token: 0x060000AC RID: 172 RVA: 0x0000412A File Offset: 0x0000312A
		public WindowsGraphics(DeviceContext dc)
		{
			this.dc = dc;
			this.dc.SaveHdc();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004148 File Offset: 0x00003148
		public static WindowsGraphics CreateMeasurementWindowsGraphics()
		{
			DeviceContext deviceContext = DeviceContext.FromCompatibleDC(IntPtr.Zero);
			return new WindowsGraphics(deviceContext)
			{
				disposeDc = true
			};
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004170 File Offset: 0x00003170
		public static WindowsGraphics FromHwnd(IntPtr hWnd)
		{
			DeviceContext deviceContext = DeviceContext.FromHwnd(hWnd);
			return new WindowsGraphics(deviceContext)
			{
				disposeDc = true
			};
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004194 File Offset: 0x00003194
		public static WindowsGraphics FromHdc(IntPtr hDc)
		{
			DeviceContext deviceContext = DeviceContext.FromHdc(hDc);
			return new WindowsGraphics(deviceContext)
			{
				disposeDc = true
			};
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000041B8 File Offset: 0x000031B8
		public static WindowsGraphics FromGraphics(Graphics g)
		{
			ApplyGraphicsProperties applyGraphicsProperties = ApplyGraphicsProperties.All;
			return WindowsGraphics.FromGraphics(g, applyGraphicsProperties);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000041D0 File Offset: 0x000031D0
		public static WindowsGraphics FromGraphics(Graphics g, ApplyGraphicsProperties properties)
		{
			WindowsRegion windowsRegion = null;
			float[] array = null;
			Region region = null;
			Matrix matrix = null;
			if ((properties & ApplyGraphicsProperties.TranslateTransform) != ApplyGraphicsProperties.None || (properties & ApplyGraphicsProperties.Clipping) != ApplyGraphicsProperties.None)
			{
				object[] array2 = g.GetContextInfo() as object[];
				if (array2 != null && array2.Length == 2)
				{
					region = array2[0] as Region;
					matrix = array2[1] as Matrix;
				}
				if (matrix != null)
				{
					if ((properties & ApplyGraphicsProperties.TranslateTransform) != ApplyGraphicsProperties.None)
					{
						array = matrix.Elements;
					}
					matrix.Dispose();
				}
				if (region != null)
				{
					if ((properties & ApplyGraphicsProperties.Clipping) != ApplyGraphicsProperties.None && !region.IsInfinite(g))
					{
						windowsRegion = WindowsRegion.FromRegion(region, g);
					}
					region.Dispose();
				}
			}
			WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(g.GetHdc());
			windowsGraphics.graphics = g;
			if (windowsRegion != null)
			{
				using (windowsRegion)
				{
					windowsGraphics.DeviceContext.IntersectClip(windowsRegion);
				}
			}
			if (array != null)
			{
				windowsGraphics.DeviceContext.TranslateTransform((int)array[4], (int)array[5]);
			}
			return windowsGraphics;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000042B0 File Offset: 0x000032B0
		~WindowsGraphics()
		{
			this.Dispose(false);
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x000042E0 File Offset: 0x000032E0
		public DeviceContext DeviceContext
		{
			get
			{
				return this.dc;
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000042E8 File Offset: 0x000032E8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000042F8 File Offset: 0x000032F8
		internal void Dispose(bool disposing)
		{
			if (this.dc != null)
			{
				try
				{
					this.dc.RestoreHdc();
					if (this.disposeDc)
					{
						this.dc.Dispose(disposing);
					}
					if (this.graphics != null)
					{
						this.graphics.ReleaseHdcInternal(this.dc.Hdc);
						this.graphics = null;
					}
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
					this.dc = null;
				}
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004388 File Offset: 0x00003388
		public IntPtr GetHdc()
		{
			return this.dc.Hdc;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004395 File Offset: 0x00003395
		public void ReleaseHdc()
		{
			this.dc.Dispose();
		}

		// Token: 0x04000119 RID: 281
		private DeviceContext dc;

		// Token: 0x0400011A RID: 282
		private bool disposeDc;

		// Token: 0x0400011B RID: 283
		private Graphics graphics;
	}
}
