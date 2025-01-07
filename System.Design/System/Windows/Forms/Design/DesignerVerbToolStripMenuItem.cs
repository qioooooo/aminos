using System;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	internal class DesignerVerbToolStripMenuItem : ToolStripMenuItem
	{
		public DesignerVerbToolStripMenuItem(DesignerVerb verb)
		{
			this.verb = verb;
			this.Text = verb.Text;
			this.RefreshItem();
		}

		public void RefreshItem()
		{
			if (this.verb != null)
			{
				base.Visible = this.verb.Visible;
				this.Enabled = this.verb.Enabled;
				base.Checked = this.verb.Checked;
			}
		}

		protected override void OnClick(EventArgs e)
		{
			if (this.verb != null)
			{
				this.verb.Invoke();
			}
		}

		private DesignerVerb verb;
	}
}
