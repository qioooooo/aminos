using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200024C RID: 588
	internal class ImageIndexEditor : UITypeEditor
	{
		// Token: 0x0600165A RID: 5722 RVA: 0x000749C2 File Offset: 0x000739C2
		public ImageIndexEditor()
		{
			this.imageEditor = (UITypeEditor)TypeDescriptor.GetEditor(typeof(Image), typeof(UITypeEditor));
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x0600165B RID: 5723 RVA: 0x000749F9 File Offset: 0x000739F9
		internal UITypeEditor ImageEditor
		{
			get
			{
				return this.imageEditor;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x0600165C RID: 5724 RVA: 0x00074A01 File Offset: 0x00073A01
		internal string ParentImageListProperty
		{
			get
			{
				return this.parentImageListProperty;
			}
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x00074A0C File Offset: 0x00073A0C
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

		// Token: 0x0600165E RID: 5726 RVA: 0x00074BA0 File Offset: 0x00073BA0
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return this.imageEditor != null && this.imageEditor.GetPaintValueSupported(context);
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x00074BB8 File Offset: 0x00073BB8
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

		// Token: 0x040012E9 RID: 4841
		protected ImageList currentImageList;

		// Token: 0x040012EA RID: 4842
		protected PropertyDescriptor currentImageListProp;

		// Token: 0x040012EB RID: 4843
		protected object currentInstance;

		// Token: 0x040012EC RID: 4844
		protected UITypeEditor imageEditor;

		// Token: 0x040012ED RID: 4845
		protected string parentImageListProperty = "Parent";

		// Token: 0x040012EE RID: 4846
		protected string imageListPropertyName;
	}
}
