using System;
using System.ComponentModel;

namespace System.Drawing.Imaging
{
	// Token: 0x020000C0 RID: 192
	[TypeConverter(typeof(ImageFormatConverter))]
	public sealed class ImageFormat
	{
		// Token: 0x06000BB9 RID: 3001 RVA: 0x00022F70 File Offset: 0x00021F70
		public ImageFormat(Guid guid)
		{
			this.guid = guid;
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000BBA RID: 3002 RVA: 0x00022F7F File Offset: 0x00021F7F
		public Guid Guid
		{
			get
			{
				return this.guid;
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000BBB RID: 3003 RVA: 0x00022F87 File Offset: 0x00021F87
		public static ImageFormat MemoryBmp
		{
			get
			{
				return ImageFormat.memoryBMP;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x00022F8E File Offset: 0x00021F8E
		public static ImageFormat Bmp
		{
			get
			{
				return ImageFormat.bmp;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x00022F95 File Offset: 0x00021F95
		public static ImageFormat Emf
		{
			get
			{
				return ImageFormat.emf;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x00022F9C File Offset: 0x00021F9C
		public static ImageFormat Wmf
		{
			get
			{
				return ImageFormat.wmf;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x00022FA3 File Offset: 0x00021FA3
		public static ImageFormat Gif
		{
			get
			{
				return ImageFormat.gif;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x00022FAA File Offset: 0x00021FAA
		public static ImageFormat Jpeg
		{
			get
			{
				return ImageFormat.jpeg;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x00022FB1 File Offset: 0x00021FB1
		public static ImageFormat Png
		{
			get
			{
				return ImageFormat.png;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x00022FB8 File Offset: 0x00021FB8
		public static ImageFormat Tiff
		{
			get
			{
				return ImageFormat.tiff;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000BC3 RID: 3011 RVA: 0x00022FBF File Offset: 0x00021FBF
		public static ImageFormat Exif
		{
			get
			{
				return ImageFormat.exif;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x00022FC6 File Offset: 0x00021FC6
		public static ImageFormat Icon
		{
			get
			{
				return ImageFormat.icon;
			}
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x00022FD0 File Offset: 0x00021FD0
		public override bool Equals(object o)
		{
			ImageFormat imageFormat = o as ImageFormat;
			return imageFormat != null && this.guid == imageFormat.guid;
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x00022FFA File Offset: 0x00021FFA
		public override int GetHashCode()
		{
			return this.guid.GetHashCode();
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00023010 File Offset: 0x00022010
		internal ImageCodecInfo FindEncoder()
		{
			ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
			foreach (ImageCodecInfo imageCodecInfo in imageEncoders)
			{
				if (imageCodecInfo.FormatID.Equals(this.guid))
				{
					return imageCodecInfo;
				}
			}
			return null;
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x0002305C File Offset: 0x0002205C
		public override string ToString()
		{
			if (this == ImageFormat.memoryBMP)
			{
				return "MemoryBMP";
			}
			if (this == ImageFormat.bmp)
			{
				return "Bmp";
			}
			if (this == ImageFormat.emf)
			{
				return "Emf";
			}
			if (this == ImageFormat.wmf)
			{
				return "Wmf";
			}
			if (this == ImageFormat.gif)
			{
				return "Gif";
			}
			if (this == ImageFormat.jpeg)
			{
				return "Jpeg";
			}
			if (this == ImageFormat.png)
			{
				return "Png";
			}
			if (this == ImageFormat.tiff)
			{
				return "Tiff";
			}
			if (this == ImageFormat.exif)
			{
				return "Exif";
			}
			if (this == ImageFormat.icon)
			{
				return "Icon";
			}
			return "[ImageFormat: " + this.guid + "]";
		}

		// Token: 0x04000A53 RID: 2643
		private static ImageFormat memoryBMP = new ImageFormat(new Guid("{b96b3caa-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x04000A54 RID: 2644
		private static ImageFormat bmp = new ImageFormat(new Guid("{b96b3cab-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x04000A55 RID: 2645
		private static ImageFormat emf = new ImageFormat(new Guid("{b96b3cac-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x04000A56 RID: 2646
		private static ImageFormat wmf = new ImageFormat(new Guid("{b96b3cad-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x04000A57 RID: 2647
		private static ImageFormat jpeg = new ImageFormat(new Guid("{b96b3cae-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x04000A58 RID: 2648
		private static ImageFormat png = new ImageFormat(new Guid("{b96b3caf-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x04000A59 RID: 2649
		private static ImageFormat gif = new ImageFormat(new Guid("{b96b3cb0-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x04000A5A RID: 2650
		private static ImageFormat tiff = new ImageFormat(new Guid("{b96b3cb1-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x04000A5B RID: 2651
		private static ImageFormat exif = new ImageFormat(new Guid("{b96b3cb2-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x04000A5C RID: 2652
		private static ImageFormat photoCD = new ImageFormat(new Guid("{b96b3cb3-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x04000A5D RID: 2653
		private static ImageFormat flashPIX = new ImageFormat(new Guid("{b96b3cb4-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x04000A5E RID: 2654
		private static ImageFormat icon = new ImageFormat(new Guid("{b96b3cb5-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x04000A5F RID: 2655
		private Guid guid;
	}
}
