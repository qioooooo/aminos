using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Diagnostics.Design
{
	// Token: 0x02000315 RID: 789
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ProcessModuleDesigner : ComponentDesigner
	{
		// Token: 0x06001DF2 RID: 7666 RVA: 0x000AB254 File Offset: 0x000AA254
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			RuntimeComponentFilter.FilterProperties(properties, null, new string[] { "FileVersionInfo" });
		}
	}
}
