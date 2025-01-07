using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal abstract class BaseTypeViewSchema : IDataSourceViewSchema
	{
		protected BaseTypeViewSchema(string viewName, Type type)
		{
			this._type = type;
			this._viewName = viewName;
		}

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

		public IDataSourceViewSchema[] GetChildren()
		{
			return null;
		}

		protected abstract Type GetRowType(Type objectType);

		public string Name
		{
			get
			{
				return this._viewName;
			}
		}

		private Type _type;

		private string _viewName;
	}
}
