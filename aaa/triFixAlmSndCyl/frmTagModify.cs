using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace triFixAlmSndCyl
{
	// Token: 0x02000013 RID: 19
	[DesignerGenerated]
	public partial class frmTagModify : Form
	{
		// Token: 0x06000208 RID: 520 RVA: 0x00014EC4 File Offset: 0x000132C4
		public frmTagModify()
		{
			base.Load += this.frmTagModify_Load;
			this.InitializeComponent();
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0001577C File Offset: 0x00013B7C
		// (set) Token: 0x0600020C RID: 524 RVA: 0x00015790 File Offset: 0x00013B90
		internal virtual TextBox txtNode
		{
			get
			{
				return this._txtNode;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._txtNode = value;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0001579C File Offset: 0x00013B9C
		// (set) Token: 0x0600020E RID: 526 RVA: 0x000157B0 File Offset: 0x00013BB0
		internal virtual SaveFileDialog dlgSave
		{
			get
			{
				return this._dlgSave;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._dlgSave = value;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600020F RID: 527 RVA: 0x000157BC File Offset: 0x00013BBC
		// (set) Token: 0x06000210 RID: 528 RVA: 0x000157D0 File Offset: 0x00013BD0
		internal virtual CheckBox ckNode
		{
			get
			{
				return this._ckNode;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ckNode = value;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000211 RID: 529 RVA: 0x000157DC File Offset: 0x00013BDC
		// (set) Token: 0x06000212 RID: 530 RVA: 0x000157F0 File Offset: 0x00013BF0
		internal virtual CheckBox ckWave
		{
			get
			{
				return this._ckWave;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ckWave = value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000213 RID: 531 RVA: 0x000157FC File Offset: 0x00013BFC
		// (set) Token: 0x06000214 RID: 532 RVA: 0x00015810 File Offset: 0x00013C10
		internal virtual TextBox txtWave
		{
			get
			{
				return this._txtWave;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._txtWave = value;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0001581C File Offset: 0x00013C1C
		// (set) Token: 0x06000216 RID: 534 RVA: 0x00015830 File Offset: 0x00013C30
		internal virtual CheckBox ckPro
		{
			get
			{
				return this._ckPro;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ckPro = value;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0001583C File Offset: 0x00013C3C
		// (set) Token: 0x06000218 RID: 536 RVA: 0x00015850 File Offset: 0x00013C50
		internal virtual NumericUpDown ndPriority
		{
			get
			{
				return this._ndPriority;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ndPriority = value;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0001585C File Offset: 0x00013C5C
		// (set) Token: 0x0600021A RID: 538 RVA: 0x00015870 File Offset: 0x00013C70
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

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600021B RID: 539 RVA: 0x000158BC File Offset: 0x00013CBC
		// (set) Token: 0x0600021C RID: 540 RVA: 0x000158D0 File Offset: 0x00013CD0
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

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0001591C File Offset: 0x00013D1C
		// (set) Token: 0x0600021E RID: 542 RVA: 0x00015930 File Offset: 0x00013D30
		internal virtual GroupBox GroupBox1
		{
			get
			{
				return this._GroupBox1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._GroupBox1 = value;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600021F RID: 543 RVA: 0x0001593C File Offset: 0x00013D3C
		// (set) Token: 0x06000220 RID: 544 RVA: 0x00015950 File Offset: 0x00013D50
		internal virtual TextBox txtTag
		{
			get
			{
				return this._txtTag;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._txtTag = value;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0001595C File Offset: 0x00013D5C
		// (set) Token: 0x06000222 RID: 546 RVA: 0x00015970 File Offset: 0x00013D70
		internal virtual CheckBox ckTag
		{
			get
			{
				return this._ckTag;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ckTag = value;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0001597C File Offset: 0x00013D7C
		// (set) Token: 0x06000224 RID: 548 RVA: 0x00015990 File Offset: 0x00013D90
		public string pNode
		{
			get
			{
				return this.sNode;
			}
			set
			{
				this.sNode = value;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0001599C File Offset: 0x00013D9C
		// (set) Token: 0x06000226 RID: 550 RVA: 0x000159B0 File Offset: 0x00013DB0
		public string pTag
		{
			get
			{
				return this.sTag;
			}
			set
			{
				this.sTag = value;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000227 RID: 551 RVA: 0x000159BC File Offset: 0x00013DBC
		// (set) Token: 0x06000228 RID: 552 RVA: 0x000159D0 File Offset: 0x00013DD0
		public string pWave
		{
			get
			{
				return this.sWave;
			}
			set
			{
				this.sWave = value;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000229 RID: 553 RVA: 0x000159DC File Offset: 0x00013DDC
		// (set) Token: 0x0600022A RID: 554 RVA: 0x000159F0 File Offset: 0x00013DF0
		public int pPriority
		{
			get
			{
				return this.iPriority;
			}
			set
			{
				this.iPriority = value;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (set) Token: 0x0600022B RID: 555 RVA: 0x000159FC File Offset: 0x00013DFC
		public Array pRowIndex
		{
			set
			{
				this.aRowIndex = (int[])value;
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00015A0C File Offset: 0x00013E0C
		private void frmTagModify_Load(object sender, EventArgs e)
		{
			try
			{
				this.Left = modpublic.frmConfig.Left;
				this.Top = modpublic.frmConfig.Top;
				this.txtNode.Text = this.sNode;
				this.txtTag.Text = this.sTag;
				this.txtWave.Text = this.sWave;
				this.ndPriority.Value = new decimal(this.iPriority);
				if (this.aRowIndex.Length > 1)
				{
					this.ckTag.Enabled = false;
					this.txtTag.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "修改Tag> frmTagModify_Load", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00015AE0 File Offset: 0x00013EE0
		private void cmdExit_Click(object sender, EventArgs e)
		{
			this.Hide();
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00015AE8 File Offset: 0x00013EE8
		private void cmdOK_Click(object sender, EventArgs e)
		{
			string text = "";
			checked
			{
				try
				{
					this.Enabled = false;
					this.Cursor = Cursors.WaitCursor;
					if (!((!this.ckNode.Checked && !this.ckWave.Checked && !this.ckPro.Checked) & !(this.ckTag.Enabled & this.ckTag.Enabled)))
					{
						if (this.ckNode.Checked && this.txtNode.Text.Length < 1)
						{
							throw new Exception("NodeName不可以空白");
						}
						if (this.ckTag.Checked && this.txtTag.Text.Length < 1)
						{
							throw new Exception("TagName不可以空白");
						}
						if (this.ckWave.Checked)
						{
							this.txtWave.Text = modSub.funCorrectPahtString(this.txtWave.Text);
							if (this.txtWave.Text.Length < 1)
							{
								throw new Exception("Wave檔名不可以空白");
							}
							FileInfo fileInfo = new FileInfo(this.txtWave.Text);
							if (Operators.CompareString(fileInfo.Extension.ToUpper(), ".WAV", false) != 0)
							{
								throw new Exception("Wave檔案不是聲音檔案");
							}
							if (!File.Exists(this.txtWave.Text))
							{
								throw new Exception("Wave檔名不存在");
							}
						}
						if (this.ckNode.Checked)
						{
							text = "Node = " + this.txtNode.Text + "\r";
						}
						if (this.ckTag.Enabled & this.ckTag.Checked)
						{
							text = text + "Tag = " + this.txtTag.Text + "\r";
						}
						if (this.ckWave.Checked)
						{
							text = text + "Wave檔名 = " + this.txtWave.Text + "\r";
						}
						if (this.ckPro.Checked)
						{
							text = text + "優先順序 = " + Conversions.ToString(this.ndPriority.Value);
						}
						text = "你確定要將這 <" + Conversions.ToString(this.aRowIndex.Length) + "> 筆資料修改為:\r" + text;
						int num = (int)MessageBox.Show(text, "修改tag", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
						if (num == 6)
						{
							if (this.aRowIndex.Length > 1)
							{
								int num2 = 0;
								int num3 = this.aRowIndex.Length - 1;
								for (int i = num2; i <= num3; i++)
								{
									this.dvgRow = modpublic.frmConfig.DataGrid1.Rows[this.aRowIndex[i]];
									string text2 = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Node = '", this.dvgRow.Cells[0].Value), "' AND Tag = '"), this.dvgRow.Cells[1].Value), "'"));
									DataRow[] array = modpublic.frmConfig._dsCFG.Tables[0].Select(text2);
									if (array.Length > 0)
									{
										if (this.ckNode.Checked)
										{
											array[0]["Node"] = this.txtNode.Text;
										}
										if (this.ckWave.Checked)
										{
											array[0]["Wave"] = this.txtWave.Text;
										}
										if (this.ckPro.Checked)
										{
											array[0]["Priority"] = this.ndPriority.Value;
										}
									}
								}
							}
							else
							{
								this.dvgRow = modpublic.frmConfig.DataGrid1.Rows[this.aRowIndex[0]];
								string text2 = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Node = '", this.dvgRow.Cells[0].Value), "' AND Tag = '"), this.dvgRow.Cells[1].Value), "'"));
								DataRow[] array = modpublic.frmConfig._dsCFG.Tables[0].Select(text2);
								if (array.Length > 0)
								{
									if (this.ckNode.Checked)
									{
										array[0]["Node"] = this.txtNode.Text;
									}
									if (this.ckTag.Enabled & this.ckTag.Checked)
									{
										array[0]["Tag"] = this.txtTag.Text;
									}
									if (this.ckWave.Checked)
									{
										array[0]["Wave"] = this.txtWave.Text;
									}
									if (this.ckPro.Checked)
									{
										array[0]["Priority"] = this.ndPriority.Value;
									}
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "修改Tag> frmTagModify_Load", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				finally
				{
					this.Enabled = true;
					this.Cursor = Cursors.Arrow;
				}
			}
		}

		// Token: 0x040000EF RID: 239
		[AccessedThroughProperty("txtNode")]
		private TextBox _txtNode;

		// Token: 0x040000F0 RID: 240
		[AccessedThroughProperty("dlgSave")]
		private SaveFileDialog _dlgSave;

		// Token: 0x040000F1 RID: 241
		[AccessedThroughProperty("ckNode")]
		private CheckBox _ckNode;

		// Token: 0x040000F2 RID: 242
		[AccessedThroughProperty("ckWave")]
		private CheckBox _ckWave;

		// Token: 0x040000F3 RID: 243
		[AccessedThroughProperty("txtWave")]
		private TextBox _txtWave;

		// Token: 0x040000F4 RID: 244
		[AccessedThroughProperty("ckPro")]
		private CheckBox _ckPro;

		// Token: 0x040000F5 RID: 245
		[AccessedThroughProperty("ndPriority")]
		private NumericUpDown _ndPriority;

		// Token: 0x040000F6 RID: 246
		[AccessedThroughProperty("cmdOK")]
		private Button _cmdOK;

		// Token: 0x040000F7 RID: 247
		[AccessedThroughProperty("cmdExit")]
		private Button _cmdExit;

		// Token: 0x040000F8 RID: 248
		[AccessedThroughProperty("GroupBox1")]
		private GroupBox _GroupBox1;

		// Token: 0x040000F9 RID: 249
		[AccessedThroughProperty("txtTag")]
		private TextBox _txtTag;

		// Token: 0x040000FA RID: 250
		[AccessedThroughProperty("ckTag")]
		private CheckBox _ckTag;

		// Token: 0x040000FB RID: 251
		private string sNode;

		// Token: 0x040000FC RID: 252
		private string sTag;

		// Token: 0x040000FD RID: 253
		private string sWave;

		// Token: 0x040000FE RID: 254
		private int iPriority;

		// Token: 0x040000FF RID: 255
		private int[] aRowIndex;

		// Token: 0x04000100 RID: 256
		private DataGridViewRow dvgRow;
	}
}
