using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200024F RID: 591
	internal class ImageListActionList : DesignerActionList
	{
		// Token: 0x0600168F RID: 5775 RVA: 0x00075356 File Offset: 0x00074356
		public ImageListActionList(ImageListDesigner designer)
			: base(designer.Component)
		{
			this._designer = designer;
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x0007536B File Offset: 0x0007436B
		public void ChooseImages()
		{
			EditorServiceContext.EditValue(this._designer, base.Component, "Images");
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001691 RID: 5777 RVA: 0x00075384 File Offset: 0x00074384
		// (set) Token: 0x06001692 RID: 5778 RVA: 0x00075396 File Offset: 0x00074396
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

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001693 RID: 5779 RVA: 0x000753BE File Offset: 0x000743BE
		// (set) Token: 0x06001694 RID: 5780 RVA: 0x000753D0 File Offset: 0x000743D0
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

		// Token: 0x06001695 RID: 5781 RVA: 0x000753F8 File Offset: 0x000743F8
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			return new DesignerActionItemCollection
			{
				new DesignerActionPropertyItem("ImageSize", SR.GetString("ImageListActionListImageSizeDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("ImageListActionListImageSizeDescription")),
				new DesignerActionPropertyItem("ColorDepth", SR.GetString("ImageListActionListColorDepthDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("ImageListActionListColorDepthDescription")),
				new DesignerActionMethodItem(this, "ChooseImages", SR.GetString("ImageListActionListChooseImagesDisplayName"), SR.GetString("LinksCategoryName"), SR.GetString("ImageListActionListChooseImagesDescription"), true)
			};
		}

		// Token: 0x040012F3 RID: 4851
		private ImageListDesigner _designer;
	}
}
