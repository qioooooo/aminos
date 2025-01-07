using System;
using System.ComponentModel.Design;
using System.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	[Obsolete("Use of this type is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public sealed class TemplateEditingService : ITemplateEditingService, IDisposable
	{
		public TemplateEditingService(IDesignerHost designerHost)
		{
			if (designerHost == null)
			{
				throw new ArgumentNullException("designerHost");
			}
			this.designerHost = designerHost;
		}

		public bool SupportsNestedTemplateEditing
		{
			get
			{
				return false;
			}
		}

		public ITemplateEditingFrame CreateFrame(TemplatedControlDesigner designer, string frameName, string[] templateNames)
		{
			return this.CreateFrame(designer, frameName, templateNames, null, null);
		}

		public ITemplateEditingFrame CreateFrame(TemplatedControlDesigner designer, string frameName, string[] templateNames, Style controlStyle, Style[] templateStyles)
		{
			if (designer == null)
			{
				throw new ArgumentNullException("designer");
			}
			if (frameName == null || frameName.Length == 0)
			{
				throw new ArgumentNullException("frameName");
			}
			if (templateNames == null || templateNames.Length == 0)
			{
				throw new ArgumentException("templateNames");
			}
			if (templateStyles != null && templateStyles.Length != templateNames.Length)
			{
				throw new ArgumentException("templateStyles");
			}
			frameName = this.CreateFrameName(frameName);
			return new TemplateEditingFrame(designer, frameName, templateNames, controlStyle, templateStyles);
		}

		private string CreateFrameName(string frameName)
		{
			int num = frameName.IndexOf('&');
			if (num < 0)
			{
				return frameName;
			}
			if (num == 0)
			{
				return frameName.Substring(num + 1);
			}
			return frameName.Substring(0, num) + frameName.Substring(num + 1);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		~TemplateEditingService()
		{
			this.Dispose(false);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.designerHost = null;
			}
		}

		public string GetContainingTemplateName(Control control)
		{
			string text = string.Empty;
			HtmlControlDesigner htmlControlDesigner = (HtmlControlDesigner)this.designerHost.GetDesigner(control);
			if (htmlControlDesigner != null)
			{
				IHtmlControlDesignerBehavior behaviorInternal = htmlControlDesigner.BehaviorInternal;
				NativeMethods.IHTMLElement ihtmlelement = (NativeMethods.IHTMLElement)behaviorInternal.DesignTimeElement;
				if (ihtmlelement != null)
				{
					object[] array = new object[1];
					NativeMethods.IHTMLElement parentElement;
					for (NativeMethods.IHTMLElement ihtmlelement2 = ihtmlelement.GetParentElement(); ihtmlelement2 != null; ihtmlelement2 = parentElement)
					{
						ihtmlelement2.GetAttribute("templatename", 0, array);
						if (array[0] != null && array[0].GetType() == typeof(string))
						{
							text = array[0].ToString();
							break;
						}
						parentElement = ihtmlelement2.GetParentElement();
					}
				}
			}
			return text;
		}

		private IDesignerHost designerHost;
	}
}
