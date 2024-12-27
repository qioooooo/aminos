using System;
using System.Reflection;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x020003A0 RID: 928
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class TypeEnumerableViewSchema : BaseTypeViewSchema
	{
		// Token: 0x0600224B RID: 8779 RVA: 0x000BBBBC File Offset: 0x000BABBC
		public TypeEnumerableViewSchema(string viewName, Type type)
			: base(viewName, type)
		{
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x000BBBC8 File Offset: 0x000BABC8
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
