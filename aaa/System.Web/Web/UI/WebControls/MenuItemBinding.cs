using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005E4 RID: 1508
	[DefaultProperty("TextField")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class MenuItemBinding : IStateManager, ICloneable, IDataSourceViewSchemaAccessor
	{
		// Token: 0x170012A2 RID: 4770
		// (get) Token: 0x06004A85 RID: 19077 RVA: 0x00130E74 File Offset: 0x0012FE74
		// (set) Token: 0x06004A86 RID: 19078 RVA: 0x00130EA1 File Offset: 0x0012FEA1
		[WebCategory("Data")]
		[WebSysDescription("Binding_DataMember")]
		[DefaultValue("")]
		public string DataMember
		{
			get
			{
				object obj = this.ViewState["DataMember"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["DataMember"] = value;
			}
		}

		// Token: 0x170012A3 RID: 4771
		// (get) Token: 0x06004A87 RID: 19079 RVA: 0x00130EB4 File Offset: 0x0012FEB4
		// (set) Token: 0x06004A88 RID: 19080 RVA: 0x00130EDD File Offset: 0x0012FEDD
		[WebCategory("Data")]
		[WebSysDescription("MenuItemBinding_Depth")]
		[TypeConverter("System.Web.UI.Design.WebControls.TreeNodeBindingDepthConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[DefaultValue(-1)]
		public int Depth
		{
			get
			{
				object obj = this.ViewState["Depth"];
				if (obj == null)
				{
					return -1;
				}
				return (int)obj;
			}
			set
			{
				this.ViewState["Depth"] = value;
			}
		}

		// Token: 0x170012A4 RID: 4772
		// (get) Token: 0x06004A89 RID: 19081 RVA: 0x00130EF8 File Offset: 0x0012FEF8
		// (set) Token: 0x06004A8A RID: 19082 RVA: 0x00130F21 File Offset: 0x0012FF21
		[WebSysDescription("MenuItemBinding_Enabled")]
		[WebCategory("DefaultProperties")]
		[DefaultValue(true)]
		public bool Enabled
		{
			get
			{
				object obj = this.ViewState["Enabled"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["Enabled"] = value;
			}
		}

		// Token: 0x170012A5 RID: 4773
		// (get) Token: 0x06004A8B RID: 19083 RVA: 0x00130F3C File Offset: 0x0012FF3C
		// (set) Token: 0x06004A8C RID: 19084 RVA: 0x00130F69 File Offset: 0x0012FF69
		[TypeConverter("System.Web.UI.Design.DataSourceViewSchemaConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[DefaultValue("")]
		[WebCategory("Databindings")]
		[WebSysDescription("MenuItemBinding_EnabledField")]
		public string EnabledField
		{
			get
			{
				object obj = this.ViewState["EnabledField"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["EnabledField"] = value;
			}
		}

		// Token: 0x170012A6 RID: 4774
		// (get) Token: 0x06004A8D RID: 19085 RVA: 0x00130F7C File Offset: 0x0012FF7C
		// (set) Token: 0x06004A8E RID: 19086 RVA: 0x00130FA9 File Offset: 0x0012FFA9
		[WebSysDescription("MenuItemBinding_FormatString")]
		[DefaultValue("")]
		[Localizable(true)]
		[WebCategory("Databindings")]
		public string FormatString
		{
			get
			{
				object obj = this.ViewState["FormatString"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["FormatString"] = value;
			}
		}

		// Token: 0x170012A7 RID: 4775
		// (get) Token: 0x06004A8F RID: 19087 RVA: 0x00130FBC File Offset: 0x0012FFBC
		// (set) Token: 0x06004A90 RID: 19088 RVA: 0x00130FE9 File Offset: 0x0012FFE9
		[WebSysDescription("MenuItemBinding_ImageUrl")]
		[UrlProperty]
		[DefaultValue("")]
		[WebCategory("DefaultProperties")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string ImageUrl
		{
			get
			{
				object obj = this.ViewState["ImageUrl"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["ImageUrl"] = value;
			}
		}

		// Token: 0x170012A8 RID: 4776
		// (get) Token: 0x06004A91 RID: 19089 RVA: 0x00130FFC File Offset: 0x0012FFFC
		// (set) Token: 0x06004A92 RID: 19090 RVA: 0x00131029 File Offset: 0x00130029
		[TypeConverter("System.Web.UI.Design.DataSourceViewSchemaConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[WebSysDescription("MenuItemBinding_ImageUrlField")]
		[DefaultValue("")]
		[WebCategory("Databindings")]
		public string ImageUrlField
		{
			get
			{
				object obj = this.ViewState["ImageUrlField"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["ImageUrlField"] = value;
			}
		}

		// Token: 0x170012A9 RID: 4777
		// (get) Token: 0x06004A93 RID: 19091 RVA: 0x0013103C File Offset: 0x0013003C
		// (set) Token: 0x06004A94 RID: 19092 RVA: 0x00131069 File Offset: 0x00130069
		[DefaultValue("")]
		[UrlProperty]
		[WebSysDescription("MenuItemBinding_NavigateUrl")]
		[WebCategory("DefaultProperties")]
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string NavigateUrl
		{
			get
			{
				object obj = this.ViewState["NavigateUrl"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["NavigateUrl"] = value;
			}
		}

		// Token: 0x170012AA RID: 4778
		// (get) Token: 0x06004A95 RID: 19093 RVA: 0x0013107C File Offset: 0x0013007C
		// (set) Token: 0x06004A96 RID: 19094 RVA: 0x001310A9 File Offset: 0x001300A9
		[TypeConverter("System.Web.UI.Design.DataSourceViewSchemaConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[WebSysDescription("MenuItemBinding_NavigateUrlField")]
		[DefaultValue("")]
		[WebCategory("Databindings")]
		public string NavigateUrlField
		{
			get
			{
				object obj = this.ViewState["NavigateUrlField"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["NavigateUrlField"] = value;
			}
		}

		// Token: 0x170012AB RID: 4779
		// (get) Token: 0x06004A97 RID: 19095 RVA: 0x001310BC File Offset: 0x001300BC
		// (set) Token: 0x06004A98 RID: 19096 RVA: 0x001310E9 File Offset: 0x001300E9
		[WebCategory("DefaultProperties")]
		[WebSysDescription("MenuItemBinding_PopOutImageUrl")]
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		[DefaultValue("")]
		public string PopOutImageUrl
		{
			get
			{
				object obj = this.ViewState["PopOutImageUrl"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["PopOutImageUrl"] = value;
			}
		}

		// Token: 0x170012AC RID: 4780
		// (get) Token: 0x06004A99 RID: 19097 RVA: 0x001310FC File Offset: 0x001300FC
		// (set) Token: 0x06004A9A RID: 19098 RVA: 0x00131129 File Offset: 0x00130129
		[TypeConverter("System.Web.UI.Design.DataSourceViewSchemaConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[WebSysDescription("MenuItemBinding_PopOutImageUrlField")]
		[DefaultValue("")]
		[WebCategory("Databindings")]
		public string PopOutImageUrlField
		{
			get
			{
				object obj = this.ViewState["PopOutImageUrlField"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["PopOutImageUrlField"] = value;
			}
		}

		// Token: 0x170012AD RID: 4781
		// (get) Token: 0x06004A9B RID: 19099 RVA: 0x0013113C File Offset: 0x0013013C
		// (set) Token: 0x06004A9C RID: 19100 RVA: 0x00131165 File Offset: 0x00130165
		[DefaultValue(true)]
		[WebSysDescription("MenuItemBinding_Selectable")]
		[WebCategory("DefaultProperties")]
		public bool Selectable
		{
			get
			{
				object obj = this.ViewState["Selectable"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["Selectable"] = value;
			}
		}

		// Token: 0x170012AE RID: 4782
		// (get) Token: 0x06004A9D RID: 19101 RVA: 0x00131180 File Offset: 0x00130180
		// (set) Token: 0x06004A9E RID: 19102 RVA: 0x001311AD File Offset: 0x001301AD
		[DefaultValue("")]
		[TypeConverter("System.Web.UI.Design.DataSourceViewSchemaConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[WebCategory("Databindings")]
		[WebSysDescription("MenuItemBinding_SelectableField")]
		public string SelectableField
		{
			get
			{
				object obj = this.ViewState["SelectableField"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["SelectableField"] = value;
			}
		}

		// Token: 0x170012AF RID: 4783
		// (get) Token: 0x06004A9F RID: 19103 RVA: 0x001311C0 File Offset: 0x001301C0
		// (set) Token: 0x06004AA0 RID: 19104 RVA: 0x001311ED File Offset: 0x001301ED
		[UrlProperty]
		[WebSysDescription("MenuItemBinding_SeparatorImageUrl")]
		[WebCategory("DefaultProperties")]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string SeparatorImageUrl
		{
			get
			{
				object obj = this.ViewState["SeparatorImageUrl"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["SeparatorImageUrl"] = value;
			}
		}

		// Token: 0x170012B0 RID: 4784
		// (get) Token: 0x06004AA1 RID: 19105 RVA: 0x00131200 File Offset: 0x00130200
		// (set) Token: 0x06004AA2 RID: 19106 RVA: 0x0013122D File Offset: 0x0013022D
		[WebCategory("Databindings")]
		[DefaultValue("")]
		[TypeConverter("System.Web.UI.Design.DataSourceViewSchemaConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[WebSysDescription("MenuItemBinding_SeparatorImageUrlField")]
		public string SeparatorImageUrlField
		{
			get
			{
				object obj = this.ViewState["SeparatorImageUrlField"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["SeparatorImageUrlField"] = value;
			}
		}

		// Token: 0x170012B1 RID: 4785
		// (get) Token: 0x06004AA3 RID: 19107 RVA: 0x00131240 File Offset: 0x00130240
		// (set) Token: 0x06004AA4 RID: 19108 RVA: 0x0013126D File Offset: 0x0013026D
		[DefaultValue("")]
		[WebSysDescription("MenuItemBinding_Target")]
		[WebCategory("DefaultProperties")]
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

		// Token: 0x170012B2 RID: 4786
		// (get) Token: 0x06004AA5 RID: 19109 RVA: 0x00131280 File Offset: 0x00130280
		// (set) Token: 0x06004AA6 RID: 19110 RVA: 0x001312AD File Offset: 0x001302AD
		[DefaultValue("")]
		[WebCategory("Databindings")]
		[WebSysDescription("MenuItemBinding_TargetField")]
		[TypeConverter("System.Web.UI.Design.DataSourceViewSchemaConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string TargetField
		{
			get
			{
				string text = (string)this.ViewState["TargetField"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				this.ViewState["TargetField"] = value;
			}
		}

		// Token: 0x170012B3 RID: 4787
		// (get) Token: 0x06004AA7 RID: 19111 RVA: 0x001312C0 File Offset: 0x001302C0
		// (set) Token: 0x06004AA8 RID: 19112 RVA: 0x00131301 File Offset: 0x00130301
		[WebCategory("DefaultProperties")]
		[WebSysDescription("MenuItemBinding_Text")]
		[DefaultValue("")]
		[Localizable(true)]
		public string Text
		{
			get
			{
				object obj = this.ViewState["Text"];
				if (obj == null)
				{
					obj = this.ViewState["Value"];
					if (obj == null)
					{
						return string.Empty;
					}
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["Text"] = value;
			}
		}

		// Token: 0x170012B4 RID: 4788
		// (get) Token: 0x06004AA9 RID: 19113 RVA: 0x00131314 File Offset: 0x00130314
		// (set) Token: 0x06004AAA RID: 19114 RVA: 0x00131341 File Offset: 0x00130341
		[DefaultValue("")]
		[WebSysDescription("MenuItemBinding_TextField")]
		[WebCategory("Databindings")]
		[TypeConverter("System.Web.UI.Design.DataSourceViewSchemaConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string TextField
		{
			get
			{
				object obj = this.ViewState["TextField"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["TextField"] = value;
			}
		}

		// Token: 0x170012B5 RID: 4789
		// (get) Token: 0x06004AAB RID: 19115 RVA: 0x00131354 File Offset: 0x00130354
		// (set) Token: 0x06004AAC RID: 19116 RVA: 0x00131381 File Offset: 0x00130381
		[WebSysDescription("MenuItemBinding_ToolTip")]
		[DefaultValue("")]
		[Localizable(true)]
		[WebCategory("DefaultProperties")]
		public string ToolTip
		{
			get
			{
				object obj = this.ViewState["ToolTip"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["ToolTip"] = value;
			}
		}

		// Token: 0x170012B6 RID: 4790
		// (get) Token: 0x06004AAD RID: 19117 RVA: 0x00131394 File Offset: 0x00130394
		// (set) Token: 0x06004AAE RID: 19118 RVA: 0x001313C1 File Offset: 0x001303C1
		[TypeConverter("System.Web.UI.Design.DataSourceViewSchemaConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[WebCategory("Databindings")]
		[WebSysDescription("MenuItemBinding_ToolTipField")]
		[DefaultValue("")]
		public string ToolTipField
		{
			get
			{
				object obj = this.ViewState["ToolTipField"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["ToolTipField"] = value;
			}
		}

		// Token: 0x170012B7 RID: 4791
		// (get) Token: 0x06004AAF RID: 19119 RVA: 0x001313D4 File Offset: 0x001303D4
		// (set) Token: 0x06004AB0 RID: 19120 RVA: 0x00131415 File Offset: 0x00130415
		[WebCategory("DefaultProperties")]
		[Localizable(true)]
		[DefaultValue("")]
		[WebSysDescription("MenuItemBinding_Value")]
		public string Value
		{
			get
			{
				object obj = this.ViewState["Value"];
				if (obj == null)
				{
					obj = this.ViewState["Text"];
					if (obj == null)
					{
						return string.Empty;
					}
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["Value"] = value;
			}
		}

		// Token: 0x170012B8 RID: 4792
		// (get) Token: 0x06004AB1 RID: 19121 RVA: 0x00131428 File Offset: 0x00130428
		// (set) Token: 0x06004AB2 RID: 19122 RVA: 0x00131455 File Offset: 0x00130455
		[DefaultValue("")]
		[TypeConverter("System.Web.UI.Design.DataSourceViewSchemaConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[WebCategory("Databindings")]
		[WebSysDescription("MenuItemBinding_ValueField")]
		public string ValueField
		{
			get
			{
				object obj = this.ViewState["ValueField"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["ValueField"] = value;
			}
		}

		// Token: 0x170012B9 RID: 4793
		// (get) Token: 0x06004AB3 RID: 19123 RVA: 0x00131468 File Offset: 0x00130468
		private StateBag ViewState
		{
			get
			{
				if (this._viewState == null)
				{
					this._viewState = new StateBag();
					if (this._isTrackingViewState)
					{
						((IStateManager)this._viewState).TrackViewState();
					}
				}
				return this._viewState;
			}
		}

		// Token: 0x06004AB4 RID: 19124 RVA: 0x00131496 File Offset: 0x00130496
		internal void SetDirty()
		{
			this.ViewState.SetDirty(true);
		}

		// Token: 0x06004AB5 RID: 19125 RVA: 0x001314A4 File Offset: 0x001304A4
		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this.DataMember))
			{
				return this.DataMember;
			}
			return SR.GetString("TreeNodeBinding_EmptyBindingText");
		}

		// Token: 0x06004AB6 RID: 19126 RVA: 0x001314C4 File Offset: 0x001304C4
		object ICloneable.Clone()
		{
			return new MenuItemBinding
			{
				DataMember = this.DataMember,
				Depth = this.Depth,
				Enabled = this.Enabled,
				EnabledField = this.EnabledField,
				FormatString = this.FormatString,
				ImageUrl = this.ImageUrl,
				ImageUrlField = this.ImageUrlField,
				NavigateUrl = this.NavigateUrl,
				NavigateUrlField = this.NavigateUrlField,
				PopOutImageUrl = this.PopOutImageUrl,
				PopOutImageUrlField = this.PopOutImageUrlField,
				Selectable = this.Selectable,
				SelectableField = this.SelectableField,
				SeparatorImageUrl = this.SeparatorImageUrl,
				SeparatorImageUrlField = this.SeparatorImageUrlField,
				Target = this.Target,
				TargetField = this.TargetField,
				Text = this.Text,
				TextField = this.TextField,
				ToolTip = this.ToolTip,
				ToolTipField = this.ToolTipField,
				Value = this.Value,
				ValueField = this.ValueField
			};
		}

		// Token: 0x170012BA RID: 4794
		// (get) Token: 0x06004AB7 RID: 19127 RVA: 0x001315EC File Offset: 0x001305EC
		bool IStateManager.IsTrackingViewState
		{
			get
			{
				return this._isTrackingViewState;
			}
		}

		// Token: 0x06004AB8 RID: 19128 RVA: 0x001315F4 File Offset: 0x001305F4
		void IStateManager.LoadViewState(object state)
		{
			if (state != null)
			{
				((IStateManager)this.ViewState).LoadViewState(state);
			}
		}

		// Token: 0x06004AB9 RID: 19129 RVA: 0x00131605 File Offset: 0x00130605
		object IStateManager.SaveViewState()
		{
			if (this._viewState != null)
			{
				return ((IStateManager)this._viewState).SaveViewState();
			}
			return null;
		}

		// Token: 0x06004ABA RID: 19130 RVA: 0x0013161C File Offset: 0x0013061C
		void IStateManager.TrackViewState()
		{
			this._isTrackingViewState = true;
			if (this._viewState != null)
			{
				((IStateManager)this._viewState).TrackViewState();
			}
		}

		// Token: 0x170012BB RID: 4795
		// (get) Token: 0x06004ABB RID: 19131 RVA: 0x00131638 File Offset: 0x00130638
		// (set) Token: 0x06004ABC RID: 19132 RVA: 0x0013164A File Offset: 0x0013064A
		object IDataSourceViewSchemaAccessor.DataSourceViewSchema
		{
			get
			{
				return this.ViewState["IDataSourceViewSchemaAccessor.DataSourceViewSchema"];
			}
			set
			{
				this.ViewState["IDataSourceViewSchemaAccessor.DataSourceViewSchema"] = value;
			}
		}

		// Token: 0x04002B7E RID: 11134
		private bool _isTrackingViewState;

		// Token: 0x04002B7F RID: 11135
		private StateBag _viewState;
	}
}
