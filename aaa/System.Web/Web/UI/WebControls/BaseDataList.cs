using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004CA RID: 1226
	[Designer("System.Web.UI.Design.WebControls.BaseDataListDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("DataSource")]
	[DefaultEvent("SelectedIndexChanged")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class BaseDataList : WebControl
	{
		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x06003AA6 RID: 15014 RVA: 0x000F7CCC File Offset: 0x000F6CCC
		// (set) Token: 0x06003AA7 RID: 15015 RVA: 0x000F7CF9 File Offset: 0x000F6CF9
		[WebSysDescription("DataControls_Caption")]
		[WebCategory("Accessibility")]
		[DefaultValue("")]
		[Localizable(true)]
		public virtual string Caption
		{
			get
			{
				string text = (string)this.ViewState["Caption"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				this.ViewState["Caption"] = value;
			}
		}

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x06003AA8 RID: 15016 RVA: 0x000F7D0C File Offset: 0x000F6D0C
		// (set) Token: 0x06003AA9 RID: 15017 RVA: 0x000F7D35 File Offset: 0x000F6D35
		[WebCategory("Accessibility")]
		[DefaultValue(TableCaptionAlign.NotSet)]
		[WebSysDescription("WebControl_CaptionAlign")]
		public virtual TableCaptionAlign CaptionAlign
		{
			get
			{
				object obj = this.ViewState["CaptionAlign"];
				if (obj == null)
				{
					return TableCaptionAlign.NotSet;
				}
				return (TableCaptionAlign)obj;
			}
			set
			{
				if (value < TableCaptionAlign.NotSet || value > TableCaptionAlign.Right)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["CaptionAlign"] = value;
			}
		}

		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x06003AAA RID: 15018 RVA: 0x000F7D60 File Offset: 0x000F6D60
		// (set) Token: 0x06003AAB RID: 15019 RVA: 0x000F7D7C File Offset: 0x000F6D7C
		[WebCategory("Layout")]
		[WebSysDescription("BaseDataList_CellPadding")]
		[DefaultValue(-1)]
		public virtual int CellPadding
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return -1;
				}
				return ((TableStyle)base.ControlStyle).CellPadding;
			}
			set
			{
				((TableStyle)base.ControlStyle).CellPadding = value;
			}
		}

		// Token: 0x17000D5D RID: 3421
		// (get) Token: 0x06003AAC RID: 15020 RVA: 0x000F7D8F File Offset: 0x000F6D8F
		// (set) Token: 0x06003AAD RID: 15021 RVA: 0x000F7DAB File Offset: 0x000F6DAB
		[DefaultValue(0)]
		[WebSysDescription("BaseDataList_CellSpacing")]
		[WebCategory("Layout")]
		public virtual int CellSpacing
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return 0;
				}
				return ((TableStyle)base.ControlStyle).CellSpacing;
			}
			set
			{
				((TableStyle)base.ControlStyle).CellSpacing = value;
			}
		}

		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x06003AAE RID: 15022 RVA: 0x000F7DBE File Offset: 0x000F6DBE
		public override ControlCollection Controls
		{
			get
			{
				this.EnsureChildControls();
				return base.Controls;
			}
		}

		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x06003AAF RID: 15023 RVA: 0x000F7DCC File Offset: 0x000F6DCC
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebSysDescription("BaseDataList_DataKeys")]
		public DataKeyCollection DataKeys
		{
			get
			{
				if (this.dataKeysCollection == null)
				{
					this.dataKeysCollection = new DataKeyCollection(this.DataKeysArray);
				}
				return this.dataKeysCollection;
			}
		}

		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x06003AB0 RID: 15024 RVA: 0x000F7DF0 File Offset: 0x000F6DF0
		protected ArrayList DataKeysArray
		{
			get
			{
				object obj = this.ViewState["DataKeys"];
				if (obj == null)
				{
					obj = new ArrayList();
					this.ViewState["DataKeys"] = obj;
				}
				return (ArrayList)obj;
			}
		}

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x06003AB1 RID: 15025 RVA: 0x000F7E30 File Offset: 0x000F6E30
		// (set) Token: 0x06003AB2 RID: 15026 RVA: 0x000F7E5D File Offset: 0x000F6E5D
		[WebSysDescription("BaseDataList_DataKeyField")]
		[DefaultValue("")]
		[Themeable(false)]
		[WebCategory("Data")]
		public virtual string DataKeyField
		{
			get
			{
				object obj = this.ViewState["DataKeyField"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["DataKeyField"] = value;
			}
		}

		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x06003AB3 RID: 15027 RVA: 0x000F7E70 File Offset: 0x000F6E70
		// (set) Token: 0x06003AB4 RID: 15028 RVA: 0x000F7E9D File Offset: 0x000F6E9D
		[WebCategory("Data")]
		[Themeable(false)]
		[DefaultValue("")]
		[WebSysDescription("BaseDataList_DataMember")]
		public string DataMember
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
				this.ViewState["DataMember"] = value;
				this.OnDataPropertyChanged();
			}
		}

		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x06003AB5 RID: 15029 RVA: 0x000F7EB6 File Offset: 0x000F6EB6
		// (set) Token: 0x06003AB6 RID: 15030 RVA: 0x000F7EC0 File Offset: 0x000F6EC0
		[WebCategory("Data")]
		[WebSysDescription("BaseDataBoundControl_DataSource")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Themeable(false)]
		[Bindable(true)]
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

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x06003AB7 RID: 15031 RVA: 0x000F7F10 File Offset: 0x000F6F10
		// (set) Token: 0x06003AB8 RID: 15032 RVA: 0x000F7F3D File Offset: 0x000F6F3D
		[WebSysDescription("BaseDataBoundControl_DataSourceID")]
		[WebCategory("Data")]
		[DefaultValue("")]
		[IDReferenceProperty(typeof(DataSourceControl))]
		[Themeable(false)]
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

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x06003AB9 RID: 15033 RVA: 0x000F7F56 File Offset: 0x000F6F56
		// (set) Token: 0x06003ABA RID: 15034 RVA: 0x000F7F72 File Offset: 0x000F6F72
		[WebCategory("Appearance")]
		[DefaultValue(GridLines.Both)]
		[WebSysDescription("DataControls_GridLines")]
		public virtual GridLines GridLines
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return GridLines.Both;
				}
				return ((TableStyle)base.ControlStyle).GridLines;
			}
			set
			{
				((TableStyle)base.ControlStyle).GridLines = value;
			}
		}

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x06003ABB RID: 15035 RVA: 0x000F7F85 File Offset: 0x000F6F85
		// (set) Token: 0x06003ABC RID: 15036 RVA: 0x000F7FA1 File Offset: 0x000F6FA1
		[DefaultValue(HorizontalAlign.NotSet)]
		[Category("Layout")]
		[WebSysDescription("WebControl_HorizontalAlign")]
		public virtual HorizontalAlign HorizontalAlign
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return HorizontalAlign.NotSet;
				}
				return ((TableStyle)base.ControlStyle).HorizontalAlign;
			}
			set
			{
				((TableStyle)base.ControlStyle).HorizontalAlign = value;
			}
		}

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x06003ABD RID: 15037 RVA: 0x000F7FB4 File Offset: 0x000F6FB4
		protected bool Initialized
		{
			get
			{
				return this._inited;
			}
		}

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x06003ABE RID: 15038 RVA: 0x000F7FBC File Offset: 0x000F6FBC
		protected bool IsBoundUsingDataSourceID
		{
			get
			{
				return this.DataSourceID.Length > 0;
			}
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x06003ABF RID: 15039 RVA: 0x000F7FCC File Offset: 0x000F6FCC
		// (set) Token: 0x06003AC0 RID: 15040 RVA: 0x000F7FD4 File Offset: 0x000F6FD4
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

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x06003AC1 RID: 15041 RVA: 0x000F7FDD File Offset: 0x000F6FDD
		protected DataSourceSelectArguments SelectArguments
		{
			get
			{
				if (this._arguments == null)
				{
					this._arguments = this.CreateDataSourceSelectArguments();
				}
				return this._arguments;
			}
		}

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x06003AC2 RID: 15042 RVA: 0x000F7FFC File Offset: 0x000F6FFC
		// (set) Token: 0x06003AC3 RID: 15043 RVA: 0x000F8025 File Offset: 0x000F7025
		[DefaultValue(false)]
		[WebCategory("Accessibility")]
		[WebSysDescription("Table_UseAccessibleHeader")]
		public virtual bool UseAccessibleHeader
		{
			get
			{
				object obj = this.ViewState["UseAccessibleHeader"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["UseAccessibleHeader"] = value;
			}
		}

		// Token: 0x14000065 RID: 101
		// (add) Token: 0x06003AC4 RID: 15044 RVA: 0x000F803D File Offset: 0x000F703D
		// (remove) Token: 0x06003AC5 RID: 15045 RVA: 0x000F8050 File Offset: 0x000F7050
		[WebSysDescription("BaseDataList_OnSelectedIndexChanged")]
		[WebCategory("Action")]
		public event EventHandler SelectedIndexChanged
		{
			add
			{
				base.Events.AddHandler(BaseDataList.EventSelectedIndexChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(BaseDataList.EventSelectedIndexChanged, value);
			}
		}

		// Token: 0x06003AC6 RID: 15046 RVA: 0x000F8063 File Offset: 0x000F7063
		protected override void AddParsedSubObject(object obj)
		{
		}

		// Token: 0x06003AC7 RID: 15047 RVA: 0x000F8068 File Offset: 0x000F7068
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

		// Token: 0x06003AC8 RID: 15048 RVA: 0x000F81EB File Offset: 0x000F71EB
		protected internal override void CreateChildControls()
		{
			this.Controls.Clear();
			if (this.ViewState["_!ItemCount"] == null)
			{
				if (this.RequiresDataBinding)
				{
					this.EnsureDataBound();
					return;
				}
			}
			else
			{
				this.CreateControlHierarchy(false);
				base.ClearChildViewState();
			}
		}

		// Token: 0x06003AC9 RID: 15049
		protected abstract void CreateControlHierarchy(bool useDataSource);

		// Token: 0x06003ACA RID: 15050 RVA: 0x000F8226 File Offset: 0x000F7226
		public override void DataBind()
		{
			if (this.IsBoundUsingDataSourceID && base.DesignMode && base.Site == null)
			{
				return;
			}
			this.RequiresDataBinding = false;
			this.OnDataBinding(EventArgs.Empty);
		}

		// Token: 0x06003ACB RID: 15051 RVA: 0x000F8253 File Offset: 0x000F7253
		protected virtual DataSourceSelectArguments CreateDataSourceSelectArguments()
		{
			return DataSourceSelectArguments.Empty;
		}

		// Token: 0x06003ACC RID: 15052 RVA: 0x000F825C File Offset: 0x000F725C
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

		// Token: 0x06003ACD RID: 15053 RVA: 0x000F82A8 File Offset: 0x000F72A8
		protected virtual IEnumerable GetData()
		{
			this.ConnectToDataSourceView();
			if (this._currentView != null)
			{
				return this._currentView.ExecuteSelect(this.SelectArguments);
			}
			return null;
		}

		// Token: 0x06003ACE RID: 15054 RVA: 0x000F82CC File Offset: 0x000F72CC
		public static bool IsBindableType(Type type)
		{
			return type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type == typeof(decimal);
		}

		// Token: 0x06003ACF RID: 15055 RVA: 0x000F82FF File Offset: 0x000F72FF
		protected override void OnDataBinding(EventArgs e)
		{
			base.OnDataBinding(e);
			this.Controls.Clear();
			base.ClearChildViewState();
			this.dataKeysCollection = null;
			this.CreateControlHierarchy(true);
			base.ChildControlsCreated = true;
			this.TrackViewState();
		}

		// Token: 0x06003AD0 RID: 15056 RVA: 0x000F8334 File Offset: 0x000F7334
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

		// Token: 0x06003AD1 RID: 15057 RVA: 0x000F8380 File Offset: 0x000F7380
		protected virtual void OnDataSourceViewChanged(object sender, EventArgs e)
		{
			this.RequiresDataBinding = true;
		}

		// Token: 0x06003AD2 RID: 15058 RVA: 0x000F838C File Offset: 0x000F738C
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

		// Token: 0x06003AD3 RID: 15059 RVA: 0x000F83DC File Offset: 0x000F73DC
		protected internal override void OnLoad(EventArgs e)
		{
			this._inited = true;
			this.ConnectToDataSourceView();
			if (this.Page != null && !this._pagePreLoadFired && this.ViewState["_!ItemCount"] == null)
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

		// Token: 0x06003AD4 RID: 15060 RVA: 0x000F8448 File Offset: 0x000F7448
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
				if (this.Page.IsPostBack && base.IsViewStateEnabled && this.ViewState["_!ItemCount"] == null)
				{
					this.RequiresDataBinding = true;
				}
			}
			this._pagePreLoadFired = true;
		}

		// Token: 0x06003AD5 RID: 15061 RVA: 0x000F84C4 File Offset: 0x000F74C4
		protected internal override void OnPreRender(EventArgs e)
		{
			this.EnsureDataBound();
			base.OnPreRender(e);
		}

		// Token: 0x06003AD6 RID: 15062 RVA: 0x000F84D4 File Offset: 0x000F74D4
		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[BaseDataList.EventSelectedIndexChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003AD7 RID: 15063
		protected internal abstract void PrepareControlHierarchy();

		// Token: 0x06003AD8 RID: 15064 RVA: 0x000F8502 File Offset: 0x000F7502
		protected internal override void Render(HtmlTextWriter writer)
		{
			this.PrepareControlHierarchy();
			this.RenderContents(writer);
		}

		// Token: 0x0400268C RID: 9868
		internal const string ItemCountViewStateKey = "_!ItemCount";

		// Token: 0x0400268D RID: 9869
		private static readonly object EventSelectedIndexChanged = new object();

		// Token: 0x0400268E RID: 9870
		private object dataSource;

		// Token: 0x0400268F RID: 9871
		private DataKeyCollection dataKeysCollection;

		// Token: 0x04002690 RID: 9872
		private bool _requiresDataBinding;

		// Token: 0x04002691 RID: 9873
		private bool _inited;

		// Token: 0x04002692 RID: 9874
		private bool _throwOnDataPropertyChange;

		// Token: 0x04002693 RID: 9875
		private DataSourceView _currentView;

		// Token: 0x04002694 RID: 9876
		private bool _currentViewIsFromDataSourceID;

		// Token: 0x04002695 RID: 9877
		private bool _currentViewValid;

		// Token: 0x04002696 RID: 9878
		private DataSourceSelectArguments _arguments;

		// Token: 0x04002697 RID: 9879
		private bool _pagePreLoadFired;
	}
}
