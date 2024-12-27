using System;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	// Token: 0x02000327 RID: 807
	internal abstract partial class DesignerForm : Form
	{
		// Token: 0x06001E42 RID: 7746 RVA: 0x000AC01F File Offset: 0x000AB01F
		protected DesignerForm(IServiceProvider serviceProvider)
		{
			this._serviceProvider = serviceProvider;
			this._firstActivate = true;
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001E43 RID: 7747 RVA: 0x000AC035 File Offset: 0x000AB035
		protected internal IServiceProvider ServiceProvider
		{
			get
			{
				return this._serviceProvider;
			}
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x000AC050 File Offset: 0x000AB050
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

		// Token: 0x06001E46 RID: 7750 RVA: 0x000AC0D2 File Offset: 0x000AB0D2
		protected override object GetService(Type serviceType)
		{
			if (this._serviceProvider != null)
			{
				return this._serviceProvider.GetService(serviceType);
			}
			return null;
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x000AC0EA File Offset: 0x000AB0EA
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if (this._firstActivate)
			{
				this._firstActivate = false;
				this.OnInitialActivated(e);
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001E48 RID: 7752
		protected abstract string HelpTopic { get; }

		// Token: 0x06001E49 RID: 7753 RVA: 0x000AC109 File Offset: 0x000AB109
		protected sealed override void OnHelpRequested(HelpEventArgs hevent)
		{
			this.ShowHelp();
			hevent.Handled = true;
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x000AC118 File Offset: 0x000AB118
		protected virtual void OnInitialActivated(EventArgs e)
		{
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x000AC11C File Offset: 0x000AB11C
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

		// Token: 0x06001E4C RID: 7756 RVA: 0x000AC15B File Offset: 0x000AB15B
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 274 && (int)m.WParam == 61824)
			{
				this.ShowHelp();
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x04001736 RID: 5942
		private const int SC_CONTEXTHELP = 61824;

		// Token: 0x04001737 RID: 5943
		private const int WM_SYSCOMMAND = 274;

		// Token: 0x04001739 RID: 5945
		private bool _firstActivate;
	}
}
