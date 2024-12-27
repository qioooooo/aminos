using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004BB RID: 1211
	[Designer("System.Web.UI.Design.WebControls.BaseDataBoundControlDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("DataSourceID")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class BaseDataBoundControl : WebControl
	{
		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x0600397A RID: 14714 RVA: 0x000F355D File Offset: 0x000F255D
		// (set) Token: 0x0600397B RID: 14715 RVA: 0x000F3565 File Offset: 0x000F2565
		[WebCategory("Data")]
		[WebSysDescription("BaseDataBoundControl_DataSource")]
		[Bindable(true)]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Themeable(false)]
		public virtual object DataSource
		{
			get
			{
				return this._dataSource;
			}
			set
			{
				if (value != null)
				{
					this.ValidateDataSource(value);
				}
				this._dataSource = value;
				this.OnDataPropertyChanged();
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x0600397C RID: 14716 RVA: 0x000F3580 File Offset: 0x000F2580
		// (set) Token: 0x0600397D RID: 14717 RVA: 0x000F35AD File Offset: 0x000F25AD
		[WebSysDescription("BaseDataBoundControl_DataSourceID")]
		[Themeable(false)]
		[WebCategory("Data")]
		[DefaultValue("")]
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
				if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(this.DataSourceID))
				{
					this._requiresBindToNull = true;
				}
				this.ViewState["DataSourceID"] = value;
				this.OnDataPropertyChanged();
			}
		}

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x0600397E RID: 14718 RVA: 0x000F35E2 File Offset: 0x000F25E2
		protected bool Initialized
		{
			get
			{
				return this._inited;
			}
		}

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x0600397F RID: 14719 RVA: 0x000F35EA File Offset: 0x000F25EA
		protected bool IsBoundUsingDataSourceID
		{
			get
			{
				return this.DataSourceID.Length > 0;
			}
		}

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x06003980 RID: 14720 RVA: 0x000F35FA File Offset: 0x000F25FA
		// (set) Token: 0x06003981 RID: 14721 RVA: 0x000F3604 File Offset: 0x000F2604
		protected bool RequiresDataBinding
		{
			get
			{
				return this._requiresDataBinding;
			}
			set
			{
				if (value && this._preRendered && this.DataSourceID.Length > 0 && this.Page != null && !this.Page.IsCallback)
				{
					this._requiresDataBinding = true;
					this.EnsureDataBound();
					return;
				}
				this._requiresDataBinding = value;
			}
		}

		// Token: 0x14000062 RID: 98
		// (add) Token: 0x06003982 RID: 14722 RVA: 0x000F3654 File Offset: 0x000F2654
		// (remove) Token: 0x06003983 RID: 14723 RVA: 0x000F3667 File Offset: 0x000F2667
		[WebCategory("Data")]
		[WebSysDescription("BaseDataBoundControl_OnDataBound")]
		public event EventHandler DataBound
		{
			add
			{
				base.Events.AddHandler(BaseDataBoundControl.EventDataBound, value);
			}
			remove
			{
				base.Events.RemoveHandler(BaseDataBoundControl.EventDataBound, value);
			}
		}

		// Token: 0x06003984 RID: 14724 RVA: 0x000F367A File Offset: 0x000F267A
		protected void ConfirmInitState()
		{
			this._inited = true;
		}

		// Token: 0x06003985 RID: 14725 RVA: 0x000F3684 File Offset: 0x000F2684
		public override void DataBind()
		{
			if (base.DesignMode)
			{
				IDictionary designModeState = this.GetDesignModeState();
				if ((designModeState == null || designModeState["EnableDesignTimeDataBinding"] == null) && base.Site == null)
				{
					return;
				}
			}
			this.PerformSelect();
		}

		// Token: 0x06003986 RID: 14726 RVA: 0x000F36C0 File Offset: 0x000F26C0
		protected virtual void EnsureDataBound()
		{
			try
			{
				this._throwOnDataPropertyChange = true;
				if (this.RequiresDataBinding && (this.DataSourceID.Length > 0 || this._requiresBindToNull))
				{
					this.DataBind();
					this._requiresBindToNull = false;
				}
			}
			finally
			{
				this._throwOnDataPropertyChange = false;
			}
		}

		// Token: 0x06003987 RID: 14727 RVA: 0x000F371C File Offset: 0x000F271C
		protected virtual void OnDataBound(EventArgs e)
		{
			EventHandler eventHandler = base.Events[BaseDataBoundControl.EventDataBound] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003988 RID: 14728 RVA: 0x000F374C File Offset: 0x000F274C
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
		}

		// Token: 0x06003989 RID: 14729 RVA: 0x000F3794 File Offset: 0x000F2794
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

		// Token: 0x0600398A RID: 14730 RVA: 0x000F37E4 File Offset: 0x000F27E4
		protected virtual void OnPagePreLoad(object sender, EventArgs e)
		{
			this._inited = true;
			if (this.Page != null)
			{
				this.Page.PreLoad -= this.OnPagePreLoad;
			}
		}

		// Token: 0x0600398B RID: 14731 RVA: 0x000F380D File Offset: 0x000F280D
		protected internal override void OnPreRender(EventArgs e)
		{
			this._preRendered = true;
			this.EnsureDataBound();
			base.OnPreRender(e);
		}

		// Token: 0x0600398C RID: 14732
		protected abstract void PerformSelect();

		// Token: 0x0600398D RID: 14733
		protected abstract void ValidateDataSource(object dataSource);

		// Token: 0x04002627 RID: 9767
		private static readonly object EventDataBound = new object();

		// Token: 0x04002628 RID: 9768
		private object _dataSource;

		// Token: 0x04002629 RID: 9769
		private bool _requiresDataBinding;

		// Token: 0x0400262A RID: 9770
		private bool _inited;

		// Token: 0x0400262B RID: 9771
		private bool _preRendered;

		// Token: 0x0400262C RID: 9772
		private bool _requiresBindToNull;

		// Token: 0x0400262D RID: 9773
		private bool _throwOnDataPropertyChange;
	}
}
