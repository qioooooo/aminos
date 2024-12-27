using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Security.Permissions;
using System.Windows.Forms;

namespace System.Web.UI.Design
{
	// Token: 0x020003A5 RID: 933
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public sealed class UrlBuilder
	{
		// Token: 0x06002265 RID: 8805 RVA: 0x000BC101 File Offset: 0x000BB101
		private UrlBuilder()
		{
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x000BC109 File Offset: 0x000BB109
		public static string BuildUrl(IComponent component, Control owner, string initialUrl, string caption, string filter)
		{
			return UrlBuilder.BuildUrl(component, owner, initialUrl, caption, filter, UrlBuilderOptions.None);
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x000BC118 File Offset: 0x000BB118
		public static string BuildUrl(IComponent component, Control owner, string initialUrl, string caption, string filter, UrlBuilderOptions options)
		{
			ISite site = component.Site;
			if (site == null)
			{
				return null;
			}
			return UrlBuilder.BuildUrl(site, owner, initialUrl, caption, filter, options);
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x000BC140 File Offset: 0x000BB140
		public static string BuildUrl(IServiceProvider serviceProvider, Control owner, string initialUrl, string caption, string filter, UrlBuilderOptions options)
		{
			string text = string.Empty;
			string text2 = null;
			IDesignerHost designerHost = (IDesignerHost)serviceProvider.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				WebFormsRootDesigner webFormsRootDesigner = designerHost.GetDesigner(designerHost.RootComponent) as WebFormsRootDesigner;
				if (webFormsRootDesigner != null)
				{
					text = webFormsRootDesigner.DocumentUrl;
				}
			}
			if (text.Length == 0)
			{
				IWebFormsDocumentService webFormsDocumentService = (IWebFormsDocumentService)serviceProvider.GetService(typeof(IWebFormsDocumentService));
				if (webFormsDocumentService != null)
				{
					text = webFormsDocumentService.DocumentUrl;
				}
			}
			IWebFormsBuilderUIService webFormsBuilderUIService = (IWebFormsBuilderUIService)serviceProvider.GetService(typeof(IWebFormsBuilderUIService));
			if (webFormsBuilderUIService != null)
			{
				text2 = webFormsBuilderUIService.BuildUrl(owner, initialUrl, text, caption, filter, options);
			}
			return text2;
		}
	}
}
