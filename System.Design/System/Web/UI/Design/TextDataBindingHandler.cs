using System;
using System.ComponentModel.Design;
using System.Design;
using System.Reflection;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class TextDataBindingHandler : DataBindingHandler
	{
		public override void DataBindControl(IDesignerHost designerHost, Control control)
		{
			DataBinding dataBinding = ((IDataBindingsAccessor)control).DataBindings["Text"];
			if (dataBinding != null)
			{
				PropertyInfo property = control.GetType().GetProperty("Text");
				if (property != null && property.PropertyType == typeof(string))
				{
					DesignTimeDataBinding designTimeDataBinding = new DesignTimeDataBinding(dataBinding);
					string text = string.Empty;
					if (!designTimeDataBinding.IsCustom)
					{
						try
						{
							text = DataBinder.Eval(((IDataItemContainer)control.NamingContainer).DataItem, designTimeDataBinding.Field, designTimeDataBinding.Format);
						}
						catch
						{
						}
					}
					if (text == null || text.Length == 0)
					{
						text = SR.GetString("Sample_Databound_Text");
					}
					property.SetValue(control, text, null);
				}
			}
		}
	}
}
