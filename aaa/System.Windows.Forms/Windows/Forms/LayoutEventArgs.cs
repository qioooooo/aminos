using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000466 RID: 1126
	public sealed class LayoutEventArgs : EventArgs
	{
		// Token: 0x06004266 RID: 16998 RVA: 0x000ED3F4 File Offset: 0x000EC3F4
		public LayoutEventArgs(IComponent affectedComponent, string affectedProperty)
		{
			this.affectedComponent = affectedComponent;
			this.affectedProperty = affectedProperty;
		}

		// Token: 0x06004267 RID: 16999 RVA: 0x000ED40A File Offset: 0x000EC40A
		public LayoutEventArgs(Control affectedControl, string affectedProperty)
			: this(affectedControl, affectedProperty)
		{
		}

		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x06004268 RID: 17000 RVA: 0x000ED414 File Offset: 0x000EC414
		public IComponent AffectedComponent
		{
			get
			{
				return this.affectedComponent;
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x06004269 RID: 17001 RVA: 0x000ED41C File Offset: 0x000EC41C
		public Control AffectedControl
		{
			get
			{
				return this.affectedComponent as Control;
			}
		}

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x0600426A RID: 17002 RVA: 0x000ED429 File Offset: 0x000EC429
		public string AffectedProperty
		{
			get
			{
				return this.affectedProperty;
			}
		}

		// Token: 0x0400209B RID: 8347
		private readonly IComponent affectedComponent;

		// Token: 0x0400209C RID: 8348
		private readonly string affectedProperty;
	}
}
