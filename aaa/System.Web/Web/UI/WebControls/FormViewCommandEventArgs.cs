using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000582 RID: 1410
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class FormViewCommandEventArgs : CommandEventArgs
	{
		// Token: 0x060045B2 RID: 17842 RVA: 0x0011E8C1 File Offset: 0x0011D8C1
		public FormViewCommandEventArgs(object commandSource, CommandEventArgs originalArgs)
			: base(originalArgs)
		{
			this._commandSource = commandSource;
		}

		// Token: 0x17001113 RID: 4371
		// (get) Token: 0x060045B3 RID: 17843 RVA: 0x0011E8D1 File Offset: 0x0011D8D1
		public object CommandSource
		{
			get
			{
				return this._commandSource;
			}
		}

		// Token: 0x04002A0D RID: 10765
		private object _commandSource;
	}
}
