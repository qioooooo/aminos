using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public abstract class BaseDataListComponentEditor : WindowsFormsComponentEditor
	{
		public BaseDataListComponentEditor(int initialPage)
		{
			this.initialPage = initialPage;
		}

		public override bool EditComponent(ITypeDescriptorContext context, object obj, IWin32Window parent)
		{
			bool flag = false;
			bool flag2 = false;
			IComponent component = (IComponent)obj;
			ISite site = component.Site;
			if (site != null)
			{
				IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
				IDesigner designer = designerHost.GetDesigner(component);
				TemplatedControlDesigner templatedControlDesigner = (TemplatedControlDesigner)designer;
				flag2 = templatedControlDesigner.InTemplateModeInternal;
			}
			if (!flag2)
			{
				Type[] componentEditorPages = this.GetComponentEditorPages();
				if (componentEditorPages != null && componentEditorPages.Length != 0)
				{
					ComponentEditorForm componentEditorForm = new ComponentEditorForm(obj, componentEditorPages);
					string @string = SR.GetString("RTL");
					if (!string.Equals(@string, "RTL_False", StringComparison.Ordinal))
					{
						componentEditorForm.RightToLeft = RightToLeft.Yes;
						componentEditorForm.RightToLeftLayout = true;
					}
					if (componentEditorForm.ShowForm(parent, this.GetInitialComponentEditorPageIndex()) == DialogResult.OK)
					{
						flag = true;
					}
				}
			}
			else
			{
				RTLAwareMessageBox.Show(null, SR.GetString("BDL_TemplateModePropBuilder"), SR.GetString("BDL_PropertyBuilder"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
			}
			return flag;
		}

		protected override int GetInitialComponentEditorPageIndex()
		{
			return this.initialPage;
		}

		private int initialPage;
	}
}
