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
	// Token: 0x02000030 RID: 48
	[Editor("System.Drawing.Design.ImageEditor, System.Drawing.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[ComVisible(true)]
	[TypeConverter(typeof(ImageConverter))]
	[ImmutableObject(true)]
	[Serializable]
	public abstract class Image : MarshalByRefObject, ISerializable, ICloneable, IDisposable
	{
		// Token: 0x0600013F RID: 319 RVA: 0x00004EC6 File Offset: 0x00003EC6
		internal Image()
		{
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00004ED0 File Offset: 0x00003ED0
		internal Image(SerializationInfo info, StreamingContext context)
		{
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			if (enumerator == null)
			{
				return;
			}
			while (enumerator.MoveNext())
			{
				if (string.Equals(enumerator.Name, "Data", StringComparison.OrdinalIgnoreCase))
				{
					try
					{
						byte[] array = (byte[])enumerator.Value;
						if (array != null)
						{
							this.InitializeFromStream(new MemoryStream(array));
						}
					}
					catch (ExternalException)
					{
					}
					catch (ArgumentException)
					{
					}
					catch (OutOfMemoryException)
					{
					}
					catch (InvalidOperationException)
					{
					}
					catch (NotImplementedException)
					{
					}
					catch (FileNotFoundException)
					{
					}
				}
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00004F84 File Offset: 0x00003F84
		// (set) Token: 0x06000142 RID: 322 RVA: 0x00004F8C File Offset: 0x00003F8C
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		[Localizable(false)]
		[Bindable(true)]
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

		// Token: 0x06000143 RID: 323 RVA: 0x00004F95 File Offset: 0x00003F95
		public static Image FromFile(string filename)
		{
			return Image.FromFile(filename, false);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00004FA0 File Offset: 0x00003FA0
		public static Image FromFile(string filename, bool useEmbeddedColorManagement)
		{
			if (!File.Exists(filename))
			{
				IntSecurity.DemandReadFileIO(filename);
				throw new FileNotFoundException(filename);
			}
			filename = Path.GetFullPath(filename);
			IntPtr zero = IntPtr.Zero;
			int num;
			if (useEmbeddedColorManagement)
			{
				num = SafeNativeMethods.Gdip.GdipLoadImageFromFileICM(filename, out zero);
			}
			else
			{
				num = SafeNativeMethods.Gdip.GdipLoadImageFromFile(filename, out zero);
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
			Image image = Image.CreateImageObject(zero);
			Image.EnsureSave(image, filename, null);
			return image;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00005025 File Offset: 0x00004025
		public static Image FromStream(Stream stream)
		{
			return Image.FromStream(stream, false);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000502E File Offset: 0x0000402E
		public static Image FromStream(Stream stream, bool useEmbeddedColorManagement)
		{
			return Image.FromStream(stream, useEmbeddedColorManagement, true);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00005038 File Offset: 0x00004038
		public static Image FromStream(Stream stream, bool useEmbeddedColorManagement, bool validateImageData)
		{
			if (!validateImageData)
			{
				IntSecurity.UnmanagedCode.Demand();
			}
			if (stream == null)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[] { "stream", "null" }));
			}
			IntPtr zero = IntPtr.Zero;
			int num;
			if (useEmbeddedColorManagement)
			{
				num = SafeNativeMethods.Gdip.GdipLoadImageFromStreamICM(new GPStream(stream), out zero);
			}
			else
			{
				num = SafeNativeMethods.Gdip.GdipLoadImageFromStream(new GPStream(stream), out zero);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			if (validateImageData)
			{
				num = SafeNativeMethods.Gdip.GdipImageForceValidation(new HandleRef(null, zero));
				if (num != 0)
				{
					SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef(null, zero));
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			Image image = Image.CreateImageObject(zero);
			Image.EnsureSave(image, null, stream);
			return image;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000050E8 File Offset: 0x000040E8
		private void InitializeFromStream(Stream stream)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipLoadImageFromStream(new GPStream(stream), out zero);
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
			this.nativeImage = zero;
			int num2 = -1;
			num = SafeNativeMethods.Gdip.GdipGetImageType(new HandleRef(this, this.nativeImage), out num2);
			Image.EnsureSave(this, null, stream);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005166 File Offset: 0x00004166
		internal Image(IntPtr nativeImage)
		{
			this.SetNativeImage(nativeImage);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005178 File Offset: 0x00004178
		public object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneImage(new HandleRef(this, this.nativeImage), out zero);
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
			return Image.CreateImageObject(zero);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000051D3 File Offset: 0x000041D3
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000051E4 File Offset: 0x000041E4
		protected virtual void Dispose(bool disposing)
		{
			if (this.nativeImage != IntPtr.Zero)
			{
				try
				{
					SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef(this, this.nativeImage));
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
					this.nativeImage = IntPtr.Zero;
				}
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00005250 File Offset: 0x00004250
		~Image()
		{
			this.Dispose(false);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00005280 File Offset: 0x00004280
		internal static void EnsureSave(Image image, string filename, Stream dataStream)
		{
			if (image.RawFormat.Equals(ImageFormat.Gif))
			{
				bool flag = false;
				Guid[] frameDimensionsList = image.FrameDimensionsList;
				foreach (Guid guid in frameDimensionsList)
				{
					FrameDimension frameDimension = new FrameDimension(guid);
					if (frameDimension.Equals(FrameDimension.Time))
					{
						flag = image.GetFrameCount(FrameDimension.Time) > 1;
						break;
					}
				}
				if (flag)
				{
					try
					{
						Stream stream = null;
						long num = 0L;
						if (dataStream != null)
						{
							num = dataStream.Position;
							dataStream.Position = 0L;
						}
						try
						{
							if (dataStream == null)
							{
								dataStream = (stream = File.OpenRead(filename));
							}
							image.rawData = new byte[(int)dataStream.Length];
							dataStream.Read(image.rawData, 0, (int)dataStream.Length);
						}
						finally
						{
							if (stream != null)
							{
								stream.Close();
							}
							else
							{
								dataStream.Position = num;
							}
						}
					}
					catch (UnauthorizedAccessException)
					{
					}
					catch (DirectoryNotFoundException)
					{
					}
					catch (IOException)
					{
					}
					catch (NotSupportedException)
					{
					}
					catch (ObjectDisposedException)
					{
					}
					catch (ArgumentException)
					{
					}
				}
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000053CC File Offset: 0x000043CC
		internal static Image CreateImageObject(IntPtr nativeImage)
		{
			int num = -1;
			int num2 = SafeNativeMethods.Gdip.GdipGetImageType(new HandleRef(null, nativeImage), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			Image image;
			switch (num)
			{
			case 1:
				image = Bitmap.FromGDIplus(nativeImage);
				break;
			case 2:
				image = Metafile.FromGDIplus(nativeImage);
				break;
			default:
				throw new ArgumentException(SR.GetString("InvalidImage"));
			}
			return image;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000542C File Offset: 0x0000442C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				this.Save(memoryStream);
				si.AddValue("Data", memoryStream.ToArray(), typeof(byte[]));
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00005480 File Offset: 0x00004480
		public EncoderParameters GetEncoderParameterList(Guid encoder)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipGetEncoderParameterListSize(new HandleRef(this, this.nativeImage), ref encoder, out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			if (num2 <= 0)
			{
				return null;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(num2);
			num = SafeNativeMethods.Gdip.GdipGetEncoderParameterList(new HandleRef(this, this.nativeImage), ref encoder, num2, intPtr);
			EncoderParameters encoderParameters;
			try
			{
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				encoderParameters = EncoderParameters.ConvertFromMemory(intPtr);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return encoderParameters;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000054FC File Offset: 0x000044FC
		public void Save(string filename)
		{
			this.Save(filename, this.RawFormat);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000550C File Offset: 0x0000450C
		public void Save(string filename, ImageFormat format)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			ImageCodecInfo imageCodecInfo = format.FindEncoder();
			if (imageCodecInfo == null)
			{
				imageCodecInfo = ImageFormat.Png.FindEncoder();
			}
			this.Save(filename, imageCodecInfo, null);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00005548 File Offset: 0x00004548
		public void Save(string filename, ImageCodecInfo encoder, EncoderParameters encoderParams)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			if (encoder == null)
			{
				throw new ArgumentNullException("encoder");
			}
			IntSecurity.DemandWriteFileIO(filename);
			IntPtr intPtr = IntPtr.Zero;
			if (encoderParams != null)
			{
				this.rawData = null;
				intPtr = encoderParams.ConvertToMemory();
			}
			int num = 0;
			try
			{
				Guid clsid = encoder.Clsid;
				bool flag = false;
				if (this.rawData != null)
				{
					ImageCodecInfo imageCodecInfo = this.RawFormat.FindEncoder();
					if (imageCodecInfo != null && imageCodecInfo.Clsid == clsid)
					{
						using (FileStream fileStream = File.OpenWrite(filename))
						{
							fileStream.Write(this.rawData, 0, this.rawData.Length);
							flag = true;
						}
					}
				}
				if (!flag)
				{
					num = SafeNativeMethods.Gdip.GdipSaveImageToFile(new HandleRef(this, this.nativeImage), filename, ref clsid, new HandleRef(encoderParams, intPtr));
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000564C File Offset: 0x0000464C
		internal void Save(MemoryStream stream)
		{
			ImageFormat imageFormat = this.RawFormat;
			if (imageFormat == ImageFormat.Jpeg)
			{
				imageFormat = ImageFormat.Png;
			}
			ImageCodecInfo imageCodecInfo = imageFormat.FindEncoder();
			if (imageCodecInfo == null)
			{
				imageCodecInfo = ImageFormat.Png.FindEncoder();
			}
			this.Save(stream, imageCodecInfo, null);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000568C File Offset: 0x0000468C
		public void Save(Stream stream, ImageFormat format)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			ImageCodecInfo imageCodecInfo = format.FindEncoder();
			this.Save(stream, imageCodecInfo, null);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000056B8 File Offset: 0x000046B8
		public void Save(Stream stream, ImageCodecInfo encoder, EncoderParameters encoderParams)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (encoder == null)
			{
				throw new ArgumentNullException("encoder");
			}
			IntPtr intPtr = IntPtr.Zero;
			if (encoderParams != null)
			{
				this.rawData = null;
				intPtr = encoderParams.ConvertToMemory();
			}
			int num = 0;
			try
			{
				Guid clsid = encoder.Clsid;
				bool flag = false;
				if (this.rawData != null)
				{
					ImageCodecInfo imageCodecInfo = this.RawFormat.FindEncoder();
					if (imageCodecInfo != null && imageCodecInfo.Clsid == clsid)
					{
						stream.Write(this.rawData, 0, this.rawData.Length);
						flag = true;
					}
				}
				if (!flag)
				{
					num = SafeNativeMethods.Gdip.GdipSaveImageToStream(new HandleRef(this, this.nativeImage), new UnsafeNativeMethods.ComStreamFromDataStream(stream), ref clsid, new HandleRef(encoderParams, intPtr));
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005798 File Offset: 0x00004798
		public void SaveAdd(EncoderParameters encoderParams)
		{
			IntPtr intPtr = IntPtr.Zero;
			if (encoderParams != null)
			{
				intPtr = encoderParams.ConvertToMemory();
			}
			this.rawData = null;
			int num = SafeNativeMethods.Gdip.GdipSaveAdd(new HandleRef(this, this.nativeImage), new HandleRef(encoderParams, intPtr));
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000057F4 File Offset: 0x000047F4
		public void SaveAdd(Image image, EncoderParameters encoderParams)
		{
			IntPtr intPtr = IntPtr.Zero;
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			if (encoderParams != null)
			{
				intPtr = encoderParams.ConvertToMemory();
			}
			this.rawData = null;
			int num = SafeNativeMethods.Gdip.GdipSaveAddImage(new HandleRef(this, this.nativeImage), new HandleRef(image, image.nativeImage), new HandleRef(encoderParams, intPtr));
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00005868 File Offset: 0x00004868
		private SizeF _GetPhysicalDimension()
		{
			float num2;
			float num3;
			int num = SafeNativeMethods.Gdip.GdipGetImageDimension(new HandleRef(this, this.nativeImage), out num2, out num3);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new SizeF(num2, num3);
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600015B RID: 347 RVA: 0x0000589C File Offset: 0x0000489C
		public SizeF PhysicalDimension
		{
			get
			{
				return this._GetPhysicalDimension();
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000058A4 File Offset: 0x000048A4
		public Size Size
		{
			get
			{
				return new Size(this.Width, this.Height);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600015D RID: 349 RVA: 0x000058B8 File Offset: 0x000048B8
		[Browsable(false)]
		[DefaultValue(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Width
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipGetImageWidth(new HandleRef(this, this.nativeImage), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return num2;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600015E RID: 350 RVA: 0x000058E4 File Offset: 0x000048E4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(false)]
		[Browsable(false)]
		public int Height
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipGetImageHeight(new HandleRef(this, this.nativeImage), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return num2;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00005910 File Offset: 0x00004910
		public float HorizontalResolution
		{
			get
			{
				float num2;
				int num = SafeNativeMethods.Gdip.GdipGetImageHorizontalResolution(new HandleRef(this, this.nativeImage), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return num2;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000593C File Offset: 0x0000493C
		public float VerticalResolution
		{
			get
			{
				float num2;
				int num = SafeNativeMethods.Gdip.GdipGetImageVerticalResolution(new HandleRef(this, this.nativeImage), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return num2;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00005968 File Offset: 0x00004968
		[Browsable(false)]
		public int Flags
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipGetImageFlags(new HandleRef(this, this.nativeImage), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return num2;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00005994 File Offset: 0x00004994
		public ImageFormat RawFormat
		{
			get
			{
				Guid guid = default(Guid);
				int num = SafeNativeMethods.Gdip.GdipGetImageRawFormat(new HandleRef(this, this.nativeImage), ref guid);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return new ImageFormat(guid);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000163 RID: 355 RVA: 0x000059D0 File Offset: 0x000049D0
		public PixelFormat PixelFormat
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipGetImagePixelFormat(new HandleRef(this, this.nativeImage), out num2);
				if (num != 0)
				{
					return PixelFormat.Undefined;
				}
				return (PixelFormat)num2;
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000059F8 File Offset: 0x000049F8
		public RectangleF GetBounds(ref GraphicsUnit pageUnit)
		{
			GPRECTF gprectf = default(GPRECTF);
			int num = SafeNativeMethods.Gdip.GdipGetImageBounds(new HandleRef(this, this.nativeImage), ref gprectf, out pageUnit);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return gprectf.ToRectangleF();
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00005A34 File Offset: 0x00004A34
		private ColorPalette _GetColorPalette()
		{
			int num = -1;
			int num2 = SafeNativeMethods.Gdip.GdipGetImagePaletteSize(new HandleRef(this, this.nativeImage), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			ColorPalette colorPalette = new ColorPalette(num);
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			num2 = SafeNativeMethods.Gdip.GdipGetImagePalette(new HandleRef(this, this.nativeImage), intPtr, num);
			try
			{
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				colorPalette.ConvertFromMemory(intPtr);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return colorPalette;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00005AB0 File Offset: 0x00004AB0
		private void _SetColorPalette(ColorPalette palette)
		{
			IntPtr intPtr = palette.ConvertToMemory();
			int num = SafeNativeMethods.Gdip.GdipSetImagePalette(new HandleRef(this, this.nativeImage), intPtr);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00005AF4 File Offset: 0x00004AF4
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00005AFC File Offset: 0x00004AFC
		[Browsable(false)]
		public ColorPalette Palette
		{
			get
			{
				return this._GetColorPalette();
			}
			set
			{
				this._SetColorPalette(value);
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00005B08 File Offset: 0x00004B08
		public Image GetThumbnailImage(int thumbWidth, int thumbHeight, Image.GetThumbnailImageAbort callback, IntPtr callbackData)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipGetImageThumbnail(new HandleRef(this, this.nativeImage), thumbWidth, thumbHeight, out zero, callback, callbackData);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return Image.CreateImageObject(zero);
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00005B44 File Offset: 0x00004B44
		[Browsable(false)]
		public Guid[] FrameDimensionsList
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipImageGetFrameDimensionsCount(new HandleRef(this, this.nativeImage), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				if (num2 <= 0)
				{
					return new Guid[0];
				}
				int num3 = Marshal.SizeOf(typeof(Guid));
				IntPtr intPtr = Marshal.AllocHGlobal(checked(num3 * num2));
				if (intPtr == IntPtr.Zero)
				{
					throw SafeNativeMethods.Gdip.StatusException(3);
				}
				num = SafeNativeMethods.Gdip.GdipImageGetFrameDimensionsList(new HandleRef(this, this.nativeImage), intPtr, num2);
				if (num != 0)
				{
					Marshal.FreeHGlobal(intPtr);
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				Guid[] array = new Guid[num2];
				try
				{
					for (int i = 0; i < num2; i++)
					{
						array[i] = (Guid)UnsafeNativeMethods.PtrToStructure((IntPtr)((long)intPtr + (long)(num3 * i)), typeof(Guid));
					}
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
				return array;
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00005C34 File Offset: 0x00004C34
		public int GetFrameCount(FrameDimension dimension)
		{
			int[] array = new int[1];
			int[] array2 = array;
			Guid guid = dimension.Guid;
			int num = SafeNativeMethods.Gdip.GdipImageGetFrameCount(new HandleRef(this, this.nativeImage), ref guid, array2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return array2[0];
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00005C74 File Offset: 0x00004C74
		public int SelectActiveFrame(FrameDimension dimension, int frameIndex)
		{
			int[] array = new int[1];
			int[] array2 = array;
			Guid guid = dimension.Guid;
			int num = SafeNativeMethods.Gdip.GdipImageSelectActiveFrame(new HandleRef(this, this.nativeImage), ref guid, frameIndex);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return array2[0];
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00005CB4 File Offset: 0x00004CB4
		public void RotateFlip(RotateFlipType rotateFlipType)
		{
			int num = SafeNativeMethods.Gdip.GdipImageRotateFlip(new HandleRef(this, this.nativeImage), (int)rotateFlipType);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00005CE0 File Offset: 0x00004CE0
		[Browsable(false)]
		public int[] PropertyIdList
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipGetPropertyCount(new HandleRef(this, this.nativeImage), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				int[] array = new int[num2];
				if (num2 == 0)
				{
					return array;
				}
				num = SafeNativeMethods.Gdip.GdipGetPropertyIdList(new HandleRef(this, this.nativeImage), num2, array);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return array;
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00005D38 File Offset: 0x00004D38
		public PropertyItem GetPropertyItem(int propid)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipGetPropertyItemSize(new HandleRef(this, this.nativeImage), propid, out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			if (num2 == 0)
			{
				return null;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(num2);
			if (intPtr == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			num = SafeNativeMethods.Gdip.GdipGetPropertyItem(new HandleRef(this, this.nativeImage), propid, num2, intPtr);
			PropertyItem propertyItem;
			try
			{
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				propertyItem = PropertyItemInternal.ConvertFromMemory(intPtr, 1)[0];
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return propertyItem;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00005DC8 File Offset: 0x00004DC8
		public void RemovePropertyItem(int propid)
		{
			int num = SafeNativeMethods.Gdip.GdipRemovePropertyItem(new HandleRef(this, this.nativeImage), propid);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00005DF4 File Offset: 0x00004DF4
		public void SetPropertyItem(PropertyItem propitem)
		{
			PropertyItemInternal propertyItemInternal = PropertyItemInternal.ConvertFromPropertyItem(propitem);
			using (propertyItemInternal)
			{
				int num = SafeNativeMethods.Gdip.GdipSetPropertyItem(new HandleRef(this, this.nativeImage), propertyItemInternal);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00005E44 File Offset: 0x00004E44
		[Browsable(false)]
		public PropertyItem[] PropertyItems
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipGetPropertyCount(new HandleRef(this, this.nativeImage), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				int num3;
				num = SafeNativeMethods.Gdip.GdipGetPropertySize(new HandleRef(this, this.nativeImage), out num3, ref num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				if (num3 == 0 || num2 == 0)
				{
					return new PropertyItem[0];
				}
				IntPtr intPtr = Marshal.AllocHGlobal(num3);
				num = SafeNativeMethods.Gdip.GdipGetAllPropertyItems(new HandleRef(this, this.nativeImage), num3, num2, intPtr);
				PropertyItem[] array = null;
				try
				{
					if (num != 0)
					{
						throw SafeNativeMethods.Gdip.StatusException(num);
					}
					array = PropertyItemInternal.ConvertFromMemory(intPtr, num2);
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
				return array;
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00005EEC File Offset: 0x00004EEC
		internal void SetNativeImage(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentException(SR.GetString("NativeHandle0"), "handle");
			}
			this.nativeImage = handle;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00005F17 File Offset: 0x00004F17
		public static Bitmap FromHbitmap(IntPtr hbitmap)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			return Image.FromHbitmap(hbitmap, IntPtr.Zero);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00005F30 File Offset: 0x00004F30
		public static Bitmap FromHbitmap(IntPtr hbitmap, IntPtr hpalette)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateBitmapFromHBITMAP(new HandleRef(null, hbitmap), new HandleRef(null, hpalette), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return Bitmap.FromGDIplus(zero);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00005F73 File Offset: 0x00004F73
		public static int GetPixelFormatSize(PixelFormat pixfmt)
		{
			return (int)((pixfmt >> 8) & (PixelFormat)255);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00005F7E File Offset: 0x00004F7E
		public static bool IsAlphaPixelFormat(PixelFormat pixfmt)
		{
			return (pixfmt & PixelFormat.Alpha) != PixelFormat.Undefined;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00005F8D File Offset: 0x00004F8D
		public static bool IsExtendedPixelFormat(PixelFormat pixfmt)
		{
			return (pixfmt & PixelFormat.Extended) != PixelFormat.Undefined;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00005F9C File Offset: 0x00004F9C
		public static bool IsCanonicalPixelFormat(PixelFormat pixfmt)
		{
			return (pixfmt & PixelFormat.Canonical) != PixelFormat.Undefined;
		}

		// Token: 0x040001BE RID: 446
		internal IntPtr nativeImage;

		// Token: 0x040001BF RID: 447
		private byte[] rawData;

		// Token: 0x040001C0 RID: 448
		private object userData;

		// Token: 0x02000031 RID: 49
		// (Invoke) Token: 0x0600017B RID: 379
		public delegate bool GetThumbnailImageAbort();

		// Token: 0x02000032 RID: 50
		private enum ImageTypeEnum
		{
			// Token: 0x040001C2 RID: 450
			Bitmap = 1,
			// Token: 0x040001C3 RID: 451
			Metafile
		}
	}
}
