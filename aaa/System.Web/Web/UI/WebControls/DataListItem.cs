using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200054E RID: 1358
	[ToolboxItem(false)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DataListItem : WebControl, IDataItemContainer, INamingContainer
	{
		// Token: 0x0600430F RID: 17167 RVA: 0x0011532E File Offset: 0x0011432E
		public DataListItem(int itemIndex, ListItemType itemType)
		{
			this.itemIndex = itemIndex;
			this.itemType = itemType;
		}

		// Token: 0x1700104D RID: 4173
		// (get) Token: 0x06004310 RID: 17168 RVA: 0x00115344 File Offset: 0x00114344
		// (set) Token: 0x06004311 RID: 17169 RVA: 0x0011534C File Offset: 0x0011434C
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

		// Token: 0x1700104E RID: 4174
		// (get) Token: 0x06004312 RID: 17170 RVA: 0x00115355 File Offset: 0x00114355
		public virtual int ItemIndex
		{
			get
			{
				return this.itemIndex;
			}
		}

		// Token: 0x1700104F RID: 4175
		// (get) Token: 0x06004313 RID: 17171 RVA: 0x0011535D File Offset: 0x0011435D
		public virtual ListItemType ItemType
		{
			get
			{
				return this.itemType;
			}
		}

		// Token: 0x06004314 RID: 17172 RVA: 0x00115365 File Offset: 0x00114365
		protected override Style CreateControlStyle()
		{
			return new TableItemStyle();
		}

		// Token: 0x06004315 RID: 17173 RVA: 0x0011536C File Offset: 0x0011436C
		protected override bool OnBubbleEvent(object source, EventArgs e)
		{
			if (e is CommandEventArgs)
			{
				DataListCommandEventArgs dataListCommandEventArgs = new DataListCommandEventArgs(this, source, (CommandEventArgs)e);
				base.RaiseBubbleEvent(this, dataListCommandEventArgs);
				return true;
			}
			return false;
		}

		// Token: 0x06004316 RID: 17174 RVA: 0x0011539C File Offset: 0x0011439C
		public virtual void RenderItem(HtmlTextWriter writer, bool extractRows, bool tableLayout)
		{
			HttpContext context = this.Context;
			if (context != null && context.TraceIsEnabled)
			{
				int bufferedLength = context.Response.GetBufferedLength();
				this.RenderItemInternal(writer, extractRows, tableLayout);
				int bufferedLength2 = context.Response.GetBufferedLength();
				context.Trace.AddControlSize(this.UniqueID, bufferedLength2 - bufferedLength);
				return;
			}
			this.RenderItemInternal(writer, extractRows, tableLayout);
		}

		// Token: 0x06004317 RID: 17175 RVA: 0x001153FC File Offset: 0x001143FC
		private void RenderItemInternal(HtmlTextWriter writer, bool extractRows, bool tableLayout)
		{
			if (!extractRows)
			{
				if (tableLayout)
				{
					this.RenderContents(writer);
					return;
				}
				this.RenderControl(writer);
				return;
			}
			else
			{
				IEnumerator enumerator = this.Controls.GetEnumerator();
				Table table = null;
				bool flag = false;
				while (enumerator.MoveNext())
				{
					flag = true;
					Control control = (Control)enumerator.Current;
					if (control is Table)
					{
						table = (Table)control;
						break;
					}
				}
				if (table != null)
				{
					foreach (object obj in table.Rows)
					{
						TableRow tableRow = (TableRow)obj;
						tableRow.RenderControl(writer);
					}
					return;
				}
				if (flag)
				{
					throw new HttpException(SR.GetString("DataList_TemplateTableNotFound", new object[]
					{
						this.Parent.ID,
						this.itemType.ToString()
					}));
				}
				return;
			}
		}

		// Token: 0x06004318 RID: 17176 RVA: 0x001154CA File Offset: 0x001144CA
		protected internal virtual void SetItemType(ListItemType itemType)
		{
			this.itemType = itemType;
		}

		// Token: 0x17001050 RID: 4176
		// (get) Token: 0x06004319 RID: 17177 RVA: 0x001154D3 File Offset: 0x001144D3
		object IDataItemContainer.DataItem
		{
			get
			{
				return this.DataItem;
			}
		}

		// Token: 0x17001051 RID: 4177
		// (get) Token: 0x0600431A RID: 17178 RVA: 0x001154DB File Offset: 0x001144DB
		int IDataItemContainer.DataItemIndex
		{
			get
			{
				return this.ItemIndex;
			}
		}

		// Token: 0x17001052 RID: 4178
		// (get) Token: 0x0600431B RID: 17179 RVA: 0x001154E3 File Offset: 0x001144E3
		int IDataItemContainer.DisplayIndex
		{
			get
			{
				return this.ItemIndex;
			}
		}

		// Token: 0x04002944 RID: 10564
		private int itemIndex;

		// Token: 0x04002945 RID: 10565
		private ListItemType itemType;

		// Token: 0x04002946 RID: 10566
		private object dataItem;
	}
}
