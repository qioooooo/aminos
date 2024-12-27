using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000553 RID: 1363
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class DayRenderEventArgs
	{
		// Token: 0x0600432A RID: 17194 RVA: 0x00115578 File Offset: 0x00114578
		public DayRenderEventArgs(TableCell cell, CalendarDay day)
		{
			this.day = day;
			this.cell = cell;
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x0011558E File Offset: 0x0011458E
		public DayRenderEventArgs(TableCell cell, CalendarDay day, string selectUrl)
		{
			this.day = day;
			this.cell = cell;
			this.selectUrl = selectUrl;
		}

		// Token: 0x17001059 RID: 4185
		// (get) Token: 0x0600432C RID: 17196 RVA: 0x001155AB File Offset: 0x001145AB
		public TableCell Cell
		{
			get
			{
				return this.cell;
			}
		}

		// Token: 0x1700105A RID: 4186
		// (get) Token: 0x0600432D RID: 17197 RVA: 0x001155B3 File Offset: 0x001145B3
		public CalendarDay Day
		{
			get
			{
				return this.day;
			}
		}

		// Token: 0x1700105B RID: 4187
		// (get) Token: 0x0600432E RID: 17198 RVA: 0x001155BB File Offset: 0x001145BB
		public string SelectUrl
		{
			get
			{
				return this.selectUrl;
			}
		}

		// Token: 0x0400294F RID: 10575
		private CalendarDay day;

		// Token: 0x04002950 RID: 10576
		private TableCell cell;

		// Token: 0x04002951 RID: 10577
		private string selectUrl;
	}
}
