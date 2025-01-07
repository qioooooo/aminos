using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.IO;

namespace System.Windows.Forms.Design
{
	[Editor(typeof(ImageListImageEditor), typeof(UITypeEditor))]
	internal class ImageListImage
	{
		public ImageListImage(Bitmap image)
		{
			this.Image = image;
		}

		public ImageListImage(Bitmap image, string name)
		{
			this.Image = image;
			this.Name = name;
		}

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

		public float HorizontalResolution
		{
			get
			{
				return this._image.HorizontalResolution;
			}
		}

		public float VerticalResolution
		{
			get
			{
				return this._image.VerticalResolution;
			}
		}

		public PixelFormat PixelFormat
		{
			get
			{
				return this._image.PixelFormat;
			}
		}

		public ImageFormat RawFormat
		{
			get
			{
				return this._image.RawFormat;
			}
		}

		public Size Size
		{
			get
			{
				return this._image.Size;
			}
		}

		public SizeF PhysicalDimension
		{
			get
			{
				return this._image.Size;
			}
		}

		public static ImageListImage ImageListImageFromStream(Stream stream, bool imageIsIcon)
		{
			if (imageIsIcon)
			{
				return new ImageListImage(new Icon(stream).ToBitmap());
			}
			return new ImageListImage((Bitmap)global::System.Drawing.Image.FromStream(stream));
		}

		private string _name;

		private Bitmap _image;
	}
}
