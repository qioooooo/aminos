using System;
using System.ComponentModel;
using System.Design;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class UrlEditor : UITypeEditor
	{
		protected virtual string Caption
		{
			get
			{
				return SR.GetString("UrlPicker_DefaultCaption");
			}
		}

		protected virtual UrlBuilderOptions Options
		{
			get
			{
				return UrlBuilderOptions.None;
			}
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					string text = (string)value;
					string caption = this.Caption;
					string filter = this.Filter;
					text = UrlBuilder.BuildUrl(provider, null, text, caption, filter, this.Options);
					if (text != null)
					{
						value = text;
					}
				}
			}
			return value;
		}

		protected virtual string Filter
		{
			get
			{
				return SR.GetString("UrlPicker_DefaultFilter");
			}
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}
