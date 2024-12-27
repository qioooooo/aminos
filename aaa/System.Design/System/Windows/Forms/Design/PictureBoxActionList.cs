using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200027B RID: 635
	internal class PictureBoxActionList : DesignerActionList
	{
		// Token: 0x060017BB RID: 6075 RVA: 0x0007B990 File Offset: 0x0007A990
		public PictureBoxActionList(PictureBoxDesigner designer)
			: base(designer.Component)
		{
			this._designer = designer;
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x060017BC RID: 6076 RVA: 0x0007B9A5 File Offset: 0x0007A9A5
		// (set) Token: 0x060017BD RID: 6077 RVA: 0x0007B9B7 File Offset: 0x0007A9B7
		public PictureBoxSizeMode SizeMode
		{
			get
			{
				return ((PictureBox)base.Component).SizeMode;
			}
			set
			{
				TypeDescriptor.GetProperties(base.Component)["SizeMode"].SetValue(base.Component, value);
			}
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x0007B9DF File Offset: 0x0007A9DF
		public void ChooseImage()
		{
			EditorServiceContext.EditValue(this._designer, base.Component, "Image");
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x0007B9F8 File Offset: 0x0007A9F8
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			return new DesignerActionItemCollection
			{
				new DesignerActionMethodItem(this, "ChooseImage", SR.GetString("ChooseImageDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("ChooseImageDescription"), true),
				new DesignerActionPropertyItem("SizeMode", SR.GetString("SizeModeDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("SizeModeDescription"))
			};
		}

		// Token: 0x040013AC RID: 5036
		private PictureBoxDesigner _designer;
	}
}
