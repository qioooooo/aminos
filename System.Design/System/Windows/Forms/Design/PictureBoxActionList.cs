using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class PictureBoxActionList : DesignerActionList
	{
		public PictureBoxActionList(PictureBoxDesigner designer)
			: base(designer.Component)
		{
			this._designer = designer;
		}

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

		public void ChooseImage()
		{
			EditorServiceContext.EditValue(this._designer, base.Component, "Image");
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			return new DesignerActionItemCollection
			{
				new DesignerActionMethodItem(this, "ChooseImage", SR.GetString("ChooseImageDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("ChooseImageDescription"), true),
				new DesignerActionPropertyItem("SizeMode", SR.GetString("SizeModeDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("SizeModeDescription"))
			};
		}

		private PictureBoxDesigner _designer;
	}
}
