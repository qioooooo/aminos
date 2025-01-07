using System;
using System.ComponentModel.Design;
using System.Design;
using System.Reflection;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class WizardStepCollectionEditor : CollectionEditor
	{
		public WizardStepCollectionEditor(Type type)
			: base(type)
		{
		}

		protected override bool CanSelectMultipleInstances()
		{
			return false;
		}

		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			CollectionEditor.CollectionForm collectionForm = base.CreateCollectionForm();
			collectionForm.Text = SR.GetString("CollectionEditorCaption", new object[] { "WizardStep" });
			return collectionForm;
		}

		protected override object CreateInstance(Type itemType)
		{
			return Activator.CreateInstance(itemType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null);
		}

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
