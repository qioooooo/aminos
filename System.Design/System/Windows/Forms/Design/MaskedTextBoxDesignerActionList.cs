using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class MaskedTextBoxDesignerActionList : DesignerActionList
	{
		public MaskedTextBoxDesignerActionList(MaskedTextBoxDesigner designer)
			: base(designer.Component)
		{
			this.maskedTextBox = (MaskedTextBox)designer.Component;
			this.discoverySvc = base.GetService(typeof(ITypeDiscoveryService)) as ITypeDiscoveryService;
			this.uiSvc = base.GetService(typeof(IUIService)) as IUIService;
			this.helpService = base.GetService(typeof(IHelpService)) as IHelpService;
			if (this.discoverySvc != null)
			{
				IUIService iuiservice = this.uiSvc;
			}
		}

		public void SetMask()
		{
			string text = MaskPropertyEditor.EditMask(this.discoverySvc, this.uiSvc, this.maskedTextBox, this.helpService);
			if (text != null)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.maskedTextBox)["Mask"];
				if (propertyDescriptor != null)
				{
					propertyDescriptor.SetValue(this.maskedTextBox, text);
				}
			}
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			return new DesignerActionItemCollection
			{
				new DesignerActionMethodItem(this, "SetMask", SR.GetString("MaskedTextBoxDesignerVerbsSetMaskDesc"))
			};
		}

		private MaskedTextBox maskedTextBox;

		private ITypeDiscoveryService discoverySvc;

		private IUIService uiSvc;

		private IHelpService helpService;
	}
}
