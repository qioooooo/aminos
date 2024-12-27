using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Drawing.Internal;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Drawing
{
	// Token: 0x02000047 RID: 71
	[TypeConverter(typeof(IconConverter))]
	[Editor("System.Drawing.Design.IconEditor, System.Drawing.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Serializable]
	public sealed class Icon : MarshalByRefObject, ISerializable, ICloneable, IDisposable
	{
		// Token: 0x06000449 RID: 1097 RVA: 0x00010883 File Offset: 0x0000F883
		private Icon()
		{
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x000108A8 File Offset: 0x0000F8A8
		internal Icon(IntPtr handle)
			: this(handle, false)
		{
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x000108B4 File Offset: 0x0000F8B4
		internal Icon(IntPtr handle, bool takeOwnership)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentException(SR.GetString("InvalidGDIHandle", new object[] { typeof(Icon).Name }));
			}
			this.handle = handle;
			this.ownHandle = takeOwnership;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00010929 File Offset: 0x0000F929
		public Icon(string fileName)
			: this(fileName, 0, 0)
		{
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00010934 File Offset: 0x0000F934
		public Icon(string fileName, Size size)
			: this(fileName, size.Width, size.Height)
		{
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0001094C File Offset: 0x0000F94C
		public Icon(string fileName, int width, int height)
			: this()
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				this.iconData = new byte[(int)fileStream.Length];
				fileStream.Read(this.iconData, 0, this.iconData.Length);
			}
			this.Initialize(width, height);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x000109B8 File Offset: 0x0000F9B8
		public Icon(Icon original, Size size)
			: this(original, size.Width, size.Height)
		{
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x000109D0 File Offset: 0x0000F9D0
		public Icon(Icon original, int width, int height)
			: this()
		{
			if (original == null)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[] { "original", "null" }));
			}
			this.iconData = original.iconData;
			if (this.iconData == null)
			{
				this.iconSize = original.Size;
				this.handle = SafeNativeMethods.CopyImage(new HandleRef(original, original.Handle), 1, this.iconSize.Width, this.iconSize.Height, 0);
				return;
			}
			this.Initialize(width, height);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00010A68 File Offset: 0x0000FA68
		public Icon(Type type, string resource)
			: this()
		{
			Stream manifestResourceStream = type.Module.Assembly.GetManifestResourceStream(type, resource);
			if (manifestResourceStream == null)
			{
				throw new ArgumentException(SR.GetString("ResourceNotFound", new object[] { type, resource }));
			}
			this.iconData = new byte[(int)manifestResourceStream.Length];
			manifestResourceStream.Read(this.iconData, 0, this.iconData.Length);
			this.Initialize(0, 0);
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00010AE1 File Offset: 0x0000FAE1
		public Icon(Stream stream)
			: this(stream, 0, 0)
		{
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00010AEC File Offset: 0x0000FAEC
		public Icon(Stream stream, Size size)
			: this(stream, size.Width, size.Height)
		{
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00010B04 File Offset: 0x0000FB04
		public Icon(Stream stream, int width, int height)
			: this()
		{
			if (stream == null)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[] { "stream", "null" }));
			}
			this.iconData = new byte[(int)stream.Length];
			stream.Read(this.iconData, 0, this.iconData.Length);
			this.Initialize(width, height);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00010B74 File Offset: 0x0000FB74
		private Icon(SerializationInfo info, StreamingContext context)
		{
			this.iconData = (byte[])info.GetValue("IconData", typeof(byte[]));
			this.iconSize = (Size)info.GetValue("IconSize", typeof(Size));
			if (this.iconSize.IsEmpty)
			{
				this.Initialize(0, 0);
				return;
			}
			this.Initialize(this.iconSize.Width, this.iconSize.Height);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00010C16 File Offset: 0x0000FC16
		public static Icon ExtractAssociatedIcon(string filePath)
		{
			return Icon.ExtractAssociatedIcon(filePath, 0);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00010C20 File Offset: 0x0000FC20
		private static Icon ExtractAssociatedIcon(string filePath, int index)
		{
			if (filePath == null)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[] { "filePath", "null" }));
			}
			Uri uri;
			try
			{
				uri = new Uri(filePath);
			}
			catch (UriFormatException)
			{
				filePath = Path.GetFullPath(filePath);
				uri = new Uri(filePath);
			}
			if (uri.IsUnc)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[] { "filePath", filePath }));
			}
			if (uri.IsFile)
			{
				if (!File.Exists(filePath))
				{
					IntSecurity.DemandReadFileIO(filePath);
					throw new FileNotFoundException(filePath);
				}
				Icon icon = new Icon();
				StringBuilder stringBuilder = new StringBuilder(260);
				stringBuilder.Append(filePath);
				IntPtr intPtr = SafeNativeMethods.ExtractAssociatedIcon(NativeMethods.NullHandleRef, stringBuilder, ref index);
				if (intPtr != IntPtr.Zero)
				{
					IntSecurity.ObjectFromWin32Handle.Demand();
					return new Icon(intPtr, true);
				}
			}
			return null;
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x00010D20 File Offset: 0x0000FD20
		[Browsable(false)]
		public IntPtr Handle
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				return this.handle;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00010D4C File Offset: 0x0000FD4C
		[Browsable(false)]
		public int Height
		{
			get
			{
				return this.Size.Height;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x00010D68 File Offset: 0x0000FD68
		public Size Size
		{
			get
			{
				if (this.iconSize.IsEmpty)
				{
					SafeNativeMethods.ICONINFO iconinfo = new SafeNativeMethods.ICONINFO();
					SafeNativeMethods.GetIconInfo(new HandleRef(this, this.Handle), iconinfo);
					SafeNativeMethods.BITMAP bitmap = new SafeNativeMethods.BITMAP();
					if (iconinfo.hbmColor != IntPtr.Zero)
					{
						SafeNativeMethods.GetObject(new HandleRef(null, iconinfo.hbmColor), Marshal.SizeOf(typeof(SafeNativeMethods.BITMAP)), bitmap);
						SafeNativeMethods.IntDeleteObject(new HandleRef(null, iconinfo.hbmColor));
						this.iconSize = new Size(bitmap.bmWidth, bitmap.bmHeight);
					}
					else if (iconinfo.hbmMask != IntPtr.Zero)
					{
						SafeNativeMethods.GetObject(new HandleRef(null, iconinfo.hbmMask), Marshal.SizeOf(typeof(SafeNativeMethods.BITMAP)), bitmap);
						this.iconSize = new Size(bitmap.bmWidth, bitmap.bmHeight / 2);
					}
					if (iconinfo.hbmMask != IntPtr.Zero)
					{
						SafeNativeMethods.IntDeleteObject(new HandleRef(null, iconinfo.hbmMask));
					}
				}
				return this.iconSize;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x00010E7C File Offset: 0x0000FE7C
		[Browsable(false)]
		public int Width
		{
			get
			{
				return this.Size.Width;
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00010E98 File Offset: 0x0000FE98
		public object Clone()
		{
			return new Icon(this, this.Size.Width, this.Size.Height);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00010EC7 File Offset: 0x0000FEC7
		internal void DestroyHandle()
		{
			if (this.ownHandle)
			{
				SafeNativeMethods.DestroyIcon(new HandleRef(this, this.handle));
				this.handle = IntPtr.Zero;
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00010EEE File Offset: 0x0000FEEE
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00010EFD File Offset: 0x0000FEFD
		private void Dispose(bool disposing)
		{
			if (this.handle != IntPtr.Zero)
			{
				this.DestroyHandle();
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00010F18 File Offset: 0x0000FF18
		private void DrawIcon(IntPtr dc, Rectangle imageRect, Rectangle targetRect, bool stretch)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			Size size = this.Size;
			int num5;
			int num6;
			if (!imageRect.IsEmpty)
			{
				num = imageRect.X;
				num2 = imageRect.Y;
				num5 = imageRect.Width;
				num6 = imageRect.Height;
			}
			else
			{
				num5 = size.Width;
				num6 = size.Height;
			}
			int num7;
			int num8;
			if (!targetRect.IsEmpty)
			{
				num3 = targetRect.X;
				num4 = targetRect.Y;
				num7 = targetRect.Width;
				num8 = targetRect.Height;
			}
			else
			{
				num7 = size.Width;
				num8 = size.Height;
			}
			int num9;
			int num10;
			int num11;
			int num12;
			if (stretch)
			{
				num9 = size.Width * num7 / num5;
				num10 = size.Height * num8 / num6;
				num11 = num7;
				num12 = num8;
			}
			else
			{
				num9 = size.Width;
				num10 = size.Height;
				num11 = ((num7 < num5) ? num7 : num5);
				num12 = ((num8 < num6) ? num8 : num6);
			}
			IntPtr intPtr = SafeNativeMethods.SaveClipRgn(dc);
			try
			{
				SafeNativeMethods.IntersectClipRect(new HandleRef(this, dc), num3, num4, num3 + num11, num4 + num12);
				SafeNativeMethods.DrawIconEx(new HandleRef(null, dc), num3 - num, num4 - num2, new HandleRef(this, this.handle), num9, num10, 0, NativeMethods.NullHandleRef, 3);
			}
			finally
			{
				SafeNativeMethods.RestoreClipRgn(dc, intPtr);
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00011078 File Offset: 0x00010078
		internal void Draw(Graphics graphics, int x, int y)
		{
			Size size = this.Size;
			this.Draw(graphics, new Rectangle(x, y, size.Width, size.Height));
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x000110A8 File Offset: 0x000100A8
		internal void Draw(Graphics graphics, Rectangle targetRect)
		{
			Rectangle rectangle = targetRect;
			rectangle.X += (int)graphics.Transform.OffsetX;
			rectangle.Y += (int)graphics.Transform.OffsetY;
			WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(graphics, ApplyGraphicsProperties.Clipping);
			IntPtr hdc = windowsGraphics.GetHdc();
			try
			{
				this.DrawIcon(hdc, Rectangle.Empty, rectangle, true);
			}
			finally
			{
				windowsGraphics.Dispose();
			}
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00011124 File Offset: 0x00010124
		internal void DrawUnstretched(Graphics graphics, Rectangle targetRect)
		{
			Rectangle rectangle = targetRect;
			rectangle.X += (int)graphics.Transform.OffsetX;
			rectangle.Y += (int)graphics.Transform.OffsetY;
			WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(graphics, ApplyGraphicsProperties.Clipping);
			IntPtr hdc = windowsGraphics.GetHdc();
			try
			{
				this.DrawIcon(hdc, Rectangle.Empty, rectangle, false);
			}
			finally
			{
				windowsGraphics.Dispose();
			}
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x000111A0 File Offset: 0x000101A0
		~Icon()
		{
			this.Dispose(false);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x000111D0 File Offset: 0x000101D0
		public static Icon FromHandle(IntPtr handle)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			return new Icon(handle);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x000111E4 File Offset: 0x000101E4
		private unsafe short GetShort(byte* pb)
		{
			int num;
			if ((pb & 1) != 0)
			{
				num = (int)(*pb);
				pb++;
				num |= (int)(*pb) << 8;
			}
			else
			{
				num = (int)(*(short*)pb);
			}
			return (short)num;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00011210 File Offset: 0x00010210
		private unsafe int GetInt(byte* pb)
		{
			int num;
			if ((pb & 3) != 0)
			{
				num = (int)(*pb);
				pb++;
				num |= (int)(*pb) << 8;
				pb++;
				num |= (int)(*pb) << 16;
				pb++;
				num |= (int)(*pb) << 24;
			}
			else
			{
				num = *(int*)pb;
			}
			return num;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00011258 File Offset: 0x00010258
		private unsafe void Initialize(int width, int height)
		{
			if (this.iconData == null || this.handle != IntPtr.Zero)
			{
				throw new InvalidOperationException(SR.GetString("IllegalState", new object[] { base.GetType().Name }));
			}
			int num = Marshal.SizeOf(typeof(SafeNativeMethods.ICONDIR));
			if (this.iconData.Length < num)
			{
				throw new ArgumentException(SR.GetString("InvalidPictureType", new object[] { "picture", "Icon" }));
			}
			if (width == 0)
			{
				width = UnsafeNativeMethods.GetSystemMetrics(11);
			}
			if (height == 0)
			{
				height = UnsafeNativeMethods.GetSystemMetrics(12);
			}
			if (Icon.bitDepth == 0)
			{
				IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
				Icon.bitDepth = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 12);
				Icon.bitDepth *= UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 14);
				UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
				if (Icon.bitDepth == 8)
				{
					Icon.bitDepth = 4;
				}
			}
			fixed (byte* ptr = this.iconData)
			{
				short @short = this.GetShort(ptr);
				short short2 = this.GetShort(ptr + 2);
				short short3 = this.GetShort(ptr + 4);
				if (@short != 0 || short2 != 1 || short3 == 0)
				{
					throw new ArgumentException(SR.GetString("InvalidPictureType", new object[] { "picture", "Icon" }));
				}
				byte b = 0;
				byte b2 = 0;
				int num2 = 0;
				byte* ptr2 = ptr + 6;
				int num3 = Marshal.SizeOf(typeof(SafeNativeMethods.ICONDIRENTRY));
				if (num3 * (int)(short3 - 1) + num > this.iconData.Length)
				{
					throw new ArgumentException(SR.GetString("InvalidPictureType", new object[] { "picture", "Icon" }));
				}
				for (int i = 0; i < (int)short3; i++)
				{
					SafeNativeMethods.ICONDIRENTRY icondirentry;
					icondirentry.bWidth = *ptr2;
					icondirentry.bHeight = ptr2[1];
					icondirentry.bColorCount = ptr2[2];
					icondirentry.bReserved = ptr2[3];
					icondirentry.wPlanes = this.GetShort(ptr2 + 4);
					icondirentry.wBitCount = this.GetShort(ptr2 + 6);
					icondirentry.dwBytesInRes = this.GetInt(ptr2 + 8);
					icondirentry.dwImageOffset = this.GetInt(ptr2 + 12);
					bool flag = false;
					int num4;
					if (icondirentry.bColorCount != 0)
					{
						num4 = 4;
						if (icondirentry.bColorCount < 16)
						{
							num4 = 1;
						}
					}
					else
					{
						num4 = (int)icondirentry.wBitCount;
					}
					if (num4 == 0)
					{
						num4 = 8;
					}
					if (num2 == 0)
					{
						flag = true;
					}
					else
					{
						int num5 = Math.Abs((int)b - width) + Math.Abs((int)b2 - height);
						int num6 = Math.Abs((int)icondirentry.bWidth - width) + Math.Abs((int)icondirentry.bHeight - height);
						if (num6 < num5 || (num6 == num5 && ((num4 <= Icon.bitDepth && num4 > this.bestBitDepth) || (this.bestBitDepth > Icon.bitDepth && num4 < this.bestBitDepth))))
						{
							flag = true;
						}
					}
					if (flag)
					{
						b = icondirentry.bWidth;
						b2 = icondirentry.bHeight;
						this.bestImageOffset = icondirentry.dwImageOffset;
						num2 = icondirentry.dwBytesInRes;
						this.bestBitDepth = num4;
					}
					ptr2 += num3;
				}
				if (this.bestImageOffset < 0)
				{
					throw new ArgumentException(SR.GetString("InvalidPictureType", new object[] { "picture", "Icon" }));
				}
				if (num2 < 0)
				{
					throw new Win32Exception(87);
				}
				checked
				{
					int num7;
					try
					{
						num7 = this.bestImageOffset + num2;
					}
					catch (OverflowException)
					{
						throw new Win32Exception(87);
					}
					if (num7 > this.iconData.Length)
					{
						throw new ArgumentException(SR.GetString("InvalidPictureType", new object[] { "picture", "Icon" }));
					}
					if (this.bestImageOffset % IntPtr.Size != 0)
					{
						byte[] array = new byte[num2];
						Array.Copy(this.iconData, this.bestImageOffset, array, 0, num2);
						fixed (byte* ptr3 = array)
						{
							this.handle = SafeNativeMethods.CreateIconFromResourceEx(ptr3, num2, true, 196608, 0, 0, 0);
						}
					}
					else
					{
						try
						{
							this.handle = SafeNativeMethods.CreateIconFromResourceEx(ptr + this.bestImageOffset, num2, true, 196608, 0, 0, 0);
						}
						catch (OverflowException)
						{
							throw new Win32Exception(87);
						}
					}
					if (this.handle == IntPtr.Zero)
					{
						throw new Win32Exception();
					}
				}
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00011710 File Offset: 0x00010710
		public void Save(Stream outputStream)
		{
			if (this.iconData != null)
			{
				outputStream.Write(this.iconData, 0, this.iconData.Length);
				return;
			}
			SafeNativeMethods.PICTDESC pictdesc = SafeNativeMethods.PICTDESC.CreateIconPICTDESC(this.Handle);
			Guid guid = typeof(SafeNativeMethods.IPicture).GUID;
			SafeNativeMethods.IPicture picture = SafeNativeMethods.OleCreatePictureIndirect(pictdesc, ref guid, false);
			if (picture != null)
			{
				try
				{
					int num;
					picture.SaveAsFile(new UnsafeNativeMethods.ComStreamFromDataStream(outputStream), -1, out num);
				}
				finally
				{
					Marshal.ReleaseComObject(picture);
				}
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00011790 File Offset: 0x00010790
		private void CopyBitmapData(BitmapData sourceData, BitmapData targetData)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < Math.Min(sourceData.Height, targetData.Height); i++)
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

		// Token: 0x0600046B RID: 1131 RVA: 0x00011860 File Offset: 0x00010860
		private unsafe static bool BitmapHasAlpha(BitmapData bmpData)
		{
			bool flag = false;
			for (int i = 0; i < bmpData.Height; i++)
			{
				for (int j = 3; j < Math.Abs(bmpData.Stride); j += 4)
				{
					byte* ptr = (byte*)((byte*)bmpData.Scan0.ToPointer() + (IntPtr)i * (IntPtr)bmpData.Stride) + j;
					if (*ptr != 0)
					{
						return true;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000118BC File Offset: 0x000108BC
		public unsafe Bitmap ToBitmap()
		{
			Bitmap bitmap = null;
			if (this.iconData != null && this.bestBitDepth == 32)
			{
				bitmap = new Bitmap(this.Size.Width, this.Size.Height, PixelFormat.Format32bppArgb);
				BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, this.Size.Width, this.Size.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
				try
				{
					uint* ptr = (uint*)bitmapData.Scan0.ToPointer();
					int num = this.bestImageOffset + Marshal.SizeOf(typeof(SafeNativeMethods.BITMAPINFOHEADER));
					int num2 = this.Size.Width * 4;
					int width = this.Size.Width;
					for (int i = (this.Size.Height - 1) * 4; i >= 0; i -= 4)
					{
						Marshal.Copy(this.iconData, num + i * width, (IntPtr)((void*)ptr), num2);
						ptr += width;
					}
					goto IL_02A4;
				}
				finally
				{
					bitmap.UnlockBits(bitmapData);
				}
			}
			if (this.bestBitDepth == 0 || this.bestBitDepth == 32)
			{
				SafeNativeMethods.ICONINFO iconinfo = new SafeNativeMethods.ICONINFO();
				SafeNativeMethods.GetIconInfo(new HandleRef(this, this.handle), iconinfo);
				SafeNativeMethods.BITMAP bitmap2 = new SafeNativeMethods.BITMAP();
				try
				{
					if (iconinfo.hbmColor != IntPtr.Zero)
					{
						SafeNativeMethods.GetObject(new HandleRef(null, iconinfo.hbmColor), Marshal.SizeOf(typeof(SafeNativeMethods.BITMAP)), bitmap2);
						if (bitmap2.bmBitsPixel == 32)
						{
							Bitmap bitmap3 = null;
							BitmapData bitmapData2 = null;
							BitmapData bitmapData3 = null;
							IntSecurity.ObjectFromWin32Handle.Assert();
							try
							{
								bitmap3 = Image.FromHbitmap(iconinfo.hbmColor);
								bitmapData2 = bitmap3.LockBits(new Rectangle(0, 0, bitmap3.Width, bitmap3.Height), ImageLockMode.ReadOnly, bitmap3.PixelFormat);
								if (Icon.BitmapHasAlpha(bitmapData2))
								{
									bitmap = new Bitmap(bitmapData2.Width, bitmapData2.Height, PixelFormat.Format32bppArgb);
									bitmapData3 = bitmap.LockBits(new Rectangle(0, 0, bitmapData2.Width, bitmapData2.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
									this.CopyBitmapData(bitmapData2, bitmapData3);
								}
							}
							finally
							{
								CodeAccessPermission.RevertAssert();
								if (bitmap3 != null && bitmapData2 != null)
								{
									bitmap3.UnlockBits(bitmapData2);
								}
								if (bitmap != null && bitmapData3 != null)
								{
									bitmap.UnlockBits(bitmapData3);
								}
							}
							bitmap3.Dispose();
						}
					}
				}
				finally
				{
					if (iconinfo.hbmColor != IntPtr.Zero)
					{
						SafeNativeMethods.IntDeleteObject(new HandleRef(null, iconinfo.hbmColor));
					}
					if (iconinfo.hbmMask != IntPtr.Zero)
					{
						SafeNativeMethods.IntDeleteObject(new HandleRef(null, iconinfo.hbmMask));
					}
				}
			}
			IL_02A4:
			if (bitmap == null)
			{
				Size size = this.Size;
				bitmap = new Bitmap(size.Width, size.Height);
				Graphics graphics = null;
				try
				{
					graphics = Graphics.FromImage(bitmap);
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						using (Bitmap bitmap4 = Bitmap.FromHicon(this.Handle))
						{
							graphics.DrawImage(bitmap4, new Rectangle(0, 0, size.Width, size.Height));
						}
					}
					catch (ArgumentException)
					{
						this.Draw(graphics, new Rectangle(0, 0, size.Width, size.Height));
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				finally
				{
					if (graphics != null)
					{
						graphics.Dispose();
					}
				}
				Color color = Color.FromArgb(13, 11, 12);
				bitmap.MakeTransparent(color);
			}
			return bitmap;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00011CD8 File Offset: 0x00010CD8
		public override string ToString()
		{
			return SR.GetString("toStringIcon");
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00011CE4 File Offset: 0x00010CE4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			if (this.iconData != null)
			{
				si.AddValue("IconData", this.iconData, typeof(byte[]));
			}
			else
			{
				MemoryStream memoryStream = new MemoryStream();
				this.Save(memoryStream);
				si.AddValue("IconData", memoryStream.ToArray(), typeof(byte[]));
			}
			si.AddValue("IconSize", this.iconSize, typeof(Size));
		}

		// Token: 0x040002B2 RID: 690
		private static int bitDepth;

		// Token: 0x040002B3 RID: 691
		private byte[] iconData;

		// Token: 0x040002B4 RID: 692
		private int bestImageOffset;

		// Token: 0x040002B5 RID: 693
		private int bestBitDepth;

		// Token: 0x040002B6 RID: 694
		private Size iconSize = Size.Empty;

		// Token: 0x040002B7 RID: 695
		private IntPtr handle = IntPtr.Zero;

		// Token: 0x040002B8 RID: 696
		private bool ownHandle = true;
	}
}
