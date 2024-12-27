using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200044B RID: 1099
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class EmbeddedMailObjectCollectionEditor : CollectionEditor
	{
		// Token: 0x060027F8 RID: 10232 RVA: 0x000DB1D7 File Offset: 0x000DA1D7
		public EmbeddedMailObjectCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x060027F9 RID: 10233 RVA: 0x000DB1E0 File Offset: 0x000DA1E0
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			object obj;
			try
			{
				context.OnComponentChanging();
				obj = base.EditValue(context, provider, value);
			}
			finally
			{
				context.OnComponentChanged();
			}
			return obj;
		}
	}
}
