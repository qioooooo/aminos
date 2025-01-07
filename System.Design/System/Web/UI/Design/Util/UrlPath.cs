using System;
using System.ComponentModel.Design;

namespace System.Web.UI.Design.Util
{
	internal class UrlPath
	{
		private UrlPath()
		{
		}

		private static bool IsAbsolutePhysicalPath(string path)
		{
			return path != null && path.Length >= 3 && (path.StartsWith("\\\\", StringComparison.Ordinal) || (char.IsLetter(path[0]) && path[1] == ':' && path[2] == '\\'));
		}

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
