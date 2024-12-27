using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004EC RID: 1260
	public class TreeNodeBindingDepthConverter : Int32Converter
	{
		// Token: 0x06002D15 RID: 11541 RVA: 0x000FE904 File Offset: 0x000FD904
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null && text.Length == 0)
			{
				return -1;
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x000FE933 File Offset: 0x000FD933
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is int && (int)value == -1)
			{
				return string.Empty;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
