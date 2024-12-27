using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x02000353 RID: 851
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataSourceBooleanViewSchemaConverter : DataSourceViewSchemaConverter
	{
		// Token: 0x06001FE9 RID: 8169 RVA: 0x000B5D84 File Offset: 0x000B4D84
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			return this.GetStandardValues(context, typeof(bool));
		}
	}
}
