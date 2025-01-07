using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class EmbeddedMailObjectCollectionEditor : CollectionEditor
	{
		public EmbeddedMailObjectCollectionEditor(Type type)
			: base(type)
		{
		}

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
