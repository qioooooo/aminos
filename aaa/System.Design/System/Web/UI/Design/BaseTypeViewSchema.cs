using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x02000320 RID: 800
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal abstract class BaseTypeViewSchema : IDataSourceViewSchema
	{
		// Token: 0x06001E22 RID: 7714 RVA: 0x000ABA0F File Offset: 0x000AAA0F
		protected BaseTypeViewSchema(string viewName, Type type)
		{
			this._type = type;
			this._viewName = viewName;
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x000ABA28 File Offset: 0x000AAA28
		public IDataSourceFieldSchema[] GetFields()
		{
			List<IDataSourceFieldSchema> list = new List<IDataSourceFieldSchema>();
			Type rowType = this.GetRowType(this._type);
			if (rowType != null && !typeof(ICustomTypeDescriptor).IsAssignableFrom(rowType))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(rowType);
				foreach (object obj in properties)
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
					list.Add(new TypeFieldSchema(propertyDescriptor));
				}
			}
			return list.ToArray();
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x000ABAC0 File Offset: 0x000AAAC0
		public IDataSourceViewSchema[] GetChildren()
		{
			return null;
		}

		// Token: 0x06001E25 RID: 7717
		protected abstract Type GetRowType(Type objectType);

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001E26 RID: 7718 RVA: 0x000ABAC3 File Offset: 0x000AAAC3
		public string Name
		{
			get
			{
				return this._viewName;
			}
		}

		// Token: 0x0400172A RID: 5930
		private Type _type;

		// Token: 0x0400172B RID: 5931
		private string _viewName;
	}
}
