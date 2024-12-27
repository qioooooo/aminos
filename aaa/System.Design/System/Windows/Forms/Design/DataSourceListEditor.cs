using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001F8 RID: 504
	internal class DataSourceListEditor : UITypeEditor
	{
		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06001349 RID: 4937 RVA: 0x0006284C File Offset: 0x0006184C
		public override bool IsDropDownResizable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x00062850 File Offset: 0x00061850
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null && context.Instance != null)
			{
				if (this.designBindingPicker == null)
				{
					this.designBindingPicker = new DesignBindingPicker();
				}
				DesignBinding designBinding = new DesignBinding(value, "");
				DesignBinding designBinding2 = this.designBindingPicker.Pick(context, provider, true, false, false, null, string.Empty, designBinding);
				if (designBinding2 != null)
				{
					value = designBinding2.DataSource;
				}
			}
			return value;
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x000628AB File Offset: 0x000618AB
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		// Token: 0x0400119D RID: 4509
		private DesignBindingPicker designBindingPicker;
	}
}
