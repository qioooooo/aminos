using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000539 RID: 1337
	[Editor("System.Web.UI.Design.WebControls.DataGridComponentEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(ComponentEditor))]
	[Designer("System.Web.UI.Design.WebControls.DataGridDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DataGrid : BaseDataList, INamingContainer
	{
		// Token: 0x17000FD6 RID: 4054
		// (get) Token: 0x060041C0 RID: 16832 RVA: 0x001101EC File Offset: 0x0010F1EC
		// (set) Token: 0x060041C1 RID: 16833 RVA: 0x00110215 File Offset: 0x0010F215
		[DefaultValue(false)]
		[WebCategory("Paging")]
		[WebSysDescription("DataGrid_AllowCustomPaging")]
		public virtual bool AllowCustomPaging
		{
			get
			{
				object obj = this.ViewState["AllowCustomPaging"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["AllowCustomPaging"] = value;
			}
		}

		// Token: 0x17000FD7 RID: 4055
		// (get) Token: 0x060041C2 RID: 16834 RVA: 0x00110230 File Offset: 0x0010F230
		// (set) Token: 0x060041C3 RID: 16835 RVA: 0x00110259 File Offset: 0x0010F259
		[DefaultValue(false)]
		[WebSysDescription("DataGrid_AllowPaging")]
		[WebCategory("Paging")]
		public virtual bool AllowPaging
		{
			get
			{
				object obj = this.ViewState["AllowPaging"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["AllowPaging"] = value;
			}
		}

		// Token: 0x17000FD8 RID: 4056
		// (get) Token: 0x060041C4 RID: 16836 RVA: 0x00110274 File Offset: 0x0010F274
		// (set) Token: 0x060041C5 RID: 16837 RVA: 0x0011029D File Offset: 0x0010F29D
		[WebCategory("Behavior")]
		[DefaultValue(false)]
		[WebSysDescription("DataGrid_AllowSorting")]
		public virtual bool AllowSorting
		{
			get
			{
				object obj = this.ViewState["AllowSorting"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["AllowSorting"] = value;
			}
		}

		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x060041C6 RID: 16838 RVA: 0x001102B5 File Offset: 0x0010F2B5
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebSysDescription("DataGrid_AlternatingItemStyle")]
		[NotifyParentProperty(true)]
		[WebCategory("Styles")]
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

		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x060041C7 RID: 16839 RVA: 0x001102E4 File Offset: 0x0010F2E4
		// (set) Token: 0x060041C8 RID: 16840 RVA: 0x0011030D File Offset: 0x0010F30D
		[DefaultValue(true)]
		[WebCategory("Behavior")]
		[WebSysDescription("DataControls_AutoGenerateColumns")]
		public virtual bool AutoGenerateColumns
		{
			get
			{
				object obj = this.ViewState["AutoGenerateColumns"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["AutoGenerateColumns"] = value;
			}
		}

		// Token: 0x17000FDB RID: 4059
		// (get) Token: 0x060041C9 RID: 16841 RVA: 0x00110325 File Offset: 0x0010F325
		// (set) Token: 0x060041CA RID: 16842 RVA: 0x00110345 File Offset: 0x0010F345
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebCategory("Appearance")]
		[WebSysDescription("WebControl_BackImageUrl")]
		[UrlProperty]
		[DefaultValue("")]
		public virtual string BackImageUrl
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return string.Empty;
				}
				return ((TableStyle)base.ControlStyle).BackImageUrl;
			}
			set
			{
				((TableStyle)base.ControlStyle).BackImageUrl = value;
			}
		}

		// Token: 0x17000FDC RID: 4060
		// (get) Token: 0x060041CB RID: 16843 RVA: 0x00110358 File Offset: 0x0010F358
		// (set) Token: 0x060041CC RID: 16844 RVA: 0x00110381 File Offset: 0x0010F381
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[WebSysDescription("DataGrid_CurrentPageIndex")]
		public int CurrentPageIndex
		{
			get
			{
				object obj = this.ViewState["CurrentPageIndex"];
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
				this.ViewState["CurrentPageIndex"] = value;
			}
		}

		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x060041CD RID: 16845 RVA: 0x001103A8 File Offset: 0x0010F3A8
		[MergableProperty(false)]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("DataControls_Columns")]
		[Editor("System.Web.UI.Design.WebControls.DataGridColumnCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebCategory("Default")]
		public virtual DataGridColumnCollection Columns
		{
			get
			{
				if (this.columnCollection == null)
				{
					this.columns = new ArrayList();
					this.columnCollection = new DataGridColumnCollection(this, this.columns);
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.columnCollection).TrackViewState();
					}
				}
				return this.columnCollection;
			}
		}

		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x060041CE RID: 16846 RVA: 0x001103E8 File Offset: 0x0010F3E8
		// (set) Token: 0x060041CF RID: 16847 RVA: 0x00110411 File Offset: 0x0010F411
		[DefaultValue(-1)]
		[WebCategory("Default")]
		[WebSysDescription("DataGrid_EditItemIndex")]
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

		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x060041D0 RID: 16848 RVA: 0x00110438 File Offset: 0x0010F438
		[WebCategory("Styles")]
		[WebSysDescription("DataGrid_EditItemStyle")]
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

		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x060041D1 RID: 16849 RVA: 0x00110466 File Offset: 0x0010F466
		[WebSysDescription("DataControls_FooterStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[NotifyParentProperty(true)]
		[WebCategory("Styles")]
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

		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x060041D2 RID: 16850 RVA: 0x00110494 File Offset: 0x0010F494
		[WebSysDescription("DataControls_HeaderStyle")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebCategory("Styles")]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DefaultValue(null)]
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

		// Token: 0x17000FE2 RID: 4066
		// (get) Token: 0x060041D3 RID: 16851 RVA: 0x001104C4 File Offset: 0x0010F4C4
		[Browsable(false)]
		[WebSysDescription("DataGrid_Items")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual DataGridItemCollection Items
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
					this.itemsCollection = new DataGridItemCollection(this.itemsArray);
				}
				return this.itemsCollection;
			}
		}

		// Token: 0x17000FE3 RID: 4067
		// (get) Token: 0x060041D4 RID: 16852 RVA: 0x00110511 File Offset: 0x0010F511
		[WebCategory("Styles")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("DataGrid_ItemStyle")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
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

		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x060041D5 RID: 16853 RVA: 0x00110540 File Offset: 0x0010F540
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[WebSysDescription("DataGrid_PageCount")]
		public int PageCount
		{
			get
			{
				if (this.pagedDataSource != null)
				{
					return this.pagedDataSource.PageCount;
				}
				object obj = this.ViewState["PageCount"];
				if (obj == null)
				{
					return 0;
				}
				return (int)obj;
			}
		}

		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x060041D6 RID: 16854 RVA: 0x0011057D File Offset: 0x0010F57D
		[WebCategory("Styles")]
		[WebSysDescription("DataGrid_PagerStyle")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual DataGridPagerStyle PagerStyle
		{
			get
			{
				if (this.pagerStyle == null)
				{
					this.pagerStyle = new DataGridPagerStyle(this);
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.pagerStyle).TrackViewState();
					}
				}
				return this.pagerStyle;
			}
		}

		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x060041D7 RID: 16855 RVA: 0x001105AC File Offset: 0x0010F5AC
		// (set) Token: 0x060041D8 RID: 16856 RVA: 0x001105D6 File Offset: 0x0010F5D6
		[DefaultValue(10)]
		[WebCategory("Paging")]
		[WebSysDescription("DataGrid_PageSize")]
		public virtual int PageSize
		{
			get
			{
				object obj = this.ViewState["PageSize"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 10;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["PageSize"] = value;
			}
		}

		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x060041D9 RID: 16857 RVA: 0x00110600 File Offset: 0x0010F600
		// (set) Token: 0x060041DA RID: 16858 RVA: 0x0011062C File Offset: 0x0010F62C
		[WebSysDescription("WebControl_SelectedIndex")]
		[DefaultValue(-1)]
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
						DataGridItem dataGridItem = (DataGridItem)this.itemsArray[selectedIndex];
						if (dataGridItem.ItemType != ListItemType.EditItem)
						{
							ListItemType listItemType = ListItemType.Item;
							if (selectedIndex % 2 != 0)
							{
								listItemType = ListItemType.AlternatingItem;
							}
							dataGridItem.SetItemType(listItemType);
						}
					}
					if (value != -1 && this.itemsArray.Count > value)
					{
						DataGridItem dataGridItem = (DataGridItem)this.itemsArray[value];
						if (dataGridItem.ItemType != ListItemType.EditItem)
						{
							dataGridItem.SetItemType(ListItemType.SelectedItem);
						}
					}
				}
			}
		}

		// Token: 0x17000FE8 RID: 4072
		// (get) Token: 0x060041DB RID: 16859 RVA: 0x001106E0 File Offset: 0x0010F6E0
		[WebSysDescription("DataGrid_SelectedItem")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual DataGridItem SelectedItem
		{
			get
			{
				int selectedIndex = this.SelectedIndex;
				DataGridItem dataGridItem = null;
				if (selectedIndex != -1)
				{
					dataGridItem = this.Items[selectedIndex];
				}
				return dataGridItem;
			}
		}

		// Token: 0x17000FE9 RID: 4073
		// (get) Token: 0x060041DC RID: 16860 RVA: 0x00110708 File Offset: 0x0010F708
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebSysDescription("DataGrid_SelectedItemStyle")]
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

		// Token: 0x17000FEA RID: 4074
		// (get) Token: 0x060041DD RID: 16861 RVA: 0x00110738 File Offset: 0x0010F738
		// (set) Token: 0x060041DE RID: 16862 RVA: 0x00110761 File Offset: 0x0010F761
		[WebCategory("Appearance")]
		[WebSysDescription("DataControls_ShowFooter")]
		[DefaultValue(false)]
		public virtual bool ShowFooter
		{
			get
			{
				object obj = this.ViewState["ShowFooter"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["ShowFooter"] = value;
			}
		}

		// Token: 0x17000FEB RID: 4075
		// (get) Token: 0x060041DF RID: 16863 RVA: 0x0011077C File Offset: 0x0010F77C
		// (set) Token: 0x060041E0 RID: 16864 RVA: 0x001107A5 File Offset: 0x0010F7A5
		[WebSysDescription("DataControls_ShowHeader")]
		[WebCategory("Appearance")]
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

		// Token: 0x17000FEC RID: 4076
		// (get) Token: 0x060041E1 RID: 16865 RVA: 0x001107BD File Offset: 0x0010F7BD
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.Table;
			}
		}

		// Token: 0x17000FED RID: 4077
		// (get) Token: 0x060041E2 RID: 16866 RVA: 0x001107C4 File Offset: 0x0010F7C4
		// (set) Token: 0x060041E3 RID: 16867 RVA: 0x001107ED File Offset: 0x0010F7ED
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebSysDescription("DataGrid_VisibleItemCount")]
		[Browsable(false)]
		public virtual int VirtualItemCount
		{
			get
			{
				object obj = this.ViewState["VirtualItemCount"];
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
				this.ViewState["VirtualItemCount"] = value;
			}
		}

		// Token: 0x14000095 RID: 149
		// (add) Token: 0x060041E4 RID: 16868 RVA: 0x00110814 File Offset: 0x0010F814
		// (remove) Token: 0x060041E5 RID: 16869 RVA: 0x00110827 File Offset: 0x0010F827
		[WebSysDescription("DataGrid_OnCancelCommand")]
		[WebCategory("Action")]
		public event DataGridCommandEventHandler CancelCommand
		{
			add
			{
				base.Events.AddHandler(DataGrid.EventCancelCommand, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EventCancelCommand, value);
			}
		}

		// Token: 0x14000096 RID: 150
		// (add) Token: 0x060041E6 RID: 16870 RVA: 0x0011083A File Offset: 0x0010F83A
		// (remove) Token: 0x060041E7 RID: 16871 RVA: 0x0011084D File Offset: 0x0010F84D
		[WebSysDescription("DataGrid_OnDeleteCommand")]
		[WebCategory("Action")]
		public event DataGridCommandEventHandler DeleteCommand
		{
			add
			{
				base.Events.AddHandler(DataGrid.EventDeleteCommand, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EventDeleteCommand, value);
			}
		}

		// Token: 0x14000097 RID: 151
		// (add) Token: 0x060041E8 RID: 16872 RVA: 0x00110860 File Offset: 0x0010F860
		// (remove) Token: 0x060041E9 RID: 16873 RVA: 0x00110873 File Offset: 0x0010F873
		[WebSysDescription("DataGrid_OnEditCommand")]
		[WebCategory("Action")]
		public event DataGridCommandEventHandler EditCommand
		{
			add
			{
				base.Events.AddHandler(DataGrid.EventEditCommand, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EventEditCommand, value);
			}
		}

		// Token: 0x14000098 RID: 152
		// (add) Token: 0x060041EA RID: 16874 RVA: 0x00110886 File Offset: 0x0010F886
		// (remove) Token: 0x060041EB RID: 16875 RVA: 0x00110899 File Offset: 0x0010F899
		[WebCategory("Action")]
		[WebSysDescription("DataGrid_OnItemCommand")]
		public event DataGridCommandEventHandler ItemCommand
		{
			add
			{
				base.Events.AddHandler(DataGrid.EventItemCommand, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EventItemCommand, value);
			}
		}

		// Token: 0x14000099 RID: 153
		// (add) Token: 0x060041EC RID: 16876 RVA: 0x001108AC File Offset: 0x0010F8AC
		// (remove) Token: 0x060041ED RID: 16877 RVA: 0x001108BF File Offset: 0x0010F8BF
		[WebSysDescription("DataControls_OnItemCreated")]
		[WebCategory("Behavior")]
		public event DataGridItemEventHandler ItemCreated
		{
			add
			{
				base.Events.AddHandler(DataGrid.EventItemCreated, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EventItemCreated, value);
			}
		}

		// Token: 0x1400009A RID: 154
		// (add) Token: 0x060041EE RID: 16878 RVA: 0x001108D2 File Offset: 0x0010F8D2
		// (remove) Token: 0x060041EF RID: 16879 RVA: 0x001108E5 File Offset: 0x0010F8E5
		[WebSysDescription("DataControls_OnItemDataBound")]
		[WebCategory("Behavior")]
		public event DataGridItemEventHandler ItemDataBound
		{
			add
			{
				base.Events.AddHandler(DataGrid.EventItemDataBound, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EventItemDataBound, value);
			}
		}

		// Token: 0x1400009B RID: 155
		// (add) Token: 0x060041F0 RID: 16880 RVA: 0x001108F8 File Offset: 0x0010F8F8
		// (remove) Token: 0x060041F1 RID: 16881 RVA: 0x0011090B File Offset: 0x0010F90B
		[WebCategory("Action")]
		[WebSysDescription("DataGrid_OnPageIndexChanged")]
		public event DataGridPageChangedEventHandler PageIndexChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EventPageIndexChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EventPageIndexChanged, value);
			}
		}

		// Token: 0x1400009C RID: 156
		// (add) Token: 0x060041F2 RID: 16882 RVA: 0x0011091E File Offset: 0x0010F91E
		// (remove) Token: 0x060041F3 RID: 16883 RVA: 0x00110931 File Offset: 0x0010F931
		[WebSysDescription("DataGrid_OnSortCommand")]
		[WebCategory("Action")]
		public event DataGridSortCommandEventHandler SortCommand
		{
			add
			{
				base.Events.AddHandler(DataGrid.EventSortCommand, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EventSortCommand, value);
			}
		}

		// Token: 0x1400009D RID: 157
		// (add) Token: 0x060041F4 RID: 16884 RVA: 0x00110944 File Offset: 0x0010F944
		// (remove) Token: 0x060041F5 RID: 16885 RVA: 0x00110957 File Offset: 0x0010F957
		[WebCategory("Action")]
		[WebSysDescription("DataGrid_OnUpdateCommand")]
		public event DataGridCommandEventHandler UpdateCommand
		{
			add
			{
				base.Events.AddHandler(DataGrid.EventUpdateCommand, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EventUpdateCommand, value);
			}
		}

		// Token: 0x060041F6 RID: 16886 RVA: 0x0011096A File Offset: 0x0010F96A
		internal void StoreEnumerator(IEnumerator dataSource, object firstDataItem)
		{
			this.storedData = dataSource;
			this.firstDataItem = firstDataItem;
			this.storedDataValid = true;
		}

		// Token: 0x060041F7 RID: 16887 RVA: 0x00110984 File Offset: 0x0010F984
		private ArrayList CreateAutoGeneratedColumns(PagedDataSource dataSource)
		{
			if (dataSource == null)
			{
				return null;
			}
			ArrayList arrayList = new ArrayList();
			bool flag = true;
			PropertyDescriptorCollection propertyDescriptorCollection = ((ITypedList)dataSource).GetItemProperties(new PropertyDescriptor[0]);
			if (propertyDescriptorCollection == null)
			{
				Type type = null;
				object obj = null;
				IEnumerable dataSource2 = dataSource.DataSource;
				Type type2 = dataSource2.GetType();
				PropertyInfo property = type2.GetProperty("Item", BindingFlags.Instance | BindingFlags.Public, null, null, new Type[] { typeof(int) }, null);
				if (property != null)
				{
					type = property.PropertyType;
				}
				if (type == null || type == typeof(object))
				{
					IEnumerator enumerator = dataSource.GetEnumerator();
					if (enumerator.MoveNext())
					{
						obj = enumerator.Current;
					}
					else
					{
						flag = false;
					}
					if (obj != null)
					{
						type = obj.GetType();
					}
					this.StoreEnumerator(enumerator, obj);
				}
				if (obj != null && obj is ICustomTypeDescriptor)
				{
					propertyDescriptorCollection = TypeDescriptor.GetProperties(obj);
				}
				else if (type != null)
				{
					if (BaseDataList.IsBindableType(type))
					{
						BoundColumn boundColumn = new BoundColumn();
						((IStateManager)boundColumn).TrackViewState();
						boundColumn.HeaderText = "Item";
						boundColumn.DataField = BoundColumn.thisExpr;
						boundColumn.SortExpression = "Item";
						boundColumn.SetOwner(this);
						arrayList.Add(boundColumn);
					}
					else
					{
						propertyDescriptorCollection = TypeDescriptor.GetProperties(type);
					}
				}
			}
			if (propertyDescriptorCollection != null && propertyDescriptorCollection.Count != 0)
			{
				foreach (object obj2 in propertyDescriptorCollection)
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj2;
					Type propertyType = propertyDescriptor.PropertyType;
					if (BaseDataList.IsBindableType(propertyType))
					{
						BoundColumn boundColumn2 = new BoundColumn();
						((IStateManager)boundColumn2).TrackViewState();
						boundColumn2.HeaderText = propertyDescriptor.Name;
						boundColumn2.DataField = propertyDescriptor.Name;
						boundColumn2.SortExpression = propertyDescriptor.Name;
						boundColumn2.ReadOnly = propertyDescriptor.IsReadOnly;
						boundColumn2.SetOwner(this);
						arrayList.Add(boundColumn2);
					}
				}
			}
			if (arrayList.Count == 0 && flag)
			{
				throw new HttpException(SR.GetString("DataGrid_NoAutoGenColumns", new object[] { this.ID }));
			}
			return arrayList;
		}

		// Token: 0x060041F8 RID: 16888 RVA: 0x00110BA4 File Offset: 0x0010FBA4
		protected virtual ArrayList CreateColumnSet(PagedDataSource dataSource, bool useDataSource)
		{
			ArrayList arrayList = new ArrayList();
			DataGridColumn[] array = new DataGridColumn[this.Columns.Count];
			this.Columns.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				arrayList.Add(array[i]);
			}
			if (this.AutoGenerateColumns)
			{
				ArrayList arrayList2;
				if (useDataSource)
				{
					arrayList2 = this.CreateAutoGeneratedColumns(dataSource);
					this.autoGenColumnsArray = arrayList2;
				}
				else
				{
					arrayList2 = this.autoGenColumnsArray;
				}
				if (arrayList2 != null)
				{
					int count = arrayList2.Count;
					for (int i = 0; i < count; i++)
					{
						arrayList.Add(arrayList2[i]);
					}
				}
			}
			return arrayList;
		}

		// Token: 0x060041F9 RID: 16889 RVA: 0x00110C3C File Offset: 0x0010FC3C
		protected override void CreateControlHierarchy(bool useDataSource)
		{
			this.pagedDataSource = this.CreatePagedDataSource();
			IEnumerator enumerator = null;
			int num = -1;
			ArrayList dataKeysArray = base.DataKeysArray;
			ArrayList arrayList = null;
			if (this.itemsArray != null)
			{
				this.itemsArray.Clear();
			}
			else
			{
				this.itemsArray = new ArrayList();
			}
			this.itemsCollection = null;
			if (!useDataSource)
			{
				num = (int)this.ViewState["_!ItemCount"];
				int num2 = (int)this.ViewState["_!DataSourceItemCount"];
				if (num != -1)
				{
					if (this.pagedDataSource.IsCustomPagingEnabled)
					{
						this.pagedDataSource.DataSource = new DummyDataSource(num);
					}
					else
					{
						this.pagedDataSource.DataSource = new DummyDataSource(num2);
					}
					enumerator = this.pagedDataSource.GetEnumerator();
					arrayList = this.CreateColumnSet(null, false);
					this.itemsArray.Capacity = num;
				}
			}
			else
			{
				dataKeysArray.Clear();
				IEnumerable data = this.GetData();
				if (data != null)
				{
					ICollection collection = data as ICollection;
					if (collection == null && this.pagedDataSource.IsPagingEnabled && !this.pagedDataSource.IsCustomPagingEnabled)
					{
						throw new HttpException(SR.GetString("DataGrid_Missing_VirtualItemCount", new object[] { this.ID }));
					}
					this.pagedDataSource.DataSource = data;
					if (this.pagedDataSource.IsPagingEnabled && (this.pagedDataSource.CurrentPageIndex < 0 || this.pagedDataSource.CurrentPageIndex >= this.pagedDataSource.PageCount))
					{
						throw new HttpException(SR.GetString("Invalid_CurrentPageIndex"));
					}
					arrayList = this.CreateColumnSet(this.pagedDataSource, useDataSource);
					if (this.storedDataValid)
					{
						enumerator = this.storedData;
					}
					else
					{
						enumerator = this.pagedDataSource.GetEnumerator();
					}
					if (collection != null)
					{
						int count = this.pagedDataSource.Count;
						dataKeysArray.Capacity = count;
						this.itemsArray.Capacity = count;
					}
				}
			}
			int num3 = 0;
			if (arrayList != null)
			{
				num3 = arrayList.Count;
			}
			if (num3 > 0)
			{
				DataGridColumn[] array = new DataGridColumn[num3];
				arrayList.CopyTo(array, 0);
				Table table = new ChildTable(string.IsNullOrEmpty(this.ID) ? null : this.ClientID);
				this.Controls.Add(table);
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Initialize();
				}
				TableRowCollection rows = table.Rows;
				int num4 = 0;
				int num5 = 0;
				string dataKeyField = this.DataKeyField;
				bool flag = useDataSource && dataKeyField.Length != 0;
				bool isPagingEnabled = this.pagedDataSource.IsPagingEnabled;
				int editItemIndex = this.EditItemIndex;
				int selectedIndex = this.SelectedIndex;
				if (this.pagedDataSource.IsPagingEnabled)
				{
					num5 = this.pagedDataSource.FirstIndexInPage;
				}
				num = 0;
				if (isPagingEnabled)
				{
					this.CreateItem(-1, -1, ListItemType.Pager, false, null, array, rows, this.pagedDataSource);
				}
				this.CreateItem(-1, -1, ListItemType.Header, useDataSource, null, array, rows, null);
				if (this.storedDataValid && this.firstDataItem != null)
				{
					if (flag)
					{
						object propertyValue = DataBinder.GetPropertyValue(this.firstDataItem, dataKeyField);
						dataKeysArray.Add(propertyValue);
					}
					ListItemType listItemType = ListItemType.Item;
					if (num4 == editItemIndex)
					{
						listItemType = ListItemType.EditItem;
					}
					else if (num4 == selectedIndex)
					{
						listItemType = ListItemType.SelectedItem;
					}
					DataGridItem dataGridItem = this.CreateItem(0, num5, listItemType, useDataSource, this.firstDataItem, array, rows, null);
					this.itemsArray.Add(dataGridItem);
					num++;
					num4++;
					num5++;
					this.storedDataValid = false;
					this.firstDataItem = null;
				}
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					if (flag)
					{
						object propertyValue2 = DataBinder.GetPropertyValue(obj, dataKeyField);
						dataKeysArray.Add(propertyValue2);
					}
					ListItemType listItemType = ListItemType.Item;
					if (num4 == editItemIndex)
					{
						listItemType = ListItemType.EditItem;
					}
					else if (num4 == selectedIndex)
					{
						listItemType = ListItemType.SelectedItem;
					}
					else if (num4 % 2 != 0)
					{
						listItemType = ListItemType.AlternatingItem;
					}
					DataGridItem dataGridItem = this.CreateItem(num4, num5, listItemType, useDataSource, obj, array, rows, null);
					this.itemsArray.Add(dataGridItem);
					num++;
					num5++;
					num4++;
				}
				this.CreateItem(-1, -1, ListItemType.Footer, useDataSource, null, array, rows, null);
				if (isPagingEnabled)
				{
					this.CreateItem(-1, -1, ListItemType.Pager, false, null, array, rows, this.pagedDataSource);
				}
			}
			if (useDataSource)
			{
				if (enumerator != null)
				{
					this.ViewState["_!ItemCount"] = num;
					if (this.pagedDataSource.IsPagingEnabled)
					{
						this.ViewState["PageCount"] = this.pagedDataSource.PageCount;
						this.ViewState["_!DataSourceItemCount"] = this.pagedDataSource.DataSourceCount;
					}
					else
					{
						this.ViewState["PageCount"] = 1;
						this.ViewState["_!DataSourceItemCount"] = num;
					}
				}
				else
				{
					this.ViewState["_!ItemCount"] = -1;
					this.ViewState["_!DataSourceItemCount"] = -1;
					this.ViewState["PageCount"] = 0;
				}
			}
			this.pagedDataSource = null;
		}

		// Token: 0x060041FA RID: 16890 RVA: 0x00111144 File Offset: 0x00110144
		protected override Style CreateControlStyle()
		{
			return new TableStyle
			{
				GridLines = GridLines.Both,
				CellSpacing = 0
			};
		}

		// Token: 0x060041FB RID: 16891 RVA: 0x00111168 File Offset: 0x00110168
		private DataGridItem CreateItem(int itemIndex, int dataSourceIndex, ListItemType itemType, bool dataBind, object dataItem, DataGridColumn[] columns, TableRowCollection rows, PagedDataSource pagedDataSource)
		{
			DataGridItem dataGridItem = this.CreateItem(itemIndex, dataSourceIndex, itemType);
			DataGridItemEventArgs dataGridItemEventArgs = new DataGridItemEventArgs(dataGridItem);
			if (itemType != ListItemType.Pager)
			{
				this.InitializeItem(dataGridItem, columns);
				if (dataBind)
				{
					dataGridItem.DataItem = dataItem;
				}
				this.OnItemCreated(dataGridItemEventArgs);
				rows.Add(dataGridItem);
				if (dataBind)
				{
					dataGridItem.DataBind();
					this.OnItemDataBound(dataGridItemEventArgs);
					dataGridItem.DataItem = null;
				}
			}
			else
			{
				this.InitializePager(dataGridItem, columns.Length, pagedDataSource);
				this.OnItemCreated(dataGridItemEventArgs);
				rows.Add(dataGridItem);
			}
			return dataGridItem;
		}

		// Token: 0x060041FC RID: 16892 RVA: 0x001111E7 File Offset: 0x001101E7
		protected virtual DataGridItem CreateItem(int itemIndex, int dataSourceIndex, ListItemType itemType)
		{
			return new DataGridItem(itemIndex, dataSourceIndex, itemType);
		}

		// Token: 0x060041FD RID: 16893 RVA: 0x001111F4 File Offset: 0x001101F4
		private PagedDataSource CreatePagedDataSource()
		{
			return new PagedDataSource
			{
				CurrentPageIndex = this.CurrentPageIndex,
				PageSize = this.PageSize,
				AllowPaging = this.AllowPaging,
				AllowCustomPaging = this.AllowCustomPaging,
				VirtualCount = this.VirtualItemCount
			};
		}

		// Token: 0x060041FE RID: 16894 RVA: 0x00111244 File Offset: 0x00110244
		protected virtual void InitializeItem(DataGridItem item, DataGridColumn[] columns)
		{
			TableCellCollection cells = item.Cells;
			for (int i = 0; i < columns.Length; i++)
			{
				TableCell tableCell;
				if (item.ItemType == ListItemType.Header && this.UseAccessibleHeader)
				{
					tableCell = new TableHeaderCell();
					tableCell.Attributes["scope"] = "col";
				}
				else
				{
					tableCell = new TableCell();
				}
				columns[i].InitializeCell(tableCell, i, item.ItemType);
				cells.Add(tableCell);
			}
		}

		// Token: 0x060041FF RID: 16895 RVA: 0x001112B4 File Offset: 0x001102B4
		protected virtual void InitializePager(DataGridItem item, int columnSpan, PagedDataSource pagedDataSource)
		{
			TableCell tableCell = new TableCell();
			if (columnSpan > 1)
			{
				tableCell.ColumnSpan = columnSpan;
			}
			DataGridPagerStyle dataGridPagerStyle = this.PagerStyle;
			if (dataGridPagerStyle.Mode == PagerMode.NextPrev)
			{
				if (!pagedDataSource.IsFirstPage)
				{
					LinkButton linkButton = new DataGridLinkButton();
					linkButton.Text = dataGridPagerStyle.PrevPageText;
					linkButton.CommandName = "Page";
					linkButton.CommandArgument = "Prev";
					linkButton.CausesValidation = false;
					tableCell.Controls.Add(linkButton);
				}
				else
				{
					Label label = new Label();
					label.Text = dataGridPagerStyle.PrevPageText;
					tableCell.Controls.Add(label);
				}
				tableCell.Controls.Add(new LiteralControl("&nbsp;"));
				if (!pagedDataSource.IsLastPage)
				{
					LinkButton linkButton2 = new DataGridLinkButton();
					linkButton2.Text = dataGridPagerStyle.NextPageText;
					linkButton2.CommandName = "Page";
					linkButton2.CommandArgument = "Next";
					linkButton2.CausesValidation = false;
					tableCell.Controls.Add(linkButton2);
				}
				else
				{
					Label label2 = new Label();
					label2.Text = dataGridPagerStyle.NextPageText;
					tableCell.Controls.Add(label2);
				}
			}
			else
			{
				int pageCount = pagedDataSource.PageCount;
				int num = pagedDataSource.CurrentPageIndex + 1;
				int pageButtonCount = dataGridPagerStyle.PageButtonCount;
				int num2 = pageButtonCount;
				if (pageCount < num2)
				{
					num2 = pageCount;
				}
				int num3 = 1;
				int num4 = num2;
				if (num > num4)
				{
					int num5 = pagedDataSource.CurrentPageIndex / pageButtonCount;
					num3 = num5 * pageButtonCount + 1;
					num4 = num3 + pageButtonCount - 1;
					if (num4 > pageCount)
					{
						num4 = pageCount;
					}
					if (num4 - num3 + 1 < pageButtonCount)
					{
						num3 = Math.Max(1, num4 - pageButtonCount + 1);
					}
				}
				if (num3 != 1)
				{
					LinkButton linkButton3 = new DataGridLinkButton();
					linkButton3.Text = "...";
					linkButton3.CommandName = "Page";
					linkButton3.CommandArgument = (num3 - 1).ToString(NumberFormatInfo.InvariantInfo);
					linkButton3.CausesValidation = false;
					tableCell.Controls.Add(linkButton3);
					tableCell.Controls.Add(new LiteralControl("&nbsp;"));
				}
				for (int i = num3; i <= num4; i++)
				{
					string text = i.ToString(NumberFormatInfo.InvariantInfo);
					if (i == num)
					{
						Label label3 = new Label();
						label3.Text = text;
						tableCell.Controls.Add(label3);
					}
					else
					{
						LinkButton linkButton3 = new DataGridLinkButton();
						linkButton3.Text = text;
						linkButton3.CommandName = "Page";
						linkButton3.CommandArgument = text;
						linkButton3.CausesValidation = false;
						tableCell.Controls.Add(linkButton3);
					}
					if (i < num4)
					{
						tableCell.Controls.Add(new LiteralControl("&nbsp;"));
					}
				}
				if (pageCount > num4)
				{
					tableCell.Controls.Add(new LiteralControl("&nbsp;"));
					LinkButton linkButton3 = new DataGridLinkButton();
					linkButton3.Text = "...";
					linkButton3.CommandName = "Page";
					linkButton3.CommandArgument = (num4 + 1).ToString(NumberFormatInfo.InvariantInfo);
					linkButton3.CausesValidation = false;
					tableCell.Controls.Add(linkButton3);
				}
			}
			item.Cells.Add(tableCell);
		}

		// Token: 0x06004200 RID: 16896 RVA: 0x001115C4 File Offset: 0x001105C4
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
					((IStateManager)this.Columns).LoadViewState(array[1]);
				}
				if (array[2] != null)
				{
					((IStateManager)this.PagerStyle).LoadViewState(array[2]);
				}
				if (array[3] != null)
				{
					((IStateManager)this.HeaderStyle).LoadViewState(array[3]);
				}
				if (array[4] != null)
				{
					((IStateManager)this.FooterStyle).LoadViewState(array[4]);
				}
				if (array[5] != null)
				{
					((IStateManager)this.ItemStyle).LoadViewState(array[5]);
				}
				if (array[6] != null)
				{
					((IStateManager)this.AlternatingItemStyle).LoadViewState(array[6]);
				}
				if (array[7] != null)
				{
					((IStateManager)this.SelectedItemStyle).LoadViewState(array[7]);
				}
				if (array[8] != null)
				{
					((IStateManager)this.EditItemStyle).LoadViewState(array[8]);
				}
				if (array[9] != null)
				{
					((IStateManager)base.ControlStyle).LoadViewState(array[9]);
				}
				if (array[10] != null)
				{
					object[] array2 = (object[])array[10];
					int num = array2.Length;
					if (num != 0)
					{
						this.autoGenColumnsArray = new ArrayList();
					}
					else
					{
						this.autoGenColumnsArray = null;
					}
					for (int i = 0; i < num; i++)
					{
						BoundColumn boundColumn = new BoundColumn();
						((IStateManager)boundColumn).TrackViewState();
						((IStateManager)boundColumn).LoadViewState(array2[i]);
						boundColumn.SetOwner(this);
						this.autoGenColumnsArray.Add(boundColumn);
					}
				}
			}
		}

		// Token: 0x06004201 RID: 16897 RVA: 0x00111700 File Offset: 0x00110700
		protected override bool OnBubbleEvent(object source, EventArgs e)
		{
			bool flag = false;
			if (e is DataGridCommandEventArgs)
			{
				DataGridCommandEventArgs dataGridCommandEventArgs = (DataGridCommandEventArgs)e;
				this.OnItemCommand(dataGridCommandEventArgs);
				flag = true;
				string commandName = dataGridCommandEventArgs.CommandName;
				if (StringUtil.EqualsIgnoreCase(commandName, "Select"))
				{
					this.SelectedIndex = dataGridCommandEventArgs.Item.ItemIndex;
					this.OnSelectedIndexChanged(EventArgs.Empty);
				}
				else if (StringUtil.EqualsIgnoreCase(commandName, "Page"))
				{
					string text = (string)dataGridCommandEventArgs.CommandArgument;
					int num = this.CurrentPageIndex;
					if (StringUtil.EqualsIgnoreCase(text, "Next"))
					{
						num++;
					}
					else if (StringUtil.EqualsIgnoreCase(text, "Prev"))
					{
						num--;
					}
					else
					{
						num = int.Parse(text, CultureInfo.InvariantCulture) - 1;
					}
					DataGridPageChangedEventArgs dataGridPageChangedEventArgs = new DataGridPageChangedEventArgs(source, num);
					this.OnPageIndexChanged(dataGridPageChangedEventArgs);
				}
				else if (StringUtil.EqualsIgnoreCase(commandName, "Sort"))
				{
					DataGridSortCommandEventArgs dataGridSortCommandEventArgs = new DataGridSortCommandEventArgs(source, dataGridCommandEventArgs);
					this.OnSortCommand(dataGridSortCommandEventArgs);
				}
				else if (StringUtil.EqualsIgnoreCase(commandName, "Edit"))
				{
					this.OnEditCommand(dataGridCommandEventArgs);
				}
				else if (StringUtil.EqualsIgnoreCase(commandName, "Update"))
				{
					this.OnUpdateCommand(dataGridCommandEventArgs);
				}
				else if (StringUtil.EqualsIgnoreCase(commandName, "Cancel"))
				{
					this.OnCancelCommand(dataGridCommandEventArgs);
				}
				else if (StringUtil.EqualsIgnoreCase(commandName, "Delete"))
				{
					this.OnDeleteCommand(dataGridCommandEventArgs);
				}
			}
			return flag;
		}

		// Token: 0x06004202 RID: 16898 RVA: 0x00111844 File Offset: 0x00110844
		internal void OnColumnsChanged()
		{
			if (base.Initialized)
			{
				base.RequiresDataBinding = true;
			}
		}

		// Token: 0x06004203 RID: 16899 RVA: 0x00111858 File Offset: 0x00110858
		protected virtual void OnCancelCommand(DataGridCommandEventArgs e)
		{
			DataGridCommandEventHandler dataGridCommandEventHandler = (DataGridCommandEventHandler)base.Events[DataGrid.EventCancelCommand];
			if (dataGridCommandEventHandler != null)
			{
				dataGridCommandEventHandler(this, e);
			}
		}

		// Token: 0x06004204 RID: 16900 RVA: 0x00111888 File Offset: 0x00110888
		protected virtual void OnDeleteCommand(DataGridCommandEventArgs e)
		{
			DataGridCommandEventHandler dataGridCommandEventHandler = (DataGridCommandEventHandler)base.Events[DataGrid.EventDeleteCommand];
			if (dataGridCommandEventHandler != null)
			{
				dataGridCommandEventHandler(this, e);
			}
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x001118B8 File Offset: 0x001108B8
		protected virtual void OnEditCommand(DataGridCommandEventArgs e)
		{
			DataGridCommandEventHandler dataGridCommandEventHandler = (DataGridCommandEventHandler)base.Events[DataGrid.EventEditCommand];
			if (dataGridCommandEventHandler != null)
			{
				dataGridCommandEventHandler(this, e);
			}
		}

		// Token: 0x06004206 RID: 16902 RVA: 0x001118E8 File Offset: 0x001108E8
		protected virtual void OnItemCommand(DataGridCommandEventArgs e)
		{
			DataGridCommandEventHandler dataGridCommandEventHandler = (DataGridCommandEventHandler)base.Events[DataGrid.EventItemCommand];
			if (dataGridCommandEventHandler != null)
			{
				dataGridCommandEventHandler(this, e);
			}
		}

		// Token: 0x06004207 RID: 16903 RVA: 0x00111918 File Offset: 0x00110918
		protected virtual void OnItemCreated(DataGridItemEventArgs e)
		{
			DataGridItemEventHandler dataGridItemEventHandler = (DataGridItemEventHandler)base.Events[DataGrid.EventItemCreated];
			if (dataGridItemEventHandler != null)
			{
				dataGridItemEventHandler(this, e);
			}
		}

		// Token: 0x06004208 RID: 16904 RVA: 0x00111948 File Offset: 0x00110948
		protected virtual void OnItemDataBound(DataGridItemEventArgs e)
		{
			DataGridItemEventHandler dataGridItemEventHandler = (DataGridItemEventHandler)base.Events[DataGrid.EventItemDataBound];
			if (dataGridItemEventHandler != null)
			{
				dataGridItemEventHandler(this, e);
			}
		}

		// Token: 0x06004209 RID: 16905 RVA: 0x00111978 File Offset: 0x00110978
		protected virtual void OnPageIndexChanged(DataGridPageChangedEventArgs e)
		{
			DataGridPageChangedEventHandler dataGridPageChangedEventHandler = (DataGridPageChangedEventHandler)base.Events[DataGrid.EventPageIndexChanged];
			if (dataGridPageChangedEventHandler != null)
			{
				dataGridPageChangedEventHandler(this, e);
			}
		}

		// Token: 0x0600420A RID: 16906 RVA: 0x001119A6 File Offset: 0x001109A6
		internal void OnPagerChanged()
		{
		}

		// Token: 0x0600420B RID: 16907 RVA: 0x001119A8 File Offset: 0x001109A8
		protected virtual void OnSortCommand(DataGridSortCommandEventArgs e)
		{
			DataGridSortCommandEventHandler dataGridSortCommandEventHandler = (DataGridSortCommandEventHandler)base.Events[DataGrid.EventSortCommand];
			if (dataGridSortCommandEventHandler != null)
			{
				dataGridSortCommandEventHandler(this, e);
			}
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x001119D8 File Offset: 0x001109D8
		protected virtual void OnUpdateCommand(DataGridCommandEventArgs e)
		{
			DataGridCommandEventHandler dataGridCommandEventHandler = (DataGridCommandEventHandler)base.Events[DataGrid.EventUpdateCommand];
			if (dataGridCommandEventHandler != null)
			{
				dataGridCommandEventHandler(this, e);
			}
		}

		// Token: 0x0600420D RID: 16909 RVA: 0x00111A08 File Offset: 0x00110A08
		protected internal override void PrepareControlHierarchy()
		{
			if (this.Controls.Count == 0)
			{
				return;
			}
			Table table = (Table)this.Controls[0];
			table.CopyBaseAttributes(this);
			table.Caption = this.Caption;
			table.CaptionAlign = this.CaptionAlign;
			if (base.ControlStyleCreated)
			{
				table.ApplyStyle(base.ControlStyle);
			}
			else
			{
				table.GridLines = GridLines.Both;
				table.CellSpacing = 0;
			}
			TableRowCollection rows = table.Rows;
			int count = rows.Count;
			if (count == 0)
			{
				return;
			}
			int count2 = this.Columns.Count;
			DataGridColumn[] array = new DataGridColumn[count2];
			if (count2 > 0)
			{
				this.Columns.CopyTo(array, 0);
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
			int num = 0;
			bool flag = true;
			int i = 0;
			while (i < count)
			{
				DataGridItem dataGridItem = (DataGridItem)rows[i];
				switch (dataGridItem.ItemType)
				{
				case ListItemType.Header:
					if (!this.ShowHeader)
					{
						dataGridItem.Visible = false;
					}
					else
					{
						if (this.headerStyle != null)
						{
							dataGridItem.MergeStyle(this.headerStyle);
							goto IL_029E;
						}
						goto IL_029E;
					}
					break;
				case ListItemType.Footer:
					if (this.ShowFooter)
					{
						dataGridItem.MergeStyle(this.footerStyle);
						goto IL_029E;
					}
					dataGridItem.Visible = false;
					break;
				case ListItemType.Item:
					dataGridItem.MergeStyle(this.itemStyle);
					goto IL_029E;
				case ListItemType.AlternatingItem:
					dataGridItem.MergeStyle(style);
					goto IL_029E;
				case ListItemType.SelectedItem:
				{
					Style style2 = new TableItemStyle();
					if (dataGridItem.ItemIndex % 2 != 0)
					{
						style2.CopyFrom(style);
					}
					else
					{
						style2.CopyFrom(this.itemStyle);
					}
					style2.CopyFrom(this.selectedItemStyle);
					dataGridItem.MergeStyle(style2);
					goto IL_029E;
				}
				case ListItemType.EditItem:
				{
					Style style3 = new TableItemStyle();
					if (dataGridItem.ItemIndex % 2 != 0)
					{
						style3.CopyFrom(style);
					}
					else
					{
						style3.CopyFrom(this.itemStyle);
					}
					if (dataGridItem.ItemIndex == this.SelectedIndex)
					{
						style3.CopyFrom(this.selectedItemStyle);
					}
					style3.CopyFrom(this.editItemStyle);
					dataGridItem.MergeStyle(style3);
					goto IL_029E;
				}
				case ListItemType.Separator:
					goto IL_029E;
				case ListItemType.Pager:
					if (this.pagerStyle.Visible)
					{
						if (i == 0)
						{
							if (!this.pagerStyle.IsPagerOnTop)
							{
								dataGridItem.Visible = false;
								break;
							}
						}
						else if (!this.pagerStyle.IsPagerOnBottom)
						{
							dataGridItem.Visible = false;
							break;
						}
						dataGridItem.MergeStyle(this.pagerStyle);
						goto IL_029E;
					}
					dataGridItem.Visible = false;
					break;
				default:
					goto IL_029E;
				}
				IL_037A:
				i++;
				continue;
				IL_029E:
				TableCellCollection cells = dataGridItem.Cells;
				int count3 = cells.Count;
				if (count2 <= 0 || dataGridItem.ItemType == ListItemType.Pager)
				{
					goto IL_037A;
				}
				int num2 = count3;
				if (count2 < count3)
				{
					num2 = count2;
				}
				for (int j = 0; j < num2; j++)
				{
					if (!array[j].Visible)
					{
						cells[j].Visible = false;
					}
					else
					{
						if (dataGridItem.ItemType == ListItemType.Item && flag)
						{
							num++;
						}
						Style style4;
						switch (dataGridItem.ItemType)
						{
						case ListItemType.Header:
							style4 = array[j].HeaderStyleInternal;
							break;
						case ListItemType.Footer:
							style4 = array[j].FooterStyleInternal;
							break;
						default:
							style4 = array[j].ItemStyleInternal;
							break;
						}
						cells[j].MergeStyle(style4);
					}
				}
				if (dataGridItem.ItemType == ListItemType.Item)
				{
					flag = false;
					goto IL_037A;
				}
				goto IL_037A;
			}
			if (this.Items.Count > 0 && num != this.Items[0].Cells.Count && this.AllowPaging)
			{
				for (int k = 0; k < count; k++)
				{
					DataGridItem dataGridItem2 = (DataGridItem)rows[k];
					if (dataGridItem2.ItemType == ListItemType.Pager && dataGridItem2.Cells.Count > 0)
					{
						dataGridItem2.Cells[0].ColumnSpan = num;
					}
				}
			}
		}

		// Token: 0x0600420E RID: 16910 RVA: 0x00111E1C File Offset: 0x00110E1C
		protected override object SaveViewState()
		{
			object obj = base.SaveViewState();
			object obj2 = ((this.columnCollection != null) ? ((IStateManager)this.columnCollection).SaveViewState() : null);
			object obj3 = ((this.pagerStyle != null) ? ((IStateManager)this.pagerStyle).SaveViewState() : null);
			object obj4 = ((this.headerStyle != null) ? ((IStateManager)this.headerStyle).SaveViewState() : null);
			object obj5 = ((this.footerStyle != null) ? ((IStateManager)this.footerStyle).SaveViewState() : null);
			object obj6 = ((this.itemStyle != null) ? ((IStateManager)this.itemStyle).SaveViewState() : null);
			object obj7 = ((this.alternatingItemStyle != null) ? ((IStateManager)this.alternatingItemStyle).SaveViewState() : null);
			object obj8 = ((this.selectedItemStyle != null) ? ((IStateManager)this.selectedItemStyle).SaveViewState() : null);
			object obj9 = ((this.editItemStyle != null) ? ((IStateManager)this.editItemStyle).SaveViewState() : null);
			object obj10 = (base.ControlStyleCreated ? ((IStateManager)base.ControlStyle).SaveViewState() : null);
			object[] array = null;
			if (this.autoGenColumnsArray != null && this.autoGenColumnsArray.Count != 0)
			{
				array = new object[this.autoGenColumnsArray.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = ((IStateManager)this.autoGenColumnsArray[i]).SaveViewState();
				}
			}
			return new object[]
			{
				obj, obj2, obj3, obj4, obj5, obj6, obj7, obj8, obj9, obj10,
				array
			};
		}

		// Token: 0x0600420F RID: 16911 RVA: 0x00111FAC File Offset: 0x00110FAC
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (this.columnCollection != null)
			{
				((IStateManager)this.columnCollection).TrackViewState();
			}
			if (this.pagerStyle != null)
			{
				((IStateManager)this.pagerStyle).TrackViewState();
			}
			if (this.headerStyle != null)
			{
				((IStateManager)this.headerStyle).TrackViewState();
			}
			if (this.footerStyle != null)
			{
				((IStateManager)this.footerStyle).TrackViewState();
			}
			if (this.itemStyle != null)
			{
				((IStateManager)this.itemStyle).TrackViewState();
			}
			if (this.alternatingItemStyle != null)
			{
				((IStateManager)this.alternatingItemStyle).TrackViewState();
			}
			if (this.selectedItemStyle != null)
			{
				((IStateManager)this.selectedItemStyle).TrackViewState();
			}
			if (this.editItemStyle != null)
			{
				((IStateManager)this.editItemStyle).TrackViewState();
			}
			if (base.ControlStyleCreated)
			{
				((IStateManager)base.ControlStyle).TrackViewState();
			}
		}

		// Token: 0x040028CB RID: 10443
		public const string SortCommandName = "Sort";

		// Token: 0x040028CC RID: 10444
		public const string SelectCommandName = "Select";

		// Token: 0x040028CD RID: 10445
		public const string EditCommandName = "Edit";

		// Token: 0x040028CE RID: 10446
		public const string DeleteCommandName = "Delete";

		// Token: 0x040028CF RID: 10447
		public const string UpdateCommandName = "Update";

		// Token: 0x040028D0 RID: 10448
		public const string CancelCommandName = "Cancel";

		// Token: 0x040028D1 RID: 10449
		public const string PageCommandName = "Page";

		// Token: 0x040028D2 RID: 10450
		public const string NextPageCommandArgument = "Next";

		// Token: 0x040028D3 RID: 10451
		public const string PrevPageCommandArgument = "Prev";

		// Token: 0x040028D4 RID: 10452
		internal const string DataSourceItemCountViewStateKey = "_!DataSourceItemCount";

		// Token: 0x040028D5 RID: 10453
		private static readonly object EventCancelCommand = new object();

		// Token: 0x040028D6 RID: 10454
		private static readonly object EventDeleteCommand = new object();

		// Token: 0x040028D7 RID: 10455
		private static readonly object EventEditCommand = new object();

		// Token: 0x040028D8 RID: 10456
		private static readonly object EventItemCommand = new object();

		// Token: 0x040028D9 RID: 10457
		private static readonly object EventItemCreated = new object();

		// Token: 0x040028DA RID: 10458
		private static readonly object EventItemDataBound = new object();

		// Token: 0x040028DB RID: 10459
		private static readonly object EventPageIndexChanged = new object();

		// Token: 0x040028DC RID: 10460
		private static readonly object EventSortCommand = new object();

		// Token: 0x040028DD RID: 10461
		private static readonly object EventUpdateCommand = new object();

		// Token: 0x040028DE RID: 10462
		private IEnumerator storedData;

		// Token: 0x040028DF RID: 10463
		private object firstDataItem;

		// Token: 0x040028E0 RID: 10464
		private bool storedDataValid;

		// Token: 0x040028E1 RID: 10465
		private PagedDataSource pagedDataSource;

		// Token: 0x040028E2 RID: 10466
		private ArrayList columns;

		// Token: 0x040028E3 RID: 10467
		private DataGridColumnCollection columnCollection;

		// Token: 0x040028E4 RID: 10468
		private TableItemStyle headerStyle;

		// Token: 0x040028E5 RID: 10469
		private TableItemStyle footerStyle;

		// Token: 0x040028E6 RID: 10470
		private TableItemStyle itemStyle;

		// Token: 0x040028E7 RID: 10471
		private TableItemStyle alternatingItemStyle;

		// Token: 0x040028E8 RID: 10472
		private TableItemStyle selectedItemStyle;

		// Token: 0x040028E9 RID: 10473
		private TableItemStyle editItemStyle;

		// Token: 0x040028EA RID: 10474
		private DataGridPagerStyle pagerStyle;

		// Token: 0x040028EB RID: 10475
		private ArrayList itemsArray;

		// Token: 0x040028EC RID: 10476
		private DataGridItemCollection itemsCollection;

		// Token: 0x040028ED RID: 10477
		private ArrayList autoGenColumnsArray;
	}
}
