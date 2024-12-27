using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200053C RID: 1340
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DataGridItem : TableRow, IDataItemContainer, INamingContainer
	{
		// Token: 0x06004218 RID: 16920 RVA: 0x001120FA File Offset: 0x001110FA
		public DataGridItem(int itemIndex, int dataSetIndex, ListItemType itemType)
		{
			this.itemIndex = itemIndex;
			this.dataSetIndex = dataSetIndex;
			this.itemType = itemType;
		}

		// Token: 0x17000FF0 RID: 4080
		// (get) Token: 0x06004219 RID: 16921 RVA: 0x00112117 File Offset: 0x00111117
		// (set) Token: 0x0600421A RID: 16922 RVA: 0x0011211F File Offset: 0x0011111F
		public virtual object DataItem
		{
			get
			{
				return this.dataItem;
			}
			set
			{
				this.dataItem = value;
			}
		}

		// Token: 0x17000FF1 RID: 4081
		// (get) Token: 0x0600421B RID: 16923 RVA: 0x00112128 File Offset: 0x00111128
		public virtual int DataSetIndex
		{
			get
			{
				return this.dataSetIndex;
			}
		}

		// Token: 0x17000FF2 RID: 4082
		// (get) Token: 0x0600421C RID: 16924 RVA: 0x00112130 File Offset: 0x00111130
		public virtual int ItemIndex
		{
			get
			{
				return this.itemIndex;
			}
		}

		// Token: 0x17000FF3 RID: 4083
		// (get) Token: 0x0600421D RID: 16925 RVA: 0x00112138 File Offset: 0x00111138
		public virtual ListItemType ItemType
		{
			get
			{
				return this.itemType;
			}
		}

		// Token: 0x0600421E RID: 16926 RVA: 0x00112140 File Offset: 0x00111140
		protected override bool OnBubbleEvent(object source, EventArgs e)
		{
			if (e is CommandEventArgs)
			{
				DataGridCommandEventArgs dataGridCommandEventArgs = new DataGridCommandEventArgs(this, source, (CommandEventArgs)e);
				base.RaiseBubbleEvent(this, dataGridCommandEventArgs);
				return true;
			}
			return false;
		}

		// Token: 0x0600421F RID: 16927 RVA: 0x0011216E File Offset: 0x0011116E
		protected internal virtual void SetItemType(ListItemType itemType)
		{
			this.itemType = itemType;
		}

		// Token: 0x17000FF4 RID: 4084
		// (get) Token: 0x06004220 RID: 16928 RVA: 0x00112177 File Offset: 0x00111177
		object IDataItemContainer.DataItem
		{
			get
			{
				return this.DataItem;
			}
		}

		// Token: 0x17000FF5 RID: 4085
		// (get) Token: 0x06004221 RID: 16929 RVA: 0x0011217F File Offset: 0x0011117F
		int IDataItemContainer.DataItemIndex
		{
			get
			{
				return this.DataSetIndex;
			}
		}

		// Token: 0x17000FF6 RID: 4086
		// (get) Token: 0x06004222 RID: 16930 RVA: 0x00112187 File Offset: 0x00111187
		int IDataItemContainer.DisplayIndex
		{
			get
			{
				return this.ItemIndex;
			}
		}

		// Token: 0x040028F0 RID: 10480
		private int itemIndex;

		// Token: 0x040028F1 RID: 10481
		private int dataSetIndex;

		// Token: 0x040028F2 RID: 10482
		private ListItemType itemType;

		// Token: 0x040028F3 RID: 10483
		private object dataItem;
	}
}
