using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Diagnostics.Design
{
	// Token: 0x02000316 RID: 790
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ProcessThreadDesigner : ComponentDesigner
	{
		// Token: 0x06001DF4 RID: 7668 RVA: 0x000AB288 File Offset: 0x000AA288
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			RuntimeComponentFilter.FilterProperties(properties, null, new string[] { "IdealProcessor", "ProcessorAffinity" });
		}
	}
}
