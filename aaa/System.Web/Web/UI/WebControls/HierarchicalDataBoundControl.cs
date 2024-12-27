using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.UI.WebControls.Adapters;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005B3 RID: 1459
	[Designer("System.Web.UI.Design.WebControls.HierarchicalDataBoundControlDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class HierarchicalDataBoundControl : BaseDataBoundControl
	{
		// Token: 0x1700119E RID: 4510
		// (get) Token: 0x06004771 RID: 18289 RVA: 0x0012414E File Offset: 0x0012314E
		// (set) Token: 0x06004772 RID: 18290 RVA: 0x00124156 File Offset: 0x00123156
		[IDReferenceProperty(typeof(HierarchicalDataSourceControl))]
		public override string DataSourceID
		{
			get
			{
				return base.DataSourceID;
			}
			set
			{
				base.DataSourceID = value;
			}
		}

		// Token: 0x06004773 RID: 18291 RVA: 0x00124160 File Offset: 0x00123160
		private IHierarchicalDataSource ConnectToHierarchicalDataSource()
		{
			if (!this._currentDataSourceValid || base.DesignMode)
			{
				if (this._currentHierarchicalDataSource != null && this._currentDataSourceIsFromControl)
				{
					this._currentHierarchicalDataSource.DataSourceChanged -= this.OnDataSourceChanged;
				}
				this._currentHierarchicalDataSource = this.GetDataSource();
				this._currentDataSourceIsFromControl = base.IsBoundUsingDataSourceID;
				if (this._currentHierarchicalDataSource == null)
				{
					this._currentHierarchicalDataSource = new ReadOnlyHierarchicalDataSource(this.DataSource);
				}
				else if (this.DataSource != null)
				{
					throw new InvalidOperationException(SR.GetString("DataControl_MultipleDataSources", new object[] { this.ID }));
				}
				this._currentDataSourceValid = true;
				if (this._currentHierarchicalDataSource != null && this._currentDataSourceIsFromControl)
				{
					this._currentHierarchicalDataSource.DataSourceChanged += this.OnDataSourceChanged;
				}
				return this._currentHierarchicalDataSource;
			}
			if (!this._currentDataSourceIsFromControl && this.DataSourceID != null && this.DataSourceID.Length != 0)
			{
				throw new InvalidOperationException(SR.GetString("DataControl_MultipleDataSources", new object[] { this.ID }));
			}
			return this._currentHierarchicalDataSource;
		}

		// Token: 0x06004774 RID: 18292 RVA: 0x0012427C File Offset: 0x0012327C
		protected virtual HierarchicalDataSourceView GetData(string viewPath)
		{
			IHierarchicalDataSource hierarchicalDataSource = this.ConnectToHierarchicalDataSource();
			HierarchicalDataSourceView hierarchicalView = hierarchicalDataSource.GetHierarchicalView(viewPath);
			if (hierarchicalView == null)
			{
				throw new InvalidOperationException(SR.GetString("HierarchicalDataControl_ViewNotFound", new object[] { this.ID }));
			}
			return hierarchicalView;
		}

		// Token: 0x06004775 RID: 18293 RVA: 0x001242C0 File Offset: 0x001232C0
		protected virtual IHierarchicalDataSource GetDataSource()
		{
			if (!base.DesignMode && this._currentDataSourceValid && this._currentHierarchicalDataSource != null)
			{
				return this._currentHierarchicalDataSource;
			}
			IHierarchicalDataSource hierarchicalDataSource = null;
			string dataSourceID = this.DataSourceID;
			if (dataSourceID.Length != 0)
			{
				Control control = DataBoundControlHelper.FindControl(this, dataSourceID);
				if (control == null)
				{
					throw new HttpException(SR.GetString("HierarchicalDataControl_DataSourceDoesntExist", new object[] { this.ID, dataSourceID }));
				}
				hierarchicalDataSource = control as IHierarchicalDataSource;
				if (hierarchicalDataSource == null)
				{
					throw new HttpException(SR.GetString("HierarchicalDataControl_DataSourceIDMustBeHierarchicalDataControl", new object[] { this.ID, dataSourceID }));
				}
			}
			return hierarchicalDataSource;
		}

		// Token: 0x06004776 RID: 18294 RVA: 0x00124361 File Offset: 0x00123361
		protected void MarkAsDataBound()
		{
			this.ViewState["_!DataBound"] = true;
		}

		// Token: 0x06004777 RID: 18295 RVA: 0x00124379 File Offset: 0x00123379
		protected override void OnDataPropertyChanged()
		{
			this._currentDataSourceValid = false;
			base.OnDataPropertyChanged();
		}

		// Token: 0x06004778 RID: 18296 RVA: 0x00124388 File Offset: 0x00123388
		protected virtual void OnDataSourceChanged(object sender, EventArgs e)
		{
			base.RequiresDataBinding = true;
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x00124394 File Offset: 0x00123394
		protected internal override void OnLoad(EventArgs e)
		{
			base.ConfirmInitState();
			this.ConnectToHierarchicalDataSource();
			if (this.Page != null && !this._pagePreLoadFired && this.ViewState["_!DataBound"] == null)
			{
				if (!this.Page.IsPostBack)
				{
					base.RequiresDataBinding = true;
				}
				else if (base.IsViewStateEnabled)
				{
					base.RequiresDataBinding = true;
				}
			}
			base.OnLoad(e);
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x001243FC File Offset: 0x001233FC
		protected override void OnPagePreLoad(object sender, EventArgs e)
		{
			base.OnPagePreLoad(sender, e);
			if (this.Page != null)
			{
				if (!this.Page.IsPostBack)
				{
					base.RequiresDataBinding = true;
				}
				else if (base.IsViewStateEnabled && this.ViewState["_!DataBound"] == null)
				{
					base.RequiresDataBinding = true;
				}
			}
			this._pagePreLoadFired = true;
		}

		// Token: 0x0600477B RID: 18299 RVA: 0x00124457 File Offset: 0x00123457
		protected internal virtual void PerformDataBinding()
		{
		}

		// Token: 0x0600477C RID: 18300 RVA: 0x0012445C File Offset: 0x0012345C
		protected override void PerformSelect()
		{
			this.OnDataBinding(EventArgs.Empty);
			if (this._adapter != null)
			{
				HierarchicalDataBoundControlAdapter hierarchicalDataBoundControlAdapter = this._adapter as HierarchicalDataBoundControlAdapter;
				if (hierarchicalDataBoundControlAdapter != null)
				{
					hierarchicalDataBoundControlAdapter.PerformDataBinding();
				}
				else
				{
					this.PerformDataBinding();
				}
			}
			else
			{
				this.PerformDataBinding();
			}
			base.RequiresDataBinding = false;
			this.MarkAsDataBound();
			this.OnDataBound(EventArgs.Empty);
		}

		// Token: 0x0600477D RID: 18301 RVA: 0x001244B9 File Offset: 0x001234B9
		protected override void ValidateDataSource(object dataSource)
		{
			if (dataSource == null || dataSource is IHierarchicalEnumerable || dataSource is IHierarchicalDataSource)
			{
				return;
			}
			throw new InvalidOperationException(SR.GetString("HierarchicalDataBoundControl_InvalidDataSource"));
		}

		// Token: 0x04002A97 RID: 10903
		private const string DataBoundViewStateKey = "_!DataBound";

		// Token: 0x04002A98 RID: 10904
		private IHierarchicalDataSource _currentHierarchicalDataSource;

		// Token: 0x04002A99 RID: 10905
		private bool _currentDataSourceIsFromControl;

		// Token: 0x04002A9A RID: 10906
		private bool _currentDataSourceValid;

		// Token: 0x04002A9B RID: 10907
		private bool _pagePreLoadFired;
	}
}
