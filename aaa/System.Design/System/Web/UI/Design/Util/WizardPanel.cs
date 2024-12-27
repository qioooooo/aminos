using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	// Token: 0x020003D3 RID: 979
	internal class WizardPanel : UserControl
	{
		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06002409 RID: 9225 RVA: 0x000C1213 File Offset: 0x000C0213
		// (set) Token: 0x0600240A RID: 9226 RVA: 0x000C1229 File Offset: 0x000C0229
		public string Caption
		{
			get
			{
				if (this._caption == null)
				{
					return string.Empty;
				}
				return this._caption;
			}
			set
			{
				this._caption = value;
				if (this._parentWizard != null)
				{
					this._parentWizard.Invalidate();
					return;
				}
				this._needsToInvalidate = true;
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x0600240B RID: 9227 RVA: 0x000C124D File Offset: 0x000C024D
		// (set) Token: 0x0600240C RID: 9228 RVA: 0x000C1255 File Offset: 0x000C0255
		public WizardPanel NextPanel
		{
			get
			{
				return this._nextPanel;
			}
			set
			{
				this._nextPanel = value;
				if (this._parentWizard != null)
				{
					this._parentWizard.RegisterPanel(this._nextPanel);
				}
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x0600240D RID: 9229 RVA: 0x000C1277 File Offset: 0x000C0277
		[Browsable(false)]
		public WizardForm ParentWizard
		{
			get
			{
				return this._parentWizard;
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x0600240E RID: 9230 RVA: 0x000C127F File Offset: 0x000C027F
		protected IServiceProvider ServiceProvider
		{
			get
			{
				return this.ParentWizard.ServiceProvider;
			}
		}

		// Token: 0x0600240F RID: 9231 RVA: 0x000C128C File Offset: 0x000C028C
		protected internal virtual void OnComplete()
		{
		}

		// Token: 0x06002410 RID: 9232 RVA: 0x000C128E File Offset: 0x000C028E
		public virtual bool OnNext()
		{
			return true;
		}

		// Token: 0x06002411 RID: 9233 RVA: 0x000C1291 File Offset: 0x000C0291
		public virtual void OnPrevious()
		{
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x000C1293 File Offset: 0x000C0293
		internal void SetParentWizard(WizardForm parent)
		{
			this._parentWizard = parent;
			if (this._parentWizard != null && this._needsToInvalidate)
			{
				this._parentWizard.Invalidate();
				this._needsToInvalidate = false;
			}
		}

		// Token: 0x040018D9 RID: 6361
		private WizardForm _parentWizard;

		// Token: 0x040018DA RID: 6362
		private string _caption;

		// Token: 0x040018DB RID: 6363
		private WizardPanel _nextPanel;

		// Token: 0x040018DC RID: 6364
		private bool _needsToInvalidate;
	}
}
