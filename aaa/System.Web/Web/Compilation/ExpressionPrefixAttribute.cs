using System;
using System.Security.Permissions;

namespace System.Web.Compilation
{
	// Token: 0x0200016F RID: 367
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ExpressionPrefixAttribute : Attribute
	{
		// Token: 0x06001063 RID: 4195 RVA: 0x00048F69 File Offset: 0x00047F69
		public ExpressionPrefixAttribute(string expressionPrefix)
		{
			if (string.IsNullOrEmpty(expressionPrefix))
			{
				throw new ArgumentNullException("expressionPrefix");
			}
			this._expressionPrefix = expressionPrefix;
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001064 RID: 4196 RVA: 0x00048F8B File Offset: 0x00047F8B
		public string ExpressionPrefix
		{
			get
			{
				return this._expressionPrefix;
			}
		}

		// Token: 0x04001651 RID: 5713
		private string _expressionPrefix;
	}
}
