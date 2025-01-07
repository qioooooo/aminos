using System;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	internal abstract partial class DesignerForm : Form
	{
		protected DesignerForm(IServiceProvider serviceProvider)
		{
			this._serviceProvider = serviceProvider;
			this._firstActivate = true;
		}

		protected internal IServiceProvider ServiceProvider
		{
			get
			{
				return this._serviceProvider;
			}
		}

		protected void InitializeForm()
		{
			Font dialogFont = UIServiceHelper.GetDialogFont(this.ServiceProvider);
			if (dialogFont != null)
			{
				this.Font = dialogFont;
			}
			string @string = SR.GetString("RTL");
			if (!string.Equals(@string, "RTL_False", StringComparison.Ordinal))
			{
				this.RightToLeft = RightToLeft.Yes;
				this.RightToLeftLayout = true;
			}
			this.AutoScaleBaseSize = new Size(5, 14);
			base.HelpButton = true;
			base.MinimizeBox = false;
			base.MaximizeBox = false;
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
		}

		protected override object GetService(Type serviceType)
		{
			if (this._serviceProvider != null)
			{
				return this._serviceProvider.GetService(serviceType);
			}
			return null;
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if (this._firstActivate)
			{
				this._firstActivate = false;
				this.OnInitialActivated(e);
			}
		}

		protected abstract string HelpTopic { get; }

		protected sealed override void OnHelpRequested(HelpEventArgs hevent)
		{
			this.ShowHelp();
			hevent.Handled = true;
		}

		protected virtual void OnInitialActivated(EventArgs e)
		{
		}

		private void ShowHelp()
		{
			if (this.ServiceProvider != null)
			{
				IHelpService helpService = (IHelpService)this.ServiceProvider.GetService(typeof(IHelpService));
				if (helpService != null)
				{
					helpService.ShowHelpFromKeyword(this.HelpTopic);
				}
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 274 && (int)m.WParam == 61824)
			{
				this.ShowHelp();
				return;
			}
			base.WndProc(ref m);
		}

		private const int SC_CONTEXTHELP = 61824;

		private const int WM_SYSCOMMAND = 274;

		private bool _firstActivate;
	}
}
