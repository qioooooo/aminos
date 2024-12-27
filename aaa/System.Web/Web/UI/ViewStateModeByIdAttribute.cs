using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200048C RID: 1164
	[AttributeUsage(AttributeTargets.Class)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ViewStateModeByIdAttribute : Attribute
	{
		// Token: 0x0600369E RID: 13982 RVA: 0x000EB8DC File Offset: 0x000EA8DC
		internal static bool IsEnabled(Type type)
		{
			if (!ViewStateModeByIdAttribute._viewStateIdTypes.ContainsKey(type))
			{
				AttributeCollection attributes = TypeDescriptor.GetAttributes(type);
				ViewStateModeByIdAttribute viewStateModeByIdAttribute = (ViewStateModeByIdAttribute)attributes[typeof(ViewStateModeByIdAttribute)];
				ViewStateModeByIdAttribute._viewStateIdTypes[type] = viewStateModeByIdAttribute != null;
			}
			return (bool)ViewStateModeByIdAttribute._viewStateIdTypes[type];
		}

		// Token: 0x040025A7 RID: 9639
		private static Hashtable _viewStateIdTypes = Hashtable.Synchronized(new Hashtable());
	}
}
