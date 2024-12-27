using System;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000221 RID: 545
	internal class DesignerVerbToolStripMenuItem : ToolStripMenuItem
	{
		// Token: 0x0600148A RID: 5258 RVA: 0x00068A6C File Offset: 0x00067A6C
		public DesignerVerbToolStripMenuItem(DesignerVerb verb)
		{
			this.verb = verb;
			this.Text = verb.Text;
			this.RefreshItem();
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x00068A8D File Offset: 0x00067A8D
		public void RefreshItem()
		{
			if (this.verb != null)
			{
				base.Visible = this.verb.Visible;
				this.Enabled = this.verb.Enabled;
				base.Checked = this.verb.Checked;
			}
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x00068ACA File Offset: 0x00067ACA
		protected override void OnClick(EventArgs e)
		{
			if (this.verb != null)
			{
				this.verb.Invoke();
			}
		}

		// Token: 0x0400122E RID: 4654
		private DesignerVerb verb;
	}
}
