using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Forms
{
	// Token: 0x0200027F RID: 639
	[ToolboxItemFilter("System.Windows.Forms")]
	[Designer("System.Windows.Forms.Design.ImageListDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("Images")]
	[TypeConverter(typeof(ImageListConverter))]
	[SRDescription("DescriptionImageList")]
	[DesignerSerializer("System.Windows.Forms.Design.ImageListCodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public sealed class ImageList : Component
	{
		// Token: 0x0600226C RID: 8812 RVA: 0x0004B2EB File Offset: 0x0004A2EB
		public ImageList()
		{
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x0004B31B File Offset: 0x0004A31B
		public ImageList(IContainer container)
		{
			container.Add(this);
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x0600226E RID: 8814 RVA: 0x0004B352 File Offset: 0x0004A352
		// (set) Token: 0x0600226F RID: 8815 RVA: 0x0004B35C File Offset: 0x0004A35C
		[SRCategory("CatAppearance")]
		[SRDescription("ImageListColorDepthDescr")]
		public ColorDepth ColorDepth
		{
			get
			{
				return this.colorDepth;
			}
			set
			{
				if (!ClientUtils.IsEnumValid_NotSequential(value, (int)value, new int[] { 4, 8, 16, 24, 32 }))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ColorDepth));
				}
				if (this.colorDepth != value)
				{
					this.colorDepth = value;
					this.PerformRecreateHandle("ColorDepth");
				}
			}
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x0004B3C7 File Offset: 0x0004A3C7
		private bool ShouldSerializeColorDepth()
		{
			return this.Images.Count == 0;
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x0004B3D7 File Offset: 0x0004A3D7
		private void ResetColorDepth()
		{
			this.ColorDepth = ColorDepth.Depth8Bit;
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06002272 RID: 8818 RVA: 0x0004B3E0 File Offset: 0x0004A3E0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ImageListHandleDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public IntPtr Handle
		{
			get
			{
				if (this.nativeImageList == null)
				{
					this.CreateHandle();
				}
				return this.nativeImageList.Handle;
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06002273 RID: 8819 RVA: 0x0004B3FB File Offset: 0x0004A3FB
		[SRDescription("ImageListHandleCreatedDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool HandleCreated
		{
			get
			{
				return this.nativeImageList != null;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06002274 RID: 8820 RVA: 0x0004B409 File Offset: 0x0004A409
		[SRDescription("ImageListImagesDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(null)]
		[SRCategory("CatAppearance")]
		[MergableProperty(false)]
		public ImageList.ImageCollection Images
		{
			get
			{
				if (this.imageCollection == null)
				{
					this.imageCollection = new ImageList.ImageCollection(this);
				}
				return this.imageCollection;
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06002275 RID: 8821 RVA: 0x0004B425 File Offset: 0x0004A425
		// (set) Token: 0x06002276 RID: 8822 RVA: 0x0004B430 File Offset: 0x0004A430
		[SRDescription("ImageListSizeDescr")]
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		public Size ImageSize
		{
			get
			{
				return this.imageSize;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[] { "ImageSize", "Size.Empty" }));
				}
				if (value.Width <= 0 || value.Width > 256)
				{
					throw new ArgumentOutOfRangeException("ImageSize", SR.GetString("InvalidBoundArgument", new object[]
					{
						"ImageSize.Width",
						value.Width.ToString(CultureInfo.CurrentCulture),
						1.ToString(CultureInfo.CurrentCulture),
						"256"
					}));
				}
				if (value.Height <= 0 || value.Height > 256)
				{
					throw new ArgumentOutOfRangeException("ImageSize", SR.GetString("InvalidBoundArgument", new object[]
					{
						"ImageSize.Height",
						value.Height.ToString(CultureInfo.CurrentCulture),
						1.ToString(CultureInfo.CurrentCulture),
						"256"
					}));
				}
				if (this.imageSize.Width != value.Width || this.imageSize.Height != value.Height)
				{
					this.imageSize = new Size(value.Width, value.Height);
					this.PerformRecreateHandle("ImageSize");
				}
			}
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x0004B59C File Offset: 0x0004A59C
		private bool ShouldSerializeImageSize()
		{
			return this.Images.Count == 0;
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06002278 RID: 8824 RVA: 0x0004B5AC File Offset: 0x0004A5AC
		// (set) Token: 0x06002279 RID: 8825 RVA: 0x0004B5C4 File Offset: 0x0004A5C4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DefaultValue(null)]
		[SRDescription("ImageListImageStreamDescr")]
		[Browsable(false)]
		public ImageListStreamer ImageStream
		{
			get
			{
				if (this.Images.Empty)
				{
					return null;
				}
				return new ImageListStreamer(this);
			}
			set
			{
				if (value != null)
				{
					ImageList.NativeImageList nativeImageList = value.GetNativeImageList();
					if (nativeImageList != null && nativeImageList != this.nativeImageList)
					{
						this.DestroyHandle();
						this.originals = null;
						this.nativeImageList = new ImageList.NativeImageList(SafeNativeMethods.ImageList_Duplicate(new HandleRef(nativeImageList, nativeImageList.Handle)));
						int num;
						int num2;
						if (SafeNativeMethods.ImageList_GetIconSize(new HandleRef(this, this.nativeImageList.Handle), out num, out num2))
						{
							this.imageSize = new Size(num, num2);
						}
						NativeMethods.IMAGEINFO imageinfo = new NativeMethods.IMAGEINFO();
						if (SafeNativeMethods.ImageList_GetImageInfo(new HandleRef(this, this.nativeImageList.Handle), 0, imageinfo))
						{
							NativeMethods.BITMAP bitmap = new NativeMethods.BITMAP();
							UnsafeNativeMethods.GetObject(new HandleRef(null, imageinfo.hbmImage), Marshal.SizeOf(bitmap), bitmap);
							short bmBitsPixel = bitmap.bmBitsPixel;
							if (bmBitsPixel <= 8)
							{
								if (bmBitsPixel != 4)
								{
									if (bmBitsPixel == 8)
									{
										this.colorDepth = ColorDepth.Depth8Bit;
									}
								}
								else
								{
									this.colorDepth = ColorDepth.Depth4Bit;
								}
							}
							else if (bmBitsPixel != 16)
							{
								if (bmBitsPixel != 24)
								{
									if (bmBitsPixel == 32)
									{
										this.colorDepth = ColorDepth.Depth32Bit;
									}
								}
								else
								{
									this.colorDepth = ColorDepth.Depth24Bit;
								}
							}
							else
							{
								this.colorDepth = ColorDepth.Depth16Bit;
							}
						}
						this.Images.ResetKeys();
						this.OnRecreateHandle(new EventArgs());
						return;
					}
				}
				else
				{
					this.DestroyHandle();
					this.Images.Clear();
				}
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x0600227A RID: 8826 RVA: 0x0004B708 File Offset: 0x0004A708
		// (set) Token: 0x0600227B RID: 8827 RVA: 0x0004B710 File Offset: 0x0004A710
		[DefaultValue(null)]
		[Localizable(false)]
		[Bindable(true)]
		[SRCategory("CatData")]
		[TypeConverter(typeof(StringConverter))]
		[SRDescription("ControlTagDescr")]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x0600227C RID: 8828 RVA: 0x0004B719 File Offset: 0x0004A719
		// (set) Token: 0x0600227D RID: 8829 RVA: 0x0004B721 File Offset: 0x0004A721
		[SRCategory("CatBehavior")]
		[SRDescription("ImageListTransparentColorDescr")]
		public Color TransparentColor
		{
			get
			{
				return this.transparentColor;
			}
			set
			{
				this.transparentColor = value;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x0600227E RID: 8830 RVA: 0x0004B72C File Offset: 0x0004A72C
		private bool UseTransparentColor
		{
			get
			{
				return this.TransparentColor.A > 0;
			}
		}

		// Token: 0x140000DC RID: 220
		// (add) Token: 0x0600227F RID: 8831 RVA: 0x0004B74A File Offset: 0x0004A74A
		// (remove) Token: 0x06002280 RID: 8832 RVA: 0x0004B763 File Offset: 0x0004A763
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ImageListOnRecreateHandleDescr")]
		public event EventHandler RecreateHandle
		{
			add
			{
				this.recreateHandler = (EventHandler)Delegate.Combine(this.recreateHandler, value);
			}
			remove
			{
				this.recreateHandler = (EventHandler)Delegate.Remove(this.recreateHandler, value);
			}
		}

		// Token: 0x140000DD RID: 221
		// (add) Token: 0x06002281 RID: 8833 RVA: 0x0004B77C File Offset: 0x0004A77C
		// (remove) Token: 0x06002282 RID: 8834 RVA: 0x0004B795 File Offset: 0x0004A795
		internal event EventHandler ChangeHandle
		{
			add
			{
				this.changeHandler = (EventHandler)Delegate.Combine(this.changeHandler, value);
			}
			remove
			{
				this.changeHandler = (EventHandler)Delegate.Remove(this.changeHandler, value);
			}
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x0004B7B0 File Offset: 0x0004A7B0
		private Bitmap CreateBitmap(ImageList.Original original, out bool ownsBitmap)
		{
			Color customTransparentColor = this.transparentColor;
			ownsBitmap = false;
			if ((original.options & ImageList.OriginalOptions.CustomTransparentColor) != ImageList.OriginalOptions.Default)
			{
				customTransparentColor = original.customTransparentColor;
			}
			Bitmap bitmap;
			if (original.image is Bitmap)
			{
				bitmap = (Bitmap)original.image;
			}
			else if (original.image is Icon)
			{
				bitmap = ((Icon)original.image).ToBitmap();
				ownsBitmap = true;
			}
			else
			{
				bitmap = new Bitmap((Image)original.image);
				ownsBitmap = true;
			}
			if (customTransparentColor.A > 0)
			{
				Bitmap bitmap2 = bitmap;
				bitmap = (Bitmap)bitmap.Clone();
				bitmap.MakeTransparent(customTransparentColor);
				if (ownsBitmap)
				{
					bitmap2.Dispose();
				}
				ownsBitmap = true;
			}
			Size size = bitmap.Size;
			if ((original.options & ImageList.OriginalOptions.ImageStrip) != ImageList.OriginalOptions.Default)
			{
				if (size.Width == 0 || size.Width % this.imageSize.Width != 0)
				{
					throw new ArgumentException(SR.GetString("ImageListStripBadWidth"), "original");
				}
				if (size.Height != this.imageSize.Height)
				{
					throw new ArgumentException(SR.GetString("ImageListImageTooShort"), "original");
				}
			}
			else if (!size.Equals(this.ImageSize))
			{
				Bitmap bitmap3 = bitmap;
				bitmap = new Bitmap(bitmap3, this.ImageSize);
				if (ownsBitmap)
				{
					bitmap3.Dispose();
				}
				ownsBitmap = true;
			}
			return bitmap;
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x0004B900 File Offset: 0x0004A900
		private int AddIconToHandle(ImageList.Original original, Icon icon)
		{
			int num2;
			try
			{
				int num = SafeNativeMethods.ImageList_ReplaceIcon(new HandleRef(this, this.Handle), -1, new HandleRef(icon, icon.Handle));
				if (num == -1)
				{
					throw new InvalidOperationException(SR.GetString("ImageListAddFailed"));
				}
				num2 = num;
			}
			finally
			{
				if ((original.options & ImageList.OriginalOptions.OwnsImage) != ImageList.OriginalOptions.Default)
				{
					icon.Dispose();
				}
			}
			return num2;
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x0004B968 File Offset: 0x0004A968
		private int AddToHandle(ImageList.Original original, Bitmap bitmap)
		{
			IntPtr intPtr = ControlPaint.CreateHBitmapTransparencyMask(bitmap);
			IntPtr intPtr2 = ControlPaint.CreateHBitmapColorMask(bitmap, intPtr);
			int num = SafeNativeMethods.ImageList_Add(new HandleRef(this, this.Handle), new HandleRef(null, intPtr2), new HandleRef(null, intPtr));
			SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr2));
			SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
			if (num == -1)
			{
				throw new InvalidOperationException(SR.GetString("ImageListAddFailed"));
			}
			return num;
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x0004B9D4 File Offset: 0x0004A9D4
		private void CreateHandle()
		{
			this.nativeImageList = null;
			int num = 1;
			ColorDepth colorDepth = this.colorDepth;
			if (colorDepth <= ColorDepth.Depth8Bit)
			{
				if (colorDepth != ColorDepth.Depth4Bit)
				{
					if (colorDepth == ColorDepth.Depth8Bit)
					{
						num |= 8;
					}
				}
				else
				{
					num |= 4;
				}
			}
			else if (colorDepth != ColorDepth.Depth16Bit)
			{
				if (colorDepth != ColorDepth.Depth24Bit)
				{
					if (colorDepth == ColorDepth.Depth32Bit)
					{
						num |= 32;
					}
				}
				else
				{
					num |= 24;
				}
			}
			else
			{
				num |= 16;
			}
			IntPtr intPtr = UnsafeNativeMethods.ThemingScope.Activate();
			try
			{
				SafeNativeMethods.InitCommonControls();
				this.nativeImageList = new ImageList.NativeImageList(SafeNativeMethods.ImageList_Create(this.imageSize.Width, this.imageSize.Height, num, 4, 4));
			}
			finally
			{
				UnsafeNativeMethods.ThemingScope.Deactivate(intPtr);
			}
			if (this.Handle == IntPtr.Zero)
			{
				throw new InvalidOperationException(SR.GetString("ImageListCreateFailed"));
			}
			SafeNativeMethods.ImageList_SetBkColor(new HandleRef(this, this.Handle), -1);
			for (int i = 0; i < this.originals.Count; i++)
			{
				ImageList.Original original = (ImageList.Original)this.originals[i];
				if (original.image is Icon)
				{
					this.AddIconToHandle(original, (Icon)original.image);
				}
				else
				{
					bool flag = false;
					Bitmap bitmap = this.CreateBitmap(original, out flag);
					this.AddToHandle(original, bitmap);
					if (flag)
					{
						bitmap.Dispose();
					}
				}
			}
			this.originals = null;
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x0004BB30 File Offset: 0x0004AB30
		private void DestroyHandle()
		{
			if (this.HandleCreated)
			{
				this.nativeImageList = null;
				this.originals = new ArrayList();
			}
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x0004BB4C File Offset: 0x0004AB4C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.originals != null)
				{
					foreach (object obj in this.originals)
					{
						ImageList.Original original = (ImageList.Original)obj;
						if ((original.options & ImageList.OriginalOptions.OwnsImage) != ImageList.OriginalOptions.Default)
						{
							((IDisposable)original.image).Dispose();
						}
					}
				}
				this.DestroyHandle();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x0004BBD0 File Offset: 0x0004ABD0
		public void Draw(Graphics g, Point pt, int index)
		{
			this.Draw(g, pt.X, pt.Y, index);
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x0004BBE8 File Offset: 0x0004ABE8
		public void Draw(Graphics g, int x, int y, int index)
		{
			this.Draw(g, x, y, this.imageSize.Width, this.imageSize.Height, index);
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x0004BC0C File Offset: 0x0004AC0C
		public void Draw(Graphics g, int x, int y, int width, int height, int index)
		{
			if (index < 0 || index >= this.Images.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			IntPtr hdc = g.GetHdc();
			try
			{
				SafeNativeMethods.ImageList_DrawEx(new HandleRef(this, this.Handle), index, new HandleRef(g, hdc), x, y, width, height, -1, -1, 1);
			}
			finally
			{
				g.ReleaseHdcInternal(hdc);
			}
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x0004BCA8 File Offset: 0x0004ACA8
		private void CopyBitmapData(BitmapData sourceData, BitmapData targetData)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < targetData.Height; i++)
			{
				IntPtr intPtr;
				IntPtr intPtr2;
				if (IntPtr.Size == 4)
				{
					intPtr = new IntPtr(sourceData.Scan0.ToInt32() + num);
					intPtr2 = new IntPtr(targetData.Scan0.ToInt32() + num2);
				}
				else
				{
					intPtr = new IntPtr(sourceData.Scan0.ToInt64() + (long)num);
					intPtr2 = new IntPtr(targetData.Scan0.ToInt64() + (long)num2);
				}
				UnsafeNativeMethods.CopyMemory(new HandleRef(this, intPtr2), new HandleRef(this, intPtr), Math.Abs(targetData.Stride));
				num += sourceData.Stride;
				num2 += targetData.Stride;
			}
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x0004BD70 File Offset: 0x0004AD70
		private unsafe static bool BitmapHasAlpha(BitmapData bmpData)
		{
			if (bmpData.PixelFormat != PixelFormat.Format32bppArgb && bmpData.PixelFormat != PixelFormat.Format32bppRgb)
			{
				return false;
			}
			bool flag = false;
			for (int i = 0; i < bmpData.Height; i++)
			{
				int num = i * bmpData.Stride;
				for (int j = 3; j < bmpData.Width * 4; j += 4)
				{
					byte* ptr = (byte*)((byte*)bmpData.Scan0.ToPointer() + num) + j;
					if (*ptr != 0)
					{
						return true;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x0004BDE8 File Offset: 0x0004ADE8
		private Bitmap GetBitmap(int index)
		{
			if (index < 0 || index >= this.Images.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			Bitmap bitmap = null;
			if (this.ColorDepth == ColorDepth.Depth32Bit)
			{
				NativeMethods.IMAGEINFO imageinfo = new NativeMethods.IMAGEINFO();
				if (SafeNativeMethods.ImageList_GetImageInfo(new HandleRef(this, this.Handle), index, imageinfo))
				{
					Bitmap bitmap2 = null;
					BitmapData bitmapData = null;
					BitmapData bitmapData2 = null;
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						bitmap2 = Image.FromHbitmap(imageinfo.hbmImage);
						bitmapData = bitmap2.LockBits(new Rectangle(imageinfo.rcImage_left, imageinfo.rcImage_top, imageinfo.rcImage_right - imageinfo.rcImage_left, imageinfo.rcImage_bottom - imageinfo.rcImage_top), ImageLockMode.ReadOnly, bitmap2.PixelFormat);
						int stride = bitmapData.Stride;
						int height = this.imageSize.Height;
						if (ImageList.BitmapHasAlpha(bitmapData))
						{
							bitmap = new Bitmap(this.imageSize.Width, this.imageSize.Height, PixelFormat.Format32bppArgb);
							bitmapData2 = bitmap.LockBits(new Rectangle(0, 0, this.imageSize.Width, this.imageSize.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
							this.CopyBitmapData(bitmapData, bitmapData2);
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
						if (bitmap2 != null)
						{
							if (bitmapData != null)
							{
								bitmap2.UnlockBits(bitmapData);
							}
							bitmap2.Dispose();
						}
						if (bitmap != null && bitmapData2 != null)
						{
							bitmap.UnlockBits(bitmapData2);
						}
					}
				}
			}
			if (bitmap == null)
			{
				bitmap = new Bitmap(this.imageSize.Width, this.imageSize.Height);
				Graphics graphics = Graphics.FromImage(bitmap);
				try
				{
					IntPtr hdc = graphics.GetHdc();
					try
					{
						SafeNativeMethods.ImageList_DrawEx(new HandleRef(this, this.Handle), index, new HandleRef(graphics, hdc), 0, 0, this.imageSize.Width, this.imageSize.Height, -1, -1, 1);
					}
					finally
					{
						graphics.ReleaseHdcInternal(hdc);
					}
				}
				finally
				{
					graphics.Dispose();
				}
			}
			bitmap.MakeTransparent(ImageList.fakeTransparencyColor);
			return bitmap;
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x0004C010 File Offset: 0x0004B010
		private void OnRecreateHandle(EventArgs eventargs)
		{
			if (this.recreateHandler != null)
			{
				this.recreateHandler(this, eventargs);
			}
		}

		// Token: 0x06002290 RID: 8848 RVA: 0x0004C027 File Offset: 0x0004B027
		private void OnChangeHandle(EventArgs eventargs)
		{
			if (this.changeHandler != null)
			{
				this.changeHandler(this, eventargs);
			}
		}

		// Token: 0x06002291 RID: 8849 RVA: 0x0004C040 File Offset: 0x0004B040
		private void PerformRecreateHandle(string reason)
		{
			if (!this.HandleCreated)
			{
				return;
			}
			if (this.originals == null || this.Images.Empty)
			{
				this.originals = new ArrayList();
			}
			if (this.originals == null)
			{
				throw new InvalidOperationException(SR.GetString("ImageListCantRecreate", new object[] { reason }));
			}
			this.DestroyHandle();
			this.CreateHandle();
			this.OnRecreateHandle(new EventArgs());
		}

		// Token: 0x06002292 RID: 8850 RVA: 0x0004C0B1 File Offset: 0x0004B0B1
		private void ResetImageSize()
		{
			this.ImageSize = ImageList.DefaultImageSize;
		}

		// Token: 0x06002293 RID: 8851 RVA: 0x0004C0BE File Offset: 0x0004B0BE
		private void ResetTransparentColor()
		{
			this.TransparentColor = Color.LightGray;
		}

		// Token: 0x06002294 RID: 8852 RVA: 0x0004C0CC File Offset: 0x0004B0CC
		private bool ShouldSerializeTransparentColor()
		{
			return !this.TransparentColor.Equals(Color.LightGray);
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x0004C0FC File Offset: 0x0004B0FC
		public override string ToString()
		{
			string text = base.ToString();
			if (this.Images != null)
			{
				return string.Concat(new string[]
				{
					text,
					" Images.Count: ",
					this.Images.Count.ToString(CultureInfo.CurrentCulture),
					", ImageSize: ",
					this.ImageSize.ToString()
				});
			}
			return text;
		}

		// Token: 0x0400150E RID: 5390
		private const int INITIAL_CAPACITY = 4;

		// Token: 0x0400150F RID: 5391
		private const int GROWBY = 4;

		// Token: 0x04001510 RID: 5392
		private static Color fakeTransparencyColor = Color.FromArgb(13, 11, 12);

		// Token: 0x04001511 RID: 5393
		private static Size DefaultImageSize = new Size(16, 16);

		// Token: 0x04001512 RID: 5394
		private ImageList.NativeImageList nativeImageList;

		// Token: 0x04001513 RID: 5395
		private ColorDepth colorDepth = ColorDepth.Depth8Bit;

		// Token: 0x04001514 RID: 5396
		private Color transparentColor = Color.Transparent;

		// Token: 0x04001515 RID: 5397
		private Size imageSize = ImageList.DefaultImageSize;

		// Token: 0x04001516 RID: 5398
		private ImageList.ImageCollection imageCollection;

		// Token: 0x04001517 RID: 5399
		private object userData;

		// Token: 0x04001518 RID: 5400
		private IList originals = new ArrayList();

		// Token: 0x04001519 RID: 5401
		private EventHandler recreateHandler;

		// Token: 0x0400151A RID: 5402
		private EventHandler changeHandler;

		// Token: 0x0400151B RID: 5403
		private bool inAddRange;

		// Token: 0x02000280 RID: 640
		internal class Indexer
		{
			// Token: 0x17000536 RID: 1334
			// (get) Token: 0x06002297 RID: 8855 RVA: 0x0004C18D File Offset: 0x0004B18D
			// (set) Token: 0x06002298 RID: 8856 RVA: 0x0004C195 File Offset: 0x0004B195
			public virtual ImageList ImageList
			{
				get
				{
					return this.imageList;
				}
				set
				{
					this.imageList = value;
				}
			}

			// Token: 0x17000537 RID: 1335
			// (get) Token: 0x06002299 RID: 8857 RVA: 0x0004C19E File Offset: 0x0004B19E
			// (set) Token: 0x0600229A RID: 8858 RVA: 0x0004C1A6 File Offset: 0x0004B1A6
			public virtual string Key
			{
				get
				{
					return this.key;
				}
				set
				{
					this.index = -1;
					this.key = ((value == null) ? string.Empty : value);
					this.useIntegerIndex = false;
				}
			}

			// Token: 0x17000538 RID: 1336
			// (get) Token: 0x0600229B RID: 8859 RVA: 0x0004C1C7 File Offset: 0x0004B1C7
			// (set) Token: 0x0600229C RID: 8860 RVA: 0x0004C1CF File Offset: 0x0004B1CF
			public virtual int Index
			{
				get
				{
					return this.index;
				}
				set
				{
					this.key = string.Empty;
					this.index = value;
					this.useIntegerIndex = true;
				}
			}

			// Token: 0x17000539 RID: 1337
			// (get) Token: 0x0600229D RID: 8861 RVA: 0x0004C1EA File Offset: 0x0004B1EA
			public virtual int ActualIndex
			{
				get
				{
					if (this.useIntegerIndex)
					{
						return this.Index;
					}
					if (this.ImageList != null)
					{
						return this.ImageList.Images.IndexOfKey(this.Key);
					}
					return -1;
				}
			}

			// Token: 0x0400151C RID: 5404
			private string key = string.Empty;

			// Token: 0x0400151D RID: 5405
			private int index = -1;

			// Token: 0x0400151E RID: 5406
			private bool useIntegerIndex = true;

			// Token: 0x0400151F RID: 5407
			private ImageList imageList;
		}

		// Token: 0x02000281 RID: 641
		internal class NativeImageList
		{
			// Token: 0x0600229F RID: 8863 RVA: 0x0004C23C File Offset: 0x0004B23C
			internal NativeImageList(IntPtr himl)
			{
				this.himl = himl;
			}

			// Token: 0x1700053A RID: 1338
			// (get) Token: 0x060022A0 RID: 8864 RVA: 0x0004C24B File Offset: 0x0004B24B
			internal IntPtr Handle
			{
				get
				{
					return this.himl;
				}
			}

			// Token: 0x060022A1 RID: 8865 RVA: 0x0004C254 File Offset: 0x0004B254
			~NativeImageList()
			{
				if (this.himl != IntPtr.Zero)
				{
					SafeNativeMethods.ImageList_Destroy(new HandleRef(null, this.himl));
					this.himl = IntPtr.Zero;
				}
			}

			// Token: 0x04001520 RID: 5408
			private IntPtr himl;
		}

		// Token: 0x02000282 RID: 642
		private class Original
		{
			// Token: 0x060022A2 RID: 8866 RVA: 0x0004C2AC File Offset: 0x0004B2AC
			internal Original(object image, ImageList.OriginalOptions options)
				: this(image, options, Color.Transparent)
			{
			}

			// Token: 0x060022A3 RID: 8867 RVA: 0x0004C2BB File Offset: 0x0004B2BB
			internal Original(object image, ImageList.OriginalOptions options, int nImages)
				: this(image, options, Color.Transparent)
			{
				this.nImages = nImages;
			}

			// Token: 0x060022A4 RID: 8868 RVA: 0x0004C2D4 File Offset: 0x0004B2D4
			internal Original(object image, ImageList.OriginalOptions options, Color customTransparentColor)
			{
				if (!(image is Icon) && !(image is Image))
				{
					throw new InvalidOperationException(SR.GetString("ImageListEntryType"));
				}
				this.image = image;
				this.options = options;
				this.customTransparentColor = customTransparentColor;
				ImageList.OriginalOptions originalOptions = options & ImageList.OriginalOptions.CustomTransparentColor;
			}

			// Token: 0x04001521 RID: 5409
			internal object image;

			// Token: 0x04001522 RID: 5410
			internal ImageList.OriginalOptions options;

			// Token: 0x04001523 RID: 5411
			internal Color customTransparentColor = Color.Transparent;

			// Token: 0x04001524 RID: 5412
			internal int nImages = 1;
		}

		// Token: 0x02000283 RID: 643
		[Flags]
		private enum OriginalOptions
		{
			// Token: 0x04001526 RID: 5414
			Default = 0,
			// Token: 0x04001527 RID: 5415
			ImageStrip = 1,
			// Token: 0x04001528 RID: 5416
			CustomTransparentColor = 2,
			// Token: 0x04001529 RID: 5417
			OwnsImage = 4
		}

		// Token: 0x02000284 RID: 644
		[Editor("System.Windows.Forms.Design.ImageCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public sealed class ImageCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x1700053B RID: 1339
			// (get) Token: 0x060022A5 RID: 8869 RVA: 0x0004C334 File Offset: 0x0004B334
			public StringCollection Keys
			{
				get
				{
					StringCollection stringCollection = new StringCollection();
					for (int i = 0; i < this.imageInfoCollection.Count; i++)
					{
						ImageList.ImageCollection.ImageInfo imageInfo = this.imageInfoCollection[i] as ImageList.ImageCollection.ImageInfo;
						if (imageInfo != null && imageInfo.Name != null && imageInfo.Name.Length != 0)
						{
							stringCollection.Add(imageInfo.Name);
						}
						else
						{
							stringCollection.Add(string.Empty);
						}
					}
					return stringCollection;
				}
			}

			// Token: 0x060022A6 RID: 8870 RVA: 0x0004C3A3 File Offset: 0x0004B3A3
			internal ImageCollection(ImageList owner)
			{
				this.owner = owner;
			}

			// Token: 0x060022A7 RID: 8871 RVA: 0x0004C3C4 File Offset: 0x0004B3C4
			internal void ResetKeys()
			{
				if (this.imageInfoCollection != null)
				{
					this.imageInfoCollection.Clear();
				}
				for (int i = 0; i < this.Count; i++)
				{
					this.imageInfoCollection.Add(new ImageList.ImageCollection.ImageInfo());
				}
			}

			// Token: 0x060022A8 RID: 8872 RVA: 0x0004C406 File Offset: 0x0004B406
			[Conditional("DEBUG")]
			private void AssertInvariant()
			{
			}

			// Token: 0x1700053C RID: 1340
			// (get) Token: 0x060022A9 RID: 8873 RVA: 0x0004C408 File Offset: 0x0004B408
			[Browsable(false)]
			public int Count
			{
				get
				{
					if (this.owner.HandleCreated)
					{
						return SafeNativeMethods.ImageList_GetImageCount(new HandleRef(this.owner, this.owner.Handle));
					}
					int num = 0;
					foreach (object obj in this.owner.originals)
					{
						ImageList.Original original = (ImageList.Original)obj;
						if (original != null)
						{
							num += original.nImages;
						}
					}
					return num;
				}
			}

			// Token: 0x1700053D RID: 1341
			// (get) Token: 0x060022AA RID: 8874 RVA: 0x0004C498 File Offset: 0x0004B498
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x1700053E RID: 1342
			// (get) Token: 0x060022AB RID: 8875 RVA: 0x0004C49B File Offset: 0x0004B49B
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700053F RID: 1343
			// (get) Token: 0x060022AC RID: 8876 RVA: 0x0004C49E File Offset: 0x0004B49E
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000540 RID: 1344
			// (get) Token: 0x060022AD RID: 8877 RVA: 0x0004C4A1 File Offset: 0x0004B4A1
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000541 RID: 1345
			// (get) Token: 0x060022AE RID: 8878 RVA: 0x0004C4A4 File Offset: 0x0004B4A4
			public bool Empty
			{
				get
				{
					return this.Count == 0;
				}
			}

			// Token: 0x17000542 RID: 1346
			[Browsable(false)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public Image this[int index]
			{
				get
				{
					if (index < 0 || index >= this.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return this.owner.GetBitmap(index);
				}
				set
				{
					if (index < 0 || index >= this.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (value == null)
					{
						throw new ArgumentNullException("value");
					}
					if (!(value is Bitmap))
					{
						throw new ArgumentException(SR.GetString("ImageListBitmap"));
					}
					Bitmap bitmap = (Bitmap)value;
					bool flag = false;
					if (this.owner.UseTransparentColor)
					{
						bitmap = (Bitmap)bitmap.Clone();
						bitmap.MakeTransparent(this.owner.transparentColor);
						flag = true;
					}
					try
					{
						IntPtr intPtr = ControlPaint.CreateHBitmapTransparencyMask(bitmap);
						IntPtr intPtr2 = ControlPaint.CreateHBitmapColorMask(bitmap, intPtr);
						bool flag2 = SafeNativeMethods.ImageList_Replace(new HandleRef(this.owner, this.owner.Handle), index, new HandleRef(null, intPtr2), new HandleRef(null, intPtr));
						SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr2));
						SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
						if (!flag2)
						{
							throw new InvalidOperationException(SR.GetString("ImageListReplaceFailed"));
						}
					}
					finally
					{
						if (flag)
						{
							bitmap.Dispose();
						}
					}
				}
			}

			// Token: 0x17000543 RID: 1347
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					if (value is Image)
					{
						this[index] = (Image)value;
						return;
					}
					throw new ArgumentException(SR.GetString("ImageListBadImage"), "value");
				}
			}

			// Token: 0x17000544 RID: 1348
			public Image this[string key]
			{
				get
				{
					if (key == null || key.Length == 0)
					{
						return null;
					}
					int num = this.IndexOfKey(key);
					if (this.IsValidIndex(num))
					{
						return this[num];
					}
					return null;
				}
			}

			// Token: 0x060022B4 RID: 8884 RVA: 0x0004C6AC File Offset: 0x0004B6AC
			public void Add(string key, Image image)
			{
				ImageList.ImageCollection.ImageInfo imageInfo = new ImageList.ImageCollection.ImageInfo();
				imageInfo.Name = key;
				ImageList.Original original = new ImageList.Original(image, ImageList.OriginalOptions.Default);
				this.Add(original, imageInfo);
			}

			// Token: 0x060022B5 RID: 8885 RVA: 0x0004C6D8 File Offset: 0x0004B6D8
			public void Add(string key, Icon icon)
			{
				ImageList.ImageCollection.ImageInfo imageInfo = new ImageList.ImageCollection.ImageInfo();
				imageInfo.Name = key;
				ImageList.Original original = new ImageList.Original(icon, ImageList.OriginalOptions.Default);
				this.Add(original, imageInfo);
			}

			// Token: 0x060022B6 RID: 8886 RVA: 0x0004C703 File Offset: 0x0004B703
			int IList.Add(object value)
			{
				if (value is Image)
				{
					this.Add((Image)value);
					return this.Count - 1;
				}
				throw new ArgumentException(SR.GetString("ImageListBadImage"), "value");
			}

			// Token: 0x060022B7 RID: 8887 RVA: 0x0004C736 File Offset: 0x0004B736
			public void Add(Icon value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.Add(new ImageList.Original(value.Clone(), ImageList.OriginalOptions.OwnsImage), null);
			}

			// Token: 0x060022B8 RID: 8888 RVA: 0x0004C75C File Offset: 0x0004B75C
			public void Add(Image value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				ImageList.Original original = new ImageList.Original(value, ImageList.OriginalOptions.Default);
				this.Add(original, null);
			}

			// Token: 0x060022B9 RID: 8889 RVA: 0x0004C788 File Offset: 0x0004B788
			public int Add(Image value, Color transparentColor)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				ImageList.Original original = new ImageList.Original(value, ImageList.OriginalOptions.CustomTransparentColor, transparentColor);
				return this.Add(original, null);
			}

			// Token: 0x060022BA RID: 8890 RVA: 0x0004C7B4 File Offset: 0x0004B7B4
			private int Add(ImageList.Original original, ImageList.ImageCollection.ImageInfo imageInfo)
			{
				if (original == null || original.image == null)
				{
					throw new ArgumentNullException("value");
				}
				int num = -1;
				if (original.image is Bitmap)
				{
					if (this.owner.originals != null)
					{
						num = this.owner.originals.Add(original);
					}
					if (this.owner.HandleCreated)
					{
						bool flag = false;
						Bitmap bitmap = this.owner.CreateBitmap(original, out flag);
						num = this.owner.AddToHandle(original, bitmap);
						if (flag)
						{
							bitmap.Dispose();
						}
					}
				}
				else
				{
					if (!(original.image is Icon))
					{
						throw new ArgumentException(SR.GetString("ImageListBitmap"));
					}
					if (this.owner.originals != null)
					{
						num = this.owner.originals.Add(original);
					}
					if (this.owner.HandleCreated)
					{
						num = this.owner.AddIconToHandle(original, (Icon)original.image);
					}
				}
				if ((original.options & ImageList.OriginalOptions.ImageStrip) != ImageList.OriginalOptions.Default)
				{
					for (int i = 0; i < original.nImages; i++)
					{
						this.imageInfoCollection.Add(new ImageList.ImageCollection.ImageInfo());
					}
				}
				else
				{
					if (imageInfo == null)
					{
						imageInfo = new ImageList.ImageCollection.ImageInfo();
					}
					this.imageInfoCollection.Add(imageInfo);
				}
				if (!this.owner.inAddRange)
				{
					this.owner.OnChangeHandle(new EventArgs());
				}
				return num;
			}

			// Token: 0x060022BB RID: 8891 RVA: 0x0004C908 File Offset: 0x0004B908
			public void AddRange(Image[] images)
			{
				if (images == null)
				{
					throw new ArgumentNullException("images");
				}
				this.owner.inAddRange = true;
				foreach (Image image in images)
				{
					this.Add(image);
				}
				this.owner.inAddRange = false;
				this.owner.OnChangeHandle(new EventArgs());
			}

			// Token: 0x060022BC RID: 8892 RVA: 0x0004C968 File Offset: 0x0004B968
			public int AddStrip(Image value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Width == 0 || value.Width % this.owner.ImageSize.Width != 0)
				{
					throw new ArgumentException(SR.GetString("ImageListStripBadWidth"), "value");
				}
				if (value.Height != this.owner.ImageSize.Height)
				{
					throw new ArgumentException(SR.GetString("ImageListImageTooShort"), "value");
				}
				int num = value.Width / this.owner.ImageSize.Width;
				ImageList.Original original = new ImageList.Original(value, ImageList.OriginalOptions.ImageStrip, num);
				return this.Add(original, null);
			}

			// Token: 0x060022BD RID: 8893 RVA: 0x0004CA1C File Offset: 0x0004BA1C
			public void Clear()
			{
				if (this.owner.originals != null)
				{
					this.owner.originals.Clear();
				}
				this.imageInfoCollection.Clear();
				if (this.owner.HandleCreated)
				{
					SafeNativeMethods.ImageList_Remove(new HandleRef(this.owner, this.owner.Handle), -1);
				}
				this.owner.OnChangeHandle(new EventArgs());
			}

			// Token: 0x060022BE RID: 8894 RVA: 0x0004CA8B File Offset: 0x0004BA8B
			[EditorBrowsable(EditorBrowsableState.Never)]
			public bool Contains(Image image)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060022BF RID: 8895 RVA: 0x0004CA92 File Offset: 0x0004BA92
			bool IList.Contains(object image)
			{
				return image is Image && this.Contains((Image)image);
			}

			// Token: 0x060022C0 RID: 8896 RVA: 0x0004CAAA File Offset: 0x0004BAAA
			public bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			// Token: 0x060022C1 RID: 8897 RVA: 0x0004CAB9 File Offset: 0x0004BAB9
			[EditorBrowsable(EditorBrowsableState.Never)]
			public int IndexOf(Image image)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060022C2 RID: 8898 RVA: 0x0004CAC0 File Offset: 0x0004BAC0
			int IList.IndexOf(object image)
			{
				if (image is Image)
				{
					return this.IndexOf((Image)image);
				}
				return -1;
			}

			// Token: 0x060022C3 RID: 8899 RVA: 0x0004CAD8 File Offset: 0x0004BAD8
			public int IndexOfKey(string key)
			{
				if (key == null || key.Length == 0)
				{
					return -1;
				}
				if (this.IsValidIndex(this.lastAccessedIndex) && this.imageInfoCollection[this.lastAccessedIndex] != null && WindowsFormsUtils.SafeCompareStrings(((ImageList.ImageCollection.ImageInfo)this.imageInfoCollection[this.lastAccessedIndex]).Name, key, true))
				{
					return this.lastAccessedIndex;
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (this.imageInfoCollection[i] != null && WindowsFormsUtils.SafeCompareStrings(((ImageList.ImageCollection.ImageInfo)this.imageInfoCollection[i]).Name, key, true))
					{
						this.lastAccessedIndex = i;
						return i;
					}
				}
				this.lastAccessedIndex = -1;
				return -1;
			}

			// Token: 0x060022C4 RID: 8900 RVA: 0x0004CB8D File Offset: 0x0004BB8D
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060022C5 RID: 8901 RVA: 0x0004CB94 File Offset: 0x0004BB94
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			// Token: 0x060022C6 RID: 8902 RVA: 0x0004CBA8 File Offset: 0x0004BBA8
			void ICollection.CopyTo(Array dest, int index)
			{
				for (int i = 0; i < this.Count; i++)
				{
					dest.SetValue(this.owner.GetBitmap(i), index++);
				}
			}

			// Token: 0x060022C7 RID: 8903 RVA: 0x0004CBE0 File Offset: 0x0004BBE0
			public IEnumerator GetEnumerator()
			{
				Image[] array = new Image[this.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.owner.GetBitmap(i);
				}
				return array.GetEnumerator();
			}

			// Token: 0x060022C8 RID: 8904 RVA: 0x0004CC1C File Offset: 0x0004BC1C
			[EditorBrowsable(EditorBrowsableState.Never)]
			public void Remove(Image image)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060022C9 RID: 8905 RVA: 0x0004CC23 File Offset: 0x0004BC23
			void IList.Remove(object image)
			{
				if (image is Image)
				{
					this.Remove((Image)image);
					this.owner.OnChangeHandle(new EventArgs());
				}
			}

			// Token: 0x060022CA RID: 8906 RVA: 0x0004CC4C File Offset: 0x0004BC4C
			public void RemoveAt(int index)
			{
				if (index < 0 || index >= this.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (!SafeNativeMethods.ImageList_Remove(new HandleRef(this.owner, this.owner.Handle), index))
				{
					throw new InvalidOperationException(SR.GetString("ImageListRemoveFailed"));
				}
				if (this.imageInfoCollection != null && index >= 0 && index < this.imageInfoCollection.Count)
				{
					this.imageInfoCollection.RemoveAt(index);
					this.owner.OnChangeHandle(new EventArgs());
				}
			}

			// Token: 0x060022CB RID: 8907 RVA: 0x0004CD00 File Offset: 0x0004BD00
			public void RemoveByKey(string key)
			{
				int num = this.IndexOfKey(key);
				if (this.IsValidIndex(num))
				{
					this.RemoveAt(num);
				}
			}

			// Token: 0x060022CC RID: 8908 RVA: 0x0004CD28 File Offset: 0x0004BD28
			public void SetKeyName(int index, string name)
			{
				if (!this.IsValidIndex(index))
				{
					throw new IndexOutOfRangeException();
				}
				if (this.imageInfoCollection[index] == null)
				{
					this.imageInfoCollection[index] = new ImageList.ImageCollection.ImageInfo();
				}
				((ImageList.ImageCollection.ImageInfo)this.imageInfoCollection[index]).Name = name;
			}

			// Token: 0x0400152A RID: 5418
			private ImageList owner;

			// Token: 0x0400152B RID: 5419
			private ArrayList imageInfoCollection = new ArrayList();

			// Token: 0x0400152C RID: 5420
			private int lastAccessedIndex = -1;

			// Token: 0x02000285 RID: 645
			internal class ImageInfo
			{
				// Token: 0x17000545 RID: 1349
				// (get) Token: 0x060022CE RID: 8910 RVA: 0x0004CD82 File Offset: 0x0004BD82
				// (set) Token: 0x060022CF RID: 8911 RVA: 0x0004CD8A File Offset: 0x0004BD8A
				public string Name
				{
					get
					{
						return this.name;
					}
					set
					{
						this.name = value;
					}
				}

				// Token: 0x0400152D RID: 5421
				private string name;
			}
		}
	}
}
