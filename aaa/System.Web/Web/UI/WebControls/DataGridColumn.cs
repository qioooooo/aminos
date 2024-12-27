using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004CC RID: 1228
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class DataGridColumn : IStateManager
	{
		// Token: 0x06003ADB RID: 15067 RVA: 0x000F8525 File Offset: 0x000F7525
		protected DataGridColumn()
		{
			this.statebag = new StateBag();
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x06003ADC RID: 15068 RVA: 0x000F8538 File Offset: 0x000F7538
		protected bool DesignMode
		{
			get
			{
				return this.owner != null && this.owner.DesignMode;
			}
		}

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x06003ADD RID: 15069 RVA: 0x000F854F File Offset: 0x000F754F
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebSysDescription("DataGridColumn_FooterStyle")]
		[WebCategory("Styles")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual TableItemStyle FooterStyle
		{
			get
			{
				if (this.footerStyle == null)
				{
					this.footerStyle = new TableItemStyle();
					if (this.IsTrackingViewState)
					{
						((IStateManager)this.footerStyle).TrackViewState();
					}
				}
				return this.footerStyle;
			}
		}

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x06003ADE RID: 15070 RVA: 0x000F857D File Offset: 0x000F757D
		internal TableItemStyle FooterStyleInternal
		{
			get
			{
				return this.footerStyle;
			}
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x06003ADF RID: 15071 RVA: 0x000F8588 File Offset: 0x000F7588
		// (set) Token: 0x06003AE0 RID: 15072 RVA: 0x000F85B5 File Offset: 0x000F75B5
		[WebSysDescription("DataGridColumn_FooterText")]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		public virtual string FooterText
		{
			get
			{
				object obj = this.ViewState["FooterText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["FooterText"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x06003AE1 RID: 15073 RVA: 0x000F85D0 File Offset: 0x000F75D0
		// (set) Token: 0x06003AE2 RID: 15074 RVA: 0x000F85FD File Offset: 0x000F75FD
		[WebSysDescription("DataGridColumn_HeaderImageUrl")]
		[DefaultValue("")]
		[UrlProperty]
		[WebCategory("Appearance")]
		public virtual string HeaderImageUrl
		{
			get
			{
				object obj = this.ViewState["HeaderImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["HeaderImageUrl"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x06003AE3 RID: 15075 RVA: 0x000F8616 File Offset: 0x000F7616
		[WebSysDescription("DataGridColumn_HeaderStyle")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DefaultValue(null)]
		[WebCategory("Styles")]
		public virtual TableItemStyle HeaderStyle
		{
			get
			{
				if (this.headerStyle == null)
				{
					this.headerStyle = new TableItemStyle();
					if (this.IsTrackingViewState)
					{
						((IStateManager)this.headerStyle).TrackViewState();
					}
				}
				return this.headerStyle;
			}
		}

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x06003AE4 RID: 15076 RVA: 0x000F8644 File Offset: 0x000F7644
		internal TableItemStyle HeaderStyleInternal
		{
			get
			{
				return this.headerStyle;
			}
		}

		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x06003AE5 RID: 15077 RVA: 0x000F864C File Offset: 0x000F764C
		// (set) Token: 0x06003AE6 RID: 15078 RVA: 0x000F8679 File Offset: 0x000F7679
		[WebCategory("Appearance")]
		[DefaultValue("")]
		[WebSysDescription("DataGridColumn_HeaderText")]
		public virtual string HeaderText
		{
			get
			{
				object obj = this.ViewState["HeaderText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["HeaderText"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x06003AE7 RID: 15079 RVA: 0x000F8692 File Offset: 0x000F7692
		[WebSysDescription("DataGridColumn_ItemStyle")]
		[DefaultValue(null)]
		[WebCategory("Styles")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual TableItemStyle ItemStyle
		{
			get
			{
				if (this.itemStyle == null)
				{
					this.itemStyle = new TableItemStyle();
					if (this.IsTrackingViewState)
					{
						((IStateManager)this.itemStyle).TrackViewState();
					}
				}
				return this.itemStyle;
			}
		}

		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x06003AE8 RID: 15080 RVA: 0x000F86C0 File Offset: 0x000F76C0
		internal TableItemStyle ItemStyleInternal
		{
			get
			{
				return this.itemStyle;
			}
		}

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x06003AE9 RID: 15081 RVA: 0x000F86C8 File Offset: 0x000F76C8
		protected DataGrid Owner
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x06003AEA RID: 15082 RVA: 0x000F86D0 File Offset: 0x000F76D0
		// (set) Token: 0x06003AEB RID: 15083 RVA: 0x000F86FD File Offset: 0x000F76FD
		[WebSysDescription("DataGridColumn_SortExpression")]
		[DefaultValue("")]
		[WebCategory("Behavior")]
		public virtual string SortExpression
		{
			get
			{
				object obj = this.ViewState["SortExpression"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["SortExpression"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x06003AEC RID: 15084 RVA: 0x000F8716 File Offset: 0x000F7716
		protected StateBag ViewState
		{
			get
			{
				return this.statebag;
			}
		}

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x06003AED RID: 15085 RVA: 0x000F8720 File Offset: 0x000F7720
		// (set) Token: 0x06003AEE RID: 15086 RVA: 0x000F8749 File Offset: 0x000F7749
		[WebSysDescription("DataGridColumn_Visible")]
		[WebCategory("Behavior")]
		[DefaultValue(true)]
		public bool Visible
		{
			get
			{
				object obj = this.ViewState["Visible"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["Visible"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x000F8767 File Offset: 0x000F7767
		public virtual void Initialize()
		{
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x000F876C File Offset: 0x000F776C
		public virtual void InitializeCell(TableCell cell, int columnIndex, ListItemType itemType)
		{
			switch (itemType)
			{
			case ListItemType.Header:
			{
				WebControl webControl = null;
				bool flag = true;
				string text = null;
				if (this.owner != null && !this.owner.AllowSorting)
				{
					flag = false;
				}
				if (flag)
				{
					text = this.SortExpression;
					if (text.Length == 0)
					{
						flag = false;
					}
				}
				string headerImageUrl = this.HeaderImageUrl;
				if (headerImageUrl.Length != 0)
				{
					if (flag)
					{
						webControl = new ImageButton
						{
							ImageUrl = this.HeaderImageUrl,
							CommandName = "Sort",
							CommandArgument = text,
							CausesValidation = false
						};
					}
					else
					{
						webControl = new Image
						{
							ImageUrl = headerImageUrl
						};
					}
				}
				else
				{
					string text2 = this.HeaderText;
					if (flag)
					{
						webControl = new DataGridLinkButton
						{
							Text = text2,
							CommandName = "Sort",
							CommandArgument = text,
							CausesValidation = false
						};
					}
					else
					{
						if (text2.Length == 0)
						{
							text2 = "&nbsp;";
						}
						cell.Text = text2;
					}
				}
				if (webControl != null)
				{
					cell.Controls.Add(webControl);
					return;
				}
				break;
			}
			case ListItemType.Footer:
			{
				string text3 = this.FooterText;
				if (text3.Length == 0)
				{
					text3 = "&nbsp;";
				}
				cell.Text = text3;
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x06003AF1 RID: 15089 RVA: 0x000F889C File Offset: 0x000F789C
		protected bool IsTrackingViewState
		{
			get
			{
				return this.marked;
			}
		}

		// Token: 0x06003AF2 RID: 15090 RVA: 0x000F88A4 File Offset: 0x000F78A4
		protected virtual void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				object[] array = (object[])savedState;
				if (array[0] != null)
				{
					((IStateManager)this.ViewState).LoadViewState(array[0]);
				}
				if (array[1] != null)
				{
					((IStateManager)this.ItemStyle).LoadViewState(array[1]);
				}
				if (array[2] != null)
				{
					((IStateManager)this.HeaderStyle).LoadViewState(array[2]);
				}
				if (array[3] != null)
				{
					((IStateManager)this.FooterStyle).LoadViewState(array[3]);
				}
			}
		}

		// Token: 0x06003AF3 RID: 15091 RVA: 0x000F8908 File Offset: 0x000F7908
		protected virtual void TrackViewState()
		{
			this.marked = true;
			((IStateManager)this.ViewState).TrackViewState();
			if (this.itemStyle != null)
			{
				((IStateManager)this.itemStyle).TrackViewState();
			}
			if (this.headerStyle != null)
			{
				((IStateManager)this.headerStyle).TrackViewState();
			}
			if (this.footerStyle != null)
			{
				((IStateManager)this.footerStyle).TrackViewState();
			}
		}

		// Token: 0x06003AF4 RID: 15092 RVA: 0x000F8960 File Offset: 0x000F7960
		protected virtual void OnColumnChanged()
		{
			if (this.owner != null)
			{
				this.owner.OnColumnsChanged();
			}
		}

		// Token: 0x06003AF5 RID: 15093 RVA: 0x000F8978 File Offset: 0x000F7978
		protected virtual object SaveViewState()
		{
			object obj = ((IStateManager)this.ViewState).SaveViewState();
			object obj2 = ((this.itemStyle != null) ? ((IStateManager)this.itemStyle).SaveViewState() : null);
			object obj3 = ((this.headerStyle != null) ? ((IStateManager)this.headerStyle).SaveViewState() : null);
			object obj4 = ((this.footerStyle != null) ? ((IStateManager)this.footerStyle).SaveViewState() : null);
			if (obj != null || obj2 != null || obj3 != null || obj4 != null)
			{
				return new object[] { obj, obj2, obj3, obj4 };
			}
			return null;
		}

		// Token: 0x06003AF6 RID: 15094 RVA: 0x000F8A02 File Offset: 0x000F7A02
		internal void SetOwner(DataGrid owner)
		{
			this.owner = owner;
		}

		// Token: 0x06003AF7 RID: 15095 RVA: 0x000F8A0B File Offset: 0x000F7A0B
		public override string ToString()
		{
			return string.Empty;
		}

		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x06003AF8 RID: 15096 RVA: 0x000F8A12 File Offset: 0x000F7A12
		bool IStateManager.IsTrackingViewState
		{
			get
			{
				return this.IsTrackingViewState;
			}
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x000F8A1A File Offset: 0x000F7A1A
		void IStateManager.LoadViewState(object state)
		{
			this.LoadViewState(state);
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x000F8A23 File Offset: 0x000F7A23
		void IStateManager.TrackViewState()
		{
			this.TrackViewState();
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x000F8A2B File Offset: 0x000F7A2B
		object IStateManager.SaveViewState()
		{
			return this.SaveViewState();
		}

		// Token: 0x040026A3 RID: 9891
		private DataGrid owner;

		// Token: 0x040026A4 RID: 9892
		private TableItemStyle itemStyle;

		// Token: 0x040026A5 RID: 9893
		private TableItemStyle headerStyle;

		// Token: 0x040026A6 RID: 9894
		private TableItemStyle footerStyle;

		// Token: 0x040026A7 RID: 9895
		private StateBag statebag;

		// Token: 0x040026A8 RID: 9896
		private bool marked;
	}
}
