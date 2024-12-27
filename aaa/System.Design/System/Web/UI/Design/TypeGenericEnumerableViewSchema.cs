using System;
using System.Collections.Generic;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x020003A2 RID: 930
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class TypeGenericEnumerableViewSchema : BaseTypeViewSchema
	{
		// Token: 0x06002259 RID: 8793 RVA: 0x000BBD94 File Offset: 0x000BAD94
		public TypeGenericEnumerableViewSchema(string viewName, Type type)
			: base(viewName, type)
		{
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x000BBDA0 File Offset: 0x000BADA0
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
