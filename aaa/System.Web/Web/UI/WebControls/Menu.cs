using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Web.UI.WebControls.Adapters;
using System.Web.Util;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005DF RID: 1503
	[Designer("System.Web.UI.Design.WebControls.MenuDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SupportsEventValidation]
	[ControlValueProperty("SelectedValue")]
	[DefaultEvent("MenuItemClick")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class Menu : HierarchicalDataBoundControl, IPostBackEventHandler, INamingContainer
	{
		// Token: 0x06004994 RID: 18836 RVA: 0x0012B8DB File Offset: 0x0012A8DB
		public Menu()
		{
			this._nodeIndex = 0;
			this._maximumDepth = 0;
		}

		// Token: 0x17001240 RID: 4672
		// (get) Token: 0x06004995 RID: 18837 RVA: 0x0012B8F1 File Offset: 0x0012A8F1
		// (set) Token: 0x06004996 RID: 18838 RVA: 0x0012B8F9 File Offset: 0x0012A8F9
		internal bool AccessKeyRendered
		{
			get
			{
				return this._accessKeyRendered;
			}
			set
			{
				this._accessKeyRendered = value;
			}
		}

		// Token: 0x17001241 RID: 4673
		// (get) Token: 0x06004997 RID: 18839 RVA: 0x0012B902 File Offset: 0x0012A902
		private Collection<int> CachedLevelsContainingCssClass
		{
			get
			{
				if (this._cachedLevelsContainingCssClass == null)
				{
					this._cachedLevelsContainingCssClass = new Collection<int>();
				}
				return this._cachedLevelsContainingCssClass;
			}
		}

		// Token: 0x17001242 RID: 4674
		// (get) Token: 0x06004998 RID: 18840 RVA: 0x0012B91D File Offset: 0x0012A91D
		private List<string> CachedMenuItemClassNames
		{
			get
			{
				if (this._cachedMenuItemClassNames == null)
				{
					this._cachedMenuItemClassNames = new List<string>();
				}
				return this._cachedMenuItemClassNames;
			}
		}

		// Token: 0x17001243 RID: 4675
		// (get) Token: 0x06004999 RID: 18841 RVA: 0x0012B938 File Offset: 0x0012A938
		private List<string> CachedMenuItemHyperLinkClassNames
		{
			get
			{
				if (this._cachedMenuItemHyperLinkClassNames == null)
				{
					this._cachedMenuItemHyperLinkClassNames = new List<string>();
				}
				return this._cachedMenuItemHyperLinkClassNames;
			}
		}

		// Token: 0x17001244 RID: 4676
		// (get) Token: 0x0600499A RID: 18842 RVA: 0x0012B953 File Offset: 0x0012A953
		private List<MenuItemStyle> CachedMenuItemStyles
		{
			get
			{
				if (this._cachedMenuItemStyles == null)
				{
					this._cachedMenuItemStyles = new List<MenuItemStyle>();
				}
				return this._cachedMenuItemStyles;
			}
		}

		// Token: 0x17001245 RID: 4677
		// (get) Token: 0x0600499B RID: 18843 RVA: 0x0012B96E File Offset: 0x0012A96E
		private List<string> CachedSubMenuClassNames
		{
			get
			{
				if (this._cachedSubMenuClassNames == null)
				{
					this._cachedSubMenuClassNames = new List<string>();
				}
				return this._cachedSubMenuClassNames;
			}
		}

		// Token: 0x17001246 RID: 4678
		// (get) Token: 0x0600499C RID: 18844 RVA: 0x0012B989 File Offset: 0x0012A989
		private List<SubMenuStyle> CachedSubMenuStyles
		{
			get
			{
				if (this._cachedSubMenuStyles == null)
				{
					this._cachedSubMenuStyles = new List<SubMenuStyle>();
				}
				return this._cachedSubMenuStyles;
			}
		}

		// Token: 0x17001247 RID: 4679
		// (get) Token: 0x0600499D RID: 18845 RVA: 0x0012B9A4 File Offset: 0x0012A9A4
		internal string ClientDataObjectID
		{
			get
			{
				return this.ClientID + "_Data";
			}
		}

		// Token: 0x17001248 RID: 4680
		// (get) Token: 0x0600499E RID: 18846 RVA: 0x0012B9B6 File Offset: 0x0012A9B6
		public override ControlCollection Controls
		{
			get
			{
				this.EnsureChildControls();
				return base.Controls;
			}
		}

		// Token: 0x17001249 RID: 4681
		// (get) Token: 0x0600499F RID: 18847 RVA: 0x0012B9C4 File Offset: 0x0012A9C4
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DefaultValue(null)]
		[WebSysDescription("Menu_Bindings")]
		[Editor("System.Web.UI.Design.WebControls.MenuBindingsEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[MergableProperty(false)]
		[WebCategory("Data")]
		public MenuItemBindingCollection DataBindings
		{
			get
			{
				if (this._bindings == null)
				{
					this._bindings = new MenuItemBindingCollection(this);
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._bindings).TrackViewState();
					}
				}
				return this._bindings;
			}
		}

		// Token: 0x1700124A RID: 4682
		// (get) Token: 0x060049A0 RID: 18848 RVA: 0x0012B9F4 File Offset: 0x0012A9F4
		// (set) Token: 0x060049A1 RID: 18849 RVA: 0x0012BA21 File Offset: 0x0012AA21
		[WebCategory("Behavior")]
		[WebSysDescription("Menu_DisappearAfter")]
		[DefaultValue(500)]
		[Themeable(false)]
		public int DisappearAfter
		{
			get
			{
				object obj = this.ViewState["DisappearAfter"];
				if (obj == null)
				{
					return 500;
				}
				return (int)obj;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["DisappearAfter"] = value;
			}
		}

		// Token: 0x1700124B RID: 4683
		// (get) Token: 0x060049A2 RID: 18850 RVA: 0x0012BA48 File Offset: 0x0012AA48
		// (set) Token: 0x060049A3 RID: 18851 RVA: 0x0012BA75 File Offset: 0x0012AA75
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
		[Themeable(true)]
		[UrlProperty]
		[WebCategory("Appearance")]
		[WebSysDescription("Menu_DynamicBottomSeparatorImageUrl")]
		public string DynamicBottomSeparatorImageUrl
		{
			get
			{
				object obj = this.ViewState["DynamicBottomSeparatorImageUrl"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["DynamicBottomSeparatorImageUrl"] = value;
			}
		}

		// Token: 0x1700124C RID: 4684
		// (get) Token: 0x060049A4 RID: 18852 RVA: 0x0012BA88 File Offset: 0x0012AA88
		// (set) Token: 0x060049A5 RID: 18853 RVA: 0x0012BAB1 File Offset: 0x0012AAB1
		[DefaultValue(true)]
		[WebSysDescription("Menu_DynamicDisplayPopOutImage")]
		[WebCategory("Appearance")]
		public bool DynamicEnableDefaultPopOutImage
		{
			get
			{
				object obj = this.ViewState["DynamicEnableDefaultPopOutImage"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["DynamicEnableDefaultPopOutImage"] = value;
			}
		}

		// Token: 0x1700124D RID: 4685
		// (get) Token: 0x060049A6 RID: 18854 RVA: 0x0012BACC File Offset: 0x0012AACC
		// (set) Token: 0x060049A7 RID: 18855 RVA: 0x0012BAF5 File Offset: 0x0012AAF5
		[WebSysDescription("Menu_DynamicHorizontalOffset")]
		[DefaultValue(0)]
		[WebCategory("Appearance")]
		public int DynamicHorizontalOffset
		{
			get
			{
				object obj = this.ViewState["DynamicHorizontalOffset"];
				if (obj == null)
				{
					return 0;
				}
				return (int)obj;
			}
			set
			{
				this.ViewState["DynamicHorizontalOffset"] = value;
			}
		}

		// Token: 0x1700124E RID: 4686
		// (get) Token: 0x060049A8 RID: 18856 RVA: 0x0012BB0D File Offset: 0x0012AB0D
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[WebCategory("Styles")]
		[WebSysDescription("Menu_DynamicHoverStyle")]
		public Style DynamicHoverStyle
		{
			get
			{
				if (this._dynamicHoverStyle == null)
				{
					this._dynamicHoverStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._dynamicHoverStyle).TrackViewState();
					}
				}
				return this._dynamicHoverStyle;
			}
		}

		// Token: 0x1700124F RID: 4687
		// (get) Token: 0x060049A9 RID: 18857 RVA: 0x0012BB3C File Offset: 0x0012AB3C
		// (set) Token: 0x060049AA RID: 18858 RVA: 0x0012BB69 File Offset: 0x0012AB69
		[DefaultValue("")]
		[WebSysDescription("Menu_DynamicItemFormatString")]
		[WebCategory("Appearance")]
		public string DynamicItemFormatString
		{
			get
			{
				object obj = this.ViewState["DynamicItemFormatString"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["DynamicItemFormatString"] = value;
			}
		}

		// Token: 0x17001250 RID: 4688
		// (get) Token: 0x060049AB RID: 18859 RVA: 0x0012BB7C File Offset: 0x0012AB7C
		[WebSysDescription("Menu_DynamicMenuItemStyle")]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public MenuItemStyle DynamicMenuItemStyle
		{
			get
			{
				if (this._dynamicItemStyle == null)
				{
					this._dynamicItemStyle = new MenuItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._dynamicItemStyle).TrackViewState();
					}
				}
				return this._dynamicItemStyle;
			}
		}

		// Token: 0x17001251 RID: 4689
		// (get) Token: 0x060049AC RID: 18860 RVA: 0x0012BBAA File Offset: 0x0012ABAA
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[WebSysDescription("Menu_DynamicMenuStyle")]
		public SubMenuStyle DynamicMenuStyle
		{
			get
			{
				if (this._dynamicMenuStyle == null)
				{
					this._dynamicMenuStyle = new SubMenuStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._dynamicMenuStyle).TrackViewState();
					}
				}
				return this._dynamicMenuStyle;
			}
		}

		// Token: 0x17001252 RID: 4690
		// (get) Token: 0x060049AD RID: 18861 RVA: 0x0012BBD8 File Offset: 0x0012ABD8
		// (set) Token: 0x060049AE RID: 18862 RVA: 0x0012BC05 File Offset: 0x0012AC05
		[DefaultValue("")]
		[WebSysDescription("Menu_DynamicPopoutImageUrl")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		[WebCategory("Appearance")]
		public string DynamicPopOutImageUrl
		{
			get
			{
				object obj = this.ViewState["DynamicPopOutImageUrl"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["DynamicPopOutImageUrl"] = value;
			}
		}

		// Token: 0x17001253 RID: 4691
		// (get) Token: 0x060049AF RID: 18863 RVA: 0x0012BC18 File Offset: 0x0012AC18
		// (set) Token: 0x060049B0 RID: 18864 RVA: 0x0012BC4A File Offset: 0x0012AC4A
		[WebCategory("Appearance")]
		[WebSysDescription("Menu_DynamicPopoutImageText")]
		[WebSysDefaultValue("MenuAdapter_Expand")]
		public string DynamicPopOutImageTextFormatString
		{
			get
			{
				object obj = this.ViewState["DynamicPopOutImageTextFormatString"];
				if (obj == null)
				{
					return SR.GetString("MenuAdapter_Expand");
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["DynamicPopOutImageTextFormatString"] = value;
			}
		}

		// Token: 0x17001254 RID: 4692
		// (get) Token: 0x060049B1 RID: 18865 RVA: 0x0012BC5D File Offset: 0x0012AC5D
		[NotifyParentProperty(true)]
		[WebSysDescription("Menu_DynamicSelectedStyle")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public MenuItemStyle DynamicSelectedStyle
		{
			get
			{
				if (this._dynamicSelectedStyle == null)
				{
					this._dynamicSelectedStyle = new MenuItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._dynamicSelectedStyle).TrackViewState();
					}
				}
				return this._dynamicSelectedStyle;
			}
		}

		// Token: 0x17001255 RID: 4693
		// (get) Token: 0x060049B2 RID: 18866 RVA: 0x0012BC8B File Offset: 0x0012AC8B
		// (set) Token: 0x060049B3 RID: 18867 RVA: 0x0012BC93 File Offset: 0x0012AC93
		[WebSysDescription("Menu_DynamicTemplate")]
		[Browsable(false)]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(MenuItemTemplateContainer))]
		public ITemplate DynamicItemTemplate
		{
			get
			{
				return this._dynamicTemplate;
			}
			set
			{
				this._dynamicTemplate = value;
			}
		}

		// Token: 0x17001256 RID: 4694
		// (get) Token: 0x060049B4 RID: 18868 RVA: 0x0012BC9C File Offset: 0x0012AC9C
		// (set) Token: 0x060049B5 RID: 18869 RVA: 0x0012BCC9 File Offset: 0x0012ACC9
		[WebCategory("Appearance")]
		[WebSysDescription("Menu_DynamicTopSeparatorImageUrl")]
		[UrlProperty]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string DynamicTopSeparatorImageUrl
		{
			get
			{
				object obj = this.ViewState["DynamicTopSeparatorImageUrl"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["DynamicTopSeparatorImageUrl"] = value;
			}
		}

		// Token: 0x17001257 RID: 4695
		// (get) Token: 0x060049B6 RID: 18870 RVA: 0x0012BCDC File Offset: 0x0012ACDC
		// (set) Token: 0x060049B7 RID: 18871 RVA: 0x0012BD05 File Offset: 0x0012AD05
		[DefaultValue(0)]
		[WebSysDescription("Menu_DynamicVerticalOffset")]
		[WebCategory("Appearance")]
		public int DynamicVerticalOffset
		{
			get
			{
				object obj = this.ViewState["DynamicVerticalOffset"];
				if (obj == null)
				{
					return 0;
				}
				return (int)obj;
			}
			set
			{
				this.ViewState["DynamicVerticalOffset"] = value;
			}
		}

		// Token: 0x17001258 RID: 4696
		// (get) Token: 0x060049B8 RID: 18872 RVA: 0x0012BD1D File Offset: 0x0012AD1D
		private string[] ImageUrls
		{
			get
			{
				if (this._imageUrls == null)
				{
					this._imageUrls = new string[3];
				}
				return this._imageUrls;
			}
		}

		// Token: 0x17001259 RID: 4697
		// (get) Token: 0x060049B9 RID: 18873 RVA: 0x0012BD39 File Offset: 0x0012AD39
		internal bool IsNotIE
		{
			get
			{
				return this._isNotIE;
			}
		}

		// Token: 0x1700125A RID: 4698
		// (get) Token: 0x060049BA RID: 18874 RVA: 0x0012BD41 File Offset: 0x0012AD41
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DefaultValue(null)]
		[WebSysDescription("Menu_Items")]
		[Editor("System.Web.UI.Design.WebControls.MenuItemCollectionEditor,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[MergableProperty(false)]
		public MenuItemCollection Items
		{
			get
			{
				return this.RootItem.ChildItems;
			}
		}

		// Token: 0x1700125B RID: 4699
		// (get) Token: 0x060049BB RID: 18875 RVA: 0x0012BD50 File Offset: 0x0012AD50
		// (set) Token: 0x060049BC RID: 18876 RVA: 0x0012BD79 File Offset: 0x0012AD79
		[DefaultValue(false)]
		[WebCategory("Appearance")]
		[WebSysDescription("Menu_ItemWrap")]
		public bool ItemWrap
		{
			get
			{
				object obj = this.ViewState["ItemWrap"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["ItemWrap"] = value;
			}
		}

		// Token: 0x1700125C RID: 4700
		// (get) Token: 0x060049BD RID: 18877 RVA: 0x0012BD91 File Offset: 0x0012AD91
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DefaultValue(null)]
		[Editor("System.Web.UI.Design.WebControls.MenuItemStyleCollectionEditor,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebCategory("Styles")]
		[WebSysDescription("Menu_LevelMenuItemStyles")]
		public MenuItemStyleCollection LevelMenuItemStyles
		{
			get
			{
				if (this._levelMenuItemStyles == null)
				{
					this._levelMenuItemStyles = new MenuItemStyleCollection();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._levelMenuItemStyles).TrackViewState();
					}
				}
				return this._levelMenuItemStyles;
			}
		}

		// Token: 0x1700125D RID: 4701
		// (get) Token: 0x060049BE RID: 18878 RVA: 0x0012BDBF File Offset: 0x0012ADBF
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[Editor("System.Web.UI.Design.WebControls.MenuItemStyleCollectionEditor,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue(null)]
		[WebCategory("Styles")]
		[WebSysDescription("Menu_LevelSelectedStyles")]
		public MenuItemStyleCollection LevelSelectedStyles
		{
			get
			{
				if (this._levelSelectedStyles == null)
				{
					this._levelSelectedStyles = new MenuItemStyleCollection();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._levelSelectedStyles).TrackViewState();
					}
				}
				return this._levelSelectedStyles;
			}
		}

		// Token: 0x1700125E RID: 4702
		// (get) Token: 0x060049BF RID: 18879 RVA: 0x0012BDED File Offset: 0x0012ADED
		[DefaultValue(null)]
		[Editor("System.Web.UI.Design.WebControls.SubMenuStyleCollectionEditor,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		[WebSysDescription("Menu_LevelSubMenuStyles")]
		public SubMenuStyleCollection LevelSubMenuStyles
		{
			get
			{
				if (this._levelStyles == null)
				{
					this._levelStyles = new SubMenuStyleCollection();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._levelStyles).TrackViewState();
					}
				}
				return this._levelStyles;
			}
		}

		// Token: 0x1700125F RID: 4703
		// (get) Token: 0x060049C0 RID: 18880 RVA: 0x0012BE1C File Offset: 0x0012AE1C
		internal int MaximumDepth
		{
			get
			{
				if (this._maximumDepth > 0)
				{
					return this._maximumDepth;
				}
				this._maximumDepth = this.MaximumDynamicDisplayLevels + this.StaticDisplayLevels;
				if (this._maximumDepth < this.MaximumDynamicDisplayLevels || this._maximumDepth < this.StaticDisplayLevels)
				{
					this._maximumDepth = int.MaxValue;
				}
				return this._maximumDepth;
			}
		}

		// Token: 0x17001260 RID: 4704
		// (get) Token: 0x060049C1 RID: 18881 RVA: 0x0012BE7C File Offset: 0x0012AE7C
		// (set) Token: 0x060049C2 RID: 18882 RVA: 0x0012BEA8 File Offset: 0x0012AEA8
		[Themeable(true)]
		[WebSysDescription("Menu_MaximumDynamicDisplayLevels")]
		[WebCategory("Behavior")]
		[DefaultValue(3)]
		public int MaximumDynamicDisplayLevels
		{
			get
			{
				object obj = this.ViewState["MaximumDynamicDisplayLevels"];
				if (obj == null)
				{
					return 3;
				}
				return (int)obj;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("MaximumDynamicDisplayLevels", SR.GetString("Menu_MaximumDynamicDisplayLevelsInvalid"));
				}
				this.ViewState["MaximumDynamicDisplayLevels"] = value;
				this._maximumDepth = 0;
				if (this._dataBound)
				{
					this._dataBound = false;
					this.PerformDataBinding();
				}
			}
		}

		// Token: 0x17001261 RID: 4705
		// (get) Token: 0x060049C3 RID: 18883 RVA: 0x0012BF00 File Offset: 0x0012AF00
		// (set) Token: 0x060049C4 RID: 18884 RVA: 0x0012BF29 File Offset: 0x0012AF29
		[WebSysDescription("Menu_Orientation")]
		[WebCategory("Layout")]
		[DefaultValue(Orientation.Vertical)]
		public Orientation Orientation
		{
			get
			{
				object obj = this.ViewState["Orientation"];
				if (obj == null)
				{
					return Orientation.Vertical;
				}
				return (Orientation)obj;
			}
			set
			{
				this.ViewState["Orientation"] = value;
			}
		}

		// Token: 0x17001262 RID: 4706
		// (get) Token: 0x060049C5 RID: 18885 RVA: 0x0012BF41 File Offset: 0x0012AF41
		internal PopOutPanel Panel
		{
			get
			{
				if (this._panel == null)
				{
					this._panel = new PopOutPanel(this, this._panelStyle);
					if (!base.DesignMode)
					{
						this._panel.Page = this.Page;
					}
				}
				return this._panel;
			}
		}

		// Token: 0x17001263 RID: 4707
		// (get) Token: 0x060049C6 RID: 18886 RVA: 0x0012BF7C File Offset: 0x0012AF7C
		// (set) Token: 0x060049C7 RID: 18887 RVA: 0x0012BFA8 File Offset: 0x0012AFA8
		[WebSysDescription("Menu_PathSeparator")]
		[DefaultValue('/')]
		public char PathSeparator
		{
			get
			{
				object obj = this.ViewState["PathSeparator"];
				if (obj == null)
				{
					return '/';
				}
				return (char)obj;
			}
			set
			{
				if (value == '\0')
				{
					this.ViewState["PathSeparator"] = null;
				}
				else
				{
					this.ViewState["PathSeparator"] = value;
				}
				foreach (object obj in this.Items)
				{
					MenuItem menuItem = (MenuItem)obj;
					menuItem.ResetValuePathRecursive();
				}
			}
		}

		// Token: 0x17001264 RID: 4708
		// (get) Token: 0x060049C8 RID: 18888 RVA: 0x0012C02C File Offset: 0x0012B02C
		internal string PopoutImageUrlInternal
		{
			get
			{
				if (this._cachedPopOutImageUrl != null)
				{
					return this._cachedPopOutImageUrl;
				}
				this._cachedPopOutImageUrl = this.Page.ClientScript.GetWebResourceUrl(typeof(Menu), "Menu_Popout.gif");
				return this._cachedPopOutImageUrl;
			}
		}

		// Token: 0x17001265 RID: 4709
		// (get) Token: 0x060049C9 RID: 18889 RVA: 0x0012C068 File Offset: 0x0012B068
		internal MenuItem RootItem
		{
			get
			{
				if (this._rootItem == null)
				{
					this._rootItem = new MenuItem(this, true);
				}
				return this._rootItem;
			}
		}

		// Token: 0x17001266 RID: 4710
		// (get) Token: 0x060049CA RID: 18890 RVA: 0x0012C085 File Offset: 0x0012B085
		internal Style RootMenuItemStyle
		{
			get
			{
				this.EnsureRootMenuStyle();
				return this._rootMenuItemStyle;
			}
		}

		// Token: 0x17001267 RID: 4711
		// (get) Token: 0x060049CB RID: 18891 RVA: 0x0012C094 File Offset: 0x0012B094
		// (set) Token: 0x060049CC RID: 18892 RVA: 0x0012C0C1 File Offset: 0x0012B0C1
		[DefaultValue("")]
		[WebSysDescription("Menu_ScrollDownImageUrl")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		[WebCategory("Appearance")]
		public string ScrollDownImageUrl
		{
			get
			{
				object obj = this.ViewState["ScrollDownImageUrl"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["ScrollDownImageUrl"] = value;
			}
		}

		// Token: 0x17001268 RID: 4712
		// (get) Token: 0x060049CD RID: 18893 RVA: 0x0012C0D4 File Offset: 0x0012B0D4
		internal string ScrollDownImageUrlInternal
		{
			get
			{
				if (this._cachedScrollDownImageUrl != null)
				{
					return this._cachedScrollDownImageUrl;
				}
				this._cachedScrollDownImageUrl = this.Page.ClientScript.GetWebResourceUrl(typeof(Menu), "Menu_ScrollDown.gif");
				return this._cachedScrollDownImageUrl;
			}
		}

		// Token: 0x17001269 RID: 4713
		// (get) Token: 0x060049CE RID: 18894 RVA: 0x0012C110 File Offset: 0x0012B110
		// (set) Token: 0x060049CF RID: 18895 RVA: 0x0012C142 File Offset: 0x0012B142
		[WebSysDefaultValue("Menu_ScrollDown")]
		[WebSysDescription("Menu_ScrollDownText")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		public string ScrollDownText
		{
			get
			{
				object obj = this.ViewState["ScrollDownText"];
				if (obj == null)
				{
					return SR.GetString("Menu_ScrollDown");
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["ScrollDownText"] = value;
			}
		}

		// Token: 0x1700126A RID: 4714
		// (get) Token: 0x060049D0 RID: 18896 RVA: 0x0012C158 File Offset: 0x0012B158
		// (set) Token: 0x060049D1 RID: 18897 RVA: 0x0012C185 File Offset: 0x0012B185
		[WebCategory("Appearance")]
		[WebSysDescription("Menu_ScrollUpImageUrl")]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		public string ScrollUpImageUrl
		{
			get
			{
				object obj = this.ViewState["ScrollUpImageUrl"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["ScrollUpImageUrl"] = value;
			}
		}

		// Token: 0x1700126B RID: 4715
		// (get) Token: 0x060049D2 RID: 18898 RVA: 0x0012C198 File Offset: 0x0012B198
		internal string ScrollUpImageUrlInternal
		{
			get
			{
				if (this._cachedScrollUpImageUrl != null)
				{
					return this._cachedScrollUpImageUrl;
				}
				this._cachedScrollUpImageUrl = this.Page.ClientScript.GetWebResourceUrl(typeof(Menu), "Menu_ScrollUp.gif");
				return this._cachedScrollUpImageUrl;
			}
		}

		// Token: 0x1700126C RID: 4716
		// (get) Token: 0x060049D3 RID: 18899 RVA: 0x0012C1D4 File Offset: 0x0012B1D4
		// (set) Token: 0x060049D4 RID: 18900 RVA: 0x0012C206 File Offset: 0x0012B206
		[WebSysDefaultValue("Menu_ScrollUp")]
		[WebSysDescription("Menu_ScrollUpText")]
		[WebCategory("Appearance")]
		[Localizable(true)]
		public string ScrollUpText
		{
			get
			{
				object obj = this.ViewState["ScrollUpText"];
				if (obj == null)
				{
					return SR.GetString("Menu_ScrollUp");
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["ScrollUpText"] = value;
			}
		}

		// Token: 0x1700126D RID: 4717
		// (get) Token: 0x060049D5 RID: 18901 RVA: 0x0012C219 File Offset: 0x0012B219
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public MenuItem SelectedItem
		{
			get
			{
				return this._selectedItem;
			}
		}

		// Token: 0x1700126E RID: 4718
		// (get) Token: 0x060049D6 RID: 18902 RVA: 0x0012C221 File Offset: 0x0012B221
		[Browsable(false)]
		[DefaultValue("")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SelectedValue
		{
			get
			{
				if (this.SelectedItem != null)
				{
					return this.SelectedItem.Value;
				}
				return string.Empty;
			}
		}

		// Token: 0x1700126F RID: 4719
		// (get) Token: 0x060049D7 RID: 18903 RVA: 0x0012C23C File Offset: 0x0012B23C
		// (set) Token: 0x060049D8 RID: 18904 RVA: 0x0012C26E File Offset: 0x0012B26E
		[WebCategory("Accessibility")]
		[WebSysDescription("WebControl_SkipLinkText")]
		[WebSysDefaultValue("Menu_SkipLinkTextDefault")]
		[Localizable(true)]
		public string SkipLinkText
		{
			get
			{
				object obj = this.ViewState["SkipLinkText"];
				if (obj == null)
				{
					return SR.GetString("Menu_SkipLinkTextDefault");
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["SkipLinkText"] = value;
			}
		}

		// Token: 0x17001270 RID: 4720
		// (get) Token: 0x060049D9 RID: 18905 RVA: 0x0012C284 File Offset: 0x0012B284
		// (set) Token: 0x060049DA RID: 18906 RVA: 0x0012C2B1 File Offset: 0x0012B2B1
		[WebSysDescription("Menu_StaticBottomSeparatorImageUrl")]
		[UrlProperty]
		[WebCategory("Appearance")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
		public string StaticBottomSeparatorImageUrl
		{
			get
			{
				object obj = this.ViewState["StaticBottomSeparatorImageUrl"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["StaticBottomSeparatorImageUrl"] = value;
			}
		}

		// Token: 0x17001271 RID: 4721
		// (get) Token: 0x060049DB RID: 18907 RVA: 0x0012C2C4 File Offset: 0x0012B2C4
		// (set) Token: 0x060049DC RID: 18908 RVA: 0x0012C2F0 File Offset: 0x0012B2F0
		[WebSysDescription("Menu_StaticDisplayLevels")]
		[DefaultValue(1)]
		[Themeable(true)]
		[WebCategory("Behavior")]
		public int StaticDisplayLevels
		{
			get
			{
				object obj = this.ViewState["StaticDisplayLevels"];
				if (obj == null)
				{
					return 1;
				}
				return (int)obj;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["StaticDisplayLevels"] = value;
				this._maximumDepth = 0;
				if (this._dataBound && !base.DesignMode)
				{
					this._dataBound = false;
					this.PerformDataBinding();
				}
			}
		}

		// Token: 0x17001272 RID: 4722
		// (get) Token: 0x060049DD RID: 18909 RVA: 0x0012C348 File Offset: 0x0012B348
		// (set) Token: 0x060049DE RID: 18910 RVA: 0x0012C371 File Offset: 0x0012B371
		[DefaultValue(true)]
		[WebCategory("Appearance")]
		[WebSysDescription("Menu_StaticDisplayPopOutImage")]
		public bool StaticEnableDefaultPopOutImage
		{
			get
			{
				object obj = this.ViewState["StaticEnableDefaultPopOutImage"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["StaticEnableDefaultPopOutImage"] = value;
			}
		}

		// Token: 0x17001273 RID: 4723
		// (get) Token: 0x060049DF RID: 18911 RVA: 0x0012C389 File Offset: 0x0012B389
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		[WebSysDescription("Menu_StaticHoverStyle")]
		public Style StaticHoverStyle
		{
			get
			{
				if (this._staticHoverStyle == null)
				{
					this._staticHoverStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._staticHoverStyle).TrackViewState();
					}
				}
				return this._staticHoverStyle;
			}
		}

		// Token: 0x17001274 RID: 4724
		// (get) Token: 0x060049E0 RID: 18912 RVA: 0x0012C3B8 File Offset: 0x0012B3B8
		// (set) Token: 0x060049E1 RID: 18913 RVA: 0x0012C3E5 File Offset: 0x0012B3E5
		[WebSysDescription("Menu_StaticItemFormatString")]
		[DefaultValue("")]
		[WebCategory("Appearance")]
		public string StaticItemFormatString
		{
			get
			{
				object obj = this.ViewState["StaticItemFormatString"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["StaticItemFormatString"] = value;
			}
		}

		// Token: 0x17001275 RID: 4725
		// (get) Token: 0x060049E2 RID: 18914 RVA: 0x0012C3F8 File Offset: 0x0012B3F8
		[DefaultValue(null)]
		[WebSysDescription("Menu_StaticMenuItemStyle")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		public MenuItemStyle StaticMenuItemStyle
		{
			get
			{
				if (this._staticItemStyle == null)
				{
					this._staticItemStyle = new MenuItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._staticItemStyle).TrackViewState();
					}
				}
				return this._staticItemStyle;
			}
		}

		// Token: 0x17001276 RID: 4726
		// (get) Token: 0x060049E3 RID: 18915 RVA: 0x0012C426 File Offset: 0x0012B426
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebSysDescription("Menu_StaticMenuStyle")]
		[DefaultValue(null)]
		[WebCategory("Styles")]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public SubMenuStyle StaticMenuStyle
		{
			get
			{
				if (this._staticMenuStyle == null)
				{
					this._staticMenuStyle = new SubMenuStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._staticMenuStyle).TrackViewState();
					}
				}
				return this._staticMenuStyle;
			}
		}

		// Token: 0x17001277 RID: 4727
		// (get) Token: 0x060049E4 RID: 18916 RVA: 0x0012C454 File Offset: 0x0012B454
		// (set) Token: 0x060049E5 RID: 18917 RVA: 0x0012C481 File Offset: 0x0012B481
		[UrlProperty]
		[WebCategory("Appearance")]
		[WebSysDescription("Menu_StaticPopoutImageUrl")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
		public string StaticPopOutImageUrl
		{
			get
			{
				object obj = this.ViewState["StaticPopOutImageUrl"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["StaticPopOutImageUrl"] = value;
			}
		}

		// Token: 0x17001278 RID: 4728
		// (get) Token: 0x060049E6 RID: 18918 RVA: 0x0012C494 File Offset: 0x0012B494
		// (set) Token: 0x060049E7 RID: 18919 RVA: 0x0012C4C6 File Offset: 0x0012B4C6
		[WebCategory("Appearance")]
		[WebSysDefaultValue("MenuAdapter_Expand")]
		[WebSysDescription("Menu_StaticPopoutImageText")]
		public string StaticPopOutImageTextFormatString
		{
			get
			{
				object obj = this.ViewState["StaticPopOutImageTextFormatString"];
				if (obj == null)
				{
					return SR.GetString("MenuAdapter_Expand");
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["StaticPopOutImageTextFormatString"] = value;
			}
		}

		// Token: 0x17001279 RID: 4729
		// (get) Token: 0x060049E8 RID: 18920 RVA: 0x0012C4D9 File Offset: 0x0012B4D9
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("Menu_StaticSelectedStyle")]
		[WebCategory("Styles")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public MenuItemStyle StaticSelectedStyle
		{
			get
			{
				if (this._staticSelectedStyle == null)
				{
					this._staticSelectedStyle = new MenuItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._staticSelectedStyle).TrackViewState();
					}
				}
				return this._staticSelectedStyle;
			}
		}

		// Token: 0x1700127A RID: 4730
		// (get) Token: 0x060049E9 RID: 18921 RVA: 0x0012C508 File Offset: 0x0012B508
		// (set) Token: 0x060049EA RID: 18922 RVA: 0x0012C537 File Offset: 0x0012B537
		[WebSysDescription("Menu_StaticSubMenuIndent")]
		[DefaultValue(typeof(Unit), "16px")]
		[Themeable(true)]
		[WebCategory("Appearance")]
		public Unit StaticSubMenuIndent
		{
			get
			{
				object obj = this.ViewState["StaticSubMenuIndent"];
				if (obj == null)
				{
					return Unit.Pixel(16);
				}
				return (Unit)obj;
			}
			set
			{
				if (value.Value < 0.0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["StaticSubMenuIndent"] = value;
			}
		}

		// Token: 0x1700127B RID: 4731
		// (get) Token: 0x060049EB RID: 18923 RVA: 0x0012C56C File Offset: 0x0012B56C
		// (set) Token: 0x060049EC RID: 18924 RVA: 0x0012C574 File Offset: 0x0012B574
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(MenuItemTemplateContainer))]
		[WebSysDescription("Menu_StaticTemplate")]
		[DefaultValue(null)]
		[Browsable(false)]
		public ITemplate StaticItemTemplate
		{
			get
			{
				return this._staticTemplate;
			}
			set
			{
				this._staticTemplate = value;
			}
		}

		// Token: 0x1700127C RID: 4732
		// (get) Token: 0x060049ED RID: 18925 RVA: 0x0012C580 File Offset: 0x0012B580
		// (set) Token: 0x060049EE RID: 18926 RVA: 0x0012C5AD File Offset: 0x0012B5AD
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		[WebSysDescription("Menu_StaticTopSeparatorImageUrl")]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		public string StaticTopSeparatorImageUrl
		{
			get
			{
				object obj = this.ViewState["StaticTopSeparatorImageUrl"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["StaticTopSeparatorImageUrl"] = value;
			}
		}

		// Token: 0x1700127D RID: 4733
		// (get) Token: 0x060049EF RID: 18927 RVA: 0x0012C5C0 File Offset: 0x0012B5C0
		// (set) Token: 0x060049F0 RID: 18928 RVA: 0x0012C5ED File Offset: 0x0012B5ED
		[DefaultValue("")]
		[WebSysDescription("MenuItem_Target")]
		public string Target
		{
			get
			{
				object obj = this.ViewState["Target"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["Target"] = value;
			}
		}

		// Token: 0x1700127E RID: 4734
		// (get) Token: 0x060049F1 RID: 18929 RVA: 0x0012C600 File Offset: 0x0012B600
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.Table;
			}
		}

		// Token: 0x140000D6 RID: 214
		// (add) Token: 0x060049F2 RID: 18930 RVA: 0x0012C604 File Offset: 0x0012B604
		// (remove) Token: 0x060049F3 RID: 18931 RVA: 0x0012C617 File Offset: 0x0012B617
		[WebCategory("Behavior")]
		[WebSysDescription("Menu_MenuItemClick")]
		public event MenuEventHandler MenuItemClick
		{
			add
			{
				base.Events.AddHandler(Menu._menuItemClickedEvent, value);
			}
			remove
			{
				base.Events.RemoveHandler(Menu._menuItemClickedEvent, value);
			}
		}

		// Token: 0x140000D7 RID: 215
		// (add) Token: 0x060049F4 RID: 18932 RVA: 0x0012C62A File Offset: 0x0012B62A
		// (remove) Token: 0x060049F5 RID: 18933 RVA: 0x0012C63D File Offset: 0x0012B63D
		[WebSysDescription("Menu_MenuItemDataBound")]
		[WebCategory("Behavior")]
		public event MenuEventHandler MenuItemDataBound
		{
			add
			{
				base.Events.AddHandler(Menu._menuItemDataBoundEvent, value);
			}
			remove
			{
				base.Events.RemoveHandler(Menu._menuItemDataBoundEvent, value);
			}
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x0012C650 File Offset: 0x0012B650
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			string accessKey = this.AccessKey;
			if (accessKey.Length != 0)
			{
				try
				{
					this.AccessKey = string.Empty;
					base.AddAttributesToRender(writer);
					return;
				}
				finally
				{
					this.AccessKey = accessKey;
				}
			}
			base.AddAttributesToRender(writer);
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x0012C6B4 File Offset: 0x0012B6B4
		private static bool AppendCssClassName(StringBuilder builder, MenuItemStyle style, bool hyperlink)
		{
			bool flag = false;
			if (style != null)
			{
				if (style.CssClass.Length != 0)
				{
					builder.Append(style.CssClass);
					builder.Append(' ');
					flag = true;
				}
				string text = (hyperlink ? style.HyperLinkStyle.RegisteredCssClass : style.RegisteredCssClass);
				if (text.Length > 0)
				{
					builder.Append(text);
					builder.Append(' ');
				}
			}
			return flag;
		}

		// Token: 0x060049F8 RID: 18936 RVA: 0x0012C720 File Offset: 0x0012B720
		private static void AppendMenuCssClassName(StringBuilder builder, SubMenuStyle style)
		{
			if (style != null)
			{
				if (style.CssClass.Length != 0)
				{
					builder.Append(style.CssClass);
					builder.Append(' ');
				}
				string registeredCssClass = style.RegisteredCssClass;
				if (registeredCssClass.Length > 0)
				{
					builder.Append(registeredCssClass);
					builder.Append(' ');
				}
			}
		}

		// Token: 0x060049F9 RID: 18937 RVA: 0x0012C774 File Offset: 0x0012B774
		private static T CacheGetItem<T>(List<T> cacheList, int index) where T : class
		{
			if (index < cacheList.Count)
			{
				return cacheList[index];
			}
			return default(T);
		}

		// Token: 0x060049FA RID: 18938 RVA: 0x0012C79C File Offset: 0x0012B79C
		private static void CacheSetItem<T>(List<T> cacheList, int index, T item) where T : class
		{
			if (cacheList.Count > index)
			{
				cacheList[index] = item;
				return;
			}
			for (int i = cacheList.Count; i < index; i++)
			{
				cacheList.Add(default(T));
			}
			cacheList.Add(item);
		}

		// Token: 0x060049FB RID: 18939 RVA: 0x0012C7E4 File Offset: 0x0012B7E4
		protected internal override void CreateChildControls()
		{
			this.Controls.Clear();
			if (this.StaticItemTemplate != null || this.DynamicItemTemplate != null)
			{
				if (base.RequiresDataBinding && (!string.IsNullOrEmpty(this.DataSourceID) || this.DataSource != null))
				{
					this.EnsureDataBound();
					return;
				}
				this.CreateChildControlsFromItems(false);
				base.ClearChildViewState();
			}
		}

		// Token: 0x060049FC RID: 18940 RVA: 0x0012C840 File Offset: 0x0012B840
		private void CreateChildControlsFromItems(bool dataBinding)
		{
			if (this.StaticItemTemplate != null || this.DynamicItemTemplate != null)
			{
				int num = 0;
				foreach (object obj in this.Items)
				{
					MenuItem menuItem = (MenuItem)obj;
					this.CreateTemplatedControls(this.StaticItemTemplate, menuItem, num++, 0, dataBinding);
				}
			}
		}

		// Token: 0x060049FD RID: 18941 RVA: 0x0012C8B8 File Offset: 0x0012B8B8
		internal int CreateItemIndex()
		{
			return this._nodeIndex++;
		}

		// Token: 0x060049FE RID: 18942 RVA: 0x0012C8D8 File Offset: 0x0012B8D8
		private void CreateTemplatedControls(ITemplate template, MenuItem item, int position, int depth, bool dataBinding)
		{
			if (template != null)
			{
				MenuItemTemplateContainer menuItemTemplateContainer = new MenuItemTemplateContainer(position, item);
				item.Container = menuItemTemplateContainer;
				template.InstantiateIn(menuItemTemplateContainer);
				this.Controls.Add(menuItemTemplateContainer);
				if (dataBinding)
				{
					menuItemTemplateContainer.DataBind();
				}
			}
			int num = 0;
			foreach (object obj in item.ChildItems)
			{
				MenuItem menuItem = (MenuItem)obj;
				int num2 = depth + 1;
				if (template == this.DynamicItemTemplate)
				{
					this.CreateTemplatedControls(this.DynamicItemTemplate, menuItem, num++, num2, dataBinding);
				}
				else if (num2 < this.StaticDisplayLevels)
				{
					this.CreateTemplatedControls(template, menuItem, num++, num2, dataBinding);
				}
				else if (this.DynamicItemTemplate != null)
				{
					this.CreateTemplatedControls(this.DynamicItemTemplate, menuItem, num++, num2, dataBinding);
				}
			}
		}

		// Token: 0x060049FF RID: 18943 RVA: 0x0012C9C0 File Offset: 0x0012B9C0
		public sealed override void DataBind()
		{
			base.DataBind();
		}

		// Token: 0x06004A00 RID: 18944 RVA: 0x0012C9C8 File Offset: 0x0012B9C8
		private void DataBindItem(MenuItem item)
		{
			HierarchicalDataSourceView data = this.GetData(item.DataPath);
			if (!base.IsBoundUsingDataSourceID && this.DataSource == null)
			{
				return;
			}
			if (data == null)
			{
				throw new InvalidOperationException(SR.GetString("Menu_DataSourceReturnedNullView", new object[] { this.ID }));
			}
			IHierarchicalEnumerable hierarchicalEnumerable = data.Select();
			item.ChildItems.Clear();
			if (hierarchicalEnumerable != null)
			{
				if (base.IsBoundUsingDataSourceID)
				{
					SiteMapDataSource siteMapDataSource = this.GetDataSource() as SiteMapDataSource;
					if (siteMapDataSource != null)
					{
						SiteMapNode currentNode = siteMapDataSource.Provider.CurrentNode;
						if (currentNode != null)
						{
							this._currentSiteMapNodeUrl = currentNode.Url;
						}
					}
				}
				try
				{
					this.DataBindRecursive(item, hierarchicalEnumerable);
				}
				finally
				{
					this._currentSiteMapNodeUrl = null;
				}
			}
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x0012CA84 File Offset: 0x0012BA84
		private void DataBindRecursive(MenuItem node, IHierarchicalEnumerable enumerable)
		{
			int num = node.Depth + 1;
			if (this.MaximumDynamicDisplayLevels != -1 && num >= this.MaximumDepth)
			{
				return;
			}
			foreach (object obj in enumerable)
			{
				IHierarchyData hierarchyData = enumerable.GetHierarchyData(obj);
				string text = null;
				string text2 = null;
				string text3 = string.Empty;
				string text4 = string.Empty;
				string text5 = string.Empty;
				string text6 = string.Empty;
				string text7 = string.Empty;
				bool flag = true;
				bool flag2 = false;
				bool flag3 = true;
				bool flag4 = false;
				string text8 = string.Empty;
				string text9 = string.Empty;
				text9 = hierarchyData.Type;
				MenuItemBinding binding = this.DataBindings.GetBinding(text9, num);
				if (binding != null)
				{
					PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
					string textField = binding.TextField;
					if (textField.Length > 0)
					{
						PropertyDescriptor propertyDescriptor = properties.Find(textField, true);
						if (propertyDescriptor == null)
						{
							throw new InvalidOperationException(SR.GetString("Menu_InvalidDataBinding", new object[] { textField, "TextField" }));
						}
						object value = propertyDescriptor.GetValue(obj);
						if (value != null)
						{
							if (binding.FormatString.Length > 0)
							{
								text = string.Format(CultureInfo.CurrentCulture, binding.FormatString, new object[] { value });
							}
							else
							{
								text = value.ToString();
							}
						}
					}
					if (string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(binding.Text))
					{
						text = binding.Text;
					}
					string valueField = binding.ValueField;
					if (valueField.Length > 0)
					{
						PropertyDescriptor propertyDescriptor2 = properties.Find(valueField, true);
						if (propertyDescriptor2 == null)
						{
							throw new InvalidOperationException(SR.GetString("Menu_InvalidDataBinding", new object[] { valueField, "ValueField" }));
						}
						object value2 = propertyDescriptor2.GetValue(obj);
						if (value2 != null)
						{
							text2 = value2.ToString();
						}
					}
					if (string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(binding.Value))
					{
						text2 = binding.Value;
					}
					string targetField = binding.TargetField;
					if (targetField.Length > 0)
					{
						PropertyDescriptor propertyDescriptor3 = properties.Find(targetField, true);
						if (propertyDescriptor3 == null)
						{
							throw new InvalidOperationException(SR.GetString("Menu_InvalidDataBinding", new object[] { targetField, "TargetField" }));
						}
						object value3 = propertyDescriptor3.GetValue(obj);
						if (value3 != null)
						{
							text7 = value3.ToString();
						}
					}
					if (string.IsNullOrEmpty(text7))
					{
						text7 = binding.Target;
					}
					string imageUrlField = binding.ImageUrlField;
					if (imageUrlField.Length > 0)
					{
						PropertyDescriptor propertyDescriptor4 = properties.Find(imageUrlField, true);
						if (propertyDescriptor4 == null)
						{
							throw new InvalidOperationException(SR.GetString("Menu_InvalidDataBinding", new object[] { imageUrlField, "ImageUrlField" }));
						}
						object value4 = propertyDescriptor4.GetValue(obj);
						if (value4 != null)
						{
							text4 = value4.ToString();
						}
					}
					if (string.IsNullOrEmpty(text4))
					{
						text4 = binding.ImageUrl;
					}
					string navigateUrlField = binding.NavigateUrlField;
					if (navigateUrlField.Length > 0)
					{
						PropertyDescriptor propertyDescriptor5 = properties.Find(navigateUrlField, true);
						if (propertyDescriptor5 == null)
						{
							throw new InvalidOperationException(SR.GetString("Menu_InvalidDataBinding", new object[] { navigateUrlField, "NavigateUrlField" }));
						}
						object value5 = propertyDescriptor5.GetValue(obj);
						if (value5 != null)
						{
							text3 = value5.ToString();
						}
					}
					if (string.IsNullOrEmpty(text3))
					{
						text3 = binding.NavigateUrl;
					}
					string popOutImageUrlField = binding.PopOutImageUrlField;
					if (popOutImageUrlField.Length > 0)
					{
						PropertyDescriptor propertyDescriptor6 = properties.Find(popOutImageUrlField, true);
						if (propertyDescriptor6 == null)
						{
							throw new InvalidOperationException(SR.GetString("Menu_InvalidDataBinding", new object[] { popOutImageUrlField, "PopOutImageUrlField" }));
						}
						object value6 = propertyDescriptor6.GetValue(obj);
						if (value6 != null)
						{
							text5 = value6.ToString();
						}
					}
					if (string.IsNullOrEmpty(text5))
					{
						text5 = binding.PopOutImageUrl;
					}
					string separatorImageUrlField = binding.SeparatorImageUrlField;
					if (separatorImageUrlField.Length > 0)
					{
						PropertyDescriptor propertyDescriptor7 = properties.Find(separatorImageUrlField, true);
						if (propertyDescriptor7 == null)
						{
							throw new InvalidOperationException(SR.GetString("Menu_InvalidDataBinding", new object[] { separatorImageUrlField, "SeparatorImageUrlField" }));
						}
						object value7 = propertyDescriptor7.GetValue(obj);
						if (value7 != null)
						{
							text6 = value7.ToString();
						}
					}
					if (string.IsNullOrEmpty(text6))
					{
						text6 = binding.SeparatorImageUrl;
					}
					string toolTipField = binding.ToolTipField;
					if (toolTipField.Length > 0)
					{
						PropertyDescriptor propertyDescriptor8 = properties.Find(toolTipField, true);
						if (propertyDescriptor8 == null)
						{
							throw new InvalidOperationException(SR.GetString("Menu_InvalidDataBinding", new object[] { toolTipField, "ToolTipField" }));
						}
						object value8 = propertyDescriptor8.GetValue(obj);
						if (value8 != null)
						{
							text8 = value8.ToString();
						}
					}
					if (string.IsNullOrEmpty(text8))
					{
						text8 = binding.ToolTip;
					}
					string enabledField = binding.EnabledField;
					if (enabledField.Length > 0)
					{
						PropertyDescriptor propertyDescriptor9 = properties.Find(enabledField, true);
						if (propertyDescriptor9 == null)
						{
							throw new InvalidOperationException(SR.GetString("Menu_InvalidDataBinding", new object[] { enabledField, "EnabledField" }));
						}
						object value9 = propertyDescriptor9.GetValue(obj);
						if (value9 != null)
						{
							if (value9 is bool)
							{
								flag = (bool)value9;
								flag2 = true;
							}
							else if (bool.TryParse(value9.ToString(), out flag))
							{
								flag2 = true;
							}
						}
					}
					if (!flag2)
					{
						flag = binding.Enabled;
					}
					string selectableField = binding.SelectableField;
					if (selectableField.Length > 0)
					{
						PropertyDescriptor propertyDescriptor10 = properties.Find(selectableField, true);
						if (propertyDescriptor10 == null)
						{
							throw new InvalidOperationException(SR.GetString("Menu_InvalidDataBinding", new object[] { selectableField, "SelectableField" }));
						}
						object value10 = propertyDescriptor10.GetValue(obj);
						if (value10 != null)
						{
							if (value10 is bool)
							{
								flag3 = (bool)value10;
								flag4 = true;
							}
							else if (bool.TryParse(value10.ToString(), out flag3))
							{
								flag4 = true;
							}
						}
					}
					if (!flag4)
					{
						flag3 = binding.Selectable;
					}
				}
				else if (obj is INavigateUIData)
				{
					INavigateUIData navigateUIData = (INavigateUIData)obj;
					text = navigateUIData.Name;
					text2 = navigateUIData.Value;
					text3 = navigateUIData.NavigateUrl;
					if (string.IsNullOrEmpty(text3))
					{
						flag3 = false;
					}
					text8 = navigateUIData.Description;
				}
				if (text == null)
				{
					text = obj.ToString();
				}
				MenuItem menuItem = null;
				if (text != null || text2 != null)
				{
					menuItem = new MenuItem(text, text2, text4, text3, text7);
				}
				if (menuItem != null)
				{
					if (text8.Length > 0)
					{
						menuItem.ToolTip = text8;
					}
					if (text5.Length > 0)
					{
						menuItem.PopOutImageUrl = text5;
					}
					if (text6.Length > 0)
					{
						menuItem.SeparatorImageUrl = text6;
					}
					menuItem.Enabled = flag;
					menuItem.Selectable = flag3;
					menuItem.SetDataPath(hierarchyData.Path);
					menuItem.SetDataBound(true);
					node.ChildItems.Add(menuItem);
					if (string.Equals(hierarchyData.Path, this._currentSiteMapNodeUrl, StringComparison.OrdinalIgnoreCase))
					{
						menuItem.Selected = true;
					}
					menuItem.SetDataItem(hierarchyData.Item);
					this.OnMenuItemDataBound(new MenuEventArgs(menuItem));
					menuItem.SetDataItem(null);
					if (hierarchyData.HasChildren && num < this.MaximumDepth)
					{
						IHierarchicalEnumerable children = hierarchyData.GetChildren();
						if (children != null)
						{
							this.DataBindRecursive(menuItem, children);
						}
					}
				}
			}
		}

		// Token: 0x06004A02 RID: 18946 RVA: 0x0012D1E0 File Offset: 0x0012C1E0
		protected override void EnsureDataBound()
		{
			base.EnsureDataBound();
			if (!this._subControlsDataBound)
			{
				foreach (object obj in this.Controls)
				{
					Control control = (Control)obj;
					control.DataBind();
				}
				this._subControlsDataBound = true;
			}
		}

		// Token: 0x06004A03 RID: 18947 RVA: 0x0012D250 File Offset: 0x0012C250
		internal void EnsureRenderSettings()
		{
			/*
An exception occurred when decompiling this method (06004A03)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Web.UI.WebControls.Menu::EnsureRenderSettings()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindLoops(HashSet`1 scope, ControlFlowNode entryPoint, Boolean excludeEntryPoint) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 193
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindLoops(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 57
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 343
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06004A04 RID: 18948 RVA: 0x0012D5EC File Offset: 0x0012C5EC
		public MenuItem FindItem(string valuePath)
		{
			if (valuePath == null)
			{
				return null;
			}
			return this.Items.FindItem(valuePath.Split(new char[] { this.PathSeparator }), 0);
		}

		// Token: 0x06004A05 RID: 18949 RVA: 0x0012D624 File Offset: 0x0012C624
		internal string GetCssClassName(MenuItem item, bool hyperLink)
		{
			bool flag;
			return this.GetCssClassName(item, hyperLink, out flag);
		}

		// Token: 0x06004A06 RID: 18950 RVA: 0x0012D63C File Offset: 0x0012C63C
		internal string GetCssClassName(MenuItem item, bool hyperlink, out bool containsClassName)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			containsClassName = false;
			int depth = item.Depth;
			string text = Menu.CacheGetItem<string>(hyperlink ? this.CachedMenuItemHyperLinkClassNames : this.CachedMenuItemClassNames, depth);
			if (this.CachedLevelsContainingCssClass.Contains(depth))
			{
				containsClassName = true;
			}
			if (!item.Selected && text != null)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (text != null)
			{
				if (!item.Selected)
				{
					return text;
				}
				stringBuilder.Append(text);
				stringBuilder.Append(' ');
			}
			else
			{
				if (hyperlink)
				{
					stringBuilder.Append(this.RootMenuItemStyle.RegisteredCssClass);
					stringBuilder.Append(' ');
				}
				if (depth < this.StaticDisplayLevels)
				{
					containsClassName |= Menu.AppendCssClassName(stringBuilder, this._staticItemStyle, hyperlink);
				}
				else
				{
					containsClassName |= Menu.AppendCssClassName(stringBuilder, this._dynamicItemStyle, hyperlink);
				}
				if (depth < this.LevelMenuItemStyles.Count && this.LevelMenuItemStyles[depth] != null)
				{
					containsClassName |= Menu.AppendCssClassName(stringBuilder, this.LevelMenuItemStyles[depth], hyperlink);
				}
				text = stringBuilder.ToString().Trim();
				Menu.CacheSetItem<string>(hyperlink ? this.CachedMenuItemHyperLinkClassNames : this.CachedMenuItemClassNames, depth, text);
				if (containsClassName && !this.CachedLevelsContainingCssClass.Contains(depth))
				{
					this.CachedLevelsContainingCssClass.Add(depth);
				}
			}
			if (item.Selected)
			{
				if (depth < this.StaticDisplayLevels)
				{
					containsClassName |= Menu.AppendCssClassName(stringBuilder, this._staticSelectedStyle, hyperlink);
				}
				else
				{
					containsClassName |= Menu.AppendCssClassName(stringBuilder, this._dynamicSelectedStyle, hyperlink);
				}
				if (depth < this.LevelSelectedStyles.Count && this.LevelSelectedStyles[depth] != null)
				{
					MenuItemStyle menuItemStyle = this.LevelSelectedStyles[depth];
					containsClassName |= Menu.AppendCssClassName(stringBuilder, menuItemStyle, hyperlink);
				}
				return stringBuilder.ToString().Trim();
			}
			return text;
		}

		// Token: 0x06004A07 RID: 18951 RVA: 0x0012D800 File Offset: 0x0012C800
		private MenuItem GetOneDynamicItem(MenuItem item)
		{
			if (item.Depth >= this.StaticDisplayLevels)
			{
				return item;
			}
			for (int i = 0; i < item.ChildItems.Count; i++)
			{
				MenuItem oneDynamicItem = this.GetOneDynamicItem(item.ChildItems[i]);
				if (oneDynamicItem != null)
				{
					return oneDynamicItem;
				}
			}
			return null;
		}

		// Token: 0x06004A08 RID: 18952 RVA: 0x0012D84C File Offset: 0x0012C84C
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		protected override IDictionary GetDesignModeState()
		{
			IDictionary designModeState = base.GetDesignModeState();
			this.CreateChildControls();
			foreach (object obj in this.Controls)
			{
				Control control = (Control)obj;
				control.DataBind();
			}
			using (StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture))
			{
				using (HtmlTextWriter designTimeWriter = this.GetDesignTimeWriter(stringWriter))
				{
					this.RenderBeginTag(designTimeWriter);
					this.RenderContents(designTimeWriter, true);
					this.RenderEndTag(designTimeWriter, true);
					designModeState["GetDesignTimeStaticHtml"] = stringWriter.ToString();
				}
			}
			int staticDisplayLevels = this.StaticDisplayLevels;
			try
			{
				MenuItem menuItem = this.GetOneDynamicItem(this.RootItem);
				if (menuItem == null)
				{
					this._dataBound = false;
					this.StaticDisplayLevels = 1;
					menuItem = new MenuItem();
					menuItem.SetDepth(0);
					menuItem.SetOwner(this);
					string @string = SR.GetString("Menu_DesignTimeDummyItemText");
					for (int i = 0; i < 5; i++)
					{
						MenuItem menuItem2 = new MenuItem(@string);
						if (this.DynamicItemTemplate != null)
						{
							MenuItemTemplateContainer menuItemTemplateContainer = new MenuItemTemplateContainer(i, menuItem2);
							menuItem2.Container = menuItemTemplateContainer;
							this.DynamicItemTemplate.InstantiateIn(menuItemTemplateContainer);
							menuItemTemplateContainer.Site = base.Site;
							menuItemTemplateContainer.DataBind();
						}
						menuItem.ChildItems.Add(menuItem2);
					}
					menuItem.ChildItems[1].ChildItems.Add(new MenuItem());
					this._cachedLevelsContainingCssClass = null;
					this._cachedMenuItemStyles = null;
					this._cachedSubMenuStyles = null;
					this._cachedMenuItemClassNames = null;
					this._cachedMenuItemHyperLinkClassNames = null;
					this._cachedSubMenuClassNames = null;
				}
				else
				{
					menuItem = menuItem.Parent;
				}
				using (StringWriter stringWriter2 = new StringWriter(CultureInfo.CurrentCulture))
				{
					using (HtmlTextWriter designTimeWriter2 = this.GetDesignTimeWriter(stringWriter2))
					{
						base.Attributes.AddAttributes(designTimeWriter2);
						designTimeWriter2.RenderBeginTag(HtmlTextWriterTag.Table);
						designTimeWriter2.RenderBeginTag(HtmlTextWriterTag.Tr);
						designTimeWriter2.RenderBeginTag(HtmlTextWriterTag.Td);
						menuItem.Render(designTimeWriter2, true, false, false);
						designTimeWriter2.RenderEndTag();
						designTimeWriter2.RenderEndTag();
						designTimeWriter2.RenderEndTag();
						designModeState["GetDesignTimeDynamicHtml"] = stringWriter2.ToString();
					}
				}
			}
			finally
			{
				if (this.StaticDisplayLevels != staticDisplayLevels)
				{
					this.StaticDisplayLevels = staticDisplayLevels;
				}
			}
			return designModeState;
		}

		// Token: 0x06004A09 RID: 18953 RVA: 0x0012DB38 File Offset: 0x0012CB38
		private HtmlTextWriter GetDesignTimeWriter(StringWriter stringWriter)
		{
			if (this._designTimeTextWriterType == null)
			{
				return new HtmlTextWriter(stringWriter);
			}
			ConstructorInfo constructor = this._designTimeTextWriterType.GetConstructor(new Type[] { typeof(TextWriter) });
			if (constructor == null)
			{
				return new HtmlTextWriter(stringWriter);
			}
			return (HtmlTextWriter)constructor.Invoke(new object[] { stringWriter });
		}

		// Token: 0x06004A0A RID: 18954 RVA: 0x0012DB98 File Offset: 0x0012CB98
		internal string GetImageUrl(int index)
		{
			if (this.ImageUrls[index] == null)
			{
				switch (index)
				{
				case 0:
					this.ImageUrls[index] = this.ScrollUpImageUrlInternal;
					break;
				case 1:
					this.ImageUrls[index] = this.ScrollDownImageUrlInternal;
					break;
				case 2:
					this.ImageUrls[index] = this.PopoutImageUrlInternal;
					break;
				}
				this.ImageUrls[index] = base.ResolveClientUrl(this.ImageUrls[index]);
			}
			return this.ImageUrls[index];
		}

		// Token: 0x06004A0B RID: 18955 RVA: 0x0012DC14 File Offset: 0x0012CC14
		internal MenuItemStyle GetMenuItemStyle(MenuItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			int depth = item.Depth;
			MenuItemStyle menuItemStyle = Menu.CacheGetItem<MenuItemStyle>(this.CachedMenuItemStyles, depth);
			if (!item.Selected && menuItemStyle != null)
			{
				return menuItemStyle;
			}
			if (menuItemStyle == null)
			{
				menuItemStyle = new MenuItemStyle();
				menuItemStyle.CopyFrom(this.RootMenuItemStyle);
				if (depth < this.StaticDisplayLevels)
				{
					if (this._staticItemStyle != null)
					{
						TreeView.GetMergedStyle(menuItemStyle, this._staticItemStyle);
					}
				}
				else if (depth >= this.StaticDisplayLevels && this._dynamicItemStyle != null)
				{
					TreeView.GetMergedStyle(menuItemStyle, this._dynamicItemStyle);
				}
				if (depth < this.LevelMenuItemStyles.Count && this.LevelMenuItemStyles[depth] != null)
				{
					TreeView.GetMergedStyle(menuItemStyle, this.LevelMenuItemStyles[depth]);
				}
				Menu.CacheSetItem<MenuItemStyle>(this.CachedMenuItemStyles, depth, menuItemStyle);
			}
			if (item.Selected)
			{
				MenuItemStyle menuItemStyle2 = new MenuItemStyle();
				menuItemStyle2.CopyFrom(menuItemStyle);
				if (depth < this.StaticDisplayLevels)
				{
					if (this._staticSelectedStyle != null)
					{
						TreeView.GetMergedStyle(menuItemStyle2, this._staticSelectedStyle);
					}
				}
				else if (depth >= this.StaticDisplayLevels && this._dynamicSelectedStyle != null)
				{
					TreeView.GetMergedStyle(menuItemStyle2, this._dynamicSelectedStyle);
				}
				if (depth < this.LevelSelectedStyles.Count && this.LevelSelectedStyles[depth] != null)
				{
					TreeView.GetMergedStyle(menuItemStyle2, this.LevelSelectedStyles[depth]);
				}
				return menuItemStyle2;
			}
			return menuItemStyle;
		}

		// Token: 0x06004A0C RID: 18956 RVA: 0x0012DD64 File Offset: 0x0012CD64
		internal string GetSubMenuCssClassName(MenuItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			int num = item.Depth + 1;
			string text = Menu.CacheGetItem<string>(this.CachedSubMenuClassNames, num);
			if (text != null)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (num < this.StaticDisplayLevels)
			{
				Menu.AppendMenuCssClassName(stringBuilder, this._staticMenuStyle);
			}
			else
			{
				SubMenuStyle subMenuStyle = this._panelStyle as SubMenuStyle;
				if (subMenuStyle != null)
				{
					Menu.AppendMenuCssClassName(stringBuilder, subMenuStyle);
				}
				Menu.AppendMenuCssClassName(stringBuilder, this._dynamicMenuStyle);
			}
			if (num < this.LevelSubMenuStyles.Count && this.LevelSubMenuStyles[num] != null)
			{
				SubMenuStyle subMenuStyle2 = this.LevelSubMenuStyles[num];
				Menu.AppendMenuCssClassName(stringBuilder, subMenuStyle2);
			}
			text = stringBuilder.ToString().Trim();
			Menu.CacheSetItem<string>(this.CachedSubMenuClassNames, num, text);
			return text;
		}

		// Token: 0x06004A0D RID: 18957 RVA: 0x0012DE28 File Offset: 0x0012CE28
		internal SubMenuStyle GetSubMenuStyle(MenuItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			int num = item.Depth + 1;
			SubMenuStyle subMenuStyle = Menu.CacheGetItem<SubMenuStyle>(this.CachedSubMenuStyles, num);
			if (subMenuStyle != null)
			{
				return subMenuStyle;
			}
			int staticDisplayLevels = this.StaticDisplayLevels;
			if (num >= staticDisplayLevels && !base.DesignMode)
			{
				subMenuStyle = new PopOutPanel.PopOutPanelStyle(this.Panel);
			}
			else
			{
				subMenuStyle = new SubMenuStyle();
			}
			if (num < staticDisplayLevels)
			{
				if (this._staticMenuStyle != null)
				{
					subMenuStyle.CopyFrom(this._staticMenuStyle);
				}
			}
			else if (num >= staticDisplayLevels && this._dynamicMenuStyle != null)
			{
				subMenuStyle.CopyFrom(this._dynamicMenuStyle);
			}
			if (this._levelStyles != null && this._levelStyles.Count > num && this._levelStyles[num] != null)
			{
				TreeView.GetMergedStyle(subMenuStyle, this._levelStyles[num]);
			}
			Menu.CacheSetItem<SubMenuStyle>(this.CachedSubMenuStyles, num, subMenuStyle);
			return subMenuStyle;
		}

		// Token: 0x06004A0E RID: 18958 RVA: 0x0012DEFC File Offset: 0x0012CEFC
		private void EnsureRootMenuStyle()
		{
			if (this._rootMenuItemStyle == null)
			{
				this._rootMenuItemStyle = new Style();
				this._rootMenuItemStyle.Font.CopyFrom(this.Font);
				if (!this.ForeColor.IsEmpty)
				{
					this._rootMenuItemStyle.ForeColor = this.ForeColor;
				}
				if (!base.ControlStyle.IsSet(8192))
				{
					this._rootMenuItemStyle.Font.Underline = false;
				}
			}
		}

		// Token: 0x06004A0F RID: 18959 RVA: 0x0012DF78 File Offset: 0x0012CF78
		protected internal override void LoadControlState(object savedState)
		{
			Pair pair = savedState as Pair;
			if (pair == null)
			{
				base.LoadControlState(savedState);
				return;
			}
			base.LoadControlState(pair.First);
			this._selectedItem = null;
			if (pair.Second != null)
			{
				string text = pair.Second as string;
				if (text != null)
				{
					this._selectedItem = this.Items.FindItem(text.Split(new char[] { '\\' }), 0);
				}
			}
		}

		// Token: 0x06004A10 RID: 18960 RVA: 0x0012DFE8 File Offset: 0x0012CFE8
		protected override void LoadViewState(object state)
		{
			if (state != null)
			{
				object[] array = (object[])state;
				if (array[1] != null)
				{
					((IStateManager)this.StaticMenuItemStyle).LoadViewState(array[1]);
				}
				if (array[2] != null)
				{
					((IStateManager)this.StaticSelectedStyle).LoadViewState(array[2]);
				}
				if (array[3] != null)
				{
					((IStateManager)this.StaticHoverStyle).LoadViewState(array[3]);
				}
				if (array[4] != null)
				{
					((IStateManager)this.StaticMenuStyle).LoadViewState(array[4]);
				}
				if (array[5] != null)
				{
					((IStateManager)this.DynamicMenuItemStyle).LoadViewState(array[5]);
				}
				if (array[6] != null)
				{
					((IStateManager)this.DynamicSelectedStyle).LoadViewState(array[6]);
				}
				if (array[7] != null)
				{
					((IStateManager)this.DynamicHoverStyle).LoadViewState(array[7]);
				}
				if (array[8] != null)
				{
					((IStateManager)this.DynamicMenuStyle).LoadViewState(array[8]);
				}
				if (array[9] != null)
				{
					((IStateManager)this.LevelMenuItemStyles).LoadViewState(array[9]);
				}
				if (array[10] != null)
				{
					((IStateManager)this.LevelSelectedStyles).LoadViewState(array[10]);
				}
				if (array[11] != null)
				{
					((IStateManager)this.LevelSubMenuStyles).LoadViewState(array[11]);
				}
				if (array[12] != null)
				{
					((IStateManager)this.Items).LoadViewState(array[12]);
					if (!string.IsNullOrEmpty(this.DataSourceID) || this.DataSource != null)
					{
						this._dataBound = true;
					}
				}
				if (array[0] != null)
				{
					base.LoadViewState(array[0]);
				}
			}
		}

		// Token: 0x06004A11 RID: 18961 RVA: 0x0012E118 File Offset: 0x0012D118
		protected override bool OnBubbleEvent(object source, EventArgs e)
		{
			MenuEventArgs menuEventArgs = e as MenuEventArgs;
			if (menuEventArgs != null && StringUtil.EqualsIgnoreCase(menuEventArgs.CommandName, Menu.MenuItemClickCommandName))
			{
				if (!base.IsEnabled)
				{
					return true;
				}
				this.OnMenuItemClick(menuEventArgs);
				if (this._adapter != null)
				{
					MenuAdapter menuAdapter = this._adapter as MenuAdapter;
					if (menuAdapter != null)
					{
						MenuItem item = menuEventArgs.Item;
						if (item != null && item.ChildItems.Count > 0 && item.Depth + 1 >= this.StaticDisplayLevels)
						{
							menuAdapter.SetPath(menuEventArgs.Item.InternalValuePath);
						}
					}
				}
				base.RaiseBubbleEvent(this, e);
				return true;
			}
			else
			{
				if (e is CommandEventArgs)
				{
					base.RaiseBubbleEvent(this, e);
					return true;
				}
				return false;
			}
		}

		// Token: 0x06004A12 RID: 18962 RVA: 0x0012E1BF File Offset: 0x0012D1BF
		protected override void OnDataBinding(EventArgs e)
		{
			this.EnsureChildControls();
			base.OnDataBinding(e);
		}

		// Token: 0x06004A13 RID: 18963 RVA: 0x0012E1CE File Offset: 0x0012D1CE
		protected internal override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Page.RegisterRequiresControlState(this);
		}

		// Token: 0x06004A14 RID: 18964 RVA: 0x0012E1E4 File Offset: 0x0012D1E4
		protected virtual void OnMenuItemClick(MenuEventArgs e)
		{
			this.SetSelectedItem(e.Item);
			MenuEventHandler menuEventHandler = (MenuEventHandler)base.Events[Menu._menuItemClickedEvent];
			if (menuEventHandler != null)
			{
				menuEventHandler(this, e);
			}
		}

		// Token: 0x06004A15 RID: 18965 RVA: 0x0012E220 File Offset: 0x0012D220
		protected virtual void OnMenuItemDataBound(MenuEventArgs e)
		{
			MenuEventHandler menuEventHandler = (MenuEventHandler)base.Events[Menu._menuItemDataBoundEvent];
			if (menuEventHandler != null)
			{
				menuEventHandler(this, e);
			}
		}

		// Token: 0x06004A16 RID: 18966 RVA: 0x0012E24E File Offset: 0x0012D24E
		protected internal override void OnPreRender(EventArgs e)
		{
			this.OnPreRender(e, base.IsEnabled);
		}

		// Token: 0x06004A17 RID: 18967 RVA: 0x0012E260 File Offset: 0x0012D260
		internal void OnPreRender(EventArgs e, bool registerScript)
		{
			base.OnPreRender(e);
			this.EnsureRenderSettings();
			if (this.Page != null && registerScript)
			{
				this.Page.RegisterWebFormsScript();
				this.Page.ClientScript.RegisterClientScriptResource(this, typeof(Menu), "Menu.js");
				string clientDataObjectID = this.ClientDataObjectID;
				StringBuilder stringBuilder = new StringBuilder("var ");
				stringBuilder.Append(clientDataObjectID);
				stringBuilder.Append(" = new Object();\r\n");
				stringBuilder.Append(clientDataObjectID);
				stringBuilder.Append(".disappearAfter = ");
				stringBuilder.Append(this.DisappearAfter);
				stringBuilder.Append(";\r\n");
				stringBuilder.Append(clientDataObjectID);
				stringBuilder.Append(".horizontalOffset = ");
				stringBuilder.Append(this.DynamicHorizontalOffset);
				stringBuilder.Append(";\r\n");
				stringBuilder.Append(clientDataObjectID);
				stringBuilder.Append(".verticalOffset = ");
				stringBuilder.Append(this.DynamicVerticalOffset);
				stringBuilder.Append(";\r\n");
				if (this._dynamicHoverStyle != null)
				{
					stringBuilder.Append(clientDataObjectID);
					stringBuilder.Append(".hoverClass = '");
					stringBuilder.Append(this._dynamicHoverStyle.RegisteredCssClass);
					if (!string.IsNullOrEmpty(this._dynamicHoverStyle.CssClass))
					{
						if (!string.IsNullOrEmpty(this._dynamicHoverStyle.RegisteredCssClass))
						{
							stringBuilder.Append(' ');
						}
						stringBuilder.Append(this._dynamicHoverStyle.CssClass);
					}
					stringBuilder.Append("';\r\n");
					if (this._dynamicHoverHyperLinkStyle != null)
					{
						stringBuilder.Append(clientDataObjectID);
						stringBuilder.Append(".hoverHyperLinkClass = '");
						stringBuilder.Append(this._dynamicHoverHyperLinkStyle.RegisteredCssClass);
						if (!string.IsNullOrEmpty(this._dynamicHoverStyle.CssClass))
						{
							if (!string.IsNullOrEmpty(this._dynamicHoverHyperLinkStyle.RegisteredCssClass))
							{
								stringBuilder.Append(' ');
							}
							stringBuilder.Append(this._dynamicHoverStyle.CssClass);
						}
						stringBuilder.Append("';\r\n");
					}
				}
				if (this._staticHoverStyle != null && this._staticHoverHyperLinkStyle != null)
				{
					stringBuilder.Append(clientDataObjectID);
					stringBuilder.Append(".staticHoverClass = '");
					stringBuilder.Append(this._staticHoverStyle.RegisteredCssClass);
					if (!string.IsNullOrEmpty(this._staticHoverStyle.CssClass))
					{
						if (!string.IsNullOrEmpty(this._staticHoverStyle.RegisteredCssClass))
						{
							stringBuilder.Append(' ');
						}
						stringBuilder.Append(this._staticHoverStyle.CssClass);
					}
					stringBuilder.Append("';\r\n");
					if (this._staticHoverHyperLinkStyle != null)
					{
						stringBuilder.Append(clientDataObjectID);
						stringBuilder.Append(".staticHoverHyperLinkClass = '");
						stringBuilder.Append(this._staticHoverHyperLinkStyle.RegisteredCssClass);
						if (!string.IsNullOrEmpty(this._staticHoverStyle.CssClass))
						{
							if (!string.IsNullOrEmpty(this._staticHoverHyperLinkStyle.RegisteredCssClass))
							{
								stringBuilder.Append(' ');
							}
							stringBuilder.Append(this._staticHoverStyle.CssClass);
						}
						stringBuilder.Append("';\r\n");
					}
				}
				if (this.Page.RequestInternal != null && string.Equals(this.Page.Request.Url.Scheme, "https", StringComparison.OrdinalIgnoreCase))
				{
					stringBuilder.Append(clientDataObjectID);
					stringBuilder.Append(".iframeUrl = '");
					stringBuilder.Append(Util.QuoteJScriptString(this.Page.ClientScript.GetWebResourceUrl(typeof(Menu), "SmartNav.htm"), false));
					stringBuilder.Append("';\r\n");
				}
				this.Page.ClientScript.RegisterStartupScript(this, base.GetType(), this.ClientID + "_CreateDataObject", stringBuilder.ToString(), true);
			}
		}

		// Token: 0x06004A18 RID: 18968 RVA: 0x0012E604 File Offset: 0x0012D604
		protected internal override void PerformDataBinding()
		{
			base.PerformDataBinding();
			this.DataBindItem(this.RootItem);
			if (!base.DesignMode && this._dataBound && string.IsNullOrEmpty(this.DataSourceID) && this.DataSource == null)
			{
				this.Items.Clear();
				this.Controls.Clear();
				base.ClearChildViewState();
				this.TrackViewState();
				base.ChildControlsCreated = true;
				return;
			}
			if (!string.IsNullOrEmpty(this.DataSourceID) || this.DataSource != null)
			{
				this.Controls.Clear();
				base.ClearChildState();
				this.TrackViewState();
				this.CreateChildControlsFromItems(true);
				base.ChildControlsCreated = true;
				this._dataBound = true;
			}
			else if (!this._subControlsDataBound)
			{
				foreach (object obj in this.Controls)
				{
					Control control = (Control)obj;
					control.DataBind();
				}
			}
			this._subControlsDataBound = true;
		}

		// Token: 0x06004A19 RID: 18969 RVA: 0x0012E710 File Offset: 0x0012D710
		private void RegisterStyle(Style style)
		{
			if (this.Page != null && this.Page.SupportsStyleSheets)
			{
				string text = this.ClientID + "_" + this._cssStyleIndex++.ToString(NumberFormatInfo.InvariantInfo);
				this.Page.Header.StyleSheet.CreateStyleRule(style, this, "." + text);
				style.SetRegisteredCssClass(text);
			}
		}

		// Token: 0x06004A1A RID: 18970 RVA: 0x0012E78A File Offset: 0x0012D78A
		protected internal override void Render(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			if (this.Items.Count > 0)
			{
				this.RenderBeginTag(writer);
				this.RenderContents(writer, false);
				this.RenderEndTag(writer, false);
			}
		}

		// Token: 0x06004A1B RID: 18971 RVA: 0x0012E7C8 File Offset: 0x0012D7C8
		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			if (this.SkipLinkText.Length != 0 && !base.DesignMode)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Href, '#' + this.ClientID + "_SkipLink");
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				writer.AddAttribute(HtmlTextWriterAttribute.Alt, this.SkipLinkText);
				writer.AddAttribute(HtmlTextWriterAttribute.Src, base.SpacerImageUrl);
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "0px");
				writer.AddAttribute(HtmlTextWriterAttribute.Width, "0");
				writer.AddAttribute(HtmlTextWriterAttribute.Height, "0");
				writer.RenderBeginTag(HtmlTextWriterTag.Img);
				writer.RenderEndTag();
				writer.RenderEndTag();
			}
			this.EnsureRootMenuStyle();
			if (this.Font != null)
			{
				this.Font.Reset();
			}
			this.ForeColor = Color.Empty;
			SubMenuStyle subMenuStyle = this.GetSubMenuStyle(this.RootItem);
			if (this.Page != null && this.Page.SupportsStyleSheets)
			{
				string subMenuCssClassName = this.GetSubMenuCssClassName(this.RootItem);
				if (subMenuCssClassName.Length > 0)
				{
					if (this.CssClass.Length == 0)
					{
						this.CssClass = subMenuCssClassName;
					}
					else
					{
						this.CssClass = this.CssClass + ' ' + subMenuCssClassName;
					}
				}
			}
			else if (subMenuStyle != null && !subMenuStyle.IsEmpty)
			{
				subMenuStyle.Font.Reset();
				subMenuStyle.ForeColor = Color.Empty;
				base.ControlStyle.CopyFrom(subMenuStyle);
			}
			this.AddAttributesToRender(writer);
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
		}

		// Token: 0x06004A1C RID: 18972 RVA: 0x0012E958 File Offset: 0x0012D958
		private void RenderContents(HtmlTextWriter writer, bool staticOnly)
		{
			if (this.Orientation == Orientation.Horizontal)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			}
			bool isEnabled = base.IsEnabled;
			if (this.StaticDisplayLevels > 1)
			{
				if (this.Orientation == Orientation.Vertical)
				{
					for (int i = 0; i < this.Items.Count; i++)
					{
						this.Items[i].RenderItem(writer, i, isEnabled, this.Orientation, staticOnly);
						if (this.Items[i].ChildItems.Count != 0)
						{
							writer.RenderBeginTag(HtmlTextWriterTag.Tr);
							writer.RenderBeginTag(HtmlTextWriterTag.Td);
							this.Items[i].Render(writer, isEnabled, staticOnly);
							writer.RenderEndTag();
							writer.RenderEndTag();
						}
					}
				}
				else
				{
					for (int j = 0; j < this.Items.Count; j++)
					{
						this.Items[j].RenderItem(writer, j, isEnabled, this.Orientation, staticOnly);
						if (this.Items[j].ChildItems.Count != 0)
						{
							writer.RenderBeginTag(HtmlTextWriterTag.Td);
							this.Items[j].Render(writer, isEnabled, staticOnly);
							writer.RenderEndTag();
						}
					}
				}
			}
			else
			{
				for (int k = 0; k < this.Items.Count; k++)
				{
					this.Items[k].RenderItem(writer, k, isEnabled, this.Orientation, staticOnly);
				}
			}
			if (this.Orientation == Orientation.Horizontal)
			{
				writer.RenderEndTag();
			}
			if (base.DesignMode)
			{
				if (this._dynamicItemStyle != null)
				{
					this._dynamicItemStyle.ResetCachedStyles();
				}
				if (this._staticItemStyle != null)
				{
					this._staticItemStyle.ResetCachedStyles();
				}
				if (this._dynamicSelectedStyle != null)
				{
					this._dynamicSelectedStyle.ResetCachedStyles();
				}
				if (this._staticSelectedStyle != null)
				{
					this._staticSelectedStyle.ResetCachedStyles();
				}
				if (this._staticHoverStyle != null)
				{
					this._staticHoverHyperLinkStyle = new HyperLinkStyle(this._staticHoverStyle);
				}
				if (this._dynamicHoverStyle != null)
				{
					this._dynamicHoverHyperLinkStyle = new HyperLinkStyle(this._dynamicHoverStyle);
				}
				foreach (object obj in this.LevelMenuItemStyles)
				{
					MenuItemStyle menuItemStyle = (MenuItemStyle)obj;
					menuItemStyle.ResetCachedStyles();
				}
				foreach (object obj2 in this.LevelSelectedStyles)
				{
					MenuItemStyle menuItemStyle2 = (MenuItemStyle)obj2;
					menuItemStyle2.ResetCachedStyles();
				}
				if (this._imageUrls != null)
				{
					for (int l = 0; l < this._imageUrls.Length; l++)
					{
						this._imageUrls[l] = null;
					}
				}
				this._cachedPopOutImageUrl = null;
				this._cachedScrollDownImageUrl = null;
				this._cachedScrollUpImageUrl = null;
				this._cachedLevelsContainingCssClass = null;
				this._cachedMenuItemClassNames = null;
				this._cachedMenuItemHyperLinkClassNames = null;
				this._cachedMenuItemStyles = null;
				this._cachedSubMenuClassNames = null;
				this._cachedSubMenuStyles = null;
			}
		}

		// Token: 0x06004A1D RID: 18973 RVA: 0x0012EC50 File Offset: 0x0012DC50
		protected internal override void RenderContents(HtmlTextWriter writer)
		{
			this.RenderContents(writer, false);
		}

		// Token: 0x06004A1E RID: 18974 RVA: 0x0012EC5C File Offset: 0x0012DC5C
		private void RenderEndTag(HtmlTextWriter writer, bool staticOnly)
		{
			writer.RenderEndTag();
			if (this.StaticDisplayLevels <= 1 && !staticOnly)
			{
				bool isEnabled = base.IsEnabled;
				for (int i = 0; i < this.Items.Count; i++)
				{
					this.Items[i].Render(writer, isEnabled, staticOnly);
				}
			}
			if (this.SkipLinkText.Length != 0 && !base.DesignMode)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_SkipLink");
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				writer.RenderEndTag();
			}
		}

		// Token: 0x06004A1F RID: 18975 RVA: 0x0012ECE6 File Offset: 0x0012DCE6
		public override void RenderEndTag(HtmlTextWriter writer)
		{
			this.RenderEndTag(writer, false);
		}

		// Token: 0x06004A20 RID: 18976 RVA: 0x0012ECF0 File Offset: 0x0012DCF0
		protected internal override object SaveControlState()
		{
			object obj = base.SaveControlState();
			if (this._selectedItem != null)
			{
				return new Pair(obj, this._selectedItem.InternalValuePath);
			}
			return obj;
		}

		// Token: 0x06004A21 RID: 18977 RVA: 0x0012ED20 File Offset: 0x0012DD20
		protected override object SaveViewState()
		{
			object[] array = new object[13];
			array[0] = base.SaveViewState();
			bool flag = array[0] != null;
			if (this._staticItemStyle != null)
			{
				array[1] = ((IStateManager)this._staticItemStyle).SaveViewState();
				flag |= array[1] != null;
			}
			if (this._staticSelectedStyle != null)
			{
				array[2] = ((IStateManager)this._staticSelectedStyle).SaveViewState();
				flag |= array[2] != null;
			}
			if (this._staticHoverStyle != null)
			{
				array[3] = ((IStateManager)this._staticHoverStyle).SaveViewState();
				flag |= array[3] != null;
			}
			if (this._staticMenuStyle != null)
			{
				array[4] = ((IStateManager)this._staticMenuStyle).SaveViewState();
				flag |= array[4] != null;
			}
			if (this._dynamicItemStyle != null)
			{
				array[5] = ((IStateManager)this._dynamicItemStyle).SaveViewState();
				flag |= array[5] != null;
			}
			if (this._dynamicSelectedStyle != null)
			{
				array[6] = ((IStateManager)this._dynamicSelectedStyle).SaveViewState();
				flag |= array[6] != null;
			}
			if (this._dynamicHoverStyle != null)
			{
				array[7] = ((IStateManager)this._dynamicHoverStyle).SaveViewState();
				flag |= array[7] != null;
			}
			if (this._dynamicMenuStyle != null)
			{
				array[8] = ((IStateManager)this._dynamicMenuStyle).SaveViewState();
				flag |= array[8] != null;
			}
			if (this._levelMenuItemStyles != null)
			{
				array[9] = ((IStateManager)this._levelMenuItemStyles).SaveViewState();
				flag |= array[9] != null;
			}
			if (this._levelSelectedStyles != null)
			{
				array[10] = ((IStateManager)this._levelSelectedStyles).SaveViewState();
				flag |= array[10] != null;
			}
			if (this._levelStyles != null)
			{
				array[11] = ((IStateManager)this._levelStyles).SaveViewState();
				flag |= array[11] != null;
			}
			array[12] = ((IStateManager)this.Items).SaveViewState();
			flag |= array[12] != null;
			if (flag)
			{
				return array;
			}
			return null;
		}
