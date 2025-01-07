using System;
using System.Reflection;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class TypeEnumerableViewSchema : BaseTypeViewSchema
	{
		public TypeEnumerableViewSchema(string viewName, Type type)
			: base(viewName, type)
		{
		}

		protected override Type GetRowType(Type objectType)
		{
			if (objectType.IsArray)
			{
				return objectType.GetElementType();
			}
			PropertyInfo[] properties = objectType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			foreach (PropertyInfo propertyInfo in properties)
			{
				ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
				if (indexParameters.Length > 0)
				{
					return propertyInfo.PropertyType;
				}
			}
			return null;
		}
	}
}
