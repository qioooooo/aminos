using System;
using System.ComponentModel.Design;
using System.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200041C RID: 1052
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class CreateUserWizardStepCollectionEditor : WizardStepCollectionEditor
	{
		// Token: 0x0600268F RID: 9871 RVA: 0x000D1473 File Offset: 0x000D0473
		public CreateUserWizardStepCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06002690 RID: 9872 RVA: 0x000D147C File Offset: 0x000D047C
		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.CreateUserWizard.StepCollectionEditor";
			}
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x000D1483 File Offset: 0x000D0483
		protected override bool CanRemoveInstance(object value)
		{
			return !(value is CompleteWizardStep) && !(value is CreateUserWizardStep);
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x000D149C File Offset: 0x000D049C
		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			CollectionEditor.CollectionForm collectionForm = base.CreateCollectionForm();
			collectionForm.Text = SR.GetString("CreateUserWizardStepCollectionEditor_Caption");
			return collectionForm;
		}
	}
}
