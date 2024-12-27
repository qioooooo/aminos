using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000567 RID: 1383
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DetailsViewRow : TableRow
	{
		// Token: 0x06004433 RID: 17459 RVA: 0x001198DA File Offset: 0x001188DA
		public DetailsViewRow(int rowIndex, DataControlRowType rowType, DataControlRowState rowState)
		{
			this._rowIndex = rowIndex;
			this._rowType = rowType;
			this._rowState = rowState;
		}

		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x06004434 RID: 17460 RVA: 0x001198F7 File Offset: 0x001188F7
		public virtual int RowIndex
		{
			get
			{
				return this._rowIndex;
			}
		}

		// Token: 0x170010AA RID: 4266
		// (get) Token: 0x06004435 RID: 17461 RVA: 0x001198FF File Offset: 0x001188FF
		public virtual DataControlRowState RowState
		{
			get
			{
				return this._rowState;
			}
		}

		// Token: 0x170010AB RID: 4267
		// (get) Token: 0x06004436 RID: 17462 RVA: 0x00119907 File Offset: 0x00118907
		public virtual DataControlRowType RowType
		{
			get
			{
				return this._rowType;
			}
		}

		// Token: 0x06004437 RID: 17463 RVA: 0x00119910 File Offset: 0x00118910
		protected override bool OnBubbleEvent(object source, EventArgs e)
		{
			if (e is CommandEventArgs)
			{
				DetailsViewCommandEventArgs detailsViewCommandEventArgs = new DetailsViewCommandEventArgs(source, (CommandEventArgs)e);
				base.RaiseBubbleEvent(this, detailsViewCommandEventArgs);
				return true;
			}
			return false;
		}

		// Token: 0x040029A3 RID: 10659
		private int _rowIndex;

		// Token: 0x040029A4 RID: 10660
		private DataControlRowType _rowType;

		// Token: 0x040029A5 RID: 10661
		private DataControlRowState _rowState;
	}
}
