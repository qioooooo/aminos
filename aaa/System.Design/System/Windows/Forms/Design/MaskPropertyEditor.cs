using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000274 RID: 628
	internal class MaskPropertyEditor : UITypeEditor
	{
		// Token: 0x060017A4 RID: 6052 RVA: 0x0007ADE4 File Offset: 0x00079DE4
		internal static string EditMask(ITypeDiscoveryService discoverySvc, IUIService uiSvc, MaskedTextBox instance, IHelpService helpService)
		{
			string text = null;
			MaskDesignerDialog maskDesignerDialog = new MaskDesignerDialog(instance, helpService);
			try
			{
				maskDesignerDialog.DiscoverMaskDescriptors(discoverySvc);
				DialogResult dialogResult = ((uiSvc != null) ? uiSvc.ShowDialog(maskDesignerDialog) : maskDesignerDialog.ShowDialog());
				if (dialogResult == DialogResult.OK)
				{
					text = maskDesignerDialog.Mask;
					if (maskDesignerDialog.ValidatingType != instance.ValidatingType)
					{
						instance.ValidatingType = maskDesignerDialog.ValidatingType;
					}
				}
			}
			finally
			{
				maskDesignerDialog.Dispose();
			}
			return text;
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x0007AE54 File Offset: 0x00079E54
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (context != null && provider != null)
			{
				ITypeDiscoveryService typeDiscoveryService = (ITypeDiscoveryService)provider.GetService(typeof(ITypeDiscoveryService));
				IUIService iuiservice = (IUIService)provider.GetService(typeof(IUIService));
				IHelpService helpService = (IHelpService)provider.GetService(typeof(IHelpService));
				string text = MaskPropertyEditor.EditMask(typeDiscoveryService, iuiservice, context.Instance as MaskedTextBox, helpService);
				if (text != null)
				{
					return text;
				}
			}
			return value;
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x0007AEC3 File Offset: 0x00079EC3
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x0007AEC6 File Offset: 0x00079EC6
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}
