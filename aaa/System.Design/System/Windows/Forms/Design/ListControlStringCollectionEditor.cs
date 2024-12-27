using System;
using System.ComponentModel;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000263 RID: 611
	internal class ListControlStringCollectionEditor : StringCollectionEditor
	{
		// Token: 0x06001724 RID: 5924 RVA: 0x000777C1 File Offset: 0x000767C1
		public ListControlStringCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x000777CC File Offset: 0x000767CC
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
