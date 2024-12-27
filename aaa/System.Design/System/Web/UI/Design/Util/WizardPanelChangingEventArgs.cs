using System;

namespace System.Web.UI.Design.Util
{
	// Token: 0x020003D4 RID: 980
	internal class WizardPanelChangingEventArgs : EventArgs
	{
		// Token: 0x06002413 RID: 9235 RVA: 0x000C12BE File Offset: 0x000C02BE
		public WizardPanelChangingEventArgs(WizardPanel currentPanel)
		{
			this._currentPanel = currentPanel;
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06002414 RID: 9236 RVA: 0x000C12CD File Offset: 0x000C02CD
		public WizardPanel CurrentPanel
		{
			get
			{
				return this._currentPanel;
			}
		}

		// Token: 0x040018DD RID: 6365
		private WizardPanel _currentPanel;
	}
}
