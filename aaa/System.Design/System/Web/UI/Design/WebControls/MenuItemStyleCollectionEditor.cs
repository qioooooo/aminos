using System;
using System.ComponentModel.Design;
using System.Design;
using System.Reflection;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000480 RID: 1152
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class MenuItemStyleCollectionEditor : CollectionEditor
	{
		// Token: 0x060029CC RID: 10700 RVA: 0x000E544D File Offset: 0x000E444D
		public MenuItemStyleCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x060029CD RID: 10701 RVA: 0x000E5456 File Offset: 0x000E4456
		protected override bool CanSelectMultipleInstances()
		{
			return false;
		}

		// Token: 0x060029CE RID: 10702 RVA: 0x000E545C File Offset: 0x000E445C
		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			CollectionEditor.CollectionForm collectionForm = base.CreateCollectionForm();
			collectionForm.Text = SR.GetString("CollectionEditorCaption", new object[] { "MenuItemStyle" });
			return collectionForm;
		}

		// Token: 0x060029CF RID: 10703 RVA: 0x000E5491 File Offset: 0x000E4491
		protected override object CreateInstance(Type itemType)
		{
			return Activator.CreateInstance(itemType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null);
		}

		// Token: 0x060029D0 RID: 10704 RVA: 0x000E54A4 File Offset: 0x000E44A4
		protected override Type[] CreateNewItemTypes()
		{
			return new Type[] { typeof(MenuItemStyle) };
		}
	}
}
