using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000563 RID: 1379
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DetailsViewModeEventArgs : CancelEventArgs
	{
		// Token: 0x06004424 RID: 17444 RVA: 0x0011988A File Offset: 0x0011888A
		public DetailsViewModeEventArgs(DetailsViewMode mode, bool cancelingEdit)
			: base(false)
		{
			this._mode = mode;
			this._cancelingEdit = cancelingEdit;
		}

		// Token: 0x170010A6 RID: 4262
		// (get) Token: 0x06004425 RID: 17445 RVA: 0x001198A1 File Offset: 0x001188A1
		public bool CancelingEdit
		{
			get
			{
				return this._cancelingEdit;
			}
		}

		// Token: 0x170010A7 RID: 4263
		// (get) Token: 0x06004426 RID: 17446 RVA: 0x001198A9 File Offset: 0x001188A9
		// (set) Token: 0x06004427 RID: 17447 RVA: 0x001198B1 File Offset: 0x001188B1
		public DetailsViewMode NewMode
		{
			get
			{
				return this._mode;
			}
			set
			{
				this._mode = value;
			}
		}

		// Token: 0x040029A0 RID: 10656
		private DetailsViewMode _mode;

		// Token: 0x040029A1 RID: 10657
		private bool _cancelingEdit;
	}
}
