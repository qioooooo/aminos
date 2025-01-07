using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class MaskedTextBoxTextEditor : UITypeEditor
	{
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (context != null && context.Instance != null && provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
				if (windowsFormsEditorService != null && context.Instance != null)
				{
					MaskedTextBox maskedTextBox = context.Instance as MaskedTextBox;
					if (maskedTextBox == null)
					{
						maskedTextBox = new MaskedTextBox();
						maskedTextBox.Text = value as string;
					}
					MaskedTextBoxTextEditorDropDown maskedTextBoxTextEditorDropDown = new MaskedTextBoxTextEditorDropDown(maskedTextBox);
					windowsFormsEditorService.DropDownControl(maskedTextBoxTextEditorDropDown);
					if (maskedTextBoxTextEditorDropDown.Value != null)
					{
						value = maskedTextBoxTextEditorDropDown.Value;
					}
				}
			}
			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if (context != null && context.Instance != null)
			{
				return UITypeEditorEditStyle.DropDown;
			}
			return base.GetEditStyle(context);
		}

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return (context == null || context.Instance == null) && base.GetPaintValueSupported(context);
		}

		public override bool IsDropDownResizable
		{
			get
			{
				return false;
			}
		}
	}
}
