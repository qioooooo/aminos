namespace triFixAlmSndCyl
{
	// Token: 0x0200000D RID: 13
	public partial class frmAbout : global::System.Windows.Forms.Form
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00004F80 File Offset: 0x00003380
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000050E0 File Offset: 0x000034E0
		[global::System.Diagnostics.DebuggerStepThrough]
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::triFixAlmSndCyl.frmAbout));
			this.lblApp = new global::System.Windows.Forms.Label();
			this.lblVersion = new global::System.Windows.Forms.Label();
			this.lblDescription = new global::System.Windows.Forms.Label();
			this.lblCopyright = new global::System.Windows.Forms.Label();
			this.Label1 = new global::System.Windows.Forms.Label();
			this.Label2 = new global::System.Windows.Forms.Label();
			this.Label3 = new global::System.Windows.Forms.Label();
			this.lblUserName = new global::System.Windows.Forms.Label();
			this.Label4 = new global::System.Windows.Forms.Label();
			this.lblComputerName = new global::System.Windows.Forms.Label();
			this.SuspendLayout();
			global::System.Windows.Forms.Control lblApp = this.lblApp;
			global::System.Drawing.Point point = new global::System.Drawing.Point(32, 20);
			lblApp.Location = point;
			this.lblApp.Name = "lblApp";
			global::System.Windows.Forms.Control lblApp2 = this.lblApp;
			global::System.Drawing.Size size = new global::System.Drawing.Size(182, 21);
			lblApp2.Size = size;
			this.lblApp.TabIndex = 1;
			this.lblApp.Text = "Application Title";
			global::System.Windows.Forms.Control lblVersion = this.lblVersion;
			point = new global::System.Drawing.Point(224, 20);
			lblVersion.Location = point;
			this.lblVersion.Name = "lblVersion";
			global::System.Windows.Forms.Control lblVersion2 = this.lblVersion;
			size = new global::System.Drawing.Size(154, 21);
			lblVersion2.Size = size;
			this.lblVersion.TabIndex = 2;
			this.lblVersion.Text = "Version";
			this.lblDescription.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			global::System.Windows.Forms.Control lblDescription = this.lblDescription;
			point = new global::System.Drawing.Point(32, 105);
			lblDescription.Location = point;
			this.lblDescription.Name = "lblDescription";
			global::System.Windows.Forms.Control lblDescription2 = this.lblDescription;
			size = new global::System.Drawing.Size(343, 73);
			lblDescription2.Size = size;
			this.lblDescription.TabIndex = 3;
			this.lblDescription.Text = "Description";
			global::System.Windows.Forms.Control lblCopyright = this.lblCopyright;
			point = new global::System.Drawing.Point(32, 46);
			lblCopyright.Location = point;
			this.lblCopyright.Name = "lblCopyright";
			global::System.Windows.Forms.Control lblCopyright2 = this.lblCopyright;
			size = new global::System.Drawing.Size(343, 21);
			lblCopyright2.Size = size;
			this.lblCopyright.TabIndex = 4;
			this.lblCopyright.Text = "Copyright";
			global::System.Windows.Forms.Control label = this.Label1;
			point = new global::System.Drawing.Point(32, 83);
			label.Location = point;
			this.Label1.Name = "Label1";
			global::System.Windows.Forms.Control label2 = this.Label1;
			size = new global::System.Drawing.Size(116, 21);
			label2.Size = size;
			this.Label1.TabIndex = 6;
			this.Label1.Text = "產品說明資訊:";
			this.Label2.BorderStyle = global::System.Windows.Forms.BorderStyle.Fixed3D;
			global::System.Windows.Forms.Control label3 = this.Label2;
			point = new global::System.Drawing.Point(-8, 185);
			label3.Location = point;
			this.Label2.Name = "Label2";
			global::System.Windows.Forms.Control label4 = this.Label2;
			size = new global::System.Drawing.Size(460, 3);
			label4.Size = size;
			this.Label2.TabIndex = 7;
			global::System.Windows.Forms.Control label5 = this.Label3;
			point = new global::System.Drawing.Point(-16, 200);
			label5.Location = point;
			this.Label3.Name = "Label3";
			global::System.Windows.Forms.Control label6 = this.Label3;
			size = new global::System.Drawing.Size(109, 21);
			label6.Size = size;
			this.Label3.TabIndex = 8;
			this.Label3.Text = "目前登入使用者:";
			this.Label3.TextAlign = global::System.Drawing.ContentAlignment.TopRight;
			global::System.Windows.Forms.Control lblUserName = this.lblUserName;
			point = new global::System.Drawing.Point(104, 200);
			lblUserName.Location = point;
			this.lblUserName.Name = "lblUserName";
			global::System.Windows.Forms.Control lblUserName2 = this.lblUserName;
			size = new global::System.Drawing.Size(91, 21);
			lblUserName2.Size = size;
			this.lblUserName.TabIndex = 9;
			global::System.Windows.Forms.Control label7 = this.Label4;
			point = new global::System.Drawing.Point(224, 200);
			label7.Location = point;
			this.Label4.Name = "Label4";
			global::System.Windows.Forms.Control label8 = this.Label4;
			size = new global::System.Drawing.Size(73, 21);
			label8.Size = size;
			this.Label4.TabIndex = 10;
			this.Label4.Text = "電腦名稱:";
			this.Label4.TextAlign = global::System.Drawing.ContentAlignment.TopRight;
			global::System.Windows.Forms.Control lblComputerName = this.lblComputerName;
			point = new global::System.Drawing.Point(304, 200);
			lblComputerName.Location = point;
			this.lblComputerName.Name = "lblComputerName";
			global::System.Windows.Forms.Control lblComputerName2 = this.lblComputerName;
			size = new global::System.Drawing.Size(91, 21);
			lblComputerName2.Size = size;
			this.lblComputerName.TabIndex = 11;
			size = new global::System.Drawing.Size(5, 15);
			this.AutoScaleBaseSize = size;
			size = new global::System.Drawing.Size(410, 229);
			this.ClientSize = size;
			this.Controls.Add(this.lblComputerName);
			this.Controls.Add(this.Label4);
			this.Controls.Add(this.lblUserName);
			this.Controls.Add(this.Label3);
			this.Controls.Add(this.Label2);
			this.Controls.Add(this.Label1);
			this.Controls.Add(this.lblCopyright);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.lblVersion);
			this.Controls.Add(this.lblApp);
			this.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			this.Name = "frmAbout";
			this.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "關於 xx 版本";
			this.ResumeLayout(false);
		}

		// Token: 0x04000020 RID: 32
		private global::System.ComponentModel.IContainer components;
	}
}
