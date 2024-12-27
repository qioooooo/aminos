using System;
using System.ComponentModel.Design;
using System.Design;
using System.Reflection;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200041B RID: 1051
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class WizardStepCollectionEditor : CollectionEditor
	{
		// Token: 0x0600268A RID: 9866 RVA: 0x000D13ED File Offset: 0x000D03ED
		public WizardStepCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x000D13F6 File Offset: 0x000D03F6
		protected override bool CanSelectMultipleInstances()
		{
			return false;
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x000D13FC File Offset: 0x000D03FC
		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			CollectionEditor.CollectionForm collectionForm = base.CreateCollectionForm();
			collectionForm.Text = SR.GetString("CollectionEditorCaption", new object[] { "WizardStep" });
			return collectionForm;
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x000D1431 File Offset: 0x000D0431
		protected override object CreateInstance(Type itemType)
		{
			return Activator.CreateInstance(itemType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null);
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x000D1444 File Offset: 0x000D0444
		protected override Type[] CreateNewItemTypes()
		{
			return new Type[]
			{
				typeof(WizardStep),
				typeof(TemplatedWizardStep)
			};
		}
	}
}
