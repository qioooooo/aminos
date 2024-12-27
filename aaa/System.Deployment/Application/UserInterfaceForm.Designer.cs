namespace System.Deployment.Application
{
	// Token: 0x020000D2 RID: 210
	internal partial class UserInterfaceForm : global::System.Windows.Forms.Form
	{
		// Token: 0x06000598 RID: 1432 RVA: 0x0001DDCE File Offset: 0x0001CDCE
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				base.Icon.Dispose();
				base.Icon = null;
				if (this.currentPiece != null)
				{
					this.currentPiece.Dispose();
				}
			}
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001DE00 File Offset: 0x0001CE00
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::System.Deployment.Application.UserInterfaceForm));
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ControlBox = false;
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "UserInterfaceForm";
			base.ShowIcon = false;
			base.ResumeLayout(false);
		}

		// Token: 0x04000494 RID: 1172
		private global::System.Deployment.Application.FormPiece currentPiece;
	}
}
