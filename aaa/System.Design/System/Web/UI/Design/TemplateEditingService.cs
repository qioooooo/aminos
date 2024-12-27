using System;
using System.ComponentModel.Design;
using System.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	// Token: 0x02000399 RID: 921
	[Obsolete("Use of this type is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public sealed class TemplateEditingService : ITemplateEditingService, IDisposable
	{
		// Token: 0x0600220E RID: 8718 RVA: 0x000BB417 File Offset: 0x000BA417
		public TemplateEditingService(IDesignerHost designerHost)
		{
			if (designerHost == null)
			{
				throw new ArgumentNullException("designerHost");
			}
			this.designerHost = designerHost;
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x0600220F RID: 8719 RVA: 0x000BB434 File Offset: 0x000BA434
		public bool SupportsNestedTemplateEditing
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x000BB437 File Offset: 0x000BA437
		public ITemplateEditingFrame CreateFrame(TemplatedControlDesigner designer, string frameName, string[] templateNames)
		{
			return this.CreateFrame(designer, frameName, templateNames, null, null);
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x000BB444 File Offset: 0x000BA444
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

		// Token: 0x06002212 RID: 8722 RVA: 0x000BB4B8 File Offset: 0x000BA4B8
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

		// Token: 0x06002213 RID: 8723 RVA: 0x000BB4F7 File Offset: 0x000BA4F7
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x000BB508 File Offset: 0x000BA508
		~TemplateEditingService()
		{
			this.Dispose(false);
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x000BB538 File Offset: 0x000BA538
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.designerHost = null;
			}
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x000BB544 File Offset: 0x000BA544
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

		// Token: 0x04001843 RID: 6211
		private IDesignerHost designerHost;
	}
}
