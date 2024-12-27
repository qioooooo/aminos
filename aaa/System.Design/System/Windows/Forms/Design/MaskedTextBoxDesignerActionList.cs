using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000271 RID: 625
	internal class MaskedTextBoxDesignerActionList : DesignerActionList
	{
		// Token: 0x06001796 RID: 6038 RVA: 0x0007AA84 File Offset: 0x00079A84
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

		// Token: 0x06001797 RID: 6039 RVA: 0x0007AB10 File Offset: 0x00079B10
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

		// Token: 0x06001798 RID: 6040 RVA: 0x0007AB64 File Offset: 0x00079B64
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			return new DesignerActionItemCollection
			{
				new DesignerActionMethodItem(this, "SetMask", SR.GetString("MaskedTextBoxDesignerVerbsSetMaskDesc"))
			};
		}

		// Token: 0x04001349 RID: 4937
		private MaskedTextBox maskedTextBox;

		// Token: 0x0400134A RID: 4938
		private ITypeDiscoveryService discoverySvc;

		// Token: 0x0400134B RID: 4939
		private IUIService uiSvc;

		// Token: 0x0400134C RID: 4940
		private IHelpService helpService;
	}
}
