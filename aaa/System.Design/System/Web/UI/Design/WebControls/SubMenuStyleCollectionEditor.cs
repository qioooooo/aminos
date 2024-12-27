using System;
using System.ComponentModel.Design;
using System.Design;
using System.Reflection;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004E7 RID: 1255
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class SubMenuStyleCollectionEditor : CollectionEditor
	{
		// Token: 0x06002D06 RID: 11526 RVA: 0x000FE6B7 File Offset: 0x000FD6B7
		public SubMenuStyleCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x06002D07 RID: 11527 RVA: 0x000FE6C0 File Offset: 0x000FD6C0
		protected override bool CanSelectMultipleInstances()
		{
			return false;
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x000FE6C4 File Offset: 0x000FD6C4
		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			CollectionEditor.CollectionForm collectionForm = base.CreateCollectionForm();
			collectionForm.Text = SR.GetString("CollectionEditorCaption", new object[] { "SubMenuStyle" });
			return collectionForm;
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x000FE6F9 File Offset: 0x000FD6F9
		protected override object CreateInstance(Type itemType)
		{
			return Activator.CreateInstance(itemType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null);
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x000FE70C File Offset: 0x000FD70C
		protected override Type[] CreateNewItemTypes()
		{
			return new Type[] { typeof(SubMenuStyle) };
		}
	}
}
