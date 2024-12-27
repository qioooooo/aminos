using System;
using System.ComponentModel.Design;
using System.Reflection;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004E6 RID: 1254
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class StyleCollectionEditor : CollectionEditor
	{
		// Token: 0x06002D04 RID: 11524 RVA: 0x000FE69E File Offset: 0x000FD69E
		public StyleCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x06002D05 RID: 11525 RVA: 0x000FE6A7 File Offset: 0x000FD6A7
		protected override object CreateInstance(Type itemType)
		{
			return Activator.CreateInstance(itemType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null);
		}
	}
}
