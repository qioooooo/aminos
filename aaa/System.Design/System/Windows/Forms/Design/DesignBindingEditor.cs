using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001FC RID: 508
	internal class DesignBindingEditor : UITypeEditor
	{
		// Token: 0x1700031D RID: 797
		// (get) Token: 0x0600135C RID: 4956 RVA: 0x00062BE0 File Offset: 0x00061BE0
		public override bool IsDropDownResizable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x00062BE4 File Offset: 0x00061BE4
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				if (this.designBindingPicker == null)
				{
					this.designBindingPicker = new DesignBindingPicker();
				}
				value = this.designBindingPicker.Pick(context, provider, true, true, false, null, string.Empty, (DesignBinding)value);
			}
			return value;
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x00062C26 File Offset: 0x00061C26
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		// Token: 0x040011A1 RID: 4513
		private DesignBindingPicker designBindingPicker;
	}
}
