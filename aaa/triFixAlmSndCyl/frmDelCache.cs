using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace triFixAlmSndCyl
{
	// Token: 0x02000012 RID: 18
	[DesignerGenerated]
	public partial class frmDelCache : Form
	{
		// Token: 0x060001F9 RID: 505 RVA: 0x00014914 File Offset: 0x00012D14
		public frmDelCache()
		{
			base.Load += this.frmDelCache_Load;
			base.Activated += this.frmDelCache_Activated;
			this.InitializeComponent();
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00014C3C File Offset: 0x0001303C
		// (set) Token: 0x060001FD RID: 509 RVA: 0x00014C50 File Offset: 0x00013050
		internal virtual ListBox ListBox1
		{
			get
			{
				return this._ListBox1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ListBox1 = value;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00014C5C File Offset: 0x0001305C
		// (set) Token: 0x060001FF RID: 511 RVA: 0x00014C70 File Offset: 0x00013070
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

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00014C7C File Offset: 0x0001307C
		// (set) Token: 0x06000201 RID: 513 RVA: 0x00014C90 File Offset: 0x00013090
		internal virtual Button cmdExit
		{
			get
			{
				return this._cmdExit;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.cmdExit_Click);
				if (this._cmdExit != null)
				{
					this._cmdExit.Click -= eventHandler;
				}
				this._cmdExit = value;
				if (this._cmdExit != null)
				{
					this._cmdExit.Click += eventHandler;
				}
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00014CDC File Offset: 0x000130DC
		// (set) Token: 0x06000203 RID: 515 RVA: 0x00014CF0 File Offset: 0x000130F0
		internal virtual Button cmdOK
		{
			get
			{
				return this._cmdOK;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.cmdOK_Click);
				if (this._cmdOK != null)
				{
					this._cmdOK.Click -= eventHandler;
				}
				this._cmdOK = value;
				if (this._cmdOK != null)
				{
					this._cmdOK.Click += eventHandler;
				}
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00014D3C File Offset: 0x0001313C
		private void frmDelCache_Load(object sender, EventArgs e)
		{
			try
			{
				string[] files = Directory.GetFiles(Application.StartupPath + "\\", "cache_txiFixAlmSndCly_*.xml");
				foreach (string text in files)
				{
					this.ListBox1.Items.Add(modSub.funGetValueInString(text, "\\cache_txiFixAlmSndCly_", ".xml"));
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00014DC0 File Offset: 0x000131C0
		private void frmDelCache_Activated(object sender, EventArgs e)
		{
			if (!this.bNoMoreCache && this.ListBox1.Items.Count < 1)
			{
				this.bNoMoreCache = true;
				string text = "沒有任合cache資料";
				MessageBox.Show(text, "刪除Cache檔案", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				this.Close();
				this.Hide();
			}
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00014E10 File Offset: 0x00013210
		private void cmdExit_Click(object sender, EventArgs e)
		{
			this.Hide();
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00014E18 File Offset: 0x00013218
		private void cmdOK_Click(object sender, EventArgs e)
		{
			checked
			{
				try
				{
					int num = 0;
					int num2 = this.ListBox1.SelectedItems.Count - 1;
					for (int i = num; i <= num2; i++)
					{
						string text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Application.StartupPath + "\\" + "cache_txiFixAlmSndCly_", this.ListBox1.SelectedItems[i]), ".xml"));
						File.Delete(text);
					}
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
				finally
				{
					this.Hide();
				}
			}
		}

		// Token: 0x040000E9 RID: 233
		[AccessedThroughProperty("ListBox1")]
		private ListBox _ListBox1;

		// Token: 0x040000EA RID: 234
		[AccessedThroughProperty("Label1")]
		private Label _Label1;

		// Token: 0x040000EB RID: 235
		[AccessedThroughProperty("cmdExit")]
		private Button _cmdExit;

		// Token: 0x040000EC RID: 236
		[AccessedThroughProperty("cmdOK")]
		private Button _cmdOK;

		// Token: 0x040000ED RID: 237
		private bool bNoMoreCache;
	}
}
