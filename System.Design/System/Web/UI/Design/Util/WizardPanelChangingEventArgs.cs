using System;

namespace System.Web.UI.Design.Util
{
	internal class WizardPanelChangingEventArgs : EventArgs
	{
		public WizardPanelChangingEventArgs(WizardPanel currentPanel)
		{
			this._currentPanel = currentPanel;
		}

		public WizardPanel CurrentPanel
		{
			get
			{
				return this._currentPanel;
			}
		}

		private WizardPanel _currentPanel;
	}
}
