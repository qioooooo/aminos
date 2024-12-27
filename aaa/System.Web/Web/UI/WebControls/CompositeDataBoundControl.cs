using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000501 RID: 1281
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class CompositeDataBoundControl : DataBoundControl, INamingContainer
	{
		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x06003E9A RID: 16026 RVA: 0x00104DBC File Offset: 0x00103DBC
		public override ControlCollection Controls
		{
			get
			{
				this.EnsureChildControls();
				return base.Controls;
			}
		}

		// Token: 0x06003E9B RID: 16027 RVA: 0x00104DCC File Offset: 0x00103DCC
		protected internal override void CreateChildControls()
		{
			this.Controls.Clear();
			object obj = this.ViewState["_!ItemCount"];
			if (obj == null && base.RequiresDataBinding)
			{
				this.EnsureDataBound();
			}
			if (obj != null && (int)obj != -1)
			{
				DummyDataSource dummyDataSource = new DummyDataSource((int)obj);
				this.CreateChildControls(dummyDataSource, false);
				base.ClearChildViewState();
			}
		}

		// Token: 0x06003E9C RID: 16028
		protected abstract int CreateChildControls(IEnumerable dataSource, bool dataBinding);

		// Token: 0x06003E9D RID: 16029 RVA: 0x00104E30 File Offset: 0x00103E30
		protected internal override void PerformDataBinding(IEnumerable data)
		{
			base.PerformDataBinding(data);
			this.Controls.Clear();
			base.ClearChildViewState();
			this.TrackViewState();
			int num = this.CreateChildControls(data, true);
			base.ChildControlsCreated = true;
			this.ViewState["_!ItemCount"] = num;
		}

		// Token: 0x0400279E RID: 10142
		internal const string ItemCountViewStateKey = "_!ItemCount";
	}
}
