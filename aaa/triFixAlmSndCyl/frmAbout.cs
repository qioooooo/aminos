using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace triFixAlmSndCyl
{
	// Token: 0x0200000D RID: 13
	public partial class frmAbout : Form
	{
		// Token: 0x06000035 RID: 53 RVA: 0x00004F4C File Offset: 0x0000334C
		public frmAbout()
		{
			base.Load += this.frmAbout_Load;
			base.Closed += this.frmAbout_Closed;
			this.InitializeComponent();
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00004FA0 File Offset: 0x000033A0
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00004FB4 File Offset: 0x000033B4
		internal virtual Label lblApp
		{
			get
			{
				return this._lblApp;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._lblApp = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00004FC0 File Offset: 0x000033C0
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00004FD4 File Offset: 0x000033D4
		internal virtual Label lblVersion
		{
			get
			{
				return this._lblVersion;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._lblVersion = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00004FE0 File Offset: 0x000033E0
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00004FF4 File Offset: 0x000033F4
		internal virtual Label lblDescription
		{
			get
			{
				return this._lblDescription;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._lblDescription = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00005000 File Offset: 0x00003400
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00005014 File Offset: 0x00003414
		internal virtual Label lblCopyright
		{
			get
			{
				return this._lblCopyright;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._lblCopyright = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00005020 File Offset: 0x00003420
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00005034 File Offset: 0x00003434
		internal virtual Label Label1
		{
			get
			{
				return this._Label1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label1 = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00005040 File Offset: 0x00003440
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00005054 File Offset: 0x00003454
		internal virtual Label Label2
		{
			get
			{
				return this._Label2;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label2 = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00005060 File Offset: 0x00003460
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00005074 File Offset: 0x00003474
		internal virtual Label Label3
		{
			get
			{
				return this._Label3;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label3 = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00005080 File Offset: 0x00003480
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00005094 File Offset: 0x00003494
		internal virtual Label lblUserName
		{
			get
			{
				return this._lblUserName;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._lblUserName = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000050A0 File Offset: 0x000034A0
		// (set) Token: 0x06000048 RID: 72 RVA: 0x000050B4 File Offset: 0x000034B4
		internal virtual Label Label4
		{
			get
			{
				return this._Label4;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label4 = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000049 RID: 73 RVA: 0x000050C0 File Offset: 0x000034C0
		// (set) Token: 0x0600004A RID: 74 RVA: 0x000050D4 File Offset: 0x000034D4
		internal virtual Label lblComputerName
		{
			get
			{
				return this._lblComputerName;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._lblComputerName = value;
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00005640 File Offset: 0x00003A40
		private void frmAbout_Load(object sender, EventArgs e)
		{
			this.Text = "關於 <iFix警報輪迴播音系統> 版本";
			this.lblApp.Text = "Product : " + Application.ProductName;
			this.lblVersion.Text = "版本 " + Application.ProductVersion;
			this.lblCopyright.Text = "Copyright (C) 2005 群泰科技版權所有";
			this.lblDescription.Text = "請尊重智慧財產權, 違者必究.\r\n\r\n<iFix警報輪迴播音系統>\r\n主要功能就是希望透過幾個簡單畫面設定便可完成\r省去攥寫一堆的警報播音程式";
			this.lblUserName.Text = SystemInformation.UserName;
			this.lblComputerName.Text = SystemInformation.ComputerName;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000056CC File Offset: 0x00003ACC
		private void frmAbout_Closed(object sender, EventArgs e)
		{
			modpublic.frmRun.Enabled = true;
			modpublic.frmRun.Visible = true;
		}

		// Token: 0x04000021 RID: 33
		[AccessedThroughProperty("lblApp")]
		private Label _lblApp;

		// Token: 0x04000022 RID: 34
		[AccessedThroughProperty("lblVersion")]
		private Label _lblVersion;

		// Token: 0x04000023 RID: 35
		[AccessedThroughProperty("lblDescription")]
		private Label _lblDescription;

		// Token: 0x04000024 RID: 36
		[AccessedThroughProperty("lblCopyright")]
		private Label _lblCopyright;

		// Token: 0x04000025 RID: 37
		[AccessedThroughProperty("Label1")]
		private Label _Label1;

		// Token: 0x04000026 RID: 38
		[AccessedThroughProperty("Label2")]
		private Label _Label2;

		// Token: 0x04000027 RID: 39
		[AccessedThroughProperty("Label3")]
		private Label _Label3;

		// Token: 0x04000028 RID: 40
		[AccessedThroughProperty("lblUserName")]
		private Label _lblUserName;

		// Token: 0x04000029 RID: 41
		[AccessedThroughProperty("Label4")]
		private Label _Label4;

		// Token: 0x0400002A RID: 42
		[AccessedThroughProperty("lblComputerName")]
		private Label _lblComputerName;
	}
}
