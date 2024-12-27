using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000272 RID: 626
	internal class MaskedTextBoxTextEditor : UITypeEditor
	{
		// Token: 0x0600179A RID: 6042 RVA: 0x0007AB9C File Offset: 0x00079B9C
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

		// Token: 0x0600179B RID: 6043 RVA: 0x0007AC1A File Offset: 0x00079C1A
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if (context != null && context.Instance != null)
			{
				return UITypeEditorEditStyle.DropDown;
			}
			return base.GetEditStyle(context);
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x0007AC30 File Offset: 0x00079C30
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return (context == null || context.Instance == null) && base.GetPaintValueSupported(context);
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x0600179D RID: 6045 RVA: 0x0007AC46 File Offset: 0x00079C46
		public override bool IsDropDownResizable
		{
			get
			{
				return false;
			}
		}
	}
}
