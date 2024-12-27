using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200046A RID: 1130
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class SupportsEventValidationAttribute : Attribute
	{
		// Token: 0x0600358F RID: 13711 RVA: 0x000E73A8 File Offset: 0x000E63A8
		internal static bool SupportsEventValidation(Type type)
		{
			object obj = SupportsEventValidationAttribute._typesSupportsEventValidation[type];
			if (obj != null)
			{
				return (bool)obj;
			}
			object[] customAttributes = type.GetCustomAttributes(typeof(SupportsEventValidationAttribute), false);
			bool flag = customAttributes != null && customAttributes.Length > 0;
			SupportsEventValidationAttribute._typesSupportsEventValidation[type] = flag;
			return flag;
		}

		// Token: 0x04002533 RID: 9523
		private static Hashtable _typesSupportsEventValidation = Hashtable.Synchronized(new Hashtable());
	}
}
