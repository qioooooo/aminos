using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004C3 RID: 1219
	[DefaultProperty("HeaderText")]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class DataControlField : IStateManager, IDataSourceViewSchemaAccessor
	{
		// Token: 0x14000064 RID: 100
		// (add) Token: 0x060039E3 RID: 14819 RVA: 0x000F4D99 File Offset: 0x000F3D99
		// (remove) Token: 0x060039E4 RID: 14820 RVA: 0x000F4DB2 File Offset: 0x000F3DB2
		internal event EventHandler FieldChanged;

		// Token: 0x060039E5 RID: 14821 RVA: 0x000F4DCB File Offset: 0x000F3DCB
		protected DataControlField()
		{
			this._statebag = new StateBag();
			this._dataSourceViewSchema = null;
		}

		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x060039E6 RID: 14822 RVA: 0x000F4DE8 File Offset: 0x000F3DE8
		// (set) Token: 0x060039E7 RID: 14823 RVA: 0x000F4E15 File Offset: 0x000F3E15
		[WebCategory("Accessibility")]
		[Localizable(true)]
		[WebSysDescription("DataControlField_AccessibleHeaderText")]
		[DefaultValue("")]
		public virtual string AccessibleHeaderText
		{
			get
			{
				object obj = this.ViewState["AccessibleHeaderText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (!object.Equals(value, this.ViewState["AccessibleHeaderText"]))
				{
					this.ViewState["AccessibleHeaderText"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x060039E8 RID: 14824 RVA: 0x000F4E46 File Offset: 0x000F3E46
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[WebSysDescription("DataControlField_ControlStyle")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Style ControlStyle
		{
			get
			{
				if (this._controlStyle == null)
				{
					this._controlStyle = new Style();
					if (this.IsTrackingViewState)
					{
						((IStateManager)this._controlStyle).TrackViewState();
					}
				}
				return this._controlStyle;
			}
		}

		// Token: 0x17000D1B RID: 3355
		// (get) Token: 0x060039E9 RID: 14825 RVA: 0x000F4E74 File Offset: 0x000F3E74
		internal Style ControlStyleInternal
		{
			get
			{
				return this._controlStyle;
			}
		}

		// Token: 0x17000D1C RID: 3356
		// (get) Token: 0x060039EA RID: 14826 RVA: 0x000F4E7C File Offset: 0x000F3E7C
		protected Control Control
		{
			get
			{
				return this._control;
			}
		}

		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x060039EB RID: 14827 RVA: 0x000F4E84 File Offset: 0x000F3E84
		protected bool DesignMode
		{
			get
			{
				return this._control != null && this._control.DesignMode;
			}
		}

		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x060039EC RID: 14828 RVA: 0x000F4E9B File Offset: 0x000F3E9B
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[WebSysDescription("DataControlField_FooterStyle")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TableItemStyle FooterStyle
		{
			get
			{
				if (this._footerStyle == null)
				{
					this._footerStyle = new TableItemStyle();
					if (this.IsTrackingViewState)
					{
						((IStateManager)this._footerStyle).TrackViewState();
					}
				}
				return this._footerStyle;
			}
		}

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x060039ED RID: 14829 RVA: 0x000F4EC9 File Offset: 0x000F3EC9
		internal TableItemStyle FooterStyleInternal
		{
			get
			{
				return this._footerStyle;
			}
		}

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x060039EE RID: 14830 RVA: 0x000F4ED4 File Offset: 0x000F3ED4
		// (set) Token: 0x060039EF RID: 14831 RVA: 0x000F4F01 File Offset: 0x000F3F01
		[Localizable(true)]
		[DefaultValue("")]
		[WebSysDescription("DataControlField_FooterText")]
		[WebCategory("Appearance")]
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
				if (!object.Equals(value, this.ViewState["FooterText"]))
				{
					this.ViewState["FooterText"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x060039F0 RID: 14832 RVA: 0x000F4F34 File Offset: 0x000F3F34
		// (set) Token: 0x060039F1 RID: 14833 RVA: 0x000F4F61 File Offset: 0x000F3F61
		[WebSysDescription("DataControlField_HeaderImageUrl")]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		[UrlProperty]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
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
				if (!object.Equals(value, this.ViewState["HeaderImageUrl"]))
				{
					this.ViewState["HeaderImageUrl"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x060039F2 RID: 14834 RVA: 0x000F4F92 File Offset: 0x000F3F92
		[WebSysDescription("DataControlField_HeaderStyle")]
		[DefaultValue(null)]
		[WebCategory("Styles")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public TableItemStyle HeaderStyle
		{
			get
			{
				if (this._headerStyle == null)
				{
					this._headerStyle = new TableItemStyle();
					if (this.IsTrackingViewState)
					{
						((IStateManager)this._headerStyle).TrackViewState();
					}
				}
				return this._headerStyle;
			}
		}

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x060039F3 RID: 14835 RVA: 0x000F4FC0 File Offset: 0x000F3FC0
		internal TableItemStyle HeaderStyleInternal
		{
			get
			{
				return this._headerStyle;
			}
		}

		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x060039F4 RID: 14836 RVA: 0x000F4FC8 File Offset: 0x000F3FC8
		// (set) Token: 0x060039F5 RID: 14837 RVA: 0x000F4FF5 File Offset: 0x000F3FF5
		[Localizable(true)]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		[WebSysDescription("DataControlField_HeaderText")]
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
				if (!object.Equals(value, this.ViewState["HeaderText"]))
				{
					this.ViewState["HeaderText"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x060039F6 RID: 14838 RVA: 0x000F5028 File Offset: 0x000F4028
		// (set) Token: 0x060039F7 RID: 14839 RVA: 0x000F5054 File Offset: 0x000F4054
		[DefaultValue(true)]
		[WebSysDescription("DataControlField_InsertVisible")]
		[WebCategory("Behavior")]
		public virtual bool InsertVisible
		{
			get
			{
				object obj = this.ViewState["InsertVisible"];
				return obj == null || (bool)obj;
			}
			set
			{
				object obj = this.ViewState["InsertVisible"];
				if (obj == null || value != (bool)obj)
				{
					this.ViewState["InsertVisible"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x060039F8 RID: 14840 RVA: 0x000F509A File Offset: 0x000F409A
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[WebSysDescription("DataControlField_ItemStyle")]
		public TableItemStyle ItemStyle
		{
			get
			{
				if (this._itemStyle == null)
				{
					this._itemStyle = new TableItemStyle();
					if (this.IsTrackingViewState)
					{
						((IStateManager)this._itemStyle).TrackViewState();
					}
				}
				return this._itemStyle;
			}
		}

		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x060039F9 RID: 14841 RVA: 0x000F50C8 File Offset: 0x000F40C8
		internal TableItemStyle ItemStyleInternal
		{
			get
			{
				return this._itemStyle;
			}
		}

		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x060039FA RID: 14842 RVA: 0x000F50D0 File Offset: 0x000F40D0
		// (set) Token: 0x060039FB RID: 14843 RVA: 0x000F50FC File Offset: 0x000F40FC
		[WebSysDescription("DataControlField_ShowHeader")]
		[WebCategory("Behavior")]
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
				object obj = this.ViewState["ShowHeader"];
				if (obj == null || (bool)obj != value)
				{
					this.ViewState["ShowHeader"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x060039FC RID: 14844 RVA: 0x000F5144 File Offset: 0x000F4144
		// (set) Token: 0x060039FD RID: 14845 RVA: 0x000F5171 File Offset: 0x000F4171
		[DefaultValue("")]
		[WebCategory("Behavior")]
		[TypeConverter("System.Web.UI.Design.DataSourceViewSchemaConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[WebSysDescription("DataControlField_SortExpression")]
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
				if (!object.Equals(value, this.ViewState["SortExpression"]))
				{
					this.ViewState["SortExpression"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x060039FE RID: 14846 RVA: 0x000F51A2 File Offset: 0x000F41A2
		protected StateBag ViewState
		{
			get
			{
				return this._statebag;
			}
		}

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x060039FF RID: 14847 RVA: 0x000F51AC File Offset: 0x000F41AC
		// (set) Token: 0x06003A00 RID: 14848 RVA: 0x000F51D8 File Offset: 0x000F41D8
		[DefaultValue(true)]
		[WebSysDescription("DataControlField_Visible")]
		[WebCategory("Behavior")]
		public bool Visible
		{
			get
			{
				object obj = this.ViewState["Visible"];
				return obj == null || (bool)obj;
			}
			set
			{
				object obj = this.ViewState["Visible"];
				if (obj == null || value != (bool)obj)
				{
					this.ViewState["Visible"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x000F5220 File Offset: 0x000F4220
		protected internal DataControlField CloneField()
		{
			DataControlField dataControlField = this.CreateField();
			this.CopyProperties(dataControlField);
			return dataControlField;
		}

		// Token: 0x06003A02 RID: 14850 RVA: 0x000F523C File Offset: 0x000F423C
		protected virtual void CopyProperties(DataControlField newField)
		{
			newField.AccessibleHeaderText = this.AccessibleHeaderText;
			newField.ControlStyle.CopyFrom(this.ControlStyle);
			newField.FooterStyle.CopyFrom(this.FooterStyle);
			newField.HeaderStyle.CopyFrom(this.HeaderStyle);
			newField.ItemStyle.CopyFrom(this.ItemStyle);
			newField.FooterText = this.FooterText;
			newField.HeaderImageUrl = this.HeaderImageUrl;
			newField.HeaderText = this.HeaderText;
			newField.InsertVisible = this.InsertVisible;
			newField.ShowHeader = this.ShowHeader;
			newField.SortExpression = this.SortExpression;
			newField.Visible = this.Visible;
		}

		// Token: 0x06003A03 RID: 14851
		protected abstract DataControlField CreateField();

		// Token: 0x06003A04 RID: 14852 RVA: 0x000F52ED File Offset: 0x000F42ED
		public virtual void ExtractValuesFromCell(IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly)
		{
		}

		// Token: 0x06003A05 RID: 14853 RVA: 0x000F52EF File Offset: 0x000F42EF
		public virtual bool Initialize(bool sortingEnabled, Control control)
		{
			this._sortingEnabled = sortingEnabled;
			this._control = control;
			return false;
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x000F5300 File Offset: 0x000F4300
		public virtual void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
		{
			switch (cellType)
			{
			case DataControlCellType.Header:
			{
				WebControl webControl = null;
				string sortExpression = this.SortExpression;
				bool flag = this._sortingEnabled && sortExpression.Length > 0;
				string headerImageUrl = this.HeaderImageUrl;
				string text = this.HeaderText;
				if (headerImageUrl.Length != 0)
				{
					if (flag)
					{
						IPostBackContainer postBackContainer = this._control as IPostBackContainer;
						ImageButton imageButton;
						if (postBackContainer != null)
						{
							imageButton = new DataControlImageButton(postBackContainer);
							((DataControlImageButton)imageButton).EnableCallback(null);
						}
						else
						{
							imageButton = new ImageButton();
						}
						imageButton.ImageUrl = this.HeaderImageUrl;
						imageButton.CommandName = "Sort";
						imageButton.CommandArgument = sortExpression;
						if (!(imageButton is DataControlImageButton))
						{
							imageButton.CausesValidation = false;
						}
						imageButton.AlternateText = text;
						webControl = imageButton;
					}
					else
					{
						Image image = new Image();
						image.ImageUrl = headerImageUrl;
						webControl = image;
						image.AlternateText = text;
					}
				}
				else if (flag)
				{
					IPostBackContainer postBackContainer2 = this._control as IPostBackContainer;
					LinkButton linkButton;
					if (postBackContainer2 != null)
					{
						linkButton = new DataControlLinkButton(postBackContainer2);
						((DataControlLinkButton)linkButton).EnableCallback(null);
					}
					else
					{
						linkButton = new LinkButton();
					}
					linkButton.Text = text;
					linkButton.CommandName = "Sort";
					linkButton.CommandArgument = sortExpression;
					if (!(linkButton is DataControlLinkButton))
					{
						linkButton.CausesValidation = false;
					}
					webControl = linkButton;
				}
				else
				{
					if (text.Length == 0)
					{
						text = "&nbsp;";
					}
					cell.Text = text;
				}
				if (webControl != null)
				{
					cell.Controls.Add(webControl);
					return;
				}
				break;
			}
			case DataControlCellType.Footer:
			{
				string text2 = this.FooterText;
				if (text2.Length == 0)
				{
					text2 = "&nbsp;";
				}
				cell.Text = text2;
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x06003A07 RID: 14855 RVA: 0x000F5499 File Offset: 0x000F4499
		protected bool IsTrackingViewState
		{
			get
			{
				return this._trackViewState;
			}
		}

		// Token: 0x06003A08 RID: 14856 RVA: 0x000F54A4 File Offset: 0x000F44A4
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

		// Token: 0x06003A09 RID: 14857 RVA: 0x000F5507 File Offset: 0x000F4507
		protected virtual void OnFieldChanged()
		{
			if (this.FieldChanged != null)
			{
				this.FieldChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x06003A0A RID: 14858 RVA: 0x000F5524 File Offset: 0x000F4524
		protected virtual object SaveViewState()
		{
			object obj = ((IStateManager)this.ViewState).SaveViewState();
			object obj2 = ((this._itemStyle != null) ? ((IStateManager)this._itemStyle).SaveViewState() : null);
			object obj3 = ((this._headerStyle != null) ? ((IStateManager)this._headerStyle).SaveViewState() : null);
			object obj4 = ((this._footerStyle != null) ? ((IStateManager)this._footerStyle).SaveViewState() : null);
			object obj5 = ((this._controlStyle != null) ? ((IStateManager)this._controlStyle).SaveViewState() : null);
			if (obj != null || obj2 != null || obj3 != null || obj4 != null || obj5 != null)
			{
				return new object[] { obj, obj2, obj3, obj4, obj5 };
			}
			return null;
		}

		// Token: 0x06003A0B RID: 14859 RVA: 0x000F55D0 File Offset: 0x000F45D0
		internal void SetDirty()
		{
			this._statebag.SetDirty(true);
			if (this._itemStyle != null)
			{
				this._itemStyle.SetDirty();
			}
			if (this._headerStyle != null)
			{
				this._headerStyle.SetDirty();
			}
			if (this._footerStyle != null)
			{
				this._footerStyle.SetDirty();
			}
			if (this._controlStyle != null)
			{
				this._controlStyle.SetDirty();
			}
		}

		// Token: 0x06003A0C RID: 14860 RVA: 0x000F5638 File Offset: 0x000F4638
		public override string ToString()
		{
			string text = this.HeaderText.Trim();
			if (text.Length <= 0)
			{
				return base.GetType().Name;
			}
			return text;
		}

		// Token: 0x06003A0D RID: 14861 RVA: 0x000F5668 File Offset: 0x000F4668
		protected virtual void TrackViewState()
		{
			this._trackViewState = true;
			((IStateManager)this.ViewState).TrackViewState();
			if (this._itemStyle != null)
			{
				((IStateManager)this._itemStyle).TrackViewState();
			}
			if (this._headerStyle != null)
			{
				((IStateManager)this._headerStyle).TrackViewState();
			}
			if (this._footerStyle != null)
			{
				((IStateManager)this._footerStyle).TrackViewState();
			}
			if (this._controlStyle != null)
			{
				((IStateManager)this._controlStyle).TrackViewState();
			}
		}

		// Token: 0x06003A0E RID: 14862 RVA: 0x000F56D4 File Offset: 0x000F46D4
		public virtual void ValidateSupportsCallback()
		{
			throw new NotSupportedException(SR.GetString("DataControlField_CallbacksNotSupported", new object[] { this.Control.ID }));
		}

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x06003A0F RID: 14863 RVA: 0x000F5706 File Offset: 0x000F4706
		bool IStateManager.IsTrackingViewState
		{
			get
			{
				return this.IsTrackingViewState;
			}
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x000F570E File Offset: 0x000F470E
		void IStateManager.LoadViewState(object state)
		{
			this.LoadViewState(state);
		}

		// Token: 0x06003A11 RID: 14865 RVA: 0x000F5717 File Offset: 0x000F4717
		void IStateManager.TrackViewState()
		{
			this.TrackViewState();
		}

		// Token: 0x06003A12 RID: 14866 RVA: 0x000F571F File Offset: 0x000F471F
		object IStateManager.SaveViewState()
		{
			return this.SaveViewState();
		}

		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x06003A13 RID: 14867 RVA: 0x000F5727 File Offset: 0x000F4727
		// (set) Token: 0x06003A14 RID: 14868 RVA: 0x000F572F File Offset: 0x000F472F
		object IDataSourceViewSchemaAccessor.DataSourceViewSchema
		{
			get
			{
				return this._dataSourceViewSchema;
			}
			set
			{
				this._dataSourceViewSchema = value;
			}
		}

		// Token: 0x0400266B RID: 9835
		private TableItemStyle _itemStyle;

		// Token: 0x0400266C RID: 9836
		private TableItemStyle _headerStyle;

		// Token: 0x0400266D RID: 9837
		private TableItemStyle _footerStyle;

		// Token: 0x0400266E RID: 9838
		private Style _controlStyle;

		// Token: 0x0400266F RID: 9839
		private StateBag _statebag;

		// Token: 0x04002670 RID: 9840
		private bool _trackViewState;

		// Token: 0x04002671 RID: 9841
		private bool _sortingEnabled;

		// Token: 0x04002672 RID: 9842
		private Control _control;

		// Token: 0x04002673 RID: 9843
		private object _dataSourceViewSchema;
	}
}
