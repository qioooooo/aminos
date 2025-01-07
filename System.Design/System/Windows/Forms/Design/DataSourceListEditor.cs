using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class DataSourceListEditor : UITypeEditor
	{
		public override bool IsDropDownResizable
		{
			get
			{
				return true;
			}
		}

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

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		private DesignBindingPicker designBindingPicker;
	}
}
