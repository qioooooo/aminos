using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class MaskPropertyEditor : UITypeEditor
	{
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

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return false;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}
