using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class DesignBindingEditor : UITypeEditor
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

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		private DesignBindingPicker designBindingPicker;
	}
}
