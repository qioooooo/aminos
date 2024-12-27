using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GeFanuc.iFixToolkit.Adapter;
using kvNetClass;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Trendtek.iFix;

namespace triFixAlmSndCyl
{
	// Token: 0x0200000F RID: 15
	[DesignerGenerated]
	public partial class frmAlmSndRun : Form
	{
		// Token: 0x06000169 RID: 361 RVA: 0x0000F8C8 File Offset: 0x0000DCC8
		public frmAlmSndRun()
		{
			base.Load += this.frmAlmSndRun_Load;
			base.Shown += this.frmAlmSndRun_Shown;
			base.SizeChanged += this.frmAlmSndRun_SizeChanged;
			base.KeyDown += this.frmAlmSndRun_KeyDown;
			base.FormClosing += this.frmAlmSndRun_FormClosing;
			this.messagesLock = RuntimeHelpers.GetObjectValue(new object());
			this.InitializeComponent();
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600016C RID: 364 RVA: 0x000113CC File Offset: 0x0000F7CC
		// (set) Token: 0x0600016D RID: 365 RVA: 0x000113E0 File Offset: 0x0000F7E0
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

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600016E RID: 366 RVA: 0x000113EC File Offset: 0x0000F7EC
		// (set) Token: 0x0600016F RID: 367 RVA: 0x00011400 File Offset: 0x0000F800
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

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000170 RID: 368 RVA: 0x0001140C File Offset: 0x0000F80C
		// (set) Token: 0x06000171 RID: 369 RVA: 0x00011420 File Offset: 0x0000F820
		internal virtual TextBox txtSoundTime
		{
			get
			{
				return this._txtSoundTime;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._txtSoundTime = value;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000172 RID: 370 RVA: 0x0001142C File Offset: 0x0000F82C
		// (set) Token: 0x06000173 RID: 371 RVA: 0x00011440 File Offset: 0x0000F840
		internal virtual TextBox txtScanTime
		{
			get
			{
				return this._txtScanTime;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._txtScanTime = value;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000174 RID: 372 RVA: 0x0001144C File Offset: 0x0000F84C
		// (set) Token: 0x06000175 RID: 373 RVA: 0x00011460 File Offset: 0x0000F860
		internal virtual Label Label9
		{
			get
			{
				return this._Label9;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label9 = value;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000176 RID: 374 RVA: 0x0001146C File Offset: 0x0000F86C
		// (set) Token: 0x06000177 RID: 375 RVA: 0x00011480 File Offset: 0x0000F880
		internal virtual Label Label10
		{
			get
			{
				return this._Label10;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label10 = value;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000178 RID: 376 RVA: 0x0001148C File Offset: 0x0000F88C
		// (set) Token: 0x06000179 RID: 377 RVA: 0x000114A0 File Offset: 0x0000F8A0
		internal virtual ListBox lstQueue
		{
			get
			{
				return this._lstQueue;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._lstQueue = value;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600017A RID: 378 RVA: 0x000114AC File Offset: 0x0000F8AC
		// (set) Token: 0x0600017B RID: 379 RVA: 0x000114C0 File Offset: 0x0000F8C0
		internal virtual ListBox Monitor
		{
			get
			{
				return this._Monitor;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Monitor = value;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600017C RID: 380 RVA: 0x000114CC File Offset: 0x0000F8CC
		// (set) Token: 0x0600017D RID: 381 RVA: 0x000114E0 File Offset: 0x0000F8E0
		internal virtual CheckBox ckMute
		{
			get
			{
				return this._ckMute;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.ckMute_CheckedChanged);
				if (this._ckMute != null)
				{
					this._ckMute.CheckedChanged -= eventHandler;
				}
				this._ckMute = value;
				if (this._ckMute != null)
				{
					this._ckMute.CheckedChanged += eventHandler;
				}
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600017E RID: 382 RVA: 0x0001152C File Offset: 0x0000F92C
		// (set) Token: 0x0600017F RID: 383 RVA: 0x00011540 File Offset: 0x0000F940
		internal virtual TextBox txtOverRun
		{
			get
			{
				return this._txtOverRun;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._txtOverRun = value;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0001154C File Offset: 0x0000F94C
		// (set) Token: 0x06000181 RID: 385 RVA: 0x00011560 File Offset: 0x0000F960
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

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000182 RID: 386 RVA: 0x0001156C File Offset: 0x0000F96C
		// (set) Token: 0x06000183 RID: 387 RVA: 0x00011580 File Offset: 0x0000F980
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

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000184 RID: 388 RVA: 0x0001158C File Offset: 0x0000F98C
		// (set) Token: 0x06000185 RID: 389 RVA: 0x000115A0 File Offset: 0x0000F9A0
		internal virtual GroupBox GroupBox2
		{
			get
			{
				return this._GroupBox2;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._GroupBox2 = value;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000186 RID: 390 RVA: 0x000115AC File Offset: 0x0000F9AC
		// (set) Token: 0x06000187 RID: 391 RVA: 0x000115C0 File Offset: 0x0000F9C0
		internal virtual GroupBox GroupBox3
		{
			get
			{
				return this._GroupBox3;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._GroupBox3 = value;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000188 RID: 392 RVA: 0x000115CC File Offset: 0x0000F9CC
		// (set) Token: 0x06000189 RID: 393 RVA: 0x000115E0 File Offset: 0x0000F9E0
		internal virtual TreeView TreeView1
		{
			get
			{
				return this._TreeView1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TreeView1 = value;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600018A RID: 394 RVA: 0x000115EC File Offset: 0x0000F9EC
		// (set) Token: 0x0600018B RID: 395 RVA: 0x00011600 File Offset: 0x0000FA00
		internal virtual GroupBox GroupBox4
		{
			get
			{
				return this._GroupBox4;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._GroupBox4 = value;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600018C RID: 396 RVA: 0x0001160C File Offset: 0x0000FA0C
		// (set) Token: 0x0600018D RID: 397 RVA: 0x00011620 File Offset: 0x0000FA20
		internal virtual DataSet _dsEDARunTime
		{
			get
			{
				return this.__dsEDARunTime;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this.__dsEDARunTime = value;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600018E RID: 398 RVA: 0x0001162C File Offset: 0x0000FA2C
		// (set) Token: 0x0600018F RID: 399 RVA: 0x00011640 File Offset: 0x0000FA40
		internal virtual DataTable DataTable1
		{
			get
			{
				return this._DataTable1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._DataTable1 = value;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000190 RID: 400 RVA: 0x0001164C File Offset: 0x0000FA4C
		// (set) Token: 0x06000191 RID: 401 RVA: 0x00011660 File Offset: 0x0000FA60
		internal virtual DataColumn DataColumn1
		{
			get
			{
				return this._DataColumn1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._DataColumn1 = value;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000192 RID: 402 RVA: 0x0001166C File Offset: 0x0000FA6C
		// (set) Token: 0x06000193 RID: 403 RVA: 0x00011680 File Offset: 0x0000FA80
		internal virtual DataColumn DataColumn2
		{
			get
			{
				return this._DataColumn2;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._DataColumn2 = value;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000194 RID: 404 RVA: 0x0001168C File Offset: 0x0000FA8C
		// (set) Token: 0x06000195 RID: 405 RVA: 0x000116A0 File Offset: 0x0000FAA0
		internal virtual DataColumn DataColumn3
		{
			get
			{
				return this._DataColumn3;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._DataColumn3 = value;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000196 RID: 406 RVA: 0x000116AC File Offset: 0x0000FAAC
		// (set) Token: 0x06000197 RID: 407 RVA: 0x000116C0 File Offset: 0x0000FAC0
		internal virtual global::System.Windows.Forms.Timer tmrRun
		{
			get
			{
				return this._tmrRun;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.tmrRun_Tick);
				if (this._tmrRun != null)
				{
					this._tmrRun.Tick -= eventHandler;
				}
				this._tmrRun = value;
				if (this._tmrRun != null)
				{
					this._tmrRun.Tick += eventHandler;
				}
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0001170C File Offset: 0x0000FB0C
		// (set) Token: 0x06000199 RID: 409 RVA: 0x00011720 File Offset: 0x0000FB20
		internal virtual global::System.Windows.Forms.Timer tmrPlaySound
		{
			get
			{
				return this._tmrPlaySound;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.tmrPlaySound_Tick);
				if (this._tmrPlaySound != null)
				{
					this._tmrPlaySound.Tick -= eventHandler;
				}
				this._tmrPlaySound = value;
				if (this._tmrPlaySound != null)
				{
					this._tmrPlaySound.Tick += eventHandler;
				}
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0001176C File Offset: 0x0000FB6C
		// (set) Token: 0x0600019B RID: 411 RVA: 0x00011780 File Offset: 0x0000FB80
		internal virtual ToolTip ToolTip1
		{
			get
			{
				return this._ToolTip1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ToolTip1 = value;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0001178C File Offset: 0x0000FB8C
		// (set) Token: 0x0600019D RID: 413 RVA: 0x000117A0 File Offset: 0x0000FBA0
		internal virtual CheckBox ckQueue
		{
			get
			{
				return this._ckQueue;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.ckQueue_CheckedChanged);
				if (this._ckQueue != null)
				{
					this._ckQueue.CheckedChanged -= eventHandler;
				}
				this._ckQueue = value;
				if (this._ckQueue != null)
				{
					this._ckQueue.CheckedChanged += eventHandler;
				}
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600019E RID: 414 RVA: 0x000117EC File Offset: 0x0000FBEC
		// (set) Token: 0x0600019F RID: 415 RVA: 0x00011800 File Offset: 0x0000FC00
		internal virtual MenuStrip MenuStrip1
		{
			get
			{
				return this._MenuStrip1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._MenuStrip1 = value;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0001180C File Offset: 0x0000FC0C
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x00011820 File Offset: 0x0000FC20
		internal virtual ToolStripMenuItem miConfig
		{
			get
			{
				return this._miConfig;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.miConfig_Click);
				if (this._miConfig != null)
				{
					this._miConfig.Click -= eventHandler;
				}
				this._miConfig = value;
				if (this._miConfig != null)
				{
					this._miConfig.Click += eventHandler;
				}
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x0001186C File Offset: 0x0000FC6C
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x00011880 File Offset: 0x0000FC80
		internal virtual ToolStripMenuItem 說明ToolStripMenuItem
		{
			get
			{
				return this._說明ToolStripMenuItem;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._說明ToolStripMenuItem = value;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0001188C File Offset: 0x0000FC8C
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x000118A0 File Offset: 0x0000FCA0
		internal virtual ContextMenuStrip ContextMenuStrip_Event
		{
			get
			{
				return this._ContextMenuStrip_Event;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ContextMenuStrip_Event = value;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x000118AC File Offset: 0x0000FCAC
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x000118C0 File Offset: 0x0000FCC0
		internal virtual ToolStripMenuItem tsmiClearEvent
		{
			get
			{
				return this._tsmiClearEvent;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.tsmiClearEvent_Click);
				if (this._tsmiClearEvent != null)
				{
					this._tsmiClearEvent.Click -= eventHandler;
				}
				this._tsmiClearEvent = value;
				if (this._tsmiClearEvent != null)
				{
					this._tsmiClearEvent.Click += eventHandler;
				}
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x0001190C File Offset: 0x0000FD0C
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00011920 File Offset: 0x0000FD20
		internal virtual ToolStripMenuItem tsmiShowEvent
		{
			get
			{
				return this._tsmiShowEvent;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.tsmiShowEvent_Click);
				if (this._tsmiShowEvent != null)
				{
					this._tsmiShowEvent.Click -= eventHandler;
				}
				this._tsmiShowEvent = value;
				if (this._tsmiShowEvent != null)
				{
					this._tsmiShowEvent.Click += eventHandler;
				}
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060001AA RID: 426 RVA: 0x0001196C File Offset: 0x0000FD6C
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00011980 File Offset: 0x0000FD80
		internal virtual ToolStripMenuItem tsmiShowError
		{
			get
			{
				return this._tsmiShowError;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.tsmiShowError_Click);
				if (this._tsmiShowError != null)
				{
					this._tsmiShowError.Click -= eventHandler;
				}
				this._tsmiShowError = value;
				if (this._tsmiShowError != null)
				{
					this._tsmiShowError.Click += eventHandler;
				}
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060001AC RID: 428 RVA: 0x000119CC File Offset: 0x0000FDCC
		// (set) Token: 0x060001AD RID: 429 RVA: 0x000119E0 File Offset: 0x0000FDE0
		internal virtual ToolStripSeparator ToolStripMenuItem1
		{
			get
			{
				return this._ToolStripMenuItem1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ToolStripMenuItem1 = value;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001AE RID: 430 RVA: 0x000119EC File Offset: 0x0000FDEC
		// (set) Token: 0x060001AF RID: 431 RVA: 0x00011A00 File Offset: 0x0000FE00
		internal virtual ContextMenuStrip ContextMenuStrip_Tree
		{
			get
			{
				return this._ContextMenuStrip_Tree;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ContextMenuStrip_Tree = value;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00011A0C File Offset: 0x0000FE0C
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x00011A20 File Offset: 0x0000FE20
		internal virtual ToolStripMenuItem cmsCollapseAll
		{
			get
			{
				return this._cmsCollapseAll;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.cmsCollapseAll_Click);
				if (this._cmsCollapseAll != null)
				{
					this._cmsCollapseAll.Click -= eventHandler;
				}
				this._cmsCollapseAll = value;
				if (this._cmsCollapseAll != null)
				{
					this._cmsCollapseAll.Click += eventHandler;
				}
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00011A6C File Offset: 0x0000FE6C
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x00011A80 File Offset: 0x0000FE80
		internal virtual ToolStripMenuItem cmdCollapseType
		{
			get
			{
				return this._cmdCollapseType;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.cmsCollapseAll_Click);
				if (this._cmdCollapseType != null)
				{
					this._cmdCollapseType.Click -= eventHandler;
				}
				this._cmdCollapseType = value;
				if (this._cmdCollapseType != null)
				{
					this._cmdCollapseType.Click += eventHandler;
				}
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00011ACC File Offset: 0x0000FECC
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x00011AE0 File Offset: 0x0000FEE0
		internal virtual ToolStripSeparator ToolStripMenuItem2
		{
			get
			{
				return this._ToolStripMenuItem2;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ToolStripMenuItem2 = value;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00011AEC File Offset: 0x0000FEEC
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x00011B00 File Offset: 0x0000FF00
		internal virtual ToolStripMenuItem cmsExpandAll
		{
			get
			{
				return this._cmsExpandAll;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.cmsExpandAll_Click);
				if (this._cmsExpandAll != null)
				{
					this._cmsExpandAll.Click -= eventHandler;
				}
				this._cmsExpandAll = value;
				if (this._cmsExpandAll != null)
				{
					this._cmsExpandAll.Click += eventHandler;
				}
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00011B4C File Offset: 0x0000FF4C
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x00011B60 File Offset: 0x0000FF60
		internal virtual ToolStripMenuItem cmsExpandNode
		{
			get
			{
				return this._cmsExpandNode;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.cmsExpandAll_Click);
				if (this._cmsExpandNode != null)
				{
					this._cmsExpandNode.Click -= eventHandler;
				}
				this._cmsExpandNode = value;
				if (this._cmsExpandNode != null)
				{
					this._cmsExpandNode.Click += eventHandler;
				}
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00011BAC File Offset: 0x0000FFAC
		// (set) Token: 0x060001BB RID: 443 RVA: 0x00011BC0 File Offset: 0x0000FFC0
		internal virtual ToolStripMenuItem miAbout
		{
			get
			{
				return this._miAbout;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.miAbout_Click);
				if (this._miAbout != null)
				{
					this._miAbout.Click -= eventHandler;
				}
				this._miAbout = value;
				if (this._miAbout != null)
				{
					this._miAbout.Click += eventHandler;
				}
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00011C0C File Offset: 0x0001000C
		// (set) Token: 0x060001BD RID: 445 RVA: 0x00011C20 File Offset: 0x00010020
		internal virtual global::System.Windows.Forms.Timer tmrSendMessage
		{
			get
			{
				return this._tmrSendMessage;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.tmrSendMessage_Tick);
				if (this._tmrSendMessage != null)
				{
					this._tmrSendMessage.Tick -= eventHandler;
				}
				this._tmrSendMessage = value;
				if (this._tmrSendMessage != null)
				{
					this._tmrSendMessage.Tick += eventHandler;
				}
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00011C6C File Offset: 0x0001006C
		// (set) Token: 0x060001BF RID: 447 RVA: 0x00011C80 File Offset: 0x00010080
		internal virtual SplitContainer SplitContainer1
		{
			get
			{
				return this._SplitContainer1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._SplitContainer1 = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00011C8C File Offset: 0x0001008C
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x00011CA0 File Offset: 0x000100A0
		internal virtual SplitContainer SplitContainer2
		{
			get
			{
				return this._SplitContainer2;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._SplitContainer2 = value;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00011CAC File Offset: 0x000100AC
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x00011CC0 File Offset: 0x000100C0
		internal virtual DataColumn DataColumn4
		{
			get
			{
				return this._DataColumn4;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._DataColumn4 = value;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00011CCC File Offset: 0x000100CC
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x00011CE0 File Offset: 0x000100E0
		internal virtual DataColumn DataColumn5
		{
			get
			{
				return this._DataColumn5;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._DataColumn5 = value;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00011CEC File Offset: 0x000100EC
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x00011D00 File Offset: 0x00010100
		internal virtual DataColumn DataColumn6
		{
			get
			{
				return this._DataColumn6;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._DataColumn6 = value;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x00011D0C File Offset: 0x0001010C
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x00011D20 File Offset: 0x00010120
		internal virtual DataColumn DataColumn7
		{
			get
			{
				return this._DataColumn7;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._DataColumn7 = value;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060001CA RID: 458 RVA: 0x00011D2C File Offset: 0x0001012C
		// (set) Token: 0x060001CB RID: 459 RVA: 0x00011D40 File Offset: 0x00010140
		internal virtual NotifyIcon NotifyIcon1
		{
			get
			{
				return this._NotifyIcon1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				MouseEventHandler mouseEventHandler = new MouseEventHandler(this.NotifyIcon1_MouseDoubleClick);
				if (this._NotifyIcon1 != null)
				{
					this._NotifyIcon1.MouseDoubleClick -= mouseEventHandler;
				}
				this._NotifyIcon1 = value;
				if (this._NotifyIcon1 != null)
				{
					this._NotifyIcon1.MouseDoubleClick += mouseEventHandler;
				}
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00011D8C File Offset: 0x0001018C
		// (set) Token: 0x060001CD RID: 461 RVA: 0x00011DA0 File Offset: 0x000101A0
		internal virtual DataColumn DataColumn8
		{
			get
			{
				return this._DataColumn8;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._DataColumn8 = value;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00011DAC File Offset: 0x000101AC
		// (set) Token: 0x060001CF RID: 463 RVA: 0x00011DC0 File Offset: 0x000101C0
		internal virtual DataColumn DataColumn9
		{
			get
			{
				return this._DataColumn9;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._DataColumn9 = value;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00011DCC File Offset: 0x000101CC
		// (set) Token: 0x060001D1 RID: 465 RVA: 0x00011DE0 File Offset: 0x000101E0
		internal virtual DataColumn DataColumn10
		{
			get
			{
				return this._DataColumn10;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._DataColumn10 = value;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00011DEC File Offset: 0x000101EC
		// (set) Token: 0x060001D3 RID: 467 RVA: 0x00011E00 File Offset: 0x00010200
		internal virtual DataColumn DataColumn11
		{
			get
			{
				return this._DataColumn11;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._DataColumn11 = value;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x00011E0C File Offset: 0x0001020C
		// (set) Token: 0x060001D5 RID: 469 RVA: 0x00011E20 File Offset: 0x00010220
		internal virtual DataColumn DataColumn12
		{
			get
			{
				return this._DataColumn12;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._DataColumn12 = value;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x00011E2C File Offset: 0x0001022C
		// (set) Token: 0x060001D7 RID: 471 RVA: 0x00011E40 File Offset: 0x00010240
		internal virtual DataColumn DataColumn13
		{
			get
			{
				return this._DataColumn13;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._DataColumn13 = value;
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00011E4C File Offset: 0x0001024C
		internal void Callback_Result(string sMsg)
		{
			if (this.Monitor.Items.Count > 100)
			{
				this.Monitor.Items.RemoveAt(100);
			}
			if (this.Monitor.InvokeRequired)
			{
				frmAlmSndRun.SetMonitorCallback setMonitorCallback = new frmAlmSndRun.SetMonitorCallback(this.Callback_Result);
				this.Invoke(setMonitorCallback, new object[] { sMsg });
			}
			else if (this.Monitor.Items.Count > 0)
			{
				this.Monitor.Items.Insert(0, sMsg);
			}
			else
			{
				this.Monitor.Items.Add(sMsg);
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00011EEC File Offset: 0x000102EC
		internal void subDbIniFileRead()
		{
			string text = Application.StartupPath + "\\" + Application.ProductName + ".ini";
			clsCommon clsCommon = new clsCommon();
			try
			{
				if (File.Exists(text))
				{
					FileStream fileStream = File.Open(text, FileMode.Open, FileAccess.Read);
					StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);
					string text2 = streamReader.ReadToEnd();
					streamReader.Close();
					fileStream.Close();
					text2 = text2.ToUpper();
					string text3 = clsCommon.funGetValueInString(text2, "SOUNDTIME=", ";");
					if (Strings.Len(text3) > 0 && Versioned.IsNumeric(text3))
					{
						modpublic.g_iSoundTime = Conversions.ToInteger(text3);
						if (modpublic.g_iSoundTime < 1)
						{
							modpublic.g_iSoundTime = 1;
						}
						else if (modpublic.g_iSoundTime > 60)
						{
							modpublic.g_iSoundTime = 60;
						}
					}
					text3 = clsCommon.funGetValueInString(text2, "SCANTIME=", ";");
					if (Strings.Len(text3) > 0 && Versioned.IsNumeric(text3))
					{
						modpublic.g_iScanTime = Conversions.ToInteger(text3);
						if (modpublic.g_iScanTime < 300)
						{
							modpublic.g_iScanTime = 300;
						}
						else if (modpublic.g_iScanTime > 5000)
						{
							modpublic.g_iScanTime = 5000;
						}
					}
					text3 = clsCommon.funGetValueInString(text2, "BACKGROUND=", ";");
					if ((Strings.Len(text3) > 0) & (Operators.CompareString(text3.ToUpper(), "TRUE", false) == 0))
					{
						modpublic.g_bBackground = true;
					}
					else
					{
						modpublic.g_bBackground = false;
					}
					text3 = clsCommon.funGetValueInString(text2, "MENUBAR=", ";");
					if ((Strings.Len(text3) > 0) & (Operators.CompareString(text3.ToUpper(), "TRUE", false) == 0))
					{
						modpublic.g_bMenuBar = true;
					}
					else
					{
						modpublic.g_bMenuBar = false;
					}
					if (!Information.IsNothing(streamReader))
					{
						streamReader.Close();
					}
					if (!Information.IsNothing(fileStream))
					{
						fileStream.Close();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "讀取 INI ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			finally
			{
				StreamReader streamReader;
				if (!Information.IsNothing(streamReader))
				{
					streamReader.Close();
				}
				FileStream fileStream;
				if (!Information.IsNothing(fileStream))
				{
					fileStream.Close();
				}
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00012120 File Offset: 0x00010520
		internal void ReloadDBOnlyForConfig(string myCfgfile)
		{
			try
			{
				this.tmrRun.Stop();
				this.tmrPlaySound.Stop();
				modpublic.g_StopSound = true;
				this.subKillQueue();
				this.subDBLoad(myCfgfile, true);
				this.FillTreeView(this._dsEDARunTime.Tables[0], this.TreeView1);
				this.subFixEDAGroupHandleCreate(this._dsEDARunTime.Tables[0]);
				this.txtOverRun.Text = Conversions.ToString(0);
				modpublic.g_StopSound = false;
				this.tmrRun.Start();
				this.tmrPlaySound_Tick(this.tmrPlaySound, null);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000121E0 File Offset: 0x000105E0
		private void subDBLoad(string myCfgfile, bool bShowErr = true)
		{
			if (File.Exists(myCfgfile))
			{
				try
				{
					FileStream fileStream = File.Open(myCfgfile, FileMode.Open, FileAccess.Read);
					StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);
					modSub.subStringToDs(streamReader, this._dsEDARunTime.Tables[0]);
					streamReader.Close();
					fileStream.Close();
					this._dsEDARunTime.Tables[0].DefaultView.Sort = "Node, BlockType, Tag";
					return;
				}
				catch (Exception ex)
				{
					string text = "檔案損毀, 無法開啟\r";
					MessageBox.Show(text + ex.Message, "讀取檔案", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				finally
				{
					StreamReader streamReader;
					if (!Information.IsNothing(streamReader))
					{
						streamReader.Close();
					}
					FileStream fileStream;
					if (!Information.IsNothing(fileStream))
					{
						fileStream.Close();
					}
				}
			}
			if (bShowErr)
			{
				MessageBox.Show("PDB組態檔案 <" + myCfgfile + "> 不存在!", "讀取檔案", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x000122DC File Offset: 0x000106DC
		private void FillTreeView(DataTable dt, TreeView trv)
		{
			string text = "";
			string text2 = "";
			try
			{
				trv.BeginUpdate();
				trv.Nodes.Clear();
				if (dt.Rows.Count >= 1)
				{
					try
					{
						foreach (object obj in dt.DefaultView)
						{
							DataRowView dataRowView = (DataRowView)obj;
							object obj2 = text;
							object obj3 = null;
							Type typeFromHandle = typeof(Strings);
							string text3 = "UCase";
							object[] array = new object[1];
							object[] array2 = array;
							int num = 0;
							DataRowView dataRowView2 = dataRowView;
							DataRowView dataRowView3 = dataRowView2;
							string text4 = "Node";
							array2[num] = RuntimeHelpers.GetObjectValue(dataRowView3[text4]);
							object[] array3 = array;
							object[] array4 = array3;
							string[] array5 = null;
							Type[] array6 = null;
							bool[] array7 = new bool[] { true };
							object obj4 = NewLateBinding.LateGet(obj3, typeFromHandle, text3, array4, array5, array6, array7);
							if (array7[0])
							{
								dataRowView2[text4] = RuntimeHelpers.GetObjectValue(array3[0]);
							}
							if (Operators.ConditionalCompareObjectNotEqual(obj2, obj4, false))
							{
								trv.Nodes.Add(Conversions.ToString(dataRowView["Node"]), Conversions.ToString(dataRowView["Node"]));
								text2 = "";
							}
							object obj5 = null;
							Type typeFromHandle2 = typeof(Strings);
							string text5 = "UCase";
							array3 = new object[1];
							object[] array8 = array3;
							int num2 = 0;
							dataRowView2 = dataRowView;
							DataRowView dataRowView4 = dataRowView2;
							text4 = "Node";
							array8[num2] = RuntimeHelpers.GetObjectValue(dataRowView4[text4]);
							array = array3;
							object[] array9 = array;
							string[] array10 = null;
							Type[] array11 = null;
							array7 = new bool[] { true };
							object obj6 = NewLateBinding.LateGet(obj5, typeFromHandle2, text5, array9, array10, array11, array7);
							if (array7[0])
							{
								dataRowView2[text4] = RuntimeHelpers.GetObjectValue(array[0]);
							}
							text = Conversions.ToString(obj6);
							TreeNode treeNode = (TreeNode)NewLateBinding.LateGet(trv.Nodes, null, "Item", new object[] { RuntimeHelpers.GetObjectValue(dataRowView["Node"]) }, null, null, null);
							TreeNode treeNode2;
							if (Conversions.ToInteger(dataRowView["BlockType"]) < 240)
							{
								if (Operators.CompareString(text2, "Tags", false) != 0)
								{
									treeNode.Nodes.Add("Tags", "Tags");
								}
								text2 = "Tags";
								treeNode2 = treeNode.Nodes["Tags"];
							}
							else
							{
								if (Operators.CompareString(text2, "AlarmCounter", false) != 0)
								{
									treeNode.Nodes.Add("AlarmCounter", "AlarmCounter");
								}
								text2 = "AlarmCounter";
								treeNode2 = treeNode.Nodes["AlarmCounter"];
							}
							string text6 = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(dataRowView["Tag"], "."), dataRowView["field"]));
							treeNode2.Nodes.Add(text6, text6);
						}
					}
					finally
					{
						IEnumerator enumerator;
						if (enumerator is IDisposable)
						{
							(enumerator as IDisposable).Dispose();
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			finally
			{
				trv.EndUpdate();
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000125E4 File Offset: 0x000109E4
		private void subFixEDAGroupHandleCreate(DataTable dt)
		{
			if (dt.Rows.Count < 1)
			{
				return;
			}
			checked
			{
				try
				{
					DataView defaultView = dt.DefaultView;
					dt.DefaultView.Sort = "node, blocktype, tag";
					if (modpublic.g_Gnum.ToInt32() > 0)
					{
						Eda.DeleteGroup(modpublic.g_Gnum);
						modpublic.g_Gnum = IntPtr.Zero;
					}
					modpublic.g_Gnum = Eda.DefineGroup(1, 0);
					if (modpublic.g_Gnum.ToInt32() == 0)
					{
						throw new Exception("無法建立EDA Group hadle");
					}
					modpublic.g_ReadTagHadles = new int[dt.Rows.Count - 1 + 1];
					int num = 0;
					int num2 = defaultView.Count - 1;
					for (int i = num; i <= num2; i++)
					{
						string text4;
						if (Conversions.ToInteger(defaultView[i]["BlockType"]) >= 240)
						{
							object obj = null;
							Type typeFromHandle = typeof(Strings);
							string text = "UCase";
							object[] array = new object[1];
							object[] array2 = array;
							int num3 = 0;
							DataRowView dataRowView = defaultView[i];
							DataRowView dataRowView2 = dataRowView;
							string text2 = "tag";
							array2[num3] = RuntimeHelpers.GetObjectValue(dataRowView2[text2]);
							object[] array3 = array;
							object[] array4 = array3;
							string[] array5 = null;
							Type[] array6 = null;
							bool[] array7 = new bool[] { true };
							object obj2 = NewLateBinding.LateGet(obj, typeFromHandle, text, array4, array5, array6, array7);
							if (array7[0])
							{
								dataRowView[text2] = RuntimeHelpers.GetObjectValue(array3[0]);
							}
							object obj3 = obj2;
							if (Operators.ConditionalCompareObjectEqual(obj3, "ALARMCOUNTERS", false))
							{
								object obj4 = null;
								Type typeFromHandle2 = typeof(Strings);
								string text3 = "UCase";
								array3 = new object[1];
								object[] array8 = array3;
								int num4 = 0;
								dataRowView = defaultView[i];
								DataRowView dataRowView3 = dataRowView;
								text2 = "field";
								array8[num4] = RuntimeHelpers.GetObjectValue(dataRowView3[text2]);
								array = array3;
								object[] array9 = array;
								string[] array10 = null;
								Type[] array11 = null;
								array7 = new bool[] { true };
								object obj5 = NewLateBinding.LateGet(obj4, typeFromHandle2, text3, array9, array10, array11, array7);
								if (array7[0])
								{
									dataRowView[text2] = RuntimeHelpers.GetObjectValue(array[0]);
								}
								object obj6 = obj5;
								if (Operators.ConditionalCompareObjectEqual(obj6, "CRITICAL", false))
								{
									text4 = "F_UNACKCRIT";
								}
								else if (Operators.ConditionalCompareObjectEqual(obj6, "HIHI", false))
								{
									text4 = "F_UNACKHIHI";
								}
								else if (Operators.ConditionalCompareObjectEqual(obj6, "HIGH", false))
								{
									text4 = "F_UNACKHI";
								}
								else if (Operators.ConditionalCompareObjectEqual(obj6, "MEDIUM", false))
								{
									text4 = "F_UNACKMED";
								}
								else if (Operators.ConditionalCompareObjectEqual(obj6, "LOW", false))
								{
									text4 = "F_UNACKLO";
								}
								else if (Operators.ConditionalCompareObjectEqual(obj6, "LOLO", false))
								{
									text4 = "F_UNACKLOLO";
								}
								else if (Operators.ConditionalCompareObjectEqual(obj6, "INFO", false))
								{
									text4 = "F_UNACKINFO";
								}
								else
								{
									text4 = "F_UNACKTOT";
								}
							}
							else
							{
								object obj7 = null;
								Type typeFromHandle3 = typeof(Strings);
								string text5 = "UCase";
								array3 = new object[1];
								object[] array12 = array3;
								int num5 = 0;
								dataRowView = defaultView[i];
								DataRowView dataRowView4 = dataRowView;
								text2 = "field";
								array12[num5] = RuntimeHelpers.GetObjectValue(dataRowView4[text2]);
								array = array3;
								object[] array13 = array;
								string[] array14 = null;
								Type[] array15 = null;
								array7 = new bool[] { true };
								object obj8 = NewLateBinding.LateGet(obj7, typeFromHandle3, text5, array13, array14, array15, array7);
								if (array7[0])
								{
									dataRowView[text2] = RuntimeHelpers.GetObjectValue(array[0]);
								}
								object obj9 = obj8;
								if (Operators.ConditionalCompareObjectEqual(obj9, "CRITICAL", false))
								{
									text4 = "F_AREA_UNACKCRIT";
								}
								else if (Operators.ConditionalCompareObjectEqual(obj9, "HIHI", false))
								{
									text4 = "F_AREA_UNACKHIHI";
								}
								else if (Operators.ConditionalCompareObjectEqual(obj9, "HIGH", false))
								{
									text4 = "F_AREA_UNACKHI";
								}
								else if (Operators.ConditionalCompareObjectEqual(obj9, "MEDIUM", false))
								{
									text4 = "F_AREA_UNACKMED";
								}
								else if (Operators.ConditionalCompareObjectEqual(obj9, "LOW", false))
								{
									text4 = "F_AREA_UNACKLO";
								}
								else if (Operators.ConditionalCompareObjectEqual(obj9, "LOLO", false))
								{
									text4 = "F_AREA_UNACKLOLO";
								}
								else if (Operators.ConditionalCompareObjectEqual(obj9, "INFO", false))
								{
									text4 = "F_AREA_UNACKINFO";
								}
								else
								{
									text4 = "F_AREA_UNACK";
								}
							}
						}
						else
						{
							text4 = "F_NALM";
						}
						modpublic.g_ReadTagHadles[i] = Eda.DefineNtf(modpublic.g_Gnum, Conversions.ToString(defaultView[i]["Node"]), Conversions.ToString(defaultView[i]["Tag"]), text4, IntPtr.Zero);
					}
				}
				catch (Exception ex)
				{
					modSub.subError(ex.Message, true, false, "", Conversions.ToString(true), false);
				}
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00012A10 File Offset: 0x00010E10
		private void subEDARead(DataTable dtEDARunTime, TreeView trv)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			string text = "";
			StringBuilder stringBuilder2 = new StringBuilder(80);
			string text2 = "";
			DataView defaultView = dtEDARunTime.DefaultView;
			checked
			{
				try
				{
					if (dtEDARunTime.Rows.Count >= 1)
					{
						Eda.Lookup(modpublic.g_Gnum);
						Eda.Wait(modpublic.g_Gnum);
						Eda.Read(modpublic.g_Gnum);
						Eda.Wait(modpublic.g_Gnum);
						trv.BeginUpdate();
						int num = 0;
						int num2 = Information.UBound(modpublic.g_ReadTagHadles, 1);
						for (int i = num; i <= num2; i++)
						{
							if (modpublic.g_StopSound)
							{
								break;
							}
							int num3 = Conversions.ToInteger(defaultView[i]["BlockType"]);
							string text3 = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(defaultView[i]["Tag"], "."), defaultView[i]["field"]));
							TreeNode treeNode;
							if (num3 < 240)
							{
								object obj = NewLateBinding.LateGet(NewLateBinding.LateGet(trv.Nodes, null, "Item", new object[] { RuntimeHelpers.GetObjectValue(defaultView[i]["Node"]) }, null, null, null), null, "Nodes", new object[] { "Tags" }, null, null, null);
								Type type = null;
								string text4 = "Nodes";
								object[] array = new object[] { text3 };
								object[] array2 = array;
								string[] array3 = null;
								Type[] array4 = null;
								bool[] array5 = new bool[] { true };
								object obj2 = NewLateBinding.LateGet(obj, type, text4, array2, array3, array4, array5);
								if (array5[0])
								{
									text3 = (string)Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array[0]), typeof(string));
								}
								treeNode = (TreeNode)obj2;
							}
							else
							{
								object obj3 = NewLateBinding.LateGet(NewLateBinding.LateGet(trv.Nodes, null, "Item", new object[] { RuntimeHelpers.GetObjectValue(defaultView[i]["Node"]) }, null, null, null), null, "Nodes", new object[] { "Alarmcounter" }, null, null, null);
								Type type2 = null;
								string text5 = "Nodes";
								object[] array6 = new object[] { text3 };
								object[] array7 = array6;
								string[] array8 = null;
								Type[] array9 = null;
								bool[] array5 = new bool[] { true };
								object obj4 = NewLateBinding.LateGet(obj3, type2, text5, array7, array8, array9, array5);
								if (array5[0])
								{
									text3 = (string)Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array6[0]), typeof(string));
								}
								treeNode = (TreeNode)obj4;
							}
							int num4;
							int num5;
							if (Operators.ConditionalCompareObjectNotEqual(text, defaultView[i]["Node"], false))
							{
								text = Conversions.ToString(defaultView[i]["Node"]);
								NewLateBinding.LateSetComplex(NewLateBinding.LateGet(trv.Nodes, null, "Item", new object[] { RuntimeHelpers.GetObjectValue(defaultView[i]["Node"]) }, null, null, null), null, "ForeColor", new object[] { Color.Gray }, null, null, false, true);
								num4 = 0;
								num5 = 0;
							}
							if (num3 < 240)
							{
								if (Operators.CompareString(text2, "Tags", false) != 0)
								{
									text2 = "Tags";
									NewLateBinding.LateSetComplex(NewLateBinding.LateGet(NewLateBinding.LateGet(trv.Nodes, null, "Item", new object[] { RuntimeHelpers.GetObjectValue(defaultView[i]["Node"]) }, null, null, null), null, "Nodes", new object[] { "Tags" }, null, null, null), null, "ForeColor", new object[] { Color.Gray }, null, null, false, true);
									num5 = 0;
								}
							}
							else if (Operators.CompareString(text2, "AlarmCounter", false) != 0)
							{
								text2 = "AlarmCounter";
								NewLateBinding.LateSetComplex(NewLateBinding.LateGet(NewLateBinding.LateGet(trv.Nodes, null, "Item", new object[] { RuntimeHelpers.GetObjectValue(defaultView[i]["Node"]) }, null, null, null), null, "Nodes", new object[] { "AlarmCounter" }, null, null, null), null, "ForeColor", new object[] { Color.Gray }, null, null, false, true);
								num5 = 0;
							}
							string text6 = Conversions.ToString(defaultView[i]["Tag"]);
							int num6 = Conversions.ToInteger(defaultView[i]["Priority"]);
							string text7 = Conversions.ToString(defaultView[i]["Wave"]);
							string text8;
							if (!Information.IsDBNull(RuntimeHelpers.GetObjectValue(defaultView[i]["field"])))
							{
								text8 = Conversions.ToString(defaultView[i]["field"]);
							}
							else
							{
								text8 = "NOPRI";
							}
							string text9 = string.Concat(new string[] { text, ".", text6, ".", text8 });
							int num7 = Conversions.ToInteger(defaultView[i]["Length"]);
							string text10 = "almtag = '" + text9 + "'";
							DataRow[] array10 = modpublic.g_dtSoundQueue.Select(text10);
							float num8 = 0f;
							short num9;
							if (num3 >= 240)
							{
								num9 = Eda.GetFloat(modpublic.g_Gnum, modpublic.g_ReadTagHadles[i], out num8);
							}
							else
							{
								num9 = Eda.GetFloat(modpublic.g_Gnum, modpublic.g_ReadTagHadles[i], out num8);
								if (num9 == 0 && Operators.CompareString(Strings.UCase(text8), "NOPRI", false) != 0 && num8 > 0f)
								{
									num9 = Eda.GetOneAscii(text, text6, "A_LAALM", stringBuilder);
									if (num9 == 0)
									{
										string text11 = Strings.UCase(text8);
										if (Operators.CompareString(text11, "HIHI", false) == 0)
										{
											if (Operators.CompareString(stringBuilder.ToString(), "HIHI", false) != 0)
											{
												num8 = 0f;
											}
										}
										else if (Operators.CompareString(text11, "HI", false) == 0)
										{
											if (Operators.CompareString(stringBuilder.ToString(), "HI", false) != 0)
											{
												num8 = 0f;
											}
										}
										else if (Operators.CompareString(text11, "LO", false) == 0)
										{
											if (Operators.CompareString(stringBuilder.ToString(), "LO", false) != 0)
											{
												num8 = 0f;
											}
										}
										else if (Operators.CompareString(text11, "LOLO", false) == 0 && Operators.CompareString(stringBuilder.ToString(), "LOLO", false) != 0)
										{
											num8 = 0f;
										}
									}
								}
							}
							if (num9 != 0)
							{
								Helper.NlsGetText((int)num9, stringBuilder2, 256);
								treeNode.ForeColor = Color.Gray;
								treeNode.ToolTipText = stringBuilder2.ToString();
								if (array10.Length > 0)
								{
									modpublic.g_dtSoundQueue.Rows.Remove(array10[0]);
								}
								defaultView[i]["Played"] = false;
								defaultView[i]["LastAlmCount"] = 0;
								defaultView[i]["playcnt"] = 0;
							}
							else
							{
								float num10 = Conversions.ToSingle(defaultView[i]["LastAlmCount"]);
								float num11 = Conversions.ToSingle(defaultView[i]["PlayCnt"]);
								defaultView[i]["LastAlmCount"] = num8;
								object obj5 = this.messagesLock;
								ObjectFlowControl.CheckForSyncLockOnValueType(obj5);
								lock (obj5)
								{
									treeNode.ToolTipText = num8.ToString().Trim();
									TreeNode treeNode2 = treeNode;
									object obj6 = Interaction.IIf(num8 > 0f, Color.Red, Color.Green);
									Color color;
									treeNode2.ForeColor = ((obj6 != null) ? ((Color)obj6) : color);
									if (num8 > 0f)
									{
										num4++;
										num5++;
										if (!modpublic.g_bPlayOnce || !Operators.ConditionalCompareObjectEqual(defaultView[i]["Played"], true, false))
										{
											if (!modpublic.g_bUsingPlayFinishedNowFunction || num11 <= 0f)
											{
												if (modpublic.g_nPlayCount <= 0 || num11 < (float)modpublic.g_nPlayCount)
												{
													if (array10.Length < 1)
													{
														if (text7.Trim().Length > 0)
														{
															DataRow dataRow = modpublic.g_dtSoundQueue.NewRow();
															dataRow["sound"] = text7.Trim();
															dataRow["timein"] = DateAndTime.Now;
															dataRow["almtag"] = text9;
															dataRow["priority"] = num6;
															dataRow["length"] = num7;
															modpublic.g_dtSoundQueue.Rows.Add(dataRow);
														}
													}
													else if (Operators.ConditionalCompareObjectGreater(array10[0]["priority"], num6, false))
													{
														array10[0]["timein"] = DateAndTime.Now;
														array10[0]["priority"] = num6;
													}
													if (Conversions.ToString(defaultView[i]["picture"]).Length > 0 && Operators.ConditionalCompareObjectEqual(defaultView[i]["Played"], false, false))
													{
														string text12 = "";
														string text13 = Conversions.ToString(defaultView[i]["picture"]);
														FixHelper fixHelper = new FixHelper();
														if (Strings.InStr(text13, "\\", CompareMethod.Binary) < 1)
														{
															short num12 = fixHelper.FixGetPath("PICPATH", ref text12);
															if (num12 != 11000)
															{
																text13 = Application.StartupPath + "\\" + text13;
															}
															else
															{
																text13 = text12 + "\\" + text13;
															}
														}
														if (Strings.InStr(text13, ".grf", CompareMethod.Text) < 1)
														{
															text13 += ".grf";
														}
														if (File.Exists(text13))
														{
															clsiFix clsiFix = new clsiFix(text13, new Callback_Method(this.Callback_Result), "", "");
															Thread thread = new Thread(new ThreadStart(clsiFix.OpenPicture));
															thread.Start();
														}
														else
														{
															string text14 = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Strings.Format(DateAndTime.Now, "yyyy/MM/dd HH:mm:ss") + ": OpenPicture <", defaultView[i]["picture"]), "> failed, because the file is not existed."));
															this.Callback_Result(text14);
														}
													}
													defaultView[i]["Played"] = true;
												}
											}
										}
									}
									else
									{
										if ((num10 > 0f) & (num8 < 1f))
										{
											if (array10.Length > 0)
											{
												modpublic.g_dtSoundQueue.Rows.Remove(array10[0]);
											}
											defaultView[i]["LastAlmCount"] = 0;
											defaultView[i]["playcnt"] = 0;
										}
										defaultView[i]["Played"] = false;
									}
								}
								if (num4 > 0)
								{
									NewLateBinding.LateSetComplex(NewLateBinding.LateGet(trv.Nodes, null, "Item", new object[] { RuntimeHelpers.GetObjectValue(defaultView[i]["Node"]) }, null, null, null), null, "ForeColor", new object[] { Color.Red }, null, null, false, true);
								}
								else
								{
									NewLateBinding.LateSetComplex(NewLateBinding.LateGet(trv.Nodes, null, "Item", new object[] { RuntimeHelpers.GetObjectValue(defaultView[i]["Node"]) }, null, null, null), null, "ForeColor", new object[] { Color.Green }, null, null, false, true);
								}
								if (num5 > 0)
								{
									object obj7 = NewLateBinding.LateGet(trv.Nodes, null, "Item", new object[] { RuntimeHelpers.GetObjectValue(defaultView[i]["Node"]) }, null, null, null);
									Type type3 = null;
									string text15 = "Nodes";
									object[] array11 = new object[] { text2 };
									object[] array12 = array11;
									string[] array13 = null;
									Type[] array14 = null;
									bool[] array5 = new bool[] { true };
									object obj8 = NewLateBinding.LateGet(obj7, type3, text15, array12, array13, array14, array5);
									if (array5[0])
									{
										text2 = (string)Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array11[0]), typeof(string));
									}
									NewLateBinding.LateSetComplex(obj8, null, "ForeColor", new object[] { Color.Red }, null, null, false, true);
								}
								else
								{
									object obj9 = NewLateBinding.LateGet(trv.Nodes, null, "Item", new object[] { RuntimeHelpers.GetObjectValue(defaultView[i]["Node"]) }, null, null, null);
									Type type4 = null;
									string text16 = "Nodes";
									object[] array11 = new object[] { text2 };
									object[] array15 = array11;
									string[] array16 = null;
									Type[] array17 = null;
									bool[] array5 = new bool[] { true };
									object obj10 = NewLateBinding.LateGet(obj9, type4, text16, array15, array16, array17, array5);
									if (array5[0])
									{
										text2 = (string)Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array11[0]), typeof(string));
									}
									NewLateBinding.LateSetComplex(obj10, null, "ForeColor", new object[] { Color.Green }, null, null, false, true);
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					string text14 = Strings.Format(DateAndTime.Now, "yyyy/MM/dd HH:mm:ss") + ": 系統錯誤(subEDARead) " + ex.Message;
					this.Callback_Result(text14);
				}
				finally
				{
					trv.EndUpdate();
				}
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0001383C File Offset: 0x00011C3C
		private void subKillQueue()
		{
			if (modpublic.g_dtSoundQueue.Rows.Count > 0)
			{
				modpublic.g_dtSoundQueue.Clear();
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0001385C File Offset: 0x00011C5C
		private void subCreateSystemTable()
		{
			DataColumn dataColumn = new DataColumn();
			try
			{
				dataColumn = new DataColumn();
				DataColumn dataColumn2 = dataColumn;
				dataColumn2.DataType = Type.GetType("System.String");
				dataColumn2.MaxLength = 255;
				dataColumn2.AllowDBNull = false;
				dataColumn2.Caption = "警報聲音";
				dataColumn2.ColumnName = "sound";
				modpublic.g_dtSoundQueue.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				DataColumn dataColumn3 = dataColumn;
				dataColumn3.DataType = Type.GetType("System.DateTime");
				dataColumn3.AllowDBNull = false;
				dataColumn3.DefaultValue = DateAndTime.Now;
				dataColumn3.Caption = "發生時間";
				dataColumn3.ColumnName = "TimeIn";
				modpublic.g_dtSoundQueue.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				DataColumn dataColumn4 = dataColumn;
				dataColumn4.DataType = Type.GetType("System.String");
				dataColumn4.MaxLength = 255;
				dataColumn4.AllowDBNull = false;
				dataColumn4.Caption = "警報點";
				dataColumn4.ColumnName = "almtag";
				modpublic.g_dtSoundQueue.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				DataColumn dataColumn5 = dataColumn;
				dataColumn5.DataType = Type.GetType("System.Int16");
				dataColumn5.AllowDBNull = false;
				dataColumn5.DefaultValue = "2";
				dataColumn5.Caption = "播音等級";
				dataColumn5.ColumnName = "priority";
				modpublic.g_dtSoundQueue.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				DataColumn dataColumn6 = dataColumn;
				dataColumn6.DataType = Type.GetType("System.Int16");
				dataColumn6.AllowDBNull = false;
				dataColumn6.DefaultValue = "0";
				dataColumn6.Caption = "秒數";
				dataColumn6.ColumnName = "Length";
				modpublic.g_dtSoundQueue.Columns.Add(dataColumn);
				DataColumn[] array = new DataColumn[] { modpublic.g_dtSoundQueue.Columns["almtag"] };
				modpublic.g_dtSoundQueue.PrimaryKey = array;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00013A84 File Offset: 0x00011E84
		protected override void WndProc(ref Message m)
		{
			try
			{
				if (m.Msg == 74)
				{
					object lparam = m.GetLParam(typeof(frmAlmSndRun.CopyData));
					frmAlmSndRun.CopyData copyData2;
					frmAlmSndRun.CopyData copyData = ((lparam != null) ? ((frmAlmSndRun.CopyData)lparam) : copyData2);
					modpublic.g_WMCOPYDATA = Marshal.PtrToStringAuto(copyData.lpData, copyData.cbData / Marshal.SystemDefaultCharSize);
					IntPtr intPtr = new IntPtr(1);
					m.Result = intPtr;
				}
				else
				{
					base.WndProc(ref m);
				}
			}
			catch (Exception ex)
			{
				modSub.subError("Sub WndProc\\ " + ex.Message, false, true, "", "", false);
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00013B38 File Offset: 0x00011F38
		private void frmAlmSndRun_Load(object sender, EventArgs e)
		{
			try
			{
				modDllChk.subDLLCheck();
				modSub.subParameterSearch();
				if (!modpublic.g_bMultiInstance)
				{
					modSub.subSingleInstance();
				}
				modSub.subChkLicense();
				this.subDbIniFileRead();
				if (modpublic.g_iDelay > 0)
				{
					Thread.Sleep(checked(modpublic.g_iDelay * 1000));
				}
				modpublic.frmRun = this;
				this.txtSoundTime.Text = Conversions.ToString(modpublic.g_iSoundTime);
				this.txtScanTime.Text = Conversions.ToString(modpublic.g_iScanTime);
				if (modpublic.g_bBackground)
				{
					this.Opacity = 0.0;
					this.ShowInTaskbar = false;
				}
				else if (modpublic.g_bMenuBar)
				{
					try
					{
						foreach (object obj in this.MenuStrip1.Items)
						{
							ToolStripItem toolStripItem = (ToolStripItem)obj;
							toolStripItem.Visible = false;
						}
					}
					finally
					{
						IEnumerator enumerator;
						if (enumerator is IDisposable)
						{
							(enumerator as IDisposable).Dispose();
						}
					}
				}
				this.subCreateSystemTable();
				modpublic.g_dtSoundQueue.DefaultView.Sort = "priority ASC, timeIn ASC";
				this.subDBLoad(modpublic.g_sCfgName, false);
				this.FillTreeView(this._dsEDARunTime.Tables[0], this.TreeView1);
				this.subFixEDAGroupHandleCreate(this._dsEDARunTime.Tables[0]);
				this.tmrSendMessage_Tick(RuntimeHelpers.GetObjectValue(sender), e);
				this.tmrRun_Tick(RuntimeHelpers.GetObjectValue(sender), e);
				this.ckQueue_CheckedChanged(RuntimeHelpers.GetObjectValue(sender), null);
				this.tmrPlaySound.Interval = 10;
				this.tmrPlaySound_Tick(RuntimeHelpers.GetObjectValue(sender), e);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00013D0C File Offset: 0x0001210C
		private void frmAlmSndRun_Shown(object sender, EventArgs e)
		{
			clsEDA clsEDA = new clsEDA();
			try
			{
				if (!clsEDA.FixIsFixRunning())
				{
					throw new Exception("FIX is not running!");
				}
				this.WindowState = FormWindowState.Minimized;
				this.Opacity = 0.0;
				this.ShowInTaskbar = false;
				this.NotifyIcon1.Visible = true;
			}
			catch (Exception ex)
			{
				modSub.subError("Form_Shown/ " + ex.Message, true, true, "", "", true);
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00013DA0 File Offset: 0x000121A0
		private void frmAlmSndRun_SizeChanged(object sender, EventArgs e)
		{
			if (this.WindowState == FormWindowState.Minimized)
			{
				this.Opacity = 0.0;
				this.ShowInTaskbar = false;
				this.NotifyIcon1.Visible = true;
			}
			else
			{
				this.Opacity = 100.0;
				this.ShowInTaskbar = true;
				this.NotifyIcon1.Visible = false;
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00013DFC File Offset: 0x000121FC
		private void frmAlmSndRun_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control)
			{
				int keyValue = e.KeyValue;
				if (keyValue == 83)
				{
					this.ckMute.Checked = true;
				}
				else if (keyValue == 115)
				{
					this.ckMute.Checked = true;
				}
				else if (keyValue == 80)
				{
					this.ckMute.Checked = false;
				}
				else if (keyValue == 112)
				{
					this.ckMute.Checked = false;
				}
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00013E64 File Offset: 0x00012264
		private void frmAlmSndRun_FormClosing(object sender, FormClosingEventArgs e)
		{
			string text = "Are you sure to shutdown the service?";
			int num = (int)MessageBox.Show(text, "Close Service", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
			if (num == 6)
			{
				this.NotifyIcon1.Visible = false;
				this.tmrRun.Stop();
				this.tmrPlaySound.Stop();
				modpublic.g_StopSound = true;
				this.subKillQueue();
				modSub.subReleaseResource("", "");
				if (modpublic.g_Demo)
				{
					text = "Thanks for Trying " + Application.ProductName + "\r\n" + modpublic.sTrendtek;
					MessageBox.Show(text, "DEMO Mode", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
			else
			{
				e.Cancel = true;
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00013F0C File Offset: 0x0001230C
		private void ckQueue_CheckedChanged(object sender, EventArgs e)
		{
			if (this.ckQueue.Checked)
			{
				this.lstQueue.DisplayMember = "sound";
				this.lstQueue.DataSource = modpublic.g_dtSoundQueue.DefaultView;
			}
			else
			{
				this.lstQueue.DisplayMember = null;
				this.lstQueue.DataSource = null;
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00013F68 File Offset: 0x00012368
		private void miCfg_Click(object sender, EventArgs e)
		{
			this.ShowInTaskbar = false;
			modpublic.frmConfig = new frmAlarmSndCfg();
			this.Hide();
			modpublic.frmConfig.ShowDialog();
			modpublic.frmConfig = null;
			this.Show();
			this.ShowInTaskbar = true;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00013FA0 File Offset: 0x000123A0
		private void ckMute_CheckedChanged(object sender, EventArgs e)
		{
			modpublic.g_Mute = this.ckMute.Checked;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00013FB4 File Offset: 0x000123B4
		private void miConfig_Click(object sender, EventArgs e)
		{
			this.ShowInTaskbar = false;
			modpublic.frmConfig = new frmAlarmSndCfg();
			this.Hide();
			modpublic.frmConfig.ShowDialog();
			modpublic.frmConfig = null;
			this.Show();
			this.ShowInTaskbar = true;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00013FEC File Offset: 0x000123EC
		private void tsmiClearEvent_Click(object sender, EventArgs e)
		{
			this.Monitor.Items.Clear();
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00014000 File Offset: 0x00012400
		private void tsmiShowEvent_Click(object sender, EventArgs e)
		{
			this.tsmiShowEvent.Checked = !this.tsmiShowEvent.Checked;
			modpublic.g_ShowEvt = this.tsmiShowEvent.Checked;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0001402C File Offset: 0x0001242C
		private void tsmiShowError_Click(object sender, EventArgs e)
		{
			this.tsmiShowError.Checked = !this.tsmiShowError.Checked;
			modpublic.g_ShowErr = this.tsmiShowError.Checked;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00014058 File Offset: 0x00012458
		private void cmsCollapseAll_Click(object sender, EventArgs e)
		{
			if (Operators.CompareString(((ToolStripMenuItem)sender).Name, "cmsCollapseAll", false) == 0)
			{
				try
				{
					foreach (object obj in this.TreeView1.Nodes)
					{
						TreeNode treeNode = (TreeNode)obj;
						treeNode.Collapse();
					}
					return;
				}
				finally
				{
					IEnumerator enumerator;
					if (enumerator is IDisposable)
					{
						(enumerator as IDisposable).Dispose();
					}
				}
			}
			try
			{
				foreach (object obj2 in this.TreeView1.Nodes)
				{
					TreeNode treeNode = (TreeNode)obj2;
					try
					{
						foreach (object obj3 in treeNode.Nodes)
						{
							TreeNode treeNode2 = (TreeNode)obj3;
							treeNode2.Collapse();
						}
					}
					finally
					{
						IEnumerator enumerator3;
						if (enumerator3 is IDisposable)
						{
							(enumerator3 as IDisposable).Dispose();
						}
					}
				}
			}
			finally
			{
				IEnumerator enumerator2;
				if (enumerator2 is IDisposable)
				{
					(enumerator2 as IDisposable).Dispose();
				}
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00014174 File Offset: 0x00012574
		private void cmsExpandAll_Click(object sender, EventArgs e)
		{
			if (Operators.CompareString(((ToolStripMenuItem)sender).Name, "cmsExpandAll", false) == 0)
			{
				this.TreeView1.ExpandAll();
			}
			else
			{
				try
				{
					foreach (object obj in this.TreeView1.Nodes)
					{
						TreeNode treeNode = (TreeNode)obj;
						treeNode.Expand();
					}
				}
				finally
				{
					IEnumerator enumerator;
					if (enumerator is IDisposable)
					{
						(enumerator as IDisposable).Dispose();
					}
				}
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x000141FC File Offset: 0x000125FC
		private void miAbout_Click(object sender, EventArgs e)
		{
			frmAbout frmAbout = new frmAbout();
			frmAbout.Show();
			this.Enabled = false;
			this.Visible = false;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00014224 File Offset: 0x00012624
		private void tmrSendMessage_Tick(object sender, EventArgs e)
		{
			clsEDA clsEDA = new clsEDA();
			try
			{
				this.tmrSendMessage.Stop();
				if (modpublic.g_biFixStarted && !clsEDA.FixIsFixRunning() && Information.IsNothing(modpublic.frmConfig))
				{
					Application.Exit();
				}
				else if (modpublic.g_WMCOPYDATA.Length >= 1)
				{
					string text = Strings.UCase(modpublic.g_WMCOPYDATA);
					modpublic.g_WMCOPYDATA = "";
					if (Strings.InStr(1, text, "SOUND=", CompareMethod.Text) != 0)
					{
						string text2 = modSub.funGetsubParameterX(text, "SOUND=", "", false, false);
						if (text2.Length > 0)
						{
							this.ckMute.Checked = Conversions.ToBoolean(Interaction.IIf(Operators.CompareString(text2.ToUpper(), "ON", false) == 0, false, true));
							modpublic.g_Mute = this.ckMute.Checked;
						}
					}
					else if (Strings.InStr(1, text, "PLAYFINISHEDNOW", CompareMethod.Text) != 0 && !modpublic.g_bPlayOnce && modpublic.g_nPlayCount < 1 && modpublic.g_bUsingPlayFinishedNowFunction)
					{
						try
						{
							foreach (object obj in this._dsEDARunTime.Tables[0].Rows)
							{
								DataRow dataRow = (DataRow)obj;
								dataRow["PlayCnt"] = RuntimeHelpers.GetObjectValue(dataRow["LastAlmCount"]);
							}
						}
						finally
						{
							IEnumerator enumerator;
							if (enumerator is IDisposable)
							{
								(enumerator as IDisposable).Dispose();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				string text3 = Strings.Format(DateAndTime.Now, "yyyy/MM/dd HH:mm:ss") + ": ";
				modSub.subError("Sub WndProc\\ " + ex.Message, false, true, "", "", false);
			}
			finally
			{
				if (!modpublic.g_StopSound)
				{
					this.tmrSendMessage.Start();
				}
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00014454 File Offset: 0x00012854
		private void tmrRun_Tick(object sender, EventArgs e)
		{
			clsEDA clsEDA = new clsEDA();
			checked
			{
				try
				{
					this.tmrRun.Stop();
					if (!modpublic.g_StopSound)
					{
						DateTime now = DateAndTime.Now;
						if (modpublic.g_biFixStarted && !clsEDA.FixIsFixRunning() && Information.IsNothing(modpublic.frmConfig))
						{
							Application.Exit();
						}
						else
						{
							modpublic.g_biFixStarted = clsEDA.FixIsFixRunning();
							this.subEDARead(this._dsEDARunTime.Tables[0], this.TreeView1);
							if ((double)(DateAndTime.Now.Ticks - now.Ticks) / 10000.0 > (double)modpublic.g_iScanTime)
							{
								this.txtOverRun.Text = Conversion.Str(Conversions.ToInteger(this.txtOverRun.Text) + 1);
							}
						}
					}
				}
				catch (Exception ex)
				{
					string text = Strings.Format(DateAndTime.Now, "yyyy/MM/dd HH:mm:ss") + ": ";
					this.Callback_Result(text + ex.Message);
				}
				finally
				{
					this.tmrRun.Interval = modpublic.g_iScanTime;
					if (!modpublic.g_StopSound)
					{
						this.tmrRun.Start();
					}
				}
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x000145A4 File Offset: 0x000129A4
		private void tmrPlaySound_Tick(object sender, EventArgs e)
		{
			List<string> list = new List<string>();
			string text = "";
			string text2 = "";
			DataTable dataTable = this._dsEDARunTime.Tables[0];
			DataView defaultView = modpublic.g_dtSoundQueue.DefaultView;
			checked
			{
				try
				{
					this.tmrPlaySound.Stop();
					if (!modpublic.g_StopSound)
					{
						if (modpublic.g_dtSoundQueue.Rows.Count > 0)
						{
							bool flag = true;
							string text3 = Conversions.ToString(defaultView[0]["sound"]);
							int num = Conversions.ToInteger(defaultView[0]["length"]);
							string text4 = "sound = '" + text3 + "'";
							DataRow[] array = modpublic.g_dtSoundQueue.Select(text4);
							foreach (DataRow dataRow in array)
							{
								list.Add(Conversions.ToString(dataRow["almtag"]));
								modpublic.g_dtSoundQueue.Rows.Remove(dataRow);
							}
							if (modpublic.g_nPlayCount > 0)
							{
								try
								{
									foreach (string text5 in list)
									{
										string text6 = "NOPRI";
										string[] array3 = text5.Split(new char[] { '.' });
										if (array3.Length > 0)
										{
											text = array3[0];
										}
										if (array3.Length > 1)
										{
											text2 = array3[1];
										}
										if (array3.Length > 2)
										{
											text6 = array3[2];
										}
										text4 = string.Concat(new string[] { "Node = '", text, "' AND Tag = '", text2, "' AND Field = '", text6, "'" });
										array = dataTable.Select(text4);
										if (array.Length > 0 && Operators.ConditionalCompareObjectLess(array[0]["PlayCnt"], 100, false))
										{
											DataRow[] array4 = array;
											DataRow[] array5 = array4;
											int num2 = 0;
											DataRow dataRow2 = array5[num2];
											string text7 = "PlayCnt";
											dataRow2[text7] = Operators.AddObject(array4[num2][text7], 1);
										}
									}
								}
								finally
								{
									List<string>.Enumerator enumerator;
									((IDisposable)enumerator).Dispose();
								}
							}
							clsPlaySound clsPlaySound = new clsPlaySound(text3, new Callback_Method(this.Callback_Result));
							Thread thread = new Thread(new ThreadStart(clsPlaySound.ThreadProc));
							thread.Start();
						}
					}
				}
				catch (Exception ex)
				{
					string text8 = Strings.Format(DateAndTime.Now, "yyyy/MM/dd HH:mm:ss") + ": ";
					this.Callback_Result(text8 + ex.Message);
				}
				finally
				{
					if (!modpublic.g_StopSound)
					{
						bool flag;
						if (flag)
						{
							int num;
							if (num <= 0)
							{
								this.tmrPlaySound.Interval = modpublic.g_iSoundTime * 1000;
							}
							else
							{
								this.tmrPlaySound.Interval = num * 1000;
							}
						}
						else
						{
							this.tmrPlaySound.Interval = 10;
						}
						this.tmrPlaySound.Start();
					}
				}
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000148E8 File Offset: 0x00012CE8
		private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.Opacity = 100.0;
			this.ShowInTaskbar = true;
			this.WindowState = FormWindowState.Normal;
			this.NotifyIcon1.Visible = false;
		}

		// Token: 0x040000AD RID: 173
		[AccessedThroughProperty("Label3")]
		private Label _Label3;

		// Token: 0x040000AE RID: 174
		[AccessedThroughProperty("Label1")]
		private Label _Label1;

		// Token: 0x040000AF RID: 175
		[AccessedThroughProperty("txtSoundTime")]
		private TextBox _txtSoundTime;

		// Token: 0x040000B0 RID: 176
		[AccessedThroughProperty("txtScanTime")]
		private TextBox _txtScanTime;

		// Token: 0x040000B1 RID: 177
		[AccessedThroughProperty("Label9")]
		private Label _Label9;

		// Token: 0x040000B2 RID: 178
		[AccessedThroughProperty("Label10")]
		private Label _Label10;

		// Token: 0x040000B3 RID: 179
		[AccessedThroughProperty("lstQueue")]
		private ListBox _lstQueue;

		// Token: 0x040000B4 RID: 180
		[AccessedThroughProperty("Monitor")]
		private ListBox _Monitor;

		// Token: 0x040000B5 RID: 181
		[AccessedThroughProperty("ckMute")]
		private CheckBox _ckMute;

		// Token: 0x040000B6 RID: 182
		[AccessedThroughProperty("txtOverRun")]
		private TextBox _txtOverRun;

		// Token: 0x040000B7 RID: 183
		[AccessedThroughProperty("Label4")]
		private Label _Label4;

		// Token: 0x040000B8 RID: 184
		[AccessedThroughProperty("GroupBox1")]
		private GroupBox _GroupBox1;

		// Token: 0x040000B9 RID: 185
		[AccessedThroughProperty("GroupBox2")]
		private GroupBox _GroupBox2;

		// Token: 0x040000BA RID: 186
		[AccessedThroughProperty("GroupBox3")]
		private GroupBox _GroupBox3;

		// Token: 0x040000BB RID: 187
		[AccessedThroughProperty("TreeView1")]
		private TreeView _TreeView1;

		// Token: 0x040000BC RID: 188
		[AccessedThroughProperty("GroupBox4")]
		private GroupBox _GroupBox4;

		// Token: 0x040000BD RID: 189
		[AccessedThroughProperty("_dsEDARunTime")]
		private DataSet __dsEDARunTime;

		// Token: 0x040000BE RID: 190
		[AccessedThroughProperty("DataTable1")]
		private DataTable _DataTable1;

		// Token: 0x040000BF RID: 191
		[AccessedThroughProperty("DataColumn1")]
		private DataColumn _DataColumn1;

		// Token: 0x040000C0 RID: 192
		[AccessedThroughProperty("DataColumn2")]
		private DataColumn _DataColumn2;

		// Token: 0x040000C1 RID: 193
		[AccessedThroughProperty("DataColumn3")]
		private DataColumn _DataColumn3;

		// Token: 0x040000C2 RID: 194
		[AccessedThroughProperty("tmrRun")]
		private global::System.Windows.Forms.Timer _tmrRun;

		// Token: 0x040000C3 RID: 195
		[AccessedThroughProperty("tmrPlaySound")]
		private global::System.Windows.Forms.Timer _tmrPlaySound;

		// Token: 0x040000C4 RID: 196
		[AccessedThroughProperty("ToolTip1")]
		private ToolTip _ToolTip1;

		// Token: 0x040000C5 RID: 197
		[AccessedThroughProperty("ckQueue")]
		private CheckBox _ckQueue;

		// Token: 0x040000C6 RID: 198
		[AccessedThroughProperty("MenuStrip1")]
		private MenuStrip _MenuStrip1;

		// Token: 0x040000C7 RID: 199
		[AccessedThroughProperty("miConfig")]
		private ToolStripMenuItem _miConfig;

		// Token: 0x040000C8 RID: 200
		[AccessedThroughProperty("說明ToolStripMenuItem")]
		private ToolStripMenuItem _說明ToolStripMenuItem;

		// Token: 0x040000C9 RID: 201
		[AccessedThroughProperty("ContextMenuStrip_Event")]
		private ContextMenuStrip _ContextMenuStrip_Event;

		// Token: 0x040000CA RID: 202
		[AccessedThroughProperty("tsmiClearEvent")]
		private ToolStripMenuItem _tsmiClearEvent;

		// Token: 0x040000CB RID: 203
		[AccessedThroughProperty("tsmiShowEvent")]
		private ToolStripMenuItem _tsmiShowEvent;

		// Token: 0x040000CC RID: 204
		[AccessedThroughProperty("tsmiShowError")]
		private ToolStripMenuItem _tsmiShowError;

		// Token: 0x040000CD RID: 205
		[AccessedThroughProperty("ToolStripMenuItem1")]
		private ToolStripSeparator _ToolStripMenuItem1;

		// Token: 0x040000CE RID: 206
		[AccessedThroughProperty("ContextMenuStrip_Tree")]
		private ContextMenuStrip _ContextMenuStrip_Tree;

		// Token: 0x040000CF RID: 207
		[AccessedThroughProperty("cmsCollapseAll")]
		private ToolStripMenuItem _cmsCollapseAll;

		// Token: 0x040000D0 RID: 208
		[AccessedThroughProperty("cmdCollapseType")]
		private ToolStripMenuItem _cmdCollapseType;

		// Token: 0x040000D1 RID: 209
		[AccessedThroughProperty("ToolStripMenuItem2")]
		private ToolStripSeparator _ToolStripMenuItem2;

		// Token: 0x040000D2 RID: 210
		[AccessedThroughProperty("cmsExpandAll")]
		private ToolStripMenuItem _cmsExpandAll;

		// Token: 0x040000D3 RID: 211
		[AccessedThroughProperty("cmsExpandNode")]
		private ToolStripMenuItem _cmsExpandNode;

		// Token: 0x040000D4 RID: 212
		[AccessedThroughProperty("miAbout")]
		private ToolStripMenuItem _miAbout;

		// Token: 0x040000D5 RID: 213
		[AccessedThroughProperty("tmrSendMessage")]
		private global::System.Windows.Forms.Timer _tmrSendMessage;

		// Token: 0x040000D6 RID: 214
		[AccessedThroughProperty("SplitContainer1")]
		private SplitContainer _SplitContainer1;

		// Token: 0x040000D7 RID: 215
		[AccessedThroughProperty("SplitContainer2")]
		private SplitContainer _SplitContainer2;

		// Token: 0x040000D8 RID: 216
		[AccessedThroughProperty("DataColumn4")]
		private DataColumn _DataColumn4;

		// Token: 0x040000D9 RID: 217
		[AccessedThroughProperty("DataColumn5")]
		private DataColumn _DataColumn5;

		// Token: 0x040000DA RID: 218
		[AccessedThroughProperty("DataColumn6")]
		private DataColumn _DataColumn6;

		// Token: 0x040000DB RID: 219
		[AccessedThroughProperty("DataColumn7")]
		private DataColumn _DataColumn7;

		// Token: 0x040000DC RID: 220
		[AccessedThroughProperty("NotifyIcon1")]
		private NotifyIcon _NotifyIcon1;

		// Token: 0x040000DD RID: 221
		[AccessedThroughProperty("DataColumn8")]
		private DataColumn _DataColumn8;

		// Token: 0x040000DE RID: 222
		[AccessedThroughProperty("DataColumn9")]
		private DataColumn _DataColumn9;

		// Token: 0x040000DF RID: 223
		[AccessedThroughProperty("DataColumn10")]
		private DataColumn _DataColumn10;

		// Token: 0x040000E0 RID: 224
		[AccessedThroughProperty("DataColumn11")]
		private DataColumn _DataColumn11;

		// Token: 0x040000E1 RID: 225
		[AccessedThroughProperty("DataColumn12")]
		private DataColumn _DataColumn12;

		// Token: 0x040000E2 RID: 226
		[AccessedThroughProperty("DataColumn13")]
		private DataColumn _DataColumn13;

		// Token: 0x040000E3 RID: 227
		private object messagesLock;

		// Token: 0x040000E4 RID: 228
		private const int WM_COPYDATA = 74;

		// Token: 0x02000010 RID: 16
		// (Invoke) Token: 0x060001F8 RID: 504
		public delegate void SetMonitorCallback(string sMsg);

		// Token: 0x02000011 RID: 17
		private struct CopyData
		{
			// Token: 0x040000E5 RID: 229
			public IntPtr dwData;

			// Token: 0x040000E6 RID: 230
			public int cbData;

			// Token: 0x040000E7 RID: 231
			public IntPtr lpData;
		}
	}
}
