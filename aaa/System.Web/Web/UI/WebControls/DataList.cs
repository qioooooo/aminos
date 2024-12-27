using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200054B RID: 1355
	[Designer("System.Web.UI.Design.WebControls.DataListDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ControlValueProperty("SelectedValue")]
	[Editor("System.Web.UI.Design.WebControls.DataListComponentEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(ComponentEditor))]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DataList : BaseDataList, INamingContainer, IRepeatInfoUser
	{
		// Token: 0x060042B3 RID: 17075 RVA: 0x00113F99 File Offset: 0x00112F99
		public DataList()
		{
			this.offset = 0;
			this.visibleItemCount = -1;
		}

		// Token: 0x1700102C RID: 4140
		// (get) Token: 0x060042B4 RID: 17076 RVA: 0x00113FB6 File Offset: 0x00112FB6
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[WebSysDescription("DataList_AlternatingItemStyle")]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual TableItemStyle AlternatingItemStyle
		{
			get
			{
				if (this.alternatingItemStyle == null)
				{
					this.alternatingItemStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.alternatingItemStyle).TrackViewState();
					}
				}
				return this.alternatingItemStyle;
			}
		}

		// Token: 0x1700102D RID: 4141
		// (get) Token: 0x060042B5 RID: 17077 RVA: 0x00113FE4 File Offset: 0x00112FE4
		// (set) Token: 0x060042B6 RID: 17078 RVA: 0x00113FEC File Offset: 0x00112FEC
		[Browsable(false)]
		[WebSysDescription("DataList_AlternatingItemTemplate")]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(DataListItem))]
		public virtual ITemplate AlternatingItemTemplate
		{
			get
			{
				return this.alternatingItemTemplate;
			}
			set
			{
				this.alternatingItemTemplate = value;
			}
		}

		// Token: 0x1700102E RID: 4142
		// (get) Token: 0x060042B7 RID: 17079 RVA: 0x00113FF8 File Offset: 0x00112FF8
		// (set) Token: 0x060042B8 RID: 17080 RVA: 0x00114021 File Offset: 0x00113021
		[WebCategory("Default")]
		[WebSysDescription("DataList_EditItemIndex")]
		[DefaultValue(-1)]
		public virtual int EditItemIndex
		{
			get
			{
				object obj = this.ViewState["EditItemIndex"];
				if (obj != null)
				{
					return (int)obj;
				}
				return -1;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["EditItemIndex"] = value;
			}
		}

		// Token: 0x1700102F RID: 4143
		// (get) Token: 0x060042B9 RID: 17081 RVA: 0x00114048 File Offset: 0x00113048
		[WebSysDescription("DataList_EditItemStyle")]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual TableItemStyle EditItemStyle
		{
			get
			{
				if (this.editItemStyle == null)
				{
					this.editItemStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.editItemStyle).TrackViewState();
					}
				}
				return this.editItemStyle;
			}
		}

		// Token: 0x17001030 RID: 4144
		// (get) Token: 0x060042BA RID: 17082 RVA: 0x00114076 File Offset: 0x00113076
		// (set) Token: 0x060042BB RID: 17083 RVA: 0x0011407E File Offset: 0x0011307E
		[TemplateContainer(typeof(DataListItem))]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("DataList_EditItemTemplate")]
		[DefaultValue(null)]
		[Browsable(false)]
		public virtual ITemplate EditItemTemplate
		{
			get
			{
				return this.editItemTemplate;
			}
			set
			{
				this.editItemTemplate = value;
			}
		}

		// Token: 0x17001031 RID: 4145
		// (get) Token: 0x060042BC RID: 17084 RVA: 0x00114088 File Offset: 0x00113088
		// (set) Token: 0x060042BD RID: 17085 RVA: 0x001140B1 File Offset: 0x001130B1
		[WebSysDescription("DataList_ExtractTemplateRows")]
		[WebCategory("Layout")]
		[DefaultValue(false)]
		public virtual bool ExtractTemplateRows
		{
			get
			{
				object obj = this.ViewState["ExtractTemplateRows"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["ExtractTemplateRows"] = value;
			}
		}

		// Token: 0x17001032 RID: 4146
		// (get) Token: 0x060042BE RID: 17086 RVA: 0x001140C9 File Offset: 0x001130C9
		[WebCategory("Styles")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("DataControls_FooterStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		public virtual TableItemStyle FooterStyle
		{
			get
			{
				if (this.footerStyle == null)
				{
					this.footerStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.footerStyle).TrackViewState();
					}
				}
				return this.footerStyle;
			}
		}

		// Token: 0x17001033 RID: 4147
		// (get) Token: 0x060042BF RID: 17087 RVA: 0x001140F7 File Offset: 0x001130F7
		// (set) Token: 0x060042C0 RID: 17088 RVA: 0x001140FF File Offset: 0x001130FF
		[WebSysDescription("DataList_FooterTemplate")]
		[Browsable(false)]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(DataListItem))]
		public virtual ITemplate FooterTemplate
		{
			get
			{
				return this.footerTemplate;
			}
			set
			{
				this.footerTemplate = value;
			}
		}

		// Token: 0x17001034 RID: 4148
		// (get) Token: 0x060042C1 RID: 17089 RVA: 0x00114108 File Offset: 0x00113108
		// (set) Token: 0x060042C2 RID: 17090 RVA: 0x00114124 File Offset: 0x00113124
		[DefaultValue(GridLines.None)]
		public override GridLines GridLines
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return GridLines.None;
				}
				return ((TableStyle)base.ControlStyle).GridLines;
			}
			set
			{
				base.GridLines = value;
			}
		}

		// Token: 0x17001035 RID: 4149
		// (get) Token: 0x060042C3 RID: 17091 RVA: 0x0011412D File Offset: 0x0011312D
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("DataControls_HeaderStyle")]
		[NotifyParentProperty(true)]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public virtual TableItemStyle HeaderStyle
		{
			get
			{
				if (this.headerStyle == null)
				{
					this.headerStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.headerStyle).TrackViewState();
					}
				}
				return this.headerStyle;
			}
		}

		// Token: 0x17001036 RID: 4150
		// (get) Token: 0x060042C4 RID: 17092 RVA: 0x0011415B File Offset: 0x0011315B
		// (set) Token: 0x060042C5 RID: 17093 RVA: 0x00114163 File Offset: 0x00113163
		[TemplateContainer(typeof(DataListItem))]
		[Browsable(false)]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("DataList_HeaderTemplate")]
		public virtual ITemplate HeaderTemplate
		{
			get
			{
				return this.headerTemplate;
			}
			set
			{
				this.headerTemplate = value;
			}
		}

		// Token: 0x17001037 RID: 4151
		// (get) Token: 0x060042C6 RID: 17094 RVA: 0x0011416C File Offset: 0x0011316C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[WebSysDescription("DataList_Items")]
		public virtual DataListItemCollection Items
		{
			get
			{
				if (this.itemsCollection == null)
				{
					if (this.itemsArray == null)
					{
						this.EnsureChildControls();
					}
					if (this.itemsArray == null)
					{
						this.itemsArray = new ArrayList();
					}
					this.itemsCollection = new DataListItemCollection(this.itemsArray);
				}
				return this.itemsCollection;
			}
		}

		// Token: 0x17001038 RID: 4152
		// (get) Token: 0x060042C7 RID: 17095 RVA: 0x001141B9 File Offset: 0x001131B9
		[WebCategory("Styles")]
		[WebSysDescription("DataList_ItemStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual TableItemStyle ItemStyle
		{
			get
			{
				if (this.itemStyle == null)
				{
					this.itemStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.itemStyle).TrackViewState();
					}
				}
				return this.itemStyle;
			}
		}

		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x060042C8 RID: 17096 RVA: 0x001141E7 File Offset: 0x001131E7
		// (set) Token: 0x060042C9 RID: 17097 RVA: 0x001141EF File Offset: 0x001131EF
		[WebSysDescription("DataList_ItemTemplate")]
		[Browsable(false)]
		[TemplateContainer(typeof(DataListItem))]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual ITemplate ItemTemplate
		{
			get
			{
				return this.itemTemplate;
			}
			set
			{
				this.itemTemplate = value;
			}
		}

		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x060042CA RID: 17098 RVA: 0x001141F8 File Offset: 0x001131F8
		// (set) Token: 0x060042CB RID: 17099 RVA: 0x00114221 File Offset: 0x00113221
		[DefaultValue(0)]
		[WebCategory("Layout")]
		[WebSysDescription("DataList_RepeatColumns")]
		public virtual int RepeatColumns
		{
			get
			{
				object obj = this.ViewState["RepeatColumns"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 0;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["RepeatColumns"] = value;
			}
		}

		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x060042CC RID: 17100 RVA: 0x00114248 File Offset: 0x00113248
		// (set) Token: 0x060042CD RID: 17101 RVA: 0x00114271 File Offset: 0x00113271
		[WebCategory("Layout")]
		[DefaultValue(RepeatDirection.Vertical)]
		[WebSysDescription("Item_RepeatDirection")]
		public virtual RepeatDirection RepeatDirection
		{
			get
			{
				object obj = this.ViewState["RepeatDirection"];
				if (obj != null)
				{
					return (RepeatDirection)obj;
				}
				return RepeatDirection.Vertical;
			}
			set
			{
				if (value < RepeatDirection.Horizontal || value > RepeatDirection.Vertical)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["RepeatDirection"] = value;
			}
		}

		// Token: 0x1700103C RID: 4156
		// (get) Token: 0x060042CE RID: 17102 RVA: 0x0011429C File Offset: 0x0011329C
		// (set) Token: 0x060042CF RID: 17103 RVA: 0x001142C5 File Offset: 0x001132C5
		[WebCategory("Layout")]
		[DefaultValue(RepeatLayout.Table)]
		[WebSysDescription("WebControl_RepeatLayout")]
		public virtual RepeatLayout RepeatLayout
		{
			get
			{
				object obj = this.ViewState["RepeatLayout"];
				if (obj != null)
				{
					return (RepeatLayout)obj;
				}
				return RepeatLayout.Table;
			}
			set
			{
				if (value < RepeatLayout.Table || value > RepeatLayout.Flow)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["RepeatLayout"] = value;
			}
		}

		// Token: 0x1700103D RID: 4157
		// (get) Token: 0x060042D0 RID: 17104 RVA: 0x001142F0 File Offset: 0x001132F0
		// (set) Token: 0x060042D1 RID: 17105 RVA: 0x0011431C File Offset: 0x0011331C
		[DefaultValue(-1)]
		[WebSysDescription("WebControl_SelectedIndex")]
		[Bindable(true)]
		public virtual int SelectedIndex
		{
			get
			{
				object obj = this.ViewState["SelectedIndex"];
				if (obj != null)
				{
					return (int)obj;
				}
				return -1;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				int selectedIndex = this.SelectedIndex;
				this.ViewState["SelectedIndex"] = value;
				if (this.itemsArray != null)
				{
					if (selectedIndex != -1 && this.itemsArray.Count > selectedIndex)
					{
						DataListItem dataListItem = (DataListItem)this.itemsArray[selectedIndex];
						if (dataListItem.ItemType != ListItemType.EditItem)
						{
							ListItemType listItemType = ListItemType.Item;
							if (selectedIndex % 2 != 0)
							{
								listItemType = ListItemType.AlternatingItem;
							}
							dataListItem.SetItemType(listItemType);
						}
					}
					if (value != -1 && this.itemsArray.Count > value)
					{
						DataListItem dataListItem = (DataListItem)this.itemsArray[value];
						if (dataListItem.ItemType != ListItemType.EditItem)
						{
							dataListItem.SetItemType(ListItemType.SelectedItem);
						}
					}
				}
			}
		}

		// Token: 0x1700103E RID: 4158
		// (get) Token: 0x060042D2 RID: 17106 RVA: 0x001143D0 File Offset: 0x001133D0
		[WebSysDescription("DataList_SelectedItem")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual DataListItem SelectedItem
		{
			get
			{
				int selectedIndex = this.SelectedIndex;
				DataListItem dataListItem = null;
				if (selectedIndex != -1)
				{
					dataListItem = this.Items[selectedIndex];
				}
				return dataListItem;
			}
		}

		// Token: 0x1700103F RID: 4159
		// (get) Token: 0x060042D3 RID: 17107 RVA: 0x001143F8 File Offset: 0x001133F8
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebSysDescription("DataList_SelectedItemStyle")]
		[WebCategory("Styles")]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual TableItemStyle SelectedItemStyle
		{
			get
			{
				if (this.selectedItemStyle == null)
				{
					this.selectedItemStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.selectedItemStyle).TrackViewState();
					}
				}
				return this.selectedItemStyle;
			}
		}

		// Token: 0x17001040 RID: 4160
		// (get) Token: 0x060042D4 RID: 17108 RVA: 0x00114426 File Offset: 0x00113426
		// (set) Token: 0x060042D5 RID: 17109 RVA: 0x0011442E File Offset: 0x0011342E
		[WebSysDescription("DataList_SelectedItemTemplate")]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[Browsable(false)]
		[TemplateContainer(typeof(DataListItem))]
		public virtual ITemplate SelectedItemTemplate
		{
			get
			{
				return this.selectedItemTemplate;
			}
			set
			{
				this.selectedItemTemplate = value;
			}
		}

		// Token: 0x17001041 RID: 4161
		// (get) Token: 0x060042D6 RID: 17110 RVA: 0x00114438 File Offset: 0x00113438
		[Browsable(false)]
		public object SelectedValue
		{
			get
			{
				if (this.DataKeyField.Length == 0)
				{
					throw new InvalidOperationException(SR.GetString("DataList_DataKeyFieldMustBeSpecified", new object[] { this.ID }));
				}
				DataKeyCollection dataKeys = base.DataKeys;
				int selectedIndex = this.SelectedIndex;
				if (dataKeys != null && selectedIndex < dataKeys.Count && selectedIndex > -1)
				{
					return dataKeys[selectedIndex];
				}
				return null;
			}
		}

		// Token: 0x17001042 RID: 4162
		// (get) Token: 0x060042D7 RID: 17111 RVA: 0x0011449A File Offset: 0x0011349A
		[DefaultValue(null)]
		[WebSysDescription("DataList_SeparatorStyle")]
		[WebCategory("Styles")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual TableItemStyle SeparatorStyle
		{
			get
			{
				if (this.separatorStyle == null)
				{
					this.separatorStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.separatorStyle).TrackViewState();
					}
				}
				return this.separatorStyle;
			}
		}

		// Token: 0x17001043 RID: 4163
		// (get) Token: 0x060042D8 RID: 17112 RVA: 0x001144C8 File Offset: 0x001134C8
		// (set) Token: 0x060042D9 RID: 17113 RVA: 0x001144D0 File Offset: 0x001134D0
		[TemplateContainer(typeof(DataListItem))]
		[Browsable(false)]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("DataList_SeparatorTemplate")]
		public virtual ITemplate SeparatorTemplate
		{
			get
			{
				return this.separatorTemplate;
			}
			set
			{
				this.separatorTemplate = value;
			}
		}

		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x060042DA RID: 17114 RVA: 0x001144DC File Offset: 0x001134DC
		// (set) Token: 0x060042DB RID: 17115 RVA: 0x00114505 File Offset: 0x00113505
		[DefaultValue(true)]
		[WebCategory("Appearance")]
		[WebSysDescription("DataControls_ShowFooter")]
		public virtual bool ShowFooter
		{
			get
			{
				object obj = this.ViewState["ShowFooter"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["ShowFooter"] = value;
			}
		}

		// Token: 0x17001045 RID: 4165
		// (get) Token: 0x060042DC RID: 17116 RVA: 0x00114520 File Offset: 0x00113520
		// (set) Token: 0x060042DD RID: 17117 RVA: 0x00114549 File Offset: 0x00113549
		[WebCategory("Appearance")]
		[WebSysDescription("DataControls_ShowHeader")]
		[DefaultValue(true)]
		public virtual bool ShowHeader
		{
			get
			{
				object obj = this.ViewState["ShowHeader"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["ShowHeader"] = value;
			}
		}

		// Token: 0x17001046 RID: 4166
		// (get) Token: 0x060042DE RID: 17118 RVA: 0x00114561 File Offset: 0x00113561
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				if (this.RepeatLayout != RepeatLayout.Table)
				{
					return HtmlTextWriterTag.Span;
				}
				return HtmlTextWriterTag.Table;
			}
		}

		// Token: 0x1400009E RID: 158
		// (add) Token: 0x060042DF RID: 17119 RVA: 0x00114570 File Offset: 0x00113570
		// (remove) Token: 0x060042E0 RID: 17120 RVA: 0x00114583 File Offset: 0x00113583
		[WebSysDescription("DataList_OnCancelCommand")]
		[WebCategory("Action")]
		public event DataListCommandEventHandler CancelCommand
		{
			add
			{
				base.Events.AddHandler(DataList.EventCancelCommand, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataList.EventCancelCommand, value);
			}
		}

		// Token: 0x1400009F RID: 159
		// (add) Token: 0x060042E1 RID: 17121 RVA: 0x00114596 File Offset: 0x00113596
		// (remove) Token: 0x060042E2 RID: 17122 RVA: 0x001145A9 File Offset: 0x001135A9
		[WebCategory("Action")]
		[WebSysDescription("DataList_OnDeleteCommand")]
		public event DataListCommandEventHandler DeleteCommand
		{
			add
			{
				base.Events.AddHandler(DataList.EventDeleteCommand, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataList.EventDeleteCommand, value);
			}
		}

		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x060042E3 RID: 17123 RVA: 0x001145BC File Offset: 0x001135BC
		// (remove) Token: 0x060042E4 RID: 17124 RVA: 0x001145CF File Offset: 0x001135CF
		[WebSysDescription("DataList_OnEditCommand")]
		[WebCategory("Action")]
		public event DataListCommandEventHandler EditCommand
		{
			add
			{
				base.Events.AddHandler(DataList.EventEditCommand, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataList.EventEditCommand, value);
			}
		}

		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x060042E5 RID: 17125 RVA: 0x001145E2 File Offset: 0x001135E2
		// (remove) Token: 0x060042E6 RID: 17126 RVA: 0x001145F5 File Offset: 0x001135F5
		[WebSysDescription("DataList_OnItemCommand")]
		[WebCategory("Action")]
		public event DataListCommandEventHandler ItemCommand
		{
			add
			{
				base.Events.AddHandler(DataList.EventItemCommand, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataList.EventItemCommand, value);
			}
		}

		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x060042E7 RID: 17127 RVA: 0x00114608 File Offset: 0x00113608
		// (remove) Token: 0x060042E8 RID: 17128 RVA: 0x0011461B File Offset: 0x0011361B
		[WebCategory("Behavior")]
		[WebSysDescription("DataControls_OnItemCreated")]
		public event DataListItemEventHandler ItemCreated
		{
			add
			{
				base.Events.AddHandler(DataList.EventItemCreated, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataList.EventItemCreated, value);
			}
		}

		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x060042E9 RID: 17129 RVA: 0x0011462E File Offset: 0x0011362E
		// (remove) Token: 0x060042EA RID: 17130 RVA: 0x00114641 File Offset: 0x00113641
		[WebCategory("Behavior")]
		[WebSysDescription("DataControls_OnItemDataBound")]
		public event DataListItemEventHandler ItemDataBound
		{
			add
			{
				base.Events.AddHandler(DataList.EventItemDataBound, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataList.EventItemDataBound, value);
			}
		}

		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x060042EB RID: 17131 RVA: 0x00114654 File Offset: 0x00113654
		// (remove) Token: 0x060042EC RID: 17132 RVA: 0x00114667 File Offset: 0x00113667
		[WebCategory("Action")]
		[WebSysDescription("DataList_OnUpdateCommand")]
		public event DataListCommandEventHandler UpdateCommand
		{
			add
			{
				base.Events.AddHandler(DataList.EventUpdateCommand, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataList.EventUpdateCommand, value);
			}
		}

		// Token: 0x060042ED RID: 17133 RVA: 0x0011467C File Offset: 0x0011367C
		protected override void CreateControlHierarchy(bool useDataSource)
		{
			IEnumerable enumerable = null;
			int num = -1;
			ArrayList dataKeysArray = base.DataKeysArray;
			this.extractTemplateRows = this.ExtractTemplateRows;
			if (this.itemsArray != null)
			{
				this.itemsArray.Clear();
			}
			else
			{
				this.itemsArray = new ArrayList();
			}
			if (!useDataSource)
			{
				num = (int)this.ViewState["_!ItemCount"];
				if (num != -1)
				{
					enumerable = new DummyDataSource(num);
					this.itemsArray.Capacity = num;
				}
			}
			else
			{
				dataKeysArray.Clear();
				enumerable = this.GetData();
				ICollection collection = enumerable as ICollection;
				if (collection != null)
				{
					dataKeysArray.Capacity = collection.Count;
					this.itemsArray.Capacity = collection.Count;
				}
			}
			if (enumerable != null)
			{
				ControlCollection controls = this.Controls;
				int num2 = 0;
				bool flag = this.separatorTemplate != null;
				int editItemIndex = this.EditItemIndex;
				int selectedIndex = this.SelectedIndex;
				string dataKeyField = this.DataKeyField;
				bool flag2 = useDataSource && dataKeyField.Length != 0;
				num = 0;
				if (this.headerTemplate != null)
				{
					this.CreateItem(-1, ListItemType.Header, useDataSource, null);
				}
				foreach (object obj in enumerable)
				{
					if (flag2)
					{
						object propertyValue = DataBinder.GetPropertyValue(obj, dataKeyField);
						dataKeysArray.Add(propertyValue);
					}
					ListItemType listItemType = ListItemType.Item;
					if (num2 == editItemIndex)
					{
						listItemType = ListItemType.EditItem;
					}
					else if (num2 == selectedIndex)
					{
						listItemType = ListItemType.SelectedItem;
					}
					else if (num2 % 2 != 0)
					{
						listItemType = ListItemType.AlternatingItem;
					}
					DataListItem dataListItem = this.CreateItem(num2, listItemType, useDataSource, obj);
					this.itemsArray.Add(dataListItem);
					if (flag)
					{
						this.CreateItem(num2, ListItemType.Separator, useDataSource, null);
					}
					num++;
					num2++;
				}
				if (this.footerTemplate != null)
				{
					this.CreateItem(-1, ListItemType.Footer, useDataSource, null);
				}
			}
			if (useDataSource)
			{
				this.ViewState["_!ItemCount"] = ((enumerable != null) ? num : (-1));
			}
		}

		// Token: 0x060042EE RID: 17134 RVA: 0x00114870 File Offset: 0x00113870
		protected override Style CreateControlStyle()
		{
			return new TableStyle
			{
				CellSpacing = 0
			};
		}

		// Token: 0x060042EF RID: 17135 RVA: 0x0011488C File Offset: 0x0011388C
		private DataListItem CreateItem(int itemIndex, ListItemType itemType, bool dataBind, object dataItem)
		{
			DataListItem dataListItem = this.CreateItem(itemIndex, itemType);
			DataListItemEventArgs dataListItemEventArgs = new DataListItemEventArgs(dataListItem);
			this.InitializeItem(dataListItem);
			if (dataBind)
			{
				dataListItem.DataItem = dataItem;
			}
			this.OnItemCreated(dataListItemEventArgs);
			this.Controls.Add(dataListItem);
			if (dataBind)
			{
				dataListItem.DataBind();
				this.OnItemDataBound(dataListItemEventArgs);
				dataListItem.DataItem = null;
			}
			return dataListItem;
		}

		// Token: 0x060042F0 RID: 17136 RVA: 0x001148E6 File Offset: 0x001138E6
		protected virtual DataListItem CreateItem(int itemIndex, ListItemType itemType)
		{
			return new DataListItem(itemIndex, itemType);
		}

		// Token: 0x060042F1 RID: 17137 RVA: 0x001148F0 File Offset: 0x001138F0
		private DataListItem GetItem(ListItemType itemType, int repeatIndex)
		{
			DataListItem dataListItem = null;
			switch (itemType)
			{
			case ListItemType.Header:
				dataListItem = (DataListItem)this.Controls[0];
				break;
			case ListItemType.Footer:
				dataListItem = (DataListItem)this.Controls[this.Controls.Count - 1];
				break;
			case ListItemType.Item:
			case ListItemType.AlternatingItem:
			case ListItemType.SelectedItem:
			case ListItemType.EditItem:
				dataListItem = (DataListItem)this.itemsArray[repeatIndex];
				break;
			case ListItemType.Separator:
			{
				int num = repeatIndex * 2 + 1;
				if (this.headerTemplate != null)
				{
					num++;
				}
				dataListItem = (DataListItem)this.Controls[num];
				break;
			}
			}
			return dataListItem;
		}

		// Token: 0x060042F2 RID: 17138 RVA: 0x00114994 File Offset: 0x00113994
		protected virtual void InitializeItem(DataListItem item)
		{
			ITemplate template = this.itemTemplate;
			switch (item.ItemType)
			{
			case ListItemType.Header:
				template = this.headerTemplate;
				goto IL_00A4;
			case ListItemType.Footer:
				template = this.footerTemplate;
				goto IL_00A4;
			case ListItemType.Item:
				goto IL_00A4;
			case ListItemType.AlternatingItem:
				break;
			case ListItemType.SelectedItem:
				goto IL_0055;
			case ListItemType.EditItem:
				if (this.editItemTemplate != null)
				{
					template = this.editItemTemplate;
					goto IL_00A4;
				}
				if (item.ItemIndex == this.SelectedIndex)
				{
					goto IL_0055;
				}
				if (item.ItemIndex % 2 == 0)
				{
					goto IL_00A4;
				}
				break;
			case ListItemType.Separator:
				template = this.separatorTemplate;
				goto IL_00A4;
			default:
				goto IL_00A4;
			}
			IL_0044:
			if (this.alternatingItemTemplate != null)
			{
				template = this.alternatingItemTemplate;
				goto IL_00A4;
			}
			goto IL_00A4;
			IL_0055:
			if (this.selectedItemTemplate != null)
			{
				template = this.selectedItemTemplate;
			}
			else if (item.ItemIndex % 2 != 0)
			{
				goto IL_0044;
			}
			IL_00A4:
			if (template != null)
			{
				template.InstantiateIn(item);
			}
		}

		// Token: 0x060042F3 RID: 17139 RVA: 0x00114A50 File Offset: 0x00113A50
		protected override void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				object[] array = (object[])savedState;
				if (array[0] != null)
				{
					base.LoadViewState(array[0]);
				}
				if (array[1] != null)
				{
					((IStateManager)this.ItemStyle).LoadViewState(array[1]);
				}
				if (array[2] != null)
				{
					((IStateManager)this.SelectedItemStyle).LoadViewState(array[2]);
				}
				if (array[3] != null)
				{
					((IStateManager)this.AlternatingItemStyle).LoadViewState(array[3]);
				}
				if (array[4] != null)
				{
					((IStateManager)this.EditItemStyle).LoadViewState(array[4]);
				}
				if (array[5] != null)
				{
					((IStateManager)this.SeparatorStyle).LoadViewState(array[5]);
				}
				if (array[6] != null)
				{
					((IStateManager)this.HeaderStyle).LoadViewState(array[6]);
				}
				if (array[7] != null)
				{
					((IStateManager)this.FooterStyle).LoadViewState(array[7]);
				}
				if (array[8] != null)
				{
					((IStateManager)base.ControlStyle).LoadViewState(array[8]);
				}
			}
		}

		// Token: 0x060042F4 RID: 17140 RVA: 0x00114B10 File Offset: 0x00113B10
		protected override bool OnBubbleEvent(object source, EventArgs e)
		{
			bool flag = false;
			if (e is DataListCommandEventArgs)
			{
				DataListCommandEventArgs dataListCommandEventArgs = (DataListCommandEventArgs)e;
				this.OnItemCommand(dataListCommandEventArgs);
				flag = true;
				string commandName = dataListCommandEventArgs.CommandName;
				if (StringUtil.EqualsIgnoreCase(commandName, "Select"))
				{
					this.SelectedIndex = dataListCommandEventArgs.Item.ItemIndex;
					this.OnSelectedIndexChanged(EventArgs.Empty);
				}
				else if (StringUtil.EqualsIgnoreCase(commandName, "Edit"))
				{
					this.OnEditCommand(dataListCommandEventArgs);
				}
				else if (StringUtil.EqualsIgnoreCase(commandName, "Delete"))
				{
					this.OnDeleteCommand(dataListCommandEventArgs);
				}
				else if (StringUtil.EqualsIgnoreCase(commandName, "Update"))
				{
					this.OnUpdateCommand(dataListCommandEventArgs);
				}
				else if (StringUtil.EqualsIgnoreCase(commandName, "Cancel"))
				{
					this.OnCancelCommand(dataListCommandEventArgs);
				}
			}
			return flag;
		}

		// Token: 0x060042F5 RID: 17141 RVA: 0x00114BC4 File Offset: 0x00113BC4
		protected virtual void OnCancelCommand(DataListCommandEventArgs e)
		{
			DataListCommandEventHandler dataListCommandEventHandler = (DataListCommandEventHandler)base.Events[DataList.EventCancelCommand];
			if (dataListCommandEventHandler != null)
			{
				dataListCommandEventHandler(this, e);
			}
		}

		// Token: 0x060042F6 RID: 17142 RVA: 0x00114BF4 File Offset: 0x00113BF4
		protected virtual void OnDeleteCommand(DataListCommandEventArgs e)
		{
			DataListCommandEventHandler dataListCommandEventHandler = (DataListCommandEventHandler)base.Events[DataList.EventDeleteCommand];
			if (dataListCommandEventHandler != null)
			{
				dataListCommandEventHandler(this, e);
			}
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x00114C24 File Offset: 0x00113C24
		protected virtual void OnEditCommand(DataListCommandEventArgs e)
		{
			DataListCommandEventHandler dataListCommandEventHandler = (DataListCommandEventHandler)base.Events[DataList.EventEditCommand];
			if (dataListCommandEventHandler != null)
			{
				dataListCommandEventHandler(this, e);
			}
		}

		// Token: 0x060042F8 RID: 17144 RVA: 0x00114C52 File Offset: 0x00113C52
		protected internal override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			if (this.Page != null && this.DataKeyField.Length > 0)
			{
				this.Page.RegisterRequiresViewStateEncryption();
			}
		}

		// Token: 0x060042F9 RID: 17145 RVA: 0x00114C7C File Offset: 0x00113C7C
		protected virtual void OnItemCommand(DataListCommandEventArgs e)
		{
			DataListCommandEventHandler dataListCommandEventHandler = (DataListCommandEventHandler)base.Events[DataList.EventItemCommand];
			if (dataListCommandEventHandler != null)
			{
				dataListCommandEventHandler(this, e);
			}
		}

		// Token: 0x060042FA RID: 17146 RVA: 0x00114CAC File Offset: 0x00113CAC
		protected virtual void OnItemCreated(DataListItemEventArgs e)
		{
			DataListItemEventHandler dataListItemEventHandler = (DataListItemEventHandler)base.Events[DataList.EventItemCreated];
			if (dataListItemEventHandler != null)
			{
				dataListItemEventHandler(this, e);
			}
		}

		// Token: 0x060042FB RID: 17147 RVA: 0x00114CDC File Offset: 0x00113CDC
		protected virtual void OnItemDataBound(DataListItemEventArgs e)
		{
			DataListItemEventHandler dataListItemEventHandler = (DataListItemEventHandler)base.Events[DataList.EventItemDataBound];
			if (dataListItemEventHandler != null)
			{
				dataListItemEventHandler(this, e);
			}
		}

		// Token: 0x060042FC RID: 17148 RVA: 0x00114D0C File Offset: 0x00113D0C
		protected virtual void OnUpdateCommand(DataListCommandEventArgs e)
		{
			DataListCommandEventHandler dataListCommandEventHandler = (DataListCommandEventHandler)base.Events[DataList.EventUpdateCommand];
			if (dataListCommandEventHandler != null)
			{
				dataListCommandEventHandler(this, e);
			}
		}

		// Token: 0x060042FD RID: 17149 RVA: 0x00114D3C File Offset: 0x00113D3C
		protected internal override void PrepareControlHierarchy()
		{
			ControlCollection controls = this.Controls;
			int count = controls.Count;
			if (count == 0)
			{
				return;
			}
			Style style;
			if (this.alternatingItemStyle != null)
			{
				style = new TableItemStyle();
				style.CopyFrom(this.itemStyle);
				style.CopyFrom(this.alternatingItemStyle);
			}
			else
			{
				style = this.itemStyle;
			}
			for (int i = 0; i < count; i++)
			{
				DataListItem dataListItem = (DataListItem)controls[i];
				Style style2 = null;
				switch (dataListItem.ItemType)
				{
				case ListItemType.Header:
					if (this.ShowHeader)
					{
						style2 = this.headerStyle;
					}
					break;
				case ListItemType.Footer:
					if (this.ShowFooter)
					{
						style2 = this.footerStyle;
					}
					break;
				case ListItemType.Item:
					style2 = this.itemStyle;
					break;
				case ListItemType.AlternatingItem:
					style2 = style;
					break;
				case ListItemType.SelectedItem:
					style2 = new TableItemStyle();
					if (dataListItem.ItemIndex % 2 != 0)
					{
						style2.CopyFrom(style);
					}
					else
					{
						style2.CopyFrom(this.itemStyle);
					}
					style2.CopyFrom(this.selectedItemStyle);
					break;
				case ListItemType.EditItem:
					style2 = new TableItemStyle();
					if (dataListItem.ItemIndex % 2 != 0)
					{
						style2.CopyFrom(style);
					}
					else
					{
						style2.CopyFrom(this.itemStyle);
					}
					if (dataListItem.ItemIndex == this.SelectedIndex)
					{
						style2.CopyFrom(this.selectedItemStyle);
					}
					style2.CopyFrom(this.editItemStyle);
					break;
				case ListItemType.Separator:
					style2 = this.separatorStyle;
					break;
				}
				if (style2 != null)
				{
					if (!this.extractTemplateRows)
					{
						dataListItem.MergeStyle(style2);
					}
					else
					{
						foreach (object obj in dataListItem.Controls)
						{
							Control control = (Control)obj;
							if (control is Table)
							{
								foreach (object obj2 in ((Table)control).Rows)
								{
									((TableRow)obj2).MergeStyle(style2);
								}
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x060042FE RID: 17150 RVA: 0x00114F2C File Offset: 0x00113F2C
		protected internal override void RenderContents(HtmlTextWriter writer)
		{
			if (this.Controls.Count == 0)
			{
				return;
			}
			RepeatInfo repeatInfo = new RepeatInfo();
			Table table = null;
			Style controlStyle = base.ControlStyle;
			if (this.extractTemplateRows)
			{
				repeatInfo.RepeatDirection = RepeatDirection.Vertical;
				repeatInfo.RepeatLayout = RepeatLayout.Flow;
				repeatInfo.RepeatColumns = 1;
				repeatInfo.OuterTableImplied = true;
				table = new Table();
				table.ID = this.ClientID;
				table.CopyBaseAttributes(this);
				table.Caption = this.Caption;
				table.CaptionAlign = this.CaptionAlign;
				table.ApplyStyle(controlStyle);
				table.RenderBeginTag(writer);
			}
			else
			{
				repeatInfo.RepeatDirection = this.RepeatDirection;
				repeatInfo.RepeatLayout = this.RepeatLayout;
				repeatInfo.RepeatColumns = this.RepeatColumns;
				if (repeatInfo.RepeatLayout == RepeatLayout.Table)
				{
					repeatInfo.Caption = this.Caption;
					repeatInfo.CaptionAlign = this.CaptionAlign;
					repeatInfo.UseAccessibleHeader = this.UseAccessibleHeader;
				}
				else
				{
					repeatInfo.EnableLegacyRendering = base.EnableLegacyRendering;
				}
			}
			repeatInfo.RenderRepeater(writer, this, controlStyle, this);
			if (table != null)
			{
				table.RenderEndTag(writer);
			}
		}

		// Token: 0x060042FF RID: 17151 RVA: 0x00115030 File Offset: 0x00114030
		protected override object SaveViewState()
		{
			object obj = base.SaveViewState();
			object obj2 = ((this.itemStyle != null) ? ((IStateManager)this.itemStyle).SaveViewState() : null);
			object obj3 = ((this.selectedItemStyle != null) ? ((IStateManager)this.selectedItemStyle).SaveViewState() : null);
			object obj4 = ((this.alternatingItemStyle != null) ? ((IStateManager)this.alternatingItemStyle).SaveViewState() : null);
			object obj5 = ((this.editItemStyle != null) ? ((IStateManager)this.editItemStyle).SaveViewState() : null);
			object obj6 = ((this.separatorStyle != null) ? ((IStateManager)this.separatorStyle).SaveViewState() : null);
			object obj7 = ((this.headerStyle != null) ? ((IStateManager)this.headerStyle).SaveViewState() : null);
			object obj8 = ((this.footerStyle != null) ? ((IStateManager)this.footerStyle).SaveViewState() : null);
			object obj9 = (base.ControlStyleCreated ? ((IStateManager)base.ControlStyle).SaveViewState() : null);
			return new object[] { obj, obj2, obj3, obj4, obj5, obj6, obj7, obj8, obj9 };
		}

		// Token: 0x06004300 RID: 17152 RVA: 0x00115140 File Offset: 0x00114140
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (this.itemStyle != null)
			{
				((IStateManager)this.itemStyle).TrackViewState();
			}
			if (this.selectedItemStyle != null)
			{
				((IStateManager)this.selectedItemStyle).TrackViewState();
			}
			if (this.alternatingItemStyle != null)
			{
				((IStateManager)this.alternatingItemStyle).TrackViewState();
			}
			if (this.editItemStyle != null)
			{
				((IStateManager)this.editItemStyle).TrackViewState();
			}
			if (this.separatorStyle != null)
			{
				((IStateManager)this.separatorStyle).TrackViewState();
			}
			if (this.headerStyle != null)
			{
				((IStateManager)this.headerStyle).TrackViewState();
			}
			if (this.footerStyle != null)
			{
				((IStateManager)this.footerStyle).TrackViewState();
			}
			if (base.ControlStyleCreated)
			{
				((IStateManager)base.ControlStyle).TrackViewState();
			}
		}

		// Token: 0x17001047 RID: 4167
		// (get) Token: 0x06004301 RID: 17153 RVA: 0x001151EB File Offset: 0x001141EB
		bool IRepeatInfoUser.HasFooter
		{
			get
			{
				return this.ShowFooter && this.footerTemplate != null;
			}
		}

		// Token: 0x17001048 RID: 4168
		// (get) Token: 0x06004302 RID: 17154 RVA: 0x00115203 File Offset: 0x00114203
		bool IRepeatInfoUser.HasHeader
		{
			get
			{
				return this.ShowHeader && this.headerTemplate != null;
			}
		}

		// Token: 0x17001049 RID: 4169
		// (get) Token: 0x06004303 RID: 17155 RVA: 0x0011521B File Offset: 0x0011421B
		bool IRepeatInfoUser.HasSeparators
		{
			get
			{
				return this.separatorTemplate != null;
			}
		}

		// Token: 0x1700104A RID: 4170
		// (get) Token: 0x06004304 RID: 17156 RVA: 0x00115229 File Offset: 0x00114229
		int IRepeatInfoUser.RepeatedItemCount
		{
			get
			{
				if (this.visibleItemCount != -1)
				{
					return this.visibleItemCount;
				}
				if (this.itemsArray == null)
				{
					return 0;
				}
				return this.itemsArray.Count;
			}
		}

		// Token: 0x06004305 RID: 17157 RVA: 0x00115250 File Offset: 0x00114250
		Style IRepeatInfoUser.GetItemStyle(ListItemType itemType, int repeatIndex)
		{
			DataListItem item = this.GetItem(itemType, repeatIndex);
			if (item != null && item.ControlStyleCreated)
			{
				return item.ControlStyle;
			}
			return null;
		}

		// Token: 0x06004306 RID: 17158 RVA: 0x0011527C File Offset: 0x0011427C
		void IRepeatInfoUser.RenderItem(ListItemType itemType, int repeatIndex, RepeatInfo repeatInfo, HtmlTextWriter writer)
		{
			DataListItem item = this.GetItem(itemType, repeatIndex + this.offset);
			if (item != null)
			{
				item.RenderItem(writer, this.extractTemplateRows, repeatInfo.RepeatLayout == RepeatLayout.Table);
			}
		}

		// Token: 0x04002923 RID: 10531
		public const string SelectCommandName = "Select";

		// Token: 0x04002924 RID: 10532
		public const string EditCommandName = "Edit";

		// Token: 0x04002925 RID: 10533
		public const string UpdateCommandName = "Update";

		// Token: 0x04002926 RID: 10534
		public const string CancelCommandName = "Cancel";

		// Token: 0x04002927 RID: 10535
		public const string DeleteCommandName = "Delete";

		// Token: 0x04002928 RID: 10536
		private static readonly object EventItemCreated = new object();

		// Token: 0x04002929 RID: 10537
		private static readonly object EventItemDataBound = new object();

		// Token: 0x0400292A RID: 10538
		private static readonly object EventItemCommand = new object();

		// Token: 0x0400292B RID: 10539
		private static readonly object EventEditCommand = new object();

		// Token: 0x0400292C RID: 10540
		private static readonly object EventUpdateCommand = new object();

		// Token: 0x0400292D RID: 10541
		private static readonly object EventCancelCommand = new object();

		// Token: 0x0400292E RID: 10542
		private static readonly object EventDeleteCommand = new object();

		// Token: 0x0400292F RID: 10543
		private TableItemStyle itemStyle;

		// Token: 0x04002930 RID: 10544
		private TableItemStyle alternatingItemStyle;

		// Token: 0x04002931 RID: 10545
		private TableItemStyle selectedItemStyle;

		// Token: 0x04002932 RID: 10546
		private TableItemStyle editItemStyle;

		// Token: 0x04002933 RID: 10547
		private TableItemStyle separatorStyle;

		// Token: 0x04002934 RID: 10548
		private TableItemStyle headerStyle;

		// Token: 0x04002935 RID: 10549
		private TableItemStyle footerStyle;

		// Token: 0x04002936 RID: 10550
		private ITemplate itemTemplate;

		// Token: 0x04002937 RID: 10551
		private ITemplate alternatingItemTemplate;

		// Token: 0x04002938 RID: 10552
		private ITemplate selectedItemTemplate;

		// Token: 0x04002939 RID: 10553
		private ITemplate editItemTemplate;

		// Token: 0x0400293A RID: 10554
		private ITemplate separatorTemplate;

		// Token: 0x0400293B RID: 10555
		private ITemplate headerTemplate;

		// Token: 0x0400293C RID: 10556
		private ITemplate footerTemplate;

		// Token: 0x0400293D RID: 10557
		private bool extractTemplateRows;

		// Token: 0x0400293E RID: 10558
		private ArrayList itemsArray;

		// Token: 0x0400293F RID: 10559
		private DataListItemCollection itemsCollection;

		// Token: 0x04002940 RID: 10560
		private int offset;

		// Token: 0x04002941 RID: 10561
		private int visibleItemCount = -1;
	}
}
