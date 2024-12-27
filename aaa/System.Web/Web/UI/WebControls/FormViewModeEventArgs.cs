using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200058D RID: 1421
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class FormViewModeEventArgs : CancelEventArgs
	{
		// Token: 0x060045E1 RID: 17889 RVA: 0x0011EA5A File Offset: 0x0011DA5A
		public FormViewModeEventArgs(FormViewMode mode, bool cancelingEdit)
			: base(false)
		{
			this._mode = mode;
			this._cancelingEdit = cancelingEdit;
		}

		// Token: 0x17001123 RID: 4387
		// (get) Token: 0x060045E2 RID: 17890 RVA: 0x0011EA71 File Offset: 0x0011DA71
		public bool CancelingEdit
		{
			get
			{
				return this._cancelingEdit;
			}
		}

		// Token: 0x17001124 RID: 4388
		// (get) Token: 0x060045E3 RID: 17891 RVA: 0x0011EA79 File Offset: 0x0011DA79
		// (set) Token: 0x060045E4 RID: 17892 RVA: 0x0011EA81 File Offset: 0x0011DA81
		public FormViewMode NewMode
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

		// Token: 0x04002A21 RID: 10785
		private FormViewMode _mode;

		// Token: 0x04002A22 RID: 10786
		private bool _cancelingEdit;
	}
}
