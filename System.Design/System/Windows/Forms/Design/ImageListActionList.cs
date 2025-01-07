using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	internal class ImageListActionList : DesignerActionList
	{
		public ImageListActionList(ImageListDesigner designer)
			: base(designer.Component)
		{
			this._designer = designer;
		}

		public void ChooseImages()
		{
			EditorServiceContext.EditValue(this._designer, base.Component, "Images");
		}

		public ColorDepth ColorDepth
		{
			get
			{
				return ((ImageList)base.Component).ColorDepth;
			}
			set
			{
				TypeDescriptor.GetProperties(base.Component)["ColorDepth"].SetValue(base.Component, value);
			}
		}

		public Size ImageSize
		{
			get
			{
				return ((ImageList)base.Component).ImageSize;
			}
			set
			{
				TypeDescriptor.GetProperties(base.Component)["ImageSize"].SetValue(base.Component, value);
			}
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			return new DesignerActionItemCollection
			{
				new DesignerActionPropertyItem("ImageSize", SR.GetString("ImageListActionListImageSizeDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("ImageListActionListImageSizeDescription")),
				new DesignerActionPropertyItem("ColorDepth", SR.GetString("ImageListActionListColorDepthDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("ImageListActionListColorDepthDescription")),
				new DesignerActionMethodItem(this, "ChooseImages", SR.GetString("ImageListActionListChooseImagesDisplayName"), SR.GetString("LinksCategoryName"), SR.GetString("ImageListActionListChooseImagesDescription"), true)
			};
		}

		private ImageListDesigner _designer;
	}
}
