using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005A6 RID: 1446
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class GridViewRow : TableRow, IDataItemContainer, INamingContainer
	{
		// Token: 0x06004717 RID: 18199 RVA: 0x00123BEC File Offset: 0x00122BEC
		public GridViewRow(int rowIndex, int dataItemIndex, DataControlRowType rowType, DataControlRowState rowState)
		{
			this._rowIndex = rowIndex;
			this._dataItemIndex = dataItemIndex;
			this._rowType = rowType;
			this._rowState = rowState;
		}

		// Token: 0x1700117E RID: 4478
		// (get) Token: 0x06004718 RID: 18200 RVA: 0x00123C11 File Offset: 0x00122C11
		// (set) Token: 0x06004719 RID: 18201 RVA: 0x00123C19 File Offset: 0x00122C19
		public virtual object DataItem
		{
			get
			{
				return this._dataItem;
			}
			set
			{
				this._dataItem = value;
			}
		}

		// Token: 0x1700117F RID: 4479
		// (get) Token: 0x0600471A RID: 18202 RVA: 0x00123C22 File Offset: 0x00122C22
		public virtual int DataItemIndex
		{
			get
			{
				return this._dataItemIndex;
			}
		}

		// Token: 0x17001180 RID: 4480
		// (get) Token: 0x0600471B RID: 18203 RVA: 0x00123C2A File Offset: 0x00122C2A
		public virtual int RowIndex
		{
			get
			{
				return this._rowIndex;
			}
		}

		// Token: 0x17001181 RID: 4481
		// (get) Token: 0x0600471C RID: 18204 RVA: 0x00123C32 File Offset: 0x00122C32
		// (set) Token: 0x0600471D RID: 18205 RVA: 0x00123C3A File Offset: 0x00122C3A
		public virtual DataControlRowState RowState
		{
			get
			{
				return this._rowState;
			}
			set
			{
				this._rowState = value;
			}
		}

		// Token: 0x17001182 RID: 4482
		// (get) Token: 0x0600471E RID: 18206 RVA: 0x00123C43 File Offset: 0x00122C43
		// (set) Token: 0x0600471F RID: 18207 RVA: 0x00123C4B File Offset: 0x00122C4B
		public virtual DataControlRowType RowType
		{
			get
			{
				return this._rowType;
			}
			set
			{
				this._rowType = value;
			}
		}

		// Token: 0x06004720 RID: 18208 RVA: 0x00123C54 File Offset: 0x00122C54
		protected override bool OnBubbleEvent(object source, EventArgs e)
		{
			if (e is CommandEventArgs)
			{
				GridViewCommandEventArgs gridViewCommandEventArgs = new GridViewCommandEventArgs(this, source, (CommandEventArgs)e);
				base.RaiseBubbleEvent(this, gridViewCommandEventArgs);
				return true;
			}
			return false;
		}

		// Token: 0x17001183 RID: 4483
		// (get) Token: 0x06004721 RID: 18209 RVA: 0x00123C82 File Offset: 0x00122C82
		object IDataItemContainer.DataItem
		{
			get
			{
				return this.DataItem;
			}
		}

		// Token: 0x17001184 RID: 4484
		// (get) Token: 0x06004722 RID: 18210 RVA: 0x00123C8A File Offset: 0x00122C8A
		int IDataItemContainer.DataItemIndex
		{
			get
			{
				return this.DataItemIndex;
			}
		}

		// Token: 0x17001185 RID: 4485
		// (get) Token: 0x06004723 RID: 18211 RVA: 0x00123C92 File Offset: 0x00122C92
		int IDataItemContainer.DisplayIndex
		{
			get
			{
				return this.RowIndex;
			}
		}

		// Token: 0x04002A81 RID: 10881
		private int _rowIndex;

		// Token: 0x04002A82 RID: 10882
		private int _dataItemIndex;

		// Token: 0x04002A83 RID: 10883
		private DataControlRowType _rowType;

		// Token: 0x04002A84 RID: 10884
		private DataControlRowState _rowState;

		// Token: 0x04002A85 RID: 10885
		private object _dataItem;
	}
}
