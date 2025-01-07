using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class ImageIndexEditor : UITypeEditor
	{
		public ImageIndexEditor()
		{
			this.imageEditor = (UITypeEditor)TypeDescriptor.GetEditor(typeof(Image), typeof(UITypeEditor));
		}

		internal UITypeEditor ImageEditor
		{
			get
			{
				return this.imageEditor;
			}
		}

		internal string ParentImageListProperty
		{
			get
			{
				return this.parentImageListProperty;
			}
		}

		protected virtual Image GetImage(ITypeDescriptorContext context, int index, string key, bool useIntIndex)
		{
			Image image = null;
			object obj = context.Instance;
			if (obj is object[])
			{
				return null;
			}
			if (index >= 0 || key != null)
			{
				if (this.currentImageList == null || obj != this.currentInstance || (this.currentImageListProp != null && (ImageList)this.currentImageListProp.GetValue(this.currentInstance) != this.currentImageList))
				{
					this.currentInstance = obj;
					PropertyDescriptor propertyDescriptor = ImageListUtils.GetImageListProperty(context.PropertyDescriptor, ref obj);
					while (obj != null && propertyDescriptor == null)
					{
						PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
						foreach (object obj2 in properties)
						{
							PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)obj2;
							if (typeof(ImageList).IsAssignableFrom(propertyDescriptor2.PropertyType))
							{
								propertyDescriptor = propertyDescriptor2;
								break;
							}
						}
						if (propertyDescriptor == null)
						{
							PropertyDescriptor propertyDescriptor3 = properties[this.ParentImageListProperty];
							if (propertyDescriptor3 != null)
							{
								obj = propertyDescriptor3.GetValue(obj);
							}
							else
							{
								obj = null;
							}
						}
					}
					if (propertyDescriptor != null)
					{
						this.currentImageList = (ImageList)propertyDescriptor.GetValue(obj);
						this.currentImageListProp = propertyDescriptor;
						this.currentInstance = obj;
					}
				}
				if (this.currentImageList != null)
				{
					if (useIntIndex)
					{
						if (this.currentImageList != null && index < this.currentImageList.Images.Count)
						{
							index = ((index > 0) ? index : 0);
							image = this.currentImageList.Images[index];
						}
					}
					else
					{
						image = this.currentImageList.Images[key];
					}
				}
				else
				{
					image = null;
				}
			}
			return image;
		}

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return this.imageEditor != null && this.imageEditor.GetPaintValueSupported(context);
		}

		public override void PaintValue(PaintValueEventArgs e)
		{
			if (this.ImageEditor != null)
			{
				Image image = null;
				if (e.Value is int)
				{
					image = this.GetImage(e.Context, (int)e.Value, null, true);
				}
				else if (e.Value is string)
				{
					image = this.GetImage(e.Context, -1, (string)e.Value, false);
				}
				if (image != null)
				{
					this.ImageEditor.PaintValue(new PaintValueEventArgs(e.Context, image, e.Graphics, e.Bounds));
				}
			}
		}

		protected ImageList currentImageList;

		protected PropertyDescriptor currentImageListProp;

		protected object currentInstance;

		protected UITypeEditor imageEditor;

		protected string parentImageListProperty = "Parent";

		protected string imageListPropertyName;
	}
}
