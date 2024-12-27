using System;
using System.ComponentModel.Design;
using System.Reflection;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004EB RID: 1259
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class TableRowsCollectionEditor : CollectionEditor
	{
		// Token: 0x06002D12 RID: 11538 RVA: 0x000FE8E8 File Offset: 0x000FD8E8
		public TableRowsCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x06002D13 RID: 11539 RVA: 0x000FE8F1 File Offset: 0x000FD8F1
		protected override bool CanSelectMultipleInstances()
		{
			return false;
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x000FE8F4 File Offset: 0x000FD8F4
		protected override object CreateInstance(Type itemType)
		{
			return Activator.CreateInstance(itemType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null);
		}
	}
}
