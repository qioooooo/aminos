using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005DA RID: 1498
	[DefaultEvent("ViewChanged")]
	[Bindable(false)]
	[ParseChildren(true)]
	[DefaultProperty("CurrentView")]
	[Themeable(true)]
	[PersistChildren(false)]
	[Designer("System.Web.UI.Design.WebControls.LoginViewDesigner,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class LoginView : Control, INamingContainer
	{
		// Token: 0x1700122E RID: 4654
		// (get) Token: 0x0600495A RID: 18778 RVA: 0x0012AF2D File Offset: 0x00129F2D
		// (set) Token: 0x0600495B RID: 18779 RVA: 0x0012AF35 File Offset: 0x00129F35
		[Browsable(false)]
		[DefaultValue(null)]
		[TemplateContainer(typeof(LoginView))]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual ITemplate AnonymousTemplate
		{
			get
			{
				return this._anonymousTemplate;
			}
			set
			{
				this._anonymousTemplate = value;
			}
		}

		// Token: 0x1700122F RID: 4655
		// (get) Token: 0x0600495C RID: 18780 RVA: 0x0012AF3E File Offset: 0x00129F3E
		// (set) Token: 0x0600495D RID: 18781 RVA: 0x0012AF46 File Offset: 0x00129F46
		[Browsable(true)]
		public override bool EnableTheming
		{
			get
			{
				return base.EnableTheming;
			}
			set
			{
				base.EnableTheming = value;
			}
		}

		// Token: 0x17001230 RID: 4656
		// (get) Token: 0x0600495E RID: 18782 RVA: 0x0012AF4F File Offset: 0x00129F4F
		// (set) Token: 0x0600495F RID: 18783 RVA: 0x0012AF57 File Offset: 0x00129F57
		[Browsable(true)]
		public override string SkinID
		{
			get
			{
				return base.SkinID;
			}
			set
			{
				base.SkinID = value;
			}
		}

		// Token: 0x17001231 RID: 4657
		// (get) Token: 0x06004960 RID: 18784 RVA: 0x0012AF60 File Offset: 0x00129F60
		public override ControlCollection Controls
		{
			get
			{
				this.EnsureChildControls();
				return base.Controls;
			}
		}

		// Token: 0x06004961 RID: 18785 RVA: 0x0012AF6E File Offset: 0x00129F6E
		public override void DataBind()
		{
			this.OnDataBinding(EventArgs.Empty);
			this.EnsureChildControls();
			this.DataBindChildren();
		}

		// Token: 0x17001232 RID: 4658
		// (get) Token: 0x06004962 RID: 18786 RVA: 0x0012AF87 File Offset: 0x00129F87
		// (set) Token: 0x06004963 RID: 18787 RVA: 0x0012AF8F File Offset: 0x00129F8F
		[TemplateContainer(typeof(LoginView))]
		[Browsable(false)]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual ITemplate LoggedInTemplate
		{
			get
			{
				return this._loggedInTemplate;
			}
			set
			{
				this._loggedInTemplate = value;
			}
		}

		// Token: 0x17001233 RID: 4659
		// (get) Token: 0x06004964 RID: 18788 RVA: 0x0012AF98 File Offset: 0x00129F98
		[WebSysDescription("LoginView_RoleGroups")]
		[Filterable(false)]
		[MergableProperty(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[Themeable(false)]
		[WebCategory("Behavior")]
		public virtual RoleGroupCollection RoleGroups
		{
			get
			{
				if (this._roleGroups == null)
				{
					this._roleGroups = new RoleGroupCollection();
				}
				return this._roleGroups;
			}
		}

		// Token: 0x17001234 RID: 4660
		// (get) Token: 0x06004965 RID: 18789 RVA: 0x0012AFB3 File Offset: 0x00129FB3
		// (set) Token: 0x06004966 RID: 18790 RVA: 0x0012AFBB File Offset: 0x00129FBB
		private int TemplateIndex
		{
			get
			{
				return this._templateIndex;
			}
			set
			{
				if (value != this.TemplateIndex)
				{
					this.OnViewChanging(EventArgs.Empty);
					this._templateIndex = value;
					base.ChildControlsCreated = false;
					this.OnViewChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x140000D4 RID: 212
		// (add) Token: 0x06004967 RID: 18791 RVA: 0x0012AFEA File Offset: 0x00129FEA
		// (remove) Token: 0x06004968 RID: 18792 RVA: 0x0012AFFD File Offset: 0x00129FFD
		[WebCategory("Action")]
		[WebSysDescription("LoginView_ViewChanged")]
		public event EventHandler ViewChanged
		{
			add
			{
				base.Events.AddHandler(LoginView.EventViewChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(LoginView.EventViewChanged, value);
			}
		}

		// Token: 0x140000D5 RID: 213
		// (add) Token: 0x06004969 RID: 18793 RVA: 0x0012B010 File Offset: 0x0012A010
		// (remove) Token: 0x0600496A RID: 18794 RVA: 0x0012B023 File Offset: 0x0012A023
		[WebSysDescription("LoginView_ViewChanging")]
		[WebCategory("Action")]
		public event EventHandler ViewChanging
		{
			add
			{
				base.Events.AddHandler(LoginView.EventViewChanging, value);
			}
			remove
			{
				base.Events.RemoveHandler(LoginView.EventViewChanging, value);
			}
		}

		// Token: 0x0600496B RID: 18795 RVA: 0x0012B038 File Offset: 0x0012A038
		protected internal override void CreateChildControls()
		{
			this.Controls.Clear();
			Page page = this.Page;
			if (page != null && !page.IsPostBack && !base.DesignMode)
			{
				this._templateIndex = this.GetTemplateIndex();
			}
			int templateIndex = this.TemplateIndex;
			ITemplate template = null;
			switch (templateIndex)
			{
			case 0:
				template = this.AnonymousTemplate;
				break;
			case 1:
				template = this.LoggedInTemplate;
				break;
			default:
			{
				int num = templateIndex - 2;
				RoleGroupCollection roleGroups = this.RoleGroups;
				if (0 <= num && num < roleGroups.Count)
				{
					template = roleGroups[num].ContentTemplate;
				}
				break;
			}
			}
			if (template != null)
			{
				Control control = new Control();
				template.InstantiateIn(control);
				this.Controls.Add(control);
			}
		}

		// Token: 0x0600496C RID: 18796 RVA: 0x0012B0EC File Offset: 0x0012A0EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void Focus()
		{
			throw new NotSupportedException(SR.GetString("NoFocusSupport", new object[] { base.GetType().Name }));
		}

		// Token: 0x0600496D RID: 18797 RVA: 0x0012B120 File Offset: 0x0012A120
		protected internal override void LoadControlState(object savedState)
		{
			if (savedState != null)
			{
				Pair pair = (Pair)savedState;
				if (pair.First != null)
				{
					base.LoadControlState(pair.First);
				}
				if (pair.Second != null)
				{
					this._templateIndex = (int)pair.Second;
				}
			}
		}

		// Token: 0x0600496E RID: 18798 RVA: 0x0012B164 File Offset: 0x0012A164
		protected internal override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			if (this.Page != null)
			{
				this.Page.RegisterRequiresControlState(this);
			}
		}

		// Token: 0x0600496F RID: 18799 RVA: 0x0012B181 File Offset: 0x0012A181
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			this.TemplateIndex = this.GetTemplateIndex();
			this.EnsureChildControls();
		}

		// Token: 0x06004970 RID: 18800 RVA: 0x0012B19C File Offset: 0x0012A19C
		protected virtual void OnViewChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[LoginView.EventViewChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004971 RID: 18801 RVA: 0x0012B1CC File Offset: 0x0012A1CC
		protected virtual void OnViewChanging(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[LoginView.EventViewChanging];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004972 RID: 18802 RVA: 0x0012B1FA File Offset: 0x0012A1FA
		protected internal override void Render(HtmlTextWriter writer)
		{
			this.EnsureChildControls();
			base.Render(writer);
		}

		// Token: 0x06004973 RID: 18803 RVA: 0x0012B20C File Offset: 0x0012A20C
		protected internal override object SaveControlState()
		{
			object obj = base.SaveControlState();
			if (obj != null || this._templateIndex != 0)
			{
				object obj2 = null;
				if (this._templateIndex != 0)
				{
					obj2 = this._templateIndex;
				}
				return new Pair(obj, obj2);
			}
			return null;
		}

		// Token: 0x06004974 RID: 18804 RVA: 0x0012B24C File Offset: 0x0012A24C
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		protected override void SetDesignModeState(IDictionary data)
		{
			if (data != null)
			{
				object obj = data["TemplateIndex"];
				if (obj != null)
				{
					this.TemplateIndex = (int)obj;
					base.ChildControlsCreated = false;
				}
			}
		}

		// Token: 0x06004975 RID: 18805 RVA: 0x0012B280 File Offset: 0x0012A280
		private int GetTemplateIndex()
		{
			if (base.DesignMode || this.Page == null || !this.Page.Request.IsAuthenticated)
			{
				return 0;
			}
			IPrincipal user = LoginUtil.GetUser(this);
			int num = -1;
			if (user != null)
			{
				num = this.RoleGroups.GetMatchingRoleGroupInternal(user);
			}
			if (num >= 0)
			{
				return num + 2;
			}
			return 1;
		}

		// Token: 0x04002B28 RID: 11048
		private const int anonymousTemplateIndex = 0;

		// Token: 0x04002B29 RID: 11049
		private const int loggedInTemplateIndex = 1;

		// Token: 0x04002B2A RID: 11050
		private const int roleGroupStartingIndex = 2;

		// Token: 0x04002B2B RID: 11051
		private RoleGroupCollection _roleGroups;

		// Token: 0x04002B2C RID: 11052
		private ITemplate _loggedInTemplate;

		// Token: 0x04002B2D RID: 11053
		private ITemplate _anonymousTemplate;

		// Token: 0x04002B2E RID: 11054
		private int _templateIndex;

		// Token: 0x04002B2F RID: 11055
		private static readonly object EventViewChanging = new object();

		// Token: 0x04002B30 RID: 11056
		private static readonly object EventViewChanged = new object();
	}
}
