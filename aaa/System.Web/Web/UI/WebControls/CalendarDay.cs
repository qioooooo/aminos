using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004DD RID: 1245
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class CalendarDay
	{
		// Token: 0x06003C18 RID: 15384 RVA: 0x000FD373 File Offset: 0x000FC373
		public CalendarDay(DateTime date, bool isWeekend, bool isToday, bool isSelected, bool isOtherMonth, string dayNumberText)
		{
			this.date = date;
			this.isWeekend = isWeekend;
			this.isToday = isToday;
			this.isOtherMonth = isOtherMonth;
			this.isSelected = isSelected;
			this.dayNumberText = dayNumberText;
		}

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x06003C19 RID: 15385 RVA: 0x000FD3A8 File Offset: 0x000FC3A8
		public DateTime Date
		{
			get
			{
				return this.date;
			}
		}

		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x06003C1A RID: 15386 RVA: 0x000FD3B0 File Offset: 0x000FC3B0
		public string DayNumberText
		{
			get
			{
				return this.dayNumberText;
			}
		}

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x06003C1B RID: 15387 RVA: 0x000FD3B8 File Offset: 0x000FC3B8
		public bool IsOtherMonth
		{
			get
			{
				return this.isOtherMonth;
			}
		}

		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x06003C1C RID: 15388 RVA: 0x000FD3C0 File Offset: 0x000FC3C0
		// (set) Token: 0x06003C1D RID: 15389 RVA: 0x000FD3C8 File Offset: 0x000FC3C8
		public bool IsSelectable
		{
			get
			{
				return this.isSelectable;
			}
			set
			{
				this.isSelectable = value;
			}
		}

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x06003C1E RID: 15390 RVA: 0x000FD3D1 File Offset: 0x000FC3D1
		public bool IsSelected
		{
			get
			{
				return this.isSelected;
			}
		}

		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x06003C1F RID: 15391 RVA: 0x000FD3D9 File Offset: 0x000FC3D9
		public bool IsToday
		{
			get
			{
				return this.isToday;
			}
		}

		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x06003C20 RID: 15392 RVA: 0x000FD3E1 File Offset: 0x000FC3E1
		public bool IsWeekend
		{
			get
			{
				return this.isWeekend;
			}
		}

		// Token: 0x040026F5 RID: 9973
		private DateTime date;

		// Token: 0x040026F6 RID: 9974
		private bool isSelectable;

		// Token: 0x040026F7 RID: 9975
		private bool isToday;

		// Token: 0x040026F8 RID: 9976
		private bool isWeekend;

		// Token: 0x040026F9 RID: 9977
		private bool isOtherMonth;

		// Token: 0x040026FA RID: 9978
		private bool isSelected;

		// Token: 0x040026FB RID: 9979
		private string dayNumberText;
	}
}
