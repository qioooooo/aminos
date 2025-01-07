using System;
using System.Collections.Generic;
using System.Design;
using System.Drawing;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	internal abstract partial class WizardForm : TaskFormBase
	{
		public WizardForm(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this._panelHistory = new Stack<WizardPanel>();
			this.InitializeComponent();
			this.InitializeUI();
		}

		public Button FinishButton
		{
			get
			{
				return this._finishButton;
			}
		}

		public Button NextButton
		{
			get
			{
				return this._nextButton;
			}
		}

		public Button PreviousButton
		{
			get
			{
				return this._previousButton;
			}
		}

		private void InitializeUI()
		{
			this._cancelButton.Text = SR.GetString("Wizard_CancelButton");
			this._nextButton.Text = SR.GetString("Wizard_NextButton");
			this._previousButton.Text = SR.GetString("Wizard_PreviousButton");
			this._finishButton.Text = SR.GetString("Wizard_FinishButton");
		}

		public void NextPanel()
		{
			WizardPanel wizardPanel = this._panelHistory.Peek();
			if (wizardPanel.OnNext())
			{
				wizardPanel.Hide();
				WizardPanel nextPanel = wizardPanel.NextPanel;
				if (nextPanel != null)
				{
					this.RegisterPanel(nextPanel);
					this._panelHistory.Push(nextPanel);
					this.OnPanelChanging(new WizardPanelChangingEventArgs(wizardPanel));
					this.ShowPanel(nextPanel);
				}
			}
		}

		protected virtual void OnCancelButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		protected override void OnInitialActivated(EventArgs e)
		{
			base.OnInitialActivated(e);
			if (this._initialPanel != null)
			{
				this.RegisterPanel(this._initialPanel);
				this._panelHistory.Push(this._initialPanel);
				this.ShowPanel(this._initialPanel);
			}
		}

		protected virtual void OnFinishButtonClick(object sender, EventArgs e)
		{
			WizardPanel wizardPanel = this._panelHistory.Peek();
			if (wizardPanel.OnNext())
			{
				WizardPanel[] array = this._panelHistory.ToArray();
				Array.Reverse(array);
				foreach (WizardPanel wizardPanel2 in array)
				{
					wizardPanel2.OnComplete();
				}
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
		}

		protected virtual void OnNextButtonClick(object sender, EventArgs e)
		{
			this.NextPanel();
		}

		protected virtual void OnPanelChanging(WizardPanelChangingEventArgs e)
		{
		}

		protected virtual void OnPreviousButtonClick(object sender, EventArgs e)
		{
			this.PreviousPanel();
		}

		public void PreviousPanel()
		{
			if (this._panelHistory.Count > 1)
			{
				WizardPanel wizardPanel = this._panelHistory.Pop();
				WizardPanel wizardPanel2 = this._panelHistory.Peek();
				wizardPanel.OnPrevious();
				wizardPanel.Hide();
				this.OnPanelChanging(new WizardPanelChangingEventArgs(wizardPanel));
				this.ShowPanel(wizardPanel2);
			}
		}

		internal void RegisterPanel(WizardPanel panel)
		{
			if (!base.TaskPanel.Controls.Contains(panel))
			{
				panel.Dock = DockStyle.Fill;
				panel.SetParentWizard(this);
				panel.Hide();
				base.TaskPanel.Controls.Add(panel);
			}
		}

		protected void SetPanels(WizardPanel[] panels)
		{
			if (panels != null && panels.Length > 0)
			{
				this.RegisterPanel(panels[0]);
				this._initialPanel = panels[0];
				for (int i = 0; i < panels.Length - 1; i++)
				{
					this.RegisterPanel(panels[i + 1]);
					panels[i].NextPanel = panels[i + 1];
				}
			}
		}

		private void ShowPanel(WizardPanel panel)
		{
			if (this._panelHistory.Count == 1)
			{
				this.PreviousButton.Enabled = false;
			}
			else
			{
				this.PreviousButton.Enabled = true;
			}
			if (panel.NextPanel == null)
			{
				this.NextButton.Enabled = false;
			}
			else
			{
				this.NextButton.Enabled = true;
			}
			panel.Show();
			base.AccessibleDescription = panel.Caption;
			base.CaptionLabel.Text = panel.Caption;
			if (base.IsHandleCreated)
			{
				base.Invalidate();
			}
			panel.Focus();
		}

		private Stack<WizardPanel> _panelHistory;

		private WizardPanel _initialPanel;
	}
}
