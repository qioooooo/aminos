using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004CF RID: 1231
	[DataBindingHandler("System.Web.UI.Design.WebControls.ListControlDataBindingHandler, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("SelectedIndexChanged")]
	[Designer("System.Web.UI.Design.WebControls.ListControlDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ControlValueProperty("SelectedValue")]
	[ParseChildren(true, "Items")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class ListControl : DataBoundControl, IEditableTextControl, ITextControl
	{
		// Token: 0x06003B0A RID: 15114 RVA: 0x000F8D35 File Offset: 0x000F7D35
		public ListControl()
		{
			this.cachedSelectedIndex = -1;
		}

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x06003B0B RID: 15115 RVA: 0x000F8D44 File Offset: 0x000F7D44
		// (set) Token: 0x06003B0C RID: 15116 RVA: 0x000F8D6D File Offset: 0x000F7D6D
		[WebCategory("Behavior")]
		[DefaultValue(false)]
		[Themeable(false)]
		[WebSysDescription("ListControl_AppendDataBoundItems")]
		public virtual bool AppendDataBoundItems
		{
			get
			{
				object obj = this.ViewState["AppendDataBoundItems"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["AppendDataBoundItems"] = value;
				if (base.Initialized)
				{
					base.RequiresDataBinding = true;
				}
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x06003B0D RID: 15117 RVA: 0x000F8D94 File Offset: 0x000F7D94
		// (set) Token: 0x06003B0E RID: 15118 RVA: 0x000F8DBD File Offset: 0x000F7DBD
		[WebSysDescription("ListControl_AutoPostBack")]
		[DefaultValue(false)]
		[WebCategory("Behavior")]
		[Themeable(false)]
		public virtual bool AutoPostBack
		{
			get
			{
				object obj = this.ViewState["AutoPostBack"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["AutoPostBack"] = value;
			}
		}

		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x06003B0F RID: 15119 RVA: 0x000F8DD8 File Offset: 0x000F7DD8
		// (set) Token: 0x06003B10 RID: 15120 RVA: 0x000F8E01 File Offset: 0x000F7E01
		[Themeable(false)]
		[WebSysDescription("AutoPostBackControl_CausesValidation")]
		[DefaultValue(false)]
		[WebCategory("Behavior")]
		public virtual bool CausesValidation
		{
			get
			{
				object obj = this.ViewState["CausesValidation"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["CausesValidation"] = value;
			}
		}

		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x06003B11 RID: 15121 RVA: 0x000F8E1C File Offset: 0x000F7E1C
		// (set) Token: 0x06003B12 RID: 15122 RVA: 0x000F8E49 File Offset: 0x000F7E49
		[DefaultValue("")]
		[Themeable(false)]
		[WebSysDescription("ListControl_DataTextField")]
		[WebCategory("Data")]
		public virtual string DataTextField
		{
			get
			{
				object obj = this.ViewState["DataTextField"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["DataTextField"] = value;
				if (base.Initialized)
				{
					base.RequiresDataBinding = true;
				}
			}
		}

		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x06003B13 RID: 15123 RVA: 0x000F8E6C File Offset: 0x000F7E6C
		// (set) Token: 0x06003B14 RID: 15124 RVA: 0x000F8E99 File Offset: 0x000F7E99
		[WebSysDescription("ListControl_DataTextFormatString")]
		[WebCategory("Data")]
		[DefaultValue("")]
		[Themeable(false)]
		public virtual string DataTextFormatString
		{
			get
			{
				object obj = this.ViewState["DataTextFormatString"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["DataTextFormatString"] = value;
				if (base.Initialized)
				{
					base.RequiresDataBinding = true;
				}
			}
		}

		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x06003B15 RID: 15125 RVA: 0x000F8EBC File Offset: 0x000F7EBC
		// (set) Token: 0x06003B16 RID: 15126 RVA: 0x000F8EE9 File Offset: 0x000F7EE9
		[DefaultValue("")]
		[Themeable(false)]
		[WebCategory("Data")]
		[WebSysDescription("ListControl_DataValueField")]
		public virtual string DataValueField
		{
			get
			{
				object obj = this.ViewState["DataValueField"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["DataValueField"] = value;
				if (base.Initialized)
				{
					base.RequiresDataBinding = true;
				}
			}
		}

		// Token: 0x17000D85 RID: 3461
		// (get) Token: 0x06003B17 RID: 15127 RVA: 0x000F8F0B File Offset: 0x000F7F0B
		internal virtual bool IsMultiSelectInternal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x06003B18 RID: 15128 RVA: 0x000F8F0E File Offset: 0x000F7F0E
		[WebSysDescription("ListControl_Items")]
		[MergableProperty(false)]
		[WebCategory("Default")]
		[PersistenceMode(PersistenceMode.InnerDefaultProperty)]
		[DefaultValue(null)]
		[Editor("System.Web.UI.Design.WebControls.ListItemsCollectionEditor,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public virtual ListItemCollection Items
		{
			get
			{
				if (this.items == null)
				{
					this.items = new ListItemCollection();
					if (base.IsTrackingViewState)
					{
						this.items.TrackViewState();
					}
				}
				return this.items;
			}
		}

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x06003B19 RID: 15129 RVA: 0x000F8F3C File Offset: 0x000F7F3C
		internal bool SaveSelectedIndicesViewState
		{
			get
			{
				if (base.Events[ListControl.EventSelectedIndexChanged] != null || base.Events[ListControl.EventTextChanged] != null || !base.IsEnabled || !this.Visible || (this.AutoPostBack && this.Page != null && !this.Page.ClientSupportsJavaScript))
				{
					return true;
				}
				foreach (object obj in this.Items)
				{
					ListItem listItem = (ListItem)obj;
					if (!listItem.Enabled)
					{
						return true;
					}
				}
				Type type = base.GetType();
				return type != typeof(DropDownList) && type != typeof(ListBox) && type != typeof(CheckBoxList) && type != typeof(RadioButtonList);
			}
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x06003B1A RID: 15130 RVA: 0x000F9030 File Offset: 0x000F8030
		// (set) Token: 0x06003B1B RID: 15131 RVA: 0x000F906C File Offset: 0x000F806C
		[Browsable(false)]
		[WebSysDescription("WebControl_SelectedIndex")]
		[Bindable(true)]
		[DefaultValue(0)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Themeable(false)]
		[WebCategory("Behavior")]
		public virtual int SelectedIndex
		{
			get
			{
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (this.Items[i].Selected)
					{
						return i;
					}
				}
				return -1;
			}
			set
			{
				if (value < -1)
				{
					if (this.Items.Count != 0)
					{
						throw new ArgumentOutOfRangeException("value", SR.GetString("ListControl_SelectionOutOfRange", new object[] { this.ID, "SelectedIndex" }));
					}
					value = -1;
				}
				if ((this.Items.Count != 0 && value < this.Items.Count) || value == -1)
				{
					this.ClearSelection();
					if (value >= 0)
					{
						this.Items[value].Selected = true;
					}
				}
				else if (this._stateLoaded)
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("ListControl_SelectionOutOfRange", new object[] { this.ID, "SelectedIndex" }));
				}
				this.cachedSelectedIndex = value;
			}
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x06003B1C RID: 15132 RVA: 0x000F9138 File Offset: 0x000F8138
		internal virtual ArrayList SelectedIndicesInternal
		{
			get
			{
				this.cachedSelectedIndices = new ArrayList(3);
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (this.Items[i].Selected)
					{
						this.cachedSelectedIndices.Add(i);
					}
				}
				return this.cachedSelectedIndices;
			}
		}

		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x06003B1D RID: 15133 RVA: 0x000F9194 File Offset: 0x000F8194
		[WebSysDescription("ListControl_SelectedItem")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Behavior")]
		[Browsable(false)]
		[DefaultValue(null)]
		public virtual ListItem SelectedItem
		{
			get
			{
				int selectedIndex = this.SelectedIndex;
				if (selectedIndex >= 0)
				{
					return this.Items[selectedIndex];
				}
				return null;
			}
		}

		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x06003B1E RID: 15134 RVA: 0x000F91BC File Offset: 0x000F81BC
		// (set) Token: 0x06003B1F RID: 15135 RVA: 0x000F91EC File Offset: 0x000F81EC
		[Browsable(false)]
		[Themeable(false)]
		[WebCategory("Behavior")]
		[Bindable(true, BindingDirection.TwoWay)]
		[DefaultValue("")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebSysDescription("ListControl_SelectedValue")]
		public virtual string SelectedValue
		{
			get
			{
				int selectedIndex = this.SelectedIndex;
				if (selectedIndex >= 0)
				{
					return this.Items[selectedIndex].Value;
				}
				return string.Empty;
			}
			set
			{
				if (this.Items.Count != 0)
				{
					if (value == null || (base.DesignMode && value.Length == 0))
					{
						this.ClearSelection();
						return;
					}
					ListItem listItem = this.Items.FindByValue(value);
					bool flag = this.Page != null && this.Page.IsPostBack && this._stateLoaded;
					if (flag && listItem == null)
					{
						throw new ArgumentOutOfRangeException("value", SR.GetString("ListControl_SelectionOutOfRange", new object[] { this.ID, "SelectedValue" }));
					}
					if (listItem != null)
					{
						this.ClearSelection();
						listItem.Selected = true;
					}
				}
				this.cachedSelectedValue = value;
			}
		}

		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x06003B20 RID: 15136 RVA: 0x000F929A File Offset: 0x000F829A
		// (set) Token: 0x06003B21 RID: 15137 RVA: 0x000F92A2 File Offset: 0x000F82A2
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Behavior")]
		[Browsable(false)]
		[Themeable(false)]
		[DefaultValue("")]
		[WebSysDescription("ListControl_Text")]
		public virtual string Text
		{
			get
			{
				return this.SelectedValue;
			}
			set
			{
				this.SelectedValue = value;
			}
		}

		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x06003B22 RID: 15138 RVA: 0x000F92AB File Offset: 0x000F82AB
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.Select;
			}
		}

		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x06003B23 RID: 15139 RVA: 0x000F92B0 File Offset: 0x000F82B0
		// (set) Token: 0x06003B24 RID: 15140 RVA: 0x000F92DD File Offset: 0x000F82DD
		[WebCategory("Behavior")]
		[WebSysDescription("PostBackControl_ValidationGroup")]
		[Themeable(false)]
		[DefaultValue("")]
		public virtual string ValidationGroup
		{
			get
			{
				string text = (string)this.ViewState["ValidationGroup"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ValidationGroup"] = value;
			}
		}

		// Token: 0x14000067 RID: 103
		// (add) Token: 0x06003B25 RID: 15141 RVA: 0x000F92F0 File Offset: 0x000F82F0
		// (remove) Token: 0x06003B26 RID: 15142 RVA: 0x000F9303 File Offset: 0x000F8303
		[WebSysDescription("ListControl_OnSelectedIndexChanged")]
		[WebCategory("Action")]
		public event EventHandler SelectedIndexChanged
		{
			add
			{
				base.Events.AddHandler(ListControl.EventSelectedIndexChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListControl.EventSelectedIndexChanged, value);
			}
		}

		// Token: 0x14000068 RID: 104
		// (add) Token: 0x06003B27 RID: 15143 RVA: 0x000F9316 File Offset: 0x000F8316
		// (remove) Token: 0x06003B28 RID: 15144 RVA: 0x000F9329 File Offset: 0x000F8329
		[WebCategory("Action")]
		[WebSysDescription("ListControl_TextChanged")]
		public event EventHandler TextChanged
		{
			add
			{
				base.Events.AddHandler(ListControl.EventTextChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListControl.EventTextChanged, value);
			}
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x000F933C File Offset: 0x000F833C
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			if (this.IsMultiSelectInternal)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Multiple, "multiple");
			}
			if (this.AutoPostBack && this.Page != null && this.Page.ClientSupportsJavaScript)
			{
				string text = null;
				if (base.HasAttributes)
				{
					text = base.Attributes["onchange"];
					if (text != null)
					{
						text = Util.EnsureEndWithSemiColon(text);
						base.Attributes.Remove("onchange");
					}
				}
				PostBackOptions postBackOptions = new PostBackOptions(this, string.Empty);
				if (this.CausesValidation)
				{
					postBackOptions.PerformValidation = true;
					postBackOptions.ValidationGroup = this.ValidationGroup;
				}
				if (this.Page.Form != null)
				{
					postBackOptions.AutoPostBack = true;
				}
				text = Util.MergeScript(text, this.Page.ClientScript.GetPostBackEventReference(postBackOptions, true));
				writer.AddAttribute(HtmlTextWriterAttribute.Onchange, text);
				if (base.EnableLegacyRendering)
				{
					writer.AddAttribute("language", "javascript", false);
				}
			}
			if (this.Enabled && !base.IsEnabled)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
			}
			base.AddAttributesToRender(writer);
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x000F9468 File Offset: 0x000F8468
		public virtual void ClearSelection()
		{
			for (int i = 0; i < this.Items.Count; i++)
			{
				this.Items[i].Selected = false;
			}
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x000F94A0 File Offset: 0x000F84A0
		protected override void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				Triplet triplet = (Triplet)savedState;
				base.LoadViewState(triplet.First);
				this.Items.LoadViewState(triplet.Second);
				ArrayList arrayList = triplet.Third as ArrayList;
				if (arrayList != null)
				{
					this.SelectInternal(arrayList);
				}
			}
			else
			{
				base.LoadViewState(null);
			}
			this._stateLoaded = true;
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x000F94FC File Offset: 0x000F84FC
		protected override void OnDataBinding(EventArgs e)
		{
			base.OnDataBinding(e);
			IEnumerable enumerable = this.GetData().ExecuteSelect(DataSourceSelectArguments.Empty);
			this.PerformDataBinding(enumerable);
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x000F9528 File Offset: 0x000F8528
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (this.Page != null && base.IsEnabled)
			{
				if (this.AutoPostBack)
				{
					this.Page.RegisterPostBackScript();
					this.Page.RegisterFocusScript();
					if (this.CausesValidation && this.Page.GetValidators(this.ValidationGroup).Count > 0)
					{
						this.Page.RegisterWebFormsScript();
					}
				}
				if (!this.SaveSelectedIndicesViewState)
				{
					this.Page.RegisterEnabledControl(this);
				}
			}
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x000F95AC File Offset: 0x000F85AC
		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ListControl.EventSelectedIndexChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			this.OnTextChanged(e);
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x000F95E4 File Offset: 0x000F85E4
		protected virtual void OnTextChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ListControl.EventTextChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003B30 RID: 15152 RVA: 0x000F9614 File Offset: 0x000F8614
		protected internal override void PerformDataBinding(IEnumerable dataSource)
		{
			base.PerformDataBinding(dataSource);
			if (dataSource != null)
			{
				bool flag = false;
				bool flag2 = false;
				string dataTextField = this.DataTextField;
				string dataValueField = this.DataValueField;
				string dataTextFormatString = this.DataTextFormatString;
				if (!this.AppendDataBoundItems)
				{
					this.Items.Clear();
				}
				ICollection collection = dataSource as ICollection;
				if (collection != null)
				{
					this.Items.Capacity = collection.Count + this.Items.Count;
				}
				if (dataTextField.Length != 0 || dataValueField.Length != 0)
				{
					flag = true;
				}
				if (dataTextFormatString.Length != 0)
				{
					flag2 = true;
				}
				foreach (object obj in dataSource)
				{
					ListItem listItem = new ListItem();
					if (flag)
					{
						if (dataTextField.Length > 0)
						{
							listItem.Text = DataBinder.GetPropertyValue(obj, dataTextField, dataTextFormatString);
						}
						if (dataValueField.Length > 0)
						{
							listItem.Value = DataBinder.GetPropertyValue(obj, dataValueField, null);
						}
					}
					else
					{
						if (flag2)
						{
							listItem.Text = string.Format(CultureInfo.CurrentCulture, dataTextFormatString, new object[] { obj });
						}
						else
						{
							listItem.Text = obj.ToString();
						}
						listItem.Value = obj.ToString();
					}
					this.Items.Add(listItem);
				}
			}
			if (this.cachedSelectedValue == null)
			{
				if (this.cachedSelectedIndex != -1)
				{
					this.SelectedIndex = this.cachedSelectedIndex;
					this.cachedSelectedIndex = -1;
				}
				return;
			}
			int num = this.Items.FindByValueInternal(this.cachedSelectedValue, true);
			if (-1 == num)
			{
				throw new ArgumentOutOfRangeException("value", SR.GetString("ListControl_SelectionOutOfRange", new object[] { this.ID, "SelectedValue" }));
			}
			if (this.cachedSelectedIndex != -1 && this.cachedSelectedIndex != num)
			{
				throw new ArgumentException(SR.GetString("Attributes_mutually_exclusive", new object[] { "SelectedIndex", "SelectedValue" }));
			}
			this.SelectedIndex = num;
			this.cachedSelectedValue = null;
			this.cachedSelectedIndex = -1;
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x000F9848 File Offset: 0x000F8848
		protected override void PerformSelect()
		{
			this.OnDataBinding(EventArgs.Empty);
			base.RequiresDataBinding = false;
			base.MarkAsDataBound();
			this.OnDataBound(EventArgs.Empty);
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x000F9870 File Offset: 0x000F8870
		protected internal override void RenderContents(HtmlTextWriter writer)
		{
			ListItemCollection listItemCollection = this.Items;
			int count = listItemCollection.Count;
			if (count > 0)
			{
				bool flag = false;
				for (int i = 0; i < count; i++)
				{
					ListItem listItem = listItemCollection[i];
					if (listItem.Enabled)
					{
						writer.WriteBeginTag("option");
						if (listItem.Selected)
						{
							if (flag)
							{
								this.VerifyMultiSelect();
							}
							flag = true;
							writer.WriteAttribute("selected", "selected");
						}
						writer.WriteAttribute("value", listItem.Value, true);
						if (listItem.HasAttributes)
						{
							listItem.Attributes.Render(writer);
						}
						if (this.Page != null)
						{
							this.Page.ClientScript.RegisterForEventValidation(this.UniqueID, listItem.Value);
						}
						writer.Write('>');
						HttpUtility.HtmlEncode(listItem.Text, writer);
						writer.WriteEndTag("option");
						writer.WriteLine();
					}
				}
			}
		}

		// Token: 0x06003B33 RID: 15155 RVA: 0x000F9960 File Offset: 0x000F8960
		protected override object SaveViewState()
		{
			object obj = base.SaveViewState();
			object obj2 = this.Items.SaveViewState();
			object obj3 = null;
			if (this.SaveSelectedIndicesViewState)
			{
				obj3 = this.SelectedIndicesInternal;
			}
			if (obj3 != null || obj2 != null || obj != null)
			{
				return new Triplet(obj, obj2, obj3);
			}
			return null;
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x000F99A4 File Offset: 0x000F89A4
		internal void SelectInternal(ArrayList selectedIndices)
		{
			this.ClearSelection();
			for (int i = 0; i < selectedIndices.Count; i++)
			{
				int num = (int)selectedIndices[i];
				if (num >= 0 && num < this.Items.Count)
				{
					this.Items[num].Selected = true;
				}
			}
			this.cachedSelectedIndices = selectedIndices;
		}

		// Token: 0x06003B35 RID: 15157 RVA: 0x000F9A00 File Offset: 0x000F8A00
		protected void SetPostDataSelection(int selectedIndex)
		{
			if (this.Items.Count != 0 && selectedIndex < this.Items.Count)
			{
				this.ClearSelection();
				if (selectedIndex >= 0)
				{
					this.Items[selectedIndex].Selected = true;
				}
			}
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x000F9A39 File Offset: 0x000F8A39
		protected override void TrackViewState()
		{
			base.TrackViewState();
			this.Items.TrackViewState();
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x000F9A4C File Offset: 0x000F8A4C
		protected internal virtual void VerifyMultiSelect()
		{
			if (!this.IsMultiSelectInternal)
			{
				throw new HttpException(SR.GetString("Cant_Multiselect_In_Single_Mode"));
			}
		}

		// Token: 0x040026AE RID: 9902
		private static readonly object EventSelectedIndexChanged = new object();

		// Token: 0x040026AF RID: 9903
		private static readonly object EventTextChanged = new object();

		// Token: 0x040026B0 RID: 9904
		private ListItemCollection items;

		// Token: 0x040026B1 RID: 9905
		private int cachedSelectedIndex;

		// Token: 0x040026B2 RID: 9906
		private string cachedSelectedValue;

		// Token: 0x040026B3 RID: 9907
		private ArrayList cachedSelectedIndices;

		// Token: 0x040026B4 RID: 9908
		private bool _stateLoaded;
	}
}
