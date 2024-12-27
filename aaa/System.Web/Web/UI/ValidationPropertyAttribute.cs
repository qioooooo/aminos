using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000484 RID: 1156
	[AttributeUsage(AttributeTargets.Class)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ValidationPropertyAttribute : Attribute
	{
		// Token: 0x0600366C RID: 13932 RVA: 0x000EB43C File Offset: 0x000EA43C
		public ValidationPropertyAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x0600366D RID: 13933 RVA: 0x000EB44B File Offset: 0x000EA44B
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x04002582 RID: 9602
		private readonly string name;
	}
}
