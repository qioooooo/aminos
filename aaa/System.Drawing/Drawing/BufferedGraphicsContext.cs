using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Drawing
{
	// Token: 0x02000037 RID: 55
	public sealed class BufferedGraphicsContext : IDisposable
	{
		// Token: 0x06000240 RID: 576 RVA: 0x000097C8 File Offset: 0x000087C8
		public BufferedGraphicsContext()
		{
			this.maximumBuffer.Width = 225;
			this.maximumBuffer.Height = 96;
			this.bufferSize = Size.Empty;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x000097F8 File Offset: 0x000087F8
		~BufferedGraphicsContext()
		{
			this.Dispose(false);
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00009828 File Offset: 0x00008828
		internal static TraceSwitch DoubleBuffering
		{
			get
			{
				if (BufferedGraphicsContext.doubleBuffering == null)
				{
					BufferedGraphicsContext.doubleBuffering = new TraceSwitch("DoubleBuffering", "Output information about double buffering");
				}
				return BufferedGraphicsContext.doubleBuffering;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000984A File Offset: 0x0000884A
		// (set) Token: 0x06000244 RID: 580 RVA: 0x00009854 File Offset: 0x00008854
		public Size MaximumBuffer
		{
			get
			{
				return this.maximumBuffer;
			}
			[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
			set
			{
				if (value.Width <= 0 || value.Height <= 0)
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[] { "MaximumBuffer", value }));
				}
				if (value.Width * value.Height < this.maximumBuffer.Width * this.maximumBuffer.Height)
				{
					this.Invalidate();
				}
				this.maximumBuffer = value;
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x000098D3 File Offset: 0x000088D3
		public BufferedGraphics Allocate(Graphics targetGraphics, Rectangle targetRectangle)
		{
			if (this.ShouldUseTempManager(targetRectangle))
			{
				return this.AllocBufferInTempManager(targetGraphics, IntPtr.Zero, targetRectangle);
			}
			return this.AllocBuffer(targetGraphics, IntPtr.Zero, targetRectangle);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x000098F9 File Offset: 0x000088F9
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public BufferedGraphics Allocate(IntPtr targetDC, Rectangle targetRectangle)
		{
			if (this.ShouldUseTempManager(targetRectangle))
			{
				return this.AllocBufferInTempManager(null, targetDC, targetRectangle);
			}
			return this.AllocBuffer(null, targetDC, targetRectangle);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00009918 File Offset: 0x00008918
		private BufferedGraphics AllocBuffer(Graphics targetGraphics, IntPtr targetDC, Rectangle targetRectangle)
		{
			int num = Interlocked.CompareExchange(ref this.busy, 1, 0);
			if (num != 0)
			{
				return this.AllocBufferInTempManager(targetGraphics, targetDC, targetRectangle);
			}
			this.targetLoc = new Point(targetRectangle.X, targetRectangle.Y);
			try
			{
				Graphics graphics;
				if (targetGraphics != null)
				{
					IntPtr hdc = targetGraphics.GetHdc();
					try
					{
						graphics = this.CreateBuffer(hdc, -this.targetLoc.X, -this.targetLoc.Y, targetRectangle.Width, targetRectangle.Height);
						goto IL_00A4;
					}
					finally
					{
						targetGraphics.ReleaseHdcInternal(hdc);
					}
				}
				graphics = this.CreateBuffer(targetDC, -this.targetLoc.X, -this.targetLoc.Y, targetRectangle.Width, targetRectangle.Height);
				IL_00A4:
				this.buffer = new BufferedGraphics(graphics, this, targetGraphics, targetDC, this.targetLoc, this.virtualSize);
			}
			catch
			{
				this.busy = 0;
				throw;
			}
			return this.buffer;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00009A14 File Offset: 0x00008A14
		private BufferedGraphics AllocBufferInTempManager(Graphics targetGraphics, IntPtr targetDC, Rectangle targetRectangle)
		{
			BufferedGraphicsContext bufferedGraphicsContext = null;
			BufferedGraphics bufferedGraphics = null;
			try
			{
				bufferedGraphicsContext = new BufferedGraphicsContext();
				if (bufferedGraphicsContext != null)
				{
					bufferedGraphics = bufferedGraphicsContext.AllocBuffer(targetGraphics, targetDC, targetRectangle);
					bufferedGraphics.DisposeContext = true;
				}
			}
			finally
			{
				if (bufferedGraphicsContext != null && (bufferedGraphics == null || (bufferedGraphics != null && !bufferedGraphics.DisposeContext)))
				{
					bufferedGraphicsContext.Dispose();
				}
			}
			return bufferedGraphics;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00009A6C File Offset: 0x00008A6C
		private bool bFillBitmapInfo(IntPtr hdc, IntPtr hpal, ref NativeMethods.BITMAPINFO_FLAT pbmi)
		{
			IntPtr intPtr = IntPtr.Zero;
			bool flag = false;
			try
			{
				intPtr = SafeNativeMethods.CreateCompatibleBitmap(new HandleRef(null, hdc), 1, 1);
				if (intPtr == IntPtr.Zero)
				{
					throw new OutOfMemoryException(SR.GetString("GraphicsBufferQueryFail"));
				}
				pbmi.bmiHeader_biSize = Marshal.SizeOf(typeof(NativeMethods.BITMAPINFOHEADER));
				pbmi.bmiColors = new byte[1024];
				SafeNativeMethods.GetDIBits(new HandleRef(null, hdc), new HandleRef(null, intPtr), 0, 0, IntPtr.Zero, ref pbmi, 0);
				if (pbmi.bmiHeader_biBitCount <= 8)
				{
					flag = this.bFillColorTable(hdc, hpal, ref pbmi);
				}
				else
				{
					if (pbmi.bmiHeader_biCompression == 3)
					{
						SafeNativeMethods.GetDIBits(new HandleRef(null, hdc), new HandleRef(null, intPtr), 0, pbmi.bmiHeader_biHeight, IntPtr.Zero, ref pbmi, 0);
					}
					flag = true;
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
					intPtr = IntPtr.Zero;
				}
			}
			return flag;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00009B68 File Offset: 0x00008B68
		private unsafe bool bFillColorTable(IntPtr hdc, IntPtr hpal, ref NativeMethods.BITMAPINFO_FLAT pbmi)
		{
			bool flag = false;
			byte[] array = new byte[sizeof(NativeMethods.PALETTEENTRY) * 256];
			fixed (byte* bmiColors = pbmi.bmiColors)
			{
				fixed (byte* ptr = array)
				{
					NativeMethods.RGBQUAD* ptr2 = (NativeMethods.RGBQUAD*)bmiColors;
					NativeMethods.PALETTEENTRY* ptr3 = (NativeMethods.PALETTEENTRY*)ptr;
					int num = 1 << (int)pbmi.bmiHeader_biBitCount;
					if (num <= 256)
					{
						IntPtr intPtr = IntPtr.Zero;
						uint num2;
						if (hpal == IntPtr.Zero)
						{
							intPtr = Graphics.GetHalftonePalette();
							num2 = SafeNativeMethods.GetPaletteEntries(new HandleRef(null, intPtr), 0, num, array);
						}
						else
						{
							num2 = SafeNativeMethods.GetPaletteEntries(new HandleRef(null, hpal), 0, num, array);
						}
						if (num2 != 0U)
						{
							for (int i = 0; i < num; i++)
							{
								ptr2[i].rgbRed = ptr3[i].peRed;
								ptr2[i].rgbGreen = ptr3[i].peGreen;
								ptr2[i].rgbBlue = ptr3[i].peBlue;
								ptr2[i].rgbReserved = 0;
							}
							flag = true;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00009CC0 File Offset: 0x00008CC0
		private Graphics CreateBuffer(IntPtr src, int offsetX, int offsetY, int width, int height)
		{
			this.busy = 2;
			this.DisposeDC();
			this.busy = 1;
			this.compatDC = UnsafeNativeMethods.CreateCompatibleDC(new HandleRef(null, src));
			if (width > this.bufferSize.Width || height > this.bufferSize.Height)
			{
				int num = Math.Max(width, this.bufferSize.Width);
				int num2 = Math.Max(height, this.bufferSize.Height);
				this.busy = 2;
				this.DisposeBitmap();
				this.busy = 1;
				IntPtr zero = IntPtr.Zero;
				this.dib = this.CreateCompatibleDIB(src, IntPtr.Zero, num, num2, ref zero);
				this.bufferSize = new Size(num, num2);
			}
			this.oldBitmap = SafeNativeMethods.SelectObject(new HandleRef(this, this.compatDC), new HandleRef(this, this.dib));
			this.compatGraphics = Graphics.FromHdcInternal(this.compatDC);
			this.compatGraphics.TranslateTransform((float)(-(float)this.targetLoc.X), (float)(-(float)this.targetLoc.Y));
			this.virtualSize = new Size(width, height);
			return this.compatGraphics;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00009DE4 File Offset: 0x00008DE4
		private IntPtr CreateCompatibleDIB(IntPtr hdc, IntPtr hpal, int ulWidth, int ulHeight, ref IntPtr ppvBits)
		{
			if (hdc == IntPtr.Zero)
			{
				throw new ArgumentNullException("hdc");
			}
			IntPtr intPtr = IntPtr.Zero;
			NativeMethods.BITMAPINFO_FLAT bitmapinfo_FLAT = default(NativeMethods.BITMAPINFO_FLAT);
			int objectType = UnsafeNativeMethods.GetObjectType(new HandleRef(null, hdc));
			int num = objectType;
			switch (num)
			{
			case 3:
			case 4:
				break;
			default:
				switch (num)
				{
				case 10:
				case 12:
					break;
				default:
					throw new ArgumentException(SR.GetString("DCTypeInvalid"));
				}
				break;
			}
			if (this.bFillBitmapInfo(hdc, hpal, ref bitmapinfo_FLAT))
			{
				bitmapinfo_FLAT.bmiHeader_biWidth = ulWidth;
				bitmapinfo_FLAT.bmiHeader_biHeight = ulHeight;
				if (bitmapinfo_FLAT.bmiHeader_biCompression == 0)
				{
					bitmapinfo_FLAT.bmiHeader_biSizeImage = 0;
				}
				else if (bitmapinfo_FLAT.bmiHeader_biBitCount == 16)
				{
					bitmapinfo_FLAT.bmiHeader_biSizeImage = ulWidth * ulHeight * 2;
				}
				else if (bitmapinfo_FLAT.bmiHeader_biBitCount == 32)
				{
					bitmapinfo_FLAT.bmiHeader_biSizeImage = ulWidth * ulHeight * 4;
				}
				else
				{
					bitmapinfo_FLAT.bmiHeader_biSizeImage = 0;
				}
				bitmapinfo_FLAT.bmiHeader_biClrUsed = 0;
				bitmapinfo_FLAT.bmiHeader_biClrImportant = 0;
				intPtr = SafeNativeMethods.CreateDIBSection(new HandleRef(null, hdc), ref bitmapinfo_FLAT, 0, ref ppvBits, IntPtr.Zero, 0);
				Win32Exception ex = null;
				if (intPtr == IntPtr.Zero)
				{
					ex = new Win32Exception(Marshal.GetLastWin32Error());
				}
				if (ex != null)
				{
					throw ex;
				}
			}
			return intPtr;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00009F15 File Offset: 0x00008F15
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00009F24 File Offset: 0x00008F24
		private void DisposeDC()
		{
			if (this.oldBitmap != IntPtr.Zero && this.compatDC != IntPtr.Zero)
			{
				SafeNativeMethods.SelectObject(new HandleRef(this, this.compatDC), new HandleRef(this, this.oldBitmap));
				this.oldBitmap = IntPtr.Zero;
			}
			if (this.compatDC != IntPtr.Zero)
			{
				UnsafeNativeMethods.DeleteDC(new HandleRef(this, this.compatDC));
				this.compatDC = IntPtr.Zero;
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00009FAD File Offset: 0x00008FAD
		private void DisposeBitmap()
		{
			if (this.dib != IntPtr.Zero)
			{
				SafeNativeMethods.DeleteObject(new HandleRef(this, this.dib));
				this.dib = IntPtr.Zero;
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00009FE0 File Offset: 0x00008FE0
		private void Dispose(bool disposing)
		{
			int num = Interlocked.CompareExchange(ref this.busy, 2, 0);
			if (disposing)
			{
				if (num == 1)
				{
					throw new InvalidOperationException(SR.GetString("GraphicsBufferCurrentlyBusy"));
				}
				if (this.compatGraphics != null)
				{
					this.compatGraphics.Dispose();
					this.compatGraphics = null;
				}
			}
			this.DisposeDC();
			this.DisposeBitmap();
			if (this.buffer != null)
			{
				this.buffer.Dispose();
				this.buffer = null;
			}
			this.bufferSize = Size.Empty;
			this.virtualSize = Size.Empty;
			this.busy = 0;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000A070 File Offset: 0x00009070
		public void Invalidate()
		{
			if (Interlocked.CompareExchange(ref this.busy, 2, 0) == 0)
			{
				this.Dispose();
				this.busy = 0;
				return;
			}
			this.invalidateWhenFree = true;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000A0A3 File Offset: 0x000090A3
		internal void ReleaseBuffer(BufferedGraphics buffer)
		{
			this.buffer = null;
			if (this.invalidateWhenFree)
			{
				this.busy = 2;
				this.Dispose();
			}
			else
			{
				this.busy = 2;
				this.DisposeDC();
			}
			this.busy = 0;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000A0D8 File Offset: 0x000090D8
		private bool ShouldUseTempManager(Rectangle targetBounds)
		{
			return targetBounds.Width * targetBounds.Height > this.MaximumBuffer.Width * this.MaximumBuffer.Height;
		}

		// Token: 0x0400025B RID: 603
		private const int BUFFER_FREE = 0;

		// Token: 0x0400025C RID: 604
		private const int BUFFER_BUSY_PAINTING = 1;

		// Token: 0x0400025D RID: 605
		private const int BUFFER_BUSY_DISPOSING = 2;

		// Token: 0x0400025E RID: 606
		private Size maximumBuffer;

		// Token: 0x0400025F RID: 607
		private Size bufferSize;

		// Token: 0x04000260 RID: 608
		private Size virtualSize;

		// Token: 0x04000261 RID: 609
		private Point targetLoc;

		// Token: 0x04000262 RID: 610
		private IntPtr compatDC;

		// Token: 0x04000263 RID: 611
		private IntPtr dib;

		// Token: 0x04000264 RID: 612
		private IntPtr oldBitmap;

		// Token: 0x04000265 RID: 613
		private Graphics compatGraphics;

		// Token: 0x04000266 RID: 614
		private BufferedGraphics buffer;

		// Token: 0x04000267 RID: 615
		private int busy;

		// Token: 0x04000268 RID: 616
		private bool invalidateWhenFree;

		// Token: 0x04000269 RID: 617
		private static TraceSwitch doubleBuffering;
	}
}
