namespace Microsoft.VisualBasic.MyServices.Internal
{
	internal partial class ProgressDialog : global::System.Windows.Forms.Form
	{
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		[global::System.Diagnostics.DebuggerStepThrough]
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::Microsoft.VisualBasic.MyServices.Internal.ProgressDialog));
			this.LabelInfo = new global::System.Windows.Forms.Label();
			this.ProgressBarWork = new global::System.Windows.Forms.ProgressBar();
			this.ButtonCloseDialog = new global::System.Windows.Forms.Button();
			this.SuspendLayout();
			componentResourceManager.ApplyResources(this.LabelInfo, "LabelInfo", global::System.Globalization.CultureInfo.CurrentUICulture);
			global::System.Windows.Forms.Control labelInfo = this.LabelInfo;
			global::System.Drawing.Size size = new global::System.Drawing.Size(300, 0);
			labelInfo.MaximumSize = size;
			this.LabelInfo.Name = "LabelInfo";
			componentResourceManager.ApplyResources(this.ProgressBarWork, "ProgressBarWork", global::System.Globalization.CultureInfo.CurrentUICulture);
			this.ProgressBarWork.Name = "ProgressBarWork";
			componentResourceManager.ApplyResources(this.ButtonCloseDialog, "ButtonCloseDialog", global::System.Globalization.CultureInfo.CurrentUICulture);
			this.ButtonCloseDialog.Name = "ButtonCloseDialog";
			componentResourceManager.ApplyResources(this, "$this", global::System.Globalization.CultureInfo.CurrentUICulture);
			this.Controls.Add(this.ButtonCloseDialog);
			this.Controls.Add(this.ProgressBarWork);
			this.Controls.Add(this.LabelInfo);
			this.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProgressDialog";
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private global::System.ComponentModel.IContainer components;
	}
}
