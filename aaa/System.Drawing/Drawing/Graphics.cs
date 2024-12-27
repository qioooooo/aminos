using System;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Internal;
using System.Drawing.Text;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Drawing
{
	// Token: 0x02000044 RID: 68
	public sealed class Graphics : MarshalByRefObject, IDeviceContext, IDisposable
	{
		// Token: 0x0600033C RID: 828 RVA: 0x0000C43E File Offset: 0x0000B43E
		private Graphics(IntPtr gdipNativeGraphics)
		{
			if (gdipNativeGraphics == IntPtr.Zero)
			{
				throw new ArgumentNullException("gdipNativeGraphics");
			}
			this.nativeGraphics = gdipNativeGraphics;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000C465 File Offset: 0x0000B465
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Graphics FromHdc(IntPtr hdc)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			if (hdc == IntPtr.Zero)
			{
				throw new ArgumentNullException("hdc");
			}
			return Graphics.FromHdcInternal(hdc);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000C490 File Offset: 0x0000B490
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Graphics FromHdcInternal(IntPtr hdc)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateFromHDC(new HandleRef(null, hdc), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new Graphics(zero);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000C4C4 File Offset: 0x0000B4C4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Graphics FromHdc(IntPtr hdc, IntPtr hdevice)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateFromHDC2(new HandleRef(null, hdc), new HandleRef(null, hdevice), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new Graphics(zero);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000C507 File Offset: 0x0000B507
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Graphics FromHwnd(IntPtr hwnd)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			return Graphics.FromHwndInternal(hwnd);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000C51C File Offset: 0x0000B51C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Graphics FromHwndInternal(IntPtr hwnd)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateFromHWND(new HandleRef(null, hwnd), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new Graphics(zero);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000C550 File Offset: 0x0000B550
		public static Graphics FromImage(Image image)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			if ((image.PixelFormat & PixelFormat.Indexed) != PixelFormat.Undefined)
			{
				throw new Exception(SR.GetString("GdiplusCannotCreateGraphicsFromIndexedPixelFormat"));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipGetImageGraphicsContext(new HandleRef(image, image.nativeImage), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new Graphics(zero)
			{
				backingImage = image
			};
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000343 RID: 835 RVA: 0x0000C5BC File Offset: 0x0000B5BC
		internal IntPtr NativeGraphics
		{
			get
			{
				return this.nativeGraphics;
			}
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000C5C4 File Offset: 0x0000B5C4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public IntPtr GetHdc()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipGetDC(new HandleRef(this, this.NativeGraphics), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.nativeHdc = zero;
			return this.nativeHdc;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000C602 File Offset: 0x0000B602
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void ReleaseHdc(IntPtr hdc)
		{
			IntSecurity.Win32HandleManipulation.Demand();
			this.ReleaseHdcInternal(hdc);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000C615 File Offset: 0x0000B615
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public void ReleaseHdc()
		{
			this.ReleaseHdcInternal(this.nativeHdc);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000C624 File Offset: 0x0000B624
		[EditorBrowsable(EditorBrowsableState.Never)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public void ReleaseHdcInternal(IntPtr hdc)
		{
			int num = SafeNativeMethods.Gdip.GdipReleaseDC(new HandleRef(this, this.NativeGraphics), new HandleRef(null, hdc));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.nativeHdc = IntPtr.Zero;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000C65F File Offset: 0x0000B65F
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000C670 File Offset: 0x0000B670
		private void Dispose(bool disposing)
		{
			while (this.previousContext != null)
			{
				GraphicsContext previous = this.previousContext.Previous;
				this.previousContext.Dispose();
				this.previousContext = previous;
			}
			if (this.nativeGraphics != IntPtr.Zero)
			{
				try
				{
					if (this.nativeHdc != IntPtr.Zero)
					{
						this.ReleaseHdc();
					}
					if (this.PrintingHelper != null)
					{
						DeviceContext deviceContext = this.PrintingHelper as DeviceContext;
						if (deviceContext != null)
						{
							deviceContext.Dispose();
							this.printingHelper = null;
						}
					}
					SafeNativeMethods.Gdip.GdipDeleteGraphics(new HandleRef(this, this.nativeGraphics));
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
					this.nativeGraphics = IntPtr.Zero;
				}
			}
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000C740 File Offset: 0x0000B740
		~Graphics()
		{
			this.Dispose(false);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000C770 File Offset: 0x0000B770
		public void Flush()
		{
			this.Flush(FlushIntention.Flush);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000C77C File Offset: 0x0000B77C
		public void Flush(FlushIntention intention)
		{
			int num = SafeNativeMethods.Gdip.GdipFlush(new HandleRef(this, this.NativeGraphics), intention);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600034D RID: 845 RVA: 0x0000C7A8 File Offset: 0x0000B7A8
		// (set) Token: 0x0600034E RID: 846 RVA: 0x0000C7D8 File Offset: 0x0000B7D8
		public CompositingMode CompositingMode
		{
			get
			{
				int num = 0;
				int num2 = SafeNativeMethods.Gdip.GdipGetCompositingMode(new HandleRef(this, this.NativeGraphics), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return (CompositingMode)num;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(CompositingMode));
				}
				int num = SafeNativeMethods.Gdip.GdipSetCompositingMode(new HandleRef(this, this.NativeGraphics), (int)value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600034F RID: 847 RVA: 0x0000C828 File Offset: 0x0000B828
		// (set) Token: 0x06000350 RID: 848 RVA: 0x0000C85C File Offset: 0x0000B85C
		public Point RenderingOrigin
		{
			get
			{
				int num2;
				int num3;
				int num = SafeNativeMethods.Gdip.GdipGetRenderingOrigin(new HandleRef(this, this.NativeGraphics), out num2, out num3);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return new Point(num2, num3);
			}
			set
			{
				int num = SafeNativeMethods.Gdip.GdipSetRenderingOrigin(new HandleRef(this, this.NativeGraphics), value.X, value.Y);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000351 RID: 849 RVA: 0x0000C894 File Offset: 0x0000B894
		// (set) Token: 0x06000352 RID: 850 RVA: 0x0000C8C0 File Offset: 0x0000B8C0
		public CompositingQuality CompositingQuality
		{
			get
			{
				CompositingQuality compositingQuality;
				int num = SafeNativeMethods.Gdip.GdipGetCompositingQuality(new HandleRef(this, this.NativeGraphics), out compositingQuality);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return compositingQuality;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, -1, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(CompositingQuality));
				}
				int num = SafeNativeMethods.Gdip.GdipSetCompositingQuality(new HandleRef(this, this.NativeGraphics), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000353 RID: 851 RVA: 0x0000C910 File Offset: 0x0000B910
		// (set) Token: 0x06000354 RID: 852 RVA: 0x0000C940 File Offset: 0x0000B940
		public TextRenderingHint TextRenderingHint
		{
			get
			{
				TextRenderingHint textRenderingHint = TextRenderingHint.SystemDefault;
				int num = SafeNativeMethods.Gdip.GdipGetTextRenderingHint(new HandleRef(this, this.NativeGraphics), out textRenderingHint);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return textRenderingHint;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 5))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(TextRenderingHint));
				}
				int num = SafeNativeMethods.Gdip.GdipSetTextRenderingHint(new HandleRef(this, this.NativeGraphics), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0000C990 File Offset: 0x0000B990
		// (set) Token: 0x06000356 RID: 854 RVA: 0x0000C9C0 File Offset: 0x0000B9C0
		public int TextContrast
		{
			get
			{
				int num = 0;
				int num2 = SafeNativeMethods.Gdip.GdipGetTextContrast(new HandleRef(this, this.NativeGraphics), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return num;
			}
			set
			{
				int num = SafeNativeMethods.Gdip.GdipSetTextContrast(new HandleRef(this, this.NativeGraphics), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000357 RID: 855 RVA: 0x0000C9EC File Offset: 0x0000B9EC
		// (set) Token: 0x06000358 RID: 856 RVA: 0x0000CA1C File Offset: 0x0000BA1C
		public SmoothingMode SmoothingMode
		{
			get
			{
				SmoothingMode smoothingMode = SmoothingMode.Default;
				int num = SafeNativeMethods.Gdip.GdipGetSmoothingMode(new HandleRef(this, this.NativeGraphics), out smoothingMode);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return smoothingMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, -1, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(SmoothingMode));
				}
				int num = SafeNativeMethods.Gdip.GdipSetSmoothingMode(new HandleRef(this, this.NativeGraphics), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000CA6C File Offset: 0x0000BA6C
		// (set) Token: 0x0600035A RID: 858 RVA: 0x0000CA9C File Offset: 0x0000BA9C
		public PixelOffsetMode PixelOffsetMode
		{
			get
			{
				PixelOffsetMode pixelOffsetMode = PixelOffsetMode.Default;
				int num = SafeNativeMethods.Gdip.GdipGetPixelOffsetMode(new HandleRef(this, this.NativeGraphics), out pixelOffsetMode);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return pixelOffsetMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, -1, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(PixelOffsetMode));
				}
				int num = SafeNativeMethods.Gdip.GdipSetPixelOffsetMode(new HandleRef(this, this.NativeGraphics), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000CAEC File Offset: 0x0000BAEC
		// (set) Token: 0x0600035C RID: 860 RVA: 0x0000CAF4 File Offset: 0x0000BAF4
		internal object PrintingHelper
		{
			get
			{
				return this.printingHelper;
			}
			set
			{
				this.printingHelper = value;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0000CB00 File Offset: 0x0000BB00
		// (set) Token: 0x0600035E RID: 862 RVA: 0x0000CB30 File Offset: 0x0000BB30
		public InterpolationMode InterpolationMode
		{
			get
			{
				int num = 0;
				int num2 = SafeNativeMethods.Gdip.GdipGetInterpolationMode(new HandleRef(this, this.NativeGraphics), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return (InterpolationMode)num;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, -1, 7))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(InterpolationMode));
				}
				int num = SafeNativeMethods.Gdip.GdipSetInterpolationMode(new HandleRef(this, this.NativeGraphics), (int)value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600035F RID: 863 RVA: 0x0000CB80 File Offset: 0x0000BB80
		// (set) Token: 0x06000360 RID: 864 RVA: 0x0000CBBC File Offset: 0x0000BBBC
		public Matrix Transform
		{
			get
			{
				Matrix matrix = new Matrix();
				int num = SafeNativeMethods.Gdip.GdipGetWorldTransform(new HandleRef(this, this.NativeGraphics), new HandleRef(matrix, matrix.nativeMatrix));
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return matrix;
			}
			set
			{
				int num = SafeNativeMethods.Gdip.GdipSetWorldTransform(new HandleRef(this, this.NativeGraphics), new HandleRef(value, value.nativeMatrix));
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000361 RID: 865 RVA: 0x0000CBF4 File Offset: 0x0000BBF4
		// (set) Token: 0x06000362 RID: 866 RVA: 0x0000CC24 File Offset: 0x0000BC24
		public GraphicsUnit PageUnit
		{
			get
			{
				int num = 0;
				int num2 = SafeNativeMethods.Gdip.GdipGetPageUnit(new HandleRef(this, this.NativeGraphics), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return (GraphicsUnit)num;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 6))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(GraphicsUnit));
				}
				int num = SafeNativeMethods.Gdip.GdipSetPageUnit(new HandleRef(this, this.NativeGraphics), (int)value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000363 RID: 867 RVA: 0x0000CC74 File Offset: 0x0000BC74
		// (set) Token: 0x06000364 RID: 868 RVA: 0x0000CCAC File Offset: 0x0000BCAC
		public float PageScale
		{
			get
			{
				float[] array = new float[1];
				float[] array2 = array;
				int num = SafeNativeMethods.Gdip.GdipGetPageScale(new HandleRef(this, this.NativeGraphics), array2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return array2[0];
			}
			set
			{
				int num = SafeNativeMethods.Gdip.GdipSetPageScale(new HandleRef(this, this.NativeGraphics), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000365 RID: 869 RVA: 0x0000CCD8 File Offset: 0x0000BCD8
		public float DpiX
		{
			get
			{
				float[] array = new float[1];
				float[] array2 = array;
				int num = SafeNativeMethods.Gdip.GdipGetDpiX(new HandleRef(this, this.NativeGraphics), array2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return array2[0];
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000366 RID: 870 RVA: 0x0000CD10 File Offset: 0x0000BD10
		public float DpiY
		{
			get
			{
				float[] array = new float[1];
				float[] array2 = array;
				int num = SafeNativeMethods.Gdip.GdipGetDpiY(new HandleRef(this, this.NativeGraphics), array2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return array2[0];
			}
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000CD46 File Offset: 0x0000BD46
		public void CopyFromScreen(Point upperLeftSource, Point upperLeftDestination, Size blockRegionSize)
		{
			this.CopyFromScreen(upperLeftSource.X, upperLeftSource.Y, upperLeftDestination.X, upperLeftDestination.Y, blockRegionSize);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000CD6B File Offset: 0x0000BD6B
		public void CopyFromScreen(int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize)
		{
			this.CopyFromScreen(sourceX, sourceY, destinationX, destinationY, blockRegionSize, CopyPixelOperation.SourceCopy);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000CD7F File Offset: 0x0000BD7F
		public void CopyFromScreen(Point upperLeftSource, Point upperLeftDestination, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
		{
			this.CopyFromScreen(upperLeftSource.X, upperLeftSource.Y, upperLeftDestination.X, upperLeftDestination.Y, blockRegionSize, copyPixelOperation);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000CDA8 File Offset: 0x0000BDA8
		public void CopyFromScreen(int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
		{
			if (copyPixelOperation <= CopyPixelOperation.SourceInvert)
			{
				if (copyPixelOperation <= CopyPixelOperation.NotSourceCopy)
				{
					if (copyPixelOperation <= CopyPixelOperation.Blackness)
					{
						if (copyPixelOperation == CopyPixelOperation.NoMirrorBitmap || copyPixelOperation == CopyPixelOperation.Blackness)
						{
							goto IL_011F;
						}
					}
					else if (copyPixelOperation == CopyPixelOperation.NotSourceErase || copyPixelOperation == CopyPixelOperation.NotSourceCopy)
					{
						goto IL_011F;
					}
				}
				else if (copyPixelOperation <= CopyPixelOperation.DestinationInvert)
				{
					if (copyPixelOperation == CopyPixelOperation.SourceErase || copyPixelOperation == CopyPixelOperation.DestinationInvert)
					{
						goto IL_011F;
					}
				}
				else if (copyPixelOperation == CopyPixelOperation.PatInvert || copyPixelOperation == CopyPixelOperation.SourceInvert)
				{
					goto IL_011F;
				}
			}
			else if (copyPixelOperation <= CopyPixelOperation.SourceCopy)
			{
				if (copyPixelOperation <= CopyPixelOperation.MergePaint)
				{
					if (copyPixelOperation == CopyPixelOperation.SourceAnd || copyPixelOperation == CopyPixelOperation.MergePaint)
					{
						goto IL_011F;
					}
				}
				else if (copyPixelOperation == CopyPixelOperation.MergeCopy || copyPixelOperation == CopyPixelOperation.SourceCopy)
				{
					goto IL_011F;
				}
			}
			else if (copyPixelOperation <= CopyPixelOperation.PatCopy)
			{
				if (copyPixelOperation == CopyPixelOperation.SourcePaint || copyPixelOperation == CopyPixelOperation.PatCopy)
				{
					goto IL_011F;
				}
			}
			else if (copyPixelOperation == CopyPixelOperation.PatPaint || copyPixelOperation == CopyPixelOperation.Whiteness || copyPixelOperation == CopyPixelOperation.CaptureBlt)
			{
				goto IL_011F;
			}
			throw new InvalidEnumArgumentException("value", (int)copyPixelOperation, typeof(CopyPixelOperation));
			IL_011F:
			new UIPermission(UIPermissionWindow.AllWindows).Demand();
			int width = blockRegionSize.Width;
			int height = blockRegionSize.Height;
			using (DeviceContext deviceContext = DeviceContext.FromHwnd(IntPtr.Zero))
			{
				HandleRef handleRef = new HandleRef(null, deviceContext.Hdc);
				HandleRef handleRef2 = new HandleRef(null, this.GetHdc());
				try
				{
					if (SafeNativeMethods.BitBlt(handleRef2, destinationX, destinationY, width, height, handleRef, sourceX, sourceY, (int)copyPixelOperation) == 0)
					{
						throw new Win32Exception();
					}
				}
				finally
				{
					this.ReleaseHdc();
				}
			}
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000CF64 File Offset: 0x0000BF64
		public void ResetTransform()
		{
			int num = SafeNativeMethods.Gdip.GdipResetWorldTransform(new HandleRef(this, this.NativeGraphics));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000CF8D File Offset: 0x0000BF8D
		public void MultiplyTransform(Matrix matrix)
		{
			this.MultiplyTransform(matrix, MatrixOrder.Prepend);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000CF98 File Offset: 0x0000BF98
		public void MultiplyTransform(Matrix matrix, MatrixOrder order)
		{
			if (matrix == null)
			{
				throw new ArgumentNullException("matrix");
			}
			int num = SafeNativeMethods.Gdip.GdipMultiplyWorldTransform(new HandleRef(this, this.NativeGraphics), new HandleRef(matrix, matrix.nativeMatrix), order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000CFDC File Offset: 0x0000BFDC
		public void TranslateTransform(float dx, float dy)
		{
			this.TranslateTransform(dx, dy, MatrixOrder.Prepend);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000CFE8 File Offset: 0x0000BFE8
		public void TranslateTransform(float dx, float dy, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipTranslateWorldTransform(new HandleRef(this, this.NativeGraphics), dx, dy, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000D014 File Offset: 0x0000C014
		public void ScaleTransform(float sx, float sy)
		{
			this.ScaleTransform(sx, sy, MatrixOrder.Prepend);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000D020 File Offset: 0x0000C020
		public void ScaleTransform(float sx, float sy, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipScaleWorldTransform(new HandleRef(this, this.NativeGraphics), sx, sy, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000D04C File Offset: 0x0000C04C
		public void RotateTransform(float angle)
		{
			this.RotateTransform(angle, MatrixOrder.Prepend);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000D058 File Offset: 0x0000C058
		public void RotateTransform(float angle, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipRotateWorldTransform(new HandleRef(this, this.NativeGraphics), angle, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000D084 File Offset: 0x0000C084
		public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, PointF[] pts)
		{
			if (pts == null)
			{
				throw new ArgumentNullException("pts");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(pts);
			int num = SafeNativeMethods.Gdip.GdipTransformPoints(new HandleRef(this, this.NativeGraphics), (int)destSpace, (int)srcSpace, intPtr, pts.Length);
			try
			{
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				PointF[] array = SafeNativeMethods.Gdip.ConvertGPPOINTFArrayF(intPtr, pts.Length);
				for (int i = 0; i < pts.Length; i++)
				{
					pts[i] = array[i];
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000D114 File Offset: 0x0000C114
		public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, Point[] pts)
		{
			if (pts == null)
			{
				throw new ArgumentNullException("pts");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(pts);
			int num = SafeNativeMethods.Gdip.GdipTransformPointsI(new HandleRef(this, this.NativeGraphics), (int)destSpace, (int)srcSpace, intPtr, pts.Length);
			try
			{
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				Point[] array = SafeNativeMethods.Gdip.ConvertGPPOINTArray(intPtr, pts.Length);
				for (int i = 0; i < pts.Length; i++)
				{
					pts[i] = array[i];
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000D1A4 File Offset: 0x0000C1A4
		public Color GetNearestColor(Color color)
		{
			int num = color.ToArgb();
			int num2 = SafeNativeMethods.Gdip.GdipGetNearestColor(new HandleRef(this, this.NativeGraphics), ref num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return Color.FromArgb(num);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000D1E0 File Offset: 0x0000C1E0
		public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawLine(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x1, y1, x2, y2);
			this.CheckErrorStatus(num);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000D226 File Offset: 0x0000C226
		public void DrawLine(Pen pen, PointF pt1, PointF pt2)
		{
			this.DrawLine(pen, pt1.X, pt1.Y, pt2.X, pt2.Y);
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000D24C File Offset: 0x0000C24C
		public void DrawLines(Pen pen, PointF[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipDrawLines(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000D2B4 File Offset: 0x0000C2B4
		public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawLineI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x1, y1, x2, y2);
			this.CheckErrorStatus(num);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000D2FA File Offset: 0x0000C2FA
		public void DrawLine(Pen pen, Point pt1, Point pt2)
		{
			this.DrawLine(pen, pt1.X, pt1.Y, pt2.X, pt2.Y);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000D320 File Offset: 0x0000C320
		public void DrawLines(Pen pen, Point[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipDrawLinesI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000D388 File Offset: 0x0000C388
		public void DrawArc(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawArc(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x, y, width, height, startAngle, sweepAngle);
			this.CheckErrorStatus(num);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000D3D2 File Offset: 0x0000C3D2
		public void DrawArc(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
		{
			this.DrawArc(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000D3FC File Offset: 0x0000C3FC
		public void DrawArc(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawArcI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x, y, width, height, (float)startAngle, (float)sweepAngle);
			this.CheckErrorStatus(num);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000D448 File Offset: 0x0000C448
		public void DrawArc(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
		{
			this.DrawArc(pen, (float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height, startAngle, sweepAngle);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000D474 File Offset: 0x0000C474
		public void DrawBezier(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawBezier(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x1, y1, x2, y2, x3, y3, x4, y4);
			this.CheckErrorStatus(num);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000D4C4 File Offset: 0x0000C4C4
		public void DrawBezier(Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4)
		{
			this.DrawBezier(pen, pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000D510 File Offset: 0x0000C510
		public void DrawBeziers(Pen pen, PointF[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipDrawBeziers(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000D578 File Offset: 0x0000C578
		public void DrawBezier(Pen pen, Point pt1, Point pt2, Point pt3, Point pt4)
		{
			this.DrawBezier(pen, (float)pt1.X, (float)pt1.Y, (float)pt2.X, (float)pt2.Y, (float)pt3.X, (float)pt3.Y, (float)pt4.X, (float)pt4.Y);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000D5CC File Offset: 0x0000C5CC
		public void DrawBeziers(Pen pen, Point[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipDrawBeziersI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000D631 File Offset: 0x0000C631
		public void DrawRectangle(Pen pen, Rectangle rect)
		{
			this.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000D658 File Offset: 0x0000C658
		public void DrawRectangle(Pen pen, float x, float y, float width, float height)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawRectangle(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x, y, width, height);
			this.CheckErrorStatus(num);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000D6A0 File Offset: 0x0000C6A0
		public void DrawRectangle(Pen pen, int x, int y, int width, int height)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawRectangleI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x, y, width, height);
			this.CheckErrorStatus(num);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000D6E8 File Offset: 0x0000C6E8
		public void DrawRectangles(Pen pen, RectangleF[] rects)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (rects == null)
			{
				throw new ArgumentNullException("rects");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertRectangleToMemory(rects);
			int num = SafeNativeMethods.Gdip.GdipDrawRectangles(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), rects.Length);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000D750 File Offset: 0x0000C750
		public void DrawRectangles(Pen pen, Rectangle[] rects)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (rects == null)
			{
				throw new ArgumentNullException("rects");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertRectangleToMemory(rects);
			int num = SafeNativeMethods.Gdip.GdipDrawRectanglesI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), rects.Length);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000D7B5 File Offset: 0x0000C7B5
		public void DrawEllipse(Pen pen, RectangleF rect)
		{
			this.DrawEllipse(pen, rect.X, rect.Y, rect.Width, rect.Height);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000D7DC File Offset: 0x0000C7DC
		public void DrawEllipse(Pen pen, float x, float y, float width, float height)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawEllipse(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x, y, width, height);
			this.CheckErrorStatus(num);
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000D822 File Offset: 0x0000C822
		public void DrawEllipse(Pen pen, Rectangle rect)
		{
			this.DrawEllipse(pen, rect.X, rect.Y, rect.Width, rect.Height);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000D848 File Offset: 0x0000C848
		public void DrawEllipse(Pen pen, int x, int y, int width, int height)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawEllipseI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x, y, width, height);
			this.CheckErrorStatus(num);
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000D88E File Offset: 0x0000C88E
		public void DrawPie(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
		{
			this.DrawPie(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000D8B8 File Offset: 0x0000C8B8
		public void DrawPie(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawPie(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x, y, width, height, startAngle, sweepAngle);
			this.CheckErrorStatus(num);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000D902 File Offset: 0x0000C902
		public void DrawPie(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
		{
			this.DrawPie(pen, (float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height, startAngle, sweepAngle);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000D930 File Offset: 0x0000C930
		public void DrawPie(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawPieI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x, y, width, height, (float)startAngle, (float)sweepAngle);
			this.CheckErrorStatus(num);
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000D97C File Offset: 0x0000C97C
		public void DrawPolygon(Pen pen, PointF[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipDrawPolygon(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000D9E4 File Offset: 0x0000C9E4
		public void DrawPolygon(Pen pen, Point[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipDrawPolygonI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000DA4C File Offset: 0x0000CA4C
		public void DrawPath(Pen pen, GraphicsPath path)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawPath(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(path, path.nativePath));
			this.CheckErrorStatus(num);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000DAA8 File Offset: 0x0000CAA8
		public void DrawCurve(Pen pen, PointF[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipDrawCurve(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000DB10 File Offset: 0x0000CB10
		public void DrawCurve(Pen pen, PointF[] points, float tension)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipDrawCurve2(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length, tension);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000DB76 File Offset: 0x0000CB76
		public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments)
		{
			this.DrawCurve(pen, points, offset, numberOfSegments, 0.5f);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000DB88 File Offset: 0x0000CB88
		public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments, float tension)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipDrawCurve3(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length, offset, numberOfSegments, tension);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000DBF4 File Offset: 0x0000CBF4
		public void DrawCurve(Pen pen, Point[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipDrawCurveI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000DC5C File Offset: 0x0000CC5C
		public void DrawCurve(Pen pen, Point[] points, float tension)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipDrawCurve2I(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length, tension);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000DCC4 File Offset: 0x0000CCC4
		public void DrawCurve(Pen pen, Point[] points, int offset, int numberOfSegments, float tension)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipDrawCurve3I(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length, offset, numberOfSegments, tension);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000DD30 File Offset: 0x0000CD30
		public void DrawClosedCurve(Pen pen, PointF[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipDrawClosedCurve(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000DD98 File Offset: 0x0000CD98
		public void DrawClosedCurve(Pen pen, PointF[] points, float tension, FillMode fillmode)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipDrawClosedCurve2(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length, tension);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000DE00 File Offset: 0x0000CE00
		public void DrawClosedCurve(Pen pen, Point[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipDrawClosedCurveI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000DE68 File Offset: 0x0000CE68
		public void DrawClosedCurve(Pen pen, Point[] points, float tension, FillMode fillmode)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipDrawClosedCurve2I(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length, tension);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000DED0 File Offset: 0x0000CED0
		public void Clear(Color color)
		{
			int num = SafeNativeMethods.Gdip.GdipGraphicsClear(new HandleRef(this, this.NativeGraphics), color.ToArgb());
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000DF00 File Offset: 0x0000CF00
		public void FillRectangle(Brush brush, RectangleF rect)
		{
			this.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000DF28 File Offset: 0x0000CF28
		public void FillRectangle(Brush brush, float x, float y, float width, float height)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			int num = SafeNativeMethods.Gdip.GdipFillRectangle(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), x, y, width, height);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000DF6E File Offset: 0x0000CF6E
		public void FillRectangle(Brush brush, Rectangle rect)
		{
			this.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000DF94 File Offset: 0x0000CF94
		public void FillRectangle(Brush brush, int x, int y, int width, int height)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			int num = SafeNativeMethods.Gdip.GdipFillRectangleI(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), x, y, width, height);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000DFDC File Offset: 0x0000CFDC
		public void FillRectangles(Brush brush, RectangleF[] rects)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (rects == null)
			{
				throw new ArgumentNullException("rects");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertRectangleToMemory(rects);
			int num = SafeNativeMethods.Gdip.GdipFillRectangles(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(this, intPtr), rects.Length);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000E044 File Offset: 0x0000D044
		public void FillRectangles(Brush brush, Rectangle[] rects)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (rects == null)
			{
				throw new ArgumentNullException("rects");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertRectangleToMemory(rects);
			int num = SafeNativeMethods.Gdip.GdipFillRectanglesI(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(this, intPtr), rects.Length);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000E0A9 File Offset: 0x0000D0A9
		public void FillPolygon(Brush brush, PointF[] points)
		{
			this.FillPolygon(brush, points, FillMode.Alternate);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000E0B4 File Offset: 0x0000D0B4
		public void FillPolygon(Brush brush, PointF[] points, FillMode fillMode)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipFillPolygon(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(this, intPtr), points.Length, (int)fillMode);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000E11A File Offset: 0x0000D11A
		public void FillPolygon(Brush brush, Point[] points)
		{
			this.FillPolygon(brush, points, FillMode.Alternate);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000E128 File Offset: 0x0000D128
		public void FillPolygon(Brush brush, Point[] points, FillMode fillMode)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipFillPolygonI(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(this, intPtr), points.Length, (int)fillMode);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000E18E File Offset: 0x0000D18E
		public void FillEllipse(Brush brush, RectangleF rect)
		{
			this.FillEllipse(brush, rect.X, rect.Y, rect.Width, rect.Height);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000E1B4 File Offset: 0x0000D1B4
		public void FillEllipse(Brush brush, float x, float y, float width, float height)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			int num = SafeNativeMethods.Gdip.GdipFillEllipse(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), x, y, width, height);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000E1FA File Offset: 0x0000D1FA
		public void FillEllipse(Brush brush, Rectangle rect)
		{
			this.FillEllipse(brush, rect.X, rect.Y, rect.Width, rect.Height);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000E220 File Offset: 0x0000D220
		public void FillEllipse(Brush brush, int x, int y, int width, int height)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			int num = SafeNativeMethods.Gdip.GdipFillEllipseI(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), x, y, width, height);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000E266 File Offset: 0x0000D266
		public void FillPie(Brush brush, Rectangle rect, float startAngle, float sweepAngle)
		{
			this.FillPie(brush, (float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height, startAngle, sweepAngle);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000E294 File Offset: 0x0000D294
		public void FillPie(Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			int num = SafeNativeMethods.Gdip.GdipFillPie(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), x, y, width, height, startAngle, sweepAngle);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000E2E0 File Offset: 0x0000D2E0
		public void FillPie(Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			int num = SafeNativeMethods.Gdip.GdipFillPieI(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), x, y, width, height, (float)startAngle, (float)sweepAngle);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000E32C File Offset: 0x0000D32C
		public void FillPath(Brush brush, GraphicsPath path)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			int num = SafeNativeMethods.Gdip.GdipFillPath(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(path, path.nativePath));
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000E388 File Offset: 0x0000D388
		public void FillClosedCurve(Brush brush, PointF[] points)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipFillClosedCurve(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(this, intPtr), points.Length);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000E3ED File Offset: 0x0000D3ED
		public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode)
		{
			this.FillClosedCurve(brush, points, fillmode, 0.5f);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000E400 File Offset: 0x0000D400
		public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode, float tension)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipFillClosedCurve2(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(this, intPtr), points.Length, tension, (int)fillmode);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000E468 File Offset: 0x0000D468
		public void FillClosedCurve(Brush brush, Point[] points)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipFillClosedCurveI(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(this, intPtr), points.Length);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000E4CD File Offset: 0x0000D4CD
		public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode)
		{
			this.FillClosedCurve(brush, points, fillmode, 0.5f);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000E4E0 File Offset: 0x0000D4E0
		public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode, float tension)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			int num = SafeNativeMethods.Gdip.GdipFillClosedCurve2I(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(this, intPtr), points.Length, tension, (int)fillmode);
			Marshal.FreeHGlobal(intPtr);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000E548 File Offset: 0x0000D548
		public void FillRegion(Brush brush, Region region)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			int num = SafeNativeMethods.Gdip.GdipFillRegion(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(region, region.nativeRegion));
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000E5A4 File Offset: 0x0000D5A4
		public void DrawString(string s, Font font, Brush brush, float x, float y)
		{
			this.DrawString(s, font, brush, new RectangleF(x, y, 0f, 0f), null);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000E5D0 File Offset: 0x0000D5D0
		public void DrawString(string s, Font font, Brush brush, PointF point)
		{
			this.DrawString(s, font, brush, new RectangleF(point.X, point.Y, 0f, 0f), null);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000E604 File Offset: 0x0000D604
		public void DrawString(string s, Font font, Brush brush, float x, float y, StringFormat format)
		{
			this.DrawString(s, font, brush, new RectangleF(x, y, 0f, 0f), format);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000E630 File Offset: 0x0000D630
		public void DrawString(string s, Font font, Brush brush, PointF point, StringFormat format)
		{
			this.DrawString(s, font, brush, new RectangleF(point.X, point.Y, 0f, 0f), format);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000E665 File Offset: 0x0000D665
		public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle)
		{
			this.DrawString(s, font, brush, layoutRectangle, null);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000E674 File Offset: 0x0000D674
		public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (s == null || s.Length == 0)
			{
				return;
			}
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			GPRECTF gprectf = new GPRECTF(layoutRectangle);
			IntPtr intPtr = ((format == null) ? IntPtr.Zero : format.nativeFormat);
			int num = SafeNativeMethods.Gdip.GdipDrawString(new HandleRef(this, this.NativeGraphics), s, s.Length, new HandleRef(font, font.NativeFont), ref gprectf, new HandleRef(format, intPtr), new HandleRef(brush, brush.NativeBrush));
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000E70C File Offset: 0x0000D70C
		public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat, out int charactersFitted, out int linesFilled)
		{
			if (text == null || text.Length == 0)
			{
				charactersFitted = 0;
				linesFilled = 0;
				return new SizeF(0f, 0f);
			}
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			GPRECTF gprectf = new GPRECTF(0f, 0f, layoutArea.Width, layoutArea.Height);
			GPRECTF gprectf2 = default(GPRECTF);
			int num = SafeNativeMethods.Gdip.GdipMeasureString(new HandleRef(this, this.NativeGraphics), text, text.Length, new HandleRef(font, font.NativeFont), ref gprectf, new HandleRef(stringFormat, (stringFormat == null) ? IntPtr.Zero : stringFormat.nativeFormat), ref gprectf2, out charactersFitted, out linesFilled);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return gprectf2.SizeF;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000E7D0 File Offset: 0x0000D7D0
		public SizeF MeasureString(string text, Font font, PointF origin, StringFormat stringFormat)
		{
			if (text == null || text.Length == 0)
			{
				return new SizeF(0f, 0f);
			}
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			GPRECTF gprectf = default(GPRECTF);
			GPRECTF gprectf2 = default(GPRECTF);
			gprectf.X = origin.X;
			gprectf.Y = origin.Y;
			gprectf.Width = 0f;
			gprectf.Height = 0f;
			int num2;
			int num3;
			int num = SafeNativeMethods.Gdip.GdipMeasureString(new HandleRef(this, this.NativeGraphics), text, text.Length, new HandleRef(font, font.NativeFont), ref gprectf, new HandleRef(stringFormat, (stringFormat == null) ? IntPtr.Zero : stringFormat.nativeFormat), ref gprectf2, out num2, out num3);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return gprectf2.SizeF;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000E8A4 File Offset: 0x0000D8A4
		public SizeF MeasureString(string text, Font font, SizeF layoutArea)
		{
			return this.MeasureString(text, font, layoutArea, null);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000E8B0 File Offset: 0x0000D8B0
		public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat)
		{
			if (text == null || text.Length == 0)
			{
				return new SizeF(0f, 0f);
			}
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			GPRECTF gprectf = new GPRECTF(0f, 0f, layoutArea.Width, layoutArea.Height);
			GPRECTF gprectf2 = default(GPRECTF);
			int num2;
			int num3;
			int num = SafeNativeMethods.Gdip.GdipMeasureString(new HandleRef(this, this.NativeGraphics), text, text.Length, new HandleRef(font, font.NativeFont), ref gprectf, new HandleRef(stringFormat, (stringFormat == null) ? IntPtr.Zero : stringFormat.nativeFormat), ref gprectf2, out num2, out num3);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return gprectf2.SizeF;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000E96C File Offset: 0x0000D96C
		public SizeF MeasureString(string text, Font font)
		{
			return this.MeasureString(text, font, new SizeF(0f, 0f));
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000E985 File Offset: 0x0000D985
		public SizeF MeasureString(string text, Font font, int width)
		{
			return this.MeasureString(text, font, new SizeF((float)width, 999999f));
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000E99B File Offset: 0x0000D99B
		public SizeF MeasureString(string text, Font font, int width, StringFormat format)
		{
			return this.MeasureString(text, font, new SizeF((float)width, 999999f), format);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000E9B4 File Offset: 0x0000D9B4
		public Region[] MeasureCharacterRanges(string text, Font font, RectangleF layoutRect, StringFormat stringFormat)
		{
			if (text == null || text.Length == 0)
			{
				return new Region[0];
			}
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			int num2;
			int num = SafeNativeMethods.Gdip.GdipGetStringFormatMeasurableCharacterRangeCount(new HandleRef(stringFormat, (stringFormat == null) ? IntPtr.Zero : stringFormat.nativeFormat), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			IntPtr[] array = new IntPtr[num2];
			GPRECTF gprectf = new GPRECTF(layoutRect);
			Region[] array2 = new Region[num2];
			for (int i = 0; i < num2; i++)
			{
				array2[i] = new Region();
				array[i] = array2[i].nativeRegion;
			}
			num = SafeNativeMethods.Gdip.GdipMeasureCharacterRanges(new HandleRef(this, this.NativeGraphics), text, text.Length, new HandleRef(font, font.NativeFont), ref gprectf, new HandleRef(stringFormat, (stringFormat == null) ? IntPtr.Zero : stringFormat.nativeFormat), num2, array);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return array2;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000EAA7 File Offset: 0x0000DAA7
		public void DrawIcon(Icon icon, int x, int y)
		{
			if (icon == null)
			{
				throw new ArgumentNullException("icon");
			}
			if (this.backingImage != null)
			{
				this.DrawImage(icon.ToBitmap(), x, y);
				return;
			}
			icon.Draw(this, x, y);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000EAD7 File Offset: 0x0000DAD7
		public void DrawIcon(Icon icon, Rectangle targetRect)
		{
			if (icon == null)
			{
				throw new ArgumentNullException("icon");
			}
			if (this.backingImage != null)
			{
				this.DrawImage(icon.ToBitmap(), targetRect);
				return;
			}
			icon.Draw(this, targetRect);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000EB05 File Offset: 0x0000DB05
		public void DrawIconUnstretched(Icon icon, Rectangle targetRect)
		{
			if (icon == null)
			{
				throw new ArgumentNullException("icon");
			}
			if (this.backingImage != null)
			{
				this.DrawImageUnscaled(icon.ToBitmap(), targetRect);
				return;
			}
			icon.DrawUnstretched(this, targetRect);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000EB33 File Offset: 0x0000DB33
		public void DrawImage(Image image, PointF point)
		{
			this.DrawImage(image, point.X, point.Y);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000EB4C File Offset: 0x0000DB4C
		public void DrawImage(Image image, float x, float y)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawImage(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), x, y);
			this.IgnoreMetafileErrors(image, ref num);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000EB97 File Offset: 0x0000DB97
		public void DrawImage(Image image, RectangleF rect)
		{
			this.DrawImage(image, rect.X, rect.Y, rect.Width, rect.Height);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000EBBC File Offset: 0x0000DBBC
		public void DrawImage(Image image, float x, float y, float width, float height)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawImageRect(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), x, y, width, height);
			this.IgnoreMetafileErrors(image, ref num);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000EC0B File Offset: 0x0000DC0B
		public void DrawImage(Image image, Point point)
		{
			this.DrawImage(image, point.X, point.Y);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000EC24 File Offset: 0x0000DC24
		public void DrawImage(Image image, int x, int y)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawImageI(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), x, y);
			this.IgnoreMetafileErrors(image, ref num);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000EC6F File Offset: 0x0000DC6F
		public void DrawImage(Image image, Rectangle rect)
		{
			this.DrawImage(image, rect.X, rect.Y, rect.Width, rect.Height);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000EC94 File Offset: 0x0000DC94
		public void DrawImage(Image image, int x, int y, int width, int height)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawImageRectI(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), x, y, width, height);
			this.IgnoreMetafileErrors(image, ref num);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000ECE3 File Offset: 0x0000DCE3
		public void DrawImageUnscaled(Image image, Point point)
		{
			this.DrawImage(image, point.X, point.Y);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000ECFA File Offset: 0x0000DCFA
		public void DrawImageUnscaled(Image image, int x, int y)
		{
			this.DrawImage(image, x, y);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000ED05 File Offset: 0x0000DD05
		public void DrawImageUnscaled(Image image, Rectangle rect)
		{
			this.DrawImage(image, rect.X, rect.Y);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000ED1C File Offset: 0x0000DD1C
		public void DrawImageUnscaled(Image image, int x, int y, int width, int height)
		{
			this.DrawImage(image, x, y);
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000ED28 File Offset: 0x0000DD28
		public void DrawImageUnscaledAndClipped(Image image, Rectangle rect)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = Math.Min(rect.Width, image.Width);
			int num2 = Math.Min(rect.Height, image.Height);
			this.DrawImage(image, rect, 0, 0, num, num2, GraphicsUnit.Pixel);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000ED78 File Offset: 0x0000DD78
		public void DrawImage(Image image, PointF[] destPoints)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = destPoints.Length;
			if (num != 3 && num != 4)
			{
				throw new ArgumentException(SR.GetString("GdiplusDestPointsInvalidLength"));
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			int num2 = SafeNativeMethods.Gdip.GdipDrawImagePoints(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), new HandleRef(this, intPtr), num);
			Marshal.FreeHGlobal(intPtr);
			this.IgnoreMetafileErrors(image, ref num2);
			this.CheckErrorStatus(num2);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000EE00 File Offset: 0x0000DE00
		public void DrawImage(Image image, Point[] destPoints)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = destPoints.Length;
			if (num != 3 && num != 4)
			{
				throw new ArgumentException(SR.GetString("GdiplusDestPointsInvalidLength"));
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			int num2 = SafeNativeMethods.Gdip.GdipDrawImagePointsI(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), new HandleRef(this, intPtr), num);
			Marshal.FreeHGlobal(intPtr);
			this.IgnoreMetafileErrors(image, ref num2);
			this.CheckErrorStatus(num2);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000EE88 File Offset: 0x0000DE88
		public void DrawImage(Image image, float x, float y, RectangleF srcRect, GraphicsUnit srcUnit)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawImagePointRect(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), x, y, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, (int)srcUnit);
			this.IgnoreMetafileErrors(image, ref num);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000EEF4 File Offset: 0x0000DEF4
		public void DrawImage(Image image, int x, int y, Rectangle srcRect, GraphicsUnit srcUnit)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawImagePointRectI(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), x, y, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, (int)srcUnit);
			this.IgnoreMetafileErrors(image, ref num);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000EF60 File Offset: 0x0000DF60
		public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawImageRectRect(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), destRect.X, destRect.Y, destRect.Width, destRect.Height, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, (int)srcUnit, NativeMethods.NullHandleRef, null, NativeMethods.NullHandleRef);
			this.IgnoreMetafileErrors(image, ref num);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000EFF0 File Offset: 0x0000DFF0
		public void DrawImage(Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawImageRectRectI(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), destRect.X, destRect.Y, destRect.Width, destRect.Height, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, (int)srcUnit, NativeMethods.NullHandleRef, null, NativeMethods.NullHandleRef);
			this.IgnoreMetafileErrors(image, ref num);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000F080 File Offset: 0x0000E080
		public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = destPoints.Length;
			if (num != 3 && num != 4)
			{
				throw new ArgumentException(SR.GetString("GdiplusDestPointsInvalidLength"));
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			int num2 = SafeNativeMethods.Gdip.GdipDrawImagePointsRect(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), new HandleRef(this, intPtr), destPoints.Length, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, (int)srcUnit, NativeMethods.NullHandleRef, null, NativeMethods.NullHandleRef);
			Marshal.FreeHGlobal(intPtr);
			this.IgnoreMetafileErrors(image, ref num2);
			this.CheckErrorStatus(num2);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000F133 File Offset: 0x0000E133
		public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr)
		{
			this.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, null, 0);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000F144 File Offset: 0x0000E144
		public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback)
		{
			this.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, callback, 0);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000F158 File Offset: 0x0000E158
		public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback, int callbackData)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = destPoints.Length;
			if (num != 3 && num != 4)
			{
				throw new ArgumentException(SR.GetString("GdiplusDestPointsInvalidLength"));
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			int num2 = SafeNativeMethods.Gdip.GdipDrawImagePointsRect(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), new HandleRef(this, intPtr), destPoints.Length, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, (int)srcUnit, new HandleRef(imageAttr, (imageAttr != null) ? imageAttr.nativeImageAttributes : IntPtr.Zero), callback, new HandleRef(null, (IntPtr)callbackData));
			Marshal.FreeHGlobal(intPtr);
			this.IgnoreMetafileErrors(image, ref num2);
			this.CheckErrorStatus(num2);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000F228 File Offset: 0x0000E228
		public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit)
		{
			this.DrawImage(image, destPoints, srcRect, srcUnit, null, null, 0);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000F238 File Offset: 0x0000E238
		public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr)
		{
			this.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, null, 0);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000F249 File Offset: 0x0000E249
		public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback)
		{
			this.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, callback, 0);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000F25C File Offset: 0x0000E25C
		public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback, int callbackData)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = destPoints.Length;
			if (num != 3 && num != 4)
			{
				throw new ArgumentException(SR.GetString("GdiplusDestPointsInvalidLength"));
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			int num2 = SafeNativeMethods.Gdip.GdipDrawImagePointsRectI(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), new HandleRef(this, intPtr), destPoints.Length, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, (int)srcUnit, new HandleRef(imageAttr, (imageAttr != null) ? imageAttr.nativeImageAttributes : IntPtr.Zero), callback, new HandleRef(null, (IntPtr)callbackData));
			Marshal.FreeHGlobal(intPtr);
			this.IgnoreMetafileErrors(image, ref num2);
			this.CheckErrorStatus(num2);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000F32C File Offset: 0x0000E32C
		public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit)
		{
			this.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, null);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000F34C File Offset: 0x0000E34C
		public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs)
		{
			this.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, null);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000F370 File Offset: 0x0000E370
		public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, Graphics.DrawImageAbort callback)
		{
			this.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, callback, IntPtr.Zero);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000F398 File Offset: 0x0000E398
		public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, Graphics.DrawImageAbort callback, IntPtr callbackData)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawImageRectRect(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), (float)destRect.X, (float)destRect.Y, (float)destRect.Width, (float)destRect.Height, srcX, srcY, srcWidth, srcHeight, (int)srcUnit, new HandleRef(imageAttrs, (imageAttrs != null) ? imageAttrs.nativeImageAttributes : IntPtr.Zero), callback, new HandleRef(null, callbackData));
			this.IgnoreMetafileErrors(image, ref num);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000F430 File Offset: 0x0000E430
		public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit)
		{
			this.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, null);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000F450 File Offset: 0x0000E450
		public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr)
		{
			this.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttr, null);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000F474 File Offset: 0x0000E474
		public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback)
		{
			this.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttr, callback, IntPtr.Zero);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000F49C File Offset: 0x0000E49C
		public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, Graphics.DrawImageAbort callback, IntPtr callbackData)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = SafeNativeMethods.Gdip.GdipDrawImageRectRectI(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), destRect.X, destRect.Y, destRect.Width, destRect.Height, srcX, srcY, srcWidth, srcHeight, (int)srcUnit, new HandleRef(imageAttrs, (imageAttrs != null) ? imageAttrs.nativeImageAttributes : IntPtr.Zero), callback, new HandleRef(null, callbackData));
			this.IgnoreMetafileErrors(image, ref num);
			this.CheckErrorStatus(num);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000F52D File Offset: 0x0000E52D
		public void EnumerateMetafile(Metafile metafile, PointF destPoint, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destPoint, callback, IntPtr.Zero);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000F53D File Offset: 0x0000E53D
		public void EnumerateMetafile(Metafile metafile, PointF destPoint, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destPoint, callback, callbackData, null);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000F54C File Offset: 0x0000E54C
		public void EnumerateMetafile(Metafile metafile, PointF destPoint, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			IntPtr intPtr = ((metafile == null) ? IntPtr.Zero : metafile.nativeImage);
			IntPtr intPtr2 = ((imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileDestPoint(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, intPtr), new GPPOINTF(destPoint), callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, intPtr2));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000F5B7 File Offset: 0x0000E5B7
		public void EnumerateMetafile(Metafile metafile, Point destPoint, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destPoint, callback, IntPtr.Zero);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000F5C7 File Offset: 0x0000E5C7
		public void EnumerateMetafile(Metafile metafile, Point destPoint, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destPoint, callback, callbackData, null);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000F5D8 File Offset: 0x0000E5D8
		public void EnumerateMetafile(Metafile metafile, Point destPoint, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			IntPtr intPtr = ((metafile == null) ? IntPtr.Zero : metafile.nativeImage);
			IntPtr intPtr2 = ((imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileDestPointI(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, intPtr), new GPPOINT(destPoint), callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, intPtr2));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000F643 File Offset: 0x0000E643
		public void EnumerateMetafile(Metafile metafile, RectangleF destRect, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destRect, callback, IntPtr.Zero);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000F653 File Offset: 0x0000E653
		public void EnumerateMetafile(Metafile metafile, RectangleF destRect, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destRect, callback, callbackData, null);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000F664 File Offset: 0x0000E664
		public void EnumerateMetafile(Metafile metafile, RectangleF destRect, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			IntPtr intPtr = ((metafile == null) ? IntPtr.Zero : metafile.nativeImage);
			IntPtr intPtr2 = ((imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes);
			GPRECTF gprectf = new GPRECTF(destRect);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileDestRect(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, intPtr), ref gprectf, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, intPtr2));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000F6D8 File Offset: 0x0000E6D8
		public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destRect, callback, IntPtr.Zero);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000F6E8 File Offset: 0x0000E6E8
		public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destRect, callback, callbackData, null);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000F6F8 File Offset: 0x0000E6F8
		public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			IntPtr intPtr = ((metafile == null) ? IntPtr.Zero : metafile.nativeImage);
			IntPtr intPtr2 = ((imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes);
			GPRECT gprect = new GPRECT(destRect);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileDestRectI(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, intPtr), ref gprect, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, intPtr2));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000F76C File Offset: 0x0000E76C
		public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destPoints, callback, IntPtr.Zero);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000F77C File Offset: 0x0000E77C
		public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destPoints, callback, IntPtr.Zero, null);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000F790 File Offset: 0x0000E790
		public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			if (destPoints.Length != 3)
			{
				throw new ArgumentException(SR.GetString("GdiplusDestPointsInvalidParallelogram"));
			}
			IntPtr intPtr = ((metafile == null) ? IntPtr.Zero : metafile.nativeImage);
			IntPtr intPtr2 = ((imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes);
			IntPtr intPtr3 = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileDestPoints(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, intPtr), intPtr3, destPoints.Length, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, intPtr2));
			Marshal.FreeHGlobal(intPtr3);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000F82A File Offset: 0x0000E82A
		public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destPoints, callback, IntPtr.Zero);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000F83A File Offset: 0x0000E83A
		public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destPoints, callback, callbackData, null);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000F848 File Offset: 0x0000E848
		public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			if (destPoints.Length != 3)
			{
				throw new ArgumentException(SR.GetString("GdiplusDestPointsInvalidParallelogram"));
			}
			IntPtr intPtr = ((metafile == null) ? IntPtr.Zero : metafile.nativeImage);
			IntPtr intPtr2 = ((imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes);
			IntPtr intPtr3 = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileDestPointsI(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, intPtr), intPtr3, destPoints.Length, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, intPtr2));
			Marshal.FreeHGlobal(intPtr3);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000F8E2 File Offset: 0x0000E8E2
		public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destPoint, srcRect, srcUnit, callback, IntPtr.Zero);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000F8F6 File Offset: 0x0000E8F6
		public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destPoint, srcRect, srcUnit, callback, callbackData, null);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000F908 File Offset: 0x0000E908
		public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			IntPtr intPtr = ((metafile == null) ? IntPtr.Zero : metafile.nativeImage);
			IntPtr intPtr2 = ((imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes);
			GPRECTF gprectf = new GPRECTF(srcRect);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileSrcRectDestPoint(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, intPtr), new GPPOINTF(destPoint), ref gprectf, (int)unit, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, intPtr2));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000F985 File Offset: 0x0000E985
		public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destPoint, srcRect, srcUnit, callback, IntPtr.Zero);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000F999 File Offset: 0x0000E999
		public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destPoint, srcRect, srcUnit, callback, callbackData, null);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000F9AC File Offset: 0x0000E9AC
		public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			IntPtr intPtr = ((metafile == null) ? IntPtr.Zero : metafile.nativeImage);
			IntPtr intPtr2 = ((imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes);
			GPPOINT gppoint = new GPPOINT(destPoint);
			GPRECT gprect = new GPRECT(srcRect);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileSrcRectDestPointI(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, intPtr), gppoint, ref gprect, (int)unit, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, intPtr2));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000FA2E File Offset: 0x0000EA2E
		public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destRect, srcRect, srcUnit, callback, IntPtr.Zero);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000FA42 File Offset: 0x0000EA42
		public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destRect, srcRect, srcUnit, callback, callbackData, null);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000FA54 File Offset: 0x0000EA54
		public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			IntPtr intPtr = ((metafile == null) ? IntPtr.Zero : metafile.nativeImage);
			IntPtr intPtr2 = ((imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes);
			GPRECTF gprectf = new GPRECTF(destRect);
			GPRECTF gprectf2 = new GPRECTF(srcRect);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileSrcRectDestRect(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, intPtr), ref gprectf, ref gprectf2, (int)unit, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, intPtr2));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000FADD File Offset: 0x0000EADD
		public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destRect, srcRect, srcUnit, callback, IntPtr.Zero);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000FAF1 File Offset: 0x0000EAF1
		public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destRect, srcRect, srcUnit, callback, callbackData, null);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000FB04 File Offset: 0x0000EB04
		public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			IntPtr intPtr = ((metafile == null) ? IntPtr.Zero : metafile.nativeImage);
			IntPtr intPtr2 = ((imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes);
			GPRECT gprect = new GPRECT(destRect);
			GPRECT gprect2 = new GPRECT(srcRect);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileSrcRectDestRectI(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, intPtr), ref gprect, ref gprect2, (int)unit, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, intPtr2));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000FB8D File Offset: 0x0000EB8D
		public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destPoints, srcRect, srcUnit, callback, IntPtr.Zero);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000FBA1 File Offset: 0x0000EBA1
		public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destPoints, srcRect, srcUnit, callback, callbackData, null);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000FBB4 File Offset: 0x0000EBB4
		public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			if (destPoints.Length != 3)
			{
				throw new ArgumentException(SR.GetString("GdiplusDestPointsInvalidParallelogram"));
			}
			IntPtr intPtr = ((metafile == null) ? IntPtr.Zero : metafile.nativeImage);
			IntPtr intPtr2 = ((imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes);
			IntPtr intPtr3 = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			GPRECTF gprectf = new GPRECTF(srcRect);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileSrcRectDestPoints(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, intPtr), intPtr3, destPoints.Length, ref gprectf, (int)unit, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, intPtr2));
			Marshal.FreeHGlobal(intPtr3);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000FC63 File Offset: 0x0000EC63
		public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destPoints, srcRect, srcUnit, callback, IntPtr.Zero);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000FC77 File Offset: 0x0000EC77
		public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destPoints, srcRect, srcUnit, callback, callbackData, null);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000FC8C File Offset: 0x0000EC8C
		public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			if (destPoints.Length != 3)
			{
				throw new ArgumentException(SR.GetString("GdiplusDestPointsInvalidParallelogram"));
			}
			IntPtr intPtr = ((metafile == null) ? IntPtr.Zero : metafile.nativeImage);
			IntPtr intPtr2 = ((imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes);
			IntPtr intPtr3 = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			GPRECT gprect = new GPRECT(srcRect);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileSrcRectDestPointsI(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, intPtr), intPtr3, destPoints.Length, ref gprect, (int)unit, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, intPtr2));
			Marshal.FreeHGlobal(intPtr3);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000FD3B File Offset: 0x0000ED3B
		public void SetClip(Graphics g)
		{
			this.SetClip(g, CombineMode.Replace);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000FD48 File Offset: 0x0000ED48
		public void SetClip(Graphics g, CombineMode combineMode)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			int num = SafeNativeMethods.Gdip.GdipSetClipGraphics(new HandleRef(this, this.NativeGraphics), new HandleRef(g, g.NativeGraphics), combineMode);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000FD8C File Offset: 0x0000ED8C
		public void SetClip(Rectangle rect)
		{
			this.SetClip(rect, CombineMode.Replace);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000FD98 File Offset: 0x0000ED98
		public void SetClip(Rectangle rect, CombineMode combineMode)
		{
			int num = SafeNativeMethods.Gdip.GdipSetClipRectI(new HandleRef(this, this.NativeGraphics), rect.X, rect.Y, rect.Width, rect.Height, combineMode);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000FDDE File Offset: 0x0000EDDE
		public void SetClip(RectangleF rect)
		{
			this.SetClip(rect, CombineMode.Replace);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000FDE8 File Offset: 0x0000EDE8
		public void SetClip(RectangleF rect, CombineMode combineMode)
		{
			int num = SafeNativeMethods.Gdip.GdipSetClipRect(new HandleRef(this, this.NativeGraphics), rect.X, rect.Y, rect.Width, rect.Height, combineMode);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000FE2E File Offset: 0x0000EE2E
		public void SetClip(GraphicsPath path)
		{
			this.SetClip(path, CombineMode.Replace);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000FE38 File Offset: 0x0000EE38
		public void SetClip(GraphicsPath path, CombineMode combineMode)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			int num = SafeNativeMethods.Gdip.GdipSetClipPath(new HandleRef(this, this.NativeGraphics), new HandleRef(path, path.nativePath), combineMode);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000FE7C File Offset: 0x0000EE7C
		public void SetClip(Region region, CombineMode combineMode)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			int num = SafeNativeMethods.Gdip.GdipSetClipRegion(new HandleRef(this, this.NativeGraphics), new HandleRef(region, region.nativeRegion), combineMode);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000FEC0 File Offset: 0x0000EEC0
		public void IntersectClip(Rectangle rect)
		{
			int num = SafeNativeMethods.Gdip.GdipSetClipRectI(new HandleRef(this, this.NativeGraphics), rect.X, rect.Y, rect.Width, rect.Height, CombineMode.Intersect);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0000FF08 File Offset: 0x0000EF08
		public void IntersectClip(RectangleF rect)
		{
			int num = SafeNativeMethods.Gdip.GdipSetClipRect(new HandleRef(this, this.NativeGraphics), rect.X, rect.Y, rect.Width, rect.Height, CombineMode.Intersect);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000FF50 File Offset: 0x0000EF50
		public void IntersectClip(Region region)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			int num = SafeNativeMethods.Gdip.GdipSetClipRegion(new HandleRef(this, this.NativeGraphics), new HandleRef(region, region.nativeRegion), CombineMode.Intersect);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0000FF94 File Offset: 0x0000EF94
		public void ExcludeClip(Rectangle rect)
		{
			int num = SafeNativeMethods.Gdip.GdipSetClipRectI(new HandleRef(this, this.NativeGraphics), rect.X, rect.Y, rect.Width, rect.Height, CombineMode.Exclude);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0000FFDC File Offset: 0x0000EFDC
		public void ExcludeClip(Region region)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			int num = SafeNativeMethods.Gdip.GdipSetClipRegion(new HandleRef(this, this.NativeGraphics), new HandleRef(region, region.nativeRegion), CombineMode.Exclude);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00010020 File Offset: 0x0000F020
		public void ResetClip()
		{
			int num = SafeNativeMethods.Gdip.GdipResetClip(new HandleRef(this, this.NativeGraphics));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001004C File Offset: 0x0000F04C
		public void TranslateClip(float dx, float dy)
		{
			int num = SafeNativeMethods.Gdip.GdipTranslateClip(new HandleRef(this, this.NativeGraphics), dx, dy);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00010078 File Offset: 0x0000F078
		public void TranslateClip(int dx, int dy)
		{
			int num = SafeNativeMethods.Gdip.GdipTranslateClip(new HandleRef(this, this.NativeGraphics), (float)dx, (float)dy);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x000100A8 File Offset: 0x0000F0A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[StrongNameIdentityPermission(SecurityAction.LinkDemand, Name = "System.Windows.Forms", PublicKey = "0x00000000000000000400000000000000")]
		public object GetContextInfo()
		{
			Region clip = this.Clip;
			Matrix transform = this.Transform;
			PointF pointF = PointF.Empty;
			PointF empty = PointF.Empty;
			if (!transform.IsIdentity)
			{
				float[] elements = transform.Elements;
				pointF.X = elements[4];
				pointF.Y = elements[5];
			}
			GraphicsContext previous = this.previousContext;
			while (previous != null)
			{
				if (!previous.TransformOffset.IsEmpty)
				{
					transform.Translate(previous.TransformOffset.X, previous.TransformOffset.Y);
				}
				if (!pointF.IsEmpty)
				{
					clip.Translate(pointF.X, pointF.Y);
					empty.X += pointF.X;
					empty.Y += pointF.Y;
				}
				if (previous.Clip != null)
				{
					clip.Intersect(previous.Clip);
				}
				pointF = previous.TransformOffset;
				do
				{
					previous = previous.Previous;
				}
				while (previous != null && previous.Next.IsCumulative && previous.IsCumulative);
			}
			if (!empty.IsEmpty)
			{
				clip.Translate(-empty.X, -empty.Y);
			}
			return new object[] { clip, transform };
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x00010200 File Offset: 0x0000F200
		// (set) Token: 0x06000426 RID: 1062 RVA: 0x0001023C File Offset: 0x0000F23C
		public Region Clip
		{
			get
			{
				Region region = new Region();
				int num = SafeNativeMethods.Gdip.GdipGetClip(new HandleRef(this, this.NativeGraphics), new HandleRef(region, region.nativeRegion));
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return region;
			}
			set
			{
				this.SetClip(value, CombineMode.Replace);
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00010248 File Offset: 0x0000F248
		public RectangleF ClipBounds
		{
			get
			{
				GPRECTF gprectf = default(GPRECTF);
				int num = SafeNativeMethods.Gdip.GdipGetClipBounds(new HandleRef(this, this.NativeGraphics), ref gprectf);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return gprectf.ToRectangleF();
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x00010284 File Offset: 0x0000F284
		public bool IsClipEmpty
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipIsClipEmpty(new HandleRef(this, this.NativeGraphics), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return num2 != 0;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x000102B8 File Offset: 0x0000F2B8
		public RectangleF VisibleClipBounds
		{
			get
			{
				if (this.PrintingHelper != null)
				{
					PrintPreviewGraphics printPreviewGraphics = this.PrintingHelper as PrintPreviewGraphics;
					if (printPreviewGraphics != null)
					{
						return printPreviewGraphics.VisibleClipBounds;
					}
				}
				GPRECTF gprectf = default(GPRECTF);
				int num = SafeNativeMethods.Gdip.GdipGetVisibleClipBounds(new HandleRef(this, this.NativeGraphics), ref gprectf);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return gprectf.ToRectangleF();
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x00010310 File Offset: 0x0000F310
		public bool IsVisibleClipEmpty
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipIsVisibleClipEmpty(new HandleRef(this, this.NativeGraphics), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00010342 File Offset: 0x0000F342
		public bool IsVisible(int x, int y)
		{
			return this.IsVisible(new Point(x, y));
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00010354 File Offset: 0x0000F354
		public bool IsVisible(Point point)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsVisiblePointI(new HandleRef(this, this.NativeGraphics), point.X, point.Y, out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00010394 File Offset: 0x0000F394
		public bool IsVisible(float x, float y)
		{
			return this.IsVisible(new PointF(x, y));
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x000103A4 File Offset: 0x0000F3A4
		public bool IsVisible(PointF point)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsVisiblePoint(new HandleRef(this, this.NativeGraphics), point.X, point.Y, out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x000103E4 File Offset: 0x0000F3E4
		public bool IsVisible(int x, int y, int width, int height)
		{
			return this.IsVisible(new Rectangle(x, y, width, height));
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x000103F8 File Offset: 0x0000F3F8
		public bool IsVisible(Rectangle rect)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsVisibleRectI(new HandleRef(this, this.NativeGraphics), rect.X, rect.Y, rect.Width, rect.Height, out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00010446 File Offset: 0x0000F446
		public bool IsVisible(float x, float y, float width, float height)
		{
			return this.IsVisible(new RectangleF(x, y, width, height));
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00010458 File Offset: 0x0000F458
		public bool IsVisible(RectangleF rect)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsVisibleRect(new HandleRef(this, this.NativeGraphics), rect.X, rect.Y, rect.Width, rect.Height, out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x000104A6 File Offset: 0x0000F4A6
		private void PushContext(GraphicsContext context)
		{
			if (this.previousContext != null)
			{
				context.Previous = this.previousContext;
				this.previousContext.Next = context;
			}
			this.previousContext = context;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x000104D0 File Offset: 0x0000F4D0
		private void PopContext(int currentContextState)
		{
			for (GraphicsContext previous = this.previousContext; previous != null; previous = previous.Previous)
			{
				if (previous.State == currentContextState)
				{
					this.previousContext = previous.Previous;
					previous.Dispose();
					return;
				}
			}
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0001050C File Offset: 0x0000F50C
		public GraphicsState Save()
		{
			GraphicsContext graphicsContext = new GraphicsContext(this);
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipSaveGraphics(new HandleRef(this, this.NativeGraphics), out num);
			if (num2 != 0)
			{
				graphicsContext.Dispose();
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			graphicsContext.State = num;
			graphicsContext.IsCumulative = true;
			this.PushContext(graphicsContext);
			return new GraphicsState(num);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00010564 File Offset: 0x0000F564
		public void Restore(GraphicsState gstate)
		{
			int num = SafeNativeMethods.Gdip.GdipRestoreGraphics(new HandleRef(this, this.NativeGraphics), gstate.nativeState);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.PopContext(gstate.nativeState);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x000105A0 File Offset: 0x0000F5A0
		public GraphicsContainer BeginContainer(RectangleF dstrect, RectangleF srcrect, GraphicsUnit unit)
		{
			GraphicsContext graphicsContext = new GraphicsContext(this);
			int num = 0;
			GPRECTF gprectf = dstrect.ToGPRECTF();
			GPRECTF gprectf2 = srcrect.ToGPRECTF();
			int num2 = SafeNativeMethods.Gdip.GdipBeginContainer(new HandleRef(this, this.NativeGraphics), ref gprectf, ref gprectf2, (int)unit, out num);
			if (num2 != 0)
			{
				graphicsContext.Dispose();
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			graphicsContext.State = num;
			this.PushContext(graphicsContext);
			return new GraphicsContainer(num);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00010608 File Offset: 0x0000F608
		public GraphicsContainer BeginContainer()
		{
			GraphicsContext graphicsContext = new GraphicsContext(this);
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipBeginContainer2(new HandleRef(this, this.NativeGraphics), out num);
			if (num2 != 0)
			{
				graphicsContext.Dispose();
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			graphicsContext.State = num;
			this.PushContext(graphicsContext);
			return new GraphicsContainer(num);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00010658 File Offset: 0x0000F658
		public void EndContainer(GraphicsContainer container)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			int num = SafeNativeMethods.Gdip.GdipEndContainer(new HandleRef(this, this.NativeGraphics), container.nativeGraphicsContainer);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.PopContext(container.nativeGraphicsContainer);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x000106A4 File Offset: 0x0000F6A4
		public GraphicsContainer BeginContainer(Rectangle dstrect, Rectangle srcrect, GraphicsUnit unit)
		{
			GraphicsContext graphicsContext = new GraphicsContext(this);
			int num = 0;
			GPRECT gprect = new GPRECT(dstrect);
			GPRECT gprect2 = new GPRECT(srcrect);
			int num2 = SafeNativeMethods.Gdip.GdipBeginContainerI(new HandleRef(this, this.NativeGraphics), ref gprect, ref gprect2, (int)unit, out num);
			if (num2 != 0)
			{
				graphicsContext.Dispose();
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			graphicsContext.State = num;
			this.PushContext(graphicsContext);
			return new GraphicsContainer(num);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00010714 File Offset: 0x0000F714
		public void AddMetafileComment(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			int num = SafeNativeMethods.Gdip.GdipComment(new HandleRef(this, this.NativeGraphics), data.Length, data);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00010750 File Offset: 0x0000F750
		public static IntPtr GetHalftonePalette()
		{
			if (Graphics.halftonePalette == IntPtr.Zero)
			{
				lock (Graphics.syncObject)
				{
					if (Graphics.halftonePalette == IntPtr.Zero)
					{
						if (Environment.OSVersion.Platform != PlatformID.Win32Windows)
						{
							AppDomain.CurrentDomain.DomainUnload += Graphics.OnDomainUnload;
						}
						AppDomain.CurrentDomain.ProcessExit += Graphics.OnDomainUnload;
						Graphics.halftonePalette = SafeNativeMethods.Gdip.GdipCreateHalftonePalette();
					}
				}
			}
			return Graphics.halftonePalette;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x000107EC File Offset: 0x0000F7EC
		[PrePrepareMethod]
		private static void OnDomainUnload(object sender, EventArgs e)
		{
			if (Graphics.halftonePalette != IntPtr.Zero)
			{
				SafeNativeMethods.IntDeleteObject(new HandleRef(null, Graphics.halftonePalette));
				Graphics.halftonePalette = IntPtr.Zero;
			}
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0001081C File Offset: 0x0000F81C
		private void CheckErrorStatus(int status)
		{
			if (status != 0)
			{
				if (status == 1 || status == 7)
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (lastWin32Error == 5 || lastWin32Error == 127 || ((UnsafeNativeMethods.GetSystemMetrics(4096) & 1) != 0 && lastWin32Error == 0))
					{
						return;
					}
				}
				throw SafeNativeMethods.Gdip.StatusException(status);
			}
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0001085C File Offset: 0x0000F85C
		private void IgnoreMetafileErrors(Image image, ref int errorStatus)
		{
			if (errorStatus != 0 && image.RawFormat.Equals(ImageFormat.Emf))
			{
				errorStatus = 0;
			}
		}

		// Token: 0x040002AB RID: 683
		private GraphicsContext previousContext;

		// Token: 0x040002AC RID: 684
		private static object syncObject = new object();

		// Token: 0x040002AD RID: 685
		private IntPtr nativeGraphics;

		// Token: 0x040002AE RID: 686
		private IntPtr nativeHdc;

		// Token: 0x040002AF RID: 687
		private object printingHelper;

		// Token: 0x040002B0 RID: 688
		private static IntPtr halftonePalette;

		// Token: 0x040002B1 RID: 689
		private Image backingImage;

		// Token: 0x02000045 RID: 69
		// (Invoke) Token: 0x06000442 RID: 1090
		public delegate bool DrawImageAbort(IntPtr callbackdata);

		// Token: 0x02000046 RID: 70
		// (Invoke) Token: 0x06000446 RID: 1094
		public delegate bool EnumerateMetafileProc(EmfPlusRecordType recordType, int flags, int dataSize, IntPtr data, PlayRecordCallback callbackData);
	}
}
