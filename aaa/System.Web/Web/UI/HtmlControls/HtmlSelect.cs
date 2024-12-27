using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x020004A9 RID: 1193
	[DefaultEvent("ServerChange")]
	[ValidationProperty("Value")]
	[ControlBuilder(typeof(HtmlSelectBuilder))]
	[SupportsEventValidation]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HtmlSelect : HtmlContainerControl, IPostBackDataHandler, IParserAccessor
	{
		// Token: 0x060037A5 RID: 14245 RVA: 0x000EE4E6 File Offset: 0x000ED4E6
		public HtmlSelect()
			: base("select")
		{
			this.cachedSelectedIndex = -1;
		}

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x060037A6 RID: 14246 RVA: 0x000EE4FC File Offset: 0x000ED4FC
		// (set) Token: 0x060037A7 RID: 14247 RVA: 0x000EE529 File Offset: 0x000ED529
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue("")]
		[WebCategory("Data")]
		[WebSysDescription("HtmlSelect_DataMember")]
		public virtual string DataMember
		{
			get
			{
				object obj = this.ViewState["DataMember"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				base.Attributes["DataMember"] = HtmlControl.MapStringAttributeToString(value);
				this.OnDataPropertyChanged();
			}
		}

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x060037A8 RID: 14248 RVA: 0x000EE547 File Offset: 0x000ED547
		// (set) Token: 0x060037A9 RID: 14249 RVA: 0x000EE550 File Offset: 0x000ED550
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(null)]
		[WebSysDescription("BaseDataBoundControl_DataSource")]
		[WebCategory("Data")]
		public virtual object DataSource
		{
			get
			{
				return this.dataSource;
			}
			set
			{
				if (value == null || value is IListSource || value is IEnumerable)
				{
					this.dataSource = value;
					this.OnDataPropertyChanged();
					return;
				}
				throw new ArgumentException(SR.GetString("Invalid_DataSource_Type", new object[] { this.ID }));
			}
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x060037AA RID: 14250 RVA: 0x000EE5A0 File Offset: 0x000ED5A0
		// (set) Token: 0x060037AB RID: 14251 RVA: 0x000EE5CD File Offset: 0x000ED5CD
		[WebSysDescription("BaseDataBoundControl_DataSourceID")]
		[DefaultValue("")]
		[WebCategory("Data")]
		public virtual string DataSourceID
		{
			get
			{
				object obj = this.ViewState["DataSourceID"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["DataSourceID"] = value;
				this.OnDataPropertyChanged();
			}
		}

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x060037AC RID: 14252 RVA: 0x000EE5E8 File Offset: 0x000ED5E8
		// (set) Token: 0x060037AD RID: 14253 RVA: 0x000EE610 File Offset: 0x000ED610
		[WebSysDescription("HtmlSelect_DataTextField")]
		[DefaultValue("")]
		[WebCategory("Data")]
		public virtual string DataTextField
		{
			get
			{
				string text = base.Attributes["DataTextField"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				base.Attributes["DataTextField"] = HtmlControl.MapStringAttributeToString(value);
				if (this._inited)
				{
					this.RequiresDataBinding = true;
				}
			}
		}

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x060037AE RID: 14254 RVA: 0x000EE638 File Offset: 0x000ED638
		// (set) Token: 0x060037AF RID: 14255 RVA: 0x000EE660 File Offset: 0x000ED660
		[WebCategory("Data")]
		[DefaultValue("")]
		[WebSysDescription("HtmlSelect_DataValueField")]
		public virtual string DataValueField
		{
			get
			{
				string text = base.Attributes["DataValueField"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				base.Attributes["DataValueField"] = HtmlControl.MapStringAttributeToString(value);
				if (this._inited)
				{
					this.RequiresDataBinding = true;
				}
			}
		}

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x060037B0 RID: 14256 RVA: 0x000EE688 File Offset: 0x000ED688
		// (set) Token: 0x060037B1 RID: 14257 RVA: 0x000EE6BC File Offset: 0x000ED6BC
		public override string InnerHtml
		{
			get
			{
				throw new NotSupportedException(SR.GetString("InnerHtml_not_supported", new object[] { base.GetType().Name }));
			}
			set
			{
				throw new NotSupportedException(SR.GetString("InnerHtml_not_supported", new object[] { base.GetType().Name }));
			}
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x060037B2 RID: 14258 RVA: 0x000EE6F0 File Offset: 0x000ED6F0
		// (set) Token: 0x060037B3 RID: 14259 RVA: 0x000EE724 File Offset: 0x000ED724
		public override string InnerText
		{
			get
			{
				throw new NotSupportedException(SR.GetString("InnerText_not_supported", new object[] { base.GetType().Name }));
			}
			set
			{
				throw new NotSupportedException(SR.GetString("InnerText_not_supported", new object[] { base.GetType().Name }));
			}
		}

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x060037B4 RID: 14260 RVA: 0x000EE756 File Offset: 0x000ED756
		protected bool IsBoundUsingDataSourceID
		{
			get
			{
				return this.DataSourceID.Length > 0;
			}
		}

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x060037B5 RID: 14261 RVA: 0x000EE766 File Offset: 0x000ED766
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ListItemCollection Items
		{
			get
			{
				if (this.items == null)
				{
					this.items = new ListItemCollection();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.items).TrackViewState();
					}
				}
				return this.items;
			}
		}

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x060037B6 RID: 14262 RVA: 0x000EE794 File Offset: 0x000ED794
		// (set) Token: 0x060037B7 RID: 14263 RVA: 0x000EE7C2 File Offset: 0x000ED7C2
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Behavior")]
		[DefaultValue("")]
		public bool Multiple
		{
			get
			{
				string text = base.Attributes["multiple"];
				return text != null && text.Equals("multiple");
			}
			set
			{
				if (value)
				{
					base.Attributes["multiple"] = "multiple";
					return;
				}
				base.Attributes["multiple"] = null;
			}
		}

		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x060037B8 RID: 14264 RVA: 0x000EE7EE File Offset: 0x000ED7EE
		// (set) Token: 0x060037B9 RID: 14265 RVA: 0x000EE7F6 File Offset: 0x000ED7F6
		[WebCategory("Behavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue("")]
		public string Name
		{
			get
			{
				return this.UniqueID;
			}
			set
			{
			}
		}

		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x060037BA RID: 14266 RVA: 0x000EE7F8 File Offset: 0x000ED7F8
		internal string RenderedNameAttribute
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x060037BB RID: 14267 RVA: 0x000EE800 File Offset: 0x000ED800
		// (set) Token: 0x060037BC RID: 14268 RVA: 0x000EE808 File Offset: 0x000ED808
		protected bool RequiresDataBinding
		{
			get
			{
				return this._requiresDataBinding;
			}
			set
			{
				this._requiresDataBinding = value;
			}
		}

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x060037BD RID: 14269 RVA: 0x000EE814 File Offset: 0x000ED814
		// (set) Token: 0x060037BE RID: 14270 RVA: 0x000EE880 File Offset: 0x000ED880
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[HtmlControlPersistable(false)]
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
				if (this.Size <= 1 && !this.Multiple)
				{
					if (this.Items.Count > 0)
					{
						this.Items[0].Selected = true;
					}
					return 0;
				}
				return -1;
			}
			set
			{
				if (this.Items.Count == 0)
				{
					this.cachedSelectedIndex = value;
					return;
				}
				if (value < -1 || value >= this.Items.Count)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ClearSelection();
				if (value >= 0)
				{
					this.Items[value].Selected = true;
				}
			}
		}

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x060037BF RID: 14271 RVA: 0x000EE8DC File Offset: 0x000ED8DC
		protected virtual int[] SelectedIndices
		{
			get
			{
				int num = 0;
				int[] array = new int[3];
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (this.Items[i].Selected)
					{
						if (num == array.Length)
						{
							int[] array2 = new int[num + num];
							array.CopyTo(array2, 0);
							array = array2;
						}
						array[num++] = i;
					}
				}
				int[] array3 = new int[num];
				Array.Copy(array, 0, array3, 0, num);
				return array3;
			}
		}

		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x060037C0 RID: 14272 RVA: 0x000EE954 File Offset: 0x000ED954
		// (set) Token: 0x060037C1 RID: 14273 RVA: 0x000EE982 File Offset: 0x000ED982
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Size
		{
			get
			{
				string text = base.Attributes["size"];
				if (text == null)
				{
					return -1;
				}
				return int.Parse(text, CultureInfo.InvariantCulture);
			}
			set
			{
				base.Attributes["size"] = HtmlControl.MapIntegerAttributeToString(value);
			}
		}

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x060037C2 RID: 14274 RVA: 0x000EE99C File Offset: 0x000ED99C
		// (set) Token: 0x060037C3 RID: 14275 RVA: 0x000EE9DC File Offset: 0x000ED9DC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Value
		{
			get
			{
				int selectedIndex = this.SelectedIndex;
				if (selectedIndex >= 0 && selectedIndex < this.Items.Count)
				{
					return this.Items[selectedIndex].Value;
				}
				return string.Empty;
			}
			set
			{
				int num = this.Items.FindByValueInternal(value, true);
				if (num >= 0)
				{
					this.SelectedIndex = num;
				}
			}
		}

		// Token: 0x1400004E RID: 78
		// (add) Token: 0x060037C4 RID: 14276 RVA: 0x000EEA02 File Offset: 0x000EDA02
		// (remove) Token: 0x060037C5 RID: 14277 RVA: 0x000EEA15 File Offset: 0x000EDA15
		[WebSysDescription("HtmlSelect_OnServerChange")]
		[WebCategory("Action")]
		public event EventHandler ServerChange
		{
			add
			{
				base.Events.AddHandler(HtmlSelect.EventServerChange, value);
			}
			remove
			{
				base.Events.RemoveHandler(HtmlSelect.EventServerChange, value);
			}
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x000EEA28 File Offset: 0x000EDA28
		protected override void AddParsedSubObject(object obj)
		{
			if (obj is ListItem)
			{
				this.Items.Add((ListItem)obj);
				return;
			}
			throw new HttpException(SR.GetString("Cannot_Have_Children_Of_Type", new object[]
			{
				"HtmlSelect",
				obj.GetType().Name
			}));
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x000EEA7C File Offset: 0x000EDA7C
		protected virtual void ClearSelection()
		{
			for (int i = 0; i < this.Items.Count; i++)
			{
				this.Items[i].Selected = false;
			}
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x000EEAB4 File Offset: 0x000EDAB4
		private DataSourceView ConnectToDataSourceView()
		{
			if (this._currentViewValid && !base.DesignMode)
			{
				return this._currentView;
			}
			if (this._currentView != null && this._currentViewIsFromDataSourceID)
			{
				this._currentView.DataSourceViewChanged -= this.OnDataSourceViewChanged;
			}
			IDataSource dataSource = null;
			string dataSourceID = this.DataSourceID;
			if (dataSourceID.Length != 0)
			{
				Control control = DataBoundControlHelper.FindControl(this, dataSourceID);
				if (control == null)
				{
					throw new HttpException(SR.GetString("DataControl_DataSourceDoesntExist", new object[] { this.ID, dataSourceID }));
				}
				dataSource = control as IDataSource;
				if (dataSource == null)
				{
					throw new HttpException(SR.GetString("DataControl_DataSourceIDMustBeDataControl", new object[] { this.ID, dataSourceID }));
				}
			}
			if (dataSource == null)
			{
				dataSource = new ReadOnlyDataSource(this.DataSource, this.DataMember);
			}
			else if (this.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataControl_MultipleDataSources", new object[] { this.ID }));
			}
			DataSourceView view = dataSource.GetView(this.DataMember);
			if (view == null)
			{
				throw new InvalidOperationException(SR.GetString("DataControl_ViewNotFound", new object[] { this.ID }));
			}
			this._currentViewIsFromDataSourceID = this.IsBoundUsingDataSourceID;
			this._currentView = view;
			if (this._currentView != null && this._currentViewIsFromDataSourceID)
			{
				this._currentView.DataSourceViewChanged += this.OnDataSourceViewChanged;
			}
			this._currentViewValid = true;
			return this._currentView;
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x000EEC37 File Offset: 0x000EDC37
		protected override ControlCollection CreateControlCollection()
		{
			return new EmptyControlCollection(this);
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x000EEC40 File Offset: 0x000EDC40
		protected void EnsureDataBound()
		{
			try
			{
				this._throwOnDataPropertyChange = true;
				if (this.RequiresDataBinding && this.DataSourceID.Length > 0)
				{
					this.DataBind();
				}
			}
			finally
			{
				this._throwOnDataPropertyChange = false;
			}
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x000EEC8C File Offset: 0x000EDC8C
		protected virtual IEnumerable GetData()
		{
			DataSourceView dataSourceView = this.ConnectToDataSourceView();
			if (dataSourceView != null)
			{
				return dataSourceView.ExecuteSelect(DataSourceSelectArguments.Empty);
			}
			return null;
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x000EECB0 File Offset: 0x000EDCB0
		protected override void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				Triplet triplet = (Triplet)savedState;
				base.LoadViewState(triplet.First);
				((IStateManager)this.Items).LoadViewState(triplet.Second);
				object third = triplet.Third;
				if (third != null)
				{
					this.Select((int[])third);
				}
			}
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x000EECFC File Offset: 0x000EDCFC
		protected override void OnDataBinding(EventArgs e)
		{
			base.OnDataBinding(e);
			IEnumerable data = this.GetData();
			if (data != null)
			{
				bool flag = false;
				string dataTextField = this.DataTextField;
				string dataValueField = this.DataValueField;
				this.Items.Clear();
				ICollection collection = data as ICollection;
				if (collection != null)
				{
					this.Items.Capacity = collection.Count;
				}
				if (dataTextField.Length != 0 || dataValueField.Length != 0)
				{
					flag = true;
				}
				foreach (object obj in data)
				{
					ListItem listItem = new ListItem();
					if (flag)
					{
						if (dataTextField.Length > 0)
						{
							listItem.Text = DataBinder.GetPropertyValue(obj, dataTextField, null);
						}
						if (dataValueField.Length > 0)
						{
							listItem.Value = DataBinder.GetPropertyValue(obj, dataValueField, null);
						}
					}
					else
					{
						listItem.Text = (listItem.Value = obj.ToString());
					}
					this.Items.Add(listItem);
				}
			}
			if (this.cachedSelectedIndex != -1)
			{
				this.SelectedIndex = this.cachedSelectedIndex;
				this.cachedSelectedIndex = -1;
			}
			this.ViewState["_!DataBound"] = true;
			this.RequiresDataBinding = false;
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x000EEE4C File Offset: 0x000EDE4C
		protected virtual void OnDataPropertyChanged()
		{
			if (this._throwOnDataPropertyChange)
			{
				throw new HttpException(SR.GetString("DataBoundControl_InvalidDataPropertyChange", new object[] { this.ID }));
			}
			if (this._inited)
			{
				this.RequiresDataBinding = true;
			}
			this._currentViewValid = false;
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x000EEE98 File Offset: 0x000EDE98
		protected virtual void OnDataSourceViewChanged(object sender, EventArgs e)
		{
			this.RequiresDataBinding = true;
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x000EEEA4 File Offset: 0x000EDEA4
		protected internal override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			if (this.Page != null)
			{
				this.Page.PreLoad += this.OnPagePreLoad;
				if (!base.IsViewStateEnabled && this.Page.IsPostBack)
				{
					this.RequiresDataBinding = true;
				}
			}
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x000EEEF4 File Offset: 0x000EDEF4
		protected internal override void OnLoad(EventArgs e)
		{
			this._inited = true;
			this.ConnectToDataSourceView();
			if (this.Page != null && !this._pagePreLoadFired && this.ViewState["_!DataBound"] == null)
			{
				if (!this.Page.IsPostBack)
				{
					this.RequiresDataBinding = true;
				}
				else if (base.IsViewStateEnabled)
				{
					this.RequiresDataBinding = true;
				}
			}
			base.OnLoad(e);
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x000EEF60 File Offset: 0x000EDF60
		private void OnPagePreLoad(object sender, EventArgs e)
		{
			this._inited = true;
			if (this.Page != null)
			{
				this.Page.PreLoad -= this.OnPagePreLoad;
				if (!this.Page.IsPostBack)
				{
					this.RequiresDataBinding = true;
				}
				if (this.Page.IsPostBack && base.IsViewStateEnabled && this.ViewState["_!DataBound"] == null)
				{
					this.RequiresDataBinding = true;
				}
			}
			this._pagePreLoadFired = true;
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x000EEFDC File Offset: 0x000EDFDC
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (this.Page != null && !base.Disabled)
			{
				if (this.Size > 1)
				{
					this.Page.RegisterRequiresPostBack(this);
				}
				this.Page.RegisterEnabledControl(this);
			}
			this.EnsureDataBound();
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x000EF01C File Offset: 0x000EE01C
		protected virtual void OnServerChange(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[HtmlSelect.EventServerChange];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060037D5 RID: 14293 RVA: 0x000EF04C File Offset: 0x000EE04C
		protected override void RenderAttributes(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.ClientScript.RegisterForEventValidation(this.RenderedNameAttribute);
			}
			writer.WriteAttribute("name", this.RenderedNameAttribute);
			base.Attributes.Remove("name");
			base.Attributes.Remove("DataValueField");
			base.Attributes.Remove("DataTextField");
			base.Attributes.Remove("DataMember");
			base.Attributes.Remove("DataSourceID");
			base.RenderAttributes(writer);
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x000EF0E0 File Offset: 0x000EE0E0
		protected internal override void RenderChildren(HtmlTextWriter writer)
		{
			bool flag = false;
			bool flag2 = !this.Multiple;
			writer.WriteLine();
			writer.Indent++;
			ListItemCollection listItemCollection = this.Items;
			int count = listItemCollection.Count;
			if (count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					ListItem listItem = listItemCollection[i];
					writer.WriteBeginTag("option");
					if (listItem.Selected)
					{
						if (flag2)
						{
							if (flag)
							{
								throw new HttpException(SR.GetString("HtmlSelect_Cant_Multiselect_In_Single_Mode"));
							}
							flag = true;
						}
						writer.WriteAttribute("selected", "selected");
					}
					writer.WriteAttribute("value", listItem.Value, true);
					listItem.Attributes.Remove("text");
					listItem.Attributes.Remove("value");
					listItem.Attributes.Remove("selected");
					listItem.Attributes.Render(writer);
					writer.Write('>');
					HttpUtility.HtmlEncode(listItem.Text, writer);
					writer.WriteEndTag("option");
					writer.WriteLine();
				}
			}
			writer.Indent--;
		}

		// Token: 0x060037D7 RID: 14295 RVA: 0x000EF208 File Offset: 0x000EE208
		protected override object SaveViewState()
		{
			object obj = base.SaveViewState();
			object obj2 = ((IStateManager)this.Items).SaveViewState();
			object obj3 = null;
			if (base.Events[HtmlSelect.EventServerChange] != null || base.Disabled || !this.Visible)
			{
				obj3 = this.SelectedIndices;
			}
			if (obj3 != null || obj2 != null || obj != null)
			{
				return new Triplet(obj, obj2, obj3);
			}
			return null;
		}

		// Token: 0x060037D8 RID: 14296 RVA: 0x000EF268 File Offset: 0x000EE268
		protected virtual void Select(int[] selectedIndices)
		{
			this.ClearSelection();
			foreach (int num in selectedIndices)
			{
				if (num >= 0 && num < this.Items.Count)
				{
					this.Items[num].Selected = true;
				}
			}
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x000EF2B1 File Offset: 0x000EE2B1
		protected override void TrackViewState()
		{
			base.TrackViewState();
			((IStateManager)this.Items).TrackViewState();
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x000EF2C4 File Offset: 0x000EE2C4
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			return this.LoadPostData(postDataKey, postCollection);
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x000EF2D0 File Offset: 0x000EE2D0
		protected virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			string[] values = postCollection.GetValues(postDataKey);
			bool flag = false;
			if (values != null)
			{
				if (!this.Multiple)
				{
					int num = this.Items.FindByValueInternal(values[0], false);
					if (this.SelectedIndex != num)
					{
						this.SelectedIndex = num;
						flag = true;
					}
				}
				else
				{
					int num2 = values.Length;
					int[] selectedIndices = this.SelectedIndices;
					int[] array = new int[num2];
					for (int i = 0; i < num2; i++)
					{
						array[i] = this.Items.FindByValueInternal(values[i], false);
					}
					if (selectedIndices.Length == num2)
					{
						for (int j = 0; j < num2; j++)
						{
							if (array[j] != selectedIndices[j])
							{
								flag = true;
								break;
							}
						}
					}
					else
					{
						flag = true;
					}
					if (flag)
					{
						this.Select(array);
					}
				}
			}
			else if (this.SelectedIndex != -1)
			{
				this.SelectedIndex = -1;
				flag = true;
			}
			if (flag)
			{
				base.ValidateEvent(postDataKey);
			}
			return flag;
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x000EF3AD File Offset: 0x000EE3AD
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			this.RaisePostDataChangedEvent();
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x000EF3B5 File Offset: 0x000EE3B5
		protected virtual void RaisePostDataChangedEvent()
		{
			this.OnServerChange(EventArgs.Empty);
		}

		// Token: 0x040025CC RID: 9676
		internal const string DataBoundViewStateKey = "_!DataBound";

		// Token: 0x040025CD RID: 9677
		private static readonly object EventServerChange = new object();

		// Token: 0x040025CE RID: 9678
		private object dataSource;

		// Token: 0x040025CF RID: 9679
		private ListItemCollection items;

		// Token: 0x040025D0 RID: 9680
		private int cachedSelectedIndex;

		// Token: 0x040025D1 RID: 9681
		private bool _requiresDataBinding;

		// Token: 0x040025D2 RID: 9682
		private bool _inited;

		// Token: 0x040025D3 RID: 9683
		private bool _throwOnDataPropertyChange;

		// Token: 0x040025D4 RID: 9684
		private DataSourceView _currentView;

		// Token: 0x040025D5 RID: 9685
		private bool _currentViewIsFromDataSourceID;

		// Token: 0x040025D6 RID: 9686
		private bool _currentViewValid;

		// Token: 0x040025D7 RID: 9687
		private bool _pagePreLoadFired;
	}
}
