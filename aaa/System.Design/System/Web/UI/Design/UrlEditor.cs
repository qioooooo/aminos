using System;
using System.ComponentModel;
using System.Design;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design
{
	// Token: 0x0200037D RID: 893
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class UrlEditor : UITypeEditor
	{
		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x0600212F RID: 8495 RVA: 0x000B8E22 File Offset: 0x000B7E22
		protected virtual string Caption
		{
			get
			{
				return SR.GetString("UrlPicker_DefaultCaption");
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06002130 RID: 8496 RVA: 0x000B8E2E File Offset: 0x000B7E2E
		protected virtual UrlBuilderOptions Options
		{
			get
			{
				return UrlBuilderOptions.None;
			}
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x000B8E34 File Offset: 0x000B7E34
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

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06002132 RID: 8498 RVA: 0x000B8E8A File Offset: 0x000B7E8A
		protected virtual string Filter
		{
			get
			{
				return SR.GetString("UrlPicker_DefaultFilter");
			}
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x000B8E96 File Offset: 0x000B7E96
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}
