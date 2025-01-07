using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	internal class GroupedContextMenuStrip : ContextMenuStrip
	{
		public bool Populated
		{
			set
			{
				this.populated = value;
			}
		}

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

		public virtual void RefreshItems()
		{
		}

		private StringCollection groupOrdering;

		private ContextMenuStripGroupCollection groups;

		private bool populated;
	}
}
