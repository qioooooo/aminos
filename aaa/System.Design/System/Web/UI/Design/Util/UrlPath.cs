using System;
using System.ComponentModel.Design;

namespace System.Web.UI.Design.Util
{
	// Token: 0x020003D1 RID: 977
	internal class UrlPath
	{
		// Token: 0x060023F4 RID: 9204 RVA: 0x000C07F3 File Offset: 0x000BF7F3
		private UrlPath()
		{
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x000C07FC File Offset: 0x000BF7FC
		private static bool IsAbsolutePhysicalPath(string path)
		{
			return path != null && path.Length >= 3 && (path.StartsWith("\\\\", StringComparison.Ordinal) || (char.IsLetter(path[0]) && path[1] == ':' && path[2] == '\\'));
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x000C0850 File Offset: 0x000BF850
		internal static string MapPath(IServiceProvider serviceProvider, string path)
		{
			if (path.Length == 0)
			{
				return null;
			}
			if (UrlPath.IsAbsolutePhysicalPath(path))
			{
				return path;
			}
			if (serviceProvider != null)
			{
				IDesignerHost designerHost = (IDesignerHost)serviceProvider.GetService(typeof(IDesignerHost));
				if (designerHost != null && designerHost.RootComponent != null)
				{
					WebFormsRootDesigner webFormsRootDesigner = designerHost.GetDesigner(designerHost.RootComponent) as WebFormsRootDesigner;
					if (webFormsRootDesigner != null)
					{
						string text = webFormsRootDesigner.ResolveUrl(path);
						IWebApplication webApplication = (IWebApplication)serviceProvider.GetService(typeof(IWebApplication));
						if (webApplication != null)
						{
							IProjectItem projectItemFromUrl = webApplication.GetProjectItemFromUrl(text);
							if (projectItemFromUrl != null)
							{
								return projectItemFromUrl.PhysicalPath;
							}
						}
					}
				}
			}
			return null;
		}
	}
}
