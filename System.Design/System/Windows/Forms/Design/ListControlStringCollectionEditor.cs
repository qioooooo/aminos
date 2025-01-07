using System;
using System.ComponentModel;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class ListControlStringCollectionEditor : StringCollectionEditor
	{
		public ListControlStringCollectionEditor(Type type)
			: base(type)
		{
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			ListControl listControl = context.Instance as ListControl;
			if (listControl != null && listControl.DataSource != null)
			{
				throw new ArgumentException(SR.GetString("DataSourceLocksItems"));
			}
			return base.EditValue(context, provider, value);
		}
	}
}
