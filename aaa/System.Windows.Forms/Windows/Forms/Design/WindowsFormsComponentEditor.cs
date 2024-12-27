using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000230 RID: 560
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class WindowsFormsComponentEditor : ComponentEditor
	{
		// Token: 0x06001AC9 RID: 6857 RVA: 0x00033387 File Offset: 0x00032387
		public override bool EditComponent(ITypeDescriptorContext context, object component)
		{
			return this.EditComponent(context, component, null);
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x00033392 File Offset: 0x00032392
		public bool EditComponent(object component, IWin32Window owner)
		{
			return this.EditComponent(null, component, owner);
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x000333A0 File Offset: 0x000323A0
		public virtual bool EditComponent(ITypeDescriptorContext context, object component, IWin32Window owner)
		{
			bool flag = false;
			Type[] componentEditorPages = this.GetComponentEditorPages();
			if (componentEditorPages != null && componentEditorPages.Length != 0)
			{
				ComponentEditorForm componentEditorForm = new ComponentEditorForm(component, componentEditorPages);
				if (componentEditorForm.ShowForm(owner, this.GetInitialComponentEditorPageIndex()) == DialogResult.OK)
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x000333D9 File Offset: 0x000323D9
		protected virtual Type[] GetComponentEditorPages()
		{
			return null;
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x000333DC File Offset: 0x000323DC
		protected virtual int GetInitialComponentEditorPageIndex()
		{
			return 0;
		}
	}
}
