using System;
using System.ComponentModel.Design;
using System.Reflection;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004E9 RID: 1257
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class TableCellsCollectionEditor : CollectionEditor
	{
		// Token: 0x06002D0D RID: 11533 RVA: 0x000FE73E File Offset: 0x000FD73E
		public TableCellsCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x000FE747 File Offset: 0x000FD747
		protected override bool CanSelectMultipleInstances()
		{
			return false;
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x000FE74A File Offset: 0x000FD74A
		protected override object CreateInstance(Type itemType)
		{
			return Activator.CreateInstance(itemType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null);
		}
	}
}
