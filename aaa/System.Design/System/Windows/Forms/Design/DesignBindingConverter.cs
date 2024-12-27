using System;
using System.Collections;
using System.ComponentModel;
using System.Design;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001FB RID: 507
	internal class DesignBindingConverter : TypeConverter
	{
		// Token: 0x06001357 RID: 4951 RVA: 0x000629C2 File Offset: 0x000619C2
		public override bool CanConvertTo(ITypeDescriptorContext context, Type sourceType)
		{
			return typeof(string) == sourceType;
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x000629D1 File Offset: 0x000619D1
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type destType)
		{
			return typeof(string) == destType;
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x000629E0 File Offset: 0x000619E0
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type sourceType)
		{
			DesignBinding designBinding = (DesignBinding)value;
			if (designBinding.IsNull)
			{
				return SR.GetString("DataGridNoneString");
			}
			string text = "";
			if (designBinding.DataSource is IComponent)
			{
				IComponent component = (IComponent)designBinding.DataSource;
				if (component.Site != null)
				{
					text = component.Site.Name;
				}
			}
			if (text.Length == 0)
			{
				if (designBinding.DataSource is IListSource || designBinding.DataSource is IList || designBinding.DataSource is Array)
				{
					text = "(List)";
				}
				else
				{
					string text2 = TypeDescriptor.GetClassName(designBinding.DataSource);
					int num = text2.LastIndexOf('.');
					if (num != -1)
					{
						text2 = text2.Substring(num + 1);
					}
					text = string.Format(CultureInfo.CurrentCulture, "({0})", new object[] { text2 });
				}
			}
			return text + " - " + designBinding.DataMember;
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x00062ACC File Offset: 0x00061ACC
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = (string)value;
			if (text == null || text.Length == 0 || string.Compare(text, SR.GetString("DataGridNoneString"), true, CultureInfo.CurrentCulture) == 0)
			{
				return DesignBinding.Null;
			}
			int num = text.IndexOf("-");
			if (num == -1)
			{
				throw new ArgumentException(SR.GetString("DesignBindingBadParseString", new object[] { text }));
			}
			string text2 = text.Substring(0, num - 1).Trim();
			string text3 = text.Substring(num + 1).Trim();
			if (context == null || context.Container == null)
			{
				throw new ArgumentException(SR.GetString("DesignBindingContextRequiredWhenParsing", new object[] { text }));
			}
			IContainer container = DesignerUtils.CheckForNestedContainer(context.Container);
			IComponent component = container.Components[text2];
			if (component != null)
			{
				return new DesignBinding(component, text3);
			}
			if (string.Equals(text2, "(List)", StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			throw new ArgumentException(SR.GetString("DesignBindingComponentNotFound", new object[] { text2 }));
		}
	}
}
