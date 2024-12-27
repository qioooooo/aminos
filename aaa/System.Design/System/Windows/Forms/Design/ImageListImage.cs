using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.IO;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000250 RID: 592
	[Editor(typeof(ImageListImageEditor), typeof(UITypeEditor))]
	internal class ImageListImage
	{
		// Token: 0x06001696 RID: 5782 RVA: 0x0007549B File Offset: 0x0007449B
		public ImageListImage(Bitmap image)
		{
			this.Image = image;
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x000754AA File Offset: 0x000744AA
		public ImageListImage(Bitmap image, string name)
		{
			this.Image = image;
			this.Name = name;
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06001698 RID: 5784 RVA: 0x000754C0 File Offset: 0x000744C0
		// (set) Token: 0x06001699 RID: 5785 RVA: 0x000754D6 File Offset: 0x000744D6
		public string Name
		{
			get
			{
				if (this._name != null)
				{
					return this._name;
				}
				return "";
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x0600169A RID: 5786 RVA: 0x000754DF File Offset: 0x000744DF
		// (set) Token: 0x0600169B RID: 5787 RVA: 0x000754E7 File Offset: 0x000744E7
		[Browsable(false)]
		public Bitmap Image
		{
			get
			{
				return this._image;
			}
			set
			{
				this._image = value;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x0600169C RID: 5788 RVA: 0x000754F0 File Offset: 0x000744F0
		public float HorizontalResolution
		{
			get
			{
				return this._image.HorizontalResolution;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x0600169D RID: 5789 RVA: 0x000754FD File Offset: 0x000744FD
		public float VerticalResolution
		{
			get
			{
				return this._image.VerticalResolution;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x0600169E RID: 5790 RVA: 0x0007550A File Offset: 0x0007450A
		public PixelFormat PixelFormat
		{
			get
			{
				return this._image.PixelFormat;
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x0600169F RID: 5791 RVA: 0x00075517 File Offset: 0x00074517
		public ImageFormat RawFormat
		{
			get
			{
				return this._image.RawFormat;
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x060016A0 RID: 5792 RVA: 0x00075524 File Offset: 0x00074524
		public Size Size
		{
			get
			{
				return this._image.Size;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x060016A1 RID: 5793 RVA: 0x00075531 File Offset: 0x00074531
		public SizeF PhysicalDimension
		{
			get
			{
				return this._image.Size;
			}
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x00075543 File Offset: 0x00074543
		public static ImageListImage ImageListImageFromStream(Stream stream, bool imageIsIcon)
		{
			if (imageIsIcon)
			{
				return new ImageListImage(new Icon(stream).ToBitmap());
			}
			return new ImageListImage((Bitmap)global::System.Drawing.Image.FromStream(stream));
		}

		// Token: 0x040012F4 RID: 4852
		private string _name;

		// Token: 0x040012F5 RID: 4853
		private Bitmap _image;
	}
}
