using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Drawing.Internal;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Drawing
{
	// Token: 0x02000033 RID: 51
	[Editor("System.Drawing.Design.BitmapEditor, System.Drawing.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[ComVisible(true)]
	[Serializable]
	public sealed class Bitmap : Image
	{
		// Token: 0x0600017E RID: 382 RVA: 0x00005FAC File Offset: 0x00004FAC
		public Bitmap(string filename)
		{
			IntSecurity.DemandReadFileIO(filename);
			filename = Path.GetFullPath(filename);
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateBitmapFromFile(filename, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			num = SafeNativeMethods.Gdip.GdipImageForceValidation(new HandleRef(null, zero));
			if (num != 0)
			{
				SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef(null, zero));
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
			Image.EnsureSave(this, filename, null);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000601C File Offset: 0x0000501C
		public Bitmap(string filename, bool useIcm)
		{
			IntSecurity.DemandReadFileIO(filename);
			filename = Path.GetFullPath(filename);
			IntPtr zero = IntPtr.Zero;
			int num;
			if (useIcm)
			{
				num = SafeNativeMethods.Gdip.GdipCreateBitmapFromFileICM(filename, out zero);
			}
			else
			{
				num = SafeNativeMethods.Gdip.GdipCreateBitmapFromFile(filename, out zero);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			num = SafeNativeMethods.Gdip.GdipImageForceValidation(new HandleRef(null, zero));
			if (num != 0)
			{
				SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef(null, zero));
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
			Image.EnsureSave(this, filename, null);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00006098 File Offset: 0x00005098
		public Bitmap(Type type, string resource)
		{
			Stream manifestResourceStream = type.Module.Assembly.GetManifestResourceStream(type, resource);
			if (manifestResourceStream == null)
			{
				throw new ArgumentException(SR.GetString("ResourceNotFound", new object[] { type, resource }));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateBitmapFromStream(new GPStream(manifestResourceStream), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			num = SafeNativeMethods.Gdip.GdipImageForceValidation(new HandleRef(null, zero));
			if (num != 0)
			{
				SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef(null, zero));
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
			Image.EnsureSave(this, null, manifestResourceStream);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00006134 File Offset: 0x00005134
		public Bitmap(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[] { "stream", "null" }));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateBitmapFromStream(new GPStream(stream), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			num = SafeNativeMethods.Gdip.GdipImageForceValidation(new HandleRef(null, zero));
			if (num != 0)
			{
				SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef(null, zero));
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
			Image.EnsureSave(this, null, stream);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000061C4 File Offset: 0x000051C4
		public Bitmap(Stream stream, bool useIcm)
		{
			if (stream == null)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[] { "stream", "null" }));
			}
			IntPtr zero = IntPtr.Zero;
			int num;
			if (useIcm)
			{
				num = SafeNativeMethods.Gdip.GdipCreateBitmapFromStreamICM(new GPStream(stream), out zero);
			}
			else
			{
				num = SafeNativeMethods.Gdip.GdipCreateBitmapFromStream(new GPStream(stream), out zero);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			num = SafeNativeMethods.Gdip.GdipImageForceValidation(new HandleRef(null, zero));
			if (num != 0)
			{
				SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef(null, zero));
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
			Image.EnsureSave(this, null, stream);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006268 File Offset: 0x00005268
		public Bitmap(int width, int height, int stride, PixelFormat format, IntPtr scan0)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateBitmapFromScan0(width, height, stride, (int)format, new HandleRef(null, scan0), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000062B4 File Offset: 0x000052B4
		public Bitmap(int width, int height, PixelFormat format)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateBitmapFromScan0(width, height, 0, (int)format, NativeMethods.NullHandleRef, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000062EF File Offset: 0x000052EF
		public Bitmap(int width, int height)
			: this(width, height, PixelFormat.Format32bppArgb)
		{
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00006300 File Offset: 0x00005300
		public Bitmap(int width, int height, Graphics g)
		{
			if (g == null)
			{
				throw new ArgumentNullException(SR.GetString("InvalidArgument", new object[] { "g", "null" }));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateBitmapFromGraphics(width, height, new HandleRef(g, g.NativeGraphics), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000636B File Offset: 0x0000536B
		public Bitmap(Image original)
			: this(original, original.Width, original.Height)
		{
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00006380 File Offset: 0x00005380
		public Bitmap(Image original, int width, int height)
			: this(width, height)
		{
			Graphics graphics = null;
			try
			{
				graphics = Graphics.FromImage(this);
				graphics.Clear(Color.Transparent);
				graphics.DrawImage(original, 0, 0, width, height);
			}
			finally
			{
				if (graphics != null)
				{
					graphics.Dispose();
				}
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000063D0 File Offset: 0x000053D0
		private Bitmap(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000063DC File Offset: 0x000053DC
		public static Bitmap FromHicon(IntPtr hicon)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateBitmapFromHICON(new HandleRef(null, hicon), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return Bitmap.FromGDIplus(zero);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00006418 File Offset: 0x00005418
		public static Bitmap FromResource(IntPtr hinstance, string bitmapName)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr intPtr = Marshal.StringToHGlobalUni(bitmapName);
			IntPtr intPtr2;
			int num = SafeNativeMethods.Gdip.GdipCreateBitmapFromResource(new HandleRef(null, hinstance), new HandleRef(null, intPtr), out intPtr2);
			Marshal.FreeHGlobal(intPtr);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return Bitmap.FromGDIplus(intPtr2);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00006462 File Offset: 0x00005462
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public IntPtr GetHbitmap()
		{
			return this.GetHbitmap(Color.LightGray);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00006470 File Offset: 0x00005470
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public IntPtr GetHbitmap(Color background)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateHBITMAPFromBitmap(new HandleRef(this, this.nativeImage), out zero, ColorTranslator.ToWin32(background));
			if (num == 2 && (base.Width >= 32767 || base.Height >= 32767))
			{
				throw new ArgumentException(SR.GetString("GdiplusInvalidSize"));
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return zero;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000064D8 File Offset: 0x000054D8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public IntPtr GetHicon()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateHICONFromBitmap(new HandleRef(this, this.nativeImage), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return zero;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000650A File Offset: 0x0000550A
		public Bitmap(Image original, Size newSize)
			: this(original, (newSize != null) ? newSize.Width : 0, (newSize != null) ? newSize.Height : 0)
		{
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00006537 File Offset: 0x00005537
		private Bitmap()
		{
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00006540 File Offset: 0x00005540
		internal static Bitmap FromGDIplus(IntPtr handle)
		{
			Bitmap bitmap = new Bitmap();
			bitmap.SetNativeImage(handle);
			return bitmap;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000655C File Offset: 0x0000555C
		public Bitmap Clone(Rectangle rect, PixelFormat format)
		{
			if (rect.Width == 0 || rect.Height == 0)
			{
				throw new ArgumentException(SR.GetString("GdiplusInvalidRectangle", new object[] { rect.ToString() }));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneBitmapAreaI(rect.X, rect.Y, rect.Width, rect.Height, (int)format, new HandleRef(this, this.nativeImage), out zero);
			if (num != 0 || zero == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return Bitmap.FromGDIplus(zero);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000065F8 File Offset: 0x000055F8
		public Bitmap Clone(RectangleF rect, PixelFormat format)
		{
			if (rect.Width == 0f || rect.Height == 0f)
			{
				throw new ArgumentException(SR.GetString("GdiplusInvalidRectangle", new object[] { rect.ToString() }));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneBitmapArea(rect.X, rect.Y, rect.Width, rect.Height, (int)format, new HandleRef(this, this.nativeImage), out zero);
			if (num != 0 || zero == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return Bitmap.FromGDIplus(zero);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000066A0 File Offset: 0x000056A0
		public void MakeTransparent()
		{
			Color pixel = Bitmap.defaultTransparentColor;
			if (base.Height > 0 && base.Width > 0)
			{
				pixel = this.GetPixel(0, base.Size.Height - 1);
			}
			if (pixel.A < 255)
			{
				return;
			}
			this.MakeTransparent(pixel);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000066F4 File Offset: 0x000056F4
		public void MakeTransparent(Color transparentColor)
		{
			if (base.RawFormat.Guid == ImageFormat.Icon.Guid)
			{
				throw new InvalidOperationException(SR.GetString("CantMakeIconTransparent"));
			}
			Size size = base.Size;
			Bitmap bitmap = null;
			Graphics graphics = null;
			try
			{
				bitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
				try
				{
					graphics = Graphics.FromImage(bitmap);
					graphics.Clear(Color.Transparent);
					Rectangle rectangle = new Rectangle(0, 0, size.Width, size.Height);
					ImageAttributes imageAttributes = null;
					try
					{
						imageAttributes = new ImageAttributes();
						imageAttributes.SetColorKey(transparentColor, transparentColor);
						graphics.DrawImage(this, rectangle, 0, 0, size.Width, size.Height, GraphicsUnit.Pixel, imageAttributes, null, IntPtr.Zero);
					}
					finally
					{
						if (imageAttributes != null)
						{
							imageAttributes.Dispose();
						}
					}
				}
				finally
				{
					if (graphics != null)
					{
						graphics.Dispose();
					}
				}
				IntPtr nativeImage = this.nativeImage;
				this.nativeImage = bitmap.nativeImage;
				bitmap.nativeImage = nativeImage;
			}
			finally
			{
				if (bitmap != null)
				{
					bitmap.Dispose();
				}
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00006818 File Offset: 0x00005818
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public BitmapData LockBits(Rectangle rect, ImageLockMode flags, PixelFormat format)
		{
			BitmapData bitmapData = new BitmapData();
			return this.LockBits(rect, flags, format, bitmapData);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00006838 File Offset: 0x00005838
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public BitmapData LockBits(Rectangle rect, ImageLockMode flags, PixelFormat format, BitmapData bitmapData)
		{
			GPRECT gprect = new GPRECT(rect);
			int num = SafeNativeMethods.Gdip.GdipBitmapLockBits(new HandleRef(this, this.nativeImage), ref gprect, flags, format, bitmapData);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return bitmapData;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00006878 File Offset: 0x00005878
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public void UnlockBits(BitmapData bitmapdata)
		{
			int num = SafeNativeMethods.Gdip.GdipBitmapUnlockBits(new HandleRef(this, this.nativeImage), bitmapdata);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000068A4 File Offset: 0x000058A4
		public Color GetPixel(int x, int y)
		{
			int num = 0;
			if (x < 0 || x >= base.Width)
			{
				throw new ArgumentOutOfRangeException("x", SR.GetString("ValidRangeX"));
			}
			if (y < 0 || y >= base.Height)
			{
				throw new ArgumentOutOfRangeException("y", SR.GetString("ValidRangeY"));
			}
			int num2 = SafeNativeMethods.Gdip.GdipBitmapGetPixel(new HandleRef(this, this.nativeImage), x, y, out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return Color.FromArgb(num);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00006920 File Offset: 0x00005920
		public void SetPixel(int x, int y, Color color)
		{
			if ((base.PixelFormat & PixelFormat.Indexed) != PixelFormat.Undefined)
			{
				throw new InvalidOperationException(SR.GetString("GdiplusCannotSetPixelFromIndexedPixelFormat"));
			}
			if (x < 0 || x >= base.Width)
			{
				throw new ArgumentOutOfRangeException("x", SR.GetString("ValidRangeX"));
			}
			if (y < 0 || y >= base.Height)
			{
				throw new ArgumentOutOfRangeException("y", SR.GetString("ValidRangeY"));
			}
			int num = SafeNativeMethods.Gdip.GdipBitmapSetPixel(new HandleRef(this, this.nativeImage), x, y, color.ToArgb());
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x000069B4 File Offset: 0x000059B4
		public void SetResolution(float xDpi, float yDpi)
		{
			int num = SafeNativeMethods.Gdip.GdipBitmapSetResolution(new HandleRef(this, this.nativeImage), xDpi, yDpi);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x040001C4 RID: 452
		private static Color defaultTransparentColor = Color.LightGray;
	}
}
