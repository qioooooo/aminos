using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000545 RID: 1349
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class DataGridPagerStyle : TableItemStyle
	{
		// Token: 0x06004277 RID: 17015 RVA: 0x00113620 File Offset: 0x00112620
		internal DataGridPagerStyle(DataGrid owner)
		{
			this.owner = owner;
		}

		// Token: 0x17001011 RID: 4113
		// (get) Token: 0x06004278 RID: 17016 RVA: 0x00113630 File Offset: 0x00112630
		internal bool IsPagerOnBottom
		{
			get
			{
				PagerPosition position = this.Position;
				return position == PagerPosition.Bottom || position == PagerPosition.TopAndBottom;
			}
		}

		// Token: 0x17001012 RID: 4114
		// (get) Token: 0x06004279 RID: 17017 RVA: 0x00113650 File Offset: 0x00112650
		internal bool IsPagerOnTop
		{
			get
			{
				PagerPosition position = this.Position;
				return position == PagerPosition.Top || position == PagerPosition.TopAndBottom;
			}
		}

		// Token: 0x17001013 RID: 4115
		// (get) Token: 0x0600427A RID: 17018 RVA: 0x0011366E File Offset: 0x0011266E
		// (set) Token: 0x0600427B RID: 17019 RVA: 0x00113694 File Offset: 0x00112694
		[DefaultValue(PagerMode.NextPrev)]
		[WebSysDescription("DataGridPagerStyle_Mode")]
		[WebCategory("Appearance")]
		[NotifyParentProperty(true)]
		public PagerMode Mode
		{
			get
			{
				if (base.IsSet(524288))
				{
					return (PagerMode)base.ViewState["Mode"];
				}
				return PagerMode.NextPrev;
			}
			set
			{
				if (value < PagerMode.NextPrev || value > PagerMode.NumericPages)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				base.ViewState["Mode"] = value;
				this.SetBit(524288);
				this.owner.OnPagerChanged();
			}
		}

		// Token: 0x17001014 RID: 4116
		// (get) Token: 0x0600427C RID: 17020 RVA: 0x001136E0 File Offset: 0x001126E0
		// (set) Token: 0x0600427D RID: 17021 RVA: 0x0011370A File Offset: 0x0011270A
		[WebCategory("Appearance")]
		[Localizable(true)]
		[DefaultValue("&gt;")]
		[WebSysDescription("PagerSettings_NextPageText")]
		[NotifyParentProperty(true)]
		public string NextPageText
		{
			get
			{
				if (base.IsSet(1048576))
				{
					return (string)base.ViewState["NextPageText"];
				}
				return "&gt;";
			}
			set
			{
				base.ViewState["NextPageText"] = value;
				this.SetBit(1048576);
				this.owner.OnPagerChanged();
			}
		}

		// Token: 0x17001015 RID: 4117
		// (get) Token: 0x0600427E RID: 17022 RVA: 0x00113733 File Offset: 0x00112733
		// (set) Token: 0x0600427F RID: 17023 RVA: 0x0011375A File Offset: 0x0011275A
		[WebSysDescription("DataGridPagerStyle_PageButtonCount")]
		[DefaultValue(10)]
		[NotifyParentProperty(true)]
		[WebCategory("Behavior")]
		public int PageButtonCount
		{
			get
			{
				if (base.IsSet(4194304))
				{
					return (int)base.ViewState["PageButtonCount"];
				}
				return 10;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				base.ViewState["PageButtonCount"] = value;
				this.SetBit(4194304);
				this.owner.OnPagerChanged();
			}
		}

		// Token: 0x17001016 RID: 4118
		// (get) Token: 0x06004280 RID: 17024 RVA: 0x00113797 File Offset: 0x00112797
		// (set) Token: 0x06004281 RID: 17025 RVA: 0x001137C0 File Offset: 0x001127C0
		[WebSysDescription("DataGridPagerStyle_Position")]
		[NotifyParentProperty(true)]
		[WebCategory("Layout")]
		[DefaultValue(PagerPosition.Bottom)]
		public PagerPosition Position
		{
			get
			{
				if (base.IsSet(8388608))
				{
					return (PagerPosition)base.ViewState["Position"];
				}
				return PagerPosition.Bottom;
			}
			set
			{
				if (value < PagerPosition.Bottom || value > PagerPosition.TopAndBottom)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				base.ViewState["Position"] = value;
				this.SetBit(8388608);
				this.owner.OnPagerChanged();
			}
		}

		// Token: 0x17001017 RID: 4119
		// (get) Token: 0x06004282 RID: 17026 RVA: 0x0011380C File Offset: 0x0011280C
		// (set) Token: 0x06004283 RID: 17027 RVA: 0x00113836 File Offset: 0x00112836
		[Localizable(true)]
		[WebSysDescription("PagerSettings_PreviousPageText")]
		[WebCategory("Appearance")]
		[DefaultValue("&lt;")]
		[NotifyParentProperty(true)]
		public string PrevPageText
		{
			get
			{
				if (base.IsSet(2097152))
				{
					return (string)base.ViewState["PrevPageText"];
				}
				return "&lt;";
			}
			set
			{
				base.ViewState["PrevPageText"] = value;
				this.SetBit(2097152);
				this.owner.OnPagerChanged();
			}
		}

		// Token: 0x17001018 RID: 4120
		// (get) Token: 0x06004284 RID: 17028 RVA: 0x0011385F File Offset: 0x0011285F
		// (set) Token: 0x06004285 RID: 17029 RVA: 0x00113885 File Offset: 0x00112885
		[NotifyParentProperty(true)]
		[WebCategory("Appearance")]
		[DefaultValue(true)]
		[WebSysDescription("DataGridPagerStyle_Visible")]
		public bool Visible
		{
			get
			{
				return !base.IsSet(16777216) || (bool)base.ViewState["PagerVisible"];
			}
			set
			{
				base.ViewState["PagerVisible"] = value;
				this.SetBit(16777216);
				this.owner.OnPagerChanged();
			}
		}

		// Token: 0x06004286 RID: 17030 RVA: 0x001138B4 File Offset: 0x001128B4
		public override void CopyFrom(Style s)
		{
			if (s != null && !s.IsEmpty)
			{
				base.CopyFrom(s);
				if (s is DataGridPagerStyle)
				{
					DataGridPagerStyle dataGridPagerStyle = (DataGridPagerStyle)s;
					if (dataGridPagerStyle.IsSet(524288))
					{
						this.Mode = dataGridPagerStyle.Mode;
					}
					if (dataGridPagerStyle.IsSet(1048576))
					{
						this.NextPageText = dataGridPagerStyle.NextPageText;
					}
					if (dataGridPagerStyle.IsSet(2097152))
					{
						this.PrevPageText = dataGridPagerStyle.PrevPageText;
					}
					if (dataGridPagerStyle.IsSet(4194304))
					{
						this.PageButtonCount = dataGridPagerStyle.PageButtonCount;
					}
					if (dataGridPagerStyle.IsSet(8388608))
					{
						this.Position = dataGridPagerStyle.Position;
					}
					if (dataGridPagerStyle.IsSet(16777216))
					{
						this.Visible = dataGridPagerStyle.Visible;
					}
				}
			}
		}

		// Token: 0x06004287 RID: 17031 RVA: 0x00113984 File Offset: 0x00112984
		public override void MergeWith(Style s)
		{
			if (s != null && !s.IsEmpty)
			{
				if (this.IsEmpty)
				{
					this.CopyFrom(s);
					return;
				}
				base.MergeWith(s);
				if (s is DataGridPagerStyle)
				{
					DataGridPagerStyle dataGridPagerStyle = (DataGridPagerStyle)s;
					if (dataGridPagerStyle.IsSet(524288) && !base.IsSet(524288))
					{
						this.Mode = dataGridPagerStyle.Mode;
					}
					if (dataGridPagerStyle.IsSet(1048576) && !base.IsSet(1048576))
					{
						this.NextPageText = dataGridPagerStyle.NextPageText;
					}
					if (dataGridPagerStyle.IsSet(2097152) && !base.IsSet(2097152))
					{
						this.PrevPageText = dataGridPagerStyle.PrevPageText;
					}
					if (dataGridPagerStyle.IsSet(4194304) && !base.IsSet(4194304))
					{
						this.PageButtonCount = dataGridPagerStyle.PageButtonCount;
					}
					if (dataGridPagerStyle.IsSet(8388608) && !base.IsSet(8388608))
					{
						this.Position = dataGridPagerStyle.Position;
					}
					if (dataGridPagerStyle.IsSet(16777216) && !base.IsSet(16777216))
					{
						this.Visible = dataGridPagerStyle.Visible;
					}
				}
			}
		}

		// Token: 0x06004288 RID: 17032 RVA: 0x00113AB0 File Offset: 0x00112AB0
		public override void Reset()
		{
			if (base.IsSet(524288))
			{
				base.ViewState.Remove("Mode");
			}
			if (base.IsSet(1048576))
			{
				base.ViewState.Remove("NextPageText");
			}
			if (base.IsSet(2097152))
			{
				base.ViewState.Remove("PrevPageText");
			}
			if (base.IsSet(4194304))
			{
				base.ViewState.Remove("PageButtonCount");
			}
			if (base.IsSet(8388608))
			{
				base.ViewState.Remove("Position");
			}
			if (base.IsSet(16777216))
			{
				base.ViewState.Remove("PagerVisible");
			}
			base.Reset();
		}

		// Token: 0x04002914 RID: 10516
		private const int PROP_MODE = 524288;

		// Token: 0x04002915 RID: 10517
		private const int PROP_NEXTPAGETEXT = 1048576;

		// Token: 0x04002916 RID: 10518
		private const int PROP_PREVPAGETEXT = 2097152;

		// Token: 0x04002917 RID: 10519
		private const int PROP_PAGEBUTTONCOUNT = 4194304;

		// Token: 0x04002918 RID: 10520
		private const int PROP_POSITION = 8388608;

		// Token: 0x04002919 RID: 10521
		private const int PROP_VISIBLE = 16777216;

		// Token: 0x0400291A RID: 10522
		private DataGrid owner;
	}
}
