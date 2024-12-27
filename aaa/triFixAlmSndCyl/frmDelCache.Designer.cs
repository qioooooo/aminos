namespace triFixAlmSndCyl
{
	// Token: 0x02000012 RID: 18
	[global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated]
	public partial class frmDelCache : global::System.Windows.Forms.Form
	{
		// Token: 0x060001FA RID: 506 RVA: 0x00014948 File Offset: 0x00012D48
		[global::System.Diagnostics.DebuggerNonUserCode]
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00014968 File Offset: 0x00012D68
		[global::System.Diagnostics.DebuggerStepThrough]
		private void InitializeComponent()
		{
			this.ListBox1 = new global::System.Windows.Forms.ListBox();
			this.Label1 = new global::System.Windows.Forms.Label();
			this.cmdExit = new global::System.Windows.Forms.Button();
			this.cmdOK = new global::System.Windows.Forms.Button();
			this.SuspendLayout();
			this.ListBox1.Font = new global::System.Drawing.Font("Arial", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.ListBox1.HorizontalScrollbar = true;
			this.ListBox1.ItemHeight = 15;
			global::System.Windows.Forms.Control listBox = this.ListBox1;
			global::System.Drawing.Point point = new global::System.Drawing.Point(8, 24);
			listBox.Location = point;
			this.ListBox1.Name = "ListBox1";
			this.ListBox1.SelectionMode = global::System.Windows.Forms.SelectionMode.MultiExtended;
			global::System.Windows.Forms.Control listBox2 = this.ListBox1;
			global::System.Drawing.Size size = new global::System.Drawing.Size(300, 199);
			listBox2.Size = size;
			this.ListBox1.TabIndex = 63;
			this.Label1.AutoSize = true;
			this.Label1.Font = new global::System.Drawing.Font("Arial", 8.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			global::System.Windows.Forms.Control label = this.Label1;
			point = new global::System.Drawing.Point(12, 8);
			label.Location = point;
			this.Label1.Name = "Label1";
			global::System.Windows.Forms.Control label2 = this.Label1;
			size = new global::System.Drawing.Size(86, 14);
			label2.Size = size;
			this.Label1.TabIndex = 66;
			this.Label1.Text = "Cache檔案清單";
			global::System.Windows.Forms.Control cmdExit = this.cmdExit;
			point = new global::System.Drawing.Point(244, 232);
			cmdExit.Location = point;
			this.cmdExit.Name = "cmdExit";
			global::System.Windows.Forms.Control cmdExit2 = this.cmdExit;
			size = new global::System.Drawing.Size(60, 24);
			cmdExit2.Size = size;
			this.cmdExit.TabIndex = 65;
			this.cmdExit.Text = "取消";
			global::System.Windows.Forms.Control cmdOK = this.cmdOK;
			point = new global::System.Drawing.Point(176, 232);
			cmdOK.Location = point;
			this.cmdOK.Name = "cmdOK";
			global::System.Windows.Forms.Control cmdOK2 = this.cmdOK;
			size = new global::System.Drawing.Size(60, 24);
			cmdOK2.Size = size;
			this.cmdOK.TabIndex = 64;
			this.cmdOK.Text = "刪除";
			global::System.Drawing.SizeF sizeF = new global::System.Drawing.SizeF(6f, 12f);
			this.AutoScaleDimensions = sizeF;
			this.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			size = new global::System.Drawing.Size(316, 260);
			this.ClientSize = size;
			this.ControlBox = false;
			this.Controls.Add(this.ListBox1);
			this.Controls.Add(this.Label1);
			this.Controls.Add(this.cmdExit);
			this.Controls.Add(this.cmdOK);
			this.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmDelCache";
			this.ShowInTaskbar = false;
			this.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "刪除Cache檔案";
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		// Token: 0x040000E8 RID: 232
		private global::System.ComponentModel.IContainer components;
	}
}
