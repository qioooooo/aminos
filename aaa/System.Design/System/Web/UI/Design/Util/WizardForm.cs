using System;
using System.Collections.Generic;
using System.Design;
using System.Drawing;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	// Token: 0x020003D2 RID: 978
	internal abstract partial class WizardForm : TaskFormBase
	{
		// Token: 0x060023F7 RID: 9207 RVA: 0x000C08E3 File Offset: 0x000BF8E3
		public WizardForm(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this._panelHistory = new Stack<WizardPanel>();
			this.InitializeComponent();
			this.InitializeUI();
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x060023F8 RID: 9208 RVA: 0x000C0903 File Offset: 0x000BF903
		public Button FinishButton
		{
			get
			{
				return this._finishButton;
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x060023F9 RID: 9209 RVA: 0x000C090B File Offset: 0x000BF90B
		public Button NextButton
		{
			get
			{
				return this._nextButton;
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x060023FA RID: 9210 RVA: 0x000C0913 File Offset: 0x000BF913
		public Button PreviousButton
		{
			get
			{
				return this._previousButton;
			}
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x000C0F28 File Offset: 0x000BFF28
		private void InitializeUI()
		{
			this._cancelButton.Text = SR.GetString("Wizard_CancelButton");
			this._nextButton.Text = SR.GetString("Wizard_NextButton");
			this._previousButton.Text = SR.GetString("Wizard_PreviousButton");
			this._finishButton.Text = SR.GetString("Wizard_FinishButton");
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x000C0F8C File Offset: 0x000BFF8C
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

		// Token: 0x060023FE RID: 9214 RVA: 0x000C0FE3 File Offset: 0x000BFFE3
		protected virtual void OnCancelButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x000C0FF2 File Offset: 0x000BFFF2
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

		// Token: 0x06002400 RID: 9216 RVA: 0x000C102C File Offset: 0x000C002C
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

		// Token: 0x06002401 RID: 9217 RVA: 0x000C108B File Offset: 0x000C008B
		protected virtual void OnNextButtonClick(object sender, EventArgs e)
		{
			this.NextPanel();
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x000C1093 File Offset: 0x000C0093
		protected virtual void OnPanelChanging(WizardPanelChangingEventArgs e)
		{
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x000C1095 File Offset: 0x000C0095
		protected virtual void OnPreviousButtonClick(object sender, EventArgs e)
		{
			this.PreviousPanel();
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x000C10A0 File Offset: 0x000C00A0
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

		// Token: 0x06002405 RID: 9221 RVA: 0x000C10F2 File Offset: 0x000C00F2
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

		// Token: 0x06002406 RID: 9222 RVA: 0x000C112C File Offset: 0x000C012C
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

		// Token: 0x06002407 RID: 9223 RVA: 0x000C117C File Offset: 0x000C017C
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

		// Token: 0x040018D7 RID: 6359
		private Stack<WizardPanel> _panelHistory;

		// Token: 0x040018D8 RID: 6360
		private WizardPanel _initialPanel;
	}
}
