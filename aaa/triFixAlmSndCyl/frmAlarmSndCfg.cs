using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using GeFanuc.iFixToolkit.Adapter;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Trendtek.iFix;

namespace triFixAlmSndCyl
{
	// Token: 0x0200000E RID: 14
	[DesignerGenerated]
	public partial class frmAlarmSndCfg : Form
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00006ABC File Offset: 0x00004EBC
		public frmAlarmSndCfg()
		{
			base.Resize += this.frmAlarmSndCfg_Resize;
			base.FormClosing += this.frmAlarmSndCfg_FormClosing;
			base.Load += this.frmAlarmSndCfg_Load;
			this._bFormInitDone = false;
			this._bFileReading = false;
			this._dsXML = new DataSet();
			this._igvdLines = 1;
			this.InitializeComponent();
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000051 RID: 81 RVA: 0x0000B084 File Offset: 0x00009484
		// (set) Token: 0x06000052 RID: 82 RVA: 0x0000B098 File Offset: 0x00009498
		internal virtual TabControl tabEnvironment
		{
			get
			{
				return this._tabEnvironment;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._tabEnvironment = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000053 RID: 83 RVA: 0x0000B0A4 File Offset: 0x000094A4
		// (set) Token: 0x06000054 RID: 84 RVA: 0x0000B0B8 File Offset: 0x000094B8
		internal virtual TabPage tabTAG
		{
			get
			{
				return this._tabTAG;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._tabTAG = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000055 RID: 85 RVA: 0x0000B0C4 File Offset: 0x000094C4
		// (set) Token: 0x06000056 RID: 86 RVA: 0x0000B0D8 File Offset: 0x000094D8
		internal virtual TabPage tabParameter
		{
			get
			{
				return this._tabParameter;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._tabParameter = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000057 RID: 87 RVA: 0x0000B0E4 File Offset: 0x000094E4
		// (set) Token: 0x06000058 RID: 88 RVA: 0x0000B0F8 File Offset: 0x000094F8
		internal virtual Button btnRemove
		{
			get
			{
				return this._btnRemove;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.btnRemove_Click);
				if (this._btnRemove != null)
				{
					this._btnRemove.Click -= eventHandler;
				}
				this._btnRemove = value;
				if (this._btnRemove != null)
				{
					this._btnRemove.Click += eventHandler;
				}
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000059 RID: 89 RVA: 0x0000B144 File Offset: 0x00009544
		// (set) Token: 0x0600005A RID: 90 RVA: 0x0000B158 File Offset: 0x00009558
		internal virtual Button btnAdd
		{
			get
			{
				return this._btnAdd;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.btnAdd_Click);
				if (this._btnAdd != null)
				{
					this._btnAdd.Click -= eventHandler;
				}
				this._btnAdd = value;
				if (this._btnAdd != null)
				{
					this._btnAdd.Click += eventHandler;
				}
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600005B RID: 91 RVA: 0x0000B1A4 File Offset: 0x000095A4
		// (set) Token: 0x0600005C RID: 92 RVA: 0x0000B1B8 File Offset: 0x000095B8
		internal virtual GroupBox gpFilter
		{
			get
			{
				return this._gpFilter;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._gpFilter = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600005D RID: 93 RVA: 0x0000B1C4 File Offset: 0x000095C4
		// (set) Token: 0x0600005E RID: 94 RVA: 0x0000B1D8 File Offset: 0x000095D8
		internal virtual TextBox txtContent
		{
			get
			{
				return this._txtContent;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._txtContent = value;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600005F RID: 95 RVA: 0x0000B1E4 File Offset: 0x000095E4
		// (set) Token: 0x06000060 RID: 96 RVA: 0x0000B1F8 File Offset: 0x000095F8
		internal virtual ComboBox cobOper
		{
			get
			{
				return this._cobOper;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._cobOper = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000061 RID: 97 RVA: 0x0000B204 File Offset: 0x00009604
		// (set) Token: 0x06000062 RID: 98 RVA: 0x0000B218 File Offset: 0x00009618
		internal virtual ComboBox cobColumn
		{
			get
			{
				return this._cobColumn;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._cobColumn = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000063 RID: 99 RVA: 0x0000B224 File Offset: 0x00009624
		// (set) Token: 0x06000064 RID: 100 RVA: 0x0000B238 File Offset: 0x00009638
		internal virtual Button btnApply
		{
			get
			{
				return this._btnApply;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.btnApply_Click);
				if (this._btnApply != null)
				{
					this._btnApply.Click -= eventHandler;
				}
				this._btnApply = value;
				if (this._btnApply != null)
				{
					this._btnApply.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000065 RID: 101 RVA: 0x0000B284 File Offset: 0x00009684
		// (set) Token: 0x06000066 RID: 102 RVA: 0x0000B298 File Offset: 0x00009698
		internal virtual Label lblRecordFiltered
		{
			get
			{
				return this._lblRecordFiltered;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._lblRecordFiltered = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000067 RID: 103 RVA: 0x0000B2A4 File Offset: 0x000096A4
		// (set) Token: 0x06000068 RID: 104 RVA: 0x0000B2B8 File Offset: 0x000096B8
		internal virtual DataGridView DataGrid1
		{
			get
			{
				return this._DataGrid1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				DataGridViewDataErrorEventHandler dataGridViewDataErrorEventHandler = new DataGridViewDataErrorEventHandler(this.DataGrid1_DataError);
				DataGridViewCellEventHandler dataGridViewCellEventHandler = new DataGridViewCellEventHandler(this.DataGrid1_CellClick);
				if (this._DataGrid1 != null)
				{
					this._DataGrid1.DataError -= dataGridViewDataErrorEventHandler;
					this._DataGrid1.CellClick -= dataGridViewCellEventHandler;
				}
				this._DataGrid1 = value;
				if (this._DataGrid1 != null)
				{
					this._DataGrid1.DataError += dataGridViewDataErrorEventHandler;
					this._DataGrid1.CellClick += dataGridViewCellEventHandler;
				}
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000069 RID: 105 RVA: 0x0000B328 File Offset: 0x00009728
		// (set) Token: 0x0600006A RID: 106 RVA: 0x0000B33C File Offset: 0x0000973C
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

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600006B RID: 107 RVA: 0x0000B348 File Offset: 0x00009748
		// (set) Token: 0x0600006C RID: 108 RVA: 0x0000B35C File Offset: 0x0000975C
		internal virtual GroupBox gpSound
		{
			get
			{
				return this._gpSound;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._gpSound = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600006D RID: 109 RVA: 0x0000B368 File Offset: 0x00009768
		// (set) Token: 0x0600006E RID: 110 RVA: 0x0000B37C File Offset: 0x0000977C
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

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600006F RID: 111 RVA: 0x0000B388 File Offset: 0x00009788
		// (set) Token: 0x06000070 RID: 112 RVA: 0x0000B39C File Offset: 0x0000979C
		internal virtual TextBox txtWavName
		{
			get
			{
				return this._txtWavName;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._txtWavName = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000071 RID: 113 RVA: 0x0000B3A8 File Offset: 0x000097A8
		// (set) Token: 0x06000072 RID: 114 RVA: 0x0000B3BC File Offset: 0x000097BC
		internal virtual NumericUpDown ndLevel
		{
			get
			{
				return this._ndLevel;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ndLevel = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000073 RID: 115 RVA: 0x0000B3C8 File Offset: 0x000097C8
		// (set) Token: 0x06000074 RID: 116 RVA: 0x0000B3DC File Offset: 0x000097DC
		internal virtual Button btnFile
		{
			get
			{
				return this._btnFile;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.btnFile_Click);
				if (this._btnFile != null)
				{
					this._btnFile.Click -= eventHandler;
				}
				this._btnFile = value;
				if (this._btnFile != null)
				{
					this._btnFile.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000075 RID: 117 RVA: 0x0000B428 File Offset: 0x00009828
		// (set) Token: 0x06000076 RID: 118 RVA: 0x0000B43C File Offset: 0x0000983C
		internal virtual GroupBox gpTagSource
		{
			get
			{
				return this._gpTagSource;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._gpTagSource = value;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000077 RID: 119 RVA: 0x0000B448 File Offset: 0x00009848
		// (set) Token: 0x06000078 RID: 120 RVA: 0x0000B45C File Offset: 0x0000985C
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

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000079 RID: 121 RVA: 0x0000B468 File Offset: 0x00009868
		// (set) Token: 0x0600007A RID: 122 RVA: 0x0000B47C File Offset: 0x0000987C
		internal virtual ComboBox cobType
		{
			get
			{
				return this._cobType;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.txtTagFilter_LostFocus);
				if (this._cobType != null)
				{
					this._cobType.SelectedIndexChanged -= eventHandler;
				}
				this._cobType = value;
				if (this._cobType != null)
				{
					this._cobType.SelectedIndexChanged += eventHandler;
				}
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600007B RID: 123 RVA: 0x0000B4C8 File Offset: 0x000098C8
		// (set) Token: 0x0600007C RID: 124 RVA: 0x0000B4DC File Offset: 0x000098DC
		internal virtual Label lblType
		{
			get
			{
				return this._lblType;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._lblType = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600007D RID: 125 RVA: 0x0000B4E8 File Offset: 0x000098E8
		// (set) Token: 0x0600007E RID: 126 RVA: 0x0000B4FC File Offset: 0x000098FC
		internal virtual ComboBox cobArea
		{
			get
			{
				return this._cobArea;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.cobArea_SelectedIndexChanged);
				if (this._cobArea != null)
				{
					this._cobArea.SelectedIndexChanged -= eventHandler;
				}
				this._cobArea = value;
				if (this._cobArea != null)
				{
					this._cobArea.SelectedIndexChanged += eventHandler;
				}
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600007F RID: 127 RVA: 0x0000B548 File Offset: 0x00009948
		// (set) Token: 0x06000080 RID: 128 RVA: 0x0000B55C File Offset: 0x0000995C
		internal virtual Label lblArea
		{
			get
			{
				return this._lblArea;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._lblArea = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000081 RID: 129 RVA: 0x0000B568 File Offset: 0x00009968
		// (set) Token: 0x06000082 RID: 130 RVA: 0x0000B57C File Offset: 0x0000997C
		internal virtual TextBox txtTagFilter
		{
			get
			{
				return this._txtTagFilter;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.txtTagFilter_LostFocus);
				if (this._txtTagFilter != null)
				{
					this._txtTagFilter.LostFocus -= eventHandler;
				}
				this._txtTagFilter = value;
				if (this._txtTagFilter != null)
				{
					this._txtTagFilter.LostFocus += eventHandler;
				}
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000083 RID: 131 RVA: 0x0000B5C8 File Offset: 0x000099C8
		// (set) Token: 0x06000084 RID: 132 RVA: 0x0000B5DC File Offset: 0x000099DC
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

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000085 RID: 133 RVA: 0x0000B5E8 File Offset: 0x000099E8
		// (set) Token: 0x06000086 RID: 134 RVA: 0x0000B5FC File Offset: 0x000099FC
		internal virtual ComboBox cobNode
		{
			get
			{
				return this._cobNode;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.cobNode_SelectedIndexChanged);
				if (this._cobNode != null)
				{
					this._cobNode.SelectedIndexChanged -= eventHandler;
				}
				this._cobNode = value;
				if (this._cobNode != null)
				{
					this._cobNode.SelectedIndexChanged += eventHandler;
				}
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000087 RID: 135 RVA: 0x0000B648 File Offset: 0x00009A48
		// (set) Token: 0x06000088 RID: 136 RVA: 0x0000B65C File Offset: 0x00009A5C
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

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000089 RID: 137 RVA: 0x0000B668 File Offset: 0x00009A68
		// (set) Token: 0x0600008A RID: 138 RVA: 0x0000B67C File Offset: 0x00009A7C
		internal virtual Label lblTagList
		{
			get
			{
				return this._lblTagList;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._lblTagList = value;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600008B RID: 139 RVA: 0x0000B688 File Offset: 0x00009A88
		// (set) Token: 0x0600008C RID: 140 RVA: 0x0000B69C File Offset: 0x00009A9C
		protected virtual TextBox txtNTF
		{
			get
			{
				return this._txtNTF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._txtNTF = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600008D RID: 141 RVA: 0x0000B6A8 File Offset: 0x00009AA8
		// (set) Token: 0x0600008E RID: 142 RVA: 0x0000B6BC File Offset: 0x00009ABC
		protected virtual Button btnEnterMode
		{
			get
			{
				return this._btnEnterMode;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.btnEnterMode_Click);
				if (this._btnEnterMode != null)
				{
					this._btnEnterMode.Click -= eventHandler;
				}
				this._btnEnterMode = value;
				if (this._btnEnterMode != null)
				{
					this._btnEnterMode.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600008F RID: 143 RVA: 0x0000B708 File Offset: 0x00009B08
		// (set) Token: 0x06000090 RID: 144 RVA: 0x0000B71C File Offset: 0x00009B1C
		internal virtual Label lblDate
		{
			get
			{
				return this._lblDate;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._lblDate = value;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000091 RID: 145 RVA: 0x0000B728 File Offset: 0x00009B28
		// (set) Token: 0x06000092 RID: 146 RVA: 0x0000B73C File Offset: 0x00009B3C
		internal virtual Button btnUpdate
		{
			get
			{
				return this._btnUpdate;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.btnUpdate_Click);
				if (this._btnUpdate != null)
				{
					this._btnUpdate.Click -= eventHandler;
				}
				this._btnUpdate = value;
				if (this._btnUpdate != null)
				{
					this._btnUpdate.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000093 RID: 147 RVA: 0x0000B788 File Offset: 0x00009B88
		// (set) Token: 0x06000094 RID: 148 RVA: 0x0000B79C File Offset: 0x00009B9C
		internal virtual ListBox comNTF
		{
			get
			{
				return this._comNTF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._comNTF = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000095 RID: 149 RVA: 0x0000B7A8 File Offset: 0x00009BA8
		// (set) Token: 0x06000096 RID: 150 RVA: 0x0000B7BC File Offset: 0x00009BBC
		internal virtual GroupBox GroupBox6
		{
			get
			{
				return this._GroupBox6;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._GroupBox6 = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000097 RID: 151 RVA: 0x0000B7C8 File Offset: 0x00009BC8
		// (set) Token: 0x06000098 RID: 152 RVA: 0x0000B7DC File Offset: 0x00009BDC
		internal virtual Label Label6
		{
			get
			{
				return this._Label6;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label6 = value;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000099 RID: 153 RVA: 0x0000B7E8 File Offset: 0x00009BE8
		// (set) Token: 0x0600009A RID: 154 RVA: 0x0000B7FC File Offset: 0x00009BFC
		internal virtual Label Label7
		{
			get
			{
				return this._Label7;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label7 = value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600009B RID: 155 RVA: 0x0000B808 File Offset: 0x00009C08
		// (set) Token: 0x0600009C RID: 156 RVA: 0x0000B81C File Offset: 0x00009C1C
		internal virtual NumericUpDown ndSoundTime
		{
			get
			{
				return this._ndSoundTime;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ndSoundTime = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600009D RID: 157 RVA: 0x0000B828 File Offset: 0x00009C28
		// (set) Token: 0x0600009E RID: 158 RVA: 0x0000B83C File Offset: 0x00009C3C
		internal virtual TextBox txtCfg
		{
			get
			{
				return this._txtCfg;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._txtCfg = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600009F RID: 159 RVA: 0x0000B848 File Offset: 0x00009C48
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x0000B85C File Offset: 0x00009C5C
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

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x0000B868 File Offset: 0x00009C68
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x0000B87C File Offset: 0x00009C7C
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

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000B888 File Offset: 0x00009C88
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x0000B89C File Offset: 0x00009C9C
		internal virtual GroupBox GroupBox7
		{
			get
			{
				return this._GroupBox7;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._GroupBox7 = value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x0000B8A8 File Offset: 0x00009CA8
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x0000B8BC File Offset: 0x00009CBC
		internal virtual CheckBox ckDisBar
		{
			get
			{
				return this._ckDisBar;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ckDisBar = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x0000B8C8 File Offset: 0x00009CC8
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x0000B8DC File Offset: 0x00009CDC
		internal virtual CheckBox ckBackGround
		{
			get
			{
				return this._ckBackGround;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ckBackGround = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x0000B8E8 File Offset: 0x00009CE8
		// (set) Token: 0x060000AA RID: 170 RVA: 0x0000B8FC File Offset: 0x00009CFC
		internal virtual GroupBox GroupBox8
		{
			get
			{
				return this._GroupBox8;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._GroupBox8 = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000AB RID: 171 RVA: 0x0000B908 File Offset: 0x00009D08
		// (set) Token: 0x060000AC RID: 172 RVA: 0x0000B91C File Offset: 0x00009D1C
		internal virtual DataSet _dsCFG
		{
			get
			{
				return this.__dsCFG;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this.__dsCFG = value;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000AD RID: 173 RVA: 0x0000B928 File Offset: 0x00009D28
		// (set) Token: 0x060000AE RID: 174 RVA: 0x0000B93C File Offset: 0x00009D3C
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

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000AF RID: 175 RVA: 0x0000B948 File Offset: 0x00009D48
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x0000B95C File Offset: 0x00009D5C
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x0000B968 File Offset: 0x00009D68
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x0000B97C File Offset: 0x00009D7C
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

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x0000B988 File Offset: 0x00009D88
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x0000B99C File Offset: 0x00009D9C
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

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x0000B9A8 File Offset: 0x00009DA8
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x0000B9BC File Offset: 0x00009DBC
		internal virtual Timer Timer1
		{
			get
			{
				return this._Timer1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.Timer1_Tick);
				if (this._Timer1 != null)
				{
					this._Timer1.Tick -= eventHandler;
				}
				this._Timer1 = value;
				if (this._Timer1 != null)
				{
					this._Timer1.Tick += eventHandler;
				}
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x0000BA08 File Offset: 0x00009E08
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x0000BA1C File Offset: 0x00009E1C
		internal virtual OpenFileDialog dlgOpen
		{
			get
			{
				return this._dlgOpen;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._dlgOpen = value;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x0000BA28 File Offset: 0x00009E28
		// (set) Token: 0x060000BA RID: 186 RVA: 0x0000BA3C File Offset: 0x00009E3C
		internal virtual Timer tmrStatus
		{
			get
			{
				return this._tmrStatus;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.tmrStatus_Tick);
				if (this._tmrStatus != null)
				{
					this._tmrStatus.Tick -= eventHandler;
				}
				this._tmrStatus = value;
				if (this._tmrStatus != null)
				{
					this._tmrStatus.Tick += eventHandler;
				}
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000BB RID: 187 RVA: 0x0000BA88 File Offset: 0x00009E88
		// (set) Token: 0x060000BC RID: 188 RVA: 0x0000BA9C File Offset: 0x00009E9C
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

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000BD RID: 189 RVA: 0x0000BAA8 File Offset: 0x00009EA8
		// (set) Token: 0x060000BE RID: 190 RVA: 0x0000BABC File Offset: 0x00009EBC
		internal virtual StatusBarPanel panelTime
		{
			get
			{
				return this._panelTime;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._panelTime = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000BF RID: 191 RVA: 0x0000BAC8 File Offset: 0x00009EC8
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x0000BADC File Offset: 0x00009EDC
		internal virtual StatusBarPanel panelStatus
		{
			get
			{
				return this._panelStatus;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._panelStatus = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x0000BAE8 File Offset: 0x00009EE8
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x0000BAFC File Offset: 0x00009EFC
		internal virtual StatusBarPanel panelFileName
		{
			get
			{
				return this._panelFileName;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._panelFileName = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x0000BB08 File Offset: 0x00009F08
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x0000BB1C File Offset: 0x00009F1C
		internal virtual StatusBar kvStatusBar
		{
			get
			{
				return this._kvStatusBar;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._kvStatusBar = value;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x0000BB28 File Offset: 0x00009F28
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x0000BB3C File Offset: 0x00009F3C
		internal virtual Button btnCancle
		{
			get
			{
				return this._btnCancle;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.btnCancle_Click);
				if (this._btnCancle != null)
				{
					this._btnCancle.Click -= eventHandler;
				}
				this._btnCancle = value;
				if (this._btnCancle != null)
				{
					this._btnCancle.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x0000BB88 File Offset: 0x00009F88
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x0000BB9C File Offset: 0x00009F9C
		internal virtual Button btnOK
		{
			get
			{
				return this._btnOK;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.btnOK_Click);
				if (this._btnOK != null)
				{
					this._btnOK.Click -= eventHandler;
				}
				this._btnOK = value;
				if (this._btnOK != null)
				{
					this._btnOK.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x0000BBE8 File Offset: 0x00009FE8
		// (set) Token: 0x060000CA RID: 202 RVA: 0x0000BBFC File Offset: 0x00009FFC
		internal virtual ContextMenuStrip ContextMenuStrip_Grid
		{
			get
			{
				return this._ContextMenuStrip_Grid;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ContextMenuStrip_Grid = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000CB RID: 203 RVA: 0x0000BC08 File Offset: 0x0000A008
		// (set) Token: 0x060000CC RID: 204 RVA: 0x0000BC1C File Offset: 0x0000A01C
		internal virtual ToolStripMenuItem miModifyAll
		{
			get
			{
				return this._miModifyAll;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.miModifyAll_Click);
				if (this._miModifyAll != null)
				{
					this._miModifyAll.Click -= eventHandler;
				}
				this._miModifyAll = value;
				if (this._miModifyAll != null)
				{
					this._miModifyAll.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000CD RID: 205 RVA: 0x0000BC68 File Offset: 0x0000A068
		// (set) Token: 0x060000CE RID: 206 RVA: 0x0000BC7C File Offset: 0x0000A07C
		internal virtual ToolStripMenuItem miModify
		{
			get
			{
				return this._miModify;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.miModify_Click);
				if (this._miModify != null)
				{
					this._miModify.Click -= eventHandler;
				}
				this._miModify = value;
				if (this._miModify != null)
				{
					this._miModify.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000CF RID: 207 RVA: 0x0000BCC8 File Offset: 0x0000A0C8
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x0000BCDC File Offset: 0x0000A0DC
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

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x0000BCE8 File Offset: 0x0000A0E8
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x0000BCFC File Offset: 0x0000A0FC
		internal virtual ToolStripMenuItem miDelAll
		{
			get
			{
				return this._miDelAll;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.miDelAll_Click);
				if (this._miDelAll != null)
				{
					this._miDelAll.Click -= eventHandler;
				}
				this._miDelAll = value;
				if (this._miDelAll != null)
				{
					this._miDelAll.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x0000BD48 File Offset: 0x0000A148
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x0000BD5C File Offset: 0x0000A15C
		internal virtual ToolStripMenuItem miDel
		{
			get
			{
				return this._miDel;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.miDel_Click);
				if (this._miDel != null)
				{
					this._miDel.Click -= eventHandler;
				}
				this._miDel = value;
				if (this._miDel != null)
				{
					this._miDel.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x0000BDA8 File Offset: 0x0000A1A8
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x0000BDBC File Offset: 0x0000A1BC
		internal virtual MenuStrip MainMenu1
		{
			get
			{
				return this._MainMenu1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._MainMenu1 = value;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x0000BDC8 File Offset: 0x0000A1C8
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x0000BDDC File Offset: 0x0000A1DC
		internal virtual ToolStripMenuItem miFile
		{
			get
			{
				return this._miFile;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._miFile = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x0000BDE8 File Offset: 0x0000A1E8
		// (set) Token: 0x060000DA RID: 218 RVA: 0x0000BDFC File Offset: 0x0000A1FC
		internal virtual ToolStripMenuItem miNew
		{
			get
			{
				return this._miNew;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.miNew_Click);
				if (this._miNew != null)
				{
					this._miNew.Click -= eventHandler;
				}
				this._miNew = value;
				if (this._miNew != null)
				{
					this._miNew.Click += eventHandler;
				}
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000DB RID: 219 RVA: 0x0000BE48 File Offset: 0x0000A248
		// (set) Token: 0x060000DC RID: 220 RVA: 0x0000BE5C File Offset: 0x0000A25C
		internal virtual ToolStripMenuItem miOpen
		{
			get
			{
				return this._miOpen;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.miOpen_Click);
				if (this._miOpen != null)
				{
					this._miOpen.Click -= eventHandler;
				}
				this._miOpen = value;
				if (this._miOpen != null)
				{
					this._miOpen.Click += eventHandler;
				}
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000DD RID: 221 RVA: 0x0000BEA8 File Offset: 0x0000A2A8
		// (set) Token: 0x060000DE RID: 222 RVA: 0x0000BEBC File Offset: 0x0000A2BC
		internal virtual ToolStripSeparator toolStripSeparator
		{
			get
			{
				return this._toolStripSeparator;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._toolStripSeparator = value;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000DF RID: 223 RVA: 0x0000BEC8 File Offset: 0x0000A2C8
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x0000BEDC File Offset: 0x0000A2DC
		internal virtual ToolStripMenuItem miSave
		{
			get
			{
				return this._miSave;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.miSave_Click);
				if (this._miSave != null)
				{
					this._miSave.Click -= eventHandler;
				}
				this._miSave = value;
				if (this._miSave != null)
				{
					this._miSave.Click += eventHandler;
				}
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x0000BF28 File Offset: 0x0000A328
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x0000BF3C File Offset: 0x0000A33C
		internal virtual ToolStripMenuItem miSaveAs
		{
			get
			{
				return this._miSaveAs;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.miSaveAs_Click);
				if (this._miSaveAs != null)
				{
					this._miSaveAs.Click -= eventHandler;
				}
				this._miSaveAs = value;
				if (this._miSaveAs != null)
				{
					this._miSaveAs.Click += eventHandler;
				}
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x0000BF88 File Offset: 0x0000A388
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x0000BF9C File Offset: 0x0000A39C
		internal virtual ToolStripSeparator toolStripSeparator1
		{
			get
			{
				return this._toolStripSeparator1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._toolStripSeparator1 = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x0000BFA8 File Offset: 0x0000A3A8
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x0000BFBC File Offset: 0x0000A3BC
		internal virtual ToolStripSeparator toolStripSeparator2
		{
			get
			{
				return this._toolStripSeparator2;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._toolStripSeparator2 = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x0000BFC8 File Offset: 0x0000A3C8
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x0000BFDC File Offset: 0x0000A3DC
		internal virtual ToolStripMenuItem miRecent
		{
			get
			{
				return this._miRecent;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._miRecent = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x0000BFE8 File Offset: 0x0000A3E8
		// (set) Token: 0x060000EA RID: 234 RVA: 0x0000BFFC File Offset: 0x0000A3FC
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

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000EB RID: 235 RVA: 0x0000C008 File Offset: 0x0000A408
		// (set) Token: 0x060000EC RID: 236 RVA: 0x0000C01C File Offset: 0x0000A41C
		internal virtual ToolStripMenuItem miDelCache
		{
			get
			{
				return this._miDelCache;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.miDelCache_Click);
				if (this._miDelCache != null)
				{
					this._miDelCache.Click -= eventHandler;
				}
				this._miDelCache = value;
				if (this._miDelCache != null)
				{
					this._miDelCache.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000ED RID: 237 RVA: 0x0000C068 File Offset: 0x0000A468
		// (set) Token: 0x060000EE RID: 238 RVA: 0x0000C07C File Offset: 0x0000A47C
		internal virtual ToolStripMenuItem miRecent1
		{
			get
			{
				return this._miRecent1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.miRecent1_Click);
				if (this._miRecent1 != null)
				{
					this._miRecent1.Click -= eventHandler;
				}
				this._miRecent1 = value;
				if (this._miRecent1 != null)
				{
					this._miRecent1.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000EF RID: 239 RVA: 0x0000C0C8 File Offset: 0x0000A4C8
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x0000C0DC File Offset: 0x0000A4DC
		internal virtual ToolStripMenuItem miRecent2
		{
			get
			{
				return this._miRecent2;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.miRecent1_Click);
				if (this._miRecent2 != null)
				{
					this._miRecent2.Click -= eventHandler;
				}
				this._miRecent2 = value;
				if (this._miRecent2 != null)
				{
					this._miRecent2.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x0000C128 File Offset: 0x0000A528
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x0000C13C File Offset: 0x0000A53C
		internal virtual ToolStripMenuItem miRecent3
		{
			get
			{
				return this._miRecent3;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.miRecent1_Click);
				if (this._miRecent3 != null)
				{
					this._miRecent3.Click -= eventHandler;
				}
				this._miRecent3 = value;
				if (this._miRecent3 != null)
				{
					this._miRecent3.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x0000C188 File Offset: 0x0000A588
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x0000C19C File Offset: 0x0000A59C
		internal virtual ToolStripMenuItem miRecent4
		{
			get
			{
				return this._miRecent4;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.miRecent1_Click);
				if (this._miRecent4 != null)
				{
					this._miRecent4.Click -= eventHandler;
				}
				this._miRecent4 = value;
				if (this._miRecent4 != null)
				{
					this._miRecent4.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x0000C1E8 File Offset: 0x0000A5E8
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x0000C1FC File Offset: 0x0000A5FC
		internal virtual ToolStripMenuItem miRun
		{
			get
			{
				return this._miRun;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.miRun_Click);
				if (this._miRun != null)
				{
					this._miRun.Click -= eventHandler;
				}
				this._miRun = value;
				if (this._miRun != null)
				{
					this._miRun.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x0000C248 File Offset: 0x0000A648
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x0000C25C File Offset: 0x0000A65C
		internal virtual ToolStripMenuItem miReloadDB
		{
			get
			{
				return this._miReloadDB;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.miReloadDB_Click);
				if (this._miReloadDB != null)
				{
					this._miReloadDB.Click -= eventHandler;
				}
				this._miReloadDB = value;
				if (this._miReloadDB != null)
				{
					this._miReloadDB.Click += eventHandler;
				}
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x0000C2A8 File Offset: 0x0000A6A8
		// (set) Token: 0x060000FA RID: 250 RVA: 0x0000C2BC File Offset: 0x0000A6BC
		internal virtual NumericUpDown ndScanTime
		{
			get
			{
				return this._ndScanTime;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ndScanTime = value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000FB RID: 251 RVA: 0x0000C2C8 File Offset: 0x0000A6C8
		// (set) Token: 0x060000FC RID: 252 RVA: 0x0000C2DC File Offset: 0x0000A6DC
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

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000FD RID: 253 RVA: 0x0000C2E8 File Offset: 0x0000A6E8
		// (set) Token: 0x060000FE RID: 254 RVA: 0x0000C2FC File Offset: 0x0000A6FC
		internal virtual NumericUpDown ndSecond
		{
			get
			{
				return this._ndSecond;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ndSecond = value;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000FF RID: 255 RVA: 0x0000C308 File Offset: 0x0000A708
		// (set) Token: 0x06000100 RID: 256 RVA: 0x0000C31C File Offset: 0x0000A71C
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

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000101 RID: 257 RVA: 0x0000C328 File Offset: 0x0000A728
		// (set) Token: 0x06000102 RID: 258 RVA: 0x0000C33C File Offset: 0x0000A73C
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

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000103 RID: 259 RVA: 0x0000C348 File Offset: 0x0000A748
		// (set) Token: 0x06000104 RID: 260 RVA: 0x0000C35C File Offset: 0x0000A75C
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

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000105 RID: 261 RVA: 0x0000C368 File Offset: 0x0000A768
		// (set) Token: 0x06000106 RID: 262 RVA: 0x0000C37C File Offset: 0x0000A77C
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

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000107 RID: 263 RVA: 0x0000C388 File Offset: 0x0000A788
		// (set) Token: 0x06000108 RID: 264 RVA: 0x0000C39C File Offset: 0x0000A79C
		internal virtual Label Label5
		{
			get
			{
				return this._Label5;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label5 = value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000109 RID: 265 RVA: 0x0000C3A8 File Offset: 0x0000A7A8
		// (set) Token: 0x0600010A RID: 266 RVA: 0x0000C3BC File Offset: 0x0000A7BC
		internal virtual ComboBox cobPriority
		{
			get
			{
				return this._cobPriority;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._cobPriority = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600010B RID: 267 RVA: 0x0000C3C8 File Offset: 0x0000A7C8
		// (set) Token: 0x0600010C RID: 268 RVA: 0x0000C3DC File Offset: 0x0000A7DC
		internal virtual GroupBox gpPicture
		{
			get
			{
				return this._gpPicture;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._gpPicture = value;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600010D RID: 269 RVA: 0x0000C3E8 File Offset: 0x0000A7E8
		// (set) Token: 0x0600010E RID: 270 RVA: 0x0000C3FC File Offset: 0x0000A7FC
		internal virtual TextBox txtPicName
		{
			get
			{
				return this._txtPicName;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._txtPicName = value;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600010F RID: 271 RVA: 0x0000C408 File Offset: 0x0000A808
		// (set) Token: 0x06000110 RID: 272 RVA: 0x0000C41C File Offset: 0x0000A81C
		internal virtual Button btnPic
		{
			get
			{
				return this._btnPic;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.btnPic_Click);
				if (this._btnPic != null)
				{
					this._btnPic.Click -= eventHandler;
				}
				this._btnPic = value;
				if (this._btnPic != null)
				{
					this._btnPic.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000111 RID: 273 RVA: 0x0000C468 File Offset: 0x0000A868
		// (set) Token: 0x06000112 RID: 274 RVA: 0x0000C47C File Offset: 0x0000A87C
		internal virtual GroupBox gpOutSource
		{
			get
			{
				return this._gpOutSource;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._gpOutSource = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000113 RID: 275 RVA: 0x0000C488 File Offset: 0x0000A888
		// (set) Token: 0x06000114 RID: 276 RVA: 0x0000C49C File Offset: 0x0000A89C
		internal virtual TextBox txtValue
		{
			get
			{
				return this._txtValue;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._txtValue = value;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000115 RID: 277 RVA: 0x0000C4A8 File Offset: 0x0000A8A8
		// (set) Token: 0x06000116 RID: 278 RVA: 0x0000C4BC File Offset: 0x0000A8BC
		internal virtual TextBox txtSource
		{
			get
			{
				return this._txtSource;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._txtSource = value;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000117 RID: 279 RVA: 0x0000C4C8 File Offset: 0x0000A8C8
		// (set) Token: 0x06000118 RID: 280 RVA: 0x0000C4DC File Offset: 0x0000A8DC
		internal virtual GroupBox gpAction
		{
			get
			{
				return this._gpAction;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._gpAction = value;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000119 RID: 281 RVA: 0x0000C4E8 File Offset: 0x0000A8E8
		// (set) Token: 0x0600011A RID: 282 RVA: 0x0000C4FC File Offset: 0x0000A8FC
		internal virtual Label Label11
		{
			get
			{
				return this._Label11;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label11 = value;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600011B RID: 283 RVA: 0x0000C508 File Offset: 0x0000A908
		// (set) Token: 0x0600011C RID: 284 RVA: 0x0000C51C File Offset: 0x0000A91C
		internal virtual Label Label8
		{
			get
			{
				return this._Label8;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label8 = value;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600011D RID: 285 RVA: 0x0000C528 File Offset: 0x0000A928
		// (set) Token: 0x0600011E RID: 286 RVA: 0x0000C53C File Offset: 0x0000A93C
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

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600011F RID: 287 RVA: 0x0000C548 File Offset: 0x0000A948
		// (set) Token: 0x06000120 RID: 288 RVA: 0x0000C55C File Offset: 0x0000A95C
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

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000121 RID: 289 RVA: 0x0000C568 File Offset: 0x0000A968
		// (set) Token: 0x06000122 RID: 290 RVA: 0x0000C57C File Offset: 0x0000A97C
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

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000123 RID: 291 RVA: 0x0000C588 File Offset: 0x0000A988
		// (set) Token: 0x06000124 RID: 292 RVA: 0x0000C59C File Offset: 0x0000A99C
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

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000125 RID: 293 RVA: 0x0000C5A8 File Offset: 0x0000A9A8
		// (set) Token: 0x06000126 RID: 294 RVA: 0x0000C5BC File Offset: 0x0000A9BC
		internal virtual DataGridViewTextBoxColumn NodeDataGridViewTextBoxColumn
		{
			get
			{
				return this._NodeDataGridViewTextBoxColumn;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._NodeDataGridViewTextBoxColumn = value;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000127 RID: 295 RVA: 0x0000C5C8 File Offset: 0x0000A9C8
		// (set) Token: 0x06000128 RID: 296 RVA: 0x0000C5DC File Offset: 0x0000A9DC
		internal virtual DataGridViewTextBoxColumn TagDataGridViewTextBoxColumn
		{
			get
			{
				return this._TagDataGridViewTextBoxColumn;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TagDataGridViewTextBoxColumn = value;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000129 RID: 297 RVA: 0x0000C5E8 File Offset: 0x0000A9E8
		// (set) Token: 0x0600012A RID: 298 RVA: 0x0000C5FC File Offset: 0x0000A9FC
		internal virtual DataGridViewTextBoxColumn clField
		{
			get
			{
				return this._clField;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._clField = value;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600012B RID: 299 RVA: 0x0000C608 File Offset: 0x0000AA08
		// (set) Token: 0x0600012C RID: 300 RVA: 0x0000C61C File Offset: 0x0000AA1C
		internal virtual DataGridViewTextBoxColumn clWave
		{
			get
			{
				return this._clWave;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._clWave = value;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600012D RID: 301 RVA: 0x0000C628 File Offset: 0x0000AA28
		// (set) Token: 0x0600012E RID: 302 RVA: 0x0000C63C File Offset: 0x0000AA3C
		internal virtual DataGridViewButtonColumn clnOpenWave
		{
			get
			{
				return this._clnOpenWave;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._clnOpenWave = value;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600012F RID: 303 RVA: 0x0000C648 File Offset: 0x0000AA48
		// (set) Token: 0x06000130 RID: 304 RVA: 0x0000C65C File Offset: 0x0000AA5C
		internal virtual DataGridViewComboBoxColumn clPriority
		{
			get
			{
				return this._clPriority;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._clPriority = value;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000131 RID: 305 RVA: 0x0000C668 File Offset: 0x0000AA68
		// (set) Token: 0x06000132 RID: 306 RVA: 0x0000C67C File Offset: 0x0000AA7C
		internal virtual DataGridViewTextBoxColumn clBlockType
		{
			get
			{
				return this._clBlockType;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._clBlockType = value;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000133 RID: 307 RVA: 0x0000C688 File Offset: 0x0000AA88
		// (set) Token: 0x06000134 RID: 308 RVA: 0x0000C69C File Offset: 0x0000AA9C
		internal virtual DataGridViewComboBoxColumn clLength
		{
			get
			{
				return this._clLength;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._clLength = value;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000135 RID: 309 RVA: 0x0000C6A8 File Offset: 0x0000AAA8
		// (set) Token: 0x06000136 RID: 310 RVA: 0x0000C6BC File Offset: 0x0000AABC
		internal virtual DataGridViewTextBoxColumn clPicture
		{
			get
			{
				return this._clPicture;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._clPicture = value;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000137 RID: 311 RVA: 0x0000C6C8 File Offset: 0x0000AAC8
		// (set) Token: 0x06000138 RID: 312 RVA: 0x0000C6DC File Offset: 0x0000AADC
		internal virtual DataGridViewButtonColumn clnOpenPic
		{
			get
			{
				return this._clnOpenPic;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._clnOpenPic = value;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000139 RID: 313 RVA: 0x0000C6E8 File Offset: 0x0000AAE8
		// (set) Token: 0x0600013A RID: 314 RVA: 0x0000C6FC File Offset: 0x0000AAFC
		internal virtual DataGridViewTextBoxColumn clOutSource
		{
			get
			{
				return this._clOutSource;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._clOutSource = value;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600013B RID: 315 RVA: 0x0000C708 File Offset: 0x0000AB08
		// (set) Token: 0x0600013C RID: 316 RVA: 0x0000C71C File Offset: 0x0000AB1C
		internal virtual DataGridViewTextBoxColumn clOutValue
		{
			get
			{
				return this._clOutValue;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._clOutValue = value;
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000C728 File Offset: 0x0000AB28
		private void miNew_Click(object sender, EventArgs e)
		{
			this.NewFile();
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000C730 File Offset: 0x0000AB30
		private void miOpen_Click(object sender, EventArgs e)
		{
			this.OpenFile();
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000C738 File Offset: 0x0000AB38
		private void miSave_Click(object sender, EventArgs e)
		{
			this.SaveFile();
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000C740 File Offset: 0x0000AB40
		private void miSaveAs_Click(object sender, EventArgs e)
		{
			this.SaveAs();
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000C748 File Offset: 0x0000AB48
		private void miRecent1_Click(object sender, EventArgs e)
		{
			if (Operators.CompareString(((ToolStripMenuItem)sender).Text, ".", false) != 0)
			{
				this.OpenRecentFile(((ToolStripMenuItem)sender).Text);
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000C774 File Offset: 0x0000AB74
		private void miDelCache_Click(object sender, EventArgs e)
		{
			try
			{
				frmDelCache frmDelCache = new frmDelCache();
				this.Enabled = false;
				frmDelCache.ShowDialog();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "刪除Cache檔案", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			finally
			{
				this.ShowInTaskbar = true;
				this.Enabled = true;
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000C7E4 File Offset: 0x0000ABE4
		private void miRun_Click(object sender, EventArgs e)
		{
			try
			{
				if (this._dsCFG.HasChanges() && !this._dsCFG.HasErrors)
				{
					this._dsCFG.AcceptChanges();
					if (Operators.CompareString(this.kvStatusBar.Panels[0].Text, modpublic.g_sCfgName, false) == 0)
					{
						DialogResult dialogResult = MessageBox.Show("檔案已經變更\r是否要存檔與Reload資料庫?", "Reload資料庫", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
						if (dialogResult == DialogResult.Yes)
						{
							string text;
							if (this._dtConfig.Rows.Count < 1)
							{
								text = "沒有任何一筆資料, 無法存檔";
								throw new Exception(text);
							}
							this.SaveFile();
							modpublic.frmRun.ReloadDBOnlyForConfig(modpublic.g_sCfgName);
							text = Strings.Format(DateAndTime.Now, "yyyy/MM/dd HH:mm:ss") + ": 載入資料庫 " + modpublic.g_sCfgName;
							modpublic.frmRun.Callback_Result(text);
						}
					}
				}
				this.Hide();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "切換至執行", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000C8F8 File Offset: 0x0000ACF8
		private void miReloadDB_Click(object sender, EventArgs e)
		{
			try
			{
				this.subChkIfChanged();
				string text = "[Reload資料庫] 將會先停止現有的警報播音, 並載入新的設定檔\r然後以新的設定執行警報播音功能\r\r確定, 請按[是],     取消, 請按[否]";
				int num = (int)MessageBox.Show(text, "Reload資料庫", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (num == 6)
				{
					this.dlgOpen.Filter = "*.tac|*.tac";
					if (this._myFile == null)
					{
						this.dlgOpen.FileName = "Default.tac";
					}
					else
					{
						this.dlgOpen.FileName = this._myFile.FullName;
					}
					this.dlgOpen.ShowReadOnly = true;
					if (this.dlgOpen.ShowDialog() == DialogResult.OK)
					{
						FileInfo fileInfo = new FileInfo(this.dlgOpen.FileName);
						modpublic.frmRun.ReloadDBOnlyForConfig(fileInfo.FullName);
						modpublic.g_sCfgName = fileInfo.FullName;
						text = Strings.Format(DateAndTime.Now, "yyyy/MM/dd HH:mm:ss") + ": 載入資料庫 " + fileInfo.FullName;
						modpublic.frmRun.Callback_Result(text);
					}
				}
			}
			catch (Exception ex)
			{
				string text = ex.Message + "\r無法Reload資料庫";
				MessageBox.Show(text, "Reload資料庫", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000CA24 File Offset: 0x0000AE24
		private void miDel_Click(object sender, EventArgs e)
		{
			if (this.DataGrid1.SelectedRows.Count < 1)
			{
				return;
			}
			int num = (int)MessageBox.Show("你確定要刪除此 " + Conversions.ToString(this.DataGrid1.SelectedRows.Count) + " 筆資料?", "刪除tag", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
			if (num != 6)
			{
				return;
			}
			int num2 = 0;
			checked
			{
				int num3 = this.DataGrid1.SelectedRows.Count - 1;
				for (int i = num2; i <= num3; i++)
				{
					this.DataGrid1.Rows.Remove(this.DataGrid1.SelectedRows[0]);
				}
				if (this._dsCFG.HasChanges() && !this._dsCFG.HasErrors)
				{
					this._dsCFG.AcceptChanges();
					this._bDocChanged = true;
				}
				this.lblRecordFiltered.Text = string.Concat(new string[]
				{
					"總共",
					Conversions.ToString(this._dtConfig.Rows.Count),
					"筆, 篩選後剩下",
					Conversions.ToString(this.DataGrid1.RowCount),
					"筆"
				});
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000CB44 File Offset: 0x0000AF44
		private void miDelAll_Click(object sender, EventArgs e)
		{
			this.DataGrid1.SelectAll();
			this.miDel_Click(RuntimeHelpers.GetObjectValue(sender), e);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000CB60 File Offset: 0x0000AF60
		private void miModify_Click(object sender, EventArgs e)
		{
			frmTagModify frmTagModify = new frmTagModify();
			if (this.DataGrid1.SelectedRows.Count < 1)
			{
				return;
			}
			checked
			{
				DataGridViewRow dataGridViewRow = this.DataGrid1.SelectedRows[this.DataGrid1.SelectedRows.Count - 1];
				int rowIndex = dataGridViewRow.Cells[0].RowIndex;
				int[] array = new int[this.DataGrid1.SelectedRows.Count - 1 + 1];
				int num = 0;
				int num2 = this.DataGrid1.SelectedRows.Count - 1;
				for (int i = num; i <= num2; i++)
				{
					array[i] = this.DataGrid1.SelectedRows[i].Cells[0].RowIndex;
				}
				frmTagModify frmTagModify2 = frmTagModify;
				frmTagModify2.pNode = dataGridViewRow.Cells[0].Value.ToString();
				frmTagModify2.pTag = dataGridViewRow.Cells[1].Value.ToString();
				frmTagModify2.pWave = dataGridViewRow.Cells[2].Value.ToString();
				frmTagModify2.pPriority = Conversions.ToInteger(dataGridViewRow.Cells[4].Value);
				frmTagModify2.pRowIndex = array;
				frmTagModify2.ShowDialog();
				if (this.DataGrid1.Rows.Count < 1)
				{
					return;
				}
				this.DataGrid1.ClearSelection();
				this.DataGrid1.FirstDisplayedScrollingRowIndex = Conversions.ToInteger(Interaction.IIf(this._dsCFG.Tables[0].Rows.Count - this._igvdLines < 0, 0, this._dsCFG.Tables[0].Rows.Count - this._igvdLines));
				this.DataGrid1.Rows[rowIndex].Selected = true;
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000CD44 File Offset: 0x0000B144
		private void miModifyAll_Click(object sender, EventArgs e)
		{
			this.DataGrid1.SelectAll();
			this.miModify_Click(RuntimeHelpers.GetObjectValue(sender), e);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000CD60 File Offset: 0x0000B160
		public void NewFile()
		{
			try
			{
				this.subChkIfChanged();
				this._myFile = null;
				this._dtConfig.Clear();
				this._bDocChanged = false;
				this._dtConfig.DefaultView.RowFilter = "";
				this.txtContent.Text = "";
				this.lblRecordFiltered.Text = "總共0筆, 篩選後剩下0筆";
				this.ShowStatus(0, "新檔");
				this.ShowStatus(1, "");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "開新檔", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000CE10 File Offset: 0x0000B210
		public void OpenFile()
		{
			try
			{
				this.subChkIfChanged();
				this.dlgOpen.Filter = "*.tac|*.tac";
				if (this.dlgOpen.ShowDialog() == DialogResult.OK)
				{
					this._myFile = new FileInfo(this.dlgOpen.FileName);
					this.ShowStatus(0, this._myFile.FullName + "  正在開啟中,請稍後.....");
					this.ReadFile();
					this.ShowStatus(0, this._myFile.FullName);
					this.ShowStatus(1, "");
					this.subRecentFile(this._myFile.FullName);
				}
			}
			catch (Exception ex)
			{
				string text = "檔案損毀, 無法開啟\r";
				MessageBox.Show(text + ex.Message, "讀取檔案", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000CEEC File Offset: 0x0000B2EC
		public void OpenRecentFile(string sFileName)
		{
			try
			{
				this.subChkIfChanged();
				this._myFile = new FileInfo(sFileName);
				this.ShowStatus(0, this._myFile.FullName + "  正在開啟中,請稍後.....");
				this.ReadFile();
				this.ShowStatus(0, this._myFile.FullName);
				this.ShowStatus(1, "");
				this.subRecentFile(this._myFile.FullName);
			}
			catch (Exception ex)
			{
				string text = "檔案損毀, 無法開啟\r";
				MessageBox.Show(text + ex.Message, "讀取檔案", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000CF9C File Offset: 0x0000B39C
		public void SaveFile()
		{
			try
			{
				if (this._myFile == null)
				{
					this.SaveAs();
				}
				else
				{
					this.subValidateData();
					this.WriteFile();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("無法存檔\r\r" + ex.Message, "儲存檔案", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000D004 File Offset: 0x0000B404
		public void SaveAs()
		{
			try
			{
				this.subValidateData();
				this.dlgSave.Filter = "*.tac|*.tac";
				this.dlgSave.FileName = "Default";
				if (this.dlgSave.ShowDialog() == DialogResult.OK)
				{
					this._myFile = new FileInfo(this.dlgSave.FileName);
					this.WriteFile();
					this.subRecentFile(this._myFile.FullName);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("無法存檔\r\r" + ex.Message, "另存檔案", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000D0B4 File Offset: 0x0000B4B4
		public void WriteFile()
		{
			try
			{
				this._dsCFG.AcceptChanges();
				string text = modSub.funDSToString(this._dtConfig);
				FileStream fileStream = this._myFile.Open(FileMode.Create, FileAccess.Write);
				StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Default);
				streamWriter.Write(text);
				streamWriter.Close();
				fileStream.Close();
				this._bDocChanged = false;
				this._dsCFG.AcceptChanges();
				this.ShowStatus(0, this._myFile.FullName);
				this.ShowStatus(1, "");
			}
			catch (Exception ex)
			{
				MessageBox.Show("無法寫入\r\r" + ex.Message, "寫入檔案", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				StreamWriter streamWriter;
				if (!Information.IsNothing(streamWriter))
				{
					streamWriter.Close();
				}
				FileStream fileStream;
				if (!Information.IsNothing(fileStream))
				{
					fileStream.Close();
				}
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000D190 File Offset: 0x0000B590
		public void ReadFile()
		{
			if (this._myFile.Exists)
			{
				try
				{
					this._bFileReading = true;
					this._dtConfig.Clear();
					FileStream fileStream = this._myFile.Open(FileMode.Open, FileAccess.Read);
					StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);
					modSub.subStringToDs(streamReader, this._dtConfig);
					streamReader.Close();
					fileStream.Close();
					this._bDocChanged = false;
					if (!this._dsCFG.HasErrors)
					{
						this._dsCFG.AcceptChanges();
					}
					this._dtConfig.DefaultView.RowFilter = "";
					this.txtContent.Text = "";
					this.lblRecordFiltered.Text = "總共0筆, 篩選後剩下0筆";
					return;
				}
				catch (Exception ex)
				{
					string text = "檔案損毀, 無法開啟\r";
					MessageBox.Show(text + ex.Message, "讀取檔案", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					this._bDocChanged = false;
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
					return;
				}
				finally
				{
					this._bFileReading = false;
				}
			}
			if (this._bFormInitDone)
			{
				MessageBox.Show("檔案不存在!", "讀取檔案", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000D2DC File Offset: 0x0000B6DC
		public void CloseFile()
		{
			if (this._bDocChanged)
			{
				switch (MessageBox.Show("檔案已經變更\r是否要存檔?", "存檔", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
				{
				case DialogResult.Yes:
					this.SaveFile();
					this.Close();
					break;
				}
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000D324 File Offset: 0x0000B724
		public void ShowStatus(int index, string statusText)
		{
			if (this._bDocChanged | this._dsCFG.HasChanges())
			{
				this.kvStatusBar.Panels[1].Text = "檔案變更";
			}
			this.kvStatusBar.Panels[index].Text = statusText;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000D378 File Offset: 0x0000B778
		private void subCreateTable_CacheTagList()
		{
			try
			{
				DataColumn dataColumn = new DataColumn();
				this._dsXML.Tables.Add("Tag");
				this._dtTag = this._dsXML.Tables["Tag"];
				dataColumn = new DataColumn();
				DataColumn dataColumn2 = dataColumn;
				dataColumn2.DataType = Type.GetType("System.String");
				dataColumn2.MaxLength = 30;
				dataColumn2.AllowDBNull = true;
				dataColumn2.Caption = "Tagname";
				dataColumn2.ColumnName = "Tagname";
				this._dtTag.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				DataColumn dataColumn3 = dataColumn;
				dataColumn3.DataType = Type.GetType("System.Int16");
				dataColumn3.AllowDBNull = false;
				dataColumn3.Caption = "Type";
				dataColumn3.ColumnName = "Type";
				this._dtTag.Columns.Add(dataColumn);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000D47C File Offset: 0x0000B87C
		private void subRecentFile(string sNew)
		{
			if (Operators.CompareString(this.miRecent4.Text, sNew, false) == 0)
			{
				return;
			}
			if (Operators.CompareString(this.miRecent3.Text, sNew, false) == 0)
			{
				return;
			}
			if (Operators.CompareString(this.miRecent2.Text, sNew, false) == 0)
			{
				return;
			}
			if (Operators.CompareString(this.miRecent1.Text, sNew, false) == 0)
			{
				return;
			}
			this.miRecent4.Text = this.miRecent3.Text;
			this.miRecent3.Text = this.miRecent2.Text;
			this.miRecent2.Text = this.miRecent1.Text;
			this.miRecent1.Text = sNew;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000D538 File Offset: 0x0000B938
		private void subChkIfChanged()
		{
			if (this._dsCFG.HasChanges() && !this._dsCFG.HasErrors)
			{
				this._dsCFG.AcceptChanges();
				this._bDocChanged = true;
			}
			if (this._bDocChanged)
			{
				DialogResult dialogResult = MessageBox.Show("檔案已經變更\r是否要存檔?", "存檔", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (dialogResult == DialogResult.Yes)
				{
					this.SaveFile();
				}
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000D598 File Offset: 0x0000B998
		private void subValidateData()
		{
			try
			{
				if (this._dtConfig.Rows.Count < 1)
				{
					string text = "沒有任何一筆資料";
					throw new Exception(text);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000D5EC File Offset: 0x0000B9EC
		private void frmAlarmSndCfg_Load(object sender, EventArgs e)
		{
			try
			{
				this.subCreateTable_CacheTagList();
				this._dtConfig = this._dsCFG.Tables["Config"];
				clsEDA clsEDA = new clsEDA();
				clsEDA.subGetNodeList(this.cobNode);
				this.cobArea.SelectedIndex = 0;
				this.cobPriority.SelectedIndex = 0;
				this.cobType.SelectedIndex = 0;
				this.cobColumn.SelectedIndex = 0;
				this.cobOper.SelectedIndex = 0;
				this.txtCfg.Text = modpublic.g_sCfgName;
				this.ndSoundTime.Value = new decimal(modpublic.g_iSoundTime);
				this.ndScanTime.Value = new decimal(modpublic.g_iScanTime);
				this.ckDisBar.Checked = modpublic.g_bMenuBar;
				this.ckBackGround.Checked = modpublic.g_bBackground;
				this.OpenRecentFile(modpublic.g_sCfgName);
				this._bFormInitDone = true;
				this.cobNode_SelectedIndexChanged(RuntimeHelpers.GetObjectValue(sender), e);
				if (this.comNTF.Items.Count < 1)
				{
					this.btnAdd.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "iFix警報輪迴播音系統態設定介面", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000D740 File Offset: 0x0000BB40
		private void frmAlarmSndCfg_Resize(object sender, EventArgs e)
		{
			checked
			{
				this._igvdLines = (int)Math.Round(unchecked((double)(checked(this.DataGrid1.Height - 31)) / 20.0 - 1.0));
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000D774 File Offset: 0x0000BB74
		private void frmAlarmSndCfg_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.Timer1.Stop();
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000D784 File Offset: 0x0000BB84
		private void cobArea_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!this._bFormInitDone)
			{
				return;
			}
			if (Operators.CompareString(this.cobArea.Text.ToUpper(), "TAG", false) == 0 && !this._bNotShowWarnning)
			{
				string text = "強烈建議使用AlarmCounter方式來觸發播音\r如果使用Tag方式, 則會增加CPU與記憶體負載!\r確定要使用 請按[是],  不要請按[否]";
				int num = (int)MessageBox.Show(text, "改變觸發方式", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (num != 6)
				{
					this.cobArea.SelectedIndex = 0;
					return;
				}
			}
			this._bNotShowWarnning = false;
			this.txtTagFilter_LostFocus(RuntimeHelpers.GetObjectValue(sender), e);
			if (Operators.CompareString(this.cobArea.Text, "警報區域", false) == 0)
			{
				this.cobType.Enabled = false;
			}
			else
			{
				this.cobType.Enabled = true;
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000D834 File Offset: 0x0000BC34
		protected void btnEnterMode_Click(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			if (Operators.CompareString(button.Text, "改用手動輸入", false) == 0)
			{
				button.Text = "改用選取清單方式";
				this.comNTF.Visible = false;
				this.cobNode.Enabled = false;
				this.cobArea.Enabled = false;
				this.txtTagFilter.Enabled = false;
				this.cobType.Enabled = false;
				this.btnUpdate.Enabled = false;
				this.txtNTF.Visible = true;
				if (Operators.CompareString(this.cobArea.Text.ToUpper(), "TAG", false) == 0)
				{
					this.txtNTF.Text = "Node.Tag";
				}
				else
				{
					this.txtNTF.Text = "Node.AlarmArea.NOPRI";
				}
				this.btnAdd.Enabled = true;
			}
			else
			{
				button.Text = "改用手動輸入";
				this.comNTF.Visible = true;
				this.cobNode.Enabled = true;
				this.cobArea.Enabled = true;
				this.txtTagFilter.Enabled = true;
				if (this.cobArea.SelectedIndex == 0)
				{
					this.cobType.Enabled = false;
				}
				else
				{
					this.cobType.Enabled = true;
				}
				this.btnUpdate.Enabled = true;
				this.txtNTF.Visible = false;
				if (this.comNTF.Items.Count > 0)
				{
					this.btnAdd.Enabled = true;
				}
				else
				{
					this.btnAdd.Enabled = false;
				}
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000D9B8 File Offset: 0x0000BDB8
		private void Timer1_Tick(object sender, EventArgs e)
		{
			if (this.cobNode.Items.Count > 0 && this._dtTag.Rows.Count < 1)
			{
				if (Operators.CompareString(this.btnUpdate.ForeColor.Name.ToUpper(), "Red".ToUpper(), false) == 0)
				{
					this.btnUpdate.ForeColor = Color.DarkGray;
				}
				else
				{
					this.btnUpdate.ForeColor = Color.Red;
				}
			}
			else
			{
				this.btnUpdate.ForeColor = Color.DarkGray;
			}
			if (!this.comNTF.Visible)
			{
				if (Operators.CompareString(this.lblArea.ForeColor.Name.ToUpper(), "Red".ToUpper(), false) == 0)
				{
					this.lblArea.ForeColor = Color.Black;
				}
				else
				{
					this.lblArea.ForeColor = Color.Red;
				}
			}
			else
			{
				this.lblArea.ForeColor = Color.Black;
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000DAB8 File Offset: 0x0000BEB8
		private void cobNode_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!this._bFormInitDone)
			{
				return;
			}
			if (this.cobNode.Items.Count < 1)
			{
				return;
			}
			try
			{
				this._dtTag.Clear();
				string text = Application.StartupPath + "\\cache_txiFixAlmSndCly_" + this.cobNode.Text + ".xml";
				if (File.Exists(text))
				{
					DateTime lastWriteTime = File.GetLastWriteTime(text);
					this.lblDate.Text = "(更新日期: " + Strings.Format(DateAndTime.Now, "yyyy/MM/dd HH:mm:ss") + ")";
					FileStream fileStream = new FileStream(text, FileMode.Open);
					XmlTextReader xmlTextReader = new XmlTextReader(fileStream);
					this._dsXML.ReadXml(xmlTextReader);
					xmlTextReader.Close();
					this.txtTagFilter_LostFocus(RuntimeHelpers.GetObjectValue(sender), e);
				}
				else
				{
					this.lblDate.Text = "(沒有快取資料)";
					if (!Operators.ConditionalCompareObjectGreater(this.cobNode.Tag, 0, false))
					{
						string text2 = "你可能是第一次使用或是Tag快取資料被刪除了\r\r是否要讓程式為你自動更新 <Tag快取資料> ?\r若要請按<是>, 不要請按<否>";
						DialogResult dialogResult = MessageBox.Show(text2, "更新 <" + this.cobNode.Text + "> Tag快取資料", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
						if (dialogResult == DialogResult.Yes)
						{
							this.btnUpdate_Click(RuntimeHelpers.GetObjectValue(sender), e);
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "變更node_讀取TagList", MessageBoxButtons.OK, MessageBoxIcon.Question);
			}
			finally
			{
				ComboBox cobNode = this.cobNode;
				cobNode.Tag = Operators.AddObject(cobNode.Tag, 1);
				XmlTextReader xmlTextReader;
				if (!Information.IsNothing(xmlTextReader))
				{
					xmlTextReader.Close();
				}
				FileStream fileStream;
				if (!Information.IsNothing(fileStream))
				{
					fileStream.Close();
				}
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000DC84 File Offset: 0x0000C084
		private void btnUpdate_Click(object sender, EventArgs e)
		{
			long ticks = DateTime.Now.Ticks;
			try
			{
				if (this.cobNode.Items.Count >= 1)
				{
					short num = 0;
					clsEDA clsEDA = new clsEDA();
					this.Enabled = false;
					this.Cursor = Cursors.WaitCursor;
					this.comNTF.ValueMember = "";
					this.comNTF.DisplayMember = "";
					this.comNTF.DataSource = null;
					this._dtTag.Clear();
					try
					{
						this.comNTF.Visible = false;
						short num2 = 1;
						do
						{
							clsEDA.GetTagsList(this.cobNode.Text, num2, num, 200, ref this._dtTag);
							num2 += 1;
						}
						while (num2 <= 100);
						num2 = 241;
						do
						{
							clsEDA.GetTagsList(this.cobNode.Text, num2, num, 1, ref this._dtTag);
							num2 += 1;
						}
						while (num2 <= 243);
						this._dtTag.DefaultView.Sort = "Tagname";
					}
					catch (Exception ex)
					{
						MessageBox.Show("無法取得Tag清單, 因為\r" + ex.Message, "更新Tag快取", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						return;
					}
					finally
					{
						this.comNTF.Visible = true;
					}
					this.txtTagFilter_LostFocus(RuntimeHelpers.GetObjectValue(sender), e);
					if (this._dtTag.Rows.Count > 0)
					{
						try
						{
							string text = Application.StartupPath + "\\cache_txiFixAlmSndCly_" + this.cobNode.Text + ".xml";
							FileStream fileStream = new FileStream(text, FileMode.Create);
							XmlTextWriter xmlTextWriter = new XmlTextWriter(fileStream, Encoding.Unicode);
							this._dsXML.WriteXml(xmlTextWriter);
							xmlTextWriter.Close();
							fileStream.Close();
							this.lblDate.Text = "(更新日期: " + Strings.Format(DateAndTime.Now, "yyyy/MM/dd HH:mm:ss") + ")";
						}
						finally
						{
							XmlTextWriter xmlTextWriter;
							if (!Information.IsNothing(xmlTextWriter))
							{
								xmlTextWriter.Close();
							}
							FileStream fileStream;
							if (!Information.IsNothing(fileStream))
							{
								fileStream.Close();
							}
						}
					}
				}
			}
			catch (Exception ex2)
			{
				MessageBox.Show(ex2.Message, "更新Tag快取", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			finally
			{
				this.Enabled = true;
				this.Cursor = Cursors.Arrow;
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000DF40 File Offset: 0x0000C340
		private void txtTagFilter_LostFocus(object sender, EventArgs e)
		{
			if (!this._bFormInitDone)
			{
				return;
			}
			DataView defaultView = this._dtTag.DefaultView;
			try
			{
				this.cobPriority.Items.Clear();
				string text;
				if (Operators.CompareString(this.cobArea.Text.ToUpper(), "TAG", false) == 0)
				{
					this.cobType.Text = this.cobType.Text.Trim().ToUpper();
					if ((Operators.CompareString(this.cobType.Text.ToUpper(), "ALL", false) != 0) & (this.cobType.Text.Length > 0))
					{
						short num = Eda.TypeToIndex(this.cobNode.Text, this.cobType.Text);
						text = "TYPE = " + Conversions.ToString((int)num);
						this.cobPriority.Items.Add("NOPRI");
						if (Array.IndexOf(new object[] { "AA", "AI", "AIS" }, this.cobType.Text.ToUpper()) >= 0)
						{
							foreach (string text2 in modpublic.g_aPriorityForAnalogTag)
							{
								this.cobPriority.Items.Add(text2.ToUpper());
							}
						}
					}
					else
					{
						text = "TYPE < 240";
					}
				}
				else
				{
					text = "TYPE > 240";
					this.cobPriority.Items.Add("NOPRI");
					foreach (string text2 in modpublic.g_aPriorityForArea)
					{
						this.cobPriority.Items.Add(text2.ToUpper());
					}
				}
				if (this.cobPriority.Items.Count > 0)
				{
					this.cobPriority.SelectedIndex = 0;
				}
				if (this.txtTagFilter.Text.Length < 1)
				{
					this.txtTagFilter.Text = "*";
				}
				text = text + " AND Tagname LIKE '" + this.txtTagFilter.Text + "'";
				if (Operators.CompareString(defaultView.RowFilter, text, false) != 0)
				{
					defaultView.RowFilter = text;
					defaultView.Sort = "Type,Tagname";
					this.lblTagList.Text = this.cobArea.Text + " (" + Conversions.ToString(defaultView.Count) + ")";
				}
				this.comNTF.Visible = false;
				this.comNTF.BeginUpdate();
				this.comNTF.DataSource = this._dtTag.DefaultView;
				this.comNTF.DisplayMember = "Tag";
				this.comNTF.ValueMember = "Tagname";
				if (this.comNTF.Items.Count > 0)
				{
					this.comNTF.SelectedIndex = 0;
					this.btnAdd.Enabled = true;
				}
				else
				{
					this.btnAdd.Enabled = false;
				}
				this.comNTF.EndUpdate();
				this.comNTF.Visible = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "txtTagFilter_LostFocus", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000E29C File Offset: 0x0000C69C
		private void btnAdd_Click(object sender, EventArgs e)
		{
			string text = "";
			string text2 = "";
			checked
			{
				try
				{
					if (this.txtNTF.Visible || this.comNTF.SelectedItems.Count >= 1)
					{
						this.Enabled = false;
						this.Cursor = Cursors.WaitCursor;
						if (this.comNTF.Visible)
						{
							if (this.comNTF.SelectedItems.Count >= 1)
							{
								this.txtWavName.Text = this.txtWavName.Text.Trim();
								if (this.txtWavName.Text.Length < 1)
								{
									throw new Exception("Wave檔名不可以空白");
								}
								string text3 = this.txtWavName.Text.Substring(this.txtWavName.Text.Length - 4);
								if (Array.IndexOf(new object[] { ".WAV", ".TXT" }, text3) >= 0)
								{
									if (Strings.InStr(this.txtWavName.Text, "\\", CompareMethod.Binary) < 0)
									{
										text2 = Application.StartupPath + "\\" + text2;
									}
									else
									{
										text2 = this.txtWavName.Text;
									}
									FileInfo fileInfo = new FileInfo(text2);
									if (!File.Exists(text2))
									{
										throw new Exception("檔案不存在 <" + text2 + ">");
									}
								}
								int num = 0;
								int num2 = this.comNTF.SelectedItems.Count - 1;
								for (int i = num; i <= num2; i++)
								{
									DataRow dataRow = this._dtConfig.NewRow();
									dataRow["node"] = this.cobNode.Text;
									dataRow["tag"] = RuntimeHelpers.GetObjectValue(NewLateBinding.LateIndexGet(this.comNTF.SelectedItems[i], new object[] { 0 }, null));
									dataRow["field"] = this.cobPriority.Text;
									dataRow["wave"] = this.txtWavName.Text;
									dataRow["priority"] = this.ndLevel.Value;
									dataRow["blocktype"] = RuntimeHelpers.GetObjectValue(this._dtTag.DefaultView[i][1]);
									dataRow["length"] = this.ndSecond.Value;
									dataRow["Picture"] = this.txtPicName.Text;
									dataRow["Outsource"] = this.txtSource.Text;
									dataRow["Outvalue"] = this.txtValue.Text;
									this._dtConfig.Rows.Add(dataRow);
								}
							}
						}
						else
						{
							if (this.txtNTF.Text.Length < 1)
							{
								throw new Exception("TagName不可以空白");
							}
							if (Strings.InStr(this.txtNTF.Text, " ", CompareMethod.Text) > 0)
							{
								throw new Exception("TagName不可以含空白字元\r" + this.txtNTF.Text);
							}
							if (Strings.InStr(this.txtNTF.Text, ":", CompareMethod.Text) > 0)
							{
								throw new Exception("TagName語法不正確(不能含有[:]字元\r" + this.txtNTF.Text);
							}
							int num3 = Strings.InStr(this.txtNTF.Text, ".", CompareMethod.Text);
							if (num3 < 1)
							{
								throw new Exception("TagName語法不正確(缺少[.]字元\r" + this.txtNTF.Text);
							}
							num3 = Strings.InStr(num3 + 1, this.txtNTF.Text, ".", CompareMethod.Text);
							string[] array = this.txtNTF.Text.Split(new char[] { '.' });
							if ((array[0].Length < 1) | (array[0].Length > 8))
							{
								throw new Exception("NodeName不可以低於1個字或超過8個字\r" + array[0]);
							}
							if ((array[1].Length < 1) | (array[1].Length > 30))
							{
								throw new Exception("TagName不可以低於1個字或超過30個字\r" + array[1]);
							}
							this.txtWavName.Text = this.txtWavName.Text.Trim();
							if (this.txtWavName.Text.Length < 1)
							{
								throw new Exception("Wave檔名不可以空白");
							}
							string text3 = this.txtWavName.Text.Substring(this.txtWavName.Text.Length - 4);
							if (Array.IndexOf(new object[] { ".WAV", ".TXT" }, text3) >= 0)
							{
								if (Strings.InStr(this.txtWavName.Text, "\\", CompareMethod.Binary) < 0)
								{
									text2 = Application.StartupPath + "\\" + text2;
								}
								else
								{
									text2 = this.txtWavName.Text;
								}
								FileInfo fileInfo = new FileInfo(text2);
								if (!File.Exists(text2))
								{
									throw new Exception("檔案不存在 <" + text2 + ">");
								}
							}
							DataRow dataRow = this._dtConfig.NewRow();
							array = this.txtNTF.Text.Split(new char[] { '.' });
							dataRow["node"] = array[0];
							dataRow["tag"] = array[1];
							if (array.Length < 3)
							{
								dataRow["field"] = "NOPRI";
							}
							else
							{
								dataRow["field"] = array[2];
							}
							if (Array.IndexOf(modpublic.g_aPriorityList, RuntimeHelpers.GetObjectValue(NewLateBinding.LateGet(dataRow["field"], null, "ToUpper", new object[0], null, null, null))) < 0)
							{
								dataRow["field"] = "NOPRI";
							}
							else
							{
								dataRow["field"] = array[2];
							}
							dataRow["wave"] = this.txtWavName.Text;
							dataRow["priority"] = this.ndLevel.Value;
							if (Operators.CompareString(this.cobArea.Text.ToUpper(), "TAG", false) != 0)
							{
								dataRow["blocktype"] = 242;
							}
							else
							{
								dataRow["blocktype"] = 1;
							}
							dataRow["length"] = this.ndSecond.Value;
							dataRow["Picture"] = this.txtPicName.Text;
							dataRow["Outsource"] = this.txtSource.Text;
							dataRow["Outvalue"] = this.txtValue.Text;
							this._dtConfig.Rows.Add(dataRow);
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "新增Tag", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					if (ex.Message.IndexOf("TagName") > -1)
					{
						text = "TAG";
					}
					else if (ex.Message.IndexOf("Wave") > -1)
					{
						text = "WAVE";
					}
				}
				finally
				{
					this.Enabled = true;
					this.Cursor = Cursors.Arrow;
					this.lblRecordFiltered.Text = string.Concat(new string[]
					{
						"總共",
						Conversions.ToString(this._dtConfig.Rows.Count),
						"筆, 篩選後剩下",
						Conversions.ToString(this.DataGrid1.RowCount),
						"筆"
					});
					if (Operators.CompareString(text, "TAG", false) == 0)
					{
						this.txtNTF.Focus();
						this.txtNTF.SelectAll();
					}
					else if (Operators.CompareString(text, "WAVE", false) == 0)
					{
						this.txtWavName.Focus();
						this.txtWavName.SelectAll();
					}
				}
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000EAB0 File Offset: 0x0000CEB0
		private void btnRemove_Click(object sender, EventArgs e)
		{
			if (this.DataGrid1.SelectedRows.Count < 1)
			{
				return;
			}
			checked
			{
				try
				{
					int num = (int)MessageBox.Show("你確定要刪除此 " + Conversions.ToString(this.DataGrid1.SelectedRows.Count) + " 筆資料?", "刪除tag", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
					if (num == 6)
					{
						this._bNotShowWarnning = true;
						this.cobNode.Text = Conversions.ToString(this.DataGrid1.SelectedRows[0].Cells[0].Value);
						string text = Conversions.ToString(this.DataGrid1.SelectedRows[0].Cells[6].Value);
						if (Conversion.Val(text) >= 240.0)
						{
							this.cobArea.SelectedIndex = 0;
						}
						else
						{
							this.cobArea.SelectedIndex = 1;
							double num2 = Conversion.Val(text);
							if (num2 == 1.0)
							{
								this.cobType.Text = "AI";
							}
							else if (num2 == 3.0)
							{
								this.cobType.Text = "DI";
							}
							else if (num2 == 12.0)
							{
								this.cobType.Text = "AA";
							}
							else if (num2 == 13.0)
							{
								this.cobType.Text = "DA";
							}
							else
							{
								this.cobType.Text = "AIS";
							}
						}
						this.comNTF.Text = Conversions.ToString(this.DataGrid1.SelectedRows[0].Cells[1].Value);
						this.cobPriority.Text = Conversions.ToString(this.DataGrid1.SelectedRows[0].Cells[2].Value);
						this.txtWavName.Text = Conversions.ToString(this.DataGrid1.SelectedRows[0].Cells[3].Value);
						this.ndLevel.Value = new decimal(Conversion.Val(RuntimeHelpers.GetObjectValue(this.DataGrid1.SelectedRows[0].Cells[5].Value)));
						this.ndSecond.Value = new decimal(Conversion.Val(RuntimeHelpers.GetObjectValue(this.DataGrid1.SelectedRows[0].Cells[7].Value)));
						int num3 = 0;
						int num4 = this.DataGrid1.SelectedRows.Count - 1;
						for (int i = num3; i <= num4; i++)
						{
							this.DataGrid1.Rows.Remove(this.DataGrid1.SelectedRows[0]);
						}
						this.lblRecordFiltered.Text = string.Concat(new string[]
						{
							"總共",
							Conversions.ToString(this._dtConfig.Rows.Count),
							"筆, 篩選後剩下",
							Conversions.ToString(this.DataGrid1.RowCount),
							"筆"
						});
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Remove tag", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000EE1C File Offset: 0x0000D21C
		private void btnFile_Click(object sender, EventArgs e)
		{
			try
			{
				this.dlgOpen.Filter = "*.wav|*.wav|*.txt|*.txt";
				if (Strings.Len(this.txtWavName.Text) < 1)
				{
					this.dlgOpen.InitialDirectory = Application.StartupPath;
					this.dlgOpen.FileName = "";
				}
				else
				{
					FileInfo fileInfo = new FileInfo(this.txtWavName.Text);
					this.dlgOpen.InitialDirectory = fileInfo.DirectoryName;
					this.dlgOpen.FileName = fileInfo.Name;
				}
				if (this.dlgOpen.ShowDialog() == DialogResult.OK)
				{
					FileInfo fileInfo = new FileInfo(this.dlgOpen.FileName);
					if (Operators.CompareString(fileInfo.DirectoryName.ToUpper(), Application.StartupPath.ToUpper(), false) == 0)
					{
						this.txtWavName.Text = fileInfo.Name;
					}
					else
					{
						this.txtWavName.Text = fileInfo.FullName;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Select sound file", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000EF38 File Offset: 0x0000D338
		private void DataGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			DataGridView dataGrid = this.DataGrid1;
			DataTable dataTable = this._dsCFG.Tables[0];
			int columnIndex = e.ColumnIndex;
			int rowIndex = e.RowIndex;
			bool flag = false;
			checked
			{
				try
				{
					if (this.DataGrid1.RowCount > 0 && e.RowIndex >= 0 && e.ColumnIndex >= 0)
					{
						string text = dataGrid.Columns[e.ColumnIndex].Name.ToUpper();
						if (Operators.CompareString(text, "clnOpenWave".ToUpper(), false) == 0)
						{
							if (Strings.InStr(Conversions.ToString(this.DataGrid1[columnIndex - 1, rowIndex].Value), "\\", CompareMethod.Binary) > 0)
							{
								flag = true;
							}
							this.dlgOpen.Filter = "*.wav|*.wav|*.txt|*.txt";
							FileInfo fileInfo = new FileInfo(Conversions.ToString(this.DataGrid1[columnIndex - 1, rowIndex].Value));
							if (flag)
							{
								this.dlgOpen.InitialDirectory = fileInfo.DirectoryName;
							}
							else
							{
								this.dlgOpen.InitialDirectory = Application.StartupPath;
							}
							this.dlgOpen.FileName = fileInfo.Name;
							if (this.dlgOpen.ShowDialog() == DialogResult.OK)
							{
								fileInfo = new FileInfo(this.dlgOpen.FileName);
								if (Operators.CompareString(fileInfo.DirectoryName.ToUpper(), Application.StartupPath.ToUpper(), false) == 0)
								{
									this.DataGrid1[columnIndex - 1, rowIndex].Value = fileInfo.Name;
								}
								else
								{
									this.DataGrid1[columnIndex - 1, rowIndex].Value = fileInfo.FullName;
								}
							}
						}
						else if (Operators.CompareString(text, "clnOpenPic".ToUpper(), false) == 0)
						{
							string text2 = "";
							FixHelper fixHelper = new FixHelper();
							short num = fixHelper.FixGetPath("PICPATH", ref text2);
							if (num != 11000)
							{
								text2 = Application.StartupPath;
							}
							this.dlgOpen.Filter = "*.grf|*.grf";
							if (this.DataGrid1[columnIndex - 1, rowIndex].Value.ToString().Length < 1)
							{
								this.dlgOpen.InitialDirectory = text2;
								this.dlgOpen.FileName = "";
							}
							else if (Strings.InStr(Conversions.ToString(this.DataGrid1[columnIndex - 1, rowIndex].Value), "\\", CompareMethod.Binary) < 1)
							{
								this.dlgOpen.InitialDirectory = text2;
								this.dlgOpen.FileName = Conversions.ToString(this.DataGrid1[columnIndex - 1, rowIndex].Value);
							}
							else
							{
								FileInfo fileInfo = new FileInfo(Conversions.ToString(this.DataGrid1[columnIndex - 1, rowIndex].Value));
								this.dlgOpen.InitialDirectory = fileInfo.DirectoryName;
								this.dlgOpen.FileName = fileInfo.Name;
							}
							if (this.dlgOpen.ShowDialog() == DialogResult.OK)
							{
								FileInfo fileInfo = new FileInfo(this.dlgOpen.FileName);
								if (Operators.CompareString(fileInfo.DirectoryName.ToUpper(), text2, false) == 0)
								{
									this.DataGrid1[columnIndex - 1, rowIndex].Value = fileInfo.Name;
								}
								else
								{
									this.DataGrid1[columnIndex - 1, rowIndex].Value = fileInfo.FullName;
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Grid Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000F2D0 File Offset: 0x0000D6D0
		private void DataGrid1_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			string message = e.Exception.Message;
			MessageBox.Show(message, "Tag修改", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000F2F8 File Offset: 0x0000D6F8
		private void btnApply_Click(object sender, EventArgs e)
		{
			string text = "";
			this.btnApply.Enabled = false;
			this.btnApply.Cursor = Cursors.WaitCursor;
			if (this.txtContent.Text.Length == 0)
			{
				string text2 = this.cobOper.Text;
				if (Operators.CompareString(text2, "LIKE", false) == 0)
				{
					text = this.cobColumn.Text + " " + this.cobOper.Text + " '*'";
				}
				else if (Operators.CompareString(text2, "=", false) == 0)
				{
					text = "";
				}
			}
			else
			{
				text = string.Concat(new string[]
				{
					this.cobColumn.Text,
					" ",
					this.cobOper.Text,
					" '",
					this.txtContent.Text,
					"'"
				});
			}
			this._dtConfig.DefaultView.RowFilter = text;
			this.DataGrid1.DataSource = this._dtConfig.DefaultView;
			this.btnApply.Cursor = Cursors.Arrow;
			this.btnApply.Enabled = true;
			this.lblRecordFiltered.Text = string.Concat(new string[]
			{
				"總共",
				Conversions.ToString(this._dtConfig.Rows.Count),
				"筆, 篩選後剩下",
				Conversions.ToString(this.DataGrid1.RowCount),
				"筆"
			});
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000F484 File Offset: 0x0000D884
		private void tmrStatus_Tick(object sender, EventArgs e)
		{
			if (!this._bFormInitDone)
			{
				return;
			}
			this.ShowStatus(2, DateAndTime.Now.ToLongTimeString());
			this.lblRecordFiltered.Text = string.Concat(new string[]
			{
				"總共",
				Conversions.ToString(this._dtConfig.Rows.Count),
				"筆, 篩選後剩下",
				Conversions.ToString(this.DataGrid1.RowCount),
				"筆"
			});
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000F50C File Offset: 0x0000D90C
		private void btnCancle_Click(object sender, EventArgs e)
		{
			this.tabTAG.Show();
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000F51C File Offset: 0x0000D91C
		private void btnOK_Click(object sender, EventArgs e)
		{
			string text = Application.StartupPath + "\\" + Application.ProductName + ".ini";
			try
			{
				FileStream fileStream = File.Open(text, FileMode.Create, FileAccess.ReadWrite);
				StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Default);
				if (this.txtCfg.Text.Length < 1)
				{
					streamWriter.WriteLine("PDB=Default.tac;");
				}
				else
				{
					streamWriter.WriteLine("PDB=" + this.txtCfg.Text + ";");
				}
				streamWriter.WriteLine("SOUNDTIME=" + Conversions.ToString(this.ndSoundTime.Value) + ";");
				modpublic.g_iSoundTime = Convert.ToInt32(this.ndSoundTime.Value);
				streamWriter.WriteLine("SCANTIME=" + Conversions.ToString(this.ndScanTime.Value) + ";");
				modpublic.g_iScanTime = Convert.ToInt32(this.ndScanTime.Value);
				streamWriter.WriteLine("BACKGROUND=" + Conversions.ToString(this.ckBackGround.Checked) + ";");
				streamWriter.WriteLine("MENUBAR=" + Conversions.ToString(this.ckDisBar.Checked) + ";");
				if (!Information.IsNothing(streamWriter))
				{
					streamWriter.Close();
				}
				if (!Information.IsNothing(fileStream))
				{
					streamWriter.Close();
				}
				frmAlmSndRun frmRun = modpublic.frmRun;
				frmRun.txtSoundTime.Text = Conversions.ToString(modpublic.g_iSoundTime);
				frmRun.tmrPlaySound.Interval = 10;
				frmRun.txtScanTime.Text = Conversions.ToString(modpublic.g_iScanTime);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "環境參數", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			finally
			{
				StreamWriter streamWriter;
				if (!Information.IsNothing(streamWriter))
				{
					streamWriter.Close();
				}
				FileStream fileStream;
				if (!Information.IsNothing(fileStream))
				{
					streamWriter.Close();
				}
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000F72C File Offset: 0x0000DB2C
		private void btnPic_Click(object sender, EventArgs e)
		{
			string text = "";
			FixHelper fixHelper = new FixHelper();
			try
			{
				short num = fixHelper.FixGetPath("PICPATH", ref text);
				if (num != 11000)
				{
					text = Application.StartupPath;
				}
				this.dlgOpen.Filter = "*.grf|*.grf";
				if (Strings.Len(this.txtPicName.Text) < 1)
				{
					this.dlgOpen.InitialDirectory = text;
					this.dlgOpen.FileName = "";
				}
				else if (Strings.InStr(this.txtPicName.Text, "\\", CompareMethod.Binary) < 1)
				{
					this.dlgOpen.InitialDirectory = text;
					this.dlgOpen.FileName = this.txtPicName.Text;
				}
				else
				{
					FileInfo fileInfo = new FileInfo(this.txtPicName.Text);
					this.dlgOpen.InitialDirectory = fileInfo.DirectoryName;
					this.dlgOpen.FileName = fileInfo.Name;
				}
				if (this.dlgOpen.ShowDialog() == DialogResult.OK)
				{
					this.txtPicName.Text = this.dlgOpen.FileName;
					FileInfo fileInfo = new FileInfo(this.dlgOpen.FileName);
					if (Operators.CompareString(fileInfo.DirectoryName.ToUpper(), text.ToUpper(), false) == 0)
					{
						this.txtPicName.Text = fileInfo.Name;
					}
					else
					{
						this.txtPicName.Text = fileInfo.FullName;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Select Picture", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x0400002C RID: 44
		[AccessedThroughProperty("tabEnvironment")]
		private TabControl _tabEnvironment;

		// Token: 0x0400002D RID: 45
		[AccessedThroughProperty("tabTAG")]
		private TabPage _tabTAG;

		// Token: 0x0400002E RID: 46
		[AccessedThroughProperty("tabParameter")]
		private TabPage _tabParameter;

		// Token: 0x0400002F RID: 47
		[AccessedThroughProperty("btnRemove")]
		private Button _btnRemove;

		// Token: 0x04000030 RID: 48
		[AccessedThroughProperty("btnAdd")]
		private Button _btnAdd;

		// Token: 0x04000031 RID: 49
		[AccessedThroughProperty("gpFilter")]
		private GroupBox _gpFilter;

		// Token: 0x04000032 RID: 50
		[AccessedThroughProperty("txtContent")]
		private TextBox _txtContent;

		// Token: 0x04000033 RID: 51
		[AccessedThroughProperty("cobOper")]
		private ComboBox _cobOper;

		// Token: 0x04000034 RID: 52
		[AccessedThroughProperty("cobColumn")]
		private ComboBox _cobColumn;

		// Token: 0x04000035 RID: 53
		[AccessedThroughProperty("btnApply")]
		private Button _btnApply;

		// Token: 0x04000036 RID: 54
		[AccessedThroughProperty("lblRecordFiltered")]
		private Label _lblRecordFiltered;

		// Token: 0x04000037 RID: 55
		[AccessedThroughProperty("DataGrid1")]
		private DataGridView _DataGrid1;

		// Token: 0x04000038 RID: 56
		[AccessedThroughProperty("GroupBox4")]
		private GroupBox _GroupBox4;

		// Token: 0x04000039 RID: 57
		[AccessedThroughProperty("gpSound")]
		private GroupBox _gpSound;

		// Token: 0x0400003A RID: 58
		[AccessedThroughProperty("Label4")]
		private Label _Label4;

		// Token: 0x0400003B RID: 59
		[AccessedThroughProperty("txtWavName")]
		private TextBox _txtWavName;

		// Token: 0x0400003C RID: 60
		[AccessedThroughProperty("ndLevel")]
		private NumericUpDown _ndLevel;

		// Token: 0x0400003D RID: 61
		[AccessedThroughProperty("btnFile")]
		private Button _btnFile;

		// Token: 0x0400003E RID: 62
		[AccessedThroughProperty("gpTagSource")]
		private GroupBox _gpTagSource;

		// Token: 0x0400003F RID: 63
		[AccessedThroughProperty("GroupBox2")]
		private GroupBox _GroupBox2;

		// Token: 0x04000040 RID: 64
		[AccessedThroughProperty("cobType")]
		private ComboBox _cobType;

		// Token: 0x04000041 RID: 65
		[AccessedThroughProperty("lblType")]
		private Label _lblType;

		// Token: 0x04000042 RID: 66
		[AccessedThroughProperty("cobArea")]
		private ComboBox _cobArea;

		// Token: 0x04000043 RID: 67
		[AccessedThroughProperty("lblArea")]
		private Label _lblArea;

		// Token: 0x04000044 RID: 68
		[AccessedThroughProperty("txtTagFilter")]
		private TextBox _txtTagFilter;

		// Token: 0x04000045 RID: 69
		[AccessedThroughProperty("Label2")]
		private Label _Label2;

		// Token: 0x04000046 RID: 70
		[AccessedThroughProperty("cobNode")]
		private ComboBox _cobNode;

		// Token: 0x04000047 RID: 71
		[AccessedThroughProperty("Label1")]
		private Label _Label1;

		// Token: 0x04000048 RID: 72
		[AccessedThroughProperty("lblTagList")]
		private Label _lblTagList;

		// Token: 0x04000049 RID: 73
		[AccessedThroughProperty("txtNTF")]
		private TextBox _txtNTF;

		// Token: 0x0400004A RID: 74
		[AccessedThroughProperty("btnEnterMode")]
		private Button _btnEnterMode;

		// Token: 0x0400004B RID: 75
		[AccessedThroughProperty("lblDate")]
		private Label _lblDate;

		// Token: 0x0400004C RID: 76
		[AccessedThroughProperty("btnUpdate")]
		private Button _btnUpdate;

		// Token: 0x0400004D RID: 77
		[AccessedThroughProperty("comNTF")]
		private ListBox _comNTF;

		// Token: 0x0400004E RID: 78
		[AccessedThroughProperty("GroupBox6")]
		private GroupBox _GroupBox6;

		// Token: 0x0400004F RID: 79
		[AccessedThroughProperty("Label6")]
		private Label _Label6;

		// Token: 0x04000050 RID: 80
		[AccessedThroughProperty("Label7")]
		private Label _Label7;

		// Token: 0x04000051 RID: 81
		[AccessedThroughProperty("ndSoundTime")]
		private NumericUpDown _ndSoundTime;

		// Token: 0x04000052 RID: 82
		[AccessedThroughProperty("txtCfg")]
		private TextBox _txtCfg;

		// Token: 0x04000053 RID: 83
		[AccessedThroughProperty("Label9")]
		private Label _Label9;

		// Token: 0x04000054 RID: 84
		[AccessedThroughProperty("Label10")]
		private Label _Label10;

		// Token: 0x04000055 RID: 85
		[AccessedThroughProperty("GroupBox7")]
		private GroupBox _GroupBox7;

		// Token: 0x04000056 RID: 86
		[AccessedThroughProperty("ckDisBar")]
		private CheckBox _ckDisBar;

		// Token: 0x04000057 RID: 87
		[AccessedThroughProperty("ckBackGround")]
		private CheckBox _ckBackGround;

		// Token: 0x04000058 RID: 88
		[AccessedThroughProperty("GroupBox8")]
		private GroupBox _GroupBox8;

		// Token: 0x04000059 RID: 89
		[AccessedThroughProperty("_dsCFG")]
		private DataSet __dsCFG;

		// Token: 0x0400005A RID: 90
		[AccessedThroughProperty("DataTable1")]
		private DataTable _DataTable1;

		// Token: 0x0400005B RID: 91
		[AccessedThroughProperty("DataColumn1")]
		private DataColumn _DataColumn1;

		// Token: 0x0400005C RID: 92
		[AccessedThroughProperty("DataColumn2")]
		private DataColumn _DataColumn2;

		// Token: 0x0400005D RID: 93
		[AccessedThroughProperty("DataColumn3")]
		private DataColumn _DataColumn3;

		// Token: 0x0400005E RID: 94
		[AccessedThroughProperty("Timer1")]
		private Timer _Timer1;

		// Token: 0x0400005F RID: 95
		[AccessedThroughProperty("dlgOpen")]
		private OpenFileDialog _dlgOpen;

		// Token: 0x04000060 RID: 96
		[AccessedThroughProperty("tmrStatus")]
		private Timer _tmrStatus;

		// Token: 0x04000061 RID: 97
		[AccessedThroughProperty("dlgSave")]
		private SaveFileDialog _dlgSave;

		// Token: 0x04000062 RID: 98
		[AccessedThroughProperty("panelTime")]
		private StatusBarPanel _panelTime;

		// Token: 0x04000063 RID: 99
		[AccessedThroughProperty("panelStatus")]
		private StatusBarPanel _panelStatus;

		// Token: 0x04000064 RID: 100
		[AccessedThroughProperty("panelFileName")]
		private StatusBarPanel _panelFileName;

		// Token: 0x04000065 RID: 101
		[AccessedThroughProperty("kvStatusBar")]
		private StatusBar _kvStatusBar;

		// Token: 0x04000066 RID: 102
		[AccessedThroughProperty("btnCancle")]
		private Button _btnCancle;

		// Token: 0x04000067 RID: 103
		[AccessedThroughProperty("btnOK")]
		private Button _btnOK;

		// Token: 0x04000068 RID: 104
		[AccessedThroughProperty("ContextMenuStrip_Grid")]
		private ContextMenuStrip _ContextMenuStrip_Grid;

		// Token: 0x04000069 RID: 105
		[AccessedThroughProperty("miModifyAll")]
		private ToolStripMenuItem _miModifyAll;

		// Token: 0x0400006A RID: 106
		[AccessedThroughProperty("miModify")]
		private ToolStripMenuItem _miModify;

		// Token: 0x0400006B RID: 107
		[AccessedThroughProperty("ToolStripMenuItem1")]
		private ToolStripSeparator _ToolStripMenuItem1;

		// Token: 0x0400006C RID: 108
		[AccessedThroughProperty("miDelAll")]
		private ToolStripMenuItem _miDelAll;

		// Token: 0x0400006D RID: 109
		[AccessedThroughProperty("miDel")]
		private ToolStripMenuItem _miDel;

		// Token: 0x0400006E RID: 110
		[AccessedThroughProperty("MainMenu1")]
		private MenuStrip _MainMenu1;

		// Token: 0x0400006F RID: 111
		[AccessedThroughProperty("miFile")]
		private ToolStripMenuItem _miFile;

		// Token: 0x04000070 RID: 112
		[AccessedThroughProperty("miNew")]
		private ToolStripMenuItem _miNew;

		// Token: 0x04000071 RID: 113
		[AccessedThroughProperty("miOpen")]
		private ToolStripMenuItem _miOpen;

		// Token: 0x04000072 RID: 114
		[AccessedThroughProperty("toolStripSeparator")]
		private ToolStripSeparator _toolStripSeparator;

		// Token: 0x04000073 RID: 115
		[AccessedThroughProperty("miSave")]
		private ToolStripMenuItem _miSave;

		// Token: 0x04000074 RID: 116
		[AccessedThroughProperty("miSaveAs")]
		private ToolStripMenuItem _miSaveAs;

		// Token: 0x04000075 RID: 117
		[AccessedThroughProperty("toolStripSeparator1")]
		private ToolStripSeparator _toolStripSeparator1;

		// Token: 0x04000076 RID: 118
		[AccessedThroughProperty("toolStripSeparator2")]
		private ToolStripSeparator _toolStripSeparator2;

		// Token: 0x04000077 RID: 119
		[AccessedThroughProperty("miRecent")]
		private ToolStripMenuItem _miRecent;

		// Token: 0x04000078 RID: 120
		[AccessedThroughProperty("ToolStripMenuItem2")]
		private ToolStripSeparator _ToolStripMenuItem2;

		// Token: 0x04000079 RID: 121
		[AccessedThroughProperty("miDelCache")]
		private ToolStripMenuItem _miDelCache;

		// Token: 0x0400007A RID: 122
		[AccessedThroughProperty("miRecent1")]
		private ToolStripMenuItem _miRecent1;

		// Token: 0x0400007B RID: 123
		[AccessedThroughProperty("miRecent2")]
		private ToolStripMenuItem _miRecent2;

		// Token: 0x0400007C RID: 124
		[AccessedThroughProperty("miRecent3")]
		private ToolStripMenuItem _miRecent3;

		// Token: 0x0400007D RID: 125
		[AccessedThroughProperty("miRecent4")]
		private ToolStripMenuItem _miRecent4;

		// Token: 0x0400007E RID: 126
		[AccessedThroughProperty("miRun")]
		private ToolStripMenuItem _miRun;

		// Token: 0x0400007F RID: 127
		[AccessedThroughProperty("miReloadDB")]
		private ToolStripMenuItem _miReloadDB;

		// Token: 0x04000080 RID: 128
		[AccessedThroughProperty("ndScanTime")]
		private NumericUpDown _ndScanTime;

		// Token: 0x04000081 RID: 129
		[AccessedThroughProperty("Label3")]
		private Label _Label3;

		// Token: 0x04000082 RID: 130
		[AccessedThroughProperty("ndSecond")]
		private NumericUpDown _ndSecond;

		// Token: 0x04000083 RID: 131
		[AccessedThroughProperty("DataColumn4")]
		private DataColumn _DataColumn4;

		// Token: 0x04000084 RID: 132
		[AccessedThroughProperty("DataColumn5")]
		private DataColumn _DataColumn5;

		// Token: 0x04000085 RID: 133
		[AccessedThroughProperty("DataColumn6")]
		private DataColumn _DataColumn6;

		// Token: 0x04000086 RID: 134
		[AccessedThroughProperty("DataColumn7")]
		private DataColumn _DataColumn7;

		// Token: 0x04000087 RID: 135
		[AccessedThroughProperty("Label5")]
		private Label _Label5;

		// Token: 0x04000088 RID: 136
		[AccessedThroughProperty("cobPriority")]
		private ComboBox _cobPriority;

		// Token: 0x04000089 RID: 137
		[AccessedThroughProperty("gpPicture")]
		private GroupBox _gpPicture;

		// Token: 0x0400008A RID: 138
		[AccessedThroughProperty("txtPicName")]
		private TextBox _txtPicName;

		// Token: 0x0400008B RID: 139
		[AccessedThroughProperty("btnPic")]
		private Button _btnPic;

		// Token: 0x0400008C RID: 140
		[AccessedThroughProperty("gpOutSource")]
		private GroupBox _gpOutSource;

		// Token: 0x0400008D RID: 141
		[AccessedThroughProperty("txtValue")]
		private TextBox _txtValue;

		// Token: 0x0400008E RID: 142
		[AccessedThroughProperty("txtSource")]
		private TextBox _txtSource;

		// Token: 0x0400008F RID: 143
		[AccessedThroughProperty("gpAction")]
		private GroupBox _gpAction;

		// Token: 0x04000090 RID: 144
		[AccessedThroughProperty("Label11")]
		private Label _Label11;

		// Token: 0x04000091 RID: 145
		[AccessedThroughProperty("Label8")]
		private Label _Label8;

		// Token: 0x04000092 RID: 146
		[AccessedThroughProperty("GroupBox1")]
		private GroupBox _GroupBox1;

		// Token: 0x04000093 RID: 147
		[AccessedThroughProperty("DataColumn8")]
		private DataColumn _DataColumn8;

		// Token: 0x04000094 RID: 148
		[AccessedThroughProperty("DataColumn9")]
		private DataColumn _DataColumn9;

		// Token: 0x04000095 RID: 149
		[AccessedThroughProperty("DataColumn10")]
		private DataColumn _DataColumn10;

		// Token: 0x04000096 RID: 150
		[AccessedThroughProperty("NodeDataGridViewTextBoxColumn")]
		private DataGridViewTextBoxColumn _NodeDataGridViewTextBoxColumn;

		// Token: 0x04000097 RID: 151
		[AccessedThroughProperty("TagDataGridViewTextBoxColumn")]
		private DataGridViewTextBoxColumn _TagDataGridViewTextBoxColumn;

		// Token: 0x04000098 RID: 152
		[AccessedThroughProperty("clField")]
		private DataGridViewTextBoxColumn _clField;

		// Token: 0x04000099 RID: 153
		[AccessedThroughProperty("clWave")]
		private DataGridViewTextBoxColumn _clWave;

		// Token: 0x0400009A RID: 154
		[AccessedThroughProperty("clnOpenWave")]
		private DataGridViewButtonColumn _clnOpenWave;

		// Token: 0x0400009B RID: 155
		[AccessedThroughProperty("clPriority")]
		private DataGridViewComboBoxColumn _clPriority;

		// Token: 0x0400009C RID: 156
		[AccessedThroughProperty("clBlockType")]
		private DataGridViewTextBoxColumn _clBlockType;

		// Token: 0x0400009D RID: 157
		[AccessedThroughProperty("clLength")]
		private DataGridViewComboBoxColumn _clLength;

		// Token: 0x0400009E RID: 158
		[AccessedThroughProperty("clPicture")]
		private DataGridViewTextBoxColumn _clPicture;

		// Token: 0x0400009F RID: 159
		[AccessedThroughProperty("clnOpenPic")]
		private DataGridViewButtonColumn _clnOpenPic;

		// Token: 0x040000A0 RID: 160
		[AccessedThroughProperty("clOutSource")]
		private DataGridViewTextBoxColumn _clOutSource;

		// Token: 0x040000A1 RID: 161
		[AccessedThroughProperty("clOutValue")]
		private DataGridViewTextBoxColumn _clOutValue;

		// Token: 0x040000A2 RID: 162
		public FileInfo _myFile;

		// Token: 0x040000A3 RID: 163
		private bool _bFormInitDone;

		// Token: 0x040000A4 RID: 164
		private bool _bFileReading;

		// Token: 0x040000A5 RID: 165
		private bool _bDocChanged;

		// Token: 0x040000A6 RID: 166
		private DataSet _dsXML;

		// Token: 0x040000A7 RID: 167
		private DataTable _dtTag;

		// Token: 0x040000A8 RID: 168
		private DataTable _dtConfig;

		// Token: 0x040000A9 RID: 169
		private int _igvdLines;

		// Token: 0x040000AA RID: 170
		private bool _bNotShowWarnning;

		// Token: 0x040000AB RID: 171
		private const string _cTblTag = "Tag";
	}
}
