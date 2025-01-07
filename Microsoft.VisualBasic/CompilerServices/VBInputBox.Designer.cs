namespace Microsoft.VisualBasic.CompilerServices
{
	internal sealed partial class VBInputBox : global::System.Windows.Forms.Form
	{
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::Microsoft.VisualBasic.CompilerServices.VBInputBox));
			this.OKButton = new global::System.Windows.Forms.Button();
			this.MyCancelButton = new global::System.Windows.Forms.Button();
			this.TextBox = new global::System.Windows.Forms.TextBox();
			this.Label = new global::System.Windows.Forms.Label();
			this.SuspendLayout();
			componentResourceManager.ApplyResources(this.OKButton, "OKButton", global::System.Globalization.CultureInfo.CurrentUICulture);
			this.OKButton.Name = "OKButton";
			this.MyCancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.MyCancelButton, "MyCancelButton", global::System.Globalization.CultureInfo.CurrentUICulture);
			this.MyCancelButton.Name = "MyCancelButton";
			componentResourceManager.ApplyResources(this.TextBox, "TextBox", global::System.Globalization.CultureInfo.CurrentUICulture);
			this.TextBox.Name = "TextBox";
			componentResourceManager.ApplyResources(this.Label, "Label", global::System.Globalization.CultureInfo.CurrentUICulture);
			this.Label.Name = "Label";
			this.AcceptButton = this.OKButton;
			componentResourceManager.ApplyResources(this, "$this", global::System.Globalization.CultureInfo.CurrentUICulture);
			this.CancelButton = this.MyCancelButton;
			this.Controls.Add(this.TextBox);
			this.Controls.Add(this.Label);
			this.Controls.Add(this.OKButton);
			this.Controls.Add(this.MyCancelButton);
			this.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "VBInputBox";
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private global::System.ComponentModel.Container components;

		private global::System.Windows.Forms.TextBox TextBox;

		private global::System.Windows.Forms.Label Label;

		private global::System.Windows.Forms.Button OKButton;

		private global::System.Windows.Forms.Button MyCancelButton;
	}
}
