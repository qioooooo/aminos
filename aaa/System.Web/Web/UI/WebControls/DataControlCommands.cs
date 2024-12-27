using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200052D RID: 1325
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class DataControlCommands
	{
		// Token: 0x06004132 RID: 16690 RVA: 0x0010ECC7 File Offset: 0x0010DCC7
		private DataControlCommands()
		{
		}

		// Token: 0x0400289F RID: 10399
		public const string SortCommandName = "Sort";

		// Token: 0x040028A0 RID: 10400
		public const string SelectCommandName = "Select";

		// Token: 0x040028A1 RID: 10401
		public const string EditCommandName = "Edit";

		// Token: 0x040028A2 RID: 10402
		public const string DeleteCommandName = "Delete";

		// Token: 0x040028A3 RID: 10403
		public const string UpdateCommandName = "Update";

		// Token: 0x040028A4 RID: 10404
		public const string CancelCommandName = "Cancel";

		// Token: 0x040028A5 RID: 10405
		public const string PageCommandName = "Page";

		// Token: 0x040028A6 RID: 10406
		public const string NextPageCommandArgument = "Next";

		// Token: 0x040028A7 RID: 10407
		public const string PreviousPageCommandArgument = "Prev";

		// Token: 0x040028A8 RID: 10408
		public const string FirstPageCommandArgument = "First";

		// Token: 0x040028A9 RID: 10409
		public const string LastPageCommandArgument = "Last";

		// Token: 0x040028AA RID: 10410
		public const string InsertCommandName = "Insert";

		// Token: 0x040028AB RID: 10411
		public const string NewCommandName = "New";
	}
}
