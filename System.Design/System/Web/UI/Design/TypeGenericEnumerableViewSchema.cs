using System;
using System.Collections.Generic;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class TypeGenericEnumerableViewSchema : BaseTypeViewSchema
	{
		public TypeGenericEnumerableViewSchema(string viewName, Type type)
			: base(viewName, type)
		{
		}

		protected override Type GetRowType(Type objectType)
		{
			Type type = null;
			if (objectType.IsInterface && objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
			{
				type = objectType;
			}
			else
			{
				Type[] interfaces = objectType.GetInterfaces();
				foreach (Type type2 in interfaces)
				{
					if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeof(IEnumerable<>))
					{
						type = type2;
						break;
					}
				}
			}
			Type[] genericArguments = type.GetGenericArguments();
			if (genericArguments[0].IsGenericParameter)
			{
				return null;
			}
			return genericArguments[0];
		}
	}
}
