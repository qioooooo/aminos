using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200019A RID: 410
	internal class GroupedContextMenuStrip : ContextMenuStrip
	{
		// Token: 0x1700026F RID: 623
		// (set) Token: 0x06000F53 RID: 3923 RVA: 0x0004303E File Offset: 0x0004203E
		public bool Populated
		{
			set
			{
				this.populated = value;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000F55 RID: 3925 RVA: 0x0004304F File Offset: 0x0004204F
		public ContextMenuStripGroupCollection Groups
		{
			get
			{
				if (this.groups == null)
				{
					this.groups = new ContextMenuStripGroupCollection();
				}
				return this.groups;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000F56 RID: 3926 RVA: 0x0004306A File Offset: 0x0004206A
		public StringCollection GroupOrdering
		{
			get
			{
				if (this.groupOrdering == null)
				{
					this.groupOrdering = new StringCollection();
				}
				return this.groupOrdering;
			}
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00043088 File Offset: 0x00042088
		public void Populate()
		{
			this.Items.Clear();
			foreach (string text in this.GroupOrdering)
			{
				if (this.groups.ContainsKey(text))
				{
					List<ToolStripItem> items = this.groups[text].Items;
					if (this.Items.Count > 0 && items.Count > 0)
					{
						this.Items.Add(new ToolStripSeparator());
					}
					foreach (ToolStripItem toolStripItem in items)
					{
						this.Items.Add(toolStripItem);
					}
				}
			}
			this.populated = true;
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x0004317C File Offset: 0x0004217C
		protected override void OnOpening(CancelEventArgs e)
		{
			base.SuspendLayout();
			if (!this.populated)
			{
				this.Populate();
			}
			this.RefreshItems();
			base.ResumeLayout(true);
			base.PerformLayout();
			e.Cancel = this.Items.Count == 0;
			base.OnOpening(e);
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x000431CB File Offset: 0x000421CB
		public virtual void RefreshItems()
		{
		}

		// Token: 0x04000FE9 RID: 4073
		private StringCollection groupOrdering;

		// Token: 0x04000FEA RID: 4074
		private ContextMenuStripGroupCollection groups;

		// Token: 0x04000FEB RID: 4075
		private bool populated;
	}
}
